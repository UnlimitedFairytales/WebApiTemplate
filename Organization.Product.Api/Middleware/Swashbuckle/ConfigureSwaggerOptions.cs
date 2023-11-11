using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api.Middleware.Swashbuckle
{
    class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider provider;
        readonly IConfiguration configuration;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            this.provider = provider;
            this.configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var cnf = configuration.GetSection("SwaggerDoc");
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    new OpenApiInfo()
                    {
                        Title = cnf["Title"],
                        Version = description.ApiVersion.ToString(),
                        Description = cnf["Description"],
                        Contact = new OpenApiContact { Name = cnf["ContactName"], Email = cnf["ContactEmail"] },
                        License = new OpenApiLicense { Name = cnf["LicenseName"], Url = new Uri(cnf["LicenseUrl"]) },
                    });
            }
        }
    }
}
