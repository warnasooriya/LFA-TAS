using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class VehicleKiloWattRequestDto
    {
        public Guid Id { get; set; }
        public string KiloWatt { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool VehicleKiloWattInsertion
        {
            get;
            set;
        }

    }
}
