using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.UsdM;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public abstract partial class ViewModel : ObservableObject, IViewModel
{
    protected readonly IBinanceUsdMRestClient _binanceUsdMRestClient;

    protected ViewModel(IBinanceUsdMRestClient binanceUsdMRestClient)
    {
        _binanceUsdMRestClient = binanceUsdMRestClient;

        InitializeAsyncCommand = new AsyncRelayCommand(
            InitializeAsync, 
            AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    protected virtual Task InitializeAsync() => Task.CompletedTask;
}
