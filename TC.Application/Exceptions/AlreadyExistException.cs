namespace TC.Application.Exceptions
{
    public class AlreadyExistException : Exception
    {
        public AlreadyExistException()
            : base("Already exist.")
        {
        }

        public AlreadyExistException(string message)
            : base(message)
        {
        }

        public AlreadyExistException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
