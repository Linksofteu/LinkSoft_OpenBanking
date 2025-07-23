using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;
using NJsonSchema.Annotations;
using System.Text.Json.Serialization;

namespace Workbench.Domain;

/// <summary>
///     Data structure for storing results of each step required to use ADAA KB API. 
/// </summary>
/// <remarks>
///     In most real word scenarios, there would be multiple instances of <see cref="ApplicationRegistrationResult"/> (ClientId + ClientSecret)
///     and <see cref="ApplicationAuthorizationResult"/> (refresh token) - one per each KB client the application can work with.
///     As this is only a demo/testing app, we are keeping only one instances of both.
/// </remarks>
public class AccountDirectAccessApplicationManifest
{
    public AccountDirectAccessApplicationManifest(string targetEnvironment, [NotNull] SoftwareStatementRequest softwareStatementRegistrationDocument)
    {
        TargetEnvironment = targetEnvironment ?? throw new ArgumentNullException(nameof(targetEnvironment));
        SoftwareStatementRegistrationDocument = softwareStatementRegistrationDocument ?? throw new ArgumentNullException(nameof(softwareStatementRegistrationDocument));

        Id = new Guid(softwareStatementRegistrationDocument.SoftwareId);
    }

    [JsonConstructor]
    private AccountDirectAccessApplicationManifest()
    {
    }

    /// <summary>
    ///     Id of the application. Extracted on creation from the <see cref="SoftwareStatementRegistrationDocument"/>
    /// </summary>
    public Guid Id { get; init; }

    public string TargetEnvironment { get; init; } = AdaaConstants.TargetEnvironment.Sandbox;

    public SoftwareStatementRequest SoftwareStatementRegistrationDocument { get; set; }

    /// <summary>
    ///     Result of the Software Statement registration
    /// </summary>
    public SoftwareStatementRegistrationResult? SoftwareStatement { get; set; }

    /// <summary>
    ///     Result of the Application registration
    /// </summary>
    public ApplicationRegistrationResult? ApplicationRegistration { get; set; }

    /// <summary>
    ///     Result of the Application authorization
    /// </summary>
    public ApplicationAuthorizationResult? ApplicationAuthorization { get; set; }

    public class SoftwareStatementRegistrationResult
    {
        public SoftwareStatementRegistrationResult(string jwt, DateTime validToUtc)
        {
            Jwt = jwt;
            ValidToUtc = validToUtc;
        }

        /// <summary>
        ///     JWT token produced by ADAA Software Statements registration API
        /// </summary>
        /// <remarks>
        /// </remarks>
        public string Jwt { get; set; }

        /// <summary>
        ///     Validity of the JWT.
        /// </summary>
        public DateTime ValidToUtc { get; set; }
    }

    public class ApplicationRegistrationResult
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string? RegistrationClientUri { get; set; }
    }

    public class ApplicationAuthorizationResult
    {
        public string RefreshToken { get; set; }

        public DateTimeOffset RefreshTokenExpirationUtc { get; set; }
    }
}