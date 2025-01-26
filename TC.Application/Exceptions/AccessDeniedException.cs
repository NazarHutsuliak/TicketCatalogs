namespace TC.Application.Exceptions
{
    public class AccessDeniedException : Exception
    {
        public AccessDeniedException()
            : base("Access denied.")
        {
        }

        public AccessDeniedException(string message)
            : base(message)
        {
        }

        public AccessDeniedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
