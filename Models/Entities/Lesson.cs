namespace learn_english_backend.Models.Entities;

public sealed class Lesson
{
    public string Id { get; set; } = string.Empty;
    public string Key { get; set; } = string.Empty;
    public int Number { get; set; }
    public string Title { get; set; } = string.Empty;
    public string English { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Color { get; set; } = string.Empty;

    public ICollection<VocabularyWord> Words { get; } = [];
}
