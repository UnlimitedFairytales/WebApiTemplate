using Organization.Product.ApplicationServices.UseCases._Sample;

namespace Organization.Product.Api
{
    public static class ProgramDIHelper
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<WeatherForecastUseCase>();
        }
    }
}
