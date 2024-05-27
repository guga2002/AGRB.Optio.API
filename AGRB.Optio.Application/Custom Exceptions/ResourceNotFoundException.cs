namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class ResourceNotFoundException:Exception
    {
        public ResourceNotFoundException() { }

        public ResourceNotFoundException(string message) : base(message) { }

        public ResourceNotFoundException(string message, Exception exception) : base(message, exception) { }
    }
}
