using System;
using System.Collections.Generic;

namespace TAS.Services.Entities
{
    #region Base Tables
    public class Contract
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CountryId { get; set; }
        public virtual Guid DealerId { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid CommodityCategoryId { get; set; }

        public virtual Guid ProductId { get; set; }
        public virtual Guid LinkDealId { get; set; }
        public virtual string DealName { get; set; }
        public virtual string DealType { get; set; }
      //  public virtual Guid ItemStatusId { get; set; }
        public virtual Guid InsurerId { get; set; }
      //  public virtual Guid ReinsurerId { get; set; }
        public virtual bool IsPromotional { get; set; }
        public virtual bool IsAutoRenewal { get; set; }
        public virtual bool DiscountAvailable { get; set; }
        public virtual bool IsActive { get; set; }
        public virtual DateTime StartDate { get; set; }
        public virtual DateTime EndDate { get; set; }
      //  public virtual decimal PremiumTotal { get; set; }
        public virtual decimal ClaimLimitation { get; set; }
        public virtual decimal LiabilityLimitation { get; set; }
        public virtual string Remark { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid CommodityUsageTypeId { get; set; }
        public virtual bool labourChargeApplicableOnPolicySold { get; set; }

        public virtual Guid ReinsurerContractId { get; set; }
        public virtual decimal ConversionRate { get; set; }
        public virtual Guid CurrencyPeriodId { get; set; }
        public virtual Guid CurrencyId { get; set; }
        public virtual bool IsPremiumVisibleToDealer { get; set; }
        public virtual decimal AnnualInterestRate { get; set; }
    }

    public class ExtensionType
    {
        public virtual Guid Id { get; set; }
        public virtual string ExtensionName { get; set; }
        public virtual int Km { get; set; }
        public virtual int Month { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual Guid ProductId { get; set; }
        public virtual int Hours { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }

    public class PremiumBasedOn
    {
        public virtual Guid Id { get; set; }
        public virtual string Code { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
    }

    public class PremiumAddonType
    {
        public virtual Guid Id { get; set; }
        public virtual Guid CommodityTypeId { get; set; }
        public virtual string Description { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual string AddonTypeCode { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual int IndexNo { get; set; }
        public virtual bool IsApplicableforVariant { get; set; }


    }

    public class ContractExtensions
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractInsuanceLimitationId { get; set; }
        public virtual string AttributeSpecification { get; set; }
        public virtual Guid AttributeSpecificationId { get; set; }


        //public virtual Guid ContractId { get; set; }
        //public virtual Guid ExtensionTypeId { get; set; }
        //public virtual Guid PremiumBasedOnIdGross { get; set; }
        //public virtual Guid PremiumBasedOnIdNett { get; set; }
        //public virtual decimal MinGross { get; set; }
        //public virtual decimal MaxGross { get; set; }
        //public virtual decimal MinNett { get; set; }
        //public virtual decimal MaxNett { get; set; }
        //public virtual decimal PremiumTotal { get; set; }
        //public virtual decimal GrossPremium { get; set; }
        //public virtual Guid PremiumCurrencyId { get; set; }
        //public virtual bool IsCustAvailableGross { get; set; }
        //public virtual bool IsCustAvailableNett { get; set; }
        //public virtual bool ManufacturerWarrantyGross { get; set; }
        //public virtual bool ManufacturerWarrantyNett { get; set; }
        //public virtual Guid WarrantyTypeId { get; set; }
        //public virtual Guid CurrencyPeriodId { get; set; }
        public virtual DateTime EntryDateTime { get; set; }
        public virtual Guid EntryUser { get; set; }
        public virtual Guid RSAProviderId { get; set; }
        public virtual Guid RegionId { get; set; }
        public virtual decimal Rate { get; set; }
        public virtual bool IsAllGVWSelected { get; set; }
        public virtual bool IsRSA { get; set; }
        public virtual bool IsAllMakesSelected { get; set; }
        public virtual bool IsAllModelsSelected { get; set; }
        public virtual bool IsAllVariantsSelected { get; set; }
        public virtual bool IsAllEngineCapacitiesSelected { get; set; }
        public virtual bool IsAllCyllinderCountsSelected { get; set; }
        public virtual Guid ProductId { get; set; }


    }
    #endregion

    #region Mappint Tables
    public class ContractExtensionsPremiumAddon
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid PremiumAddonTypeId { get; set; }
        public virtual decimal Value { get; set; }
        public virtual String PremiumType { get; set; }
        public virtual Guid ContractExtensionPremiumId { get; set; }



    }

    public class ContractExtensionMake
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid MakeId { get; set; }
    }

    public class ContractExtensionModel
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid ModelId { get; set; }
    }

    public class ContractExtensionManufacturerWarranty
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid ManufacturerWarrantyId { get; set; }
    }

    public class ContractExtensionEngineCapacity
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid EngineCapacityId { get; set; }
    }

    public class ContractExtensionGVW
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid GVWId { get; set; }
    }

    public class ContractExtensionCylinderCount
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractExtensionId { get; set; }
        public virtual Guid CylinderCountId { get; set; }
    }

    public class ContractTaxes
    {
        public virtual Guid Id { get; set; }
        public virtual Guid ContractId { get; set; }
        public virtual Guid CountryTaxId { get; set; }
    }
    #endregion

    public class ContractExtensionsInfo : ContractExtensions
    {
        public virtual List<Guid> Makes { get; set; }
        public virtual List<Guid> Modeles { get; set; }
        public virtual List<Guid> EngineCapacities { get; set; }
        public virtual List<Guid> CylinderCounts { get; set; }
        public virtual List<ContractExtensionsPremiumAddon> PremiumAddones { get; set; }
    }
}