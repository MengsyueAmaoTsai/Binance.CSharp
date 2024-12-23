﻿using CommunityToolkit.Mvvm.ComponentModel;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdM;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class AccountInfoViewModel : ViewModel
{
    public AccountInfoViewModel(
        IWindowService windowService,
        IDialogService dialogService,
        IMessageBoxService messageBoxService,
        IBinanceSpotRestClient binanceSpotRestClient,
        IBinanceUsdMRestClient binanceUsdMRestClient)
        : base(windowService, dialogService, messageBoxService, binanceSpotRestClient, binanceUsdMRestClient)
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
