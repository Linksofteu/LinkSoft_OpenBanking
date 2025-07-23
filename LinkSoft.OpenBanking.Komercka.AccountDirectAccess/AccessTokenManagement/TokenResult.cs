using System.Diagnostics.CodeAnalysis;

namespace LinkSoft.OpenBanking.Komercka.AccountDirectAccess.AccessTokenManagement;

public abstract record TokenResult
{
    public static FailedResult Failure(string error, string? errorDescription = null)
    {
        return new FailedResult(error, errorDescription);
    }

    public static TokenResult<T> Success<T>(T token) where T : class
    {
        return token;
    }
}

/// <summary>
///     Represents the result of a token request. It can either be a token or a failure.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed record TokenResult<T> : TokenResult
    where T : class
{
    private TokenResult(T input)
    {
        Token = input;
    }

    private TokenResult(FailedResult failure)
    {
        FailedResult = failure;
    }


    [MemberNotNullWhen(true, nameof(Token))]
    [MemberNotNullWhen(false, nameof(FailedResult))]
    public bool Succeeded => FailedResult == null;

    public FailedResult? FailedResult { get; }

    public T? Token { get; }

    public static implicit operator TokenResult<T>(T input)
    {
        return new TokenResult<T>(input);
    }

    public static implicit operator TokenResult<T>(FailedResult failure)
    {
        return new TokenResult<T>(failure);
    }

    public bool WasSuccessful(out T result)
    {
        if (Succeeded)
        {
            result = Token;
            return true;
        }

        result = null!;
        return false;
    }

    public bool WasSuccessful([NotNullWhen(true)] out T? result, [NotNullWhen(false)] out FailedResult? failure)
    {
        if (Succeeded)
        {
            failure = null;
            result = Token;
            return true;
        }

        failure = FailedResult;
        result = null;
        return false;
    }
}