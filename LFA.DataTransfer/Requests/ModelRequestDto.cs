using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ModelRequestDto
    {
        public Guid Id { get; set; }
        public string ModelCode { get; set; }
        public string ModelName { get; set; }
        public Guid MakeId { get; set; }
        public Guid CategoryId { get; set; }
        public int NoOfDaysToRiskStart { get; set; }
        public bool WarantyGiven { get; set; }
        public bool IsActive { get; set; }
        public Guid ContryOfOrigineId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool AdditionalPremium { get; set; }

        public bool ModelInsertion
        {
            get;
            set;
        }

    }
}
