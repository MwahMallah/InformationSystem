using CommunityToolkit.Mvvm.Messaging;
using InformationSystem.App.Services;
using InformationSystem.App.ViewModels;
using InformationSystem.App.Views;

namespace InformationSystem.App;

public static class AppInstaller
{
    public static IServiceCollection AddAppServices(this IServiceCollection services)
    {
        services.AddSingleton<AppShell>();
        
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<BaseView>())
            .AsSelf()
            .WithTransientLifetime());
        
        services.Scan(selector => selector
            .FromAssemblyOf<App>()
            .AddClasses(filter => filter.AssignableTo<IViewModel>())
            .AsSelfWithInterfaces()
            .WithTransientLifetime());

        services.AddTransient<INavigationService, NavigationService>();
        return services;
    }
}