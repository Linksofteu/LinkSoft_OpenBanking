using System.Text;

namespace LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;

public abstract partial class AccountDirectAccessClient
{
    protected virtual Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task PrepareRequestAsync(HttpClient client, HttpRequestMessage request, string url, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    protected virtual Task ProcessResponseAsync(HttpClient client, HttpResponseMessage response, CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}