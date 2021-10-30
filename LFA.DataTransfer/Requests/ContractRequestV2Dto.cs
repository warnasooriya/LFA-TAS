using System;
using System.Collections.Generic;

namespace TAS.DataTransfer.Requests
{
    public class EligibilityV2
    {
        public int ageMin { get; set; }
        public int ageMax { get; set; }
        public int milageMin { get; set; }
        public int milageMax { get; set; }
        public int monthsMin { get; set; }
        public int monthsMax { get; set; }
        public bool isPresentage { get; set; }
        public string plusMinus { get; set; }
        public decimal premium { get; set; }
        public bool isMandatory { get; set; }
    }

    public class TaxV2
    {
        public string TaxCode { get; set; }
        public string TaxName { get; set; }
        public bool OnNRP { get; set; }
        public bool OnGross { get; set; }
        public Guid Id { get; set; }
        public bool IsSelected { get; set; }
    }

    //public class TexV2
    //{
    //    public TaxV2 Tax { get; set; }
    //    public string Guid { get; set; }
    //    public string CountryId { get; set; }
    //    public string TaxTypeId { get; set; }
    //    public double TaxValue { get; set; }
    //    public bool IsPercentage { get; set; }
    //    public bool IsOnPreviousTax { get; set; }
    //    public bool IsOnNRP { get; set; }
    //    public bool IsOnGross { get; set; }
    //    public double MinimumValue { get; set; }
    //    public int IndexVal { get; set; }
    //    public string currencyPeriodId { get; set; }
    //    public string TpaCurrencyId { get; set; }
    //    public int ConversionRate { get; set; }
    //    public int ValueIncluededInPolicy { get; set; }
    //    public bool IsCountryTaxesExists { get; set; }
    //    public bool IsSelected { get; set; }
    //}

