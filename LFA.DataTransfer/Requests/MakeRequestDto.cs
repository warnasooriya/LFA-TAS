using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class MakeRequestDto
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string MakeCode { get; set; }
        public string MakeName { get; set; }
        public Guid ManufacturerId { get; set; }
        public bool WarantyGiven { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsMakeExists { get; set; }
        public string message { get; set; }

        public bool MakeInsertion
        {
            get;
            set;
        }

    }
}
