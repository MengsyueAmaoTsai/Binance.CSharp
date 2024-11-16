using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class MainViewModel : ViewModel
{
    private readonly IBinanceUsdMRestClient _usdMRestClient;

    public MainViewModel(IBinanceUsdMRestClient usdMRestClient)
    {
        _usdMRestClient = usdMRestClient;   
    }

    [ObservableProperty]
    private DateTimeOffset _serverTime;

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

        var response = result.Value;
        var message = "";

        var firstSymbol = response.Symbols.FirstOrDefault();

        if (firstSymbol is not null)
        {
            message = $"First symbol:\n";
            message += $"Symbol: {firstSymbol.Symbol}\n";
            message += $"Pair: {firstSymbol.Pair}\n";
            message += $"Contract type: {firstSymbol.ContractType}\n";
            message += $"Delivery date: {firstSymbol.DeliveryDate}\n";
            message += $"On board date: {firstSymbol.OnBoardDate}\n";
        }
        else
        {
            message = "No symbols found";
        }

        MessageBox.Show(message);
    }
}