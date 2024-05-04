using System.Security.Claims;

namespace Organization.Product.Domain.Common.ValueObjects
{
    public interface ICommonParams
    {
        ClaimsPrincipal? Principal { get; }
        string? User { get; }
        string? Prog { get; }
        string? Term { get; }
    }
}
