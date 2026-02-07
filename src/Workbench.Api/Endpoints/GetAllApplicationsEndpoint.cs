using FastEndpoints;
using Microsoft.Extensions.Options;
using Workbench.Domain;

namespace Workbench.Endpoints;

/// <summary>
///     Gets all applications for current target environment. 
/// </summary>
public class GetAllApplicationsEndpoint : EndpointWithoutRequest<IList<AccountDirectAccessApplicationManifest>>
{
    private readonly IApplicationStore _applicationStore;
    private readonly IOptionsSnapshot<WorkbenchOptions> _options;

    public GetAllApplicationsEndpoint(IApplicationStore applicationStore, IOptionsSnapshot<WorkbenchOptions> options)
    {
        _applicationStore = applicationStore;
        _options = options;
    }

    public override void Configure()
    {
        Get("/applications");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        IList<AccountDirectAccessApplicationManifest> apps = await _applicationStore.GetApplicationsAsync(_options.Value.TargetEnvironment);
        await Send.OkAsync(apps);
    }
}