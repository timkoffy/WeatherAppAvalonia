using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace WeatherAppAvalonia.Views;

public partial class MainView : UserControl
{
    public MainView()
    {
        InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        SpawnForecastInStackPanel("07:00", "e", "e");
    }

    private void SpawnForecastInStackPanel(string time, string weather, string degrees)
    {
        var dockPanel = new DockPanel
        {
            VerticalSpacing = 16, LastChildFill = true
        };

        var timeText = new TextBlock
        {
            Text = time, Margin = new Thickness(0, 0, 0, 6)
        };
        DockPanel.SetDock(timeText, Dock.Top);
        timeText.Classes.Add("SmallText");
        
        dockPanel.Children.Add(timeText);
        
        ForecastStackPanel.Children.Add(dockPanel);
    }
}