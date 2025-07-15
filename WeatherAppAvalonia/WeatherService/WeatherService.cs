using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;
using WeatherAppAvalonia.Assets;
using WeatherAppAvalonia.CityData;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService : ReactiveObject
{
    private const string BaseUrl = "http://api.weatherapi.com/v1/";
    private const string ApiKey = "665604c20d5e4084a4c184203250707";
    
    
    private string[] _weatherInfo;
    public string[] WeatherInfo
    {
        get => _weatherInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
    }

    public async Task<WeatherCurrentData> GetCurrentWeatherFromIdAsync(string city)
    {
        using var client = new HttpClient();
        var url = $"{BaseUrl}/current.json?key={ApiKey}&q={city}&aqi=yes";
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
        var city = "Аткарск";
        var data = await GetCurrentWeatherFromIdAsync(city);
        var data1 = await GetCode(data.current.condition.code, data.current.is_day);
        
        WeatherInfo = new[]
        {
            data.current.formattedTempC,
            data1[0],
            data1[1]
        };
        return WeatherInfo;
    }
    
    public async Task<Dictionary<string, string>> GetAllCityNamesAsync(string city)
    {
        using var client = new HttpClient();
        var url = $"{BaseUrl}search.json?key={ApiKey}&q={city}";
        var response = await client.GetStringAsync(url);
        
        var cityList = JsonConvert.DeserializeObject<List<CitiSearchData>>(response);

        var nameCountryList = cityList.ToDictionary(c => c.name, c => c.country);
        return nameCountryList;
    }
    
    
}