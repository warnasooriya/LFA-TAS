using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class AddContractRequestObject
    {
        public AddContractRequestDto data { get; set; }
    }

    public class UpdateContractRequestObject
    {
        public Guid contractId { get; set; }
        public AddContractRequestDto data { get; set; }
    }
    public class AddContractRequestDto
    {
        public Contract contract { get; set; }
        public List<Make> makes { get; set; }
        public List<Model> models { get; set; }
        public List<Cc> ccs { get; set; }
        public List<Variant> variants { get; set; }
        public List<Engine> engine { get; set; }
        public List<PremiumAddonsNett> premiumAddonsNett { get; set; }
        public List<PremiumAddonsGross> premiumAddonsGross { get; set; }
        public bool isAllMakes { get; set; }
        public bool isAllModels { get; set; }
        public bool isAllVariants { get; set; }
        public bool isAllEngineCapacities { get; set; }
        public bool isAllCyllinderCount { get; set; }

    }

    public class Eligibility
    {
        public int ageMin { get; set; }
        public int ageMax { get; set; }
        public int milageMin { get; set; }
        public int milageMax { get; set; }
        public int monthsMin { get; set; }
        public int monthsMax { get; set; }
        public bool isPresentage { get; set; }
        public string plusMinus { get; set; }
        public int premium { get; set; }
    }

    public class Contract
    {
        public Guid countryId { get; set; }
        public Guid dealerId { get; set; }
        public Guid commodityTypeId { get; set; }
        public Guid productId { get; set; }
        public Guid linkDealId { get; set; }
        public string dealName { get; set; }
        public string dealType { get; set; }
        public bool isPromotional { get; set; }
        public bool discountAvailable { get; set; }
        public Guid insurerId { get; set; }
        public Guid reinsurerContractId { get; set; }
        public string contractStartDate { get; set; }
        public string contractEndDate { get; set; }
        public string remark { get; set; }
        public bool isAutoRenewal { get; set; }
        public bool isActive { get; set; }
        public Guid itemStatusId { get; set; }
        public Guid commodityUsageTypeId { get; set; }
        public decimal claimLimitation { get; set; }
        public decimal liabilityLimitation { get; set; }
        public string extensionTypeId { get; set; }
        public string attributeSpecificationId { get; set; }
        public string attributeSpecificationPrefix { get; set; }
        public string attributeSpecificationName { get; set; }
        public Guid warrantyTypeId { get; set; }
        public string premiumBasedOnIdGross { get; set; }
        public string premiumBasedOnIdNett { get; set; }
        public bool isCustAvailableGross { get; set; }
        public bool isCustAvailableNett { get; set; }
        public bool manufacturerWarrantyGross { get; set; }
        public bool manufacturerWarrantyNett { get; set; }
        public Guid rsaProviderId { get; set; }
        public Guid regionId { get; set; }
        public Guid commodityCategoryId { get; set; }
        public decimal minValueGross { get; set; }
        public decimal maxValueGross { get; set; }
        public decimal minValueNett { get; set; }
        public decimal maxValueNett { get; set; }
        public decimal annualPremiumTotal { get; set; }
        public decimal netPremium { get; set; }
        public decimal grossPremium { get; set; }
        public Guid premiumCurrencyId { get; set; }
        public List<Eligibility> eligibilities { get; set; }
        public List<Tax> texes { get; set; }
        public List<Commission> commissions { get; set; }
        public List<AnnualPremium> annualPremium { get; set; }
    }
    public class AnnualPremium
    {
        public int Year { get; set; }
        public decimal Value { get; set; }
    }
    public class Make
    {
        public Guid id { get; set; }
    }

    public class Model
    {
        public Guid id { get; set; }
    }

    public class Variant
    {
        public Guid id { get; set; }
    }

    public class Cc
    {
        public Guid id { get; set; }
    }

    public class Engine
    {
        public Guid id { get; set; }
    }

    public class Tax
    {
        public Guid Id { get; set; }
        public bool IsSelected { get; set; }

    }
    public class Commission
    {
        public Guid Id { get; set; }
        public decimal commission { get; set; }
        public bool IsPercentage { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGROSS { get; set; }

    }

    public class PremiumAddonsNett
    {
        public Guid Id { get; set; }
        public Guid CommodityTypeId { get; set; }
        public string Description { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public int IndexNo { get; set; }
        public bool IsPremiumAddonTypeExists { get; set; }
        public decimal value { get; set; }
        public decimal Value { get; set; }
    }

    public class PremiumAddonsGross
    {
        public Guid Id { get; set; }
        public string CommodityTypeId { get; set; }
        public string Description { get; set; }
        public string EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public int IndexNo { get; set; }
        public bool IsPremiumAddonTypeExists { get; set; }
        public decimal value { get; set; }
        public decimal Value { get; set; }
    }



}
