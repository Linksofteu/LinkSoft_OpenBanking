using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using Microsoft.Extensions.Options;

namespace Workbench.Domain.ADAA;

/// <summary>
///     <see cref="IAccountDirectAccessClient" /> factory responsible for creating ADAA clients bound to specific <see cref="AccountDirectAccessApplicationManifest" />
/// </summary>
public class ApplicationManifestBoundAdaaClientFactory
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _options;

    public ApplicationManifestBoundAdaaClientFactory(HttpClient httpClient, IOptionsSnapshot<AccountDirectAccessOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public IAccountDirectAccessClient GetClient(AccountDirectAccessApplicationManifest manifest)
    {
        ApplicationManifestBoundAdaaAuthorizationContext authorizationContext = new(manifest);
        
        return AccountDirectAccessClientFactory<ApplicationManifestBoundAdaaAuthorizationContext>.GetClient(_httpClient, authorizationContext, _options.Value);
    }
}