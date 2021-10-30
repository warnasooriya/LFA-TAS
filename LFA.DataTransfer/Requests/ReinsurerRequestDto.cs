using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ReinsurerRequestDto
    {
        public Guid Id { get; set; }
        public string ReinsurerName { get; set; }
        public string ReinsurerCode { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public Guid CurrencyId { get; set; }

        public bool ReinsurerInsertion
        {
            get;
            set;
        }

    }
}
