using System.Reflection;
using CommunityToolkit.Maui.Core;
using InformationSystem.BL;
using InformationSystem.BL.Facades;
using InformationSystem.DAL;
using InformationSystem.DAL.Migrator;
using InformationSystem.DAL.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace InformationSystem.App;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkitCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        ConfigureAppSettings(builder);

        builder.Services
            .AddDALServices(GetDALOptions(builder.Configuration))
            .AddAppServices()
            .AddBLServices();
        
        var app = builder.Build();

        MigrateDb(app.Services.GetRequiredService<IDbMigrator>());
        return app;
    }

    private static void MigrateDb(IDbMigrator migrator)
    {
        migrator.Migrate();
    }

    private static DALOptions GetDALOptions(IConfiguration configuration)
    {
        DALOptions options = new()
        {
            DatabaseDirectory = FileSystem.AppDataDirectory,
        };
        
        configuration.GetSection("InformationSystem:DAL").Bind(options);

        return options;
    }

    private static void ConfigureAppSettings(MauiAppBuilder builder)
    {
        var configurationBuilder = new ConfigurationBuilder();

        var assembly = Assembly.GetExecutingAssembly();
        const string appSettingsFilePath = "InformationSystem.App.appsettings.json";

        using var appSettingsStream = assembly.GetManifestResourceStream(appSettingsFilePath);
        if (appSettingsStream is not null)
        {
            configurationBuilder.AddJsonStream(appSettingsStream);
        }

        var configuration = configurationBuilder.Build();
        builder.Configuration.AddConfiguration(configuration);
    }
}