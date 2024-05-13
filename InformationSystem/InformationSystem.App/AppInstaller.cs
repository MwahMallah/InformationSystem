using CommunityToolkit.Mvvm.Messaging;

namespace InformationSystem.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();
        services.AddTransient<MainPage>();

        return services;
    }
}