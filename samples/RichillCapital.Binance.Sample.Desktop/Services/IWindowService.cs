using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.Services;

public interface IWindowService
{
    void ShowWindow<TWindow>() where TWindow : Window;
}

internal sealed class WindowService(
    IServiceProvider _serviceProvider) : IWindowService
{
    public void ShowWindow<TWindow>() where TWindow : Window
    {
        var window = (TWindow)_serviceProvider.GetRequiredService(typeof(TWindow));

        window.Show();
    }
}

internal static class WindowServiceExtensions
{
    internal static IServiceCollection AddWindowService(this IServiceCollection services) =>
        services.AddSingleton<IWindowService, WindowService>();
}