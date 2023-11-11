using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api.Middleware.Swashbuckle
{
#pragma warning disable IDE1006
    static class _Extension
#pragma warning restore IDE1006
    {
        // https://github.com/dotnet/aspnet-api-versioning
        // https://github.com/dotnet/aspnet-api-versioning/blob/release/6.0/examples/AspNetCore/WebApi/OpenApiExample/
        public static void MyAddTransient_AddSwaggerGen(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
            });
        }

        public static void MyUseSwaggerUI(this IApplicationBuilder app)
        {
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            if (provider == null) return;

            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"WebApi {description.GroupName}");
                }
            });
        }
    }
}
