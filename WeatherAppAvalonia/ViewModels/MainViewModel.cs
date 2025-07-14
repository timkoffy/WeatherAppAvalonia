using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Threading;
using ReactiveUI;

namespace WeatherAppAvalonia.ViewModels;

public partial class MainViewModel : ReactiveObject

{
    private string[] _weatherCurInfo;
    public string[] WeatherCurInfo
    {
        get => _weatherCurInfo;
        set => this.RaiseAndSetIfChanged(ref _weatherCurInfo, value);
    }
    
    public ReactiveCommand<Unit, Unit> LoadWeatherCommand { get; }
    public MainViewModel()
    {
        LoadWeatherCommand = ReactiveCommand.CreateFromTask(LoadWeatherAsync);
    }

    public async Task LoadWeatherAsync()
    {
        var service = new WeatherService.WeatherService();
        var data = await service.GetWeatherAsync("4368");

        await Dispatcher.UIThread.InvokeAsync(() =>
        {
            WeatherCurInfo =
            [
                data.temperature.air.Celsius + "°",
                data.description.full
            ];
        });
    }
}