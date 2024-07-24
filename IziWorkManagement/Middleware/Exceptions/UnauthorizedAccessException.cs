namespace IziWorkManagement.Middleware.Exceptions
{
    public class UnauthorizedAccessException : Exception
    {
        public UnauthorizedAccessException(string message) : base(message)
        { }
    }
}
