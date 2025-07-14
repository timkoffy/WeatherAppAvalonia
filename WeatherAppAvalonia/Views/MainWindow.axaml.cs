using System.Reactive.Linq;
using Avalonia.Controls;
using WeatherAppAvalonia.ViewModels;

namespace WeatherAppAvalonia.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var viewModel = new MainViewModel();
        DataContext = viewModel;
        
        this.Opened += async (_, _) =>
        {
            await viewModel.LoadWeatherCommand.Execute();
        };
    }
}