namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

public abstract class TokenRequest
{
    /// <summary>
    ///     Gets or sets the client identifier.
    /// </summary>
    /// <value>
    ///     The client identifier.
    /// </value>
    public string ClientId { get; init; } = null!;

    /// <summary>
    ///     Gets or sets the client secret.
    /// </summary>
    /// <value>
    ///     The client secret.
    /// </value>
    public string ClientSecret { get; init; } = null!;

    /// <summary>
    ///     Redirect URI used in authorization code request. These URLs must be identical.
    /// </summary>
    public string RedirectUri { get; init; } = null!;

    /// <summary>
    ///     Correlation ID used for tracing. If not set, a new GUID will be generated.
    /// </summary>
    public Guid CorrelationId { get; init; } = Guid.Empty;

    public IEnumerable<KeyValuePair<string, string>> GetRequestParameters()
    {
        return GetRequestParametersInternal();
    }

    protected virtual IDictionary<string, string> GetRequestParametersInternal()
    {
        Dictionary<string, string> parameters = new()
        {
            { "client_id", ClientId },
            { "client_secret", ClientSecret },
            { "redirect_uri", RedirectUri }
        };

        return parameters;
    }
}