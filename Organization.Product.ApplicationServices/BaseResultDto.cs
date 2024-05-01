using Organization.Product.Domain.Common.ValueObjects;

namespace Organization.Product.ApplicationServices
{
    public class BaseResultDto
    {
        public AppMessage? Error { get; set; }
    }
}
