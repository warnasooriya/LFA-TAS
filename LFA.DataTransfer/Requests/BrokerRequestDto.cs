using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BrokerRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid CountryId { get; set; }
        public string BrokerStatus { get; set; }
        public string TelNumber { get; set; }
        public string Address { get; set; }
       
    }
}
