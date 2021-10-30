using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleColorRequestDto
    {
        public Guid Id { get; set; }
        public string VehicleColorCode { get; set; }
        public string Color { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }



        public bool VehicleColorInsertion
        {
            get;
            set;
        }
    }
}
