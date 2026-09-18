using learn_english_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace learn_english_backend.Data.Configurations;

public sealed class UserWordProgressConfiguration : IEntityTypeConfiguration<UserWordProgress>
{
    public void Configure(EntityTypeBuilder<UserWordProgress> entity)
    {
        entity.ToTable("UserWordProgress");
        entity.HasKey(progress => new { progress.UserId, progress.WordId });

        entity.HasOne(progress => progress.User)
            .WithMany(user => user.WordProgress)
            .HasForeignKey(progress => progress.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        entity.HasOne(progress => progress.Word)
            .WithMany(word => word.UserProgress)
            .HasForeignKey(progress => progress.WordId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
