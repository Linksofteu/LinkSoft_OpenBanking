namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

/// <summary>
///     Common information needed by access token handler
/// </summary>
public interface IToken
{
    string? AccessTokenValue { get; }

    /// <summary>
    ///     The Client id that this token was originally requested for.
    /// </summary>
    string ClientId { get; }

    string? AccessTokenType { get; }
}