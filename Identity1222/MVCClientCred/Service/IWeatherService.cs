using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MVCClientCred.Service
{
    public interface IWeatherService
    {
        Task<IEnumerable<WeatherForecast>> GetWeathers();
    }

    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}
