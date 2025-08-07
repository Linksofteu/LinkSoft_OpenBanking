using FastEndpoints;
using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;
using Workbench.Domain;

namespace Workbench.Endpoints;

/// <summary>
///     Endpoint that processes the callback data sent back from both the KB's OAuth2 application registration UI AND the authorization UI.
///     If the request contains the response from the KB's OAuth2 application registration UI, then responds with the URL needed to get the authorization code.
///     If the request contains the response from the authorization UI, then responds with updated application manifest containing retrieved refresh token.
/// </summary>
public class HandleCallbackFlowDataEndpoint : Endpoint<HandleCallbackDataRequest, HandleCallbackDataResponse>
{
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _adaaOptions;
    private readonly IApplicationStore _applicationStore;
    private readonly ILogger<HandleCallbackFlowDataEndpoint> _logger;
    private readonly AccountDirectAccessManagementClient _managementClient;
    private readonly IOptionsSnapshot<WorkbenchOptions> _options;
    private readonly StateWorkaroundHandler _stateWorkaroundHandler;

    public HandleCallbackFlowDataEndpoint(IApplicationStore applicationStore, IOptionsSnapshot<WorkbenchOptions> options,
        IOptionsSnapshot<AccountDirectAccessOptions> adaaOptions,
        AccountDirectAccessManagementClient managementClient, StateWorkaroundHandler stateWorkaroundHandler, ILogger<HandleCallbackFlowDataEndpoint> logger)
    {
        _applicationStore = applicationStore;
        _options = options;
        _adaaOptions = adaaOptions;
        _managementClient = managementClient;
        _stateWorkaroundHandler = stateWorkaroundHandler;
        _logger = logger;
    }

    public override void Configure()
    {
        Post("/applications/handleCallbackFlow");
        AllowAnonymous();
    }

    /// <summary>
    ///     Handle callback flow data
    /// </summary>
    /// <param name="req"></param>
    /// <param name="ct"></param>
    /// <returns></returns>
    /// <remarks>
    /// </remarks>
    public override async Task HandleAsync(HandleCallbackDataRequest req, CancellationToken ct)
    {
        _logger.LogDebug(nameof(HandleCallbackFlowDataEndpoint) + ": {Request}", req);

        if (!string.IsNullOrEmpty(req.EncryptedData) && !string.IsNullOrEmpty(req.Salt))
        {
            ApplicationRegistrationResult registrationResult = AppRegistrationFlowHelper.DecryptAndParseAppRegistrationData(
                Base64UrlEncoder.DecodeBytes(req.EncryptedData),
                Base64UrlEncoder.DecodeBytes(req.Salt),
                Convert.FromBase64String(_adaaOptions.Value.ApplicationRegistration.EncryptionKey)
            );

            AccountDirectAccessApplicationManifest application = await FindApplicationByStateAsync(registrationResult.State);

            string clientId = registrationResult.ClientId;
            string clientSecret = registrationResult.ClientSecret;
            string? registrationClientUri = registrationResult.RegistrationClientUri;

            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
            {
                ThrowError("App registration callback data is not valid");
            }

            application.ApplicationRegistration = new AccountDirectAccessApplicationManifest.ApplicationRegistrationResult
            {
                ClientId = clientId,
                ClientSecret = clientSecret,
                RegistrationClientUri = registrationClientUri
            };

            // clear authorization info as we now have a different ClientId/ClientSecret
            application.ApplicationAuthorization = null;

            await _applicationStore.SaveApplication(application);

            await SendAsync(new HandleCallbackDataResponse(application));

            return;
        }

        if (!string.IsNullOrEmpty(req.Code))
        {
            AccountDirectAccessApplicationManifest application = await FindApplicationByStateAsync(req.State);

            if (application.ApplicationRegistration is null)
            {
                ThrowError("Application not registered yet", StatusCodes.Status400BadRequest);
            }

            TokenResult<RefreshAndAccessToken> refreshTokenResult = await _managementClient.RedeemAuthorizationCodeForRefreshTokenAsync(
                application.ApplicationRegistration.ClientId,
                application.ApplicationRegistration.ClientSecret,
                req.Code,
                _options.Value.CallbackUrl,
                ct
            );

            if (refreshTokenResult.WasSuccessful(out RefreshAndAccessToken? token, out FailedResult? failedResult))
            {
                application.ApplicationAuthorization = new AccountDirectAccessApplicationManifest.ApplicationAuthorizationResult
                {
                    RefreshToken = token.RefreshTokenValue,
                    RefreshTokenExpirationUtc = DateTimeOffset.UtcNow.AddMonths(12)
                };
                await _applicationStore.SaveApplication(application);

                await SendAsync(new HandleCallbackDataResponse(application));
            }
            else
            {
                ThrowError(failedResult.ToString(), StatusCodes.Status400BadRequest);
            }

            return;
        }

        AddError("Invalid callback data");
    }


