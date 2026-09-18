using learn_english_backend.Models.Entities;
using learn_english_backend.Data.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace learn_english_backend.Data.Configurations;

public sealed class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> entity)
    {
        entity.ToTable("Lessons");
        entity.HasKey(lesson => lesson.Id);

        entity.Property(lesson => lesson.Id).HasMaxLength(64);
        entity.Property(lesson => lesson.Key).HasMaxLength(64).IsRequired();
        entity.Property(lesson => lesson.Title).HasMaxLength(160).IsRequired();
        entity.Property(lesson => lesson.English).HasMaxLength(160).IsRequired();
        entity.Property(lesson => lesson.Description).HasMaxLength(800).IsRequired();
        entity.Property(lesson => lesson.Icon).HasMaxLength(32).IsRequired();
        entity.Property(lesson => lesson.Color).HasMaxLength(32).IsRequired();

        entity.HasIndex(lesson => lesson.Key).IsUnique();
        entity.HasIndex(lesson => lesson.Number).IsUnique();

        entity.HasData(LearningSeedData.Lessons.Select(lesson => new
        {
            lesson.Id,
            lesson.Key,
            lesson.Number,
            lesson.Title,
            lesson.English,
            lesson.Description,
            lesson.Icon,
            lesson.Color
        }));
    }
}
