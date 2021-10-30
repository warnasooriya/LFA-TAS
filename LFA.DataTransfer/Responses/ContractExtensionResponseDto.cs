using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Responses
{
    public class ContractExtensionResponseDto
    {
        public Guid Id { get; set; }
        public string AttributeSpecification { get; set; }
        public Guid ContractId { get; set; }
        public Guid ContractInsuanceLimitationId { get; set; }
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
        public bool IsCustAvailableGross { get; set; }
        public bool IsCustAvailableNett { get; set; }
        public bool ManufacturerWarrantyNett { get; set; }
        public bool ManufacturerWarrantyGross { get; set; }

        public Guid WarrantyTypeId { get; set; }
        public DateTime EntryDateTime { get; set; }
        public Guid EntryUser { get; set; }
        public Guid RSAProviderId { get; set; }
        public Guid RegionId { get; set; }
        public decimal Rate { get; set; }

        public List<Guid> Makes { get; set; }
        public List<Guid> Modeles { get; set; }
        public List<Guid> Variants { get; set; }

        public List<Guid> EngineCapacities { get; set; }
        public List<Guid> CylinderCounts { get; set; }
        public List<ContractExtensionsPremiumAddonResponseDto> PremiumAddones { get; set; }

        public bool IsContractExtensionsExists { get; set; }
        public decimal taxValue { get; set; }

        public bool isAllMakes { get; set; }
        public bool isAllModels { get; set; }
        public bool isAllVariants { get; set; }
        public bool isAllEngineCapacities { get; set; }
        public bool isAllCyllinderCount { get; set; }
    }
}
