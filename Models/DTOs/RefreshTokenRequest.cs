using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Models.DTOs;

public sealed record RefreshTokenRequest(
    [Required] string RefreshToken);
