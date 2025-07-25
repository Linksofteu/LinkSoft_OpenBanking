namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Utils;

public static class UriValidator
{
    public static bool IsValidUri(this string urlString, bool absolute = true, bool mustBeHttps = false)
    {
        if(string.IsNullOrEmpty(urlString)) return false;
        
        if (Uri.TryCreate(urlString, absolute ? UriKind.Absolute : UriKind.RelativeOrAbsolute, out Uri? uri))
        {
            return !mustBeHttps || uri.Scheme == Uri.UriSchemeHttps;
        }
        
        return false;
    }
}