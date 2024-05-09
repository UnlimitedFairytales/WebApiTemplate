namespace Organization.Product.Shared.Configurations
{
    public class AntiCsrfOption
    {
        public bool Enabled { get; set; } = false;
        public string RequestTokenName { get; set; } = "";

        public AntiCsrfOption(IConfiguration configuration)
        {
            configuration.GetSection("AntiCsrf").Bind(this);
        }
    }
}
