using System.Dynamic;

namespace Expando.Services {
    public class WeatherForecastService {
        private static readonly string[] Summaries = [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate) {
            var rng = new Random();
            return Task.FromResult(Enumerable.Range(1, 20).Select(index => new WeatherForecast {
                Date = DateOnly.FromDateTime(startDate.AddDays(index)),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            }).ToArray());
        }

        public async Task<List<ExpandoObject>> GetForecastAsyncExpando(DateTime startDate) {
            var weatherForecasts = await GetForecastAsync(DateTime.Now);
            List<ExpandoObject> dynamicList = [];
            foreach (WeatherForecast weatherForecast in weatherForecasts) {
                dynamic dynamicWeatherForecast = new ExpandoObject();
                dynamicWeatherForecast.Id = Guid.NewGuid();
                dynamicWeatherForecast.Date = weatherForecast.Date;
                dynamicWeatherForecast.TemperatureC = weatherForecast.TemperatureC;
                dynamicWeatherForecast.Summary = weatherForecast.Summary;
                dynamicList.Add(dynamicWeatherForecast);
            }
            return dynamicList;
        }
    }
}