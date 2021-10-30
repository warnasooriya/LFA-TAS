using NHibernate;
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
    public class BordxEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Bordx> GetBordxs()
        {
            List<Bordx> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Bordx> BordxData = session.Query<Bordx>();
            entities = BordxData.ToList();
            return entities;
        }

        public BordxResponseDto GetBordxById(Guid BordxId)
        {
            ISession session = EntitySessionManager.GetSession();

            BordxResponseDto pDto = new BordxResponseDto();

            var query =
                from Bordx in session.Query<Bordx>()
                where Bordx.Id == BordxId
                select new { Bordx = Bordx };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Bordx.Id;
                pDto.IsConformed = result.First().Bordx.IsConformed;
                pDto.IsProcessed = result.First().Bordx.IsProcessed;
                pDto.Month = result.First().Bordx.Month;
                pDto.Year = result.First().Bordx.Year;
                pDto.EntryDateTime = result.First().Bordx.EntryDateTime;
                pDto.EntryUser = result.First().Bordx.EntryUser;

                pDto.IsBordxExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBordxExists = false;
                return null;
            }
        }



        internal bool AddBordx(BordxRequestDto Bordx)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Bordx pr = new Entities.Bordx();

                pr.Id = new Guid();
                pr.IsConformed = Bordx.IsConformed;
                pr.IsProcessed = Bordx.IsProcessed;
                pr.Month = Bordx.Month;
                pr.Year = Bordx.Year;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    Bordx.Id = pr.Id;
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


        internal bool UpdateBordx(BordxRequestDto Bordx)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Bordx pr = new Entities.Bordx();

                pr.Id = Bordx.Id;
                pr.IsConformed = Bordx.IsConformed;
                pr.IsProcessed = Bordx.IsProcessed;
                pr.Month = Bordx.Month;
                pr.Year = Bordx.Year;
                pr.EntryDateTime = Bordx.EntryDateTime;
                pr.EntryUser = Bordx.EntryUser;
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

        public List<BordxDetails> GetBordxsDetails()
        {
            List<BordxDetails> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<BordxDetails> BordxData = session.Query<BordxDetails>();
            entities = BordxData.ToList();
            return entities;
        }

        public BordxDetailsResponseDto GetBordxDetailsById(Guid BordxId)
        {
            ISession session = EntitySessionManager.GetSession();

            BordxDetailsResponseDto pDto = new BordxDetailsResponseDto();

            var query =
                from Bordx in session.Query<BordxDetails>()
                where Bordx.Id == BordxId
                select new { Bordx = Bordx };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Bordx.Id;
                pDto.PolicyId = result.First().Bordx.PolicyId;
                pDto.BordxId = result.First().Bordx.BordxId;

                pDto.IsBordxDetailsExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBordxDetailsExists = false;
                return null;
            }
        }

        internal bool AddBordxDetails(BordxDetailsRequestDto Bordx)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                BordxDetails pr = new Entities.BordxDetails();

                List<BordxDetails> entities = null;
                IQueryable<BordxDetails> BordxData = session.Query<BordxDetails>();
                entities = BordxData.Where(b => b.PolicyId == Bordx.PolicyId && b.BordxId == Bordx.BordxId).ToList();

                if (entities.Count > 0)
                {
                    Bordx.Id = entities[0].Id;
                    return true;
                }
                pr.Id = new Guid();
                pr.PolicyId = Bordx.PolicyId;
                pr.BordxId = Bordx.BordxId;

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

        internal bool UpdateBordxDetails(BordxDetailsRequestDto Bordx)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                BordxDetails pr = new Entities.BordxDetails();


                pr.Id = Bordx.Id;
                pr.PolicyId = Bordx.PolicyId;
                pr.BordxId = Bordx.BordxId;

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

        public List<BordxReportColumns> GetBordxReportColumnses(BordxColumnRequestDto bordxColumnRequestDto)
        {
            List<BordxReportColumns> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<BordxReportColumns> BordxData = session.Query<BordxReportColumns>().Where(a=>a.ProductType== bordxColumnRequestDto.ProductType);
            entities = BordxData.ToList();
            return entities;
        }

        public List<BordxReportColumnsMap> GetBordxReportColumnsMaps(Guid UserId)
        {
            List<BordxReportColumnsMap> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<BordxReportColumnsMap> BordxData = session.Query<BordxReportColumnsMap>();
            entities = BordxData.Where(b => b.UserId == UserId).ToList();
            return entities;
        }

        internal bool AddBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                BordxReportColumnsMap pr = new Entities.BordxReportColumnsMap();

                pr.Id = new Guid();
                pr.ColumnId = Bordx.ColumnId;
                pr.IsActive = Bordx.IsActive;
                pr.UserId = Bordx.UserId;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    Bordx.Id = pr.Id;
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

        internal bool UpdateBordxReportColumnsMap(BordxReportColumnsMapRequestDto Bordx)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                BordxReportColumns col = session.Query<BordxReportColumns>()
                    .Where(a => a.Id == Bordx.Id).FirstOrDefault();
                if (col != null)
                {
                    col.IsActive = Bordx.IsActive;
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(col, col.Id);
                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public BordxReportColumnsMapResponseDto GetBordxReportColumnsMapById(Guid BordxId)
        {
            ISession session = EntitySessionManager.GetSession();

            BordxReportColumnsMapResponseDto pDto = new BordxReportColumnsMapResponseDto();

            var query =
                from Bordx in session.Query<BordxReportColumnsMap>()
                where Bordx.Id == BordxId
                select new { Bordx = Bordx };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Bordx.Id;
                pDto.ColumnId = result.First().Bordx.ColumnId;
                pDto.IsActive = result.First().Bordx.IsActive;
                pDto.UserId = result.First().Bordx.UserId;

                pDto.IsBordxReportColumnsMapExists = true;
                return pDto;
            }
            else
            {
                pDto.IsBordxReportColumnsMapExists = false;
                return null;
            }
        }



        internal static object GetConfirmedConfirmedBordxForGrid
            (BordxSearchGridSearchCriterias bordxSearchGridSearchCriterias,
            PaginationOptionsbordxSearchGrid paginationOptionsbordxSearchGrid,
            string action)
        {
            CommonEntityManager cem = new CommonEntityManager();
            ISession session = EntitySessionManager.GetSession();
            //expression builder for bordx
            Expression<Func<Bordx, bool>> filterBordx = PredicateBuilder.True<Bordx>();

            if (bordxSearchGridSearchCriterias.year > 0)
                filterBordx = filterBordx.And(p => p.Year == bordxSearchGridSearchCriterias.year);
            if (bordxSearchGridSearchCriterias.month > 0)
                filterBordx = filterBordx.And(p => p.Month == bordxSearchGridSearchCriterias.month);
            if (IsGuid(bordxSearchGridSearchCriterias.commodityTypeId.ToString()))
                filterBordx = filterBordx.And(p => p.CommodityTypeId == bordxSearchGridSearchCriterias.commodityTypeId);
            if (IsGuid(bordxSearchGridSearchCriterias.insureId.ToString()))
                filterBordx = filterBordx.And(p => p.Insurer == bordxSearchGridSearchCriterias.insureId);
            if (IsGuid(bordxSearchGridSearchCriterias.reinsureId.ToString()))
                filterBordx = filterBordx.And(p => p.Reinsurer == bordxSearchGridSearchCriterias.reinsureId);
             if (IsGuid(bordxSearchGridSearchCriterias.productId.ToString()))
                filterBordx = filterBordx.And(p => p.ProductId == bordxSearchGridSearchCriterias.productId);


            if (action == "bordxview")
            {
                filterBordx = filterBordx.And(p => p.IsConformed);
            }
            else if (action == "bordxreopen")
            {
                filterBordx = filterBordx.And(p => p.IsConformed);
            }
            else
            {
                return new object();
            }



            //get bordx list
            IEnumerable<Bordx> currentBordxList = session.Query<Bordx>().Where(filterBordx)
                .OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).ThenByDescending(a => a.Number);
            if (action == "bordxreopen")
                currentBordxList = currentBordxList.GroupBy(x => new { x.CommodityTypeId, x.Insurer,x.Reinsurer , x.ProductId, x.Year, x.Month,x.Number },
                    (key, y) => y.OrderBy(z => z.Number).First());



            var policyGridDetails = currentBordxList
               .Select(a => new
               {
                   a.Id,
                   CommodityType = cem.GetCommodityTypeNameById(a.CommodityTypeId),
                   //Country = cem.GetCountryNameById(),
                   Insurer = cem.GetInsurerNameById(a.Insurer),
                   Reinsurer = cem.GetReinsurerNameById(a.Reinsurer),
                   Product = cem.GetProductCodeById(a.ProductId),
                   ProductId = a.ProductId,
                   a.Year,
                   a.Month,
                   a.Number,
                   StartDate = a.StartDate.ToString("dd-MMM-yyyy"),
                   EndDate = a.EndDate.ToString("dd-MMM-yyyy")
               });
            long TotalRecords = policyGridDetails.Count();
            var policyGridDetailsFilterd = policyGridDetails.Skip((paginationOptionsbordxSearchGrid.pageNumber - 1) * paginationOptionsbordxSearchGrid.pageSize)
            .Take(paginationOptionsbordxSearchGrid.pageSize)
            .ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = policyGridDetailsFilterd
            };
            return new JavaScriptSerializer().Serialize(response);
        }

        private static object GetDealerNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Dealer>().Where(a => a.Id == guid).SingleOrDefault().DealerName;
        }

        private static object GetCommodityNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CommodityType>().Where(a => a.CommodityTypeId == guid).SingleOrDefault().CommodityTypeDescription;
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

        internal static List<int> GetConfirmedBordxYears()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Bordx>()
               .Select(c => c.Year).Distinct().ToList();
        }

        internal static BordxExportResponseDto GetConfirmedBordxForExport(ExportPoliciesToExcelByBordxIdRequestDto ExportPoliciesToExcelByBordxIdRequestDto)
        {
            BordxExportResponseDto Response = new BordxExportResponseDto();

            try
            {
                if (IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString())
                && IsGuid(ExportPoliciesToExcelByBordxIdRequestDto.userId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    List<BordxTaxInfo> MasterTaxInformationListByCountry = new List<BordxTaxInfo>();

                    //bordx
                    Bordx bordx = session.Query<Bordx>().Where(a => a.Id == ExportPoliciesToExcelByBordxIdRequestDto.bordxId).FirstOrDefault();

                    Product product = session.Query<Product>().Where(a => a.Id == bordx.ProductId).FirstOrDefault();

                    CommodityType commodityType = session.Query<CommodityType>().Where(b => b.CommodityTypeId == bordx.CommodityTypeId).FirstOrDefault();

                    String reportNameQuery = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExportReportName.sql"));

                    reportNameQuery = reportNameQuery.Replace("{BordxReportTemplateId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxReportTemplateId.ToString());

                    reportNameQuery = reportNameQuery.Replace("{bordexId}",ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());
                    reportNameQuery = reportNameQuery.Replace("{ProductName}", product.Productname.ToString().Replace(" ","_"));


                    var ReportName = session.CreateSQLQuery(reportNameQuery).SetResultTransformer(Transformers.AliasToBean<ExcelBordxReportNameRequestDto>()).List<ExcelBordxReportNameRequestDto>();

                    if (product.Productcode.ToUpper() == "TYRE" || product.Productcode.ToUpper() == "ADT")
                    {
                        String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExportTyre.sql"));
                        Query = Query.Replace("{bordexId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());
                        if (ExportPoliciesToExcelByBordxIdRequestDto.dealerId == Guid.Empty)
                        {
                            Query = Query.Replace("{dealerFilter}", " ");
                        }
                        else
                        {
                            Query = Query.Replace("{dealerFilter}", "  AND d.Id='" + ExportPoliciesToExcelByBordxIdRequestDto.dealerId.ToString() + "'");
                        }
                        var Policies = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<BordxViewTyreResponseDto>())
                        .List<BordxViewTyreResponseDto>();


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

                        List<BordxReportColumns> bordxReportcolumnslist = session.Query<BordxReportColumns>().Where(a => a.IsActive && a.ProductType==product.ProductTypeId).OrderBy(o=>o.Sequance).ToList();
                        List<BordxReportTemplateDetails> bordxReportTemplateDetailsList = session.Query<BordxReportTemplateDetails>().Where(a => a.IsActive && a.BordxReportTemplateId == ExportPoliciesToExcelByBordxIdRequestDto.bordxReportTemplateId).ToList();
                        BordxReportTemplate bordxReportTemplate = session.Query<BordxReportTemplate>().Where(a => a.Id == bordxReportTemplateDetailsList.FirstOrDefault().BordxReportTemplateId).FirstOrDefault();

                        List<BordxReportColumns> columns = (from bordxReportcolumns in bordxReportcolumnslist
                                                            join template in bordxReportTemplateDetailsList
                                                            on bordxReportcolumns.Id equals template.BordxReportColumnsId
                                                            select new BordxReportColumns
                                                            {
                                                                Id = bordxReportcolumns.Id,
                                                                DisplayName = bordxReportcolumns.DisplayName,
                                                                HeaderId = bordxReportcolumns.HeaderId,
                                                                IsActive = bordxReportcolumns.IsActive,
                                                                KeyName = bordxReportcolumns.KeyName,
                                                                Sequance = bordxReportcolumns.Sequance,
                                                                ColumnWidth = bordxReportcolumns.ColumnWidth,
                                                                Alignment = bordxReportcolumns.Alignment
                                                            }).ToList();

                        var columnHdrlist = session.Query<BordxReportColumnHeaders>().OrderBy(a => a.Sequance).ToList();

                        var colHeaders = (from columnHeader in columnHdrlist
                                          join column in columns
                                          on columnHeader.Id equals column.HeaderId
                                          group columnHeader by new { columnHeader.Id, columnHeader.HeaderName, columnHeader.Sequance, columnHeader.GenarateSum } into grpHdr
                                          select new BordxReportColumnHeaders
                                          {
                                              Id = grpHdr.Key.Id,
                                              HeaderName = grpHdr.Key.HeaderName,
                                              Sequance = grpHdr.Key.Sequance,
                                              GenarateSum = grpHdr.Key.GenarateSum
                                          }).ToList();

                        List<BordxReportColumnHeaders> columnHeaders = colHeaders.OrderBy(a => a.Sequance).ToList();

                        //List<BordxReportColumns> columns = session.Query<BordxReportColumns>().Where(a => a.IsActive).ToList();


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
                            tpaLogo = image.DisplayImageSrc,
                            reportName = ReportName.First().ReportName,
                            BordxReportTemplateName = bordxReportTemplate.Name,
                            BordxStartDate = bordx.StartDate.ToString("dd-MMM-yyyy"),
                            BordxEndDate = bordx.EndDate.ToString("dd-MMM-yyyy")
                        };
                    }
                    else
                    {
                        String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExport.sql"));
                        Query = Query.Replace("{bordexId}", ExportPoliciesToExcelByBordxIdRequestDto.bordxId.ToString());

                        if (ExportPoliciesToExcelByBordxIdRequestDto.dealerId == Guid.Empty)
                        {
                            Query = Query.Replace("{dealerFilter}", " ");
                        }
                        else {
                            Query = Query.Replace("{dealerFilter}", "  AND d.Id='" + ExportPoliciesToExcelByBordxIdRequestDto.dealerId.ToString() + "'");
                        }
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

                        List<BordxReportColumns> bordxReportcolumnslist = session.Query<BordxReportColumns>().Where(a => a.IsActive && a.ProductType==product.ProductTypeId).ToList();
                        List<BordxReportTemplateDetails> bordxReportTemplateDetailsList = session.Query<BordxReportTemplateDetails>().Where(a => a.IsActive && a.BordxReportTemplateId == ExportPoliciesToExcelByBordxIdRequestDto.bordxReportTemplateId).ToList();
                        BordxReportTemplate bordxReportTemplate = session.Query<BordxReportTemplate>().Where(a => a.Id == bordxReportTemplateDetailsList.FirstOrDefault().BordxReportTemplateId).FirstOrDefault();

                        List<BordxReportColumns> columns = (from bordxReportcolumns in bordxReportcolumnslist
                                                            join template in bordxReportTemplateDetailsList
                                                            on bordxReportcolumns.Id equals template.BordxReportColumnsId
                                                            select new BordxReportColumns
                                                            {
                                                                Id = bordxReportcolumns.Id,
                                                                DisplayName = bordxReportcolumns.DisplayName,
                                                                HeaderId = bordxReportcolumns.HeaderId,
                                                                IsActive = bordxReportcolumns.IsActive,
                                                                KeyName = bordxReportcolumns.KeyName,
                                                                Sequance = bordxReportcolumns.Sequance,
                                                                ColumnWidth = bordxReportcolumns.ColumnWidth,
                                                                Alignment = bordxReportcolumns.Alignment
                                                            }).ToList();

                        var columnHdrlist = session.Query<BordxReportColumnHeaders>().OrderBy(a => a.Sequance).ToList();

                        var colHeaders = (from columnHeader in columnHdrlist
                                          join column in columns
                                          on columnHeader.Id equals column.HeaderId
                                          group columnHeader by new { columnHeader.Id, columnHeader.HeaderName, columnHeader.Sequance , columnHeader.GenarateSum } into grpHdr
                                          select new BordxReportColumnHeaders
                                          {
                                              Id = grpHdr.Key.Id,
                                              HeaderName = grpHdr.Key.HeaderName,
                                              Sequance = grpHdr.Key.Sequance,
                                              GenarateSum = grpHdr.Key.GenarateSum
                                          }).ToList();

                        List<BordxReportColumnHeaders> columnHeaders = colHeaders.OrderBy(a => a.Sequance).ToList();

                        //List<BordxReportColumns> columns = session.Query<BordxReportColumns>().Where(a => a.IsActive).ToList();


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
                            tpaLogo = image.DisplayImageSrc,
                            reportName = ReportName.First().ReportName,
                            BordxReportTemplateName = bordxReportTemplate.Name,
                            BordxStartDate = bordx.StartDate.ToString("dd-MMM-yyyy"),
                            BordxEndDate = bordx.EndDate.ToString("dd-MMM-yyyy")
                        };

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return Response;
        }

        private static DataTable CreateDataTable<T>(IEnumerable<T> list)
        {
            Type type = typeof(T);
            var properties = type.GetProperties();

            DataTable dataTable = new DataTable();

            try
            {
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
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
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

        internal static object GetConfirmedBordxForView(BordxViewGridSearchCriterias bordxViewGridSearchCriterias, PaginationOptionsbordxSearchGrid paginationOptionsbordxSearchGrid)
        {
            CommonGridResponseDto Response = new CommonGridResponseDto();

            try
            {
                if (IsGuid(bordxViewGridSearchCriterias.bordxId.ToString()))
                {
                    //ExportPoliciesToExcelByBordxIdRequestDto.userId = Guid.Parse("BA56EC84-1FE0-4385-ABE4-182F62CAA050");
                    ISession session = EntitySessionManager.GetSession();

                    String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BordxExport.sql"));
                    Query = Query.Replace("{bordexId}", bordxViewGridSearchCriterias.bordxId.ToString());
                    var Policies = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<AddBordxSummeryRequestDto>())
                    .List<AddBordxSummeryRequestDto>();
                    var totalRecords = Policies.Count;
                    var policyGridDetailsFilterd = Policies
                        .Skip(paginationOptionsbordxSearchGrid.pageNumber - 1)
                        .Take(paginationOptionsbordxSearchGrid.pageSize)
                        .Select(a => new
                        {
                            //a.ContractDetailsCoverholderName,
                            //a.ContractDetailsAgreementNo,
                            //a.ContractDetailsTPAName,
                            //a.ContractDetailsReportingPeriodStartDate,
                            //a.ContractDetailsReportingPeriodEndDate,
                            //a.ContractDetailsClassofBusiness,
                            //a.ContractDetailsSettlementCurrency,
                            //a.ReferencesCertificateReference,
                            //a.ClaimantAddressClaimantCountry,
                            //a.ClaimantAddressClaimantPostcode
                        }).ToArray();

                    Response = new CommonGridResponseDto()
                    {
                        totalRecords = totalRecords,
                        data = policyGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(Response);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        //internal static object GetBordexAllowedYearsMonths()
        //{
        //    Object Response = new object();
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        List<int> bordxYearList = session.Query<Bordx>()
        //            .Select(a => a.Year).Distinct().ToList();

        //        List<int> policyYearList = session.Query<Policy>()
        //            .Select(a => a.PolicySoldDate.Year).Distinct().ToList();

        //        List<int> totaYears = policyYearList.Union(bordxYearList).OrderByDescending(a => a).ToList();
        //        int lastYear = 0;
        //        if (totaYears != null && totaYears.Count() > 0)
        //        {
        //            lastYear = totaYears.FirstOrDefault();
        //        }
        //        else
        //        {

        //            lastYear = DateTime.UtcNow.Year;
        //            totaYears.Add(lastYear);
        //        }
        //        totaYears.Add(lastYear + 1);

        //        //months
        //        int defaultMonth = 0;
        //        Bordx lastConfirmedBordx = session.Query<Bordx>()
        //            .Where(a => a.IsConformed == true)
        //            .OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).FirstOrDefault();
        //        if (lastConfirmedBordx == null)
        //        {
        //            defaultMonth = DateTime.UtcNow.Month;
        //        }
        //        else
        //        {
        //            defaultMonth = lastConfirmedBordx.Month == 12 ? 1 : lastConfirmedBordx.Month++;

        //        }
        //        List<MonthsDto> monthsList = new List<MonthsDto>();
        //        foreach (Months month in Enum.GetValues(typeof(Months)))
        //        {
        //            //monthsDict.Add((int)month, month.ToString());
        //            MonthsDto monthObj = new MonthsDto()
        //            {
        //                monthsName = month.ToString(),
        //                monthsSeq = (int)month,
        //                isDefault = defaultMonth == (int)month ? true : false
        //            };
        //            monthsList.Add(monthObj);
        //        }


        //        MonthsAndYearsResponseDto response = new MonthsAndYearsResponseDto()
        //        {
        //            months = monthsList.ToArray(),
        //            years = totaYears.OrderByDescending(i => i).ToArray()
        //        };
        //        Response = response;

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return Response;
        //}

        internal static object GetAllBordxDetailsByYearMonth(BordxDetailsByYearMonthRequestDto BordxDetailsByYearMonthRequestDto)
        {
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currem = new CurrencyEntityManager();
                List<Policy> entities = new List<Policy>();
                //List<PolicyResponseDto> entities = new List<PolicyResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                Bordx bordx = session.Query<Bordx>()
                    .Where(a => a.Year == BordxDetailsByYearMonthRequestDto.year
                        && a.Month == BordxDetailsByYearMonthRequestDto.month
                        && a.Number == BordxDetailsByYearMonthRequestDto.number
                        && a.Insurer == BordxDetailsByYearMonthRequestDto.insurerId
                        && a.Reinsurer == BordxDetailsByYearMonthRequestDto.reinsurerId
                        && a.ProductId == BordxDetailsByYearMonthRequestDto.productId
                        //&& a.CountryId == BordxDetailsByYearMonthRequestDto.countryId
                        && a.CommodityTypeId == BordxDetailsByYearMonthRequestDto.commodityTypeId).FirstOrDefault();
                IEnumerable<Policy> policies;
                if (bordx == null)
                {
                    var emptyObj = new CommonGridResponseDto()
                    {
                        totalRecords = 0,
                        data = new object()
                    };
                    return new JavaScriptSerializer().Serialize(emptyObj);
                }
                else
                {
                    if (bordx.IsConformed == true)
                    {
                        //policies = session.Query<Policy>()
                        //.Where(a => a.BordxId == bordx.Id);

                        IEnumerable<BordxDetails> policyIds = session.Query<BordxDetails>().Where(a => a.BordxId == bordx.Id);
                        entities = session.Query<Policy>()
                       .Where(a => policyIds.Any(c => c.PolicyId == a.Id)).ToList();


                    }
                    else
                    {
                        IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                            .Where(a => a.ReinsurerId == BordxDetailsByYearMonthRequestDto.reinsurerId);
                        IEnumerable<Contract> contracts = session.Query<Contract>()
                        .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerContractId));

                        entities =
                   session.CreateSQLQuery("exec BordxProcess :StartDate,:EndDate,:CommodityTypeId,:BordxYear,:BordxMonth,:BordxNumber,:BordxId,:ProductId")
                       .SetDateTime("StartDate", bordx.StartDate.AddHours(-12))
                       .SetDateTime("EndDate", bordx.EndDate.AddHours(12))
                       .SetGuid("CommodityTypeId", BordxDetailsByYearMonthRequestDto.commodityTypeId)
                       .SetInt32("BordxYear", BordxDetailsByYearMonthRequestDto.year)
                       .SetInt32("BordxMonth", BordxDetailsByYearMonthRequestDto.month)
                       .SetInt32("BordxNumber", BordxDetailsByYearMonthRequestDto.number)
                       .SetGuid("BordxId", bordx.Id)
                       .SetGuid("ProductId", bordx.ProductId)
                       .SetResultTransformer(Transformers.AliasToBean<Policy>()).List<Policy>().ToList();


                        // policies = session.Query<Policy>()
                        //.Where(a => (a.BordxId == null || a.BordxId == Guid.Empty && a.IsApproved == true ) &&
                        //        (
                        //             (a.Year == 0 && a.Month == 0) &&
                        //             (a.ApprovedDate > bordx.StartDate.AddHours(-12) && a.ApprovedDate <= bordx.EndDate.AddHours(12)) &&
                        //             a.CommodityTypeId == BordxDetailsByYearMonthRequestDto.commodityTypeId &&
                        //             contracts.Any(b => b.Id == a.ContractId)
                        //        )||
                        //        (
                        //                 a.IsApproved == true &&
                        //                 a.Year == BordxDetailsByYearMonthRequestDto.year &&
                        //                 a.Month == BordxDetailsByYearMonthRequestDto.month &&
                        //                 a.BordxNumber == BordxDetailsByYearMonthRequestDto.number &&
                        //                 a.CommodityTypeId == BordxDetailsByYearMonthRequestDto.commodityTypeId &&
                        //                 contracts.Any(b => b.Id == a.ContractId)
                        //         )
                        //    );
                    }
                }

                long TotalRecords = entities.Count();
                var policyGridDetailsFilterd = entities.OrderBy(a=>a.ApprovedDate).Skip((BordxDetailsByYearMonthRequestDto.page - 1) * BordxDetailsByYearMonthRequestDto.pageSize)
                .Take(BordxDetailsByYearMonthRequestDto.pageSize)
                .Select(a => new
                {
                    a.Id,
                    a.PolicyNo,
                    CommodityType = cem.GetCommodityTypeNameById(a.CommodityTypeId),
                    Premium = currem.ConvertFromBaseCurrency(a.Premium, a.PremiumCurrencyTypeId, a.CurrencyPeriodId),
                    Currency = cem.GetCurrencyTypeById(a.CustomerPaymentCurrencyTypeId),
                    PolicySoldDate = a.ApprovedDate.ToString("dd-MMM-yyyy"),
                    Dealer = cem.GetDealerNameById(a.DealerId),
                    IsConfirm = bordx.IsConformed
                })
                .ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = policyGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return new JavaScriptSerializer().Serialize("");
            }

        }

        internal static string ProcessBordx(BordxProcessRequestDto BordxProcessRequestDto)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<Policy> policies = new List<Policy>();
                Bordx bordx = session.Query<Bordx>()
                   .Where(a => a.Year == BordxProcessRequestDto.year && a.Month == BordxProcessRequestDto.month
                   && a.Number == BordxProcessRequestDto.number && a.CommodityTypeId == BordxProcessRequestDto.commodityTypeId &&
                   a.Reinsurer == BordxProcessRequestDto.reinsurerId && a.Insurer == BordxProcessRequestDto.insurerId && a.ProductId== BordxProcessRequestDto.productId).FirstOrDefault();
                if (bordx == null)
                {
                    Response = "Bordx year/month/number dosen't match with any existing bordereaux";
                    return Response;
                }
                else
                {
                    if (bordx.IsConformed)
                    {
                        Response = "Cannot process already confirmed bordereaux";
                        return Response;
                    }
                    else
                    {
                        IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                          .Where(a => a.ReinsurerId == BordxProcessRequestDto.reinsurerId);
                        IEnumerable<Contract> contracts = session.Query<Contract>()
                        .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerContractId));

                        //IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                        //   .Where(a => a.ReinsurerId == BordxProcessRequestDto.reinsurerId
                        //        && a.InsurerId == BordxProcessRequestDto.insurerId);
                        //IEnumerable<Contract> contracts = session.Query<Contract>();
                        ////  .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerId));
                        ///

                        policies =
                  session.CreateSQLQuery("exec BordxProcess :StartDate,:EndDate,:CommodityTypeId,:BordxYear,:BordxMonth,:BordxNumber,:BordxId,:ProductId")
                      .SetDateTime("StartDate", bordx.StartDate.AddHours(-12))
                      .SetDateTime("EndDate", bordx.EndDate.AddHours(12))
                      .SetGuid("CommodityTypeId", BordxProcessRequestDto.commodityTypeId)
                      .SetInt32("BordxYear", BordxProcessRequestDto.year)
                      .SetInt32("BordxMonth", BordxProcessRequestDto.month)
                      .SetInt32("BordxNumber", BordxProcessRequestDto.number)
                      .SetGuid("BordxId", bordx.Id)
                      .SetGuid("ProductId", bordx.ProductId)
                      .SetResultTransformer(Transformers.AliasToBean<Policy>()).List<Policy>().ToList();

                        // IEnumerable<Policy> policies = session.Query<Policy>()
                        //.Where(a =>
                        //    (a.BordxId == null || a.BordxId == Guid.Empty && a.IsApproved == true) &&
                        //        (
                        //             (a.Year == 0 && a.Month == 0) &&
                        //              (a.ApprovedDate > bordx.StartDate.AddHours(-12) && a.ApprovedDate <= bordx.EndDate.AddHours(12)) &&
                        //             a.CommodityTypeId == BordxProcessRequestDto.commodityTypeId &&
                        //             contracts.Any(b => b.Id == a.ContractId)
                        //        ) ||
                        //        (
                        //                 a.Year == BordxProcessRequestDto.year &&
                        //                 a.Month == BordxProcessRequestDto.month &&
                        //                 a.BordxNumber == BordxProcessRequestDto.number &&
                        //                 a.CommodityTypeId == BordxProcessRequestDto.commodityTypeId &&
                        //                 contracts.Any(b => b.Id == a.ContractId)
                        //         )
                        //    );

                        foreach (Policy policy in policies)
                        {
                            //policy.BordxCountryId = BordxProcessRequestDto.;
                            policy.Year = BordxProcessRequestDto.year;
                            policy.Month = BordxProcessRequestDto.month;
                            policy.BordxNumber = BordxProcessRequestDto.number;
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(policy);
                                session.Update(policy, policy.Id);
                                transaction.Commit();

                            }
                        }
                    }
                }
                Response = "successful";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured";
            }
            return Response;
        }

        internal static string ConfirmBordx(BordxProcessRequestDto BordxProcessRequestDto)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<Policy> policies = new List<Policy>();
                Bordx bordx = session.Query<Bordx>()
                   .Where(a => a.Year == BordxProcessRequestDto.year &&
                        a.Month == BordxProcessRequestDto.month &&
                       a.Number == BordxProcessRequestDto.number &&
                       a.CommodityTypeId == BordxProcessRequestDto.commodityTypeId &&
                       a.Reinsurer == BordxProcessRequestDto.reinsurerId &&
                       a.Insurer == BordxProcessRequestDto.insurerId &&
                       a.ProductId == BordxProcessRequestDto.productId
                       ).FirstOrDefault();
                if (bordx == null)
                {
                    Response = "Bordereaux selection is invalid.";
                    return Response;
                }
                else
                {
                    if (bordx.IsConformed)
                    {
                        Response = "Select bordereaux is already confirmed";
                        return Response;
                    }
                    else
                    {
                        //IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                        //    .Where(a => a.ReinsurerId == BordxProcessRequestDto.reinsurerId
                        //            && a.InsurerId == BordxProcessRequestDto.insurerId);
                        //IEnumerable<Contract> contracts = session.Query<Contract>();

                        IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                         .Where(a => a.ReinsurerId == BordxProcessRequestDto.reinsurerId);
                        IEnumerable<Contract> contracts = session.Query<Contract>()
                        .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerContractId));

                        policies =
                 session.CreateSQLQuery("exec BordxProcess :StartDate,:EndDate,:CommodityTypeId,:BordxYear,:BordxMonth,:BordxNumber,:BordxId,:ProductId")
                     .SetDateTime("StartDate", bordx.StartDate.AddHours(-12))
                     .SetDateTime("EndDate", bordx.EndDate.AddHours(12))
                     .SetGuid("CommodityTypeId", BordxProcessRequestDto.commodityTypeId)
                     .SetInt32("BordxYear", BordxProcessRequestDto.year)
                     .SetInt32("BordxMonth", BordxProcessRequestDto.month)
                     .SetInt32("BordxNumber", BordxProcessRequestDto.number)
                     .SetGuid("BordxId", bordx.Id)
                     .SetGuid("ProductId", bordx.ProductId)
                     .SetResultTransformer(Transformers.AliasToBean<Policy>()).List<Policy>().ToList();

                        // .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerId));
                        //update policies
                        //IEnumerable<Policy> policies = session.Query<Policy>()
                        //   .Where(a => a.Year == BordxProcessRequestDto.year
                        //       && a.Month == BordxProcessRequestDto.month
                        //       && a.BordxNumber == BordxProcessRequestDto.number
                        //       && a.CommodityTypeId == BordxProcessRequestDto.commodityTypeId
                        //       && contracts.Any(b => b.Id == a.ContractId)
                        //       );

                        foreach (Policy policy in policies)
                        {
                            BordxDetails bordxPolicy = new BordxDetails()
                            {
                                BordxId = bordx.Id,
                                Id = Guid.NewGuid(),
                                PolicyId = policy.Id
                            };
                            policy.BordxId = bordx.Id;
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(policy);
                                session.Update(policy, policy.Id);
                                session.Save(bordxPolicy, bordxPolicy.Id);
                                transaction.Commit();

                            }
                        }
                        //update bordx
                        bordx.IsConformed = true;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(bordx, bordx.Id);
                            transaction.Commit();

                        }

                        try
                        {

                            String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BiBordxSummery.sql"));
                            Query = Query.Replace("{bordexId}", bordx.Id.ToString());

                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                var result = session.CreateSQLQuery(Query);
                                result.ExecuteUpdate();
                                transaction.Commit();

                            }

                        }
                        catch (Exception ex)
                        {
                            logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                            Response = "Error occured";
                        }


                    }
                }
                Response = "successful";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured";
            }
            return Response;
        }



        internal static string CreateBordx(BordxCreateRequestDto BordxCreateRequest)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //date validity
                List<Bordx> FromDateOverlaps = session.Query<Bordx>()
                    .Where(a => a.StartDate <= BordxCreateRequest.startDate && a.EndDate >= BordxCreateRequest.startDate
                     && a.Reinsurer == BordxCreateRequest.reinsurerId
                     && a.Insurer == BordxCreateRequest.insurerId &&
                     a.CommodityTypeId == BordxCreateRequest.commodityTypeId && a.ProductId==BordxCreateRequest.productId).ToList();

                int Sequence = session.Query<Bordx>().Count();
                int SequenceNo = Sequence + 1;

                if (BordxCreateRequest.startDate > DateTime.Today)
                {
                    return Response = "Create Bordx Up to Current Date.";
                }

                if (FromDateOverlaps != null && FromDateOverlaps.Count() > 0)
                {
                    Response = "Form Date you have selected overlaps with bordereaux " +
                        FromDateOverlaps.FirstOrDefault().Year + "-" +
                        ((Months)FromDateOverlaps.FirstOrDefault().Month).ToString() + "-" +
                        FromDateOverlaps.FirstOrDefault().Number + "'s  date range.";
                    return Response;
                }

                List<Bordx> ToDateOverlaps = session.Query<Bordx>()
                    .Where(a => a.StartDate <= BordxCreateRequest.endDate && a.EndDate >= BordxCreateRequest.endDate
                     && a.Reinsurer == BordxCreateRequest.reinsurerId
                     && a.Insurer == BordxCreateRequest.insurerId
                     && a.CommodityTypeId == BordxCreateRequest.commodityTypeId &&  a.ProductId == BordxCreateRequest.productId).ToList();

                if (ToDateOverlaps != null && ToDateOverlaps.Count() > 0)
                {
                    Response = "To Date you have selected overlaps with bordereaux " +
                        ToDateOverlaps.FirstOrDefault().Year + "-" +
                        ((Months)ToDateOverlaps.FirstOrDefault().Month).ToString() + "-" +
                        ToDateOverlaps.FirstOrDefault().Number + "'s  date range.";
                    return Response;
                }

                //full range overlaps


                //get next bordx number
                int BordxNumber = int.Parse(GetNextBordxNumber(BordxCreateRequest.year, BordxCreateRequest.month,
                    BordxCreateRequest.reinsurerId, BordxCreateRequest.insurerId, BordxCreateRequest.productId, BordxCreateRequest.commodityTypeId));

                ///creating new bordx
                Bordx newBordx = new Bordx()
                {
                    EndDate = BordxCreateRequest.endDate,
                    EntryDateTime = DateTime.UtcNow,
                    EntryUser = BordxCreateRequest.userId,
                    Id = Guid.NewGuid(),
                    IsConformed = false,
                    IsProcessed = false,
                    CommodityTypeId = BordxCreateRequest.commodityTypeId,
                    Reinsurer = BordxCreateRequest.reinsurerId,
                    Insurer = BordxCreateRequest.insurerId,
                    Month = BordxCreateRequest.month,
                    Number = BordxNumber,
                    StartDate = BordxCreateRequest.startDate,
                    Year = BordxCreateRequest.year,
                    SequenceNo = SequenceNo,
                    ProductId = BordxCreateRequest.productId

                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newBordx, newBordx.Id);
                    transaction.Commit();
                }

                Response = "successful";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured";
            }
            return Response;
        }

        internal static object GetLast10Bordx(BordxListRequestDto bordxListRequestDto)
        {
            object Response = null;
            try
            {
                Expression<Func<Bordx, bool>> filterBordx = PredicateBuilder.True<Bordx>();

                if (bordxListRequestDto.CommodityTypeId != Guid.Empty) {
                    filterBordx = filterBordx.And(a => a.CommodityTypeId == bordxListRequestDto.CommodityTypeId);
                }
                if (bordxListRequestDto.InsurerId != Guid.Empty)
                {
                    filterBordx= filterBordx.And(a => a.Insurer == bordxListRequestDto.InsurerId);
                }
                if (bordxListRequestDto.ReinsurerId != Guid.Empty)
                {
                    filterBordx =filterBordx.And(a => a.Reinsurer == bordxListRequestDto.ReinsurerId);
                }
                if (bordxListRequestDto.ProductId != Guid.Empty)
                {
                    filterBordx = filterBordx.And(a => a.ProductId == bordxListRequestDto.ProductId);
                }



                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();

                Response = session.Query<Bordx>().Where(filterBordx)
                    .OrderByDescending(a => a.Year)
                    .ThenByDescending(a => a.Month)
                    .ThenByDescending(a => a.Number)
                    .Take(10)
                    .Join(session.Query<Insurer>(), m => m.Insurer, n => n.Id, (m, n) => new { m, n })
                    .Join(session.Query<Reinsurer>(), o => o.m.Reinsurer, p => p.Id, (o, p) => new { o, p })
                    .Join(session.Query<CommodityType>(), q => q.o.m.CommodityTypeId, r => r.CommodityTypeId, (q, r) => new { q, r })
                    .Join(session.Query<Product>(), s => s.q.o.m.ProductId, t => t.Id, (s, t) => new { s, t })
                    .ToList()
                    .Select(a => new
                    {
                        a.s.q.o.m.Id,
                        CommodityType = a.s.r.CommodityTypeDescription,
                        a.s.q.o.m.Year,
                        a.s.q.o.m.Month,
                        a.s.q.o.m.Number,
                        StartDate = a.s.q.o.m.StartDate.ToString("dd-MMM-yyyy"),
                        EndDate = a.s.q.o.m.EndDate.ToString("dd-MMM-yyyy"),
                        Insurer = a.s.q.o.n.InsurerShortName,
                        Reinsurer = a.s.q.p.ReinsurerName,
                        Product =a.t.Productcode

                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static string GetNextBordxNumber(int year, int month, Guid reinsurerId,Guid insurerId,Guid productId, Guid CommodityTypeId)
        {
            string response = "";
            try
            {
                int BordxNumber = 1;
                ISession session = EntitySessionManager.GetSession();
                List<Bordx> existingBordexForRequestMonth = session.Query<Bordx>()
                    .Where(a => a.Year == year && a.Month == month
                        && a.Reinsurer == reinsurerId && a.Insurer == insurerId && a.CommodityTypeId == CommodityTypeId && a.ProductId== productId).ToList();

                if (existingBordexForRequestMonth != null && existingBordexForRequestMonth.Count() != 0)
                {
                    BordxNumber = existingBordexForRequestMonth.OrderByDescending(a => a.Number).FirstOrDefault().Number + 1;
                }
                response = BordxNumber.ToString();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static string DeleteBordx(Guid BordxId)
        {
            string Response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //validate
                Bordx bordx = session.Query<Bordx>()
                    .Where(a => a.Id == BordxId).FirstOrDefault();
                if (bordx == null)
                {
                    Response = "Invalid bordereaux selection";
                    return Response;
                }

                if (bordx.IsConformed)
                {
                    Response = "Cannot delete confirmed bordereaux";
                    return Response;
                }

                //revert policies
                List<Policy> policies = session.Query<Policy>()
                    .Where(a => a.Year == bordx.Year && a.Month == bordx.Month && a.BordxNumber == bordx.Number).ToList();
                foreach (Policy policy in policies)
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        policy.Year = 0;
                        policy.Month = 0;
                        policy.BordxNumber = 0;
                        policy.BordxCountryId = Guid.Empty;

                        session.Update(policy, policy.Id);
                        transaction.Commit();
                    }
                    session.Evict(policy);
                }

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
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured";
            }
            return Response;
        }

        internal static object GetBordxNumbers(Guid commodityTypeId,Guid reinsurerId,Guid insurerId,Guid productId, int year, int month)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<Bordx> existingBordexForRequestMonth = session.Query<Bordx>()
                    .Where(a => a.Year == year && a.Month == month && a.CommodityTypeId == commodityTypeId
                    && a.Reinsurer == reinsurerId && a.Insurer == insurerId && a.ProductId==productId).OrderByDescending(a => a.Number).ToList();
                response = existingBordexForRequestMonth.Select(a => new
                {
                    a.Number,
                    a.Id
                }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }

            return response;
        }

        internal static string TransferPolicyToBordx(BordxTransferRequestDto BordxTransferRequestDto)
        {
            string response = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Bordx requestedBordx = session.Query<Bordx>()
                    .Where(a => a.Year == BordxTransferRequestDto.year
                        && a.Month == BordxTransferRequestDto.month &&
                        a.Number == BordxTransferRequestDto.number).FirstOrDefault();

                if (requestedBordx == null)
                {
                    response = "Invalid new bordereaux selection";
                    return response;
                }
                if (requestedBordx.IsConformed)
                {
                    response = "Selected bordereaux already confirmed";
                    return response;
                }

                Policy selectedPolicy = session.Query<Policy>()
                    .Where(a => a.Id == BordxTransferRequestDto.policyId).FirstOrDefault();
                if (requestedBordx == null)
                {
                    response = "Invalid policy selection";
                    return response;
                }

                if (IsGuid(selectedPolicy.BordxId.ToString()))
                {
                    response = "Policy already assigned to a confirmed bordereaux";
                    return response;
                }

                if (selectedPolicy.Year == BordxTransferRequestDto.year && selectedPolicy.Month == BordxTransferRequestDto.month
                    && selectedPolicy.BordxNumber == BordxTransferRequestDto.number)
                {
                    response = "Policy already assigned to selected bordereaux.";
                    return response;
                }

                //update policy
                using (ITransaction transaction = session.BeginTransaction())
                {
                    selectedPolicy.Year = BordxTransferRequestDto.year;
                    selectedPolicy.Month = BordxTransferRequestDto.month;
                    selectedPolicy.BordxNumber = BordxTransferRequestDto.number;

                    session.Update(selectedPolicy, selectedPolicy.Id);
                    transaction.Commit();
                }
                response = "successful";

            }
            catch (Exception ex)
            {
                response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static object getBordxNumbersYearsAndMonth(string bordxYear, string bordxMonth)
        {
            Object Response = new object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<string> bordxNumbersList = session.Query<ClaimBordx>()
                    .Where(a => a.BordxYear == Convert.ToInt32(bordxYear) && a.Bordxmonth == Convert.ToInt32(bordxMonth))
                    .Select(a => a.BordxNumber).Distinct().ToList();

                Response = bordxNumbersList;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetBordexAllowedYearsMonths(Guid InsurerId, Guid ReinsurerId, Guid CommodityTypeId)
        {
            Object Response = new object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<int> bordxYearList = session.Query<Bordx>()
                    .Where(a => a.Reinsurer == ReinsurerId && a.Insurer == InsurerId && a.CommodityTypeId == CommodityTypeId)
                    .Select(a => a.Year).Distinct().ToList();

                IEnumerable<ReinsurerContract> reinsCon = session.Query<ReinsurerContract>()
                    .Where(a => a.ReinsurerId == ReinsurerId);
                IEnumerable<Contract> contracts = session.Query<Contract>();
                // .Where(b => reinsCon.Any(a => a.Id == b.ReinsurerId));

                List<int> policyYearList = session.Query<Policy>()
                     .Where(a => contracts.Any(b => b.Id == a.ContractId) && a.CommodityTypeId == CommodityTypeId)
                    .Select(a => a.PolicySoldDate.Year).Distinct().ToList();

                List<int> totaYears = policyYearList.Union(bordxYearList).OrderByDescending(a => a).ToList();
                int lastYear = 0;
                if (totaYears != null && totaYears.Count() > 0)
                {
                    lastYear = totaYears.FirstOrDefault();
                }
                else
                {
                    lastYear = DateTime.UtcNow.Year;
                    totaYears.Add(lastYear);
                }
                totaYears.Add(lastYear + 1);

                //months
                int defaultMonth = 0;
                Bordx lastConfirmedBordx = session.Query<Bordx>()
                    .Where(a => a.IsConformed == true)
                    .OrderByDescending(a => a.Year).ThenByDescending(a => a.Month).FirstOrDefault();
                if (lastConfirmedBordx == null)
                {
                    defaultMonth = DateTime.UtcNow.Month;
                }
                else
                {
                    defaultMonth = lastConfirmedBordx.Month == 12 ? 1 : lastConfirmedBordx.Month++;

                }
                List<MonthsDto> monthsList = new List<MonthsDto>();
                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    //monthsDict.Add((int)month, month.ToString());
                    MonthsDto monthObj = new MonthsDto()
                    {
                        monthsName = month.ToString(),
                        monthsSeq = (int)month,
                        isDefault = defaultMonth == (int)month ? true : false
                    };
                    monthsList.Add(monthObj);
                }


                MonthsAndYearsResponseDto response = new MonthsAndYearsResponseDto()
                {
                    months = monthsList.ToArray(),
                    years = totaYears.OrderByDescending(i => i).ToArray()
                };
                Response = response;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }



        internal static string BordxReopen(BordxReopenRequestDto BordxReopenRequestDto)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Bordx bordx = session.Query<Bordx>()
                    .Where(a => a.Id == BordxReopenRequestDto.bordxId).FirstOrDefault();
                if (bordx == null)
                    return "Invalid bordereaux selection";

                IEnumerable<Policy> policyList = session.Query<Policy>()
                    .Where(a => a.BordxId == bordx.Id);
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (Policy policy in policyList)
                    {
                        session.Evict(policy);
                        policy.BordxId = Guid.Empty;
                        session.Update(policy, policy.Id);

                    }

                    bordx.IsConformed = false;
                    session.Update(bordx, bordx.Id);

                    transaction.Commit();
                }
                Response = "successful";

                try
                {

                    String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\BiBordxSummerydelete.sql"));
                    Query = Query.Replace("{bordexId}", bordx.Id.ToString());

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var result = session.CreateSQLQuery(Query);
                        result.ExecuteUpdate();
                        transaction.Commit();

                    }

                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    Response = "Error occured";
                }

            }
            catch (Exception ex)
            {
                Response = "Error occured";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static ValidateContractWithTaxResponseDto ContractWithTaxValidity(Guid countryTaxId, Guid contratcId, Guid PolicyId)
        {
            ValidateContractWithTaxResponseDto Response = new ValidateContractWithTaxResponseDto();
            Response.Status = false;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<ContractTaxMapping> contratTaxes = session.Query<ContractTaxMapping>()
                    .Where(a => a.ContractId == contratcId && a.CountryTaxId == countryTaxId);

                if (contratTaxes.Count() == 0)
                    return Response;

                CountryTaxes countryTax = session.Query<CountryTaxes>()
                    .FirstOrDefault(a => a.Id == countryTaxId);
                TaxTypes tax = session.Query<TaxTypes>()
                    .FirstOrDefault(a => a.Id == countryTax.TaxTypeId);
                Policy policy = session.Query<Policy>()
                    .FirstOrDefault(a => a.Id == PolicyId);
                if (tax != null)
                {
                    Response.Status = true;
                    if (countryTax.IsPercentage)
                    {
                        if (countryTax.IsOnGross)
                        {
                            Response.CountryTax = new CountryTaxesResponseDto()
                            {
                                TaxValue = countryTax.TaxValue,
                                IsPercentage = true,
                                IsOnGross = true,
                                IsOnNRP = false,
                                IsOnPreviousTax = countryTax.IsOnPreviousTax,
                                ValueIncluededInPolicy = (countryTax.TaxValue * (policy.Premium - (policy.TotalTax / policy.LocalCurrencyConversionRate))) / 100
                            };
                        }
                        else
                        {
                            Response.CountryTax = new CountryTaxesResponseDto()
                            {
                                TaxValue = countryTax.TaxValue,
                                IsPercentage = true,
                                IsOnGross = false,
                                IsOnNRP = true,
                                IsOnPreviousTax = countryTax.IsOnPreviousTax,
                                ValueIncluededInPolicy = (countryTax.TaxValue * policy.NRP) / 100
                            };
                        }
                    }
                    else
                    {
                        Response.CountryTax = new CountryTaxesResponseDto()
                        {
                            TaxValue = countryTax.TaxValue,
                            IsPercentage = false,
                            IsOnGross = false,
                            IsOnNRP = false,
                            IsOnPreviousTax = countryTax.IsOnPreviousTax,
                            ValueIncluededInPolicy = countryTax.TaxValue
                        };

                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        #region BordxReport
        public List<BordxReportTemplateResponseDto> GetBordxReportTemplates(BordxTemplateRequestDto bordxTemplateRequestDto)
        {
            List<BordxReportTemplateResponseDto> bordxReportTemplates = new List<BordxReportTemplateResponseDto>();


            try {
                ISession session = EntitySessionManager.GetSession();
                var result = session.Query<BordxReportTemplate>().Where(a => a.IsActive)
                                                             .Join(session.Query<ProductType>(), b => b.ProductType, c => c.Id, (b, c) => new { b, c })
                                                             .Join(session.Query<Product>(), d => d.c.Id, e => e.ProductTypeId, (d, e) => new { d, e })
                                                            .Select(a => new
                                                            {
                                                                Id = a.d.b.Id,
                                                                Name = a.d.b.Name,
                                                                ProductId = a.e.Id
                                                            }).ToList();


                if (bordxTemplateRequestDto.ProductId != Guid.Empty)
                {
                    bordxReportTemplates = result.Where(a => a.ProductId == bordxTemplateRequestDto.ProductId)
                        .Select(a => new BordxReportTemplateResponseDto
                        {
                            Id = a.Id,
                            Name = a.Name,
                        }).ToList();

                }
                else {
                    bordxReportTemplates= result
                       .Select(a => new BordxReportTemplateResponseDto
                       {
                           Id = a.Id,
                           Name = a.Name,
                       }).ToList();
                }

            } catch (Exception ex) {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return bordxReportTemplates;
        }

        public List<BordxReportColumns> GetBordxReportTemplateColumns(BordxColumnRequestDto bordxColumnRequestDto)
        {
            List<BordxReportColumns> entities = null;
            Expression<Func<BordxReportColumns, bool>> filterBordxColumns = PredicateBuilder.True<BordxReportColumns>();
            filterBordxColumns = filterBordxColumns.And(a => a.ProductType == bordxColumnRequestDto.ProductType);
            filterBordxColumns = filterBordxColumns.And(a => a.IsActive);

            ISession session = EntitySessionManager.GetSession();
            IQueryable<BordxReportColumns> BordxData = session.Query<BordxReportColumns>().Where(filterBordxColumns);
            entities = BordxData.ToList();
            return entities;
        }

        public object GetAllBordxReportTemplateForSearchGrid(BordxReportTemplateSearchGridRequestDto bordxReportTemplateSearchGridRequestDto)
        {

            try
            {
                if (bordxReportTemplateSearchGridRequestDto != null && bordxReportTemplateSearchGridRequestDto.paginationOptionsSearchGrid != null)
                {
                    Expression<Func<BordxReportTemplate, bool>> filterBordxReportTemplate = PredicateBuilder.True<BordxReportTemplate>();
                    //filterBordxReportTemplate = filterBordxReportTemplate.And(a => a.IsActive == true);
                    if (bordxReportTemplateSearchGridRequestDto.bordxReportTemplateSearchGridSearchCriterias.ProductType != Guid.Empty) {
                        filterBordxReportTemplate = filterBordxReportTemplate.And(a => a.ProductType == bordxReportTemplateSearchGridRequestDto.bordxReportTemplateSearchGridSearchCriterias.ProductType);
                    }
                    if (!string.IsNullOrEmpty(bordxReportTemplateSearchGridRequestDto.bordxReportTemplateSearchGridSearchCriterias.TemplateName))
                    {
                        filterBordxReportTemplate = filterBordxReportTemplate.And(a => a.Name.ToLower().Contains(bordxReportTemplateSearchGridRequestDto.bordxReportTemplateSearchGridSearchCriterias.TemplateName.ToLower()));
                    }
                    CommonEntityManager cem = new CommonEntityManager();
                    ISession session = EntitySessionManager.GetSession();
                    var filteredBordxReportTemplate = session.Query<BordxReportTemplate>().Where(filterBordxReportTemplate);

                    long TotalRecords = filteredBordxReportTemplate.Count();
                    var customerGridDetailsFilterd = filteredBordxReportTemplate.Skip((bordxReportTemplateSearchGridRequestDto.paginationOptionsSearchGrid.pageNumber - 1) * bordxReportTemplateSearchGridRequestDto.paginationOptionsSearchGrid.pageSize)
                    .Take(bordxReportTemplateSearchGridRequestDto.paginationOptionsSearchGrid.pageSize)
                    .Select(a => new { a.Id,a.Name, ProductTypeId=a.ProductType, ProductType = cem.getProductTypeById(a.ProductType), a.IsActive })
                    .ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = customerGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        public BordxReportTemplateResponseDto GetBordxReportTemplateById(Guid bordxReportTemplateId)
        {
            ISession session = EntitySessionManager.GetSession();

            BordxReportTemplateResponseDto bordxReportResponse = new BordxReportTemplateResponseDto();

            var bordxTemplate = session.Query<BordxReportTemplate>().FirstOrDefault(a => a.Id == bordxReportTemplateId);
            if (bordxTemplate != null)
            {
                bordxReportResponse.Id = bordxTemplate.Id;
                bordxReportResponse.Name = bordxTemplate.Name;
                bordxReportResponse.IsActive = bordxTemplate.IsActive;
                bordxReportResponse.ProductType = bordxTemplate.ProductType;

                var bordxReportTemplateDetails = session.Query<BordxReportTemplateDetails>().Where(a => a.BordxReportTemplateId == bordxReportTemplateId)
                                                    .Select(a => new BordxReportTemplateDetailsResponseDto
                                                    {
                                                        Id = a.Id,
                                                        BordxReportColumnsId = a.BordxReportColumnsId,
                                                        BordxReportTemplateId = a.BordxReportTemplateId,
                                                        IsEnable = true
                                                    });

                bordxReportResponse.BordxReportTemplateDetails = bordxReportTemplateDetails.ToList();
                return bordxReportResponse;
            }
            else return null;
        }

        public bool BordxReportTemplateIsExist(Guid bordxReportTemplateId)
        {
            ISession session = EntitySessionManager.GetSession();
            var query = session.Query<BordxReportTemplate>().FirstOrDefault(a => a.Id == bordxReportTemplateId);
            return query == null ? false : true;
        }

        public bool IsBordxReportTemplateNameExists(BordxReportTemplateRequestDto BordxReport)
        {
            bool result = false;
            ISession session = EntitySessionManager.GetSession();
            var query = session.Query<BordxReportTemplate>().Where(a => a.Name.ToLower() == BordxReport.Name.ToLower() && a.ProductType== BordxReport.ProductType);
            if (BordxReport.Id == Guid.Empty)
            {
                result = query.Any();
            }
            else
            {
                result = query.Any(a => a.Id != BordxReport.Id);
            }
            return result;
        }

        internal bool AddBordxReportTemplate(BordxReportTemplateRequestDto BordxReport)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                BordxReportTemplate brt = new BordxReportTemplate
                {
                    Id = new Guid(),
                    Name = BordxReport.Name,
                    ProductType = BordxReport.ProductType,
                    IsActive = BordxReport.IsActive,
                    EntryDateTime = DateTime.Today.ToUniversalTime(),
                    EntryUser = BordxReport.EntryUser,
                    UpdateDateTime = null,
                    UpdateUser = null
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(brt);
                    BordxReport.Id = brt.Id;

                    foreach (var bordxReportTemplate in BordxReport.BordxReportTemplateDetails.Where(a => a.IsEnable))
                    {
                        BordxReportTemplateDetails brtd = new BordxReportTemplateDetails
                        {
                            Id = new Guid(),
                            BordxReportTemplateId = BordxReport.Id,
                            BordxReportColumnsId = bordxReportTemplate.BordxReportColumnsId,
                            IsActive = bordxReportTemplate.IsEnable,
                            EntryDateTime = DateTime.Today.ToUniversalTime(),
                            EntryUser = bordxReportTemplate.EntryUser
                        };
                        session.Evict(brtd);
                        session.Save(brtd);
                    }
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

        internal bool UpdateBordxReportTemplate(BordxReportTemplateRequestDto BordxReport)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                BordxReportTemplate brt = session.Query<BordxReportTemplate>().FirstOrDefault(a => a.Id == BordxReport.Id);

                brt.Name = BordxReport.Name;
                brt.ProductType = BordxReport.ProductType;
                brt.IsActive = BordxReport.IsActive;
                brt.UpdateDateTime = DateTime.Today.ToUniversalTime();
                brt.UpdateUser = BordxReport.UpdateUser;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    var entryUser = brt.EntryUser;
                    var entryDate = brt.EntryDateTime;

                    session.Update(brt);

                    IQueryable<BordxReportTemplateDetails> oldBordxReportTemplateDetails = session.Query<BordxReportTemplateDetails>().Where(a => a.BordxReportTemplateId == BordxReport.Id);

                    foreach (var removeBordxReportTemplate in oldBordxReportTemplateDetails.ToList())
                    {
                        session.Delete(removeBordxReportTemplate);
                    }

                    foreach (var bordxReportTemplate in BordxReport.BordxReportTemplateDetails.Where(a => a.IsEnable))
                    {
                        BordxReportTemplateDetails brtd = new BordxReportTemplateDetails
                        {
                            Id = (bordxReportTemplate.Id == Guid.Empty) ? new Guid() : bordxReportTemplate.Id,
                            BordxReportTemplateId = BordxReport.Id,
                            BordxReportColumnsId = bordxReportTemplate.BordxReportColumnsId,
                            IsActive = bordxReportTemplate.IsEnable,
                            EntryDateTime = entryDate,
                            EntryUser = entryUser,
                            UpdateDateTime = DateTime.Today.ToUniversalTime(),
                            UpdateUser = bordxReportTemplate.UpdateUser
                        };
                        session.Evict(brtd);
                        session.Save(brtd);

                    }
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
    }
}
