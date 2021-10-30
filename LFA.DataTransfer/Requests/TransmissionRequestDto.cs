using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class TransmissionTypeRequestDto
    {
        public Guid Id { get; set; }
        public string TransmissionTypeCode { get; set; }
        public List<string> Technology { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool TransmissionTypeInsertion
        {
            get;
            set;
        }

    }

    public class TransmissionTechnologyRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public bool TransmissionTechnologyInsertion
        {
            get;
            set;
        }

    }
}
