using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MVCClient.Service
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient http;

        public WeatherService(HttpClient httpClient)
        {
            this.http = httpClient;
        }

        public async Task<IEnumerable<WeatherForecast>> GetWeathers()
        {
            var url = @"https://localhost:44362/WeatherForecast";
            var responseString = await http.GetStringAsync(url);
            var result = JsonSerializer.Deserialize<List<WeatherForecast>>(responseString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            return result;
        }
    }
}
