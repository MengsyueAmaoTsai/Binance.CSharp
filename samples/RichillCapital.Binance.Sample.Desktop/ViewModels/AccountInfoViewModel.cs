using CommunityToolkit.Mvvm.ComponentModel;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class AccountInfoViewModel : ViewModel
{
    public AccountInfoViewModel(
        IWindowService windowService,
        IMessageBoxService messageBoxService,
        IBinanceUsdMRestClient binanceUsdMRestClient) 
        : base(windowService, messageBoxService, binanceUsdMRestClient)
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
