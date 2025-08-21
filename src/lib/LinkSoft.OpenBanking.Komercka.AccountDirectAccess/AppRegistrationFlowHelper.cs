using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

/// <summary>
///     Helper class for indirect flows (sending user to some KB URL and decoding data returned on callback URL)
/// </summary>
public static class AppRegistrationFlowHelper
{
    /// <summary>
    ///     Generates OAuth2 authorization URL to be used in the browser.
    /// </summary>
    /// <param name="baseUrl">Base URL of the KB OAuth2 registration UI</param>
    /// <param name="registrationRequest">Content of the registration request (including the redirect URL and Software Statement JWT)</param>
    /// <param name="state">Optional state parameter (will be returned to you in the callback URL)</param>
    /// <returns>Url to be used in the user's browser to register the application with his KB account.</returns>
    public static string GenerateAppRegistrationFlowUri(string baseUrl, ApplicationRegistrationRequest registrationRequest, string? state)
    {
        string requestJson = JsonSerializer.Serialize(registrationRequest);
        
        /* Issue #2
         * registrationRequest param data should be base64 encoded (see https://github.com/komercka/adaa-client/wiki/03-Application-Registration-OAuth2)
         * Problem is, base64 can include +, / and = characters, which are not URL friendly.
         * I tried to use base64url encoding but unfortunately it is not accepted by the KB sandbox (returns BadRequest).
         * I tried to use base64 + ulr encode, which worked for some time but then stopped working for KB Production (see https://github.com/Linksofteu/LinkSoft_OpenBanking/issues/2)
         *
         * As I dont want to introduce any logic based on target environment, only way right now is to use base64 encoding as is,
         * even though it is not URL friendly... 
         */
        //var urlEncodedJson = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(requestJson);
        string urlEncodedJson = Convert.ToBase64String(Encoding.UTF8.GetBytes(requestJson));
        
        StringBuilder urlBuilder = new(baseUrl);
        urlBuilder.Append("?registrationRequest=").Append(urlEncodedJson);

        if (state != null)
        {
            urlBuilder.Append("&state=");
            urlBuilder.Append(Uri.EscapeDataString(state));
        }

        return urlBuilder.ToString();
    }

    public static ApplicationRegistrationResult DecryptAndParseAppRegistrationData(byte[] encryptedData, byte[] nonce, byte[] key)
    {
        // tag length used by KB
        const int authenticationTagLength = 16;

        using AesGcm aesGcm = new(key, authenticationTagLength);

        byte[] decrypted = new byte[encryptedData.Length - authenticationTagLength];
        aesGcm.Decrypt(nonce, encryptedData[..^authenticationTagLength], encryptedData[^authenticationTagLength..], decrypted);

        string content = Encoding.UTF8.GetString(decrypted);
        return JsonSerializer.Deserialize<ApplicationRegistrationResult>(content);
    }

    public static string GenerateAuthorizationCodeFlowUri(string baseUrl, string clientId, string redirectUri, string? state)
    {
        StringBuilder urlBuilder = new(baseUrl);

        urlBuilder.Append($"?{AdaaConstants.AuthorizeRequest.ResponseType}=code");
        urlBuilder.Append($"&{AdaaConstants.AuthorizeRequest.ClientId}=").Append(Uri.EscapeDataString(clientId));
        urlBuilder.Append($"&{AdaaConstants.AuthorizeRequest.RedirectUri}=").Append(Uri.EscapeDataString(redirectUri));
        urlBuilder.Append($"&{AdaaConstants.AuthorizeRequest.Scope}=").Append(Uri.EscapeDataString(AdaaConstants.Scope.AccountDirectAccess));

        if (state != null)
        {
            urlBuilder.Append("&state=");
            urlBuilder.Append(Uri.EscapeDataString(state));
        }

        return urlBuilder.ToString();
    }
}