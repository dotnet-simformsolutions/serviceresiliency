using Microsoft.AspNetCore.Mvc;
using Polly.Registry;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace ServiceResilienceDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _weatherService;

        public WeatherController(WeatherService weatherService)
        {
            _weatherService = weatherService;
        }
        
        [HttpGet("slow")]
        public async Task<IActionResult> GetSlowAsync()
        {
            await Task.Delay(TimeSpan.FromSeconds(10)); 
            return Ok("Slow response");
        }
        
        [HttpGet("GetAsync")]
        public async Task<IActionResult> GetAsync()
        {
            try
            {
                var result = await _weatherService.GetWeatherAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        
        [HttpGet("GetCurrentWeatherAsync{city}")]
        public async Task<IActionResult> GetCurrentWeatherAsync(string city)
        {
            try
            {
                var result = await _weatherService.GetCurrentWeatherAsync(city);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("simulate-failure")]
        public async Task<IActionResult> SimulateFailureAsync()
        {         
            if (new Random().NextDouble() < 0.7)
            {
                throw new Exception("Simulated failure");
            }

            return Ok("Success");
        }
                
        [HttpGet("simulate-delay")]
        public async Task<IActionResult> SimulateDelayAsync()
        {            
            await Task.Delay(TimeSpan.FromSeconds(5));
            return Ok("Success");
        }

        [HttpGet("GetWeather")]
        public async Task<IActionResult> GetWeather(string location)
        {
            var result = await _weatherService.GetWeatherDataAsync(location);
            return Ok(result);
        }
    }
}