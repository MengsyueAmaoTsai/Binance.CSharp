using RichillCapital.Binance.Sample.Desktop.ViewModels;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop.Views.Windows;

public sealed partial class AccountInfoWindow : Window
{
    public AccountInfoWindow(AccountInfoViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        Height = SystemParameters.PrimaryScreenHeight * 0.8;
        Width = SystemParameters.PrimaryScreenWidth * 0.8;
    }

    protected override async void OnInitialized(EventArgs e)
    {
        base.OnInitialized(e); 

        if (DataContext is not IViewModel viewModel)
        {
            return;
        }

        await viewModel.InitializeAsyncCommand.ExecuteAsync(null);
    }
}
