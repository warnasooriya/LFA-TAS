using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    [Serializable]
    public class CurrencyResponseDto
    {
        public virtual Guid Id { get; set; }
        public virtual string Country { get; set; }
        public virtual string CurrencyName { get; set; }
        public virtual string Code { get; set; }
        public virtual string Symbol { get; set; }
    }
}
