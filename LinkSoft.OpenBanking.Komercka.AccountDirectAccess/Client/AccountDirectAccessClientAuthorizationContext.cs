namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     Base implementation of <see cref="IAccountDirectAccessClientAuthorizationContext"/>
/// </summary> 
public class AccountDirectAccessClientAuthorizationContext : IAccountDirectAccessClientAuthorizationContext
{
    /// <summary>
    ///     User defined value that identifies the context. 
    /// </summary>
    public string ContextId { get; }

    /// <summary>
    ///     Callback function that returns client id, client secret and refresh token.
    ///     Argument is the <see cref="ContextId"/>
    /// </summary>
    public Func<string, (string clientId, string clientSecret, string refreshToken)> GetAuthorizationContext { get; }


    public AccountDirectAccessClientAuthorizationContext(string contextId, Func<string, (string clientId, string clientSecret, string refreshToken)> getAuthorizationContext)
    {
        ContextId = contextId;
        GetAuthorizationContext = getAuthorizationContext;
    }
}