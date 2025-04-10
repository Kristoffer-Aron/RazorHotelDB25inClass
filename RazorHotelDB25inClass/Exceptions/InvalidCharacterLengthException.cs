namespace RazorHotelDB25inClass.Exceptions
{
    public class InvalidCharacterLengthException : Exception
    {
        public InvalidCharacterLengthException() : base()
        {

        }
        public InvalidCharacterLengthException(string message) : base(message)
        {

        }
    }
}
