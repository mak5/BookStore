using System.Runtime.Serialization;

namespace BookStore
{
    [Serializable]
    public class InvalidDeliveryDateException : Exception
    {
        public InvalidDeliveryDateException()
        {
        }

        public InvalidDeliveryDateException(string? message) : base(message)
        {
        }

        public InvalidDeliveryDateException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected InvalidDeliveryDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}