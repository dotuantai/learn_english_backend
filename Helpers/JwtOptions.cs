using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Helpers;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Issuer { get; set; } = "learn-english-backend";

    [Required]
    public string Audience { get; set; } = "learn-english-client";

    [Required]
    public string SigningKey { get; set; } = string.Empty;

    [Range(1, 1440)]
    public int AccessTokenLifetimeMinutes { get; set; } = 15;

    [Range(1, 365)]
    public int RefreshTokenLifetimeDays { get; set; } = 30;
}
