namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     ADAA client authorization context.
/// </summary>
/// <remarks>
///     ADAA API always operates in the context of specific KB client account - or more precisely, authorization to specific client and selected accounts.
///     This context is represented by ClientId, ClientSecret and RefreshToken (all generated during app registration and authorization steps). 
/// </remarks>
public interface IAccountDirectAccessClientAuthorizationContext
{
    /// <summary>
    ///     User defined value that identifies the context. 
    /// </summary>
    string ContextId { get; }

    /// <summary>
    ///     Callback function that returns client id, client secret and refresh token.
    ///     Argument is the <see cref="ContextId"/>
    /// </summary>
    Func<string, (string clientId, string clientSecret, string refreshToken)> GetAuthorizationContext { get; }

    static HttpRequestOptionsKey<IAccountDirectAccessClientAuthorizationContext> AuthorizationContextKey { get; } =
        new("LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AuthorizationContextKey");
}