using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService : ReactiveObject
{
    private string[] _weatherInfo;
    public string[] WeatherInfo
    {
        get => _weatherInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
    }

    public async Task<WeatherCurrentData> GetWeatherFromIdAsync()
    {
        using var client = new HttpClient();
        var url = $"http://api.weatherapi.com/v1/current.json?key=665604c20d5e4084a4c184203250707&q=Saratov&aqi=yes";
        var response = await client.GetStringAsync(url);
        var weather = JsonConvert.DeserializeObject<WeatherCurrentData>(response);
        return weather;
    }

    public async Task<string[]> LoadWeatherAsync()
    {
        var data = await GetWeatherFromIdAsync();
        WeatherInfo = new[]
        {
            data.current.formattedTempC,
            data.current.condition.text
        };
        return WeatherInfo;
    }
}