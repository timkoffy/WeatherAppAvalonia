using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Newtonsoft.Json;
using ReactiveUI;
using WeatherAppAvalonia.ViewModels;
using WeatherAppAvalonia.WeatherService;

namespace WeatherAppAvalonia.Views;

public partial class MainWindow : Window 
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    
    
    // private async void LoadWeatherCommand(object sender, RoutedEventArgs e)
    // {
    //     var result = await Dispatcher.UIThread.InvokeAsync(LoadWeatherAsync, DispatcherPriority.Background);
    //     
    //     this.FindControl<TextBlock>("TestText").Text = result;
    //     this.FindControl<Button>("TestButton").IsEnabled = true;
    // }
}
