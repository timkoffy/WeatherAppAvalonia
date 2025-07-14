using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReactiveUI;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService : ReactiveObject
{
    private const string ApiKey = "56b30cb255.3443075";
    public const string BaseUrl = "https://api.gismeteo.net/v2";
    
    private string _weatherInfo;
    public string WeatherInfo
    {
        get => _weatherInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
    }

    public async Task<WeatherData> GetWeatherFromIdAsync(string id)
    {
        using var client = new HttpClient();
        client.DefaultRequestHeaders.Add("X-Gismeteo-Token", ApiKey);

        var url = $"{BaseUrl}/weather/current/{id}/";
        var response = await client.GetStringAsync(url);
        var weather = JsonConvert.DeserializeObject<WeatherData>(response);
        return weather;
    }

    public async Task<string> LoadWeatherAsync(string id = "5016")
    {
        var data = await GetWeatherFromIdAsync(id);
        WeatherInfo = $"Температура: {data.temperature.air}°C\nОписание: {data.description.full}";
        return WeatherInfo;
    }
}