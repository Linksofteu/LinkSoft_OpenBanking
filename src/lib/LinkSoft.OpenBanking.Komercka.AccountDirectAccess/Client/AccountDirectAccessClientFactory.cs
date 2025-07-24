using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using Microsoft.Extensions.Options;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     Factory for ADAA clients.
/// </summary>
/// <remarks>
///     <para>
///         As the generated client is not usable without authorization context (client id, secret and refresh token) and valid API key,
///         the generated implementation is marked as internal and this class is the only way got to get a client.
///     </para>
///     <para>
///         Can be used both as instance (usually by DI) or as static method.
///     </para>
/// </remarks>
/// <typeparam name="TContext"></typeparam>
public class AccountDirectAccessClientFactory<TContext> where TContext : IAccountDirectAccessClientAuthorizationContext
{
    private readonly HttpClient _httpClient;
    private readonly IOptionsSnapshot<AccountDirectAccessOptions> _options;

    public AccountDirectAccessClientFactory(HttpClient httpClient, IOptionsSnapshot<AccountDirectAccessOptions> options)
    {
        _httpClient = httpClient;
        _options = options;
    }

    public IAccountDirectAccessClient GetClient(TContext authorizationContext)
    {
        return new AccountDirectAccessClientWithContext<TContext>(_httpClient, authorizationContext, _options.Value);
    }

    /// <summary>
    ///     Creates an instance of <see cref="AccountDirectAccessClientWithContext{TContext}" /> using the provided HTTP client,
    ///     authorization context, and account direct access options.
    /// </summary>
    /// <param name="httpClient">The HTTP client used to make requests.</param>
    /// <param name="authorizationContext">The authorization context containing client ID, secret, and refresh token.</param>
    /// <param name="options">Configuration options for account direct access.</param>
    /// <returns>An instance of <see cref="IAccountDirectAccessClient" /> initialized with the provided parameters.</returns>
    public static IAccountDirectAccessClient GetClient(HttpClient httpClient, TContext authorizationContext, AccountDirectAccessOptions options)
    {
        return new AccountDirectAccessClientWithContext<TContext>(httpClient, authorizationContext, options);
    }
}