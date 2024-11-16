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

        MessageBox.Show($"Server time: {result.Value.ServerTime:yyyy-MM-dd HH:mm:ss.fff}");
    }
}