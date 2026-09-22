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

    /// <inheritdoc />
    /// <remarks>
    ///     KB documents UTC timestamps for transaction filters. The generated client formats dates with a literal Z
    ///     without converting their offset, which would change the requested instant for non-UTC values.
    ///     Normalize here so regeneration preserves this fix. The overload without a cancellation token also calls this override.
    /// </remarks>
    public override Task<PageSlice> GetTransactionsAsync(string accountId, DateTimeOffset? toDateTime, DateTimeOffset? fromDateTime, int page, int? size,
        CancellationToken cancellationToken)
    {
        return base.GetTransactionsAsync(accountId, toDateTime?.ToUniversalTime(), fromDateTime?.ToUniversalTime(), page, size, cancellationToken);
    }

    /// <inheritdoc />
    /// <remarks>
    ///     KB's statement examples use UTC timestamps. As with transaction filters, the generated client adds a literal Z
    ///     without converting the offset. Normalize here to preserve the requested instant without editing generated code.
    ///     The overload without a cancellation token also calls this override.
    /// </remarks>
    public override Task<ICollection<Statement>> GetAccountStatementsAsync(string accountId, DateTimeOffset dateFrom, CancellationToken cancellationToken)
    {
        return base.GetAccountStatementsAsync(accountId, dateFrom.ToUniversalTime(), cancellationToken);
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