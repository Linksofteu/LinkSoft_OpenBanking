using Microsoft.Extensions.Logging;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

/// <summary>
///     This HttpMessageHandler is used to acquire and add an access token while sending http requests.
/// </summary>
public sealed class AccessTokenRequestDelegatingHandler : DelegatingHandler
{
    private readonly ILogger<AccessTokenRequestDelegatingHandler> _logger;
    private readonly ITokenRetriever _tokenRetriever;

    /// <summary>
    ///     This HttpMessageHandler is used to acquire and add an access token while sending http requests.
    ///     The method on how to acquire tokens and the retry policy can be customized.
    /// </summary>
    public AccessTokenRequestDelegatingHandler(ITokenRetriever tokenRetriever, ILogger<AccessTokenRequestDelegatingHandler> logger)
    {
        _tokenRetriever = tokenRetriever;
        _logger = logger;
    }

    protected override HttpResponseMessage Send(HttpRequestMessage request, CancellationToken ct)
    {
        throw new NotSupportedException(
            "The (synchronous) Send() method is not supported. Please use the async SendAsync variant. "
        );
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
    {
        TokenResult<IToken> tokenResult = await SetTokenAsync(request, false, ct).ConfigureAwait(false);

        // if token result is a failure, we sand the request without token anyway...
        HttpResponseMessage response = await base.SendAsync(request, ct).ConfigureAwait(false);

        // retry if 401
        if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized && tokenResult.Succeeded)
        {
            response.Dispose();

            await SetTokenAsync(request, true, ct).ConfigureAwait(false);

            return await base.SendAsync(request, ct).ConfigureAwait(false);
        }

        return response;
    }

    /// <summary>
    ///     Set an access token on the HTTP request
    /// </summary>
    /// <returns></returns>
    private async Task<TokenResult<IToken>> SetTokenAsync(HttpRequestMessage request, bool forceTokenRenewal, CancellationToken ct)
    {
        TokenResult<IToken> tokenResult = await _tokenRetriever.GetTokenAsync(request, forceTokenRenewal, ct);

        if (tokenResult.WasSuccessful(out IToken? token, out FailedResult? failure))
        {
            _logger.SendingAccessTokenToEndpoint(LogLevel.Debug, request.RequestUri);

            request.SetToken(token);

            return tokenResult;
        }

        _logger.FailedToObtainAccessTokenWhileSendingRequest(LogLevel.Warning, failure.Error, failure.ErrorDescription);

        return tokenResult;
    }
}