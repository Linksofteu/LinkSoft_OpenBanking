namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

public record AccessToken : IToken
{
    /// <summary>
    ///     When the access token expires. When no expiration is set, DateTimeOffset.MaxValue is used.
    /// </summary>
    public DateTimeOffset Expiration { get; init; }

    /// <summary>
    ///     The scope(s) associated with the access token.
    /// </summary>
    public string? Scope { get; init; }

    /// <summary>
    ///     The value of the access token.
    /// </summary>
    public string? AccessTokenValue { get; init; }

    /// <summary>
    ///     The type of the access token. Typically Bearer or DPoP.
    /// </summary>
    public string? AccessTokenType { get; init; }

    /// <summary>
    ///     The ClientID that was used to retrieve the access token.
    /// </summary>
    public string ClientId { get; init; }
}