using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

/// <summary>
///     Models a response from an OpenID Connect/OAuth 2 token endpoint
/// </summary>
public class TokenResponse
{
    [JsonPropertyName("token_type")]
    [Required(AllowEmptyStrings = true)]
    public string TokenType { get; set; } = default!;

    /// <summary>
    ///     The access token issued by the authorization server.
    /// </summary>
    [JsonPropertyName("access_token")]
    [Required(AllowEmptyStrings = true)]
    public string AccessToken { get; set; } = default!;

    /// <summary>
    ///     Refresh tokens are credentials used to obtain new access tokens once authorization has already been granted.
    /// </summary>
    [JsonPropertyName("refresh_token")]
    public string? RefreshToken { get; set; } = default!;

    /// <summary>
    ///     The lifetime in seconds of the access token.
    /// </summary>
    [JsonPropertyName("expires_in")]
    public long ExpiresIn { get; set; } = default!;

    /// <summary>
    ///     The security level of authentication. Value '0' refers to nonSCA.
    /// </summary>
    [JsonPropertyName("acr")]
    public long? Acr { get; set; } = default!;
}