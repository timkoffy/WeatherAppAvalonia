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
    public double temp_c { get; set; }
    
    public string last_updated { get; set; }
    
    public Condition condition { get; set; }
    
    public int is_day { get; set; }
    
    public double wind_kph { get; set; }
    
    public string wind_dir { get; set; }
    
    public int humidity { get; set; }
    
    public double feelslike_c { get; set; }
    
    public double pressure_mb { get; set; }
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
    public Astro astro { get; set; }
    public List<Hour> hour { get; set; }
}

public class Hour
{
    public string time { get; set; }

    public double temp_c { get; set; }

    public ConditionHour condition { get; set; }
    
    public int is_day { get; set; }
}

public class ConditionHour {
    public string code { get; set; }
}

public class Astro
{
    public string sunrise { get; set; }
    public string sunset { get; set; }
}