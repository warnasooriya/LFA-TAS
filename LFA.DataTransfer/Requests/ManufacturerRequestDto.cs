using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ManufacturerRequestDto
    {
        public Guid Id { get; set; }
        public string ManufacturerCode { get; set; }
        public string ManufacturerName { get; set; }
        public List<Guid> ComodityTypes { get; set; }
        public string ManufacturerClassId { get; set; }
        public bool IsWarrentyGiven { get; set; }
        public bool IsActive { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }

        public bool ManufacturerInsertion
        {
            get;
            set;
        }

    }
}
