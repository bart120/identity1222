using IdentityModel.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ConsoleAppDemo
{
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute:Attribute
    {
        public void OnBeforeExecute()
        {
            Console.WriteLine("OnBeforeExecute says.");
        }

    }
    internal class Program
    {
        [Test()]
        public void test(string name)
        {

        }

        static async Task Main(string[] args)
        {
            var p = new Program();
            typeof(Program)
            .GetMethod(nameof(p.test))
            .GetCustomAttributes(true)
            .OfType<TestAttribute>()
            .First()
            .OnBeforeExecute();
            p.test("bob");
               
            /*using(var client = new HttpClient())
            {
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:44361/");

                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,
                    ClientId = "client_console",
                    ClientSecret = "secret_console",
                    Scope = "api_demo_scope"
                });

                if (tokenResponse.IsError)
                {
                    Console.WriteLine(tokenResponse.Error);
                }

                Console.WriteLine(tokenResponse.Json);

                using(var apiClient = new HttpClient())
                {
                    apiClient.SetBearerToken(tokenResponse.AccessToken);
                    var response = await apiClient.GetAsync("https://localhost:44362/WeatherForecast");
                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                    }
                    else
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        Console.WriteLine(content);
                    }
                }
                Console.ReadLine();
            }*/

        }
    }
}
