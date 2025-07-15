namespace WeatherAppAvalonia.Assets;

public class WeatherCodeData
{
    public WeatherTimeData day { get; set; }
    public WeatherTimeData night { get; set; }
}

public class WeatherTimeData
{
    public string text { get; set; }
    public string icon { get; set; }
}