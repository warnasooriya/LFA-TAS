using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Responses
{
    public class PolicyContractProductResponseDto
    {
        public Guid Id { get; set; }
        public string PolicyNo { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Guid AttributeSpecificationId { get; set; }
        public Guid ContractExtensionsId { get; set; }
        public Decimal Premium { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public Guid CoverTypeId { get; set; }
        public string Type { get; set; }
        public Guid ReferenceId { get; set; }
        public string Name { get; set; }
        public string PremiumCurrencyName { get; set; }
        public bool RSA { get; set; }
        public List<ContractResponseDto> Contracts { get; set; }
        public object ExtensionTypes { get; set; }
        public List<InsuaranceLimitationResponseDto> ExtensionTypesLimi { get; set; }
        public object CoverTypess { get; set; }
        public List<WarrantyTypeResponseDto> CoverTypes { get; set; }
        public List<RSAProviderResponseDto> RSAProviders { get; set; }
        public List<ContractExtensionResponseDto> AttributeSpecifications { get; set; }
        public bool IsPolicyContractProductExists { get; set; }
        public string BookletNumber { get; set; }
        public Decimal FinanceAmount { get; set; }
        public object PolicyAdditionalDetails { get; set; }

    }
}
