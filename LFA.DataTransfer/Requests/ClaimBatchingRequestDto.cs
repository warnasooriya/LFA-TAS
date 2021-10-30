using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ClaimBatchingRequestDto
    {
        //
        public  Guid Id { get; set; }
        public  Guid CountryId { get; set; }
        public  Guid DealerId { get; set; }
        public  Guid InsurerId { get; set; }
        public  Guid ReinsurerId { get; set; }
        public  string BatchNumber { get; set; }
        public string Prefix { get; set; }

        
        public  DateTime EntryDate { get; set; }
        public  Guid EntryBy { get; set; }
        public bool ClaimBatchingEntryInsertion
        {
            get;
            set;
        }

    }
}
