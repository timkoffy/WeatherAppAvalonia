using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Data.Converters;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using WeatherAppAvalonia.ViewModels;


namespace WeatherAppAvalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }
    
    private async void OnLoaded(object sender, RoutedEventArgs e)
    {
        await BuildMainUIWeatherTask();
    }

    public async Task BuildMainUIWeatherTask(string city = "Саратов")
    {
        var _weatherService = new WeatherService.WeatherService();
        var weather = await _weatherService.LoadWeatherAsync(city);
        
        var hours = weather[1];
        var conditionIcons = weather[2];
        var temps = weather[3];
        var events = weather[4];
        string nowHour = events[0];
        string sunriseTime = events[1];
        string sunsetTime = events[2];

        CurTempText.Text = weather[0][0];
        CurConditionText.Text = weather[0][1];
        string uri = $"avares://WeatherAppAvalonia/Assets/icons/{weather[0][2]}.png";
        CurConditionIcon.Source = new Bitmap(AssetLoader.Open(new Uri(uri)));
        LocationText.Text = weather[0][7];
        
        BuildMainForecast(hours, conditionIcons, temps, nowHour, sunriseTime, sunsetTime);
        
        BuildAdditionalWeatherInfoToday(weather[0][3], weather[0][4], weather[0][5], weather[0][6]);
    }

    private void CitySearchBox_Result(object sender, SelectionChangedEventArgs e)
    {
        if (CitySearchBox.SelectedItem is string selectedCity)
        {
            var viewModel = DataContext as MainWindowViewModel;
            viewModel.FinalCityName = selectedCity;
            
            BuildMainUIWeatherTask(selectedCity);
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
    
    private void BuildMainForecast(List<string> hours, List<string> conditionIcons, List<string> temps, string nowHour,
        string sunriseTime, string sunsetTime)
    {
        ForecastStackPanel.Children.Clear();

        for (int i = 0; i < hours.Count; i++)
        {
            if (i != 0 && hours[i - 1] == sunriseTime && sunriseTime == sunsetTime)
            {
                SpawnForecastInStackPanel(sunriseTime, "voshod", "Полярный день", true);
                continue;
            }

            if (hours[i] == nowHour)
            {
                SpawnForecastInStackPanel("Сейчас", conditionIcons[i], temps[i], true);
                continue;
            }

            if (hours[i] == "00:00")
            {
                if (i != 0)
                {
                    var rectangle = new Rectangle
                    {
                        Width = 3,
                        Height = 130,
                        RadiusX = 1.5,
                        RadiusY = 1.5,
                        Fill = SolidColorBrush.Parse("#5060728B")
                    };
                    ForecastStackPanel.Children.Add(rectangle);
                }

                SpawnForecastInStackPanel(hours[i], conditionIcons[i], temps[i]);
                continue;
            }

            if (i != 0 && hours[i - 1].Split(':')[0] == sunriseTime.Split(':')[0])
            {
                SpawnForecastInStackPanel(sunriseTime, "voshod", "Восход", true);
                SpawnForecastInStackPanel(hours[i], conditionIcons[i], temps[i]);
                continue;
            }

            if (i != 0 && hours[i - 1].Split(':')[0] == sunsetTime.Split(':')[0])
            {
                SpawnForecastInStackPanel(sunsetTime, "zakat", "Закат", true);
                SpawnForecastInStackPanel(hours[i], conditionIcons[i], temps[i]);
            }
            else
            {
                SpawnForecastInStackPanel(hours[i], conditionIcons[i], temps[i]);
            }
        }
    }

    private void BuildAdditionalWeatherInfoToday(string feelslike, string humidity, string wind, string pressure)
    {
        FeelsLikeTempText.Text = feelslike;
        HumidityText.Text = humidity;
        WindText.Text = wind;
        PressureText.Text = pressure;
    }
}