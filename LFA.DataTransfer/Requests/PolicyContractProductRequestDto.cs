using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace TAS.DataTransfer.Requests
{
    public class PolicyContractProductRequestDto
    {
        public Guid Id { get; set; }
        public string PolicyNo { get; set; }
        public DateTime PolicyStartDate { get; set; }
        public DateTime PolicyEndDate { get; set; }
        public Guid ProductId { get; set; }
        public Guid ParentProductId { get; set; }
        public Guid ContractId { get; set; }
        public Guid ExtensionTypeId { get; set; }
        public Decimal Premium { get; set; }
        public Guid PremiumCurrencyTypeId { get; set; }
        public Guid CoverTypeId { get; set; }
        public string Type { get; set; }
        public Guid ReferenceId { get; set; }
        public string Name { get; set; }
        public string PremiumCurrencyName { get; set; }
        public bool RSA { get; set; }
        public List<ContractRequestDto> Contracts { get; set; }
        public List<ExtensionTypeRequestDto> ExtensionTypes { get; set; }
        public List<WarrantyTypeRequestDto> CoverTypes { get; set; }
        public List<RSAProviderRequestDto> RSAProviders { get; set; }

        public bool PolicyContractProductInsertion
        {
            get;
            set;
        }

    }
}
