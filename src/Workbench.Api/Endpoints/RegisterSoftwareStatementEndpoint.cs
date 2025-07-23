using FastEndpoints;
using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;
using Microsoft.Extensions.Options;
using Workbench.Domain;

namespace Workbench.Endpoints;

public class RegisterSoftwareStatementEndpoint : Endpoint<RegisterSoftwareStatementRequest, AccountDirectAccessApplicationManifest>
{
    private readonly IApplicationStore _applicationStore;
    private readonly AccountDirectAccessManagementClient _managementClient;
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _options;

    public RegisterSoftwareStatementEndpoint(IApplicationStore applicationStore, AccountDirectAccessManagementClient managementClient,
        IOptionsSnapshot<AccountDirectAccessOptions> options)
    {
        _applicationStore = applicationStore;
        _managementClient = managementClient;
        _options = options;
    }

    public override void Configure()
    {
        Patch("/applications/registerSoftwareStatement");
        AllowAnonymous();
        Description(b =>
            b.ProducesProblemFE(StatusCodes.Status404NotFound)
        );
    }

    public override async Task HandleAsync(RegisterSoftwareStatementRequest req, CancellationToken ct)
    {
        AccountDirectAccessApplicationManifest? manifest = await _applicationStore.GetApplicationAsync(req.ApplicationId);

        if (manifest == null)
        {
            await SendNotFoundAsync();
            return;
        }

        SoftwareStatementRegistrationResult registrationResult = await _managementClient.RegisterSoftwareStatementAsync(manifest.SoftwareStatementRegistrationDocument, ct);

        manifest.SoftwareStatement = new AccountDirectAccessApplicationManifest.SoftwareStatementRegistrationResult(registrationResult.Jwt, registrationResult.ValidToUtc);
        await _applicationStore.SaveApplication(manifest);

        await SendAsync(manifest);
    }
}

public class RegisterSoftwareStatementRequest
{
    public Guid ApplicationId { get; set; }
}

[UsedImplicitly]
public class RegisterSoftwareStatementRequestValidator : Validator<RegisterSoftwareStatementRequest>
{
    public RegisterSoftwareStatementRequestValidator()
    {
        RuleFor(x => x.ApplicationId).NotEmpty();
    }
}