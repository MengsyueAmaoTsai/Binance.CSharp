using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.UsdM;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public abstract partial class ViewModel : ObservableObject, IViewModel
{
    protected readonly IWindowService _windowService;
    protected readonly IMessageBoxService _messageBoxService;
    protected readonly IBinanceUsdMRestClient _binanceUsdMRestClient;

    protected ViewModel(
        IWindowService windowService,
        IMessageBoxService messageBoxService,
        IBinanceUsdMRestClient binanceUsdMRestClient)
    {
        _windowService = windowService;
        _messageBoxService = messageBoxService;
        _binanceUsdMRestClient = binanceUsdMRestClient;

        InitializeAsyncCommand = new AsyncRelayCommand(
            InitializeAsync, 
            AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    protected virtual Task InitializeAsync() => Task.CompletedTask;
}
