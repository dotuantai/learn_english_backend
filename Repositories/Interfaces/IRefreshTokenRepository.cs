using learn_english_backend.Models.Entities;

namespace learn_english_backend.Repositories.Interfaces;

public interface IRefreshTokenRepository
{
    // Returns a tracked token with its user so service mutations are persisted on save.
    Task<RefreshToken?> FindByHashAsync(string tokenHash, CancellationToken cancellationToken);

    void Add(RefreshToken refreshToken);

    Task SaveChangesAsync(CancellationToken cancellationToken);

    // Atomically saves the rotation (old token revocation + replacement insertion).
    // Returns false on an optimistic concurrency conflict; other failures propagate.
    Task<bool> TrySaveChangesAsync(CancellationToken cancellationToken);
}
