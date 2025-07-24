namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

/// <summary>
///     Token response containing both the access token and the refresh token.
/// </summary>
public record RefreshAndAccessToken : AccessToken
{
    /// <summary>
    ///     The value of the refresh token.
    /// </summary>
    public string RefreshTokenValue { get; init; }
}