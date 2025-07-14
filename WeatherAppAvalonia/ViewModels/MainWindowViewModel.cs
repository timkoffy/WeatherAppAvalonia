using System.Reactive;
using ReactiveUI;

namespace WeatherAppAvalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private readonly WeatherService.WeatherService _weatherService;

    private string _result;
    public string Result
    {
        get => _result;
        set => this.RaiseAndSetIfChanged(ref _result, value);
    }

    public ReactiveCommand<Unit, Unit> LoadWeatherCommand { get; }

    public MainWindowViewModel()
    {
        _weatherService = new WeatherService.WeatherService();

        LoadWeatherCommand = ReactiveCommand.CreateFromTask(async () =>
        {
            Result = "Loading...";
            var weather = await _weatherService.LoadWeatherAsync();
            Result = weather;
        });
    }
}