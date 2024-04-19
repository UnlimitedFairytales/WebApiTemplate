using Organization.Product.Domain.ValueObjects;

namespace Organization.Product.ApplicationServices
{
    public class BaseResultDto
    {
        public AppMessage? Error { get; set; }
    }
}
