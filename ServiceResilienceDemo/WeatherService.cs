using Polly;
using Polly.Bulkhead;
using Polly.Registry;
using System.Net;
using System.Net.Http;

namespace ServiceResilienceDemo
{
    public class WeatherService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ResiliencePipelineProvider<string> _pipelineProvider;
        private readonly AsyncBulkheadPolicy<string> _bulkheadPolicy;


        public WeatherService(IHttpClientFactory httpClientFactory,
            ResiliencePipelineProvider<string> pipelineProvider)
        {
            _httpClientFactory = httpClientFactory;
            _pipelineProvider = pipelineProvider;
            // Define the Bulkhead policy
            _bulkheadPolicy = Policy.BulkheadAsync<string>(maxParallelization: 5, maxQueuingActions: 10);
        }

        public async Task<WeatherResponse?> GetCurrentWeatherAsync(string city)
        {
            string appId = "YOUR_API_KEY";
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid{appId}=&units=metric";
            var httpClient = _httpClientFactory.CreateClient();

            var pipeline = _pipelineProvider.GetPipeline("default");

            var weatherResponse = await pipeline.ExecuteAsync(
                async ct => await httpClient.GetAsync(url, ct));

            if (weatherResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await weatherResponse.Content.ReadFromJsonAsync<WeatherResponse>();
        }

        public async Task<string> GetWeatherAsync()
        {
            var pipeline = _pipelineProvider.GetPipeline("default");
            var httpClient = _httpClientFactory.CreateClient();
            var response = await pipeline.ExecuteAsync(async ct =>
                  //await httpClient.GetAsync("https://localhost:7187/weatherforecast/slow", ct));
                  //await httpClient.GetAsync("https://localhost:7187/weatherforecast/simulate-failure", ct));            
                    await httpClient.GetAsync("https://localhost:7187/weatherforecast/simulate-delay", ct));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            return await response.Content.ReadAsStringAsync();
        }

        public async Task<string> GetWeatherDataAsync(string location)
        {
            var url = $"https://api.openweathermap.org/data/2.5/weather?q={location}&appid=c710dfd1bc436b3cbbc236282176335a&units=metric";
            var httpClient = _httpClientFactory.CreateClient();
            // Use the Bulkhead policy to execute the HTTP request
            return await _bulkheadPolicy.ExecuteAsync(async () =>
            {
                // Replace with your actual API endpoint
                var response = await httpClient.GetAsync(url);
                return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : "Failed to fetch weather data.";
            });
        }
    }

}
