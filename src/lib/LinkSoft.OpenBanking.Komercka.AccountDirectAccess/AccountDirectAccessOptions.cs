using JetBrains.Annotations;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Utils;
using Microsoft.Extensions.Options;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class AccountDirectAccessOptions
{
    public const string Sandbox = nameof(Sandbox);
    public const string Production = nameof(Production);

    public EndpointOptions SoftwareStatementsEndpoint { get; set; }

    public ApplicationRegistrationOptions ApplicationRegistration { get; set; }

    public string AuthorizationCodeFlowUrl { get; set; }

    public EndpointOptions TokenEndpoint { get; set; }

    public EndpointOptions AccountDirectAccessEndpoint { get; set; }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class EndpointOptions
    {
        public string BaseUrl { get; set; }

        public string ApiKey { get; set; }

        public class EndpointOptionsValidator : IValidateOptions<EndpointOptions>
        {
            public ValidateOptionsResult Validate(string? name, EndpointOptions options)
            {
                if (string.IsNullOrEmpty(options.BaseUrl))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.BaseUrl)} is required.");
                }

                if (!options.BaseUrl.IsValidUri())
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.BaseUrl)} is not valid URI.");
                }

                if (string.IsNullOrEmpty(options.ApiKey))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.ApiKey)} is required.");
                }

                return ValidateOptionsResult.Success;
            }
        }
    }

    [UsedImplicitly(ImplicitUseTargetFlags.Members)]
    public class ApplicationRegistrationOptions
    {
        /// <summary>
        ///     Encryption key send in the registration request. Can be used to decrypt data returned from registration UI.
        ///     Default is the key used by the KB Sandbox.
        /// </summary>
        /// <remarks>
        ///     AES-256 (Gcm variant) algorithm is used.
        ///     The key must be 32 bytes long and Base64 encoded (44 characters long).
        /// </remarks>
        public string EncryptionKey { get; set; } = "MnM1djh5L0I/RShIK01iUWVUaFdtWnEzdDZ3OXokQyY=";

        /// <summary>
        ///     URL of the registration UI page.
        /// </summary>
        public string Url { get; set; }

        public class ApplicationRegistrationOptionsValidator : IValidateOptions<ApplicationRegistrationOptions>
        {
            public ValidateOptionsResult Validate(string? name, ApplicationRegistrationOptions options)
            {
                if (string.IsNullOrEmpty(options.Url))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.Url)} is required.");
                }

                if (!options.Url.IsValidUri())
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.Url)} is not valid URI.");
                }

                if (string.IsNullOrEmpty(options.EncryptionKey))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.EncryptionKey)} is required.");
                }

                Span<byte> buffer = new(new byte[options.EncryptionKey.Length]);
                if (!Convert.TryFromBase64String(options.EncryptionKey, buffer, out int bytesParsed))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.EncryptionKey)} is not a valid Base64 string.");
                }

                if (!bytesParsed.Equals(32))
                {
                    return ValidateOptionsResult.Fail($"{nameof(options.EncryptionKey)} has to be 32 bytes long key (44 after Base64 encoding).");
                }

                return ValidateOptionsResult.Success;
            }
        }
    }

    public class AccountDirectAccessOptionsValidator : IValidateOptions<AccountDirectAccessOptions>
    {
        public ValidateOptionsResult Validate(string? name, AccountDirectAccessOptions options)
        {
            // ReSharper disable ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (!IsEndpointOptionsValid(nameof(options.SoftwareStatementsEndpoint), options.SoftwareStatementsEndpoint, out ValidateOptionsResult? result))
            {
                return result;
            }

            if (options.ApplicationRegistration == null)
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApplicationRegistration)} is required.");
            }

            if (!IsChildOptionsValid(new ApplicationRegistrationOptions.ApplicationRegistrationOptionsValidator(), options.ApplicationRegistration, out result))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.ApplicationRegistration)}.{result.FailureMessage}");
            }

            if (string.IsNullOrEmpty(options.AuthorizationCodeFlowUrl))
            {
                return ValidateOptionsResult.Fail($"{nameof(options.AuthorizationCodeFlowUrl)} is required.");
            }

            if (!options.AuthorizationCodeFlowUrl.IsValidUri())
            {
                return ValidateOptionsResult.Fail($"{nameof(options.AuthorizationCodeFlowUrl)} is not valid URI.");
            }

            if (!IsEndpointOptionsValid(nameof(options.TokenEndpoint), options.TokenEndpoint, out result))
            {
                return result;
            }

            if (!IsEndpointOptionsValid(nameof(options.AccountDirectAccessEndpoint), options.AccountDirectAccessEndpoint, out result))
            {
                return result;
            }

            return ValidateOptionsResult.Success;
            // ReSharper restore ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
        }

        private bool IsEndpointOptionsValid(string propertyName, EndpointOptions? options, out ValidateOptionsResult result)
        {
            if (options == null)
            {
                result = ValidateOptionsResult.Fail($"{propertyName} is required.");
                return false;
            }

            EndpointOptions.EndpointOptionsValidator endpointValidator = new();
            result = endpointValidator.Validate(null, options);

            if (!result.Succeeded)
            {
                result = ValidateOptionsResult.Fail($"{propertyName}.{result.FailureMessage}");
            }

            return result.Succeeded;
        }

        private bool IsChildOptionsValid<TOptions>(IValidateOptions<TOptions> validator, TOptions options, out ValidateOptionsResult result) where TOptions : class
        {
            result = validator.Validate(null, options);

            return result.Succeeded;
        }
    }
}