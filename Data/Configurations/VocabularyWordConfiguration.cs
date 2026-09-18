using learn_english_backend.Models.Entities;
using learn_english_backend.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace learn_english_backend.Data.Configurations;

public sealed class VocabularyWordConfiguration : IEntityTypeConfiguration<VocabularyWord>
{
    public void Configure(EntityTypeBuilder<VocabularyWord> entity)
    {
        entity.ToTable("VocabularyWords");
        entity.HasKey(word => word.Id);

        entity.Property(word => word.Id).ValueGeneratedNever();
        entity.Property(word => word.LessonId).HasMaxLength(64).IsRequired();
        entity.Property(word => word.Text).HasMaxLength(160).IsRequired();
        entity.Property(word => word.Type).HasMaxLength(40).IsRequired();
        entity.Property(word => word.Ipa).HasMaxLength(160).IsRequired();
        entity.Property(word => word.Meaning).HasMaxLength(600).IsRequired();

        entity.HasIndex(word => new { word.LessonId, word.Order }).IsUnique();
        entity.HasOne(word => word.Lesson)
            .WithMany(lesson => lesson.Words)
            .HasForeignKey(word => word.LessonId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasData(LearningSeedData.Words.Select(word => new
        {
            word.Id,
            word.LessonId,
            word.Order,
            word.Text,
            word.Type,
            word.Ipa,
            word.Meaning
        }));
    }
}
