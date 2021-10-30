using Microsoft.VisualBasic;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.Caching;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class ContractEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region Contract
        public List<Contract> GetContracts()
        {
            List<Contract> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Contract> ContractData = session.Query<Contract>();
            entities = ContractData.ToList();
            return entities;
        }

        public List<ContractResponseDto> GetContractsByCommodityType(Guid commodityTypeId)
        {
            List<ContractResponseDto> contractResponsesList = new List<ContractResponseDto>();
            ISession session = EntitySessionManager.GetSession();
            List<Contract> contractList = session.Query<Contract>().Where(a => a.CommodityTypeId == commodityTypeId).ToList();
            ContractExtensions contractExtension = GetFirstContractExtensionByContractId(contractList[0].Id);
            if (contractExtension != null)
            {
                foreach (var Contract in contractList)
                {

                    ContractResponseDto contract = new ContractResponseDto();

                    contract.Id = Contract.Id;
                    contract.IsAutoRenewal = Contract.IsAutoRenewal;
                    contract.DiscountAvailable = Contract.DiscountAvailable;
                    contract.IsPromotional = Contract.IsPromotional;
                    contract.CommodityTypeId = Contract.CommodityTypeId;
                    contract.CountryId = Contract.CountryId;
                    contract.DealName = Contract.DealName;
                    contract.DealerId = Contract.DealerId;
                    contract.DealType = Contract.DealType;
                    contract.EndDate = Contract.EndDate;
                    //   contract.ItemStatusId = Contract.ItemStatusId;
                    contract.LinkDealId = Contract.LinkDealId;
                    contract.ProductId = Contract.ProductId;
                    contract.Remark = Contract.Remark;
                    contract.StartDate = Contract.StartDate;
                    contract.InsurerId = Contract.InsurerId;
                    // contract.ReinsurerId = Contract.ReinsurerId;
                    contract.IsActive = Contract.IsActive;
                    contract.EntryDateTime = Contract.EntryDateTime;
                    contract.EntryUser = Contract.EntryUser;
                    // contract.PremiumTotal = Contract.PremiumTotal;
                    //  contract.GrossPremium = Contract.GrossPremium;
                    //  contract.ClaimLimitation = currencyEntityManager.ConvertFromBaseCurrency(Contract.ClaimLimitation, contractExtension.PremiumCurrencyId, contractExtension.CurrencyPeriodId);
                    //  contract.LiabilityLimitation = currencyEntityManager.ConvertFromBaseCurrency(Contract.LiabilityLimitation, contractExtension.PremiumCurrencyId, contractExtension.CurrencyPeriodId);
                    contract.CommodityUsageTypeId = Contract.CommodityUsageTypeId;

                    //need to write other fields
                    contractResponsesList.Add(contract);
                }
            }
            return contractResponsesList;
        }


        public List<Contract> GetContractsByDealerAndProduct(Guid dealerId , Guid productId)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Contract>().Where(c=>c.DealerId==dealerId && c.ProductId == productId).ToList(); ;
        }

        public ContractResponseDto GetContractById(Guid ContractId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                ContractResponseDto contract = new ContractResponseDto();

                var query =
                    from Contract in session.Query<Contract>()
                    where Contract.Id == ContractId
                    select new { Contract = Contract };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    contract.Id = result.First().Contract.Id;
                    contract.IsPromotional = result.First().Contract.IsPromotional;
                    contract.IsAutoRenewal = result.First().Contract.IsAutoRenewal;
                    contract.CommodityTypeId = result.First().Contract.CommodityTypeId;
                    contract.CountryId = result.First().Contract.CountryId;
                    contract.DealName = result.First().Contract.DealName;
                    contract.DealerId = result.First().Contract.DealerId;
                    contract.DealType = result.First().Contract.DealType;
                    contract.EndDate = result.First().Contract.EndDate;
                    //contract.ItemStatusId = result.First().Contract.ItemStatusId;
                    contract.DiscountAvailable = result.First().Contract.DiscountAvailable;
                    contract.LinkDealId = result.First().Contract.LinkDealId;
                    contract.ProductId = result.First().Contract.ProductId;
                    contract.Remark = result.First().Contract.Remark;
                    contract.StartDate = result.First().Contract.StartDate;
                    contract.InsurerId = result.First().Contract.InsurerId;
                    //contract.ReinsurerId = result.First().Contract.ReinsurerId;
                    contract.IsActive = result.First().Contract.IsActive;
                    contract.EntryDateTime = result.First().Contract.EntryDateTime;
                    contract.EntryUser = result.First().Contract.EntryUser;
                    // contract.PremiumTotal = result.First().Contract.PremiumTotal;
                    contract.CommodityUsageTypeId = result.First().Contract.CommodityUsageTypeId;
                    // contract.GrossPremium = result.First().Contract.GrossPremium;
                    contract.ClaimLimitation = result.First().Contract.ClaimLimitation;
                    contract.LiabilityLimitation = result.First().Contract.LiabilityLimitation;

                    contract.IsContractExists = true;
                    return contract;
                }
                else
                {
                    contract.IsContractExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }



        internal bool AddContract(ContractRequestDto Contract)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Contract contract = new Entities.Contract
                {
                    Id = new Guid(),
                    IsAutoRenewal = Contract.IsAutoRenewal,
                    IsPromotional = Contract.IsPromotional,
                    CommodityTypeId = Contract.CommodityTypeId,
                    CountryId = Contract.CountryId,
                    DealName = Contract.DealName,
                    DealerId = Contract.DealerId,
                    DealType = Contract.DealType,
                    EndDate = Contract.EndDate,
                    // contract.ItemStatusId = Contract.ItemStatusId;
                    DiscountAvailable = Contract.DiscountAvailable,
                    LinkDealId = Contract.LinkDealId,
                    ProductId = Contract.ProductId,
                    Remark = Contract.Remark,
                    StartDate = Contract.StartDate,
                    InsurerId = Contract.InsurerId,
                    //contract.ReinsurerId = Contract.ReinsurerId;
                    IsActive = Contract.IsActive,
                    //contract.PremiumTotal = Contract.PremiumTotal;
                    CommodityUsageTypeId = Contract.CommodityUsageTypeId,
                    //contract.GrossPremium = Contract.GrossPremium;
                    ClaimLimitation = Contract.ClaimLimitation,
                    LiabilityLimitation = Contract.LiabilityLimitation,
                    EntryDateTime = DateTime.Today.ToUniversalTime(),
                    EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050")
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(contract);
                    transaction.Commit();
                    Contract.Id = contract.Id;
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateContract(ContractRequestDto Contract)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Contract contract = new Entities.Contract
                {
                    Id = Contract.Id,
                    IsPromotional = Contract.IsPromotional,
                    IsAutoRenewal = Contract.IsAutoRenewal,
                    CommodityTypeId = Contract.CommodityTypeId,
                    CountryId = Contract.CountryId,
                    DealName = Contract.DealName,
                    DealerId = Contract.DealerId,
                    DealType = Contract.DealType,
                    EndDate = Contract.EndDate,
                    DiscountAvailable = Contract.DiscountAvailable,
                    LinkDealId = Contract.LinkDealId,
                    ProductId = Contract.ProductId,
                    Remark = Contract.Remark,
                    StartDate = Contract.StartDate,
                    InsurerId = Contract.InsurerId,
                    IsActive = Contract.IsActive,
                    EntryDateTime = Contract.EntryDateTime,
                    EntryUser = Contract.EntryUser,
                    CommodityUsageTypeId = Contract.CommodityUsageTypeId,
                    ClaimLimitation = Contract.ClaimLimitation,
                    LiabilityLimitation = Contract.LiabilityLimitation
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(contract);
                    Contract.Id = contract.Id;
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal object ContrctsRetrievalInPolicyRegistration(Guid ProductId, Guid DealerId, DateTime Date,
            Guid CylinderCountId, Guid EngineCapacityId, Guid ItemStatusId ,Guid MakeId, Guid ModelId, Guid VariantId, Guid UsageTypeId, decimal GrossVehicleWeight = 0)
        {
            object response = new object();
            try
            {

                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.ProductId == ProductId);
                filterContract = filterContract.And(a => a.DealerId == DealerId);
                filterContract = filterContract.And(a => a.StartDate <= Date);
                filterContract = filterContract.And(a => a.EndDate > Date);
                filterContract = filterContract.And(a => a.CommodityUsageTypeId == UsageTypeId);

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId))
                    .Select(d => d.Id);
                IEnumerable<Guid> ContractExtensionPremiumIds = session.Query<ContractExtensionPremium>()
                    .Where(a => a.ItemStatusId == ItemStatusId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == MakeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == ModelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == CylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == EngineCapacityId).Select(b => b.ContractExtensionId);

                Product product = session.Query<Product>().FirstOrDefault(a => a.Id == ProductId);
                string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels).Intersect(ContractExtensionPremiumIds);
                if (commodityType.ToLower() == "a")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }

                if (commodityType.ToLower() == "b")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }

                if (IsGuid(VariantId.ToString()))
                {
                    IEnumerable<Guid> contractExtensionIdVariants = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == VariantId).Select(b => b.ContractExtensionId);
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdVariants);
                }

                if (GrossVehicleWeight > 0)
                {
                    VehicleWeight matchingVehicleWeight = session.Query<VehicleWeight>()
                        .Where(a => a.WeightTo != decimal.Zero)
                        .FirstOrDefault(a => a.WeightFrom <= GrossVehicleWeight && a.WeightTo >= GrossVehicleWeight);
                    //we are good coz one slab is only gonna match
                    if (matchingVehicleWeight == null)
                    {
                        matchingVehicleWeight = session.Query<VehicleWeight>().Where(a => a.WeightTo == decimal.Zero)
                             .FirstOrDefault(a => a.WeightFrom <= GrossVehicleWeight);
                    }

                    //if no weight slab matching, we have to consider other cirteras
                    if (matchingVehicleWeight != null)
                    {
                        List<Guid> allEligibleContractsForGvw = new List<Guid>();
                        //by contract filtering
                        IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        .Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                        //by contracts which dont selected any contractExt Gvw
                        List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                            .Select(a => a.Id).ToList();
                        List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                            .Select(a => a.ContractExtensionId).ToList();
                        //no gvw contracts
                        IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                        allEligibleContractsForGvw.AddRange(allowedContracts);

                        allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                    }
                }

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<Guid> insuranceLimitationIds = session.QueryOver<ContractExtensions>()
                    .WhereRestrictionOn(c => c.Id).IsIn(finalExtensionIds)
                    .Select(o => o.ContractInsuanceLimitationId).List<Guid>();


                IList<Guid> finalContractIds = session.QueryOver<ContractInsuaranceLimitation>()
                    .WhereRestrictionOn(k => k.Id).IsIn(insuranceLimitationIds.ToList())
                      .Select(o => o.ContractId).List<Guid>();


                IList<Contract> finalContracts = session.QueryOver<Contract>()
                      .WhereRestrictionOn(k => k.Id).IsIn(finalContractIds.ToList())
                      .List<Contract>();


                response = finalContracts
                .Select(z => new
                {
                    z.Id,
                    z.DealName,
                    z.IsPremiumVisibleToDealer
                }).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }

        internal object GetContractsForSearchGrid(ContractSearchGridSearchCriterias contractSearchGridSearchCriterias, GridPaginationOptions gridPaginationOptions)
        {
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                if (IsGuid(contractSearchGridSearchCriterias.countryId.ToString()))
                    filterContract = filterContract.And(a => a.CountryId == contractSearchGridSearchCriterias.countryId);
                if (IsGuid(contractSearchGridSearchCriterias.dealerId.ToString()))
                    filterContract = filterContract.And(a => a.DealerId == contractSearchGridSearchCriterias.dealerId);
                if (IsGuid(contractSearchGridSearchCriterias.productId.ToString()))
                    filterContract = filterContract.And(a => a.ProductId == contractSearchGridSearchCriterias.productId);
                if (!String.IsNullOrEmpty(contractSearchGridSearchCriterias.dealName))
                    filterContract = filterContract.And(a => a.DealName.ToLower().Contains(contractSearchGridSearchCriterias.dealName.ToLower()));

                int skip = 0;
                if (gridPaginationOptions.pageNumber > 1)
                {
                    skip = (gridPaginationOptions.pageNumber - 1) * gridPaginationOptions.pageSize;
                }

                //queryed contract list
                var ContractGridDetailsFilterd = session.Query<Contract>().Where(filterContract)
                    .Join(session.Query<Product>(), b => b.ProductId, c => c.Id, (b, c) => new { b, c })
                    .Join(session.Query<Country>(), d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                    .Join(session.Query<Dealer>(), f => f.d.b.DealerId, g => g.Id, (f, g) => new { f, g })
                    .Join(session.Query<CommodityType>(), h => h.f.d.b.CommodityTypeId, i => i.CommodityTypeId, (h, i) => new { h, i })
                    .Skip(skip)
                    .Take(gridPaginationOptions.pageSize)
                    .Select(a => new
                    {
                        a.h.f.d.b.Id,
                        CommodityType = a.i.CommodityTypeDescription,
                        a.h.f.e.CountryName,
                        a.h.g.DealerName,
                        a.h.f.d.c.Productname,
                        a.h.f.d.b.DealName,
                        a.h.f.d.b.IsActive
                    }).ToArray();


                long TotalRecords = long.Parse(session.Query<Contract>().Where(filterContract)
                    .Join(session.Query<Product>(), b => b.ProductId, c => c.Id, (b, c) => new { b, c })
                    .Join(session.Query<Country>(), d => d.b.CountryId, e => e.Id, (d, e) => new { d, e })
                    .Join(session.Query<Dealer>(), f => f.d.b.DealerId, g => g.Id, (f, g) => new { f, g })
                    .Join(session.Query<CommodityType>(), h => h.f.d.b.CommodityTypeId, i => i.CommodityTypeId, (h, i) => new { h, i })
                    .Count().ToString());
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = ContractGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
                return false;
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
        #endregion

        #region Extensions
        internal static List<ExtensionType> GetContractExtensionsByModelId(Guid modelId, Guid dealerId)
        {
            List<ExtensionType> entities = new List<ExtensionType>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //var query =
                //               from contract in session.Query<Contract>()
                //               join contractExtension in session.Query<ContractExtensions>() on contract.Id equals contractExtension.ContractId
                //               join contractExtensionModel in session.Query<ContractExtensionModel>() on contractExtension.Id equals contractExtensionModel.ContractExtensionId
                //               join extensionType in session.Query<ExtensionType>() on contractExtension.ExtensionTypeId equals extensionType.Id
                //               where contract.DealerId == dealerId && contractExtensionModel.ModelId == modelId
                //               select extensionType;


                //foreach (var item in query.Distinct())
                //{
                //    var a = new ExtensionType();
                //    a.Id = item.Id;
                //    a.CommodityTypeId = item.CommodityTypeId;
                //    a.EntryDateTime = item.EntryDateTime;
                //    a.EntryUser = item.EntryUser;
                //    a.ExtensionName = item.ExtensionName;
                //    a.Hours = item.Hours;
                //    a.Km = item.Km;
                //    a.Month = item.Month;

                //    entities.Add(a);
                //}

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return entities;
        }
        #endregion//Move to contract extensions entity manager : Mihiri

        #region Contract Price
        internal static List<ContractPriceResponseDto> GetPrices(Guid modelId, Guid DealerId) //, Guid ExtensionTypeid
        {
            List<ContractPriceResponseDto> entities = new List<ContractPriceResponseDto>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                //var query =
                //               from contract in session.Query<Contract>()
                //               join contractExtension in session.Query<ContractExtensions>() on contract.Id equals contractExtension.ContractId
                //               join contractExtensionModel in session.Query<ContractExtensionModel>() on contractExtension.Id equals contractExtensionModel.ContractExtensionId
                //               join extensionType in session.Query<ExtensionType>() on contractExtension.ExtensionTypeId equals extensionType.Id
                //               join warantyType in session.Query<WarrantyType>() on contractExtension.WarrantyTypeId equals warantyType.Id
                //               join premiumBasedOn in session.Query<PremiumBasedOn>() on contractExtension.PremiumBasedOnIdNett equals premiumBasedOn.Id
                //               where contract.DealerId == DealerId && contractExtensionModel.ModelId == modelId  //&& contractExtension.ExtensionTypeId == ExtensionTypeid
                //               select new { contractExtension, warantyType, contract, premiumBasedOn, extensionType };

                //var list = query.ToList();

                //foreach (var item in query)
                //{
                //    ContractPriceResponseDto contractPrice = new ContractPriceResponseDto();

                //    contractPrice.contractId = item.contract.Id;
                //    contractPrice.coverType = item.warantyType.WarrantyTypeDescription;
                //    contractPrice.dealName = item.contract.DealName;
                //    contractPrice.Max = Convert.ToDouble(item.contractExtension.MaxNett);
                //    contractPrice.Min = Convert.ToDouble(item.contractExtension.MinNett);
                //    contractPrice.PremiumTotal = Convert.ToDouble(item.contractExtension.PremiumTotal);
                //    contractPrice.PremiumBasedon = item.premiumBasedOn.Code;
                //    contractPrice.price = 0;
                //    contractPrice.Month = item.extensionType.Month;
                //    contractPrice.ExtensionName = item.extensionType.ExtensionName;
                //    contractPrice.ExtensionID = item.extensionType.Id;

                //    entities.Add(contractPrice);
                //}
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return entities;
        }
        #endregion

        #region Taxes
        public List<ContractTaxes> GetContractTaxess()
        {
            List<ContractTaxes> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ContractTaxes> ContractTaxesData = session.Query<ContractTaxes>();
            entities = ContractTaxesData.ToList();
            return entities;
        }

        public ContractTaxesResponseDto GetContractTaxesById(Guid ContractTaxesId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                ContractTaxesResponseDto pDto = new ContractTaxesResponseDto();

                var query =
                    from ContractTaxes in session.Query<ContractTaxes>()
                    where ContractTaxes.Id == ContractTaxesId
                    select new { ContractTaxes = ContractTaxes };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ContractTaxes.Id;
                    pDto.ContractId = result.First().ContractTaxes.ContractId;
                    pDto.CountryTaxesId = result.First().ContractTaxes.CountryTaxId;

                    pDto.IsContractTaxesExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsContractTaxesExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        internal bool AddContractTaxes(ContractTaxesRequestDto ContractTaxes)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                ContractTaxes pr = new Entities.ContractTaxes
                {
                    Id = new Guid(),
                    ContractId = ContractTaxes.ContractId,
                    CountryTaxId = ContractTaxes.CountryTaxesId
                };



                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateContractTaxes(ContractTaxesRequestDto ContractTaxes)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ContractTaxes pr = new Entities.ContractTaxes
                {
                    Id = ContractTaxes.Id,
                    ContractId = ContractTaxes.ContractId,
                    CountryTaxId = ContractTaxes.CountryTaxesId
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }
        #endregion



        internal ContractViewData FullContractDetailsById(Guid currentContractId, Guid variantId, Guid modelId, bool withTax)
        {
            ContractViewData contractViewData = new ContractViewData();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //contract
                Contract contract = session.Query<Contract>()
                    .FirstOrDefault(a => a.Id == currentContractId);
                //contract extention
                IEnumerable<ContractExtensions> contractExtention = session.Query<ContractExtensions>();
                //   .Where(a => a.ContractId == currentContractId);

                //commissions
                IEnumerable<NRPCommissionContractMapping> NRPCommissionContractMapping = session.Query<NRPCommissionContractMapping>()
                    .Where(a => a.ContractId == currentContractId);

                var T_NRPCommissionContractMapping = new DBDTOTransformer().Commissions(NRPCommissionContractMapping);

                //tax
                IEnumerable<ContractTaxMapping> ContractTaxes = session.Query<ContractTaxMapping>()
                    .Where(a => a.ContractId == currentContractId);
                IEnumerable<CountryTaxes> CountryTaxes = session.Query<CountryTaxes>()
                    .Where(a => ContractTaxes.Any(b => b.CountryTaxId == a.Id));
                var T_ContractTaxes = new DBDTOTransformer().CountryTaxes(CountryTaxes);
                //eligibilities
                IEnumerable<Eligibility> Eligibility = session.Query<Eligibility>()
                    .Where(a => a.ContractId == currentContractId);
                //var T_Eligibility =
                //    new DBDTOTransformer().Eligibilities(Eligibility, contractExtention.FirstOrDefault()., contractExtention.FirstOrDefault().PremiumCurrencyId);
                //cyllinder count
                IEnumerable<ContractExtensionCylinderCount> ContractExtensionCylinderCount = session.Query<ContractExtensionCylinderCount>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);

                //engine capacity
                IEnumerable<ContractExtensionEngineCapacity> ContractExtensionEngineCapacity = session.Query<ContractExtensionEngineCapacity>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);
                //makes
                IEnumerable<ContractExtensionMake> ContractExtensionMake = session.Query<ContractExtensionMake>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);

                //models
                IEnumerable<ContractExtensionModel> ContractExtensionModel = session.Query<ContractExtensionModel>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);

                //variants
                IEnumerable<ContractExtensionVariant> ContractExtensionaVariants = session.Query<ContractExtensionVariant>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);

                //addones
                IEnumerable<ContractExtensionsPremiumAddon> ContractExtensionsPremiumAddon = session.Query<ContractExtensionsPremiumAddon>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);

                //annual premium
                IEnumerable<RSAAnualPremium> RSAAnualPremium = session.Query<RSAAnualPremium>()
                    .Where(a => a.ContractExtensionId == contractExtention.FirstOrDefault().Id);
                //extention type
                // IEnumerable<ExtensionType> ExtentionType = session.Query<ExtensionType>()
                //.Where(a => a.Id == contractExtention.FirstOrDefault().ExtensionTypeId);
                //variant
                Variant variant = session.Query<Variant>()
                    .Where(a => a.Id == variantId).FirstOrDefault();

                List<PremiumAddonType> addonTypes = session.Query<PremiumAddonType>()
                    .Where(a => ContractExtensionsPremiumAddon.Any(b => b.PremiumAddonTypeId == a.Id)).ToList();
                //model
                Model model = session.Query<Model>()
                    .Where(a => a.Id == modelId).FirstOrDefault();
                contractViewData = new ContractViewData()
                {
                    Contract = new DBDTOTransformer().ContractToContractInfo(
                    contract,
                    T_NRPCommissionContractMapping,
                    T_ContractTaxes,
                    null, contractExtention.FirstOrDefault(), withTax,
                    addonTypes, variant, model, ContractExtensionsPremiumAddon),
                    ContractExtensions = new DBDTOTransformer().ContractExteintion(
                    RSAAnualPremium, ContractExtensionsPremiumAddon, contractExtention,
                    ContractExtensionCylinderCount, ContractExtensionEngineCapacity,
                ContractExtensionMake, ContractExtensionModel, ContractExtensionaVariants, T_ContractTaxes, withTax,
                addonTypes, variant, model, ContractExtensionsPremiumAddon),
                    ExtensionType = new DBDTOTransformer().ExtentionType(null)
                };

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                contractViewData = null;
            }
            return contractViewData;
        }


        internal static string AddNewContract(ContractRequestV2Dto addContractRequestObject,string UniqueDbName)
        {
            string response;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                #region "validation"
                if (addContractRequestObject.data.contract == null
                    || addContractRequestObject.data.products == null)
                {
                    response = "Input data is invalid.";
                    return response;
                }
                //currency validationContractInsuaranceLimitation

                Guid currencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currencyPeriodId.ToString()))
                {
                    response = "No currency period is defined for today.";
                    return response;
                }
                Dealer dealer =
                    session.Query<Dealer>().FirstOrDefault(a => a.Id == addContractRequestObject.data.contract.dealerId);
                if (dealer == null)
                {
                    response = "Invalid dealer selection.";
                    return response;
                }
                Guid dealerCurrency = dealer.CurrencyId;
                if (!IsGuid(dealerCurrency.ToString()))
                {
                    response = "Selected dealer's currency not assigned.";
                    return response;
                }
                CurrencyConversions conversions = session.Query<CurrencyConversions>()
                    .FirstOrDefault(
                        a => a.CurrencyConversionPeriodId == currencyPeriodId && a.CurrencyId == dealerCurrency);
                if (conversions == null)
                {
                    response = "Selected dealer's currency not present in current conversion period.";
                    return response;
                }
                #endregion
                List<ContractExtensions> contractontractExtensionsList = new List<ContractExtensions>();
                List<ContractExtensionMake> contractExtensionMakeList = new List<ContractExtensionMake>();
                List<ContractExtensionModel> contractExtensionModelList = new List<ContractExtensionModel>();
                List<ContractExtensionCylinderCount> contractExtensionCcList = new List<ContractExtensionCylinderCount>();
                List<ContractExtensionEngineCapacity> contractExtensionEcList = new List<ContractExtensionEngineCapacity>();
                List<ContractExtensionVariant> contractExtensionVariantList = new List<ContractExtensionVariant>();
                List<ContractExtensionGVW> contractExtensionGvwList = new List<ContractExtensionGVW>();
                List<ContractExtensionPremium> contractExtensionPremiumList = new List<ContractExtensionPremium>();
                List<ContractExtensionsPremiumAddon> contractExtensionAddonList = new List<ContractExtensionsPremiumAddon>();
                List<RSAAnualPremium> contractAnualPremiums = new List<RSAAnualPremium>();
                List<ContractInsuaranceLimitation> contractInsuaranceLimitations = new List<ContractInsuaranceLimitation>();

                Guid contractId = Guid.NewGuid();
                Contract contract = new Contract()
                {
                    Id = contractId,
                    CommodityTypeId = addContractRequestObject.data.contract.commodityTypeId,
                    CountryId = addContractRequestObject.data.contract.countryId,
                    ClaimLimitation = currencyEntityManager.ConvertToBaseCurrency(
                        addContractRequestObject.data.contract.claimLimitation, dealerCurrency, currencyPeriodId),
                    CommodityCategoryId = addContractRequestObject.data.contract.commodityCategoryId,
                    CommodityUsageTypeId = addContractRequestObject.data.contract.commodityUsageTypeId,
                    DealName = addContractRequestObject.data.contract.dealName,
                    DealType = addContractRequestObject.data.contract.dealType,
                    DealerId = addContractRequestObject.data.contract.dealerId,
                    DiscountAvailable = addContractRequestObject.data.contract.discountAvailable,
                    EndDate = addContractRequestObject.data.contract.contractEndDate.ToUniversalTime(),
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = addContractRequestObject.data.contract.loggedInUserId,
                    InsurerId = addContractRequestObject.data.contract.insurerId,
                    IsActive = addContractRequestObject.data.contract.isActive,
                    IsAutoRenewal = addContractRequestObject.data.contract.isAutoRenewal,
                    IsPromotional = addContractRequestObject.data.contract.isPromotional,
                    LiabilityLimitation = currencyEntityManager.ConvertToBaseCurrency(
                        addContractRequestObject.data.contract.liabilityLimitation, dealerCurrency, currencyPeriodId),
                    LinkDealId = addContractRequestObject.data.contract.linkDealId,
                    ProductId = addContractRequestObject.data.contract.productId,
                    Remark = addContractRequestObject.data.contract.remark,
                    StartDate = addContractRequestObject.data.contract.contractStartDate.ToUniversalTime(),
                    labourChargeApplicableOnPolicySold = addContractRequestObject.data.contract.labourChargeApplicableOnPolicySold,
                    ReinsurerContractId = addContractRequestObject.data.contract.reinsurerContractId,
                    ConversionRate = currencyEntityManager.GetConversionRate(dealerCurrency, currencyPeriodId, true),
                    CurrencyId = dealerCurrency,
                    CurrencyPeriodId = currencyPeriodId,
                    IsPremiumVisibleToDealer = addContractRequestObject.data.contract.isPremiumVisibleToDealer,
                    AnnualInterestRate = addContractRequestObject.data.contract.AnnualInterestRate

                };




                foreach (ProductV2 product in addContractRequestObject.data.products)
                {
                    Guid insuaranceLimitationId = Guid.NewGuid();
                    ContractInsuaranceLimitation contractInsuaranceLimitation = new ContractInsuaranceLimitation()
                    {
                        Id = insuaranceLimitationId,
                        ContractId = contractId,
                        EntryDateTime = DateTime.UtcNow,
                        BaseProductId = product.Id,
                        InsuaranceLimitationId = product.InsuaranceLimitationId
                    };
                    contractInsuaranceLimitations.Add(contractInsuaranceLimitation);

                    Guid contractExtensionId = Guid.NewGuid();
                    ContractExtensions contractExtensions = new ContractExtensions()
                    {
                        Id = contractExtensionId,
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = addContractRequestObject.data.contract.loggedInUserId,
                        Rate = product.annualPremiumTotal,
                        IsRSA = product.Productcode.ToUpper() == "RSA",
                        RegionId = product.regionId,
                        RSAProviderId = product.rsaProviderId,
                        ContractInsuanceLimitationId = insuaranceLimitationId,
                        AttributeSpecification = product.attributeSpecificationName,
                        ProductId = product.Id,
                        AttributeSpecificationId = Guid.NewGuid()
                    };
                    contractontractExtensionsList.Add(contractExtensions);
                    //make details
                    contractExtensionMakeList.AddRange(product.selectedMakeList.Select(make => new ContractExtensionMake()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        MakeId = make.id
                    }));
                    //model details
                    contractExtensionModelList.AddRange(product.selectedModelsList.Select(model => new ContractExtensionModel()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        ModelId = model.id
                    }));
                    // cc
                    contractExtensionCcList.AddRange(product.selectedClinderCounts.Select(cc => new ContractExtensionCylinderCount()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        CylinderCountId = cc.id
                    }));
                    //ec
                    contractExtensionEcList.AddRange(product.selectedEngineCapacities.Select(ec => new ContractExtensionEngineCapacity()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        EngineCapacityId = ec.id
                    }));
                    //variant
                    contractExtensionVariantList.AddRange(product.selectedVariantList.Select(variant => new ContractExtensionVariant()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        VariantId = variant.id
                    }));
                    //GVW
                    contractExtensionGvwList.AddRange(product.selectedGrossWeights.Select(gvw => new ContractExtensionGVW()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        GVWId = gvw.id
                    }));

                    //premiums
                    var contractExtensionPremiumId = Guid.NewGuid();
                    ContractExtensionPremium contractExtensionPremium = new ContractExtensionPremium()
                    {
                        Id = contractExtensionPremiumId,
                        //EntryDateTime = DateTime.UtcNow,
                        ContractExtensionId = contractExtensionId,
                        CurrencyId = dealerCurrency,
                        ConversionRate = currencyEntityManager.GetConversionRate(dealerCurrency, currencyPeriodId, true),
                        CurrencyPeriodId = currencyPeriodId,
                        Gross = new CommonEntityManager()
                        .GetPremiumBasedonCodeById(product.premiumBasedOnIdGross).ToLower() == "rp" ? product.grossPremium :
                        currencyEntityManager.ConvertToBaseCurrency(product.grossPremium, dealerCurrency, currencyPeriodId),
                        IsClaimLabourChargesFixed = addContractRequestObject.data.contract.labourChargeApplicableOnPolicySold,
                        IsCustomerAvailableGross = product.isCustAvailableGross,
                        IsCustomerAvailableNett = product.isCustAvailableNett,
                        ItemStatusId = product.itemStatusId,
                        MaxValueGross = currencyEntityManager.ConvertToBaseCurrency(product.maxValueGross, dealerCurrency, currencyPeriodId),
                        MaxValueNett = currencyEntityManager.ConvertToBaseCurrency(product.maxValueNett, dealerCurrency, currencyPeriodId),
                        MinValueGross = currencyEntityManager.ConvertToBaseCurrency(product.minValueGross, dealerCurrency, currencyPeriodId),
                        MinValueNett = currencyEntityManager.ConvertToBaseCurrency(product.minValueNett, dealerCurrency, currencyPeriodId),
                        NRP = new CommonEntityManager()
                        .GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? product.totalNRP :
                        currencyEntityManager.ConvertToBaseCurrency(product.totalNRP, dealerCurrency, currencyPeriodId),
                        PremiumBasedOnGross = product.premiumBasedOnIdGross,
                        PremiumBasedOnNett = product.premiumBasedOnIdNett,
                        WarrentyTypeId = product.warrantyTypeId
                    };
                    contractExtensionPremiumList.Add(contractExtensionPremium);
                    //premium addons
                    //gorss
                    contractExtensionAddonList.AddRange(product.premiumAddonsGross.Select(addon => new ContractExtensionsPremiumAddon()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        PremiumAddonTypeId = addon.Id,
                        PremiumType = "GROSS",
                        Value = new CommonEntityManager()
                        .GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? addon.Value :
                        currencyEntityManager.ConvertToBaseCurrency(addon.Value, dealerCurrency, currencyPeriodId),
                        ContractExtensionPremiumId = contractExtensionPremiumId

                    }));
                    //nett
                    contractExtensionAddonList.AddRange(product.premiumAddonsNett.Select(addon => new ContractExtensionsPremiumAddon()
                    {
                        Id = Guid.NewGuid(),
                        ContractExtensionId = contractExtensionId,
                        PremiumAddonTypeId = addon.Id,
                        PremiumType = "NETT",
                        Value = new CommonEntityManager()
                        .GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? addon.value :
                        currencyEntityManager.ConvertToBaseCurrency(addon.value, dealerCurrency, currencyPeriodId),
                        ContractExtensionPremiumId = contractExtensionPremiumId
                    }));

                    //rsa
                    //if (product.Productcode.ToLower() == "rsa")
                    //{
                    //    //product.annualPremium = List<500>();
                    //    contractAnualPremiums.AddRange(product.annualPremium.Select(annualPremium => new RSAAnualPremium()
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        ContractExtensionId = contractExtensionId,
                    //        Year = annualPremium.Year,
                    //        Value = currencyEntityManager.ConvertToBaseCurrency(annualPremium.Value, dealerCurrency, currencyPeriodId)
                    //    }));
                    //}
                }

                //commissions
                List<NRPCommissionContractMapping> commissionContractMappingsList = addContractRequestObject.data.contract.commissions
                    .Select(commission => new NRPCommissionContractMapping()
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contractId,
                        IsOnGROSS = commission.IsOnGROSS,
                        IsOnNRP = commission.IsOnNRP,
                        IsPercentage = commission.IsPercentage,
                        NRPCommissionId = commission.Id,
                        Commission = commission.IsPercentage ? commission.Commission : currencyEntityManager.ConvertToBaseCurrency(commission.Commission, dealerCurrency, currencyPeriodId)
                    }).ToList();

                //taxes
                List<ContractTaxMapping> contractTaxList = addContractRequestObject.data.contract.texes
                    .Where(a => a.IsSelected)
                    .Select(tax => new ContractTaxMapping()
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contractId,
                        CountryTaxId = tax.Id,

                    }).ToList();

                //eligibility
                List<Eligibility> eligibilityList = addContractRequestObject.data.contract.eligibilities
                    .Select(eligibility => new Eligibility()
                    {
                        ContractId = contractId,
                        Id = Guid.NewGuid(),
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = addContractRequestObject.data.contract.loggedInUserId,
                        IsPercentage = eligibility.isPresentage,
                        AgeFrom = eligibility.ageMin,
                        AgeTo = eligibility.ageMax,
                        MileageFrom = eligibility.milageMin,
                        MileageTo = eligibility.milageMax,
                        MonthsFrom = eligibility.milageMin,
                        MonthsTo = eligibility.monthsMax,
                        PlusMinus = eligibility.plusMinus,
                        Premium = eligibility.isPresentage
                            ? eligibility.premium
                            : currencyEntityManager.ConvertToBaseCurrency(
                                eligibility.premium, dealerCurrency, currencyPeriodId),
                        isMandatory = eligibility.isMandatory
                    }).ToList();


                List<ClaimCriteria> claimCriteriaList = addContractRequestObject.data.contract.claimCriteria
                    .Select(cc => new ClaimCriteria()
                    {
                        Id = Guid.NewGuid(),
                        Label = cc.label,
                        Year = cc.year,
                        ContractId = contractId,
                        Presentage = cc.presentage
                    }).ToList();

                GetContractDetailsByContractIdDto cacheDto = new GetContractDetailsByContractIdDto();

                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(contract, contractId);
                    //limitations

                    foreach (var contractInsuaranceLimitation in contractInsuaranceLimitations)
                    {
                        session.Evict(contractInsuaranceLimitation);
                        session.Save(contractInsuaranceLimitation, contractInsuaranceLimitation.Id);
                    }
                    //ext
                    foreach (var extension in contractontractExtensionsList)
                    {
                        session.Evict(extension);
                        session.Save(extension, extension.Id);
                    }
                    //ext make
                    foreach (var extensionMake in contractExtensionMakeList)
                    {
                        session.Evict(extensionMake);
                        session.Save(extensionMake, extensionMake.Id);
                    }
                    //ext model
                    foreach (var extensionModel in contractExtensionModelList)
                    {
                        session.Evict(extensionModel);
                        session.Save(extensionModel, extensionModel.Id);
                    }
                    //ext cc
                    foreach (var extensioncc in contractExtensionCcList)
                    {
                        session.Evict(extensioncc);
                        session.Save(extensioncc, extensioncc.Id);
                    }
                    //ec
                    foreach (var extensionec in contractExtensionEcList)
                    {
                        session.Evict(extensionec);
                        session.Save(extensionec, extensionec.Id);
                    }
                    //variant
                    foreach (var extensionVariant in contractExtensionVariantList)
                    {
                        session.Evict(extensionVariant);
                        session.Save(extensionVariant, extensionVariant.Id);
                    }
                    //gvw
                    foreach (var extensionGvw in contractExtensionGvwList)
                    {
                        session.Evict(extensionGvw);
                        session.Save(extensionGvw, extensionGvw.Id);
                    }
                    //addons
                    foreach (var addon in contractExtensionAddonList)
                    {
                        session.Evict(addon);
                        session.Save(addon, addon.Id);
                    }

                    //premium
                    foreach (var extensionPremium in contractExtensionPremiumList)
                    {
                        session.Evict(extensionPremium);
                        session.Save(extensionPremium, extensionPremium.Id);
                    }
                    //rsa
                    foreach (var extensionAnnualPremium in contractAnualPremiums)
                    {
                        session.Evict(extensionAnnualPremium);
                        session.Save(extensionAnnualPremium, extensionAnnualPremium.Id);
                    }
                    //commissions
                    foreach (var contractCommission in commissionContractMappingsList)
                    {
                        session.Evict(contractCommission);
                        session.Save(contractCommission, contractCommission.Id);
                    }
                    //taxes
                    foreach (var contractTax in contractTaxList)
                    {
                        session.Evict(contractTax);
                        session.Save(contractTax, contractTax.Id);
                    }
                    //eligibility
                    foreach (var contractEligibility in eligibilityList)
                    {
                        session.Evict(contractEligibility);
                        session.Save(contractEligibility, contractEligibility.Id);
                    }
                    // claim criteria
                    foreach (var claimCriteria in claimCriteriaList)
                    {
                        session.Evict(claimCriteria);
                        session.Save(claimCriteria, claimCriteria.Id);
                    }



                    transaction.Commit();
                }
                response = "Success";

                // add to cache
                cacheDto.Id = contractId;
                cacheDto.data = new ContractAddRequestV2()
                {
                    contract = new ContractV2()
                    {
                        claimLimitation = Math.Round(contract.ClaimLimitation * contract.ConversionRate * 100) / 100,
                        liabilityLimitation = Math.Round(contract.LiabilityLimitation * contract.ConversionRate * 100) / 100,
                        countryId = contract.CountryId,
                        dealerId = contract.DealerId,
                        commodityTypeId = contract.CommodityTypeId,
                        commodityCategoryId = contract.CommodityCategoryId,
                        productId = contract.ProductId,
                        linkDealId = contract.LinkDealId,
                        dealType = contract.DealType,
                        insurerId = contract.InsurerId,
                        reinsurerContractId = contract.ReinsurerContractId,
                        commodityUsageTypeId = contract.CommodityUsageTypeId,
                        contractStartDate = contract.StartDate,
                        contractEndDate = contract.EndDate,
                        isAutoRenewal = contract.IsAutoRenewal,
                        isActive = contract.IsActive,
                        dealName = contract.DealName,
                        remark = contract.Remark,
                        isPromotional = contract.IsPromotional,
                        discountAvailable = contract.DiscountAvailable,
                        labourChargeApplicableOnPolicySold = contract.labourChargeApplicableOnPolicySold,
                        commissions = GetCommissionList(commissionContractMappingsList, contract.ConversionRate),
                        eligibilities = GetEligibilityList(eligibilityList, contract.ConversionRate),
                        texes = GetTaxList(contractTaxList, contract.ConversionRate),
                        isPremiumVisibleToDealer = contract.IsPremiumVisibleToDealer,
                        AnnualInterestRate = contract.AnnualInterestRate
                    },
                    products = GetProductsListDetails(contract.ProductId, contractInsuaranceLimitations, contractontractExtensionsList, contractExtensionMakeList, contractExtensionModelList,
                contractExtensionVariantList, contractExtensionCcList, contractExtensionEcList, contractExtensionGvwList, contractExtensionPremiumList,
                contractExtensionAddonList)
                };

                ICache cache = CacheFactory.GetCache();
                string uniqueCacheKey = UniqueDbName +"_ContractId_" + contractId.ToString().ToLower();
                cache[uniqueCacheKey] = cacheDto;

            }
            catch (Exception ex)
            {
                response = ex.Message;
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static bool GetIsPercentageOrNotByAddonId(Guid premiumAddonId)
        {
            bool response = false;
            try
            {

                ISession session = EntitySessionManager.GetSession();
                PremiumBasedOn premiumBasedOn = session.Query<PremiumBasedOn>().Where(a => a.Id == premiumAddonId).FirstOrDefault();
                if (premiumBasedOn != null)
                {
                    if (premiumBasedOn.Code.ToLower().StartsWith("rp"))
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public bool GetIsPercentageOrNotByAddonIdPub(Guid premiumAddonId)
        {
            bool response = false;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                PremiumBasedOn premiumBasedOn = session.Query<PremiumBasedOn>().Where(a => a.Id == premiumAddonId).FirstOrDefault();
                if (premiumBasedOn != null)
                {
                    if (premiumBasedOn.Code.ToLower().StartsWith("rp"))
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static string UpdateContractV2(ContractUpdateRequestV2Dto updateContractRequestObject,string UniqueDbName)
        {
            string response = String.Empty;
            try
            {
                //prerequisits
                var currencyEntityManager = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();
                #region "validation"
                if (updateContractRequestObject.data.contract == null
                    || updateContractRequestObject.data.products == null
                    || !IsGuid(updateContractRequestObject.contractId.ToString()))
                {
                    response = "Input data is invalid.";
                    return response;
                }
                //currency validationContractInsuaranceLimitation

                Guid currencyPeriodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currencyPeriodId.ToString()))
                {
                    response = "No currency period is defined for today.";
                    return response;
                }
                Dealer dealer =
                    session.Query<Dealer>().FirstOrDefault(a => a.Id == updateContractRequestObject.data.contract.dealerId);
                if (dealer == null)
                {
                    response = "Invalid dealer selection.";
                    return response;
                }
                Guid dealerCurrency = dealer.CurrencyId;
                if (!IsGuid(dealerCurrency.ToString()))
                {
                    response = "Selected dealer's currency not assigned.";
                    return response;
                }
                CurrencyConversions conversions = session.Query<CurrencyConversions>()
                    .FirstOrDefault(
                        a => a.CurrencyConversionPeriodId == currencyPeriodId && a.CurrencyId == dealerCurrency);
                if (conversions == null)
                {
                    response = "Selected dealer's currency not present in current conversion period.";
                    return response;
                }
                #endregion

                Contract contractToUpdate = new Contract();
                bool isContractAvailable = session.Query<Policy>()
                       .Any(a => a.ContractId == updateContractRequestObject.contractId);
                decimal converionRate = currencyEntityManager.GetConversionRate(dealerCurrency, currencyPeriodId, false);
                if (!isContractAvailable)
                {
                    //update contract
                    contractToUpdate = new Contract()
                    {
                        Id = updateContractRequestObject.contractId,
                        CommodityTypeId = updateContractRequestObject.data.contract.commodityTypeId,
                        CountryId = updateContractRequestObject.data.contract.countryId,
                        ClaimLimitation = currencyEntityManager.ConvertToBaseCurrency(
                                updateContractRequestObject.data.contract.claimLimitation, dealerCurrency, currencyPeriodId),
                        CommodityCategoryId = updateContractRequestObject.data.contract.commodityCategoryId,
                        CommodityUsageTypeId = updateContractRequestObject.data.contract.commodityUsageTypeId,
                        DealName = updateContractRequestObject.data.contract.dealName,
                        DealType = updateContractRequestObject.data.contract.dealType,
                        DealerId = updateContractRequestObject.data.contract.dealerId,
                        DiscountAvailable = updateContractRequestObject.data.contract.discountAvailable,
                        EndDate = updateContractRequestObject.data.contract.contractEndDate.ToUniversalTime(),
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = updateContractRequestObject.data.contract.loggedInUserId,
                        InsurerId = updateContractRequestObject.data.contract.insurerId,
                        IsActive = updateContractRequestObject.data.contract.isActive,
                        IsAutoRenewal = updateContractRequestObject.data.contract.isAutoRenewal,
                        IsPromotional = updateContractRequestObject.data.contract.isPromotional,
                        LiabilityLimitation = currencyEntityManager.ConvertToBaseCurrency(
                                updateContractRequestObject.data.contract.liabilityLimitation, dealerCurrency, currencyPeriodId),
                        LinkDealId = updateContractRequestObject.data.contract.linkDealId,
                        ProductId = updateContractRequestObject.data.contract.productId,
                        Remark = updateContractRequestObject.data.contract.remark,
                        StartDate = updateContractRequestObject.data.contract.contractStartDate.ToUniversalTime(),
                        labourChargeApplicableOnPolicySold = updateContractRequestObject.data.contract.labourChargeApplicableOnPolicySold,
                        ReinsurerContractId = updateContractRequestObject.data.contract.reinsurerContractId,
                        ConversionRate = converionRate,
                        CurrencyId = dealerCurrency,
                        CurrencyPeriodId = currencyPeriodId,
                        IsPremiumVisibleToDealer = updateContractRequestObject.data.contract.isPremiumVisibleToDealer,
                        AnnualInterestRate = updateContractRequestObject.data.contract.AnnualInterestRate
                    };
                }
                else
                {
                    // not changing master deal data if contract present in policy
                    contractToUpdate = session.Query<Contract>().FirstOrDefault(a => a.Id == updateContractRequestObject.contractId);
                }

                //commissions
                var existingContractCommissions = session.Query<NRPCommissionContractMapping>()
                 .Where(a => a.ContractId == contractToUpdate.Id).ToList();

                List<NRPCommissionContractMapping> commissionContractMappingsList = updateContractRequestObject.data.contract.commissions
                   .Select(commission => new NRPCommissionContractMapping()
                   {
                       Id = Guid.NewGuid(),
                       ContractId = contractToUpdate.Id,
                       IsOnGROSS = commission.IsOnGROSS,
                       IsOnNRP = commission.IsOnNRP,
                       IsPercentage = commission.IsPercentage,
                       NRPCommissionId = commission.Id,
                       Commission = commission.IsPercentage ? commission.Commission : currencyEntityManager.ConvertToBaseCurrency(commission.Commission, dealerCurrency, currencyPeriodId)
                   }).ToList();

                //taxes
                var existingTaxes = session.Query<ContractTaxMapping>()
                .Where(a => a.ContractId == contractToUpdate.Id).ToList();

                List<ContractTaxMapping> contractTaxList = updateContractRequestObject.data.contract.texes
                    .Where(a => a.IsSelected)
                    .Select(tax => new ContractTaxMapping()
                    {
                        Id = Guid.NewGuid(),
                        ContractId = contractToUpdate.Id,
                        CountryTaxId = tax.Id,

                    }).ToList();

                //eligibility
                var existingEligibilities = session.Query<Eligibility>()
              .Where(a => a.ContractId == contractToUpdate.Id).ToList();

                List<Eligibility> eligibilityList = updateContractRequestObject.data.contract.eligibilities
                    .Select(eligibility => new Eligibility()
                    {
                        ContractId = contractToUpdate.Id,
                        Id = Guid.NewGuid(),
                        EntryDateTime = DateTime.UtcNow,
                        EntryUser = updateContractRequestObject.data.contract.loggedInUserId,
                        IsPercentage = eligibility.isPresentage,
                        AgeFrom = eligibility.ageMin,
                        AgeTo = eligibility.ageMax,
                        MileageFrom = eligibility.milageMin,
                        MileageTo = eligibility.milageMax,
                        MonthsFrom = eligibility.milageMin,
                        MonthsTo = eligibility.monthsMax,
                        PlusMinus = eligibility.plusMinus,
                        Premium = eligibility.isPresentage
                            ? eligibility.premium
                            : currencyEntityManager.ConvertToBaseCurrency(
                                eligibility.premium, dealerCurrency, currencyPeriodId),
                        isMandatory = eligibility.isMandatory
                    }).ToList();


                var exeistingClaimCriterias = session.Query<ClaimCriteria>()
              .Where(a => a.ContractId == contractToUpdate.Id).ToList();

                List<ClaimCriteria> ClaimCriteriasList = new List<ClaimCriteria>();
                if (updateContractRequestObject.data.contract.claimCriteria.Count > 0) {
                     ClaimCriteriasList= updateContractRequestObject.data.contract.claimCriteria
                    .Select(eligibility => new ClaimCriteria()
                    {
                        ContractId = contractToUpdate.Id,
                        Id = Guid.NewGuid(),
                        Presentage = eligibility.presentage,
                        Year = eligibility.year,
                        Label = eligibility.label

                    }).ToList();
                }

                foreach (var product in updateContractRequestObject.data.products)
                {
                    bool isInsLimitationAvailable, isAttributeSpecAvailable, isPremiumAvailable;
                    CheckUpdateValidityByContractId(updateContractRequestObject, product, out isContractAvailable,
                        out isInsLimitationAvailable, out isAttributeSpecAvailable, out isPremiumAvailable);

                    List<ContractExtensionMake> contractExtensionMakeList = new List<ContractExtensionMake>();
                    List<ContractExtensionModel> contractExtensionModelList = new List<ContractExtensionModel>();
                    List<ContractExtensionCylinderCount> contractExtensionCcList = new List<ContractExtensionCylinderCount>();
                    List<ContractExtensionEngineCapacity> contractExtensionEcList = new List<ContractExtensionEngineCapacity>();
                    List<ContractExtensionVariant> contractExtensionVariantList = new List<ContractExtensionVariant>();
                    List<ContractExtensionGVW> contractExtensionGvwList = new List<ContractExtensionGVW>();
                    List<ContractExtensionsPremiumAddon> contractExtensionAddonList = new List<ContractExtensionsPremiumAddon>();
                    List<RSAAnualPremium> contractAnualPremiums = new List<RSAAnualPremium>();


                    ContractInsuaranceLimitation contractInsuaranceLimitationToUpdate = new ContractInsuaranceLimitation();
                    Guid newContractInsurerLimitationId = contractInsuaranceLimitationToUpdate.Id = Guid.NewGuid();
                    if (!isInsLimitationAvailable)
                    {
                        //check if selected insuarnce limitation is available
                        ContractInsuaranceLimitation contractInsuaranceLimitation = session
                            .Query<ContractInsuaranceLimitation>()
                            .FirstOrDefault(a => a.ContractId == updateContractRequestObject.contractId &&
                                                 a.InsuaranceLimitationId == product.InsuaranceLimitationId);
                        if (contractInsuaranceLimitation != null)
                        {
                            //do nothing coz same insurance limitation available in db
                            contractInsuaranceLimitationToUpdate = contractInsuaranceLimitation;
                        }
                        else
                        {
                            //new insurance limitation
                            contractInsuaranceLimitationToUpdate = new ContractInsuaranceLimitation()
                            {
                                Id = newContractInsurerLimitationId,
                                ContractId = updateContractRequestObject.contractId,
                                EntryDateTime = DateTime.UtcNow,
                                BaseProductId = product.Id,
                                InsuaranceLimitationId = product.InsuaranceLimitationId
                            };

                        }
                    }

                    ContractExtensions contractExtensionsUpdate = new ContractExtensions();
                    Guid newContractExtensionId = contractExtensionsUpdate.Id = Guid.NewGuid();
                    if (!isAttributeSpecAvailable)
                    {
                        //check for db values
                        ContractExtensions contractExtensions = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.ContractInsuanceLimitationId == contractInsuaranceLimitationToUpdate.Id
                                             && a.AttributeSpecificationId == product.attributeSpecificationId);
                        if (contractExtensions != null)
                        {
                            contractExtensionsUpdate = contractExtensions;
                            contractExtensionsUpdate.AttributeSpecification = product.attributeSpecificationName;
                        }
                        else
                        {
                            contractExtensionsUpdate = new ContractExtensions()
                            {
                                Id = newContractExtensionId,
                                EntryDateTime = DateTime.UtcNow,
                                EntryUser = updateContractRequestObject.data.contract.loggedInUserId,
                                Rate = product.annualPremiumTotal,
                                IsRSA = product.Productcode.ToUpper() == "RSA",
                                RegionId = product.regionId,
                                RSAProviderId = product.rsaProviderId,
                                ContractInsuanceLimitationId = contractInsuaranceLimitationToUpdate.Id,
                                AttributeSpecification = product.attributeSpecificationName,
                                ProductId = product.Id,
                                AttributeSpecificationId = Guid.NewGuid(),
                                IsAllCyllinderCountsSelected = product.isAllCyllinderCountSelected,
                                IsAllEngineCapacitiesSelected = product.isAllEngineCapacitySelected,
                                IsAllGVWSelected = product.isAllGvwSelected,
                                IsAllMakesSelected = product.isAllMakesSelected,
                                IsAllModelsSelected = product.isAllModelsSelected,
                                IsAllVariantsSelected = product.isAllVariantSelected

                            };
                        }

                        //make details
                        contractExtensionMakeList.AddRange(product.selectedMakeList.Select(make => new ContractExtensionMake()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            MakeId = make.id
                        }));
                        //model details

                        contractExtensionModelList.AddRange(product.selectedModelsList.Select(model => new ContractExtensionModel()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            ModelId = model.id
                        }));
                        // cc

                        contractExtensionCcList.AddRange(product.selectedClinderCounts.Select(cc => new ContractExtensionCylinderCount()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            CylinderCountId = cc.id
                        }));
                        //ec

                        contractExtensionEcList.AddRange(product.selectedEngineCapacities.Select(ec => new ContractExtensionEngineCapacity()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            EngineCapacityId = ec.id
                        }));
                        //variant

                        contractExtensionVariantList.AddRange(product.selectedVariantList.Select(variant => new ContractExtensionVariant()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            VariantId = variant.id
                        }));
                        //GVW

                        contractExtensionGvwList.AddRange(product.selectedGrossWeights.Select(gvw => new ContractExtensionGVW()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            GVWId = gvw.id
                        }));


                        //rsa
                        if (product.Productcode.ToLower() == "rsa")
                        {
                            contractAnualPremiums.AddRange(product.annualPremium.Select(annualPremium => new RSAAnualPremium()
                            {
                                Id = Guid.NewGuid(),
                                ContractExtensionId = contractExtensionsUpdate.Id,
                                Year = annualPremium.Year,
                                Value = currencyEntityManager.ConvertToBaseCurrency(annualPremium.Value, dealerCurrency, currencyPeriodId)
                            }));
                        }

                    }

                    ContractExtensionPremium contractExtensionPremium = new ContractExtensionPremium();
                    Guid newContractPremiumId = contractExtensionPremium.Id = Guid.NewGuid();
                    if (!isPremiumAvailable)
                    {
                        ContractExtensionPremium contractExtensionPremiumData = session.Query<ContractExtensionPremium>()
                            .FirstOrDefault(
                                a =>
                                    a.ContractExtensionId == contractExtensionsUpdate.Id &&
                                    a.ItemStatusId == product.itemStatusId
                                    && a.WarrentyTypeId == product.warrantyTypeId);
                        if (contractExtensionPremiumData != null)
                        {
                            CommonEntityManager cem = new CommonEntityManager();
                            contractExtensionPremium = contractExtensionPremiumData;
                            contractExtensionPremium.Gross = cem.GetPremiumBasedonCodeById(product.premiumBasedOnIdGross).ToLower() == "rp" ? product.grossPremium :
                                currencyEntityManager.ConvertToBaseCurrency(product.grossPremium, dealerCurrency, currencyPeriodId);
                            contractExtensionPremium.NRP = cem.GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? product.totalNRP :
                               currencyEntityManager.ConvertToBaseCurrency(product.totalNRP, dealerCurrency, currencyPeriodId);
                            contractExtensionPremium.WarrentyTypeId = product.warrantyTypeId;
                        }
                        else
                        {
                            CommonEntityManager cem = new CommonEntityManager();
                            contractExtensionPremium = new ContractExtensionPremium()
                            {
                                Id = newContractPremiumId,
                                ContractExtensionId = contractExtensionsUpdate.Id,
                                CurrencyId = dealerCurrency,
                                ConversionRate = converionRate,
                                CurrencyPeriodId = currencyPeriodId,
                                Gross = cem.GetPremiumBasedonCodeById(product.premiumBasedOnIdGross).ToLower() == "rp" ? product.grossPremium :
                                currencyEntityManager.ConvertToBaseCurrency(product.grossPremium, dealerCurrency, currencyPeriodId),
                                IsClaimLabourChargesFixed = updateContractRequestObject.data.contract.labourChargeApplicableOnPolicySold,
                                IsCustomerAvailableGross = product.isCustAvailableGross,
                                IsCustomerAvailableNett = product.isCustAvailableNett,
                                ItemStatusId = product.itemStatusId,
                                MaxValueGross = currencyEntityManager.ConvertToBaseCurrency(product.maxValueGross, dealerCurrency, currencyPeriodId),
                                MaxValueNett = currencyEntityManager.ConvertToBaseCurrency(product.maxValueNett, dealerCurrency, currencyPeriodId),
                                MinValueGross = currencyEntityManager.ConvertToBaseCurrency(product.minValueGross, dealerCurrency, currencyPeriodId),
                                MinValueNett = currencyEntityManager.ConvertToBaseCurrency(product.minValueNett, dealerCurrency, currencyPeriodId),
                                NRP = cem.GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? product.totalNRP :
                                currencyEntityManager.ConvertToBaseCurrency(product.totalNRP, dealerCurrency, currencyPeriodId),
                                PremiumBasedOnGross = product.premiumBasedOnIdGross,
                                PremiumBasedOnNett = product.premiumBasedOnIdNett,
                                WarrentyTypeId = product.warrantyTypeId
                            };
                        }

                        contractExtensionAddonList.AddRange(product.premiumAddonsGross.Select(addon => new ContractExtensionsPremiumAddon()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            PremiumAddonTypeId = addon.Id,
                            PremiumType = "GROSS",
                            Value = new CommonEntityManager()
                        .GetPremiumBasedonCodeById(product.premiumBasedOnIdGross).ToLower() == "rp" ? addon.Value :
                            addon.Value / converionRate,
                            ContractExtensionPremiumId = contractExtensionPremium.Id

                        }));
                        //nett
                        contractExtensionAddonList.AddRange(product.premiumAddonsNett.Select(addon => new ContractExtensionsPremiumAddon()
                        {
                            Id = Guid.NewGuid(),
                            ContractExtensionId = contractExtensionsUpdate.Id,
                            PremiumAddonTypeId = addon.Id,
                            PremiumType = "NETT",
                            Value = new CommonEntityManager()
                            .GetPremiumBasedonCodeById(product.premiumBasedOnIdNett).ToLower() == "rp" ? addon.value :
                            addon.value / converionRate,
                            ContractExtensionPremiumId = contractExtensionPremium.Id,

                        }));
                    }

                    var contractExtensionMakeListDelete = session.Query<ContractExtensionMake>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionModelListDelete = session.Query<ContractExtensionModel>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionCcDelete = session.Query<ContractExtensionCylinderCount>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionEcListDelete = session.Query<ContractExtensionEngineCapacity>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionVariantListDelete = session.Query<ContractExtensionVariant>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionGvwListDelete = session.Query<ContractExtensionGVW>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();
                    var contractExtensionAddonListDelete = session.Query<ContractExtensionsPremiumAddon>()
                        .Where(a => a.ContractExtensionPremiumId == contractExtensionPremium.Id).ToList();
                    var contractAnualPremiumsDelete = session.Query<RSAAnualPremium>()
                        .Where(a => a.ContractExtensionId == contractExtensionsUpdate.Id).ToList();

                    //db updates
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        if (!isContractAvailable)
                        {
                            //master details can only be chnged if deal dosent associated with any policy

                            session.SaveOrUpdate(contractToUpdate);

                            //commissions
                            foreach (var existingCommissions in existingContractCommissions)
                            {
                                session.Evict(existingCommissions);
                                session.Delete(existingCommissions);
                            }

                            foreach (var newCommissions in commissionContractMappingsList)
                            {
                                session.Evict(newCommissions);
                                session.Save(newCommissions, newCommissions.Id);
                            }
                            existingContractCommissions = new List<NRPCommissionContractMapping>();
                            commissionContractMappingsList = new List<NRPCommissionContractMapping>();
                            //tex
                            foreach (var existingTax in existingTaxes)
                            {
                                session.Evict(existingTax);
                                session.Delete(existingTax);
                            }
                            foreach (var newTax in contractTaxList)
                            {
                                session.Evict(newTax);
                                session.Save(newTax, newTax.Id);
                            }
                            existingTaxes = new List<ContractTaxMapping>();
                            contractTaxList = new List<ContractTaxMapping>();
                            //eligibility
                            foreach (var existingEligibility in existingEligibilities)
                            {
                                session.Evict(existingEligibility);
                                session.Delete(existingEligibility);
                            }
                            foreach (var newEligibility in eligibilityList)
                            {
                                session.Evict(newEligibility);
                                session.Save(newEligibility, newEligibility.Id);
                            }
                            existingEligibilities = new List<Eligibility>();
                            eligibilityList = new List<Eligibility>();


                            // Claim Criteria
                            foreach (var exeistingClaimCriteria in exeistingClaimCriterias)
                            {
                                session.Evict(exeistingClaimCriteria);
                                session.Delete(exeistingClaimCriteria);
                            }

                            foreach (var claimCriteria in ClaimCriteriasList)
                            {
                                session.Evict(claimCriteria);
                                session.Save(claimCriteria, claimCriteria.Id);
                            }
                            ClaimCriteriasList = new List<ClaimCriteria>();
                            exeistingClaimCriterias = new List<ClaimCriteria>();

                        }

                        if (!isInsLimitationAvailable)
                            session.SaveOrUpdate(contractInsuaranceLimitationToUpdate);
                        if (!isAttributeSpecAvailable)
                        {
                            if (contractExtensionsUpdate.Id == newContractExtensionId)
                                session.Save(contractExtensionsUpdate, contractExtensionsUpdate.Id);
                            else
                                session.Update(contractExtensionsUpdate, contractExtensionsUpdate.Id);

                            //ext make
                            foreach (var extensionMake in contractExtensionMakeListDelete)
                            {
                                session.Evict(extensionMake);
                                session.Delete(extensionMake);
                            }
                            foreach (var extensionMake in contractExtensionMakeList)
                            {
                                session.Evict(extensionMake);
                                session.Save(extensionMake, extensionMake.Id);
                            }

                            //ext model

                            foreach (var extensionMake in contractExtensionModelListDelete)
                            {
                                session.Evict(extensionMake);
                                session.Delete(extensionMake);
                            }
                            foreach (var extensionModel in contractExtensionModelList)
                            {
                                session.Evict(extensionModel);
                                session.Save(extensionModel, extensionModel.Id);
                            }
                            //ext cc
                            foreach (var extensionCc in contractExtensionCcDelete)
                            {
                                session.Evict(extensionCc);
                                session.Delete(extensionCc);
                            }
                            foreach (var extensioncc in contractExtensionCcList)
                            {
                                session.Evict(extensioncc);
                                session.Save(extensioncc, extensioncc.Id);
                            }
                            //ec
                            foreach (var extensionEc in contractExtensionEcListDelete)
                            {
                                session.Evict(extensionEc);
                                session.Delete(extensionEc);
                            }
                            foreach (var extensionec in contractExtensionEcList)
                            {
                                session.Evict(extensionec);
                                session.Save(extensionec, extensionec.Id);
                            }
                            //variant
                            foreach (var extensionVariant in contractExtensionVariantListDelete)
                            {
                                session.Evict(extensionVariant);
                                session.Delete(extensionVariant);
                            }
                            foreach (var extensionVariant in contractExtensionVariantList)
                            {
                                session.Evict(extensionVariant);
                                session.Save(extensionVariant, extensionVariant.Id);
                            }
                            //gvw
                            foreach (var extensionGw in contractExtensionGvwListDelete)
                            {
                                session.Evict(extensionGw);
                                session.Delete(extensionGw);
                            }
                            foreach (var extensionGvw in contractExtensionGvwList)
                            {
                                session.Evict(extensionGvw);
                                session.Save(extensionGvw, extensionGvw.Id);
                            }

                        }

                        if (!isPremiumAvailable)
                        {
                            if (newContractPremiumId == contractExtensionPremium.Id)
                                session.Save(contractExtensionPremium, contractExtensionPremium.Id);
                            else
                                session.Update(contractExtensionPremium, contractExtensionPremium.Id);
                            //addons
                            foreach (var extensionAddon in contractExtensionAddonListDelete)
                            {
                                session.Evict(extensionAddon);
                                session.Delete(extensionAddon);
                            }
                            foreach (var addon in contractExtensionAddonList)
                            {
                                session.Evict(addon);
                                session.Save(addon, addon.Id);
                            }
                            //rsa
                            if (product.Productcode.ToLower() == "rsa")
                            {
                                foreach (var extensionPremium in contractAnualPremiumsDelete)
                                {
                                    session.Evict(extensionPremium);
                                    session.Delete(extensionPremium);
                                }
                                foreach (var extensionAnnualPremium in contractAnualPremiums)
                                {
                                    session.Evict(extensionAnnualPremium);
                                    session.Save(extensionAnnualPremium, extensionAnnualPremium.Id);
                                }
                            }
                        }

                        transaction.Commit();
                        response = "ok";

                        // update cache data
                        List<ContractInsuaranceLimitation> contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                        .Where(a => a.ContractId == contractToUpdate.Id).ToList();

                        var limitationIds = contractInsuaranceLimitation.Select(a => a.Id).ToArray();
                        List<ContractExtensions> contractExtensions = session.Query<ContractExtensions>()
                            .Where(a => limitationIds.Contains(a.ContractInsuanceLimitationId))
                            .ToList();

                        List<ContractExtensionPremium> contractExtensionPremiums = session.Query<ContractExtensionPremium>()
                        .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();

                        GetContractDetailsByContractIdDto cacheDto = new GetContractDetailsByContractIdDto();
                        cacheDto.Id = contractToUpdate.Id;
                        cacheDto.data = new ContractAddRequestV2()
                        {
                            contract = new ContractV2()
                            {
                                claimLimitation = Math.Round(contractToUpdate.ClaimLimitation * contractToUpdate.ConversionRate * 100) / 100,
                                liabilityLimitation = Math.Round(contractToUpdate.LiabilityLimitation * contractToUpdate.ConversionRate * 100) / 100,
                                countryId = contractToUpdate.CountryId,
                                dealerId = contractToUpdate.DealerId,
                                commodityTypeId = contractToUpdate.CommodityTypeId,
                                commodityCategoryId = contractToUpdate.CommodityCategoryId,
                                productId = contractToUpdate.ProductId,
                                linkDealId = contractToUpdate.LinkDealId,
                                dealType = contractToUpdate.DealType,
                                insurerId = contractToUpdate.InsurerId,
                                reinsurerContractId = contractToUpdate.ReinsurerContractId,
                                commodityUsageTypeId = contractToUpdate.CommodityUsageTypeId,
                                contractStartDate = contractToUpdate.StartDate,
                                contractEndDate = contractToUpdate.EndDate,
                                isAutoRenewal = contractToUpdate.IsAutoRenewal,
                                isActive = contractToUpdate.IsActive,
                                dealName = contractToUpdate.DealName,
                                remark = contractToUpdate.Remark,
                                isPromotional = contractToUpdate.IsPromotional,
                                discountAvailable = contractToUpdate.DiscountAvailable,
                                labourChargeApplicableOnPolicySold = contractToUpdate.labourChargeApplicableOnPolicySold,
                                commissions = GetCommissionList(commissionContractMappingsList, contractToUpdate.ConversionRate),
                                eligibilities = GetEligibilityList(eligibilityList, contractToUpdate.ConversionRate),
                                texes = GetTaxList(contractTaxList, contractToUpdate.ConversionRate),
                                isPremiumVisibleToDealer = contractToUpdate.IsPremiumVisibleToDealer,
                                AnnualInterestRate = contractToUpdate.AnnualInterestRate
                            },
                            products = GetProductsListDetails(contractToUpdate.ProductId, contractInsuaranceLimitation, contractExtensions, contractExtensionMakeList, contractExtensionModelList,
                        contractExtensionVariantList, contractExtensionCcList, contractExtensionEcList, contractExtensionGvwList, contractExtensionPremiums,
                        contractExtensionAddonList)
                        };

                        ICache cache = CacheFactory.GetCache();
                        string uniqueCacheKey = UniqueDbName + "_ContractId_" + contractToUpdate.Id.ToString().ToLower();
                        cache.Remove(UniqueDbName);
                        cache[uniqueCacheKey] = cacheDto;

                    }
                }
            }
            catch (Exception ex)
            {

                response = "Error occured while saving contract.Please retry.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        private static void CheckUpdateValidityByContractId(ContractUpdateRequestV2Dto updateContractRequestObject, ProductV2 product,
            out bool isContractAvailable, out bool isInsLimitationAvailable, out bool isAttributeSpecAvailable,
            out bool isPremiumAvailable)
        {
            isContractAvailable = false;
            isInsLimitationAvailable = false;
            isAttributeSpecAvailable = false;
            isPremiumAvailable = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                isContractAvailable = session.Query<Policy>()
                        .Any(a => a.ContractId == updateContractRequestObject.contractId);

                ContractInsuaranceLimitation contractInsuaranceLimitation =
                    session.Query<ContractInsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == product.InsuaranceLimitationId);

                if (contractInsuaranceLimitation != null)
                {
                    isInsLimitationAvailable = session.Query<Policy>()
                        .Any(a => a.ContractInsuaranceLimitationId == contractInsuaranceLimitation.Id);
                }

                if (isInsLimitationAvailable)
                {
                    ContractExtensions contractExtensions = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.ContractInsuanceLimitationId == product.InsuaranceLimitationId
                                             && a.AttributeSpecificationId == product.attributeSpecificationId);
                    if (contractExtensions != null)
                    {
                        isAttributeSpecAvailable = session.Query<Policy>()
                            .Any(a => a.ContractExtensionsId == contractExtensions.Id);
                    }
                }

                if (isAttributeSpecAvailable)
                {
                    ContractExtensions contractExtensions = session.Query<ContractExtensions>()
                       .FirstOrDefault(a => a.ContractInsuanceLimitationId == product.InsuaranceLimitationId
                                            && a.AttributeSpecificationId == product.attributeSpecificationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == contractExtensions.Id
                                             && a.WarrentyTypeId == product.warrantyTypeId &&
                                             a.ItemStatusId == product.itemStatusId);

                    if (contractExtensionPremium != null)
                    {
                        isPremiumAvailable = session.Query<Policy>()
                      .Any(a => a.ContractExtensionPremiumId == contractExtensionPremium.Id);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
        }

        internal static bool ContractsUpdateValidityCheck(Guid ContractId)
        {
            bool _Response = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                int rowCount = session.Query<Policy>().Where(a => a.ContractId == ContractId).Count();
                if (rowCount == 0)
                    _Response = true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;

        }

        internal static bool UpdateContractStatus(Guid ContractId, bool Status)
        {
            bool _Response = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Contract Contract = session.Query<Contract>().FirstOrDefault(a => a.Id == ContractId);
                Contract.IsActive = Status;
                session.Update(Contract);
                session.Flush();
                _Response = true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal Guid GetCurrencyPeriodByContractId(Guid contractId)
        {
            Guid _Response = Guid.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //ContractExtensions Contract = session.Query<ContractExtensions>().Where(a => a. == contractId).FirstOrDefault();
                //ContractExtensionPremium ContractExtPre = session.Query<ContractExtensionPremium>().Where(a => a.ContractExtensionId == contractId).FirstOrDefault();
                //if (Contract != null)
                //{
                //    _Response = Contract.;
                //}
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal static object GetContractTaxesByExtensionId(Guid ContractId, decimal AmountForTaxCalc)
        {
            object _Response = 0.00;
            try
            {
                decimal taxAmt = decimal.Zero;
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<ContractTaxMapping> contractTaxes = session.Query<ContractTaxMapping>()
                    .Where(a => a.ContractId == ContractId);
                IEnumerable<CountryTaxes> countryTaxes = session.Query<CountryTaxes>()
                    .Where(a => contractTaxes.Any(b => b.CountryTaxId == a.Id)).OrderBy(a => a.IndexVal);

                if (countryTaxes != null && countryTaxes.Count() > 0)
                {
                    foreach (CountryTaxes tax in countryTaxes)
                    {
                        if (tax.IsPercentage)
                        {
                            if (tax.IsOnPreviousTax)
                            {

                                taxAmt += (AmountForTaxCalc + taxAmt) * tax.TaxValue / 100;
                            }
                            else
                            {
                                taxAmt += AmountForTaxCalc * tax.TaxValue / 100;
                            }
                        }
                        else
                        {
                            taxAmt += (tax.TaxValue * tax.ConversionRate);
                        }
                    }
                }
                _Response = taxAmt;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return _Response;
        }

        internal string GetPremimBasedOnCodeByContractId(Guid contractId, string type)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ContractExtensions contractExtension = session.Query<ContractExtensions>()
                    .Where(a => a.Id == contractId).FirstOrDefault();
                if (contractExtension != null)
                {
                    //Guid basedOnId = type == "GROSS" ? contractExtension.PremiumBasedOnIdGross : contractExtension.PremiumBasedOnIdNett;
                    PremiumBasedOn PremiumBasedOn = session.Query<PremiumBasedOn>()
                        .Where(a => a.Id == Guid.Empty).FirstOrDefault();
                    if (PremiumBasedOn != null)
                    {
                        Response = PremiumBasedOn.Code.ToUpper();
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static ContractExtensions GetFirstContractExtensionByContractId(Guid contractId)
        {
            ContractExtensions Response = new ContractExtensions();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response =
                    session.Query<ContractExtensions>().FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        public static List<InsuaranceLimitResponseDto> GetInsuaranceLimitationsByCommodityType(Guid commodityTypeId, string productType)
        {
            List<InsuaranceLimitResponseDto> response = new List<InsuaranceLimitResponseDto>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                bool isRsa = productType.ToLower() == "rsa" ? true : false;

                response = session.Query<InsuaranceLimitation>()
                    .Where(a => a.IsRsa == isRsa && a.CommodityTypeId == commodityTypeId)
                    .Select(a => new InsuaranceLimitResponseDto
                    {
                        Id= a.Id,
                        InsuaranceLimitationName=a.InsuaranceLimitationName,
                        Months=a.Months
                    }).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object AddNewInsuaranceLimitation(InsuaranceLimitationRequestDto insuaranceLimitationRequest)
        {
            GenericCodeMsgResponse response = new GenericCodeMsgResponse { code = "ok" };
            try
            {
                ISession session = EntitySessionManager.GetSession();
                #region validation

                if (insuaranceLimitationRequest != null && IsGuid(insuaranceLimitationRequest.commodityTypeId.ToString()))
                {
                    if (insuaranceLimitationRequest.insuaranceLimitation != null)
                    {
                        if (insuaranceLimitationRequest.isAutomobile || insuaranceLimitationRequest.isYellowGood)
                        {
                            if (insuaranceLimitationRequest.insuaranceLimitation.month == decimal.Zero)
                            {
                                response.code = "error";
                                response.msg = "Month is invalid.";
                                return response;
                            }
                            if (!insuaranceLimitationRequest.isRsa && !insuaranceLimitationRequest.insuaranceLimitation.unlimitedcheck)
                            {
                                if (insuaranceLimitationRequest.insuaranceLimitation.km == decimal.Zero)
                                {
                                    response.code = "error";
                                    response.msg = "Mileage is invalid.";
                                    return response;
                                }
                            }

                        }
                        else if (insuaranceLimitationRequest.isOther)
                        {
                            if (insuaranceLimitationRequest.insuaranceLimitation.hours == decimal.Zero)
                            {
                                response.code = "error";
                                response.msg = "Hours is invalid.";
                                return response;
                            }

                        }
                        else
                        {
                            if (insuaranceLimitationRequest.insuaranceLimitation.month == decimal.Zero)
                            {
                                response.code = "error";
                                response.msg = "Month is invalid.";
                                return response;
                            }

                        }
                    }
                    else
                    {
                        response.code = "error";
                        response.msg = "Request data invalid.";
                    }
                }
                else
                {
                    response.code = "error";
                    response.msg = "Request data invalid.";
                }

                //db validation
                InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                    .FirstOrDefault(a => a.CommodityTypeId == insuaranceLimitationRequest.commodityTypeId
                    && a.IsRsa == insuaranceLimitationRequest.isRsa
                    && a.TopOfMW == !insuaranceLimitationRequest.insuaranceLimitation.upto
                    && a.Km == insuaranceLimitationRequest.insuaranceLimitation.km
                    && a.Months == insuaranceLimitationRequest.insuaranceLimitation.month
                    && a.Hrs == insuaranceLimitationRequest.insuaranceLimitation.hours);
                if (insuaranceLimitation != null)
                {
                    response.code = "error";
                    response.msg = "Enterd insuarance limitation already exist.";
                }

                #endregion

                InsuaranceLimitation newInsuaranceLimitation = new InsuaranceLimitation()
                {
                    CommodityTypeId = insuaranceLimitationRequest.commodityTypeId,
                    TopOfMW = !insuaranceLimitationRequest.insuaranceLimitation.upto,
                    Months = insuaranceLimitationRequest.insuaranceLimitation.month,
                    Km = insuaranceLimitationRequest.insuaranceLimitation.km,
                    IsRsa = insuaranceLimitationRequest.isRsa,
                    Hrs = insuaranceLimitationRequest.insuaranceLimitation.hours,
                    EntryBy = insuaranceLimitationRequest.loggedInUserId,
                    EntryDateTime = DateTime.UtcNow,
                    Id = Guid.NewGuid(),
                    CommodityTypeCode = new CommonEntityManager().GetCommodityTypeNameById(insuaranceLimitationRequest.commodityTypeId),
                    InsuaranceLimitationName = GetInsuaranceLimitationName(insuaranceLimitationRequest.insuaranceLimitation)
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newInsuaranceLimitation, newInsuaranceLimitation.Id);
                    transaction.Commit();
                }
                response.code = "ok";

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.code = "error";
                response.msg = ex.Message;
            }
            return response;
        }

        private static string GetInsuaranceLimitationName(InsuaranceLimitationDto insuaranceLimitation)
        {
            List<string> limitations = new List<string>();
            if (insuaranceLimitation.month > 0)
            {
                limitations.Add(insuaranceLimitation.month + "M");
            }
            if (insuaranceLimitation.hours > 0)
            {
                limitations.Add(insuaranceLimitation.hours + "H");
            }
            if (insuaranceLimitation.km > 0 || insuaranceLimitation.unlimitedcheck)
            {
                if (insuaranceLimitation.unlimitedcheck)
                {
                    limitations.Add("Unlimited");
                }
                else
                {
                    limitations.Add((insuaranceLimitation.upto ? "Upto " : "MW +") + insuaranceLimitation.km + "KM");
                }

            }

            return string.Join(" - ", limitations);
        }

        public static GetContractDetailsByContractIdDto GetContractDetailsByContractId(Guid contractId)
        {

            GetContractDetailsByContractIdDto response = new GetContractDetailsByContractIdDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //getting first details for first view
                Contract contract = session.Query<Contract>().Where(a => a.Id == contractId).FirstOrDefault();
                List<ContractInsuaranceLimitation> contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                    .Where(a => a.ContractId == contract.Id).ToList();
                var limitationIds = contractInsuaranceLimitation.Select(a => a.Id).ToArray();
                List<ContractExtensions> contractExtensions = session.Query<ContractExtensions>()
                    .Where(a => limitationIds.Contains(a.ContractInsuanceLimitationId))
                    .ToList();
                List<ContractExtensionMake> contractExtensionMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionModel> contractExtensionModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionVariant> contractExtensionVariants = session.Query<ContractExtensionVariant>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionCylinderCount> contractExtensionCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionEngineCapacity> contractExtensionEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionGVW> contractExtensionGvws = session.Query<ContractExtensionGVW>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<ContractExtensionPremium> contractExtensionPremiums = session.Query<ContractExtensionPremium>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();
                List<NRPCommissionContractMapping> commissionContractMappings =
                    session.Query<NRPCommissionContractMapping>()
                        .Where(a => a.ContractId == contractId).ToList();
                List<ContractTaxMapping> contractTaxeses = session.Query<ContractTaxMapping>()
                    .Where(a => a.ContractId == contractId).ToList();
                List<Eligibility> eligibilities = session.Query<Eligibility>()
                    .Where(a => a.ContractId == contractId).ToList();
                List<ContractExtensionsPremiumAddon> premiumAddons = session.Query<ContractExtensionsPremiumAddon>()
                    .Where(a => a.ContractExtensionId == contractExtensions.FirstOrDefault().Id).ToList();

                List<ClaimCriteria> claimCriteriaList = session.Query<ClaimCriteria>().Where(a => a.ContractId == contractId).ToList();

                decimal rate = contractExtensionPremiums.FirstOrDefault().ConversionRate;

                response.Id = contractId;
                response.data = new ContractAddRequestV2()
                {
                    contract = new ContractV2()
                    {
                        claimLimitation = Math.Round(contract.ClaimLimitation * rate * 100) / 100,
                        liabilityLimitation = Math.Round(contract.LiabilityLimitation * rate * 100) / 100,
                        countryId = contract.CountryId,
                        dealerId = contract.DealerId,
                        commodityTypeId = contract.CommodityTypeId,
                        commodityCategoryId = contract.CommodityCategoryId,
                        productId = contract.ProductId,
                        linkDealId = contract.LinkDealId,
                        dealType = contract.DealType,
                        insurerId = contract.InsurerId,
                        reinsurerContractId = contract.ReinsurerContractId,
                        commodityUsageTypeId = contract.CommodityUsageTypeId,
                        contractStartDate = contract.StartDate,
                        contractEndDate = contract.EndDate,
                        isAutoRenewal = contract.IsAutoRenewal,
                        isActive = contract.IsActive,
                        dealName = contract.DealName,
                        remark = contract.Remark,
                        isPromotional = contract.IsPromotional,
                        discountAvailable = contract.DiscountAvailable,
                        labourChargeApplicableOnPolicySold = contract.labourChargeApplicableOnPolicySold,
                        commissions = GetCommissionList(commissionContractMappings, rate),
                        eligibilities = GetEligibilityList(eligibilities, rate),
                        texes = GetTaxList(contractTaxeses, rate),
                        isPremiumVisibleToDealer = contract.IsPremiumVisibleToDealer,
                        AnnualInterestRate = contract.AnnualInterestRate,
                        claimCriteria = GetClaimCriterias(claimCriteriaList)
                    },
                    products = GetProductsListDetails(contract.ProductId, contractInsuaranceLimitation, contractExtensions, contractExtensionMakes, contractExtensionModels,
                    contractExtensionVariants, contractExtensionCylinderCounts, contractExtensionEngineCapacities, contractExtensionGvws, contractExtensionPremiums,
                    premiumAddons)
                };

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static List<ClaimCriteriaDto> GetClaimCriterias(List<ClaimCriteria> claimCriteriaList)
        {
            return claimCriteriaList.Select(claimlCri => new ClaimCriteriaDto()
            {
                id= claimlCri.Id,
                label = claimlCri.Label,
                presentage = claimlCri.Presentage,
                year = claimlCri.Year
            }).ToList();
        }

        private static List<ProductV2> GetProductsListDetails(Guid productId, List<ContractInsuaranceLimitation> contractInsuaranceLimitations,
            List<ContractExtensions> contractExtensions, List<ContractExtensionMake> contractExtensionMakes,
            List<ContractExtensionModel> contractExtensionModels, List<ContractExtensionVariant> contractExtensionVariants,
            List<ContractExtensionCylinderCount> contractExtensionCylinderCounts, List<ContractExtensionEngineCapacity> contractExtensionEngineCapacities,
            List<ContractExtensionGVW> contractExtensionGvws, List<ContractExtensionPremium> contractExtensionPremiums, List<ContractExtensionsPremiumAddon> premiumAddons)
        {
            List<ProductV2> response = new List<ProductV2>();
            ISession session = EntitySessionManager.GetSession();
            Product product = session.Query<Product>()
                .Where(a => a.Id == productId).FirstOrDefault();

            //List<BundledProduct> bundledProducts = new List<BundledProduct>();
            if (product.Isbundledproduct)
            {

                List<BundledProduct> bundledProducts = session.Query<BundledProduct>()
                    .Where(a => a.ProductId == product.Id && a.IsCurrentProduct)
                    .ToList();


                foreach (var bundledProduct in bundledProducts)
                {
                    ContractExtensions contractExtension =
                        contractExtensions.FirstOrDefault(
                            a =>
                            {
                                var contractInsuaranceLimitation = contractInsuaranceLimitations
                                    .FirstOrDefault(b => b.BaseProductId == bundledProduct.ProductId);
                                return contractInsuaranceLimitation != null && a.ContractInsuanceLimitationId == contractInsuaranceLimitation.Id;
                            });
                    ContractExtensionPremium contractExtensionPremium = contractExtensionPremiums.Where
                        (a => a.ContractExtensionId == contractExtension.Id).FirstOrDefault();
                    ProductV2 productV2 = new ProductV2()
                    {
                        Id = bundledProduct.Id,
                        InsuaranceLimitationId = contractInsuaranceLimitations.FirstOrDefault(a => a.BaseProductId == bundledProduct.Id).InsuaranceLimitationId,
                        attributeSpecificationId = contractExtension.AttributeSpecificationId,
                        attributeSpecificationName = contractExtension.AttributeSpecification,
                        selectedMakeList = GetSelectedMakeList(contractExtension.Id, contractExtensionMakes),
                        selectedModelsList = GetSelectedModelList(contractExtension.Id, contractExtensionModels),
                        selectedVariantList = GetSelectedVariantList(contractExtension.Id, contractExtensionVariants),
                        selectedEngineCapacities = GetSelectedEngineCapacities(contractExtension.Id, contractExtensionEngineCapacities),
                        selectedClinderCounts = GetSelectedCyllinderCounts(contractExtension.Id, contractExtensionCylinderCounts),
                        selectedGrossWeights = GetSelectedGrossWeights(contractExtension.Id, contractExtensionGvws),

                        warrantyTypeId = contractExtensionPremium.WarrentyTypeId,
                        itemStatusId = contractExtensionPremium.ItemStatusId,

                        premiumBasedOnIdNett = contractExtensionPremium.PremiumBasedOnNett,
                        minValueNett = contractExtensionPremium.MinValueNett * contractExtensionPremium.ConversionRate,
                        maxValueNett = contractExtensionPremium.MaxValueNett * contractExtensionPremium.ConversionRate,
                        isCustAvailableNett = contractExtensionPremium.IsCustomerAvailableNett,
                        premiumAddonsNett = GetPremiumAddons(premiumAddons, contractExtensionPremium.Id,
                            "NETT", contractExtensionPremium),

                        premiumBasedOnIdGross = contractExtensionPremium.PremiumBasedOnGross,
                        minValueGross = contractExtensionPremium.MinValueGross * contractExtensionPremium.ConversionRate,
                        maxValueGross = contractExtensionPremium.MaxValueGross * contractExtensionPremium.ConversionRate,
                        isCustAvailableGross = contractExtensionPremium.IsCustomerAvailableGross,
                        premiumAddonsGross = GetPremiumAddonsGross(premiumAddons, contractExtensionPremium.Id,
                            "GROSS", contractExtensionPremium),

                        totalNRP = new CommonEntityManager().GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnNett).ToLower() == "rp" ?
                        contractExtensionPremium.NRP : contractExtensionPremium.NRP * contractExtensionPremium.ConversionRate,

                        grossPremium = new CommonEntityManager().GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnGross).ToLower() == "rp" ?
                        contractExtensionPremium.Gross : contractExtensionPremium.Gross * contractExtensionPremium.ConversionRate,
                    };
                    response.Add(productV2);
                }
            }
            else
            {
                List<Product> baseProduct = session.Query<Product>()
                   .Where(a => a.Id == product.Id)
                   .ToList();
                foreach (var bundledProduct in baseProduct)
                {
                    ContractExtensions contractExtension =
                        contractExtensions.FirstOrDefault(
                            a =>
                            {
                                var contractInsuaranceLimitation = contractInsuaranceLimitations.Where
                                    (b => b.BaseProductId == bundledProduct.Id).FirstOrDefault();
                                return contractInsuaranceLimitation != null && a.ContractInsuanceLimitationId == contractInsuaranceLimitation.Id;
                            });
                    ContractExtensionPremium contractExtensionPremium = contractExtensionPremiums
                        .Where(a => a.ContractExtensionId == contractExtension.Id).FirstOrDefault();
                    ProductV2 productV2 = new ProductV2()
                    {

                        Id = bundledProduct.Id,
                        InsuaranceLimitationId = contractInsuaranceLimitations.Where(a => a.BaseProductId == bundledProduct.Id).FirstOrDefault().InsuaranceLimitationId,
                        attributeSpecificationId = contractExtension.AttributeSpecificationId,
                        attributeSpecificationName = contractExtension.AttributeSpecification,
                        selectedMakeList = GetSelectedMakeList(contractExtension.Id, contractExtensionMakes),
                        selectedModelsList = GetSelectedModelList(contractExtension.Id, contractExtensionModels),
                        selectedVariantList = GetSelectedVariantList(contractExtension.Id, contractExtensionVariants),
                        selectedEngineCapacities = GetSelectedEngineCapacities(contractExtension.Id, contractExtensionEngineCapacities),
                        selectedClinderCounts = GetSelectedCyllinderCounts(contractExtension.Id, contractExtensionCylinderCounts),
                        selectedGrossWeights = GetSelectedGrossWeights(contractExtension.Id, contractExtensionGvws),

                        warrantyTypeId = contractExtensionPremium.WarrentyTypeId,
                        itemStatusId = contractExtensionPremium.ItemStatusId,

                        premiumBasedOnIdNett = contractExtensionPremium.PremiumBasedOnNett,
                        minValueNett = contractExtensionPremium.MinValueNett * contractExtensionPremium.ConversionRate,
                        maxValueNett = contractExtensionPremium.MaxValueNett * contractExtensionPremium.ConversionRate,
                        isCustAvailableNett = contractExtensionPremium.IsCustomerAvailableNett,
                        premiumAddonsNett = GetPremiumAddons(premiumAddons, contractExtensionPremium.Id,
                            "NETT", contractExtensionPremium),

                        premiumBasedOnIdGross = contractExtensionPremium.PremiumBasedOnGross,
                        minValueGross = contractExtensionPremium.MinValueGross * contractExtensionPremium.ConversionRate,
                        maxValueGross = contractExtensionPremium.MaxValueGross * contractExtensionPremium.ConversionRate,
                        isCustAvailableGross = contractExtensionPremium.IsCustomerAvailableGross,
                        premiumAddonsGross = GetPremiumAddonsGross(premiumAddons, contractExtensionPremium.Id,
                            "GROSS", contractExtensionPremium),

                        totalNRP = new CommonEntityManager().GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnNett).ToLower() == "rp" ?
                        contractExtensionPremium.NRP : contractExtensionPremium.NRP * contractExtensionPremium.ConversionRate,

                        grossPremium = new CommonEntityManager().GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnGross).ToLower() == "rp" ?
                        contractExtensionPremium.Gross : contractExtensionPremium.Gross * contractExtensionPremium.ConversionRate,
                    };
                    response.Add(productV2);
                }
            }



            return response;
        }

        private static List<PremiumAddonsGrossV2> GetPremiumAddonsGross(List<ContractExtensionsPremiumAddon> premiumAddons,
         Guid contractExtensionPremiumId, string type, ContractExtensionPremium contractExtensionPremium)
        {
            var premiumAddonsFilterd = premiumAddons.Where(a => a.ContractExtensionPremiumId == contractExtensionPremiumId);
            return (from premiumAddon in premiumAddonsFilterd
                    let addonType = new CommonEntityManager().GetPremiumBasedonCodeById(premiumAddon.ContractExtensionId)
                    where premiumAddon.PremiumType.ToLower() == type.ToLower()
                    select new PremiumAddonsGrossV2()
                    {
                        Id = premiumAddon.PremiumAddonTypeId,
                        Value = addonType.ToLower() == "rp" ? premiumAddon.Value : premiumAddon.Value * contractExtensionPremium.ConversionRate
                    }).ToList();
        }

        private static List<PremiumAddonsNettV2> GetPremiumAddons(List<ContractExtensionsPremiumAddon> premiumAddons,
            Guid contractExtensionPremiumId, string type, ContractExtensionPremium contractExtensionPremium)
        {

            ISession session = EntitySessionManager.GetSession();
            var premiumAddonsFilterd = premiumAddons.Where(a => a.ContractExtensionPremiumId == contractExtensionPremiumId);
            //var contractIds = premiumAddonsFilterd.Select(a => new { Id = a.ContractExtensionId } ).Distinct().ToList();


            IEnumerable<Guid> contractIds = premiumAddonsFilterd.Select(a => a.ContractExtensionId).Distinct().ToList();
            //IEnumerable<PremiumBasedOn> premiumBasedOn = session.Query<PremiumBasedOn>().Where(a => contractIds.Any(b => b == a.Id));
            IEnumerable<PremiumBasedOn> premiumBasedOn = session.Query<PremiumBasedOn>().Where(a => contractExtensionPremium.PremiumBasedOnNett == a.Id);
            // List<PremiumBasedOn> premiumBasedOns = premiumBasedOn.ToList();

            return (from premiumAddon in premiumAddonsFilterd
                    let addonType = new CommonEntityManager().getAddonTypeCodeById(contractExtensionPremium.PremiumBasedOnNett, premiumBasedOn)
                    where premiumAddon.PremiumType.ToLower() == type.ToLower()
                    select new PremiumAddonsNettV2()
                    {
                        Id = premiumAddon.PremiumAddonTypeId,
                        value = addonType.ToLower() == "rp" ? premiumAddon.Value : premiumAddon.Value * contractExtensionPremium.ConversionRate
                    }).ToList();
        }

        private static List<SelectedGrossWeight> GetSelectedGrossWeights(Guid contractExtensionId, List<ContractExtensionGVW> contractExtensionGvws)
        {
            return contractExtensionGvws
                .Where(a => a.ContractExtensionId == contractExtensionId)
                .Select(ContractExtensionGVW => new SelectedGrossWeight()
                {
                    id = ContractExtensionGVW.GVWId,
                    value = ContractExtensionGVW.GVWId,
                    open = true
                }).ToList();
        }

        private static List<SelectedClinderCount> GetSelectedCyllinderCounts(Guid contractExtensionId, List<ContractExtensionCylinderCount> contractExtensionCylinderCounts)
        {
            return contractExtensionCylinderCounts
                .Where(a => a.ContractExtensionId == contractExtensionId)
                .Select(contractExtensionCc => new SelectedClinderCount()
                {
                    id = contractExtensionCc.CylinderCountId,
                    value = contractExtensionCc.CylinderCountId,
                    open = true
                }).ToList();
        }

        private static List<SelectedEngineCapacity> GetSelectedEngineCapacities(Guid contractExtensionId, List<ContractExtensionEngineCapacity> contractExtensionEngineCapacities)
        {
            return contractExtensionEngineCapacities
                .Where(a => a.ContractExtensionId == contractExtensionId)
                .Select(contractExtensionEngineCapacity => new SelectedEngineCapacity()
                {
                    id = contractExtensionEngineCapacity.EngineCapacityId,
                    value = contractExtensionEngineCapacity.EngineCapacityId,
                    open = true
                }).ToList();
        }

        private static List<SelectedVariantList> GetSelectedVariantList(Guid contractExtensionId, List<ContractExtensionVariant> contractExtensionVariants)
        {
            return contractExtensionVariants
                 .Where(a => a.ContractExtensionId == contractExtensionId)
                 .Select(contractExtensionVariant => new SelectedVariantList()
                 {
                     id = contractExtensionVariant.VariantId,
                     value = contractExtensionVariant.VariantId,
                     open = true
                 }).ToList();
        }

        private static List<SelectedModelsList> GetSelectedModelList(Guid contractExtensionId, List<ContractExtensionModel> contractExtensionModels)
        {
            return contractExtensionModels
                 .Where(a => a.ContractExtensionId == contractExtensionId)
                 .Select(contractExtensionModel => new SelectedModelsList()
                 {
                     id = contractExtensionModel.ModelId,
                     value = contractExtensionModel.ModelId,
                     open = true
                 }).ToList();
        }

        private static List<SelectedMakeList> GetSelectedMakeList(Guid contractExtensionId, List<ContractExtensionMake> contractExtensionMakes)
        {
            return contractExtensionMakes
                .Where(a => a.ContractExtensionId == contractExtensionId)
                .Select(contractExtensionMake => new SelectedMakeList()
                {
                    id = contractExtensionMake.MakeId,
                    value = contractExtensionMake.MakeId,
                    open = true
                }).ToList();
        }

        private static List<TaxV2> GetTaxList(List<ContractTaxMapping> contractTaxeses, decimal rate)
        {
            return contractTaxeses.Select(tax => new TaxV2()
            {
                Id = tax.CountryTaxId,
            }).ToList();
        }

        private static List<EligibilityV2> GetEligibilityList(List<Eligibility> eligibilities, decimal rate)
        {
            return eligibilities.Select(eligibility => new EligibilityV2()
            {
                isPresentage = eligibility.IsPercentage,
                milageMin = eligibility.MileageFrom,
                ageMax = eligibility.AgeTo,
                monthsMax = eligibility.MonthsTo,
                ageMin = eligibility.AgeFrom,
                milageMax = eligibility.MileageTo,
                premium = eligibility.IsPercentage ? eligibility.Premium : eligibility.Premium * rate,
                plusMinus = eligibility.PlusMinus,
                isMandatory = eligibility.isMandatory,
                monthsMin = eligibility.MonthsFrom
            }).ToList();
        }

        private static List<CommissionV2> GetCommissionList(List<NRPCommissionContractMapping> commissionContractMappings, decimal rate)
        {
            return commissionContractMappings.Select(commission => new CommissionV2()
            {
                Id = commission.NRPCommissionId,
                Commission = Math.Round(commission.IsPercentage ? commission.Commission * 100 : commission.Commission * rate * 100) / 100,
                IsOnGROSS = commission.IsOnGROSS,
                IsOnNRP = commission.IsOnNRP,
                IsPercentage = commission.IsPercentage,

            }).ToList();
        }

        public static object GetAllAttributeSpecificationsByInsuranceLimitationId(Guid insuranceLimitationId, Guid contractId)
        {
            Object response = new Object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ContractInsuaranceLimitation contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                    .FirstOrDefault(a => a.ContractId == contractId && a.InsuaranceLimitationId == insuranceLimitationId);
                response = session.Query<ContractExtensions>()
                    .Where(a => a.ContractInsuanceLimitationId == contractInsuaranceLimitation.Id)
                    .Select(a => new
                    {
                        a.AttributeSpecificationId,
                        a.Id,
                        a.AttributeSpecification
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetAllMakeModelDetailsByExtensionId(Guid extensionId)
        {
            AllMakeModelDetailsByExtensionIdResponseDto response = new AllMakeModelDetailsByExtensionIdResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ContractExtensions contractExtension = session.Query<ContractExtensions>()
                    .FirstOrDefault(a => a.Id == extensionId);
                if (contractExtension != null)
                {
                    //make
                    List<ContractExtensionMake> contractExtensionMakes = session.Query<ContractExtensionMake>()
                        .Where(a => a.ContractExtensionId == extensionId).ToList();

                    response.selectedMakeList = new List<SelectedMakeList>();
                    foreach (var selectedMake in contractExtensionMakes)
                    {
                        var make = new SelectedMakeList()
                        {
                            id = selectedMake.MakeId,
                            @checked = true,
                            value = selectedMake.MakeId,
                            text = new CommonEntityManager().GetMakeNameById(selectedMake.MakeId),
                            open = false
                        };
                        response.selectedMakeList.Add(make);
                    }

                    //model
                    List<ContractExtensionModel> contractExtensionModels = session.Query<ContractExtensionModel>()
                        .Where(a => a.ContractExtensionId == extensionId).ToList();
                    response.selectedModelsList = new List<SelectedModelsList>();
                    foreach (var selectedModel in contractExtensionModels)
                    {
                        var model = new SelectedModelsList()
                        {
                            id = selectedModel.ModelId,
                            @checked = true,
                            value = selectedModel.ModelId,
                            text = "",
                            open = true
                        };
                        response.selectedModelsList.Add(model);
                    }

                    //variant
                    List<ContractExtensionVariant> contractExtensionVariants = session.Query<ContractExtensionVariant>()
                       .Where(a => a.ContractExtensionId == extensionId).ToList();
                    response.selectedVariantList = new List<SelectedVariantList>();
                    foreach (var selectedVariant in contractExtensionVariants)
                    {
                        var variant = new SelectedVariantList()
                        {
                            id = selectedVariant.VariantId,
                            @checked = true,
                            value = selectedVariant.VariantId,
                            text = "",
                            open = false
                        };
                        response.selectedVariantList.Add(variant);
                    }
                    //ec
                    List<ContractExtensionEngineCapacity> contractExtensionEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                      .Where(a => a.ContractExtensionId == extensionId).ToList();
                    response.selectedEngineCapacities = new List<SelectedEngineCapacity>();
                    foreach (var selectedEc in contractExtensionEngineCapacities)
                    {
                        var ec = new SelectedEngineCapacity()
                        {
                            id = selectedEc.EngineCapacityId,
                            @checked = true,
                            value = selectedEc.EngineCapacityId,
                            text = "",
                            open = false
                        };
                        response.selectedEngineCapacities.Add(ec);
                    }
                    //cc

                    List<ContractExtensionCylinderCount> contractExtensionCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                       .Where(a => a.ContractExtensionId == extensionId).ToList();
                    response.selectedClinderCounts = new List<SelectedClinderCount>();
                    foreach (var selectedCc in contractExtensionCylinderCounts)
                    {
                        var cc = new SelectedClinderCount()
                        {
                            id = selectedCc.CylinderCountId,
                            @checked = true,
                            value = selectedCc.CylinderCountId,
                            text = "",
                            open = false
                        };
                        response.selectedClinderCounts.Add(cc);
                    }

                    //gvw


                    List<ContractExtensionGVW> contractExtensioGvw = session.Query<ContractExtensionGVW>()
                       .Where(a => a.ContractExtensionId == extensionId).ToList();
                    response.selectedGrossWeights = new List<SelectedGrossWeight>();
                    foreach (var selectedGvw in contractExtensioGvw)
                    {
                        var gvw = new SelectedGrossWeight()
                        {
                            id = selectedGvw.GVWId,
                            @checked = true,
                            value = selectedGvw.GVWId,
                            text = "",
                            open = false
                        };
                        response.selectedGrossWeights.Add(gvw);
                    }

                    response.isAllMakesSelected = contractExtension.IsAllMakesSelected;
                    response.isAllModelsSelected = contractExtension.IsAllModelsSelected;
                    response.isAllVariantsSelected = contractExtension.IsAllVariantsSelected;
                    response.isAllCCsSelected = contractExtension.IsAllCyllinderCountsSelected;
                    response.isAllECsSelected = contractExtension.IsAllEngineCapacitiesSelected;
                    response.isAllGVWsSelected = contractExtension.IsAllGVWSelected;

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetAllAPremiumByExtensionId(Guid extensionId)
        {
            List<AllPremiumDetailsByExtensionIdResponseDto> response = new List<AllPremiumDetailsByExtensionIdResponseDto>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ContractExtensions contractExtension = session.Query<ContractExtensions>()
                    .FirstOrDefault(a => a.Id == extensionId);
                if (contractExtension != null)
                {
                    List<ContractExtensionPremium> premiums = session.Query<ContractExtensionPremium>()
                        .Where(a => a.ContractExtensionId == extensionId).ToList();
                    foreach (var premium in premiums)
                    {
                        decimal conversionRate = premium.ConversionRate;
                        bool isRetailPriceGross =
                            new CommonEntityManager().GetPremiumBasedonCodeById(premium.PremiumBasedOnGross).ToLower() ==
                            "rp";
                        bool isRetailPriceNett =
                           new CommonEntityManager().GetPremiumBasedonCodeById(premium.PremiumBasedOnNett).ToLower() ==
                           "rp";

                        var premiumDetail = new AllPremiumDetailsByExtensionIdResponseDto()
                        {
                            maxValueNett = Math.Round(premium.MaxValueNett * conversionRate * 100) / 100,
                            isCustAvailableGross = premium.IsCustomerAvailableGross,
                            isCustAvailableNett = premium.IsCustomerAvailableNett,
                            itemStatusId = premium.ItemStatusId,
                            maxValueGross = Math.Round(premium.MaxValueGross * conversionRate * 100) / 100,
                            minValueGross = Math.Round(premium.MinValueGross * conversionRate * 100) / 100,
                            minValueNett = Math.Round(premium.MinValueNett * conversionRate * 100) / 100,
                            nrp = Math.Round(isRetailPriceNett ? premium.NRP * 100 : premium.NRP * conversionRate * 100) / 100,
                            gross = Math.Round(isRetailPriceGross ? premium.Gross * 100 : premium.Gross * conversionRate * 100) / 100,
                            premiumAddonsNett = GetPremiumAddonsByContractExtensionPremiumIdNett(premium.Id, isRetailPriceNett, conversionRate),
                            premiumAddonsGross = GetPremiumAddonsByContractExtensionPremiumIdGross(premium.Id, isRetailPriceGross, conversionRate),
                            premiumBasedOnIdNett = premium.PremiumBasedOnNett,
                            premiumBasedOnIdGross = premium.PremiumBasedOnGross,
                            warrentyTypeId = premium.WarrentyTypeId
                        };
                        response.Add(premiumDetail);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static List<PremiumAddonsNettV2> GetPremiumAddonsByContractExtensionPremiumIdNett(Guid contractExtensionPremiumAddonId,
            bool isRetailPriceNett, decimal conversionRate)
        {
            List<PremiumAddonsNettV2> response = new List<PremiumAddonsNettV2>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddons =
                    session.Query<ContractExtensionsPremiumAddon>()
                        .Where(a => a.ContractExtensionPremiumId == contractExtensionPremiumAddonId
                        && a.PremiumType.ToLower() == "nett").ToList();
                foreach (var contractExtensionsPremiumAddon in contractExtensionsPremiumAddons)
                {
                    var premiumAddon = new PremiumAddonsNettV2()
                    {
                        Id = contractExtensionsPremiumAddon.PremiumAddonTypeId,
                        value = Math.Round(isRetailPriceNett ? contractExtensionsPremiumAddon.Value * 100 : contractExtensionsPremiumAddon.Value * conversionRate * 100) / 100
                    };
                    response.Add(premiumAddon);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static List<PremiumAddonsGrossV2> GetPremiumAddonsByContractExtensionPremiumIdGross(Guid contractExtensionPremiumAddonId,
          bool isRetailPriceGross, decimal conversionRate)
        {
            List<PremiumAddonsGrossV2> response = new List<PremiumAddonsGrossV2>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddons =
                    session.Query<ContractExtensionsPremiumAddon>()
                        .Where(a => a.ContractExtensionPremiumId == contractExtensionPremiumAddonId
                        && a.PremiumType.ToLower() == "gross").ToList();
                foreach (var contractExtensionsPremiumAddon in contractExtensionsPremiumAddons)
                {
                    var premiumAddon = new PremiumAddonsGrossV2()
                    {
                        Id = contractExtensionsPremiumAddon.PremiumAddonTypeId,

                        Value = Math.Round(isRetailPriceGross ? contractExtensionsPremiumAddon.Value * 100 : contractExtensionsPremiumAddon.Value * conversionRate * 100) / 100
                    };
                    response.Add(premiumAddon);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetAllExtensionTypeByContractId(Guid contractId, Guid productId, Guid dealerId, DateTime date,
            Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight)
        {
            object response = new object();
            try
            {
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.ProductId == productId);
                filterContract = filterContract.And(a => a.DealerId == dealerId);
                filterContract = filterContract.And(a => a.StartDate <= date);
                filterContract = filterContract.And(a => a.EndDate > date);
                filterContract = filterContract.And(a => a.Id == contractId);

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId))
                    .Select(d => d.Id);
                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == makeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == modelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == cylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == engineCapacityId).Select(b => b.ContractExtensionId);



                Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                //string commodityTypes = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                CommodityType commodityType = session.Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == product.CommodityTypeId);

                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels);
                if (commodityType.CommodityCode.ToLower() == "a")
                {

                    if (commodityType.CommodityTypeDescription.ToLower() != "tyre")
                    {
                        allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                       .Intersect(contractExtensionIdsEngineCapacities);
                    }


                }

                if (commodityType.CommodityCode.ToLower() == "b")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }

                if (IsGuid(variantId.ToString()))
                {
                    IEnumerable<Guid> contractExtensionIdVariants = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == variantId).Select(b => b.ContractExtensionId);
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdVariants);
                }

                if (grossWeight > 0)
                {
                    VehicleWeight matchingVehicleWeight = session.Query<VehicleWeight>()
                        .Where(a => a.WeightFrom <= grossWeight && a.WeightTo >= grossWeight).FirstOrDefault();
                    if (matchingVehicleWeight != null)
                    {
                        //  IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        //.Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        //  allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdGw);

                        List<Guid> allEligibleContractsForGvw = new List<Guid>();
                        //by contract filtering
                        IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        .Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                        //by contracts which dont selected any contractExt Gvw
                        List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                            .Select(a => a.Id).ToList();
                        List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                            .Select(a => a.ContractExtensionId).ToList();
                        //no gvw contracts
                        IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                        allEligibleContractsForGvw.AddRange(allowedContracts);

                        allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                    }
                }

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<Guid> insuranceLimitationIds = session.QueryOver<ContractExtensions>()
                    .WhereRestrictionOn(c => c.Id).IsIn(finalExtensionIds)
                    .Select(o => o.ContractInsuanceLimitationId).List<Guid>();

                IList<ContractInsuaranceLimitation> insuaranceLimitations = session.QueryOver<ContractInsuaranceLimitation>()
                    .WhereRestrictionOn(k => k.Id).IsIn(insuranceLimitationIds.ToList())
                    .List<ContractInsuaranceLimitation>();

                response = insuaranceLimitations
                .Select(z => new
                {
                    z.Id,
                    ExtensionName = new CommonEntityManager().GetInsuranceLimitationNameById(z.InsuaranceLimitationId),
                    ExtensionMonths = new CommonEntityManager().GetInsuranceLimitationMonthsById(z.InsuaranceLimitationId)
                }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetAllExtensionTypeByMakeModel(Guid dealerId, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId)
        {
            object response = new object();
            try
            {
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.DealerId == dealerId);


                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId))
                    .Select(d => d.Id);
                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == makeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == modelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == cylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == engineCapacityId).Select(b => b.ContractExtensionId);



                // Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                //  string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels);

                allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                .Intersect(contractExtensionIdsEngineCapacities);


                List<Guid> allEligibleContractsForGvw = new List<Guid>();
                //by contract filtering
                IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                .Select(b => b.ContractExtensionId);
                allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                //by contracts which dont selected any contractExt Gvw
                List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                    .Select(a => a.Id).ToList();
                List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                    .Select(a => a.ContractExtensionId).ToList();
                //no gvw contracts
                IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                allEligibleContractsForGvw.AddRange(allowedContracts);

                allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                //    }
                //}

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<Guid> insuranceLimitationIds = session.QueryOver<ContractExtensions>()
                    .WhereRestrictionOn(c => c.Id).IsIn(finalExtensionIds)
                    .Select(o => o.ContractInsuanceLimitationId).List<Guid>();

                IList<ContractInsuaranceLimitation> insuaranceLimitations = session.QueryOver<ContractInsuaranceLimitation>()
                    .WhereRestrictionOn(k => k.Id).IsIn(insuranceLimitationIds.ToList())
                    .List<ContractInsuaranceLimitation>();

                response = insuaranceLimitations
                .Select(z => new
                {
                    z.InsuaranceLimitationId,
                    ExtensionName = new CommonEntityManager().GetInsuranceLimitationNameById(z.InsuaranceLimitationId)
                }).Distinct()
                .ToArray();


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetAttributeSpecificationByExtensionId(Guid extensionId, Guid contractId, Guid productId, Guid dealerId,
            DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight)
        {
            object response = new object();
            try
            {
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.ProductId == productId);
                filterContract = filterContract.And(a => a.DealerId == dealerId);
                filterContract = filterContract.And(a => a.StartDate <= date);
                filterContract = filterContract.And(a => a.EndDate > date);
                filterContract = filterContract.And(a => a.Id == contractId);

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId) && c.ContractInsuanceLimitationId == extensionId)
                    .Select(d => d.Id);
                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == makeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == modelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == cylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == engineCapacityId).Select(b => b.ContractExtensionId);

                Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                //string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                CommodityType commodityType = session.Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == product.CommodityTypeId);
                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels);
                if (commodityType.CommodityCode.ToLower() == "a")
                {
                    if (commodityType.CommodityTypeDescription.ToLower() != "tyre")
                    {
                        allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                    }
                }

                if (commodityType.CommodityCode.ToLower() == "b")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }



                if (IsGuid(variantId.ToString()))
                {
                    IEnumerable<Guid> contractExtensionIdVariants = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == variantId).Select(b => b.ContractExtensionId);
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdVariants);
                }

                if (grossWeight > 0)
                {
                    VehicleWeight matchingVehicleWeight = session.Query<VehicleWeight>()
                        .FirstOrDefault(a => a.WeightFrom <= grossWeight && a.WeightTo >= grossWeight);
                    if (matchingVehicleWeight != null)
                    {
                        //  IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        //.Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        //  allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdGw);

                        List<Guid> allEligibleContractsForGvw = new List<Guid>();
                        //by contract filtering
                        IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        .Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                        //by contracts which dont selected any contractExt Gvw
                        List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                            .Select(a => a.Id).ToList();
                        List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                            .Select(a => a.ContractExtensionId).ToList();
                        //no gvw contracts
                        IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                        allEligibleContractsForGvw.AddRange(allowedContracts);

                        allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                    }
                }

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<ContractExtensions> insuranceLimitationIds = session.QueryOver<ContractExtensions>()
                    .WhereRestrictionOn(c => c.Id).IsIn(finalExtensionIds)
                    .List<ContractExtensions>();
                response = insuranceLimitationIds.Select(a => new
                {
                    a.Id,
                    a.AttributeSpecification
                }).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        public object GetGetCoverTypesByExtensionIdUnitOfWork(Guid attributeSpecificationId, Guid extensionId, Guid contractId, Guid productId,
            Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight, Guid itemStatusId)
        {
            object response = new object();
            try
            {
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.ProductId == productId);
                filterContract = filterContract.And(a => a.DealerId == dealerId);
                filterContract = filterContract.And(a => a.StartDate <= date);
                filterContract = filterContract.And(a => a.EndDate > date);
                filterContract = filterContract.And(a => a.Id == contractId);

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId) && c.ContractInsuanceLimitationId == extensionId)
                    .Select(d => d.Id);

                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == makeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == modelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == cylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == engineCapacityId).Select(b => b.ContractExtensionId);

                Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                //string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                CommodityType commodityType = session.Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == product.CommodityTypeId);
                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels);
                if (commodityType.CommodityCode.ToLower() == "a")
                {
                    if (commodityType.CommodityTypeDescription.ToLower() != "tyre")
                    {

                        allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                    }
                }

                if (commodityType.CommodityCode.ToLower() == "b")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }



                if (IsGuid(variantId.ToString()))
                {
                    IEnumerable<Guid> contractExtensionIdVariants = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == variantId).Select(b => b.ContractExtensionId);
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdVariants);
                }

                if (grossWeight > 0)
                {
                    VehicleWeight matchingVehicleWeight = session.Query<VehicleWeight>()
                        .FirstOrDefault(a => a.WeightFrom <= grossWeight && a.WeightTo >= grossWeight);
                    if (matchingVehicleWeight != null)
                    {
                        //  IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        //.Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        //  allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdGw);
                        List<Guid> allEligibleContractsForGvw = new List<Guid>();
                        //by contract filtering
                        IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        .Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                        //by contracts which dont selected any contractExt Gvw
                        List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                            .Select(a => a.Id).ToList();
                        List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                            .Select(a => a.ContractExtensionId).ToList();
                        //no gvw contracts
                        IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                        allEligibleContractsForGvw.AddRange(allowedContracts);

                        allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                    }
                }

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<ContractExtensionPremium> contractExtensionPremiums = session.QueryOver<ContractExtensionPremium>()
                    .WhereRestrictionOn(c => c.ContractExtensionId).IsIn(finalExtensionIds)
                    .Where(x => x.ItemStatusId == itemStatusId)
                    .List<ContractExtensionPremium>();


                response = contractExtensionPremiums.Select(a => new
                {
                    a.Id,
                    WarrantyTypeDescription = new CommonEntityManager().GetWarrentyTypeNameById(a.WarrentyTypeId)

                }).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object GetGetCoverTypesByExtensionIdUnitOfWork2(Guid attributeSpecificationId, Guid extensionId, Guid contractId, Guid productId,
            Guid dealerId, DateTime date, Guid cylinderCountId, Guid engineCapacityId, Guid makeId, Guid modelId, Guid variantId, decimal grossWeight, Guid itemStatusId)
        {
            object response = new object();
            try
            {
                //expression builder for contract
                Expression<Func<Contract, bool>> filterContract = PredicateBuilder.True<Contract>();
                filterContract = filterContract.And(a => a.IsActive == true);
                filterContract = filterContract.And(a => a.ProductId == productId);
                filterContract = filterContract.And(a => a.DealerId == dealerId);
                filterContract = filterContract.And(a => a.StartDate <= date);
                filterContract = filterContract.And(a => a.EndDate > date);
                filterContract = filterContract.And(a => a.Id == contractId);

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Contract> currentContractList = session.Query<Contract>().Where(filterContract);

                IEnumerable<ContractInsuaranceLimitation> currentContractListContractInsuaranceLimitations = session
                    .Query<ContractInsuaranceLimitation>()
                    .Where(a => currentContractList.Any(b => b.Id == a.ContractId));
                if (!currentContractList.Any())
                    return response;

                IEnumerable<Guid> contractExtensionsIds = session.Query<ContractExtensions>()
                    .Where(c => currentContractListContractInsuaranceLimitations
                        .Any(d => d.Id == c.ContractInsuanceLimitationId) && c.ContractInsuanceLimitationId == extensionId)
                    .Select(d => d.Id);

                IEnumerable<Guid> contractExtensionIdsMakes = session.Query<ContractExtensionMake>()
                    .Where(a => a.MakeId == makeId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsModels = session.Query<ContractExtensionModel>()
                    .Where(a => a.ModelId == modelId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsCylinderCounts = session.Query<ContractExtensionCylinderCount>()
                  .Where(a => a.CylinderCountId == cylinderCountId).Select(b => b.ContractExtensionId);
                IEnumerable<Guid> contractExtensionIdsEngineCapacities = session.Query<ContractExtensionEngineCapacity>()
                  .Where(a => a.EngineCapacityId == engineCapacityId).Select(b => b.ContractExtensionId);

                Product product = session.Query<Product>().FirstOrDefault(a => a.Id == productId);
                //string commodityType = new CommonEntityManager().GetCommodityTypeUniqueCodeById(product.CommodityTypeId);
                CommodityType commodityType = session.Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == product.CommodityTypeId);
                IEnumerable<Guid> allowExtensionIds = contractExtensionIdsMakes.Intersect(contractExtensionIdsModels);
                if (commodityType.CommodityCode.ToLower() == "a")
                {
                    if (commodityType.CommodityTypeDescription.ToLower() != "tyre")
                    {

                        allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                    }
                }

                if (commodityType.CommodityCode.ToLower() == "b")
                {
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdsCylinderCounts)
                    .Intersect(contractExtensionIdsEngineCapacities);
                }

                if (IsGuid(variantId.ToString()))
                {
                    IEnumerable<Guid> contractExtensionIdVariants = session.Query<ContractExtensionVariant>()
                        .Where(a => a.VariantId == variantId).Select(b => b.ContractExtensionId);
                    allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdVariants);
                }

                if (grossWeight > 0)
                {
                    VehicleWeight matchingVehicleWeight = session.Query<VehicleWeight>()
                        .FirstOrDefault(a => a.WeightFrom <= grossWeight && a.WeightTo >= grossWeight);
                    if (matchingVehicleWeight != null)
                    {
                        //  IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        //.Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        //  allowExtensionIds = allowExtensionIds.Intersect(contractExtensionIdGw);

                        List<Guid> allEligibleContractsForGvw = new List<Guid>();
                        //by contract filtering
                        IEnumerable<Guid> contractExtensionIdGw = session.Query<ContractExtensionGVW>()
                        .Where(a => a.GVWId == matchingVehicleWeight.Id).Select(b => b.ContractExtensionId);
                        allEligibleContractsForGvw.AddRange(contractExtensionIdGw);
                        //by contracts which dont selected any contractExt Gvw
                        List<Guid> allContractExtIds = session.Query<ContractExtensions>()
                            .Select(a => a.Id).ToList();
                        List<Guid> allGvwAvailableContracts = session.Query<ContractExtensionGVW>()
                            .Select(a => a.ContractExtensionId).ToList();
                        //no gvw contracts
                        IEnumerable<Guid> allowedContracts = allContractExtIds.Except(allGvwAvailableContracts);
                        allEligibleContractsForGvw.AddRange(allowedContracts);

                        allowExtensionIds = allowExtensionIds.Intersect(allEligibleContractsForGvw);
                    }
                }

                List<Guid> finalExtensionIds = allowExtensionIds.Intersect(contractExtensionsIds).ToList();

                IList<ContractExtensionPremium> contractExtensionPremiums = session.QueryOver<ContractExtensionPremium>()
                    .WhereRestrictionOn(c => c.ContractExtensionId).IsIn(finalExtensionIds)
                    .Where(x => x.ItemStatusId == itemStatusId)
                    .List<ContractExtensionPremium>();


                response = contractExtensionPremiums.Select(a => new
                {
                    a.Id,
                    WarrantyTypeDescription = new CommonEntityManager().GetWarrentyTypeNameById(a.WarrentyTypeId)

                }).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public object GetPremium(Guid contractPremiumId, decimal usage, Guid attributeSpecificationId, Guid extensionId,
            Guid contractId, Guid productId, Guid dealerId, DateTime policySoldDate, Guid cylinderCountId, Guid engineCapacityId,
            Guid makeId, Guid modelId, Guid variantId, decimal grossWeight, Guid itemStatusId, decimal DealerPrice, DateTime ItemPurchasedDate)
        {
            GetPremiumResponseDto response = new GetPremiumResponseDto
            {
                Status = "ok"
            };

            try
            {
                ISession session = EntitySessionManager.GetSession();
                var commonEntityManeger = new CommonEntityManager();
                Guid commodityTypeId = commonEntityManeger.GetCommodityTypeIdByContractId(contractId);
                ContractExtensionPremium contractExtensionPremium =
                    session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.Id == contractPremiumId);
                response.Currency = commonEntityManeger.GetCurrencyCodeById(contractExtensionPremium.CurrencyId);

                ///this modification has done for iloe premium calclation
                Product product = session.Query<Product>().FirstOrDefault(x => x.Id == productId);
                if (product == null)
                    return response.Status = "Invalid product information provided";
                ProductType productType = session.Query<ProductType>().FirstOrDefault(x => x.Id == product.ProductTypeId);
                if (productType == null)
                    return response.Status = "Invalid product type.";
                if (productType.Code.ToUpper().Trim() != "ILOE")
                {
                    //gross
                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddons = session
                            .QueryOver<ContractExtensionsPremiumAddon>()
                            .WhereRestrictionOn(a => a.ContractExtensionPremiumId).IsIn(new Guid[] { contractPremiumId })
                            .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "GROSS" })
                            .List<ContractExtensionsPremiumAddon>();

                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddonsNrp = session
                         .QueryOver<ContractExtensionsPremiumAddon>()
                         .WhereRestrictionOn(a => a.ContractExtensionPremiumId).IsIn(new Guid[] { contractPremiumId })
                           .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "NETT" })
                             .List<ContractExtensionsPremiumAddon>();

                    IList<Guid> mandatoryAddonIds = session.QueryOver<PremiumAddonType>()
                        .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                        .Select(a => a.Id)
                        .List<Guid>();
                    IList<Guid> mandatoryAddonIdsNrp = session.QueryOver<PremiumAddonType>()
                       .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                       .Select(a => a.Id)
                       .List<Guid>();

                    response.BasicPremium = contractExtensionsPremiumAddons
                        .Where(a => mandatoryAddonIds.Any(c => c == a.PremiumAddonTypeId))
                        .Sum(a => a.Value);
                    response.BasicPremiumNRP = contractExtensionsPremiumAddonsNrp
                     .Where(a => mandatoryAddonIdsNrp.Any(c => c == a.PremiumAddonTypeId))
                     .Sum(a => a.Value);


                    //variant based premium addons
                    if (IsGuid(variantId.ToString()))
                    {
                        IList<Guid> variantAddonIds = session.QueryOver<VariantPremiumAddon>()
                            .Where(a => a.VariantId == variantId)
                            .Select(a => a.PremiumAddonTypeId).List<Guid>();

                        decimal variantPremium = contractExtensionsPremiumAddons
                        .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                         .Sum(a => a.Value);

                        response.VariantPremium = variantPremium;



                        decimal variantPremiumNrp = contractExtensionsPremiumAddonsNrp
                        .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                         .Sum(a => a.Value);

                        response.VariantPremium = variantPremium;
                        response.VariantPremiumNRP = variantPremiumNrp;


                    }
                    //setting premium if it is based on retail price
                    if (
                        commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnGross)
                            .ToLower() == "rp")
                    {
                        var calculatedRate = response.BasicPremium + response.VariantPremium;
                        var calculatedPremiumUsd = (DealerPrice / contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueGross)
                            calculatedPremiumUsd = contractExtensionPremium.MinValueGross;
                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueGross)
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueGross;
                        response.TotalPremium = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        response.TotalPremium = (response.VariantPremium + response.BasicPremium) * contractExtensionPremium.ConversionRate;
                    }

                    if (
                       commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnNett)
                           .ToLower() == "rp")
                    {
                        var calculatedRate = response.BasicPremiumNRP + response.VariantPremiumNRP;
                        var calculatedPremiumUsd = (DealerPrice / contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueNett)
                            calculatedPremiumUsd = contractExtensionPremium.MinValueNett;
                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueNett)
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueNett;
                        response.TotalPremiumNRP = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        response.TotalPremiumNRP = (response.VariantPremiumNRP + response.BasicPremiumNRP) * contractExtensionPremium.ConversionRate;
                    }
                }
                else
                {

                    String iloePremiumDetailsQuery = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\IloePremiumDetails.sql"));
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{contractId}", contractId.ToString());
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{contractInsuranceLimitationId}", extensionId.ToString());
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{conractExtensionPremiumId}", contractPremiumId.ToString());

                    IloePremiumDetailsResponseDto iloePremiumDetails = session.CreateSQLQuery(iloePremiumDetailsQuery).SetResultTransformer(Transformers.AliasToBean<IloePremiumDetailsResponseDto>()).List<IloePremiumDetailsResponseDto>().FirstOrDefault();
                    if (iloePremiumDetails == null)
                        return response.Status = "No premiums foud for selected criterias.";

                    if(iloePremiumDetails.AnnualInterestRate == 0)
                        return response.Status = "Selected Deal's AER is zero. Please contract the Administrator.";

                    if (iloePremiumDetails.MinimumPayment == 0 || iloePremiumDetails.MaximumPayment==0)
                        return response.Status = "Selected Deal's Per Claim Liability or Total Liability Limitations are zero. Please contract the Administrator.";


                    double loanAmountLocal = (double)DealerPrice;
                    double conversionRate = (double)contractExtensionPremium.ConversionRate;
                    double monthlyInterestRate = (iloePremiumDetails.AnnualInterestRate * 0.1) / 12;
                    //starting calculaton
                    PolicyEntityManager pem = new PolicyEntityManager();
                    object emivalue = pem.GetEMIValue(DealerPrice, contractId);
                    double emiLocal = Math.Abs(Financial.Pmt(monthlyInterestRate, iloePremiumDetails.LoanPeriod, loanAmountLocal));
                    double emiUsd = Convert.ToDouble(emivalue) / conversionRate;

                    double payoutUsd = (emiUsd > iloePremiumDetails.MinimumPayment ? (iloePremiumDetails.MinimumPayment * iloePremiumDetails.IncubationPeriod) : emiUsd * iloePremiumDetails.IncubationPeriod);
                    double nrpPerMonth = payoutUsd * iloePremiumDetails.NRPRate;
                    double nrpTotal = nrpPerMonth * iloePremiumDetails.LoanPeriod;

                    //Change value
                    double grossPerMonth = payoutUsd * (iloePremiumDetails.GrossRate/100);
                    double grossTotal = grossPerMonth * iloePremiumDetails.LoanPeriod;

                    response.TotalPremium = ToDecimalSafe(grossTotal * conversionRate) + 150;
                    response.TotalPremiumNRP = ToDecimalSafe(nrpTotal * conversionRate);
                }

                //eligibility fee
                IList<Eligibility> eligibilities = session.QueryOver<Eligibility>()
                    .Where(a => a.ContractId == contractId)
                    .List<Eligibility>();
                bool isMandatoryEligibilitySatisfied = false;
                foreach (Eligibility eligibility in eligibilities)
                {
                    var commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);

                    if (commodityCode == "A")
                    {
                        double usedMonths = (double)(policySoldDate - ItemPurchasedDate).Days / 30;
                        if (eligibility.MileageFrom <= usage && eligibility.MileageTo >= usage &&
                            eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                        {
                            if (eligibility.IsPercentage)
                            {
                                response.EligibilityPremium = response.TotalPremium * eligibility.Premium / 100;
                            }
                            else
                            {
                                response.EligibilityPremium = eligibility.Premium *
                                                              contractExtensionPremium.ConversionRate;
                            }
                            if (eligibility.PlusMinus.ToLower() != "plus")
                                response.EligibilityPremium = response.EligibilityPremium * (-1);

                            if (eligibility.isMandatory)
                                isMandatoryEligibilitySatisfied = true;
                        }
                    }
                    else
                    {
                        if (eligibility.MonthsFrom <= usage && eligibility.MonthsTo >= usage)
                        {
                            if (eligibility.IsPercentage)
                            {
                                response.EligibilityPremium = response.TotalPremium * eligibility.Premium / 100;
                            }
                            else
                            {
                                response.EligibilityPremium = eligibility.Premium *
                                                              contractExtensionPremium.ConversionRate;
                            }
                            if (eligibility.PlusMinus.ToLower() != "plus")
                                response.EligibilityPremium = response.EligibilityPremium * (-1);
                            if (eligibility.isMandatory)
                                isMandatoryEligibilitySatisfied = true;
                        }
                    }
                }


                response.TotalPremium += response.EligibilityPremium;
                response.TotalPremiumNRP += response.EligibilityPremium;

                bool isMandatoryEligibilityAvailable = eligibilities.Any(a => a.isMandatory);
                if (isMandatoryEligibilityAvailable && !isMandatoryEligibilitySatisfied)
                    return response.Status = "Mandatory eligibility criterias not satisfied. Please check usage & registration date";


                //tax
                IList<ContractTaxMapping> contractTaxesList = session.QueryOver<ContractTaxMapping>()
                    .Where(a => a.ContractId == contractId).List<ContractTaxMapping>();
                IList<CountryTaxes> countryTaxesList = session.QueryOver<CountryTaxes>()
                   .WhereRestrictionOn(b => b.Id).IsIn(contractTaxesList.Select(a => a.CountryTaxId).ToList())
                   .List<CountryTaxes>();

                var totalTax = decimal.Zero;

                foreach (var countryTax in countryTaxesList.OrderBy(a => a.IndexVal))
                {
                    var currentTax = decimal.Zero;
                    if (countryTax.IsPercentage)
                    {
                        if (countryTax.IsOnGross)
                        {
                            if (countryTax.IsOnPreviousTax)
                            {
                                currentTax = (response.TotalPremium + totalTax) * countryTax.TaxValue / 100;
                            }
                            else
                            {
                                currentTax = (response.TotalPremium) * countryTax.TaxValue / 100;
                            }
                        }
                        else
                        {
                            if (countryTax.IsOnPreviousTax)
                            {
                                currentTax = (response.TotalPremiumNRP + totalTax) * countryTax.TaxValue / 100;
                            }
                            else
                            {
                                currentTax = (response.TotalPremiumNRP) * countryTax.TaxValue / 100;
                            }
                        }
                        if (currentTax < countryTax.MinimumValue * countryTax.ConversionRate)
                            currentTax = countryTax.MinimumValue * countryTax.ConversionRate;

                    }
                    else
                    {
                        currentTax += countryTax.TaxValue * countryTax.ConversionRate;
                    }
                    totalTax += currentTax;
                }
                response.Tax = totalTax;
                response.TotalPremium += totalTax;

                response.TotalPremium = Math.Round(response.TotalPremium * 100) / 100;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }



        public object GetPremiumParsingSession(Guid contractPremiumId, decimal usage, Guid attributeSpecificationId, Guid extensionId,
    Guid contractId, Guid productId, Guid dealerId, DateTime policySoldDate, Guid cylinderCountId, Guid engineCapacityId,
    Guid makeId, Guid modelId, Guid variantId, decimal grossWeight, Guid itemStatusId, decimal DealerPrice, DateTime ItemPurchasedDate , ISession session)
        {
            GetPremiumResponseDto response = new GetPremiumResponseDto
            {
                Status = "ok"
            };

            try
            {
                var commonEntityManeger = new CommonEntityManager();
                Guid commodityTypeId = commonEntityManeger.GetCommodityTypeIdByContractId(contractId);
                ContractExtensionPremium contractExtensionPremium =
                    session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.Id == contractPremiumId);
                response.Currency = commonEntityManeger.GetCurrencyCodeById(contractExtensionPremium.CurrencyId);

                ///this modification has done for iloe premium calclation
                Product product = session.Query<Product>().FirstOrDefault(x => x.Id == productId);
                if (product == null)
                    return response.Status = "Invalid product information provided";
                ProductType productType = session.Query<ProductType>().FirstOrDefault(x => x.Id == product.ProductTypeId);
                if (productType == null)
                    return response.Status = "Invalid product type.";
                if (productType.Code.ToUpper().Trim() != "ILOE")
                {
                    //gross
                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddons = session
                            .QueryOver<ContractExtensionsPremiumAddon>()
                            .WhereRestrictionOn(a => a.ContractExtensionPremiumId).IsIn(new Guid[] { contractPremiumId })
                            .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "GROSS" })
                            .List<ContractExtensionsPremiumAddon>();

                    IList<ContractExtensionsPremiumAddon> contractExtensionsPremiumAddonsNrp = session
                         .QueryOver<ContractExtensionsPremiumAddon>()
                         .WhereRestrictionOn(a => a.ContractExtensionPremiumId).IsIn(new Guid[] { contractPremiumId })
                           .WhereRestrictionOn(c => c.PremiumType).IsIn(new string[] { "NETT" })
                             .List<ContractExtensionsPremiumAddon>();

                    IList<Guid> mandatoryAddonIds = session.QueryOver<PremiumAddonType>()
                        .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                        .Select(a => a.Id)
                        .List<Guid>();
                    IList<Guid> mandatoryAddonIdsNrp = session.QueryOver<PremiumAddonType>()
                       .Where(a => a.IsApplicableforVariant != true && a.CommodityTypeId == commodityTypeId)
                       .Select(a => a.Id)
                       .List<Guid>();

                    response.BasicPremium = contractExtensionsPremiumAddons
                        .Where(a => mandatoryAddonIds.Any(c => c == a.PremiumAddonTypeId))
                        .Sum(a => a.Value);
                    response.BasicPremiumNRP = contractExtensionsPremiumAddonsNrp
                     .Where(a => mandatoryAddonIdsNrp.Any(c => c == a.PremiumAddonTypeId))
                     .Sum(a => a.Value);


                    //variant based premium addons
                    if (IsGuid(variantId.ToString()))
                    {
                        IList<Guid> variantAddonIds = session.QueryOver<VariantPremiumAddon>()
                            .Where(a => a.VariantId == variantId)
                            .Select(a => a.PremiumAddonTypeId).List<Guid>();

                        decimal variantPremium = contractExtensionsPremiumAddons
                        .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                         .Sum(a => a.Value);

                        response.VariantPremium = variantPremium;



                        decimal variantPremiumNrp = contractExtensionsPremiumAddonsNrp
                        .Where(a => variantAddonIds.Any(c => c == a.PremiumAddonTypeId))
                         .Sum(a => a.Value);

                        response.VariantPremium = variantPremium;
                        response.VariantPremiumNRP = variantPremiumNrp;


                    }
                    //setting premium if it is based on retail price
                    if (
                        commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnGross)
                            .ToLower() == "rp")
                    {
                        var calculatedRate = response.BasicPremium + response.VariantPremium;
                        var calculatedPremiumUsd = (DealerPrice / contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueGross)
                            calculatedPremiumUsd = contractExtensionPremium.MinValueGross;
                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueGross)
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueGross;
                        response.TotalPremium = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        response.TotalPremium = (response.VariantPremium + response.BasicPremium) * contractExtensionPremium.ConversionRate;
                    }

                    if (
                       commonEntityManeger.GetPremiumBasedonCodeById(contractExtensionPremium.PremiumBasedOnNett)
                           .ToLower() == "rp")
                    {
                        var calculatedRate = response.BasicPremiumNRP + response.VariantPremiumNRP;
                        var calculatedPremiumUsd = (DealerPrice / contractExtensionPremium.ConversionRate) * calculatedRate / 100;
                        if (calculatedPremiumUsd < contractExtensionPremium.MinValueNett)
                            calculatedPremiumUsd = contractExtensionPremium.MinValueNett;
                        if (calculatedPremiumUsd > contractExtensionPremium.MaxValueNett)
                            calculatedPremiumUsd = contractExtensionPremium.MaxValueNett;
                        response.TotalPremiumNRP = calculatedPremiumUsd * contractExtensionPremium.ConversionRate;
                    }
                    else
                    {
                        response.TotalPremiumNRP = (response.VariantPremiumNRP + response.BasicPremiumNRP) * contractExtensionPremium.ConversionRate;
                    }
                }
                else
                {

                    String iloePremiumDetailsQuery = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\IloePremiumDetails.sql"));
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{contractId}", contractId.ToString());
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{contractInsuranceLimitationId}", extensionId.ToString());
                    iloePremiumDetailsQuery = iloePremiumDetailsQuery.Replace("{conractExtensionPremiumId}", contractPremiumId.ToString());

                    IloePremiumDetailsResponseDto iloePremiumDetails = session.CreateSQLQuery(iloePremiumDetailsQuery).SetResultTransformer(Transformers.AliasToBean<IloePremiumDetailsResponseDto>()).List<IloePremiumDetailsResponseDto>().FirstOrDefault();
                    if (iloePremiumDetails == null)
                        return response.Status = "No premiums foud for selected criterias.";

                    if (iloePremiumDetails.AnnualInterestRate == 0)
                        return response.Status = "Selected Deal's AER is zero. Please contract the Administrator.";

                    if (iloePremiumDetails.MinimumPayment == 0 || iloePremiumDetails.MaximumPayment == 0)
                        return response.Status = "Selected Deal's Per Claim Liability or Total Liability Limitations are zero. Please contract the Administrator.";


                    double loanAmountLocal = (double)DealerPrice;
                    double conversionRate = (double)contractExtensionPremium.ConversionRate;
                    double monthlyInterestRate = (iloePremiumDetails.AnnualInterestRate * 0.1) / 12;
                    //starting calculaton
                    PolicyEntityManager pem = new PolicyEntityManager();
                    object emivalue = pem.GetEMIValue(DealerPrice, contractId);
                    double emiLocal = Math.Abs(Financial.Pmt(monthlyInterestRate, iloePremiumDetails.LoanPeriod, loanAmountLocal));
                    double emiUsd = Convert.ToDouble(emivalue) / conversionRate;

                    double payoutUsd = (emiUsd > iloePremiumDetails.MinimumPayment ? (iloePremiumDetails.MinimumPayment * iloePremiumDetails.IncubationPeriod) : emiUsd * iloePremiumDetails.IncubationPeriod);
                    double nrpPerMonth = payoutUsd * iloePremiumDetails.NRPRate;
                    double nrpTotal = nrpPerMonth * iloePremiumDetails.LoanPeriod;

                    //Change value
                    double grossPerMonth = payoutUsd * (iloePremiumDetails.GrossRate / 100);
                    double grossTotal = grossPerMonth * iloePremiumDetails.LoanPeriod;

                    response.TotalPremium = ToDecimalSafe(grossTotal * conversionRate) + 150;
                    response.TotalPremiumNRP = ToDecimalSafe(nrpTotal * conversionRate);
                }

                //eligibility fee
                IList<Eligibility> eligibilities = session.QueryOver<Eligibility>()
                    .Where(a => a.ContractId == contractId)
                    .List<Eligibility>();
                bool isMandatoryEligibilitySatisfied = false;
                foreach (Eligibility eligibility in eligibilities)
                {
                    var commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);

                    if (commodityCode == "A")
                    {
                        double usedMonths = (double)(policySoldDate - ItemPurchasedDate).Days / 30;
                        if (eligibility.MileageFrom <= usage && eligibility.MileageTo >= usage &&
                            eligibility.AgeFrom <= usedMonths && eligibility.AgeTo >= usedMonths)
                        {
                            if (eligibility.IsPercentage)
                            {
                                response.EligibilityPremium = response.TotalPremium * eligibility.Premium / 100;
                            }
                            else
                            {
                                response.EligibilityPremium = eligibility.Premium *
                                                              contractExtensionPremium.ConversionRate;
                            }
                            if (eligibility.PlusMinus.ToLower() != "plus")
                                response.EligibilityPremium = response.EligibilityPremium * (-1);

                            if (eligibility.isMandatory)
                                isMandatoryEligibilitySatisfied = true;
                        }
                    }
                    else
                    {
                        if (eligibility.MonthsFrom <= usage && eligibility.MonthsTo >= usage)
                        {
                            if (eligibility.IsPercentage)
                            {
                                response.EligibilityPremium = response.TotalPremium * eligibility.Premium / 100;
                            }
                            else
                            {
                                response.EligibilityPremium = eligibility.Premium *
                                                              contractExtensionPremium.ConversionRate;
                            }
                            if (eligibility.PlusMinus.ToLower() != "plus")
                                response.EligibilityPremium = response.EligibilityPremium * (-1);
                            if (eligibility.isMandatory)
                                isMandatoryEligibilitySatisfied = true;
                        }
                    }
                }


                response.TotalPremium += response.EligibilityPremium;
                response.TotalPremiumNRP += response.EligibilityPremium;

                bool isMandatoryEligibilityAvailable = eligibilities.Any(a => a.isMandatory);
                if (isMandatoryEligibilityAvailable && !isMandatoryEligibilitySatisfied)
                    return response.Status = "Mandatory eligibility criterias not satisfied. Please check usage & registration date";


                //tax
                IList<ContractTaxMapping> contractTaxesList = session.QueryOver<ContractTaxMapping>()
                    .Where(a => a.ContractId == contractId).List<ContractTaxMapping>();
                IList<CountryTaxes> countryTaxesList = session.QueryOver<CountryTaxes>()
                   .WhereRestrictionOn(b => b.Id).IsIn(contractTaxesList.Select(a => a.CountryTaxId).ToList())
                   .List<CountryTaxes>();

                var totalTax = decimal.Zero;

                foreach (var countryTax in countryTaxesList.OrderBy(a => a.IndexVal))
                {
                    var currentTax = decimal.Zero;
                    if (countryTax.IsPercentage)
                    {
                        if (countryTax.IsOnGross)
                        {
                            if (countryTax.IsOnPreviousTax)
                            {
                                currentTax = (response.TotalPremium + totalTax) * countryTax.TaxValue / 100;
                            }
                            else
                            {
                                currentTax = (response.TotalPremium) * countryTax.TaxValue / 100;
                            }
                        }
                        else
                        {
                            if (countryTax.IsOnPreviousTax)
                            {
                                currentTax = (response.TotalPremiumNRP + totalTax) * countryTax.TaxValue / 100;
                            }
                            else
                            {
                                currentTax = (response.TotalPremiumNRP) * countryTax.TaxValue / 100;
                            }
                        }
                        if (currentTax < countryTax.MinimumValue * countryTax.ConversionRate)
                            currentTax = countryTax.MinimumValue * countryTax.ConversionRate;

                    }
                    else
                    {
                        currentTax += countryTax.TaxValue * countryTax.ConversionRate;
                    }
                    totalTax += currentTax;
                }
                response.Tax = totalTax;
                response.TotalPremium += totalTax;

                response.TotalPremium = Math.Round(response.TotalPremium * 100) / 100;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        public decimal ToDecimalSafe(double input)
        {
            if (input < (double)decimal.MinValue)
                return decimal.MinValue;
            else if (input > (double)decimal.MaxValue)
                return decimal.MaxValue;
            else
                return (decimal)input;
        }

        public static object GetCoverTypesByAttributeId(Guid attributeSpecId)
        {
            object response = new object();
            try
            {
                CommonEntityManager commonEntityManager = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<ContractExtensionPremium> ContractAttributes = session
                    .Query<ContractExtensionPremium>()
                    .Where(a => a.Id == attributeSpecId);
                response = ContractAttributes.Select(a => new
                {
                    a.Id,
                    WarrantyTypeDescription = commonEntityManager.GetWarrentyTypeNameById(a.WarrentyTypeId)
                }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }




        public static ContractInsuaranceLimitationResponseDto GetContractInsuaranceLimitationsByContractId(Guid ContractId)
        {
            ContractInsuaranceLimitationResponseDto response = new ContractInsuaranceLimitationResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //getting first details for first view

                ContractInsuaranceLimitation contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                    .Where(a => a.ContractId == ContractId).FirstOrDefault();

                response = new ContractInsuaranceLimitationResponseDto()
                {
                    Id = contractInsuaranceLimitation.Id,
                    InsuaranceLimitationId = contractInsuaranceLimitation.InsuaranceLimitationId,
                    ContractId = contractInsuaranceLimitation.ContractId,
                    BaseProductId = contractInsuaranceLimitation.BaseProductId,

                };

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public List<InsuaranceLimitationResponseDto> GetInsuararceLimitation()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<InsuaranceLimitation>().Select(ExtensionType=> new InsuaranceLimitationResponseDto {
                Id = ExtensionType.Id,
                Km = ExtensionType.Km,
                CommodityTypeCode = ExtensionType.CommodityTypeCode,
                CommodityTypeId = ExtensionType.CommodityTypeId,
                EntryBy = ExtensionType.EntryBy,
                InsuaranceLimitationName = ExtensionType.InsuaranceLimitationName,
                IsRsa = ExtensionType.IsRsa,
                EntryDateTime = ExtensionType.EntryDateTime,
                Months = ExtensionType.Months,
                TopOfMW = ExtensionType.TopOfMW
            }).ToList();
        }

        internal static InsuaranceLimitationResponseDto GetInsuaranceLimitationByContractInsuaranceLimitationId(Guid ContractInsuaranceLimitationId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                InsuaranceLimitationResponseDto pDto = new InsuaranceLimitationResponseDto();

                ContractInsuaranceLimitation ContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>().Where(a => a.Id == ContractInsuaranceLimitationId).FirstOrDefault();


                var query =
                from InsuaranceLimitation in session.Query<InsuaranceLimitation>()
                where InsuaranceLimitation.Id == ContractInsuaranceLimitation.InsuaranceLimitationId
                select new { InsuaranceLimitation = InsuaranceLimitation };


                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().InsuaranceLimitation.Id;
                    pDto.CommodityTypeCode = result.First().InsuaranceLimitation.CommodityTypeCode;
                    pDto.CommodityTypeId = result.First().InsuaranceLimitation.CommodityTypeId;
                    pDto.Hrs = result.First().InsuaranceLimitation.Hrs;
                    pDto.InsuaranceLimitationName = result.First().InsuaranceLimitation.InsuaranceLimitationName;
                    pDto.Km = result.First().InsuaranceLimitation.Km;
                    pDto.Months = result.First().InsuaranceLimitation.Months;
                    pDto.TopOfMW = result.First().InsuaranceLimitation.TopOfMW;

                    pDto.IsInsuaranceLimitationyExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsInsuaranceLimitationyExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
    }
}
