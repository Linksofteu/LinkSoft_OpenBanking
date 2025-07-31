using FastEndpoints;
using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using Workbench.Domain;
using Workbench.Domain.ADAA;
using Workbench.Models;

namespace Workbench.Endpoints;

public class GetAccountsEndpoint : Endpoint<ApplicationRequestBase, ICollection<Account>>
{
    private readonly IApplicationStore _applicationStore;
    private readonly ApplicationManifestBoundAdaaClientFactory _clientFactory;

    public GetAccountsEndpoint(IApplicationStore applicationStore, ApplicationManifestBoundAdaaClientFactory clientFactory)
    {
        _applicationStore = applicationStore;
        _clientFactory = clientFactory;
    }

    public override void Configure()
    {
        Get("applications/{ApplicationId}/accounts");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ApplicationRequestBase req, CancellationToken ct)
    {
        AccountDirectAccessApplicationManifest? application = await _applicationStore.GetApplicationAsync(req.ApplicationId);
        if (application == null)
        {
            ThrowError("Application not found", StatusCodes.Status404NotFound);
        }

        IAccountDirectAccessClient client = _clientFactory.GetClient(application);
        await SendOkAsync(await client.GetAccountsAsync(ct));
    }
}