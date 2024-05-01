using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api.Middleware.Swashbuckle
{
    class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        readonly IConfiguration _configuration;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration)
        {
            this._provider = provider;
            this._configuration = configuration;
        }

        public void Configure(SwaggerGenOptions options)
        {
            var cnf = this._configuration.GetSection("SwaggerDoc");
            foreach (var description in this._provider.ApiVersionDescriptions)
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
