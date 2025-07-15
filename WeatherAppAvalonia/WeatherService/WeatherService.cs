using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using WeatherAppAvalonia.Assets;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService : ReactiveObject
{
    private string[] _weatherInfo;
    public string[] WeatherInfo
    {
        get => _weatherInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
    }

    public async Task<WeatherCurrentData> GetCurrentWeatherFromIdAsync()
    {
        using var client = new HttpClient();
        var url = "http://api.weatherapi.com/v1/current.json?key=665604c20d5e4084a4c184203250707&q=Saratov&aqi=yes";
        var response = await client.GetStringAsync(url);
        var weather = JsonConvert.DeserializeObject<WeatherCurrentData>(response);
        return weather;
    }
    
    public async Task<string[]> GetCode(string code, bool isDay)
    {
        string filePath = "WeatherCodes.json";
        string jsonString = File.ReadAllText(filePath);
        var codeData = JsonConvert.DeserializeObject<WeatherCodeData>(jsonString);
        //обработка кода
        return codeData.code;
    }

    public async Task<string[]> LoadWeatherAsync()
    {
        var data = await GetCurrentWeatherFromIdAsync();
        var data1 = await GetCode(data.current.condition.code, );
        
        WeatherInfo = new[]
        {
            data.current.formattedTempC,
            data
        };
        return WeatherInfo;
    }
}