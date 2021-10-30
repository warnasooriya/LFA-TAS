using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class DriveTypeRequestDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string DriveTypeDescription { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool DriveTypeInsertion
        {
            get;
            set;
        }
    }
}
