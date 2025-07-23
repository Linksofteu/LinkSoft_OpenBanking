using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;
using LinkSoft.OpenBanking.Komercka.Client;
using LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;
using LinkSoft.OpenBanking.Komercka.Client.OAuth;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ClientRegistrationClient = LinkSoft.OpenBanking.Komercka.Client.ClientRegistration.ClientRegistrationClient;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     Typed HTTP client for Account Direct Access APIs used for application management (SoftwareStatement registration, authorization and token requests).
///     None of the operations require access token.
/// </summary>
public class AccountDirectAccessManagementClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AccountDirectAccessManagementClient> _logger;
    private readonly AccountDirectAccessOptions _options;

    public AccountDirectAccessManagementClient(HttpClient httpClient, IOptionsSnapshot<AccountDirectAccessOptions> options,
        ILogger<AccountDirectAccessManagementClient> logger)
    {
        _httpClient = httpClient;
        _options = options.Value;
        _logger = logger;
    }

    /// <summary>
    ///     Register new software statement.
    /// </summary>
    /// <param name="clientRegistrationRequest"></param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    public async Task<SoftwareStatementRegistrationResult> RegisterSoftwareStatementAsync(SoftwareStatementRequest clientRegistrationRequest, CancellationToken cancellationToken)
    {
        ClientRegistrationClient client = new(_httpClient)
        {
            BaseUrl = _options.SoftwareStatementsEndpoint.BaseUrl,
            ApiKey = _options.SoftwareStatementsEndpoint.ApiKey
        };

        if (_logger.IsEnabled(LogLevel.Debug))
        {
            string serialized = JsonSerializer.Serialize(clientRegistrationRequest);
            _logger.RegisteringSoftwareStatement(LogLevel.Debug, _options.SoftwareStatementsEndpoint.BaseUrl, serialized);
        }

        string jwt = await client.PostSoftwareStatementsAsync(clientRegistrationRequest, cancellationToken);

        return new SoftwareStatementRegistrationResult
        {
            Jwt = jwt,
            ValidToUtc = DateTime.UtcNow.AddMonths(12)
        };
    }

    /// <summary>
    ///     Exchanges authorization code for a refresh token.
    /// </summary>
    /// <param name="request">Request</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="TokenResult{RefreshAndAccessToken}" />
    ///     which includes the refreshed access token or a failure result if the operation was unsuccessful.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when the access token or refresh token is null.</exception>
    public async Task<TokenResult<RefreshAndAccessToken>> RedeemAuthorizationCodeForRefreshTokenAsync(AuthorizationCodeTokenRequest request, CancellationToken ct)
    {
        _logger.RedeemingAccessCodeForRefreshToken(LogLevel.Debug, request.ClientId, _options.TokenEndpoint.BaseUrl);

        OAuthClient client = new(_httpClient)
        {
            BaseUrl = _options.TokenEndpoint.BaseUrl,
            ApiKey = _options.TokenEndpoint.ApiKey
        };

        FailedResult failure;
        try
        {
            TokenResponse response = await client.RequestAuthorizationCodeTokenAsync(request, ct).ConfigureAwait(false);

            RefreshAndAccessToken token = new()
            {
                RefreshTokenValue = response.RefreshToken ?? throw new InvalidOperationException("Refresh token should not be null"),
                AccessTokenValue = response.AccessToken,
                AccessTokenType = response.TokenType,
                Expiration = response.ExpiresIn == 0
                    ? DateTimeOffset.MaxValue
                    : DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn),
                ClientId = request.ClientId
            };

            return token;
        }
        catch (AccountDirectAccessApiException<ErrorResponse> errorException)
        {
            failure = TokenResult.Failure(errorException.Message, errorException.Result.ToString());
        }
        catch (AccountDirectAccessApiException exception)
        {
            failure = TokenResult.Failure(exception.Message, exception.Response);
        }
        catch (Exception exception)
        {
            failure = TokenResult.Failure(exception.Message);
        }

        _logger.FailedToRedeemAccessCodeForRefreshToken(LogLevel.Error, request.ClientId, failure.Error, failure.ErrorDescription ?? "unknown");

        return TokenResult.Failure($"Failed to redeem authorization code for refresh token: {failure.Error}", failure.ErrorDescription ?? "unknown");
    }

    /// <summary>
    ///     Exchanges authorization code for a refresh token.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="clientSecret">The client secret.</param>
    /// <param name="authorizationCode">The authorization code.</param>
    /// <param name="redirectUri">Redirect URI used in authorization code request. These URLs must be identical.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="TokenResult{RefreshAndAccessToken}" />
    ///     which includes the refreshed access token or a failure result if the operation was unsuccessful.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when the access token or refresh token is null.</exception>
    public Task<TokenResult<RefreshAndAccessToken>> RedeemAuthorizationCodeForRefreshTokenAsync(string clientId, string clientSecret, string authorizationCode, string redirectUri,
        CancellationToken ct)
    {
        AuthorizationCodeTokenRequest request = new()
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            Code = authorizationCode,
            RedirectUri = redirectUri
        };

        return RedeemAuthorizationCodeForRefreshTokenAsync(request, ct);
    }

    /// <summary>
    ///     Refreshes an access token using a provided refresh token.
    /// </summary>
    /// <param name="request">Request object containing client identifier, client secret and refresh token.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="TokenResult{AccessToken}" />
    ///     which includes the refreshed access token or a failure result if the operation was unsuccessful.
    /// </returns>
    public async Task<TokenResult<AccessToken>> RefreshAccessTokenAsync(RefreshTokenRequest request, CancellationToken ct)
    {
        OAuthClient client = new(_httpClient)
        {
            BaseUrl = _options.TokenEndpoint.BaseUrl,
            ApiKey = _options.TokenEndpoint.ApiKey
        };

        _logger.RefreshingAccessTokenUsingRefreshToken(LogLevel.Debug, request.ClientId, _options.TokenEndpoint.BaseUrl);

        FailedResult failure;
        try
        {
            TokenResponse response = await client.RequestRefreshTokenAsync(request, ct).ConfigureAwait(false);

            AccessToken token = new()
            {
                AccessTokenValue = response.AccessToken ?? throw new InvalidOperationException("Access token should not be null"),
                AccessTokenType = response.TokenType,
                Expiration = response.ExpiresIn == 0
                    ? DateTimeOffset.MaxValue
                    : DateTimeOffset.UtcNow.AddSeconds(response.ExpiresIn),
                ClientId = request.ClientId
            };

            return token;
        }
        catch (AccountDirectAccessApiException<ErrorResponse> errorException)
        {
            failure = TokenResult.Failure(errorException.Message, errorException.Result.ToString());
        }
        catch (AccountDirectAccessApiException exception)
        {
            failure = TokenResult.Failure(exception.Message, exception.Response);
        }
        catch (Exception exception)
        {
            failure = TokenResult.Failure(exception.Message);
        }

        _logger.FailedToRefreshAccessToken(LogLevel.Error, request.ClientId, failure.Error, failure.ErrorDescription ?? "unknown");

        return TokenResult.Failure($"Failed to refresh access token using refresh token: {failure.Error}", failure.ErrorDescription ?? "unknown");
    }

    /// <summary>
    ///     Refreshes an access token using a provided refresh token.
    /// </summary>
    /// <param name="clientId">The client identifier.</param>
    /// <param name="clientSecret">The client secret.</param>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="redirectUri">Redirect URI used in authorization code request. These URLs must be identical.</param>
    /// <param name="ct">A cancellation token to observe while waiting for the task to complete.</param>
    /// <returns>
    ///     A task that represents the asynchronous operation. The task result contains a <see cref="TokenResult{AccessToken}" />
    ///     which includes the refreshed access token or a failure result if the operation was unsuccessful.
    /// </returns>
    /// <exception cref="InvalidOperationException">Thrown when the access token is null.</exception>
    public Task<TokenResult<AccessToken>> RefreshAccessTokenAsync(string clientId, string clientSecret, string refreshToken, string redirectUri, CancellationToken ct)
    {
        RefreshTokenRequest request = new()
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            RefreshToken = refreshToken,
            RedirectUri = redirectUri
        };

        return RefreshAccessTokenAsync(request, ct);
    }
}