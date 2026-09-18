using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Models.DTOs;

public sealed record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password);
