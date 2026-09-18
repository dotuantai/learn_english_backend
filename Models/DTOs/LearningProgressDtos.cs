using System.ComponentModel.DataAnnotations;

namespace learn_english_backend.Models.DTOs;

public sealed record LearningProgressResponse(
    IReadOnlyCollection<int> MasteredWordIds);

public sealed record UpdateWordProgressRequest(bool Mastered);

public sealed record ImportLearningProgressRequest(
    [Required] IReadOnlyCollection<int> MasteredWordIds);
