using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxProcessRequestDto
    {
        public int year { get; set; }
        public int month { get; set; }
        public int number { get; set; }
        public Guid userId { get; set; }
        //public Guid countryId { get; set; }
        public Guid insurerId { get; set; }
        public Guid reinsurerId { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid productId { get; set; }


    }
}
