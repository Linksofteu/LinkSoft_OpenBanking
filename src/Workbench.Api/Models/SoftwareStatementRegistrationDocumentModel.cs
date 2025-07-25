using FastEndpoints;
using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Utils;
using LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;

namespace Workbench.Models;

/// <summary>
///     API Model for the content of <see cref="SoftwareStatementRequest"/>
/// </summary>
/// <remarks>
///     Properties of <see cref="SoftwareStatementRequest"/> that have well-known expected values are not defined here.
/// </remarks>
[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SoftwareStatementRegistrationDocumentModel
{
    /// <summary>
    ///     If <see langword="true"/>, <see cref="SoftwareStatementRequest.SoftwareId"/> will be auto-generated and <see cref="SoftwareId"/> value will be ignored.
    /// </summary>
    public bool GenerateSoftwareId { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.SoftwareName"/>
    public string SoftwareName { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.SoftwareNameEn"/>
    public string SoftwareNameEn { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.SoftwareId"/>
    public string SoftwareId { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.SoftwareVersion"/>
    public string SoftwareVersion { get; set; }

    public ICollection<string> Contacts { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.SoftwareUri"/>
    public string SoftwareUri { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.PolicyUri"/>
    public string PolicyUri { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.TosUri"/>
    public string TosUri { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.LogoUri"/>
    public string LogoUri { get; set; }

    /// <inheritdoc cref="SoftwareStatementRequest.RegistrationBackUri"/>
    public string RegistrationBackUri { get; set; }

    public ICollection<string> RedirectUris { get; set; } = new HashSet<string>();

    public class Validator : Validator<SoftwareStatementRegistrationDocumentModel>
    {
        public Validator()
        {
            RuleFor(x => x.SoftwareName).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SoftwareNameEn).NotEmpty().MaximumLength(50);
            RuleFor(x => x.SoftwareVersion).NotEmpty().MaximumLength(30);

            RuleFor(x => x.Contacts).NotEmpty();
            RuleFor(x => x.RedirectUris)
                .NotEmpty()
                .Must(x => x!.Count > 1).WithMessage("At least one redirect URI is required.")
                .Must(x => x!.Count <= 2).WithMessage("At most two redirect URIs are allowed.");
                
            RuleForEach(x => x.RedirectUris)
                .NotEmpty()
                .Must(x => x!.IsValidUri(absolute: true, mustBeHttps: true));

            RuleFor(x => x.SoftwareUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.PolicyUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.TosUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.LogoUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.RegistrationBackUri).NotEmpty().Must(x => x!.IsValidUri(absolute: true, mustBeHttps: true));

            RuleFor(x => x.SoftwareId).NotEmpty().MaximumLength(64).When(x => !x.GenerateSoftwareId);
        }
    }
}