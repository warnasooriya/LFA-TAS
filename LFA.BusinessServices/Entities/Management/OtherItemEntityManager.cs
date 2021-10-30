using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class OtherItemEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal bool AddOtherItemDetails(OtherItemRequestDto OtherItemDetails)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                YellowGoodDetails pr = new Entities.YellowGoodDetails();

                pr.Id = new Guid();
                //pr.Id = OtherItemDetails.Id;
                pr.AddnSerialNo = OtherItemDetails.AddnSerialNo;
                pr.CategoryId = OtherItemDetails.CategoryId;
                pr.DealerPrice = OtherItemDetails.DealerPrice;
                pr.InvoiceNo = OtherItemDetails.InvoiceNo;
                pr.MakeId = OtherItemDetails.MakeId;
                pr.ModelCode = OtherItemDetails.ModelCode;
                pr.ModelId = OtherItemDetails.ModelId;
                pr.ModelYear = OtherItemDetails.ModelYear;
                pr.ItemPrice = OtherItemDetails.ItemPrice;
                pr.ItemPurchasedDate = OtherItemDetails.ItemPurchasedDate;
                pr.ItemStatusId = OtherItemDetails.ItemStatusId;
                pr.CommodityUsageTypeId = OtherItemDetails.CommodityUsageTypeId;
                pr.SerialNo = OtherItemDetails.SerialNo;
                pr.EntryDateTime = OtherItemDetails.EntryDateTime;
                pr.EntryUser = OtherItemDetails.EntryUser;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.currencyPeriodId = OtherItemDetails.currencyPeriodId;
                pr.DealerCurrencyId = OtherItemDetails.DealerCurrencyId;
                pr.ConversionRate = OtherItemDetails.ConversionRate;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    OtherItemDetails.Id = pr.Id;
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

        internal static BrownAndWhiteDetailsResponseDto OtherItemDetailsRetrievalById(Guid itemId)
        {
            ISession session = EntitySessionManager.GetSession();
            var currencyEm = new CurrencyEntityManager();

            BrownAndWhiteDetailsResponseDto pDto = new BrownAndWhiteDetailsResponseDto();

            var query =
                from OtherItemDetails in session.Query<OtherItemDetails>()
                where OtherItemDetails.Id == itemId
                select new { OtherItemDetails = OtherItemDetails };

            var result = query.ToList();

            //todo: currency implementation
            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().OtherItemDetails.Id;
                pDto.AddnSerialNo = result.First().OtherItemDetails.AddnSerialNo;
                pDto.CategoryId = result.First().OtherItemDetails.CategoryId;
               // pDto.DealerPrice = result.First().OtherItemDetails.DealerPrice;//currencyEm.ConvertFromBaseCurrency(result.First().YellowGoodDetails.DealerPrice, result.First().BrownAndWhiteDetails.DealerCurrencyId, result.First().BrownAndWhiteDetails.currencyPeriodId);
                pDto.DealerPrice = currencyEm.ConvertFromBaseCurrency(result.First().OtherItemDetails.DealerPrice, result.First().OtherItemDetails.DealerCurrencyId, result.First().OtherItemDetails.currencyPeriodId);
                //pDto.DealerPrice = result.First().BrownAndWhiteDetails.DealerPrice;
                pDto.InvoiceNo = result.First().OtherItemDetails.InvoiceNo;
                pDto.MakeId = result.First().OtherItemDetails.MakeId;
                pDto.ModelCode = result.First().OtherItemDetails.ModelCode;
                pDto.ModelId = result.First().OtherItemDetails.ModelId;
                //  pDto.CountryId = result.First().YellowGoodDetails.CountryId;
                //  pDto.DealerId = result.First().YellowGoodDetails.DealerId;
                pDto.ModelYear = result.First().OtherItemDetails.ModelYear;
                //pDto.ItemPrice = result.First().OtherItemDetails.ItemPrice;//todo: currency implementation
                pDto.ItemPrice = currencyEm.ConvertFromBaseCurrency(result.First().OtherItemDetails.ItemPrice, result.First().OtherItemDetails.DealerCurrencyId, result.First().OtherItemDetails.currencyPeriodId);
                //pDto.ItemPrice = result.First().BrownAndWhiteDetails.ItemPrice;
                pDto.ItemPurchasedDate = result.First().OtherItemDetails.ItemPurchasedDate;
                pDto.ItemStatusId = result.First().OtherItemDetails.ItemStatusId;
                pDto.SerialNo = result.First().OtherItemDetails.SerialNo;
                pDto.EntryDateTime = result.First().OtherItemDetails.EntryDateTime;
                pDto.EntryUser = result.First().OtherItemDetails.EntryUser;
                pDto.CommodityUsageTypeId = result.First().OtherItemDetails.CommodityUsageTypeId;
                
               
                pDto.IsBrownAndWhiteDetailsExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBrownAndWhiteDetailsExists = false;
                return null;
            }
        }

        internal bool UpdateOtherItemDetails(OtherItemRequestDto OtherItemDetails)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                OtherItemDetails pr = new Entities.OtherItemDetails();
                session.Load(pr, OtherItemDetails.Id);

                pr.Id = OtherItemDetails.Id;
                pr.AddnSerialNo = OtherItemDetails.AddnSerialNo;
                pr.CategoryId = OtherItemDetails.CategoryId;
                pr.DealerPrice = OtherItemDetails.DealerPrice;
                pr.InvoiceNo = OtherItemDetails.InvoiceNo;
                pr.MakeId = OtherItemDetails.MakeId;
                pr.ModelCode = OtherItemDetails.ModelCode;
                pr.ModelId = OtherItemDetails.ModelId;
                pr.CommodityUsageTypeId = OtherItemDetails.CommodityUsageTypeId;
                pr.ModelYear = OtherItemDetails.ModelYear;
                pr.ItemPrice = OtherItemDetails.ItemPrice;
                pr.ItemPurchasedDate = OtherItemDetails.ItemPurchasedDate;
                pr.ItemStatusId = OtherItemDetails.ItemStatusId;
                pr.SerialNo = OtherItemDetails.SerialNo;
                pr.VariantId = OtherItemDetails.VariantId;
                pr.EntryDateTime = DateTime.Today;
                pr.EntryUser = OtherItemDetails.EntryUser;

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

        internal object GetAllItemsForSearchGrid(OtherItemSearchGridRequestDto OtherItemSearchGridRequestDto)
        {

            try
            {
                if (OtherItemSearchGridRequestDto == null || OtherItemSearchGridRequestDto.paginationOptionsOtherSearchGrid == null)
                    return null;
                Expression<Func<OtherItemDetails, bool>> filterOtherItems = PredicateBuilder.True<OtherItemDetails>();

                if (!String.IsNullOrEmpty(OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.serialNo))
                {
                    filterOtherItems = filterOtherItems.And(a => a.SerialNo.ToLower().Contains(OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.serialNo.ToLower()));
                }

                if (IsGuid(OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.make.ToString()))
                {
                    filterOtherItems = filterOtherItems.And(a => a.MakeId == OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.make);
                }

                if (IsGuid(OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.model.ToString()))
                {
                    filterOtherItems = filterOtherItems.And(a => a.ModelId == OtherItemSearchGridRequestDto.OtherSearchGridSearchCriterias.model);
                }

                ISession session = EntitySessionManager.GetSession();
                var filterdeBnWdetails = session.Query<OtherItemDetails>().Where(filterOtherItems);
                long TotalRecords = filterdeBnWdetails.Count();
                var BnwDetailsForGrid = filterdeBnWdetails.Skip((OtherItemSearchGridRequestDto.paginationOptionsOtherSearchGrid.pageNumber - 1) * OtherItemSearchGridRequestDto.paginationOptionsOtherSearchGrid.pageSize)
               .Take(OtherItemSearchGridRequestDto.paginationOptionsOtherSearchGrid.pageSize)
               .Select(a => new
               {
                   a.SerialNo,
                   a.Id,
                   Make = GetMakeById(a.MakeId),
                   Model = GetModelById(a.ModelId)
               }).ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = BnwDetailsForGrid
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

        internal static OtherItemDetailsResponseDto OtherDetailRetrievalById(Guid BrownAndWhiteDetailsId)
        {
            ISession session = EntitySessionManager.GetSession();
            var currencyEm = new CurrencyEntityManager();

            OtherItemDetailsResponseDto pDto = new OtherItemDetailsResponseDto();

            var query =
                from OtherItemDetails in session.Query<OtherItemDetails>()
                where OtherItemDetails.Id == BrownAndWhiteDetailsId
                select new { OtherItemDetails = OtherItemDetails };

            var result = query.ToList();

            //todo: currency implementation
            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().OtherItemDetails.Id;
                pDto.AddnSerialNo = result.First().OtherItemDetails.AddnSerialNo;
                pDto.CategoryId = result.First().OtherItemDetails.CategoryId;
                // pDto.DealerPrice = result.First().OtherItemDetails.DealerPrice;//currencyEm.ConvertFromBaseCurrency(result.First().YellowGoodDetails.DealerPrice, result.First().BrownAndWhiteDetails.DealerCurrencyId, result.First().BrownAndWhiteDetails.currencyPeriodId);
                pDto.DealerPrice = currencyEm.ConvertFromBaseCurrency(result.First().OtherItemDetails.DealerPrice, result.First().OtherItemDetails.DealerCurrencyId, result.First().OtherItemDetails.currencyPeriodId);
                //pDto.DealerPrice = result.First().BrownAndWhiteDetails.DealerPrice;
                pDto.InvoiceNo = result.First().OtherItemDetails.InvoiceNo;
                pDto.MakeId = result.First().OtherItemDetails.MakeId;
                pDto.ModelCode = result.First().OtherItemDetails.ModelCode;
                pDto.ModelId = result.First().OtherItemDetails.ModelId;
                //  pDto.CountryId = result.First().YellowGoodDetails.CountryId;
                //  pDto.DealerId = result.First().YellowGoodDetails.DealerId;
                pDto.ModelYear = result.First().OtherItemDetails.ModelYear;
                //pDto.ItemPrice = result.First().OtherItemDetails.ItemPrice;//todo: currency implementation
                pDto.ItemPrice = currencyEm.ConvertFromBaseCurrency(result.First().OtherItemDetails.ItemPrice, result.First().OtherItemDetails.DealerCurrencyId, result.First().OtherItemDetails.currencyPeriodId);
                //pDto.ItemPrice = result.First().BrownAndWhiteDetails.ItemPrice;
                pDto.ItemPurchasedDate = result.First().OtherItemDetails.ItemPurchasedDate;
                pDto.ItemStatusId = result.First().OtherItemDetails.ItemStatusId;
                pDto.SerialNo = result.First().OtherItemDetails.SerialNo;
                pDto.EntryDateTime = result.First().OtherItemDetails.EntryDateTime;
                pDto.EntryUser = result.First().OtherItemDetails.EntryUser;
                pDto.CommodityUsageTypeId = result.First().OtherItemDetails.CommodityUsageTypeId;


                pDto.IsBrownAndWhiteDetailsExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBrownAndWhiteDetailsExists = false;
                return null;
            }
        }
    }
}
