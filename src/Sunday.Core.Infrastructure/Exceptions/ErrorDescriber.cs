
namespace System
{
    public class ErrorDescriber
    {
        public static BusinessException DomainException(object errorCode, string message) => new BusinessException(errorCode, message);

        public static BusinessException DomainException(string message) => new BusinessException("domainException", message);
    }
}