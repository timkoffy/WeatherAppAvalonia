using System.Collections.Generic;

namespace WeatherAppAvalonia.WeatherService;

public class CitySearchResponse
{
    public List<City> Response { get; set; }
}

public class City
{
    public int Id { get; set; }
    public string Name { get; set; }
    public Country Country { get; set; }
}

public class Country
{
    public string Name { get; set; }
}
