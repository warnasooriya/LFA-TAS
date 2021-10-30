using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class VehicleDetailsEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //public Guid BaseCurrencyPeriodId;
        public List<VehicleDetails> GetVehicleDetailss()
        {
            List<VehicleDetails> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<VehicleDetails> VehicleDetailsData = session.Query<VehicleDetails>();
            entities = VehicleDetailsData.ToList();
            return entities;
        }

        public VehicleDetailsResponseDto GetVehicleDetailsById(Guid VehicleDetailsId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleDetailsResponseDto pDto = new VehicleDetailsResponseDto();
                var currencyEm = new CurrencyEntityManager();

                var query =
                    from VehicleDetails in session.Query<VehicleDetails>()
                    where VehicleDetails.Id == VehicleDetailsId
                    select new { VehicleDetails = VehicleDetails };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().VehicleDetails.Id;
                    pDto.CommodityUsageTypeId = result.First().VehicleDetails.CommodityUsageTypeId;
                    pDto.BodyTypeId = result.First().VehicleDetails.BodyTypeId;
                    pDto.AspirationId = result.First().VehicleDetails.AspirationId;
                    pDto.CylinderCountId = result.First().VehicleDetails.CylinderCountId;
                    pDto.CategoryId = result.First().VehicleDetails.CategoryId;
                    pDto.EngineCapacityId = result.First().VehicleDetails.EngineCapacityId;
                    pDto.DriveTypeId = result.First().VehicleDetails.DriveTypeId;
                    pDto.MakeId = result.First().VehicleDetails.MakeId;
                    pDto.FuelTypeId = result.First().VehicleDetails.FuelTypeId;
                    pDto.ModelId = result.First().VehicleDetails.ModelId;
                    pDto.CountryId = result.First().VehicleDetails.CountryId;
                    pDto.DealerId = result.First().VehicleDetails.DealerId;
                    pDto.ModelYear = result.First().VehicleDetails.ModelYear;
                    pDto.PlateNo = result.First().VehicleDetails.PlateNo;
                    pDto.ItemPurchasedDate = result.First().VehicleDetails.ItemPurchasedDate;
                    pDto.ItemStatusId = result.First().VehicleDetails.ItemStatusId;
                    // pDto.VehiclePrice =  result.First().VehicleDetails.VehiclePrice;
                    pDto.VehiclePrice = currencyEm.ConvertFromBaseCurrency(result.First().VehicleDetails.VehiclePrice, result.First().VehicleDetails.DealerCurrencyId, result.First().VehicleDetails.currencyPeriodId);
                    //pDto.DealerPrice = result.First().VehicleDetails.DealerPrice;
                    pDto.DealerPrice = currencyEm.ConvertFromBaseCurrency(result.First().VehicleDetails.DealerPrice, result.First().VehicleDetails.DealerCurrencyId, result.First().VehicleDetails.currencyPeriodId);
                    pDto.VINNo = result.First().VehicleDetails.VINNo;
                    pDto.Variant = result.First().VehicleDetails.Variant;
                    pDto.TransmissionId = result.First().VehicleDetails.TransmissionId;
                    pDto.EntryDateTime = result.First().VehicleDetails.EntryDateTime;
                    pDto.DealerCurrencyId = result.First().VehicleDetails.DealerCurrencyId;
                    //pDto.currencyPeriodId = result.First().VehicleDetails.currencyPeriodId;
                    Guid currentCurrencyPeriodId = result.First().VehicleDetails.currencyPeriodId;
                    pDto.currencyPeriodId = currentCurrencyPeriodId;

                    decimal ConRate = result.First().VehicleDetails.ConversionRate;

                    pDto.ConversionRate = currencyEm.ConvertFromBaseCurrency(ConRate, result.First().VehicleDetails.DealerCurrencyId, currentCurrencyPeriodId);

                    pDto.EntryUser = result.First().VehicleDetails.EntryUser;
                    pDto.RegistrationDate = result.First().VehicleDetails.RegistrationDate;
                    pDto.GrossWeight = result.First().VehicleDetails.GrossWeight;
                    pDto.EngineNumber = result.First().VehicleDetails.EngineNumber;

                    pDto.IsVehicleDetailsExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVehicleDetailsExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool CheckMoreThanOneVehicleByWinNo(string vinNo)
        {
            bool isUsed = false;
            ISession session = EntitySessionManager.GetSession();
          int count =  session.Query<VehicleDetails>().Where(a => a.VINNo == vinNo)
                .Join(session.Query<VehiclePolicy>(), b => b.Id, c => c.VehicleId, (b, c) => new { b, c })
                 .Join(session.Query<Policy>(), d => d.c.PolicyId, e => e.Id, (d, e) => new { d, e })
                 .Where(w => w.e.IsApproved == true && w.e.IsPolicyCanceled == false  && w.e.IsPolicyRenewed ==false)
                 .Count();
            if (count > 0) {
                isUsed = true;
            }
            return isUsed;
        }

        internal List<VehicleDetailsResponseDto> GetVehicleDetailsOptimized()
        {
            List<VehicleDetailsResponseDto> vehicleDetailsResponsesList = new List<VehicleDetailsResponseDto>();
            List<VehicleDetails> VehicleDetailsEntities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<VehicleDetails> VehicleDetailsData = session.Query<VehicleDetails>();
            VehicleDetailsEntities = VehicleDetailsData.ToList();

            CurrencyEntityManager CurrencyEm = new CurrencyEntityManager();
            List<CurrencyConversionUtilDto> currencyDealerList = VehicleDetailsEntities.Select(a => new { DealerCurrencyId = a.DealerCurrencyId, currencyPeriodId = a.currencyPeriodId })
                .Distinct().ToList().Select(ss => new CurrencyConversionUtilDto { currencyPeriodId = ss.currencyPeriodId, DealerCurrencyId = ss.DealerCurrencyId }).ToList();
            List<CurrencyConversionUtilDto> ccList = CurrencyEm.getConversionRatesForMultipleCIds(currencyDealerList);

            foreach (var VehicleDetails in VehicleDetailsEntities)
            {
                VehicleDetailsResponseDto pr = new VehicleDetailsResponseDto();

                pr.Id = VehicleDetails.Id;
                pr.CommodityUsageTypeId = VehicleDetails.CommodityUsageTypeId;
                pr.BodyTypeId = VehicleDetails.BodyTypeId;
                pr.AspirationId = VehicleDetails.AspirationId;
                pr.CylinderCountId = VehicleDetails.CylinderCountId;
                pr.CategoryId = VehicleDetails.CategoryId;
                pr.EngineCapacityId = VehicleDetails.EngineCapacityId;
                pr.DriveTypeId = VehicleDetails.DriveTypeId;
                pr.MakeId = VehicleDetails.MakeId;
                pr.FuelTypeId = VehicleDetails.FuelTypeId;
                pr.ModelId = VehicleDetails.ModelId;
                pr.ModelYear = VehicleDetails.ModelYear;
                pr.PlateNo = VehicleDetails.PlateNo;
                pr.ItemPurchasedDate = VehicleDetails.ItemPurchasedDate;
                pr.ItemStatusId = VehicleDetails.ItemStatusId;
                pr.VehiclePrice = CurrencyEm.ConvertFromBaseCurrencyMultiple(VehicleDetails.VehiclePrice, VehicleDetails.DealerCurrencyId, VehicleDetails.currencyPeriodId, ccList);
                pr.DealerPrice = CurrencyEm.ConvertFromBaseCurrencyMultiple(VehicleDetails.DealerPrice, VehicleDetails.DealerCurrencyId, VehicleDetails.currencyPeriodId, ccList);
                pr.VINNo = VehicleDetails.VINNo;
                pr.Variant = VehicleDetails.Variant;
                pr.EntryDateTime = VehicleDetails.EntryDateTime;
                pr.EntryUser = VehicleDetails.EntryUser;
                pr.CountryId = VehicleDetails.CountryId;
                pr.DealerId = VehicleDetails.DealerId;
                pr.DealerCurrencyId = VehicleDetails.DealerCurrencyId;
                pr.EngineNumber = VehicleDetails.EngineNumber;
                //pr.DealerCurrencyId = VehicleDetails.DealerCurrencyId;
                //need to write other fields
                vehicleDetailsResponsesList.Add(pr);
            }
            return vehicleDetailsResponsesList;
        }

        public VehicleDetailsResponseDto GetVehicleDetailsByVin(string VinNo)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleDetailsResponseDto pDto = new VehicleDetailsResponseDto();
                var currencyEm = new CurrencyEntityManager();

                var query =
                    from VehicleDetails in session.Query<VehicleDetails>()
                    where VehicleDetails.VINNo == VinNo
                    select new { VehicleDetails = VehicleDetails };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().VehicleDetails.Id;
                    pDto.CommodityUsageTypeId = result.First().VehicleDetails.CommodityUsageTypeId;
                    pDto.BodyTypeId = result.First().VehicleDetails.BodyTypeId;
                    pDto.AspirationId = result.First().VehicleDetails.AspirationId;
                    pDto.CylinderCountId = result.First().VehicleDetails.CylinderCountId;
                    pDto.CategoryId = result.First().VehicleDetails.CategoryId;
                    pDto.EngineCapacityId = result.First().VehicleDetails.EngineCapacityId;
                    pDto.DriveTypeId = result.First().VehicleDetails.DriveTypeId;
                    pDto.MakeId = result.First().VehicleDetails.MakeId;
                    pDto.FuelTypeId = result.First().VehicleDetails.FuelTypeId;
                    pDto.ModelId = result.First().VehicleDetails.ModelId;
                    pDto.CountryId = result.First().VehicleDetails.CountryId;
                    pDto.DealerId = result.First().VehicleDetails.DealerId;
                    pDto.ModelYear = result.First().VehicleDetails.ModelYear;
                    pDto.PlateNo = result.First().VehicleDetails.PlateNo;
                    pDto.ItemPurchasedDate = result.First().VehicleDetails.ItemPurchasedDate;
                    pDto.ItemStatusId = result.First().VehicleDetails.ItemStatusId;
                    // pDto.VehiclePrice =  result.First().VehicleDetails.VehiclePrice;
                    pDto.VehiclePrice = currencyEm.ConvertFromBaseCurrency(result.First().VehicleDetails.VehiclePrice, result.First().VehicleDetails.DealerCurrencyId, result.First().VehicleDetails.currencyPeriodId);
                    //pDto.DealerPrice = result.First().VehicleDetails.DealerPrice;
                    pDto.DealerPrice = currencyEm.ConvertFromBaseCurrency(result.First().VehicleDetails.DealerPrice, result.First().VehicleDetails.DealerCurrencyId, result.First().VehicleDetails.currencyPeriodId);
                    pDto.VINNo = result.First().VehicleDetails.VINNo;
                    pDto.Variant = result.First().VehicleDetails.Variant;
                    pDto.TransmissionId = result.First().VehicleDetails.TransmissionId;
                    pDto.EntryDateTime = result.First().VehicleDetails.EntryDateTime;
                    pDto.DealerCurrencyId = result.First().VehicleDetails.DealerCurrencyId;
                    //pDto.currencyPeriodId = result.First().VehicleDetails.currencyPeriodId;
                    Guid currentCurrencyPeriodId = result.First().VehicleDetails.currencyPeriodId;
                    pDto.currencyPeriodId = currentCurrencyPeriodId;

                    decimal ConRate = result.First().VehicleDetails.ConversionRate;

                    pDto.ConversionRate = currencyEm.ConvertFromBaseCurrency(ConRate, result.First().VehicleDetails.DealerCurrencyId, currentCurrencyPeriodId);

                    pDto.EntryUser = result.First().VehicleDetails.EntryUser;
                    pDto.RegistrationDate = result.First().VehicleDetails.RegistrationDate;
                    pDto.GrossWeight = result.First().VehicleDetails.GrossWeight;
                    pDto.EngineNumber = result.First().VehicleDetails.EngineNumber;

                    pDto.IsVehicleDetailsExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVehicleDetailsExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddVehicleDetails(VehicleDetailsRequestDto VehicleDetails)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                var currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                Dealer dealer = session.Query<Dealer>().Where(a => a.Id == VehicleDetails.DealerId).FirstOrDefault();
                VehicleDetails pr = new Entities.VehicleDetails();

                pr.Id = new Guid();
                pr.BodyTypeId = VehicleDetails.BodyTypeId;
                pr.CommodityUsageTypeId = VehicleDetails.CommodityUsageTypeId;
                pr.AspirationId = VehicleDetails.AspirationId;
                pr.CylinderCountId = VehicleDetails.CylinderCountId;
                pr.CategoryId = VehicleDetails.CategoryId;
                pr.EngineCapacityId = VehicleDetails.EngineCapacityId;
                pr.DriveTypeId = VehicleDetails.DriveTypeId;
                pr.MakeId = VehicleDetails.MakeId;
                pr.FuelTypeId = VehicleDetails.FuelTypeId;
                pr.ModelId = VehicleDetails.ModelId;
                pr.CountryId = VehicleDetails.CountryId;
                pr.DealerId = VehicleDetails.DealerId;
                pr.ModelYear = VehicleDetails.ModelYear;
                pr.PlateNo = VehicleDetails.PlateNo;
                pr.ItemPurchasedDate = VehicleDetails.ItemPurchasedDate;
                pr.ItemStatusId = VehicleDetails.ItemStatusId;
                pr.VehiclePrice = currencyEm.ConvertToBaseCurrency(VehicleDetails.VehiclePrice, dealer.CurrencyId, currentCurrencyPeriodId);
                //pr.VehiclePrice = VehicleDetails.VehiclePrice;
                pr.DealerPrice = currencyEm.ConvertToBaseCurrency(VehicleDetails.DealerPrice, dealer.CurrencyId, currentCurrencyPeriodId);
                //pr.DealerPrice = VehicleDetails.DealerPrice;
                pr.VINNo = VehicleDetails.VINNo;
                pr.Variant = VehicleDetails.Variant;
                pr.AspirationId = VehicleDetails.AspirationId;
                pr.TransmissionId = VehicleDetails.TransmissionId;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.DealerCurrencyId = VehicleDetails.DealerCurrencyId;
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.currencyPeriodId = VehicleDetails.currencyPeriodId;
                pr.DealerCurrencyId = VehicleDetails.DealerCurrencyId;
                pr.currencyPeriodId = currentCurrencyPeriodId;
                pr.RegistrationDate = VehicleDetails.RegistrationDate;
                pr.GrossWeight = VehicleDetails.GrossWeight;
                pr.EngineNumber = VehicleDetails.EngineNumber;

                pr.ConversionRate = currencyEm.GetConversionRate(dealer.CurrencyId, currentCurrencyPeriodId);

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    VehicleDetails.Id = pr.Id;
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

        internal bool UpdateVehicleDetails(VehicleDetailsRequestDto VehicleDetails)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                var currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                VehicleDetails pr = new Entities.VehicleDetails();
                session.Load(pr, VehicleDetails.Id);
                pr.Id = VehicleDetails.Id;
                if (IsGuid(VehicleDetails.CommodityUsageTypeId.ToString()))
                {
                    pr.CommodityUsageTypeId = VehicleDetails.CommodityUsageTypeId;
                }
                pr.BodyTypeId = VehicleDetails.BodyTypeId;
                pr.AspirationId = VehicleDetails.AspirationId;
                pr.CylinderCountId = VehicleDetails.CylinderCountId;
                pr.CategoryId = VehicleDetails.CategoryId;
                pr.EngineCapacityId = VehicleDetails.EngineCapacityId;
                pr.DriveTypeId = VehicleDetails.DriveTypeId;
                pr.MakeId = VehicleDetails.MakeId;
                pr.FuelTypeId = VehicleDetails.FuelTypeId;
                pr.ModelId = VehicleDetails.ModelId;
                pr.DealerId = VehicleDetails.DealerId;
                pr.CountryId = VehicleDetails.CountryId;
                pr.ModelYear = VehicleDetails.ModelYear;
                pr.PlateNo = VehicleDetails.PlateNo;
                pr.ItemPurchasedDate = VehicleDetails.ItemPurchasedDate;
                pr.ItemStatusId = VehicleDetails.ItemStatusId;
                pr.VehiclePrice = currencyEm.ConvertToBaseCurrency(VehicleDetails.VehiclePrice, VehicleDetails.DealerCurrencyId, currentCurrencyPeriodId);
                //pr.VehiclePrice = VehicleDetails.VehiclePrice;
                pr.DealerPrice = currencyEm.ConvertToBaseCurrency(VehicleDetails.DealerPrice, VehicleDetails.DealerCurrencyId, currentCurrencyPeriodId);
                pr.VINNo = VehicleDetails.VINNo;
                pr.Variant = VehicleDetails.Variant;
                //pr.EntryDateTime = VehicleDetails.EntryDateTime;
                pr.AspirationId = VehicleDetails.AspirationId;
                pr.TransmissionId = VehicleDetails.TransmissionId;
                pr.EntryUser = VehicleDetails.EntryUser;
                //Guid BaseCurrencyId = VehicleDetails.DealerCurrencyId;
                pr.DealerCurrencyId = VehicleDetails.DealerCurrencyId;
                pr.currencyPeriodId = currentCurrencyPeriodId;
                if (VehicleDetails.RegistrationDate == DateTime.MinValue)
                    VehicleDetails.RegistrationDate = SqlDateTime.MinValue.Value;
                pr.RegistrationDate = VehicleDetails.RegistrationDate;
                pr.GrossWeight = VehicleDetails.GrossWeight;
                pr.EngineNumber = VehicleDetails.EngineNumber;

                pr.ConversionRate = currencyEm.GetConversionRate(VehicleDetails.DealerCurrencyId, currentCurrencyPeriodId);

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

        internal static object GetAllVehiclesForSearchGrid(VehicleSearchGridRequestDto VehicleSearchGridRequestDto)
        {

            try
            {
                if (VehicleSearchGridRequestDto == null || VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid == null)
                    return null;
                Expression<Func<VehicleDetails, bool>> filterVehicle = PredicateBuilder.True<VehicleDetails>();

                if (!String.IsNullOrEmpty(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.plateNo))
                {
                    filterVehicle = filterVehicle.And(a => a.PlateNo.ToLower().Contains(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.plateNo.ToLower()));
                }

                if (!String.IsNullOrEmpty(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.vinNo))
                {
                    filterVehicle = filterVehicle.And(a => a.VINNo.ToLower().Contains(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.vinNo.ToLower()));
                }

                if (IsGuid(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.modelid.ToString()))
                {
                    filterVehicle = filterVehicle.And(a => a.ModelId == VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.modelid);
                }

                if (IsGuid(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.makeid.ToString()))
                {
                    filterVehicle = filterVehicle.And(a => a.MakeId == VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.makeid);
                }

                ISession session = EntitySessionManager.GetSession();
                var FilterdVehicleDetails = session.Query<VehicleDetails>().Where(filterVehicle);
                long TotalRecords = FilterdVehicleDetails.Count();
                var VehicleDetailsForGrid = FilterdVehicleDetails.Skip((VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageNumber - 1) * VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageSize)
                .Take(VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageSize)
                .Select(a => new
                {
                    a.VINNo,
                    Model = GetModelById(a.ModelId),
                    Make = GetMakeById(a.MakeId),
                    a.Id,
                    a.PlateNo
                }).ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = VehicleDetailsForGrid
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        private static object GetModelById(Guid modelId)
        {
            if (!IsGuid(modelId.ToString()))
                return "";
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Model>().Where(a => a.Id == modelId).FirstOrDefault().ModelName;
        }

        private static object GetMakeById(Guid makeId)
        {
            if (!IsGuid(makeId.ToString()))
                return "";
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Make>().Where(a => a.Id == makeId).FirstOrDefault().MakeName;
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



        internal static string ValidateDealerCurrency(Guid dealerId)
        {
            string response = "ok";
            try
            {
                if (!IsGuid(dealerId.ToString()))
                {
                    response = "Invalid Dealer selection.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                Dealer dealer = session.Query<Dealer>()
                    .Where(a => a.Id == dealerId).FirstOrDefault();
                if (dealer == null)
                {
                    response = "Invalid Dealer selection.";
                    return response;
                }

                if (!IsGuid(dealer.CurrencyId.ToString()))
                {
                    response = "No Currency is assigned to Dealer.";
                    return response;
                }


                Guid currencyPeriodId = new CurrencyEntityManager().GetCurrentCurrencyPeriodId();
                if (!IsGuid(currencyPeriodId.ToString()))
                {
                    response = "No Currency period is defined for today.Please defind a Currency period first.";
                    return response;
                }

                string currencyCode = session.Query<Currency>()
                   .Where(a => a.Id == dealer.CurrencyId).FirstOrDefault().Code;

                CurrencyConversions conversions = session.Query<CurrencyConversions>()
                    .Where(a => a.CurrencyId == dealer.CurrencyId && a.CurrencyConversionPeriodId == currencyPeriodId).FirstOrDefault();
                if (conversions == null)
                {
                    response = "Dealer Currency (" + currencyCode + ") is not found on current Currency conversion period.";
                    return response;
                }

            }
            catch (Exception ex)
            {
                response = "Error occured in Currency validation.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllVehiclesForSearchGridByDealerId(VehicleSearchGridRequestDto VehicleSearchGridRequestDto)
        {
            try
            {
                if (VehicleSearchGridRequestDto == null || VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid == null)
                    return null;
                Expression<Func<VehicleDetails, bool>> filterVehicle = PredicateBuilder.True<VehicleDetails>();
                filterVehicle = filterVehicle.And(a => a.DealerId == VehicleSearchGridRequestDto.dealerId);

                if (!String.IsNullOrEmpty(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.plateNo))
                {
                    filterVehicle = filterVehicle.And(a => a.PlateNo.ToLower().Contains(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.plateNo.ToLower()));
                }

                if (!String.IsNullOrEmpty(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.vinNo))
                {
                    filterVehicle = filterVehicle.And(a => a.VINNo.ToLower().Contains(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.vinNo.ToLower()));
                }

                if (IsGuid(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.modelid.ToString()))
                {
                    filterVehicle = filterVehicle.And(a => a.ModelId == VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.modelid);
                }

                if (IsGuid(VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.makeid.ToString()))
                {
                    filterVehicle = filterVehicle.And(a => a.MakeId == VehicleSearchGridRequestDto.vehicalSearchGridSearchCriterias.makeid);
                }

                ISession session = EntitySessionManager.GetSession();
                var FilterdVehicleDetails = session.Query<VehicleDetails>().Where(filterVehicle);
                long TotalRecords = FilterdVehicleDetails.Count();
                var VehicleDetailsForGrid = FilterdVehicleDetails.Skip((VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageNumber - 1) * VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageSize)
                .Take(VehicleSearchGridRequestDto.paginationOptionsVehicleSearchGrid.pageSize)
                .Select(a => new
                {
                    a.VINNo,
                    Model = GetModelById(a.ModelId),
                    Make = GetMakeById(a.MakeId),
                    a.Id,
                    a.PlateNo
                }).ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = VehicleDetailsForGrid
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }
    }
}
