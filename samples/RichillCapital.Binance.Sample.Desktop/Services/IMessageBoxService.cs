using Microsoft.Extensions.DependencyInjection;
using RichillCapital.SharedKernel;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.Services;

public interface IMessageBoxService
{
    MessageBoxResult ShowBinanceError(Error error);
}

internal sealed class MessageBoxService : IMessageBoxService
{
    public MessageBoxResult ShowBinanceError(Error error) => MessageBox.Show(
        messageBoxText: error.Message, 
        caption: "Binance Error", 
        button: MessageBoxButton.OK, 
        icon: MessageBoxImage.Error);
}

internal static class MessageBoxServiceExtensions
{
    internal static IServiceCollection AddMessageBoxService(this IServiceCollection services) =>
        services.AddSingleton<IMessageBoxService, MessageBoxService>();
}