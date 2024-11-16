using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Models;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Sample.Desktop.Views.Windows;
using RichillCapital.Binance.UsdM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class MainViewModel : ViewModel
{
    public MainViewModel(
        IWindowService windowService,
        IMessageBoxService messageBoxService,
        IBinanceUsdMRestClient usdMRestClient)
        : base(windowService, messageBoxService ,usdMRestClient)
    {
        BindingOperations.EnableCollectionSynchronization(Logs, new object());
        BindingOperations.EnableCollectionSynchronization(Symbols, new object());
    }

    [ObservableProperty]
    private DateTimeOffset _serverTime;

    [ObservableProperty]
    private bool _serverAvailable;

    [ObservableProperty]
    private BinanceSymbolResponse? _selectedSymbol;

    [ObservableProperty]
    private string _orderType = "Market";

    [ObservableProperty]
    private string _side = "Buy";

    [ObservableProperty]
    private decimal _quantity;

    public ObservableCollection<LogDataGridItem> Logs { get; } = [];
    public ObservableCollection<BinanceSymbolResponse> Symbols { get; } = [];

    protected override async Task InitializeAsync()
    {
        await TestConnectivityAsync();
        await GetServerTimeAsync();
        await LoadTradableInstrumentsAsync();

        if (Symbols.Count == 0)
        {
            return;
        }

        var defaultSymbol = Symbols.First();

        SelectedSymbol = defaultSymbol;
    }

    #region File menu commands
    
    [RelayCommand]
    private void Quit() => Application.Current.Shutdown();

    [RelayCommand]
    private void ShowSymbolsWindow() => _windowService.ShowWindow<SymbolsWindow>();

    [RelayCommand]
    private void ShowAccountInfoWindow() => _windowService.ShowWindow<AccountInfoWindow>();
    
    #endregion

    [RelayCommand]
    private async Task PlaceOrderAsync()
    {
        if (SelectedSymbol is null)
        {
            MessageBox.Show("Please select a symbol.");
            return;
        }

        if (Quantity <= 0)
        {
            MessageBox.Show("Please enter a valid quantity.");
            return;
        }

        var isConfirmed = MessageBox.Show(
            $"Are you sure you want to place a market order for {Quantity} {SelectedSymbol.Symbol}?",
            "Confirmation",
            MessageBoxButton.YesNo);

        if (isConfirmed.HasFlag(MessageBoxResult.No))
        {
            return;
        }

        var result = await _binanceUsdMRestClient.NewOrderAsync(
            new NewOrderRequest
            {
                Symbol = SelectedSymbol.Symbol,
                Side = "BUY",
                Type = "MARKET",
                Quantity = Quantity,
            },
            default);

        if (result.IsFailure)
        {
            MessageBox.Show($"Failed on placing order: {result.Error.Message}");
            return;
        }

        MessageBox.Show($"Place order successfully. {result.Value}");
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
    }
}