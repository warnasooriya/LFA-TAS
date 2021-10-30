using System;

namespace TAS.DataTransfer.Exceptions
{
    public class CurrencyPeriodNotSetException : Exception
    {
        public CurrencyPeriodNotSetException(String messege)
            : base(messege)
        {
        }
    }
}
