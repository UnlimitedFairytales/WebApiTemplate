namespace Organization.Product.Domain.Common.Entities
{
    public class UserInfo
    {
        public long? UserId { get; set; }
        public string? UserCd { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }

        public long? Ver { get; set; }

        // AuthZ
        // Menu list
        // Sub menu list
        // App Roles / App privilege group
        // App privilege
        // Other flags...
    }
}
