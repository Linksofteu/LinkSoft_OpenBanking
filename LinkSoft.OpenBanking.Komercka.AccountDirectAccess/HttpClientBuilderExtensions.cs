using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder ConfigurePrimaryHttpMessageHandlerForTlsAuth(this IHttpClientBuilder builder, X509Certificate2 certificate)
    {
        return builder.ConfigurePrimaryHttpMessageHandler(sp =>
            {
                ILogger? logger = sp.GetService<ILogger>();

                logger?.LogInformation("Setting up certificate for ADAA (TLS client auth.)");

                HttpClientHandler handler = new()
                {
                    ClientCertificateOptions = ClientCertificateOption.Manual
                };
                handler.ClientCertificates.Add(certificate);

                return handler;
            }
        );
    }
}