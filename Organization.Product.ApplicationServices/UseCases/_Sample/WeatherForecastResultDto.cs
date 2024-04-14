using Organization.Product.Domain.Entities._Sample;

namespace Organization.Product.ApplicationServices.UseCases._Sample
{
    public class WeatherForecastResultDto
    {
        public IEnumerable<WeatherForecast>? WeatherForecasts { get; set; }
        public string? NullPropertySample { get; set; }
        public readonly int ReadonlyField = 1;
        public int ReadonlyProperty { get; } = 1;
    }
}
