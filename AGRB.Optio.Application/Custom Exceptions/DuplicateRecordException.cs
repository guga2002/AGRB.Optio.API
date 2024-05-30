namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class DuplicateRecordException:Exception
    {
        public DuplicateRecordException() { }

        public DuplicateRecordException(string message) : base(message) { }

        public DuplicateRecordException(string message, Exception exception) : base(message, exception) { }
    }
}