    public class CommissionV2
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsForTPA { get; set; }
        public bool IsNRPCommissionTypesExists { get; set; }
        public bool IsOnNRP { get; set; }
        public bool IsOnGROSS { get; set; }
        public bool IsPercentage { get; set; }
        public decimal Commission { get; set; }
    }

    public class ContractV2
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
        public DateTime contractStartDate { get; set; }
        public DateTime contractEndDate { get; set; }
        public string remark { get; set; }
        public bool isAutoRenewal { get; set; }
        public bool isActive { get; set; }
        public Guid itemStatusId { get; set; }
        public Guid commodityUsageTypeId { get; set; }
        public decimal claimLimitation { get; set; }
        public decimal liabilityLimitation { get; set; }
        //public string InsuaranceLimitationId { get; set; }
       // public Guid attributeSpecificationId { get; set; }
        //public string attributeSpecificationPrefix { get; set; }
       // public string attributeSpecificationName { get; set; }
       // public Guid warrantyTypeId { get; set; }
      //  public Guid premiumBasedOnIdGross { get; set; }
       // public Guid premiumBasedOnIdNett { get; set; }
       // public bool isCustAvailableGross { get; set; }
      //  public bool isCustAvailableNett { get; set; }
      //  public bool manufacturerWarrantyGross { get; set; }
      //  public bool manufacturerWarrantyNett { get; set; }
       // public Guid rsaProviderId { get; set; }
       // public Guid regionId { get; set; }
        public Guid commodityCategoryId { get; set; }
        //public decimal minValueGross { get; set; }
        //public decimal maxValueGross { get; set; }
        //public decimal minValueNett { get; set; }
        //public decimal maxValueNett { get; set; }
        //public decimal annualPremiumTotal { get; set; }
     //   public Guid premiumCurrencyId { get; set; }
        public List<EligibilityV2> eligibilities { get; set; }
        public List<TaxV2> texes { get; set; }
        public List<CommissionV2> commissions { get; set; }
        public bool labourChargeApplicableOnPolicySold { get; set; }
        public bool isPremiumVisibleToDealer { get; set; }
        public int netPremium { get; set; }
        public int grossPremium { get; set; }
        public List<object> annualPremium { get; set; }
        public Guid loggedInUserId { get; set; }
        public decimal AnnualInterestRate { get; set; }

        public List<ClaimCriteriaDto> claimCriteria { get; set; }


    }

    public class ClaimCriteriaDto {
        public Guid id { get; set; }
        public int year { get; set; }
        public string label { get; set; }
        public decimal presentage { get; set; }
    }
    public class PremiumAddonsNettV2
    {
        public Guid Id { get; set; }
        public string CommodityTypeId { get; set; }
        public string Description { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public int IndexNo { get; set; }
        public bool IsApplicableforVariant { get; set; }
        public bool IsPremiumAddonTypeExists { get; set; }
        public decimal value { get; set; }
    }

    public class PremiumAddonsGrossV2
    {
        public Guid Id { get; set; }
        public string CommodityTypeId { get; set; }
        public string Description { get; set; }
        public DateTime EntryDateTime { get; set; }
        public string EntryUser { get; set; }
        public int IndexNo { get; set; }
        public bool IsApplicableforVariant { get; set; }
        public bool IsPremiumAddonTypeExists { get; set; }
        public decimal Value { get; set; }
    }

    public class InsuaranceLimitationV2
    {
        public string Id { get; set; }
        public string InsuaranceLimitationName { get; set; }
        public int Months { get; set; }
    }

    public class AnnualPremiumV2
    {
        public int Year { get; set; }
        public decimal Value { get; set; }
    }

    public class SelectedMakeList
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }

    public class SelectedModelsList
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }

    public class SelectedVariantList
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }


    public class SelectedClinderCount
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }

    public class SelectedEngineCapacity
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }

    public class SelectedGrossWeight
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public Guid value { get; set; }
        public bool open { get; set; }
        public bool @checked { get; set; }
    }


    public class ProductV2
    {
        public Guid Id { get; set; }
        public string CommodityTypeId { get; set; }
        public string CommodityType { get; set; }
        public string Productname { get; set; }
        public string Productcode { get; set; }
        public string Productdescription { get; set; }
        public string Productshortdescription { get; set; }
        public string Displayimage { get; set; }
        public string ProductTypeId { get; set; }
        public object DisplayImageSrc { get; set; }
        public object ProductTypeCode { get; set; }
        public bool Isbundledproduct { get; set; }
        public bool Isactive { get; set; }
        public bool Ismandatoryproduct { get; set; }

        public bool isAllCyllinderCountSelected { get; set; }
        public bool isAllEngineCapacitySelected { get; set; }
        public bool isAllGvwSelected { get; set; }
        public bool isAllMakesSelected { get; set; }
        public bool isAllModelsSelected { get; set; }
        public bool isAllVariantSelected { get; set; }



        public DateTime Entrydatetime { get; set; }
        public string Entryuser { get; set; }
        public DateTime? Lastupdatedatetime { get; set; }
        public string Lastupdateuser { get; set; }
        public object BundledProducts { get; set; }
        public bool IsProductExists { get; set; }
        public List<SelectedMakeList> selectedMakeList { get; set; }
        public List<SelectedModelsList> selectedModelsList { get; set; }
        //public List<Guid> availableModelsDrp { get; set; }
        public List<SelectedVariantList> selectedVariantList { get; set; }
       // public List<Guid> availableVariantsDrp { get; set; }
        public List<SelectedClinderCount> selectedClinderCounts { get; set; }
        public List<SelectedEngineCapacity> selectedEngineCapacities { get; set; }
        public List<SelectedGrossWeight> selectedGrossWeights { get; set; }
        public List<PremiumAddonsNettV2> premiumAddonsNett { get; set; }
        public List<PremiumAddonsGrossV2> premiumAddonsGross { get; set; }
        public List<InsuaranceLimitationV2> insuaranceLimitations { get; set; }
        public Guid InsuaranceLimitationId { get; set; }
        public string attributeSpecificationPrefix { get; set; }
        public Guid attributeSpecificationId { get; set; }
        public string attributeSpecificationName { get; set; }
        public Guid warrantyTypeId { get; set; }
        public Guid itemStatusId { get; set; }
        public Guid premiumBasedOnIdNett { get; set; }
        public bool IsMinMaxVisibleNett { get; set; }
        public bool percentageVisibleNett { get; set; }
        public bool isCustAvailableNett { get; set; }
        public decimal minValueNett { get; set; }
        public decimal maxValueNett { get; set; }
        public decimal minValueGross { get; set; }
        public decimal maxValueGross { get; set; }
        public decimal totalNRP { get; set; }
        public bool manufacturerWarrantyNett { get; set; }
        public Guid premiumBasedOnIdGross { get; set; }
        public bool percentageVisibleGross { get; set; }
        public bool IsMinMaxVisibleGross { get; set; }
        public bool isCustAvailableGross { get; set; }
        public bool manufacturerWarrantyGross { get; set; }
        public decimal grossPremium { get; set; }
        public List<AnnualPremiumV2> annualPremium { get; set; }
        public decimal annualPremiumTotal { get; set; }
        public Guid regionId { get; set; }
        public Guid rsaProviderId { get; set; }
    }

    public class ContractAddRequestV2
    {
        public ContractV2 contract { get; set; }
        public List<ProductV2> products { get; set; }
    }

    public class ContractRequestV2Dto
    {
        public ContractAddRequestV2 data { get; set; }

    }
}