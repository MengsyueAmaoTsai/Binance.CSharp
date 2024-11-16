using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Sample.Desktop.ViewModels;
using RichillCapital.Binance.Sample.Desktop.Views.Windows;
using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop;


public sealed partial class App : Application
{
    [STAThread]
    private static void Main(string[] args) => StartAsync(args).GetAwaiter().GetResult();

    private static async Task StartAsync(string[] args)
    {
        using var host = CreateHostBuilder(args).Build();

        await host.StartAsync().ConfigureAwait(true);

        var app = new App();

        app.InitializeComponent();

        app.MainWindow = host.Services.GetRequiredService<MainWindow>();
        app.MainWindow.Show();

        app.Run();

        await host
            .StopAsync()
            .ConfigureAwait(true);
    }

    private static IHostBuilder CreateHostBuilder(string[] args) => Host
        .CreateDefaultBuilder(args)
        .ConfigureAppConfiguration((hostBuilderContext, configurationBuilder) =>
            configurationBuilder.AddUserSecrets(typeof(App).Assembly))
        .ConfigureServices((hostContext, services) =>
        {
            services.AddBinanceSpot();
            services.AddBinanceUsdM();

            services.AddWindowService();

            services.AddViews();
            services.AddViewModels();

            services.AddCommunityToolkitMvvm();

            services.AddSingleton(_ => Current.Dispatcher);
        });
}

internal static class ServiceExtensions
{
    internal static IServiceCollection AddViews(this IServiceCollection services)
    {
        services.AddSingleton<MainWindow>();

        services.AddTransient<SymbolsWindow>();

        return services;
    }

    internal static IServiceCollection AddViewModels(this IServiceCollection services)
    {
        services.AddSingleton<MainViewModel>();

        services.AddTransient<SymbolsViewModel>();

        return services;
    }

    internal static IServiceCollection AddCommunityToolkitMvvm(this IServiceCollection services)
    {
        services.AddSingleton<WeakReferenceMessenger>();
        services.AddSingleton<IMessenger, WeakReferenceMessenger>(provider =>
            provider.GetRequiredService<WeakReferenceMessenger>());

        return services;
    }
}