using LinkSoft.OpenBanking.Komercka;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using Microsoft.Extensions.Options;

namespace Workbench.Domain;

/// <summary>
///     Protoze v ramci Sandboxu KB vubec State do callbacku neposila.
/// </summary>
public class StateWorkaroundHandler
{
    private readonly IOptions<WorkbenchOptions> _options;
    private string? _callbackState { get; set; }

    public StateWorkaroundHandler(IOptions<WorkbenchOptions> options)
    {
        _options = options;
    }

    public void SetState(string state)
    {
        _callbackState = state;
    }

    public string? GetState()
    {
        ThrowIfProduction();

        return _callbackState;
    }

    private void ThrowIfProduction()
    {
        if (_options.Value.TargetEnvironment == AdaaConstants.TargetEnvironment.Production)
        {
            throw new InvalidOperationException("Special State handling should not be needed when dealing with production environment");
        }
    }
}