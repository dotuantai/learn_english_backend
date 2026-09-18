namespace learn_english_backend.Models.DTOs;

public sealed record LearningContentResponse(
    IReadOnlyCollection<LessonResponse> Lessons);

public sealed record LessonResponse(
    string Id,
    string Key,
    int Number,
    string Title,
    string English,
    string Description,
    string Icon,
    string Color,
    IReadOnlyCollection<WordResponse> Words);

public sealed record WordResponse(
    int Id,
    string Word,
    string Type,
    string Ipa,
    string Meaning);
