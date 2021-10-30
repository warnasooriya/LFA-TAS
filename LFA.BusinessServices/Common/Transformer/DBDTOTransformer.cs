using NHibernate;
using NHibernate.Linq;
using NHibernate.Util;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Exceptions;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using Contract = TAS.Services.Entities.Contract;
using Make = TAS.Services.Entities.Make;
using Model = TAS.Services.Entities.Model;

namespace TAS.Services.Common.Transformer
{
    public class DBDTOTransformer
    {
        private static readonly Lazy<DBDTOTransformer> lazy = new Lazy<DBDTOTransformer>(() => new DBDTOTransformer());
        public static DBDTOTransformer Instance { get { return lazy.Value; } }

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<NRPCommissionContractMappingResponseDto> Commissions(IEnumerable<NRPCommissionContractMapping> currentNRPContractMapping)
        {
            List<NRPCommissionContractMappingResponseDto> CommissionsList = new List<NRPCommissionContractMappingResponseDto>();
            foreach (var commission in currentNRPContractMapping)
            {
                var Commission = new NRPCommissionContractMappingResponseDto()
                {
                    Id = commission.Id,
                    ContractId = commission.ContractId,
                    Commission = commission.Commission,
                    IsNRPCommissionContractMappingExists = true,
                    IsPercentage = commission.IsPercentage,
                    NRPCommissionId = commission.NRPCommissionId,
                    IsOnNRP = commission.IsOnNRP,
                    IsOnGROSS = commission.IsOnGROSS

                };
                CommissionsList.Add(Commission);
            }
            return CommissionsList;
        }
        public List<CountryTaxesResponseDto> CountryTaxes(IEnumerable<CountryTaxes> currentFullTaxes)
        {
            List<CountryTaxesResponseDto> TaxDetailsList = new List<CountryTaxesResponseDto>();
            foreach (var tax in currentFullTaxes)
            {
                var Tax = new CountryTaxesResponseDto()
                {
                    IndexVal = tax.IndexVal,
                    CountryId = tax.CountryId,
                    Id = tax.Id,
                    IsCountryTaxesExists = true,
                    IsOnGross = tax.IsOnGross,
                    IsOnNRP = tax.IsOnNRP,
                    IsOnPreviousTax = tax.IsOnPreviousTax,
                    IsPercentage = tax.IsPercentage,
                    MinimumValue = tax.MinimumValue,
                    TaxTypeId = tax.TaxTypeId,
                    TaxValue = tax.TaxValue
                };
                TaxDetailsList.Add(Tax);
            }
            return TaxDetailsList;
        }
        public List<EligibilityResponseDto> Eligibilities(IEnumerable<TAS.Services.Entities.Eligibility> currentEligibilities, Guid CurrencyPeriodId, Guid CurrencyId)
        {
            List<EligibilityResponseDto> EligibilitiesList = new List<EligibilityResponseDto>();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            foreach (var eligibility in currentEligibilities)
            {
                EligibilityResponseDto Eligibility = new EligibilityResponseDto()
                {
                    ContractId = eligibility.ContractId,
                    EntryDateTime = eligibility.EntryDateTime,
                    EntryUser = eligibility.EntryUser,
                    Id = eligibility.Id,
                    IsEligibilityExists = true,
                    IsPercentage = eligibility.IsPercentage,
                    MileageFrom = eligibility.MileageFrom,
                    MileageTo = eligibility.MileageTo,
                    MonthsFrom = eligibility.MonthsFrom,
                    MonthsTo = eligibility.MonthsTo,
                    AgeFrom = eligibility.AgeFrom,
                    AgeTo = eligibility.AgeTo,
                    PlusMinus = eligibility.PlusMinus,
                    Premium = eligibility.IsPercentage == true ? eligibility.Premium :
                    currencyEm.ConvertFromBaseCurrency(eligibility.Premium, CurrencyId, CurrencyPeriodId)
                };
                EligibilitiesList.Add(Eligibility);

            }
            return EligibilitiesList;
        }
        public ContractInfo ContractToContractInfo(TAS.Services.Entities.Contract currentContract,
            List<NRPCommissionContractMappingResponseDto> CommissionsList,
            List<CountryTaxesResponseDto> TaxesList,
            List<EligibilityResponseDto> EligibilityList,
            ContractExtensions currentExtentionInfo,
            bool withTax,
            List<PremiumAddonType> premiumAddonTypes,
            Entities.Variant variant,
             Entities.Model model,
            IEnumerable<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddon
            )
        {
            CurrencyEntityManager currencyEM = new CurrencyEntityManager();
            CommonEntityManager cem = new CommonEntityManager();
            string premiumBasedOnCodeGross = "";
            // cem.GetPremiumBasedonCodeById(currentExtentionInfo.PremiumBasedOnIdNett);

            var Contract = new ContractInfo()
            {
                // ClaimLimitation = currencyEM.ConvertFromBaseCurrency(currentContract.ClaimLimitation, currentExtentionInfo.PremiumCurrencyId, currentExtentionInfo.CurrencyPeriodId),
                CommodityTypeId = currentContract.CommodityTypeId,
                CommodityUsageTypeId = currentContract.CommodityUsageTypeId,
                CommodityCategoryId = currentContract.CommodityCategoryId,
                CountryId = currentContract.CountryId,
                DealerId = currentContract.DealerId,
                DealName = currentContract.DealName,
                DealType = currentContract.DealType,
                DiscountAvailable = currentContract.DiscountAvailable,
                EndDate = currentContract.EndDate,
                EntryDateTime = currentContract.EntryDateTime,
                EntryUser = currentContract.EntryUser,
                //  GrossPremium = premiumBasedOnCodeGross.ToLower() == "rp" ?
                //   currentContract.GrossPremium : currencyEM.ConvertFromBaseCurrency(currentContract.GrossPremium, currentExtentionInfo.PremiumCurrencyId, currentExtentionInfo.CurrencyPeriodId),
                Id = currentContract.Id,
                InsurerId = currentContract.InsurerId,
                IsActive = currentContract.IsActive,
                IsAutoRenewal = currentContract.IsAutoRenewal,
                IsPromotional = currentContract.IsPromotional,
                //  ItemStatusId = currentContract.ItemStatusId,
                //LiabilityLimitation = currencyEM.ConvertFromBaseCurrency(currentContract.LiabilityLimitation, currentExtentionInfo.PremiumCurrencyId, currentExtentionInfo.CurrencyPeriodId),
                LinkDealId = currentContract.LinkDealId,
                //  PremiumTotal = premiumBasedOnCodeNett.ToLower() == "rp" ?
                //   currentContract.PremiumTotal : currencyEM.ConvertFromBaseCurrency(currentContract.PremiumTotal, currentExtentionInfo.PremiumCurrencyId, currentExtentionInfo.CurrencyPeriodId),
                //  ProductId = currentContract.ProductId,
                // ReinsurerId = currentContract.ReinsurerId,
                Remark = currentContract.Remark,
                StartDate = currentContract.StartDate,
                //Commissions = GetCommisonListConverted(CommissionsList, currentExtentionInfo.PremiumCurrencyId, currentExtentionInfo.CurrencyPeriodId),
                Taxes = TaxesList,
                Eligibilities = EligibilityList
            };
            //adjust premium on additional premium
            Guid addonIdAdditionalPremium = Guid.Empty;
            var addonAdditionalPremium = premiumAddonTypes.Where(a => a.AddonTypeCode == "A").FirstOrDefault();
            if (addonAdditionalPremium != null)
            {
                addonIdAdditionalPremium = addonAdditionalPremium.Id;
            }

            //premium adjust on variant


            Guid addoneTypeIdFourByFour = Guid.Empty;
            Guid addoneTypeIdSports = Guid.Empty;
            var addonFourByFour = premiumAddonTypes.Where(a => a.AddonTypeCode == "F").FirstOrDefault();
            var addonSports = premiumAddonTypes.Where(a => a.AddonTypeCode == "S").FirstOrDefault();
            if (addonFourByFour != null)
            {
                addoneTypeIdFourByFour = addonFourByFour.Id;
            }

            if (addonSports != null)
            {
                addoneTypeIdSports = addonSports.Id;
            }

            foreach (ContractExtensionsPremiumAddon extAddon in ContractExtensionsPremiumAddon)
            {
                if (extAddon.PremiumType.ToLower() == "gross")
                {
                    if (variant != null)
                    {
                        //if (!variant.IsForuByFour)
                        //{
                        //    if (extAddon.PremiumAddonTypeId == addoneTypeIdFourByFour)
                        //    {
                        //        Contract.GrossPremium -= extAddon.Value;
                        //    }
                        //}
                        //if (!variant.IsSports)
                        //{
                        //    if (extAddon.PremiumAddonTypeId == addoneTypeIdSports)
                        //    {
                        //        Contract.GrossPremium -= extAddon.Value;
                        //    }
                        //}
                    }

                    if (model != null)
                    {
                        if (!model.AdditionalPremium)
                        {
                            Contract.GrossPremium -= extAddon.Value;
                        }
                    }


                }
                else
                {
                    if (variant != null)
                    {
                        //if (!variant.IsForuByFour)
                        //{
                        //    if (extAddon.PremiumAddonTypeId == addoneTypeIdFourByFour)
                        //    {
                        //        Contract.PremiumTotal -= extAddon.Value;
                        //    }
                        //}
                        //if (!variant.IsSports)
                        //{
                        //    if (extAddon.PremiumAddonTypeId == addoneTypeIdSports)
                        //    {
                        //        Contract.PremiumTotal -= extAddon.Value;
                        //    }
                        //}

                        if (model != null)
                        {
                            if (!model.AdditionalPremium)
                            {
                                Contract.PremiumTotal -= extAddon.Value;
                            }
                        }
                    }
                }
            }




            if (withTax)
            {
                decimal premiumBeforeTax = Contract.GrossPremium;
                //applying taxes for gross
                foreach (var tax in TaxesList.OrderBy(a => a.IndexVal).ToList())
                {
                    if (tax.IsOnNRP != true)
                    {
                        if (premiumBasedOnCodeGross.ToLower() == "rp")
                        {
                            Contract.GrossPremium += Math.Round((tax.IsPercentage ?
                                                                              (tax.TaxValue) :
                                                                              0) * 100) / 100;
                            if (tax.IsPercentage)
                            {
                                Contract.taxValue = (decimal)0.00;
                            }
                            else
                            {
                                //Contract.taxValue = currencyEM.ConvertFromBaseCurrency(tax.TaxValue,
                                //            currentExtentionInfo.PremiumCurrencyId,
                                //            currentExtentionInfo.CurrencyPeriodId);
                            }
                        }
                        else
                        {
                            Contract.GrossPremium += Math.Round((tax.IsPercentage ?
                                                 tax.IsOnPreviousTax ?
                                                     (Contract.GrossPremium * tax.TaxValue / 100) :
                                                     (premiumBeforeTax * tax.TaxValue / 100) :
                                                tax.TaxValue) * 100) / 100;
                        }

                    }
                }
            }

            return Contract;
        }





        private List<NRPCommissionContractMappingResponseDto> GetCommisonListConverted(List<NRPCommissionContractMappingResponseDto> CommissionsList, Guid PremiumCurrencyId, Guid CurrencyPeriodId)
        {
            CurrencyEntityManager cem = new CurrencyEntityManager();
            foreach (NRPCommissionContractMappingResponseDto commission in CommissionsList)
            {
                if (!commission.IsPercentage)
                {
                    commission.Commission = cem.ConvertFromBaseCurrency(commission.Commission, PremiumCurrencyId, CurrencyPeriodId);
                }
                else
                {
                    commission.Commission = Math.Round(commission.Commission * 100) / 100;
                }
            }
            return CommissionsList;
        }
        public List<ContractExtensionInfo> ContractExteintion(
                    IEnumerable<RSAAnualPremium> currentRSAAnualPremium,
                    IEnumerable<ContractExtensionsPremiumAddon> currentContractExtensionsPremiumAddon,
                    IEnumerable<ContractExtensions> currentExtentionInfo,
                    IEnumerable<ContractExtensionCylinderCount> ContractExtensionCylinderCount,
                    IEnumerable<ContractExtensionEngineCapacity> ContractExtensionEngineCapacity,
                    IEnumerable<ContractExtensionMake> ContractExtensionMake,
                    IEnumerable<ContractExtensionModel> ContractExtensionModel,
                    IEnumerable<ContractExtensionVariant> ContractExtensionVariant,
                    List<CountryTaxesResponseDto> TaxesList, bool withTax,
                    List<PremiumAddonType> premiumAddonTypes,
                    Entities.Variant variant, Entities.Model model,
                    IEnumerable<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddon)
        {
            CurrencyEntityManager currencyEM = new CurrencyEntityManager();
            ContractEntityManager contractEm = new ContractEntityManager();
            CommonEntityManager cem = new CommonEntityManager();

            //  decimal bal = currencyEM.ConvertFromBaseCurrency(decimal value,decimal currencyId);
            List<ContractExtensionInfo> ContractExtentionList = new List<ContractExtensionInfo>();
            foreach (var contractExtention in currentExtentionInfo)
            {
                string premiumBasedOnCodeGross = ""; //cem.GetPremiumBasedonCodeById(contractExtention.PremiumBasedOnIdGross);
                string premiumBasedOnCodeNett = "";//cem.GetPremiumBasedonCodeById(contractExtention.PremiumBasedOnIdNett);

                ContractExtensionInfo ContractExtention = new ContractExtensionInfo()
                {
                    AnualPremiums = RASAnnulaPremiums(currentRSAAnualPremium),
                    //  AttributeSpecification = contractExtention.AttributeSpecification,
                    // ContractId = contractExtention.ContractId,
                    CylinderCounts = CylinderCountToGuidList(ContractExtensionCylinderCount),
                    EngineCapacities = EngineCapacityToGuidList(ContractExtensionEngineCapacity),
                    EntryDateTime = contractExtention.EntryDateTime,
                    EntryUser = contractExtention.EntryUser,
                    //  ExtensionTypeId = contractExtention.ExtensionTypeId,
                    // GrossPremium = premiumBasedOnCodeGross.ToLower() == "rp" ?
                    // contractExtention.GrossPremium : currencyEM.ConvertFromBaseCurrency(contractExtention.GrossPremium, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    Id = contractExtention.Id,
                    IsContractExtensionsExists = true,
                    // IsCustAvailableNett = contractExtention.IsCustAvailableNett,
                    //IsCustAvailableGross = contractExtention.IsCustAvailableGross,
                    Makes = MakeToGuidList(ContractExtensionMake),
                    // ManufacturerWarrantyGross = contractExtention.ManufacturerWarrantyGross,
                    // ManufacturerWarrantyNett = contractExtention.ManufacturerWarrantyNett,
                    // MaxGross = currencyEM.ConvertFromBaseCurrency(contractExtention.MaxGross, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    // MaxNett = currencyEM.ConvertFromBaseCurrency(contractExtention.MaxNett, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    //MinGross = currencyEM.ConvertFromBaseCurrency(contractExtention.MinGross, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    // MinNett = currencyEM.ConvertFromBaseCurrency(contractExtention.MinNett, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    Modeles = ModelToGuidList(ContractExtensionModel),
                    Variants = VariantToGuidList(ContractExtensionVariant),
                    //PremiumAddones = PremiumAddon(currentContractExtensionsPremiumAddon, contractExtention.PremiumBasedOnIdGross,
                    //   contractExtention.PremiumBasedOnIdGross, contractExtention.PremiumCurrencyId,
                    //   contractExtention.CurrencyPeriodId, premiumBasedOnCodeNett, premiumBasedOnCodeGross),
                    //PremiumBasedOnIdGross = contractExtention.PremiumBasedOnIdGross,
                    //PremiumBasedOnIdNett = contractExtention.PremiumBasedOnIdNett,
                    //PremiumCurrencyId = contractExtention.PremiumCurrencyId,
                    //PremiumTotal = premiumBasedOnCodeNett.ToLower() == "rp" ?
                    //contractExtention.PremiumTotal : currencyEM.ConvertFromBaseCurrency(contractExtention.PremiumTotal, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    //Rate = currencyEM.ConvertFromBaseCurrency(contractExtention.Rate, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId),
                    RegionId = contractExtention.RegionId,
                    RSAProviderId = contractExtention.RSAProviderId,
                    //WarrantyTypeId = contractExtention.WarrantyTypeId,
                    isAllCyllinderCount = contractExtention.IsAllCyllinderCountsSelected,
                    isAllEngineCapacities = contractExtention.IsAllEngineCapacitiesSelected,
                    isAllMakes = contractExtention.IsAllMakesSelected,
                    isAllModels = contractExtention.IsAllModelsSelected,
                    isAllVariants = contractExtention.IsAllVariantsSelected

                };
                //adjust premium on additional premium
                Guid addonIdAdditionalPremium = Guid.Empty;
                var addonAdditionalPremium = premiumAddonTypes.Where(a => a.AddonTypeCode == "A").FirstOrDefault();
                if (addonAdditionalPremium != null)
                {
                    addonIdAdditionalPremium = addonAdditionalPremium.Id;
                }
                //premium adjust on variant
                // var contractExt = contractExtention.FirstOrDefault();
                Guid addoneTypeIdFourByFour = Guid.Empty;
                Guid addoneTypeIdSports = Guid.Empty;
                var addonFourByFour = premiumAddonTypes.Where(a => a.AddonTypeCode == "F").FirstOrDefault();
                var addonSports = premiumAddonTypes.Where(a => a.AddonTypeCode == "S").FirstOrDefault();
                if (addonFourByFour != null)
                {
                    addoneTypeIdFourByFour = addonFourByFour.Id;
                }

                if (addonSports != null)
                {
                    addoneTypeIdSports = addonSports.Id;
                }

                foreach (ContractExtensionsPremiumAddon extAddon in ContractExtensionsPremiumAddon)
                {
                    // currencyEM.ConvertFromBaseCurrency(extAddon.Value, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId);

                    if (extAddon.PremiumType.ToLower() == "gross")
                    {
                        if (premiumBasedOnCodeGross.ToLower() != "rp")
                        {
                            // extAddon.Value = currencyEM.ConvertFromBaseCurrency(extAddon.Value, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId);
                            if (variant != null)
                            {
                                //if (!variant.IsForuByFour)
                                //{
                                //    if (extAddon.PremiumAddonTypeId == addoneTypeIdFourByFour)
                                //    {
                                //        ContractExtention.GrossPremium -= extAddon.Value;
                                //    }
                                //}
                                //if (!variant.IsSports)
                                //{
                                //    if (extAddon.PremiumAddonTypeId == addoneTypeIdSports)
                                //    {
                                //        ContractExtention.GrossPremium -= extAddon.Value;
                                //    }
                                //}


                            }
                        }

                        if (model != null)
                        {
                            if (!model.AdditionalPremium)
                            {
                                ContractExtention.GrossPremium -= extAddon.Value;
                            }
                        }
                    }
                    else
                    {
                        if (premiumBasedOnCodeNett.ToLower() != "rp")
                        {
                            // extAddon.Value = currencyEM.ConvertFromBaseCurrency(extAddon.Value, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId);
                            if (variant != null)
                            {
                                //if (!variant.IsForuByFour)
                                //{
                                //    if (extAddon.PremiumAddonTypeId == addoneTypeIdFourByFour)
                                //    {
                                //        ContractExtention.PremiumTotal -= extAddon.Value;
                                //    }
                                //}
                                //if (!variant.IsSports)
                                //{
                                //    if (extAddon.PremiumAddonTypeId == addoneTypeIdSports)
                                //    {
                                //        ContractExtention.PremiumTotal -= extAddon.Value;
                                //    }
                                //}
                            }
                        }

                        if (model != null)
                        {
                            if (!model.AdditionalPremium)
                            {
                                ContractExtention.GrossPremium -= extAddon.Value;
                            }
                        }
                    }

                }

                if (withTax)
                {
                    decimal premiumBeforeTax = ContractExtention.GrossPremium;
                    //applying taxes for gross
                    foreach (var tax in TaxesList.OrderBy(a => a.IndexVal).ToList())
                    {
                        if (tax.IsOnNRP != true)
                        {
                            if (premiumBasedOnCodeGross.ToLower() == "rp")
                            {
                                if (tax.IsPercentage)
                                {
                                    ContractExtention.taxValue = (decimal)0.00;
                                }
                                else
                                {
                                    // ContractExtention.taxValue = currencyEM.ConvertFromBaseCurrency(tax.TaxValue, contractExtention.PremiumCurrencyId, contractExtention.CurrencyPeriodId);
                                }
                            }
                            else
                            {
                                //ContractExtention.GrossPremium += Math.Round((tax.IsPercentage ?
                                //                    tax.IsOnPreviousTax ?
                                //                        (ContractExtention.GrossPremium * tax.TaxValue / 100) :
                                //                        (premiumBeforeTax * tax.TaxValue / 100) :
                                //                    tax.TaxValue) * 100) / 100;
                                ContractExtention.GrossPremium += Math.Round((tax.IsPercentage ?
                                                 tax.IsOnPreviousTax ?
                                                     (ContractExtention.GrossPremium * tax.TaxValue / 100) :
                                                     (premiumBeforeTax * tax.TaxValue / 100) :
                                                 0) * 100) / 100;
                            }

                        }
                    }
                }

                ContractExtentionList.Add(ContractExtention);
            }


            return ContractExtentionList;
        }



        internal Customer CustomerEnterdTirePolicyDetailsCustomer(Customer_data customer_Data)
        {
            Customer response = new Customer();
            try
            {
                response = new Customer()
                {
                    Id = Guid.NewGuid(),
                    BusinessAddress1 = customer_Data.businessAddress1,
                    Address1 = customer_Data.address1,
                    Address2 = customer_Data.address2,
                    Address3 = customer_Data.address3,
                    Address4 = customer_Data.address4,
                    BusinessAddress2 = customer_Data.businessAddress2,
                    BusinessAddress3 = customer_Data.businessAddress3,
                    BusinessAddress4 = customer_Data.businessAddress4,
                    BusinessName = customer_Data.businessName,
                    BusinessTelNo = customer_Data.businessTelNo,
                    CityId = customer_Data.cityId,
                    CountryId = customer_Data.countryId,
                    CustomerTypeId = customer_Data.customerTypeId,
                    DateOfBirth = SqlDateTime.MinValue.Value ,
                    DLIssueDate = SqlDateTime.MinValue.Value,
                    Email = customer_Data.email,
                    EntryDateTime = DateTime.UtcNow,
                    FirstName = customer_Data.firstName,
                    IDTypeId = 1,
                    IDNo = string.Empty,
                    IsActive = false,
                    LastName = customer_Data.lastName,
                    MobileNo = customer_Data.mobileNo,
                    NationalityId = customer_Data.nationalityId,
                    OtherTelNo = customer_Data.otherTelNo,
                    Password = customer_Data.password,
                    UsageTypeId = customer_Data.usageTypeId,
                    UserName = customer_Data.email
                };
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal Customer CustomerEnterdTirePolicyDetailsToCustomer(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest)
        {
            ISession session = EntitySessionManager.GetSession();
            Customer response = new Customer();
            try
            {
                response = new Customer()
                {
                    Id = Guid.NewGuid(),
                    BusinessAddress1 = saveCustomerEnterdPolicyRequest.data.customer.businessAddress1,
                    Address1 = saveCustomerEnterdPolicyRequest.data.customer.address1,
                    Address2 = saveCustomerEnterdPolicyRequest.data.customer.address2,
                    Address3 = saveCustomerEnterdPolicyRequest.data.customer.address3,
                    Address4 = saveCustomerEnterdPolicyRequest.data.customer.address4,
                    BusinessAddress2 = saveCustomerEnterdPolicyRequest.data.customer.businessAddress2,
                    BusinessAddress3 = saveCustomerEnterdPolicyRequest.data.customer.businessAddress3,
                    BusinessAddress4 = saveCustomerEnterdPolicyRequest.data.customer.businessAddress4,
                    BusinessName = saveCustomerEnterdPolicyRequest.data.customer.businessName,
                    BusinessTelNo = saveCustomerEnterdPolicyRequest.data.customer.businessTelNo,
                    CityId = saveCustomerEnterdPolicyRequest.data.customer.cityId,
                    CountryId = saveCustomerEnterdPolicyRequest.data.customer.countryId,
                    CustomerTypeId = saveCustomerEnterdPolicyRequest.data.customer.customerTypeId,
                    DateOfBirth = saveCustomerEnterdPolicyRequest.data.customer.dateOfBirth == DateTime.MaxValue ? SqlDateTime.MinValue.Value : saveCustomerEnterdPolicyRequest.data.customer.dateOfBirth,
                    DLIssueDate = SqlDateTime.MinValue.Value,
                    Email = saveCustomerEnterdPolicyRequest.data.customer.email,
                    EntryDateTime = DateTime.UtcNow,
                    FirstName = saveCustomerEnterdPolicyRequest.data.customer.firstName,
                    IDTypeId = 1,
                    IDNo = string.Empty,
                    IsActive = false,
                    LastName = saveCustomerEnterdPolicyRequest.data.customer.lastName,
                    MobileNo = saveCustomerEnterdPolicyRequest.data.customer.mobileNo,
                    NationalityId = saveCustomerEnterdPolicyRequest.data.customer.nationalityId,
                    OtherTelNo = saveCustomerEnterdPolicyRequest.data.customer.otherTelNo,
                    Password = saveCustomerEnterdPolicyRequest.data.customer.password,
                    UsageTypeId = saveCustomerEnterdPolicyRequest.data.customer.usageTypeId,
                    UserName = saveCustomerEnterdPolicyRequest.data.customer.email
                };
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }




        internal List<OtherItemDetails> CustomerEnterdTirePolicyDetailsToOtherItemDetailsNew(Customer_data customer_Data,
                    Guid tpaId, Guid policyBundleId, Guid customerEnterdInvoiceDetailsId)
        {
            ISession session = EntitySessionManager.GetSession();
            List<OtherItemDetails> response = new List<OtherItemDetails>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetailsId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);


                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                     .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    List<Guid> contractIds = session.Query<Contract>()
                        .Where(a => a.CommodityTypeId == commodityType.CommodityTypeId &&
                        a.CountryId == invoiceCode.CountryId && a.ProductId == product.Id &&
                        a.DealerId == dealer.Id && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                        .Select(a => a.Id).ToList();

                    IList<Guid> allAvailableContractInsuranceLimitationIds = session.QueryOver<ContractInsuaranceLimitation>()
                        .WhereRestrictionOn(c => c.ContractId).IsIn(contractIds)
                        .Select(o => o.Id).List<Guid>();

                    IList<Guid> allAvailableContractExtensionIds = session.QueryOver<ContractExtensions>()
                       .WhereRestrictionOn(c => c.ContractInsuanceLimitationId).IsIn(allAvailableContractInsuranceLimitationIds.ToList())
                       .Select(o => o.Id).List<Guid>();

                    List<Guid> contractExtensionIdsForVariant = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == _invCodeDtl.VariantId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForModel = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == _invCodeDtl.ModelId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForProduct = session.Query<ContractExtensions>()
                    .Where(a => a.ProductId == product.Id).Select(a => a.Id).ToList();
                    //eligible extension ids
                    Guid eligibleExtensionId = contractExtensionIdsForVariant
                        .Intersect(contractExtensionIdsForProduct)
                        .Intersect(allAvailableContractExtensionIds)
                        .Intersect(contractExtensionIdsForModel).FirstOrDefault();

                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    Contract contract = session.Query<Contract>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.ContractId);
                    ItemStatus itemStatus = session.Query<ItemStatus>()
                        .FirstOrDefault(a => a.Status.ToLower() == "new");

                    if (contractExtensionPremium == null)
                    {
                        return response;
                    }


                    var otherItemDetails = new OtherItemDetails()
                    {
                        Id = Guid.NewGuid(),
                        MakeId = _invCodeDtl.MakeId,
                        ModelId = _invCodeDtl.ModelId,
                        VariantId = _invCodeDtl.VariantId,
                        DealerCurrencyId = dealer.CurrencyId,
                        AddnSerialNo = string.Empty,
                        DealerPrice = dealerPrice,
                        CategoryId = contract.CommodityCategoryId,
                        CommodityUsageTypeId = contract.CommodityUsageTypeId,
                        ConversionRate = contractExtensionPremium.ConversionRate,
                        currencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        EntryDateTime = DateTime.UtcNow,
                        ItemPrice = dealerPrice,
                        ItemPurchasedDate = invoiceCode.GeneratedDate,
                        ItemStatusId = itemStatus.Id,
                        InvoiceNo = customerEnterdInvoiceDetails.InvoiceNumber,
                    };
                    response.Add(otherItemDetails);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }


        internal List<Policy> CustomerEnterdTirePolicyDetailsToPolicyListNew(Customer_data customer_Data,
                     Guid tpaId, Guid policyBundleId, Guid customerEnterdInvoiceDetailsId)
        {
            ISession session = EntitySessionManager.GetSession();
            List<Policy> response = new List<Policy>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetailsId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);
                Guid commodityUsageTypeId = new CommonEntityManager().GetCommodityUsageTypeByName(customerEnterdInvoiceDetails.UsageTypeCode);

                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetail = session.Query<CustomerEnterdInvoiceTireDetails>()
                        .Where(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).FirstOrDefault();
                    var dealerPrice = customerEnterdInvoiceTireDetail.PurchasedPrice;
                    //var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                    // .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.InvoiceCodeDetailId).PurchasedPrice;
                    //get contract details
                    List<Guid> contractIds = session.Query<Contract>()
                        .Where(a => a.CommodityTypeId == commodityType.CommodityTypeId &&
                        a.CountryId == invoiceCode.CountryId && a.ProductId == product.Id &&
                        a.DealerId == dealer.Id && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow
                        && a.CommodityUsageTypeId == commodityUsageTypeId)
                        .Select(a => a.Id).ToList();

                    if (contractIds == null || contractIds.Count == 0)
                        throw new DealNotFoundException("Deal not found");

                    IList<Guid> allAvailableContractInsuranceLimitationIds = session.QueryOver<ContractInsuaranceLimitation>()
                        .WhereRestrictionOn(c => c.ContractId).IsIn(contractIds)
                        .Select(o => o.Id).List<Guid>();

                    IList<Guid> allAvailableContractExtensionIds = session.QueryOver<ContractExtensions>()
                       .WhereRestrictionOn(c => c.ContractInsuanceLimitationId).IsIn(allAvailableContractInsuranceLimitationIds.ToList())
                       .Select(o => o.Id).List<Guid>();


                    List<Guid> contractExtensionIdsForVariant = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == _invCodeDtl.VariantId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForModel = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == _invCodeDtl.ModelId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForProduct = session.Query<ContractExtensions>()
                    .Where(a => a.ProductId == product.Id).Select(a => a.Id).ToList();

                    if (contractExtensionIdsForVariant == null || contractExtensionIdsForVariant.Count == 0 ||
                        contractExtensionIdsForProduct == null || contractExtensionIdsForProduct.Count == 0 ||
                        allAvailableContractExtensionIds == null || allAvailableContractExtensionIds.Count == 0 ||
                        contractExtensionIdsForModel == null || contractExtensionIdsForModel.Count == 0)
                        throw new DealNotFoundException("Deal not found");


                    //eligible extension ids
                    Guid? eligibleExtensionId = contractExtensionIdsForVariant
                        .Intersect(contractExtensionIdsForProduct)
                        .Intersect(allAvailableContractExtensionIds)
                        .Intersect(contractExtensionIdsForModel)
                        .DefaultIfEmpty().First();


                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    if (contractExtensionPremium == null)
                        throw new DealNotFoundException("Deal not found");

                    var premiumDetails = new ContractEntityManager().GetPremium(contractExtensionPremium.Id,
                   decimal.Zero, Guid.Empty,
                   eligibleContractExtension.Id,
                   eligibleContractInsuaranceLimitation.ContractId, product.Id, dealer.Id,
                   DateTime.UtcNow,
                   Guid.Empty, Guid.Empty,
                   _invCodeDtl.MakeId, _invCodeDtl.ModelId,
                   _invCodeDtl.VariantId, decimal.Zero,
                   Guid.Empty, dealerPrice,
                   DateTime.UtcNow) as GetPremiumResponseDto;

                    if (premiumDetails == null)
                        throw new DealNotFoundException("Deal not found");

                    var policy = new Policy()
                    {
                        Id = Guid.NewGuid(),
                        Comment = string.Empty,
                        CommodityTypeId = commodityType.CommodityTypeId,
                        CustomerId = customer_Data.customerId,
                        CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerId = dealer.Id,
                        DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerLocationId = dealerLocation.Id,
                        EntryDateTime = DateTime.UtcNow,
                        TPABranchId = dealerLocation.TpaBranchId,
                        ProductId = product.Id,
                        IsApproved = false,
                        CurrencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        LocalCurrencyConversionRate = contractExtensionPremium.ConversionRate,
                        IsPolicyCanceled = false,
                        CustomerPayment = decimal.Zero,
                        DealerPolicy = true,
                        DealerPayment = decimal.Zero,
                        Discount = decimal.Zero,
                        DiscountPercentage = decimal.Zero,
                        ForwardComment = string.Empty,
                        IsPartialPayment = false,
                        MWIsAvailable = false,
                        PolicyBundleId = policyBundleId,
                        PaymentMethodFee = decimal.Zero,
                        PaymentMethodFeePercentage = decimal.Zero,
                        IsSpecialDeal = false,
                        RefNo = string.Empty,
                        ContractExtensionPremiumId = contractExtensionPremium.Id,
                        ContractExtensionsId = eligibleContractExtension.ContractInsuanceLimitationId,
                        ContractInsuaranceLimitationId = eligibleExtensionId.Value,
                        ContractId = eligibleContractInsuaranceLimitation.ContractId,
                        CoverTypeId = contractExtensionPremium.WarrentyTypeId,
                        ExtensionTypeId = eligibleExtensionId.Value,
                        HrsUsedAtPolicySale = "0",
                        IsPolicyRenewed = false,
                        IsPreWarrantyCheck = false,
                        MWStartDate = SqlDateTime.MinValue.Value,
                        PolicySoldDate = DateTime.UtcNow,
                        PolicyStartDate = DateTime.UtcNow,
                        PolicyEndDate = DateTime.UtcNow.AddMonths(insuaranceLimitation.Months),//fixed 1 year extension for tyre
                        PremiumCurrencyTypeId = dealer.CurrencyId,

                        Premium = premiumDetails.TotalPremium / contractExtensionPremium.ConversionRate,
                        GrossPremiumBeforeTax = (premiumDetails.BasicPremium + premiumDetails.EligibilityPremium),
                        TotalTax = premiumDetails.Tax,
                        NRP = premiumDetails.TotalPremiumNRP / contractExtensionPremium.ConversionRate,
                        EligibilityFee = premiumDetails.EligibilityPremium,

                        TransferFee = decimal.Zero,
                        PolicyNo = null

                    };
                    response.Add(policy);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }


        internal PolicyBundle CustomerEnterdTirePolicyDetailsToPolicyBundleNew(Customer_data customer_Data, InvoiceCode invoiceCode,
                Guid customerEnterdInvoiceDetailsId, Guid tpaId)
        {
            ISession session = EntitySessionManager.GetSession();
            PolicyBundle response = new PolicyBundle();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetailsId);
                InvoiceCode invoiceCodes = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                response = new PolicyBundle()
                {
                    Id = Guid.NewGuid(),
                    BookletNumber = string.Empty,
                    Comment = string.Empty,
                    CommodityTypeId = commodityType.CommodityTypeId,
                    CustomerId = customer_Data.customerId,
                    DealerId = invoiceCode.DealerId,
                    DealerLocationId = invoiceCode.DealerLocation,
                    EntryDateTime = DateTime.UtcNow,
                    IsApproved = false,
                    ProductId = product.Id,
                    DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                    CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                    MWStartDate = DateTime.MinValue,
                    PolicySoldDate = DateTime.UtcNow

                };

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }




        internal List<OtherItemPolicy> CustomerEnterdTirePolicyDetailsToOtherItemPolicyDetails(List<Policy> policies,
            List<OtherItemDetails> otherItemDetails)
        {
            ISession session = EntitySessionManager.GetSession();
            List<OtherItemPolicy> response = new List<OtherItemPolicy>();
            try
            {
                foreach (Policy policy in policies)
                {
                    foreach (OtherItemDetails otherItem in otherItemDetails)
                    {
                        ContractExtensionVariant contractExtensionVariant = session.Query<ContractExtensionVariant>()
                            .FirstOrDefault(a => a.ContractExtensionId == policy.ContractInsuaranceLimitationId
                            && a.VariantId == otherItem.VariantId);
                        if (contractExtensionVariant != null)
                        {
                            var otherItemPolicy = new OtherItemPolicy()
                            {
                                Id = Guid.NewGuid(),
                                OtherItemId = otherItem.Id,
                                PolicyId = policy.Id
                            };
                            response.Add(otherItemPolicy);
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal PolicyAttachment CustomerEnterdTirePolicyDetailsToPolicyAttachmentDetails(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest, Guid PolicyBundleId)
        {
            ISession session = EntitySessionManager.GetSession();
            PolicyAttachment response = new PolicyAttachment();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.data.tempInvId);

                UserAttachment userAttachment = session.Query<UserAttachment>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceAttachmentId);
                AttachmentSection attachmentSection = session.Query<AttachmentSection>()
                    .FirstOrDefault(a => a.AttachmentScreenName.ToLower() == "policyreg" &&
                    a.AttachmentSectionCode.ToLower() == "policy");
                AttachmentType attachmentType = session.Query<AttachmentType>()
                    .FirstOrDefault(a => a.AttachmentSectionId == attachmentSection.Id);
                UserAttachment userAttachment_save = new UserAttachment()
                {
                    Id = Guid.NewGuid(),
                    AttachmentFileName = userAttachment.AttachmentFileName,
                    AttachmentFileType = userAttachment.AttachmentFileType,
                    AttachmentSectionId = attachmentSection.Id,
                    AttachmentSizeKB = userAttachment.AttachmentSizeKB,
                    AttachmentTypeId = attachmentType.Id,
                    FileServerReference = userAttachment.FileServerReference,
                    UploadedDateTime = DateTime.UtcNow,
                    UserDefinedName = userAttachment.UserDefinedName
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(userAttachment_save, userAttachment_save.Id);
                    transaction.Commit();
                }


                response = new PolicyAttachment()
                {
                    Id = Guid.NewGuid(),
                    PolicyBundleId = PolicyBundleId,
                    UserAttachmentId = userAttachment_save.Id
                };
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal List<OtherItemDetails> CustomerEnterdTirePolicyDetailsToOtherItemDetails2(Customer_data customer_Data,
            TyrePolicySaveResponse tyrePolicySaveResponse, Guid tpaId, Guid id , List<ContractDetails> contractDetails)
        {
            ISession session = EntitySessionManager.GetSession();
            List<OtherItemDetails> response = new List<OtherItemDetails>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == tyrePolicySaveResponse.tempInvId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);


                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                     .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    ContractDetails contractDetail = contractDetails.Where(a => a.Position == _invCodeDtl.Position).FirstOrDefault();
                    //eligible extension ids
                    Guid? eligibleExtensionId = contractDetail.ContractExtensionsId;

                    //List<Guid> contractIds = session.Query<Contract>()
                    //    .Where(a => a.CommodityTypeId == commodityType.CommodityTypeId &&
                    //    a.CountryId == invoiceCode.CountryId && a.ProductId == product.Id &&
                    //    a.DealerId == dealer.Id && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                    //    .Select(a => a.Id).ToList();

                    //IList<Guid> allAvailableContractInsuranceLimitationIds = session.QueryOver<ContractInsuaranceLimitation>()
                    //    .WhereRestrictionOn(c => c.ContractId).IsIn(contractIds)
                    //    .Select(o => o.Id).List<Guid>();

                    //IList<Guid> allAvailableContractExtensionIds = session.QueryOver<ContractExtensions>()
                    //   .WhereRestrictionOn(c => c.ContractInsuanceLimitationId).IsIn(allAvailableContractInsuranceLimitationIds.ToList())
                    //   .Select(o => o.Id).List<Guid>();

                    //List<Guid> contractExtensionIdsForVariant = session.Query<ContractExtensionVariant>()
                    //    .Where(a => a.VariantId == _invCodeDtl.VariantId).Select(a => a.ContractExtensionId).ToList();

                    //List<Guid> contractExtensionIdsForModel = session.Query<ContractExtensionModel>()
                    //.Where(a => a.ModelId == _invCodeDtl.ModelId).Select(a => a.ContractExtensionId).ToList();

                    //List<Guid> contractExtensionIdsForProduct = session.Query<ContractExtensions>()
                    //.Where(a => a.ProductId == product.Id).Select(a => a.Id).ToList();
                    ////eligible extension ids
                    //Guid eligibleExtensionId = contractExtensionIdsForVariant
                    //    .Intersect(contractExtensionIdsForProduct)
                    //    .Intersect(allAvailableContractExtensionIds)
                    //    .Intersect(contractExtensionIdsForModel).FirstOrDefault();

                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    Contract contract = session.Query<Contract>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.ContractId);
                    ItemStatus itemStatus = session.Query<ItemStatus>()
                        .FirstOrDefault(a => a.Status.ToLower() == "new");

                    if (contractExtensionPremium == null)
                    {
                        return response;
                    }


                    var otherItemDetails = new OtherItemDetails()
                    {
                        Id = Guid.NewGuid(),
                        MakeId = _invCodeDtl.MakeId,
                        ModelId = _invCodeDtl.ModelId,
                        VariantId = _invCodeDtl.VariantId,
                        DealerCurrencyId = dealer.CurrencyId,
                        AddnSerialNo = string.Empty,
                        DealerPrice = dealerPrice,
                        CategoryId = contract.CommodityCategoryId,
                        CommodityUsageTypeId = contract.CommodityUsageTypeId,
                        ConversionRate = contractExtensionPremium.ConversionRate,
                        currencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        EntryDateTime = DateTime.UtcNow,
                        ItemPrice = dealerPrice,
                        ItemPurchasedDate = invoiceCode.GeneratedDate,
                        ItemStatusId = itemStatus.Id,
                        InvoiceNo = customerEnterdInvoiceDetails.InvoiceNumber,
                    };
                    response.Add(otherItemDetails);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal List<OtherItemDetails> CustomerEnterdTirePolicyDetailsToOtherItemDetails(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest, Guid id)
        {
            ISession session = EntitySessionManager.GetSession();
            List<OtherItemDetails> response = new List<OtherItemDetails>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.data.tempInvId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);


                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                     .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    List<Guid> contractIds = session.Query<Contract>()
                        .Where(a => a.CommodityTypeId == commodityType.CommodityTypeId &&
                        a.CountryId == invoiceCode.CountryId && a.ProductId == product.Id &&
                        a.DealerId == dealer.Id && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow)
                        .Select(a => a.Id).ToList();

                    IList<Guid> allAvailableContractInsuranceLimitationIds = session.QueryOver<ContractInsuaranceLimitation>()
                        .WhereRestrictionOn(c => c.ContractId).IsIn(contractIds)
                        .Select(o => o.Id).List<Guid>();

                    IList<Guid> allAvailableContractExtensionIds = session.QueryOver<ContractExtensions>()
                       .WhereRestrictionOn(c => c.ContractInsuanceLimitationId).IsIn(allAvailableContractInsuranceLimitationIds.ToList())
                       .Select(o => o.Id).List<Guid>();

                    List<Guid> contractExtensionIdsForVariant = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == _invCodeDtl.VariantId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForModel = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == _invCodeDtl.ModelId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForProduct = session.Query<ContractExtensions>()
                    .Where(a => a.ProductId == product.Id).Select(a => a.Id).ToList();
                    //eligible extension ids
                    Guid eligibleExtensionId = contractExtensionIdsForVariant
                        .Intersect(contractExtensionIdsForProduct)
                        .Intersect(allAvailableContractExtensionIds)
                        .Intersect(contractExtensionIdsForModel).FirstOrDefault();

                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    Contract contract = session.Query<Contract>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.ContractId);
                    ItemStatus itemStatus = session.Query<ItemStatus>()
                        .FirstOrDefault(a => a.Status.ToLower() == "new");

                    if (contractExtensionPremium == null)
                    {
                        return response;
                    }


                    var otherItemDetails = new OtherItemDetails()
                    {
                        Id = Guid.NewGuid(),
                        MakeId = _invCodeDtl.MakeId,
                        ModelId = _invCodeDtl.ModelId,
                        VariantId = _invCodeDtl.VariantId,
                        DealerCurrencyId = dealer.CurrencyId,
                        AddnSerialNo = string.Empty,
                        DealerPrice = dealerPrice,
                        CategoryId = contract.CommodityCategoryId,
                        CommodityUsageTypeId = contract.CommodityUsageTypeId,
                        ConversionRate = contractExtensionPremium.ConversionRate,
                        currencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        EntryDateTime = DateTime.UtcNow,
                        ItemPrice = dealerPrice,
                        ItemPurchasedDate = invoiceCode.GeneratedDate,
                        ItemStatusId = itemStatus.Id,
                        InvoiceNo = customerEnterdInvoiceDetails.InvoiceNumber,
                    };
                    response.Add(otherItemDetails);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal List<Policy> CustomerEnterdTirePolicyDetailsToPolicyList2(Customer_data customer_Data,
            TyrePolicySaveResponse tyrePolicySaveResponse, Guid tpaId, Guid policyBundleId , List<ContractDetails> contractDetails)
        {
            ISession session = EntitySessionManager.GetSession();
            List<Policy> response = new List<Policy>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == tyrePolicySaveResponse.tempInvId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);
                Guid commodityUsageTypeId = new CommonEntityManager().GetCommodityUsageTypeByName(customerEnterdInvoiceDetails.UsageTypeCode);

                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                     .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    ContractDetails contractDetail = contractDetails.Where(a => a.Position == _invCodeDtl.Position).FirstOrDefault();
                    //eligible extension ids
                    Guid? eligibleExtensionId = contractDetail.ContractExtensionsId;

                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    if (contractExtensionPremium == null)
                        throw new DealNotFoundException("Deal not found");


                    var premiumDetails = new ContractEntityManager().GetPremium(contractExtensionPremium.Id,
                 decimal.Zero, Guid.Empty,
                 eligibleContractExtension.Id,
                 eligibleContractInsuaranceLimitation.ContractId, product.Id, dealer.Id,
                  contractDetail.purchaseDate,
                   Guid.Empty, Guid.Empty,
                   _invCodeDtl.MakeId, _invCodeDtl.ModelId,
                   _invCodeDtl.VariantId, decimal.Zero,
                   Guid.Empty, dealerPrice,
                    contractDetail.purchaseDate) as GetPremiumResponseDto;

                    if (premiumDetails == null)
                        throw new DealNotFoundException("Deal not found");

                    var policy = new Policy()
                    {
                        Id = Guid.NewGuid(),
                        Comment = string.Empty,
                        CommodityTypeId = commodityType.CommodityTypeId,
                        CustomerId = customer_Data.customerId,
                        CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerId = dealer.Id,
                        DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerLocationId = dealerLocation.Id,
                        EntryDateTime = DateTime.UtcNow,
                        TPABranchId = dealerLocation.TpaBranchId,
                        ProductId = product.Id,
                        IsApproved = false,
                        CurrencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        LocalCurrencyConversionRate = contractExtensionPremium.ConversionRate,
                        IsPolicyCanceled = false,
                        CustomerPayment = decimal.Zero,
                        DealerPolicy = true,
                        DealerPayment = decimal.Zero,
                        Discount = decimal.Zero,
                        DiscountPercentage = decimal.Zero,
                        ForwardComment = string.Empty,
                        IsPartialPayment = false,
                        MWIsAvailable = false,
                        PolicyBundleId = policyBundleId,
                        PaymentMethodFee = decimal.Zero,
                        PaymentMethodFeePercentage = decimal.Zero,
                        IsSpecialDeal = false,
                        RefNo = string.Empty,
                        ContractExtensionPremiumId = contractExtensionPremium.Id,
                        ContractExtensionsId = eligibleContractExtension.ContractInsuanceLimitationId,
                        ContractInsuaranceLimitationId = eligibleExtensionId.Value,
                        ContractId = eligibleContractInsuaranceLimitation.ContractId,
                        CoverTypeId = contractExtensionPremium.WarrentyTypeId,
                        ExtensionTypeId = eligibleExtensionId.Value,
                        HrsUsedAtPolicySale = "0",
                        IsPolicyRenewed = false,
                        IsPreWarrantyCheck = false,
                        MWStartDate = SqlDateTime.MinValue.Value,
                        PolicySoldDate = contractDetail.purchaseDate,
                        PolicyStartDate = contractDetail.purchaseDate,
                        PolicyEndDate = contractDetail.purchaseDate.AddMonths(insuaranceLimitation.Months),//fixed 1 year extension for tyre
                        PremiumCurrencyTypeId = dealer.CurrencyId,

                        Premium = premiumDetails.TotalPremium / contractExtensionPremium.ConversionRate,
                        GrossPremiumBeforeTax = (premiumDetails.BasicPremium + premiumDetails.EligibilityPremium),
                        TotalTax = premiumDetails.Tax,
                        NRP = premiumDetails.TotalPremiumNRP / contractExtensionPremium.ConversionRate,
                        EligibilityFee = premiumDetails.EligibilityPremium,

                        TransferFee = decimal.Zero,
                        PolicyNo = null

                    };
                    response.Add(policy);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal List<Policy> CustomerEnterdTirePolicyDetailsToPolicyList(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest,
            Guid policyBundleId)
        {
            ISession session = EntitySessionManager.GetSession();
            List<Policy> response = new List<Policy>();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.data.tempInvId);
                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>()
                    .FirstOrDefault(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);
                Guid commodityUsageTypeId = new CommonEntityManager().GetCommodityUsageTypeByName(customerEnterdInvoiceDetails.UsageTypeCode);

                foreach (InvoiceCodeDetails _invCodeDtl in invoiceCodeDetails)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = session.Query<CustomerEnterdInvoiceTireDetails>()
                     .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    List<Guid> contractIds = session.Query<Contract>()
                        .Where(a => a.CommodityTypeId == commodityType.CommodityTypeId &&
                        a.CountryId == invoiceCode.CountryId && a.ProductId == product.Id &&
                        a.DealerId == dealer.Id && a.StartDate <= DateTime.UtcNow && a.EndDate >= DateTime.UtcNow
                        && a.CommodityUsageTypeId == commodityUsageTypeId)
                        .Select(a => a.Id).ToList();

                    if (contractIds == null || contractIds.Count == 0)
                        throw new DealNotFoundException("Deal not found");

                    IList<Guid> allAvailableContractInsuranceLimitationIds = session.QueryOver<ContractInsuaranceLimitation>()
                        .WhereRestrictionOn(c => c.ContractId).IsIn(contractIds)
                        .Select(o => o.Id).List<Guid>();

                    IList<Guid> allAvailableContractExtensionIds = session.QueryOver<ContractExtensions>()
                       .WhereRestrictionOn(c => c.ContractInsuanceLimitationId).IsIn(allAvailableContractInsuranceLimitationIds.ToList())
                       .Select(o => o.Id).List<Guid>();


                    List<Guid> contractExtensionIdsForVariant = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == _invCodeDtl.VariantId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForModel = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == _invCodeDtl.ModelId).Select(a => a.ContractExtensionId).ToList();

                    List<Guid> contractExtensionIdsForProduct = session.Query<ContractExtensions>()
                    .Where(a => a.ProductId == product.Id).Select(a => a.Id).ToList();

                    if (contractExtensionIdsForVariant == null || contractExtensionIdsForVariant.Count == 0 ||
                        contractExtensionIdsForProduct == null || contractExtensionIdsForProduct.Count == 0 ||
                        allAvailableContractExtensionIds == null || allAvailableContractExtensionIds.Count == 0 ||
                        contractExtensionIdsForModel == null || contractExtensionIdsForModel.Count == 0)
                        throw new DealNotFoundException("Deal not found");


                    //eligible extension ids
                    Guid? eligibleExtensionId = contractExtensionIdsForVariant
                        .Intersect(contractExtensionIdsForProduct)
                        .Intersect(allAvailableContractExtensionIds)
                        .Intersect(contractExtensionIdsForModel)
                        .DefaultIfEmpty().First();


                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                      .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    if (contractExtensionPremium == null)
                        throw new DealNotFoundException("Deal not found");

                    var premiumDetails = new ContractEntityManager().GetPremium(contractExtensionPremium.Id,
                   decimal.Zero, Guid.Empty,
                   eligibleContractExtension.Id,
                   eligibleContractInsuaranceLimitation.ContractId, product.Id, dealer.Id,
                   DateTime.UtcNow,
                   Guid.Empty, Guid.Empty,
                   _invCodeDtl.MakeId, _invCodeDtl.ModelId,
                   _invCodeDtl.VariantId, decimal.Zero,
                   Guid.Empty, dealerPrice,
                   DateTime.UtcNow) as GetPremiumResponseDto;

                    if (premiumDetails == null)
                        throw new DealNotFoundException("Deal not found");

                    var policy = new Policy()
                    {
                        Id = Guid.NewGuid(),
                        Comment = string.Empty,
                        CommodityTypeId = commodityType.CommodityTypeId,
                        CustomerId = saveCustomerEnterdPolicyRequest.data.customer.customerId,
                        CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerId = dealer.Id,
                        DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerLocationId = dealerLocation.Id,
                        EntryDateTime = DateTime.UtcNow,
                        TPABranchId = dealerLocation.TpaBranchId,
                        ProductId = product.Id,
                        IsApproved = false,
                        CurrencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        LocalCurrencyConversionRate = contractExtensionPremium.ConversionRate,
                        IsPolicyCanceled = false,
                        CustomerPayment = decimal.Zero,
                        DealerPolicy = true,
                        DealerPayment = decimal.Zero,
                        Discount = decimal.Zero,
                        DiscountPercentage = decimal.Zero,
                        ForwardComment = string.Empty,
                        IsPartialPayment = false,
                        MWIsAvailable = false,
                        PolicyBundleId = policyBundleId,
                        PaymentMethodFee = decimal.Zero,
                        PaymentMethodFeePercentage = decimal.Zero,
                        IsSpecialDeal = false,
                        RefNo = string.Empty,
                        ContractExtensionPremiumId = contractExtensionPremium.Id,
                        ContractExtensionsId = eligibleContractExtension.ContractInsuanceLimitationId,
                        ContractInsuaranceLimitationId = eligibleExtensionId.Value,
                        ContractId = eligibleContractInsuaranceLimitation.ContractId,
                        CoverTypeId = contractExtensionPremium.WarrentyTypeId,
                        ExtensionTypeId = eligibleExtensionId.Value,
                        HrsUsedAtPolicySale = "0",
                        IsPolicyRenewed = false,
                        IsPreWarrantyCheck = false,
                        MWStartDate = SqlDateTime.MinValue.Value,
                        PolicySoldDate = DateTime.UtcNow,
                        PolicyStartDate = DateTime.UtcNow,
                        PolicyEndDate = DateTime.UtcNow.AddMonths(insuaranceLimitation.Months),//fixed 1 year extension for tyre
                        PremiumCurrencyTypeId = dealer.CurrencyId,

                        Premium = premiumDetails.TotalPremium / contractExtensionPremium.ConversionRate,
                        GrossPremiumBeforeTax = (premiumDetails.BasicPremium + premiumDetails.EligibilityPremium),
                        TotalTax = premiumDetails.Tax,
                        NRP = premiumDetails.TotalPremiumNRP / contractExtensionPremium.ConversionRate,
                        EligibilityFee = premiumDetails.EligibilityPremium,

                        TransferFee = decimal.Zero,
                        PolicyNo = null

                    };
                    response.Add(policy);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal PolicyBundle CustomerEnterdTirePolicyDetailsToPolicyBundle2(Customer_data customer_Data, TyrePolicySaveResponse tyrePolicySaveResponse, Guid tpaId)
        {
            ISession session = EntitySessionManager.GetSession();
            PolicyBundle response = new PolicyBundle();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == tyrePolicySaveResponse.tempInvId);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                response = new PolicyBundle()
                {
                    Id = Guid.NewGuid(),
                    BookletNumber = string.Empty,
                    Comment = string.Empty,
                    CommodityTypeId = commodityType.CommodityTypeId,
                    CustomerId = customer_Data.customerId,
                    DealerId = invoiceCode.DealerId,
                    DealerLocationId = invoiceCode.DealerLocation,
                    EntryDateTime = DateTime.UtcNow,
                    IsApproved = false,
                    ProductId = product.Id,
                    DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                    CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                    MWStartDate = DateTime.MinValue,
                    PolicySoldDate = DateTime.UtcNow

                };

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal PolicyBundle CustomerEnterdTirePolicyDetailsToPolicyBundle(SaveCustomerEnterdPolicyRequestDto saveCustomerEnterdPolicyRequest)
        {
            ISession session = EntitySessionManager.GetSession();
            PolicyBundle response = new PolicyBundle();
            try
            {
                CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                    .FirstOrDefault(a => a.Id == saveCustomerEnterdPolicyRequest.data.tempInvId);
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == customerEnterdInvoiceDetails.InvoiceCodeId);
                CommodityType commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                Product product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");
                Dealer dealer = session.Query<Dealer>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerId);
                response = new PolicyBundle()
                {
                    Id = Guid.NewGuid(),
                    BookletNumber = string.Empty,
                    Comment = string.Empty,
                    CommodityTypeId = commodityType.CommodityTypeId,
                    CustomerId = saveCustomerEnterdPolicyRequest.data.customer.customerId,
                    DealerId = invoiceCode.DealerId,
                    DealerLocationId = invoiceCode.DealerLocation,
                    EntryDateTime = DateTime.UtcNow,
                    IsApproved = false,
                    ProductId = product.Id,
                    DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                    CustomerPaymentCurrencyTypeId = dealer.CurrencyId
                };

            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private List<Guid> VariantToGuidList(IEnumerable<ContractExtensionVariant> ContractExtensionVariant)
        {
            List<Guid> Respones = new List<Guid>();
            foreach (var item in ContractExtensionVariant)
            {
                Respones.Add(item.VariantId);
            }
            return Respones;
        }



        private List<Guid> ModelToGuidList(IEnumerable<ContractExtensionModel> ContractExtensionModel)
        {
            List<Guid> Respones = new List<Guid>();
            foreach (var item in ContractExtensionModel)
            {
                Respones.Add(item.ModelId);
            }
            return Respones;
        }

        private List<Guid> MakeToGuidList(IEnumerable<ContractExtensionMake> ContractExtensionMake)
        {
            List<Guid> Respones = new List<Guid>();
            foreach (var item in ContractExtensionMake)
            {
                Respones.Add(item.MakeId);
            }
            return Respones;
        }

        private List<Guid> EngineCapacityToGuidList(IEnumerable<ContractExtensionEngineCapacity> ContractExtensionEngineCapacity)
        {
            List<Guid> Respones = new List<Guid>();
            foreach (var item in ContractExtensionEngineCapacity)
            {
                Respones.Add(item.EngineCapacityId);
            }
            return Respones;
        }

        private List<Guid> CylinderCountToGuidList(IEnumerable<ContractExtensionCylinderCount> ContractExtensionCylinderCount)
        {
            List<Guid> Respones = new List<Guid>();
            foreach (var item in ContractExtensionCylinderCount)
            {
                Respones.Add(item.CylinderCountId);
            }
            return Respones;
        }
        public List<RSAAnualPremiumResponseDto> RASAnnulaPremiums(IEnumerable<RSAAnualPremium> currentRSAAnualPremium)
        {
            List<RSAAnualPremiumResponseDto> AnnualPremiumList = new List<RSAAnualPremiumResponseDto>();
            foreach (var annulPremium in currentRSAAnualPremium)
            {
                RSAAnualPremiumResponseDto AnnualPremium = new RSAAnualPremiumResponseDto()
                {
                    ContractExtensionId = annulPremium.ContractExtensionId,
                    Id = annulPremium.Id,
                    IsRSAAnualPremiumExists = true,
                    Value = annulPremium.Value,
                    Year = annulPremium.Year
                };
                AnnualPremiumList.Add(AnnualPremium);
            }
            return AnnualPremiumList;
        }
        public List<ContractExtensionsPremiumAddonResponseDto> PremiumAddon(IEnumerable<ContractExtensionsPremiumAddon> currentContractExtensionsPremiumAddon,
            Guid premiumAddonIdGross, Guid premiumAddonIdNett, Guid PremiumCurrencyId, Guid CurrencyPeriodId, string premiumBasedOnCodeNett, string premiumBasedOnCodeGross)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            ContractEntityManager contracEm = new ContractEntityManager();

            List<ContractExtensionsPremiumAddonResponseDto> PremiumAddonList = new List<ContractExtensionsPremiumAddonResponseDto>();
            foreach (var premiumAddon in currentContractExtensionsPremiumAddon)
            {
                decimal currentAddonValue = Decimal.Zero;
                if (premiumAddon.PremiumType.ToLower().StartsWith("gr"))
                {
                    if (premiumBasedOnCodeGross.ToLower() == "rp")
                    {
                        currentAddonValue = Math.Round(premiumAddon.Value * 100) / 100;
                    }
                    else
                    {
                        currentAddonValue = currencyEm.ConvertFromBaseCurrency(premiumAddon.Value, PremiumCurrencyId, CurrencyPeriodId);
                    }
                }
                else
                {
                    if (premiumBasedOnCodeNett.ToLower() == "rp")
                    {
                        currentAddonValue = Math.Round(premiumAddon.Value * 100) / 100;
                    }
                    else
                    {
                        currentAddonValue = currencyEm.ConvertFromBaseCurrency(premiumAddon.Value, PremiumCurrencyId, CurrencyPeriodId);
                    }
                }
                ContractExtensionsPremiumAddonResponseDto PremiumAddon = new ContractExtensionsPremiumAddonResponseDto()
                {
                    ContractExtensionId = premiumAddon.ContractExtensionId,
                    Id = premiumAddon.Id,
                    IsContractExtensionsPremiumAddonExists = true,
                    PremiumAddonTypeId = premiumAddon.PremiumAddonTypeId,
                    Value = currentAddonValue,
                    PremiumType = premiumAddon.PremiumType
                };
                PremiumAddonList.Add(PremiumAddon);
            }
            return PremiumAddonList;
        }

        internal ExtensionTypeResponseDto ExtentionType(IEnumerable<ExtensionType> ExtentionType)
        {
            if (ExtentionType == null || ExtentionType.FirstOrDefault() == null)
            {
                return new ExtensionTypeResponseDto();
            }

            ExtensionTypeResponseDto ExtensionTypeResponseDto = new ExtensionTypeResponseDto()
            {
                CommodityTypeId = ExtentionType.FirstOrDefault().CommodityTypeId,
                EntryDateTime = ExtentionType.FirstOrDefault().EntryDateTime,
                EntryUser = ExtentionType.FirstOrDefault().EntryUser,
                ExtensionName = ExtentionType.FirstOrDefault().ExtensionName,
                Hours = ExtentionType.FirstOrDefault().Hours,
                Id = ExtentionType.FirstOrDefault().Id,
                IsExtensionTypeExists = true,
                Km = ExtentionType.FirstOrDefault().Km,
                Month = ExtentionType.FirstOrDefault().Month,
                ProductId = ExtentionType.FirstOrDefault().ProductId,
            };
            return ExtensionTypeResponseDto;
        }



        internal CustomerRequestDto PolicyCustomerToCustomerEntity(DataTransfer.Requests.Customer_ customer)
        {
            var Customer = new CustomerRequestDto()
            {
                Address1 = customer.address1,
                Address2 = customer.address2,
                Address3 = customer.address3,
                Address4 = customer.address4,
                BusinessAddress1 = customer.businessAddress1,
                BusinessAddress2 = customer.businessAddress2,
                BusinessAddress3 = customer.businessAddress3,
                BusinessAddress4 = customer.businessAddress4,
                BusinessName = customer.businessName,
                BusinessTelNo = customer.businessTelNo,
                CityId = customer.cityId,
                CountryId = customer.countryId,
                CustomerTypeId = customer.customerTypeId,
                DateOfBirth = customer.dateOfBirth,
                DLIssueDate = customer.idIssueDate,
                Email = customer.email,
                FirstName = customer.firstName,
                Gender = customer.gender,
                Id = customer.customerId.ToString(),
                IDNo = customer.idNo,
                IDTypeId = customer.idTypeId,
                LastModifiedDateTime = DateTime.Now,
                LastName = customer.lastName,
                MobileNo = customer.mobileNo,
                NationalityId = customer.nationalityId,
                OtherTelNo = customer.otherTelNo,
                UsageTypeId = customer.usageTypeId,
                IsActive = true
            };
            return Customer;
        }

        internal VehicleDetailsRequestDto PolicyVehicleToVehicleEntity(DataTransfer.Requests.Product_ product, Policy_ policy_)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            //CommonEntityManager commenEm = new CommonEntityManager();
            Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(policy_.productContracts.FirstOrDefault().ContractId);

            var vehicleDetails = new VehicleDetailsRequestDto()
            {
                AspirationId = product.aspirationTypeId,
                BodyTypeId = product.bodyTypeId,
                CategoryId = product.categoryId,
                CommodityUsageTypeId = product.commodityUsageTypeId,
                CylinderCountId = product.cylinderCountId,
                DealerPrice = product.dealerPrice,
                EngineCapacityId = product.engineCapacityId,
                FuelTypeId = product.fuelTypeId,
                Id = product.id,
                ItemPurchasedDate = product.itemPurchasedDate,
                ItemStatusId = product.itemStatusId,
                MakeId = product.makeId,
                ModelId = product.modelId,
                ModelYear = product.modelYear,
                PlateNo = product.additionalSerial,
                TransmissionId = product.transmissionTypeId,
                Variant = product.variantId,
                VehiclePrice = product.itemPrice,
                VINNo = product.serialNumber,
                DealerCurrencyId = product.dealerPaymentCurrencyTypeId,
                currencyPeriodId = relevantCurrencyPeriodId,
                DealerId = product.dealerId,
                RegistrationDate = product.registrationDate,
                GrossWeight = product.grossWeight,
                DriveTypeId = product.driveTypeId,
                EngineNumber = product.engineNumber
                //CountryId = commenEm.GetDealerCountryByLocationId(product.dealerLocationId)
            };
            return vehicleDetails;
        }

        internal BrownAndWhiteDetailsRequestDto PolicyBnWToBnWEntity(DataTransfer.Requests.Product_ product, Policy_ policy_)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(policy_.productContracts.FirstOrDefault().ContractId);

            var electronicDetails = new BrownAndWhiteDetailsRequestDto()
            {
                InvoiceNo = product.invoiceNo,
                ItemPrice = product.itemPrice,
                CategoryId = product.categoryId,
                CommodityUsageTypeId = product.commodityUsageTypeId,
                DealerPrice = product.dealerPrice,
                Id = product.id,
                ItemPurchasedDate = product.itemPurchasedDate,
                ItemStatusId = product.itemStatusId,
                MakeId = product.makeId,
                ModelId = product.modelId,
                ModelYear = product.modelYear,
                SerialNo = product.serialNumber,
                AddnSerialNo = product.additionalSerial,
                DealerCurrencyId = product.dealerPaymentCurrencyTypeId,
                DealerId = product.dealerId,
            };
            return electronicDetails;
        }

        internal PolicyBundleRequestDto PolicyDetailsToPolicyBundle(Policy_ policy_, Payment_ payment_, Product_ product_, Guid bundleId)
        {
            CurrencyEntityManager currencyEM = new CurrencyEntityManager();
            Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(policy_.productContracts.FirstOrDefault().ContractId);
            var policyBundle = new PolicyBundleRequestDto()
            {
                Comment = payment_.comment,
                CommodityTypeId = product_.commodityTypeId,
                ContractId = Guid.Empty,
                //CoverTypeId = Guid.Empty,
                CustomerId = Guid.Empty,
                CustomerPayment = currencyEM.ConvertToBaseCurrency(payment_.customerPayment, product_.customerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                CustomerPaymentCurrencyTypeId = product_.customerPaymentCurrencyTypeId,
                DealerId = product_.dealerId,
                DealerLocationId = product_.dealerLocationId,
                DealerPayment = currencyEM.ConvertToBaseCurrency(payment_.dealerPayment, product_.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                DealerPaymentCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                DealerPolicy = policy_.dealerPolicy,
                Discount = currencyEM.ConvertToBaseCurrency(payment_.discount, product_.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                EntryDateTime = DateTime.Now,
                EntryUser = Guid.Empty,
                PaymentTypeId = payment_.paymentTypeId,
                //ExtensionTypeId = Guid.Empty,
                HrsUsedAtPolicySale = policy_.hrsUsedAtPolicySale,
                IsApproved = new DealerEntityManager().GetDealerById(product_.dealerId).IsAutoApproval,
                IsPartialPayment = payment_.isPartialPayment,
                IsPolicyCanceled = false,
                IsPreWarrantyCheck = false,
                IsSpecialDeal = payment_.isSpecialDeal,
                ItemId = Guid.Empty,
                PaymentModeId = payment_.paymentModeId,
                PolicyEndDate = SqlDateTime.MinValue.Value,
                PolicyNo = null,
                PolicySoldDate = policy_.policySoldDate,
                PolicyStartDate = SqlDateTime.MinValue.Value,
                Premium = currencyEM.ConvertToBaseCurrency(policy_.premium, product_.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                PremiumCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                ProductId = product_.productId,
                RefNo = payment_.refNo,
                SalesPersonId = policy_.salesPersonId,
                BookletNumber = policy_.productContracts.FirstOrDefault().BookletNumber,
                MWStartDate = product_.MWStartDate,
                CoverTypeId = policy_.productContracts.FirstOrDefault().CoverTypeId,
                ExtensionTypeId = policy_.productContracts.FirstOrDefault().ExtensionTypeId,
                AttributeSpecificationId = policy_.productContracts.FirstOrDefault().AttributeSpecificationId



            };

            if (bundleId != Guid.Empty)
            {
                //PolicyBundleResponseDto policyBundleDB = new PolicyEntityManager().GetPolicyBundleById(bundleId);
                //policyBundle.EntryDateTime = policyBundleDB.EntryDateTime;
                //policyBundle.EntryUser = policyBundleDB.EntryUser;


            }
            return policyBundle;
        }

        internal List<PolicyRequestDto> PolicyDetailsToPolicy(Policy_ policy_, Payment_ payment_, Product_ product_)
        {
            var policyList = new List<PolicyRequestDto>();

            CurrencyEntityManager currencyEM = new CurrencyEntityManager();
            ContractEntityManager contractEnitiyManager = new ContractEntityManager();
            Decimal paymentMethodPercentage = Decimal.Zero;
            if (payment_.paymentTypeId != Guid.Empty)
            {
                paymentMethodPercentage = new CommonEntityManager().GetPaymentMethodPercentageByPaymentMethodId(payment_.paymentTypeId);
            }

            Guid getCurrencyPeriodId = currencyEM.GetCurrentCurrencyPeriodId();
            decimal rate = currencyEM.GetConversionRate(product_.dealerPaymentCurrencyTypeId,
            getCurrencyPeriodId, true);


            foreach (ProductContract_ contract in policy_.productContracts)
            {


                var premiumDetails = new ContractEntityManager().GetPremium(contract.CoverTypeId,
                   decimal.Parse(policy_.hrsUsedAtPolicySale), contract.AttributeSpecificationId,
                   contract.ExtensionTypeId,
                   contract.ContractId, product_.productId, product_.dealerId,
                   policy_.policySoldDate,
                   product_.cylinderCountId, product_.engineCapacityId,
                   product_.makeId, product_.modelId,
                   product_.variantId, product_.grossWeight,
                   product_.itemStatusId, product_.dealerPrice,
                   product_.itemPurchasedDate) as GetPremiumResponseDto;

                Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(contract.ContractId);
                string PremiumBasedOnGross = contractEnitiyManager.GetPremimBasedOnCodeByContractId(contract.ContractId, "GROSS");
                string PremiumBasedOnNett = contractEnitiyManager.GetPremimBasedOnCodeByContractId(contract.ContractId, "NETT");
                decimal BasicPremium = GetBasicGross(contract.ContractId, product_.dealerPrice, product_.variantId, PremiumBasedOnGross, product_.modelId);
                decimal EligibilityFee = GetEligibilityFee(BasicPremium, contract.ContractId, policy_.policySoldDate, decimal.Parse(policy_.hrsUsedAtPolicySale));
                decimal BasicNRP = GetBasicNRP(contract.ContractId, product_.dealerPrice, product_.variantId, PremiumBasedOnNett);
                decimal TotalTax = GetTotalTax(BasicPremium, contract.ContractId);
                var policy = new PolicyRequestDto()
                {
                    Id = contract.Id,
                    ContractId = contract.ContractId,
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = Guid.Empty,
                    BordxId = Guid.Empty,
                    Comment = string.Empty,
                    CommodityTypeId = product_.commodityTypeId,
                    ContractExtensionPremiumId = contract.CoverTypeId,
                    ContractExtensionsId = contract.ExtensionTypeId,
                    ContractInsuaranceLimitationId = contract.AttributeSpecificationId,
                    CustomerId = Guid.Empty,
                    CurrencyPeriodId = relevantCurrencyPeriodId,
                    CustomerPayment = currencyEM.ConvertToBaseCurrency(payment_.customerPayment, product_.customerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                    CustomerPaymentCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                    DealerId = product_.dealerId,
                    DealerLocationId = product_.dealerLocationId,
                    DealerPayment = currencyEM.ConvertToBaseCurrency(payment_.dealerPayment, product_.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                    DealerPolicy = policy_.dealerPolicy,
                    DealerPaymentCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                    Discount = currencyEM.ConvertToBaseCurrency(payment_.discount, contract.PremiumCurrencyTypeId, relevantCurrencyPeriodId),
                    DiscountPercentage = Convert.ToInt16(payment_.discount),
                    EligibilityFee = EligibilityFee,
                    CoverTypeId = contract.CoverTypeId,
                    ExtensionTypeId = contract.ExtensionTypeId,
                    ForwardComment = string.Empty,
                    NRP = BasicNRP,
                    ProductId = contract.ProductId,
                    //Premium = premiumDetails.TotalPremium / rate,
                    Premium = contract.Premium,
                    Year = 0,
                    SalesPersonId = policy_.salesPersonId,
                    IsSpecialDeal = payment_.isSpecialDeal,
                    IsApproved = new DealerEntityManager().GetDealerById(product_.dealerId).IsAutoApproval,
                    HrsUsedAtPolicySale = policy_.hrsUsedAtPolicySale,
                    PolicyNo = contract.PolicyNo,
                    PaymentModeId = payment_.paymentModeId,
                    IsPreWarrantyCheck = false,
                    RefNo = payment_.refNo,
                    IsPartialPayment = payment_.isPartialPayment,
                    PaymentTypeId = payment_.paymentTypeId,
                    IsPolicyCanceled = false,
                    PolicySoldDate = policy_.policySoldDate,
                    PremiumCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                    GrossPremiumBeforeTax = BasicPremium,
                    Co_Customer = Guid.Empty,
                    LocalCurrencyConversionRate = currencyEM.GetLocalCurrencyConversionRate(contract.ContractId, relevantCurrencyPeriodId),
                    Month = 0,
                    PaymentMethodFee = GetPaymentCharges(paymentMethodPercentage, BasicPremium, EligibilityFee, contract.ContractId),
                    PaymentMethodFeePercentage = paymentMethodPercentage,
                    PolicyBundleId = Guid.Empty,
                    TotalTax = TotalTax,
                    TPABranchId = policy_.tpaBranchId,
                    TransferFee = (decimal)0.00,
                    // PolicyEndDate = contractEnitiyManager.GetContractById(contract.ContractId).EndDate,
                    // PolicyStartDate = contractEnitiyManager.GetContractById(contract.ContractId).StartDate,
                    BookletNumber = policy_.productContracts.First().BookletNumber,
                    //  MWStartDate = product_.MWStartDate,

                    PolicyEndDate = GetPolicyEndDate(product_.MWStartDate, policy_.policySoldDate,
                        product_.makeId, product_.modelId,
                        product_.dealerId, contract.ExtensionTypeId, product_.MWIsAvailable),
                    PolicyStartDate = GetPolicyStartDate(product_.MWStartDate, policy_.policySoldDate,
                        product_.makeId, product_.modelId,
                        product_.dealerId, contract.ExtensionTypeId, product_.MWIsAvailable),
                    // BookletNumber = policy_.BookletNumber,
                    MWStartDate = product_.MWStartDate,
                    MWIsAvailable = product_.MWIsAvailable,
                    EmiValue = policy_.Emi,


                    //Id = contract.Id,
                    //BordxId = Guid.Empty,
                    //Comment = payment_.comment,
                    //TPABranchId = policy_.tpaBranchId,
                    //CommodityTypeId = product_.commodityTypeId,
                    //ContractId = contract.ContractId,
                    //CurrencyPeriodId = relevantCurrencyPeriodId,
                    //CoverTypeId = contract.CoverTypeId,
                    //CustomerId = Guid.Empty,
                    //CustomerPayment = currencyEM.ConvertToBaseCurrency(payment_.customerPayment, product_.customerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                    //CustomerPaymentCurrencyTypeId = product_.customerPaymentCurrencyTypeId,
                    //DealerId = product_.dealerId,
                    //DealerLocationId = product_.dealerLocationId,
                    //DealerPayment = currencyEM.ConvertToBaseCurrency(payment_.dealerPayment, product_.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                    //DealerPaymentCurrencyTypeId = product_.dealerPaymentCurrencyTypeId,
                    //DealerPolicy = policy_.dealerPolicy,
                    //EntryDateTime = DateTime.Now,
                    //Discount = currencyEM.ConvertToBaseCurrency(payment_.discount, contract.PremiumCurrencyTypeId, relevantCurrencyPeriodId),

                    //EntryUser = Guid.Empty,
                    //ExtensionTypeId = contract.ExtensionTypeId,
                    //HrsUsedAtPolicySale = policy_.hrsUsedAtPolicySale,
                    //IsApproved = new DealerEntityManager().GetDealerById(product_.dealerId).IsAutoApproval,
                    //IsPartialPayment = payment_.isPartialPayment,
                    //IsPolicyCanceled = false,
                    //IsPreWarrantyCheck = false,
                    //IsSpecialDeal = payment_.isSpecialDeal,
                    //ItemId = Guid.Empty,
                    //Month = 0,
                    //PaymentModeId = payment_.paymentModeId,
                    //PaymentTypeId = payment_.paymentTypeId,
                    //PolicyBundleId = Guid.Empty,
                    //PolicyEndDate = contractEnitiyManager.GetContractById(contract.ContractId).EndDate,
                    //PolicyNo = contract.PolicyNo,
                    //PolicySoldDate = policy_.policySoldDate,
                    //PolicyStartDate = contractEnitiyManager.GetContractById(contract.ContractId).StartDate,
                    //Premium = currencyEM.ConvertToBaseCurrency(policy_.premium, contract.PremiumCurrencyTypeId, relevantCurrencyPeriodId),
                    //PremiumCurrencyTypeId = contract.PremiumCurrencyTypeId,
                    //ProductId = product_.productId,
                    //RefNo = payment_.refNo,
                    //SalesPersonId = policy_.salesPersonId,
                    //TransferFee = (decimal)0.00,
                    //Year = 0,
                    //LocalCurrencyConversionRate = currencyEM.GetLocalCurrencyConversionRate(contract.ContractId, relevantCurrencyPeriodId),
                    ////todo
                    //PaymentMethodFeePercentage = paymentMethodPercentage,
                    //GrossPremiumBeforeTax = BasicPremium,
                    //EligibilityFee = EligibilityFee,
                    //PaymentMethodFee = GetPaymentCharges(paymentMethodPercentage,
                    //BasicPremium, EligibilityFee, contract.ContractId),
                    //NRP = BasicNRP,
                    //TotalTax = TotalTax,
                    //BookletNumber = contract.BookletNumber,
                    //DiscountPercentage = Convert.ToInt16( payment_.discount),
                    //Co_Customer = Guid.Empty
                };
                policyList.Add(policy);
            }
            return policyList;
        }

        private decimal GetPaymentCharges(decimal paymentMethodPercentage, decimal BasicPremium, decimal EligibilityFee, Guid contractId)
        {
            decimal Response = decimal.Zero;
            try
            {
                decimal totalTaxValue = GetTotalTax(BasicPremium, contractId);
                Response = (BasicPremium + totalTaxValue + EligibilityFee) * paymentMethodPercentage;

            }
            catch (Exception)
            { }
            return Response;
        }

        private decimal GetTotalTax(decimal BasicPremium, Guid contractId)
        {
            decimal Response = decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<ContractTaxMapping> contractTaxes = session.Query<ContractTaxMapping>()
                    .Where(a => a.ContractId == contractId);
                IEnumerable<CountryTaxes> countryTaxes = session.Query<CountryTaxes>()
                    .Where(a => contractTaxes.Any(b => b.CountryTaxId == a.Id));
                decimal TotaltaxValue = decimal.Zero;
                decimal TaxWithGross = decimal.Zero;
                foreach (CountryTaxes tax in countryTaxes.OrderBy(a => a.IndexVal))
                {
                    if (tax.IsOnGross)
                    {
                        if (tax.IsPercentage)
                        {
                            if (tax.IsOnPreviousTax)
                            {

                                TotaltaxValue += (TaxWithGross * tax.TaxValue) / 100;
                                TaxWithGross += (TaxWithGross * tax.TaxValue) / 100;
                            }
                            else
                            {
                                TotaltaxValue += (BasicPremium * tax.TaxValue) / 100;
                                TaxWithGross += (BasicPremium * tax.TaxValue) / 100;

                            }
                        }
                        else
                        {
                            TotaltaxValue += tax.TaxValue;
                            TaxWithGross += tax.TaxValue;
                        }
                    }
                }
                Response = TotaltaxValue;

            }
            catch (Exception)
            { }
            return Response;
        }



        private decimal GetBasicGross(Guid contractId, decimal dealerPrice, Guid varientId, string premiumBasedOnGross, Guid modelId)
        {
            decimal respone = Decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                Entities.Contract contract = session
                    .Query<Contract>().FirstOrDefault(a => a.Id == contractId);
                if (contract == null)
                {
                    return respone;
                }

                decimal grossValue = Decimal.Zero;
                IEnumerable<Entities.ContractExtensions> contractExts = session.Query<Entities.ContractExtensions>();
                //  .Where(a => a.ContractId == contractId);

                foreach (Entities.ContractExtensions contractExt in contractExts)
                {
                    IEnumerable<Entities.ContractExtensionsPremiumAddon> premiumAddons = session.Query<Entities.ContractExtensionsPremiumAddon>()
                    .Where(a => a.ContractExtensionId == contractExt.Id);
                    IEnumerable<Entities.ContractExtensionVariant> contractVariants = session.Query<Entities.ContractExtensionVariant>()
                        .Where(a => a.ContractExtensionId == contractExt.Id && a.VariantId == varientId);
                    if (contractVariants.FirstOrDefault() != null)
                    {
                        Entities.Variant variant = session.Query<Entities.Variant>()
                            .Where(a => a.Id == varientId).FirstOrDefault();
                        Entities.Model model = session.Query<Entities.Model>()
                            .Where(a => a.Id == modelId).FirstOrDefault();
                        decimal premium = decimal.Zero;//contractExt.GrossPremium;

                        foreach (Entities.ContractExtensionsPremiumAddon addon in premiumAddons)
                        {
                            Entities.PremiumAddonType currenctAddonType = session.Query<Entities.PremiumAddonType>()
                                .Where(a => a.Id == addon.PremiumAddonTypeId).FirstOrDefault();
                            if (currenctAddonType == null || addon.PremiumType.ToLower() == "nett")
                            {
                                continue;
                            }
                            //if (!variant.IsForuByFour)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "F")
                            //        premium -= addon.Value;
                            //}

                            //if (!variant.IsSports)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "S")
                            //        premium -= addon.Value;
                            //}

                            if (!model.AdditionalPremium)
                            {
                                if (currenctAddonType.AddonTypeCode == "A")
                                {
                                    premium -= addon.Value;
                                }
                            }


                        }
                        if (premiumBasedOnGross.ToLower() == "rp")
                        {
                            //grossValue += currencyEM.ConvertToBaseCurrency(
                            //    (premium / 100) * dealerPrice, contractExt.PremiumCurrencyId, contractExt.CurrencyPeriodId);
                        }
                        else
                        {
                            grossValue += premium;
                        }
                    }
                    else
                    {
                        if (premiumBasedOnGross.ToLower() == "rp")
                        {
                            //grossValue += currencyEM.ConvertToBaseCurrency(
                            //     (contractExt.GrossPremium / 100) * dealerPrice, contractExt.PremiumCurrencyId, contractExt.CurrencyPeriodId);
                        }
                        else
                        {
                            //  grossValue += contractExt.GrossPremium;
                        }
                    }
                }
                respone = grossValue;
            }
            catch (Exception)
            { }
            return respone;
        }


        public decimal GetBasicGrossWODealerPrice(Guid contractId, Guid varientId, string premiumBasedOnGross)
        {
            decimal respone = Decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                Entities.Contract contract = session.Query<Entities.Contract>()
                    .Where(a => a.Id == contractId).FirstOrDefault();
                if (contract == null)
                {
                    return respone;
                }

                decimal grossValue = Decimal.Zero;
                IEnumerable<Entities.ContractExtensions> contractExts = session.Query<Entities.ContractExtensions>();
                //  .Where(a => a.ContractId == contractId);

                foreach (Entities.ContractExtensions contractExt in contractExts)
                {
                    IEnumerable<Entities.ContractExtensionsPremiumAddon> premiumAddons = session.Query<Entities.ContractExtensionsPremiumAddon>()
                    .Where(a => a.ContractExtensionId == contractExt.Id);
                    IEnumerable<Entities.ContractExtensionVariant> contractVariants = session.Query<Entities.ContractExtensionVariant>()
                        .Where(a => a.ContractExtensionId == contractExt.Id && a.VariantId == varientId);
                    if (contractVariants.FirstOrDefault() != null)
                    {
                        Entities.Variant variant = session.Query<Entities.Variant>()
                            .Where(a => a.Id == varientId).FirstOrDefault();
                        decimal premium = decimal.Zero;//contractExt.GrossPremium;

                        foreach (Entities.ContractExtensionsPremiumAddon addon in premiumAddons)
                        {
                            Entities.PremiumAddonType currenctAddonType = session.Query<Entities.PremiumAddonType>()
                                .Where(a => a.Id == addon.PremiumAddonTypeId).FirstOrDefault();
                            if (currenctAddonType == null || addon.PremiumType.ToLower() == "nett")
                            {
                                continue;
                            }
                            //if (!variant.IsForuByFour)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "F")
                            //        premium -= addon.Value;
                            //}

                            //if (!variant.IsSports)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "S")
                            //        premium -= addon.Value;
                            //}
                        }

                        grossValue += premium;
                    }
                    else
                    {
                        //grossValue += contractExt.GrossPremium;
                    }
                }
                respone = grossValue;
            }
            catch (Exception)
            { }
            return respone;
        }
        private decimal GetBasicNRP(Guid contractId, decimal dealerPrice, Guid varientId, string premiumBasedOnNett)
        {
            decimal respone = Decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                Entities.Contract contract = session.Query<Entities.Contract>()
                    .Where(a => a.Id == contractId).FirstOrDefault();
                if (contract == null)
                {
                    return respone;
                }

                decimal NRPValue = Decimal.Zero;
                IEnumerable<Entities.ContractExtensions> contractExts = session.Query<Entities.ContractExtensions>();
                // .Where(a => a.ContractId == contractId);

                foreach (Entities.ContractExtensions contractExt in contractExts)
                {
                    IEnumerable<Entities.ContractExtensionsPremiumAddon> premiumAddons = session.Query<Entities.ContractExtensionsPremiumAddon>()
                    .Where(a => a.ContractExtensionId == contractExt.Id);
                    IEnumerable<Entities.ContractExtensionVariant> contractVariants = session.Query<Entities.ContractExtensionVariant>()
                        .Where(a => a.ContractExtensionId == contractExt.Id && a.VariantId == varientId);
                    if (contractVariants.FirstOrDefault() != null)
                    {
                        Entities.Variant variant = session.Query<Entities.Variant>()
                            .Where(a => a.Id == varientId).FirstOrDefault();
                        decimal premium = decimal.Zero;///contractExt.PremiumTotal;

                        foreach (Entities.ContractExtensionsPremiumAddon addon in premiumAddons)
                        {
                            Entities.PremiumAddonType currenctAddonType = session.Query<Entities.PremiumAddonType>()
                                .Where(a => a.Id == addon.PremiumAddonTypeId).FirstOrDefault();
                            if (currenctAddonType == null && addon.PremiumType.ToLower() == "gross")
                            {
                                continue;
                            }
                            //if (!variant.IsForuByFour)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "F")
                            //        premium -= addon.Value;
                            //}

                            //if (!variant.IsSports)
                            //{
                            //    if (currenctAddonType.AddonTypeCode == "S")
                            //        premium -= addon.Value;
                            //}
                        }
                        if (premiumBasedOnNett.ToLower() == "rp")
                        {
                            // NRPValue += (premium / 100) * dealerPrice;
                            //NRPValue += currencyEM.ConvertToBaseCurrency(
                            //   (premium / 100) * dealerPrice, contractExt.PremiumCurrencyId, contractExt.CurrencyPeriodId);
                        }
                        else
                        {
                            NRPValue += premium;
                        }
                    }
                    else
                    {
                        if (premiumBasedOnNett.ToLower() == "rp")
                        {
                            //NRPValue += (contractExt.PremiumTotal / 100) * dealerPrice;
                            //NRPValue += currencyEM.ConvertToBaseCurrency(
                            //   (contractExt.PremiumTotal / 100) * dealerPrice, contractExt.PremiumCurrencyId, contractExt.CurrencyPeriodId);
                        }
                        else
                        {
                            //NRPValue += contractExt.GrossPremium;
                        }
                    }
                }
                respone = NRPValue;
            }
            catch (Exception)
            { }
            return respone;
        }
        private decimal GetEligibilityFee(decimal BasicPremium, Guid contractId, DateTime policySoldDate, decimal amtUsed)
        {
            decimal Response = decimal.Zero;
            try
            {

                EligibilityCheckRequest eligibilityReq = new EligibilityCheckRequest()
                {
                    contractId = contractId,
                    itemPurchesedDate = policySoldDate,
                    usedAmount = amtUsed
                };
                EligibilityCheckResponse eligibilityResp = new PolicyEntityManager().EligibilityCheck(eligibilityReq);
                if (eligibilityResp == null)
                {
                    return Response;
                }

                if (eligibilityResp.isPercentage)
                {
                    Response = (BasicPremium * eligibilityResp.premium) / 100;
                }
                else
                {
                    Response = eligibilityResp.premium;
                }
            }
            catch (Exception)
            { }
            return Response;
        }
        private decimal GetGrossBeforeTax(decimal ContractGross, decimal DealerPrice, string PremiumBasedOnGross)
        {
            decimal Response = decimal.Zero;
            if (PremiumBasedOnGross == "RP")
            {
                Response = (DealerPrice * ContractGross) / 100;
            }
            else
            {
                Response = ContractGross;
            }
            return Response;
        }


        public Guid GetCurrencyPeriodByContractId(Guid contractId)
        {
            ContractEntityManager contractEm = new ContractEntityManager();
            return contractEm.GetCurrencyPeriodByContractId(contractId);
        }

        internal YellowGoodRequestDto PolicyYellowGoodToYellowGoodEntity(Product_ product, Policy_ policy_)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(policy_.productContracts.FirstOrDefault().ContractId);

            var yellowGoodDetails = new YellowGoodRequestDto()
            {
                InvoiceNo = product.invoiceNo,
                ItemPrice = product.itemPrice,
                CategoryId = product.categoryId,
                CommodityUsageTypeId = product.commodityUsageTypeId,
                DealerPrice = product.dealerPrice,
                Id = product.id,
                ItemPurchasedDate = product.itemPurchasedDate,
                ItemStatusId = product.itemStatusId,
                MakeId = product.makeId,
                ModelId = product.modelId,
                ModelYear = product.modelYear,
            };
            return yellowGoodDetails;
        }

        internal OtherItemRequestDto PolicyOtherItemToOtherItemEntity(Product_ product, Policy_ policy_)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid relevantCurrencyPeriodId = GetCurrencyPeriodByContractId(policy_.productContracts.FirstOrDefault().ContractId);

            var otherItemDetails = new OtherItemRequestDto()
            {
                InvoiceNo = product.invoiceNo,
                ItemPrice = product.itemPrice,
                CategoryId = product.categoryId,
                CommodityUsageTypeId = product.commodityUsageTypeId,
                DealerPrice = currencyEm.ConvertToBaseCurrency(product.dealerPrice, product.dealerPaymentCurrencyTypeId, relevantCurrencyPeriodId),
                Id = product.id,
                ItemPurchasedDate = product.itemPurchasedDate,
                ItemStatusId = product.itemStatusId,
                MakeId = product.makeId,
                ModelId = product.modelId,
                ModelYear = product.modelYear,
                SerialNo = product.serialNumber,
                AddnSerialNo = product.additionalSerial,
                VariantId = product.variantId,

            };
            return otherItemDetails;
        }

        internal Customer_E CustomerEntityToEndorsementCustomer(Customer customer)
        {
            CommonEntityManager cem = new CommonEntityManager();
            if (customer.DateOfBirth != null && customer.DLIssueDate != null)
            {
                return new Customer_E()
                {
                    address1 = customer.Address1,
                    address2 = customer.Address2,
                    address3 = customer.Address3,
                    address4 = customer.Address4,
                    businessAddress1 = customer.BusinessAddress1,
                    businessAddress2 = customer.BusinessAddress2,
                    businessAddress3 = customer.BusinessAddress3,
                    businessAddress4 = customer.BusinessAddress4,
                    businessName = customer.BusinessName,
                    businessTelNo = customer.BusinessTelNo,
                    city = cem.GetCityNameById(customer.CityId),
                    country = cem.GetCountryNameById(customer.CountryId),
                    customerId = customer.Id,
                    customerType = cem.GetCustomerTypeNameById(customer.CustomerTypeId),
                    dateOfBirth = customer.DateOfBirth.Value,
                    email = customer.Email,
                    firstName = customer.FirstName,
                    gender = customer.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                    idIssueDate = customer.DLIssueDate.Value,
                    idNo = customer.IDNo,
                    idType = cem.GetIdTypeNameById(customer.IDTypeId),
                    lastName = customer.LastName,
                    mobileNo = customer.MobileNo,
                    nationality = cem.GetNationaltyNameById(customer.NationalityId),
                    otherTelNo = customer.OtherTelNo,
                    usageType = cem.GetUsageTypeNameById(customer.UsageTypeId),
                };
            }
            else {

                return new Customer_E()
                {
                    address1 = customer.Address1,
                    address2 = customer.Address2,
                    address3 = customer.Address3,
                    address4 = customer.Address4,
                    businessAddress1 = customer.BusinessAddress1,
                    businessAddress2 = customer.BusinessAddress2,
                    businessAddress3 = customer.BusinessAddress3,
                    businessAddress4 = customer.BusinessAddress4,
                    businessName = customer.BusinessName,
                    businessTelNo = customer.BusinessTelNo,
                    city = cem.GetCityNameById(customer.CityId),
                    country = cem.GetCountryNameById(customer.CountryId),
                    customerId = customer.Id,
                    customerType = cem.GetCustomerTypeNameById(customer.CustomerTypeId),
                    email = customer.Email,
                    firstName = customer.FirstName,
                    gender = customer.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                    idNo = customer.IDNo,
                    idType = cem.GetIdTypeNameById(customer.IDTypeId),
                    lastName = customer.LastName,
                    mobileNo = customer.MobileNo,
                    nationality = cem.GetNationaltyNameById(customer.NationalityId),
                    otherTelNo = customer.OtherTelNo,
                    usageType = cem.GetUsageTypeNameById(customer.UsageTypeId),
                };

            }
        }

        internal Payment_E PolicyBundleEntityToEndorsementPayment(PolicyBundle policyBundle)
        {
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).FirstOrDefault();
            return new Payment_E()
            {
                comment = policyBundle.Comment,
                customerPayment = currencyEm.ConvertFromBaseCurrency(policyBundle.CustomerPayment, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                dealerPayment = currencyEm.ConvertFromBaseCurrency(policyBundle.DealerPayment, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                discount = currencyEm.ConvertFromBaseCurrency(policyBundle.Discount, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                isPartialPayment = policyBundle.IsPartialPayment,
                isSpecialDeal = policyBundle.IsSpecialDeal,
                paymentMode = cem.GetPaymentMethodNameById(policyBundle.PaymentModeId),
                refNo = policyBundle.RefNo
            };
        }

        internal Product_E PolicyBundleEntityToEndorsementProduct(PolicyBundle policyBundle)
        {
            Product_E response = new Product_E();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            ISession session = EntitySessionManager.GetSession();
            CommodityType commodityType = session.Query<CommodityType>()
                .Where(a => a.CommodityTypeId == policyBundle.CommodityTypeId).FirstOrDefault();
            if (commodityType == null)
            {
                return response;
            }

            Policy policy = session.Query<Policy>().Where(a => a.PolicyBundleId == policyBundle.Id).FirstOrDefault();
            if (policy == null)
            {
                return response;
            }

            Guid policyId = policy.Id;
            if (commodityType.CommodityCode.ToLower().StartsWith("a"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == policyId).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                response = new Product_E()
                {
                    additionalSerial = ItemDetails.PlateNo,
                    aspirationType = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                    bodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                    category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                    commodityType = commodityType.CommodityTypeDescription,
                    commodityUsageType = cem.GetCommodityUsageTypeById(ItemDetails.CommodityUsageTypeId),
                    customerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.CustomerPaymentCurrencyTypeId),
                    cylinderCount = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                    dealer = cem.GetDealerNameById(policyBundle.DealerId),
                    dealerLocation = cem.GetDealerLocationNameById(policyBundle.DealerLocationId),
                    dealerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.DealerPaymentCurrencyTypeId),
                    dealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                    engineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                    fuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                    id = ItemDetails.Id,
                    invoiceNo = "",
                    itemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                    itemPurchasedDate = ItemDetails.ItemPurchasedDate,
                    itemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                    make = cem.GetMakeNameById(ItemDetails.MakeId),
                    model = cem.GetModelNameById(ItemDetails.ModelId),
                    modelYear = ItemDetails.ModelYear,
                    product = cem.GetProductNameById(policyBundle.ProductId),
                    productCode=cem.GetProductCodeById(policyBundle.ProductId),
                    productTypeCode=cem.getProductTypeCodebYproductId(policyBundle.ProductId),
                    serialNumber = ItemDetails.VINNo,
                    transmissionType = cem.GetTransmissionTypeNameById(ItemDetails.TransmissionId),
                    variant = cem.GetVariantNameById(ItemDetails.Variant),
                    registrationDate = ItemDetails.RegistrationDate,
                    driveType =cem.GetDriverTypeByName(ItemDetails.DriveTypeId),
                    engineNumber = ItemDetails.EngineNumber

                };

            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == policyId).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                response = new Product_E()
                {
                    additionalSerial = ItemDetails.PlateNo,
                    aspirationType = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                    bodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                    category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                    commodityType = commodityType.CommodityTypeDescription,
                    commodityUsageType = cem.GetCommodityUsageTypeById(ItemDetails.CommodityUsageTypeId),
                    customerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.CustomerPaymentCurrencyTypeId),
                    cylinderCount = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                    dealer = cem.GetDealerNameById(policyBundle.DealerId),
                    dealerLocation = cem.GetDealerLocationNameById(policyBundle.DealerLocationId),
                    dealerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.DealerPaymentCurrencyTypeId),
                    dealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                    engineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                    fuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                    id = ItemDetails.Id,
                    invoiceNo = "",
                    itemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                    itemPurchasedDate = ItemDetails.ItemPurchasedDate,
                    itemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                    make = cem.GetMakeNameById(ItemDetails.MakeId),
                    model = cem.GetModelNameById(ItemDetails.ModelId),
                    modelYear = ItemDetails.ModelYear,
                    product = cem.GetProductNameById(policyBundle.ProductId),
                    productCode = cem.GetProductCodeById(policyBundle.ProductId),
                    productTypeCode = cem.getProductTypeCodebYproductId(policyBundle.ProductId),
                    serialNumber = ItemDetails.VINNo,
                    transmissionType = cem.GetTransmissionTypeNameById(ItemDetails.TransmissionId),
                    variant = cem.GetVariantNameById(ItemDetails.Variant),
                    registrationDate = ItemDetails.RegistrationDate,
                    engineNumber = ItemDetails.EngineNumber
                };

            }
            else
            {
                if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                {
                    BAndWPolicy electronicPolicyMapping = session.Query<BAndWPolicy>()
                   .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (electronicPolicyMapping == null)
                    {
                        return response;
                    }

                    BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                    .Where(a => a.Id == electronicPolicyMapping.BAndWId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return response;
                    }

                    response = new Product_E()
                    {
                        additionalSerial = String.Empty,
                        aspirationType = String.Empty,
                        bodyType = String.Empty,
                        category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                        commodityType = commodityType.CommodityTypeDescription,
                        commodityUsageType = cem.GetCommodityUsageTypeById(ItemDetails.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.CustomerPaymentCurrencyTypeId),
                        cylinderCount = String.Empty,
                        dealer = cem.GetDealerNameById(policyBundle.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(policyBundle.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        engineCapacity = String.Empty,
                        fuelType = String.Empty,
                        id = ItemDetails.Id,
                        invoiceNo = ItemDetails.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        itemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        make = cem.GetMakeNameById(ItemDetails.MakeId),
                        model = cem.GetModelNameById(ItemDetails.ModelId),
                        modelYear = ItemDetails.ModelYear,
                        product = cem.GetProductNameById(policyBundle.ProductId),
                        serialNumber = ItemDetails.SerialNo,
                        transmissionType = String.Empty,
                        variant = String.Empty
                    };
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                {
                    YellowGoodPolicy yellowgoodPolicyMapping = session.Query<YellowGoodPolicy>()
                   .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (yellowgoodPolicyMapping == null)
                    {
                        return response;
                    }

                    YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                    .Where(a => a.Id == yellowgoodPolicyMapping.YellowGoodId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return response;
                    }

                    response = new Product_E()
                    {
                        additionalSerial = String.Empty,
                        aspirationType = String.Empty,
                        bodyType = String.Empty,
                        category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                        commodityType = commodityType.CommodityTypeDescription,
                        commodityUsageType = cem.GetCommodityUsageTypeById(ItemDetails.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.CustomerPaymentCurrencyTypeId),
                        cylinderCount = String.Empty,
                        dealer = cem.GetDealerNameById(policyBundle.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(policyBundle.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        engineCapacity = String.Empty,
                        fuelType = String.Empty,
                        id = ItemDetails.Id,
                        invoiceNo = ItemDetails.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        itemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        make = cem.GetMakeNameById(ItemDetails.MakeId),
                        model = cem.GetModelNameById(ItemDetails.ModelId),
                        modelYear = ItemDetails.ModelYear,
                        product = cem.GetProductNameById(policyBundle.ProductId),
                        serialNumber = ItemDetails.SerialNo,
                        transmissionType = String.Empty,
                        variant = String.Empty
                    };
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                {
                    OtherItemPolicy otherItemPolicyMapping = session.Query<OtherItemPolicy>()
                   .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (otherItemPolicyMapping == null)
                    {
                        return response;
                    }

                    OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                    .Where(a => a.Id == otherItemPolicyMapping.OtherItemId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return response;
                    }

                    response = new Product_E()
                    {
                        additionalSerial = ItemDetails.AddnSerialNo,
                        aspirationType = String.Empty,
                        bodyType = String.Empty,
                        category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                        commodityType = commodityType.CommodityTypeDescription,
                        commodityUsageType = cem.GetCommodityUsageTypeById(ItemDetails.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.CustomerPaymentCurrencyTypeId),
                        cylinderCount = String.Empty,
                        dealer = cem.GetDealerNameById(policyBundle.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(policyBundle.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(policyBundle.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        engineCapacity = String.Empty,
                        fuelType = String.Empty,
                        id = ItemDetails.Id,
                        invoiceNo = ItemDetails.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyBundle.PremiumCurrencyTypeId, policy.CurrencyPeriodId),

                        itemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        make = cem.GetMakeNameById(ItemDetails.MakeId),
                        model = cem.GetModelNameById(ItemDetails.ModelId),
                        modelYear = ItemDetails.ModelYear,
                        product = cem.GetProductNameById(policyBundle.ProductId),
                        serialNumber = ItemDetails.SerialNo,
                        transmissionType = String.Empty,
                        variant = String.Empty
                    };
                }
                else
                {
                    return response;
                }




            }

            return response;
        }

        internal Policy_E PolicyListToEndorsementPolicy(List<Policy> Policies)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();
            ContractEntityManager ContractEntityManager = new ContractEntityManager();
            List<ProductContract_E> contractList = new List<ProductContract_E>();
            Policy_E response = new Policy_E();
            CommodityType commodityType = session.Query<CommodityType>()
                .Where(a => a.CommodityTypeId == Policies.First().CommodityTypeId).FirstOrDefault();
            Policy policyD = session.Query<Policy>().Where(a => a.Id == Policies.First().Id).FirstOrDefault();

            if (commodityType.CommodityCode.ToLower().StartsWith("a"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == policyD.Id).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                foreach (Policy policy in Policies)
                {
                    response = new Policy_E()
                    {
                        dealerPolicy = policy.DealerPolicy,
                        hrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        id = policy.Id,
                        policySoldDate = policy.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(policy.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),
                    };
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Contract = cem.GetContractNameById(policy.ContractId),
                        //CoverType = cem.GetCoverTypeNameById(policy.CoverTypeId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                               policy.ContractExtensionsId, policy.ContractId,
                                               policy.ProductId, policy.DealerId, policy.PolicySoldDate, ItemDetails.CylinderCountId,
                                               ItemDetails.EngineCapacityId,
                                               ItemDetails.MakeId, ItemDetails.ModelId, ItemDetails.Variant,
                                               ItemDetails.GrossWeight, ItemDetails.ItemStatusId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policy.ContractId,
                                    policy.ProductId, policy.DealerId, policy.PolicySoldDate,
                                    ItemDetails.CylinderCountId, ItemDetails.EngineCapacityId, ItemDetails.MakeId, ItemDetails.ModelId,
                                    ItemDetails.Variant, ItemDetails.GrossWeight),
                        //ExtensionType = cem.GetExtentionTypeNameById(policy.ExtensionTypeId),
                        Id = policy.ContractId,
                        Name = "",
                        ParentProduct = "",
                        PolicyNo = policy.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyCodeById(policy.PremiumCurrencyTypeId),
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        Product = cem.GetProductNameById(policy.ProductId),
                        RSA = cem.GetRSAByProductId(policy.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        RefNo = policy.RefNo,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale
                    };
                    contractList.Add(contract);
                }
                response.productContracts = contractList;
            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == policyD.Id).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                foreach (Policy policy in Policies)
                {
                    response = new Policy_E()
                    {
                        dealerPolicy = policy.DealerPolicy,
                        hrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        id = policy.Id,
                        policySoldDate = policy.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(policy.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),
                    };
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Contract = cem.GetContractNameById(policy.ContractId),
                        //CoverType = cem.GetCoverTypeNameById(policy.CoverTypeId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                               policy.ContractExtensionsId, policy.ContractId,
                                               policy.ProductId, policy.DealerId, policy.PolicySoldDate, ItemDetails.CylinderCountId,
                                               ItemDetails.EngineCapacityId,
                                               ItemDetails.MakeId, ItemDetails.ModelId, ItemDetails.Variant,
                                               ItemDetails.GrossWeight, ItemDetails.ItemStatusId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policy.ContractId,
                                    policy.ProductId, policy.DealerId, policy.PolicySoldDate,
                                    ItemDetails.CylinderCountId, ItemDetails.EngineCapacityId, ItemDetails.MakeId, ItemDetails.ModelId,
                                    ItemDetails.Variant, ItemDetails.GrossWeight),
                        //ExtensionType = cem.GetExtentionTypeNameById(policy.ExtensionTypeId),
                        Id = policy.ContractId,
                        Name = "",
                        ParentProduct = "",
                        PolicyNo = policy.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyCodeById(policy.PremiumCurrencyTypeId),
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        Product = cem.GetProductNameById(policy.ProductId),
                        RSA = cem.GetRSAByProductId(policy.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        RefNo = policy.RefNo,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale
                    };
                    contractList.Add(contract);
                }
                response.productContracts = contractList;
            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
            {
                OtherItemPolicy OtherItemPolicyMapping = session.Query<OtherItemPolicy>()
                    .Where(a => a.PolicyId == policyD.Id).FirstOrDefault();
                if (OtherItemPolicyMapping == null)
                {
                    return response;
                }

                OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                    .Where(a => a.Id == OtherItemPolicyMapping.OtherItemId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                foreach (Policy policy in Policies)
                {
                    response = new Policy_E()
                    {
                        dealerPolicy = policy.DealerPolicy,
                        hrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        id = policy.Id,
                        policySoldDate = policy.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(policy.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),
                    };
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Contract = cem.GetContractNameById(policy.ContractId),
                        //CoverType = cem.GetCoverTypeNameById(policy.CoverTypeId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                               policy.ContractExtensionsId, policy.ContractId,
                                               policy.ProductId, policy.DealerId, policy.PolicySoldDate, Guid.Empty,
                                               Guid.Empty,
                                               ItemDetails.MakeId, ItemDetails.ModelId, Guid.Empty,
                                               0, ItemDetails.ItemStatusId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policy.ContractId,
                                    policy.ProductId, policy.DealerId, policy.PolicySoldDate,
                                    Guid.Empty, Guid.Empty, ItemDetails.MakeId, ItemDetails.ModelId,
                                    Guid.Empty, 0),
                        //ExtensionType = cem.GetExtentionTypeNameById(policy.ExtensionTypeId),
                        Id = policy.ContractId,
                        Name = "",
                        ParentProduct = "",
                        PolicyNo = policy.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyTypeById(policy.PremiumCurrencyTypeId),
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        Product = cem.GetProductNameById(policy.ProductId),
                        RSA = cem.GetRSAByProductId(policy.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        RefNo = policy.RefNo,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale
                    };
                    contractList.Add(contract);
                }
                response.productContracts = contractList;
            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("e"))
            {
                BAndWPolicy BAndWPolicyMapping = session.Query<BAndWPolicy>()
                  .Where(a => a.PolicyId == policyD.Id).FirstOrDefault();
                if (BAndWPolicyMapping == null)
                {
                    return response;
                }

                BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                    .Where(a => a.Id == BAndWPolicyMapping.BAndWId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                foreach (Policy policy in Policies)
                {
                    response = new Policy_E()
                    {
                        dealerPolicy = policy.DealerPolicy,
                        hrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        id = policy.Id,
                        policySoldDate = policy.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(policy.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),
                    };
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Contract = cem.GetContractNameById(policy.ContractId),
                        //CoverType = cem.GetCoverTypeNameById(policy.CoverTypeId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                               policy.ContractExtensionsId, policy.ContractId,
                                               policy.ProductId, policy.DealerId, policy.PolicySoldDate, Guid.Empty,
                                               Guid.Empty,
                                               ItemDetails.MakeId, ItemDetails.ModelId, Guid.Empty,
                                               0, ItemDetails.ItemStatusId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policy.ContractId,
                                    policy.ProductId, policy.DealerId, policy.PolicySoldDate,
                                    Guid.Empty, Guid.Empty, ItemDetails.MakeId, ItemDetails.ModelId,
                                    Guid.Empty, 0),
                        //ExtensionType = cem.GetExtentionTypeNameById(policy.ExtensionTypeId),
                        Id = policy.ContractId,
                        Name = "",
                        ParentProduct = "",
                        PolicyNo = policy.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyTypeById(policy.PremiumCurrencyTypeId),
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        Product = cem.GetProductNameById(policy.ProductId),
                        RSA = cem.GetRSAByProductId(policy.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        RefNo = policy.RefNo,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale
                    };
                    contractList.Add(contract);
                }
                response.productContracts = contractList;
            }
            else
            {
                YellowGoodPolicy YellowGoodPolicyMapping = session.Query<YellowGoodPolicy>()
                  .Where(a => a.PolicyId == policyD.Id).FirstOrDefault();
                if (YellowGoodPolicyMapping == null)
                {
                    return response;
                }

                YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                    .Where(a => a.Id == YellowGoodPolicyMapping.YellowGoodId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                foreach (Policy policy in Policies)
                {
                    response = new Policy_E()
                    {
                        dealerPolicy = policy.DealerPolicy,
                        hrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                        id = policy.Id,
                        policySoldDate = policy.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(policy.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),
                    };
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Contract = cem.GetContractNameById(policy.ContractId),
                        //CoverType = cem.GetCoverTypeNameById(policy.CoverTypeId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                               policy.ContractExtensionsId, policy.ContractId,
                                               policy.ProductId, policy.DealerId, policy.PolicySoldDate, Guid.Empty,
                                               Guid.Empty,
                                               ItemDetails.MakeId, ItemDetails.ModelId, Guid.Empty,
                                               0, ItemDetails.ItemStatusId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policy.ContractId,
                                    policy.ProductId, policy.DealerId, policy.PolicySoldDate,
                                    Guid.Empty, Guid.Empty, ItemDetails.MakeId, ItemDetails.ModelId,
                                    Guid.Empty, 0),
                        //ExtensionType = cem.GetExtentionTypeNameById(policy.ExtensionTypeId),
                        Id = policy.ContractId,
                        Name = "",
                        ParentProduct = "",
                        PolicyNo = policy.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policy.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyTypeById(policy.PremiumCurrencyTypeId),
                        PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                        Product = cem.GetProductNameById(policy.ProductId),
                        RSA = cem.GetRSAByProductId(policy.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        RefNo = policy.RefNo,
                        HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale
                    };
                    contractList.Add(contract);
                }
                response.productContracts = contractList;
            }
            return response;
        }

        internal PolicyDetails_E FullEndorsmentDetailsByPolicyBundleId(Guid PolicyBundleId)
        {
            PolicyDetails_E response = new PolicyDetails_E();
            CommonEntityManager cem = new CommonEntityManager();
            //ContractEntityManager ContractEntity = new ContractEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            ISession session = EntitySessionManager.GetSession();
            PolicyTransactionType transactionType = session.Query<PolicyTransactionType>()
                .Where(a => a.Code.ToLower().StartsWith("endors")).FirstOrDefault();
            if (transactionType == null)
            {
                return response;
            }

            PolicyBundleTransaction latestEndorsement = session.Query<PolicyBundleTransaction>()
                .Where(a => a.TransactionTypeId == transactionType.Id && a.PolicyBundleId == PolicyBundleId
                && a.IsApproved != true && a.IsPolicyCanceled != true).FirstOrDefault();

            if (latestEndorsement == null)
            {
                return response;
            }

            List<PolicyTransaction> latestEndorsmentList = session.Query<PolicyTransaction>()
                .Where(a => a.PolicyBundleTransactionId == latestEndorsement.Id).ToList();
            if (latestEndorsmentList == null || latestEndorsmentList.Count() == 0)
            {
                return response;
            }

            PolicyTransaction dummyPolicyEndorsmentData = latestEndorsmentList.FirstOrDefault();

            CommodityType commodityType = session.Query<CommodityType>()
                .Where(a => a.CommodityTypeId == dummyPolicyEndorsmentData.CommodityTypeId).FirstOrDefault();
            Policy policy = session.Query<Policy>().Where(a => a.Id == dummyPolicyEndorsmentData.PolicyId).FirstOrDefault();

            if (commodityType.CommodityCode.ToLower().StartsWith("a"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == dummyPolicyEndorsmentData.PolicyId).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                response = new PolicyDetails_E()
                {
                    customer = new Customer_E()
                    {
                        address1 = dummyPolicyEndorsmentData.Address1,
                        address2 = dummyPolicyEndorsmentData.Address2,
                        address3 = dummyPolicyEndorsmentData.Address3,
                        address4 = dummyPolicyEndorsmentData.Address4,
                        businessAddress1 = dummyPolicyEndorsmentData.BusinessAddress1,
                        businessAddress2 = dummyPolicyEndorsmentData.BusinessAddress2,
                        businessAddress3 = dummyPolicyEndorsmentData.BusinessAddress3,
                        businessAddress4 = dummyPolicyEndorsmentData.BusinessAddress4,
                        businessName = dummyPolicyEndorsmentData.BusinessName,
                        businessTelNo = dummyPolicyEndorsmentData.BusinessTelNo,
                        city = cem.GetCityNameById(dummyPolicyEndorsmentData.CityId),
                        country = cem.GetCountryNameById(dummyPolicyEndorsmentData.CountryId),
                        customerId = dummyPolicyEndorsmentData.CustomerId,
                        customerType = cem.GetCustomerTypeNameById(dummyPolicyEndorsmentData.CustomerTypeId),
                        dateOfBirth = dummyPolicyEndorsmentData.DateOfBirth.Value,
                        email = dummyPolicyEndorsmentData.Email,
                        firstName = dummyPolicyEndorsmentData.FirstName,
                        gender = dummyPolicyEndorsmentData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        idIssueDate = dummyPolicyEndorsmentData.DLIssueDate.Value,
                        idNo = dummyPolicyEndorsmentData.IDNo,
                        idType = cem.GetIdTypeNameById(dummyPolicyEndorsmentData.IDTypeId),
                        lastName = dummyPolicyEndorsmentData.LastName,
                        mobileNo = dummyPolicyEndorsmentData.MobileNo,
                        nationality = cem.GetNationaltyNameById(dummyPolicyEndorsmentData.NationalityId),
                        otherTelNo = dummyPolicyEndorsmentData.OtherTelNo,
                        usageType = cem.GetUsageTypeNameById(dummyPolicyEndorsmentData.UsageTypeId)
                    },
                    payment = new Payment_E()
                    {
                        comment = dummyPolicyEndorsmentData.Comment,
                        customerPayment = dummyPolicyEndorsmentData.CustomerPayment,
                        dealerPayment = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPayment, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        discount = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Discount, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        isPartialPayment = dummyPolicyEndorsmentData.IsPartialPayment,
                        isSpecialDeal = dummyPolicyEndorsmentData.IsSpecialDeal,
                        paymentMode = cem.GetPaymentMethodNameById(dummyPolicyEndorsmentData.PaymentModeId),
                        refNo = dummyPolicyEndorsmentData.RefNo
                    },
                    policy = new Policy_E()
                    {
                        dealerPolicy = dummyPolicyEndorsmentData.DealerPolicy,
                        hrsUsedAtPolicySale = dummyPolicyEndorsmentData.HrsUsedAtPolicySale,
                        id = dummyPolicyEndorsmentData.PolicyId,
                        policySoldDate = dummyPolicyEndorsmentData.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(dummyPolicyEndorsmentData.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),

                    },
                    product = new Product_E()
                    {
                        additionalSerial = dummyPolicyEndorsmentData.AddnSerialNo,
                        aspirationType = cem.GetAspirationTypeNameById(dummyPolicyEndorsmentData.AspirationId),
                        bodyType = cem.GetBodyTypeNameById(dummyPolicyEndorsmentData.BodyTypeId),
                        category = cem.GetCategoryNameById(dummyPolicyEndorsmentData.CategoryId),
                        commodityType = cem.GetCommodityTypeNameById(dummyPolicyEndorsmentData.CommodityTypeId),
                        commodityUsageType = cem.GetCommodityUsageTypeById(dummyPolicyEndorsmentData.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.CustomerPaymentCurrencyTypeId),
                        cylinderCount = cem.GetCyllinderCountValueById(dummyPolicyEndorsmentData.CylinderCountId),
                        dealer = cem.GetDealerNameById(dummyPolicyEndorsmentData.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(dummyPolicyEndorsmentData.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        engineCapacity = cem.GetEngineCapacityNameById(dummyPolicyEndorsmentData.EngineCapacityId),
                        fuelType = cem.GetFuelTypeNameById(dummyPolicyEndorsmentData.FuelTypeId),
                        id = dummyPolicyEndorsmentData.ProductId,
                        invoiceNo = dummyPolicyEndorsmentData.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.ItemPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        itemPurchasedDate = dummyPolicyEndorsmentData.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(dummyPolicyEndorsmentData.ItemStatusId),
                        make = cem.GetMakeNameById(dummyPolicyEndorsmentData.MakeId),
                        model = cem.GetModelNameById(dummyPolicyEndorsmentData.ModelId),
                        modelYear = dummyPolicyEndorsmentData.ModelYear,
                        product = cem.GetProductNameById(dummyPolicyEndorsmentData.ProductId),
                        serialNumber = dummyPolicyEndorsmentData.SerialNo,
                        transmissionType = cem.GetTransmissionTypeNameById(dummyPolicyEndorsmentData.TransmissionId),
                        variant = cem.GetVariantNameById(dummyPolicyEndorsmentData.Variant),
                        registrationDate = dummyPolicyEndorsmentData.RegistrationDate,
                        engineNumber = dummyPolicyEndorsmentData.EngineNumber
                    }
                };

                List<ProductContract_E> contractList = new List<ProductContract_E>();
                foreach (PolicyTransaction policyTansaction in latestEndorsmentList)
                {
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Id = policyTansaction.PolicyId,
                        Contract = cem.GetContractNameById(policyTansaction.ContractId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                                policy.ContractExtensionsId, policy.ContractId,
                                                policy.ProductId, policy.DealerId, policy.PolicySoldDate, ItemDetails.CylinderCountId,
                                                ItemDetails.EngineCapacityId,
                                                ItemDetails.MakeId, ItemDetails.ModelId, ItemDetails.Variant,
                                                ItemDetails.GrossWeight, ItemDetails.ItemStatusId),
                        //CoverType = cem.GetCoverTypeNameById(policyTansaction.CoverTypeId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policyTansaction.ContractId,
                                    dummyPolicyEndorsmentData.ProductId, dummyPolicyEndorsmentData.DealerId, dummyPolicyEndorsmentData.PolicySoldDate,
                                    dummyPolicyEndorsmentData.CylinderCountId, dummyPolicyEndorsmentData.EngineCapacityId, dummyPolicyEndorsmentData.MakeId, dummyPolicyEndorsmentData.ModelId,
                                    dummyPolicyEndorsmentData.Variant, ItemDetails.GrossWeight),
                        //ExtensionType = cem.GetExtentionTypeNameById(policyTansaction.ExtensionTypeId),
                        Name = cem.GetContractNameById(policyTansaction.ContractId),
                        ParentProduct = cem.GetParentProductNameByProductId(policyTansaction.ProductId),
                        PolicyNo = policyTansaction.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policyTansaction.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyCodeById(policy.PremiumCurrencyTypeId),
                        Product = cem.GetProductNameById(policyTansaction.ProductId),
                        RSA = cem.GetRSAByProductId(policyTansaction.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy")
                    };
                    contractList.Add(contract);
                }
                response.policy.productContracts = contractList;
            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
            {
                VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                    .Where(a => a.PolicyId == dummyPolicyEndorsmentData.PolicyId).FirstOrDefault();
                if (VehiclePolicyMapping == null)
                {
                    return response;
                }

                VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                    .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                response = new PolicyDetails_E()
                {
                    customer = new Customer_E()
                    {
                        address1 = dummyPolicyEndorsmentData.Address1,
                        address2 = dummyPolicyEndorsmentData.Address2,
                        address3 = dummyPolicyEndorsmentData.Address3,
                        address4 = dummyPolicyEndorsmentData.Address4,
                        businessAddress1 = dummyPolicyEndorsmentData.BusinessAddress1,
                        businessAddress2 = dummyPolicyEndorsmentData.BusinessAddress2,
                        businessAddress3 = dummyPolicyEndorsmentData.BusinessAddress3,
                        businessAddress4 = dummyPolicyEndorsmentData.BusinessAddress4,
                        businessName = dummyPolicyEndorsmentData.BusinessName,
                        businessTelNo = dummyPolicyEndorsmentData.BusinessTelNo,
                        city = cem.GetCityNameById(dummyPolicyEndorsmentData.CityId),
                        country = cem.GetCountryNameById(dummyPolicyEndorsmentData.CountryId),
                        customerId = dummyPolicyEndorsmentData.CustomerId,
                        customerType = cem.GetCustomerTypeNameById(dummyPolicyEndorsmentData.CustomerTypeId),
                        dateOfBirth = dummyPolicyEndorsmentData.DateOfBirth.Value,
                        email = dummyPolicyEndorsmentData.Email,
                        firstName = dummyPolicyEndorsmentData.FirstName,
                        gender = dummyPolicyEndorsmentData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        idIssueDate = dummyPolicyEndorsmentData.DLIssueDate.Value,
                        idNo = dummyPolicyEndorsmentData.IDNo,
                        idType = cem.GetIdTypeNameById(dummyPolicyEndorsmentData.IDTypeId),
                        lastName = dummyPolicyEndorsmentData.LastName,
                        mobileNo = dummyPolicyEndorsmentData.MobileNo,
                        nationality = cem.GetNationaltyNameById(dummyPolicyEndorsmentData.NationalityId),
                        otherTelNo = dummyPolicyEndorsmentData.OtherTelNo,
                        usageType = cem.GetUsageTypeNameById(dummyPolicyEndorsmentData.UsageTypeId)
                    },
                    payment = new Payment_E()
                    {
                        comment = dummyPolicyEndorsmentData.Comment,
                        customerPayment = dummyPolicyEndorsmentData.CustomerPayment,
                        dealerPayment = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPayment, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        discount = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Discount, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        isPartialPayment = dummyPolicyEndorsmentData.IsPartialPayment,
                        isSpecialDeal = dummyPolicyEndorsmentData.IsSpecialDeal,
                        paymentMode = cem.GetPaymentMethodNameById(dummyPolicyEndorsmentData.PaymentModeId),
                        refNo = dummyPolicyEndorsmentData.RefNo
                    },
                    policy = new Policy_E()
                    {
                        dealerPolicy = dummyPolicyEndorsmentData.DealerPolicy,
                        hrsUsedAtPolicySale = dummyPolicyEndorsmentData.HrsUsedAtPolicySale,
                        id = dummyPolicyEndorsmentData.PolicyId,
                        policySoldDate = dummyPolicyEndorsmentData.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(dummyPolicyEndorsmentData.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),

                    },
                    product = new Product_E()
                    {
                        additionalSerial = dummyPolicyEndorsmentData.AddnSerialNo,
                        aspirationType = cem.GetAspirationTypeNameById(dummyPolicyEndorsmentData.AspirationId),
                        bodyType = cem.GetBodyTypeNameById(dummyPolicyEndorsmentData.BodyTypeId),
                        category = cem.GetCategoryNameById(dummyPolicyEndorsmentData.CategoryId),
                        commodityType = cem.GetCommodityTypeNameById(dummyPolicyEndorsmentData.CommodityTypeId),
                        commodityUsageType = cem.GetCommodityUsageTypeById(dummyPolicyEndorsmentData.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.CustomerPaymentCurrencyTypeId),
                        cylinderCount = cem.GetCyllinderCountValueById(dummyPolicyEndorsmentData.CylinderCountId),
                        dealer = cem.GetDealerNameById(dummyPolicyEndorsmentData.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(dummyPolicyEndorsmentData.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        engineCapacity = cem.GetEngineCapacityNameById(dummyPolicyEndorsmentData.EngineCapacityId),
                        fuelType = cem.GetFuelTypeNameById(dummyPolicyEndorsmentData.FuelTypeId),
                        id = dummyPolicyEndorsmentData.ProductId,
                        invoiceNo = dummyPolicyEndorsmentData.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.ItemPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        itemPurchasedDate = dummyPolicyEndorsmentData.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(dummyPolicyEndorsmentData.ItemStatusId),
                        make = cem.GetMakeNameById(dummyPolicyEndorsmentData.MakeId),
                        model = cem.GetModelNameById(dummyPolicyEndorsmentData.ModelId),
                        modelYear = dummyPolicyEndorsmentData.ModelYear,
                        product = cem.GetProductNameById(dummyPolicyEndorsmentData.ProductId),
                        serialNumber = dummyPolicyEndorsmentData.SerialNo,
                        transmissionType = cem.GetTransmissionTypeNameById(dummyPolicyEndorsmentData.TransmissionId),
                        variant = cem.GetVariantNameById(dummyPolicyEndorsmentData.Variant),
                        registrationDate = dummyPolicyEndorsmentData.RegistrationDate,



                    }
                };

                List<ProductContract_E> contractList = new List<ProductContract_E>();
                foreach (PolicyTransaction policyTansaction in latestEndorsmentList)
                {
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Id = policyTansaction.PolicyId,
                        Contract = cem.GetContractNameById(policyTansaction.ContractId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                                policy.ContractExtensionsId, policy.ContractId,
                                                policy.ProductId, policy.DealerId, policy.PolicySoldDate, ItemDetails.CylinderCountId,
                                                ItemDetails.EngineCapacityId,
                                                ItemDetails.MakeId, ItemDetails.ModelId, ItemDetails.Variant,
                                                ItemDetails.GrossWeight, ItemDetails.ItemStatusId),
                        //CoverType = cem.GetCoverTypeNameById(policyTansaction.CoverTypeId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policyTansaction.ContractId,
                                    dummyPolicyEndorsmentData.ProductId, dummyPolicyEndorsmentData.DealerId, dummyPolicyEndorsmentData.PolicySoldDate,
                                    dummyPolicyEndorsmentData.CylinderCountId, dummyPolicyEndorsmentData.EngineCapacityId, dummyPolicyEndorsmentData.MakeId, dummyPolicyEndorsmentData.ModelId,
                                    dummyPolicyEndorsmentData.Variant, ItemDetails.GrossWeight),
                        //ExtensionType = cem.GetExtentionTypeNameById(policyTansaction.ExtensionTypeId),
                        Name = cem.GetContractNameById(policyTansaction.ContractId),
                        ParentProduct = cem.GetParentProductNameByProductId(policyTansaction.ProductId),
                        PolicyNo = policyTansaction.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policyTansaction.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyCodeById(policy.PremiumCurrencyTypeId),
                        Product = cem.GetProductNameById(policyTansaction.ProductId),
                        RSA = cem.GetRSAByProductId(policyTansaction.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy")
                    };
                    contractList.Add(contract);
                }
                response.policy.productContracts = contractList;
            }
            else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
            {
                OtherItemPolicy OtherItemPolicyMapping = session.Query<OtherItemPolicy>()
                    .Where(a => a.PolicyId == dummyPolicyEndorsmentData.PolicyId).FirstOrDefault();
                if (OtherItemPolicyMapping == null)
                {
                    return response;
                }

                OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                    .Where(a => a.Id == OtherItemPolicyMapping.OtherItemId).FirstOrDefault();
                if (ItemDetails == null)
                {
                    return response;
                }

                response = new PolicyDetails_E()
                {
                    customer = new Customer_E()
                    {
                        address1 = dummyPolicyEndorsmentData.Address1,
                        address2 = dummyPolicyEndorsmentData.Address2,
                        address3 = dummyPolicyEndorsmentData.Address3,
                        address4 = dummyPolicyEndorsmentData.Address4,
                        businessAddress1 = dummyPolicyEndorsmentData.BusinessAddress1,
                        businessAddress2 = dummyPolicyEndorsmentData.BusinessAddress2,
                        businessAddress3 = dummyPolicyEndorsmentData.BusinessAddress3,
                        businessAddress4 = dummyPolicyEndorsmentData.BusinessAddress4,
                        businessName = dummyPolicyEndorsmentData.BusinessName,
                        businessTelNo = dummyPolicyEndorsmentData.BusinessTelNo,
                        city = cem.GetCityNameById(dummyPolicyEndorsmentData.CityId),
                        country = cem.GetCountryNameById(dummyPolicyEndorsmentData.CountryId),
                        customerId = dummyPolicyEndorsmentData.CustomerId,
                        customerType = cem.GetCustomerTypeNameById(dummyPolicyEndorsmentData.CustomerTypeId),
                        dateOfBirth = dummyPolicyEndorsmentData.DateOfBirth.Value,
                        email = dummyPolicyEndorsmentData.Email,
                        firstName = dummyPolicyEndorsmentData.FirstName,
                        gender = dummyPolicyEndorsmentData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        idIssueDate = dummyPolicyEndorsmentData.DLIssueDate.Value,
                        idNo = dummyPolicyEndorsmentData.IDNo,
                        idType = cem.GetIdTypeNameById(dummyPolicyEndorsmentData.IDTypeId),
                        lastName = dummyPolicyEndorsmentData.LastName,
                        mobileNo = dummyPolicyEndorsmentData.MobileNo,
                        nationality = cem.GetNationaltyNameById(dummyPolicyEndorsmentData.NationalityId),
                        otherTelNo = dummyPolicyEndorsmentData.OtherTelNo,
                        usageType = cem.GetUsageTypeNameById(dummyPolicyEndorsmentData.UsageTypeId)
                    },
                    payment = new Payment_E()
                    {
                        comment = dummyPolicyEndorsmentData.Comment,
                        customerPayment = dummyPolicyEndorsmentData.CustomerPayment,
                        dealerPayment = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPayment, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        discount = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Discount, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        isPartialPayment = dummyPolicyEndorsmentData.IsPartialPayment,
                        isSpecialDeal = dummyPolicyEndorsmentData.IsSpecialDeal,
                        paymentMode = cem.GetPaymentMethodNameById(dummyPolicyEndorsmentData.PaymentModeId),
                        refNo = dummyPolicyEndorsmentData.RefNo
                    },
                    policy = new Policy_E()
                    {
                        dealerPolicy = dummyPolicyEndorsmentData.DealerPolicy,
                        hrsUsedAtPolicySale = dummyPolicyEndorsmentData.HrsUsedAtPolicySale,
                        id = dummyPolicyEndorsmentData.PolicyId,
                        policySoldDate = dummyPolicyEndorsmentData.PolicySoldDate,
                        premium = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        salesPerson = cem.GetUserNameById(dummyPolicyEndorsmentData.SalesPersonId),
                        tpaBranch = cem.GetBranchNameById(policy.TPABranchId),

                    },
                    product = new Product_E()
                    {
                        additionalSerial = dummyPolicyEndorsmentData.AddnSerialNo,
                        aspirationType = cem.GetAspirationTypeNameById(dummyPolicyEndorsmentData.AspirationId),
                        bodyType = cem.GetBodyTypeNameById(dummyPolicyEndorsmentData.BodyTypeId),
                        category = cem.GetCategoryNameById(dummyPolicyEndorsmentData.CategoryId),
                        commodityType = cem.GetCommodityTypeNameById(dummyPolicyEndorsmentData.CommodityTypeId),
                        commodityUsageType = cem.GetCommodityUsageTypeById(dummyPolicyEndorsmentData.CommodityUsageTypeId),
                        customerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.CustomerPaymentCurrencyTypeId),
                        cylinderCount = cem.GetCyllinderCountValueById(dummyPolicyEndorsmentData.CylinderCountId),
                        dealer = cem.GetDealerNameById(dummyPolicyEndorsmentData.DealerId),
                        dealerLocation = cem.GetDealerLocationNameById(dummyPolicyEndorsmentData.DealerLocationId),
                        dealerPaymentCurrencyType = cem.GetCurrencyTypeById(dummyPolicyEndorsmentData.DealerPaymentCurrencyTypeId),
                        dealerPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.DealerPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        engineCapacity = cem.GetEngineCapacityNameById(dummyPolicyEndorsmentData.EngineCapacityId),
                        fuelType = cem.GetFuelTypeNameById(dummyPolicyEndorsmentData.FuelTypeId),
                        id = dummyPolicyEndorsmentData.ProductId,
                        invoiceNo = dummyPolicyEndorsmentData.InvoiceNo,
                        itemPrice = currencyEm.ConvertFromBaseCurrency(dummyPolicyEndorsmentData.ItemPrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        itemPurchasedDate = dummyPolicyEndorsmentData.ItemPurchasedDate,
                        itemStatus = cem.GetItemStatusNameById(dummyPolicyEndorsmentData.ItemStatusId),
                        make = cem.GetMakeNameById(dummyPolicyEndorsmentData.MakeId),
                        model = cem.GetModelNameById(dummyPolicyEndorsmentData.ModelId),
                        modelYear = dummyPolicyEndorsmentData.ModelYear,
                        product = cem.GetProductNameById(dummyPolicyEndorsmentData.ProductId),
                        serialNumber = dummyPolicyEndorsmentData.SerialNo,
                        transmissionType = cem.GetTransmissionTypeNameById(dummyPolicyEndorsmentData.TransmissionId),
                        variant = cem.GetVariantNameById(dummyPolicyEndorsmentData.Variant),
                        registrationDate = dummyPolicyEndorsmentData.RegistrationDate,



                    }
                };

                List<ProductContract_E> contractList = new List<ProductContract_E>();
                foreach (PolicyTransaction policyTansaction in latestEndorsmentList)
                {
                    ProductContract_E contract = new ProductContract_E()
                    {
                        Id = policyTansaction.PolicyId,
                        Contract = cem.GetContractNameById(policyTansaction.ContractId),
                        CoverType = ContractEntityManager.GetGetCoverTypesByExtensionIdUnitOfWork2(policy.ContractInsuaranceLimitationId,
                                                policy.ContractExtensionsId, policy.ContractId,
                                                policy.ProductId, policy.DealerId, policy.PolicySoldDate, Guid.Empty,
                                                Guid.Empty,
                                                ItemDetails.MakeId, ItemDetails.ModelId, ItemDetails.VariantId,
                                                0, ItemDetails.ItemStatusId),
                        //CoverType = cem.GetCoverTypeNameById(policyTansaction.CoverTypeId),
                        ExtensionType = ContractEntityManager.GetAllExtensionTypeByContractId(policyTansaction.ContractId,
                                    dummyPolicyEndorsmentData.ProductId, dummyPolicyEndorsmentData.DealerId, dummyPolicyEndorsmentData.PolicySoldDate,
                                    dummyPolicyEndorsmentData.CylinderCountId, dummyPolicyEndorsmentData.EngineCapacityId, dummyPolicyEndorsmentData.MakeId, dummyPolicyEndorsmentData.ModelId,
                                    dummyPolicyEndorsmentData.Variant, 0),
                        //ExtensionType = cem.GetExtentionTypeNameById(policyTansaction.ExtensionTypeId),
                        Name = cem.GetContractNameById(policyTansaction.ContractId),
                        ParentProduct = cem.GetParentProductNameByProductId(policyTansaction.ProductId),
                        PolicyNo = policyTansaction.PolicyNo,
                        Premium = currencyEm.ConvertFromBaseCurrency(policyTansaction.Premium, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        PremiumCurrencyName = cem.GetCurrencyCodeById(policy.PremiumCurrencyTypeId),
                        Product = cem.GetProductNameById(policyTansaction.ProductId),
                        RSA = cem.GetRSAByProductId(policyTansaction.ProductId),
                        PolicyStartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        PolicyEndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy")
                    };
                    contractList.Add(contract);
                }
                response.policy.productContracts = contractList;
            }



            return response;
        }

        internal VehicleDetailsHistory DBVehicleDetailsToVehicleDetailsHistory(VehicleDetails Vehicle, PolicyBundleTransaction bundleTransaction)
        {
            VehicleDetailsHistory vehicleHistory = new VehicleDetailsHistory()
            {
                AspirationId = Vehicle.AspirationId,
                BodyTypeId = Vehicle.BodyTypeId,
                CategoryId = Vehicle.CategoryId,
                CommodityUsageTypeId = Vehicle.CommodityUsageTypeId,
                CylinderCountId = Vehicle.CylinderCountId,
                DealerPrice = Vehicle.DealerPrice,
                DriveTypeId = Vehicle.DriveTypeId,
                EngineCapacityId = Vehicle.EngineCapacityId,
                EntryDateTime = DateTime.UtcNow,
                EntryUser = Guid.Empty,
                FuelTypeId = Vehicle.FuelTypeId,
                Id = Guid.NewGuid(),
                ItemPurchasedDate = Vehicle.ItemPurchasedDate,
                ItemStatusId = Vehicle.ItemStatusId,
                MakeId = Vehicle.MakeId,
                ModelId = Vehicle.ModelId,
                ModelYear = Vehicle.ModelYear,
                PlateNo = Vehicle.PlateNo,
                PolicyTransactionId = bundleTransaction.Id,
                TransmissionId = Vehicle.TransmissionId,
                Variant = Vehicle.Variant,
                VehicleDetailsId = Vehicle.Id,
                VehiclePrice = Vehicle.VehiclePrice,
                VINNo = Vehicle.VINNo,
                RegistrationDate = Vehicle.RegistrationDate,
                GrossWeight = Vehicle.GrossWeight
            };
            return vehicleHistory;
        }

        internal VehicleDetails PolicyTransactionToVehicleEntity(VehicleDetails Vehicle, PolicyBundleTransaction bundleTransaction, PolicyTransaction policyTransaction)
        {
            Vehicle.AspirationId = policyTransaction.AspirationId;
            Vehicle.BodyTypeId = policyTransaction.BodyTypeId;
            Vehicle.CategoryId = policyTransaction.CategoryId;
            Vehicle.CommodityUsageTypeId = policyTransaction.CommodityUsageTypeId;
            Vehicle.CylinderCountId = policyTransaction.CylinderCountId;
            Vehicle.DealerPrice = policyTransaction.DealerPrice;
            Vehicle.DriveTypeId = policyTransaction.DriveTypeId;
            Vehicle.EngineCapacityId = policyTransaction.EngineCapacityId;
            Vehicle.FuelTypeId = policyTransaction.FuelTypeId;
            Vehicle.Id = Vehicle.Id;
            Vehicle.ItemPurchasedDate = policyTransaction.ItemPurchasedDate;
            Vehicle.ItemStatusId = policyTransaction.ItemStatusId;
            Vehicle.MakeId = policyTransaction.MakeId;
            Vehicle.ModelId = policyTransaction.ModelId;
            Vehicle.ModelYear = policyTransaction.ModelYear;
            Vehicle.PlateNo = policyTransaction.PlateNo;
            Vehicle.TransmissionId = policyTransaction.TransmissionId;
            Vehicle.Variant = policyTransaction.Variant;
            Vehicle.VehiclePrice = policyTransaction.ItemPrice;
            Vehicle.VINNo = policyTransaction.VINNo;

            return Vehicle;
        }

        internal BrownAndWhiteDetailsHistory DBBrownAndWhiteDetailsToBrownAndWhiteHistory(BrownAndWhiteDetails electronicItem, PolicyBundleTransaction bundleTransaction)
        {
            BrownAndWhiteDetailsHistory response = new BrownAndWhiteDetailsHistory()
            {
                CategoryId = electronicItem.CategoryId,
                CommodityUsageTypeId = electronicItem.CommodityUsageTypeId,
                DealerPrice = electronicItem.DealerPrice,
                EntryDateTime = DateTime.UtcNow,
                EntryUser = Guid.Empty,
                Id = Guid.NewGuid(),
                ItemPurchasedDate = electronicItem.ItemPurchasedDate,
                ItemStatusId = electronicItem.ItemStatusId,
                MakeId = electronicItem.MakeId,
                ModelId = electronicItem.ModelId,
                ModelYear = electronicItem.ModelYear,
                AddnSerialNo = electronicItem.AddnSerialNo,
                PolicyTransactionId = bundleTransaction.Id,
                BrownAndWhiteDetailsId = electronicItem.Id,
                ItemPrice = electronicItem.ItemPrice,
                SerialNo = electronicItem.SerialNo
            };
            return response;
        }

        internal BrownAndWhiteDetails PolicyTransactionToBrownAndWhiteEntity(BrownAndWhiteDetails electronicItem, PolicyBundleTransaction bundleTransaction, PolicyTransaction policyTransaction)
        {
            electronicItem.CategoryId = policyTransaction.CategoryId;
            electronicItem.CommodityUsageTypeId = policyTransaction.CommodityUsageTypeId;
            electronicItem.DealerPrice = policyTransaction.DealerPrice;
            electronicItem.Id = policyTransaction.BAndWId;
            electronicItem.ItemPurchasedDate = policyTransaction.ItemPurchasedDate;
            electronicItem.ItemStatusId = policyTransaction.ItemStatusId;
            electronicItem.MakeId = policyTransaction.MakeId;
            electronicItem.ModelId = policyTransaction.ModelId;
            electronicItem.ModelYear = policyTransaction.ModelYear;
            electronicItem.AddnSerialNo = policyTransaction.AddnSerialNo;
            electronicItem.ItemPrice = policyTransaction.ItemPrice;
            electronicItem.SerialNo = policyTransaction.SerialNo;
            electronicItem.InvoiceNo = policyTransaction.InvoiceNo;
            return electronicItem;
        }

        internal OtherItemDetailsHistory DBOtherItemDetailsToOtherItemDetailsHistory(OtherItemDetails otherItem, PolicyBundleTransaction bundleTransaction)
        {
            OtherItemDetailsHistory response = new OtherItemDetailsHistory()
            {
                CategoryId = otherItem.CategoryId,
                CommodityUsageTypeId = otherItem.CommodityUsageTypeId,
                DealerPrice = otherItem.DealerPrice,
                EntryDateTime = DateTime.UtcNow,
                EntryUser = Guid.Empty,
                Id = Guid.NewGuid(),
                ItemPurchasedDate = otherItem.ItemPurchasedDate,
                ItemStatusId = otherItem.ItemStatusId,
                MakeId = otherItem.MakeId,
                ModelId = otherItem.ModelId,
                ModelYear = otherItem.ModelYear,
                AddnSerialNo = otherItem.AddnSerialNo,
                PolicyTransactionId = bundleTransaction.Id,
                OtherItemDetailsId = otherItem.Id,
                ItemPrice = otherItem.ItemPrice,
                SerialNo = otherItem.SerialNo
            };
            return response;
        }

        internal OtherItemDetails PolicyTransactionToOtherItemEntity(OtherItemDetails otherItem, PolicyBundleTransaction bundleTransaction, PolicyTransaction policyTransaction)
        {

            otherItem.CategoryId = policyTransaction.CategoryId;
            otherItem.CommodityUsageTypeId = policyTransaction.CommodityUsageTypeId;
            otherItem.DealerPrice = policyTransaction.DealerPrice;
            otherItem.Id = policyTransaction.BAndWId;
            otherItem.ItemPurchasedDate = policyTransaction.ItemPurchasedDate;
            otherItem.ItemStatusId = policyTransaction.ItemStatusId;
            otherItem.MakeId = policyTransaction.MakeId;
            otherItem.ModelId = policyTransaction.ModelId;
            otherItem.ModelYear = policyTransaction.ModelYear;
            otherItem.AddnSerialNo = policyTransaction.AddnSerialNo;
            otherItem.ItemPrice = policyTransaction.ItemPrice;
            otherItem.SerialNo = policyTransaction.SerialNo;
            otherItem.InvoiceNo = policyTransaction.InvoiceNo;
            return otherItem;
        }

        internal YellowGoodDetailsHistory DBYellowGoodDetailsToYellowGoodHistory(YellowGoodDetails yellowGood, PolicyBundleTransaction bundleTransaction)
        {
            YellowGoodDetailsHistory response = new YellowGoodDetailsHistory()
            {
                CategoryId = yellowGood.CategoryId,
                CommodityUsageTypeId = yellowGood.CommodityUsageTypeId,
                DealerPrice = yellowGood.DealerPrice,
                EntryDateTime = DateTime.UtcNow,
                EntryUser = Guid.Empty,
                Id = Guid.NewGuid(),
                ItemPurchasedDate = yellowGood.ItemPurchasedDate,
                ItemStatusId = yellowGood.ItemStatusId,
                MakeId = yellowGood.MakeId,
                ModelId = yellowGood.ModelId,
                ModelYear = yellowGood.ModelYear,
                AddnSerialNo = yellowGood.AddnSerialNo,
                PolicyTransactionId = bundleTransaction.Id,
                YellowGoodDetailsId = yellowGood.Id,
                ItemPrice = yellowGood.ItemPrice,
                SerialNo = yellowGood.SerialNo
            };
            return response;
        }

        internal YellowGoodDetails PolicyTransactionToYellowGoodEntity(YellowGoodDetails yellowGood, PolicyBundleTransaction bundleTransaction, PolicyTransaction policyTransaction)
        {
            yellowGood.CategoryId = policyTransaction.CategoryId;
            yellowGood.CommodityUsageTypeId = policyTransaction.CommodityUsageTypeId;
            yellowGood.DealerPrice = policyTransaction.DealerPrice;
            yellowGood.Id = policyTransaction.BAndWId;
            yellowGood.ItemPurchasedDate = policyTransaction.ItemPurchasedDate;
            yellowGood.ItemStatusId = policyTransaction.ItemStatusId;
            yellowGood.MakeId = policyTransaction.MakeId;
            yellowGood.ModelId = policyTransaction.ModelId;
            yellowGood.ModelYear = policyTransaction.ModelYear;
            yellowGood.AddnSerialNo = policyTransaction.AddnSerialNo;
            yellowGood.ItemPrice = policyTransaction.ItemPrice;
            yellowGood.SerialNo = policyTransaction.SerialNo;
            yellowGood.InvoiceNo = policyTransaction.InvoiceNo;
            return yellowGood;
        }

        internal CustomerHistory CustomerEntityToCustomerHistory(Customer customer, PolicyBundleTransaction bundleTransaction)
        {
            CustomerHistory response = new CustomerHistory()
            {
                Address1 = customer.Address1,
                Address2 = customer.Address2,
                Address3 = customer.Address3,
                Address4 = customer.Address4,
                BusinessAddress1 = customer.BusinessAddress1,
                BusinessAddress2 = customer.BusinessAddress2,
                BusinessAddress3 = customer.BusinessAddress3,
                BusinessAddress4 = customer.BusinessAddress4,
                BusinessName = customer.BusinessName,
                BusinessTelNo = customer.BusinessTelNo,
                CityId = customer.CityId,
                CountryId = customer.CountryId,
                CustomerId = customer.Id,
                CustomerTypeId = customer.IDTypeId,
                DateOfBirth = customer.DateOfBirth,
                DLIssueDate = customer.DLIssueDate,
                Email = customer.Email,
                EntryDateTime = DateTime.UtcNow,
                EntryUserId = string.Empty,
                FirstName = customer.FirstName,
                Gender = customer.Gender,
                Id = Guid.NewGuid(),
                IDNo = customer.IDNo,
                IDTypeId = customer.IDTypeId,
                IsActive = customer.IsActive,
                LastModifiedDateTime = DateTime.UtcNow,
                LastName = customer.LastName,
                MaritalStatusId = customer.MaritalStatusId,
                MobileNo = customer.MobileNo,
                NationalityId = customer.NationalityId,
                OccupationId = customer.OccupationId,
                OtherTelNo = customer.OtherTelNo,
                Password = customer.Password,
                PolicyTransactionId = bundleTransaction.Id,
                PostalCode = customer.PostalCode,
                ProfilePicture = customer.ProfilePicture,
                TitleId = customer.TitleId,
                UsageTypeId = customer.UsageTypeId,
                UserName = customer.UserName
            };

            return response;
        }

        internal PolicyBundleHistory PolicyBundleToPolicyBundleHistory(PolicyBundle policyBundle, PolicyBundleTransaction bundleTransaction)
        {
            PolicyBundleHistory response = new PolicyBundleHistory()
            {
                Comment = policyBundle.Comment,
                CommodityTypeId = policyBundle.CommodityTypeId,
                ContractId = policyBundle.ContractId,
                CoverTypeId = policyBundle.CoverTypeId,
                CustomerId = policyBundle.CustomerId,
                CustomerPayment = policyBundle.CustomerPayment,
                CustomerPaymentCurrencyTypeId = policyBundle.CustomerPaymentCurrencyTypeId,
                DealerId = policyBundle.DealerId,
                DealerLocationId = policyBundle.DealerLocationId,
                DealerPayment = policyBundle.DealerPayment,
                DealerPaymentCurrencyTypeId = policyBundle.DealerPaymentCurrencyTypeId,
                DealerPolicy = policyBundle.DealerPolicy,
                Discount = policyBundle.Discount,
                EntryDateTime = DateTime.UtcNow,
                EntryUser = Guid.Empty,
                ExtensionTypeId = policyBundle.ExtensionTypeId,
                HrsUsedAtPolicySale = policyBundle.HrsUsedAtPolicySale,
                Id = Guid.NewGuid(),
                IsApproved = policyBundle.IsApproved,
                IsPartialPayment = policyBundle.IsPartialPayment,
                IsPolicyCanceled = policyBundle.IsPolicyCanceled,
                IsPreWarrantyCheck = policyBundle.IsPreWarrantyCheck,
                IsSpecialDeal = policyBundle.IsSpecialDeal,
                PaymentModeId = policyBundle.PaymentModeId,
                PolicyId = policyBundle.Id,
                PolicyNo = policyBundle.PolicyNo,
                PolicySoldDate = policyBundle.PolicySoldDate,
                Premium = policyBundle.Premium,
                PremiumCurrencyTypeId = policyBundle.PremiumCurrencyTypeId,
                ProductId = policyBundle.ProductId,
                RefNo = policyBundle.RefNo,
                SalesPersonId = policyBundle.SalesPersonId,
                TransactionTypeId = bundleTransaction.TransactionTypeId,
                Type = "Endorsement"
            };
            return response;
        }

        internal Customer PolicyTransactionToCustomer(Customer customer, PolicyBundleTransaction bundleTransaction, PolicyTransaction policyTransaction)
        {
            customer.Address1 = policyTransaction.Address1;
            customer.Address2 = policyTransaction.Address2;
            customer.Address3 = policyTransaction.Address3;
            customer.Address4 = policyTransaction.Address4;
            customer.BusinessAddress1 = policyTransaction.BusinessAddress1;
            customer.BusinessAddress2 = policyTransaction.BusinessAddress2;
            customer.BusinessAddress3 = policyTransaction.BusinessAddress3;
            customer.BusinessAddress4 = policyTransaction.BusinessAddress4;
            customer.BusinessName = policyTransaction.BusinessName;
            customer.BusinessTelNo = policyTransaction.BusinessTelNo;
            customer.CityId = policyTransaction.CityId;
            customer.CountryId = policyTransaction.CountryId;
            customer.DateOfBirth = policyTransaction.DateOfBirth;
            customer.DLIssueDate = policyTransaction.DLIssueDate;
            customer.Email = policyTransaction.Email;
            customer.EntryDateTime = DateTime.UtcNow;
            customer.EntryUserId = customer.EntryUserId;
            customer.FirstName = policyTransaction.FirstName;
            customer.Gender = policyTransaction.Gender;
            customer.Id = policyTransaction.CustomerId;
            customer.IDNo = policyTransaction.IDNo;
            customer.IDTypeId = policyTransaction.IDTypeId;
            customer.IsActive = policyTransaction.IsActive;
            customer.LastModifiedDateTime = DateTime.UtcNow;
            customer.LastName = policyTransaction.LastName;
            customer.MobileNo = policyTransaction.MobileNo;
            customer.NationalityId = policyTransaction.NationalityId;
            customer.OtherTelNo = policyTransaction.OtherTelNo;
            customer.Password = policyTransaction.Password;
            customer.ProfilePicture = policyTransaction.ProfilePicture;
            customer.UsageTypeId = policyTransaction.UsageTypeId;
            customer.UserName = policyTransaction.Email;
            return customer;
        }



        internal PolicyBundle PolicyTransactionToPolicyBundle(PolicyBundle policyBundle, PolicyBundleTransaction bundleTransaction)
        {
            policyBundle.Comment = bundleTransaction.Comment;
            policyBundle.CommodityTypeId = bundleTransaction.CommodityTypeId;
            policyBundle.ContractId = bundleTransaction.ContractId;
            policyBundle.CoverTypeId = bundleTransaction.CoverTypeId;
            policyBundle.CustomerId = bundleTransaction.CustomerId;
            policyBundle.CustomerPayment = bundleTransaction.CustomerPayment;
            policyBundle.CustomerPaymentCurrencyTypeId = bundleTransaction.CustomerPaymentCurrencyTypeId;
            policyBundle.DealerId = bundleTransaction.DealerId;
            policyBundle.DealerLocationId = bundleTransaction.DealerLocationId;
            policyBundle.DealerPayment = bundleTransaction.DealerPayment;
            policyBundle.DealerPaymentCurrencyTypeId = bundleTransaction.DealerPaymentCurrencyTypeId;
            policyBundle.DealerPolicy = bundleTransaction.DealerPolicy;
            policyBundle.Discount = bundleTransaction.Discount;
            policyBundle.ExtensionTypeId = bundleTransaction.ExtensionTypeId;
            policyBundle.HrsUsedAtPolicySale = bundleTransaction.HrsUsedAtPolicySale;
            policyBundle.IsApproved = bundleTransaction.IsApproved;
            policyBundle.IsPartialPayment = bundleTransaction.IsPartialPayment;
            policyBundle.IsPolicyCanceled = bundleTransaction.IsPolicyCanceled;
            policyBundle.IsPreWarrantyCheck = bundleTransaction.IsPreWarrantyCheck;
            policyBundle.IsSpecialDeal = bundleTransaction.IsSpecialDeal;
            policyBundle.PaymentModeId = bundleTransaction.PaymentModeId;
            policyBundle.PolicyNo = bundleTransaction.PolicyNo;
            policyBundle.PolicySoldDate = bundleTransaction.PolicySoldDate;
            policyBundle.Premium = bundleTransaction.Premium;
            policyBundle.PremiumCurrencyTypeId = bundleTransaction.PremiumCurrencyTypeId;
            policyBundle.ProductId = bundleTransaction.ProductId;
            policyBundle.RefNo = bundleTransaction.RefNo;
            policyBundle.SalesPersonId = bundleTransaction.SalesPersonId;

            return policyBundle;
        }

        internal List<PolicyHistory> PolicyToPolicyHistory(List<Policy> Policylist)
        {
            List<PolicyHistory> response = new List<PolicyHistory>();
            foreach (Policy policy in Policylist)
            {
                PolicyHistory policyHistory = new PolicyHistory()
                {
                    Comment = policy.Comment,
                    CommodityTypeId = policy.CommodityTypeId,
                    ContractId = policy.ContractId,
                    CoverTypeId = policy.CoverTypeId,
                    CurrencyPeriodId = policy.CurrencyPeriodId,
                    CustomerId = policy.CustomerId,
                    CustomerPayment = policy.CustomerPayment,
                    CustomerPaymentCurrencyTypeId = policy.CustomerPaymentCurrencyTypeId,
                    DealerId = policy.DealerId,
                    DealerLocationId = policy.DealerLocationId,
                    DealerPayment = policy.DealerPayment,
                    DealerPaymentCurrencyTypeId = policy.DealerPaymentCurrencyTypeId,
                    DealerPolicy = policy.DealerPolicy,
                    Discount = policy.Discount,
                    ExtensionTypeId = policy.ExtensionTypeId,
                    HrsUsedAtPolicySale = policy.HrsUsedAtPolicySale,
                    PolicyId = policy.Id,
                    IsApproved = policy.IsApproved,
                    IsPartialPayment = policy.IsPartialPayment,
                    IsPolicyCanceled = policy.IsPolicyCanceled,
                    IsPreWarrantyCheck = policy.IsPreWarrantyCheck,
                    IsSpecialDeal = policy.IsSpecialDeal,
                    //Month = policy.Month,
                    PaymentModeId = policy.PaymentModeId,
                    PolicyApprovedBy = "AutoApprovedByEndorsementApproval",
                    PolicyBundleId = policy.PolicyBundleId,
                    PolicyEndDate = policy.PolicyEndDate,
                    PolicyNo = policy.PolicyNo,
                    PolicySoldDate = policy.PolicySoldDate,
                    PolicyStartDate = policy.PolicyStartDate,
                    Premium = policy.Premium,
                    PremiumCurrencyTypeId = policy.PremiumCurrencyTypeId,
                    ProductId = policy.ProductId,
                    RefNo = policy.RefNo,
                    SalesPersonId = policy.SalesPersonId,
                    TPABranchId = policy.TPABranchId,

                    TransferFee = policy.TransferFee,
                    // Year = policy.Year,
                    Id = Guid.NewGuid(),
                    DateOfBirth = SqlDateTime.MinValue.Value,
                    ModifiedDate = DateTime.UtcNow,
                    DLIssueDate = SqlDateTime.MinValue.Value,
                    ItemPurchasedDate = SqlDateTime.MinValue.Value,

                };
                response.Add(policyHistory);
            }
            return response;
        }

        internal List<Policy> PolicyTransactionToPolicyList(List<PolicyTransaction> policyTransactionList,
            PolicyBundleTransaction bundleTransaction, List<Policy> PolicyList)
        {
            foreach (Policy policy in PolicyList)
            {
                PolicyTransaction relevantTransaction = policyTransactionList.Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                if (relevantTransaction == null)
                {
                    continue;
                }

                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                Guid getCurrencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                decimal rate = currencyEntityManager.GetConversionRate(relevantTransaction.DealerPaymentCurrencyTypeId,
                    getCurrencyPeriodId, true);

                policy.Comment = relevantTransaction.Comment;
                policy.CommodityTypeId = relevantTransaction.CommodityTypeId;
                policy.ContractId = relevantTransaction.ContractId;
                policy.CoverTypeId = relevantTransaction.CoverTypeId;
                policy.CustomerId = relevantTransaction.CustomerId;
                policy.CustomerPayment = relevantTransaction.CustomerPayment / rate;
                policy.CustomerPaymentCurrencyTypeId = relevantTransaction.CustomerPaymentCurrencyTypeId;
                policy.DealerId = relevantTransaction.DealerId;
                policy.DealerLocationId = relevantTransaction.DealerLocationId;
                policy.DealerPayment = relevantTransaction.DealerPayment / rate;
                policy.DealerPaymentCurrencyTypeId = relevantTransaction.DealerPaymentCurrencyTypeId;
                policy.DealerPolicy = relevantTransaction.DealerPolicy;
                policy.Discount = relevantTransaction.Discount;
                policy.ExtensionTypeId = relevantTransaction.ExtensionTypeId;
                policy.HrsUsedAtPolicySale = relevantTransaction.HrsUsedAtPolicySale;
                policy.Id = relevantTransaction.PolicyId;
                policy.IsApproved = policy.IsApproved;
                policy.IsPartialPayment = relevantTransaction.IsPartialPayment;
                policy.IsPreWarrantyCheck = relevantTransaction.IsPreWarrantyCheck;
                policy.IsSpecialDeal = relevantTransaction.IsSpecialDeal;
                policy.PaymentModeId = relevantTransaction.PaymentModeId;
                policy.PolicyBundleId = policy.PolicyBundleId;
                policy.PolicyEndDate = relevantTransaction.PolicyEndDate;
                policy.PolicyNo = relevantTransaction.PolicyNo;
                policy.PolicySoldDate = relevantTransaction.PolicySoldDate;
                policy.PolicyStartDate = relevantTransaction.PolicyStartDate;
                policy.Premium = relevantTransaction.Premium / rate;
                policy.PremiumCurrencyTypeId = PolicyList.FirstOrDefault().PremiumCurrencyTypeId;
                policy.ProductId = relevantTransaction.ProductId;
                policy.RefNo = relevantTransaction.RefNo;
                policy.SalesPersonId = relevantTransaction.SalesPersonId;
                policy.TransferFee = relevantTransaction.TransferFee;
                policy.ApprovedDate = DateTime.UtcNow;
                policy.Month = 0;
                policy.Year = 0;
                policy.BordxNumber = 0;
                policy.BordxId = Guid.Empty;
            }
            return PolicyList;
        }

        internal List<DataTransfer.Responses.BordxReportColumnHeaders> GetBordxReportColumnHeadersToDto(List<Entities.BordxReportColumnHeaders> columnHeaders)
        {
            List<DataTransfer.Responses.BordxReportColumnHeaders> Response = new List<DataTransfer.Responses.BordxReportColumnHeaders>();
            foreach (Entities.BordxReportColumnHeaders header in columnHeaders)
            {
                DataTransfer.Responses.BordxReportColumnHeaders responseHeader = new DataTransfer.Responses.BordxReportColumnHeaders()
                {
                    HeaderName = header.HeaderName,
                    Id = header.Id,
                    Sequance = header.Sequance,
                    GenarateSum = header.GenarateSum
                };
                Response.Add(responseHeader);
            }
            return Response;
        }

        internal List<DataTransfer.Responses.InvoiceCodeColumnHeaders> GetDealerInvoiceReportColumnHeadersToDto(List<Entities.DealerInvoicecodeReportColumnHeaders> columnHeaders)
        {
            List<DataTransfer.Responses.InvoiceCodeColumnHeaders> Response = new List<DataTransfer.Responses.InvoiceCodeColumnHeaders>();
            foreach (Entities.DealerInvoicecodeReportColumnHeaders header in columnHeaders)
            {
                DataTransfer.Responses.InvoiceCodeColumnHeaders responseHeader = new DataTransfer.Responses.InvoiceCodeColumnHeaders()
                {
                    HeaderName = header.HeaderName,
                    Id = header.Id,
                    Sequance = header.Sequance
                };
                Response.Add(responseHeader);
            }
            return Response;
        }


        internal List<DataTransfer.Responses.BordxReportColumns> GetBordxReportColumnsToDto(List<Entities.BordxReportColumns> columns)
        {
            List<DataTransfer.Responses.BordxReportColumns> Response = new List<DataTransfer.Responses.BordxReportColumns>();
            foreach (Entities.BordxReportColumns column in columns)
            {
                DataTransfer.Responses.BordxReportColumns ResponseColumn = new DataTransfer.Responses.BordxReportColumns()
                {
                    DisplayName = column.DisplayName,
                    HeaderId = column.HeaderId,
                    KeyName = column.KeyName,
                    Id = column.Id,
                    IsActive = column.IsActive,
                    Sequance = column.Sequance,
                    ColumnWidth = column.ColumnWidth,
                    Alignment = column.Alignment
                };
                Response.Add(ResponseColumn);
            }
            return Response;
        }

        internal BordxTaxInfo GetBordxTaxDetailsByCountryId(Guid countryId)
        {
            BordxTaxInfo Response = new BordxTaxInfo();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Response.CountryId = countryId;
                Response.CountryTaxes = new List<CountryTaxInfo>();
                List<CountryTaxes> countryTaxes = session.Query<CountryTaxes>().Where(a => a.CountryId == countryId).ToList();
                foreach (CountryTaxes countryTax in countryTaxes)
                {
                    CountryTaxInfo countryTaxInfo = new CountryTaxInfo()
                    {
                        TaxId = countryTax.Id,
                        OnGross = countryTax.IsOnGross,
                        OnNRP = countryTax.IsOnNRP,
                        IndexVal = countryTax.IndexVal,
                        IsOnPreviousTax = countryTax.IsOnPreviousTax,
                        IsPercentage = countryTax.IsPercentage,
                        MinimumValue = countryTax.MinimumValue,
                        TaxCode = cem.GetTaxCodeByTaxTypeId(countryTax.TaxTypeId),
                        TaxName = cem.GetTaxNameByTaxTypeId(countryTax.TaxTypeId),
                        TaxValue = countryTax.TaxValue

                    };
                    Response.CountryTaxes.Add(countryTaxInfo);
                }
            }
            catch (Exception)
            {
            }

            return Response;
        }

        internal List<DealerInvoiceReportColumnsResponseDto> DealerInvoiceReportColumnsToDealerInvoiceReportColumnsDto(List<DealerInvoiceReportColumns> ReportHeaderColumns)
        {
            List<DealerInvoiceReportColumnsResponseDto> Response = new List<DealerInvoiceReportColumnsResponseDto>();
            foreach (DealerInvoiceReportColumns rptColumns in ReportHeaderColumns)
            {
                var ReportColumnDto = new DealerInvoiceReportColumnsResponseDto()
                {
                    DisplayName = rptColumns.DisplayName,
                    Id = rptColumns.Id,
                    IsActive = rptColumns.IsActive,
                    KeyName = rptColumns.KeyName,
                    Sequance = rptColumns.Sequance
                };

                Response.Add(ReportColumnDto);
            }
            return Response;
        }

        private string GetModelCodeById(Guid Id)
        {
            String Response = String.Empty;

            try
            {
                if (IsGuid(Id.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    Model model = session.Query<Model>()
                        .Where(a => a.Id == Id).FirstOrDefault();
                    if (model != null)
                    {
                        Response = model.ModelCode;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
            //ISession session = EntitySessionManager.GetSession();
            //return session.Query<TAS.Services.Entities.Model>().Where(a => a.Id == guid).SingleOrDefault().ModelCode;
        }

        internal policyDetailsToInquiry PolicyDtoToInquiryPolicyDetails(PolicyResponseDto policyData, Customer CustomerData)
        {
            CommonEntityManager cem = new CommonEntityManager();
            policyDetailsToInquiry Response = new policyDetailsToInquiry();

            try
            {


                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();

                Nullable<bool> allTyresAreSame = null;


                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policyData.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                Policy policy = session.Query<Policy>().Where(a => a.Id == policyData.Id).FirstOrDefault();
                if (policy == null)
                {
                    return Response;
                }

                Guid policyId = policy.Id;
                if (commodityType.CommodityCode.ToLower().StartsWith("a"))
                {

                    // Gargash policy data

                    List<PolicyAdditionalDetailsResponseDto> policyadditionalDetailsList =
                                                                new List<PolicyAdditionalDetailsResponseDto>();

                    if(policyData.ProductCode == "ADT")
                    {
                        PolicyAdditionalDetails policyAdditionalDetails = session.Query<PolicyAdditionalDetails>()
                            .Where(n => n.PolicyId == policy.Id).FirstOrDefault();
                        PolicyAdditionalDetailsResponseDto policyAdditionalList = new PolicyAdditionalDetailsResponseDto()
                        {
                            Id = policyAdditionalDetails.Id,
                            PolicyId = policyAdditionalDetails.PolicyId,
                            FrontDot = policyAdditionalDetails.FrontDot,
                            FrontRadius = policyAdditionalDetails.FrontRadius,
                            FrontSpeedRating = policyAdditionalDetails.FrontSpeedRating,
                            FrontTyreProfile = policyAdditionalDetails.FrontTyreProfile,
                            FrontWidth = policyAdditionalDetails.FrontWidth,
                            NumberOfTyreCover = policyAdditionalDetails.NumberOfTyreCover,
                            RearDot = policyAdditionalDetails.RearDot,
                            RearTyreProfile = policyAdditionalDetails.RearTyreProfile,
                            RearRadius = policyAdditionalDetails.RearRadius,
                            RearSpeedRating = policyAdditionalDetails.RearSpeedRating,
                            RearWidth = policyAdditionalDetails.RearWidth,
                            TyreBrand = policyAdditionalDetails.TyreBrand
                        };
                        policyadditionalDetailsList.Add(policyAdditionalList);
                    }



                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();


                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>()
                        .FirstOrDefault(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId);
                    ManufacturerWarranty mw = null;
                    if (mwd != null)
                    {
                        mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    }

                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;
                    ExtensionType ExtensionType = session.Query<ExtensionType>().Where(a => a.Id == policy.ExtensionTypeId).FirstOrDefault();

                    ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                    InsuaranceLimitation IL = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();

                    int ExtensionTypeMonth = ExtensionType == null ? 0 : ExtensionType.Month;



                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Response = new policyDetailsToInquiry()
                    {
                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,

                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        DLIssueDate = CustomerData.DLIssueDate,
                        Email = CustomerData.Email,
                        FirstName = CustomerData.FirstName,
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : CustomerData.Gender.ToString().ToLower() == "f" ? "Female" : " ",
                        IDNo = CustomerData.IDNo,
                        IDType = cem.GetIdTypeNameById(CustomerData.IDTypeId),
                        LastName = CustomerData.LastName,
                        MobileNo = CustomerData.MobileNo,
                        NationalityId = CustomerData.NationalityId == 0 ? "0" : cem.GetNationaltyNameById(CustomerData.NationalityId),
                        //cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),

                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        Category = cem.GetCategoryNameById(ItemDetails.CategoryId),

                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        CommodityTypeCode = commodityType.CommodityCode,

                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),

                        CylinderCount = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),

                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        //DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DealerPayment = policyData.DealerPayment,
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = policyData.Discount,

                        DriveTypeId = ItemDetails.DriveTypeId == Guid.Empty ? "" : cem.GetDriverTypeByName(ItemDetails.DriveTypeId),

                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                        EngineNumber = ItemDetails.EngineNumber,
                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),

                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),

                        MakeId = cem.GetMakeNameById(ItemDetails.MakeId),

                        ModelId = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelYear = ItemDetails.ModelYear,
                        RefNo = policyData.RefNo,
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,

                        PlateNo = ItemDetails.PlateNo,
                        PolicyEndDate = policyData.PolicyEndDate.AddDays(1),
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PolicyNo = policyData.PolicyNo,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyStartDate = policyData.PolicyStartDate.AddDays(1),
                        ProductId = cem.GetProductNameById(policyData.ProductId),
                        ProductCode = policyData.ProductCode,
                        SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                        TransmissionId = cem.GetTransmissionTypeNameById(ItemDetails.TransmissionId),

                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        ModelCode = GetModelCodeById(ItemDetails.ModelId),
                        ////MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        ////MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        ////ExtensionStartDate = ItemDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1),
                        ExtensionStartDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(mwMonths)
                                                : policyData.PolicySoldDate,
                        ////ExtensionEndDate = ItemDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).AddMonths(ExtensionTypeMonth),
                        ExtensionEndDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(mwMonths)
                                                    .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1) : policyData.PolicySoldDate
                                                    .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1),
                        MWIsAvailable = policyData.MWIsAvailable,
                        GrossWeight = ItemDetails.GrossWeight,
                        Status = getPolicyStatus(policyData),
                        AdditionalTyreDetails = policyadditionalDetailsList

                    };

                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();


                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>()
                        .FirstOrDefault(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId);
                    ManufacturerWarranty mw = null;
                    if (mwd == null)
                    {
                        mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    }

                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;
                    ExtensionType ExtensionType = session.Query<ExtensionType>().Where(a => a.Id == policy.ExtensionTypeId).FirstOrDefault();

                    ContractInsuaranceLimitation CIL = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == Contract.Id).FirstOrDefault();
                    InsuaranceLimitation IL = session.Query<InsuaranceLimitation>().Where(a => a.CommodityTypeId == policy.CommodityTypeId && a.Id == CIL.InsuaranceLimitationId).FirstOrDefault();

                    int ExtensionTypeMonth = ExtensionType == null ? 0 : ExtensionType.Month;

                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Response = new policyDetailsToInquiry()
                    {
                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,

                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        DLIssueDate = CustomerData.DLIssueDate,
                        Email = CustomerData.Email,
                        FirstName = CustomerData.FirstName,
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : CustomerData.Gender.ToString().ToLower() == "f" ? "Female" : " ",
                        IDNo = CustomerData.IDNo,
                        IDType = cem.GetIdTypeNameById(CustomerData.IDTypeId),
                        LastName = CustomerData.LastName,
                        MobileNo = CustomerData.MobileNo,
                        NationalityId = CustomerData.NationalityId == 0 ? "0" : cem.GetNationaltyNameById(CustomerData.NationalityId),
                        //cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),

                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        Category = cem.GetCategoryNameById(ItemDetails.CategoryId),

                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        CommodityTypeCode = commodityType.CommodityCode,

                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),

                        CylinderCount = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),

                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        //DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DealerPayment = policyData.DealerPayment,
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = policyData.Discount,

                        DriveTypeId = ItemDetails.DriveTypeId == Guid.Empty ? "" : cem.GetDriverTypeByName(ItemDetails.DriveTypeId),

                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),

                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),

                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),

                        MakeId = cem.GetMakeNameById(ItemDetails.MakeId),

                        ModelId = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelYear = ItemDetails.ModelYear,
                        RefNo = policyData.RefNo,
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,

                        PlateNo = ItemDetails.PlateNo,
                        PolicyEndDate = policyData.PolicyEndDate.AddDays(1),
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PolicyNo = policyData.PolicyNo,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyStartDate = policyData.PolicyStartDate.AddDays(1),
                        ProductId = cem.GetProductNameById(policyData.ProductId),
                        SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                        TransmissionId = cem.GetTransmissionTypeNameById(ItemDetails.TransmissionId),

                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policy.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        ModelCode = GetModelCodeById(ItemDetails.ModelId),
                        ////MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        ////MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        ////ExtensionStartDate = ItemDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1),
                        ExtensionStartDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(mwMonths)
                                                : policyData.PolicySoldDate,
                        ////ExtensionEndDate = ItemDetails.ItemPurchasedDate.AddMonths(mwMonths).AddDays(1).AddMonths(ExtensionTypeMonth),
                        ExtensionEndDate = policyData.MWIsAvailable ? policyData.MWStartDate.AddMonths(mwMonths)
                                                    .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1) : policyData.PolicySoldDate
                                                    .AddMonths(IL == null ? 0 : IL.Months).AddDays(-1),
                        GrossWeight = ItemDetails.GrossWeight,
                        Status = getPolicyStatus(policyData)


                    };

                }
                else
                {
                    if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                    {
                        BAndWPolicy electronicPolicyMapping = session.Query<BAndWPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (electronicPolicyMapping == null)
                        {
                            return Response;
                        }

                        BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                        .Where(a => a.Id == electronicPolicyMapping.BAndWId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyDetailsToInquiry()
                        {
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,

                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,

                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            Category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            CommodityTypeCode = commodityType.CommodityCode,
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCount = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveTypeId = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : CustomerData.Gender.ToString().ToLower() == "f" ? "Female" : " ",
                            IDNo = CustomerData.IDNo,
                            IDType = cem.GetIdTypeNameById(CustomerData.IDTypeId),
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            LastName = CustomerData.LastName,
                            MakeId = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            ModelId = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelYear = ItemDetails.ModelYear,
                            RefNo = policyData.RefNo,
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            NationalityId = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PolicyNo = policyData.PolicyNo,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyStartDate = policyData.PolicyStartDate,
                            ProductId = cem.GetProductNameById(policyData.ProductId),
                            SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                            TransmissionId = String.Empty,
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            //VehiclePrice = String.Empty,
                            VINNo = String.Empty,
                            SerialNo = ItemDetails.SerialNo,
                            AddnSerialNo = ItemDetails.AddnSerialNo,
                            InvoiceNo = ItemDetails.InvoiceNo,
                            ModelCode = GetModelCodeById(ItemDetails.ModelId)
                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                    {
                        YellowGoodPolicy yellowgoodPolicyMapping = session.Query<YellowGoodPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (yellowgoodPolicyMapping == null)
                        {
                            return Response;
                        }

                        YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                        .Where(a => a.Id == yellowgoodPolicyMapping.YellowGoodId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyDetailsToInquiry()
                        {
                            AddnSerialNo = ItemDetails.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,

                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,

                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            Category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            CommodityTypeCode = commodityType.CommodityCode,
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCount = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveTypeId = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : CustomerData.Gender.ToString().ToLower() == "f" ? "Female" : " ",
                            IDNo = CustomerData.IDNo,
                            IDType = cem.GetIdTypeNameById(CustomerData.IDTypeId),
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            LastName = CustomerData.LastName,
                            MakeId = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            ModelId = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelYear = ItemDetails.ModelYear,
                            RefNo = policyData.RefNo,
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            NationalityId = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PolicyNo = policyData.PolicyNo,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyStartDate = policyData.PolicyStartDate,
                            ProductId = cem.GetProductNameById(policyData.ProductId),
                            SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                            TransmissionId = String.Empty,
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            //VehiclePrice = String.Empty,
                            VINNo = String.Empty,
                            SerialNo = ItemDetails.SerialNo,
                            InvoiceNo = ItemDetails.InvoiceNo,
                            ModelCode = GetModelCodeById(ItemDetails.ModelId)
                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                    {
                        OtherItemPolicy otherItemPolicyMapping = session.Query<OtherItemPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (otherItemPolicyMapping == null)
                        {
                            return Response;
                        }

                        string PlateNumber = String.Empty;
                        string cityForPlate = "N/A";

                        CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = null;
                        List<InvoiceCodeTireDetailsResponseDto> ListInvoiceCodeTireDetails = new List<InvoiceCodeTireDetailsResponseDto>();
                        List<PolicyContractTireProductResponseDto> contractTireProduct = new List<PolicyContractTireProductResponseDto>();

                        if (commodityType.CommodityTypeDescription == "Tyre")
                        {

                            List<Policy> Bundle = session.Query<Policy>().Where(a => a.PolicyBundleId == policy.PolicyBundleId).ToList();

                            foreach (var item in Bundle)
                            {

                                InvoiceCodeDetails invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                                   .Where(a => a.PolicyId == item.Id).FirstOrDefault();

                                InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == invoiceCodeDetails.InvoiceCodeId).FirstOrDefault();
                                PlateNumber = invoiceCode.PlateNumber;
                                if (invoiceCode.PlateRelatedCityId != null && invoiceCode.PlateRelatedCityId.ToString().Length > 0 && Guid.Empty != invoiceCode.PlateRelatedCityId)
                                {
                                    City city = session.Query<City>().Where(a => a.Id == invoiceCode.PlateRelatedCityId).FirstOrDefault();
                                    cityForPlate = city.CityName;
                                }


                                CurrencyEntityManager Currencyem = new CurrencyEntityManager();
                                allTyresAreSame = invoiceCode.AllTyresAreSame;
                                customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).FirstOrDefault();
                                InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                                    .Where(a => a.InvoiceCodeDetailId == invoiceCodeDetails.Id).FirstOrDefault();

                                List<InvoiceCodeTireDetails> ListInvoiceCodeTire = session.Query<InvoiceCodeTireDetails>()
                                    .Where(a => a.InvoiceCodeDetailId == invoiceCodeDetails.Id).ToList();

                                foreach (var IVTD in ListInvoiceCodeTire)
                                {

                                    AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>().Where(a => a.Id == IVTD.AvailableTireSizesPatternId).FirstOrDefault();
                                    string pattern = String.Empty;
                                    if (availableTireSizesPattern != null)
                                    {
                                        pattern = availableTireSizesPattern.Pattern;
                                    }
                                    InvoiceCodeTireDetailsResponseDto ot = new InvoiceCodeTireDetailsResponseDto()
                                    {
                                        Id = IVTD.Id,
                                        ArticleNumber = IVTD.ArticleNumber,
                                        CrossSection = IVTD.CrossSection,
                                        Diameter = IVTD.Diameter,
                                        InvoiceCodeDetailId = IVTD.InvoiceCodeDetailId,
                                        LoadSpeed = IVTD.LoadSpeed,
                                        Position = IVTD.Position,
                                        SerialNumber = IVTD.SerialNumber,
                                        Width = IVTD.Width,
                                        DotNumber = IVTD.DotNumber,
                                        Pattern = pattern,
                                        Price = IVTD.Price
                                    };

                                    ListInvoiceCodeTireDetails.Add(ot);
                                };

                                PolicyContractTireProductResponseDto p = new PolicyContractTireProductResponseDto()
                                {
                                    ContractId = item.ContractId,
                                    CoverTypeId = item.ContractExtensionPremiumId,
                                    ExtensionTypeId = item.ExtensionTypeId,
                                    AttributeSpecificationId = item.ContractInsuaranceLimitationId,
                                    ContractExtensionsId = item.ContractExtensionsId,
                                    Id = item.Id,
                                    Premium = Currencyem.ConvertFromBaseCurrency(item.Premium, item.PremiumCurrencyTypeId, item.CurrencyPeriodId),// item.Premium * CurrencyConversions.Rate,
                                    PremiumCurrencyTypeId = item.PremiumCurrencyTypeId,
                                    ProductId = item.ProductId,
                                    PolicyNo = item.PolicyNo,
                                    PolicyEndDate = item.PolicyEndDate,
                                    PolicyStartDate = item.PolicyStartDate,
                                    BookletNumber = item.BookletNumber,
                                    ArticleNumber = invoiceCodeTireDetails.ArticleNumber,
                                    SerialNumber = invoiceCodeTireDetails.SerialNumber,
                                    Position = invoiceCodeTireDetails.Position,
                                    InvoiceCodeId = invoiceCode.Id,
                                    NoOfDate = (item.PolicySoldDate - invoiceCode.GeneratedDate).TotalDays,
                                    VariantId = invoiceCodeDetails.VariantId
                                };
                                contractTireProduct.Add(p);
                            }

                        }
                        Make Make = new Make();
                        Model Model = new Model();

                        OtherItemDetails ItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == otherItemPolicyMapping.OtherItemId).FirstOrDefault();
                        Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                        Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                        AdditionalPolicyMakeData additionalPolicyMakeData = session.Query<AdditionalPolicyMakeData>().Where(a => a.Id == customerEnterdInvoiceDetails.AdditionalDetailsMakeId).FirstOrDefault();
                        AdditionalPolicyModelData additionalPolicyModelData = session.Query<AdditionalPolicyModelData>().Where(a => a.Id == customerEnterdInvoiceDetails.AdditionalDetailsModelId).FirstOrDefault();

                        UserAttachment userAttachment = session.Query<UserAttachment>().Where(ua => ua.Id == customerEnterdInvoiceDetails.InvoiceAttachmentId).FirstOrDefault();
                        string uploadedInvoiceFileName = String.Empty;
                        string uploadedInvoiceFileRef = String.Empty;
                        if (userAttachment != null)
                        {
                            uploadedInvoiceFileName = userAttachment.AttachmentFileName;
                            uploadedInvoiceFileRef = userAttachment.FileServerReference;
                        }
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyDetailsToInquiry()
                        {
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,

                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,

                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            Category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            CommodityTypeCode = commodityType.CommodityCode,
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCount = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveTypeId = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : CustomerData.Gender.ToString().ToLower() == "f" ? "Female" : " ",
                            IDNo = CustomerData.IDNo,
                            IDType = cem.GetIdTypeNameById(CustomerData.IDTypeId),
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatus = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            LastName = CustomerData.LastName,
                            MakeId = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            ModelId = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelYear = ItemDetails.ModelYear,
                            RefNo = policyData.RefNo,
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            NationalityId = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PlateNo = PlateNumber,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PolicyNo = policyData.PolicyNo,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyStartDate = policyData.PolicyStartDate,
                            ProductId = cem.GetProductNameById(policyData.ProductId),
                            SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                            TransmissionId = String.Empty,
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            //VehiclePrice = String.Empty,
                            VINNo = String.Empty,
                            SerialNo = ItemDetails.SerialNo,
                            AddnSerialNo = ItemDetails.AddnSerialNo,
                            InvoiceNo = ItemDetails.InvoiceNo,
                            ModelCode = GetModelCodeById(ItemDetails.ModelId),
                            Status = getPolicyStatus(policyData),
                            AllTyresAreSame = allTyresAreSame,
                            ContractTireProducts = contractTireProduct,
                            AditionalMake = additionalPolicyMakeData.MakeName,
                            AditionalModel = additionalPolicyModelData.ModelName,
                            AditionalMilage = Convert.ToString(customerEnterdInvoiceDetails.AdditionalDetailsMileage),
                            AditionalModelYear = Convert.ToString(customerEnterdInvoiceDetails.AdditionalDetailsModelYear),
                            AditionalCity = cityForPlate
                        };


                        Response.TireDetails = ListInvoiceCodeTireDetails;
                        Response.retBundle = contractTireProduct;

                    }
                    else
                    {
                        return Response;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return Response;
        }

        internal String getPolicyStatus(PolicyResponseDto policyData)
        {

             if (policyData.IsRenewed)
            {
                return "Policy Renewed";
            }
            else if (policyData.IsPolicyCanceled)
            {

                return "Policy Canceled";
            }
            else if(policyData.IsApproved)
            {
                return "Policy Approved";
            }
            else
                return "Policy Registered";

        }
        internal policyEndorsementInquiry PolicyDtoToInquiryEnrolmentDetails(PolicyTransaction policytransaction, PolicyResponseDto policyData, Customer CustomerData)
        {
            CommonEntityManager cem = new CommonEntityManager();
            policyEndorsementInquiry Response = new policyEndorsementInquiry();

            try
            {
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policyData.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                Policy policy = session.Query<Policy>().Where(a => a.Id == policyData.Id).FirstOrDefault();
                if (policy == null)
                {
                    return Response;
                }

                Guid policyId = policy.Id;

                Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                Make Make = session.Query<Make>().Where(a => a.Id == policytransaction.MakeId).FirstOrDefault();
                Model Model = session.Query<Model>().Where(a => a.Id == policytransaction.ModelId).FirstOrDefault();
                ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                            a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                //int ExtensionTypeMonth = ExtensionType == null ? 0 : ExtensionType.Month;

                if (commodityType.CommodityCode.ToLower().StartsWith("a"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Response = new policyEndorsementInquiry()
                    {
                        AddnSerialNo = policytransaction.AddnSerialNo,
                        Address1 = policytransaction.Address1,
                        Address2 = policytransaction.Address2,
                        Address3 = policytransaction.Address3,
                        Address4 = policytransaction.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(policytransaction.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(policytransaction.BodyTypeId),
                        BusinessAddress1 = policytransaction.BusinessAddress1,
                        BusinessAddress2 = policytransaction.BusinessAddress2,
                        BusinessAddress3 = policytransaction.BusinessAddress3,
                        BusinessAddress4 = policytransaction.BusinessAddress4,
                        BusinessName = policytransaction.BusinessName,
                        BusinessTelNo = policytransaction.BusinessTelNo,
                        City = cem.GetCityNameById(policytransaction.CityId),
                        Comment = policytransaction.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(policytransaction.CommodityTypeId),
                        Contract = cem.GetContractNameById(policytransaction.ContractId),
                        Category = cem.GetCategoryNameById(policytransaction.CategoryId),
                        Country = cem.GetCountryNameById(policytransaction.CountryId),
                        CoverType = cem.GetCoverTypeNameById(policytransaction.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policytransaction.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        CustomerType = cem.GetCustomerTypeNameById(policytransaction.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(policytransaction.CylinderCountId),
                        DateOfBirth = policytransaction.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policytransaction.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policytransaction.DealerLocationId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = policytransaction.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policytransaction.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DLIssueDate = policytransaction.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(policytransaction.DriveTypeId),
                        Email = policytransaction.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(policytransaction.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policytransaction.ExtensionTypeId),
                        FirstName = policytransaction.FirstName,
                        FuelType = cem.GetFuelTypeNameById(policytransaction.FuelTypeId),
                        Gender = policytransaction.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                        IDNo = policytransaction.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(policytransaction.IDTypeId),
                        InvoiceNo = policytransaction.InvoiceNo,
                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //ItemPrice = policytransaction.ItemPrice,
                        ItemPurchasedDate = policytransaction.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(policytransaction.ItemStatusId),
                        KmAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                        LastName = policytransaction.LastName,
                        Make = cem.GetMakeNameById(policytransaction.MakeId),
                        MobileNo = policytransaction.MobileNo,
                        Model = cem.GetModelNameById(policytransaction.ModelId),
                        ModelCode = policytransaction.ModelCode,
                        ModelYear = policytransaction.ModelYear,
                        Nationality = cem.GetNationaltyNameById(policytransaction.NationalityId),
                        OtherTelNo = policytransaction.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policytransaction.PaymentModeId),
                        PlateNo = policytransaction.PlateNo,
                        PolicyEndDate = policytransaction.PolicyEndDate,
                        PolicySoldDate = policytransaction.PolicySoldDate,
                        PolicyNo = policytransaction.PolicyNo,
                        PolicyStartDate = policytransaction.PolicyStartDate,
                        Premium = policytransaction.Premium,
                        Product = cem.GetProductNameById(policytransaction.ProductId),
                        RefNo = policytransaction.RefNo,
                        SalesPerson = cem.GetUserNameById(policytransaction.SalesPersonId),
                        SerialNo = policytransaction.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(policytransaction.TransmissionId),
                        UsageType = cem.GetUsageTypeNameById(policytransaction.UsageTypeId),
                        Variant = cem.GetVariantNameById(policytransaction.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = policytransaction.VINNo,
                        IsSpecialDeal = policytransaction.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        IsApproved = policytransaction.IsApproved == true ? "Approved" : "",
                        RegistrationDate = policytransaction.RegistrationDate,
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        Status = "Policy Endrosed",
                        //MWStartDate = mw == null ? "" : policytransaction.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policytransaction.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight,
                        IsPolicyEndrosed = "Policy Endrosed"
                    };
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Response = new policyEndorsementInquiry()
                    {
                        AddnSerialNo = policytransaction.AddnSerialNo,
                        Address1 = policytransaction.Address1,
                        Address2 = policytransaction.Address2,
                        Address3 = policytransaction.Address3,
                        Address4 = policytransaction.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(policytransaction.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(policytransaction.BodyTypeId),
                        BusinessAddress1 = policytransaction.BusinessAddress1,
                        BusinessAddress2 = policytransaction.BusinessAddress2,
                        BusinessAddress3 = policytransaction.BusinessAddress3,
                        BusinessAddress4 = policytransaction.BusinessAddress4,
                        BusinessName = policytransaction.BusinessName,
                        BusinessTelNo = policytransaction.BusinessTelNo,
                        City = cem.GetCityNameById(policytransaction.CityId),
                        Comment = policytransaction.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(policytransaction.CommodityTypeId),
                        Contract = cem.GetContractNameById(policytransaction.ContractId),
                        Category = cem.GetCategoryNameById(policytransaction.CategoryId),
                        Country = cem.GetCountryNameById(policytransaction.CountryId),
                        CoverType = cem.GetCoverTypeNameById(policytransaction.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policytransaction.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        CustomerType = cem.GetCustomerTypeNameById(policytransaction.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(policytransaction.CylinderCountId),
                        DateOfBirth = policytransaction.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policytransaction.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policytransaction.DealerLocationId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = policytransaction.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policytransaction.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DLIssueDate = policytransaction.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(policytransaction.DriveTypeId),
                        Email = policytransaction.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(policytransaction.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policytransaction.ExtensionTypeId),
                        FirstName = policytransaction.FirstName,
                        FuelType = cem.GetFuelTypeNameById(policytransaction.FuelTypeId),
                        Gender = policytransaction.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                        IDNo = policytransaction.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(policytransaction.IDTypeId),
                        InvoiceNo = policytransaction.InvoiceNo,
                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //ItemPrice = policytransaction.ItemPrice,
                        ItemPurchasedDate = policytransaction.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(policytransaction.ItemStatusId),
                        KmAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                        LastName = policytransaction.LastName,
                        Make = cem.GetMakeNameById(policytransaction.MakeId),
                        MobileNo = policytransaction.MobileNo,
                        Model = cem.GetModelNameById(policytransaction.ModelId),
                        ModelCode = policytransaction.ModelCode,
                        ModelYear = policytransaction.ModelYear,
                        Nationality = cem.GetNationaltyNameById(policytransaction.NationalityId),
                        OtherTelNo = policytransaction.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policytransaction.PaymentModeId),
                        PlateNo = policytransaction.PlateNo,
                        PolicyEndDate = policytransaction.PolicyEndDate,
                        PolicySoldDate = policytransaction.PolicySoldDate,
                        PolicyNo = policytransaction.PolicyNo,
                        PolicyStartDate = policytransaction.PolicyStartDate,
                        Premium = policytransaction.Premium,
                        Product = cem.GetProductNameById(policytransaction.ProductId),
                        RefNo = policytransaction.RefNo,
                        SalesPerson = cem.GetUserNameById(policytransaction.SalesPersonId),
                        SerialNo = policytransaction.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(policytransaction.TransmissionId),
                        UsageType = cem.GetUsageTypeNameById(policytransaction.UsageTypeId),
                        Variant = cem.GetVariantNameById(policytransaction.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = policytransaction.VINNo,
                        IsSpecialDeal = policytransaction.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        IsApproved = policytransaction.IsApproved == true ? "Approved" : "",
                        RegistrationDate = policytransaction.RegistrationDate,
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),

                        //MWStartDate = mw == null ? "" : policytransaction.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policytransaction.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight,
                        IsPolicyEndrosed = "Policy Endrosed"
                    };
                }
                else
                {
                    if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                    {
                        BAndWPolicy electronicPolicyMapping = session.Query<BAndWPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (electronicPolicyMapping == null)
                        {
                            return Response;
                        }

                        BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                        .Where(a => a.Id == electronicPolicyMapping.BAndWId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyEndorsementInquiry()
                        {

                            AddnSerialNo = policytransaction.AddnSerialNo,
                            Address1 = policytransaction.Address1,
                            Address2 = policytransaction.Address2,
                            Address3 = policytransaction.Address3,
                            Address4 = policytransaction.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = policytransaction.BusinessAddress1,
                            BusinessAddress2 = policytransaction.BusinessAddress2,
                            BusinessAddress3 = policytransaction.BusinessAddress3,
                            BusinessAddress4 = policytransaction.BusinessAddress4,
                            BusinessName = policytransaction.BusinessName,
                            BusinessTelNo = policytransaction.BusinessTelNo,
                            City = cem.GetCityNameById(policytransaction.CityId),
                            Comment = policytransaction.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(policytransaction.CommodityTypeId),
                            Contract = cem.GetContractNameById(policytransaction.ContractId),
                            Country = cem.GetCountryNameById(policytransaction.CountryId),
                            Category = cem.GetCategoryNameById(policytransaction.CategoryId),
                            CoverType = cem.GetCoverTypeNameById(policytransaction.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policytransaction.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(policytransaction.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = policytransaction.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policytransaction.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policytransaction.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policytransaction.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = policytransaction.DLIssueDate,
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DriveType = cem.GetDriverTypeByName(policytransaction.DriveTypeId),
                            Email = policytransaction.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policytransaction.ExtensionTypeId),
                            FirstName = policytransaction.FirstName,
                            FuelType = String.Empty,
                            Gender = policytransaction.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                            IDNo = policytransaction.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(policytransaction.IDTypeId),
                            InvoiceNo = policytransaction.InvoiceNo,
                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            //ItemPrice = policytransaction.ItemPrice,
                            ItemPurchasedDate = policytransaction.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(policytransaction.ItemStatusId),
                            KmAtPolicySale = String.Empty,
                            LastName = policytransaction.LastName,
                            Make = cem.GetMakeNameById(policytransaction.MakeId),
                            MobileNo = policytransaction.MobileNo,
                            Model = cem.GetModelNameById(policytransaction.ModelId),
                            ModelCode = policytransaction.ModelCode,
                            ModelYear = policytransaction.ModelYear,
                            Nationality = cem.GetNationaltyNameById(policytransaction.NationalityId),
                            OtherTelNo = policytransaction.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policytransaction.PaymentModeId),
                            PlateNo = policytransaction.PlateNo,
                            PolicyEndDate = policytransaction.PolicyEndDate,
                            PolicySoldDate = policytransaction.PolicySoldDate,
                            PolicyNo = policytransaction.PolicyNo,
                            PolicyStartDate = policytransaction.PolicyStartDate,
                            Premium = policytransaction.Premium,
                            Product = cem.GetProductNameById(policytransaction.ProductId),
                            RefNo = policytransaction.RefNo,
                            SalesPerson = cem.GetUserNameById(policytransaction.SalesPersonId),
                            SerialNo = policytransaction.SerialNo,
                            TransmissionId = String.Empty,
                            UsageType = cem.GetUsageTypeNameById(policytransaction.UsageTypeId),
                            Variant = String.Empty,
                            // VehiclePrice = string.Empty,
                            VINNo = policytransaction.VINNo,
                            IsSpecialDeal = policytransaction.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No"
                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                    {
                        YellowGoodPolicy yellowgoodPolicyMapping = session.Query<YellowGoodPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (yellowgoodPolicyMapping == null)
                        {
                            return Response;
                        }

                        YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                        .Where(a => a.Id == yellowgoodPolicyMapping.YellowGoodId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyEndorsementInquiry()
                        {

                            AddnSerialNo = policytransaction.AddnSerialNo,
                            Address1 = policytransaction.Address1,
                            Address2 = policytransaction.Address2,
                            Address3 = policytransaction.Address3,
                            Address4 = policytransaction.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = policytransaction.BusinessAddress1,
                            BusinessAddress2 = policytransaction.BusinessAddress2,
                            BusinessAddress3 = policytransaction.BusinessAddress3,
                            BusinessAddress4 = policytransaction.BusinessAddress4,
                            BusinessName = policytransaction.BusinessName,
                            BusinessTelNo = policytransaction.BusinessTelNo,
                            City = cem.GetCityNameById(policytransaction.CityId),
                            Comment = policytransaction.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(policytransaction.CommodityTypeId),
                            Contract = cem.GetContractNameById(policytransaction.ContractId),
                            Country = cem.GetCountryNameById(policytransaction.CountryId),
                            CoverType = cem.GetCoverTypeNameById(policytransaction.CoverTypeId),
                            Category = cem.GetCategoryNameById(policytransaction.CategoryId),
                            Customer = cem.GetCustomerNameById(policytransaction.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(policytransaction.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = policytransaction.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policytransaction.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policytransaction.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policytransaction.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = policytransaction.DLIssueDate,
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DriveType = cem.GetDriverTypeByName(policytransaction.DriveTypeId),
                            Email = policytransaction.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policytransaction.ExtensionTypeId),
                            FirstName = policytransaction.FirstName,
                            FuelType = String.Empty,
                            Gender = policytransaction.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                            IDNo = policytransaction.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(policytransaction.IDTypeId),
                            InvoiceNo = policytransaction.InvoiceNo,
                            ItemPrice = policytransaction.ItemPrice,
                            ItemPurchasedDate = policytransaction.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(policytransaction.ItemStatusId),
                            KmAtPolicySale = String.Empty,
                            LastName = policytransaction.LastName,
                            Make = cem.GetMakeNameById(policytransaction.MakeId),
                            MobileNo = policytransaction.MobileNo,
                            Model = cem.GetModelNameById(policytransaction.ModelId),
                            ModelCode = policytransaction.ModelCode,
                            ModelYear = policytransaction.ModelYear,
                            Nationality = cem.GetNationaltyNameById(policytransaction.NationalityId),
                            OtherTelNo = policytransaction.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policytransaction.PaymentModeId),
                            PlateNo = policytransaction.PlateNo,
                            PolicyEndDate = policytransaction.PolicyEndDate,
                            PolicySoldDate = policytransaction.PolicySoldDate,
                            PolicyNo = policytransaction.PolicyNo,
                            PolicyStartDate = policytransaction.PolicyStartDate,
                            Premium = policytransaction.Premium,
                            Product = cem.GetProductNameById(policytransaction.ProductId),
                            RefNo = policytransaction.RefNo,
                            SalesPerson = cem.GetUserNameById(policytransaction.SalesPersonId),
                            SerialNo = policytransaction.SerialNo,
                            TransmissionId = String.Empty,
                            UsageType = cem.GetUsageTypeNameById(policytransaction.UsageTypeId),
                            Variant = String.Empty,
                            //VehiclePrice = policytransaction.VehiclePrice,
                            VINNo = String.Empty,
                            IsSpecialDeal = policytransaction.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No"
                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                    {
                        OtherItemPolicy otherItemPolicyMapping = session.Query<OtherItemPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (otherItemPolicyMapping == null)
                        {
                            return Response;
                        }

                        OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                        .Where(a => a.Id == otherItemPolicyMapping.OtherItemId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyEndorsementInquiry()
                        {
                            AddnSerialNo = policytransaction.AddnSerialNo,
                            Address1 = policytransaction.Address1,
                            Address2 = policytransaction.Address2,
                            Address3 = policytransaction.Address3,
                            Address4 = policytransaction.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = policytransaction.BusinessAddress1,
                            BusinessAddress2 = policytransaction.BusinessAddress2,
                            BusinessAddress3 = policytransaction.BusinessAddress3,
                            BusinessAddress4 = policytransaction.BusinessAddress4,
                            BusinessName = policytransaction.BusinessName,
                            BusinessTelNo = policytransaction.BusinessTelNo,
                            City = cem.GetCityNameById(policytransaction.CityId),
                            Comment = policytransaction.Comment,
                            Category = cem.GetCategoryNameById(policytransaction.CategoryId),
                            CommodityType = cem.GetCommodityTypeNameById(policytransaction.CommodityTypeId),
                            Contract = cem.GetContractNameById(policytransaction.ContractId),
                            Country = cem.GetCountryNameById(policytransaction.CountryId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            CoverType = cem.GetCoverTypeNameById(policytransaction.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policytransaction.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(policytransaction.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = policytransaction.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policytransaction.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policytransaction.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policytransaction.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policytransaction.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = policytransaction.DLIssueDate,
                            DriveType = cem.GetDriverTypeByName(policytransaction.DriveTypeId),
                            Email = policytransaction.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policytransaction.ExtensionTypeId),
                            FirstName = policytransaction.FirstName,
                            FuelType = String.Empty,
                            Gender = policytransaction.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policytransaction.HrsUsedAtPolicySale,
                            IDNo = policytransaction.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(policytransaction.IDTypeId),
                            InvoiceNo = policytransaction.InvoiceNo,
                            ItemPrice = policytransaction.ItemPrice,
                            ItemPurchasedDate = policytransaction.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(policytransaction.ItemStatusId),
                            KmAtPolicySale = String.Empty,
                            LastName = policytransaction.LastName,
                            Make = cem.GetMakeNameById(policytransaction.MakeId),
                            MobileNo = policytransaction.MobileNo,
                            Model = cem.GetModelNameById(policytransaction.ModelId),
                            ModelCode = policytransaction.ModelCode,
                            ModelYear = policytransaction.ModelYear,
                            Nationality = cem.GetNationaltyNameById(policytransaction.NationalityId),
                            OtherTelNo = policytransaction.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policytransaction.PaymentModeId),
                            PlateNo = policytransaction.PlateNo,
                            PolicyEndDate = policytransaction.PolicyEndDate,
                            PolicySoldDate = policytransaction.PolicySoldDate,
                            PolicyNo = policytransaction.PolicyNo,
                            PolicyStartDate = policytransaction.PolicyStartDate,
                            Premium = policytransaction.Premium,
                            Product = cem.GetProductNameById(policytransaction.ProductId),
                            RefNo = policytransaction.RefNo,
                            SalesPerson = cem.GetUserNameById(policytransaction.SalesPersonId),
                            SerialNo = policytransaction.SerialNo,
                            TransmissionId = String.Empty,
                            UsageType = cem.GetUsageTypeNameById(policytransaction.UsageTypeId),
                            Variant = String.Empty,
                            //VehiclePrice = policytransaction.VehiclePrice,
                            VINNo = String.Empty,
                            IsSpecialDeal = policytransaction.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No"
                        };
                    }
                    else
                    {
                        return Response;
                    }
                }
            }
            catch (Exception) { }

            return Response;
        }

        internal policyCancelletionInquiry PolicyDtoToInquiryCancelationDetails(PolicyResponseDto policyData, Customer CustomerData, PolicyTransactionResponseDto PTResponse)
        {
            CommonEntityManager cem = new CommonEntityManager();
            policyCancelletionInquiry Response = new policyCancelletionInquiry();

            try
            {
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policyData.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                Policy policy = session.Query<Policy>().Where(a => a.Id == policyData.Id).FirstOrDefault();
                if (policy == null)
                {
                    return Response;
                }

                Guid policyId = policy.Id;
                if (commodityType.CommodityCode.ToLower().StartsWith("a"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                    ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                    Response = new policyCancelletionInquiry()
                    {

                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        Contract = cem.GetContractNameById(policyData.ContractId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        Category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = ItemDetails.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DLIssueDate = CustomerData.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(ItemDetails.DriveTypeId),
                        Email = CustomerData.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                        FirstName = CustomerData.FirstName,
                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                        IDNo = CustomerData.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                        LastName = CustomerData.LastName,
                        Make = cem.GetMakeNameById(ItemDetails.MakeId),
                        MobileNo = CustomerData.MobileNo,
                        Model = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelCode = PTResponse.ModelCode,
                        ModelYear = ItemDetails.ModelYear,
                        Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PlateNo = ItemDetails.PlateNo,
                        PolicyEndDate = policyData.PolicyEndDate,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyNo = policyData.PolicyNo,
                        PolicyStartDate = policyData.PolicyStartDate,
                        Premium = policyData.Premium,
                        ProductId = cem.GetProductNameById(policyData.ProductId),
                        RefNo = policyData.RefNo,
                        SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                        SerialNo = PTResponse.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransmissionId),
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        ModifiedDate = PTResponse.ModifiedDate,
                        ModifiedUser = cem.GetUserNameById(PTResponse.ModifiedUser),
                        CancelationComment = PTResponse.CancelationComment,
                        IsPolicyCancele = policyData.IsPolicyCanceled,
                        IsPolicyCanceled = "Policy Canceled",
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),

                        //MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight

                    };
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                    ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                    Response = new policyCancelletionInquiry()
                    {

                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        Contract = cem.GetContractNameById(policyData.ContractId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        Category = cem.GetCategoryNameById(ItemDetails.CategoryId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = ItemDetails.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        DLIssueDate = CustomerData.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(ItemDetails.DriveTypeId),
                        Email = CustomerData.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                        FirstName = CustomerData.FirstName,
                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                        IDNo = CustomerData.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                        LastName = CustomerData.LastName,
                        Make = cem.GetMakeNameById(ItemDetails.MakeId),
                        MobileNo = CustomerData.MobileNo,
                        Model = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelCode = PTResponse.ModelCode,
                        ModelYear = ItemDetails.ModelYear,
                        Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PlateNo = ItemDetails.PlateNo,
                        PolicyEndDate = policyData.PolicyEndDate,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyNo = policyData.PolicyNo,
                        PolicyStartDate = policyData.PolicyStartDate,
                        Premium = policyData.Premium,
                        ProductId = cem.GetProductNameById(policyData.ProductId),
                        RefNo = policyData.RefNo,
                        SalesPersonId = cem.GetUserNameById(policyData.SalesPersonId),
                        SerialNo = PTResponse.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransmissionId),
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        ModifiedDate = PTResponse.ModifiedDate,
                        ModifiedUser = cem.GetUserNameById(PTResponse.ModifiedUser),
                        CancelationComment = PTResponse.CancelationComment,
                        IsPolicyCancele = policyData.IsPolicyCanceled,
                        IsPolicyCanceled = "Policy Canceled",
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),

                        //MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight

                    };
                }
                else
                {
                    if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                    {
                        BAndWPolicy electronicPolicyMapping = session.Query<BAndWPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (electronicPolicyMapping == null)
                        {
                            return Response;
                        }

                        BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                        .Where(a => a.Id == electronicPolicyMapping.BAndWId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyCancelletionInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = policyData.CustomerPayment,
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ModifiedDate = PTResponse.ModifiedDate,
                            ModifiedUser = cem.GetUserNameById(PTResponse.ModifiedUser),
                            CancelationComment = PTResponse.CancelationComment,



                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                    {
                        YellowGoodPolicy yellowgoodPolicyMapping = session.Query<YellowGoodPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (yellowgoodPolicyMapping == null)
                        {
                            return Response;
                        }

                        YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                        .Where(a => a.Id == yellowgoodPolicyMapping.YellowGoodId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyCancelletionInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ModifiedDate = PTResponse.ModifiedDate,
                            ModifiedUser = cem.GetUserNameById(PTResponse.ModifiedUser),
                            CancelationComment = PTResponse.CancelationComment

                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                    {
                        OtherItemPolicy otherItemPolicyMapping = session.Query<OtherItemPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (otherItemPolicyMapping == null)
                        {
                            return Response;
                        }

                        OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                        .Where(a => a.Id == otherItemPolicyMapping.OtherItemId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyCancelletionInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                            ModifiedDate = PTResponse.ModifiedDate,
                            ModifiedUser = cem.GetUserNameById(PTResponse.ModifiedUser),
                            CancelationComment = PTResponse.CancelationComment
                        };
                    }
                    else
                    {
                        return Response;
                    }
                }
            }
            catch (Exception) { }
            return Response;
        }


        internal policyRenewalInquiry PolicyDtoToInquiryRenewalDetails(PolicyResponseDto policyData, Customer CustomerData, PolicyTransactionResponseDto PTResponse)
        {
            CommonEntityManager cem = new CommonEntityManager();
            policyRenewalInquiry Response = new policyRenewalInquiry();

            try
            {

                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policyData.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                Policy policy = session.Query<Policy>().Where(a => a.Id == policyData.Id).FirstOrDefault();
                if (policy == null)
                {
                    return Response;
                }

                Guid policyId = policy.Id;
                if (commodityType.CommodityCode.ToLower().StartsWith("a"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                    ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                    Response = new policyRenewalInquiry()
                    {

                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        Contract = cem.GetContractNameById(policyData.ContractId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                        CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = ItemDetails.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //Discount = policyData.Discount,
                        DLIssueDate = CustomerData.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(ItemDetails.DriveTypeId),
                        Email = CustomerData.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                        FirstName = CustomerData.FirstName,
                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                        IDNo = CustomerData.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                        LastName = CustomerData.LastName,
                        Make = cem.GetMakeNameById(ItemDetails.MakeId),
                        MobileNo = CustomerData.MobileNo,
                        Model = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelCode = PTResponse.ModelCode,
                        ModelYear = ItemDetails.ModelYear,
                        Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PlateNo = ItemDetails.PlateNo,
                        Product = cem.GetProductNameById(PTResponse.ProductId),
                        PolicyEndDate = policyData.PolicyEndDate,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyNo = policyData.PolicyNo,
                        PolicyStartDate = policyData.PolicyStartDate,
                        Premium = policyData.Premium,
                        ProductId = cem.GetProductNameById(PTResponse.ProductId),
                        RefNo = policyData.RefNo,
                        SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                        SerialNo = PTResponse.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransmissionId),
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        IsPolicyRenewed = "Policy Renewed",
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),

                        //MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight

                    };
                }
                else if (commodityType.CommodityCode.ToLower().StartsWith("b"))
                {
                    VehiclePolicy VehiclePolicyMapping = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policyId).FirstOrDefault();
                    if (VehiclePolicyMapping == null)
                    {
                        return Response;
                    }

                    VehicleDetails ItemDetails = session.Query<VehicleDetails>()
                        .Where(a => a.Id == VehiclePolicyMapping.VehicleId).FirstOrDefault();
                    if (ItemDetails == null)
                    {
                        return Response;
                    }

                    Contract Contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    Make Make = session.Query<Make>().Where(a => a.Id == ItemDetails.MakeId).FirstOrDefault();
                    Model Model = session.Query<Model>().Where(a => a.Id == ItemDetails.ModelId).FirstOrDefault();
                    ReinsurerContract ReinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == Contract.ReinsurerContractId).FirstOrDefault();
                    ManufacturerWarrantyDetails mwd = session.Query<ManufacturerWarrantyDetails>().Where(a => a.ModelId == Model.Id &&
                                                a.CountryId == ReinsurerContract.CountryId).FirstOrDefault();
                    ManufacturerWarranty mw = session.Query<ManufacturerWarranty>().Where(a => a.Id == mwd.ManufacturerWarrantyId && a.MakeId == Make.Id).FirstOrDefault();
                    int mwMonths = mw == null ? 0 : mw.WarrantyMonths;

                    Response = new policyRenewalInquiry()
                    {

                        Address1 = CustomerData.Address1,
                        Address2 = CustomerData.Address2,
                        Address3 = CustomerData.Address3,
                        Address4 = CustomerData.Address4,
                        Aspiration = cem.GetAspirationTypeNameById(ItemDetails.AspirationId),
                        BodyType = cem.GetBodyTypeNameById(ItemDetails.BodyTypeId),
                        BusinessAddress1 = CustomerData.BusinessAddress1,
                        BusinessAddress2 = CustomerData.BusinessAddress2,
                        BusinessAddress3 = CustomerData.BusinessAddress3,
                        BusinessAddress4 = CustomerData.BusinessAddress4,
                        BusinessName = CustomerData.BusinessName,
                        BusinessTelNo = CustomerData.BusinessTelNo,
                        City = cem.GetCityNameById(CustomerData.CityId),
                        Comment = policyData.Comment,
                        CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                        Contract = cem.GetContractNameById(policyData.ContractId),
                        Country = cem.GetCountryNameById(CustomerData.CountryId),
                        Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                        CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                        Customer = cem.GetCustomerNameById(policyData.CustomerId),
                        CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                        CylinderCountId = cem.GetCyllinderCountValueById(ItemDetails.CylinderCountId),
                        DateOfBirth = CustomerData.DateOfBirth,
                        Dealer = cem.GetDealerNameById(policyData.DealerId),
                        DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                        CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                        DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                        DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //DealerPrice = ItemDetails.DealerPrice,
                        DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        //Discount = policyData.Discount,
                        DLIssueDate = CustomerData.DLIssueDate,
                        DriveType = cem.GetDriverTypeByName(ItemDetails.DriveTypeId),
                        Email = CustomerData.Email,
                        EngineCapacity = cem.GetEngineCapacityNameById(ItemDetails.EngineCapacityId),
                        ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                        FirstName = CustomerData.FirstName,
                        FuelType = cem.GetFuelTypeNameById(ItemDetails.FuelTypeId),
                        Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                        HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                        IDNo = CustomerData.IDNo,
                        IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                        ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                        ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                        KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                        LastName = CustomerData.LastName,
                        Make = cem.GetMakeNameById(ItemDetails.MakeId),
                        MobileNo = CustomerData.MobileNo,
                        Model = cem.GetModelNameById(ItemDetails.ModelId),
                        ModelCode = PTResponse.ModelCode,
                        ModelYear = ItemDetails.ModelYear,
                        Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                        OtherTelNo = CustomerData.OtherTelNo,
                        PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                        PlateNo = ItemDetails.PlateNo,
                        Product = cem.GetProductNameById(PTResponse.ProductId),
                        PolicyEndDate = policyData.PolicyEndDate,
                        PolicySoldDate = policyData.PolicySoldDate,
                        PolicyNo = policyData.PolicyNo,
                        PolicyStartDate = policyData.PolicyStartDate,
                        Premium = policyData.Premium,
                        ProductId = cem.GetProductNameById(PTResponse.ProductId),
                        RefNo = policyData.RefNo,
                        SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                        SerialNo = PTResponse.SerialNo,
                        TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransmissionId),
                        UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                        Variant = cem.GetVariantNameById(ItemDetails.Variant),
                        VehiclePrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.VehiclePrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                        VINNo = ItemDetails.VINNo,
                        IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",
                        IsPolicyRenewed = "Policy Renewed",
                        MWStartDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.ToString("dd-MMM-yyyy"),
                        MWEndDate = mw == null ? policyData.PolicySoldDate.ToString("dd-MMM-yyyy") : policyData.MWStartDate.AddMonths(mw.WarrantyMonths).AddDays(-1).ToString("dd-MMM-yyyy"),

                        //MWStartDate = mw == null ? "" : policyData.PolicySoldDate.ToString("dd-MMM-yyyy"),
                        //MWEndDate = mw == null ? "" : policyData.PolicySoldDate.AddMonths(mwMonths).AddDays(-1).ToString("dd-MMM-yyyy"),
                        GrossWeight = ItemDetails.GrossWeight

                    };
                }
                else
                {
                    if (commodityType.CommodityCode.ToLower().StartsWith("e"))
                    {
                        BAndWPolicy electronicPolicyMapping = session.Query<BAndWPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (electronicPolicyMapping == null)
                        {
                            return Response;
                        }

                        BrownAndWhiteDetails ItemDetails = session.Query<BrownAndWhiteDetails>()
                        .Where(a => a.Id == electronicPolicyMapping.BAndWId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyRenewalInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",




                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("y"))
                    {
                        YellowGoodPolicy yellowgoodPolicyMapping = session.Query<YellowGoodPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (yellowgoodPolicyMapping == null)
                        {
                            return Response;
                        }

                        YellowGoodDetails ItemDetails = session.Query<YellowGoodDetails>()
                        .Where(a => a.Id == yellowgoodPolicyMapping.YellowGoodId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyRenewalInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No",


                        };
                    }
                    else if (commodityType.CommodityCode.ToLower().StartsWith("o"))
                    {
                        OtherItemPolicy otherItemPolicyMapping = session.Query<OtherItemPolicy>()
                       .Where(a => a.PolicyId == policyId).FirstOrDefault();
                        if (otherItemPolicyMapping == null)
                        {
                            return Response;
                        }

                        OtherItemDetails ItemDetails = session.Query<OtherItemDetails>()
                        .Where(a => a.Id == otherItemPolicyMapping.OtherItemId).FirstOrDefault();
                        if (ItemDetails == null)
                        {
                            return Response;
                        }

                        Response = new policyRenewalInquiry()
                        {
                            AddnSerialNo = PTResponse.AddnSerialNo,
                            Address1 = CustomerData.Address1,
                            Address2 = CustomerData.Address2,
                            Address3 = CustomerData.Address3,
                            Address4 = CustomerData.Address4,
                            Aspiration = String.Empty,
                            BodyType = String.Empty,
                            BusinessAddress1 = CustomerData.BusinessAddress1,
                            BusinessAddress2 = CustomerData.BusinessAddress2,
                            BusinessAddress3 = CustomerData.BusinessAddress3,
                            BusinessAddress4 = CustomerData.BusinessAddress4,
                            BusinessName = CustomerData.BusinessName,
                            BusinessTelNo = CustomerData.BusinessTelNo,
                            City = cem.GetCityNameById(CustomerData.CityId),
                            Comment = policyData.Comment,
                            CommodityType = cem.GetCommodityTypeNameById(commodityType.CommodityTypeId),
                            Contract = cem.GetContractNameById(policyData.ContractId),
                            Country = cem.GetCountryNameById(CustomerData.CountryId),
                            Category = cem.GetCategoryNameById(PTResponse.CategoryId),
                            CoverType = cem.GetCoverTypeNameById(policyData.CoverTypeId),
                            Customer = cem.GetCustomerNameById(policyData.CustomerId),
                            CustomerPayment = currencyEm.ConvertFromBaseCurrency(policyData.CustomerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerType = cem.GetCustomerTypeNameById(CustomerData.CustomerTypeId),
                            CylinderCountId = String.Empty,
                            DateOfBirth = CustomerData.DateOfBirth,
                            Dealer = cem.GetDealerNameById(policyData.DealerId),
                            DealerLocation = cem.GetDealerLocationNameById(policyData.DealerLocationId),
                            DealerPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.DealerPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            CustomerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.CustomerPaymentCurrencyTypeId),
                            DealerPaymentCurrencyTypeId = cem.GetCurrencyTypeByIdCode(policy.DealerPaymentCurrencyTypeId),
                            //DealerPrice = ItemDetails.DealerPrice,
                            DealerPayment = currencyEm.ConvertFromBaseCurrency(policyData.DealerPayment, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            Discount = currencyEm.ConvertFromBaseCurrency(policyData.Discount, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            DLIssueDate = CustomerData.DLIssueDate,
                            DriveType = String.Empty,
                            Email = CustomerData.Email,
                            EngineCapacity = String.Empty,
                            ExtensionType = cem.GetExtentionTypeNameById(policyData.ExtensionTypeId),
                            FirstName = CustomerData.FirstName,
                            FuelType = String.Empty,
                            Gender = CustomerData.Gender.ToString().ToLower() == "m" ? "Male" : "Female",
                            HrsUsedAtPolicySale = policyData.HrsUsedAtPolicySale,
                            IDNo = CustomerData.IDNo,
                            IDTypeId = cem.GetIdTypeNameById(CustomerData.IDTypeId),

                            ItemPrice = currencyEm.ConvertFromBaseCurrency(ItemDetails.ItemPrice, policyData.PremiumCurrencyTypeId, policy.CurrencyPeriodId),
                            ItemPurchasedDate = ItemDetails.ItemPurchasedDate,
                            ItemStatusId = cem.GetItemStatusNameById(ItemDetails.ItemStatusId),
                            KmAtPolicySale = policyData.HrsUsedAtPolicySale,
                            LastName = CustomerData.LastName,
                            Make = cem.GetMakeNameById(ItemDetails.MakeId),
                            MobileNo = CustomerData.MobileNo,
                            Model = cem.GetModelNameById(ItemDetails.ModelId),
                            ModelCode = PTResponse.ModelCode,
                            ModelYear = ItemDetails.ModelYear,
                            Nationality = cem.GetNationaltyNameById(CustomerData.NationalityId),
                            OtherTelNo = CustomerData.OtherTelNo,
                            PaymentModeId = cem.GetPaymentMethodNameById(policyData.PaymentModeId),
                            PlateNo = String.Empty,
                            PolicyEndDate = policyData.PolicyEndDate,
                            PolicySoldDate = policyData.PolicySoldDate,
                            PolicyNo = policyData.PolicyNo,
                            PolicyStartDate = policyData.PolicyStartDate,
                            Premium = policyData.Premium,
                            ProductId = cem.GetProductNameById(PTResponse.ProductId),
                            RefNo = policyData.RefNo,
                            SalesPersonId = cem.GetUserNameById(PTResponse.SalesPersonId),
                            SerialNo = PTResponse.SerialNo,
                            TransmissionId = cem.GetTransmissionTypeNameById(PTResponse.TransactionTypeId),
                            UsageTypeId = cem.GetUsageTypeNameById(CustomerData.UsageTypeId),
                            Variant = String.Empty,
                            InvoiceNo = PTResponse.InvoiceNo,
                            //VehiclePrice =  .Empty,
                            VINNo = String.Empty,
                            IsSpecialDeal = policyData.IsSpecialDeal.ToString().ToLower() == "0" ? "Yes" : "No"

                        };
                    }
                    else
                    {
                        return Response;
                    }
                }
            }
            catch (Exception)
            {
            }
            return Response;
        }



        internal ClaimSubmission ConvertClaimDataToClaimSubmissionRequest(Dealer claimDealer, ClaimSubmissionRequestDto claimSubmissionData, Policy policy, Guid currentConversionPeriodId)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimSubmission Response;
            try
            {
                CommonEntityManager commonEm = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                var newWip =
                            session.Query<ClaimSubmission>().Count(a => a.ClaimSubmittedDealerId == claimSubmissionData.dealerId) + 1;
                var DealerCode = new CommonEntityManager().GetDealerCodeById(claimSubmissionData.dealerId);



                //creating claim request
                ClaimSubmission claimRequest = new ClaimSubmission()
                {
                    ClaimCountryId = claimDealer.CountryId,
                    ClaimCurrencyId = claimDealer.CurrencyId,
                    ClaimNumber = "",
                    ClaimSubmittedBy = claimSubmissionData.requestedUserId,
                    ClaimSubmittedDealerId = claimDealer.Id,
                    CommodityCategoryId = commonEm.GetCommodityCategoryIdByContractId(policy.ContractId),
                    CommodityTypeId = policy.CommodityTypeId,
                    ConversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId),
                    CurrencyPeriodId = currentConversionPeriodId,
                    CustomerComplaint = claimSubmissionData.complaint == null ? "" : claimSubmissionData.complaint.customer,
                    CustomerId = policy.CustomerId,
                    DealerComment = claimSubmissionData.complaint == null ? "" : claimSubmissionData.complaint.dealer,
                    EntryDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    LastUpdatedBy = null,
                    LastUpdatedDate = DateTime.UtcNow,
                    MakeId = Guid.Parse(commonEm.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "make")),
                    ModelId = Guid.Parse(commonEm.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "model")),
                    PolicyCountryId = commonEm.GetDealerCuntryByDealerId(policy.DealerId),
                    PolicyDealerId = policy.DealerId,
                    PolicyId = policy.Id,
                    PolicyNumber = policy.PolicyNo,
                    StatusId = commonEm.GetClaimStatusIdByCode("sub"),
                    TotalClaimAmount = currencyEm.ConvertToBaseCurrency(claimSubmissionData.totalClaimAmount, claimDealer.CurrencyId, currentConversionPeriodId),
                    ClaimDate = claimSubmissionData.claimDate,
                    ClaimMileage = claimSubmissionData.claimMileage,
                    CustomerName = claimSubmissionData.policyDetails.customerName,
                    LastServiceDate = claimSubmissionData.policyDetails.lastServiceDate,
                    LastServiceMileage = claimSubmissionData.policyDetails.lastServiceMileage,
                    PlateNo = claimSubmissionData.policyDetails.plateNumber,
                    RepairCenter = claimDealer.DealerName,
                    RepairCenterLocation = commonEm.GetCountryNameById(claimDealer.CountryId),
                    VINNo = claimSubmissionData.policyDetails.vinNo,
                    FailureDate= claimSubmissionData.policyDetails.failureDate,
                    FailureMileage= claimSubmissionData.policyDetails.failureMileage,
                    Wip = DealerCode.ToUpper() + "/" + Convert.ToString(newWip).PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0')

            };
                Response = claimRequest;
            }
            catch (Exception)
            { throw; }
            return Response;
        }

        internal List<ClaimSubmissionItem> ConvertClaimDataToClaimItemListTire(ClaimOtherTireUpdateRequestDto claimData,
            Guid guid, Dealer claimDealer, Guid currentConversionPeriodId)
        {
            List<ClaimSubmissionItem> Response = new List<ClaimSubmissionItem>();
            try
            {
                //CommonEntityManager commonEm = new CommonEntityManager();
                //CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                //ISession session = EntitySessionManager.GetSession();

                //InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == claimData.InvoiceCodeId).FirstOrDefault();

                //InvoiceCodeDetails invoiceCodeDetails = session.Query<InvoiceCodeDetails>().FirstOrDefault();

                //if (invoiceCode.TireQuantity == 2)
                //{

                //}
            }
            catch (Exception)
            { throw; }
            return Response;
        }

        internal List<ClaimSubmissionItem> ConvertClaimDataToClaimItemList(ClaimSubmissionRequestDto claimSubmissionData, Guid claimRequestId,
            Dealer claimDealer, Guid currentConversionPeriodId)
        {
            List<ClaimSubmissionItem> Response = new List<ClaimSubmissionItem>();
            try
            {
                CommonEntityManager commonEm = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                int index = 0;
                foreach (var claimitem in claimSubmissionData.claimItemList)
                {

                    Guid? partId = claimitem.partId;
                    if (claimitem.partId == Guid.Empty)
                    {
                        partId = null;
                    }

                    var claimItem = new ClaimSubmissionItem()
                    {
                        ClaimItemTypeId = commonEm.GetClaimTypeIdByClaimTypeCode(claimitem.itemType),
                        ClaimSubmissionId = claimRequestId,
                        Id = Guid.NewGuid(),
                        ItemCode = claimitem.itemNumber,
                        ItemName = claimitem.itemName,
                        PartId = partId,
                        Quantity = claimitem.qty,
                        Remark = claimitem.remarks,
                        TotalPrice = currencyEm.ConvertToBaseCurrency(claimitem.totalPrice, claimDealer.CurrencyId, currentConversionPeriodId),
                        UnitPrice = currencyEm.ConvertToBaseCurrency(claimitem.unitPrice, claimDealer.CurrencyId, currentConversionPeriodId),
                        DiscountAmount = currencyEm.ConvertToBaseCurrency(claimitem.discountAmount, claimDealer.CurrencyId, currentConversionPeriodId),
                        DiscountRate = claimitem.discountRate,
                        IsDiscountPercentage = claimitem.isDiscountPercentage,
                        GoodWillAmount = currencyEm.ConvertToBaseCurrency(claimitem.goodWillAmount, claimDealer.CurrencyId, currentConversionPeriodId),
                        GoodWillRate = claimitem.goodWillRate,
                        IsGoodWillPercentage = claimitem.isGoodWillPercentage,
                        TotalGrossPrice = currencyEm.ConvertToBaseCurrency(claimitem.totalGrossPrice, claimDealer.CurrencyId, currentConversionPeriodId),
                        ParentId = claimitem.parentId
                    };
                    index++;
                    Response.Add(claimItem);
                }

            }
            catch (Exception)
            { throw; }
            return Response;
        }

        //private Guid GetParentId(ClaimItemList claimitem, List<ClaimSubmissionItem> submissionList, ClaimSubmissionRequestDto claimSubmissionData)
        //{
        //    Guid Response = Guid.Empty;

        //    Guid? parentPartId = claimSubmissionData.claimItemList.FirstOrDefault(a => a.id == claimitem.parentId).partId;
        //    foreach (ClaimSubmissionItem submissionItem in submissionList)
        //    {
        //        if (parentPartId == submissionItem.PartId)
        //        {
        //            Response = submissionItem.Id;
        //            break;
        //        }
        //    }
        //    return Response;
        //}

        internal Claim ClaimItemRequestToNewClaim(ClamItemRequestDto clamItemRequest, Policy policy)
        {
            Claim response;
            CommonEntityManager cem = new CommonEntityManager();

            try
            {
                Dealer policyDealer = cem.GetDealerById(policy.DealerId);
                String commodtyType = cem.GetCommodityTypeUniqueCodeById(policy.CommodityTypeId);
                object item = GetPolicyItemByPolicyId(policy.Id, commodtyType);
                #region "switch"

                Guid commodityCategoryId = Guid.Empty, makeId = Guid.Empty, modelId = Guid.Empty;
                switch (commodtyType.ToLower())
                {
                    case "a":
                        {
                            VehicleDetails vehicleDetails = item as VehicleDetails;
                            if (vehicleDetails != null)
                            {
                                commodityCategoryId = vehicleDetails.CategoryId;
                                makeId = vehicleDetails.MakeId;
                                modelId = vehicleDetails.ModelId;
                            }
                            break;
                        }
                    case "e":
                        {
                            BrownAndWhiteDetails brownAndWhiteDetails = item as BrownAndWhiteDetails;
                            if (brownAndWhiteDetails != null)
                            {
                                commodityCategoryId = brownAndWhiteDetails.CategoryId;
                                makeId = brownAndWhiteDetails.MakeId;
                                modelId = brownAndWhiteDetails.ModelId;

                            }
                            break;
                        }
                    case "o":
                        {
                            OtherItemDetails otherItemDetails = item as OtherItemDetails;
                            if (otherItemDetails != null)
                            {
                                commodityCategoryId = otherItemDetails.CategoryId;
                                makeId = otherItemDetails.MakeId;
                                modelId = otherItemDetails.ModelId;

                            }
                            break;
                        }
                    case "y":
                        {
                            YellowGoodDetails yellowGoodDetails = item as YellowGoodDetails;
                            if (yellowGoodDetails != null)
                            {
                                commodityCategoryId = yellowGoodDetails.CategoryId;
                                makeId = yellowGoodDetails.MakeId;
                                modelId = yellowGoodDetails.ModelId;
                            }
                            break;
                        }
                }

                #endregion
                response = new Claim()
                {
                    StatusId = cem.GetClaimStatusIdByCode("rev"),
                    Id = Guid.NewGuid(),
                    PolicyId = policy.Id,
                    ExamineBy = clamItemRequest.entryBy,
                    IsApproved = false,
                    ClaimCountryId = policyDealer.CountryId,
                    ClaimCurrencyId = policyDealer.CurrencyId,
                    ClaimSubmittedBy = clamItemRequest.entryBy,
                    ClaimSubmittedDealerId = policyDealer.Id,
                    CommodityCategoryId = commodityCategoryId,
                    CommodityTypeId = policy.CommodityTypeId,
                    CustomerId = policy.CustomerId,
                    EntryDate = DateTime.UtcNow,
                    IsBatching = false,
                    IsInvoiced = false,
                    MakeId = makeId,
                    ModelId = modelId,
                    PolicyCountryId = policyDealer.CountryId,
                    PolicyDealerId = policyDealer.Id,
                    PolicyNumber = policy.PolicyNo,
                    LastUpdatedDate = SqlDateTime.MinValue.Value,
                    ApprovedDate = SqlDateTime.MinValue.Value,
                    ClaimDate = SqlDateTime.MinValue.Value,
                    FailureDate = SqlDateTime.MinValue.Value




                };


            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        private object GetPolicyItemByPolicyId(Guid policyId, string commodtyType)
        {
            ISession session = EntitySessionManager.GetSession();
            object response = null;
            switch (commodtyType.ToLower())
            {
                case "a":
                    {
                        VehiclePolicy vehiclePolicy =
                            session.Query<VehiclePolicy>().FirstOrDefault(a => a.PolicyId == policyId);
                        if (vehiclePolicy != null)
                        {
                            VehicleDetails vehicleDetails =
                                session.Query<VehicleDetails>().FirstOrDefault(a => a.Id == vehiclePolicy.VehicleId);
                            response = vehicleDetails;
                        }

                        break;
                    }
                case "e":
                    {
                        BAndWPolicy bAndWPolicy =
                            session.Query<BAndWPolicy>().FirstOrDefault(a => a.PolicyId == policyId);
                        if (bAndWPolicy != null)
                        {
                            BrownAndWhiteDetails brownAndWhiteDetails =
                                session.Query<BrownAndWhiteDetails>().FirstOrDefault(a => a.Id == bAndWPolicy.BAndWId);
                            response = brownAndWhiteDetails;
                        }
                        break;
                    }
                case "y":
                    {
                        YellowGoodPolicy yellowGoodPolicy =
                            session.Query<YellowGoodPolicy>().FirstOrDefault(a => a.PolicyId == policyId);
                        if (yellowGoodPolicy != null)
                        {
                            YellowGoodDetails yellowGoodDetails =
                                session.Query<YellowGoodDetails>().FirstOrDefault(a => a.Id == yellowGoodPolicy.YellowGoodId);
                            response = yellowGoodDetails;
                        }
                        break;
                    }
                case "o":
                    {
                        OtherItemPolicy otherItemPolicy =
                           session.Query<OtherItemPolicy>().FirstOrDefault(a => a.PolicyId == policyId);
                        if (otherItemPolicy != null)
                        {
                            OtherItemDetails otherItemDetails =
                                session.Query<OtherItemDetails>().FirstOrDefault(a => a.Id == otherItemPolicy.OtherItemId);
                            response = otherItemDetails;
                        }
                        break;
                    }
                default:
                    break;
            }
            return response;
        }

        internal Entities.ClaimItem ClaimItemRequestToNewClaimItem(ClamItemRequestDto clamItemRequest, Claim newClaim)
        {
            Entities.ClaimItem response;
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currencyPeriod = currencyEm.GetCurrentCurrencyPeriodId();
            Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == clamItemRequest.dealerId);
            if (dealer == null)
            {
                throw new Exception("Selected dealer is invalid.");
            }
            else if (dealer.CurrencyId == Guid.Empty)
            {
                throw new Exception("Selected dealer's currency not set.");
            }

            response = new Entities.ClaimItem()
            {
                ParentId = Guid.Empty,
                Id = clamItemRequest.serverId,
                IsDiscountPercentage = clamItemRequest.isDiscountPercentage,
                DiscountAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.discountAmount, dealer.CurrencyId, currencyPeriod),
                DiscountRate = clamItemRequest.isDiscountPercentage ? clamItemRequest.discountRate : currencyEm.ConvertToBaseCurrency(clamItemRequest.discountRate, dealer.CurrencyId, currencyPeriod),
                ClaimItemTypeId = cem.GetClaimTypeIdByClaimTypeCode(clamItemRequest.itemType),
                IsApproved = clamItemRequest.status.ToLower() == "a" ? true : false,
                GoodWillRate = clamItemRequest.isGoodWillPercentage ? clamItemRequest.goodWillRate : currencyEm.ConvertToBaseCurrency(clamItemRequest.goodWillRate, dealer.CurrencyId, currencyPeriod),
                TotalGrossPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.totalGrossPrice, dealer.CurrencyId, currencyPeriod),
                IsGoodWillPercentage = clamItemRequest.isGoodWillPercentage,
                GoodWillAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.goodWillAmount, dealer.CurrencyId, currencyPeriod),
                PartId = clamItemRequest.partId == Guid.Empty ? null : clamItemRequest.partId,
                AuthorizedAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.authorizedAmt, dealer.CurrencyId, currencyPeriod),
                ClaimId = newClaim.Id,
                FaultId = clamItemRequest.faultId == Guid.Empty ? null : clamItemRequest.faultId,
                ItemCode = clamItemRequest.itemNumber,
                ItemName = clamItemRequest.itemName,
                Quantity = clamItemRequest.qty,
                Remark = clamItemRequest.remarks,
                TotalPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.totalPrice, dealer.CurrencyId, currencyPeriod),
                TpaComment = clamItemRequest.comment,
                UnitPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.unitPrice, dealer.CurrencyId, currencyPeriod),
                ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                CurrencyPeriodId = currencyPeriod,
                RejectionTypeId = clamItemRequest.rejectionTypeId == Guid.Empty ? null : clamItemRequest.rejectionTypeId,
                CauseOfFailureId = clamItemRequest.causeOfFailureId,

                DiscountSchemeId = clamItemRequest.discountSchemeId,
                DiscountSchemeCode = new CommonEntityManager().GetDiscountSchemeCodeById(clamItemRequest.discountSchemeId),

            };
            return response;
        }

        internal Claim DealerClaimToTpaClaim(ClamItemRequestDto clamItemRequest)
        {
            Claim response;
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currencyPeriod = currencyEm.GetCurrentCurrencyPeriodId();
            if (currencyPeriod == Guid.Empty)
            {
                throw new CurrencyPeriodNotSetException("Currency period not set.");
            }

            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                .FirstOrDefault(a => a.Id == clamItemRequest.claimId);
            if (claimSubmission == null)
            {
                throw new Exception("Invalid claim selection.");
            }

            Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == claimSubmission.ClaimSubmittedDealerId);
            if (dealer == null)
            {
                throw new Exception("Selected dealer is invalid.");
            }
            else if (dealer.CurrencyId == Guid.Empty)
            {
                throw new Exception("Selected dealer's currency not set.");
            }

            response = new Claim()
            {
                Id = Guid.NewGuid(),
                CommodityTypeId = claimSubmission.CommodityTypeId,
                ClaimCountryId = claimSubmission.ClaimCountryId,
                StatusId = cem.GetClaimStatusIdByCode("rev"),
                PolicyCountryId = claimSubmission.PolicyCountryId,
                IsApproved = false,
                ClaimCurrencyId = claimSubmission.ClaimCurrencyId,
                ClaimSubmittedBy = claimSubmission.ClaimSubmittedBy,
                ClaimSubmittedDealerId = claimSubmission.ClaimSubmittedDealerId,
                CommodityCategoryId = claimSubmission.CommodityCategoryId,
                CustomerComplaint = claimSubmission.CustomerComplaint,
                CustomerId = claimSubmission.CustomerId,
                DealerComment = claimSubmission.DealerComment,
                EntryDate = DateTime.UtcNow,
                ExamineBy = clamItemRequest.entryBy,
                MakeId = claimSubmission.MakeId,
                ModelId = claimSubmission.ModelId,
                PolicyDealerId = claimSubmission.PolicyDealerId,
                PolicyId = claimSubmission.PolicyId,
                PolicyNumber = claimSubmission.PolicyNumber,
                IsBatching = false,
                IsInvoiced = false,
                LastUpdatedDate = (DateTime)SqlDateTime.MinValue,
                CurrencyPeriodId = currencyPeriod,
                ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                GroupId = null,
                LastUpdatedBy = null,
                ApprovedDate = (DateTime)SqlDateTime.MinValue,
                ClaimDate = claimSubmission.ClaimDate,
                FailureDate = claimSubmission.FailureDate,
                ClaimMileageKm = claimSubmission.ClaimMileage,
                IsEndorsed = false,
                IsGoodwill = false,
                ClaimSubmissionId = claimSubmission.Id,
            };
            return response;
        }

        internal List<Entities.ClaimItem> DealerClaimItemsToTpaClaimItems(ClamItemRequestDto clamItemRequest, Claim newClaim)
        {
            List<Entities.ClaimItem> claimItems = new List<Entities.ClaimItem>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                CommonEntityManager commminEm = new CommonEntityManager();
                if (!IsGuid(clamItemRequest.faultId.ToString()))
                {
                    clamItemRequest.faultId = null;
                }

                List<ClaimSubmissionItem> claimSubmissionItems = session.Query<ClaimSubmissionItem>()
                    .Where(a => a.ClaimSubmissionId == clamItemRequest.claimId && a.PartId == clamItemRequest.partId && a.Id== clamItemRequest.serverId).ToList();
                claimItems.AddRange(claimSubmissionItems.Select(claimSubmissionItem => new Entities.ClaimItem()
                {
                    Id = claimSubmissionItem.Id,
                    ParentId = claimSubmissionItem.ParentId,
                    IsApproved = null,
                    IsDiscountPercentage = claimSubmissionItem.IsDiscountPercentage,
                    DiscountAmount = claimSubmissionItem.DiscountAmount,
                    DiscountRate = claimSubmissionItem.DiscountRate,
                    GoodWillRate = claimSubmissionItem.GoodWillRate,
                    IsGoodWillPercentage = claimSubmissionItem.IsGoodWillPercentage,
                    GoodWillAmount = claimSubmissionItem.GoodWillAmount,
                    ClaimItemTypeId = claimSubmissionItem.ClaimItemTypeId,
                    ClaimId = newClaim.Id,
                    ItemCode = claimSubmissionItem.ItemCode,
                    ItemName = claimSubmissionItem.ItemName,
                    PartId = claimSubmissionItem.PartId,
                    Quantity = clamItemRequest.qty,
                    Remark = claimSubmissionItem.Remark,
                    TotalPrice = currencyEM.ConvertToBaseCurrency(clamItemRequest.totalPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                    TotalGrossPrice = currencyEM.ConvertToBaseCurrency(clamItemRequest.totalGrossPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                    UnitPrice = currencyEM.ConvertToBaseCurrency(clamItemRequest.unitPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                    ConversionRate = newClaim.ConversionRate,
                    CurrencyPeriodId = newClaim.CurrencyPeriodId,
                    AuthorizedAmount = currencyEM.ConvertToBaseCurrency(clamItemRequest.authorizedAmt, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                }));
                //new item process
                if (!IsGuid(clamItemRequest.serverId.ToString()))
                {
                    claimItems.Add(new Entities.ClaimItem()
                    {

                        IsApproved = true,
                        IsDiscountPercentage = clamItemRequest.isDiscountPercentage,
                        DiscountAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.discountAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        DiscountRate = clamItemRequest.isDiscountPercentage ? clamItemRequest.discountRate : currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.discountRate, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        GoodWillRate = clamItemRequest.isGoodWillPercentage ? clamItemRequest.goodWillRate : currencyEM.ConvertToBaseCurrency(
                              clamItemRequest.goodWillRate, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        TotalGrossPrice = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.totalGrossPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        IsGoodWillPercentage = clamItemRequest.isGoodWillPercentage,
                        GoodWillAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.goodWillAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode(clamItemRequest.itemType),
                        ClaimId = newClaim.Id,
                        ItemCode = clamItemRequest.itemNumber,
                        ItemName = clamItemRequest.itemName,
                        PartId = clamItemRequest.partId,
                        Quantity = clamItemRequest.qty,
                        Remark = clamItemRequest.remarks,
                        TpaComment = clamItemRequest.comment,
                        TotalPrice = currencyEM.ConvertToBaseCurrency(clamItemRequest.totalPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        UnitPrice = currencyEM.ConvertToBaseCurrency(
                               clamItemRequest.unitPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        ConversionRate = newClaim.ConversionRate,
                        CurrencyPeriodId = newClaim.CurrencyPeriodId,
                        AuthorizedAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.authorizedAmt, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                        FaultId = clamItemRequest.faultId,
                        DiscountSchemeId = clamItemRequest.discountSchemeId,
                        DiscountSchemeCode = commminEm.GetDiscountSchemeCodeById(clamItemRequest.discountSchemeId),
                        RejectionTypeId = clamItemRequest.rejectionTypeId == Guid.Empty ? null : clamItemRequest.rejectionTypeId,

                    });
                }
                //excisting item change details
                foreach (Entities.ClaimItem claimItem in claimItems)
                {
                    if (IsGuid(clamItemRequest.serverId.ToString()))
                    {
                        if (claimItem.Id == clamItemRequest.serverId)
                        {
                            claimItem.AuthorizedAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.authorizedAmt, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.TotalPrice = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.totalPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.DiscountAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.discountAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.DiscountRate = clamItemRequest.isDiscountPercentage ? clamItemRequest.discountRate : currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.discountRate, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);

                            claimItem.GoodWillAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.goodWillAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.GoodWillRate = clamItemRequest.isGoodWillPercentage ? clamItemRequest.goodWillRate : currencyEM.ConvertToBaseCurrency(
                              clamItemRequest.goodWillRate, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.IsApproved = clamItemRequest.status.ToLower() == "a" ? true : false;
                            claimItem.IsDiscountPercentage = clamItemRequest.isDiscountPercentage;
                            claimItem.IsGoodWillPercentage = clamItemRequest.isGoodWillPercentage;
                            claimItem.Quantity = clamItemRequest.qty;
                            claimItem.TotalGrossPrice = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.totalGrossPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.TpaComment = clamItemRequest.comment;
                            claimItem.UnitPrice = currencyEM.ConvertToBaseCurrency(
                               clamItemRequest.unitPrice, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.AuthorizedAmount = currencyEM.ConvertToBaseCurrency(
                                clamItemRequest.authorizedAmt, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId);
                            claimItem.FaultId = clamItemRequest.faultId;
                        }
                    }

                    // claimItem.Id = Guid.NewGuid();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return claimItems;
        }
        internal List<ClaimAttachment> DealerClaimAttachementsToClaimAttachemnts(ClamItemRequestDto clamItemRequest, Claim newClaim)
        {
            List<ClaimAttachment> claimAttachments = new List<ClaimAttachment>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<ClaimSubmissionAttachment> claimSubmissionAttachments = session.Query<ClaimSubmissionAttachment>()
                    .Where(a => a.ClaimSubmissionId == clamItemRequest.claimId).ToList();
                foreach (ClaimSubmissionAttachment claimSubmissionAttachment in claimSubmissionAttachments)
                {
                    ClaimAttachment claimAttachment = new ClaimAttachment()
                    {
                        Id = Guid.NewGuid(),
                        ClaimId = newClaim.Id,
                        UserAttachmentId = claimSubmissionAttachment.UserAttachmentId,
                        DateOfAttachment = DateTime.UtcNow

                };
                    claimAttachments.Add(claimAttachment);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return claimAttachments;
        }

        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
            {
                return false;
            }

            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }



        internal Claim ClaimSubmissionToClaim(ClaimSubmission claimSubmission)
        {
            Claim response = new Claim();
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                response = new Claim()
                {
                    Id = Guid.NewGuid(),
                    ClaimCountryId = claimSubmission.ClaimCountryId,
                    ClaimCurrencyId = claimSubmission.ClaimCurrencyId,
                    CommodityCategoryId = claimSubmission.CommodityCategoryId,
                    CommodityTypeId = claimSubmission.CommodityTypeId,
                    ConversionRate = claimSubmission.ConversionRate,
                    CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                    CustomerComplaint = claimSubmission.CustomerComplaint,
                    CustomerId = claimSubmission.CustomerId,
                    EntryDate = DateTime.UtcNow,
                    IsApproved = false,
                    LastUpdatedDate = DateTime.UtcNow,
                    MakeId = claimSubmission.MakeId,
                    ModelId = claimSubmission.ModelId,
                    PolicyCountryId = claimSubmission.PolicyCountryId,
                    PolicyId = claimSubmission.PolicyId,
                    StatusId = cem.GetClaimStatusIdByCode("rev"),
                    PolicyNumber = claimSubmission.PolicyNumber,
                    PolicyDealerId = claimSubmission.PolicyDealerId,
                    ClaimSubmittedDealerId = claimSubmission.ClaimSubmittedDealerId,
                    DealerComment = claimSubmission.DealerComment,
                    IsInvoiced = false,
                    ApprovedDate = SqlDateTime.MinValue.Value,
                    ClaimDate = claimSubmission.ClaimDate < SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : claimSubmission.ClaimDate,
                    FailureDate = claimSubmission.FailureDate


                };
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal List<Entities.ClaimItem> ClaimSubmissionItemsToClaimItems(ClaimSubmission claimSubmission, AddLabourChargeToClaimRequestDto addLabourChargeToClaimRequestDto)
        {
            List<Entities.ClaimItem> response = new List<Entities.ClaimItem>();
            try
            {
                CurrencyEntityManager CurrencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                session.Query<ClaimSubmissionItem>()
                    .Where(a => a.ClaimSubmissionId == addLabourChargeToClaimRequestDto.claimId).ForEach(
                    a => response.Add(new Entities.ClaimItem()
                    {
                        Id = Guid.NewGuid(),
                        TotalGrossPrice = a.TotalGrossPrice,
                        DiscountAmount = a.DiscountAmount,
                        AuthorizedAmount = decimal.Zero,
                        TotalPrice = a.TotalPrice,
                        GoodWillRate = a.GoodWillRate,
                        ParentId = a.ParentId,
                        IsGoodWillPercentage = a.IsGoodWillPercentage,
                        FaultId = null,
                        ClaimId = addLabourChargeToClaimRequestDto.claimId,
                        TpaComment = string.Empty,
                        DiscountRate = a.DiscountRate,
                        IsApproved = false,
                        GoodWillAmount = a.GoodWillAmount,
                        IsDiscountPercentage = a.IsDiscountPercentage,
                        ConversionRate = claimSubmission.ConversionRate,
                        CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                        ClaimItemTypeId = a.ClaimItemTypeId,
                        PartId = null,
                        Quantity = a.Quantity,
                        UnitPrice = a.UnitPrice,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        Remark = a.Remark,

                    }));


                //add new labour charge
                decimal grossAmt = addLabourChargeToClaimRequestDto.labourCharge.chargeType == "H"
                    ? (addLabourChargeToClaimRequestDto.labourCharge.hourlyRate *
                       addLabourChargeToClaimRequestDto.labourCharge.hours)
                    : addLabourChargeToClaimRequestDto.labourCharge.hourlyRate;
                var claimItem = new Entities.ClaimItem()
                {
                    Id = Guid.NewGuid(),
                    TotalGrossPrice =
                        CurrencyEm.ConvertToBaseCurrency(grossAmt, claimSubmission.ClaimCurrencyId,
                            claimSubmission.CurrencyPeriodId),
                    DiscountAmount =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.discountAmount,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    AuthorizedAmount =
                        CurrencyEm.ConvertToBaseCurrency(
                            grossAmt -
                            (addLabourChargeToClaimRequestDto.labourCharge.discountAmount +
                             addLabourChargeToClaimRequestDto.labourCharge.goodWillAmount),
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    TotalPrice =
                        CurrencyEm.ConvertToBaseCurrency(
                            grossAmt -
                            (addLabourChargeToClaimRequestDto.labourCharge.discountAmount +
                             addLabourChargeToClaimRequestDto.labourCharge.goodWillAmount),
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    GoodWillRate = addLabourChargeToClaimRequestDto.labourCharge.goodWillType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.goodWillValue,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId) : addLabourChargeToClaimRequestDto.labourCharge.goodWillValue,
                    ParentId = addLabourChargeToClaimRequestDto.labourCharge.partId,
                    IsGoodWillPercentage = addLabourChargeToClaimRequestDto.labourCharge.goodWillType != "F",
                    FaultId = null,
                    ClaimId = addLabourChargeToClaimRequestDto.claimId,
                    TpaComment = string.Empty,
                    DiscountRate = addLabourChargeToClaimRequestDto.labourCharge.discountType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.discountValue,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId) : addLabourChargeToClaimRequestDto.labourCharge.discountValue,
                    IsApproved = true,
                    GoodWillAmount =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.goodWillAmount,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    IsDiscountPercentage = addLabourChargeToClaimRequestDto.labourCharge.discountType != "F",

                    ConversionRate = claimSubmission.ConversionRate,
                    CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                    ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("l"),
                    PartId = null,
                    Quantity = addLabourChargeToClaimRequestDto.labourCharge.hours == 0 ? 1 : addLabourChargeToClaimRequestDto.labourCharge.hours,
                    UnitPrice =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.hourlyRate,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    ItemCode = addLabourChargeToClaimRequestDto.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                    ItemName = "Labour Charge",
                    Remark = "Added By Claim Engineer",
                };

                response.Add(claimItem);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal Entities.ClaimItem LabourChargeToClaimItem(Claim claim, AddLabourChargeToClaimRequestDto addLabourChargeToClaimRequestDto)
        {
            Entities.ClaimItem response = new Entities.ClaimItem();
            try
            {
                CurrencyEntityManager CurrencyEm = new CurrencyEntityManager();
                //add new labour charge
                decimal grossAmt = addLabourChargeToClaimRequestDto.labourCharge.chargeType == "H"
                    ? (addLabourChargeToClaimRequestDto.labourCharge.hourlyRate *
                       addLabourChargeToClaimRequestDto.labourCharge.hours)
                    : addLabourChargeToClaimRequestDto.labourCharge.hourlyRate;
                response = new Entities.ClaimItem()
                {
                    Id = Guid.NewGuid(),
                    TotalGrossPrice =
                        CurrencyEm.ConvertToBaseCurrency(grossAmt, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    DiscountAmount =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.discountAmount,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                    AuthorizedAmount =
                        CurrencyEm.ConvertToBaseCurrency(
                            addLabourChargeToClaimRequestDto.labourCharge.authorizedAmount,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                    TotalPrice =
                        CurrencyEm.ConvertToBaseCurrency(
                            grossAmt -
                            (addLabourChargeToClaimRequestDto.labourCharge.discountAmount +
                             addLabourChargeToClaimRequestDto.labourCharge.goodWillAmount),
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                    GoodWillRate = addLabourChargeToClaimRequestDto.labourCharge.goodWillType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.goodWillValue,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId) : addLabourChargeToClaimRequestDto.labourCharge.goodWillValue,
                    ParentId = addLabourChargeToClaimRequestDto.labourCharge.parentId,
                    IsGoodWillPercentage = addLabourChargeToClaimRequestDto.labourCharge.goodWillType != "F",
                    FaultId = null,
                    ClaimId = addLabourChargeToClaimRequestDto.claimId,
                    TpaComment = string.Empty,
                    DiscountRate = addLabourChargeToClaimRequestDto.labourCharge.discountType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.discountValue,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId) : addLabourChargeToClaimRequestDto.labourCharge.discountValue,
                    IsApproved = true,
                    GoodWillAmount =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.goodWillAmount,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                    IsDiscountPercentage = addLabourChargeToClaimRequestDto.labourCharge.discountType != "F",

                    ConversionRate = claim.ConversionRate,
                    CurrencyPeriodId = claim.CurrencyPeriodId,
                    ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("l"),
                    PartId = null,
                    Quantity = addLabourChargeToClaimRequestDto.labourCharge.hours,
                    UnitPrice =
                        CurrencyEm.ConvertToBaseCurrency(addLabourChargeToClaimRequestDto.labourCharge.hourlyRate,
                            claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                    ItemCode = addLabourChargeToClaimRequestDto.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                    ItemName = "Labour Charge",
                    Remark = addLabourChargeToClaimRequestDto.labourCharge.description,
                    DiscountSchemeId = addLabourChargeToClaimRequestDto.labourCharge.discountSchemeId,
                    DiscountSchemeCode = addLabourChargeToClaimRequestDto.labourCharge.discountSchemeName,
                    RejectionTypeId = null
                };
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal ClaimEndorsement ClaimSubmissionToClaimEndorsement(Claim claimSubmission)
        {
            ClaimEndorsement response = new ClaimEndorsement();
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                response = new ClaimEndorsement()
                {
                    Id = Guid.NewGuid(),
                    ClaimCountryId = claimSubmission.ClaimCountryId,
                    ClaimCurrencyId = claimSubmission.ClaimCurrencyId,
                    CommodityCategoryId = claimSubmission.CommodityCategoryId,
                    CommodityTypeId = claimSubmission.CommodityTypeId,
                    ConversionRate = claimSubmission.ConversionRate,
                    CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                    CustomerComplaint = claimSubmission.CustomerComplaint,
                    CustomerId = claimSubmission.CustomerId,
                    EntryDate = DateTime.UtcNow,
                    IsApproved = false,
                    LastUpdatedDate = DateTime.UtcNow,
                    MakeId = claimSubmission.MakeId,
                    ModelId = claimSubmission.ModelId,
                    PolicyCountryId = claimSubmission.PolicyCountryId,
                    PolicyId = claimSubmission.PolicyId,
                    StatusId = cem.GetClaimStatusIdByCode("rev"),
                    PolicyNumber = claimSubmission.PolicyNumber,
                    PolicyDealerId = claimSubmission.PolicyDealerId,
                    ClaimSubmittedDealerId = claimSubmission.ClaimSubmittedDealerId,
                    DealerComment = claimSubmission.DealerComment,
                    IsInvoiced = false,

                };
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal List<ClaimEndorsementItem> ClaimSubmissionItemsToClaimEndorsementItem(Claim claimSubmission, ClaimEndorsementItemRequestDto claimEndorsementLInfo)
        {
            List<ClaimEndorsementItem> response = new List<ClaimEndorsementItem>();
            try
            {
                CurrencyEntityManager CurrencyEm = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                session.Query<ClaimSubmissionItem>()
                    .Where(a => a.ClaimSubmissionId == claimEndorsementLInfo.claimId).ForEach(
                    a => response.Add(new ClaimEndorsementItem()
                    {
                        Id = Guid.NewGuid(),
                        TotalGrossPrice = a.TotalGrossPrice,
                        DiscountAmount = a.DiscountAmount,
                        AuthorizedAmount = decimal.Zero,
                        TotalPrice = a.TotalPrice,
                        GoodWillRate = a.GoodWillRate,
                        ParentId = a.ParentId,
                        IsGoodWillPercentage = a.IsGoodWillPercentage,
                        FaultId = null,
                        ClaimId = claimEndorsementLInfo.claimId,
                        TpaComment = string.Empty,
                        DiscountRate = a.DiscountRate,
                        IsApproved = false,
                        GoodWillAmount = a.GoodWillAmount,
                        IsDiscountPercentage = a.IsDiscountPercentage,
                        ConversionRate = claimSubmission.ConversionRate,
                        CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                        ClaimItemTypeId = a.ClaimItemTypeId,
                        PartId = null,
                        Quantity = a.Quantity,
                        UnitPrice = a.UnitPrice,
                        ItemCode = a.ItemCode,
                        ItemName = a.ItemName,
                        Remark = a.Remark
                    }));


                //add new labour charge
                decimal grossAmt = claimEndorsementLInfo.itemType == "H"
                    ? (claimEndorsementLInfo.hourlyRate *
                       claimEndorsementLInfo.hours)
                    : claimEndorsementLInfo.hourlyRate;
                var claimEndorsementItem = new ClaimEndorsementItem()
                {
                    Id = Guid.NewGuid(),
                    TotalGrossPrice = CurrencyEm.ConvertToBaseCurrency(grossAmt, claimSubmission.ClaimCurrencyId,
                            claimSubmission.CurrencyPeriodId),
                    DiscountAmount = CurrencyEm.ConvertToBaseCurrency(claimEndorsementLInfo.discountAmount,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    AuthorizedAmount = CurrencyEm.ConvertToBaseCurrency(grossAmt -
                            (claimEndorsementLInfo.discountAmount + claimEndorsementLInfo.goodWillAmount),
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    TotalPrice = CurrencyEm.ConvertToBaseCurrency(grossAmt -
                            (claimEndorsementLInfo.discountAmount +
                             claimEndorsementLInfo.goodWillAmount),
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    GoodWillRate = claimEndorsementLInfo.goodWillType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(claimEndorsementLInfo.goodWillRate,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId) : claimEndorsementLInfo.goodWillRate,
                    ParentId = claimEndorsementLInfo.partId,
                    IsGoodWillPercentage = claimEndorsementLInfo.goodWillType != "F",
                    FaultId = null,
                    ClaimId = claimEndorsementLInfo.claimId,
                    TpaComment = string.Empty,
                    DiscountRate = claimEndorsementLInfo.discountType == "F" ?
                        CurrencyEm.ConvertToBaseCurrency(claimEndorsementLInfo.discountRate,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId) : claimEndorsementLInfo.discountRate,
                    IsApproved = true,
                    GoodWillAmount =
                        CurrencyEm.ConvertToBaseCurrency(claimEndorsementLInfo.goodWillAmount,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    IsDiscountPercentage = claimEndorsementLInfo.discountType != "F",

                    ConversionRate = claimSubmission.ConversionRate,
                    CurrencyPeriodId = claimSubmission.CurrencyPeriodId,
                    ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("l"),
                    PartId = null,
                    Quantity = claimEndorsementLInfo.hours == 0 ? 1 : claimEndorsementLInfo.hours,
                    UnitPrice =
                        CurrencyEm.ConvertToBaseCurrency(claimEndorsementLInfo.hourlyRate,
                            claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId),
                    ItemCode = claimEndorsementLInfo.itemType == "H" ? "Hourly" : "Fixed",
                    ItemName = "Labour Charge",
                    Remark = "Added By Claim Engineer",
                };

                response.Add(claimEndorsementItem);
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal ClaimEndorsementItem LabourChargeToClaimEndorsementItem(ClaimEndorsement claimEndorsement, AddLabourChargeToClaimEndorsementRequestDto AddLabourChargeToClaimEndorsementRequestDto)
        {
            ClaimEndorsementItem response = new ClaimEndorsementItem();
            try
            {
                CurrencyEntityManager CurrencyEm = new CurrencyEntityManager();
                //add new labour charge
                decimal grossAmt = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.chargeType == "H"
                    ? (AddLabourChargeToClaimEndorsementRequestDto.labourCharge.hourlyRate *
                       AddLabourChargeToClaimEndorsementRequestDto.labourCharge.hours)
                    : AddLabourChargeToClaimEndorsementRequestDto.labourCharge.hourlyRate;
                response = new ClaimEndorsementItem()
                {
                    Id = Guid.NewGuid(),
                    TotalGrossPrice =
                        CurrencyEm.ConvertToBaseCurrency(grossAmt, claimEndorsement.ClaimCurrencyId,
                            claimEndorsement.CurrencyPeriodId),
                    DiscountAmount =
                        CurrencyEm.ConvertToBaseCurrency(AddLabourChargeToClaimEndorsementRequestDto.labourCharge.discountAmount,
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    AuthorizedAmount =
                        CurrencyEm.ConvertToBaseCurrency(
                            grossAmt -
                            (AddLabourChargeToClaimEndorsementRequestDto.labourCharge.discountAmount +
                             AddLabourChargeToClaimEndorsementRequestDto.labourCharge.goodWillAmount),
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    TotalPrice =
                        CurrencyEm.ConvertToBaseCurrency(
                            grossAmt -
                            (AddLabourChargeToClaimEndorsementRequestDto.labourCharge.discountAmount +
                             AddLabourChargeToClaimEndorsementRequestDto.labourCharge.goodWillAmount),
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    GoodWillRate =
                        CurrencyEm.ConvertToBaseCurrency(AddLabourChargeToClaimEndorsementRequestDto.labourCharge.goodWillValue,
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    ParentId = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.partId,
                    IsGoodWillPercentage = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.goodWillType != "F",
                    FaultId = null,
                    ClaimId = AddLabourChargeToClaimEndorsementRequestDto.claimId,
                    TpaComment = string.Empty,
                    DiscountRate =
                        CurrencyEm.ConvertToBaseCurrency(AddLabourChargeToClaimEndorsementRequestDto.labourCharge.discountValue,
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    IsApproved = true,
                    GoodWillAmount =
                        CurrencyEm.ConvertToBaseCurrency(AddLabourChargeToClaimEndorsementRequestDto.labourCharge.goodWillAmount,
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    IsDiscountPercentage = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.discountType != "F",

                    ConversionRate = claimEndorsement.ConversionRate,
                    CurrencyPeriodId = claimEndorsement.CurrencyPeriodId,
                    ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("l"),
                    PartId = null,
                    Quantity = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.hours,
                    UnitPrice =
                        CurrencyEm.ConvertToBaseCurrency(AddLabourChargeToClaimEndorsementRequestDto.labourCharge.hourlyRate,
                            claimEndorsement.ClaimCurrencyId, claimEndorsement.CurrencyPeriodId),
                    ItemCode = AddLabourChargeToClaimEndorsementRequestDto.labourCharge.chargeType == "H" ? "Hourly" : "Fixed",
                    ItemName = "Labour Charge",
                    Remark = "Added By Claim Engineer"
                };
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        internal ClaimEndorsement ClaimEndorsementItemRequestToNewClaim(ClaimEndorsementItemRequestDto claimEndorsementItemRequest, Policy policy)
        {
            ClaimEndorsement response;
            CommonEntityManager cem = new CommonEntityManager();

            try
            {
                Dealer policyDealer = cem.GetDealerById(policy.DealerId);
                String commodtyType = cem.GetCommodityTypeUniqueCodeById(policy.CommodityTypeId);
                object item = GetPolicyItemByPolicyId(policy.Id, commodtyType);
                #region "switch"

                Guid commodityCategoryId = Guid.Empty, makeId = Guid.Empty, modelId = Guid.Empty;
                switch (commodtyType.ToLower())
                {
                    case "a":
                        {
                            VehicleDetails vehicleDetails = item as VehicleDetails;
                            if (vehicleDetails != null)
                            {
                                commodityCategoryId = vehicleDetails.CategoryId;
                                makeId = vehicleDetails.MakeId;
                                modelId = vehicleDetails.ModelId;
                            }
                            break;
                        }
                    case "e":
                        {
                            BrownAndWhiteDetails brownAndWhiteDetails = item as BrownAndWhiteDetails;
                            if (brownAndWhiteDetails != null)
                            {
                                commodityCategoryId = brownAndWhiteDetails.CategoryId;
                                makeId = brownAndWhiteDetails.MakeId;
                                modelId = brownAndWhiteDetails.ModelId;

                            }
                            break;
                        }
                    case "o":
                        {
                            OtherItemDetails otherItemDetails = item as OtherItemDetails;
                            if (otherItemDetails != null)
                            {
                                commodityCategoryId = otherItemDetails.CategoryId;
                                makeId = otherItemDetails.MakeId;
                                modelId = otherItemDetails.ModelId;

                            }
                            break;
                        }
                    case "y":
                        {
                            YellowGoodDetails yellowGoodDetails = item as YellowGoodDetails;
                            if (yellowGoodDetails != null)
                            {
                                commodityCategoryId = yellowGoodDetails.CategoryId;
                                makeId = yellowGoodDetails.MakeId;
                                modelId = yellowGoodDetails.ModelId;
                            }
                            break;
                        }
                }

                #endregion
                response = new ClaimEndorsement()
                {
                    StatusId = cem.GetClaimStatusIdByCode("REV"),
                    Id = Guid.NewGuid(),
                    PolicyId = claimEndorsementItemRequest.policyId,
                    ExamineBy = claimEndorsementItemRequest.entryBy,
                    IsApproved = false,
                    ClaimCountryId = policyDealer.CountryId,
                    ClaimCurrencyId = policyDealer.CurrencyId,
                    ClaimSubmittedBy = claimEndorsementItemRequest.entryBy,
                    ClaimSubmittedDealerId = policyDealer.Id,
                    CommodityCategoryId = commodityCategoryId,
                    CommodityTypeId = policy.CommodityTypeId,
                    CustomerId = policy.CustomerId,
                    EntryDate = DateTime.UtcNow,
                    IsBatching = false,
                    IsInvoiced = false,
                    MakeId = makeId,
                    ModelId = modelId,
                    PolicyCountryId = policyDealer.CountryId,
                    PolicyDealerId = claimEndorsementItemRequest.dealerId,
                    PolicyNumber = policy.PolicyNo,
                    LastUpdatedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    ApprovedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    IsRejected = false,
                    EndorsementIsApproved = false,
                    EndorsementApprovedOrRejectedBy = Guid.Empty,
                    EndorsementApprovedOrRejectedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    ClaimId = claimEndorsementItemRequest.claimId,


                };
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal ClaimEndorsementItem ClaimEndorsementItemRequestToNewClaimItem(ClaimEndorsementItemRequestDto claimEndorsementItemRequest, ClaimEndorsement newClaim)
        {
            ClaimEndorsementItem response;
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currencyPeriod = currencyEm.GetCurrentCurrencyPeriodId();
            Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == claimEndorsementItemRequest.dealerId);
            if (dealer == null)
            {
                throw new Exception("Selected dealer is invalid.");
            }
            else if (dealer.CurrencyId == Guid.Empty)
            {
                throw new Exception("Selected dealer's currency not set.");
            }

            response = new ClaimEndorsementItem()
            {
                ParentId = Guid.Empty,
                Id = Guid.NewGuid(),
                IsDiscountPercentage = claimEndorsementItemRequest.isDiscountPercentage,
                DiscountAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.discountAmount, dealer.CurrencyId, currencyPeriod),
                DiscountRate = claimEndorsementItemRequest.isDiscountPercentage ? claimEndorsementItemRequest.discountRate : currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.discountRate, dealer.CurrencyId, currencyPeriod),
                ClaimItemTypeId = cem.GetClaimTypeIdByClaimTypeCode(claimEndorsementItemRequest.itemType),
                IsApproved = claimEndorsementItemRequest.status.ToLower() == "a" ? true : false,
                GoodWillRate = claimEndorsementItemRequest.isGoodWillPercentage ? claimEndorsementItemRequest.goodWillRate : currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.goodWillRate, dealer.CurrencyId, currencyPeriod),
                TotalGrossPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.totalGrossPrice, dealer.CurrencyId, currencyPeriod),
                IsGoodWillPercentage = claimEndorsementItemRequest.isGoodWillPercentage,
                GoodWillAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.goodWillAmount, dealer.CurrencyId, currencyPeriod),
                PartId = claimEndorsementItemRequest.partId == Guid.Empty ? null : claimEndorsementItemRequest.partId,
                AuthorizedAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.authorizedAmt, dealer.CurrencyId, currencyPeriod),
                ClaimId = claimEndorsementItemRequest.claimId,
                FaultId = claimEndorsementItemRequest.faultId == Guid.Empty ? null : claimEndorsementItemRequest.faultId,
                ItemCode = claimEndorsementItemRequest.itemNumber,
                ItemName = claimEndorsementItemRequest.itemName,
                Quantity = claimEndorsementItemRequest.qty,
                Remark = claimEndorsementItemRequest.remarks,
                TotalPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.totalPrice, dealer.CurrencyId, currencyPeriod),
                TpaComment = claimEndorsementItemRequest.comment,
                UnitPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementItemRequest.unitPrice, dealer.CurrencyId, currencyPeriod),
                ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                CurrencyPeriodId = currencyPeriod
            };
            return response;
        }

        internal ClaimEndorsementItem ClaimItemRequestToNewClaimEndorsementItem(ClaimEndorsementItemRequestDto clamItemRequest, ClaimEndorsement newClaim)
        {
            ClaimEndorsementItem response;
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currencyPeriod = currencyEm.GetCurrentCurrencyPeriodId();
            Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == clamItemRequest.dealerId);
            if (dealer == null)
            {
                throw new Exception("Selected dealer is invalid.");
            }
            else if (dealer.CurrencyId == Guid.Empty)
            {
                throw new Exception("Selected dealer's currency not set.");
            }

            response = new ClaimEndorsementItem()
            {
                ParentId = Guid.Empty,
                Id = Guid.NewGuid(),
                IsDiscountPercentage = clamItemRequest.isDiscountPercentage,
                DiscountAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.discountAmount, dealer.CurrencyId, currencyPeriod),
                DiscountRate = clamItemRequest.isDiscountPercentage ? clamItemRequest.discountRate : currencyEm.ConvertToBaseCurrency(clamItemRequest.discountRate, dealer.CurrencyId, currencyPeriod),
                ClaimItemTypeId = cem.GetClaimTypeIdByClaimTypeCode(clamItemRequest.itemType),
                IsApproved = clamItemRequest.status.ToLower() == "a" ? true : false,
                GoodWillRate = clamItemRequest.isGoodWillPercentage ? clamItemRequest.goodWillRate : currencyEm.ConvertToBaseCurrency(clamItemRequest.goodWillRate, dealer.CurrencyId, currencyPeriod),
                TotalGrossPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.totalGrossPrice, dealer.CurrencyId, currencyPeriod),
                IsGoodWillPercentage = clamItemRequest.isGoodWillPercentage,
                GoodWillAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.goodWillAmount, dealer.CurrencyId, currencyPeriod),
                PartId = clamItemRequest.partId == Guid.Empty ? null : clamItemRequest.partId,
                AuthorizedAmount = currencyEm.ConvertToBaseCurrency(clamItemRequest.authorizedAmt, dealer.CurrencyId, currencyPeriod),
                ClaimId = newClaim.Id,
                FaultId = clamItemRequest.faultId == Guid.Empty ? null : clamItemRequest.faultId,
                ItemCode = clamItemRequest.itemNumber,
                ItemName = clamItemRequest.itemName,
                Quantity = clamItemRequest.qty,
                Remark = clamItemRequest.remarks,
                TotalPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.totalPrice, dealer.CurrencyId, currencyPeriod),
                TpaComment = clamItemRequest.comment,
                UnitPrice = currencyEm.ConvertToBaseCurrency(clamItemRequest.unitPrice, dealer.CurrencyId, currencyPeriod),
                ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                CurrencyPeriodId = currencyPeriod
            };
            return response;
        }



        internal ClaimEndorsement ClaimEndorsementToTpaClaim(ClaimEndorsementItemRequestDto claimEndorsementInfo, Policy policy)
        {
            ClaimEndorsement response;
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();




            try
            {
                Dealer policyDealer = cem.GetDealerById(policy.DealerId);
                String commodtyType = cem.GetCommodityTypeUniqueCodeById(policy.CommodityTypeId);
                object item = GetPolicyItemByPolicyId(policy.Id, commodtyType);
                #region "switch"

                Guid commodityCategoryId = Guid.Empty, makeId = Guid.Empty, modelId = Guid.Empty;
                switch (commodtyType.ToLower())
                {
                    case "a":
                        {
                            VehicleDetails vehicleDetails = item as VehicleDetails;
                            if (vehicleDetails != null)
                            {
                                commodityCategoryId = vehicleDetails.CategoryId;
                                makeId = vehicleDetails.MakeId;
                                modelId = vehicleDetails.ModelId;
                            }
                            break;
                        }
                    case "e":
                        {
                            BrownAndWhiteDetails brownAndWhiteDetails = item as BrownAndWhiteDetails;
                            if (brownAndWhiteDetails != null)
                            {
                                commodityCategoryId = brownAndWhiteDetails.CategoryId;
                                makeId = brownAndWhiteDetails.MakeId;
                                modelId = brownAndWhiteDetails.ModelId;

                            }
                            break;
                        }
                    case "o":
                        {
                            OtherItemDetails otherItemDetails = item as OtherItemDetails;
                            if (otherItemDetails != null)
                            {
                                commodityCategoryId = otherItemDetails.CategoryId;
                                makeId = otherItemDetails.MakeId;
                                modelId = otherItemDetails.ModelId;

                            }
                            break;
                        }
                    case "y":
                        {
                            YellowGoodDetails yellowGoodDetails = item as YellowGoodDetails;
                            if (yellowGoodDetails != null)
                            {
                                commodityCategoryId = yellowGoodDetails.CategoryId;
                                makeId = yellowGoodDetails.MakeId;
                                modelId = yellowGoodDetails.ModelId;
                            }
                            break;
                        }
                }

                #endregion
                response = new ClaimEndorsement()
                {
                    StatusId = cem.GetClaimStatusIdByCode("REV"),
                    Id = Guid.NewGuid(),
                    PolicyId = claimEndorsementInfo.policyId,
                    ExamineBy = claimEndorsementInfo.entryBy,
                    IsApproved = false,
                    ClaimCountryId = policyDealer.CountryId,
                    ClaimCurrencyId = policyDealer.CurrencyId,
                    ClaimSubmittedBy = claimEndorsementInfo.entryBy,
                    ClaimSubmittedDealerId = policyDealer.Id,
                    CommodityCategoryId = commodityCategoryId,
                    CommodityTypeId = policy.CommodityTypeId,
                    CustomerId = policy.CustomerId,
                    EntryDate = DateTime.UtcNow,
                    IsBatching = false,
                    IsInvoiced = false,
                    MakeId = makeId,
                    ModelId = modelId,
                    PolicyCountryId = policyDealer.CountryId,
                    PolicyDealerId = claimEndorsementInfo.dealerId,
                    PolicyNumber = policy.PolicyNo,
                    LastUpdatedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    ApprovedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    IsRejected = false,
                    EndorsementIsApproved = false,
                    EndorsementApprovedOrRejectedBy = Guid.Empty,
                    EndorsementApprovedOrRejectedDate = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                    ClaimId = claimEndorsementInfo.claimId,


                };
            }
            catch (Exception)
            {
                throw;
            }
            return response;
        }

        internal ClaimEndorsementItem ClaimEndorsementItemsToTpaClaimItems(ClaimEndorsementItemRequestDto claimEndorsementInfo, ClaimEndorsement newClaim)
        {
            ClaimEndorsementItem claimItems = new ClaimEndorsementItem();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();

                if (!IsGuid(claimEndorsementInfo.faultId.ToString()))
                {
                    claimEndorsementInfo.faultId = null;
                }

                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currencyPeriod = currencyEm.GetCurrentCurrencyPeriodId();
                Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == claimEndorsementInfo.dealerId);
                if (dealer == null)
                {
                    throw new Exception("Selected dealer is invalid.");
                }
                else if (dealer.CurrencyId == Guid.Empty)
                {
                    throw new Exception("Selected dealer's currency not set.");
                }

                //new item process
                if (!IsGuid(claimEndorsementInfo.serverId.ToString()))
                {

                    claimItems = new ClaimEndorsementItem()
                    {
                        ParentId = Guid.Empty,
                        Id = Guid.NewGuid(),
                        IsDiscountPercentage = claimEndorsementInfo.isDiscountPercentage,
                        DiscountAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.discountAmount, dealer.CurrencyId, currencyPeriod),
                        DiscountRate = claimEndorsementInfo.isDiscountPercentage ? claimEndorsementInfo.discountRate : currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.discountRate, dealer.CurrencyId, currencyPeriod),
                        ClaimItemTypeId = cem.GetClaimTypeIdByClaimTypeCode(claimEndorsementInfo.itemType),
                        IsApproved = claimEndorsementInfo.status.ToLower() == "a" ? true : false,
                        GoodWillRate = claimEndorsementInfo.isGoodWillPercentage ? claimEndorsementInfo.goodWillRate : currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.goodWillRate, dealer.CurrencyId, currencyPeriod),
                        TotalGrossPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.totalGrossPrice, dealer.CurrencyId, currencyPeriod),
                        IsGoodWillPercentage = claimEndorsementInfo.isGoodWillPercentage,
                        GoodWillAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.goodWillAmount, dealer.CurrencyId, currencyPeriod),
                        PartId = claimEndorsementInfo.partId == Guid.Empty ? null : claimEndorsementInfo.partId,
                        AuthorizedAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.authorizedAmt, dealer.CurrencyId, currencyPeriod),
                        ClaimId = claimEndorsementInfo.claimId,
                        FaultId = claimEndorsementInfo.faultId == Guid.Empty ? null : claimEndorsementInfo.faultId,
                        ItemCode = claimEndorsementInfo.itemNumber,
                        ItemName = claimEndorsementInfo.itemName,
                        Quantity = claimEndorsementInfo.qty,
                        Remark = claimEndorsementInfo.remarks,
                        TotalPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.totalPrice, dealer.CurrencyId, currencyPeriod),
                        TpaComment = claimEndorsementInfo.comment,
                        UnitPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.unitPrice, dealer.CurrencyId, currencyPeriod),
                        ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                        CurrencyPeriodId = currencyPeriod
                    };


                }
                else
                {
                    claimItems = new ClaimEndorsementItem()
                    {
                        ParentId = Guid.Empty,
                        Id = Guid.NewGuid(),
                        IsDiscountPercentage = claimEndorsementInfo.isDiscountPercentage,
                        DiscountAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.discountAmount, dealer.CurrencyId, currencyPeriod),
                        DiscountRate = claimEndorsementInfo.isDiscountPercentage ? claimEndorsementInfo.discountRate : currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.discountRate, dealer.CurrencyId, currencyPeriod),
                        ClaimItemTypeId = cem.GetClaimTypeIdByClaimTypeCode(claimEndorsementInfo.itemType),
                        IsApproved = claimEndorsementInfo.status.ToLower() == "a" ? true : false,
                        GoodWillRate = claimEndorsementInfo.isGoodWillPercentage ? claimEndorsementInfo.goodWillRate : currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.goodWillRate, dealer.CurrencyId, currencyPeriod),
                        TotalGrossPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.totalGrossPrice, dealer.CurrencyId, currencyPeriod),
                        IsGoodWillPercentage = claimEndorsementInfo.isGoodWillPercentage,
                        GoodWillAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.goodWillAmount, dealer.CurrencyId, currencyPeriod),
                        PartId = claimEndorsementInfo.partId == Guid.Empty ? null : claimEndorsementInfo.partId,
                        AuthorizedAmount = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.authorizedAmt, dealer.CurrencyId, currencyPeriod),
                        ClaimId = claimEndorsementInfo.claimId,
                        FaultId = claimEndorsementInfo.faultId == Guid.Empty ? null : claimEndorsementInfo.faultId,
                        ItemCode = claimEndorsementInfo.itemNumber,
                        ItemName = claimEndorsementInfo.itemName,
                        Quantity = claimEndorsementInfo.qty,
                        Remark = claimEndorsementInfo.remarks,
                        TotalPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.totalPrice, dealer.CurrencyId, currencyPeriod),
                        TpaComment = claimEndorsementInfo.comment,
                        UnitPrice = currencyEm.ConvertToBaseCurrency(claimEndorsementInfo.unitPrice, dealer.CurrencyId, currencyPeriod),
                        ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currencyPeriod),
                        CurrencyPeriodId = currencyPeriod
                    };

                }

            }
            catch (Exception)
            {
                throw;
            }
            return claimItems;
        }

        internal OldCliamEndrosment OldClaimInquiryEnrolmentDetails(Claim claim, Guid cliamEndrosmentId)
        {
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            OldCliamEndrosment Response = new OldCliamEndrosment();

            Policy Policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claim.PolicyId);

            List<Entities.ClaimItem> claimItems = session.Query<Entities.ClaimItem>()
                       .Where(a => a.ClaimId == claim.Id).ToList();

            var ListMM = new List<ClaimItemList>();

            Response.ClaimItemList = new List<ClaimItemList>();
            int claimItemseq = 0;
            foreach (Entities.ClaimItem claimItem in claimItems)
            {
                claimItemseq++;
                ClaimItemList claimItemObj = new ClaimItemList()
                {
                    partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                    serverId = claimItem.Id,
                    partId = claimItem.PartId,
                    id = claimItemseq,
                    itemName = claimItem.ItemName,
                    itemNumber = claimItem.ItemCode,
                    itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                    qty = claimItem.Quantity,
                    remarks = claimItem.Remark,
                    totalPrice =
                        currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    unitPrice =
                        currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    authorizedAmount = currencyEm.ConvertFromBaseCurrency(claimItem.AuthorizedAmount, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    faultId = claimItem.FaultId,
                    faultName = cem.GetFaultNameById(claimItem.FaultId),
                    status = claimItem.IsApproved == true ? "Approved" : (claimItem.IsApproved == false ? "Rejected" : ""),
                    statusCode = claimItem.IsApproved == true ? "A" : (claimItem.IsApproved == false ? "R" : ""),
                    discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claim.ClaimCurrencyId,
                claim.CurrencyPeriodId),
                    comment = claimItem.TpaComment,
                    isDiscountPercentage = claimItem.IsDiscountPercentage,
                    isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                    totalGrossPrice = currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate : currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                    goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate : currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, claim.ClaimCurrencyId,
                    claim.CurrencyPeriodId),
                    parentId = claimItem.ParentId,
                    discountSchemeCode = claimItem.DiscountSchemeCode,
                    discountSchemeId = claimItem.DiscountSchemeId,
                    rejectionTypeId = claimItem.RejectionTypeId

                };
                ListMM.Add(claimItemObj);


            }

            Response = new OldCliamEndrosment()
            {

                PolicyNumber = claim.PolicyNumber,
                ClaimNumber = claim.ClaimNumber,
                customer = cem.GetCustomerNameById(claim.CustomerId),
                Serial = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(Policy.Id, Policy.CommodityTypeId, "serial"),
                MakeId = cem.GetMakeNameById(claim.MakeId),
                ModelId = cem.GetModelNameById(claim.ModelId),
                PolicyDealerId = cem.GetDealerNameById(claim.PolicyDealerId),
                ClaimDealer = cem.GetDealerNameById(claim.ClaimSubmittedDealerId),
                TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                TotalGrossAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalGrossClaimAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                PaidAmount = currencyEm.ConvertFromBaseCurrency(claim.PaidAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                ClaimItemList = ListMM,
                Complaint = new Complaint
                {
                    //colmplaint
                    customer = claim.CustomerComplaint,
                    dealer = claim.DealerComment,
                    conclution = claim.Conclution,
                    engineer = claim.EngineerComment

                }
            };


            return Response;



        }


        internal NewCliamEndrosment NewClaimInquiryEnrolmentDetails(Claim newClaim, Guid cliamEndrosmentId)
        {
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            NewCliamEndrosment Response = new NewCliamEndrosment();

            Policy Policy = session.Query<Policy>().FirstOrDefault(a => a.Id == newClaim.PolicyId);

            List<Entities.ClaimItem> claimItems = session.Query<Entities.ClaimItem>()
                       .Where(a => a.ClaimId == newClaim.Id).ToList();

            var ListMM = new List<ClaimItemList>();

            Response.ClaimItemList = new List<ClaimItemList>();
            int claimItemseq = 0;
            foreach (Entities.ClaimItem claimItem in claimItems)
            {
                claimItemseq++;
                ClaimItemList claimItemObj = new ClaimItemList()
                {
                    partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                    serverId = claimItem.Id,
                    partId = claimItem.PartId,
                    id = claimItemseq,
                    itemName = claimItem.ItemName,
                    itemNumber = claimItem.ItemCode,
                    itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                    qty = claimItem.Quantity,
                    remarks = claimItem.Remark,
                    totalPrice =
                        currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    unitPrice =
                        currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    authorizedAmount = currencyEm.ConvertFromBaseCurrency(claimItem.AuthorizedAmount, newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    faultId = claimItem.FaultId,
                    faultName = cem.GetFaultNameById(claimItem.FaultId),
                    status = claimItem.IsApproved == true ? "Approved" : (claimItem.IsApproved == false ? "Rejected" : ""),
                    statusCode = claimItem.IsApproved == true ? "A" : (claimItem.IsApproved == false ? "R" : ""),
                    discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, newClaim.ClaimCurrencyId,
                newClaim.CurrencyPeriodId),
                    comment = claimItem.TpaComment,
                    isDiscountPercentage = claimItem.IsDiscountPercentage,
                    isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                    totalGrossPrice = currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate : currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, newClaim.ClaimCurrencyId,
                            newClaim.CurrencyPeriodId),
                    goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate : currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, newClaim.ClaimCurrencyId,
                    newClaim.CurrencyPeriodId),
                    //parentId = claimItem.ParentId,
                    //discountSchemeCode = claimItem.DiscountSchemeCode,
                    //discountSchemeId = claimItem.DiscountSchemeId,
                    //rejectionTypeId = claimItem.RejectionTypeId

                };
                ListMM.Add(claimItemObj);


            }

            Response = new NewCliamEndrosment()
            {

                PolicyNumber = newClaim.PolicyNumber,
                ClaimNumber = newClaim.ClaimNumber,
                customer = cem.GetCustomerNameById(newClaim.CustomerId),
                Serial = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(Policy.Id, Policy.CommodityTypeId, "serial"),
                MakeId = cem.GetMakeNameById(newClaim.MakeId),
                ModelId = cem.GetModelNameById(newClaim.ModelId),
                PolicyDealerId = cem.GetDealerNameById(newClaim.PolicyDealerId),
                ClaimDealer = cem.GetDealerNameById(newClaim.ClaimSubmittedDealerId),
                TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(newClaim.TotalClaimAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                TotalGrossAmount = currencyEm.ConvertFromBaseCurrency(newClaim.TotalGrossClaimAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                PaidAmount = currencyEm.ConvertFromBaseCurrency(newClaim.PaidAmount, newClaim.ClaimCurrencyId, newClaim.CurrencyPeriodId),
                ClaimItemList = ListMM,
                Complaint = new Complaint
                {
                    //colmplaint
                    customer = newClaim.CustomerComplaint,
                    dealer = newClaim.DealerComment,
                    conclution = newClaim.Conclution,
                    engineer = newClaim.EngineerComment

                }

            };


            return Response;
        }

        public Customer ConvertPolicyCustomerToCustomerEntity(Customer_ policyCustomer, Guid loggedInUserId)
        {
            var customer = new Customer()
            {
                Id = policyCustomer.customerId == Guid.Empty ? Guid.Empty : policyCustomer.customerId,
                EntryDateTime = DateTime.UtcNow,
                IsActive = true,
                CountryId = policyCustomer.countryId,
                PostalCode= policyCustomer.PostalCode,
                Address1 = policyCustomer.address1,
                Address2 = policyCustomer.address2,
                Address3 = policyCustomer.address3,
                Address4 = policyCustomer.address4,
                BusinessAddress1 = policyCustomer.businessAddress1,
                BusinessAddress2 = policyCustomer.businessAddress2,
                BusinessAddress3 = policyCustomer.businessAddress3,
                BusinessAddress4 = policyCustomer.businessAddress4,
                BusinessName = policyCustomer.businessName,
                BusinessTelNo = policyCustomer.businessTelNo,
                CityId = policyCustomer.cityId,
                CustomerTypeId = policyCustomer.customerTypeId,
                DLIssueDate = policyCustomer.idIssueDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : policyCustomer.idIssueDate,
                DateOfBirth = policyCustomer.dateOfBirth == DateTime.MinValue ? SqlDateTime.MinValue.Value : policyCustomer.dateOfBirth,
                Email = policyCustomer.email,
                UserName = policyCustomer.email,
                EntryUserId = loggedInUserId.ToString(),
                FirstName = policyCustomer.firstName,
                Gender = policyCustomer.gender,
                IDNo = policyCustomer.idNo,
                IDTypeId = policyCustomer.idTypeId,
                LastModifiedDateTime = DateTime.UtcNow,
                LastName = policyCustomer.lastName,
                MaritalStatusId = Guid.Empty,
                MobileNo = policyCustomer.mobileNo,
                NationalityId = policyCustomer.nationalityId,
                OccupationId = Guid.Empty,
                OtherTelNo = policyCustomer.otherTelNo,
                Password = string.Empty,
                TitleId = Guid.Empty,
                UsageTypeId = policyCustomer.usageTypeId
            };
            return customer;
        }

        public GenericCodeMsgObjResponse ConvertPolicyProductToProductEntity(Product_ product, Guid loggedInUserId)
        {
            string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.commodityTypeId);
            CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
            Guid getCurrencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
            decimal rate = currencyEntityManager.GetConversionRate(product.dealerPaymentCurrencyTypeId,
                getCurrencyPeriodId, true);
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = commodityType.ToUpper();
            if (commodityType.ToLower() == "a")
            {
                VehicleDetails vehicleDetails = new VehicleDetails()
                {
                    Id = product.id == Guid.Empty ? Guid.Empty : product.id,
                    EntryDateTime = DateTime.UtcNow,
                    AspirationId = product.aspirationTypeId,
                    BodyTypeId = product.bodyTypeId,
                    CategoryId = product.categoryId,
                    CommodityUsageTypeId = product.commodityUsageTypeId,
                    CountryId = Guid.Empty,
                    CylinderCountId = product.cylinderCountId,
                    DealerCurrencyId = product.dealerPaymentCurrencyTypeId,
                    DealerId = product.dealerId,
                    DriveTypeId = product.driveTypeId,
                    EngineCapacityId = product.engineCapacityId,
                    EntryUser = loggedInUserId,
                    FuelTypeId = product.fuelTypeId,
                    GrossWeight = product.grossWeight,
                    ItemPurchasedDate = product.itemPurchasedDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.itemPurchasedDate,
                    ItemStatusId = product.itemStatusId,
                    MakeId = product.makeId,
                    ModelId = product.modelId,
                    ModelYear = product.modelYear,
                    PlateNo = product.additionalSerial,
                    RegistrationDate = product.registrationDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.registrationDate,
                    TransmissionId = product.transmissionTypeId,
                    VINNo = product.serialNumber,
                    Variant = product.variantId,
                    ConversionRate = rate,
                    currencyPeriodId = getCurrencyPeriodId,
                    DealerPrice = product.dealerPrice / rate,
                    VehiclePrice = product.itemPrice / rate,
                    EngineNumber = product.engineNumber

                };
                response.obj = vehicleDetails;

            }
            else if (commodityType.ToLower() == "b")
            {
                VehicleDetails vehicleDetails = new VehicleDetails()
                {
                    Id = product.id == Guid.Empty ? Guid.Empty : product.id,
                    EntryDateTime = DateTime.UtcNow,
                    AspirationId = product.aspirationTypeId,
                    BodyTypeId = product.bodyTypeId,
                    CategoryId = product.categoryId,
                    CommodityUsageTypeId = product.commodityUsageTypeId,
                    CountryId = Guid.Empty,
                    CylinderCountId = product.cylinderCountId,
                    DealerCurrencyId = product.dealerPaymentCurrencyTypeId,
                    DealerId = product.dealerId,
                    DriveTypeId = product.driveTypeId,
                    EngineCapacityId = product.engineCapacityId,
                    EntryUser = loggedInUserId,
                    FuelTypeId = product.fuelTypeId,
                    GrossWeight = product.grossWeight,
                    ItemPurchasedDate = product.itemPurchasedDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.itemPurchasedDate,
                    ItemStatusId = product.itemStatusId,
                    MakeId = product.makeId,
                    ModelId = product.modelId,
                    ModelYear = product.modelYear,
                    PlateNo = product.additionalSerial,
                    RegistrationDate = product.registrationDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.registrationDate,
                    TransmissionId = product.transmissionTypeId,
                    VINNo = product.serialNumber,
                    Variant = product.variantId,
                    ConversionRate = rate,
                    currencyPeriodId = getCurrencyPeriodId,
                    DealerPrice = product.dealerPrice / rate,
                    VehiclePrice = product.itemPrice / rate,
                    EngineNumber = product.engineNumber
                };
                response.obj = vehicleDetails;

            }
            else if (commodityType.ToLower() == "e")
            {
                BrownAndWhiteDetails brownAndWhiteDetails = new BrownAndWhiteDetails()
                {
                    Id = product.id == Guid.Empty ? Guid.Empty : product.id,
                    EntryDateTime = DateTime.UtcNow,
                    CategoryId = product.categoryId,
                    CommodityUsageTypeId = product.commodityUsageTypeId,
                    CountryId = Guid.Empty,
                    DealerCurrencyId = product.dealerPaymentCurrencyTypeId,
                    DealerId = product.dealerId,
                    EntryUser = loggedInUserId,
                    ItemPurchasedDate = product.itemPurchasedDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.itemPurchasedDate,
                    ItemStatusId = product.itemStatusId,
                    MakeId = product.makeId,
                    ModelId = product.modelId,
                    ModelYear = product.modelYear,
                    Variant = product.variantId,
                    ConversionRate = rate,
                    currencyPeriodId = getCurrencyPeriodId,
                    DealerPrice = product.dealerPrice / rate,
                    AddnSerialNo = product.additionalSerial,
                    InvoiceNo = product.invoiceNo,
                    ModelCode = string.Empty,
                    SerialNo = product.serialNumber,
                    ItemPrice = product.itemPrice / rate,
                };
                response.obj = brownAndWhiteDetails;
            }
            else if (commodityType.ToLower() == "y")
            {
                YellowGoodDetails yellowGoodDetails = new YellowGoodDetails()
                {
                    Id = product.id == Guid.Empty ? Guid.Empty : product.id,
                    EntryDateTime = DateTime.UtcNow,
                    CategoryId = product.categoryId,
                    CommodityUsageTypeId = product.commodityUsageTypeId,
                    EntryUser = loggedInUserId,
                    ItemPurchasedDate = product.itemPurchasedDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.itemPurchasedDate,
                    ItemStatusId = product.itemStatusId,
                    MakeId = product.makeId,
                    ModelId = product.modelId,
                    ModelYear = product.modelYear,
                    DealerPrice = product.dealerPrice / rate,
                    AddnSerialNo = product.additionalSerial,
                    InvoiceNo = product.invoiceNo,
                    ModelCode = string.Empty,
                    SerialNo = product.serialNumber,
                    ItemPrice = product.itemPrice / rate,

                };
                response.obj = yellowGoodDetails;
            }
            else if (commodityType.ToLower() == "o")
            {
                OtherItemDetails otherItemDetails = new OtherItemDetails()
                {
                    Id = product.id == Guid.Empty ? Guid.Empty : product.id,
                    EntryDateTime = DateTime.UtcNow,
                    CategoryId = product.categoryId,
                    CommodityUsageTypeId = product.commodityUsageTypeId,
                    EntryUser = loggedInUserId,
                    ItemPurchasedDate = product.itemPurchasedDate == DateTime.MinValue ? SqlDateTime.MinValue.Value : product.itemPurchasedDate,
                    ItemStatusId = product.itemStatusId,
                    MakeId = product.makeId,
                    ModelId = product.modelId,
                    ModelYear = product.modelYear,
                    DealerPrice = product.dealerPrice / rate,
                    AddnSerialNo = product.additionalSerial,
                    InvoiceNo = product.invoiceNo,
                    ModelCode = string.Empty,
                    SerialNo = product.serialNumber,
                    ItemPrice = product.itemPrice / rate,
                };
                response.obj = otherItemDetails;
            }
            return response;
        }

        public PolicyBundle ConvertPolicyBundleToPolicyBundleEntityRenewal(PolicyDetails_ policyDetails)
        {
            CommonEntityManager commonEntityManager = new CommonEntityManager();
            var policyBundle = new PolicyBundle()
            {
                Id = policyDetails.policy.policyBundleId,
                ContractId = Guid.Empty,
                EntryDateTime = DateTime.UtcNow,
                Comment = string.Empty,
                EntryUser = policyDetails.loggedInUserId,
                ProductId = policyDetails.product.productId,
                DealerId = policyDetails.product.dealerId,
                Premium = decimal.Zero,
                CommodityTypeId = policyDetails.product.commodityTypeId,
                PolicyNo = string.Empty,
                CoverTypeId = Guid.Empty,
                CustomerId = policyDetails.customer.customerId,
                CustomerPayment = decimal.Zero,
                CustomerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                DealerLocationId = policyDetails.product.dealerLocationId,
                DealerPayment = decimal.Zero,
                DealerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                DealerPolicy = false,
                Discount = decimal.Zero,
                ExtensionTypeId = Guid.Empty,
                HrsUsedAtPolicySale = policyDetails.policy.hrsUsedAtPolicySale,
                IsApproved = commonEntityManager.GetDealerById(policyDetails.product.dealerId).IsAutoApproval,
                IsPartialPayment = policyDetails.payment.isPartialPayment,
                IsPolicyCanceled = false,
                IsPreWarrantyCheck = false,
                IsSpecialDeal = policyDetails.payment.isSpecialDeal,
                PaymentModeId = policyDetails.payment.paymentModeId,
                PaymentTypeId = policyDetails.payment.paymentTypeId,
                PolicySoldDate = policyDetails.policy.policySoldDate,
                PremiumCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                RefNo = policyDetails.payment.refNo,
                SalesPersonId = policyDetails.policy.salesPersonId,
                BookletNumber = policyDetails.policy.productContracts.FirstOrDefault().BookletNumber,
                ContractExtensionPremiumId = Guid.Empty,
                ContractExtensionsId = Guid.Empty,
                ContractInsuaranceLimitationId = Guid.Empty,
                MWStartDate = policyDetails.product.MWStartDate,
                MWIsAvailable = policyDetails.product.MWIsAvailable

            };
            return policyBundle;
        }

        public PolicyBundle ConvertPolicyBundleToPolicyBundleEntity(PolicyDetails_ policyDetails)
        {
            CommonEntityManager commonEntityManager = new CommonEntityManager();
            var policyBundle = new PolicyBundle()
            {
                Id = Guid.NewGuid(),
                ContractId = Guid.Empty,
                EntryDateTime = DateTime.UtcNow,
                Comment = string.Empty,
                EntryUser = policyDetails.loggedInUserId,
                ProductId = policyDetails.product.productId,
                DealerId = policyDetails.product.dealerId,
                Premium = decimal.Zero,
                CommodityTypeId = policyDetails.product.commodityTypeId,
                PolicyNo = string.Empty,
                CoverTypeId = Guid.Empty,
                CustomerId = policyDetails.customer.customerId,
                CustomerPayment = decimal.Zero,
                CustomerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                DealerLocationId = policyDetails.product.dealerLocationId,
                DealerPayment = decimal.Zero,
                DealerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                DealerPolicy = false,
                Discount = decimal.Zero,
                ExtensionTypeId = Guid.Empty,
                HrsUsedAtPolicySale = policyDetails.policy.hrsUsedAtPolicySale,
                IsApproved = commonEntityManager.GetDealerById(policyDetails.product.dealerId).IsAutoApproval,
                IsPartialPayment = policyDetails.payment.isPartialPayment,
                IsPolicyCanceled = false,
                IsPreWarrantyCheck = false,
                IsSpecialDeal = policyDetails.payment.isSpecialDeal,
                PaymentModeId = policyDetails.payment.paymentModeId,
                PaymentTypeId = policyDetails.payment.paymentTypeId,
                PolicySoldDate = policyDetails.policy.policySoldDate,
                PremiumCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                RefNo = policyDetails.payment.refNo,
                SalesPersonId = policyDetails.policy.salesPersonId,
                BookletNumber = policyDetails.policy.productContracts.FirstOrDefault().BookletNumber,
                ContractExtensionPremiumId = Guid.Empty,
                ContractExtensionsId = Guid.Empty,
                ContractInsuaranceLimitationId = Guid.Empty,
                MWStartDate = policyDetails.product.MWStartDate

            };
            return policyBundle;
        }

        public List<Policy> ConvertPolicyListToPolicyListEntity(PolicyDetails_ policyDetails)
        {
            CommonEntityManager commonEntityManager = new CommonEntityManager();
            CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
            Guid getCurrencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
            decimal rate = currencyEntityManager.GetConversionRate(policyDetails.product.dealerPaymentCurrencyTypeId,
                getCurrencyPeriodId, true);
            List<Policy> policies = new List<Policy>();
            foreach (var contractData in policyDetails.policy.productContracts)
            {
                var premiumDetails = new ContractEntityManager().GetPremium(contractData.CoverTypeId,
                    decimal.Parse(policyDetails.policy.hrsUsedAtPolicySale), contractData.AttributeSpecificationId,
                    contractData.ExtensionTypeId,
                    contractData.ContractId, policyDetails.product.productId, policyDetails.product.dealerId,
                    policyDetails.policy.policySoldDate,
                    policyDetails.product.cylinderCountId, policyDetails.product.engineCapacityId,
                    policyDetails.product.makeId, policyDetails.product.modelId,
                    policyDetails.product.variantId, policyDetails.product.grossWeight,
                    policyDetails.product.itemStatusId, policyDetails.product.dealerPrice,
                    policyDetails.product.itemPurchasedDate) as GetPremiumResponseDto;


                //mw should not be applicable to tyre product
                //so hard coding
                if (commonEntityManager.GetProductCodeById(policyDetails.product.productId).ToUpper() == "TYRE")
                {
                    policyDetails.product.MWIsAvailable = false;
                }

                Policy policy = new Policy()
                {
                    Id = Guid.NewGuid(),
                    ContractId = contractData.ContractId,
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = policyDetails.loggedInUserId,
                    BordxCountryId = Guid.Empty,
                    BordxId = Guid.Empty,
                    BordxNumber = 0,
                    Comment = string.Empty,
                    CommodityTypeId = policyDetails.product.commodityTypeId,
                    ContractExtensionPremiumId = contractData.CoverTypeId,
                    ContractExtensionsId = contractData.ExtensionTypeId,
                    ContractInsuaranceLimitationId = contractData.AttributeSpecificationId,
                    CustomerId = policyDetails.customer.customerId,
                    CurrencyPeriodId = getCurrencyPeriodId,
                    CustomerPayment = policyDetails.payment.customerPayment / rate,
                    CustomerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                    DealerId = policyDetails.product.dealerId,
                    DealerLocationId = policyDetails.product.dealerLocationId,
                    DealerPayment = policyDetails.payment.dealerPayment / rate,
                    DealerPolicy = policyDetails.policy.dealerPolicy,
                    DealerPaymentCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                    DiscountPercentage = Convert.ToInt16(policyDetails.payment.discount),
                    CoverTypeId = contractData.CoverTypeId,
                    ExtensionTypeId = contractData.ExtensionTypeId,
                    ForwardComment = string.Empty,

                    ProductId = contractData.ProductId,
                    Premium = premiumDetails.TotalPremium / rate,
                    GrossPremiumBeforeTax = (premiumDetails.BasicPremium + premiumDetails.EligibilityPremium),
                    PaymentMethodFee = premiumDetails.TotalPremium / rate * (commonEntityManager.GetPaymentMethodPercentageByPaymentMethodId(policyDetails.payment.paymentModeId) / 100),
                    TotalTax = premiumDetails.Tax,
                    NRP = premiumDetails.TotalPremiumNRP / rate,
                    EligibilityFee = premiumDetails.EligibilityPremium,
                    Discount = premiumDetails.TotalPremium / rate * policyDetails.payment.discount / 100,

                    Year = 0,
                    SalesPersonId = policyDetails.policy.salesPersonId,
                    IsSpecialDeal = policyDetails.payment.isSpecialDeal,
                    IsApproved = commonEntityManager.GetDealerById(policyDetails.product.dealerId).IsAutoApproval,
                    HrsUsedAtPolicySale = policyDetails.policy.hrsUsedAtPolicySale,
                    PolicyNo = contractData.PolicyNo,
                    PaymentModeId = policyDetails.payment.paymentModeId,
                    IsPreWarrantyCheck = false,
                    RefNo = policyDetails.payment.refNo,
                    IsPartialPayment = policyDetails.payment.isPartialPayment,
                    PaymentTypeId = policyDetails.payment.paymentTypeId,
                    IsPolicyCanceled = false,
                    PolicySoldDate = policyDetails.policy.policySoldDate,
                    PremiumCurrencyTypeId = policyDetails.product.dealerPaymentCurrencyTypeId,
                    Co_Customer = Guid.Empty,
                    IsPolicyRenewed = false,
                    LocalCurrencyConversionRate = rate,
                    Month = 0,
                    PaymentMethodFeePercentage = commonEntityManager.GetPaymentMethodPercentageByPaymentMethodId(policyDetails.payment.paymentModeId),
                    PolicyApprovedBy = Guid.Empty,
                    PolicyBundleId = Guid.Empty,
                    TPABranchId = policyDetails.policy.tpaBranchId,
                    TransferFee = decimal.Zero,
                    PolicyEndDate = GetPolicyEndDate(policyDetails.product.MWStartDate, policyDetails.policy.policySoldDate, policyDetails.product.makeId, policyDetails.product.modelId,
                    policyDetails.product.dealerId, contractData.ExtensionTypeId, policyDetails.product.MWIsAvailable),
                    PolicyStartDate = GetPolicyStartDate(policyDetails.product.MWStartDate, policyDetails.policy.policySoldDate, policyDetails.product.makeId, policyDetails.product.modelId,
                    policyDetails.product.dealerId, contractData.ExtensionTypeId, policyDetails.product.MWIsAvailable),
                    BookletNumber = contractData.BookletNumber,
                    MWStartDate = policyDetails.product.MWStartDate,
                    MWIsAvailable = policyDetails.product.MWIsAvailable,
                    MonthlyEMI = policyDetails.policy.Emi,
                    //SequenceNo = policyDetails.policy.,
                    FinanceAmount = contractData.FinanceAmount / rate,
                    ApprovedDate = DateTime.UtcNow
                };
                policies.Add(policy);
            }
            return policies;
        }

        public DateTime GetPolicyStartDate(DateTime mwStartDate, DateTime policySoldDate, Guid makeId, Guid modelId, Guid dealerId,
            Guid extensionTypeId, bool isMwAvailable)
        {
            ISession session = EntitySessionManager.GetSession();
            if (!isMwAvailable)
            {
                return policySoldDate;
            }

            Guid countryId = session.Query<Dealer>().FirstOrDefault(a => a.Id == dealerId).CountryId;
            IList<ManufacturerWarrantyDetails> manufacturerWarrantyDetails =
                session.QueryOver<ManufacturerWarrantyDetails>()
                    .WhereRestrictionOn(a => a.CountryId).IsIn(new Guid[] { countryId })
                       .WhereRestrictionOn(c => c.ModelId).IsIn(new Guid[] { modelId })
                    .List<ManufacturerWarrantyDetails>();
            int mwMonths = 0;
            ManufacturerWarranty mw = session.QueryOver<ManufacturerWarranty>()
                 .WhereRestrictionOn(a => a.MakeId).IsIn(new Guid[] { makeId })
                       .WhereRestrictionOn(c => c.Id).IsIn(manufacturerWarrantyDetails.Select(a => a.ManufacturerWarrantyId).ToArray())
                    .List<ManufacturerWarranty>().FirstOrDefault();

            if (mw != null)
            {
                mwMonths = mw.WarrantyMonths;
                return mwStartDate.AddMonths(mwMonths).AddDays(-1);
            }
            else
            {
                return policySoldDate;
            }

        }

        public DateTime GetPolicyEndDate(DateTime mwStartDate, DateTime policySoldDate, Guid makeId, Guid modelId, Guid dealerId,
            Guid extensionTypeId, bool isMwAvailable)
        {
            ISession session = EntitySessionManager.GetSession();


            Guid countryId = session.Query<Dealer>().FirstOrDefault(a => a.Id == dealerId).CountryId;

            IList<ManufacturerWarrantyDetails> manufacturerWarrantyDetails =
                session.QueryOver<ManufacturerWarrantyDetails>()
                    .WhereRestrictionOn(a => a.CountryId).IsIn(new Guid[] { countryId })
                       .WhereRestrictionOn(c => c.ModelId).IsIn(new Guid[] { modelId })
                    .List<ManufacturerWarrantyDetails>();
            int mwMonths = 0, extMonths = 0;
            ManufacturerWarranty mw = session.QueryOver<ManufacturerWarranty>()
                 .WhereRestrictionOn(a => a.MakeId).IsIn(new Guid[] { makeId })
                       .WhereRestrictionOn(c => c.Id)
                       .IsIn(manufacturerWarrantyDetails.Select(a => a.ManufacturerWarrantyId).ToArray())
                    .List<ManufacturerWarranty>().FirstOrDefault();

            if (mw != null)
            {
                mwMonths = mw.WarrantyMonths;
            }

            ContractInsuaranceLimitation contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                .FirstOrDefault(a => a.Id == extensionTypeId);
            if (contractInsuaranceLimitation != null)
            {
                InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                    .FirstOrDefault(a => a.Id == contractInsuaranceLimitation.InsuaranceLimitationId);
                if (insuaranceLimitation != null)
                {
                    extMonths = insuaranceLimitation.Months;
                }
            }


            if (mw != null && isMwAvailable)
            {
                return mwStartDate.AddMonths(mwMonths + extMonths).AddDays(-2);
            }
            else
            {
                return policySoldDate.AddMonths(extMonths).AddDays(-1);
            }
        }

        internal ClaimSubmission ConvertClaimDataToClaimSubmissionTireRequest(Dealer claimDealer, ClaimSubmissionOtherTireRequestDto claimSubmissionOtherTireData, Policy policy, Guid currentConversionPeriodId)
        {
            ClaimSubmission Response;
            try
            {
                CommonEntityManager commonEm = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                //creating claim request
                ClaimSubmission claimRequest = new ClaimSubmission()
                {
                    ClaimCountryId = claimDealer.CountryId,
                    ClaimCurrencyId = claimDealer.CurrencyId,
                    ClaimNumber = "",
                    ClaimSubmittedBy = claimSubmissionOtherTireData.requestedUserId,
                    ClaimSubmittedDealerId = claimDealer.Id,
                    CommodityCategoryId = commonEm.GetCommodityCategoryIdByContractId(policy.ContractId),
                    CommodityTypeId = policy.CommodityTypeId,
                    ConversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId),
                    CurrencyPeriodId = currentConversionPeriodId,
                    CustomerComplaint = claimSubmissionOtherTireData.OtherTireDetails.customerComplaint,
                    CustomerId = policy.CustomerId,
                    DealerComment = claimSubmissionOtherTireData.OtherTireDetails.dealerComment,
                    EntryDate = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    LastUpdatedBy = null,
                    LastUpdatedDate = DateTime.UtcNow,
                    MakeId = Guid.Parse(commonEm.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "make")),
                    ModelId = Guid.Parse(commonEm.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "model")),
                    PolicyCountryId = commonEm.GetDealerCuntryByDealerId(policy.DealerId),
                    PolicyDealerId = policy.DealerId,
                    PolicyId = policy.Id,
                    PolicyNumber = policy.PolicyNo,
                    StatusId = claimSubmissionOtherTireData.Reject ? commonEm.GetClaimStatusIdByCode("rwp") : commonEm.GetClaimStatusIdByCode("sub"),
                    TotalClaimAmount = Convert.ToDecimal("0.00"),
                    //TotalClaimAmount = currencyEm.ConvertToBaseCurrency(claimSubmissionData.totalClaimAmount, claimDealer.CurrencyId, currentConversionPeriodId),
                    ClaimDate = claimSubmissionOtherTireData.claimDate,
                    ClaimMileage = claimSubmissionOtherTireData.policyDetails.failureMileage,
                    CustomerName = claimSubmissionOtherTireData.policyDetails.customerName,
                    MobileNo = claimSubmissionOtherTireData.policyDetails.mobileNo,
                    LastServiceDate = claimSubmissionOtherTireData.policyDetails.lastServiceDate,
                    LastServiceMileage = claimSubmissionOtherTireData.policyDetails.failureMileage,
                    PlateNo = claimSubmissionOtherTireData.policyDetails.plateNumber,
                    RepairCenter = claimDealer.DealerName,
                    RepairCenterLocation = commonEm.GetCountryNameById(claimDealer.CountryId),
                    VINNo = claimSubmissionOtherTireData.policyDetails.vinNo,
                    FailureDate = DateTime.UtcNow,
                    FailureMileage = claimSubmissionOtherTireData.policyDetails.failureMileage,
                    RejectionTypeId = claimSubmissionOtherTireData.Reject ? commonEm.getClaimRejectTypeByCode("002") : Guid.Empty
                };
                Response = claimRequest;
            }
            catch (Exception)
            { throw; }
            return Response;
        }




    }

    public class ContractViewData
    {
        public ContractInfo Contract { get; set; }
        public List<ContractExtensionInfo> ContractExtensions { get; set; }
        public ExtensionTypeResponseDto ExtensionType { get; set; }
    }
    public class ContractInfo : ContractResponseDto
    {
        public List<NRPCommissionContractMappingResponseDto> Commissions { get; set; }
        public List<CountryTaxesResponseDto> Taxes { get; set; }
        public List<EligibilityResponseDto> Eligibilities { get; set; }
        public decimal taxValue { get; set; }
    }
    public class ContractExtensionInfo : ContractExtensionResponseDto
    {
        public List<RSAAnualPremiumResponseDto> AnualPremiums { get; set; }
        public decimal taxValue { get; set; }

    }
}

