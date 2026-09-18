using learn_english_backend.Models.DTOs;
using learn_english_backend.Models.Results;

namespace learn_english_backend.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResult> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken);

    Task<AuthResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken);

    Task<AuthResult> RefreshAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken);

    Task RevokeAsync(
        RevokeTokenRequest request,
        CancellationToken cancellationToken);
}
