using learn_english_backend.Models.DTOs;
using learn_english_backend.Repositories.Interfaces;
using learn_english_backend.Services.Interfaces;

namespace learn_english_backend.Services;

public sealed class LearningService(
    ILearningRepository learningRepository,
    TimeProvider timeProvider) : ILearningService
{
    public async Task<LearningContentResponse> GetContentAsync(
        CancellationToken cancellationToken)
    {
        var lessons = await learningRepository.GetLessonsAsync(cancellationToken);

        return new LearningContentResponse(
            lessons.Select(lesson => new LessonResponse(
                lesson.Id,
                lesson.Key,
                lesson.Number,
                lesson.Title,
                lesson.English,
                lesson.Description,
                lesson.Icon,
                lesson.Color,
                lesson.Words
                    .OrderBy(word => word.Order)
                    .Select(word => new WordResponse(
                        word.Id,
                        word.Text,
                        word.Type,
                        word.Ipa,
                        word.Meaning))
                    .ToArray()))
                .ToArray());
    }

    public async Task<LearningProgressResponse> GetProgressAsync(
        string userId,
        CancellationToken cancellationToken) =>
        new(await learningRepository.GetMasteredWordIdsAsync(
            userId, cancellationToken));

    public async Task<bool> SetMasteredAsync(
        string userId,
        int wordId,
        bool mastered,
        CancellationToken cancellationToken)
    {
        if (!await learningRepository.WordExistsAsync(
                wordId, cancellationToken))
        {
            return false;
        }

        await learningRepository.SetMasteredAsync(
            userId,
            wordId,
            mastered,
            timeProvider.GetUtcNow().UtcDateTime,
            cancellationToken);

        return true;
    }

    public async Task<LearningProgressResponse> ImportProgressAsync(
        string userId,
        IReadOnlyCollection<int> wordIds,
        CancellationToken cancellationToken) =>
        new(await learningRepository.ImportMasteredAsync(
            userId,
            wordIds,
            timeProvider.GetUtcNow().UtcDateTime,
            cancellationToken));
}
