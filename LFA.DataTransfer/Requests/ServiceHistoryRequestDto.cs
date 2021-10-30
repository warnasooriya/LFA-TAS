using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ServiceHistoryRequestDto
    {
        public Guid Id { get; set; }
        public int serviceNumber { get; set; }
        public decimal milage { get; set; }
        public string remarks { get; set; }
        public DateTime serviceDate { get; set; }
       
    }
}
