using LinkSoft.OpenBanking.Komercka.Client;
using LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Client;

/// <summary>
///     Simple wrapper for <see cref="AccountDirectAccessClient" /> that attaches provided <see cref="TContext" /> and API key to every request.
///     Also sets client's base URL from options.
/// </summary>
/// <remarks>
///     <see cref="TContext" /> is later used by middleware (DelegatedHandler) responsible for token management.
/// </remarks>
/// <typeparam name="TContext"></typeparam>
internal class AccountDirectAccessClientWithAuthorizationContext<TContext> : AccountDirectAccessClient
    where TContext : IAccountDirectAccessClientAuthorizationContext
{
    private readonly TContext _context;
    private readonly AccountDirectAccessOptions _options;

    public AccountDirectAccessClientWithAuthorizationContext(HttpClient httpClient, [NotNull] TContext context, AccountDirectAccessOptions options) : base(httpClient)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _options = options ?? throw new ArgumentNullException(nameof(options));

        BaseUrl = options.AccountDirectAccessEndpoint.BaseUrl;
    }

    /// <summary>
    ///     Prepare request by adding authorization context and API key.
    /// </summary>
    /// <param name="client"></param>
    /// <param name="request"></param>
    /// <param name="urlBuilder"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    protected override Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        request.Options.Set(IAccountDirectAccessClientAuthorizationContext.AuthorizationContextKey, _context);
        request.Headers.Add("ApiKey", _options.AccountDirectAccessEndpoint.ApiKey);
        request.AddCorrelationIdHeader(Guid.NewGuid());

        return base.PrepareRequestAsync(client, request, urlBuilder, cancellationToken);
    }
}