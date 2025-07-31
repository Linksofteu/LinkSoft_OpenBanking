namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     ADAA client authorization context.
/// </summary>
/// <remarks>
///     ADAA API always operates in the context of specific KB client account - or more precisely, authorization to specific client and selected accounts.
///     This context is represented by ClientId, ClientSecret, RefreshToken and CallbackUrl (all generated during app registration and authorization steps). 
/// </remarks>
public interface IAccountDirectAccessClientAuthorizationContext
{
    /// <summary>
    ///     User defined value that identifies the context. 
    /// </summary>
    /// <remarks>
    ///     
    /// </remarks>
    string ContextId { get; }

    /// <summary>
    ///     Retrieves authorization data for the current context.
    /// </summary>
    public IAccountDirectAccessClientAuthorizationData GetAuthorizationData();

    static HttpRequestOptionsKey<IAccountDirectAccessClientAuthorizationContext> AuthorizationContextKey { get; } =
        new("LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AuthorizationContextKey");
}