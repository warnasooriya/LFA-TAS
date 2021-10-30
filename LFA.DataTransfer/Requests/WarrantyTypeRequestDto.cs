using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class WarrantyTypeRequestDto
    {
        public Guid Id { get; set; }
        public string WarrantyTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool WarrantyTypeInsertion
        {
            get;
            set;
        }

    }
}
