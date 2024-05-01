using Organization.Product.Domain._Sample.Entities;

namespace Organization.Product.ApplicationServices.UseCases._Sample
{
    public class WeatherForecastResultDto : BaseResultDto
    {
        public IEnumerable<WeatherForecast>? WeatherForecasts { get; set; }
        public string? NullPropertySample { get; set; }
        public readonly int ReadonlyField = 1;
        public int ReadonlyProperty { get; } = 1;
    }
}
