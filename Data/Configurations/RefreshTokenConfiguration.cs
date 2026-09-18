using learn_english_backend.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace learn_english_backend.Data.Configurations;

public sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> entity)
    {
        entity.ToTable("RefreshTokens");
        entity.HasKey(refreshToken => refreshToken.Id);

        entity.Property(refreshToken => refreshToken.TokenHash)
            .HasMaxLength(64)
            .IsRequired();

        entity.HasIndex(refreshToken => refreshToken.TokenHash)
            .IsUnique();

        entity.HasIndex(refreshToken => new
        {
            refreshToken.UserId,
            refreshToken.ExpiresAtUtc
        });

        entity.Property(refreshToken => refreshToken.SecurityStamp)
            .HasMaxLength(64)
            .IsRequired();

        entity.Property(refreshToken => refreshToken.ConcurrencyToken)
            .IsConcurrencyToken();

        entity.HasOne(refreshToken => refreshToken.User)
            .WithMany(user => user.RefreshTokens)
            .HasForeignKey(refreshToken => refreshToken.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
