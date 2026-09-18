namespace learn_english_backend.Models.Entities;

public sealed class UserWordProgress
{
    public string UserId { get; set; } = string.Empty;
    public int WordId { get; set; }
    public DateTime MasteredAtUtc { get; set; }

    public User User { get; set; } = null!;
    public VocabularyWord Word { get; set; } = null!;
}
