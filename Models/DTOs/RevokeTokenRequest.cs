using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Models.DTOs;

public sealed record RevokeTokenRequest(
    [Required] string RefreshToken);
