namespace Organization.Product.ApplicationServices.UseCases.Login
{
    public class LoginResultDto : BaseResultDto
    {
        public string? Token { get; set; }

        public LoginResultDto(string? token)
        {
            this.Token = token;
        }
    }
}
