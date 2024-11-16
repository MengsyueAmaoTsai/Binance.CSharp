using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Binance.Sample.Desktop.Services;

public interface IDialogService
{
    void ShowDialog<TDialog>() where TDialog : Window;
}

internal sealed class DialogService(
    IServiceProvider _serviceProvider) :
    IDialogService
{
    public void ShowDialog<TDialog>() where TDialog : Window
    {
        var dialog = (Window)_serviceProvider.GetRequiredService(typeof(TDialog));

        _ = dialog.ShowDialog();
    }
}

internal static class DialogServiceExtensions
{
    public static IServiceCollection AddDialogService(this IServiceCollection services) =>
        services.AddSingleton<IDialogService, DialogService>();
}