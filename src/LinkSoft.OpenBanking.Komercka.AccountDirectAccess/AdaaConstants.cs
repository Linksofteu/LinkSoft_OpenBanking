namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess;

public static class AdaaConstants
{
    public static class Scope
    {
        public const string AccountDirectAccess = "adaa";
        public const string BatchPayments = "bpisp";
        public const string CardData = "card_data";
    }

    public static class AuthorizeRequest
    {
        public const string Scope = "scope";
        public const string ResponseType = "response_type";
        public const string ClientId = "client_id";
        public const string RedirectUri = "redirect_uri";
    }

    public static class TargetEnvironment
    {
        public const string Sandbox = "Sandbox";
        public const string Production = "Production";

        public static readonly string[] ExistingEnvironments =
        {
            Sandbox,
            Production
        };
    }
}