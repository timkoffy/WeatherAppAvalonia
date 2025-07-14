using Newtonsoft.Json;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherData
{
    public Temperature temperature { get; set; }
    public Description description { get; set; }
    public Humidity humidity { get; set; }
    public Pressure pressure { get; set; }
}

public class Temperature
{
    public TempDetail air { get; set; }
    public TempDetail comfort { get; set; }
}

public class TempDetail
{
    [JsonProperty("C")]
    public float? Celsius { get; set; }
}

public class Description
{
    public string full { get; set; }
}

public class Humidity
{
    public int percent { get; set; }
}

public class Pressure
{
    public int mm_hg_atm { get; set; }
}
