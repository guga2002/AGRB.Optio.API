namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class OperationTimeoutException:Exception
    {
        public OperationTimeoutException() { }

        public OperationTimeoutException(string message) : base(message) { }

        public OperationTimeoutException(string message, Exception exception) : base(message, exception) { }
    }
}
