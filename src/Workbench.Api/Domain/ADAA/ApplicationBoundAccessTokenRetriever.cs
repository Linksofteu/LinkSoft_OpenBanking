using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using Microsoft.Extensions.Caching.Hybrid;
using System.Collections.Concurrent;

namespace Workbench.Domain.ADAA;

/// <summary>
///     Token retriever that uses application manifest data to get refresh token for a given application.
///     Access tokens are cached using <see cref="HybridCache" />
/// </summary>
/// <remarks>
///     <para>
///         Retrieved tokens are cached for 3 minutes . We don't use expiration information attached to the retrieved token for simplicity.
///     </para>
/// </remarks>
public class ApplicationBoundAccessTokenRetriever : ITokenRetriever
{
    // A flag that's written into the Data property of exceptions to distinguish
    // between exceptions that are thrown inside the cache and those that are thrown
    // inside the factory. 
    private const string ThrownInsideFactoryExceptionKey = "Workbench.Domain.ADAA.ApplicationBoundAccessTokenRetriever.ThrownInside";

    private const string CacheTag = "Workbench.Domain.ADAA.ApplicationBoundAccessTokenRetriever";
    private readonly HybridCache _cache;

    // We're assuming that the cache duration for access tokens will remain (relatively) stable
    // First time we acquire an access token, don't know yet know how long it will be valid, so we're assuming 
    // a specific period. However, after that, we'll use the actual expiration time to set the cache duration.
    private readonly ConcurrentDictionary<string, TimeSpan> _cacheDuration = new();

    private readonly ILogger<ApplicationBoundAccessTokenRetriever> _logger;
    private readonly AccountDirectAccessManagementClient _managementClient;

    public ApplicationBoundAccessTokenRetriever(AccountDirectAccessManagementClient managementClient, HybridCache cache,
        ILogger<ApplicationBoundAccessTokenRetriever> logger)
    {
        _managementClient = managementClient;
        _cache = cache;
        _logger = logger;
    }

    /// <summary>
    ///     Default cached token expiration time (as per KB API documentation).
    /// </summary>
    public TimeSpan DefaultCacheLifetime { get; set; } = TimeSpan.FromMinutes(3);

    public ICollection<string> CacheTags { get; } = new[]
    {
        CacheTag
    };

    public async Task<TokenResult<IToken>> GetTokenAsync(HttpRequestMessage request, bool forceTokenRenewal, CancellationToken ct)
    {
        IAccountDirectAccessClientAuthorizationContext authorizationContext = GetAuthorizationContext(request);

        string cacheKey = GetCacheKey(authorizationContext);
        TimeSpan cacheExpiration = _cacheDuration.GetValueOrDefault(cacheKey, DefaultCacheLifetime);

        // On force renewal, don't read from the cache, so we always get a new token.
        HybridCacheEntryFlags disableDistributedCacheRead = forceTokenRenewal
            ? HybridCacheEntryFlags.DisableLocalCacheRead | HybridCacheEntryFlags.DisableDistributedCacheRead
            : HybridCacheEntryFlags.None; // Even with "none", we still get cache stampede protection

        HybridCacheEntryOptions entryOptions = new()
        {
            Expiration = cacheExpiration,
            LocalCacheExpiration = cacheExpiration,
            Flags = disableDistributedCacheRead
        };

        IToken token;
        try
        {
            token = await _cache.GetOrCreateAsync(
                cacheKey,
                async c => await RequestToken(cacheKey, authorizationContext, c),
                entryOptions,
                CacheTags,
                ct
            );
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (PreventCacheException ex)
        {
            // This exception is thrown if there was a failure while retrieving an access token. We 
            // don't want to cache this failure, so we throw an exception to bypass the cache action. 
            // logger.WillNotCacheTokenResultWithError(LogLevel.Debug, clientName, ex.Failure.Error, ex.Failure.ErrorDescription);
            return ex.Failure;
        }
        catch (Exception ex) when (!ex.Data.Contains(ThrownInsideFactoryExceptionKey))
        {
            // if there was an exception in the cache, we'll just retry without the cache and hope for the best
            token = await RequestToken(cacheKey, authorizationContext, ct);
        }

        return TokenResult.Success(token);
    }

    private IAccountDirectAccessClientAuthorizationContext GetAuthorizationContext(HttpRequestMessage request)
    {
        if (!request.Options.TryGetValue(
                IAccountDirectAccessClientAuthorizationContext.AuthorizationContextKey,
                out IAccountDirectAccessClientAuthorizationContext? authorizationContext
            ))
        {
            throw new InvalidOperationException($"{nameof(IAccountDirectAccessClientAuthorizationContext)} not found in request options.");
        }

        return authorizationContext;
    }

    private string GetCacheKey(IAccountDirectAccessClientAuthorizationContext authorizationContext)
    {
        return $"{nameof(ApplicationBoundAccessTokenRetriever)}_{authorizationContext.ContextId}";
    }

    private async Task<AccessToken> RequestToken(string cacheKey, IAccountDirectAccessClientAuthorizationContext authorizationContext, CancellationToken ct)
    {
        TokenResult<AccessToken> tokenResult;
        var authData = authorizationContext.GetAuthorizationData();

        try
        {
            _logger.LogDebug("Getting new ADAA access token for authorization context {ContextId}", authorizationContext.ContextId);

            tokenResult = await _managementClient.RefreshAccessTokenAsync(authData.ClientId, authData.ClientSecret, authData.RefreshToken, authData.RedirectUri, ct);
        }
        catch (Exception ex)
        {
            // If there is a problem with retrieving data, then we want to bubble this back to the client. 
            // However, we want to distinguish this from exceptions that happen inside the cache itself. 
            // So, any exception that happens internally gets a special flag. 
            ex.Data[ThrownInsideFactoryExceptionKey] = true;
            throw;
        }

        if (!tokenResult.WasSuccessful(out AccessToken? token, out FailedResult? failure))
        {
            // Unfortunately, hybrid cache has no clean way to prevent failures from being cached. 
            // So we have to use an exception here. 
            throw new PreventCacheException(failure);
        }

        // See if we need to record how long this access token is valid, to be used the next time this access token is used. 
        if (token.Expiration != DateTimeOffset.MaxValue)
        {
            // Calculate how long this access token should be valid in the cache. 
            // Note, the expiration time was just calculated by adding time.GetUTcNow() to the token lifetime.
            // So for now it's safe to subtract this time from the expiration time.
            _cacheDuration[cacheKey] = DateTimeOffset.UtcNow - token.Expiration;

            _logger.LogDebug("Caching ADAA access token. Expiration: {Expiration}", token.Expiration);
        }

        return token;
    }

    private class PreventCacheException : Exception
    {
        public PreventCacheException(FailedResult failure)
        {
            Failure = failure;
        }

        public FailedResult Failure { get; }
    }
}