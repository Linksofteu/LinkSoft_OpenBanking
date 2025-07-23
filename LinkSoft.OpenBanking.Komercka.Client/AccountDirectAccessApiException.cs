namespace LinkSoft.OpenBanking.Komercka.Client;

public class AccountDirectAccessApiException : Exception
{
    public AccountDirectAccessApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, Exception? innerException)
        : base(
            message + "\n\nStatus: " + statusCode + "\nResponse: \n" + (response == null ? "(null)" : response.Substring(0, response.Length >= 512 ? 512 : response.Length)),
            innerException
        )
    {
        StatusCode = statusCode;
        Response = response;
        Headers = headers;
    }

    public int StatusCode { get; private set; }

    public string? Response { get; }

    public IReadOnlyDictionary<string, IEnumerable<string>> Headers { get; private set; }

    public override string ToString()
    {
        return $"HTTP Response: \n\n{Response}\n\n{base.ToString()}";
    }
}

public class AccountDirectAccessApiException<TResult> : AccountDirectAccessApiException
{
    public AccountDirectAccessApiException(string message, int statusCode, string? response, IReadOnlyDictionary<string, IEnumerable<string>> headers, TResult result,
        Exception? innerException)
        : base(message, statusCode, response, headers, innerException)
    {
        Result = result;
    }

    public TResult Result { get; private set; }
}