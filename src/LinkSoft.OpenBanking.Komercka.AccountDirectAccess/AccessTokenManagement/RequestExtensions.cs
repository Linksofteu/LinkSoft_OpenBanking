namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

public static class RequestExtensions
{
    public static readonly HttpRequestOptionsKey<ForceTokenRenewal> ForceRenewalOptionsKey =
        new("LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement.ForceTokenRenewal");

    private static readonly HttpRequestOptionsKey<string> ClientIdOptionsKey = new("LinkSoft.OpenBanking.Komercka.AccountDirectAccess.ClientId");

    /// <summary>
    ///     Set the token to a http request.
    /// </summary>
    /// <param name="request">The http request</param>
    /// <param name="token">The token to set</param>
    internal static void SetToken(this HttpRequestMessage request, IToken token)
    {
        // Set the client ID on the request so down the line we know what client ID this token was issued for
        request.Options.Set(ClientIdOptionsKey, token.ClientId);
        request.SetToken("Bearer", token.AccessTokenValue);
    }

    /// <summary>
    ///     Retrieve the client id, that was previously set using <see cref="SetToken" />
    /// </summary>
    /// <param name="request">The request</param>
    /// <returns>If present, the client id</returns>
    internal static string? GetClientId(this HttpRequestMessage request)
    {
        request.Options.TryGetValue(ClientIdOptionsKey, out string? clientId);

        return clientId;
    }

    public static void SetForceRenewal(this HttpRequestMessage request, ForceTokenRenewal forceTokenRenewal)
    {
        request.Options.Set(ForceRenewalOptionsKey, forceTokenRenewal);
    }

    public static ForceTokenRenewal GetForceRenewal(this HttpRequestMessage request)
    {
        if (request.Options.TryGetValue(ForceRenewalOptionsKey, out ForceTokenRenewal forceRenewal))
        {
            return forceRenewal;
        }

        return new ForceTokenRenewal(false);
    }
}