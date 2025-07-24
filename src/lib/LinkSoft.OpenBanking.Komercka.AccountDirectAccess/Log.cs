using Microsoft.Extensions.Logging;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

internal static partial class Log
{
    internal class LogParameters
    {
        public const string Error = "Error";
        public const string ErrorDescription = "ErrorDescription";
        public const string Url = "Url";
        public const string ClientId = "ClientId";
        public const string Value = "Value";
    }

    [LoggerMessage(Message = $"Registering new software statement at endpoint: {{{LogParameters.Url}}}. Content: {{{LogParameters.Value}}}")]
    public static partial void RegisteringSoftwareStatement(this ILogger logger, LogLevel logLevel, string url, string value);

    [LoggerMessage(Message = $"Redeeming access code for refresh token for client {{{LogParameters.ClientId}}} at endpoint: {{{LogParameters.Url}}}")]
    public static partial void RedeemingAccessCodeForRefreshToken(this ILogger logger, LogLevel logLevel, string clientId, string url);

    [LoggerMessage(
        Message = $"Failed to redeem access code for refresh token for client {{{LogParameters.ClientId}}}: {{{LogParameters.Error}}} ({{{LogParameters.ErrorDescription}}})"
    )]
    public static partial void FailedToRedeemAccessCodeForRefreshToken(this ILogger logger, LogLevel logLevel, string clientId, string error, string errorDescription);

    [LoggerMessage(Message = $"Refreshing access token using refresh token for client {{{LogParameters.ClientId}}} at endpoint: {{{LogParameters.Url}}}")]
    public static partial void RefreshingAccessTokenUsingRefreshToken(this ILogger logger, LogLevel logLevel, string clientId, string url);

    [LoggerMessage(
        Message = $"Failed to refresh access token using refresh token for client {{{LogParameters.ClientId}}}: {{{LogParameters.Error}}} ({{{LogParameters.ErrorDescription}}})"
    )]
    public static partial void FailedToRefreshAccessToken(this ILogger logger, LogLevel logLevel, string clientId, string error, string errorDescription);
}