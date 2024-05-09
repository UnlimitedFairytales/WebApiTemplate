using Microsoft.AspNetCore.Antiforgery;
using Organization.Product.Api._6_ActionFilters;
using Organization.Product.Shared.Configurations;

namespace Organization.Product.Api._1_Middleware.AntiCsrf
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        public static void MyAddAntiforgery(this IServiceCollection services, IConfiguration configuration)
        {
            var antiCsrfOption = new AntiCsrfOption(configuration);
            services.AddAntiforgery(options =>
            {
                options.HeaderName = antiCsrfOption.RequestTokenName;
                // options.Cookie.Name // .AspNetCore.Antiforgery.XXXXXXXXXXX
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = SameSiteMode.Strict;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });
            services.AddControllers(options =>
            {
                options.Filters.Add<AntiForgeryTokenGenerationFilter>();
            });
        }

        // 参考
        // https://source.dot.net/#Microsoft.AspNetCore.Antiforgery/AntiforgeryApplicationBuilderExtensions.cs,817738001e16edde
        // 詳細はCHANGED:を参照のこと
        private const string AntiforgeryMiddlewareSetKey = "__AntiforgeryMiddlewareSet";
        public static IApplicationBuilder MyUseAntiforgery(this IApplicationBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);
            builder.VerifyAntiforgeryServicesAreRegistered();

            builder.Properties[AntiforgeryMiddlewareSetKey] = true;
            // CHANGED: MyAntiforgeryMiddlewareに変更
            builder.UseMiddleware<MyAntiforgeryMiddleware>();

            return builder;
        }

        private static void VerifyAntiforgeryServicesAreRegistered(this IApplicationBuilder builder)
        {
            if (builder.ApplicationServices.GetService(typeof(IAntiforgery)) == null)
            {
                throw new InvalidOperationException("Unable to find the required services. Please add all the required services by calling 'IServiceCollection.AddAntiforgery' in the application startup code.");
            }
        }
    }
}
