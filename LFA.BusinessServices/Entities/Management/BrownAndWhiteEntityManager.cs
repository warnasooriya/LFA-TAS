using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
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
    public class BrownAndWhiteDetailsEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<BrownAndWhiteDetailsResponseDto> GetBrownAndWhiteDetailss()
        {
            List<BrownAndWhiteDetailsResponseDto> entities = null;
            ISession session = EntitySessionManager.GetSession();
           return  session.Query<BrownAndWhiteDetails>().Select(BrownAndWhiteDetails => new BrownAndWhiteDetailsResponseDto {
               Id = BrownAndWhiteDetails.Id,
               AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo,
               CategoryId = BrownAndWhiteDetails.CategoryId,
               DealerPrice = BrownAndWhiteDetails.DealerPrice,
               InvoiceNo = BrownAndWhiteDetails.InvoiceNo,
               MakeId = BrownAndWhiteDetails.MakeId,
               ModelCode = BrownAndWhiteDetails.ModelCode,
               ModelId = BrownAndWhiteDetails.ModelId,
               CountryId = BrownAndWhiteDetails.CountryId,
               DealerId = BrownAndWhiteDetails.DealerId,
               ModelYear = BrownAndWhiteDetails.ModelYear,
               ItemPrice = BrownAndWhiteDetails.ItemPrice,
               ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate,
               ItemStatusId = BrownAndWhiteDetails.ItemStatusId,
               SerialNo = BrownAndWhiteDetails.SerialNo,
               EntryDateTime = BrownAndWhiteDetails.EntryDateTime,
               EntryUser = BrownAndWhiteDetails.EntryUser,
               CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId
           }).ToList();
        }


        public BrownAndWhiteDetailsResponseDto GetBrownAndWhiteDetailsById(Guid BrownAndWhiteDetailsId)
        {
            ISession session = EntitySessionManager.GetSession();
            var currencyEm = new CurrencyEntityManager();

            BrownAndWhiteDetailsResponseDto pDto = new BrownAndWhiteDetailsResponseDto();

            var query =
                from BrownAndWhiteDetails in session.Query<BrownAndWhiteDetails>()
                where BrownAndWhiteDetails.Id == BrownAndWhiteDetailsId
                select new { BrownAndWhiteDetails = BrownAndWhiteDetails };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().BrownAndWhiteDetails.Id;
                pDto.Id = result.First().BrownAndWhiteDetails.Id;
                pDto.AddnSerialNo = result.First().BrownAndWhiteDetails.AddnSerialNo;
                pDto.CategoryId = result.First().BrownAndWhiteDetails.CategoryId;
                pDto.DealerPrice = currencyEm.ConvertFromBaseCurrency(result.First().BrownAndWhiteDetails.DealerPrice, result.First().BrownAndWhiteDetails.DealerCurrencyId, result.First().BrownAndWhiteDetails.currencyPeriodId);
                //pDto.DealerPrice = result.First().BrownAndWhiteDetails.DealerPrice;
                pDto.InvoiceNo = result.First().BrownAndWhiteDetails.InvoiceNo;
                pDto.MakeId = result.First().BrownAndWhiteDetails.MakeId;
                pDto.ModelCode = result.First().BrownAndWhiteDetails.ModelCode;
                pDto.ModelId = result.First().BrownAndWhiteDetails.ModelId;
                pDto.CountryId = result.First().BrownAndWhiteDetails.CountryId;
                pDto.DealerId = result.First().BrownAndWhiteDetails.DealerId;
                pDto.ModelYear = result.First().BrownAndWhiteDetails.ModelYear;
                pDto.ItemPrice = currencyEm.ConvertFromBaseCurrency(result.First().BrownAndWhiteDetails.ItemPrice, result.First().BrownAndWhiteDetails.DealerCurrencyId, result.First().BrownAndWhiteDetails.currencyPeriodId);
                //pDto.ItemPrice = result.First().BrownAndWhiteDetails.ItemPrice;
                pDto.ItemPurchasedDate = result.First().BrownAndWhiteDetails.ItemPurchasedDate;
                pDto.ItemStatusId = result.First().BrownAndWhiteDetails.ItemStatusId;
                pDto.SerialNo = result.First().BrownAndWhiteDetails.SerialNo;
                pDto.EntryDateTime = result.First().BrownAndWhiteDetails.EntryDateTime;
                pDto.EntryUser = result.First().BrownAndWhiteDetails.EntryUser;
                pDto.CommodityUsageTypeId = result.First().BrownAndWhiteDetails.CommodityUsageTypeId;
                pDto.Variant = result.First().BrownAndWhiteDetails.Variant;
                pDto.DealerCurrencyId = result.First().BrownAndWhiteDetails.DealerCurrencyId;

                Guid currentCurrencyPeriodId = result.First().BrownAndWhiteDetails.currencyPeriodId;
                pDto.currencyPeriodId = currentCurrencyPeriodId;

                decimal ConRate = result.First().BrownAndWhiteDetails.ConversionRate;

                pDto.ConversionRate = currencyEm.ConvertFromBaseCurrency(ConRate, result.First().BrownAndWhiteDetails.DealerCurrencyId, currentCurrencyPeriodId);

                pDto.IsBrownAndWhiteDetailsExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBrownAndWhiteDetailsExists = false;
                return null;
            }
        }


        internal bool AddBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                var currencyEm = new CurrencyEntityManager();
                BrownAndWhiteDetails pr = new Entities.BrownAndWhiteDetails();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                pr.Id = new Guid();
                // pr.Id = BrownAndWhiteDetails.Id;
                pr.AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo;
                pr.CategoryId = BrownAndWhiteDetails.CategoryId;
                pr.DealerPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.DealerPrice, BrownAndWhiteDetails.DealerCurrencyId, currentCurrencyPeriodId);
                pr.InvoiceNo = BrownAndWhiteDetails.InvoiceNo;
                pr.MakeId = BrownAndWhiteDetails.MakeId;
                pr.ModelCode = BrownAndWhiteDetails.ModelCode;
                pr.ModelId = BrownAndWhiteDetails.ModelId;
                pr.CountryId = BrownAndWhiteDetails.CountryId;
                pr.DealerId = BrownAndWhiteDetails.DealerId;
                pr.ModelYear = BrownAndWhiteDetails.ModelYear;
                pr.ItemPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.ItemPrice, BrownAndWhiteDetails.DealerCurrencyId, currentCurrencyPeriodId);
                pr.ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate;
                pr.ItemStatusId = BrownAndWhiteDetails.ItemStatusId;
                pr.CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId;
                pr.SerialNo = BrownAndWhiteDetails.SerialNo;
                pr.EntryUser = BrownAndWhiteDetails.EntryUser;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.DealerCurrencyId = BrownAndWhiteDetails.DealerCurrencyId;
                // BaseCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                pr.currencyPeriodId = currentCurrencyPeriodId;

                pr.ConversionRate = currencyEm.GetConversionRate(BrownAndWhiteDetails.DealerCurrencyId, currentCurrencyPeriodId);
                pr.Variant = BrownAndWhiteDetails.Variant;
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    BrownAndWhiteDetails.Id = pr.Id;
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

        internal bool UpdateBrownAndWhiteDetails(BrownAndWhiteDetailsRequestDto BrownAndWhiteDetails)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                var currencyEm = new CurrencyEntityManager();
                BrownAndWhiteDetails pr = new Entities.BrownAndWhiteDetails();

                CommodityType commodityType = session.Query<CommodityType>().Where(c => c.CommodityTypeId == BrownAndWhiteDetails.CommodityTypeId).FirstOrDefault();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (commodityType.CommodityCode == "O")
                {
                    OtherItemDetails oid = new Entities.OtherItemDetails();
                    session.Evict(oid);
                    session.Load(oid, BrownAndWhiteDetails.Id);
                    Guid dealerCurrencyId = new CommonEntityManager().GetDealerCurrencyIdByDealerId(BrownAndWhiteDetails.DealerId);

                    oid.Id = BrownAndWhiteDetails.Id;
                    oid.AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo;
                    oid.CategoryId =  BrownAndWhiteDetails.CategoryId;
                    oid.CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId;
                    oid.DealerPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.DealerPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    oid.InvoiceNo = BrownAndWhiteDetails.InvoiceNo;
                    oid.ItemPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.ItemPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    oid.ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate;
                    oid.ItemStatusId = BrownAndWhiteDetails.ItemStatusId;
                    oid.MakeId = BrownAndWhiteDetails.MakeId;
                    oid.ModelCode = BrownAndWhiteDetails.ModelCode;
                    oid.ModelId = BrownAndWhiteDetails.ModelId;
                    oid.ModelYear = BrownAndWhiteDetails.ModelYear;
                    oid.SerialNo = BrownAndWhiteDetails.SerialNo;


                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(oid);

                        transaction.Commit();
                    }

                    return true;

                }
                else if (commodityType.CommodityCode == "Y")
                {
                    YellowGoodDetails ygd = new Entities.YellowGoodDetails();
                    session.Evict(ygd);
                    session.Load(ygd, BrownAndWhiteDetails.Id);
                    Guid dealerCurrencyId = new CommonEntityManager().GetDealerCurrencyIdByDealerId(BrownAndWhiteDetails.DealerId);

                    ygd.Id = BrownAndWhiteDetails.Id;
                    ygd.AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo;
                    ygd.CategoryId = BrownAndWhiteDetails.CategoryId;
                    ygd.CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId;
                    ygd.DealerPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.DealerPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    ygd.InvoiceNo = BrownAndWhiteDetails.InvoiceNo;
                    ygd.ItemPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.ItemPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    ygd.ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate;
                    ygd.ItemStatusId = BrownAndWhiteDetails.ItemStatusId;
                    ygd.MakeId = BrownAndWhiteDetails.MakeId;
                    ygd.ModelCode = BrownAndWhiteDetails.ModelCode;
                    ygd.ModelId = BrownAndWhiteDetails.ModelId;
                    ygd.ModelYear = BrownAndWhiteDetails.ModelYear;
                    ygd.SerialNo = BrownAndWhiteDetails.SerialNo;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(ygd);

                        transaction.Commit();
                    }
                    return true;
                }
                else
                {
                    session.Evict(pr);
                    session.Load(pr, BrownAndWhiteDetails.Id);


                    Guid dealerCurrencyId = new CommonEntityManager().GetDealerCurrencyIdByDealerId(BrownAndWhiteDetails.DealerId);


                    pr.Id = BrownAndWhiteDetails.Id;
                    pr.AddnSerialNo = BrownAndWhiteDetails.AddnSerialNo;
                    pr.CategoryId = BrownAndWhiteDetails.CategoryId;
                    //pr.DealerPrice = BrownAndWhiteDetails.DealerPrice;
                    pr.DealerPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.DealerPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    pr.InvoiceNo = BrownAndWhiteDetails.InvoiceNo;
                    pr.MakeId = BrownAndWhiteDetails.MakeId;
                    pr.ModelCode = BrownAndWhiteDetails.ModelCode;
                    pr.ModelId = BrownAndWhiteDetails.ModelId;
                    if (IsGuid(BrownAndWhiteDetails.CommodityUsageTypeId.ToString()))
                        pr.CommodityUsageTypeId = BrownAndWhiteDetails.CommodityUsageTypeId;
                    pr.CountryId = BrownAndWhiteDetails.CountryId;
                    pr.DealerId = BrownAndWhiteDetails.DealerId;
                    pr.ModelYear = BrownAndWhiteDetails.ModelYear;
                    pr.ItemPrice = currencyEm.ConvertToBaseCurrency(BrownAndWhiteDetails.ItemPrice, dealerCurrencyId, currentCurrencyPeriodId);
                    // pr.ItemPrice = BrownAndWhiteDetails.ItemPrice;
                    pr.ItemPurchasedDate = BrownAndWhiteDetails.ItemPurchasedDate;
                    pr.ItemStatusId = BrownAndWhiteDetails.ItemStatusId;
                    pr.SerialNo = BrownAndWhiteDetails.SerialNo;
                    pr.EntryUser = BrownAndWhiteDetails.EntryUser;
                    pr.Variant = BrownAndWhiteDetails.Variant;
                    pr.DealerCurrencyId = BrownAndWhiteDetails.DealerCurrencyId;
                    // BaseCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                    pr.currencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                    pr.ConversionRate = currencyEm.GetConversionRate(dealerCurrencyId, currentCurrencyPeriodId);
                    //pr.ConversionRate = currencyEm.GetConversionRate(BrownAndWhiteDetails.DealerCurrencyId, currencyEm.GetCurrentCurrencyPeriodId());
                    pr.DealerCurrencyId = dealerCurrencyId;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(pr);

                        transaction.Commit();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal static object GetAllItemsForSearchGrid(BnWSearchGridRequestDto BnWSearchGridRequestDto)
        {
            if (BnWSearchGridRequestDto == null || BnWSearchGridRequestDto.paginationOptionsBnWSearchGrid == null)
                return null;
            Expression<Func<BrownAndWhiteDetails, bool>> filterBnW = PredicateBuilder.True<BrownAndWhiteDetails>();

            if (!String.IsNullOrEmpty(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.serialNo))
            {
                filterBnW = filterBnW.And(a => a.SerialNo.ToLower().Contains(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.serialNo.ToLower()));
            }

            if (IsGuid(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.make.ToString()))
            {
                filterBnW = filterBnW.And(a => a.MakeId == BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.make);
            }

            if (IsGuid(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.model.ToString()))
            {
                filterBnW = filterBnW.And(a => a.ModelId == BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.model);
            }

            if (!String.IsNullOrEmpty(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.invoiceno))
            {
                filterBnW = filterBnW.And(a => a.InvoiceNo.ToLower().Contains(BnWSearchGridRequestDto.bnWSearchGridSearchCriterias.invoiceno.ToLower()));
            }

            ISession session = EntitySessionManager.GetSession();
            var filterdeBnWdetails = session.Query<BrownAndWhiteDetails>().Where(filterBnW);
            long TotalRecords = filterdeBnWdetails.Count();
            var BnwDetailsForGrid = filterdeBnWdetails.Skip((BnWSearchGridRequestDto.paginationOptionsBnWSearchGrid.pageNumber - 1) * BnWSearchGridRequestDto.paginationOptionsBnWSearchGrid.pageSize)
           .Take(BnWSearchGridRequestDto.paginationOptionsBnWSearchGrid.pageSize)
           .Select(a => new
           {
               a.SerialNo,
               a.Id,
               Make = GetMakeById(a.MakeId),
               Model = GetModelById(a.ModelId),
               a.InvoiceNo
           }).ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = BnwDetailsForGrid
            };
            return new JavaScriptSerializer().Serialize(response);
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
    }
}
