using FastEndpoints;
using LinkSoft.OpenBanking.Komercka;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Workbench.Domain;
using Workbench.Models;

namespace Workbench.Endpoints;

/// <summary>
///     Endpoint that for a given application generates an URL to be used in the user's browser to register the application with his KB account.
/// </summary>
public class GetApplicationRegistrationUrlEndpoint : Endpoint<ApplicationRequestBase, Results<Ok<string>, NotFound, BadRequest>>
{
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _adaaOptions;
    private readonly IApplicationStore _applicationStore;
    private readonly IOptionsSnapshot<WorkbenchOptions> _options;
    private readonly StateWorkaroundHandler _stateWorkaroundHandler;

    public GetApplicationRegistrationUrlEndpoint(IApplicationStore applicationStore, IOptionsSnapshot<WorkbenchOptions> options,
        IOptionsSnapshot<AccountDirectAccessOptions> adaaOptions, StateWorkaroundHandler stateWorkaroundHandler)
    {
        _applicationStore = applicationStore;
        _options = options;
        _adaaOptions = adaaOptions;
        _stateWorkaroundHandler = stateWorkaroundHandler;
    }

    public override void Configure()
    {
        Get("/applications/{ApplicationId:guid}/registrationUrl");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ApplicationRequestBase req, CancellationToken ct)
    {
        AccountDirectAccessApplicationManifest? application = await _applicationStore.GetApplicationAsync(req.ApplicationId);
        if (application == null)
        {
            await Send.ResultAsync(TypedResults.NotFound());
            return;
        }

        if (application.SoftwareStatement == null || application.SoftwareStatementRegistrationDocument == null)
        {
            await Send.ResultAsync(TypedResults.BadRequest("Software Statement is not registered"));
            return;
        }

        ApplicationRegistrationRequest registrationRequest = new()
        {
            ClientName = application.SoftwareStatementRegistrationDocument.SoftwareName,
            ClientNameEn = application.SoftwareStatementRegistrationDocument.SoftwareNameEn,
            ApplicationType = "web",
            SoftwareStatementJwt = application.SoftwareStatement.Jwt,
            RedirectUris = _options.Value.GetRedirectUris(),
            EncryptionKey = _adaaOptions.Value.ApplicationRegistration.EncryptionKey
        };

        string state = req.ApplicationId.ToString("N");
        _stateWorkaroundHandler.SetState(state);

        string appRegistrationUri = AppRegistrationFlowHelper.GenerateAppRegistrationFlowUri(_adaaOptions.Value.ApplicationRegistration.Url, registrationRequest, state);

        await Send.ResultAsync(TypedResults.Ok(appRegistrationUri));
    }
}