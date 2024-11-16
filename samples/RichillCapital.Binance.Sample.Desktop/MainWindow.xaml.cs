using RichillCapital.Binance.Sample.Desktop.ViewModels;
using System.Windows;

namespace RichillCapital.Binance.Sample.Desktop;

public sealed partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        DataContext = viewModel;
        InitializeComponent();

        Height = SystemParameters.PrimaryScreenHeight * 0.8;
        Width = SystemParameters.PrimaryScreenWidth * 0.8;
    }
}