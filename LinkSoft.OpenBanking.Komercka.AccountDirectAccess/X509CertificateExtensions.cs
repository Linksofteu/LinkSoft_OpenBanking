using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

public static class X509CertificateExtensions
{
    /// <summary>
    ///     Augments certificate for client TLS authentication
    /// </summary>
    /// <remarks>
    ///     This code is needed due to .NET6 platform requiring correct key usage extension for client TLS authentication.
    ///     Certificates required by KB ADAA are issued without client auth key usage extension.
    /// </remarks>
    /// <param name="certificate">The certificate.</param>
    /// <returns>Augmented certificate</returns>
    public static X509Certificate2 AugmentCertificateForTlsClientAuth(this X509Certificate2 certificate)
    {
        // OID of enhanced/extended key usage extension, see https://oidref.com/1.3.6.1.5.5.7.3.2
        string clientAuthOid = "1.3.6.1.5.5.7.3.2";

        foreach (X509Extension extension in certificate.Extensions)
        {
            if (extension is X509KeyUsageExtension)
            {
                // replace key usage extension with one with valid flags for client TLS authentication
                X509KeyUsageExtension keyUsageExtension = new(X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.KeyEncipherment, false);
                extension.RawData = keyUsageExtension.RawData;
            }
            else if (extension is X509EnhancedKeyUsageExtension)
            {
                // replace enhanced/extended key usage extension with one with valid flags for client TLS authentication
                X509EnhancedKeyUsageExtension enhancedKeyUsageExtension = new(
                    new OidCollection
                    {
                        Oid.FromOidValue(clientAuthOid, OidGroup.EnhancedKeyUsage)
                    },
                    false
                );
                extension.RawData = enhancedKeyUsageExtension.RawData;
            }
        }

        return certificate;
    }
}