using System.Diagnostics;

namespace Workbench;

public sealed class KbHttpLoggingHandler : DelegatingHandler
{
    private readonly ILogger<KbHttpLoggingHandler> _logger;

    public KbHttpLoggingHandler(ILogger<KbHttpLoggingHandler> logger)
    {
        _logger = logger;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        string correlationId = request.Headers.TryGetValues("x-correlation-id", out IEnumerable<string>? values)
            ? string.Join(", ", values)
            : "(missing)";
        string endpoint = request.RequestUri?.GetComponents(UriComponents.SchemeAndServer | UriComponents.Path, UriFormat.UriEscaped) ?? "(unknown)";
        long started = Stopwatch.GetTimestamp();

        _logger.LogInformation("KB HTTP {Method} {Endpoint} sending; x-correlation-id={CorrelationId}", request.Method, endpoint, correlationId);

        try
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
            LogLevel level = (int)response.StatusCode >= 400 ? LogLevel.Warning : LogLevel.Information;
            _logger.Log(level,
                "KB HTTP {Method} {Endpoint} completed: HTTP {StatusCode} in {ElapsedMilliseconds} ms; x-correlation-id={CorrelationId}",
                request.Method, endpoint, (int)response.StatusCode, Stopwatch.GetElapsedTime(started).TotalMilliseconds, correlationId);
            return response;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            _logger.LogInformation("KB HTTP {Method} {Endpoint} canceled after {ElapsedMilliseconds} ms; x-correlation-id={CorrelationId}",
                request.Method, endpoint, Stopwatch.GetElapsedTime(started).TotalMilliseconds, correlationId);
            throw;
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "KB HTTP {Method} {Endpoint} failed after {ElapsedMilliseconds} ms; x-correlation-id={CorrelationId}",
                request.Method, endpoint, Stopwatch.GetElapsedTime(started).TotalMilliseconds, correlationId);
            throw;
        }
    }
}