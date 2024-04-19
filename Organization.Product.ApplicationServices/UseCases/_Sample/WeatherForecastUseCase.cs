using Organization.Product.Domain.Entities._Sample;
using Organization.Product.Domain.ValueObjects;

namespace Organization.Product.ApplicationServices.UseCases._Sample
{
    [Aop.Log]
    public class WeatherForecastUseCase
    {
        public WeatherForecastResultDto GetWeatherForecastForNext5Days()
        {
            var data = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = WeatherForecast.Summaries[Random.Shared.Next(WeatherForecast.Summaries.Length)]
            })
            .ToArray();
            return new WeatherForecastResultDto() { WeatherForecasts = data };
        }

        public WeatherForecastResultDto Parrot(WeatherForecastRequestDto requestDto, bool isPost)
        {
            var text = isPost ? "Post" : "Get";
            var data = new WeatherForecast[]
            {
                new WeatherForecast()
                {
                    Date = DateTime.Now,
                    TemperatureC = requestDto.P2_Int,
                    Summary = $"{text} {requestDto.P1_Bool} <>&'\"+____\\`____あ {requestDto.P3_String}",
                    Double = requestDto.P4_Double
                }
            };
            return new WeatherForecastResultDto() { WeatherForecasts = data };
        }

        public WeatherForecastResultDto ThrowException()
        {
            throw new Exception("BOOOO!!");
        }

        public WeatherForecastResultDto ThrowAppException()
        {
            var param = new[] { "TextParam0" };
            var error = AppMessage.W3003(param);
            throw AppException.Create(error);
        }
    }
}
