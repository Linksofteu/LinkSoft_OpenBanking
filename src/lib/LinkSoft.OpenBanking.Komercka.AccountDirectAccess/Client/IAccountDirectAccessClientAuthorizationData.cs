namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     An abstraction for authorization data used by ADAA client.
/// </summary>
/// <remarks>
///     ADAA API always operates in the context of specific KB client account - or more precisely, authorization to specific client and selected accounts.
///     This interface represents authorization data generated during app registration and authorization process and is used by the token management infrastructure.
/// </remarks>
public interface IAccountDirectAccessClientAuthorizationData
{
    /// <summary>
    ///     Client ID generated during app registration process.
    /// </summary>
    string ClientId { get; }

    /// <summary>
    ///     Client Secret generated during app registration process.
    /// </summary>
    string ClientSecret { get; }

    /// <summary>
    ///     Refresh token generated during the app authorization process.
    /// </summary>
    string RefreshToken { get; }

    /// <summary>
    ///     Redirect URI. Must be same as the one used in authorization code request.
    /// </summary>
    string RedirectUri { get; }
}