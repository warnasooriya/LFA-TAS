using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class InsurerRequestDto
    {
        public Guid Id { get; set; }
        public string InsurerCode { get; set; }
        public string BookletCode { get; set; }
        public string InsurerShortName { get; set; }
        public string InsurerFullName { get; set; }
        public string Comments { get; set; }
        public List<Guid> Countries { get; set; }
        public List<Guid> Products { get; set; }
        public List<Guid> CommodityTypes { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool InsurerInsertion
        {
            get;
            set;
        }

    }
}
