using RichillCapital.Binance.Sample.Desktop.ViewModels;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.Views.Windows;

public sealed partial class SymbolsWindow : Window
{
    public SymbolsWindow(SymbolsViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        Height = SystemParameters.PrimaryScreenHeight * 0.8;
        Width = SystemParameters.PrimaryScreenWidth * 0.8;
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e);

        if (DataContext is not SymbolsViewModel viewModel)
        {
            return;
        }

        await viewModel.InitializeAsyncCommand.ExecuteAsync(null);
    }
}
