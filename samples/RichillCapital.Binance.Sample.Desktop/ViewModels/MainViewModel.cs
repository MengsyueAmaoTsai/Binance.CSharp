using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.UsdM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class MainViewModel : ViewModel
{
    private readonly IBinanceUsdMRestClient _usdMRestClient;

    public MainViewModel(IBinanceUsdMRestClient usdMRestClient)
    {
        _usdMRestClient = usdMRestClient;   
        
        BindingOperations.EnableCollectionSynchronization(Symbols, new object());
    }

    [ObservableProperty]
    private DateTimeOffset _serverTime;

    public ObservableCollection<BinanceSymbolResponse> Symbols { get; } = [];

    [RelayCommand]
    private async Task TestConnectivityAsync()
    {
        var result = await _usdMRestClient.TestConnectivityAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }

        MessageBox.Show("Connected!");
    }

    [RelayCommand]
    private async Task GetServerTimeAsync()
    {
        var result = await _usdMRestClient.GetServerTimeAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }

        ServerTime = result.Value.ServerTime;
    }

    [RelayCommand]
    private async Task GetExchangeInfoAsync()
    {
        var result = await _usdMRestClient.GetExchangeInfoAsync(default);

        if (result.IsFailure)
        {
            MessageBox.Show(result.Error.Message);
            return;
        }

        Symbols.Clear();

        var response = result.Value;
        
        foreach (var symbol in response.Symbols)
        {
            Symbols.Add(symbol);
        }
    }
}