namespace Organization.Product.Domain.Common.ValueObjects
{
    public class AppException : Exception
    {
        public static AppException Create(AppMessage error, Exception? innerException = null)
        {
            return new AppException(error.FormattedText, innerException, error);
        }

        // static
        // ----------------------------------------
        // instance

        public AppMessage Error { get; private set; }

        private AppException(string message, Exception? innerException, AppMessage error)
            : base(message, innerException)
        {
            this.Error = error;
        }
    }
}
