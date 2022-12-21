using System;
using WebApi.Attributes;

namespace WebApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        [DisplayApi("ADMIN")]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
