using System;

namespace TAS.DataTransfer.Exceptions
{
    public class CustomException : Exception
    {
        public int errorCode { get; set; }
        public CustomException(String messege, int errorCode)
            : base(messege)
        {
            this.errorCode = errorCode;
        }
        public CustomException(String messege, Exception innerException, int errorCode)
            : base(messege, innerException)
        {
            this.errorCode = errorCode;
        }
    }
}
