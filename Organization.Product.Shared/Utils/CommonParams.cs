using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;

namespace Organization.Product.Shared.Utils
{
    public class CommonParams
    {
        readonly HttpContext? _context;

        public ClaimsPrincipal? Principal { get => this._context?.User; }
        public string? User
        {
            get
            {
                // https://stackoverflow.com/questions/62475109/asp-net-core-jwt-authentication-changes-claims-sub
                // 既定の動作はsubをNameIdentifierに変換するため、ClaimTypes.NameIdentifierでヒットする
                if (this.Principal?.Claims == null) return null;
                return (from c in this.Principal.Claims
                        where c.Type == ClaimTypes.NameIdentifier || c.Type == JwtRegisteredClaimNames.Sub
                        select c.Value).FirstOrDefault();
            }
        }
        public string? Prog { get => this._context?.Request.RouteValues["controller"]?.ToString()?.Replace("Controller", string.Empty); }
        public string? Term { get => this._context?.Connection.RemoteIpAddress?.MapToIPv4()?.ToString() ?? ""; }

        public CommonParams(IHttpContextAccessor httpContextAccessor)
        {
            this._context = httpContextAccessor.HttpContext;
        }
    }
}
