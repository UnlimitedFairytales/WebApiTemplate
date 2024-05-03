using Asp.Versioning;

namespace Organization.Product.Api.Middleware.ApiExplorer
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        // https://github.com/dotnet/aspnet-api-versioning
        // https://github.com/dotnet/aspnet-api-versioning/blob/release/6.0/examples/AspNetCore/WebApi/OpenApiExample/
        // https://github.com/dotnet/aspnet-api-versioning/wiki/FAQ
        // https://github.com/dotnet/aspnet-api-versioning/wiki/Known-Limitations#url-path-segment-routing-with-a-default-api-version
        // https://github.com/dotnet/aspnet-api-versioning/wiki/API-Version-Reader
        // https://github.com/dotnet/aspnet-api-versioning/wiki/Version-Format#custom-api-version-format-strings
        public static void MyAddApiVersioning_AddVersionedApiExplorer(this IServiceCollection services, IConfiguration configuration)
        {
            var cnf = configuration.GetSection("ApiVersioning");
            var parameterName = cnf["ApiVersionReaderParameterName"];
            IApiVersionReader apiVersionReader =
                cnf["ApiVersionReader"] == "QueryStringApiVersionReader" ? new QueryStringApiVersionReader(parameterName) :
                cnf["ApiVersionReader"] == "HeaderApiVersionReader" ? new HeaderApiVersionReader(parameterName) :
                cnf["ApiVersionReader"] == "MediaTypeApiVersionReader" ? new MediaTypeApiVersionReader(parameterName) :
                new UrlSegmentApiVersionReader();

            var builder = services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = cnf.GetValue<bool>("ReportApiVersions");
                options.AssumeDefaultVersionWhenUnspecified = cnf.GetValue<bool>("AssumeDefaultVersionWhenUnspecified");
                options.DefaultApiVersion = new ApiVersion(
                    cnf.GetValue<int>("DefaultApiVersion_Major"),
                    cnf.GetValue<int>("DefaultApiVersion_Minor"));
                options.ApiVersionReader = apiVersionReader;
            });
            builder.AddApiExplorer(options =>
            {
                options.GroupNameFormat = cnf["GroupNameFormat"];
                options.SubstituteApiVersionInUrl = apiVersionReader is UrlSegmentApiVersionReader; // RouteAttribute.Templateで{version:apiVersion}を有効にするか;
            });
        }
    }
}
