namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

/// <summary>
///     Request for token using authorization_code
/// </summary>
/// <seealso cref="TokenRequest" />
public class AuthorizationCodeTokenRequest : TokenRequest
{
    public string Code { get; set; } = default!;

    protected override IDictionary<string, string> GetRequestParametersInternal()
    {
        IDictionary<string, string> parameters = base.GetRequestParametersInternal();

        parameters.Add("grant_type", "authorization_code");
        parameters.Add("code", Code);

        return parameters;
    }
}