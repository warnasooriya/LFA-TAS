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
    public class ClaimBordxProcessEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<int> GetClaimBordxYears()
        {
            ISession session = EntitySessionManager.GetSession();
            var entities = session.Query<ClaimBordx>().Select(a => a.BordxYear).Distinct().ToList();
            return entities;
        }

        internal static List<int> GetClaimYears()
        {
            ISession session = EntitySessionManager.GetSession();
            var entities = session.Query<ClaimSubmission>().Select(a => a.ClaimDate.Year).Distinct().ToList();
            return entities;
        }

        public List<ClaimBordxResponseDto> GetClaimBordxByYearAndMonth(int year, int month, Guid insurerid, Guid reinsurerid)
        {
            ISession session = EntitySessionManager.GetSession();
            List<ClaimBordxResponseDto> resultlist = null;
            try
            {
                var entities = session.Query<ClaimBordx>().Where(a => a.Insurer == insurerid && a.Reinsurer == reinsurerid && a.BordxYear == year && a.Bordxmonth == month).Distinct().ToList();

                if (entities != null && entities.Count > 0)
                {
                    resultlist = new List<ClaimBordxResponseDto>();
                    ClaimBordxResponseDto claimprocess;
                    foreach (var item in entities)
                    {
                        claimprocess = new ClaimBordxResponseDto();

                        claimprocess.Id = item.Id;
                        claimprocess.BordxNumber = item.BordxNumber;
                        claimprocess.IsConfirmed = item.IsConfirmed;
                        claimprocess.IsProcessed = item.IsProcessed;
                        resultlist.Add(claimprocess);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return resultlist;
        }

        public bool ProcessClaimBordx(Guid claimbordxId, Guid userId, bool isProcess = true)
        {
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    ClaimBordx _claimbordx;

                    _claimbordx = session.Query<ClaimBordx>().FirstOrDefault(a => a.Id == claimbordxId);

                    if (_claimbordx != null)
                    {
                        _claimbordx.IsProcessed = isProcess;
                        _claimbordx.ProcessedUserId = userId;
                        _claimbordx.ProcessedDateTime = DateTime.Today.ToUniversalTime();

                        session.SaveOrUpdate(_claimbordx);

                        var clamprocessdtllist = session.Query<ClaimBordxDetail>().Where(a => a.ClaimBordxId == claimbordxId).ToList();

                        if (clamprocessdtllist != null && clamprocessdtllist.Count > 0)
                        {
                            foreach (var claimBordxDetail in clamprocessdtllist)
                            {
                                session.Delete(claimBordxDetail);
                            }
                        }

                        ClaimBordxDetail _claimbdxprdtl;

                        //------------------------- Batching Claims ----------------------------------------------------------------
                        // in hear previously retrived batching reference tables data but currently it change to Reference Tables and IsBatching=true in claim table
                        // therefore this data loading from claimInvoiceEntry table isConfirmed=true
                        var claimbatches = (from CI in session.Query<ClaimInvoiceEntry>()
                                         join CIC in session.Query<ClaimInvoiceEntryClaim>() on CI.Id equals CIC.ClaimInvoiceEntryId
                                         join C in session.Query<Claim>() on CIC.ClaimId equals C.Id
                                         join P in session.Query<Policy>() on C.PolicyId equals P.Id
                                         join CO in session.Query<Contract>() on P.ContractId equals CO.Id
                                         join RIC in session.Query<ReinsurerContract>() on CO.ReinsurerContractId equals RIC.Id
                                         where (CI.EntryDateTime.Year == _claimbordx.BordxYear && CI.EntryDateTime.Month == _claimbordx.Bordxmonth && CI.IsConfirm == true
                                         && CO.InsurerId == _claimbordx.Insurer && RIC.ReinsurerId == _claimbordx.Reinsurer  )
                                         select new { C.Id, C.IsApproved, C.IsBatching, C.PaidAmount, C.PolicyCountryId, C.ConversionRate }).ToList();

                        if (claimbatches != null && claimbatches.Count > 0)
                        {
                            foreach (var claimbatch in claimbatches)
                            {
                                _claimbdxprdtl = new Entities.ClaimBordxDetail();

                                _claimbdxprdtl.Id = new Guid();
                                _claimbdxprdtl.ClaimBordxId = claimbordxId;
                                _claimbdxprdtl.ClaimId = claimbatch.Id;
                                _claimbdxprdtl.UserId = userId;
                                _claimbdxprdtl.EntryDateTime = DateTime.Today.ToUniversalTime();
                                _claimbdxprdtl.IsBatching = claimbatch.IsBatching;
                                _claimbdxprdtl.IsApproved = claimbatch.IsApproved;

                                session.SaveOrUpdate(_claimbdxprdtl);
                            }
                        }

                        //------------------------- Approved Claims ----------------------------------------------------------------

                        var claims = (from claim in session.Query<Claim>()
                                      where (claim.IsApproved == true  && claim.ApprovedDate.Year == _claimbordx.BordxYear && claim.ApprovedDate.Month == _claimbordx.Bordxmonth)
                                      join policy in session.Query<Policy>() on claim.PolicyId equals policy.Id
                                      join contract in session.Query<Contract>() on policy.ContractId equals contract.Id
                                      join reinsurerContract  in session.Query<ReinsurerContract>() on contract.ReinsurerContractId equals reinsurerContract.Id
                                      where (reinsurerContract.ReinsurerId == _claimbordx.Reinsurer && contract.InsurerId == _claimbordx.Insurer)
                                      select new {
                                          claim.Id,
                                          claim.IsApproved,
                                          claim.IsBatching,
                                          claim.PaidAmount,
                                          claim.PolicyCountryId,
                                          claim.ConversionRate })
                                          .ToList();
                        //.Where(a => a.IsApproved == true && a.ApprovedDate.Year == _claimbordx.BordxYear && a.ApprovedDate.Month == _claimbordx.Bordxmonth)
                        //.Select(s => new { s.Id, s.IsApproved, s.IsBatching, s.PaidAmount, s.PolicyCountryId, s.ConversionRate })
                        //.ToList()
                        //.Where(r => (!claimbatches.Contains(r)))
                        //.ToList();

                        if (claims != null && claims.Count > 0)
                        {
                            //bool isAdd = true;
                            foreach (var claim in claims)
                            {
                                _claimbdxprdtl = new Entities.ClaimBordxDetail();


                                _claimbdxprdtl.Id = new Guid();
                                _claimbdxprdtl.ClaimBordxId = claimbordxId;
                                _claimbdxprdtl.ClaimId = claim.Id;
                                _claimbdxprdtl.UserId = userId;
                                _claimbdxprdtl.EntryDateTime = DateTime.Today.ToUniversalTime();
                                _claimbdxprdtl.IsBatching = claim.IsBatching;
                                _claimbdxprdtl.IsApproved = claim.IsApproved;

                                session.SaveOrUpdate(_claimbdxprdtl);

                                //if (isAdd)
                                //{
                                //    session.SaveOrUpdate(_claimbdxprdtl);
                                //}
                            }
                        }


                        /// ----------------------------- bordx Value Detail -----------------------------------------------------------------

                        var bordxvaluelist = session.Query<ClaimBordxValueDetail>().Where(a => a.ClaimBordxId == claimbordxId).ToList();

                        if (bordxvaluelist != null && bordxvaluelist.Count > 0)
                        {
                            foreach (var claimBordxValueDetail in bordxvaluelist) session.Delete(claimBordxValueDetail);
                        }

                        var countrylist = claimbatches.Union(claims).GroupBy(g => new { g.ConversionRate, g.PolicyCountryId }).Select(a => new { ConversionRate = a.Key.ConversionRate, CountryId = a.Key.PolicyCountryId }).ToList();

                        if (countrylist != null && countrylist.Count > 0)
                        {
                            var value = claims.Sum(a => a.PaidAmount);

                            ClaimBordxValueDetail _claimbdxvaldtl;

                            foreach (var country in countrylist)
                            {
                                _claimbdxvaldtl = new Entities.ClaimBordxValueDetail();

                                _claimbdxvaldtl.Id = new Guid();
                                _claimbdxvaldtl.ClaimBordxId = claimbordxId;
                                _claimbdxvaldtl.Rate = country.ConversionRate;
                                _claimbdxvaldtl.CountryId = country.CountryId;
                                _claimbdxvaldtl.USDValue = value;
                                _claimbdxvaldtl.Value = value == null ? 0 : value * country.ConversionRate;

                                session.SaveOrUpdate(_claimbdxvaldtl);
                            }
                        }

                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            }
            return true;
        }


        internal static object ProcessClaimBordxForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto)
        {

            ISession session = EntitySessionManager.GetSession();
            List<Guid> claimBordxDetail = session.Query<ClaimBordxDetail>()
                    .Select(a => a.ClaimId).ToList();

            Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();
            filterClaim = filterClaim.And(a => claimBordxDetail.Contains(a.Id));

            ClaimBordx _claimbordx;
            _claimbordx = session.Query<ClaimBordx>().FirstOrDefault(a => a.Id == ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.claimId);


            var claims = (from claim in session.Query<Claim>()
                        where (claim.IsApproved == true && claim.IsBatching == false && claim.ApprovedDate.Year == _claimbordx.BordxYear && claim.ApprovedDate.Month == _claimbordx.Bordxmonth)
                        join policy in session.Query<Policy>() on claim.PolicyId equals policy.Id
                        join contract in session.Query<Contract>() on policy.ContractId equals contract.Id
                        // where (contract.ReinsurerId == _claimbordx.Reinsurer && contract.InsurerId == _claimbordx.Insurer)
                        select new
                        {
                            claim.ClaimNumber,
                            claim.PolicyNumber,
                            claim.ClaimDate,
                            claim.ApprovedDate,
                            claim.AuthorizedAmount,

                        });
            long TotalRecords = claims.Count();

            var customerGridDetailsFilterd = claims.Skip((ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageNumber - 1) * ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageSize)
                .Take(ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageSize);

                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = customerGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);

        }

        public bool ConfirmClaimBordxProcess(Guid claimbordxprocessId, Guid userId, bool isConfirm = true)
        {
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    ClaimBordx _claimbordx = session.Query<ClaimBordx>().FirstOrDefault(a => a.Id == claimbordxprocessId);
                    if (_claimbordx != null)
                    {

                        _claimbordx.IsConfirmed = isConfirm;
                        _claimbordx.ConfirmedUserId = userId;
                        _claimbordx.ConfirmedDateTime = DateTime.Today.ToUniversalTime();

                        session.SaveOrUpdate(_claimbordx);

                        transaction.Commit();
                    }
                    else
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            }
            return true;
        }

        public List<ClaimBordxProcessResponseDto> GetClaimBordxProcessedDetailsByYear(int year, Guid insurerid, Guid reinsurerid)
        {
            ISession session = EntitySessionManager.GetSession();
            List<ClaimBordxProcessResponseDto> resultlist = null;
            try
            {

                var entities = session.Query<ClaimBordx>().Where(a => a.Insurer == insurerid && a.Reinsurer == reinsurerid && a.BordxYear == year && a.IsProcessed == true).Distinct().ToList();

                if (entities != null && entities.Count > 0)
                {
                    resultlist = new List<ClaimBordxProcessResponseDto>();
                    ClaimBordxProcessResponseDto claimprocess;
                    foreach (var item in entities)
                    {
                        claimprocess = new ClaimBordxProcessResponseDto();

                        var amount = session.Query<ClaimBordxValueDetail>().FirstOrDefault(a => a.ClaimBordxId == item.Id);

                        claimprocess.Id = item.Id;
                        claimprocess.BordxNumber = item.BordxNumber;
                        claimprocess.IsConfirmed = item.IsConfirmed;
                        claimprocess.Month = item.Bordxmonth;
                        claimprocess.Year = item.BordxYear;
                        claimprocess.Amount = amount != null ? amount.USDValue : 0;

                        resultlist.Add(claimprocess);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return resultlist;
        }

        public bool BordexCanConfirmedByYear(int year, Guid insurerid, Guid reinsurerid)
        {
            bool result = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                var entities = session.Query<ClaimBordxYearlySum>().Where(a => a.Insurer == insurerid && a.Reinsurer == reinsurerid).ToList();
                var lastYearObj = session.Query<ClaimBordx>().OrderByDescending(a => a.BordxYear).FirstOrDefault(a => a.BordxYear < year);
                var minYear = session.Query<ClaimBordx>().Min(a => a.BordxYear);
                var lastYear = (lastYearObj != null) ? lastYearObj.BordxYear : (year - 1);
                if (entities != null && entities.Count > 0)
                {
                    if (entities.Count(a => a.Year == lastYear) > 0)
                    {
                        result = entities.First(a => a.Year == lastYear).IsConformed;
                    }
                }
                else
                {
                    minYear = ((minYear != null) ? minYear : year) - 1;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        try
                        {
                            ClaimBordxYearlySum _claimbdxyearsum = new Entities.ClaimBordxYearlySum();

                            //_claimbdxyearsum.Id = new Guid();
                            _claimbdxyearsum.Reinsurer = reinsurerid;
                            _claimbdxyearsum.Insurer = insurerid;
                            _claimbdxyearsum.Year = minYear;
                            _claimbdxyearsum.BordxAmount = 0;
                            _claimbdxyearsum.IsConformed = true;

                            session.SaveOrUpdate(_claimbdxyearsum);

                            transaction.Commit();

                            result = (minYear == lastYear) ? true : false;
                        }
                        catch
                        {
                            result = false;
                            transaction.Rollback();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                result = false;
            }

            return result;
        }

        public ClaimBordxExportResponseDto GetProcessClaimBordxExport(Guid bordexId)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBordxExportResponseDto result = null;

            ClaimBordx claimBordxTable = session.Query<ClaimBordx>().Where(a => a.Id == bordexId).FirstOrDefault();

            try
            {
                result = new ClaimBordxExportResponseDto();

                String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\ClaimBordx.sql"));
                Query = Query.Replace("{bordexId}", bordexId.ToString());
                var claimBordx = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<ClaimBordxHeader>())
                .List<ClaimBordxHeader>();

                String Query1 = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxClaimSummary.sql"));
                Query1 = Query1.Replace("{bordexYear}", claimBordxTable.BordxYear.ToString());
                var claimSummries = session.CreateSQLQuery(Query1).SetResultTransformer(Transformers.AliasToBean<ClaimSummary>())
                .List<ClaimSummary>();

                String Query2 = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxClaimMonth&Year.sql"));
                Query2 = Query2.Replace("{bordexId}", bordexId.ToString());
                var claimMonthAndYears = session.CreateSQLQuery(Query2).SetResultTransformer(Transformers.AliasToBean<ClaimMonthAndYear>())
                .List<ClaimMonthAndYear>();

                String Query3 = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\PaymentSchedule.sql"));
                Query3 = Query3.Replace("{bordexId}", bordexId.ToString());
                var paymentSchedule = session.CreateSQLQuery(Query3).SetResultTransformer(Transformers.AliasToBean<PaymentSchedule>())
                .List<PaymentSchedule>();

                String Query4 = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\CountryWise.sql"));
                Query4 = Query4.Replace("{bordexId}", bordexId.ToString());
                var countryWise = session.CreateSQLQuery(Query4).SetResultTransformer(Transformers.AliasToBean<CountryWise>())
                .List<CountryWise>();

                result.ClaimBordxHeader = claimBordx.First();
                result.ClaimSummaries = claimSummries.ToList();
                result.ClaimMonthAndYears = claimMonthAndYears.ToList();
                result.PaymentSchedule = paymentSchedule.ToList();
                result.CountryWise = countryWise.ToList();
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        internal static BordxExportResponseDto GetConfirmedBordxForExport(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        {
            BordxExportResponseDto Response = new BordxExportResponseDto();
            if (IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString())
                && IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.userId.ToString()))
            {
                ISession session = EntitySessionManager.GetSession();
                List<BordxTaxInfo> MasterTaxInformationListByCountry = new List<BordxTaxInfo>();

                //bordx
                Bordx bordx = session.Query<Bordx>().Where(a => a.Id == ExportPoliciesToExcelByBordxIdRequestDto.bordxId).FirstOrDefault();

                String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExport.sql"));
                Query = Query.Replace("{bordexId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());
                var Policies = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<AddBordxSummeryRequestDto>())
                .List<AddBordxSummeryRequestDto>();

                List<Guid> AllCountryList = Policies.Select(a => a.BaseCountryId).Distinct().ToList();
                List<DataTable> FinalPolicyTableList = new List<DataTable>();
                foreach (Guid countryId in AllCountryList)
                {

                    MasterTaxInformationListByCountry.Add(DBDTOTransformer.Instance.GetBordxTaxDetailsByCountryId(countryId));
                    DataTable ConvertedTable = CreateDataTable(Policies.Where(a => a.BaseCountryId == countryId).ToList());
                    DataTable FinalPolicyTable = new DataTable();
                    FinalPolicyTable = ConvertedTable.Copy();
                    FinalPolicyTableList.Add(FinalPolicyTable);
                }
                //get usd currency rate @ now
                string currencyDetails = new CurrencyEntityManager().GetCurrentUSDRateByCountryId(AllCountryList.FirstOrDefault());
                //get headers and columns data
                List<BordxReportColumnHeaders> columnHeaders = session.Query<BordxReportColumnHeaders>()
                   .OrderBy(a => a.Sequance).ToList();
                List<BordxReportColumns> columns = session.Query<BordxReportColumns>().Where(a => a.IsActive).ToList();
                var TpaName = session.Query<TPA>().FirstOrDefault().Name;
                var TpaLogo = session.Query<TPA>().FirstOrDefault().Logo;

                var image = new ImageEntityManager().GetImageById(TpaLogo);

                Response = new BordxExportResponseDto()
                {
                    BordxData = FinalPolicyTableList,
                    BordxReportColumnHeaders = new DBDTOTransformer().GetBordxReportColumnHeadersToDto(columnHeaders),
                    BordxReportColumns = new DBDTOTransformer().GetBordxReportColumnsToDto(columns),
                    TpaName = TpaName,
                    BordxYear = bordx.Year.ToString(),
                    BordxMonth = bordx.Month.ToString(),
                    CountryTaxInfo = MasterTaxInformationListByCountry,
                    currentUSDConversionRate = decimal.Parse(currencyDetails.Split('-')[0]),
                    currencyCode = currencyDetails.Split('-')[1],
                    tpaLogo = image.DisplayImageSrc
                };

            }
            return Response;
        }

        private static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();
            foreach (PropertyInfo info in properties)
            {
                dataTable.Columns.Add(new DataColumn(info.Name, Nullable.GetUnderlyingType(info.PropertyType) ?? info.PropertyType));
            }

            foreach (T entity in list)
            {
                object[] values = new object[properties.Length];
                for (int i = 0; i < properties.Length; i++)
                {
                    if (properties[i].PropertyType == typeof(DateTime))
                    {
                        values[i] = Convert.ToDateTime(properties[i].GetValue(entity)).ToString("dd-MMM-yyyy");
                    }
                    else
                    {
                        values[i] = properties[i].GetValue(entity);
                    }

                }

                dataTable.Rows.Add(values);
            }

            return dataTable;
        }
        private static string GetDtoPropertyByTablePropertyName(String ColName)
        {
            if (ColName == "Accident and Health / Medical Expenses Date of Treatment") return "AccidentandHealthMedicalExpensesDateofTreatment";
            if (ColName == "Accident and Health / Medical Expenses Patient Name") return "AccidentandHealthMedicalExpensesPatientName";
            if (ColName == "Accident and Health / Medical Expenses Plan") return "AccidentandHealthMedicalExpensesPlan";
            if (ColName == "Accident and Health / Medical Expenses Treatment Type") return "AccidentandHealthMedicalExpensesTreatmentType";
            if (ColName == "Accident and Health / Medical Expenses Country of Treatment") return "AccidentandHealthMedicalExpensesCountryofTreatment";
            if (ColName == "Claim Details Catastrophe Name") return "ClaimDetailsCatastropheName";
            if (ColName == "Claim Details Cause of Loss Code") return "ClaimDetailsCauseofLossCode";
            if (ColName == "Claim Details Claim Status") return "ClaimDetailsClaimStatus";
            if (ColName == "Claim Details Claimant Name") return "ClaimDetailsDateClaimMade";
            if (ColName == "Claim Details Date Claim Made") return "ClaimDetailsDateClaimMade";
            if (ColName == "Claim Details Date Closed") return "ClaimDetailsDateClosed";
            if (ColName == "Claim Details Date First Advised / Notification Date") return "ClaimDetailsDateFirstAdvisedNotificationDate";
            if (ColName == "Claim Details Date of Loss (From)") return "ClaimDetailsDateofLossFrom";
            if (ColName == "Claim Details Date of Loss to") return "ClaimDetailsDateofLossto";
            if (ColName == "Claim Details Denial") return "ClaimDetailsDenial";
            if (ColName == "Claim Details Lloyd's Cat Code") return "ClaimDetailsLloydsCatCode";
            if (ColName == "Claim Details Loss Description") return "ClaimDetailsLossDescription";
            if (ColName == "Claim Details Refer to Underwriters") return "ClaimDetailsRefertoUnderwriters";
            if (ColName == "Claim Notes Amount Claimed") return "ClaimNotesAmountClaimed";
            if (ColName == "Claim Notes Claim not paid as within excess") return "ClaimNotesClaimnotpaidaswithinexcess";
            if (ColName == "Claim Notes Complaint Reason") return "ClaimNotesComplaintReason";
            if (ColName == "Claim Notes Date claim closed") return "ClaimNotesDateclaimclosed";
            if (ColName == "Claim Notes Date Claim Denied") return "ClaimNotesDateClaimDenied";
            if (ColName == "Claim Notes Date claim withdrawn") return "ClaimNotesDateclaimwithdrawn";
            if (ColName == "Claim Notes Date of Complaint") return "ClaimNotesDateofComplaint";
            if (ColName == "Claim Notes Ex gratia payment") return "ClaimNotesExgratiapayment";
            if (ColName == "Claim Notes In Litigation") return "ClaimNotesInLitigation";
            if (ColName == "Claim Notes Reason for Denial") return "ClaimNotesReasonforDenial";
            if (ColName == "Claim Status Changes Date Claim Amount Agreed") return "ClaimStatusChangesDateClaimAmountAgreed";
            if (ColName == "Claim Status Changes Date Claim Opened") return "ClaimStatusChangesDateClaimOpened";
            if (ColName == "Claim Status Changes Date Claims Paid") return "ClaimStatusChangesDateClaimsPaid";
            if (ColName == "Claim Status Changes Date Coverage Confirmed") return "ClaimStatusChangesDateCoverageConfirmed";
            if (ColName == "Claim Status Changes Date Fees Paid") return "ClaimStatusChangesDateFeesPaid";
            if (ColName == "Claim Status Changes Date of Subrogation") return "ClaimStatusChangesDateofSubrogation";
            if (ColName == "Claim Status Changes Date Reopened") return "ClaimStatusChangesDateReopened";
            if (ColName == "Claimant Address Claimant Address") return "ClaimantAddressClaimantAddress";
            if (ColName == "Claimant Address Claimant Country") return "ClaimantAddressClaimantCountry";
            if (ColName == "Claimant Address Claimant Postcode") return "ClaimantAddressClaimantPostcode";
            if (ColName == "Class of Business Specific % Ceded (Reinsurance)") return "ClassofBusinessSpecificPercentCededReinsurance";
            if (ColName == "Class of Business Specific Name or Reg No of Aircraft Vehicle, Vessel etc") return "ClassofBusinessSpecificNameorRegNoofAircraftVehicleVesseletc";
            if (ColName == "Contract Details Agreement No") return "ContractDetailsAgreementNo";
            if (ColName == "Contract Details Class of Business") return "ContractDetailsClassofBusiness";
            if (ColName == "Contract Details Contract Expiry") return "ContractDetailsContractExpiry";
            if (ColName == "Contract Details Contract Inception") return "ContractDetailsContractInception";
            if (ColName == "Contract Details Coverholder Name") return "ContractDetailsCoverholderName";
            if (ColName == "Contract Details Coverholder PIN") return "ContractDetailsCoverholderPIN";
            if (ColName == "Contract Details Lloyd's Risk Code") return "ContractDetailsLloydsRiskCode";
            if (ColName == "Contract Details Original Currency") return "ContractDetailsOriginalCurrency";
            if (ColName == "Contract Details Rate of Exchange") return "ContractDetailsRateofExchange";
            if (ColName == "Contract Details Reporting Period (End Date)") return "ContractDetailsReportingPeriodEndDate";
            if (ColName == "Contract Details Reporting Period Start Date") return "ContractDetailsReportingPeriodStartDate";
            if (ColName == "Contract Details Section No") return "ContractDetailsSectionNo";
            if (ColName == "Contract Details Settlement Currency") return "ContractDetailsSettlementCurrency";
            if (ColName == "Contract Details TPA Name") return "ContractDetailsTPAName";
            if (ColName == "Contract Details Type of Insurance (Direct, or Type of RI)") return "ContractDetailsTypeofInsuranceDirectorTypeofRI";
            if (ColName == "Contract Details Unique Market Reference (UMR)") return "ContractDetailsUniqueMarketReferenceUMR";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc Role") return "ExpertsLawyerAdjusterAttorneyetcRole";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Address") return "ExpertsLawyerAdjusterAttorneyetcAddress";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Country") return "ExpertsLawyerAdjusterAttorneyetcCountry";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Firm / Company Name") return "ExpertsLawyerAdjusterAttorneyetcFirmCompanyName";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Notes") return "ExpertsLawyerAdjusterAttorneyetcNotes";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Postcode / Zip Code or similar") return "ExpertsLawyerAdjusterAttorneyetcPostcodeZipCodeorsimilar";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) Reference No etc") return "ExpertsLawyerAdjusterAttorneyetcReferenceNoetc";
            if (ColName == "Experts (Lawyer/Adjuster/Attorney etc) State, Province, Territory, Canton etc") return "ExpertsLawyerAdjusterAttorneyetcStateProvinceTerritoryCantonetc";
            if (ColName == "Insured Details Address") return "InsuredDetailsAddress";
            if (ColName == "Insured Details Country") return "InsuredDetailsCountry";
            if (ColName == "Insured Details Full Name or Company Name") return "InsuredDetailsFullNameorCompanyName";
            if (ColName == "Insured Details Postcode / Zip Code or similar") return "InsuredDetailsPostcodeZipCodeorsimilar";
            if (ColName == "Insured Details State, Province, Territory, Canton etc") return "InsuredDetailsStateProvinceTerritoryCantonetc";
            if (ColName == "Location of Loss Address") return "LocationofLossAddress";
            if (ColName == "Location of Loss Country") return "LocationofLossCountry";
            if (ColName == "Location of Loss Postcode / Zip Code or similar") return "LocationofLossPostcodeZipCodeorsimilar";
            if (ColName == "Location of Loss State, Province, Territory, Canton etc") return "LocationofLossStateProvinceTerritoryCantonetc";
            if (ColName == "Location of Risk Address") return "LocationofRiskAddress";
            if (ColName == "Location of Risk Country") return "LocationofRiskCountry";
            if (ColName == "Location of Risk Location ID") return "LocationofRiskLocationID";
            if (ColName == "Location of Risk Postcode / Zip Code or similar") return "LocationofRiskPostcodeZipCodeorsimilar";
            if (ColName == "Location of Risk State, Province, Territory, Canton etc") return "LocationofRiskStateProvinceTerritoryCantonetc";
            if (ColName == "References Certificate Reference") return "ReferencesCertificateReference";
            if (ColName == "References Claim Reference") return "ReferencesClaimReference";
            if (ColName == "Refs Policy or Group Ref") return "RefsPolicyorGroupRef";
            if (ColName == "Risk Details Deductible Amount") return "RiskDetailsDeductibleAmount";
            if (ColName == "Risk Details Deductible Basis") return "RiskDetailsDeductibleBasis";
            if (ColName == "Risk Details Period of Cover - Narrative") return "RiskDetailsPeriodofCoverNarrative";
            if (ColName == "Risk Details Risk Expiry Date") return "RiskDetailsRiskExpiryDate";
            if (ColName == "Risk Details Risk Inception Date") return "RiskDetailsRiskInceptionDate";
            if (ColName == "Risk Details Sums Insured Amount") return "RiskDetailsSumsInsuredAmount";
            if (ColName == "Row No Row No") return "RowNo";
            if (ColName == "Transaction Details Change this month - Fees") return "TransactionDetailsChangethismonthFees";
            if (ColName == "Transaction Details Change this month - Indemnity") return "TransactionDetailsChangethismonthIndemnity";
            if (ColName == "Transaction Details Paid this month - Adjusters Fees") return "TransactionDetailsPaidthismonthAdjustersFees";
            if (ColName == "Transaction Details Paid this month - Attorney Coverage Fees") return "TransactionDetailsPaidthismonthAttorneyCoverageFees";
            if (ColName == "Transaction Details Paid this month - Defence Fees") return "TransactionDetailsPaidthismonthDefenceFees";
            if (ColName == "Transaction Details Paid this month - Expenses") return "TransactionDetailsPaidthismonthExpenses";
            if (ColName == "Transaction Details Paid this month - Fees") return "TransactionDetailsPaidthismonthFees";
            if (ColName == "Transaction Details Paid this month - Indemnity") return "TransactionDetailsPaidthismonthIndemnity";
            if (ColName == "Transaction Details Paid this month - TPA Fees") return "TransactionDetailsPaidthismonthTPAFees";
            if (ColName == "Transaction Details Previously Paid - Adjusters Fees") return "TransactionDetailsPreviouslyPaidAdjustersFees";
            if (ColName == "Transaction Details Previously Paid - Attorney Coverage Fees") return "TransactionDetailsPreviouslyPaidAttorneyCoverageFees";
            if (ColName == "Transaction Details Previously Paid - Defence Fees") return "TransactionDetailsPreviouslyPaidDefenceFees";
            if (ColName == "Transaction Details Previously Paid - Expenses") return "TransactionDetailsPreviouslyPaidExpenses";
            if (ColName == "Transaction Details Previously Paid - Fees") return "TransactionDetailsPreviouslyPaidFees";
            if (ColName == "Transaction Details Previously Paid - Indemnity") return "TransactionDetailsPreviouslyPaidIndemnity";
            if (ColName == "Transaction Details Previously Paid - TPA Fees") return "TransactionDetailsPreviouslyPaidTPAFees";
            if (ColName == "Transaction Details Reserve - Adjusters Fees") return "TransactionDetailsReserveAdjustersFees";
            if (ColName == "Transaction Details Reserve - Attorney Coverage Fees") return "TransactionDetailsReserveAttorneyCoverageFees";
            if (ColName == "Transaction Details Reserve - Defence Fees") return "TransactionDetailsReserveDefenceFees";
            if (ColName == "Transaction Details Reserve - Expenses") return "TransactionDetailsReserveExpenses";
            if (ColName == "Transaction Details Reserve - Fees") return "TransactionDetailsReserveFees";
            if (ColName == "Transaction Details Reserve - Indemnity") return "TransactionDetailsReserveIndemnity";
            if (ColName == "Transaction Details Reserve - TPA Fees") return "TransactionDetailsReserveTPAFees";
            if (ColName == "Transaction Details Total Incurred") return "TransactionDetailsTotalIncurred";
            if (ColName == "Transaction Details Total Incurred - Fees") return "TransactionDetailsTotalIncurredFees";
            if (ColName == "Transaction Details Total Incurred - Indemnity") return "TransactionDetailsTotalIncurredIndemnity";
            if (ColName == "US Details Loss County") return "USDetailsLossCounty";
            if (ColName == "US Details Medicare Conditional Payments") return "USDetailsMedicareConditionalPayments";
            if (ColName == "US Details Medicare Eligibility Check Performance") return "USDetailsMedicareEligibilityCheckPerformance";
            if (ColName == "US Details Medicare MSP Compliance Services") return "USDetailsMedicareMSPComplianceServices";
            if (ColName == "US Details Medicare Outcome of Eligilibility Status Check") return "USDetailsMedicareOutcomeofEligilibilityStatusCheck";
            if (ColName == "US Details Medicare United States Bodily Injury") return "USDetailsMedicareUnitedStatesBodilyInjury";
            if (ColName == "US Details PCS Code") return "USDetailsPCSCode";
            if (ColName == "US Details State of Filing") return "USDetailsStateofFiling";
            else return ColName;

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

    }
}
