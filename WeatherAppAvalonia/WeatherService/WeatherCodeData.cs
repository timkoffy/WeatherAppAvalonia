namespace WeatherAppAvalonia.Assets;

public class WeatherCodeData
{
    public string code { get; set; }
    public Day day { get; set; }
    public Night night { get; set; }
}

public class Day
{
    public string text { get; set; }
    public string icon { get; set; }
}

public class Night
{
    public string text { get; set; }
    public string icon { get; set; }
}