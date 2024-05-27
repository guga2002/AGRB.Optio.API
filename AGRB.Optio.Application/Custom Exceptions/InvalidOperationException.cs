namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class InvalidOperationException:Exception
    {
        public InvalidOperationException() { }

        public InvalidOperationException(string message) : base(message) { }

        public InvalidOperationException(string message, Exception exception) : base(message, exception) { }
    }
}
