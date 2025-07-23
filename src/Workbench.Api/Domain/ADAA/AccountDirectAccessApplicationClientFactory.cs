using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using Microsoft.Extensions.Options;

namespace Workbench.Domain.ADAA;

/// <summary>
///     <see cref="IAccountDirectAccessClient" /> factory responsible for creating ADAA clients bound to specific <see cref="AccountDirectAccessApplicationManifest" />
///     (containing client id, client secret and refresh token).
/// </summary>
public class AccountDirectAccessApplicationClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _options;

    public AccountDirectAccessApplicationClientFactory(HttpClient httpClient, IOptionsSnapshot<AccountDirectAccessOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public IAccountDirectAccessClient GetClient(AccountDirectAccessApplicationManifest manifest)
    {
        if (manifest == null)
        {
            throw new ArgumentNullException(nameof(manifest));
        }

        if (manifest.ApplicationRegistration == null)
        {
            throw new InvalidOperationException("ApplicationRegistration is not set (app not registered)");
        }

        if (manifest.ApplicationAuthorization == null)
        {
            throw new InvalidOperationException("ApplicationAuthorization is not set (app not authorized)");
        }

        (string clientId, string clientSecret, string refreshToken) GetAuthorizationContext(string contextId)
        {
            return (manifest.ApplicationRegistration.ClientId, manifest.ApplicationRegistration.ClientSecret, manifest.ApplicationAuthorization.RefreshToken);
        }

        AccountDirectAccessClientAuthorizationContext authorizationContext = new(
            manifest.Id.ToString(),
            GetAuthorizationContext
        );

        return AccountDirectAccessClientFactory<AccountDirectAccessClientAuthorizationContext>.GetClient(
            _httpClient,
            authorizationContext,
            _options.Value
        );
    }
}