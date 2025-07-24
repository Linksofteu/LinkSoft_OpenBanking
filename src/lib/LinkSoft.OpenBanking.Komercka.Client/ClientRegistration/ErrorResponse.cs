using JetBrains.Annotations;
using System.Text;
using System.Text.Json.Serialization;

namespace LinkSoft.OpenBanking.Komercka.Client.ClientRegistration;

public class Error
{
    private IDictionary<string, object>? _additionalProperties;

    /// <summary>
    ///     Error code.
    /// </summary>

    [JsonPropertyName("errorCode")]
    public string ErrorCode { get; set; } = default!;

    /// <summary>
    ///     Field where error occurred.
    /// </summary>

    [JsonPropertyName("scope")]
    public string Scope { get; set; } = default!;

    /// <summary>
    ///     Error details.
    /// </summary>

    [JsonPropertyName("description")]
    public string Description { get; set; } = default!;

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }
}

/// <summary>
///     Collection of errors defined by individual elements.
/// </summary>
[UsedImplicitly(ImplicitUseTargetFlags.Members)]
public class ErrorResponse
{
    private IDictionary<string, object>? _additionalProperties;

    [JsonPropertyName("errors")]
    public ICollection<Error>? Errors { get; set; }

    [JsonExtensionData]
    public IDictionary<string, object> AdditionalProperties
    {
        get => _additionalProperties ??= new Dictionary<string, object>();
        set => _additionalProperties = value;
    }

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