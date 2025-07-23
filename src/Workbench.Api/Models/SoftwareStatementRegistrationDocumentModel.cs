using FastEndpoints;
using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Utils;

namespace Workbench.Models;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class SoftwareStatementRegistrationDocumentModel
{
    public bool GenerateSoftwareId { get; set; }

    public string SoftwareName { get; set; }

    public string SoftwareNameEn { get; set; }

    public string SoftwareId { get; set; }

    public string SoftwareVersion { get; set; }

    public ICollection<string> Contacts { get; set; }

    public string SoftwareUri { get; set; }

    public string PolicyUri { get; set; }

    public string TosUri { get; set; }

    public string LogoUri { get; set; }

    public string RegistrationBackUri { get; set; }

    public ICollection<string> RedirectUris { get; set; } = new HashSet<string>();

    public class Validator : Validator<SoftwareStatementRegistrationDocumentModel>
    {
        public Validator()
        {
            RuleFor(x => x.SoftwareName).NotEmpty().MaximumLength(100);
            RuleFor(x => x.SoftwareNameEn).NotEmpty().MaximumLength(100);
            RuleFor(x => x.SoftwareVersion).NotEmpty().MaximumLength(100);

            RuleFor(x => x.Contacts).NotEmpty();
            RuleFor(x => x.RedirectUris).NotEmpty()
                .ForEach(item => item.NotEmpty().Must(x => x!.IsValidUri()));

            RuleFor(x => x.SoftwareUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.PolicyUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.TosUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.LogoUri).NotEmpty().Must(x => x!.IsValidUri());
            RuleFor(x => x.RegistrationBackUri).NotEmpty().Must(x => x!.IsValidUri());

            RuleFor(x => x.SoftwareId).NotEmpty().MaximumLength(100).When(x => !x.GenerateSoftwareId);
        }
    }
}