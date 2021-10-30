using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleBodyTypeRequestDto
    {
        public Guid Id { get; set; }
        public string VehicleBodyTypeCode { get; set; }
        public string VehicleBodyTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }


        public bool VehicleBodyTypeInsertion
        {
            get;
            set;
        }
    }
}
