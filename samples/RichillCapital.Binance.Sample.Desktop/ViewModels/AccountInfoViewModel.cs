using CommunityToolkit.Mvvm.ComponentModel;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class AccountInfoViewModel : ViewModel
{
    public AccountInfoViewModel(
        IBinanceUsdMRestClient binanceUsdMRestClient) 
        : base(binanceUsdMRestClient)
    {
    }

    [ObservableProperty]
    private BinanceAccountInformationResponse? _accountInfo;

    protected override async Task InitializeAsync()
    {
        var accountInfoResult = await _binanceUsdMRestClient.GetAccountInformationAsync(default);
        
        if (accountInfoResult.IsFailure)
        {
            MessageBox.Show(accountInfoResult.Error.Message);
            return;
        }

        AccountInfo = accountInfoResult.Value;
    }
}
