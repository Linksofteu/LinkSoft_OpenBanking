using FastEndpoints;
using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using Workbench.Domain;
using Workbench.Domain.ADAA;
using Workbench.Models;

namespace Workbench.Endpoints;

public class GetAccountTransactionsEndpoint : Endpoint<GetAccountTransactionsRequest, PageSlice>
{
    private readonly IApplicationStore _applicationStore;
    private readonly ApplicationManifestBoundAdaaClientFactory _clientFactory;

    public GetAccountTransactionsEndpoint(IApplicationStore applicationStore, ApplicationManifestBoundAdaaClientFactory clientFactory)
    {
        _applicationStore = applicationStore;
        _clientFactory = clientFactory;
    }

    public override void Configure()
    {
        Get("applications/{ApplicationId}/accounts/{AccountId}/transactions");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetAccountTransactionsRequest req, CancellationToken ct)
    {
        AccountDirectAccessApplicationManifest? application = await _applicationStore.GetApplicationAsync(req.ApplicationId);
        if (application == null)
        {
            ThrowError("Application not found", StatusCodes.Status404NotFound);
        }

        IAccountDirectAccessClient client = _clientFactory.GetClient(application);
        await Send.OkAsync(
            await client.GetTransactionsAsync(
                req.AccountId,
                DateTimeOffset.UtcNow, 
                DateTimeOffset.UtcNow.AddMonths(-1),
                0,
                100,
                ct
            )
        );
    }
}

public class GetAccountTransactionsRequest : ApplicationRequestBase
{
    public string AccountId { get; set; }
}

public class GetAccountTransactionsRequestValidator : Validator<GetAccountTransactionsRequest>
{
    public GetAccountTransactionsRequestValidator()
    {
        Include(new ApplicationRequestBaseValidator());
        RuleFor(x => x.AccountId).NotEmpty();
    }
}