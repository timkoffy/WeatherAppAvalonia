using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherService
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
}