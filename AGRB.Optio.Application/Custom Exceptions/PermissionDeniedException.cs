namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class PermissionDeniedException:Exception
    {
        public PermissionDeniedException() { }

        public PermissionDeniedException(string message) : base(message) { }

        public PermissionDeniedException(string message, Exception exception) : base(message, exception) { }
    }
}
