using Microsoft.AspNetCore.Identity;

namespace learn_english_backend.Models.Entities;

public sealed class User : IdentityUser
{
    public ICollection<RefreshToken> RefreshTokens { get; } = [];
    public ICollection<UserWordProgress> WordProgress { get; } = [];
}
