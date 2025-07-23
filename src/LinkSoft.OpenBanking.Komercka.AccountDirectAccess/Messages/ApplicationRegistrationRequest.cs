using JetBrains.Annotations;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;

/// <summary>
///     Content of the request to OAuth2 application registration UI which facilitates the process of connecting the app to a specific KB client's account.
/// </summary>
/// <remarks>
///     <para>
///         Serialized as JSON and Base64 encoded is send as ULR parameter to KB OAuth2 UI.
///     </para>
/// </remarks>
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ApplicationRegistrationRequest
{
    /// <summary>
    ///     Human-readable string name of the client to be presented to the end-user during authorization.
    /// </summary>
    [JsonPropertyName("clientName")]
    public string ClientName { get; set; }

    /// <summary>
    ///     Human-readable string name of the client to be presented to the end-user during authorization.
    /// </summary>
    [JsonPropertyName("clientNameEn")]
    public string ClientNameEn { get; set; }

    /// <summary>
    ///     Kind of the application.
    /// </summary>
    /// <remarks>
    ///     The defined values are "native" or "web". Default is "web".
    /// </remarks>
    [JsonPropertyName("applicationType")]
    public string ApplicationType { get; set; } = "web";

    /// <summary>
    ///     List of redirection URI strings for use in redirect-based flows such as the authorization code and implicit flows.
    /// </summary>
    /// <remarks>
    ///     Clients using flows with redirection must register their redirection URI values.
    /// </remarks>
    [JsonPropertyName("redirectUris")]
    public ICollection<Uri> RedirectUris { get; set; } = new HashSet<Uri>();

    /// <summary>
    ///     String containing a space-separated list of scope values that the client can use when requesting access tokens.
    /// </summary>
    [JsonPropertyName("scope")]
    public ICollection<string> Scope { get; set; } = new HashSet<string>
    {
        AdaaConstants.Scope.AccountDirectAccess
    };

    /// <summary>
    ///     Software statement JWT
    /// </summary>
    /// <remarks>
    ///     If omitted, an authorization server may register a client with a default set of scopes.
    /// </remarks>
    [JsonPropertyName("softwareStatement")]
    public string SoftwareStatementJwt { get; set; }

    /// <summary>
    ///     Algorithm to be used to encrypt/decrypt the response.
    /// </summary>
    [JsonPropertyName("encryptionAlg")]
    public string EncryptionAlgorithm { get; } = "AES-256";

    /// <summary>
    ///     Encryption key to be used to encrypt/decrypt the response.
    /// </summary>
    /// <remarks>
    ///     An AES 256-bit key expressed as 32 bytes string (32 characters) + Base64 encoding (required 44 characters).
    /// </remarks>
    [JsonPropertyName("encryptionKey")]
    public string EncryptionKey { get; set; }
}