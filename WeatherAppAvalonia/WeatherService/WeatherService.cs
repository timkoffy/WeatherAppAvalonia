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
        var url = $"{BaseUrl}forecast.json?key={ApiKey}&q={city}&days=2&aqi=no&alerts=no";
        var response = await client.GetStringAsync(url);
        var weather = JsonConvert.DeserializeObject<WeatherCurrentData>(response);
        return weather;
    }
    
    public async Task<string[]> GetConditionFromCode(string code, int isDay)
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

    public async Task<string[]> LoadWeatherAsync(string city)
    {
        var data = await GetCurrentWeatherFromIdAsync(city);
        var data1 = await GetConditionFromCode(data.current.condition.code, data.current.is_day);
        
        var forecastHours = weather.forecast.forecastday[0].hour;

        var hours = new List<string>();
        var codes = new List<string>();
        var temps = new List<string>();

        foreach (var h in forecastHours)
        {
            string hour = h.Time.Split(' ')[1];
            hours.Add(hour);
            
            codes.Add(h.condition.code);
            
            temps.Add($"+{Math.Round(h.TempC)}°");
        }

        
        WeatherInfo = new[]
        {
            data.current.formattedTempC,
            data1[0],
            data1[1]
        };
        return WeatherInfo;
    }
    
    public async Task<List<string>> GetAllCityNamesAsync(string cityResponse="Аткарск")
    {
        using var client = new HttpClient();
        var url = $"{BaseUrl}search.json?key={ApiKey}&q={cityResponse}";
        var response = await client.GetStringAsync(url);
        
        var cityList = JsonConvert.DeserializeObject<List<CitiSearchData>>(response);
        
        var nameList = cityList.Select(c => c.name).ToList();
        return nameList;
    }
    
    
}