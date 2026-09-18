using learn_english_backend.Models.Entities;
using learn_english_backend.Models.Results;

namespace learn_english_backend.Services.Interfaces;

public interface IJwtTokenGenerator
{
    GeneratedTokenPair Generate(
        User user,
        IReadOnlyCollection<string> roles);

    string HashRefreshToken(string refreshToken);
}
