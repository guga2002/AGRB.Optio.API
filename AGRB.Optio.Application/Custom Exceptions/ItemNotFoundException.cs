namespace RGBA.Optio.Domain.Custom_Exceptions
{
    public class ItemNotFoundException:Exception
    {
        public ItemNotFoundException() { }

        public ItemNotFoundException(string message):base(message) { }

        public ItemNotFoundException(string message, Exception exception) : base(message, exception) { }
      
    }
}
