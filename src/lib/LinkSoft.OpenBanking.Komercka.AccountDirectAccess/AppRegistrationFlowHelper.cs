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
        // Currently not accepted by KB Sandbox (returns BadRequest)
        // var urlEncodedJson = Microsoft.IdentityModel.Tokens.Base64UrlEncoder.Encode(requestJson);
        string urlEncodedJson = Uri.EscapeDataString(Convert.ToBase64String(Encoding.UTF8.GetBytes(requestJson)));

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