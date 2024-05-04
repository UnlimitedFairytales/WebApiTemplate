using Microsoft.AspNetCore.Cors.Infrastructure;
using Organization.Product.Api.Utils.Extensions;

namespace Organization.Product.Api.Middleware.CorsPolicy
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        public static void MyAddCorsPolicies(this CorsOptions options, IConfiguration configuration)
        {
            var cnfs = configuration.GetValueArray<IConfigurationSection>("CorsPolicies");
            if (cnfs == null || cnfs.Length == 0) return;

            foreach (var cnf in cnfs)
            {
                if (cnf == null || !cnf.Exists()) continue;
                options.AddPolicy(cnf["Name"], builder =>
                {
                    builder.WithOrigins(cnf.GetValueArray<string>("Origins"))
                    .WithMethods(cnf.GetValueArray<string>("Methods"))
                    .WithHeaders(cnf.GetValueArray<string>("Headers"))
                    .AllowCredentials();
                });
            }
        }

        public static void MyUseCorsPolicies(this IApplicationBuilder app, IConfiguration configuration)
        {
            var cnfs = configuration.GetValueArray<IConfigurationSection>("CorsPolicies");
            if (cnfs == null || cnfs.Length == 0) return;

            foreach (var cnf in cnfs)
            {
                if (cnf == null || !cnf.Exists()) continue;
                app.UseCors(cnf["Name"]);
            }
        }
    }
}
