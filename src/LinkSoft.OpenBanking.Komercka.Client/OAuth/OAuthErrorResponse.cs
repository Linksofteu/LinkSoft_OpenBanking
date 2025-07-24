using JetBrains.Annotations;
using System.Text.Json.Serialization;

namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

/// <summary>
///     Error response from OAuth2 token endpoint
/// </summary>
[UsedImplicitly]
public class OAuthErrorResponse
{
    [JsonPropertyName("error")]
    public string? Message { get; set; }

    /// <summary>
    ///     Error details.
    /// </summary>
    [JsonPropertyName("error_description")]
    public string? Description { get; set; } = null!;

    public override string ToString()
    {
        return $"{nameof(Message)}: {Message}, {nameof(Description)}: {Description}";
    }
}