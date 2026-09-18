using System.IdentityModel.Tokens.Jwt;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using learn_english_backend.Models.Entities;
using learn_english_backend.Models.Results;
using learn_english_backend.Services.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace learn_english_backend.Helpers;

public sealed class JwtHelper(
    IOptions<JwtOptions> options,
    TimeProvider timeProvider) : IJwtTokenGenerator
{
    private readonly JwtOptions _options = options.Value;

    public GeneratedTokenPair Generate(
        User user,
        IReadOnlyCollection<string> roles)
    {
        var issuedAt = timeProvider.GetUtcNow();
        var accessTokenExpiresAt = issuedAt.AddMinutes(
            _options.AccessTokenLifetimeMinutes);
        var refreshTokenExpiresAt = issuedAt.AddDays(
            _options.RefreshTokenLifetimeDays);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(
                JwtRegisteredClaimNames.Iat,
                EpochTime.GetIntDate(issuedAt.UtcDateTime)
                    .ToString(CultureInfo.InvariantCulture),
                ClaimValueTypes.Integer64),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName ?? user.Id)
        };

        if (!string.IsNullOrWhiteSpace(user.Email))
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
        }

        claims.AddRange(
            roles.Distinct(StringComparer.Ordinal)
                .Select(role => new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_options.SigningKey));

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: issuedAt.UtcDateTime,
            expires: accessTokenExpiresAt.UtcDateTime,
            signingCredentials: new SigningCredentials(
                signingKey,
                SecurityAlgorithms.HmacSha256));

        var accessToken = new JwtSecurityTokenHandler().WriteToken(token);
        var refreshToken = WebEncoders.Base64UrlEncode(
            RandomNumberGenerator.GetBytes(64));

        return new GeneratedTokenPair(
            accessToken,
            accessTokenExpiresAt.UtcDateTime,
            refreshToken,
            refreshTokenExpiresAt.UtcDateTime);
    }

    public string HashRefreshToken(string refreshToken)
    {
        var tokenBytes = Encoding.UTF8.GetBytes(refreshToken);
        return Convert.ToHexString(SHA256.HashData(tokenBytes));
    }
}
