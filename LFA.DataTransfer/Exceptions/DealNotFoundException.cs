using System;

namespace TAS.DataTransfer.Exceptions
{
    public class DealNotFoundException : Exception
    {
        public DealNotFoundException(String messege)
            : base(messege)
        {
        }
    }
}
