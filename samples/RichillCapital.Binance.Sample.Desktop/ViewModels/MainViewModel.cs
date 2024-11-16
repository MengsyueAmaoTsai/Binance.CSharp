using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Sample.Desktop.Views.Windows;
using RichillCapital.Binance.UsdM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

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

        BindingOperations.EnableCollectionSynchronization(Symbols, new object());
    }

    [ObservableProperty]
    private DateTimeOffset _serverTime;

    [ObservableProperty]
    private bool _serverAvailable;

    [ObservableProperty]
    private BinanceSymbolResponse _selectedSymbol;

    public ObservableCollection<BinanceSymbolResponse> Symbols { get; } = [];


    protected override async Task InitializeAsync()
    {
        await TestConnectivityAsync();
        await GetServerTimeAsync();

        await LoadTradableInstrumentsAsync();
    }

    [RelayCommand]
    private void ShowSymbolsWindow() => _windowService.ShowWindow<SymbolsWindow>();

    [RelayCommand]
    private void ShowAccountInfoWindow() => _windowService.ShowWindow<AccountInfoWindow>();

    [RelayCommand]
    private async Task PlaceOrderAsync()
    {
        MessageBox.Show($"Place order for {SelectedSymbol}");
    }
    
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

    private async Task LoadTradableInstrumentsAsync()
    {
        var exchangeInfoResult = await _binanceUsdMRestClient.GetExchangeInfoAsync(default);

        if (exchangeInfoResult.IsFailure)
        {
            MessageBox.Show($"Failed to loading tradable instruments. {exchangeInfoResult.Error.Message}");
            return;
        }

        var exchangeInfo = exchangeInfoResult.Value;

        Symbols.Clear();

        foreach (var symbol in exchangeInfo.Symbols)
        {
            Symbols.Add(symbol);
        }

        if (Symbols.Count > 0)
        {
            SelectedSymbol = Symbols.First();
        }
    }
}