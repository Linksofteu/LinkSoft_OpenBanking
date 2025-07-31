using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

namespace Workbench.Domain.ADAA;

/// <summary>
///     ADAA client authorization context bound to specific <see cref="AccountDirectAccessApplicationManifest" />
/// </summary>
public class ApplicationManifestBoundAdaaAuthorizationContext: IAccountDirectAccessClientAuthorizationContext
{
    private readonly IAccountDirectAccessClientAuthorizationData _authData;
    
    public ApplicationManifestBoundAdaaAuthorizationContext(AccountDirectAccessApplicationManifest manifest)
    {
        ArgumentNullException.ThrowIfNull(manifest);

        if (manifest.ApplicationRegistration == null)
        {
            throw new InvalidOperationException("ApplicationRegistration is not set (app not registered)");
        }

        if (manifest.ApplicationAuthorization == null)
        {
            throw new InvalidOperationException("ApplicationAuthorization is not set (app not authorized)");
        }
        
        if (manifest.SoftwareStatementRegistrationDocument == null)
        {
            throw new InvalidOperationException("SoftwareStatementRegistrationDocument is not set");
        }

        ContextId = manifest.Id.ToString();
        
        _authData = new Data
        {
            ClientId = manifest.ApplicationRegistration.ClientId,
            ClientSecret = manifest.ApplicationRegistration.ClientSecret,
            RefreshToken = manifest.ApplicationAuthorization.RefreshToken,
            RedirectUri = manifest.SoftwareStatementRegistrationDocument.RegistrationBackUri.ToString()
        };

    }

    public string ContextId { get; }
    
    public IAccountDirectAccessClientAuthorizationData GetAuthorizationData()
    {
        return _authData;
    }

    private class Data : IAccountDirectAccessClientAuthorizationData
    {
        /// <summary>
        ///     Client ID generated during app registration process.
        /// </summary>
        public string ClientId { get; init; }
    
        /// <summary>
        ///     Client Secret generated during app registration process.
        /// </summary>
        public string ClientSecret { get; init; }
    
        /// <summary>
        ///     Refresh token generated during the app authorization process.
        /// </summary>
        public string RefreshToken { get; init; }
    
        /// <summary>
        ///     Redirect URI. Must be same as the one used in authorization code request.
        /// </summary>
        public string RedirectUri { get; init;  }    
    }
}