using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
    public class ClaimBordxEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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


        public List<ClaimBordx> GetClaimBordxs()
        {
            List<ClaimBordx> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ClaimBordx> BordxData = session.Query<ClaimBordx>();
            entities = BordxData
                .OrderBy(a => a.BordxYear)
                .ThenBy(b => b.Bordxmonth)
                .ThenBy(c => c.BordxNumber)
                .Take(10).ToList();
            return entities;
        }

        public ClaimBordxsResponseDto GetBordxsById(Guid ReId, Guid InId, int Year)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBordxsResponseDto res = new ClaimBordxsResponseDto();

            try
            {

                var query =
                from ClaimBordx in session.Query<ClaimBordx>()
                join claimbordxpayment in session.Query<ClaimBordxPayment>() on ClaimBordx.Id equals claimbordxpayment.ClaimBordxID
                join claimbordxdetail in session.Query<ClaimBordxDetail>() on ClaimBordx.Id equals claimbordxdetail.ClaimBordxId
                join claiminvoiceEntryclaim in session.Query<ClaimInvoiceEntryClaim>() on claimbordxdetail.ClaimId equals claiminvoiceEntryclaim.ClaimId
                join claimInvoiceentry in session.Query<ClaimInvoiceEntry>() on claiminvoiceEntryclaim.ClaimInvoiceEntryId equals claimInvoiceentry.Id
                where ClaimBordx.Reinsurer == ReId && ClaimBordx.Insurer == InId && ClaimBordx.BordxYear == Year && ClaimBordx.IsConfirmed == true
                select new { ClaimBordx.Id, ClaimBordx.Bordxmonth, ClaimBordx.BordxYear, ClaimBordx.BordxNumber, ClaimBordx.EntryDateTime, ClaimBordx.UserId, ClaimBordx.IsPaid, claimbordxpayment.RefNo, claimInvoiceentry.InvoiceReceivedDate };




                var result = query.ToList();


                if (result.Count != 0)
                {


                    res.ClaimBordxs = new List<ClaimBordxResponseDto>();
                    foreach (var Bordx in result)
                    {
                        ClaimBordxResponseDto pr = new ClaimBordxResponseDto();

                        pr.Id = Bordx.Id;
                        pr.Bordxmonth = Bordx.Bordxmonth;
                        pr.BordxYear = Bordx.BordxYear;
                        pr.BordxNumber = Bordx.BordxNumber;
                        pr.EntryDateTime = Bordx.EntryDateTime;
                        pr.UserId = Bordx.UserId;
                        pr.BordxAmount = GetborxAmout(Bordx.Id);
                        pr.BalanceAmount = GetBalanceAmout(Bordx.Id);
                        pr.PaidAmount = GetPaidAmout(Bordx.Id);
                        pr.IsPaid = Bordx.IsPaid;
                        pr.InvoiceReceivedDate = Bordx.InvoiceReceivedDate.ToString("dd/M/yyyy");
                        pr.RefNo = Bordx.RefNo;
                        res.ClaimBordxs.Add(pr);
                    }
                }
                else {

                var query2 =
                from ClaimBordx in session.Query<ClaimBordx>()
                join claimbordxdetail in session.Query<ClaimBordxDetail>() on ClaimBordx.Id equals claimbordxdetail.ClaimBordxId
                join claiminvoiceEntryclaim in session.Query<ClaimInvoiceEntryClaim>() on claimbordxdetail.ClaimId equals claiminvoiceEntryclaim.ClaimId
                join claimInvoiceentry in session.Query<ClaimInvoiceEntry>() on claiminvoiceEntryclaim.ClaimInvoiceEntryId equals claimInvoiceentry.Id
                where ClaimBordx.Reinsurer == ReId && ClaimBordx.Insurer == InId && ClaimBordx.BordxYear == Year && ClaimBordx.IsConfirmed == true
                select new { ClaimBordx.Id, ClaimBordx.Bordxmonth, ClaimBordx.BordxYear, ClaimBordx.BordxNumber, ClaimBordx.EntryDateTime, ClaimBordx.UserId, ClaimBordx.IsPaid, claimInvoiceentry.InvoiceReceivedDate };

                    var result2 = query2.ToList();
                    res.ClaimBordxs = new List<ClaimBordxResponseDto>();
                    foreach (var Bordx in result2)
                    {
                        ClaimBordxResponseDto pr = new ClaimBordxResponseDto();

                        pr.Id = Bordx.Id;
                        pr.Bordxmonth = Bordx.Bordxmonth;
                        pr.BordxYear = Bordx.BordxYear;
                        pr.BordxNumber = Bordx.BordxNumber;
                        pr.EntryDateTime = Bordx.EntryDateTime;
                        pr.UserId = Bordx.UserId;
                        pr.BordxAmount = GetborxAmout(Bordx.Id);
                        pr.BalanceAmount = GetBalanceAmout(Bordx.Id);
                        pr.PaidAmount = GetPaidAmout(Bordx.Id);
                        pr.IsPaid = Bordx.IsPaid;
                        pr.InvoiceReceivedDate = Bordx.InvoiceReceivedDate.ToString("dd/M/yyyy");
                        pr.RefNo = "" ;
                        res.ClaimBordxs.Add(pr);
                    }


                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return res;

        }

        public decimal GetborxAmout(Guid ClaimBordxID)
        {
            ISession session = EntitySessionManager.GetSession();
            decimal result =0M;

            try
            {
                var query =
                from ClaimBordxValueDetail in session.Query<ClaimBordxValueDetail>()
                where ClaimBordxValueDetail.ClaimBordxId == ClaimBordxID
                select new { ClaimBordxValueDetail = ClaimBordxValueDetail };
                if (query.FirstOrDefault() != null)
                    result = query.FirstOrDefault().ClaimBordxValueDetail.Value;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return result;
        }

        public decimal GetBalanceAmout(Guid ClaimBordxID)
        {
            ISession session = EntitySessionManager.GetSession();
            decimal result = 0M;

            try
            {
                var query =
                from ClaimBordxPayment in session.Query<ClaimBordxPayment>()
                where ClaimBordxPayment.ClaimBordxID == ClaimBordxID
                select new { ClaimBordxPayment = ClaimBordxPayment };
                if (query.FirstOrDefault() != null)
                    result = query.FirstOrDefault().ClaimBordxPayment.BalanceAmount;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return result;
        }

        public decimal GetPaidAmout(Guid ClaimBordxID)
        {
            ISession session = EntitySessionManager.GetSession();
            decimal result = 0M;

            try
            {
                var query =
                from ClaimBordxPayment in session.Query<ClaimBordxPayment>()
                where ClaimBordxPayment.ClaimBordxID == ClaimBordxID
                select new { ClaimBordxPayment = ClaimBordxPayment };
                if (query.FirstOrDefault() != null)
                    result = query.FirstOrDefault().ClaimBordxPayment.PaidAmount;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return result;


        }

        public ClaimBordxResponseDto GetClaimBordxById(Guid ClaimBordxID)
        {
            ISession session = EntitySessionManager.GetSession();

            ClaimBordxResponseDto pDto = new ClaimBordxResponseDto();

            var query =
                from ClaimBordx in session.Query<ClaimBordx>()
                where ClaimBordx.Id == ClaimBordxID
                select new { ClaimBordx = ClaimBordx };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().ClaimBordx.Id;
                pDto.Insurer = result.First().ClaimBordx.Insurer;
                pDto.Reinsurer = result.First().ClaimBordx.Reinsurer;
                pDto.Bordxmonth = result.First().ClaimBordx.Bordxmonth;
                pDto.BordxYear = result.First().ClaimBordx.BordxYear;
                pDto.BordxNumber = result.First().ClaimBordx.BordxNumber;
                pDto.Fromdate = result.First().ClaimBordx.Fromdate;
                pDto.Todate = result.First().ClaimBordx.Todate;
                pDto.IsConfirmed = result.First().ClaimBordx.IsConfirmed;
                pDto.EntryDateTime = result.First().ClaimBordx.EntryDateTime;

                //pDto.IsBordxExists = true;
                return pDto;
            }
            else
            {
                //pDto.IsBordxExists = false;
                return null;
            }
        }

        internal string AddClaimBordx(ClaimBordxRequestDto ClaimBordx)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                //date validity
                //List<ClaimBordx> FromDateOverlaps = session.Query<ClaimBordx>()
                //    .Where(a => a.Fromdate <= ClaimBordx.Fromdate && a.Todate >= ClaimBordx.Todate
                //     && a.Insurer == ClaimBordx.Insurer && a.Reinsurer == ClaimBordx.Reinsurer).ToList();

                List<ClaimBordx> FromDateOverlaps = session.Query<ClaimBordx>()
                    .Where(a => a.Fromdate <= ClaimBordx.Fromdate && a.Todate >= ClaimBordx.Fromdate
                     && a.Reinsurer == ClaimBordx.Reinsurer
                     && a.Insurer == ClaimBordx.Insurer).ToList();

                if (FromDateOverlaps != null && FromDateOverlaps.Count() > 0)
                {
                    Response = "Form Date you have selected overlaps with bordereaux " +
                        FromDateOverlaps.FirstOrDefault().BordxYear + "-" +
                        ((Months)FromDateOverlaps.FirstOrDefault().Bordxmonth).ToString() + "-" +
                        FromDateOverlaps.FirstOrDefault().BordxNumber + "'s  date range.";
                    return Response;
                }

                List<ClaimBordx> ToDateOverlaps = session.Query<ClaimBordx>()
                    .Where(a => a.Fromdate <= ClaimBordx.Todate && a.Todate >= ClaimBordx.Todate
                     && a.Reinsurer == ClaimBordx.Reinsurer
                     && a.Insurer == ClaimBordx.Insurer).ToList();

                if (ToDateOverlaps != null && ToDateOverlaps.Count() > 0)
                {
                    Response = "To Date you have selected overlaps with bordereaux " +
                        ToDateOverlaps.FirstOrDefault().BordxYear + "-" +
                        ((Months)ToDateOverlaps.FirstOrDefault().Bordxmonth).ToString() + "-" +
                        ToDateOverlaps.FirstOrDefault().BordxNumber + "'s  date range.";
                    return Response;
                }

                ClaimBordx pr = new Entities.ClaimBordx();

                pr.Id = new Guid();
                pr.Insurer = ClaimBordx.Insurer;
                pr.Reinsurer = ClaimBordx.Reinsurer;
                pr.Bordxmonth = ClaimBordx.Bordxmonth;
                pr.BordxYear = ClaimBordx.BordxYear;
                pr.Fromdate = ClaimBordx.Fromdate;
                pr.Todate = ClaimBordx.Todate;
                pr.BordxNumber = GetNextBordxNumber(ClaimBordx.BordxYear, ClaimBordx.Bordxmonth, ClaimBordx.Insurer, ClaimBordx.Reinsurer);
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.UserId = ClaimBordx.UserId;//Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.ProcessedDateTime = DateTime.Today.ToUniversalTime();
                pr.ConfirmedDateTime = DateTime.Today.ToUniversalTime();


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    ClaimBordx.Id = pr.Id;
                    transaction.Commit();
                }

                return "successful";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured";
            }
        }

        public string GetNextBordxNumber(int year, int month, Guid insurerId, Guid ReinsurerId)
        {

            string response = "";
            try
            {
                int BordxNumber = 1;
                ISession session = EntitySessionManager.GetSession();
                List<ClaimBordx> existingBordexForRequestMonth = session.Query<ClaimBordx>()
                        .Where(a => a.BordxYear == year && a.Bordxmonth == month
                            && a.Insurer == insurerId && a.Reinsurer == ReinsurerId).ToList();

                if (existingBordexForRequestMonth != null && existingBordexForRequestMonth.Count() != 0)
                {
                    BordxNumber = Convert.ToInt32(existingBordexForRequestMonth.OrderByDescending(a => a.BordxNumber).FirstOrDefault().BordxNumber) + 1;
                }
                response = year.ToString() + month.ToString("00") + BordxNumber.ToString("00");
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;

        }

        internal static string DeleteClaimBordx(Guid ClaimBordxId)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //validate
                ClaimBordx bordx = session.Query<ClaimBordx>()
                    .Where(a => a.Id == ClaimBordxId).FirstOrDefault();



                if (bordx == null)
                {
                    Response = "Invalid bordereaux selection";
                    return Response;
                }


                IEnumerable<ClaimBordxDetail> claimBordxDetails = session.Query<ClaimBordxDetail>()
                   .Where(b => b.ClaimBordxId == ClaimBordxId);
                IEnumerable<ClaimBordxPayment> claimBordxpayments = session.Query<ClaimBordxPayment>()
                   .Where(c => c.ClaimBordxID == ClaimBordxId);
                //delete bordx details
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (ClaimBordxDetail claimBordxDetail in claimBordxDetails)
                    {

                        session.Delete(claimBordxDetail);

                    }
                    transaction.Commit();
                }
                //delete bordx payments
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (ClaimBordxPayment claimBordxpayment in claimBordxpayments)
                    {

                        session.Delete(claimBordxpayment);

                    }
                    transaction.Commit();
                }



                //if (bordx.Isconfirmed)
                //{
                //    Response = "Cannot delete confirmed bordereaux";
                //    return Response;
                //}

                //revert policies
                //List<Policy> policies = session.Query<Policy>()
                //    .Where(a => a.Year == bordx.Year && a.Month == bordx.Month && a.BordxNumber == bordx.Number).ToList();
                //foreach (Policy policy in policies)
                //{
                //    using (ITransaction transaction = session.BeginTransaction())
                //    {
                //        policy.Year = 0;
                //        policy.Month = 0;
                //        policy.BordxNumber = 0;
                //        policy.BordxCountryId = Guid.Empty;

                //        session.Update(policy, policy.Id);
                //        transaction.Commit();
                //    }
                //    session.Evict(policy);
                //}



                //delete bordx
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(bordx);
                    transaction.Commit();
                }

                Response = "successful";
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<int> GetConfirmedClaimBordxYears()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ClaimBordx>()
               .Select(c => c.BordxYear).Distinct().ToList();
        }

        internal static object GetConfirmedClaimBordxForGrid(ClaimBordxSearchGridSearchCriterias claimBordxSearchGridSearchCriterias, PaginationOptionsClaimbordxSearchGrid paginationOptionsClaimbordxSearchGrid, string action)
        {
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();
            //expression builder for bordx
            Expression<Func<ClaimBordx, bool>> filterBordx = PredicateBuilder.True<ClaimBordx>();

            if (claimBordxSearchGridSearchCriterias.year > 0)
                filterBordx = filterBordx.And(p => p.BordxYear == claimBordxSearchGridSearchCriterias.year);
            if (claimBordxSearchGridSearchCriterias.month > 0)
                filterBordx = filterBordx.And(p => p.Bordxmonth == claimBordxSearchGridSearchCriterias.month);
            if (IsGuid(claimBordxSearchGridSearchCriterias.Insurer.ToString()))
                filterBordx = filterBordx.And(p => p.Insurer == claimBordxSearchGridSearchCriterias.Insurer);
            if (IsGuid(claimBordxSearchGridSearchCriterias.Reinsurer.ToString()))
                filterBordx = filterBordx.And(p => p.Reinsurer == claimBordxSearchGridSearchCriterias.Reinsurer);


            if (action == "bordxview")
            {
                filterBordx = filterBordx.And(p => p.IsConfirmed);
            }
            else if (action == "bordxreopen")
            {
                filterBordx = filterBordx.And(p => p.IsConfirmed);
            }
            else
            {
                return new object();
            }



            //get bordx list
            IEnumerable<ClaimBordx> currentBordxList = session.Query<ClaimBordx>().Where(filterBordx)
                .OrderByDescending(a => a.BordxYear).ThenByDescending(a => a.Bordxmonth).ThenByDescending(a => a.BordxNumber);
            if (action == "bordxreopen")
                currentBordxList = currentBordxList.GroupBy(x => new { x.Insurer, x.Reinsurer, x.BordxYear, x.Bordxmonth },
                    (key, y) => y.OrderBy(z => z.BordxNumber).First());

            var policyGridDetails = currentBordxList
       .Select(a => new
       {
           a.Id,
           Insurer = cem.GetInsurerNameById(a.Insurer),
           Reinsurer = cem.GetReinsurerNameById(a.Reinsurer),
           a.BordxYear,
           a.Bordxmonth,
           a.BordxNumber,
           StartDate = a.Fromdate.ToString("dd-MMM-yyyy"),
           EndDate = a.Todate.ToString("dd-MMM-yyyy")
       });
            long TotalRecords = policyGridDetails.Count();
            var policyGridDetailsFilterd = policyGridDetails.Skip((paginationOptionsClaimbordxSearchGrid.pageNumber - 1) * paginationOptionsClaimbordxSearchGrid.pageSize)
            .Take(paginationOptionsClaimbordxSearchGrid.pageSize)
            .ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = policyGridDetailsFilterd
            };
            return new JavaScriptSerializer().Serialize(response);
        }

        internal static object GetAllClaimBordxByDateForGrid(GetAllClaimBordxByDateRequestDto _GetAllClaimBordxByDateRequestDto)
        {
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();

            Expression<Func<ClaimBordx, bool>> filterBordx = PredicateBuilder.True<ClaimBordx>();

            if (_GetAllClaimBordxByDateRequestDto.year > 0)
                filterBordx = filterBordx.And(p => p.BordxYear == _GetAllClaimBordxByDateRequestDto.year);
            if (_GetAllClaimBordxByDateRequestDto.month > 0)
                filterBordx = filterBordx.And(p => p.Bordxmonth == _GetAllClaimBordxByDateRequestDto.month);
            if (_GetAllClaimBordxByDateRequestDto.number!="")
                filterBordx = filterBordx.And(p => p.BordxNumber == _GetAllClaimBordxByDateRequestDto.number);

            filterBordx = filterBordx.And(p => p.IsConfirmed==true);

            IEnumerable<ClaimBordx> currentBordxList = session.Query<ClaimBordx>().Where(filterBordx)
               .OrderByDescending(a => a.BordxYear).ThenByDescending(a => a.Bordxmonth).ThenByDescending(a => a.BordxNumber);


            //IQueryable<ClaimBordx> bordxDetails = session.Query<ClaimBordx>()
            //    .Where(a => a.Bordxmonth == _GetAllClaimBordxByDateRequestDto.month && a.BordxYear == _GetAllClaimBordxByDateRequestDto.year && a.IsConfirmed == true
            //        && a.BordxNumber == _GetAllClaimBordxByDateRequestDto.number);
            //IQueryable<Policy> PolicyList = session.Query<Policy>()
            //    .Where(a => bordxDetails.Any(b => b.PolicyId == a.Id));
            long TotalRecords = currentBordxList.Count();
            var policyGridDetailsFilterd = currentBordxList.Skip(_GetAllClaimBordxByDateRequestDto.page - 1)
            .Take(_GetAllClaimBordxByDateRequestDto.pageSize)
            .ToList();
            var policyGridDetails = policyGridDetailsFilterd.Select(a => new
            {
                a.Id,
                a.Bordxmonth,
                a.BordxNumber,
                a.BordxYear,
                Createddate = a.EntryDateTime.ToString("dd-MMM-yyyy"),
                Fromdate = a.Fromdate.ToString("dd-MMM-yyyy"),
                Insurer = cem.GetInsurerNameById(a.Insurer),
                a.IsConfirmed,
                Reinsurer = cem.GetReinsurerNameById(a.Reinsurer),
                Todate = a.Todate.ToString("dd-MMM-yyyy")


            }).ToArray();

            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = policyGridDetails
            };
            return new JavaScriptSerializer().Serialize(response);
        }

        public ClaimBordxsYearsResponseDto GetClaimBordxYears()
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBordxsYearsResponseDto res = new ClaimBordxsYearsResponseDto();
            try
            {
                var result =
                session.Query<Claim>()
                .Select(c => c.EntryDate.Year).Distinct().ToArray();
                res.BordxYears = result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return res;
        }

        internal string ClaimBordxReopen(ClaimBordxReopenRequestDto ClaimBordxReopenRequestDto)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBordx claimBordx = session.Query<ClaimBordx>()
                    .Where(a => a.BordxYear == ClaimBordxReopenRequestDto.claimBordxYear).FirstOrDefault();
                if (claimBordx == null)
                    return "Invalid bordereaux selection";

                IEnumerable<ClaimBordx> claimBordxList = session.Query<ClaimBordx>()
                    .Where(a => a.BordxYear == claimBordx.BordxYear && a.Bordxmonth == ClaimBordxReopenRequestDto.claimBordxMonth && a.IsConfirmed == true);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (ClaimBordx claimbordx in claimBordxList)
                    {
                        session.Evict(claimbordx);
                        claimbordx.IsConfirmed = false;
                        session.Update(claimbordx, claimbordx.Id);
                        transaction.Commit();

                    }
                }
                Response = "successful";
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<int> GetConfirmedClaimBordxYearlybyYear()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ClaimBordxYearly>()
               .Select(c => c.Year).Distinct().ToList();
        }

        internal static object GetAllClaimBordxByYearandCountryForGrid(GetAllClaimBordxByYearandCountryRequestDto _GetAllClaimBordxByYearandCountryRequestDto)
        {
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

            var query =
                from Country in session.Query<Country>()
                where Country.Id == _GetAllClaimBordxByYearandCountryRequestDto.Country
                select new { Country = Country };


            var result = query.FirstOrDefault();
            IQueryable<ClaimBordxYearly> bordxDetails;
            if (_GetAllClaimBordxByYearandCountryRequestDto.Year != 0) {

                 bordxDetails = session.Query<ClaimBordxYearly>()
                    .Where(a => a.Year == _GetAllClaimBordxByYearandCountryRequestDto.Year && a.CountryId == _GetAllClaimBordxByYearandCountryRequestDto.Country && a.IsConformed == true);

            }
            else {

                bordxDetails = session.Query<ClaimBordxYearly>()
                   .Where(a => a.CountryId == _GetAllClaimBordxByYearandCountryRequestDto.Country && a.IsConformed == true);


            }

            long TotalRecords = bordxDetails.Count();
            var ClaimBordxYearlyGridDetailsFilterd = bordxDetails.Skip(_GetAllClaimBordxByYearandCountryRequestDto.page - 1)
            .Take(_GetAllClaimBordxByYearandCountryRequestDto.pageSize)
            .ToList();
            var ClaimBordxYearlyGridDetails = ClaimBordxYearlyGridDetailsFilterd.Select(a => new
            {
                a.Id,
                BordxAmount = currencyEm.ConvertFromBaseCurrency(a.BordxAmount, result.Country.CurrencyId, currentCurrencyPeriodId),
                //a.BordxAmount,
                CountryId = cem.GetCountryNameById(a.CountryId),
                a.Year


            }).ToArray();

            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = ClaimBordxYearlyGridDetails
            };
            return new JavaScriptSerializer().Serialize(response);
        }

        internal string ClaimBordxYealyReopen(ClaimBordxYearlyReopenRequestDto ClaimBordxYearlyReopenRequestDto)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBordxYearly claimBordxYearly = session.Query<ClaimBordxYearly>()
                    .Where(a => a.Year == ClaimBordxYearlyReopenRequestDto.claimBordxYear).FirstOrDefault();
                if (claimBordxYearly == null)
                    return "Invalid bordereaux selection";

                IEnumerable<ClaimBordxYearly> claimBordxYearlyList = session.Query<ClaimBordxYearly>()
                    .Where(a => a.Year == claimBordxYearly.Year && a.CountryId == ClaimBordxYearlyReopenRequestDto.claimBordxCountry && a.IsConformed == true);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (ClaimBordxYearly claimbordx in claimBordxYearlyList)
                    {
                        session.Evict(claimbordx);
                        claimbordx.IsConformed = false;
                        session.Update(claimbordx, claimbordx.Id);
                        transaction.Commit();

                    }
                }
                Response = "successful";
            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static List<String> GetClaimBordxBordxNumbers()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ClaimBordx>()
               .Select(c => c.BordxNumber).Distinct().ToList();
        }

        public ClaimBordxsYearsResponseDto GetClaimBordxYearsForProcess()
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBordxsYearsResponseDto res = new ClaimBordxsYearsResponseDto();
            try
            {
                var result =
                session.Query<Claim>()
                .Select(c => c.EntryDate.Year).Distinct().ToArray();
                // .Select(c => c.EntryDate.Year).Distinct().ToArray();
                res.BordxYears = result;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return res;
        }

        public string ClaimBordxYearlyProcess(int year, Guid Reinsurer, Guid Insurer)
        {
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();

                var bordxDetails = session.Query<ClaimBordx>()
                    .Where(a => a.BordxYear == year && a.Insurer == Insurer && a.Reinsurer == Reinsurer && a.IsConfirmed == true).Select(a => a.Id).Distinct().ToList();
                foreach (var Id in bordxDetails)
                {
                    var ProcessDetails = session.Query<ClaimBordxValueDetail>()
                    .Where(a => a.ClaimBordxId == Id).Select(a =>
                        new
                        {
                            a.CountryId,
                            a.USDValue,
                            a.Value
                        }).Distinct().ToList();

                    foreach (var Prodata in ProcessDetails)
                    {

                        ClaimBordxYearly result = session.Query<ClaimBordxYearly>()
                        .Where(a => a.CountryId == Prodata.CountryId && a.Year == year).FirstOrDefault();


                        if (result != null)
                        {
                            result.BordxAmount = result.BordxAmount + Prodata.USDValue;
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Update(result, result.Id);
                                transaction.Commit();
                            }

                        }
                        else
                        {
                            ClaimBordxYearly pr = new Entities.ClaimBordxYearly();

                            pr.Id = new Guid();
                            pr.Year = year;
                            pr.CountryId = Prodata.CountryId;
                            pr.BordxAmount = Prodata.USDValue;
                            pr.IsConformed = false;

                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.SaveOrUpdate(pr);
                                transaction.Commit();
                            }
                        }

                        //Yearly Sum
                        ClaimBordxYearlySum resultSum = session.Query<ClaimBordxYearlySum>()
                     .Where(a => a.Reinsurer == Reinsurer && a.Insurer == Insurer && a.Year == year).FirstOrDefault();


                        if (resultSum != null)
                        {
                            resultSum.BordxAmount = resultSum.BordxAmount + Prodata.USDValue;
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Update(resultSum, resultSum.Id);
                                transaction.Commit();
                            }

                        }
                        else
                        {
                            ClaimBordxYearlySum prSum = new Entities.ClaimBordxYearlySum();

                            prSum.Id = new Guid();
                            prSum.Year = year;
                            prSum.Reinsurer = Reinsurer;
                            prSum.Insurer = Insurer;
                            prSum.BordxAmount = Prodata.USDValue;
                            prSum.IsConformed = false;

                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.SaveOrUpdate(prSum);
                                transaction.Commit();
                            }
                        }

                    }

                }
                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error";
            }
        }

        public string ClaimBordxYearlyProcessConfirm(int year , Guid Reinsurer, Guid Insurer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBordxYearly claimBordxYearly = session.Query<ClaimBordxYearly>()
                    .Where(a => a.Year == year).FirstOrDefault();

                ClaimBordxYearlySum claimBordxYearlySum  = session.Query<ClaimBordxYearlySum>()
                    .Where(a => a.Year == year && a.Reinsurer == Reinsurer && a.Insurer == Insurer).FirstOrDefault();



                if (claimBordxYearlySum.IsConformed) {
                    return "Yearly Claim  Bordereau Already Confirmed";
                }

                if (claimBordxYearlySum == null)
                    return "Invalid Re-insurer , Insurer Or Year selection";

                IEnumerable<ClaimBordxYearly> claimBordxYearlyList = session.Query<ClaimBordxYearly>()
                    .Where(a => a.Year == claimBordxYearly.Year);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(claimBordxYearlySum);
                    claimBordxYearlySum.IsConformed = true;
                    session.Update(claimBordxYearlySum, claimBordxYearlySum.Id);

                    foreach (ClaimBordxYearly claimbordx in claimBordxYearlyList)
                    {
                        if (claimbordx.IsConformed == false)
                        {
                            session.Evict(claimbordx);
                            claimbordx.IsConformed = true;
                            session.Update(claimbordx, claimbordx.Id);
                            transaction.Commit();
                        }
                        //else {

                        //    return "Yearly Claim Bordx Already Confirmed";
                        //}
                    }
                }

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error";

            }
        }


    }
}
