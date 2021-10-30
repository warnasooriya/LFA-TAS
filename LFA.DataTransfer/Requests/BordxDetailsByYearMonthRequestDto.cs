using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class BordxDetailsByYearMonthRequestDto
    {
        public int page { get; set; }
        public int pageSize { get; set; }
        public int year { get; set; }
        public int month { get; set; }
        public int number { get; set; }
        public Guid insurerId { get; set; }
        public Guid reinsurerId { get; set; }
        //public Guid countryId { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid productId { get; set; }

    }

}
