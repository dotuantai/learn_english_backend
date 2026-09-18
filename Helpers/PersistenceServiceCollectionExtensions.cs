using learn_english_backend.Data;
using learn_english_backend.Repositories;
using learn_english_backend.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace learn_english_backend.Helpers;

public static class PersistenceServiceCollectionExtensions
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("PostgresConnection")
            ?? throw new InvalidOperationException(
                "Connection string 'PostgresConnection' was not found.");

        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ILearningRepository, LearningRepository>();

        return services;
    }
}
