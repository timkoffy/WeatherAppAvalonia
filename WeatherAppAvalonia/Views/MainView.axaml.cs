using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace WeatherAppAvalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }
    
    

    private async Task LongRunningTask()
    {
        this.FindControl<Button>("RunButton").IsEnabled = false;
        this.FindControl<TextBlock>("ResultText").Text = "I'm working ...";
        await Task.Delay(5000);
        this.FindControl<TextBlock>("ResultText").Text = "Done";
        this.FindControl<Button>("RunButton").IsEnabled = true;
    }
    
    private void ButtonClickHandler(object sender, RoutedEventArgs e)
    {
        Dispatcher.UIThread.Post(() => LongRunningTask(), 
                                                DispatcherPriority.Background);
    }
    
    
    
    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        string[] weathers =
        {
            "sunny",
            "sunny",
            "mostly-cloudy-day",
            "mostly-cloudy-day",
            "mostly-cloudy-w-rain-day",
            "rain",
            "rain-w-lights",
            "rain-w-lights",
            "rain-w-lights",
            "cloudy",
            "mostly-cloudy-day",
            "mostly-cloudy-day",
            "mostly-cloudy-w-rain-day",
            "mostly-cloudy-w-rain-day",
            "sunny",
            "sunny",
            "mostly-cloudy-day",
            "mostly-cloudy-night",
            "mostly-cloudy-w-rain-night",
            "moony",
            "moony",
            "mostly-cloudy-w-rain-night",
            "mostly-cloudy-w-rain-night",
            "mostly-cloudy-night",
            "moony"
        };
        
        string[] temperatures =
        {
            "+13°",
            "+15°",
            "+17°",
            "+19°",
            "+20°",
            "+20°",
            "+20°",
            "+21°",
            "+21°",
            "+21°",
            "+20°",
            "+19°",
            "+20°",
            "+18°",
            "+17°",
            "+15°",
            "+13°",
            "+11°",
            "+11°",
            "+10°",
            "+10°",
            "+10°",
            "+11°",
            "+12°"
        };

        string[] times = new string[24];
        for (int i = 0; i < 24; i++)
        {
            int hour = (7 + i) % 24;
            times[i] = hour.ToString("D2")+":00";
        }

        for (int i = 0; i < 24; i++)
        {
            switch (times[i])
            {
                case "09:00":
                    SpawnForecastInStackPanel("Сейчас", weathers[i], temperatures[i], true);
                    break;
                case "00:00":
                    var rectangle = new Rectangle {Width = 3, Height = 130, RadiusX = 1.5, RadiusY = 1.5, Fill = SolidColorBrush.Parse("#5060728B") };
                    ForecastStackPanel.Children.Add(rectangle);
                    SpawnForecastInStackPanel(times[i], weathers[i], temperatures[i]);
                    break;
                case "05:00":
                    SpawnForecastInStackPanel("04:43", "voshod", "Восход", true);
                    SpawnForecastInStackPanel(times[i], weathers[i], temperatures[i]);
                    break;
                case "21:00":
                    SpawnForecastInStackPanel("20:24", "zakat", "Закат", true);
                    SpawnForecastInStackPanel(times[i], weathers[i], temperatures[i]);
                    break;
                default:
                    SpawnForecastInStackPanel(times[i], weathers[i], temperatures[i]);
                    break;
            }
            
        }
        
    }

    private void SpawnForecastInStackPanel(string time, string weather, string degrees, bool isBold=false)
    {
        var dockPanel = new DockPanel
        {
            VerticalSpacing = 16, 
            LastChildFill = true
        };

        var timeText = new TextBlock
        {
            Text = time, 
            Margin = new Thickness(0, 0, 0, 6)
        };
        DockPanel.SetDock(timeText, Dock.Top);
        timeText.Classes.Add("SmallText");

        var weatherIcon = new Image
        {
            Source = new Bitmap(AssetLoader.Open(new Uri($"avares://WeatherAppAvalonia/Assets/icons/{weather}.png"))), 
            Width = 60
        };
        DockPanel.SetDock(weatherIcon, Dock.Top);
        
        var tempText = new TextBlock
        {
            Text = degrees
        };
        DockPanel.SetDock(tempText, Dock.Bottom);
        tempText.Classes.Add("SmallText");
        if (isBold)
        {
            timeText.FontWeight = FontWeight.SemiBold;
            tempText.FontWeight = FontWeight.SemiBold;
        }
        dockPanel.Children.Add(timeText);
        dockPanel.Children.Add(weatherIcon);
        dockPanel.Children.Add(tempText);
        
        ForecastStackPanel.Children.Add(dockPanel);
    }
}