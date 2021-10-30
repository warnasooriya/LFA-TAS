using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Enums;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class IncurredErningProcessEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<string> GetUNWYears()
        {
            ISession session = EntitySessionManager.GetSession();
            var entities = session.Query<ReinsurerContract>().Select(a => a.UWYear).Distinct().ToList();
            return entities;
        }

        public IncurredErningProcessResponseDto IncurredErningProcess(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId, DateTime BordxStartDate, DateTime BordxEndDate, DateTime ErnedDate, DateTime ClaimedDate, DateTime earnedDate)
        {
            ISession session = EntitySessionManager.GetSession();
            IncurredErningProcessResponseDto res = new IncurredErningProcessResponseDto();
            res.IncurredErnings = new List<IncurredErningResponseDto>();
            List<IncurredErningResponseDto> TemPList = new List<IncurredErningResponseDto>();

            Guid cId = CountryId;
            if (!IsGuid(MakeId.ToString()))
            {

                MakeId = Guid.Empty;

            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(ModelId.ToString()))
            {

                ModelId = Guid.Empty;

            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(CylindercountId.ToString()))
            {

                CylindercountId = Guid.Empty;
            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(EngineCapacityId.ToString()))
            {

                EngineCapacityId = Guid.Empty;
            }
            else
            {
                CountryId = Guid.Empty;

            }

            if (ErnedDate == DateTime.MinValue) {
                ErnedDate = DateTime.UtcNow;
            }



            if (IsGuid(CountryId.ToString())) {

                try
                {
                    string bordxFilter = " ";
                    if (BordxStartDate != DateTime.MinValue || BordxEndDate != DateTime.MinValue)
                    {
                        bordxFilter = " AND Bordx.StartDate='" + BordxStartDate.ToString("yyyy-MM-dd") + "' and Bordx.EndDate='" + BordxEndDate.ToString("yyyy-MM-dd") + "'  ";
                    }

                    var Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\IncurredProcessMain.sql"));
                    Query = Query.Replace("{CountryId}", CountryId.ToString());
                    Query = Query.Replace("{dealerId}", Dealer.ToString());
                    Query = Query.Replace("{UWYear}", UNWyear);
                    Query = Query.Replace("{EarnedDate}", ErnedDate.ToString());
                    Query = Query.Replace("{BordxFilter}", bordxFilter);

                    res.IncurredErnings = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<IncurredErningResponseDto>())
                  .List<IncurredErningResponseDto>().ToList();


                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }


                return res;

            }
            else
            {
                try
                {
                     res = new IncurredErningProcessResponseDto();

                    string dateString = earnedDate.ToString("MM/dd/yyyy");
                    DateTime value = new DateTime(0001, 01, 01);
                    string dateString2 = value.ToString("MM/dd/yyyy");

                    if (dateString == dateString2)
                    {

                        earnedDate = DateTime.Today;

                    }

                    var Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\IncurredProcess.sql"));
                    Query = Query.Replace("{CountryId}", cId.ToString());
                    Query = Query.Replace("{DealerId}", Dealer.ToString());
                    Query = Query.Replace("{UWYear}", UNWyear);
                    Query = Query.Replace("{MakeId}", MakeId.ToString());
                    Query = Query.Replace("{ModelId}", ModelId.ToString());
                    Query = Query.Replace("{CylinderCountId}", CylindercountId.ToString());
                    Query = Query.Replace("{BordxStartDate}", BordxStartDate.ToString());
                    Query = Query.Replace("{BordxEndDate}", BordxEndDate.ToString());
                    Query = Query.Replace("{EngineCapacityId}", EngineCapacityId.ToString());
                    Query = Query.Replace("{EarnedDate}", earnedDate.ToString());

                    res.IncurredErnings = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<IncurredErningResponseDto>())
                  .List<IncurredErningResponseDto>().ToList();

                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }


                return res;
            }

        }

        public decimal getErnPercent(DateTime polStrDte,DateTime polEndDte, DateTime earnedDate ) {
            decimal res = 0;

            if (earnedDate < polStrDte)
            {
                res = 0;
            }
            else if (polStrDte < earnedDate && earnedDate < polEndDte)
            {
                res = (Convert.ToDecimal((earnedDate - polStrDte).TotalDays) / Convert.ToDecimal((polEndDte - polStrDte).TotalDays)) * 100;
            }
            else if (earnedDate > polEndDte)
            {
                res = 100;
            }
            //res = (Convert.ToDecimal((DateTime.Now.Date - polStrDte).TotalDays) / Convert.ToDecimal((polEndDte - polStrDte).TotalDays)) * premium;
            return Math.Round(res, 2);
        }

        public string getpolicyStatus(Guid policyId)
        {
            ISession session = EntitySessionManager.GetSession();
            string Res;
            var query =
                from Vpolicy in session.Query<VehiclePolicy>()
                join VehicleDetail in session.Query<VehicleDetails>() on Vpolicy.VehicleId equals VehicleDetail.Id
                join ItemState in session.Query<ItemStatus>() on VehicleDetail.ItemStatusId equals ItemState.Id
                where Vpolicy.PolicyId == policyId
                select new
               {
                   PState = ItemState.Status
               };

            Res = query != null ? query.FirstOrDefault().PState != null ? query.FirstOrDefault().PState.ToString() :null:null;

            if (Res != null)
            {
                return Res;
            }

            var query1 =
                from YGpolicy in session.Query<YellowGoodPolicy>()
                join YGDetail in session.Query<YellowGoodDetails>() on YGpolicy.YellowGoodId equals YGDetail.Id
                join ItemState in session.Query<ItemStatus>() on YGDetail.ItemStatusId equals ItemState.Id
                where YGpolicy.PolicyId == policyId
                select new
                {
                    PState = ItemState.Status
                };

            Res = query1 != null ? query1.FirstOrDefault().PState != null ? query1.FirstOrDefault().PState.ToString() : null : null;

            if (Res != null)
            {
                return Res;
            }

            var query2 =
                from BWpolicy in session.Query<BAndWPolicy>()
                join BWDetail in session.Query<BrownAndWhiteDetails>() on BWpolicy.BAndWId equals BWDetail.Id
                join ItemState in session.Query<ItemStatus>() on BWDetail.ItemStatusId equals ItemState.Id
                where BWpolicy.PolicyId == policyId
                select new
                {
                    PState = ItemState.Status
                };

            Res = query2 != null ? query2.FirstOrDefault().PState != null ? query2.FirstOrDefault().PState.ToString() : null : null;

            if (Res != null)
            {
                return Res;
            }

            var query3 =
                from OIpolicy in session.Query<OtherItemPolicy>()
                join OIDetail in session.Query<OtherItemDetails>() on OIpolicy.OtherItemId equals OIDetail.Id
                join ItemState in session.Query<ItemStatus>() on OIDetail.ItemStatusId equals ItemState.Id
                where OIpolicy.PolicyId == policyId
                select new
                {
                    PState = ItemState.Status
                };

            Res = query3 != null ? query3.FirstOrDefault().PState != null ? query3.FirstOrDefault().PState.ToString() : null : null;

            if (Res != null)
            {
                return Res;
            }
            else
            {
                return "";
            }

        }

        public IncurredErningExportResponseDto GetIncurredErningExport(string UNWyear, Guid CountryId, Guid Dealer, Guid Reinsurer, Guid Insurer, Guid MakeId, Guid ModelId, Guid CylindercountId, Guid EngineCapacityId, Guid InsuaranceLimitationId, DateTime BordxStartDate, DateTime BordxEndDate, DateTime ErnedDate, DateTime ClaimDate)
        {
            ISession session = EntitySessionManager.GetSession();

            Guid cid = CountryId;

            if (!IsGuid(MakeId.ToString()))
            {

                MakeId = Guid.Empty;

            }
            else {
                CountryId= Guid.Empty;

            }
            if (!IsGuid(ModelId.ToString()))
            {

                ModelId = Guid.Empty;

            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(CylindercountId.ToString()))
            {

                CylindercountId = Guid.Empty;
            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(EngineCapacityId.ToString()))
            {

                EngineCapacityId = Guid.Empty;
            }
            else
            {
                CountryId = Guid.Empty;

            }
            if (!IsGuid(InsuaranceLimitationId.ToString()))
            {

                InsuaranceLimitationId = Guid.Empty;
            }
            else
            {
                CountryId = Guid.Empty;

            }




            IncurredErningExportResponseDto result = null;



            if (IsGuid(CountryId.ToString()))
            {
                try
                {
                    string bordxFilter = " ";
                    if (BordxStartDate != DateTime.MinValue || BordxEndDate != DateTime.MinValue)
                    {
                        bordxFilter = " AND b.StartDate='" + BordxStartDate.ToString("yyyy-MM-dd") + "' and b.EndDate='" + BordxEndDate.ToString("yyyy-MM-dd") + "'  ";
                    }



                    result = new IncurredErningExportResponseDto();

                    var TpaName = session.Query<TPA>().FirstOrDefault().Name;
                    var TpaLogo = session.Query<TPA>().FirstOrDefault().Logo;
                    var image = new ImageEntityManager().GetImageById(TpaLogo);

                    String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\LostRatio.sql"));
                    Query = Query.Replace("{DealerId}", Dealer.ToString());
                    Query = Query.Replace("{CountryId}", CountryId.ToString());
                    Query = Query.Replace("{UWYear}", UNWyear.ToString());
                    Query = Query.Replace("{EarnedDate}", ErnedDate.ToString("yyyy-MM-dd 00:00:00.000"));
                    Query = Query.Replace("{BordxFilter}", bordxFilter);
                    Query = Query.Replace("{ClaimDate}", ClaimDate.ToString("yyyy-MM-dd"));
                    var LostRatioSummary = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<LostRatioSummary>())
                    .List<LostRatioSummary>();

                    result.LostRatioSummary = LostRatioSummary.ToList();

                    result.tpaLogo = image.DisplayImageSrc;
                }
                catch (Exception e)
                {

                }

                return result;


            }
            else
            {

                try
                {
                    result = new IncurredErningExportResponseDto();

                    var TpaName = session.Query<TPA>().FirstOrDefault().Name;
                    var TpaLogo = session.Query<TPA>().FirstOrDefault().Logo;
                    var image = new ImageEntityManager().GetImageById(TpaLogo);


                    String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\LostRatioOther.sql"));
                    Query = Query.Replace("{DealerId}", Dealer.ToString());
                    Query = Query.Replace("{CountryId}", cid.ToString());
                    Query = Query.Replace("{UWYear}", UNWyear.ToString());
                    Query = Query.Replace("{MakeId}", MakeId.ToString());
                    Query = Query.Replace("{ModelId}", ModelId.ToString());
                    Query = Query.Replace("{cylinderCountId}", CylindercountId.ToString());
                    Query = Query.Replace("{EngineCapacityId}", EngineCapacityId.ToString());
                    Query = Query.Replace("{ClaimDate}", ClaimDate.ToString("yyyy-MM-dd"));
                    Query = Query.Replace("{BordxStartDate}", BordxStartDate.ToString());
                    Query = Query.Replace("{BordxEndDate}", BordxEndDate.ToString());
                    Query = Query.Replace("{EarnedDate}", ErnedDate.ToString("yyyy-MM-dd 00:00:00.000"));
                    Query = Query.Replace("{InsuaranceLimitationId}", InsuaranceLimitationId.ToString());
                    var LostRatioSummaryOther = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<LostRatioSummaryOther>())
                    .List<LostRatioSummaryOther>();

                    result.LostRatioSummaryOther = LostRatioSummaryOther.ToList();

                    result.tpaLogo = image.DisplayImageSrc;
                }
                catch (Exception e)
                {

                }

                return result;
            }


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

    }
}
