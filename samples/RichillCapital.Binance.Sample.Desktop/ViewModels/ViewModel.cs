﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RichillCapital.Binance.Sample.Desktop.Services;
using RichillCapital.Binance.Spot;
using RichillCapital.Binance.UsdM;

namespace RichillCapital.Binance.Sample.Desktop.ViewModels;

public abstract partial class ViewModel : ObservableObject, IViewModel
{
    protected readonly IWindowService _windowService;
    protected readonly IDialogService _dialogService;
    protected readonly IMessageBoxService _messageBoxService;
    protected readonly IBinanceSpotRestClient _binanceSpotRestClient;
    protected readonly IBinanceUsdMRestClient _binanceUsdMRestClient;

    protected ViewModel(
        IWindowService windowService,
        IDialogService dialogService,
        IMessageBoxService messageBoxService,
        IBinanceSpotRestClient binanceSpotRestClient,
        IBinanceUsdMRestClient binanceUsdMRestClient)
    {
        _windowService = windowService;
        _dialogService = dialogService;
        _messageBoxService = messageBoxService;

        _binanceSpotRestClient = binanceSpotRestClient;
        _binanceUsdMRestClient = binanceUsdMRestClient;

        InitializeAsyncCommand = new AsyncRelayCommand(
            InitializeAsync,
            AsyncRelayCommandOptions.FlowExceptionsToTaskScheduler);
    }

    public IAsyncRelayCommand InitializeAsyncCommand { get; }

    protected virtual Task InitializeAsync() => Task.CompletedTask;
}
