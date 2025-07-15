using System.Collections.ObjectModel;
using System.Reactive;
using System.Threading.Tasks;
using ReactiveUI;
using WeatherAppAvalonia;

namespace WeatherAppAvalonia.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private string _searchText;
    private string _finalCityName;

    private readonly WeatherService.WeatherService _weatherService = new();
    
    public string SearchText
    {
        get => _searchText;
        set
        {
            this.RaiseAndSetIfChanged(ref _searchText, value);
            _ = OnSearchTextChangedAsync(value);
        }
    }
    public string FinalCityName
    {
        get => _finalCityName;
        set => this.RaiseAndSetIfChanged(ref _finalCityName, value);
    }
    
    public ObservableCollection<string> Suggestions { get; } = new();

    private async Task OnSearchTextChangedAsync(string response)
    {
        if (string.IsNullOrWhiteSpace(response) || response.Length < 2)
            return;

        var result = await _weatherService.GetAllCityNamesAsync(response);

        Suggestions.Clear();
        foreach (var item in result)
            Suggestions.Add(item);
    }
    
}
