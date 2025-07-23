namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

/// <summary>
///     Request for token using refresh_token
/// </summary>
/// <seealso cref="TokenRequest" />
public class RefreshTokenRequest : TokenRequest
{
    /// <summary>
    ///     Gets or sets the refresh token.
    /// </summary>
    /// <value>
    ///     The refresh token.
    /// </value>
    public string RefreshToken { get; set; } = default!;

    protected override IDictionary<string, string> GetRequestParametersInternal()
    {
        IDictionary<string, string> parameters = base.GetRequestParametersInternal();

        parameters.Add("grant_type", "refresh_token");
        parameters.Add("refresh_token", RefreshToken);

        return parameters;
    }
}