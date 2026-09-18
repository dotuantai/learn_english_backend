using learn_english_backend.Models.Entities;

namespace learn_english_backend.Repositories.Interfaces;

public interface ILearningRepository
{
    Task<IReadOnlyCollection<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<int>> GetMasteredWordIdsAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<bool> WordExistsAsync(
        int wordId,
        CancellationToken cancellationToken);

    Task SetMasteredAsync(
        string userId,
        int wordId,
        bool mastered,
        DateTime utcNow,
        CancellationToken cancellationToken);

    Task<IReadOnlyCollection<int>> ImportMasteredAsync(
        string userId,
        IReadOnlyCollection<int> wordIds,
        DateTime utcNow,
        CancellationToken cancellationToken);
}
