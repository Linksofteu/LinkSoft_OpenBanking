using JetBrains.Annotations;

namespace LinkSoft.OpenBanking.Komercka.Client.AccountDirectAccess;

[UsedImplicitly]
public partial class Error
{
    public override string ToString()
    {
        return $"{nameof(Code)}: {Code}, {nameof(Message)}: {Message}";
    }
}