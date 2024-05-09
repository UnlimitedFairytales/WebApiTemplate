using Microsoft.AspNetCore.Antiforgery;

namespace Organization.Product.Api._1_Middleware.AntiCsrf
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class IgnoreAntiforgeryAttribute : Attribute, IAntiforgeryMetadata
    {
        public bool RequiresValidation => false;
    }
}
