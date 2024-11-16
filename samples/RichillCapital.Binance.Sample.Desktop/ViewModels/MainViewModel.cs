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
}