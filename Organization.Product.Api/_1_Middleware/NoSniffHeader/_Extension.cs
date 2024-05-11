namespace Organization.Product.Api._1_Middleware.NoSniffHeader
{
#pragma warning disable IDE1006
    public static class _Extension
#pragma warning restore IDE1006
    {
        public static void UseNoSniffHeader(this IApplicationBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                context.Response.Headers.Append("X-Content-Type-Options", "nosniff");
                await next();
            });
        }
    }
}
