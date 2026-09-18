namespace learn_english_backend.Models.Entities;

public sealed class RefreshToken
{
    private RefreshToken()
    {
    }

    public RefreshToken(
        string tokenHash,
        string userId,
        string securityStamp,
        DateTime createdAtUtc,
        DateTime expiresAtUtc)
    {
        Id = Guid.NewGuid();
        TokenHash = tokenHash;
        UserId = userId;
        SecurityStamp = securityStamp;
        CreatedAtUtc = createdAtUtc;
        ExpiresAtUtc = expiresAtUtc;
        ConcurrencyToken = Guid.NewGuid();
    }

    public Guid Id { get; private set; }

    public string TokenHash { get; private set; } = string.Empty;

    public string UserId { get; private set; } = string.Empty;

    public string SecurityStamp { get; private set; } = string.Empty;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime ExpiresAtUtc { get; private set; }

    public DateTime? RevokedAtUtc { get; private set; }

    public Guid? ReplacedByTokenId { get; private set; }

    public Guid ConcurrencyToken { get; private set; }

    public User User { get; private set; } = null!;

    public bool IsActive(DateTime utcNow) =>
        RevokedAtUtc is null && ExpiresAtUtc > utcNow;

    public void Revoke(DateTime utcNow, Guid? replacedByTokenId = null)
    {
        RevokedAtUtc = utcNow;
        ReplacedByTokenId = replacedByTokenId;
        ConcurrencyToken = Guid.NewGuid();
    }
}
