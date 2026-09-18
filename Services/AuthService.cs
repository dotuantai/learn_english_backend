using learn_english_backend.Models.DTOs;
using learn_english_backend.Models.Entities;
using learn_english_backend.Models.Results;
using learn_english_backend.Repositories.Interfaces;
using learn_english_backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace learn_english_backend.Services;

public sealed class AuthService(
    UserManager<User> userManager,
    SignInManager<User> signInManager,
    IRefreshTokenRepository refreshTokenRepository,
    IJwtTokenGenerator tokenGenerator,
    TimeProvider timeProvider) : IAuthService
{
    public async Task<AuthResult> RegisterAsync(
        RegisterRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var email = request.Email.Trim();
        var user = new User
        {
            Email = email,
            UserName = email
        };

        var createResult = await userManager.CreateAsync(user, request.Password);

        if (!createResult.Succeeded)
        {
            return AuthResult.Validation(ToValidationErrors(createResult));
        }

        var response = await CreateTokenPairAsync(user);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return AuthResult.Success(response);
    }

    public async Task<AuthResult> LoginAsync(
        LoginRequest request,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var user = await userManager.FindByEmailAsync(request.Email.Trim());

        if (user is null)
        {
            return InvalidCredentials();
        }

        var signInResult = await signInManager.CheckPasswordSignInAsync(
            user,
            request.Password,
            lockoutOnFailure: true);

        if (!signInResult.Succeeded)
        {
            return signInResult.IsLockedOut
                ? AuthResult.Unauthorized(
                    "account_locked",
                    "The account is temporarily locked.")
                : InvalidCredentials();
        }

        var response = await CreateTokenPairAsync(user);
        await refreshTokenRepository.SaveChangesAsync(cancellationToken);

        return AuthResult.Success(response);
    }

    public async Task<AuthResult> RefreshAsync(
        RefreshTokenRequest request,
        CancellationToken cancellationToken)
    {
        var tokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);
        var storedToken = await refreshTokenRepository.FindByHashAsync(
            tokenHash, cancellationToken);

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        if (storedToken is null ||
            !storedToken.IsActive(utcNow) ||
            !string.Equals(
                storedToken.SecurityStamp,
                storedToken.User.SecurityStamp,
                StringComparison.Ordinal))
        {
            return InvalidRefreshToken();
        }

        if (await userManager.IsLockedOutAsync(storedToken.User))
        {
            return AuthResult.Unauthorized(
                "account_locked",
                "The account is temporarily locked.");
        }

        var (response, replacementToken) =
            await CreateTokenPairWithEntityAsync(storedToken.User);

        storedToken.Revoke(utcNow, replacementToken.Id);

        if (!await refreshTokenRepository.TrySaveChangesAsync(cancellationToken))
        {
            return InvalidRefreshToken();
        }

        return AuthResult.Success(response);
    }

    public async Task RevokeAsync(
        RevokeTokenRequest request,
        CancellationToken cancellationToken)
    {
        var tokenHash = tokenGenerator.HashRefreshToken(request.RefreshToken);
        var storedToken = await refreshTokenRepository.FindByHashAsync(
            tokenHash, cancellationToken);

        var utcNow = timeProvider.GetUtcNow().UtcDateTime;

        if (storedToken is null || !storedToken.IsActive(utcNow))
        {
            return;
        }

        storedToken.Revoke(utcNow);

        // Revocation remains idempotent if another request already changed the token.
        await refreshTokenRepository.TrySaveChangesAsync(cancellationToken);
    }

    private async Task<AuthResponse> CreateTokenPairAsync(
        User user)
    {
        var (response, _) = await CreateTokenPairWithEntityAsync(user);
        return response;
    }

    private async Task<(AuthResponse Response, RefreshToken RefreshToken)>
        CreateTokenPairWithEntityAsync(User user)
    {
        var roles = await userManager.GetRolesAsync(user);
        var generatedTokens = tokenGenerator.Generate(user, roles.ToArray());

        var refreshToken = new RefreshToken(
            tokenGenerator.HashRefreshToken(generatedTokens.RefreshToken),
            user.Id,
            user.SecurityStamp ?? string.Empty,
            timeProvider.GetUtcNow().UtcDateTime,
            generatedTokens.RefreshTokenExpiresAtUtc);

        refreshTokenRepository.Add(refreshToken);

        var response = new AuthResponse(
            "Bearer",
            generatedTokens.AccessToken,
            generatedTokens.AccessTokenExpiresAtUtc,
            generatedTokens.RefreshToken,
            generatedTokens.RefreshTokenExpiresAtUtc);

        return (response, refreshToken);
    }

    private static Dictionary<string, string[]> ToValidationErrors(
        IdentityResult identityResult) =>
        identityResult.Errors
            .GroupBy(error => error.Code)
            .ToDictionary(
                group => group.Key,
                group => group.Select(error => error.Description).ToArray());

    private static AuthResult InvalidCredentials() =>
        AuthResult.Unauthorized(
            "invalid_credentials",
            "The email or password is invalid.");

    private static AuthResult InvalidRefreshToken() =>
        AuthResult.Unauthorized(
            "invalid_refresh_token",
            "The refresh token is invalid or expired.");
}
