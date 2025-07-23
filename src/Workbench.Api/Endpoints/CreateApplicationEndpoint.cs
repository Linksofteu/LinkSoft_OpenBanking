using FastEndpoints;
using LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;
using Microsoft.Extensions.Options;
using Workbench.Domain;
using Workbench.Models;

namespace Workbench.Endpoints;

public class CreateApplicationEndpoint : Endpoint<SoftwareStatementRegistrationDocumentModel, AccountDirectAccessApplicationManifest>
{
    private readonly IApplicationStore _applicationStore;
    private readonly IOptionsSnapshot<WorkbenchOptions> _options;

    public CreateApplicationEndpoint(IApplicationStore applicationStore, IOptionsSnapshot<WorkbenchOptions> options)
    {
        _applicationStore = applicationStore;
        _options = options;
    }

    public override void Configure()
    {
        Post("/applications");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SoftwareStatementRegistrationDocumentModel req, CancellationToken ct)
    {
        SoftwareStatementRequest registrationDocument = new()
        {
            SoftwareName = req.SoftwareName,
            SoftwareNameEn = req.SoftwareNameEn,
            SoftwareVersion = req.SoftwareVersion,
            SoftwareId = req.SoftwareId,
            SoftwareUri = new Uri(req.SoftwareUri),
            PolicyUri = new Uri(req.PolicyUri),
            TosUri = new Uri(req.TosUri),
            LogoUri = new Uri(req.LogoUri),
            RegistrationBackUri = new Uri(_options.Value.CallbackUrl),
            Contacts = req.Contacts,
            RedirectUris = new List<Uri>
            {
                new(_options.Value.CallbackUrl)
            }
        };

        if (req.GenerateSoftwareId)
        {
            registrationDocument.SoftwareId = Guid.NewGuid().ToString();
        }

        AccountDirectAccessApplicationManifest manifest = new(_options.Value.TargetEnvironment, registrationDocument);
        await _applicationStore.SaveApplication(manifest);

        await SendAsync(manifest);
    }
}