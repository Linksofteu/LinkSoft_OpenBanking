namespace LinkSoft.OpenBanking.Komercka.Client;

public static class Extensions
{
    public static void AddCorrelationIdHeader(this HttpRequestMessage request, Guid correlationId)
    {
        request.Headers.TryAddWithoutValidation(Constants.CorrelationIdHeaderName, (correlationId == Guid.Empty ? Guid.NewGuid() : correlationId).ToString());
    }
}