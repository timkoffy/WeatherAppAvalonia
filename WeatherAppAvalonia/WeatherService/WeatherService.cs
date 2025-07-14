using System;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using Newtonsoft.Json;
using ReactiveUI;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService : ReactiveObject
{
    private const string ApiKey = "56b30cb255.3443075";
    public const string BaseUrl = "https://api.gismeteo.net/v2";
    public async Task<WeatherData> GetWeatherFromIdAsync(string id)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Gismeteo-Token", ApiKey);
        
        var url = $"{BaseUrl}/weather/current/{id}/";
        var response = await client.GetStringAsync(url);
            
        var weather = JsonConvert.DeserializeObject<WeatherData>(response);
        return weather;
    }
    
    private string _weatherInfo;
    public string WeatherInfo
    {
        get
        {
            return _weatherInfo;
        }
        set
        {
            this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
        }
    }

    public async Task<string> LoadWeatherAsync()
    {
        this.FindControl<Button>("RunButton").IsEnabled = false;
        this.FindControl<TextBlock>("ResultText").Text = "I'm working ...";
        
        var data = await GetWeatherFromIdAsync("5016");
        WeatherInfo = $"Температура: {data.temperature.air}°C\n" + "Описание: {data.description.full}";
        return WeatherInfo;
    }

    public async void LoadWeatherCommand(object sender, RoutedEventArgs e)
    { 
        var result = await Dispatcher.UIThread.InvokeAsync(LoadWeatherAsync, DispatcherPriority.Background);
        Console.WriteLine(result);
        this.FindControl<TextBlock>("ResultText").Text = result;
        this.FindControl<Button>("RunButton").IsEnabled = true;
    }
    

    // {
    //     var result = await Dispatcher.UIThread.InvokeAsync(LoadWeatherAsync, DispatcherPriority.Background);
    //     
    //     this.FindControl<TextBlock>("TestText").Text = result;
    //     this.FindControl<Button>("TestButton").IsEnabled = true;
    // }}
}