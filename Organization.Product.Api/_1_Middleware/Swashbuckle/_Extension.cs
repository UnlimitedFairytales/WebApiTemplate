using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Organization.Product.Api._1_Middleware.Auth;
using Organization.Product.Shared.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api._1_Middleware.Swashbuckle
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        // https://github.com/dotnet/aspnet-api-versioning
        // https://github.com/dotnet/aspnet-api-versioning/blob/release/6.0/examples/AspNetCore/WebApi/OpenApiExample/
        public static void MyAddTransient_AddSwaggerGen(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();
                options.MyAddSecurityDefinition_AddSecurityRequirement(configuration);
            });
        }

        public static void MyUseSwaggerUI(this IApplicationBuilder app, IConfiguration configuration)
        {
            var provider = app.ApplicationServices.GetService<IApiVersionDescriptionProvider>();
            if (provider == null) return;

            var antiCsrfOption = new AntiCsrfOption(configuration);
            app.UseSwaggerUI(options =>
            {
                var jsText = "";
                jsText += @"(req) => {";
                jsText += @"    const splitted = document.cookie.split(';');";
                jsText += @"    if (splitted != null && splitted != undefined) {";
                jsText += @"        for (const kvp of splitted) {";
                jsText += @"            const trimmed = kvp.trim();";
                jsText += $"            if (trimmed.startsWith('{antiCsrfOption.RequestTokenName}')) {{";
                jsText += @"                console.log(trimmed);";
                jsText += @"                const tokenValue = trimmed.split('=')[1].trim();";
                jsText += $"                req.headers['{antiCsrfOption.RequestTokenName}'] = tokenValue;";
                jsText += @"            }";
                jsText += @"        }";
                jsText += @"    }";
                jsText += @"    return req;";
                jsText += @"}";
                options.UseRequestInterceptor(jsText);
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"WebApi {description.GroupName}");
                }
            });
        }
    }
}
