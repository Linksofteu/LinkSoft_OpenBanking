using System.Net.Http.Headers;
using System.Text.Json;

namespace LinkSoft.OpenBanking.Komercka.Client.OAuth;

/// <summary>
///     Client for OAuth2 support service (https://github.com/komercka/adaa-client/wiki/04-Tokens).
/// </summary>
public class OAuthClient
{
    private readonly HttpClient _httpClient;
    private string _baseUrl;

    public OAuthClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _baseUrl = "https://api-gateway.kb.cz/oauth2/v3/access_token/";
    }

    public string ApiKey { get; set; }

    public string BaseUrl
    {
        get => _baseUrl;
        set
        {
            _baseUrl = value;
            if (!string.IsNullOrEmpty(_baseUrl) && !_baseUrl.EndsWith("/"))
            {
                _baseUrl += '/';
            }
        }
    }

    /// <summary>
    ///     Sends a token request using the authorization_code grant type.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public Task<TokenResponse> RequestAuthorizationCodeTokenAsync(AuthorizationCodeTokenRequest request, CancellationToken cancellationToken = default)
    {
        return GetTokenAsync(request.GetRequestParameters(), cancellationToken);
    }

    /// <summary>
    ///     Sends a token request using the refresh_token grant type.
    /// </summary>
    /// <param name="request">The request.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public Task<TokenResponse> RequestRefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        return GetTokenAsync(request.GetRequestParameters(), cancellationToken);
    }


    private async Task<TokenResponse> GetTokenAsync(IEnumerable<KeyValuePair<string, string>> parameters, CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = new();

        request.Content = new FormUrlEncodedContent(parameters);
        request.Method = new HttpMethod("POST");
        request.RequestUri = new Uri(BaseUrl, UriKind.RelativeOrAbsolute);
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));

        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new InvalidOperationException($"{ApiKey} is null or empty.");
        }

        request.Headers.Add("ApiKey", ApiKey);

        HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        try
        {
            Dictionary<string, IEnumerable<string>> headers = new();
            foreach (KeyValuePair<string, IEnumerable<string>> item in response.Headers)
            {
                headers[item.Key] = item.Value;
            }

            foreach (KeyValuePair<string, IEnumerable<string>> item in response.Content.Headers)
            {
                headers[item.Key] = item.Value;
            }

            int status = (int)response.StatusCode;
            if (status == 200)
            {
                ObjectResponseResult<TokenResponse> objectResponse = await ReadObjectResponseAsync<TokenResponse>(response, headers).ConfigureAwait(false);
                if (objectResponse.Object == null)
                {
                    throw new AccountDirectAccessApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                }

                return objectResponse.Object;
            }
            else if (status == 400)
            {
                ObjectResponseResult<OAuthErrorResponse> objectResponse = await ReadObjectResponseAsync<OAuthErrorResponse>(response, headers).ConfigureAwait(false);
                if (objectResponse.Object == null)
                {
                    throw new AccountDirectAccessApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                }

                throw new AccountDirectAccessApiException<OAuthErrorResponse>(
                    "Invalid request parameter (missing parameter or wrong parameter value). For more details about possible error types, see \'https://tools.ietf.org/html/rfc6749#section-5.2\'",
                    status,
                    objectResponse.Text,
                    headers,
                    objectResponse.Object,
                    null
                );
            }
            else
            {
                string responseData = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new AccountDirectAccessApiException(
                    "The HTTP status code of the response was not expected (" + status + ").",
                    status,
                    responseData,
                    headers,
                    null
                );
            }
        }
        finally
        {
            response.Dispose();
        }
    }

    protected async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers)
    {
        string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        try
        {
            T? typedBody = JsonSerializer.Deserialize<T>(responseText);
            return new ObjectResponseResult<T>(typedBody!, responseText);
        }
        catch (JsonException exception)
        {
            string message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
            throw new AccountDirectAccessApiException(message, (int)response.StatusCode, responseText, headers, exception);
        }
    }

    protected readonly struct ObjectResponseResult<T>(T responseObject, string responseText)
    {
        public T Object { get; } = responseObject;

        public string Text { get; } = responseText;
    }
}