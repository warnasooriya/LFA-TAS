
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class ContractManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        #region Contracts

        //[HttpPost]
        //public string AddContract(JObject data)
        //{
        //    ILog logger = LogManager.GetLogger(typeof(ApiController));
        //    logger.Debug("Add Contract Details method!");
        //    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

        //    ContractViewData NewContract = data.ToObject<ContractViewData>();

        //    IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();
        //    ExtensionTypeRequestDto ExtensionType = new ExtensionTypeRequestDto();
        //    ExtensionTypeRequestDto resultE = new ExtensionTypeRequestDto();

        //    IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
        //    ContractExtensionsRequestDto ContractExtension = new ContractExtensionsRequestDto();
        //    ContractExtensionsRequestDto result = new ContractExtensionsRequestDto();

        //    ContractRequestDto Contract = new ContractRequestDto();
        //    ContractRequestDto resultC = new ContractRequestDto();

        //    List<ContractExtensionsPremiumAddonRequestDto> addons = new List<ContractExtensionsPremiumAddonRequestDto>();

        //    #region PremiumAddones
        //    foreach (var item in NewContract.ContractExtensions.First().PremiumAddones)
        //    {
        //        addons.Add(new ContractExtensionsPremiumAddonRequestDto()
        //        {
        //            Id = item.Id,
        //            ContractExtensionId = item.ContractExtensionId,
        //            PremiumAddonTypeId = item.PremiumAddonTypeId,
        //            Value = item.Value
        //        });
        //    }
        //    #endregion

        //    #region Contract
        //    Contract = new ContractRequestDto()
        //    {
        //        CommodityTypeId = NewContract.Contract.CommodityTypeId,
        //        CountryId = NewContract.Contract.CountryId,
        //        DealerId = NewContract.Contract.DealerId,
        //        DealType = NewContract.Contract.DealType,
        //        DealName = NewContract.Contract.DealName,
        //        EndDate = NewContract.Contract.EndDate,
        //        IsActive = NewContract.Contract.IsActive,
        //        IsAutoRenewal = NewContract.Contract.IsAutoRenewal,
        //        DiscountAvailable = NewContract.Contract.DiscountAvailable,
        //        IsPromotional = NewContract.Contract.IsPromotional,
        //        ItemStatusId = NewContract.Contract.ItemStatusId,
        //        LinkDealId = NewContract.Contract.LinkDealId,
        //        PremiumTotal = NewContract.Contract.PremiumTotal,
        //        GrossPremium = NewContract.Contract.GrossPremium,
        //        ProductId = NewContract.Contract.ProductId,
        //        CommodityUsageTypeId = NewContract.Contract.CommodityUsageTypeId,
        //        Remark = NewContract.Contract.Remark,
        //        StartDate = NewContract.Contract.StartDate,
        //        InsurerId = NewContract.Contract.InsurerId,
        //        ReinsurerId = NewContract.Contract.ReinsurerId,
        //        ClaimLimitation = NewContract.Contract.ClaimLimitation,
        //        LiabilityLimitation = NewContract.Contract.LiabilityLimitation
        //    };
        //    resultC = ContractManagementService.AddContract(Contract, SecurityHelper.Context, AuditHelper.Context);
        //    #endregion

        //    #region Commissions
        //    if (NewContract.Contract.Commissions.Count > 0)
        //    {
        //        INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();
        //        foreach (var item in NewContract.Contract.Commissions)
        //        {
        //            if (item.Id.ToString() == "00000000-0000-0000-0000-000000000000")
        //            {
        //                NRPCommissionContractMappingRequestDto commission = new NRPCommissionContractMappingRequestDto()
        //                {
        //                    Commission = item.Commission,
        //                    ContractId = resultC.Id,
        //                    Id = item.Id,
        //                    IsPercentage = item.IsPercentage,
        //                    NRPCommissionId = item.NRPCommissionId
        //                };
        //                NRPCommissionTypesManagementService.AddNRPCommissionContractMapping(commission, SecurityHelper.Context, AuditHelper.Context);
        //            }
        //            else
        //            {
        //                NRPCommissionContractMappingRequestDto commission = new NRPCommissionContractMappingRequestDto()
        //                {
        //                    Commission = item.Commission,
        //                    ContractId = resultC.Id,
        //                    Id = item.Id,
        //                    IsPercentage = item.IsPercentage,
        //                    NRPCommissionId = item.NRPCommissionId
        //                };
        //                NRPCommissionTypesManagementService.UpdateNRPCommissionContractMapping(commission, SecurityHelper.Context, AuditHelper.Context);
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Taxes
        //    if (NewContract.Contract.Taxes.Count > 0)
        //    {
        //        foreach (var item in NewContract.Contract.Taxes)
        //        {
        //            ITaxManagementService taxMng = ServiceFactory.GetTaxManagementService();
        //            ContractTaxesesResponseDto taxes = ContractManagementService.GetContractTaxess(SecurityHelper.Context, AuditHelper.Context);
        //            ContractTaxesResponseDto tax = taxes.ContractTaxess.Find(c => c.ContractId == NewContract.Contract.Id);
        //            if (tax == null)
        //            {
        //                ContractTaxesRequestDto taxAdd = new ContractTaxesRequestDto()
        //                {
        //                    ContractId = NewContract.Contract.Id,
        //                    CountryTaxesId = item.Id
        //                };
        //                ContractManagementService.AddContractTaxes(taxAdd, SecurityHelper.Context, AuditHelper.Context);
        //            }
        //        }
        //    }
        //    #endregion

        //    #region Eligibility
        //    if (NewContract.Contract.Eligibilities.Count > 0)
        //    {
        //        IEligibilityManagementService EMng = ServiceFactory.GetEligibilityManagementService();
        //        foreach (var item in NewContract.Contract.Eligibilities)
        //        {
        //            item.ContractId = resultC.Id;
        //            EligibilityRequestDto input = new EligibilityRequestDto()
        //            {
        //                AgeFrom = item.AgeFrom,
        //                AgeTo = item.AgeTo,
        //                ContractId = resultC.Id,
        //                IsPercentage = item.IsPercentage,
        //                MileageFrom = item.MileageFrom,
        //                MileageTo = item.MileageTo,
        //                MonthsFrom = item.MonthsFrom,
        //                MonthsTo = item.MonthsTo,

        //                PlusMinus = item.PlusMinus,
        //                Premium = item.Premium
        //            };
        //            EligibilityRequestDto resE = EMng.AddEligibility(input, SecurityHelper.Context, AuditHelper.Context);
        //        }
        //    }
        //    #endregion

        //    if (NewContract.ExtensionType.Id.ToString() == "00000000-0000-0000-0000-000000000000")
        //    {
        //        ExtensionType = new ExtensionTypeRequestDto()
        //        {
        //            CommodityTypeId = NewContract.ExtensionType.CommodityTypeId,
        //            Hours = NewContract.ExtensionType.Hours,
        //            Km = NewContract.ExtensionType.Km,
        //            Month = NewContract.ExtensionType.Month,
        //            ExtensionName = NewContract.ExtensionType.ExtensionName,
        //            ProductId = NewContract.ExtensionType.ProductId
        //        };
        //        resultE = ExtensionTypeManagementService.AddExtensionType(ExtensionType, SecurityHelper.Context, AuditHelper.Context);
        //    }
        //    else
        //    {
        //        ExtensionType = new ExtensionTypeRequestDto()
        //       {
        //           Id = NewContract.ExtensionType.Id,
        //           EntryDateTime = NewContract.ExtensionType.EntryDateTime,
        //           EntryUser = NewContract.ExtensionType.EntryUser,
        //           CommodityTypeId = NewContract.ExtensionType.CommodityTypeId,
        //           Hours = NewContract.ExtensionType.Hours,
        //           Km = NewContract.ExtensionType.Km,
        //           Month = NewContract.ExtensionType.Month,
        //           ProductId = NewContract.ExtensionType.ProductId,
        //           ExtensionName = NewContract.ExtensionType.ExtensionName
        //       };
        //        resultE = ExtensionTypeManagementService.UpdateExtensionType(ExtensionType, SecurityHelper.Context, AuditHelper.Context);
        //    }
        //    if (resultE.ExtensionTypeInsertion && resultC.ContractInsertion)
        //    {
        //        ContractExtension = new ContractExtensionsRequestDto()
        //          {
        //              ContractId = resultC.Id,
        //              AttributeSpecification = NewContract.ContractExtensions.First().AttributeSpecification,
        //              CylinderCounts = NewContract.ContractExtensions.First().CylinderCounts,
        //              EngineCapacities = NewContract.ContractExtensions.First().EngineCapacities,
        //              ExtensionTypeId = resultE.Id,
        //              IsCustAvailable = NewContract.ContractExtensions.First().IsCustAvailable,
        //              Makes = NewContract.ContractExtensions.First().Makes,
        //              MaxGross = NewContract.ContractExtensions.First().MaxGross,
        //              MaxNett = NewContract.ContractExtensions.First().MaxNett,
        //              MinNett = NewContract.ContractExtensions.First().MinNett,
        //              MinGross = NewContract.ContractExtensions.First().MinGross,
        //              Modeles = NewContract.ContractExtensions.First().Modeles,
        //              ManufacturerWarranty = NewContract.ContractExtensions.First().ManufacturerWarranty,
        //              WarrantyTypeId = NewContract.ContractExtensions.First().WarrantyTypeId,
        //              PremiumAddones = addons,
        //              PremiumBasedOnIdGross = NewContract.ContractExtensions.First().PremiumBasedOnIdGross,
        //              PremiumBasedOnIdNett = NewContract.ContractExtensions.First().PremiumBasedOnIdNett,

        //              PremiumCurrencyId = NewContract.ContractExtensions.First().PremiumCurrencyId,
        //              PremiumTotal = NewContract.ContractExtensions.First().PremiumTotal,
        //              GrossPremium = NewContract.ContractExtensions.First().GrossPremium,
        //              RSAProviderId = NewContract.ContractExtensions.First().RSAProviderId,
        //              RegionId = NewContract.ContractExtensions.First().RegionId,
        //              Rate = NewContract.ContractExtensions.First().Rate
        //          };
        //        result = ContractManagementService.AddContractExtensions(ContractExtension, SecurityHelper.Context, AuditHelper.Context);
        //        #region AnualPremiums
        //        if (NewContract.ContractExtensions[0].AnualPremiums.Count > 0)
        //        {
        //            IRSAProviderManagementService rsa = ServiceFactory.GetRSAProviderManagementService();
        //            foreach (var item in NewContract.ContractExtensions[0].AnualPremiums)
        //            {
        //                if (item.Id.ToString() == "00000000-0000-0000-0000-000000000000")
        //                {
        //                    RSAAnualPremiumRequestDto aPremium = new RSAAnualPremiumRequestDto()
        //                    {
        //                        Id = item.Id,
        //                        ContractExtensionId = result.Id,
        //                        Value = item.Value,
        //                        Year = item.Year
        //                    };
        //                    rsa.AddRSAAnualPremium(aPremium, SecurityHelper.Context, AuditHelper.Context);
        //                }
        //                else
        //                {
        //                    RSAAnualPremiumRequestDto aPremium = new RSAAnualPremiumRequestDto()
        //                    {
        //                        Id = item.Id,
        //                        ContractExtensionId = result.Id,
        //                        Value = item.Value,
        //                        Year = item.Year
        //                    };
        //                    rsa.UpdateRSAAnualPremium(aPremium, SecurityHelper.Context, AuditHelper.Context);
        //                }
        //            }
        //        }
        //        #endregion

        //        logger.Info("Contract Details Added");
        //        if (result.ContractExtensionsInsertion)
        //            return "OK";
        //        else
        //            return "Add Contract Details failed!";
        //    }
        //    else
        //        return "Add Contract Details failed!";

        //}


        [HttpPost]
        public string AddNewContract(JObject data)
        {
            String Response = "";
            try
            {
                var contractRequestV2Dto = data.ToObject<ContractRequestV2Dto>();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Response = contractManagementService.AddNewContract(
                    contractRequestV2Dto,
                    SecurityHelper.Context,
                    AuditHelper.Context
                    );
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }

            return Response;
        }


        [HttpPost]
        public string UpdateContract(JObject data)
        {
            String Response = "";
            try
            {
                var contractUpdateRequestV2Dto = data.ToObject<ContractUpdateRequestV2Dto>();
                //var contractRequestV2Dto = data.ToObject<ContractRequestV2Dto>();
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                Response = ContractManagementService.UpdateContractV2(
                    contractUpdateRequestV2Dto,
                    SecurityHelper.Context,
                    AuditHelper.Context
                    );
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }

            return Response;
        }


        [HttpPost]
        public object GetContractId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
            Guid contractId = Guid.Parse(data["Id"].ToString());
            GetContractDetailsByContractIdDto result = contractManagementService.GetFullContractDetailsByIdV2(contractId,
                 SecurityHelper.Context,
            AuditHelper.Context);
            //ContractViewData result = ContractManagementService.GetFullContractDetailsById(ContractId, Guid.Empty, Guid.Empty, false, SecurityHelper.Context, AuditHelper.Context);
            return result;

        }

        [HttpPost]
        public object GetContracts()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

            ContractsResponseDto ContractData = ContractManagementService.GetContracts(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ContractData.Contracts.ToArray();
        }

        [HttpPost]
        public object GetContractsForSearchGrid(ContractSearchGridRequestDto ContractSearchGridRequestDto)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

                object ContractData = ContractManagementService.GetContractsForSearchGrid(
                ContractSearchGridRequestDto,
                SecurityHelper.Context,
                AuditHelper.Context);
                return ContractData;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }
        [HttpPost]
        public bool ContractsUpdateValidityCheck(JObject data)
        {
            bool Response = false;
            try
            {
                Guid ContractId = Guid.Parse(data["Id"].ToString());
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                Response = ContractManagementService.ContractsUpdateValidityCheck(
                ContractId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;

        }

        [HttpPost]
        public bool UpdateContractStatus(JObject data)
        {
            bool Response = false;
            try
            {
                Guid ContractId = Guid.Parse(data["Id"].ToString());
                bool Status = bool.Parse(data["status"].ToString());

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                Response = ContractManagementService.UpdateContractStatus(
                ContractId, Status,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;

        }
        [HttpPost]
        public object GetContractsByIds(JObject data)
        {
            object Response = new object();

            try
            {
                Guid ProductId = Guid.Parse(data["ProductId"].ToString());
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                DateTime Date = DateTime.Parse(data["Date"].ToString());
                Guid CylinderCountId = Guid.Parse(data["CylinderCountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());
                Guid VariantId = Guid.Parse(data["VariantId"].ToString());
                decimal GrossWeight = Convert.ToDecimal(data["grossWeight"].ToString());
                Guid UsageTypeId = Guid.Parse(data["UsageTypeId"].ToString());
                Guid ItemStatusId = Guid.Parse(data["ItemStatusId"].ToString());


                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Response = contractManagementService.GetContrcts(ProductId, DealerId, Date,
                    CylinderCountId, EngineCapacityId, ItemStatusId, MakeId, ModelId, VariantId, GrossWeight, UsageTypeId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
            #region "Shit"

            //ICommodityManagementService commodity = ServiceFactory.GetCommodityManagementService();

            //CommoditiesResponseDto com = commodity.GetAllCommodities(SecurityHelper.Context, AuditHelper.Context);

            //List<ContractResponseDto> result = new List<ContractResponseDto>();

            //ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(SecurityHelper.Context, AuditHelper.Context);

            //List<ContractResponseDto> ContractData = ContractManagementService.GetContracts(SecurityHelper.Context, AuditHelper.Context).Contracts.
            //    FindAll(c =>
            //        c.ProductId == Guid.Parse(data["ProductId"].ToString()) &&
            //        c.DealerId == Guid.Parse(data["DealerId"].ToString()) &&
            //        c.StartDate <= DateTime.Parse(data["Date"].ToString()) &&
            //        c.EndDate > DateTime.Parse(data["Date"].ToString())
            //        );

            //foreach (var item in ContractData)
            //{
            //    IReinsurerContractManagementService ReinsurerContractManagementService = ServiceFactory.GetReinsurerContractManagementService();

            //    ReinsurerContractResponseDto ReinsurerContract = ReinsurerContractManagementService.GetReinsurerContractById(item.ReinsurerId,
            //        SecurityHelper.Context,
            //        AuditHelper.Context);
            //    if (!(ReinsurerContract.ToDate > DateTime.Now.Date))
            //    {
            //        ReinsurerContract = GetContract(ReinsurerContractManagementService.GetReinsurerContracts(SecurityHelper.Context, AuditHelper.Context).ReinsurerContracts, item.ReinsurerId);
            //        if(ReinsurerContract ==null)
            //        {
            //            continue;
            //        }
            //    }
            //    List<ContractExtensionResponseDto> extList = ContractExtension.ContractExtensions.FindAll(c => c.ContractId == item.Id);
            //    foreach (var val in extList)
            //    {
            //        ContractExtensionResponseDto ext = ContractManagementService.GetContractExtensionsById(val.Id, SecurityHelper.Context, AuditHelper.Context);
            //        if (item.CommodityTypeId == com.Commmodities.Find(c => c.CommodityTypeDescription == "Automobile").CommodityTypeId)
            //        {
            //            if (
            //                ext.Makes.Contains(Guid.Parse(data["MakeId"].ToString())) &&
            //                ext.Modeles.Contains(Guid.Parse(data["ModelId"].ToString())) &&
            //                ext.CylinderCounts.Contains(Guid.Parse(data["CylinderCountId"].ToString())) &&
            //                ext.EngineCapacities.Contains(Guid.Parse(data["EngineCapacityId"].ToString()))
            //                )
            //            {
            //                result.Add(item);
            //                break;

            //            }
            //            else if (ext.RSAProviderId.ToString() != "00000000-0000-0000-0000-000000000000")
            //            {
            //                result.Add(item);
            //                break;
            //            }
            //        }

            //        else
            //        {
            //            if (
            //                ext.Makes.Contains(Guid.Parse(data["MakeId"].ToString())) &&
            //                ext.Modeles.Contains(Guid.Parse(data["ModelId"].ToString()))
            //                )
            //            {
            //                result.Add(item);
            //            }
            //        }
            //    }
            //}
            //return result;
            #endregion "Shit"
        }

        ReinsurerContractResponseDto GetContract(List<ReinsurerContractResponseDto> reinsurerList, Guid Id)
        {
            try
            {
                ReinsurerContractResponseDto ret = reinsurerList.Find(r => r.LinkContractId == Id && r.ToDate > DateTime.Now.Date);
                if (ret != null)
                    return ret;
                else
                    return GetContract(reinsurerList.FindAll(r => r.LinkContractId != Id), reinsurerList.Find(r => r.LinkContractId == Id).Id);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        [HttpPost]
        public object GetContractsByCommodityTypeId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
            Guid commodityTypeId = Guid.Parse(data["Id"].ToString());
            ContractsResponseDto ContractData = ContractManagementService.GetContractsByCommodityTypeId(
            commodityTypeId,
            SecurityHelper.Context,
            AuditHelper.Context);
            return ContractData.Contracts.ToArray();
        }



        [HttpPost]
        public object GetLinkContracts(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                Guid dealerId = Guid.Parse(data["DealerId"].ToString());
                Guid productId = Guid.Parse(data["ProductId"].ToString());
                ContractsResponseDto ContractData = ContractManagementService.GetContractsByDealerAndProduct(
                    dealerId, productId,
                SecurityHelper.Context,
                AuditHelper.Context);
                return ContractData.Contracts.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        #endregion

        #region Contract Extensions

        [HttpPost]
        public object GetContractExtensions()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

                ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);

                return ContractExtension.ContractExtensions.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        [HttpPost]
        public object GetContractExtensionById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                IRSAProviderManagementService rsa = ServiceFactory.GetRSAProviderManagementService();

                ContractExtensionResponseDto ContractExtension = ContractManagementService.GetContractExtensionsById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                ContractExtensionInfo ret = new ContractExtensionInfo()
                {
                    AnualPremiums = rsa.GetRSAAnualPremiums(SecurityHelper.Context, AuditHelper.Context).RSAAnualPremiums.FindAll(r => r.ContractExtensionId == ContractExtension.Id),
                    AttributeSpecification = ContractExtension.AttributeSpecification,
                    ContractId = ContractExtension.ContractId,
                    CylinderCounts = ContractExtension.CylinderCounts,
                    EngineCapacities = ContractExtension.EngineCapacities,
                    EntryDateTime = ContractExtension.EntryDateTime,
                    EntryUser = ContractExtension.EntryUser,
                    ExtensionTypeId = ContractExtension.ExtensionTypeId,
                    GrossPremium = ContractExtension.GrossPremium,
                    Id = ContractExtension.Id,
                    IsCustAvailableNett = ContractExtension.IsCustAvailableNett,

                    IsCustAvailableGross = ContractExtension.IsCustAvailableGross,
                    Makes = ContractExtension.Makes,
                    ManufacturerWarrantyGross = ContractExtension.ManufacturerWarrantyGross,
                    ManufacturerWarrantyNett = ContractExtension.ManufacturerWarrantyNett,

                    MaxGross = ContractExtension.MaxGross,
                    MaxNett = ContractExtension.MaxNett,

                    MinNett = ContractExtension.MinNett,
                    MinGross = ContractExtension.MinGross,

                    Modeles = ContractExtension.Modeles,
                    PremiumAddones = ContractExtension.PremiumAddones,
                    PremiumBasedOnIdGross = ContractExtension.PremiumBasedOnIdGross,
                    PremiumBasedOnIdNett = ContractExtension.PremiumBasedOnIdNett,

                    PremiumCurrencyId = ContractExtension.PremiumCurrencyId,
                    PremiumTotal = ContractExtension.PremiumTotal,
                    Rate = ContractExtension.Rate,
                    RegionId = ContractExtension.RegionId,
                    RSAProviderId = ContractExtension.RSAProviderId,
                    WarrantyTypeId = ContractExtension.WarrantyTypeId
                };
                ret.AnualPremiums = ret.AnualPremiums.OrderBy(a => a.Year).ToList();
                return ret;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetContractTaxesByExtensionId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid contractId = Guid.Parse(data["ContractId"].ToString());
                decimal PremiumForTax = decimal.Parse(data["PremiumForTax"].ToString());
                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                return ContractManagementService.GetContractTaxesByExtensionId(contractId, PremiumForTax,
                    SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetContractExtensionByIds(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid contractId = Guid.Parse(data["ContractId"].ToString());
                Guid extentianTypeId = Guid.Parse(data["ExtensionTypeId"].ToString());
                Guid warrentyAndrsaProviderId = Guid.Parse(data["WarrantyTypeId"].ToString());
                Guid variantId = Guid.Parse(data["VariantId"].ToString());
                Guid modelId = Guid.Parse(data["ModelId"].ToString());
                bool isRsa = Boolean.Parse(data["RSA"].ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ContractViewData ContractViewData = ContractManagementService.GetFullContractDetailsById(contractId, variantId, modelId, true,
                    SecurityHelper.Context,
                    AuditHelper.Context);
                ContractExtensionsResponseDto response = new ContractExtensionsResponseDto();
                if (ContractViewData == null)
                    return response;
                List<TAS.Services.Common.Transformer.ContractExtensionInfo> filterdList = ContractViewData.ContractExtensions
                    .Where(a => a.ExtensionTypeId == extentianTypeId &&
                        (isRsa) ? a.RSAProviderId == warrentyAndrsaProviderId : a.WarrantyTypeId == warrentyAndrsaProviderId)
                        .ToList();


                response.ContractExtensions = new List<ContractExtensionResponseDto>();
                foreach (var contractExtention in filterdList)
                {
                    ContractExtensionResponseDto extentionData = new ContractExtensionResponseDto()
                    {
                        AttributeSpecification = contractExtention.AttributeSpecification,
                        ContractId = contractExtention.ContractId,
                        CylinderCounts = contractExtention.CylinderCounts,
                        EngineCapacities = contractExtention.EngineCapacities,
                        EntryDateTime = contractExtention.EntryDateTime,
                        EntryUser = contractExtention.EntryUser,
                        ExtensionTypeId = contractExtention.ExtensionTypeId,
                        GrossPremium = contractExtention.GrossPremium,
                        Id = contractExtention.Id,
                        IsContractExtensionsExists = contractExtention.IsContractExtensionsExists,
                        IsCustAvailableGross = contractExtention.IsCustAvailableGross,
                        IsCustAvailableNett = contractExtention.IsCustAvailableNett,
                        Makes = contractExtention.Makes,
                        ManufacturerWarrantyGross = contractExtention.ManufacturerWarrantyGross,
                        ManufacturerWarrantyNett = contractExtention.ManufacturerWarrantyNett,
                        MaxGross = contractExtention.MaxGross,
                        MaxNett = contractExtention.MaxNett,
                        MinGross = contractExtention.MinGross,
                        MinNett = contractExtention.MinNett,
                        Modeles = contractExtention.Modeles,
                        PremiumAddones = contractExtention.PremiumAddones,
                        PremiumBasedOnIdGross = contractExtention.PremiumBasedOnIdGross,
                        PremiumBasedOnIdNett = contractExtention.PremiumBasedOnIdNett,
                        PremiumCurrencyId = contractExtention.PremiumCurrencyId,
                        PremiumTotal = contractExtention.PremiumTotal,
                        Rate = contractExtention.Rate,
                        RegionId = contractExtention.RegionId,
                        RSAProviderId = contractExtention.RSAProviderId,
                        WarrantyTypeId = contractExtention.WarrantyTypeId,
                        taxValue = contractExtention.taxValue
                    };
                    response.ContractExtensions.Add(extentionData);
                };
                return response.ContractExtensions.FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetContractExtensionsById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();

                ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);

                if (data["ContractId"].ToString() != "00000000-0000-0000-0000-000000000000")
                {
                    Guid x = ContractExtension.ContractExtensions.FindAll(c => c.RSAProviderId.ToString() != "00000000-0000-0000-0000-000000000000")[0].RSAProviderId;
                    return ContractExtension.ContractExtensions.Where(c => c.ContractId == Guid.Parse(data["ContractId"].ToString()) && c.ExtensionTypeId == Guid.Parse(data["ExtensionTypeId"].ToString())).ToArray();
                }
                else
                {
                    return new ContractExtensionsResponseDto();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetContractExtensionsByContractId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return ContractExtension.ContractExtensions.FindAll(c => c.ContractId == Guid.Parse(data["Id"].ToString())).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        #endregion

        #region Eligibility

        [HttpPost]
        public string AddEligibility(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                EligibilityRequestDto Eligibility = data.ToObject<EligibilityRequestDto>();
                IEligibilityManagementService EligibilityManagementService = ServiceFactory.GetEligibilityManagementService();
                EligibilityRequestDto result = EligibilityManagementService.AddEligibility(Eligibility, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Eligibility Added");
                if (result.EligibilityInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Eligibility failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Eligibility failed!";
            }

        }

        [HttpPost]
        public string UpdateEligibility(JObject data)
        {
            try
            {

                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                EligibilityRequestDto Eligibility = data.ToObject<EligibilityRequestDto>();
                IEligibilityManagementService EligibilityManagementService = ServiceFactory.GetEligibilityManagementService();
                EligibilityRequestDto result = EligibilityManagementService.UpdateEligibility(Eligibility, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Eligibility Added");
                if (result.EligibilityInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Eligibility failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Eligibility failed!";
            }

        }

        [HttpPost]
        public object GetEligibilityById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IEligibilityManagementService EligibilityManagementService = ServiceFactory.GetEligibilityManagementService();

                EligibilityResponseDto Eligibility = EligibilityManagementService.GetEligibilityById(Guid.Parse(data["Id"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                return Eligibility;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        [HttpPost]
        public object GetEligibilities()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IEligibilityManagementService EligibilityManagementService = ServiceFactory.GetEligibilityManagementService();

                EligibilitiesResponseDto EligibilityData = EligibilityManagementService.GetEligibilitys(
                SecurityHelper.Context,
                AuditHelper.Context);
                return EligibilityData.Eligibilities.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetEligibilitiesByContractId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                int Mileage = Convert.ToInt32(data["Mileage"].ToString());
                DateTime n = DateTime.Now;
                DateTime val = Convert.ToDateTime(data["Age"].ToString());
                int Age = ((n.Year - val.Year) * 12) + n.Month - val.Month;

                IEligibilityManagementService EligibilityManagementService = ServiceFactory.GetEligibilityManagementService();
                EligibilitiesResponseDto EligibilityData = EligibilityManagementService.GetEligibilitys(SecurityHelper.Context, AuditHelper.Context);
                List<EligibilityResponseDto> selected = EligibilityData.Eligibilities.FindAll(e => e.ContractId == Guid.Parse(data["Id"].ToString()));
                List<EligibilityResponseDto> ret = new List<EligibilityResponseDto>();
                foreach (var item in selected)
                {
                    var eligibility = new EligibilityResponseDto
                    {
                        AgeFrom = item.AgeFrom,
                        AgeTo = item.AgeTo,
                        IsEligibilityExists = item.IsEligibilityExists,
                        IsPercentage = item.IsPercentage,
                        MileageFrom = item.MileageFrom,
                        MileageTo = item.MileageTo,
                        MonthsFrom = item.MonthsTo,
                        PlusMinus = item.PlusMinus,
                        Premium = item.Premium,
                    };

                    ret.Add(eligibility);

                }
                //foreach (var item in selected)
                //{
                //    if (item.AgeComparison == "Less Than or Equal" && Age <= item.Age)
                //    {
                //        ret.Add(item);
                //    }
                //    else if (item.AgeComparison == "Less Than" && Age < item.Age)
                //    {
                //        ret.Add(item);
                //    }
                //    else if (item.AgeComparison == "Equal" && Age == item.Age)
                //    {
                //        ret.Add(item);
                //    }
                //}
                //selected = ret;
                //ret = new List<EligibilityResponseDto>();
                //foreach (var item in selected)
                //{
                //    if (item.MileageComparison == "Less Than or Equal" && Mileage <= item.Mileage)
                //    {
                //        ret.Add(item);
                //    }
                //    else if (item.MileageComparison == "Less Than" && Mileage < item.Mileage)
                //    {
                //        ret.Add(item);
                //    }
                //    else if (item.MileageComparison == "Equal" && Mileage == item.Mileage)
                //    {
                //        ret.Add(item);
                //    }
                //}
                if (ret.Count > 0)
                    return ret.OrderBy(e => e.Premium).First();
                else
                    return ret;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
        #endregion

        #region other

        [HttpPost]
        public object GetCommissionTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();

                NRPCommissionTypessResponseDto Commition = NRPCommissionTypesManagementService.GetNRPCommissionTypess(
                SecurityHelper.Context,
                AuditHelper.Context);
                return Commition.NRPCommissionTypess.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCommissionTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();

                NRPCommissionTypesResponseDto Commition = NRPCommissionTypesManagementService.GetNRPCommissionTypess(
                SecurityHelper.Context,
                AuditHelper.Context).NRPCommissionTypess.Find(c => c.Id == Guid.Parse(data["Id"].ToString()));
                return Commition;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCommissionTypesByContractId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                INRPCommissionTypesManagementService NRPCommissionTypesManagementService = ServiceFactory.GetNRPCommissionTypesManagementService();

                NRPCommissionContractMappingsResponseDto Commition = NRPCommissionTypesManagementService.GetNRPCommissionContractMappings(
                SecurityHelper.Context,
                AuditHelper.Context);
                return Commition.NRPCommissionContractMappings.FindAll(c => c.ContractId == Guid.Parse(data["Id"].ToString())).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCountryTaxesByContractId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ITaxManagementService TaxManagementService = ServiceFactory.GetTaxManagementService();

                List<CountryTaxesResponseDto> result = new List<CountryTaxesResponseDto>();

                ContractTaxesesResponseDto ContractTax = ContractManagementService.GetContractTaxess(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                CountryTaxessResponseDto CountryTax = TaxManagementService.GetAllCountryTaxes(
                    SecurityHelper.Context,
                    AuditHelper.Context);

                foreach (var c in ContractTax.ContractTaxess.FindAll(c => c.ContractId == Guid.Parse(data["Id"].ToString())))
                {
                    foreach (var t in CountryTax.CountryTaxes)
                    {
                        if (t.Id == c.CountryTaxesId)
                        {
                            result.Add(t);
                        }
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetRSAProviders()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();

                RSAProvideresResponseDto Commition = RSAProviderManagementService.GetRSAProviders(
                SecurityHelper.Context,
                AuditHelper.Context);
                return Commition.RSAProviders.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetRegions()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IRegionManagementService RegionManagementService = ServiceFactory.GetRegionManagementService();

                RegionesResponseDto region = RegionManagementService.GetRegions(
                SecurityHelper.Context,
                AuditHelper.Context);
                return region.Regions.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetWarrantyTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();

                WarrantyTypesResponseDto WarrantyType = WarrantyTypeManagementService.GetWarrantyTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return WarrantyType.WarrantyTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetWarrantyTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();

                WarrantyTypesResponseDto WarrantyType = WarrantyTypeManagementService.GetWarrantyTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                if (WarrantyType == null)
                    return null;
                return WarrantyType.WarrantyTypes.Find(w => w.Id == Guid.Parse(data["Id"].ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremiumAddonTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPremiumAddonTypeManagementService PremiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();

                PremiumAddonTypesResponseDto PremiumAddonTypes = PremiumAddonTypeManagementService.GetPremiumAddonTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return PremiumAddonTypes.PremiumAddonTypes.OrderBy(p => p.IndexNo).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremiumAddonTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPremiumAddonTypeManagementService PremiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();

                PremiumAddonTypesResponseDto PremiumAddonTypes = PremiumAddonTypeManagementService.GetPremiumAddonTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return PremiumAddonTypes.PremiumAddonTypes.Find(p => p.Id == Guid.Parse(data["Id"].ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremiumAddonTypeByCommodityTypeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPremiumAddonTypeManagementService PremiumAddonTypeManagementService = ServiceFactory.GetPremiumAddonTypeManagementService();
                Guid CommodityTypeId = Guid.Parse(data["Id"].ToString());
                PremiumAddonTypesResponseDto PremiumAddonTypes = PremiumAddonTypeManagementService.GetPremiumAddonTypes(CommodityTypeId,
                SecurityHelper.Context,
                AuditHelper.Context);
                return PremiumAddonTypes.PremiumAddonTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremiumBasedOns()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();

                PremiumBasedOnesResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOns(
                SecurityHelper.Context,
                AuditHelper.Context);
                return PremiumBasedOns.PremiumBasedOns.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremiumBasedOnsById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IPremiumBasedOnManagementService PremiumBasedOnManagementService = ServiceFactory.GetPremiumBasedOnManagementService();

                PremiumBasedOnResponseDto PremiumBasedOns = PremiumBasedOnManagementService.GetPremiumBasedOnById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
                return PremiumBasedOns;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetExtensionTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();

                ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return ExtensionTypes.ExtensionTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetExtensionTypesByIds(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();

                ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return ExtensionTypes.ExtensionTypes.FindAll(e => e.ProductId == Guid.Parse(data["ProductId"].ToString()) && e.CommodityTypeId == Guid.Parse(data["CommodityTypeId"].ToString())).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetExtensionTypesByContractId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ContractId = Guid.Parse(data["ContractId"].ToString());
                Guid ProductId = Guid.Parse(data["ProductId"].ToString());
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                DateTime Date = DateTime.Parse(data["Date"].ToString());
                Guid CylinderCountId = Guid.Parse(data["CylinderCountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());
                Guid VariantId = Guid.Parse(data["VariantId"].ToString());
                decimal GrossWeight = Convert.ToDecimal(data["grossWeight"].ToString());

                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetAllExtensionTypeByContractId(
                    ContractId, ProductId, DealerId, Date,
                     CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight,
                     SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }



        [HttpPost]
        public object GetExtensionTypesByMakeModel(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                Guid DealerId = Guid.Parse(data["DealerId"].ToString());

                Guid CylinderCountId = Guid.Parse(data["CylindercountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());


                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetAllExtensionTypeByMakeModel(
                   DealerId,CylinderCountId, EngineCapacityId, MakeId, ModelId,
                     SecurityHelper.Context,
                    AuditHelper.Context);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }



        [HttpPost]
        public object GetAttributeSpecificationByExtensionId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid ExtensionId = Guid.Parse(data["ExtensionId"].ToString());
                Guid ContractId = Guid.Parse(data["ContractId"].ToString());
                Guid ProductId = Guid.Parse(data["ProductId"].ToString());
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                DateTime Date = DateTime.Parse(data["Date"].ToString());
                Guid CylinderCountId = Guid.Parse(data["CylinderCountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());
                Guid VariantId = Guid.Parse(data["VariantId"].ToString());
                decimal GrossWeight = Convert.ToDecimal(data["grossWeight"].ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetAttributeSpecificationByExtensionId(
                    ExtensionId,ContractId, ProductId, DealerId, Date,
                     CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight,
                     SecurityHelper.Context,
                    AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCoverTypesByExtensionId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid AttributeSpecificationId = Guid.Parse(data["AttributeSpecificationId"].ToString());
                Guid ExtensionId = Guid.Parse(data["ExtensionId"].ToString());
                Guid ContractId = Guid.Parse(data["ContractId"].ToString());
                Guid ProductId = Guid.Parse(data["ProductId"].ToString());
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                DateTime Date = DateTime.Parse(data["Date"].ToString());
                Guid CylinderCountId = Guid.Parse(data["CylinderCountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());
                Guid VariantId = Guid.Parse(data["VariantId"].ToString());
                Guid ItemStatusId = Guid.Parse(data["ItemStatusId"].ToString());


                decimal GrossWeight = Convert.ToDecimal(data["grossWeight"].ToString());

                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetCoverTypesByExtensionId(
                    AttributeSpecificationId,ExtensionId, ContractId, ProductId, DealerId, Date,
                     CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight,
                     ItemStatusId,SecurityHelper.Context,
                    AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetPremium(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                Guid ContractPremiumId = Guid.Parse(data["CoverTypeId"].ToString());
                decimal Usage = Convert.ToDecimal(data["Usage"].ToString());
                Guid AttributeSpecificationId = Guid.Parse(data["AttributeSpecificationId"].ToString());
                Guid ExtensionId = Guid.Parse(data["ExtensionId"].ToString());
                Guid ContractId = Guid.Parse(data["ContractId"].ToString());
                Guid ItemStatusId = Guid.Parse(data["ItemStatusId"].ToString());


                Guid ProductId = Guid.Parse(data["ProductId"].ToString());
                Guid DealerId = Guid.Parse(data["DealerId"].ToString());
                DateTime Date = DateTime.Parse(data["Date"].ToString());
                Guid CylinderCountId = Guid.Parse(data["CylinderCountId"].ToString());
                Guid EngineCapacityId = Guid.Parse(data["EngineCapacityId"].ToString());
                Guid MakeId = Guid.Parse(data["MakeId"].ToString());
                Guid ModelId = Guid.Parse(data["ModelId"].ToString());
                Guid VariantId = Guid.Parse(data["VariantId"].ToString());
                decimal GrossWeight = Convert.ToDecimal(data["grossWeight"].ToString());


                decimal DealerPrice = Convert.ToDecimal(data["DealerPrice"].ToString());
                DateTime ItemPurchasedDate = DateTime.Parse(data["ItemPurchasedDate"].ToString());

                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetPremium(
                    ContractPremiumId, Usage,AttributeSpecificationId, ExtensionId, ContractId, ProductId, DealerId, Date,
                     CylinderCountId, EngineCapacityId, MakeId, ModelId, VariantId, GrossWeight, ItemStatusId,
                     DealerPrice, ItemPurchasedDate,
                     SecurityHelper.Context,
                    AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCoverTypesByAttributeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                Guid attributeSpecId = Guid.Parse(data["Id"].ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();

                return contractManagementService.GetCoverTypesByAttributeId(
                    attributeSpecId,
                    SecurityHelper.Context,
                    AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        //

        [HttpPost]
        public object GetWarrantyTypesByExtensionTypeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IWarrantyTypeManagementService WarrantyTypeManagementService = ServiceFactory.GetWarrantyTypeManagementService();
                WarrantyTypesResponseDto WarrantyType = WarrantyTypeManagementService.GetWarrantyTypes(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                List<WarrantyTypeResponseDto> result = new List<WarrantyTypeResponseDto>();

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                List<ContractExtensionResponseDto> con = ContractExtension.ContractExtensions.FindAll(c =>
                    c.ContractId == Guid.Parse(data["ContractId"].ToString()) &&
                    c.ExtensionTypeId == Guid.Parse(data["ExtensionTypeId"].ToString()));

                foreach (var item in con)
                {
                    WarrantyTypeResponseDto et = WarrantyType.WarrantyTypes.Find(e => e.Id == item.WarrantyTypeId);
                    if (et != null && result.FindAll(i => i.Id == item.WarrantyTypeId).Count == 0)
                    {
                        result.Add(et);
                    }
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetRSAProvidersByExtensionTypeId(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IRSAProviderManagementService RSAProviderManagementService = ServiceFactory.GetRSAProviderManagementService();
                RSAProvideresResponseDto RSAProviders = RSAProviderManagementService.GetRSAProviders(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                List<RSAProviderResponseDto> result = new List<RSAProviderResponseDto>();

                IContractManagementService ContractManagementService = ServiceFactory.GetContractManagementService();
                ContractExtensionsResponseDto ContractExtension = ContractManagementService.GetContractExtensions(
                    SecurityHelper.Context,
                    AuditHelper.Context);
                List<ContractExtensionResponseDto> con = ContractExtension.ContractExtensions.FindAll(c =>
                    c.ContractId == Guid.Parse(data["ContractId"].ToString()) &&
                    c.ExtensionTypeId == Guid.Parse(data["ExtensionTypeId"].ToString()));

                foreach (var item in con)
                {
                    RSAProviderResponseDto et = RSAProviders.RSAProviders.Find(e => e.Id == item.RSAProviderId);
                    if (et != null && result.FindAll(i => i.Id == item.RSAProviderId).Count == 0)
                    {
                        result.Add(et);
                    }
                }

                return result.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetExtensionTypeById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IExtensionTypeManagementService ExtensionTypeManagementService = ServiceFactory.GetExtensionTypeManagementService();

                ExtensionTypesResponseDto ExtensionTypes = ExtensionTypeManagementService.GetExtensionTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return ExtensionTypes.ExtensionTypes.Find(e => e.Id == Guid.Parse(data["Id"].ToString()));
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetAllCountries()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();
                CountriesResponseDto countryData = countryManagementService.GetAllActiveCountries(
                SecurityHelper.Context,
                AuditHelper.Context);
                return countryData.Countries.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        [HttpPost]
        public object GetCountryById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                ICountryManagementService countryManagementService = ServiceFactory.GetCountryManagementService();

                CountriesResponseDto countryData = countryManagementService.GetAllCountries(
                SecurityHelper.Context,
                AuditHelper.Context);
                return countryData.Countries.Find(c => c.Id == Guid.Parse(data["Id"].ToString()) && c.IsActive);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        #endregion


        [HttpPost]
        public object GetDealTypes()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                IDealTypeManagementService DealTypeManagementService = ServiceFactory.GetDealTypeManagementService();

                DealTypesResponseDto DealTypesData = DealTypeManagementService.GetDealTypes(
                SecurityHelper.Context,
                AuditHelper.Context);
                return DealTypesData.DealTypes.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        [HttpPost]
        public object GetAllInsuaranceLimitaionsByCommodityType(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Guid commodityTypeId = Guid.Parse(data["Id"].ToString());
                string productType = data["productType"].ToString();
                response = contractManagementService.GetAllInsuaranceLimitaionsByCommodityType(
                commodityTypeId, productType,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }


        [HttpPost]
        public object AddNewInsuaranceLimitation(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                InsuaranceLimitationRequestDto insuaranceLimitation = data.ToObject<InsuaranceLimitationRequestDto>();
                response = contractManagementService.AddNewInsuaranceLimitation(
                insuaranceLimitation,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        [HttpPost]
        public object GetAllAttributeSpecificationsByInsuranceLimitataionId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Guid insuranceLimitationId = Guid.Parse(data["insuaranceLimitationId"].ToString());
                Guid contractId = Guid.Parse(data["contractId"].ToString());

                response = contractManagementService.GetAllAttributeSpecificationsByInsuranceLimitataionId(
                insuranceLimitationId, contractId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }


        [HttpPost]
        public object GetAllMakeModelDetailsByExtensionId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Guid extensionId = Guid.Parse(data["extensionId"].ToString());

                response = contractManagementService.GetAllMakeModelDetailsByExtensionId(
                extensionId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }

        [HttpPost]
        public object GetAllPremiumByExtensionId(JObject data)
        {
            object response = null;
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IContractManagementService contractManagementService = ServiceFactory.GetContractManagementService();
                Guid extensionId = Guid.Parse(data["extensionId"].ToString());

                response = contractManagementService.GetAllPremiumByExtensionId(
                extensionId,
                SecurityHelper.Context,
                AuditHelper.Context);
            }
            catch (Exception ex)
            { logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException); }

            return response;
        }
    }

}
