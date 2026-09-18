using learn_english_backend.Services;
using learn_english_backend.Services.Interfaces;

namespace learn_english_backend.Helpers;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<ILearningService, LearningService>();

        return services;
    }
}
