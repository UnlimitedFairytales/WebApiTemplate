﻿using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Product.Api._1_Middleware.Auth
{
    public interface IAuthMethods
    {
        void AddAuthentication(IServiceCollection services, IConfiguration configuration);
        void AddSecurityDefinition_AddSecurityRequirement(SwaggerGenOptions options);
        void UseAuthentication(WebApplication app, IConfiguration configuration);
    }
}
