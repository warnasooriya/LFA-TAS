using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class ContractExtensionsRequestDto
    {
        public Guid Id { get; set; }
        public string AttributeSpecification { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public virtual Guid PremiumBasedOnIdGross { get; set; }
        public virtual Guid PremiumBasedOnIdNett { get; set; }
        public virtual decimal MinGross { get; set; }
        public virtual decimal MaxGross { get; set; }
        public virtual decimal MinNett { get; set; }
        public virtual decimal MaxNett { get; set; }
        public decimal PremiumTotal { get; set; }
        public decimal GrossPremium { get; set; }
        public Guid PremiumCurrencyId { get; set; }
        public bool IsCustAvailable { get; set; }
        public bool ManufacturerWarranty { get; set; }
        public Guid WarrantyTypeId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public Guid RSAProviderId { get; set; }
        public Guid RegionId { get; set; }
        public decimal Rate { get; set; }

        public List<Guid> Makes { get; set; }
        public List<Guid> Modeles { get; set; }
        public List<Guid> EngineCapacities { get; set; }
        public List<Guid> CylinderCounts { get; set; }
        public List<ContractExtensionsPremiumAddonRequestDto> PremiumAddones { get; set; }

        public bool ContractExtensionsInsertion
        {
            get;
            set;
        }

    }
}
