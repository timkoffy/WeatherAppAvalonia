using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;

namespace WeatherAppAvalonia.ViewModels;

public partial class MainWindowViewModel : ReactiveObject
{
    // private string _weatherInfo;
    // public string WeatherInfo
    // {
    //     get
    //     {
    //         return _weatherInfo;
    //     }
    //     set
    //     {
    //         this.RaiseAndSetIfChanged(ref _weatherInfo, value); 
    //     }
    // }
    //
    // public async Task LoadWeatherAsync()
    // {
    //     var service = new WeatherService.WeatherService();
    //     var data = await service.GetWeatherFromIdAsync("5016");
    //
    //     WeatherInfo = $"Температура: {data.temperature.air}°C\n" + "Описание: {data.description.full}";
    // }
    
    // public ReactiveCommand<Unit, Unit> LoadWeatherCommand { get; }
    //
    // public MainViewModel()
    // {
    //     LoadWeatherCommand = ReactiveCommand.CreateFromTask(LoadWeatherAsync);
    // }
}