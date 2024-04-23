using Organization.Product.ApplicationServices.UseCases._Sample;
using Organization.Product.ApplicationServices.UseCases.Login;

namespace Organization.Product.Api
{
    public static class ProgramDIHelper
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddTransient<LoginUseCase>();

            // _Sample
            services.AddTransient<WeatherForecastUseCase>();
        }
    }
}
