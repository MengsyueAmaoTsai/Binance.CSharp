using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Sample.Desktop.Views.Windows;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class MainViewModel : ViewModel
{
    private readonly IWindowService _windowService;

    public MainViewModel(
        IBinanceUsdMRestClient usdMRestClient,
        IWindowService windowService)
        : base(usdMRestClient)
    {
        _windowService = windowService;
    }

    [ObservableProperty]
    private DateTimeOffset _serverTime;

    [ObservableProperty]
    private bool _serverAvailable;

    protected override async Task InitializeAsync()
    {
        await TestConnectivityAsync();
        await GetServerTimeAsync();
        await GetAccountInformationAsync();
        await GetAccountBalancesAsync();
    }

    [RelayCommand]
    private void ShowSymbolsWindow() => _windowService.ShowWindow<SymbolsWindow>();

    [RelayCommand]
    private void ShowAccountInfoWindow() => _windowService.ShowWindow<AccountInfoWindow>();
    
    [RelayCommand]
    private async Task TestConnectivityAsync()
    {
        var result = await _binanceUsdMRestClient.TestConnectivityAsync(default);

        if (result.IsFailure)
        {
            ServerAvailable = false;
            MessageBox.Show(result.Error.Message);
            return;
        }

        ServerAvailable = true;
    }

    [RelayCommand]
    private async Task GetServerTimeAsync()
    {
        var result = await _binanceUsdMRestClient.GetServerTimeAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }

        ServerTime = result.Value.ServerTime;
    }

    private async Task GetAccountInformationAsync()
    {
        var result = await _binanceUsdMRestClient.GetAccountInformationAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }
    }

    private async Task GetAccountBalancesAsync()
    {
        var result = await _binanceUsdMRestClient.GetAccountBalancesAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }
    }
}