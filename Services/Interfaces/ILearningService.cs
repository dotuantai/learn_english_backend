using learn_english_backend.Models.DTOs;

namespace learn_english_backend.Services.Interfaces;

public interface ILearningService
{
    Task<LearningContentResponse> GetContentAsync(
        CancellationToken cancellationToken);

    Task<LearningProgressResponse> GetProgressAsync(
        string userId,
        CancellationToken cancellationToken);

    Task<bool> SetMasteredAsync(
        string userId,
        int wordId,
        bool mastered,
        CancellationToken cancellationToken);

    Task<LearningProgressResponse> ImportProgressAsync(
        string userId,
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken);
}
