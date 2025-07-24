using Microsoft.Extensions.Logging;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

internal static partial class Log
{
    [LoggerMessage(Message = $"Sending Access token to endpoint: {{{LogParameters.Url}}}.")]
    public static partial void SendingAccessTokenToEndpoint(this ILogger logger, LogLevel logLevel, Uri? url);

    [LoggerMessage(
        Message = $"Failed to obtain an access token while sending the request. Error: {{{LogParameters.Error}}}, ErrorDescription {{{LogParameters.ErrorDescription}}}"
    )]
    public static partial void FailedToObtainAccessTokenWhileSendingRequest(this ILogger logger, LogLevel logLevel, string? error, string? errorDescription);

    private class LogParameters
    {
        public const string Error = "Error";
        public const string ErrorDescription = "ErrorDescription";
        public const string Url = "Url";
        public const string ClientId = "ClientId";
    }
}