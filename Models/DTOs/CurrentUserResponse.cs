namespace learn_english_backend.Models.DTOs;

public sealed record CurrentUserResponse(
    string UserId,
    string? Email,
    IReadOnlyCollection<string> Roles);
