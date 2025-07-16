using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace WeatherAppAvalonia.WeatherService;

public class WeatherCurrentData
{
    public Current current { get; set; }
    
    public Forecast forecast { get; set; }
}

public class Current
{
    [JsonProperty("temp_c")]
    public double tempC { get; set; }
    [JsonIgnore]
    public string formattedTempC => $"+{Math.Round(tempC)}°";
    
    [JsonProperty("last_updated")]
    public string LastUpdated { get; set; }
    [JsonIgnore] public string formattedLastUpdated => LastUpdated.Split(" ")[1];
    
    public Condition condition { get; set; }
    
    public int is_day {get; set;}
}

public class Condition
{
    public string code { get; set; }
}

public class Forecast
{
    public List<ForecastDay> forecastday { get; set; }
}

public class ForecastDay
{
    // public Astro astro { get; set; }
    public List<Hour> hour { get; set; }
}

public class Hour
{
    [JsonProperty("time")]
    public string Time { get; set; }

    [JsonProperty("temp_c")]
    public double TempC { get; set; }

    [JsonIgnore]
    public string FormattedTemp => $"+{Math.Round(TempC)}°";

    public ConditionHour condition { get; set; }
}

public class ConditionHour {
    public string code { get; set; }
}