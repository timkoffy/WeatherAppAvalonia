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

        var result = new List<string[]>
        {
            new[]
            {
                entry.night.text, entry.night.icon
            },
            new[]
            {
                entry.day.text, entry.day.icon
            }
        };
        return result[isDay];
    }

    public async Task<List<List<string>>> LoadWeatherAsync(string city)
    {
        var data = await GetCurrentWeatherFromIdAsync(city);
        var data1 = await GetConditionFromCode(data.current.condition.code, data.current.is_day);

        int currentTime = int.Parse(data.current.last_updated.Split(" ")[1].Split(":")[0]);
        int start = Math.Max(0, currentTime-2);
        
        string sunriseTime = GetActualTime("sunrise", data, currentTime);
        string sunsetTime = GetActualTime("sunset", data, currentTime);
        
        var forecastHours = data.forecast.forecastday[0].hour;
        forecastHours.AddRange(data.forecast.forecastday[1].hour);
        forecastHours = forecastHours.GetRange(start, 21);
        
        var hours = new List<string>();
        var conditionIcons = new List<string>();
        var temps = new List<string>();

        foreach (var h in forecastHours)
        {
            string hour = h.time.Split(' ')[1];
            hours.Add(hour);
            
            string[] condition = await GetConditionFromCode(h.condition.code, h.is_day);
            conditionIcons.Add(condition[1]);
            
            temps.Add($"+{Math.Round(h.temp_c)}°");
        }
        
        var weatherInfo = new List<string>
        {
            $"+{Math.Round(data.current.temp_c)}°",
            data1[0],
            data1[1]
        };

        var events = new List<string>
        {
            $"{currentTime}:00",
            $"{sunriseTime}",
            $"{sunsetTime}"
        };

        var result = new List<List<string>>
        {
            weatherInfo,
            hours,
            conditionIcons,
            temps,
            events
        };
        return result;
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

    private string Time12ToTime24(string time)
    {
        string[] parts = time.Split(' ');
        string[] hm = parts[0].Split(':');
        int hour = int.Parse(hm[0]);
        string minute = hm[1];
        string ampm = parts[1];

        if (ampm == "AM")
        {
            if (hour == 12) hour = 0;
        }
        else if (ampm == "PM")
        {
            if (hour != 12) hour += 12;
        }

        return $"{hour:D2}:{minute}";
    }

    private string GetActualTime(string type, WeatherCurrentData data, int currentTime)
    {
        string time = type == "sunrise"
            ? data.forecast.forecastday[0].astro.sunrise
            : data.forecast.forecastday[0].astro.sunset;
        time = Time12ToTime24(time);
        
        if (int.Parse(time.Split(":")[0]) < currentTime-2)
        {
            time = type == "sunrise"
                ? data.forecast.forecastday[1].astro.sunrise
                : data.forecast.forecastday[1].astro.sunset;
            time = Time12ToTime24(time);
        }
        return time;
    }
    
}