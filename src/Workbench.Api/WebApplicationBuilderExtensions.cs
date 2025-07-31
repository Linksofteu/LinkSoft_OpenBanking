using FastEndpoints;
using FastEndpoints.Swagger;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;
using LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;
using Microsoft.Extensions.Options;
using System.Security.Cryptography.X509Certificates;
using Workbench.Domain;
using Workbench.Domain.ADAA;

namespace Workbench;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureServices(this IServiceCollection serviceCollection, IConfigurationManager configuration)
    {
        serviceCollection.AddSingleton<IApplicationStore, ApplicationsStore>();

        WorkbenchOptions workbenchOptions = AddWorkbenchOptions(serviceCollection, configuration);

        serviceCollection.AddOptions<AccountDirectAccessOptions>()
            .Bind(configuration.GetSection($"ADAA:{workbenchOptions.TargetEnvironment}"))
            .ValidateOnStart();

        serviceCollection.AddSingleton<IValidateOptions<AccountDirectAccessOptions>, AccountDirectAccessOptions.AccountDirectAccessOptionsValidator>();
        serviceCollection.AddFastEndpoints(config =>
            {
            }
        );
        serviceCollection.SwaggerDocument(options =>
            {
                options.DocumentSettings = s =>
                {
                    s.DocumentName = "v1";
                    s.Title = "OpenBanking Workbench API";
                    s.Version = "v1";
                    s.MarkNonNullablePropsAsRequired();
                };
                options.ShortSchemaNames = true;
                options.EnableJWTBearerAuth = false;
            }
        );

        // HybridCache is used by for caching access tokens by ApplicationBoundAccessTokenRetriever
        serviceCollection.AddHybridCache();

        serviceCollection.AddSingleton<StateWorkaroundHandler>();
        serviceCollection.AddTransient<AccessTokenRequestDelegatingHandler>();
        serviceCollection.AddTransient<ITokenRetriever, HybridCacheTokenRetriever>();

        if (!string.IsNullOrEmpty(workbenchOptions.Certificate) && !string.IsNullOrEmpty(workbenchOptions.CertificatePassword))
        {
            X509Certificate2 certificate = GetCertificate(workbenchOptions).AugmentCertificateForTlsClientAuth();
            
            // management client needs TLS with client auth
            serviceCollection.AddHttpClient<AccountDirectAccessManagementClient>()
                .ConfigurePrimaryHttpMessageHandlerForTlsAuth(certificate);

            // ADAA client itself needs TLS with client auth + access token
            serviceCollection.AddHttpClient<ApplicationManifestBoundAdaaClientFactory>()
                .ConfigurePrimaryHttpMessageHandlerForTlsAuth(certificate)
                .AddHttpMessageHandler<AccessTokenRequestDelegatingHandler>();
        }
        else
        {
            if (workbenchOptions.TargetEnvironment == AdaaConstants.TargetEnvironment.Production)
            {
                throw new InvalidOperationException("Certificate is required for production environment.");
            }

            serviceCollection.AddHttpClient<AccountDirectAccessManagementClient>();
            serviceCollection.AddHttpClient<ApplicationManifestBoundAdaaClientFactory>()
                .AddHttpMessageHandler<AccessTokenRequestDelegatingHandler>();
        }
    }

    private static WorkbenchOptions AddWorkbenchOptions(IServiceCollection serviceCollection, IConfigurationManager configuration)
    {
        IConfigurationSection configurationSection = configuration.GetSection(WorkbenchOptions.SectionName);

        WorkbenchOptions options = new();
        configurationSection.Bind(options);

        ValidateOptionsResult validationResult = new WorkbenchOptionsValidator().Validate(WorkbenchOptions.SectionName, options);
        if (validationResult.Failed)
        {
            throw new Exception("Failed to validate Workbench options.");
        }

        serviceCollection.AddOptions<WorkbenchOptions>().Bind(configurationSection);

        return options;
    }

    private static X509Certificate2 GetCertificate(WorkbenchOptions options)
    {
        if (string.IsNullOrEmpty(options.Certificate) || string.IsNullOrEmpty(options.CertificatePassword))
        {
            throw new InvalidOperationException("Certificate and certificate password are required.");
        }
        
        return X509CertificateLoader.LoadPkcs12(Convert.FromBase64String(options.Certificate), options.CertificatePassword); 
    }
}