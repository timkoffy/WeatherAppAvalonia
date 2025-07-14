using System.Reactive;
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
    
    // public ReactiveCommand<Unit, Unit> LoadWeatherCommand { get; }
    //
    // public MainViewModel()
    // {
    //     LoadWeatherCommand = ReactiveCommand.CreateFromTask(LoadWeatherAsync);
    // }

    public async Task LoadWeatherAsync()
    {
        var service = new WeatherService.WeatherService();
        var data = await service.GetWeatherFromIdAsync("5016");

        WeatherInfo = $"Температура: {data.temperature.air}°C\n" +
                      $"Описание: {data.description.full}";
    }
}