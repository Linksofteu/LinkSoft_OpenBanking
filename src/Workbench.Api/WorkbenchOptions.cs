using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using Microsoft.Extensions.Options;

namespace Workbench;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class WorkbenchOptions
{
    public const string SectionName = "Workbench";

    [ValueProvider(nameof(AdaaConstants.TargetEnvironment))]
    public string TargetEnvironment { get; set; }

    public string CallbackUrl { get; set; } = "http://localhost:3000/callback";

    /// <summary>
    ///     Certificate with private key in PEM format (base64 encoded)
    /// </summary>
    public string Certificate { get; set; }

    /// <summary>
    ///     Certificate password
    /// </summary>
    public string CertificatePassword { get; set; }

    public ICollection<Uri> GetRedirectUris()
    {
        return new[]
        {
            new Uri(CallbackUrl)
        };
    }
}

public class WorkbenchOptionsValidator : IValidateOptions<WorkbenchOptions>
{
    public ValidateOptionsResult Validate(string? name, WorkbenchOptions options)
    {
        if (string.IsNullOrEmpty(options.TargetEnvironment))
        {
            return ValidateOptionsResult.Fail($"{nameof(options.TargetEnvironment)} is required.");
        }

        if (AdaaConstants.TargetEnvironment.ExistingEnvironments.All(x => !x.Equals(options.TargetEnvironment, StringComparison.OrdinalIgnoreCase)))
        {
            return ValidateOptionsResult.Fail($"{nameof(options.TargetEnvironment)} is not valid target environment.");
        }

        if (string.IsNullOrEmpty(options.CallbackUrl))
        {
            return ValidateOptionsResult.Fail($"{nameof(options.CallbackUrl)} is required.");
        }

        return ValidateOptionsResult.Success;
    }
}