namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.Messages;

public class SoftwareStatementRegistrationResult
{
    /// <summary>
    ///     JWT as send by the KB API
    /// </summary>
    public string Jwt { get; set; } //string

    /// <summary>
    ///     Validity of the JWT (Software Statement).
    /// </summary>
    /// <remarks>
    ///     As the JWT returned from the KB service does not contain an expiration date,
    ///     this property is set to NOW + 12 months (as specified in the KB documentation). 
    /// </remarks>
    public DateTime ValidToUtc { get; set; }
}