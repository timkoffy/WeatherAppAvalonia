using System;
using Newtonsoft.Json;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherCurrentData
{
    public Location location { get; set; }
    public Current current { get; set; }
}

public class Location
{
    public string name { get; set; }
    public string region { get; set; }
    public string country { get; set; }
    public string localtime { get; set; }
}

public class Current
{
    [JsonProperty("temp_c")]
    public double tempC { get; set; }
    [JsonIgnore]
    public string formattedTempC => $"+{Math.Round(tempC)}°";
    
    public Condition condition { get; set; }
    
    public int is_day {get; set;}
}

public class Condition
{
    public string code { get; set; }
}