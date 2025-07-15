using System;
using System.Collections.Generic;
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
    
    public async Task<string[]> GetCode(string code, int isDay)
    {
        string jsonString = await File.ReadAllTextAsync("WeatherService/WeatherCodes.json");
        var codeData = JsonConvert.DeserializeObject<Dictionary<string, WeatherCodeData>>(jsonString);

        var entry = codeData[code];
        
        switch (isDay)  
        {
            case 1: 
                return [entry.day.text, entry.day.icon]; 
            case 0: 
                return [entry.night.text, entry.night.icon];
            default: return null;
        }
    }

    public async Task<string[]> LoadWeatherAsync()
    {
        var data = await GetCurrentWeatherFromIdAsync();
        var data1 = await GetCode(data.current.condition.code, data.current.is_day);
        
        WeatherInfo = new[]
        {
            data.current.formattedTempC,
            data1[0],
            data1[1]
        };
        return WeatherInfo;
    }
}