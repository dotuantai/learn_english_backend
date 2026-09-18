namespace learn_english_backend.Models.Entities;

public sealed class VocabularyWord
{
    public int Id { get; set; }
    public string LessonId { get; set; } = string.Empty;
    public int Order { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Ipa { get; set; } = string.Empty;
    public string Meaning { get; set; } = string.Empty;

    public Lesson Lesson { get; set; } = null!;
    public ICollection<UserWordProgress> UserProgress { get; } = [];
}
