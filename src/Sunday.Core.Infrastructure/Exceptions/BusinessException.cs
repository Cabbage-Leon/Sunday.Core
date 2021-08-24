using Homsom.Friday.Exceptions;

namespace System
{
    public class BusinessException : DomainException
    {
        public BusinessException()
        {
        }

        public BusinessException(object errorCode, string? message = null, Exception? innerException = null) : base(errorCode, message, innerException)
        {
        }
    }
}