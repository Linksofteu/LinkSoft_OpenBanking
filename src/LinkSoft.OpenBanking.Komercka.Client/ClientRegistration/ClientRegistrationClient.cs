using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using System.Text.Json;

namespace LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;

/*
 * Automatic code generation of the ClientRegistration client not possible right now due to mismatch of OpenAPI spec and actual behavior.
 * - server should return 201 Created, but returns 200 OK (Sandbox)
 * - code 400 response sometimes not matched to OpenAPI spec
 *  - for example: {"status":"BAD_REQUEST","timestamp":"23-07-2025 07:57:05","message":"Validation failed","errors":["One or more redirectUris are not valid due to not matching a URI pattern."]}
 *
 * If fixed in the future, then code generation can be enabled in csproj and second partial class in this file can be deleted...
 */

public partial class ClientRegistrationClient
{
    public string ApiKey { get; set; }

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url)
    {
        if (string.IsNullOrEmpty(ApiKey))
        {
            throw new InvalidOperationException($"{ApiKey} is null or empty.");
        }

        request.Headers.Add("ApiKey", ApiKey);
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }
}

// this should be generated....
public partial class ClientRegistrationClient
{
    private static readonly Lazy<JsonSerializerOptions> Settings = new(CreateSerializerSettings, true);

    private readonly HttpClient _httpClient;
#pragma warning disable 8618
    private string _baseUrl;
#pragma warning restore 8618
    private JsonSerializerOptions _instanceSettings;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public ClientRegistrationClient(HttpClient httpClient)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        BaseUrl = "https://client-registration.api-gateway.kb.cz/v3";
        _httpClient = httpClient;
        Initialize();
    }

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

    protected JsonSerializerOptions JsonSerializerSettings => _instanceSettings ?? Settings.Value;

    private static JsonSerializerOptions CreateSerializerSettings()
    {
        JsonSerializerOptions settings = new();
        UpdateJsonSerializerSettings(settings);
        return settings;
    }

    static partial void UpdateJsonSerializerSettings(JsonSerializerOptions settings);

    partial void Initialize();

    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, string url);
    partial void PrepareRequest(HttpClient client, HttpRequestMessage request, StringBuilder urlBuilder);
    partial void ProcessResponse(HttpClient client, HttpResponseMessage response);

    /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
    /// <summary>
    ///     API to register new software.
    /// </summary>
    /// <param name="requestContent">Data for new software statement.</param>
    /// <returns>SoftwareStatement in form of JWT created.</returns>
    /// <exception cref="AccountDirectAccessApiException">A server side error occurred.</exception>
    public virtual async Task<string> PostSoftwareStatementsAsync(SoftwareStatementRequest requestContent, CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = new();

        string serialized = JsonSerializer.Serialize(requestContent, JsonSerializerSettings);
        request.Content = new StringContent(serialized, Encoding.UTF8, MediaTypeNames.Application.Json);
        request.Method = new HttpMethod("POST");
        request.Headers.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("text/plain; charset=utf-8"));

        StringBuilder urlBuilder = new();
        if (!string.IsNullOrEmpty(_baseUrl))
        {
            urlBuilder.Append(_baseUrl);
        }

        // Operation Path: "software-statements"
        urlBuilder.Append("software-statements");

        PrepareRequest(_httpClient, request, urlBuilder);

        string url = urlBuilder.ToString();
        request.RequestUri = new Uri(url, UriKind.RelativeOrAbsolute);

        PrepareRequest(_httpClient, request, url);

        HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
        bool disposeResponse = true;
        try
        {
            Dictionary<string, IEnumerable<string>> headers = new();
            foreach (KeyValuePair<string, IEnumerable<string>> item in response.Headers)
            {
                headers[item.Key] = item.Value;
            }

            foreach (KeyValuePair<string, IEnumerable<string>> item_ in response.Content.Headers)
            {
                headers[item_.Key] = item_.Value;
            }

            ProcessResponse(_httpClient, response);

            int status = (int)response.StatusCode;
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            }
            else if (status == 400 || status == 422)
            {
                ObjectResponseResult<ErrorResponse> objectResponse = await ReadObjectResponseAsync<ErrorResponse>(response, headers, cancellationToken).ConfigureAwait(false);
                if (objectResponse.Object == null)
                {
                    throw new AccountDirectAccessApiException("Response was null which was not expected.", status, objectResponse.Text, headers, null);
                }

                throw new AccountDirectAccessApiException<ErrorResponse>(
                    status == 400
                        ? "The request could not be understood by the server due to malformed syntax."
                        : "Syntax of the request is correct but the input is invalid. Please check input parameters.",
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
            if (disposeResponse)
            {
                response.Dispose();
            }
        }
    }

    protected virtual async Task<ObjectResponseResult<T>> ReadObjectResponseAsync<T>(HttpResponseMessage response, IReadOnlyDictionary<string, IEnumerable<string>> headers,
        CancellationToken cancellationToken)
    {
        string responseText = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            T? typedBody = JsonSerializer.Deserialize<T>(responseText, JsonSerializerSettings);
            return new ObjectResponseResult<T>(typedBody!, responseText);
        }
        catch (JsonException exception)
        {
            string message = "Could not deserialize the response body string as " + typeof(T).FullName + ".";
            throw new AccountDirectAccessApiException(message, (int)response.StatusCode, responseText, headers, exception);
        }
    }

    protected struct ObjectResponseResult<T>
    {
        public ObjectResponseResult(T responseObject, string responseText)
        {
            Object = responseObject;
            Text = responseText;
        }

        public T Object { get; }

        public string Text { get; }
    }
}