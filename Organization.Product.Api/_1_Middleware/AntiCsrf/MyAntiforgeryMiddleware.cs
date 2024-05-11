using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http.Features;

#pragma warning disable IDE0009 // メンバー アクセスを修飾する必要があります。
#pragma warning disable IDE0090 // 'new(...)' を使用する
namespace Organization.Product.Api._1_Middleware.AntiCsrf
{
    // 参考
    // https://source.dot.net/#Microsoft.AspNetCore.Antiforgery/AntiforgeryMiddleware.cs,ca607666f5a0089b,references
    // 詳細はCHANGED:を参照のこと
    public class MyAntiforgeryMiddleware(IAntiforgery antiforgery, RequestDelegate next, ILogger<MyAntiforgeryMiddleware> logger)
    {
        private readonly RequestDelegate _next = next;
        private readonly IAntiforgery _antiforgery = antiforgery;
        // CHANGED logger追加
        private readonly ILogger<MyAntiforgeryMiddleware> _logger = logger;

        private const string AntiforgeryMiddlewareWithEndpointInvokedKey = "__AntiforgeryMiddlewareWithEndpointInvoked";
        private static readonly object AntiforgeryMiddlewareWithEndpointInvokedValue = new object();

        public Task Invoke(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint is not null)
            {
                context.Items[AntiforgeryMiddlewareWithEndpointInvokedKey] = AntiforgeryMiddlewareWithEndpointInvokedValue;
            }

            var method = context.Request.Method;
            if (!HttpExtensions.IsValidHttpMethodForForm(method))
            {
                return _next(context);
            }

            // CHANGED: nullの場合もチェックするように
            var antiforgeryMetadata = endpoint?.Metadata.GetMetadata<IAntiforgeryMetadata>();
            if (antiforgeryMetadata == null || antiforgeryMetadata.RequiresValidation)
            {
                return InvokeAwaited(context);
            }

            return _next(context);
        }

        public async Task InvokeAwaited(HttpContext context)
        {
            try
            {
                await _antiforgery.ValidateRequestAsync(context);
                context.Features.Set(AntiforgeryValidationFeature.Valid);
            }
            catch (AntiforgeryValidationException e)
            {
                // CHANGED: LogWarning追加
#pragma warning disable CA2254 // テンプレートは静的な式にする必要があります
                this._logger.LogWarning(e.Message + Environment.NewLine + e.StackTrace);
#pragma warning restore CA2254 // テンプレートは静的な式にする必要があります
                context.Features.Set<IAntiforgeryValidationFeature>(new AntiforgeryValidationFeature(false, e));
            }
            await _next(context);
        }
    }

    // Copy
    // https://source.dot.net/#Microsoft.AspNetCore.Antiforgery/src/Shared/HttpExtensions.cs,5202ba04cf9f51a9,references
    internal static class HttpExtensions
    {
        internal const string UrlEncodedFormContentType = "application/x-www-form-urlencoded";
        internal const string MultipartFormContentType = "multipart/form-data";

        internal static bool IsValidHttpMethodForForm(string method) =>
            HttpMethods.IsPost(method) || HttpMethods.IsPut(method) || HttpMethods.IsPatch(method);

        // Key is a string so shared code works across different assemblies (hosting, error handling middleware, etc).
        internal const string OriginalEndpointKey = "__OriginalEndpoint";

        internal static bool IsValidContentTypeForForm(string? contentType)
        {
            if (contentType == null)
            {
                return false;
            }

            // Abort early if this doesn't look like it could be a form-related content-type

            if (contentType.Length < MultipartFormContentType.Length)
            {
                return false;
            }

            return contentType.Equals(UrlEncodedFormContentType, StringComparison.OrdinalIgnoreCase) ||
                contentType.StartsWith(MultipartFormContentType, StringComparison.OrdinalIgnoreCase);
        }

        internal static Endpoint? GetOriginalEndpoint(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            // Some middleware re-execute the middleware pipeline with the HttpContext. Before they do this, they clear state from context, such as the previously matched endpoint.
            // The original endpoint is stashed with a known key in HttpContext.Items. Use it as a fallback.
            if (endpoint == null && context.Items.TryGetValue(OriginalEndpointKey, out var e) && e is Endpoint originalEndpoint)
            {
                endpoint = originalEndpoint;
            }
            return endpoint;
        }

        internal static void ClearEndpoint(HttpContext context)
        {
            var endpoint = context.GetEndpoint();
            if (endpoint != null)
            {
                context.Items[OriginalEndpointKey] = endpoint;

                // An endpoint may have already been set. Since we're going to re-invoke the middleware pipeline we need to reset
                // the endpoint and route values to ensure things are re-calculated.
                context.SetEndpoint(endpoint: null);
            }

            var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
            if (routeValuesFeature != null)
            {
                routeValuesFeature.RouteValues = null!;
            }
        }
    }

    // Copy
    // https://source.dot.net/#Microsoft.AspNetCore.Antiforgery/Internal/AntiforgeryValidationFeature.cs,a62e84601b8cf97d,references
    internal sealed class AntiforgeryValidationFeature(bool isValid, AntiforgeryValidationException? exception) : IAntiforgeryValidationFeature
    {
        public static readonly IAntiforgeryValidationFeature Valid = new AntiforgeryValidationFeature(true, null);

        public bool IsValid { get; } = isValid;
        public Exception? Error { get; } = exception;
    }
}
