namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Utils;

public static class UriValidator
{
    public static bool IsValidUri(this string uri)
    {
        return Uri.TryCreate(uri, UriKind.Absolute, out Uri? _);
    }
}