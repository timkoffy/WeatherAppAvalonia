using System.Threading.Tasks;
using ReactiveUI;

namespace WeatherAppAvalonia.ViewModels;

public partial class MainViewModel : ReactiveObject

{
    private string _weatherInfo;
    public string WeatherInfo
    {
        get => _weatherInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherInfo, value);
    }

    public async Task LoadWeatherAsync()
    {
        var service = new WeatherService.WeatherService();
        var data = await service.GetWeatherAsync("Moscow");

        WeatherInfo = $"{data.temperature.comfort}" +
                      $"" +
                      $"";
    }
}