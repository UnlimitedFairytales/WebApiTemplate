namespace Organization.Product.Domain.ValueObjects
{
    public class AppException : Exception
    {
        public static AppException Create(AppMessage error, Exception? innerException = null)
        {
            return new AppException(error.FormattedText, innerException, error);
        }

        public AppMessage Error { get; private set; }

        private AppException(string message, Exception? innerException, AppMessage error)
            : base(message, innerException)
        {
            this.Error = error;
        }
    }
}
