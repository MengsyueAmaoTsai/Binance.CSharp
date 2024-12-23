﻿using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdM;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Data;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public sealed partial class SymbolsViewModel : ViewModel
{
    public SymbolsViewModel(
        IWindowService windowService,
        IDialogService dialogService,
        IMessageBoxService messageBoxService,
        IBinanceSpotRestClient binanceSpotRestClient,
        IBinanceUsdMRestClient binanceUsdMRestClient)
        : base(windowService, dialogService, messageBoxService, binanceSpotRestClient, binanceUsdMRestClient)
    {
        BindingOperations.EnableCollectionSynchronization(Symbols, new object());
    }

    public ObservableCollection<BinanceSymbolResponse> Symbols { get; } = [];

    protected override async Task InitializeAsync()
    {
        await GetExchangeInfoAsync();
    }

    [RelayCommand]
    private async Task GetExchangeInfoAsync()
    {
        var result = await _binanceUsdMRestClient.GetExchangeInfoAsync(default);

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
