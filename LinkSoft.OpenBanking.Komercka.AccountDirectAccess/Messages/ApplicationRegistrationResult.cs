namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;

/// <summary>
///     Content of the response from KB's OAuth2 application registration UI (only selected properties).
/// </summary>
/// <remarks>
///     https://github.com/komercka/adaa-client/wiki/03-Application-Registration-OAuth2#response
/// </remarks>
public class ApplicationRegistrationResult
{
    private IDictionary<string, object>? _additionalProperties;

    [JsonPropertyName("client_id")]
    public string ClientId { get; set; }

    [JsonPropertyName("client_secret")]
    public string ClientSecret { get; set; }

    [JsonPropertyName("registration_client_uri")]
    public string? RegistrationClientUri { get; set; }

    /// <summary>
    ///     State from the request
    /// </summary>
    [JsonPropertyName("state")]
    public string? State { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }
}