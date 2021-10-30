using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleHorsePowerRequestDto
    {
        public Guid Id { get; set; }
        public string HorsePower { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool VehicleHorsePowerInsertion
        {
            get;
            set;
        }

    }
}
