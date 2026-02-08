using FastEndpoints;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Options;
using Workbench.Domain;
using Workbench.Models;

namespace Workbench.Endpoints;

/// <summary>
///     Endpoint that for a given application generates an URL to be used in the user's browser to authorize application to access client's account(s).
/// </summary>
public class GetApplicationAuthorizationUrlEndpoint : Endpoint<ApplicationRequestBase, Results<Ok<string>, NotFound, BadRequest>>
{
    private readonly IApplicationStore _applicationStore;
    private readonly IOptionsSnapshot<WorkbenchOptions> _options;
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _adaaOptions;

    public GetApplicationAuthorizationUrlEndpoint(IApplicationStore applicationStore, IOptionsSnapshot<WorkbenchOptions> options, IOptionsSnapshot<AccountDirectAccessOptions> adaaOptions)
    {
        _applicationStore = applicationStore;
        _options = options;
        _adaaOptions = adaaOptions;
    }

    public override void Configure()
    {
        Get("/applications/{ApplicationId:guid}/authorizationUrl");
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

        if (application.ApplicationRegistration == null)
        {
            await Send.ResultAsync(TypedResults.BadRequest("Application is not registered yet."));
            return;
        }

        string appAuthorizationUri = AppRegistrationFlowHelper.GenerateAuthorizationCodeFlowUri(
            _adaaOptions.Value.AuthorizationCodeFlowUrl,
            application.ApplicationRegistration.ClientId,
            _options.Value.CallbackUrl,
            application.Id.ToString("N")
        );

        await Send.ResultAsync(TypedResults.Ok(appAuthorizationUri));
    }
}