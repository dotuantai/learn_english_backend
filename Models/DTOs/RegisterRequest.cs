using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Models.DTOs;

public sealed record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password);