    private async Task<AccountDirectAccessApplicationManifest> FindApplicationByStateAsync(string? state)
    {
        if (string.IsNullOrEmpty(state))
        {
            state = _stateWorkaroundHandler.GetState();
        }

        if (!string.IsNullOrEmpty(state) && Guid.TryParse(state, out Guid appId))
        {
            AccountDirectAccessApplicationManifest? app = await _applicationStore.GetApplicationAsync(appId);

            if (app is null)
            {
                ThrowError("Application not found", StatusCodes.Status404NotFound);
            }

            return app;
        }

        ThrowError("Invalid State", StatusCodes.Status400BadRequest);

        return null;
    }
}

/// <summary>
///     Request to process application registration callback data returned from KB App registration UI.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class HandleCallbackDataRequest
{
    private IDictionary<string, object>? _additionalProperties;

    /// <summary>
    ///     Encrypted data produced by KB App registration flow.
    /// </summary>
    /// <remarks>
    ///     Data are base64url encoded JSON payload. <see cref="ApplicationRegistrationResult" />
    /// </remarks>
    public string? EncryptedData { get; set; }

    /// <summary>
    ///     Nonce used to encrypt <see cref="EncryptedData" /> in KB App registration flow.
    /// </summary>
    /// <remarks>
    ///     Nonce is base64url encoded.
    /// </remarks>
    public string? Salt { get; set; }

    /// <summary>
    ///     Authorization code returned from KB Authorization token flow.
    /// </summary>
    public string? Code { get; set; }

    /// <summary>
    ///     Arbitrary state value send in the original request URL.
    /// </summary>
    public string? State { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }

    public override string ToString()
    {
        StringBuilder sb = new();
        sb.AppendLine($"EncryptedData: {EncryptedData}");
        sb.AppendLine($"Salt: {Salt}");
        sb.AppendLine($"Code: {Code}");
        sb.AppendLine($"State: {State}");

        if (_additionalProperties != null && _additionalProperties.Count > 0)
        {
            sb.AppendLine("AdditionalProperties:");
            foreach (KeyValuePair<string, object> prop in _additionalProperties)
            {
                sb.AppendLine($"  {prop.Key}: {prop.Value}");
            }
        }

        return sb.ToString();
    }
}

public class ProcessApplicationRegistrationCallbackDataRequestValidator : Validator<HandleCallbackDataRequest>
{
    public ProcessApplicationRegistrationCallbackDataRequestValidator()
    {
        When(
            x => string.IsNullOrEmpty(x.Code),
            () =>
            {
                RuleFor(x => x.EncryptedData).NotEmpty();
                RuleFor(x => x.Salt).NotEmpty();
                //RuleFor(x => x.State).NotEmpty();
            }
        ).Otherwise(() =>
            {
                RuleFor(x => x.Code).NotEmpty();
                RuleFor(x => x.State).NotEmpty();
            }
        );
    }
}

public class HandleCallbackDataResponse
{
    public HandleCallbackDataResponse(AccountDirectAccessApplicationManifest updatedApplication)
    {
        UpdatedApplication = updatedApplication;
    }

    [JsonConstructor]
    private HandleCallbackDataResponse()
    {
    }

    public string? RedirectUri { get; set; }

    public AccountDirectAccessApplicationManifest UpdatedApplication { get; set; }
}