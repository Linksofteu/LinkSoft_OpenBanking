namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

public sealed record FailedResult(string Error, string? ErrorDescription = null) : TokenResult
{
    public override string ToString()
    {
        string description = string.IsNullOrEmpty(ErrorDescription) ? string.Empty : $" with description {ErrorDescription}";
        return $"Failed to retrieve access token due to {Error}{description}.";
    }
}