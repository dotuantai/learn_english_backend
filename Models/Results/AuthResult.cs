using learn_english_backend.Models.DTOs;

namespace learn_english_backend.Models.Results;

public sealed record AuthResult
{
    private AuthResult(
        AuthResponse? response,
        AuthFailureKind failureKind,
        string? errorCode,
        string? errorMessage,
        Dictionary<string, string[]>? validationErrors)
    {
        Response = response;
        FailureKind = failureKind;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        ValidationErrors = validationErrors;
    }

    public AuthResponse? Response { get; }

    public AuthFailureKind FailureKind { get; }

    public string? ErrorCode { get; }

    public string? ErrorMessage { get; }

    public Dictionary<string, string[]>? ValidationErrors { get; }

    public bool Succeeded => Response is not null;

    public static AuthResult Success(AuthResponse response) =>
        new(response, AuthFailureKind.None, null, null, null);

    public static AuthResult Validation(
        Dictionary<string, string[]> validationErrors) =>
        new(null, AuthFailureKind.Validation, "validation_failed",
            "One or more validation errors occurred.", validationErrors);

    public static AuthResult Unauthorized(string errorCode, string errorMessage) =>
        new(null, AuthFailureKind.Unauthorized, errorCode, errorMessage, null);
}
