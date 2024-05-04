namespace Organization.Product.Api.Utils.Extensions
{
    public static class ConfigurationExtension
    {
        public static T[] GetValueArray<T>(this IConfiguration cnf, string key)
        {
            return cnf.GetSection(key).Get<T[]>();
        }

        public static T[] GetValueArray<T>(this IConfigurationSection cnf, string key)
        {
            return cnf.GetSection(key).Get<T[]>();
        }
    }
}
