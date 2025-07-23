using System.Text;

namespace LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;

public partial class ErrorResponse
{
    public override string ToString()
    {
        if (Errors != null && Errors.Any())
        {
            StringBuilder sb = new();
            foreach (Error error in Errors)
            {
                sb.AppendLine(error.ToString());
            }

            return sb.ToString();
        }

        return string.Empty;
    }
}