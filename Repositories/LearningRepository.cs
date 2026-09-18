using learn_english_backend.Data;
using learn_english_backend.Models.Entities;
using learn_english_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace learn_english_backend.Repositories;

public sealed class LearningRepository(AppDbContext dbContext)
    : ILearningRepository
{
    public async Task<IReadOnlyCollection<Lesson>> GetLessonsAsync(
        CancellationToken cancellationToken) =>
        await dbContext.Lessons
            .AsNoTracking()
            .Include(lesson => lesson.Words)
            .OrderBy(lesson => lesson.Number)
            .ToArrayAsync(cancellationToken);

    public async Task<IReadOnlyCollection<int>> GetMasteredWordIdsAsync(
        string userId,
        CancellationToken cancellationToken) =>
        await dbContext.UserWordProgress
            .AsNoTracking()
            .Where(progress => progress.UserId == userId)
            .OrderBy(progress => progress.WordId)
            .Select(progress => progress.WordId)
            .ToArrayAsync(cancellationToken);

    public Task<bool> WordExistsAsync(
        int wordId,
        CancellationToken cancellationToken) =>
        dbContext.VocabularyWords.AnyAsync(
            word => word.Id == wordId,
            cancellationToken);

    public async Task SetMasteredAsync(
        string userId,
        int wordId,
        bool mastered,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        var progress = await dbContext.UserWordProgress.FindAsync(
            [userId, wordId], cancellationToken);

        if (mastered && progress is null)
        {
            dbContext.UserWordProgress.Add(new UserWordProgress
            {
                UserId = userId,
                WordId = wordId,
                MasteredAtUtc = utcNow
            });
        }
        else if (!mastered && progress is not null)
        {
            dbContext.UserWordProgress.Remove(progress);
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<int>> ImportMasteredAsync(
        string userId,
        IReadOnlyCollection<int> wordIds,
        DateTime utcNow,
        CancellationToken cancellationToken)
    {
        var requestedIds = wordIds.Distinct().ToArray();
        var validIds = await dbContext.VocabularyWords
            .Where(word => requestedIds.Contains(word.Id))
            .Select(word => word.Id)
            .ToArrayAsync(cancellationToken);
        var existingIds = await dbContext.UserWordProgress
            .Where(progress => progress.UserId == userId)
            .Select(progress => progress.WordId)
            .ToArrayAsync(cancellationToken);
        var existing = existingIds.ToHashSet();

        dbContext.UserWordProgress.AddRange(
            validIds
                .Where(wordId => !existing.Contains(wordId))
                .Select(wordId => new UserWordProgress
                {
                    UserId = userId,
                    WordId = wordId,
                    MasteredAtUtc = utcNow
                }));

        await dbContext.SaveChangesAsync(cancellationToken);

        return existingIds
            .Concat(validIds)
            .Distinct()
            .Order()
            .ToArray();
    }
}
