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
    public class YellowGoodEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal object GetAllItemsForSearchGrid(YellowGoodSearchGridRequestDto YellowGoodSearchGridRequestDto)
        {
            try
            {
                if (YellowGoodSearchGridRequestDto == null || YellowGoodSearchGridRequestDto.paginationOptionsYellowGoodsSearchGrid == null)
                    return null;
                Expression<Func<YellowGoodDetails, bool>> filterYellowGoods = PredicateBuilder.True<YellowGoodDetails>();

                if (!String.IsNullOrEmpty(YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.serialNo))
                {
                    filterYellowGoods = filterYellowGoods.And(a => a.SerialNo.ToLower().Contains(YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.serialNo.ToLower()));
                }

                if (IsGuid(YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.make.ToString()))
                {
                    filterYellowGoods = filterYellowGoods.And(a => a.MakeId == YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.make);
                }

                if (IsGuid(YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.model.ToString()))
                {
                    filterYellowGoods = filterYellowGoods.And(a => a.ModelId == YellowGoodSearchGridRequestDto.YellowGoodSearchGridSearchCriterias.model);
                }

                ISession session = EntitySessionManager.GetSession();
                var filterdeYellowGooddetails = session.Query<YellowGoodDetails>().Where(filterYellowGoods);
                long TotalRecords = filterdeYellowGooddetails.Count();
                var YellowGoodDetailsForGrid = filterdeYellowGooddetails.Skip((YellowGoodSearchGridRequestDto.paginationOptionsYellowGoodsSearchGrid.pageNumber - 1) * YellowGoodSearchGridRequestDto.paginationOptionsYellowGoodsSearchGrid.pageSize)
               .Take(YellowGoodSearchGridRequestDto.paginationOptionsYellowGoodsSearchGrid.pageSize)
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
                    data = YellowGoodDetailsForGrid
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal static BrownAndWhiteDetailsResponseDto YellowGoodDetailRetrievalById(Guid yellowGoodId)
        {
            ISession session = EntitySessionManager.GetSession();
            var currencyEm = new CurrencyEntityManager();

            BrownAndWhiteDetailsResponseDto pDto = new BrownAndWhiteDetailsResponseDto();

            var query =
                from YellowGoodDetails in session.Query<YellowGoodDetails>()
                where YellowGoodDetails.Id == yellowGoodId
                select new { YellowGoodDetails = YellowGoodDetails };

            var result = query.ToList();

            //todo: currency implementation
            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().YellowGoodDetails.Id;
                pDto.Id = result.First().YellowGoodDetails.Id;
                pDto.AddnSerialNo = result.First().YellowGoodDetails.AddnSerialNo;
                pDto.CategoryId = result.First().YellowGoodDetails.CategoryId;
                pDto.DealerPrice = result.First().YellowGoodDetails.DealerPrice;//currencyEm.ConvertFromBaseCurrency(result.First().YellowGoodDetails.DealerPrice, result.First().BrownAndWhiteDetails.DealerCurrencyId, result.First().BrownAndWhiteDetails.currencyPeriodId);
                //pDto.DealerPrice = result.First().BrownAndWhiteDetails.DealerPrice;
                pDto.InvoiceNo = result.First().YellowGoodDetails.InvoiceNo;
                pDto.MakeId = result.First().YellowGoodDetails.MakeId;
                pDto.ModelCode = result.First().YellowGoodDetails.ModelCode;
                pDto.ModelId = result.First().YellowGoodDetails.ModelId;
              //  pDto.CountryId = result.First().YellowGoodDetails.CountryId;
              //  pDto.DealerId = result.First().YellowGoodDetails.DealerId;
                pDto.ModelYear = result.First().YellowGoodDetails.ModelYear;
                pDto.ItemPrice = result.First().YellowGoodDetails.ItemPrice;//todo: currency implementation
                //pDto.ItemPrice = result.First().BrownAndWhiteDetails.ItemPrice;
                pDto.ItemPurchasedDate = result.First().YellowGoodDetails.ItemPurchasedDate;
                pDto.ItemStatusId = result.First().YellowGoodDetails.ItemStatusId;
                pDto.SerialNo = result.First().YellowGoodDetails.SerialNo;
                pDto.EntryDateTime = result.First().YellowGoodDetails.EntryDateTime;
                pDto.EntryUser = result.First().YellowGoodDetails.EntryUser;
                pDto.CommodityUsageTypeId = result.First().YellowGoodDetails.CommodityUsageTypeId;
              //  pDto.Variant = result.First().YellowGoodDetails.Variant;
              //  pDto.DealerCurrencyId = result.First().YellowGoodDetails.DealerCurrencyId;

             //   Guid currentCurrencyPeriodId = result.First().YellowGoodDetails.currencyPeriodId;
               // pDto.currencyPeriodId = currentCurrencyPeriodId;

              //  decimal ConRate = result.First().YellowGoodDetails.ConversionRate;

              //  pDto.ConversionRate = result.First().YellowGoodDetails.DealerPrice; //todo: currency implementation

                pDto.IsBrownAndWhiteDetailsExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBrownAndWhiteDetailsExists = false;
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
    }
}
