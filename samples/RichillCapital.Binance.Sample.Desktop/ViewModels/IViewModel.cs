using CommunityToolkit.Mvvm.Input;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public interface IViewModel
{
    IAsyncRelayCommand InitializeAsyncCommand { get; }
}