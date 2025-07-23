namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

/// <summary>
///     Interface for retrieving access tokens, on behalf of <see cref="AccessTokenRequestDelegatingHandler" />
/// </summary>
public interface ITokenRetriever
{
    /// <summary>
    ///     Method that retrieves the actual access token
    /// </summary>
    /// <param name="request"></param>
    /// <param name="forceTokenRenewal">If <c>true</c>, the token should be renewed (instead of using one in the cache)</param>
    /// <param name="ct"></param>
    /// <returns></returns>
    Task<TokenResult<IToken>> GetTokenAsync(HttpRequestMessage request, bool forceTokenRenewal, CancellationToken ct);
}