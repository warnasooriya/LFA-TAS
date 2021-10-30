using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ManufacturerWarrantyRequestDto
    {
        public Guid Id { get; set; }
        public Guid MakeId { get; set; }
        public List<Guid> Countrys { get; set; }
        public List<Guid> Models { get; set; }
        public string WarrantyName { get; set; }
        public int WarrantyMonths { get; set; }
        public int WarrantyKm { get; set; }
        public DateTime ApplicableFrom { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public bool IsUnlimited { get; set; }

        public bool ManufacturerWarrantyInsertion
        {
            get;
            set;
        }

    }
}
