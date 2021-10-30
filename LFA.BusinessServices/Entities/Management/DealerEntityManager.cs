using HashidsNet;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Exceptions;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Enums;
using TAS.Services.Common.Notification;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class DealerEntityManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        internal List<DealerRespondDto> GetAllDealers()
        {
            List<DealerRespondDto> DealerWithMakes = new List<DealerRespondDto>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<Dealer> DealerData = session.Query<Dealer>();
                foreach (var item in DealerData)
                {
                    List<Guid> makesIds = session.Query<DealerMakes>().Where(m => m.DealerId == item.Id).Select(c=>c.MakeId).ToList();
                    DealerRespondDto DealerWithMake = new DealerRespondDto()
                    {
                        Id = item.Id,
                        CityId = item.CityId,
                        CommodityTypeId = item.CommodityTypeId,
                        CountryId = item.CountryId,
                        CurrencyId = item.CurrencyId,
                        DealerAliase = item.DealerAliase,
                        DealerCode = item.DealerCode,
                        DealerName = item.DealerName,
                        InsurerId = item.InsurerId,
                        Location = item.Location,
                        Type = item.Type,
                        Makes = makesIds,
                        IsActive = item.IsActive,
                        EntryDateTime = item.EntryDateTime,
                        EntryUser = item.EntryUser
                    };
                    DealerWithMakes.Add(DealerWithMake);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return DealerWithMakes;
        }

        internal DealersRespondDto GetDealersByBordx(Guid bordxId)
        {
            DealersRespondDto dealersRespondDto = new DealersRespondDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<Guid> dealerIdsList = session.Query<Bordx>().Where(a => a.Id == bordxId)
                   .Join(session.Query<Policy>(), b => b.Id, c => c.BordxId, (b, c) => new { b, c })
                   .Select(s => s.c.DealerId).Distinct().ToList();

                dealersRespondDto.Dealers = session.Query<Dealer>().Where(a=> dealerIdsList.Contains(a.Id))
                    .Select(item => new DealerRespondDto
                    {
                        Id = item.Id,
                        CityId = item.CityId,
                        CommodityTypeId = item.CommodityTypeId,
                        CountryId = item.CountryId,
                        CurrencyId = item.CurrencyId,
                        DealerAliase = item.DealerAliase,
                        DealerCode = item.DealerCode,
                        DealerName = item.DealerName,
                        InsurerId = item.InsurerId,
                        Location = item.Location,
                        Type = item.Type,
                        IsActive = item.IsActive,
                        EntryDateTime = item.EntryDateTime,
                        EntryUser = item.EntryUser
                    }
                    ).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return dealersRespondDto;
        }

        internal List<DealerRespondDto> GetAllDealersByCountry(Guid countryId)
        {
            List<DealerRespondDto> DealerWithMakes = new List<DealerRespondDto>();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<Dealer> DealerData = session.Query<Dealer>().Where(a=>a.CountryId==countryId);
                foreach (var item in DealerData)
                {
                    List<Guid> makesIds = session.Query<DealerMakes>().Where(m => m.DealerId == item.Id).Select(c => c.MakeId).ToList();

                    DealerRespondDto DealerWithMake = new DealerRespondDto()
                    {
                        Id = item.Id,
                        CityId = item.CityId,
                        CommodityTypeId = item.CommodityTypeId,
                        CountryId = item.CountryId,
                        CurrencyId = item.CurrencyId,
                        DealerAliase = item.DealerAliase,
                        DealerCode = item.DealerCode,
                        DealerName = item.DealerName,
                        InsurerId = item.InsurerId,
                        Location = item.Location,
                        Type = item.Type,
                        Makes = makesIds,
                        IsActive = item.IsActive,
                        EntryDateTime = item.EntryDateTime,
                        EntryUser = item.EntryUser
                    };
                    DealerWithMakes.Add(DealerWithMake);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return DealerWithMakes;
        }

        internal static object GetAllAvailabelTireSizesByloadSpeed(string cross, string width, string diameter)
        {
            AllAvailableLoadSpeedSizeResponseDto response = new AllAvailableLoadSpeedSizeResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<AvailableTireSizes> availableTireSizes = session.Query<AvailableTireSizes>().OrderBy(a=>a.LoadSpeed).ToList();
                List<AvailableTireSizesPattern> AvailableTireSizesPattern = session.Query<AvailableTireSizesPattern>().OrderBy(a=>a.Pattern).ToList();

                if (cross != null && width != null)
                {
                    response.LoadSpeedList = availableTireSizes.Where(x => x.Width == width
                            && x.CrossSection == cross
                            && x.Diameter == Convert.ToInt32(diameter)).Select(a => a.LoadSpeed).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetArticleNoByTyreSize(string width, string cross, string diameter, string loadSpeed, string pattern)
        {
            object response = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();

                response = (from am in session.Query<ArticleMapping>()
                                                              join ats in session.Query<AvailableTireSizes>() on am.AvailableTireSizeId equals ats.Id
                                                              join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                                                              where ats.Width == width && ats.CrossSection == cross && ats.Diameter.ToString() == diameter && ats.LoadSpeed == loadSpeed && atsp.Pattern == pattern
                                                              select new
                                                              {
                                                                  Id = am.Id,
                                                                  ArticleNo = am.ArticleNo,
                                                                  AvailableTireSizeId = am.AvailableTireSizeId
                                                              }
                                                              ).FirstOrDefault();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllAvailabelTireSizesByPattern(string cross, string width, string diameter, string loadSpeed)
        {
            AllAvailablePatternSizeResponseDto response = new AllAvailablePatternSizeResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<AvailableTireSizes> availableTireSizes = session.Query<AvailableTireSizes>().ToList();
                List<AvailableTireSizesPattern> AvailableTireSizesPattern = session.Query<AvailableTireSizesPattern>().ToList();

                List<AvailableTireSizesPatternResponse> PatternL = new List<AvailableTireSizesPatternResponse>();
                availableTireSizes = session.Query<AvailableTireSizes>().Where(x => x.Width == width
                            && x.CrossSection == cross
                            && x.Diameter == Convert.ToInt32(diameter)
                            && x.LoadSpeed == loadSpeed).ToList();
                foreach (var pattern in availableTireSizes)
                {
                    AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                        .Where(x => x.AvailableTireSizesId == pattern.Id).FirstOrDefault();

                    if (availableTireSizesPattern != null)
                    {
                        AvailableTireSizesPatternResponse AvailableTire = new AvailableTireSizesPatternResponse()
                        {
                            Id = availableTireSizesPattern.Id,
                            AvailableTireSizesId = availableTireSizesPattern.AvailableTireSizesId,
                            Pattern = availableTireSizesPattern.Pattern
                        };
                        PatternL.Add(AvailableTire);
                    }
                }
                response.PatternList = PatternL.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllAvailabelTireSizesByDiameter(string cross, string width)
        {
            AllAvailableCrossSizeResponseDto response = new AllAvailableCrossSizeResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<AvailableTireSizes> availableTireSizes = session.Query<AvailableTireSizes>().OrderBy(a=>a.Diameter).ToList();
                List<AvailableTireSizesPattern> AvailableTireSizesPattern = session.Query<AvailableTireSizesPattern>().OrderBy(a=>a.Pattern).ToList();

                if (cross != null && width != null)
                {
                    response.DiameterList = availableTireSizes.Where(x => x.Diameter != 0
                            && x.Width == width && x.CrossSection == cross).Select(a => a.Diameter).Distinct().ToList();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllAvailabelTireSizesByWidth(LoadTyreDetailsByWidthRequestDto LoadTyreDetailsByWidthRequestDto)
        {
            AllAvailableTireSizeResponseDto response = new AllAvailableTireSizeResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<AvailableTireSizes> availableTireSizes = session.Query<AvailableTireSizes>().OrderBy(a=>a.CrossSection).ToList();
                List<AvailableTireSizesPattern> AvailableTireSizesPattern = session.Query<AvailableTireSizesPattern>().OrderBy(a=>a.Pattern).ToList();

                if (LoadTyreDetailsByWidthRequestDto.frontwidth != "" && LoadTyreDetailsByWidthRequestDto.frontcross == null
                    && LoadTyreDetailsByWidthRequestDto.frontdiameter == null && LoadTyreDetailsByWidthRequestDto.frontloadSpeed == null)
                {
                    response.CrossSectionList = availableTireSizes.Where(x => x.CrossSection != "0" && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth).Select(a => a.CrossSection).Distinct().ToList();
                    response.DiameterList = availableTireSizes.Where(x => x.Diameter != 0 && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth).Select(a => a.Diameter).Distinct().ToList();
                    response.LoadSpeedList = availableTireSizes.Where(x => x.LoadSpeed != null && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth).Select(a => a.LoadSpeed).Distinct().ToList();
                    response.WidthList = availableTireSizes.Where(x => x.Width != "0" && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth).Select(a => a.Width).Distinct().ToList();
                    response.PatternList = AvailableTireSizesPattern.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();
                }
                else if (LoadTyreDetailsByWidthRequestDto.frontwidth != "" && LoadTyreDetailsByWidthRequestDto.frontcross != null
                    && LoadTyreDetailsByWidthRequestDto.frontdiameter == null && LoadTyreDetailsByWidthRequestDto.frontloadSpeed == null)
                {
                    response.CrossSectionList = availableTireSizes.Where(x => x.CrossSection != "0"
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross).Select(a => a.CrossSection).Distinct().ToList();
                    response.DiameterList = availableTireSizes.Where(x => x.Diameter != 0
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross).Select(a => a.Diameter).Distinct().ToList();
                    response.LoadSpeedList = availableTireSizes.Where(x => x.LoadSpeed != null
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross).Select(a => a.LoadSpeed).Distinct().ToList();
                    response.WidthList = availableTireSizes.Where(x => x.Width != "0"
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross).Select(a => a.Width).Distinct().ToList();
                    response.PatternList = AvailableTireSizesPattern.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();
                }
                else if (LoadTyreDetailsByWidthRequestDto.frontwidth != "" && LoadTyreDetailsByWidthRequestDto.frontcross != null
                        && LoadTyreDetailsByWidthRequestDto.frontdiameter != null && LoadTyreDetailsByWidthRequestDto.frontloadSpeed == null)
                {
                    response.CrossSectionList = availableTireSizes.Where(x => x.CrossSection != "0"
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth
                            && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross
                            && x.Diameter == Convert.ToInt32(LoadTyreDetailsByWidthRequestDto.frontdiameter)).Select(a => a.CrossSection).Distinct().ToList();
                    response.DiameterList = availableTireSizes.Where(x => x.Diameter != 0
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth
                            && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross
                            && x.Diameter == Convert.ToInt32(LoadTyreDetailsByWidthRequestDto.frontdiameter)).Select(a => a.Diameter).Distinct().ToList();
                    response.LoadSpeedList = availableTireSizes.Where(x => x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth
                            && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross
                            && x.Diameter == Convert.ToInt32(LoadTyreDetailsByWidthRequestDto.frontdiameter)).Select(a => a.LoadSpeed).Distinct().ToList();
                    response.WidthList = availableTireSizes.Where(x => x.Width != "0"
                            && x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth &&
                            x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross
                            && x.Diameter == Convert.ToInt32(LoadTyreDetailsByWidthRequestDto.frontdiameter)).Select(a => a.Width).Distinct().ToList();
                    response.PatternList = AvailableTireSizesPattern.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();
                }
                else if (LoadTyreDetailsByWidthRequestDto.frontwidth != "" && LoadTyreDetailsByWidthRequestDto.frontcross != null
                    && LoadTyreDetailsByWidthRequestDto.frontdiameter != null && LoadTyreDetailsByWidthRequestDto.frontloadSpeed != null)
                {
                    List<AvailableTireSizesPatternResponse> PatternL = new List<AvailableTireSizesPatternResponse>();
                    availableTireSizes = session.Query<AvailableTireSizes>().Where(x => x.Width == LoadTyreDetailsByWidthRequestDto.frontwidth
                                && x.CrossSection == LoadTyreDetailsByWidthRequestDto.frontcross
                                && x.Diameter == Convert.ToInt32(LoadTyreDetailsByWidthRequestDto.frontdiameter)
                                && x.LoadSpeed == LoadTyreDetailsByWidthRequestDto.frontloadSpeed).ToList();
                    foreach (var pattern in availableTireSizes)
                    {
                        AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                            .Where(x => x.AvailableTireSizesId == pattern.Id).FirstOrDefault();

                        if (availableTireSizesPattern != null)
                        {
                            AvailableTireSizesPatternResponse AvailableTire = new AvailableTireSizesPatternResponse()
                            {
                                Id = availableTireSizesPattern.Id,
                                AvailableTireSizesId = availableTireSizesPattern.AvailableTireSizesId,
                                Pattern = availableTireSizesPattern.Pattern
                            };
                            PatternL.Add(AvailableTire);
                        }
                    }
                    response.PatternList = PatternL.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();

                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetTyreDetailsByArticleNo(string articleNo)
        {
            object response = new object();


            try
            {
                ISession session = EntitySessionManager.GetSession();
                response = (from am in session.Query<ArticleMapping>()
                            join ats in session.Query<AvailableTireSizes>() on am.AvailableTireSizeId equals ats.Id
                            join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                            where am.ArticleNo == articleNo
                            select new { AvailableTireSizeId = ats.Id, ats.Width, ats.CrossSection, ats.Diameter, ats.LoadSpeed, atsp.Pattern, AvailableTireSizePatternId = atsp.Id }).FirstOrDefault();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetInvoiceSummaryReportDataId(Guid dealerId, Guid bordxId,
            string tpaName, string dbConnectionString)
        {
            object response = null;
            try
            {
                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\InvoiceSummary\\" + tpaName.ToLower() + "\\InvoiceSummary.sql")
                    );
                string ReportLocation = isExist ? tpaName.ToLower() : "Default";

                ISession session = EntitySessionManager.GetSession();

                String Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath +
                        "\\InvoiceSummary\\" + ReportLocation + "\\InvoiceSummary.sql"));
                Query = Query
                    .Replace("{BordxId}", bordxId.ToString())
                    .Replace("{DealerId}", dealerId.ToString());

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = "InvoiceSummary",
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\InvoiceSummary\\" + ReportLocation
                };
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(ReportDataQuery, ReportDataQuery.Id);
                    transaction.Commit();
                }

                response = reportKey;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllConfirmedBordxByYearAndMonth(int year, int month)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                response = session.Query<Bordx>()
                    .Where(a => a.Year == year && a.Month == month && a.IsConformed == true)
                    .Select(b => new
                    {
                        b.Id,
                        b.Number
                    }).Distinct().ToArray();


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllAvailableTireSizes()
        {
            AllAvailableTireSizeResponseDto response = new AllAvailableTireSizeResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<AvailableTireSizes> availableTireSizes = session.Query<AvailableTireSizes>()
                    .OrderBy(a=> a.Width)
                    .ThenBy(b=>b.CrossSection)
                    .ThenBy( c=> c.Diameter)
                    .ThenBy(d => d.LoadSpeed)
                    .ToList();
                List<AvailableTireSizesPattern> AvailableTireSizesPattern = session.Query<AvailableTireSizesPattern>().OrderBy(a=>a.Pattern).ToList();
                response.CrossSectionList = availableTireSizes.Where(x => x.CrossSection != "0").Select(a => a.CrossSection).Distinct().ToList();
                response.DiameterList = availableTireSizes.Where(x => x.Diameter != 0).Select(a => a.Diameter).Distinct().ToList();
                response.LoadSpeedList = availableTireSizes.Where(x => x.LoadSpeed != null || x.LoadSpeed != string.Empty).Select(a => a.LoadSpeed).Distinct().ToList();
                response.WidthList = availableTireSizes.Where(x => x.Width != "0").Select(a => a.Width).Distinct().ToList();
                response.PatternList = AvailableTireSizesPattern.Where(x => x.Pattern != null || x.Pattern != string.Empty).Select(a => a.Pattern).Distinct().ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object LoadInvoceCodeDetailsById(LoadInvoceCodeByIdRequestDto loadInvoceCodeByIdRequestDto)
        {
            InvoceCodeDetailsResponseDto response = new InvoceCodeDetailsResponseDto();
            try
            {
                #region validate
                if (loadInvoceCodeByIdRequestDto == null)
                {
                    return response;
                }
                #endregion
                ISession session = EntitySessionManager.GetSession();
                InvoiceCode invoiceCode = session.Query<InvoiceCode>()
                    .FirstOrDefault(a => a.Id == loadInvoceCodeByIdRequestDto.invoiceId);
                if (invoiceCode == null)
                {
                    return response;
                }

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == loadInvoceCodeByIdRequestDto.invoiceId).ToList();
                if (invoiceCodeDetails == null)
                {
                    return response;
                }

                IList<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.QueryOver<InvoiceCodeTireDetails>()
                    .WhereRestrictionOn(x => x.InvoiceCodeDetailId)
                    .IsIn(invoiceCodeDetails.Select(y => y.Id).ToList())
                    .List<InvoiceCodeTireDetails>();
                if (invoiceCodeTireDetails == null)
                {
                    return response;
                }

                IList<AvailableTireSizesPattern> availableTireSizesPattern = session.QueryOver<AvailableTireSizesPattern>()
                    .WhereRestrictionOn(a => a.Id).IsIn(invoiceCodeTireDetails.Select(b => b.AvailableTireSizesPatternId).ToList())
                    .List<AvailableTireSizesPattern>();

                if (availableTireSizesPattern == null)
                {
                    return response;
                }

                var frontLeft = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "fl");
                var frontRight = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "fr");
                var BackLeft = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "bl");
                var BackRight = invoiceCodeTireDetails.FirstOrDefault(a => a.Position.ToLower() == "br");



                AvailableTireSizesPattern frontLeftPattern = null;
                AvailableTireSizesPattern frontRightPattern = null;
                AvailableTireSizesPattern BackLeftPattern = null;
                AvailableTireSizesPattern BackRightPattern = null;

                if (frontLeft != null)
                {
                    frontLeftPattern = availableTireSizesPattern.FirstOrDefault(a => a.Id == frontLeft.AvailableTireSizesPatternId);
                    frontRightPattern = availableTireSizesPattern.FirstOrDefault(a => a.Id == frontRight.AvailableTireSizesPatternId);
                }

                if (BackLeft != null)
                {
                    BackLeftPattern = availableTireSizesPattern.FirstOrDefault(a => a.Id == BackLeft.AvailableTireSizesPatternId);
                    BackRightPattern = availableTireSizesPattern.FirstOrDefault(a => a.Id == BackRight.AvailableTireSizesPatternId);

                }


                response.PlateNumber = invoiceCode.PlateNumber;
                response.Quantity = invoiceCode.TireQuantity;
                response.TireBack = new TireDtls()
                {
                    Cross = BackLeft != null ? BackLeft.CrossSection : "0",
                    Diameter = BackLeft != null ? BackLeft.Diameter : 0,
                    LoadSpeed = BackLeft != null ? BackLeft.LoadSpeed : string.Empty,
                    Wide = BackLeft != null ? BackLeft.Width : "0",
                    SerialLeft = BackLeft != null ? BackLeft.SerialNumber : string.Empty,
                    SerialRight = BackRight != null ? BackRight.SerialNumber : string.Empty,
                    PatternLeft = BackLeftPattern != null ? BackLeftPattern.Pattern : string.Empty,
                    PatternRight = BackRightPattern != null ? BackRightPattern.Pattern : string.Empty,
                };
                response.TireFront = new TireDtls()
                {
                    Cross = frontLeft != null ? frontLeft.CrossSection : "0",
                    Diameter = frontLeft != null ? frontLeft.Diameter : 0,
                    LoadSpeed = frontLeft != null ? frontLeft.LoadSpeed : string.Empty,
                    Wide = frontLeft != null ? frontLeft.Width : "0",
                    SerialLeft = frontLeft != null ? frontLeft.SerialNumber : string.Empty,
                    SerialRight = frontRight != null ? frontRight.SerialNumber : string.Empty,
                    PatternLeft = frontLeftPattern != null ? frontLeftPattern.Pattern : string.Empty,
                    PatternRight = frontRightPattern != null ? frontRightPattern.Pattern : string.Empty,
                };

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object SearchDealerInvoiceCode(DealerInvoiceCodeSearchRequestDto dealerInvoiceCodeSearchData)
        {
            CommonGridResponseDto response = new CommonGridResponseDto();
            try
            {
                #region validate
                if (dealerInvoiceCodeSearchData == null || dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation == null
                 || dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias == null || dealerInvoiceCodeSearchData.paginationOptionsDealerDealerInvoiceSearchGrid == null)
                {
                    return response;
                }

                if (!IsGuid(dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.dealerId.ToString()) || !IsGuid(dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.dealerBranchId.ToString())
                     || !IsGuid(dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.countryId.ToString()) || !IsGuid(dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.cityId.ToString()))
                {
                    return response;
                }

                #endregion

                Expression<Func<InvoiceCode, bool>> InvoiceCodefilter =
                                 PredicateBuilder.True<InvoiceCode>();
                //mandetory filtering
                InvoiceCodefilter
                    .And(a => a.DealerId == dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.dealerId
                    && a.DealerLocation == dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.dealerBranchId
                    && a.CityId == dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.cityId
                    && a.CountryId == dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.countryId);
                //user enterd filtering
                if (dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.date != null &&
                    dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.date != DateTime.MinValue)
                {
                    InvoiceCodefilter = InvoiceCodefilter.And(x => x.GeneratedDate.Date == dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.date.Value.Date);
                }

                if (!string.IsNullOrEmpty(dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.code))
                {
                    InvoiceCodefilter = InvoiceCodefilter.And(y => y.Code.ToLower() == dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.code.ToLower());
                }

                if (!string.IsNullOrEmpty(dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.plateNo))
                {
                    InvoiceCodefilter = InvoiceCodefilter.And(z => z.PlateNumber.ToLower().Contains(dealerInvoiceCodeSearchData.dealerInvoiceSearchGridSearchCriterias.plateNo.ToLower()));
                }

                //db in action
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                var filteredInvoiceCodeData = session.Query<InvoiceCode>().Where(InvoiceCodefilter);
                filteredInvoiceCodeData = filteredInvoiceCodeData.Where(a => a.DealerId == dealerInvoiceCodeSearchData.dealerInvoiceFilterInformation.dealerId);

                long totalRecords = filteredInvoiceCodeData.Count();
                var pagedInvoiceCodeData = filteredInvoiceCodeData
                    .OrderByDescending(l => l.GeneratedDate)
                    .Skip((dealerInvoiceCodeSearchData.paginationOptionsDealerDealerInvoiceSearchGrid.pageNumber - 1) *
                    (dealerInvoiceCodeSearchData.paginationOptionsDealerDealerInvoiceSearchGrid.pageSize))
                    .Take(dealerInvoiceCodeSearchData.paginationOptionsDealerDealerInvoiceSearchGrid.pageSize);


                response = new CommonGridResponseDto()
                {
                    totalRecords = totalRecords,
                    data = pagedInvoiceCodeData.Select(a => new
                    {
                        a.Id,
                        Date = a.GeneratedDate.ToString("dd-MMM-yyyy"),
                        a.PlateNumber,
                        a.Code,
                        a.TireQuantity
                    }).ToArray()
                };
                return new JavaScriptSerializer().Serialize(response);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        internal static object GetTyreContractDetails(TyreContractRequestDto tyreContractRequestDto)
        {
            ISession session = EntitySessionManager.GetSession();
            Product product = session.Query<Product>().FirstOrDefault();

            CommodityUsageType commodityUsageType = session.Query<CommodityUsageType>().FirstOrDefault(a => a.Name == tyreContractRequestDto.CommodityUsageTypeId);
            List<object> responseList = new List<object>();
            foreach (TyreData tyre in tyreContractRequestDto.tyreData) {


            List<Guid> variantIdList = session.Query<TireSizeVariantMap>()
                   .Where(a => a.Quantity == tyre.quantity && a.SizeFrom <= tyre.diameter && a.SizeTo >= tyre.diameter && a.IsActive==true ).Select(a => a.VariantId).ToList();

                object response = (from c in session.Query<Contract>()
                                   join cil in session.Query<ContractInsuaranceLimitation>() on c.Id equals cil.ContractId
                                   join ce in session.Query<ContractExtensions>() on cil.Id equals ce.ContractInsuanceLimitationId
                                   join cev in session.Query<ContractExtensionVariant>() on ce.Id equals cev.ContractExtensionId
                                   join cep in session.Query<ContractExtensionPremium>() on ce.Id equals cep.ContractExtensionId
                                   join il in session.Query<InsuaranceLimitation>() on cil.InsuaranceLimitationId equals il.Id
                                   where c.CommodityTypeId == tyreContractRequestDto.CommodityTypeId &&
                                   c.IsActive == true &&
                                   c.CountryId == tyreContractRequestDto.countryId &&
                                   c.ProductId == product.Id &&
                                   c.DealerId == tyreContractRequestDto.dealerId &&
                                   c.CommodityUsageTypeId == commodityUsageType.Id &&
                                   c.StartDate <= tyreContractRequestDto.purchaseDate && c.EndDate >= tyreContractRequestDto.purchaseDate &&
                                   variantIdList.Contains(cev.VariantId)
                                   select new
                                   {
                                       ContractId = c.Id,
                                       ContractExtensionPremiumId = cep.Id,
                                       ContractExtensionVariantId = cev.Id,
                                       ContractExtensionsId = ce.Id,
                                       ContractInsuaranceLimitationId = cil.Id,
                                       DealName = c.DealName,
                                       ContractStartDate = tyreContractRequestDto.purchaseDate,
                                       ContractEndDate = tyreContractRequestDto.purchaseDate.AddMonths(il.Months).AddDays(-1)
                                   //    NRP = cep.NRP,
                                  //     Gross = cep.Gross
                                   }).ToList();


                responseList.Add(new { position = tyre.position, contract = response });
            }
            return  responseList;
        }


        internal static object SaveTyrePolicy(SaveTyrePolicySalesRequestDto generateInvoiceCodeData)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = "ERROR";
            try
            {
                #region validate
                if (generateInvoiceCodeData == null || generateInvoiceCodeData.dealerInvoiceDetails == null ||
                generateInvoiceCodeData.dealerInvoiceTireSaveDetails == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }

                if (!IsGuid(generateInvoiceCodeData.LoggedInUserId.ToString()))
                { response.msg = "Logged in user is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerId.ToString()))
                { response.msg = "Logged in user's dealer is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.countryId.ToString()))
                { response.msg = "Logged in user's dealer cointry is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.cityId.ToString()))
                { response.msg = "Logged in user's dealer city is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId.ToString()))
                { response.msg = "Logged in user's dealer branch is invalid."; return response; }
                //if (string.IsNullOrEmpty(generateInvoiceCodeData.dealerInvoiceDetails.plateNumber.Trim()))
                //{ response.msg = "Enterd plate number not found."; return response; }
                if (generateInvoiceCodeData.dealerInvoiceDetails.quantity == 0)
                { response.msg = "Enterd tire quantity is invalid."; return response; }




                //variant-diameter map validation & tire db availability validation
                ISession session = EntitySessionManager.GetSession();

                // validate already exist invoice Number added in Tyre
                int alreadyUsed = 0;
                if (generateInvoiceCodeData.customerDetails.invoiceNo.Length > 0)
                {
                    alreadyUsed = session.Query<CustomerEnterdInvoiceDetails>()
                          .Where(a => a.InvoiceNumber == generateInvoiceCodeData.customerDetails.invoiceNo).ToList().Count();
                    if (alreadyUsed > 0)
                    {
                        response.msg = "This Invoice Number Already Entered in Previous Tyre Sales."; return response;
                    }
                }
                else
                {
                    response.msg = "Enter Invoice Number ."; return response;
                }

                #endregion

                #region Invoice Code && Invoice Code Details


                Guid InvoiceCodeId = Guid.NewGuid();
                InvoiceCode invoiceCode = new InvoiceCode()
                {
                    CityId = generateInvoiceCodeData.dealerInvoiceDetails.cityId,
                    Code = "",
                    CountryId = generateInvoiceCodeData.dealerInvoiceDetails.countryId,
                    DealerId = generateInvoiceCodeData.dealerInvoiceDetails.dealerId,
                    DealerLocation = generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId,
                    GeneratedBy = generateInvoiceCodeData.LoggedInUserId,
                    GeneratedDate = DateTime.UtcNow,
                    Id = InvoiceCodeId,
                    //IsPolicyApproved = false,
                    //IsPolicyCreated = false,
                    PlateNumber = generateInvoiceCodeData.dealerInvoiceDetails.plateNumber,
                    //PolicyCreatedDate = SqlDateTime.MinValue.Value,
                    //PolicyId = null,
                    PurcheasedDate = generateInvoiceCodeData.contractDetails.FirstOrDefault().purchaseDate,
                    TireQuantity = generateInvoiceCodeData.dealerInvoiceDetails.quantity,
                    AllTyresAreSame = generateInvoiceCodeData.alltyres,
                    PlateRelatedCityId = generateInvoiceCodeData.customerDetails.PlateRelatedCityId
                };
                List<InvoiceCodeDetails> InvoiceCodeDetailList = new List<InvoiceCodeDetails>();
                List<InvoiceCodeTireDetails> InvoiceCodeTireDetailList = new List<InvoiceCodeTireDetails>();
                bool isFrontEnterd = false;
                decimal price = 0;

                foreach (var details in generateInvoiceCodeData.tyreDetails)
                {
                    if (details.Position == "F")
                    {
                        isFrontEnterd = true;
                    }
                    else
                    {
                        isFrontEnterd = false;
                    }

                    TireSizeVariantMap tireSizeVariantMapFront = new TireSizeVariantMap(),
                    tireSizeVariantMapback = new TireSizeVariantMap();

                    Guid AvailableTireSizesPatternIdfront = Guid.Empty;
                    Guid AvailableTireSizesPatternIdback = Guid.Empty;
                    AvailableTireSizes availableTire = new AvailableTireSizes();


                    tireSizeVariantMapFront = session.Query<TireSizeVariantMap>()
                        .FirstOrDefault(a => a.Quantity == details.TireQuantity
                        && a.SizeFrom <= details.tyres.FirstOrDefault().diameter &&
                        a.SizeTo >= details.tyres.FirstOrDefault().diameter && a.IsActive==true);

                    if (tireSizeVariantMapFront == null)
                    {
                        response.msg = "Entered front tire diameter is not matching with any existing variant."; return response;
                    }
                    else
                    {
                        string serialNo = "C"+  details.tyres[0].serialNo + "0000";
                        AvailableTireSizesPatternIdfront = (from ats in session.Query<AvailableTireSizes>()
                                                            join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                                                            join atm in session.Query<ArticleMapping>() on ats.Id equals atm.AvailableTireSizeId
                                                            where atm.ArticleNo==serialNo &&  ats.CrossSection == details.tyres.FirstOrDefault().cross && ats.Diameter == details.tyres.FirstOrDefault().diameter && ats.LoadSpeed == details.tyres.FirstOrDefault().loadSpeed
                                                            select new { AvailableTireSizePatternId = atsp.Id }).FirstOrDefault().AvailableTireSizePatternId;

                        if (AvailableTireSizesPatternIdfront == null || AvailableTireSizesPatternIdfront == Guid.Empty)
                        {
                            response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                        }
                    }




                    Guid invoiceCodeDetailId = Guid.NewGuid();
                    InvoiceCodeDetails invoiceCodeDetail = new InvoiceCodeDetails()
                    {
                        Id = invoiceCodeDetailId,
                        InvoiceCodeId = InvoiceCodeId,
                        MakeId = tireSizeVariantMapFront.MakeId,
                        ModelId = tireSizeVariantMapFront.ModelId,
                        VariantId = tireSizeVariantMapFront.VariantId,
                        Position = details.Position,
                        TireQuantity = details.TireQuantity,
                        IsPolicyApproved = false,
                        IsPolicyCreated = false,
                        PolicyCreatedDate = SqlDateTime.MinValue.Value,
                        PolicyId = null,
                        Price = details.price
                    };

                    InvoiceCodeDetailList.Add(invoiceCodeDetail);

                    foreach (var tyre in details.tyres)
                    {
                        InvoiceCodeTireDetails invoiceCodeTires = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumberOptimize(tyre),
                            CrossSection = tyre.cross,
                            Diameter = tyre.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = tyre.loadSpeed,
                            Position = tyre.position,
                            SerialNumber = tyre.serialNo,
                            Width = tyre.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront,
                            DotNumber = tyre.dotNumber,
                            Price = tyre.price
                        };
                        InvoiceCodeTireDetailList.Add(invoiceCodeTires);
                    }

                }

                #endregion

                #region customer Entered Invoice Details

                    Customer_data customer_Data = new Customer_data()
                    {
                        address1 = "",
                        address2 = "",
                        address3 = "",
                        address4 = "",
                        usageTypeId = generateInvoiceCodeData.customerDetails.usageTypeId,
                        firstName = generateInvoiceCodeData.customerDetails.firstName,
                        lastName = generateInvoiceCodeData.customerDetails.lastName,
                        mobileNo = generateInvoiceCodeData.customerDetails.mobileNo,
                        email = generateInvoiceCodeData.customerDetails.email,
                        customerTypeId = generateInvoiceCodeData.customerDetails.customerTypeId,
                        businessName = generateInvoiceCodeData.customerDetails.businessName,
                        businessTelNo = generateInvoiceCodeData.customerDetails.businessTelNo
                    };
                    CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                    //currency details validation
                    Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == invoiceCode.DealerId);

                    if (dealer == null)
                    {
                        response.msg = "Dealer is invalid.";
                        return response;
                    }
                    Guid currentPeriodId = Guid.Empty;

                    #region get conversion rate
                    decimal conversionRate = decimal.Zero;
                    try
                    {
                        DateTime utcToday = DateTime.UtcNow;
                        IEnumerable<CurrencyConversionPeriods> currencyPeriods = session.Query<CurrencyConversionPeriods>()
                        .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday);
                        if (currencyPeriods.Count() < 1)
                        {
                            response.msg = "Currency period is not set.";
                            return response;
                        }
                        currentPeriodId = currencyPeriods.FirstOrDefault().Id;

                        CurrencyConversions conversion = session
                    .Query<CurrencyConversions>().FirstOrDefault(a => a.CurrencyConversionPeriodId == currentPeriodId && a.CurrencyId == dealer.CurrencyId);
                        if (conversion != null)
                        {
                            conversionRate = conversion.Rate;
                        }
                        else
                        {
                            Currency currency = session.Query<Currency>().FirstOrDefault(a => a.Id == dealer.CurrencyId);
                            if (currency != null && currency.Code.ToLower().Contains("usd"))
                            {
                                conversionRate = decimal.One;
                            }
                            else
                            {
                                throw new Exception("Currency not exist in the system");
                            }
                        }
                        conversionRate = currencyEntityManager.GetConversionRate(dealer.CurrencyId, currentPeriodId, true);
                    }
                    catch (Exception)
                    {
                        response.msg = "Currency conversion is not found in current conversion period.";
                        return response;
                    }
                    #endregion


                    Guid customerEnterdInvoiceDetailsId = Guid.NewGuid();
                    CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = new CustomerEnterdInvoiceDetails()
                    {
                        AdditionalDetailsMakeId = generateInvoiceCodeData.customerDetails.addMakeId,
                        AdditionalDetailsModelId = generateInvoiceCodeData.customerDetails.addModelId,
                        AdditionalDetailsMileage = generateInvoiceCodeData.customerDetails.addMileage,
                        AdditionalDetailsModelYear = generateInvoiceCodeData.customerDetails.addModelYear,
                        GeneratedDateTime = DateTime.UtcNow,
                        Id = customerEnterdInvoiceDetailsId,
                        InvoiceAttachmentId = generateInvoiceCodeData.customerDetails.invoiceAttachmentId == "" ? Guid.Empty : Guid.Parse(generateInvoiceCodeData.customerDetails.invoiceAttachmentId),
                        InvoiceCodeId = invoiceCode.Id,
                        InvoiceNumber = generateInvoiceCodeData.customerDetails.invoiceNo,
                        UsageTypeCode = generateInvoiceCodeData.customerDetails.commodityUsageType
                    };
                    List<CustomerEnterdInvoiceTireDetails> customerEnterdInvoiceTireDetailsList = new List<CustomerEnterdInvoiceTireDetails>();
                    //foreach (var tireData in saveCustomerPolicyInfoRequestDto.availableTireList)
                    foreach (var tireData in InvoiceCodeDetailList)
                    {

                    List<InvoiceCodeTireDetails> invoiceCodeTireDetailslist = InvoiceCodeTireDetailList
                     .Where(a => a.InvoiceCodeDetailId == tireData.Id).ToList();

                    foreach (var invCodeTireDetailslist in invoiceCodeTireDetailslist)
                        {
                            var invTireDetail = new CustomerEnterdInvoiceTireDetails()
                            {
                                Id = Guid.NewGuid(),
                                // PurchasedPrice = Convert.ToDecimal("200")  / conversionRate, // hard coded
                                PurchasedPrice = invCodeTireDetailslist.Price / conversionRate, // Update Price With Frontend Entering Price
                                ConversionRate = conversionRate,
                                CurrencyId = dealer.CurrencyId,
                                CurrencyPeriodId = currentPeriodId,
                                CustomerEnterdInvoiceId = customerEnterdInvoiceDetailsId,
                                InvoiceCodeTireDetailId = invCodeTireDetailslist.Id,
                                TirePositionCode = invCodeTireDetailslist.Position,

                            };
                            customerEnterdInvoiceTireDetailsList.Add(invTireDetail);
                        }
                    }


                Customer customer = new Customer();
                customer = DBDTOTransformer.Instance.CustomerEnterdTirePolicyDetailsCustomer(customer_Data);
                if (customer != null && IsGuid(customer.Id.ToString()))
                {
                    customer_Data.customerId = customer.Id;
                }

                #endregion

                #region Generate Policy Bundle
                CommodityType commodityType = new CommodityType();
                Product product = new Product();

                     commodityType = session.Query<CommodityType>()
                    .FirstOrDefault(a => a.CommodityCode.ToLower() == "o");
                     product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "tyre");

                    if(commodityType == null)
                {
                     commodityType = session.Query<CommodityType>()
                   .FirstOrDefault(a => a.CommodityCode.ToLower() == "A");
                     product = session.Query<Product>()
                    .FirstOrDefault(a => a.Productcode.ToLower() == "AEW");
                }

                    PolicyBundle policyBundle = new PolicyBundle()
                    {
                        Id = Guid.NewGuid(),
                        BookletNumber = string.Empty,
                        Comment = string.Empty,
                        CommodityTypeId = commodityType.CommodityTypeId,
                        CustomerId = customer_Data.customerId,
                        DealerId = invoiceCode.DealerId,
                        DealerLocationId = invoiceCode.DealerLocation,
                        EntryDateTime = DateTime.UtcNow,
                        IsApproved = false,
                        ProductId = product.Id,
                        DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                        CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                        MWStartDate = DateTime.MinValue,
                        PolicySoldDate = DateTime.UtcNow

                    };

                #endregion

                #region Generate Policies
                    List<Policy> policyList = new List<Policy>();
                    DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == invoiceCode.DealerLocation);
                    Guid commodityUsageTypeId = new CommonEntityManager().GetCommodityUsageTypeByName(customerEnterdInvoiceDetails.UsageTypeCode);

                    foreach (InvoiceCodeDetails _invCodeDtl in InvoiceCodeDetailList)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = InvoiceCodeTireDetailList
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);

                    var dealerPrice = customerEnterdInvoiceTireDetailsList
                        .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    ContractDetails contractDetail = generateInvoiceCodeData.contractDetails.Where(a => a.Position == _invCodeDtl.Position).FirstOrDefault();
                    //eligible extension ids
                    Guid? eligibleExtensionId = contractDetail.ContractExtensionsId;

                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    if (contractExtensionPremium == null)
                        throw new DataTransfer.Exceptions.DealNotFoundException("Deal not found");


                    var premiumDetails = new ContractEntityManager().GetPremiumParsingSession(contractExtensionPremium.Id,
                    decimal.Zero, Guid.Empty,
                    eligibleContractExtension.Id,
                    eligibleContractInsuaranceLimitation.ContractId, product.Id, dealer.Id,
                    contractDetail.purchaseDate,
                    Guid.Empty, Guid.Empty,
                    _invCodeDtl.MakeId, _invCodeDtl.ModelId,
                    _invCodeDtl.VariantId, decimal.Zero,
                    Guid.Empty, dealerPrice,
                    contractDetail.purchaseDate,session) as GetPremiumResponseDto;

                    if (premiumDetails == null)
                        throw new DealNotFoundException("Deal not found");

                    var policy = new Policy()
                    {
                        Id = Guid.NewGuid(),
                        Comment = string.Empty,
                        CommodityTypeId = commodityType.CommodityTypeId,
                        CustomerId = customer_Data.customerId,
                        CustomerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerId = dealer.Id,
                        DealerPaymentCurrencyTypeId = dealer.CurrencyId,
                        DealerLocationId = dealerLocation.Id,
                        EntryDateTime = DateTime.UtcNow,
                        TPABranchId = dealerLocation.TpaBranchId,
                        ProductId = product.Id,
                        IsApproved = false,
                        CurrencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        LocalCurrencyConversionRate = contractExtensionPremium.ConversionRate,
                        IsPolicyCanceled = false,
                        CustomerPayment = decimal.Zero,
                        DealerPolicy = true,
                        DealerPayment = decimal.Zero,
                        Discount = decimal.Zero,
                        DiscountPercentage = decimal.Zero,
                        ForwardComment = string.Empty,
                        IsPartialPayment = false,
                        MWIsAvailable = false,
                        PolicyBundleId = policyBundle.Id,
                        PaymentMethodFee = decimal.Zero,
                        PaymentMethodFeePercentage = decimal.Zero,
                        IsSpecialDeal = false,
                        RefNo = string.Empty,
                        ContractExtensionPremiumId = contractExtensionPremium.Id,
                        ContractExtensionsId = eligibleContractExtension.ContractInsuanceLimitationId,
                        ContractInsuaranceLimitationId = eligibleExtensionId.Value,
                        ContractId = eligibleContractInsuaranceLimitation.ContractId,
                        CoverTypeId = contractExtensionPremium.WarrentyTypeId,
                        ExtensionTypeId = eligibleExtensionId.Value,
                        HrsUsedAtPolicySale = "0",
                        IsPolicyRenewed = false,
                        IsPreWarrantyCheck = false,
                        MWStartDate = SqlDateTime.MinValue.Value,
                        PolicySoldDate = contractDetail.purchaseDate,
                        PolicyStartDate = contractDetail.purchaseDate,
                        PolicyEndDate = contractDetail.purchaseDate.AddMonths(insuaranceLimitation.Months).AddDays(-1),//fixed 1 year extension for tyre
                        PremiumCurrencyTypeId = dealer.CurrencyId,

                        Premium = premiumDetails.TotalPremium / contractExtensionPremium.ConversionRate,
                        GrossPremiumBeforeTax = (premiumDetails.BasicPremium + premiumDetails.EligibilityPremium),
                        TotalTax = premiumDetails.Tax,
                        NRP = premiumDetails.TotalPremiumNRP / contractExtensionPremium.ConversionRate,
                        EligibilityFee = premiumDetails.EligibilityPremium,

                        TransferFee = decimal.Zero,
                        PolicyNo = null

                    };
                    policyList.Add(policy);
                }

                #endregion

                #region Generate Other Item Details

                List<OtherItemDetails> otherItemDetailList = new List<OtherItemDetails>();
                foreach (InvoiceCodeDetails _invCodeDtl in InvoiceCodeDetailList)
                {

                    InvoiceCodeTireDetails invoiceCodeTireDetails = InvoiceCodeTireDetailList
                        .FirstOrDefault(a => a.InvoiceCodeDetailId == _invCodeDtl.Id);
                    var dealerPrice = customerEnterdInvoiceTireDetailsList
                        .FirstOrDefault(a => a.InvoiceCodeTireDetailId == invoiceCodeTireDetails.Id).PurchasedPrice;
                    //get contract details
                    ContractDetails contractDetail = generateInvoiceCodeData.contractDetails.Where(a => a.Position == _invCodeDtl.Position).FirstOrDefault();
                    //eligible extension ids
                    Guid? eligibleExtensionId = contractDetail.ContractExtensionsId;


                    ContractExtensions eligibleContractExtension = session.Query<ContractExtensions>()
                        .FirstOrDefault(a => a.Id == eligibleExtensionId);
                    ContractInsuaranceLimitation eligibleContractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractExtension.ContractInsuanceLimitationId);
                    ContractExtensionPremium contractExtensionPremium = session.Query<ContractExtensionPremium>()
                        .FirstOrDefault(a => a.ContractExtensionId == eligibleExtensionId);
                    InsuaranceLimitation insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.InsuaranceLimitationId);
                    Contract contract = session.Query<Contract>()
                        .FirstOrDefault(a => a.Id == eligibleContractInsuaranceLimitation.ContractId);
                    ItemStatus itemStatus = session.Query<ItemStatus>()
                        .FirstOrDefault(a => a.Status.ToLower() == "new");

                    if (contractExtensionPremium == null)
                    {
                        return response;
                    }


                    var otherItemDetail = new OtherItemDetails()
                    {
                        Id = Guid.NewGuid(),
                        MakeId = _invCodeDtl.MakeId,
                        ModelId = _invCodeDtl.ModelId,
                        VariantId = _invCodeDtl.VariantId,
                        DealerCurrencyId = dealer.CurrencyId,
                        AddnSerialNo = string.Empty,
                        DealerPrice = dealerPrice,
                        CategoryId = contract.CommodityCategoryId,
                        CommodityUsageTypeId = contract.CommodityUsageTypeId,
                        ConversionRate = contractExtensionPremium.ConversionRate,
                        currencyPeriodId = contractExtensionPremium.CurrencyPeriodId,
                        EntryDateTime = DateTime.UtcNow,
                        ItemPrice = dealerPrice,
                        ItemPurchasedDate = invoiceCode.GeneratedDate,
                        ItemStatusId = itemStatus.Id,
                        InvoiceNo = customerEnterdInvoiceDetails.InvoiceNumber,
                    };
                    otherItemDetailList.Add(otherItemDetail);
                }
                #endregion

                #region Generate Other Item Policy Generate
                    List<OtherItemPolicy> otherItemPolicyList = new List<OtherItemPolicy>();
                    foreach (Policy policy in policyList)
                    {
                        foreach (OtherItemDetails otherItem in otherItemDetailList)
                        {
                            ContractExtensionVariant contractExtensionVariant = session.Query<ContractExtensionVariant>()
                                .FirstOrDefault(a => a.ContractExtensionId == policy.ContractInsuaranceLimitationId
                                && a.VariantId == otherItem.VariantId);
                            if (contractExtensionVariant != null)
                            {
                                var otherItemPolicy = new OtherItemPolicy()
                                {
                                    Id = Guid.NewGuid(),
                                    OtherItemId = otherItem.Id,
                                    PolicyId = policy.Id
                                };
                                otherItemPolicyList.Add(otherItemPolicy);
                            }
                        }

                    }
                #endregion

                #region Data saving with Transaction
                var policyNumbers = string.Empty;
                List<string> policyNumbersList = new List<string>();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try {


                    session.Evict(invoiceCode);
                    session.Save(invoiceCode, invoiceCode.Id);

                    foreach (InvoiceCodeDetails invDtl in InvoiceCodeDetailList)
                    {
                        session.Evict(invDtl);
                        session.Save(invDtl, invDtl.Id);
                    }

                    foreach (InvoiceCodeTireDetails invTireDtl in InvoiceCodeTireDetailList)
                    {
                        session.Evict(invTireDtl);
                        session.Save(invTireDtl, invTireDtl.Id);
                    }

                    //on save successfull, get the auto increment id and decorate it with unique code and update

                    InvoiceCode invCodeSaved = session.Query<InvoiceCode>()
                        .FirstOrDefault(a => a.Id == InvoiceCodeId);
                    if (invCodeSaved != null)
                    {
                        var sql = "SELECT CodeInt FROM InvoiceCode WHERE Id = :InvoiceCodeId ";
                        int generatedId = session
                        .CreateSQLQuery(sql)
                        .SetString("InvoiceCodeId", InvoiceCodeId.ToString())
                        .UniqueResult<int>();

                        string hash = GenerateHashIdForNumber(generatedId);
                        invCodeSaved.Code = hash;
                        session.Evict(invCodeSaved);
                        session.Update(invCodeSaved, invCodeSaved.Id);


                    #region save customer invoice details
                            customerEnterdInvoiceDetails.InvoiceCode = invCodeSaved.Code;
                            session.Save(customerEnterdInvoiceDetails, customerEnterdInvoiceDetails.Id);
                            foreach (CustomerEnterdInvoiceTireDetails customerInvTireDetail in customerEnterdInvoiceTireDetailsList)
                            {
                                session.Evict(customerInvTireDetail);
                                session.Save(customerInvTireDetail, customerInvTireDetail.Id);
                            }

                        #endregion
                        #region Save Policy Details
                        if (customer.IDTypeId == 0)
                        {
                            customer.IDTypeId = 1;
                        }

                        if (customer != null && IsGuid(customer.Id.ToString()))
                        {
                            // customer.CustomerTypeId = Convert.ToInt16("2");
                            session.Save(customer, customer.Id);
                        }

                        policyBundle.MWStartDate = SqlDateTime.MinValue.Value;
                        policyBundle.PolicySoldDate = SqlDateTime.MinValue.Value;

                        session.Save(policyBundle, policyBundle.Id);
                        int counter = 0; string generatedFirstPolicyNumber = string.Empty;
                        foreach (Policy policy in policyList)
                        {
                            counter++;
                            var policyNum = GetNextPolicyNumberForTyreNew(generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId, policy.DealerId,
                               policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId), policyBundle.CommodityTypeId, generateInvoiceCodeData.customerDetails.tpaId,session);
                            //trick for get next policy number since stored procedures not working properly inside transaction
                            if (counter == 1)
                            {
                                generatedFirstPolicyNumber = policyNum;
                            }

                            if (generatedFirstPolicyNumber == policyNum && counter != 1)
                            {
                                policyNum = GetNextPolicyNumberPlusCounterForTyreNew(counter, generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId, policy.DealerId,
                                    policy.ProductId, new CommonEntityManager().GetDealerCountryByDealerId(policy.DealerId),session);
                            }


                            policy.PolicyNo = policyNum;
                            policy.ApprovedDate = DateTime.UtcNow;
                            policyNumbersList.Add(policyNum);
                            policyNumbers += policyNum + "<br>" + (policyNumbers.Length > 0 ? "," : string.Empty);
                            session.Evict(policy);
                            session.Save(policy, policy.Id);


                        }

                        int count = 0;
                        //update invoiceCodeDetails
                        foreach (var tiredetails in InvoiceCodeDetailList)
                        {
                            count++;
                            if (count == 1)
                            {
                                tiredetails.IsPolicyCreated = true;
                                tiredetails.PolicyId = policyList.FirstOrDefault().Id;

                                session.Evict(tiredetails);
                                session.Update(tiredetails);
                            }
                            else
                            {
                                tiredetails.IsPolicyCreated = true;
                                tiredetails.PolicyId = policyList.LastOrDefault().Id;

                                session.Evict(tiredetails);
                                session.Update(tiredetails);
                            }



                        }

                        // session.Save(policyAttachment, policyAttachment.Id);

                        foreach (OtherItemDetails otherItem in otherItemDetailList)
                        {
                            session.Evict(otherItem);
                            session.Save(otherItem, otherItem.Id);
                        }

                        foreach (OtherItemPolicy otherItemPolicy in otherItemPolicyList)
                        {
                            session.Evict(otherItemPolicy);
                            session.Save(otherItemPolicy, otherItemPolicy.Id);
                        }

                        // policy attachment save
                        PolicyAttachment policyAttachment = new PolicyAttachment {
                            PolicyBundleId = policyBundle.Id,
                            UserAttachmentId = customerEnterdInvoiceDetails.InvoiceAttachmentId,
                            Id = Guid.NewGuid()
                        };
                        session.Save(policyAttachment);

                        #endregion

                        }
                        else
                    {
                        transaction.Rollback();
                        response.msg = "Error occured while generating the code.";
                        return response;
                    }
                    transaction.Commit();
                    response.code = "SUCCESS";
                    response.msg = "Your details are submitted for approval,<br> Email is sent for your reference.";

                        #region Email Notification
                        try
                         {
                            List<string> toEmailList = new List<string>();
                            toEmailList.Add(customer_Data.email);
                            string policyNumberCommaSeparated = string.Join(",", policyNumbersList);
                            new GetMyEmail().TyreSalesSubmitComfirmation(toEmailList, customer_Data.firstName, policyNumberCommaSeparated);
                            int month = DateTime.UtcNow.Month;
                            int year = DateTime.UtcNow.Year;

                            double days = DateTime.DaysInMonth(year, month);
                            double NoofdatesinPolicy = (DateTime.UtcNow - invoiceCode.PurcheasedDate).TotalDays;
                            if (NoofdatesinPolicy > days)
                            {
                                TPA tpa = session.Query<TPA>().FirstOrDefault(a => a.Id == generateInvoiceCodeData.customerDetails.tpaId);
                                var logo = new ImageEntityManager().GetImageBase64ById(tpa.Logo);
                                var bytes = Convert.FromBase64String(logo);
                                Stream stream = new MemoryStream(bytes);
                                new GetMyEmail().RejectionEmail(toEmailList, customer.FirstName, "", customer.Id, stream);
                            }
                            }
                            catch (Exception emailError)
                            {
                                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: Email Sending Problem in Tyre Sales" + emailError.Message + ", " + emailError.InnerException);
                            }
                        #endregion
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + e.Message + ", " + e.InnerException);
                        response.msg = "Error occured while Data Saving  . Please contact administrator.";
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.msg = "Error occured while validating user. Please contact administrator.";
            }

            return response;
        }

        internal static object SaveTyrePolicy_old(SaveTyrePolicySalesRequestDto generateInvoiceCodeData)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = "ERROR";
            try
            {
                #region validate
                if (generateInvoiceCodeData == null || generateInvoiceCodeData.dealerInvoiceDetails == null ||
                generateInvoiceCodeData.dealerInvoiceTireSaveDetails == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }

                if (!IsGuid(generateInvoiceCodeData.LoggedInUserId.ToString()))
                { response.msg = "Logged in user is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerId.ToString()))
                { response.msg = "Logged in user's dealer is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.countryId.ToString()))
                { response.msg = "Logged in user's dealer cointry is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.cityId.ToString()))
                { response.msg = "Logged in user's dealer city is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId.ToString()))
                { response.msg = "Logged in user's dealer branch is invalid."; return response; }
                //if (string.IsNullOrEmpty(generateInvoiceCodeData.dealerInvoiceDetails.plateNumber.Trim()))
                //{ response.msg = "Enterd plate number not found."; return response; }
                if (generateInvoiceCodeData.dealerInvoiceDetails.quantity == 0)
                { response.msg = "Enterd tire quantity is invalid."; return response; }




                //variant-diameter map validation & tire db availability validation
                ISession session = EntitySessionManager.GetSession();

                // validate already exist invoice Number added in Tyre
                int alreadyUsed = 0;
                if (generateInvoiceCodeData.customerDetails.invoiceNo.Length > 0)
                {
                    alreadyUsed = session.Query<CustomerEnterdInvoiceDetails>()
                          .Where(a => a.InvoiceNumber == generateInvoiceCodeData.customerDetails.invoiceNo).ToList().Count();
                    if (alreadyUsed > 0)
                    {
                        response.msg = "This Invoice Number Already Entered in Previous Tyre Sales."; return response;
                    }
                }
                else
                {
                    response.msg = "Enter Invoice Number ."; return response;
                }

                #endregion

                Guid InvoiceCodeId = Guid.NewGuid();
                InvoiceCode invoiceCode = new InvoiceCode()
                {
                    CityId = generateInvoiceCodeData.dealerInvoiceDetails.cityId,
                    Code = "",
                    CountryId = generateInvoiceCodeData.dealerInvoiceDetails.countryId,
                    DealerId = generateInvoiceCodeData.dealerInvoiceDetails.dealerId,
                    DealerLocation = generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId,
                    GeneratedBy = generateInvoiceCodeData.LoggedInUserId,
                    GeneratedDate = DateTime.UtcNow,
                    Id = InvoiceCodeId,
                    //IsPolicyApproved = false,
                    //IsPolicyCreated = false,
                    PlateNumber = generateInvoiceCodeData.dealerInvoiceDetails.plateNumber,
                    //PolicyCreatedDate = SqlDateTime.MinValue.Value,
                    //PolicyId = null,
                    PurcheasedDate = generateInvoiceCodeData.contractDetails.FirstOrDefault().purchaseDate,
                    TireQuantity = generateInvoiceCodeData.dealerInvoiceDetails.quantity,
                    AllTyresAreSame = generateInvoiceCodeData.alltyres,
                    PlateRelatedCityId = generateInvoiceCodeData.customerDetails.PlateRelatedCityId
                };
                List<InvoiceCodeDetails> InvoiceCodeDetailList = new List<InvoiceCodeDetails>();
                List<InvoiceCodeTireDetails> InvoiceCodeTireDetailList = new List<InvoiceCodeTireDetails>();
                bool isFrontEnterd = false;
                decimal price = 0;

                foreach (var details in generateInvoiceCodeData.tyreDetails)
                {
                    if (details.Position == "F")
                    {
                        isFrontEnterd = true;
                    }
                    else
                    {
                        isFrontEnterd = false;
                    }

                    TireSizeVariantMap tireSizeVariantMapFront = new TireSizeVariantMap(),
                    tireSizeVariantMapback = new TireSizeVariantMap();

                    Guid AvailableTireSizesPatternIdfront = Guid.Empty;
                    Guid AvailableTireSizesPatternIdback = Guid.Empty;
                    AvailableTireSizes availableTire = new AvailableTireSizes();


                    tireSizeVariantMapFront = session.Query<TireSizeVariantMap>()
                        .FirstOrDefault(a => a.Quantity == details.TireQuantity
                        && a.SizeFrom <= details.tyres.FirstOrDefault().diameter &&
                        a.SizeTo >= details.tyres.FirstOrDefault().diameter && a.IsActive==true);
                    if (tireSizeVariantMapFront == null)
                    {
                        response.msg = "Entered front tire diameter is not matching with any existing variant."; return response;
                    }
                    else
                    {
                            AvailableTireSizesPatternIdfront = (from ats in session.Query<AvailableTireSizes>()
                                                                join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                                                                where ats.CrossSection == details.tyres.FirstOrDefault().cross && ats.Diameter == details.tyres.FirstOrDefault().diameter && ats.LoadSpeed == details.tyres.FirstOrDefault().loadSpeed
                                                                select new {AvailableTireSizePatternId = atsp.Id }).FirstOrDefault().AvailableTireSizePatternId;

                            if (AvailableTireSizesPatternIdfront==null || AvailableTireSizesPatternIdfront == Guid.Empty)
                            {
                                response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                            }
                    }




                    Guid invoiceCodeDetailId = Guid.NewGuid();
                    InvoiceCodeDetails invoiceCodeDetail = new InvoiceCodeDetails()
                    {
                        Id = invoiceCodeDetailId,
                        InvoiceCodeId = InvoiceCodeId,
                        MakeId =   tireSizeVariantMapFront.MakeId ,
                        ModelId =   tireSizeVariantMapFront.ModelId ,
                        VariantId =   tireSizeVariantMapFront.VariantId,
                        Position = details.Position,
                        TireQuantity = details.TireQuantity,
                        IsPolicyApproved = false,
                        IsPolicyCreated = false,
                        PolicyCreatedDate = SqlDateTime.MinValue.Value,
                        PolicyId = null,
                        Price = details.price
                    };

                    InvoiceCodeDetailList.Add(invoiceCodeDetail);

                    foreach (var tyre in details.tyres)
                    {
                        InvoiceCodeTireDetails invoiceCodeTires = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumberOptimize(tyre),
                            CrossSection = tyre.cross,
                            Diameter = tyre.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = tyre.loadSpeed,
                            Position = tyre.position,
                            SerialNumber = tyre.serialNo,
                            Width = tyre.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront,
                            DotNumber = tyre.dotNumber,
                            Price=tyre.price
                        };
                        InvoiceCodeTireDetailList.Add(invoiceCodeTires);
                    }

                }

                //saving data

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(invoiceCode);
                    session.Save(invoiceCode, invoiceCode.Id);

                    foreach (InvoiceCodeDetails invDtl in InvoiceCodeDetailList)
                    {
                        session.Evict(invDtl);
                        session.Save(invDtl, invDtl.Id);
                    }

                    foreach (InvoiceCodeTireDetails invTireDtl in InvoiceCodeTireDetailList)
                    {
                        session.Evict(invTireDtl);
                        session.Save(invTireDtl, invTireDtl.Id);
                    }

                    //on save successfull, get the auto increment id and decorate it with unique code and update

                    InvoiceCode invCodeSaved = session.Query<InvoiceCode>()
                        .FirstOrDefault(a => a.Id == InvoiceCodeId);
                    if (invCodeSaved != null)
                    {
                        var sql = "SELECT CodeInt FROM InvoiceCode WHERE Id = '" + InvoiceCodeId.ToString() + "'";
                        int generatedId = session
                        .CreateSQLQuery(sql)
                        .UniqueResult<int>();

                        string hash = GenerateHashIdForNumber(generatedId);
                        invCodeSaved.Code = hash;
                        session.Evict(invCodeSaved);
                        session.Update(invCodeSaved, invCodeSaved.Id);
                    }
                    else
                    {
                        transaction.Rollback();
                        response.msg = "Error occured while generating the code.";
                        return response;
                    }
                    transaction.Commit();
                    response.code = "SUCCESS";

                    TyreProduct product = new TyreProduct()
                    {
                        addMakeId = generateInvoiceCodeData.customerDetails.addMakeId,
                        addMileage = generateInvoiceCodeData.customerDetails.addMileage,
                        addModelId = generateInvoiceCodeData.customerDetails.addModelId,
                        addModelYear = generateInvoiceCodeData.customerDetails.addModelYear,
                        commodityUsageType = generateInvoiceCodeData.customerDetails.commodityUsageType,
                        invoiceCode = invCodeSaved.Code,
                        invoiceNo = generateInvoiceCodeData.customerDetails.invoiceNo,
                        invoiceAttachmentId = generateInvoiceCodeData.customerDetails.invoiceAttachmentId == "" ? Guid.Empty : Guid.Parse(generateInvoiceCodeData.customerDetails.invoiceAttachmentId),
                        makeId = generateInvoiceCodeData.customerDetails.addMakeId,
                        modelId = generateInvoiceCodeData.customerDetails.addModelId,

                    };

                    Customer_data customer_Data = new Customer_data()
                    {
                        address1 = "",
                        address2 = "",
                        address3 = "",
                        address4 = "",
                        usageTypeId = generateInvoiceCodeData.customerDetails.usageTypeId,
                        firstName = generateInvoiceCodeData.customerDetails.firstName,
                        lastName = generateInvoiceCodeData.customerDetails.lastName,
                        mobileNo = generateInvoiceCodeData.customerDetails.mobileNo,
                        email = generateInvoiceCodeData.customerDetails.email,
                        customerTypeId = generateInvoiceCodeData.customerDetails.customerTypeId,
                        businessName = generateInvoiceCodeData.customerDetails.businessName,
                        businessTelNo = generateInvoiceCodeData.customerDetails.businessTelNo
                    };





                    TyrePolicySaveResponse TyrePolicySaveResponse = PolicyEntityManager.SaveCustomerEnteredInvoiceDetails2(product, generateInvoiceCodeData.customerDetails.tpaId,
                        generateInvoiceCodeData.tempInvId, generateInvoiceCodeData.customerId);

                    TyreCustomerEnterdPolicySaveResponse TyreCustomerEnterdPolicySaveResponse = PolicyEntityManager.SaveCustomerEnterdPolicyByTpaId2(TyrePolicySaveResponse, customer_Data, generateInvoiceCodeData.customerDetails.tpaId, generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId, generateInvoiceCodeData.contractDetails);

                    if (TyreCustomerEnterdPolicySaveResponse.code == "SUCCESS")
                    {
                        response.code = TyreCustomerEnterdPolicySaveResponse.code;
                        response.msg = TyreCustomerEnterdPolicySaveResponse.msg;
                    }
                    else
                    {
                        response.code = TyreCustomerEnterdPolicySaveResponse.code;
                        response.msg = TyreCustomerEnterdPolicySaveResponse.msg;
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.msg = "Error occured while validating user. Please contact administrator.";
            }

            return response;
        }




        internal static string GetNextPolicyNumberForTyreNew(Guid BranchId, Guid DealerId, Guid ProductId, Guid CountryId, Guid CommodityId, Guid TpaId , ISession session)
        {
            string response = string.Empty;
            try
            {

                TPA tpa = session.Query<TPA>().FirstOrDefault();

                //if (tpa.Name == "CycleandCarriage" || tpa.Name == "CNC" || tpa.Name == "CycleandCarriagetest" || tpa.Name == "continental" || tpa.Name == "contidentalTest" || tpa.Name=="")
                //{
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumber :BranchId,:DealerId,:ProductId,:CountryId ")
                        .SetGuid("BranchId", BranchId)
                        .SetGuid("DealerId", DealerId)
                        .SetGuid("ProductId", ProductId)
                        .SetGuid("CountryId", CountryId)
                        .UniqueResult<string>();
                //}
                //else
                //{
                //    response =
                //    session.CreateSQLQuery("exec GenerateNextPolicyNumberNew :BranchId,:DealerId,:ProductId,:CountryId,:CommodityId,:TpaId ")
                //        .SetGuid("BranchId", BranchId)
                //        .SetGuid("DealerId", DealerId)
                //        .SetGuid("ProductId", ProductId)
                //        .SetGuid("CountryId", CountryId)
                //        .SetGuid("CommodityId", CommodityId)
                //        .SetGuid("TpaId", TpaId)

                //        .UniqueResult<string>();
                //}



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        internal static string GetNextPolicyNumberPlusCounterForTyreNew(int counter, Guid dealerBranch, Guid dealerId, Guid productId, Guid countryId, ISession session)
        {
            string response = string.Empty;
            try
            {

                TPA tpa = session.Query<TPA>().FirstOrDefault();

                if (tpa.Name == "CycleandCarriage")
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", dealerBranch)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }
                else
                {
                    response =
                    session.CreateSQLQuery("exec GenerateNextPolicyNumberWithCounter :BranchId,:DealerId,:ProductId,:CountryId, :Counter ")
                        .SetGuid("BranchId", dealerBranch)
                        .SetGuid("DealerId", dealerId)
                        .SetGuid("ProductId", productId)
                        .SetGuid("CountryId", countryId)
                        .SetInt32("Counter", counter)
                        .UniqueResult<string>();
                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }


        internal static object GenerateInvoceCode(GenerateInvoiceCodeRequestDto generateInvoiceCodeData)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            response.code = "ERROR";
            try
            {
                #region validate
                if (generateInvoiceCodeData == null || generateInvoiceCodeData.dealerInvoiceDetails == null ||
                generateInvoiceCodeData.dealerInvoiceTireDetails == null)
                {
                    response.msg = "Request data is invalid.";
                    return response;
                }

                if (!IsGuid(generateInvoiceCodeData.loggedInUserId.ToString()))
                { response.msg = "Logged in user is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerId.ToString()))
                { response.msg = "Logged in user's dealer is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.countryId.ToString()))
                { response.msg = "Logged in user's dealer cointry is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.cityId.ToString()))
                { response.msg = "Logged in user's dealer city is invalid."; return response; }
                if (!IsGuid(generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId.ToString()))
                { response.msg = "Logged in user's dealer branch is invalid."; return response; }
                //if (string.IsNullOrEmpty(generateInvoiceCodeData.dealerInvoiceDetails.plateNumber.Trim()))
                //{ response.msg = "Enterd plate number not found."; return response; }
                if (generateInvoiceCodeData.dealerInvoiceDetails.quantity == 0)
                { response.msg = "Enterd tire quantity is invalid."; return response; }

                //variant-diameter map validation & tire db availability validation
                ISession session = EntitySessionManager.GetSession();
                TireSizeVariantMap tireSizeVariantMapFront = new TireSizeVariantMap(),
                    tireSizeVariantMapback = new TireSizeVariantMap();

                Guid AvailableTireSizesPatternIdfront = Guid.Empty;
                Guid AvailableTireSizesPatternIdback = Guid.Empty;
                AvailableTireSizes availableTire = new AvailableTireSizes();

                if (generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter != 0)
                {
                    tireSizeVariantMapFront = session.Query<TireSizeVariantMap>()
                        .FirstOrDefault(a => a.Quantity == generateInvoiceCodeData.dealerInvoiceDetails.quantity
                        && a.SizeFrom <= generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter &&
                        a.SizeTo >= generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter && a.IsActive==true);
                    if (tireSizeVariantMapFront == null)
                    {
                        response.msg = "Entered front tire diameter is not matching with any existing variant."; return response;
                    }
                    else
                    {
                        //existing tire match validation
                        List<AvailableTireSizesPattern> availableTireSizesPattern = new List<AvailableTireSizesPattern>();

                        availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                            .Where(a => a.Pattern == generateInvoiceCodeData.dealerInvoiceTireDetails.front.pattern).ToList();


                        if (availableTireSizesPattern.Count == 0)
                        {
                            response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                        }
                        else
                        {
                            foreach (var availableSizesPattern in availableTireSizesPattern)
                            {
                                availableTire = session.Query<AvailableTireSizes>()
                                .FirstOrDefault(a => a.CrossSection == generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross
                                && a.Diameter == generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter
                                && a.LoadSpeed.ToLower().Trim() == generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed.ToLower().Trim()
                                && a.Width == generateInvoiceCodeData.dealerInvoiceTireDetails.front.width
                                && a.Id == availableSizesPattern.AvailableTireSizesId);

                                if (availableTire == null)
                                {
                                    //response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                                }
                                else
                                {
                                    AvailableTireSizesPatternIdfront = availableSizesPattern.Id;
                                }
                            }

                            if (AvailableTireSizesPatternIdfront == Guid.Empty)
                            {
                                response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                            }

                        }
                    }
                }
                if (generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter != 0)
                {
                    tireSizeVariantMapback = session.Query<TireSizeVariantMap>()
                        .FirstOrDefault(a => a.Quantity == generateInvoiceCodeData.dealerInvoiceDetails.quantity
                        && a.SizeFrom <= generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter &&
                        a.SizeTo >= generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter && a.IsActive == true);
                    if (tireSizeVariantMapback == null)
                    {
                        response.msg = "Entered back tire diameter is not matching with any existing variant."; return response;
                    }
                    else
                    {
                        //existing tire match validation
                        List<AvailableTireSizesPattern> availableTireSizesPattern = new List<AvailableTireSizesPattern>();

                        availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                            .Where(a => a.Pattern == generateInvoiceCodeData.dealerInvoiceTireDetails.back.pattern).ToList();


                        if (availableTireSizesPattern.Count == 0)
                        {
                            response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                        }
                        else
                        {
                            foreach (var availableSizesPattern in availableTireSizesPattern)
                            {
                                availableTire = session.Query<AvailableTireSizes>()
                                   .FirstOrDefault(a => a.CrossSection == generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross
                                   && a.Diameter == generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter
                                   && a.LoadSpeed.ToLower().Trim() == generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed.ToLower().Trim()
                                   && a.Width == generateInvoiceCodeData.dealerInvoiceTireDetails.back.width
                                   && a.Id == availableSizesPattern.AvailableTireSizesId);

                                if (availableTire == null)
                                {
                                    //response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                                }
                                else
                                {
                                    AvailableTireSizesPatternIdback = availableSizesPattern.Id;
                                }
                            }
                            if (AvailableTireSizesPatternIdback == Guid.Empty)
                            {
                                response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                            }
                        }

                        //AvailableTireSizes availableTire = session.Query<AvailableTireSizes>()
                        //    .FirstOrDefault(a => a.CrossSection == generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross
                        //    && a.Diameter == generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter
                        //    && a.LoadSpeed.ToLower().Trim() == generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed.ToLower().Trim()
                        //    && a.Width == generateInvoiceCodeData.dealerInvoiceTireDetails.back.width);
                        //List<AvailableTireSizes> availableTire = session.Query<AvailableTireSizes>()
                        //    .Where(a => a.CrossSection == generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross
                        //    && a.Diameter == generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter
                        //    && a.LoadSpeed.ToLower().Trim() == generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed.ToLower().Trim()
                        //    && a.Width == generateInvoiceCodeData.dealerInvoiceTireDetails.front.width).ToList();

                        //AvailableTireSizesPattern availableTireSizesPattern = new AvailableTireSizesPattern();
                        //List<AvailableTireSizesPattern> availableTireSizesPatternList = new List<AvailableTireSizesPattern>();

                        //if (availableTire == null)
                        //{
                        //    response.msg = "Entered back tire not found in the database. Please check again or contact administrator."; return response;
                        //}
                        //else
                        //{
                        //    foreach (var avtire in availableTire)
                        //    {
                        //        availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                        //           .Where(a => a.AvailableTireSizesId == avtire.Id ).FirstOrDefault();

                        //        if (availableTireSizesPattern != null)
                        //        {
                        //            //availableTireSizesPatternList.Add(availableTireSizesPattern);
                        //            AvailableTireSizesback.Add(availableTireSizesPattern);
                        //        }
                        //    }
                        //    if (availableTireSizesPatternList.Count == 0)
                        //    {
                        //        response.msg = "Entered front tire sizes cannot found in the database. Please check again or contact administrator."; return response;
                        //    }
                        //}
                    }
                }

                #endregion

                Guid InvoiceCodeId = Guid.NewGuid();
                InvoiceCode invoiceCode = new InvoiceCode()
                {
                    CityId = generateInvoiceCodeData.dealerInvoiceDetails.cityId,
                    Code = "",
                    CountryId = generateInvoiceCodeData.dealerInvoiceDetails.countryId,
                    DealerId = generateInvoiceCodeData.dealerInvoiceDetails.dealerId,
                    DealerLocation = generateInvoiceCodeData.dealerInvoiceDetails.dealerBranchId,
                    GeneratedBy = generateInvoiceCodeData.loggedInUserId,
                    GeneratedDate = DateTime.UtcNow,
                    Id = InvoiceCodeId,
                    //IsPolicyApproved = false,
                    //IsPolicyCreated = false,
                    PlateNumber = generateInvoiceCodeData.dealerInvoiceDetails.plateNumber,
                    //PolicyCreatedDate = SqlDateTime.MinValue.Value,
                    //PolicyId = null,
                    PurcheasedDate = DateTime.Now,
                    TireQuantity = generateInvoiceCodeData.dealerInvoiceDetails.quantity
                };
                List<InvoiceCodeDetails> InvoiceCodeDetailList = new List<InvoiceCodeDetails>();
                List<InvoiceCodeTireDetails> InvoiceCodeTireDetailList = new List<InvoiceCodeTireDetails>();

                if (generateInvoiceCodeData.dealerInvoiceDetails.quantity == 2)
                {
                    bool isFrontEnterd = false;
                    if (generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter != 0)
                    {
                        isFrontEnterd = true;
                    }

                    Guid invoiceCodeDetailId = Guid.NewGuid();
                    InvoiceCodeDetails invoiceCodeDetail = new InvoiceCodeDetails()
                    {
                        Id = invoiceCodeDetailId,
                        InvoiceCodeId = InvoiceCodeId,
                        MakeId = isFrontEnterd ? tireSizeVariantMapFront.MakeId : tireSizeVariantMapback.MakeId,
                        ModelId = isFrontEnterd ? tireSizeVariantMapFront.ModelId : tireSizeVariantMapback.ModelId,
                        VariantId = isFrontEnterd ? tireSizeVariantMapFront.VariantId : tireSizeVariantMapback.VariantId,
                        Position = isFrontEnterd ? "F" : "B",
                        TireQuantity = 2,
                        IsPolicyApproved = false,
                        IsPolicyCreated = false,
                        PolicyCreatedDate = SqlDateTime.MinValue.Value,
                        PolicyId = null
                    };
                    InvoiceCodeDetailList.Add(invoiceCodeDetail);

                    //right&left tire entry
                    InvoiceCodeTireDetails invoiceCodeTireRight = new InvoiceCodeTireDetails()
                    {
                        ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, isFrontEnterd),
                        CrossSection = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross : generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                        Diameter = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter : generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                        Id = Guid.NewGuid(),
                        LoadSpeed = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed : generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                        Position = isFrontEnterd ? "FR" : "BR",
                        SerialNumber = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialRight : generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialRight,
                        Width = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.width : generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                        InvoiceCodeDetailId = invoiceCodeDetailId,
                        AvailableTireSizesPatternId = isFrontEnterd ? AvailableTireSizesPatternIdfront : AvailableTireSizesPatternIdback,
                    };
                    InvoiceCodeTireDetails invoiceCodeTireLeft = new InvoiceCodeTireDetails()
                    {
                        ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, isFrontEnterd),
                        CrossSection = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross : generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                        Diameter = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter : generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                        Id = Guid.NewGuid(),
                        LoadSpeed = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed : generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                        Position = isFrontEnterd ? "FL" : "BL",
                        SerialNumber = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialLeft : generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialLeft,
                        Width = isFrontEnterd ? generateInvoiceCodeData.dealerInvoiceTireDetails.front.width : generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                        InvoiceCodeDetailId = invoiceCodeDetailId,
                        AvailableTireSizesPatternId = isFrontEnterd ? AvailableTireSizesPatternIdfront : AvailableTireSizesPatternIdback,
                    };
                    InvoiceCodeTireDetailList.Add(invoiceCodeTireRight);
                    InvoiceCodeTireDetailList.Add(invoiceCodeTireLeft);

                }
                else
                {
                    //all 4 tire scenario
                    bool isAll4Same = false;
                    if (generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter == generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter)
                    {
                        isAll4Same = true;
                    }

                    if (isAll4Same)
                    {
                        Guid invoiceCodeDetailId = Guid.NewGuid();
                        InvoiceCodeDetails invoiceCodeDetail = new InvoiceCodeDetails()
                        {
                            Id = invoiceCodeDetailId,
                            InvoiceCodeId = InvoiceCodeId,
                            MakeId = tireSizeVariantMapFront.MakeId,
                            ModelId = tireSizeVariantMapFront.ModelId,
                            VariantId = tireSizeVariantMapFront.VariantId,
                            Position = "A",
                            TireQuantity = 4,
                            IsPolicyApproved = false,
                            IsPolicyCreated = false,
                            PolicyCreatedDate = SqlDateTime.MinValue.Value,
                            PolicyId = null
                        };
                        InvoiceCodeDetailList.Add(invoiceCodeDetail);

                        //right&left-front&back tire entry
                        InvoiceCodeTireDetails invoiceCodeTireFrontRight = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, true),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed,
                            Position = "FR",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialRight,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.front.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront
                        };
                        InvoiceCodeTireDetails invoiceCodeTireFrontLeft = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, true),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed,
                            Position = "FL",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialLeft,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.front.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront
                        };
                        InvoiceCodeTireDetails invoiceCodeTireBackRight = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, false),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                            Position = "BR",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialRight,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdback
                        };
                        InvoiceCodeTireDetails invoiceCodeTireBackLeft = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, false),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                            Position = "BL",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialLeft,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                            InvoiceCodeDetailId = invoiceCodeDetailId,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdback
                        };
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireFrontRight);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireFrontLeft);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireBackRight);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireBackLeft);

                    }
                    else
                    {
                        Guid invoiceCodeDetailIdFront = Guid.NewGuid();
                        Guid invoiceCodeDetailIdBack = Guid.NewGuid();

                        InvoiceCodeDetails invoiceCodeDetailFront = new InvoiceCodeDetails()
                        {
                            Id = invoiceCodeDetailIdFront,
                            InvoiceCodeId = InvoiceCodeId,
                            MakeId = tireSizeVariantMapFront.MakeId,
                            ModelId = tireSizeVariantMapFront.ModelId,
                            VariantId = tireSizeVariantMapFront.VariantId,
                            Position = "F",
                            TireQuantity = 2,
                            IsPolicyApproved = false,
                            IsPolicyCreated = false,
                            PolicyCreatedDate = SqlDateTime.MinValue.Value,
                            PolicyId = null
                        };
                        InvoiceCodeDetailList.Add(invoiceCodeDetailFront);
                        InvoiceCodeDetails invoiceCodeDetailBack = new InvoiceCodeDetails()
                        {
                            Id = invoiceCodeDetailIdBack,
                            InvoiceCodeId = InvoiceCodeId,
                            MakeId = tireSizeVariantMapback.MakeId,
                            ModelId = tireSizeVariantMapback.ModelId,
                            VariantId = tireSizeVariantMapback.VariantId,
                            Position = "B",
                            TireQuantity = 2,
                            IsPolicyApproved = false,
                            IsPolicyCreated = false,
                            PolicyCreatedDate = SqlDateTime.MinValue.Value,
                            PolicyId = null
                        };
                        InvoiceCodeDetailList.Add(invoiceCodeDetailBack);

                        //right&left-front&back tire entry
                        InvoiceCodeTireDetails invoiceCodeTireFrontRight = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, true),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed,
                            Position = "FR",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialRight,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.front.width,
                            InvoiceCodeDetailId = invoiceCodeDetailIdFront,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront
                        };
                        InvoiceCodeTireDetails invoiceCodeTireFrontLeft = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, true),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.front.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.front.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.front.loadSpeed,
                            Position = "FL",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.front.serialLeft,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.front.width,
                            InvoiceCodeDetailId = invoiceCodeDetailIdFront,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdfront
                        };
                        InvoiceCodeTireDetails invoiceCodeTireBackRight = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, false),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                            Position = "BR",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialRight,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                            InvoiceCodeDetailId = invoiceCodeDetailIdBack,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdback
                        };
                        InvoiceCodeTireDetails invoiceCodeTireBackLeft = new InvoiceCodeTireDetails()
                        {
                            ArticleNumber = GenerareArticleNumber(generateInvoiceCodeData.dealerInvoiceTireDetails, false),
                            CrossSection = generateInvoiceCodeData.dealerInvoiceTireDetails.back.cross,
                            Diameter = generateInvoiceCodeData.dealerInvoiceTireDetails.back.diameter,
                            Id = Guid.NewGuid(),
                            LoadSpeed = generateInvoiceCodeData.dealerInvoiceTireDetails.back.loadSpeed,
                            Position = "BL",
                            SerialNumber = generateInvoiceCodeData.dealerInvoiceTireDetails.back.serialLeft,
                            Width = generateInvoiceCodeData.dealerInvoiceTireDetails.back.width,
                            InvoiceCodeDetailId = invoiceCodeDetailIdBack,
                            AvailableTireSizesPatternId = AvailableTireSizesPatternIdback
                        };


                        InvoiceCodeTireDetailList.Add(invoiceCodeTireFrontRight);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireFrontLeft);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireBackRight);
                        InvoiceCodeTireDetailList.Add(invoiceCodeTireBackLeft);
                    }
                }

                //saving data

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(invoiceCode);
                    session.Save(invoiceCode, invoiceCode.Id);

                    foreach (InvoiceCodeDetails invDtl in InvoiceCodeDetailList)
                    {
                        session.Evict(invDtl);
                        session.Save(invDtl, invDtl.Id);
                    }

                    foreach (InvoiceCodeTireDetails invTireDtl in InvoiceCodeTireDetailList)
                    {
                        session.Evict(invTireDtl);
                        session.Save(invTireDtl, invTireDtl.Id);
                    }

                    //on save successfull, get the auto increment id and decorate it with unique code and update

                    InvoiceCode invCodeSaved = session.Query<InvoiceCode>()
                        .FirstOrDefault(a => a.Id == InvoiceCodeId);
                    if (invCodeSaved != null)
                    {
                        var sql = "SELECT CodeInt FROM InvoiceCode WHERE Id = '" + InvoiceCodeId.ToString() + "'";
                        int generatedId = session
                        .CreateSQLQuery(sql)
                        .UniqueResult<int>();

                        string hash = GenerateHashIdForNumber(generatedId);
                        invCodeSaved.Code = hash;
                        session.Evict(invCodeSaved);
                        session.Update(invCodeSaved, invCodeSaved.Id);
                    }
                    else
                    {
                        transaction.Rollback();
                        response.msg = "Error occured while generating the code.";
                        return response;
                    }

                    transaction.Commit();
                    response.code = "SUCCESS";
                    response.msg = invCodeSaved.Code;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.msg = "Error occured while validating user. Please contract administratior.";
            }

            return response;
        }

        private static string GenerateHashIdForNumber(int number)
        {
            var hashids = new Hashids(ConfigurationData.NotificationKey, 6, "ABCDEFGHIJKLMNPQRSTUVWXYZ123456789");
            var uniqueId = hashids.Encode(number);
            // var unique = hashids.Decode(id);
            return uniqueId;
        }

        private static string GenerareArticleNumbernew(dealerInvoiceTireDetails dealerInvoiceTireDetails, bool isFrontEnterd)
        {
            string articleNumber = string.Empty;
            if (isFrontEnterd)
            {
                articleNumber = dealerInvoiceTireDetails.front.width + "/" +
                    dealerInvoiceTireDetails.front.cross + "R" + dealerInvoiceTireDetails.front.diameter +
                    " " + dealerInvoiceTireDetails.front.loadSpeed;
            }
            else
            {
                articleNumber = dealerInvoiceTireDetails.back.width + "/" +
                   dealerInvoiceTireDetails.back.cross + "R" + dealerInvoiceTireDetails.back.diameter +
                   " " + dealerInvoiceTireDetails.back.loadSpeed;
            }
            return articleNumber;
        }

        private static string GenerareArticleNumberOptimize(Tyres tyre)
        {
            string articleNumber = string.Empty;

            articleNumber = tyre.width + "/" +
                tyre.cross + "R" + tyre.diameter +
                " " + tyre.loadSpeed;

            return articleNumber;
        }

        private static string GenerareArticleNumber(DealerInvoiceTireDetails dealerInvoiceTireDetails, bool isFrontEnterd)
        {
            string articleNumber = string.Empty;
            if (isFrontEnterd)
            {
                articleNumber = dealerInvoiceTireDetails.front.width + "/" +
                    dealerInvoiceTireDetails.front.cross + "R" + dealerInvoiceTireDetails.front.diameter +
                    " " + dealerInvoiceTireDetails.front.loadSpeed;
            }
            else
            {
                articleNumber = dealerInvoiceTireDetails.back.width + "/" +
                   dealerInvoiceTireDetails.back.cross + "R" + dealerInvoiceTireDetails.back.diameter +
                   " " + dealerInvoiceTireDetails.back.loadSpeed;
            }
            return articleNumber;
        }

        internal static object ValidateUserOnDealerInvoiceCodeGeneration(Guid userId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {

                if (!IsGuid(userId.ToString()))
                {
                    response.code = "ERROR";
                    response.msg = "Invalid user.";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                SystemUser systemUser = session.Query<SystemUser>()
                    .FirstOrDefault(a => a.LoginMapId == userId);
                if (systemUser == null)
                {
                    response.code = "ERROR";
                    response.msg = "User not found.";
                    return response;
                }
                UserType userType = session.Query<UserType>()
                    .FirstOrDefault(a => a.Id == systemUser.UserTypeId);
                if (userType == null)
                {
                    response.code = "ERROR";
                    response.msg = "User Type is invalid.";
                    return response;
                }
                if (userType.Code.ToUpper() != "DU")
                {
                    response.code = "ERROR";
                    response.msg = "You have to be logged in as a dealer to access this page.";
                    return response;
                }
                DealerStaff dealerStaff = session.Query<DealerStaff>()
                    .FirstOrDefault(a => a.UserId == userId);
                if (dealerStaff == null)
                {
                    response.code = "ERROR";
                    response.msg = "Logged in user is not mapped to a dealer.";
                    return response;
                }
                DealerBranchStaff dealerBranchStaff = session.Query<DealerBranchStaff>()
                    .FirstOrDefault(a => a.DealerStaffId == dealerStaff.Id);
                if (dealerBranchStaff == null)
                {
                    response.code = "ERROR";
                    response.msg = "Logged in user is not mapped to a any dealer branch.";
                    return response;
                }
                Dealer dealer = session.Query<Dealer>()
                 .FirstOrDefault(a => a.Id == dealerStaff.DealerId);
                DealerLocation dealerLocation = session.Query<DealerLocation>()
                    .FirstOrDefault(a => a.Id == dealerBranchStaff.BranchId);
                if (dealerLocation == null)
                {
                    response.code = "ERROR";
                    response.msg = "Dealer branch is not found.";
                    return response;
                }
                if (!IsGuid(dealerLocation.CityId.ToString()))
                {
                    response.code = "ERROR";
                    response.msg = "Please map a city to assigned dealer branch.(" + dealerLocation.Location + ")";
                    return response;
                }

                //good to go

                response.code = "SUCCESS";
                dynamic requiredIds = new ExpandoObject();
                requiredIds.dealerId = dealerStaff.DealerId;
                requiredIds.dealerBranchId = dealerLocation.Id;
                requiredIds.countryId = dealer.CountryId;
                requiredIds.cityId = dealerLocation.CityId;
                response.obj = requiredIds;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.code = "ERROR";
                response.msg = "Error occured while validating user. Please contact administratior.";
            }

            return response;
        }

        public DealerRespondDto GetDealerById(Guid DealerId)
        {
            ISession session = EntitySessionManager.GetSession();
            CurrencyEntityManager CurrencyEM = new CurrencyEntityManager();
            DealerRespondDto pDto = new DealerRespondDto();

            var query =
                from Dealer in session.Query<Dealer>()
                where Dealer.Id == DealerId
                select new { Dealer = Dealer };
            var result = query.ToList();

            List<DealerMakes> makes = null;
            IQueryable<DealerMakes> data = session.Query<DealerMakes>();
            makes = data.Where(x => x.DealerId == DealerId).ToList();

            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Dealer.Id;
                pDto.Type = result.First().Dealer.Type;
                pDto.CityId = result.First().Dealer.CityId;
                pDto.CommodityTypeId = result.First().Dealer.CommodityTypeId;
                pDto.CountryId = result.First().Dealer.CountryId;
                pDto.DealerAliase = result.First().Dealer.DealerAliase;
                pDto.CurrencyId = result.First().Dealer.CurrencyId;
                pDto.DealerCode = result.First().Dealer.DealerCode;
                pDto.DealerName = result.First().Dealer.DealerName;
                pDto.Makes = makes.Select(c => c.MakeId).ToList();
                pDto.IsActive = result.First().Dealer.IsActive;
                pDto.IsAutoApproval = result.First().Dealer.IsAutoApproval;
                pDto.Location = result.First().Dealer.Location;
                pDto.InsurerId = result.First().Dealer.InsurerId;
                pDto.EntryDateTime = result.First().Dealer.EntryDateTime;
                pDto.EntryUser = result.First().Dealer.EntryUser;
                pDto.IsDealerExists = true;
                pDto.ManHourRate = Math.Round(result.First().Dealer.ManHourRate * result.First().Dealer.ConversionRate * 100) / 100;

                return pDto;
            }
            else
            {
                pDto.IsDealerExists = false;
                return pDto;
            }
        }

        internal string AddDealer(DealerRequestDto Dealer,string UniqueDbName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentCurrencyPeriodId.ToString()))
                {
                    return "No currency period defined for today.";
                }

                if (!IsGuid(Dealer.CurrencyId.ToString()))
                {
                    return "Invalid currency selection for dealer.";
                }

                var conversion = EntityCacheData.GetCurrencyConversions(UniqueDbName)
                    .Where(a => a.CurrencyConversionPeriodId == currentCurrencyPeriodId && a.CurrencyId == Dealer.CurrencyId).FirstOrDefault();
                if (conversion == null)
                {
                    return "Selected currency not present in current currency conversion period.";
                }

                Dealer pr = new Entities.Dealer();

                pr.Id = new Guid();
                pr.Type = Dealer.Type;
                pr.CityId = Dealer.CityId;
                pr.CommodityTypeId = Dealer.CommodityTypeId;
                pr.CountryId = Dealer.CountryId;
                pr.CurrencyId = Dealer.CurrencyId;
                pr.DealerAliase = Dealer.DealerAliase;
                pr.DealerCode = Dealer.DealerCode;
                pr.DealerName = Dealer.DealerName;
                pr.IsActive = Dealer.IsActive;
                pr.IsAutoApproval = Dealer.IsAutoApproval;
                pr.Location = Dealer.Location;
                pr.InsurerId = Dealer.InsurerId;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.CurrencyPeriodId = currentCurrencyPeriodId;
                pr.ConversionRate = currencyEm.GetConversionRate(Dealer.CurrencyId, currentCurrencyPeriodId);
                pr.ManHourRate = currencyEm.ConvertToBaseCurrency(Dealer.ManHourRate, Dealer.CurrencyId, currentCurrencyPeriodId);
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");



                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in Dealer.Makes)
                    {
                        DealerMakes cc = new Entities.DealerMakes();
                        cc.Id = new Guid();
                        cc.MakeId = item;
                        cc.DealerId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    transaction.Commit();
                }

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured while saving dealer.";

            }
        }

        internal string AddDealerComment(AddDealerCommentRequestDto DealerComment)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                DealerComment Dc = new DealerComment();


                Dc.Id = new Guid();
                Dc.Comment = DealerComment.Comment;
                Dc.CommentCode = DealerComment.CommentCode;
                Dc.IsRejectionType = DealerComment.isrejectiontype;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(Dc);
                    session.SaveOrUpdate(Dc);
                    transaction.Commit();
                }

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured while saving dealer.";

            }
        }

        internal string UpdateDealer(DealerRequestDto Dealer,string UniqueDbName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentCurrencyPeriodId.ToString()))
                {
                    return "No currency period defined for today.";
                }

                if (!IsGuid(Dealer.CurrencyId.ToString()))
                {
                    return "Invalid currency selection for dealer.";
                }

                var conversion = EntityCacheData.GetCurrencyConversions(UniqueDbName)
                    .Where(a => a.CurrencyConversionPeriodId == currentCurrencyPeriodId && a.CurrencyId == Dealer.CurrencyId).FirstOrDefault();
                if (conversion == null)
                {
                    return "Selected currency not present in current currency conversion period.";
                }

                Dealer pr = new Entities.Dealer();

                pr.Id = Dealer.Id;
                pr.Type = Dealer.Type;
                pr.CityId = Dealer.CityId;
                pr.CommodityTypeId = Dealer.CommodityTypeId;
                pr.CountryId = Dealer.CountryId;
                pr.CurrencyId = Dealer.CurrencyId;
                pr.DealerAliase = Dealer.DealerAliase;
                pr.DealerCode = Dealer.DealerCode;
                pr.DealerName = Dealer.DealerName;
                pr.IsActive = Dealer.IsActive;
                pr.IsAutoApproval = Dealer.IsAutoApproval;
                pr.Location = Dealer.Location;
                pr.InsurerId = Dealer.InsurerId;
                pr.EntryDateTime = Dealer.EntryDateTime;
                pr.EntryUser = Dealer.EntryUser;
                pr.CurrencyPeriodId = currentCurrencyPeriodId;
                pr.ConversionRate = currencyEm.GetConversionRate(Dealer.CurrencyId, currentCurrencyPeriodId);
                pr.ManHourRate = currencyEm.ConvertToBaseCurrency(Dealer.ManHourRate, Dealer.CurrencyId, currentCurrencyPeriodId);


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    //Add New Makes
                    foreach (var item in Dealer.Makes)
                    {
                        var query = from DealerMake in session.Query<DealerMakes>()
                                    where DealerMake.MakeId == item && DealerMake.DealerId == Dealer.Id
                                    select new { Id = DealerMake.Id };
                        if (query.ToList().Count == 0)
                        {
                            DealerMakes cc = new Entities.DealerMakes();
                            cc.Id = new Guid();
                            cc.MakeId = item;
                            cc.DealerId = Dealer.Id;
                            session.SaveOrUpdate(cc);
                        }
                    }
                    //Delete Removed Makes
                    var queryDeleted = from DealerMake in session.Query<DealerMakes>()
                                       where DealerMake.DealerId == Dealer.Id
                                       select DealerMake;
                    foreach (var item in queryDeleted.ToList())
                    {
                        if (Dealer.Makes.Contains(item.MakeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    transaction.Commit();
                }

                return "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Error occured while updating dealer.";

            }
        }

        public List<DealerStaff> GetDealerStaffs()
        {
            List<DealerStaff> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<DealerStaff> DealerStaffData = session.Query<DealerStaff>();
            entities = DealerStaffData.ToList();
            return entities;
        }

        public DealerStaffResponseDto GetDealerStaffById(Guid DealerStaffId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                DealerStaffResponseDto pDto = new DealerStaffResponseDto();

                var query =
                    from DealerStaff in session.Query<DealerStaff>()
                    where DealerStaff.Id == DealerStaffId
                    select new { DealerStaff = DealerStaff };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().DealerStaff.Id;
                    pDto.DealerId = result.First().DealerStaff.DealerId;
                    pDto.UserId = result.First().DealerStaff.UserId;

                    pDto.IsDealerStaffExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsDealerStaffExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal DealerStaffAddResponse AddDealerStaff(List<DealerStaffRequestDto> DealerStaffList, List<DealerBranchRequestDto> DealerBranchList)
        {
            DealerStaffAddResponse dealerStaffAddResponse = new DealerStaffAddResponse();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                if (CheckAlreadyAssignedUserToAnotherDealer(DealerStaffList,  dealerStaffAddResponse, session).Assigned==false)
                {
                    List<DealerStaff> dealerStaffListForDel = new List<DealerStaff>();

                    foreach (DealerBranchRequestDto dealerBranchList in DealerBranchList)
                    {
                        List<DealerBranchStaff> exitingDealerBranch = session.Query<DealerBranchStaff>()
                       .Where(a => a.BranchId == dealerBranchList.BranchId).ToList();

                        foreach (DealerBranchStaff dealerBranchStaff in exitingDealerBranch)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                DealerStaff dss = session.Query<DealerStaff>().Where(a => a.Id == dealerBranchStaff.DealerStaffId).FirstOrDefault();
                                int alreadyAdded = dealerStaffListForDel.Where(a => a.Id == dss.Id).Count();
                                if (alreadyAdded == 0) {
                                    dealerStaffListForDel.Add(dss);
                                }
                                session.Evict(dealerBranchStaff);
                                session.Delete(dealerBranchStaff);
                                transaction.Commit();

                            }
                        }
                    }
                    //delete all existing maooings for dealer

                    foreach (DealerStaff dealerstaff in dealerStaffListForDel)
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Evict(dealerstaff);
                            session.Delete(dealerstaff);
                            transaction.Commit();
                        }
                    }

                    //add records

                    foreach (var DealerStaff in DealerStaffList)
                    {
                        DealerStaff pr = new Entities.DealerStaff();
                        pr.Id = new Guid();
                        pr.DealerId = DealerStaff.DealerId;
                        pr.UserId = DealerStaff.UserId;

                        SystemUser su = session.Query<SystemUser>()
                            .Where(a => a.LoginMapId == DealerStaff.UserId).FirstOrDefault();
                        Guid DealerUserTypeId = session.Query<UserType>()
                            .Where(a => a.Code.ToLower() == "du").FirstOrDefault().Id;
                        su.UserTypeId = DealerUserTypeId;

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Evict(pr);
                            session.SaveOrUpdate(pr);
                            session.Evict(su);
                            //update user type
                            session.Update(su, su.Id);
                            transaction.Commit();

                            foreach (var DealerBranch in DealerBranchList)
                            {
                                DealerBranchStaff prr = new Entities.DealerBranchStaff();
                                prr.Id = new Guid();
                                prr.BranchId = DealerBranch.BranchId;
                                prr.DealerStaffId = pr.Id;

                                using (ITransaction transactionn = session.BeginTransaction())
                                {
                                    session.Evict(prr);
                                    session.SaveOrUpdate(prr);
                                    transactionn.Commit();
                                }
                            }
                        }

                    }
                    dealerStaffAddResponse.message = "Dealer Staff Successfully Saved";
                }
                return dealerStaffAddResponse;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                dealerStaffAddResponse.message="Problem with Saving Dealer Staff";
                return dealerStaffAddResponse;
            }
        }


        internal DealerStaffAddResponse CheckAlreadyAssignedUserToAnotherDealer(List<DealerStaffRequestDto> DealerStaffList, DealerStaffAddResponse dealerStaffAddResponse, ISession session) {
            dealerStaffAddResponse.userExistList =new List<string>();
            dealerStaffAddResponse.Assigned = false;
            int index = 0;
            foreach (DealerStaffRequestDto de in DealerStaffList) {
                if (session.Query<DealerStaff>().Where(a => a.DealerId != de.DealerId && a.UserId == de.UserId).ToList().Count() > 0) {
                    InternalUser internalUser = session.Query<InternalUser>().Where(b => b.Id == de.UserId.ToString()).FirstOrDefault();
                    string existUser = internalUser.FirstName + ' ' + internalUser.LastName;
                    dealerStaffAddResponse.userExistList.Add(existUser);
                    dealerStaffAddResponse.Assigned = true;
                    index++;

                }
            }
            if (dealerStaffAddResponse.Assigned) {
                string pre = "User ";
                if (index > 1) {
                    pre = "Users ";
                }
                dealerStaffAddResponse.message = pre+" Already Assigned for Another Dealer ";
            }
            return dealerStaffAddResponse;
        }
        internal bool UpdateDealerStaff(DealerStaffRequestDto DealerStaff, bool Enabled)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                DealerStaff pr = new Entities.DealerStaff();
                if (Enabled)
                {
                    pr.Id = DealerStaff.Id;
                    pr.DealerId = DealerStaff.DealerId;
                    pr.UserId = DealerStaff.UserId;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Update(pr);
                        transaction.Commit();
                    }

                }
                else
                {
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        var queryDeleted = from Staff in session.Query<DealerStaff>()
                                           where Staff.Id == DealerStaff.Id
                                           select Staff;
                        var list = queryDeleted.ToList();
                        foreach (var item in list)
                        {
                            session.Delete(item);
                            transaction.Commit();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal static DealersRespondDto GetDealersByUserId(Guid UserId)
        {
            DealersRespondDto Response = new DealersRespondDto();

            try
            {
                Response.Dealers = new List<DealerRespondDto>();
                ISession session = EntitySessionManager.GetSession();
                List<Dealer> DealerList = new List<Dealer>();
                SystemUser su = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == UserId).FirstOrDefault();
                bool isInternalUser = false;
                if (su != null)
                {
                    UserType ut = session.Query<UserType>()
                        .Where(a => a.Id == su.UserTypeId).FirstOrDefault();
                    if (ut != null)
                    {
                        if (ut.Code.ToLower() == "iu")
                        {
                            isInternalUser = true;
                        }
                    }
                }
                if (isInternalUser)
                {
                    DealerList = session.Query<Dealer>().ToList();
                }
                else
                {
                    IEnumerable<DealerStaff> dealerStaff = session.Query<DealerStaff>().Where(a => a.UserId == UserId);
                    DealerList = session.Query<Dealer>().Where(a => dealerStaff.Any(b => b.DealerId == a.Id)).ToList();
                }

                foreach (Dealer dealer in DealerList)
                {
                    var dealerObj = new DealerRespondDto()
                    {
                        Id = dealer.Id,
                        DealerName = dealer.DealerName,
                        CurrencyId = dealer.CurrencyId
                    };
                    Response.Dealers.Add(dealerObj);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static object GetDealerInvoicesYearsMonths()
        {
            object Result = new object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                //years
                var totaYears = session.Query<Policy>()
                    .Select(a => a.PolicySoldDate.Year).Distinct().ToList();
                //months
                List<MonthsDto> monthsList = new List<MonthsDto>();
                foreach (Months month in Enum.GetValues(typeof(Months)))
                {
                    MonthsDto monthObj = new MonthsDto()
                    {
                        monthsName = month.ToString(),
                        monthsSeq = (int)month,
                        isDefault = 1 == (int)month ? true : false
                    };
                    monthsList.Add(monthObj);
                }


                MonthsAndYearsResponseDto response = new MonthsAndYearsResponseDto()
                {
                    months = monthsList.ToArray(),
                    years = totaYears.OrderByDescending(i => i).ToArray()
                };
                Result = response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Result;
        }

        internal DealerInvoicesGenerateResponseDto DealerInvoicesGenerate(DealerInvoicesGenerateRequestDto DealerInvoicesGenerateRequest)
        {
            DealerInvoicesGenerateResponseDto Response = new DealerInvoicesGenerateResponseDto();

            try
            {
                Response.Year = DealerInvoicesGenerateRequest.year;
                Response.Month = DealerInvoicesGenerateRequest.month;

                if (IsGuid(DealerInvoicesGenerateRequest.dealerId.ToString()) &&
                     DealerInvoicesGenerateRequest.year > 0 && DealerInvoicesGenerateRequest.month > 0)
                {
                    ISession session = EntitySessionManager.GetSession();
                    string DealerName = String.Empty, DealerCurrencyName = String.Empty,
                        DealerSalesPersonName = String.Empty;
                    Dealer dealer = session.Query<Dealer>().Where(a => a.Id == DealerInvoicesGenerateRequest.dealerId).FirstOrDefault();
                    if (dealer != null)
                    {
                        DealerName = dealer.DealerName;
                        DealerCurrencyName = new CommonEntityManager().GetCurrencyTypeById(dealer.CurrencyId);

                        DealerSalesPersonName = new CommonEntityManager().GetHeadOfficeContactPersonNameByDealerId(dealer.Id);
                    }

                    String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\DealerInvoiceExport.sql"));
                    Query = Query
                        .Replace("{DealerId}", DealerInvoicesGenerateRequest.dealerId.ToString())
                        .Replace("{Year}", DealerInvoicesGenerateRequest.year.ToString())
                        .Replace("{Month}", DealerInvoicesGenerateRequest.month.ToString());

                    var Policies = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<DealerInvoiceResponseDto>())
                    .List<DealerInvoiceResponseDto>();

                    List<DealerInvoiceReportColumns> ReportHeaderColumns = session.Query<DealerInvoiceReportColumns>()
                        .Where(a => a.IsActive == true).OrderBy(a => a.Sequance).ToList();
                    // var TpaName = session.Query<TPA>().FirstOrDefault().Name;
                    var TpaLogo = session.Query<TPA>().FirstOrDefault().Logo;
                    var image = new ImageEntityManager().GetImageById(TpaLogo);

                    Response.DealerInvoiceData = CreateDataTable(Policies);
                    Response.DealerCurrencyName = DealerCurrencyName;
                    Response.DealerName = DealerName;
                    Response.DealerSalesPersonName = DealerSalesPersonName;
                    Response.TpaLogo = image.DisplayImageSrc;
                    Response.DealerInvoiceReportColumns = DBDTOTransformer.Instance.DealerInvoiceReportColumnsToDealerInvoiceReportColumnsDto(ReportHeaderColumns);

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
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

        internal static object GetAllDealerSchemes()
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                response = session.Query<DealerDiscountScheme>()
                    .Where(a => a.IsActive == true)
                    .Select(a => new
                    {
                        a.Id,
                        a.SchemeName
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object SearchDealerDiscounts(DealerDiscountSchemesSearchRequestDto dealerDiscountSchemesSearchRequestDto)
        {
            object response = null;
            try
            {
                #region validation
                if (dealerDiscountSchemesSearchRequestDto == null ||
                           dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias == null ||
                           dealerDiscountSchemesSearchRequestDto.paginationOptionsDealerDiscountSearchGrid == null)
                {
                    return response;
                }

                #endregion

                if (!IsGuid(dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.id.ToString()))
                {
                    //search only item is not in update status
                    Expression<Func<DealerDiscount, bool>> dealerDiscountfilter =
                        PredicateBuilder.True<DealerDiscount>();
                    dealerDiscountfilter =
                        dealerDiscountfilter.And(
                            a =>
                                a.IsApplicable ==
                                dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.isActive);

                    if (
                        IsGuid(
                            dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.countryId
                                .ToString()))
                    {
                        dealerDiscountfilter =
                            dealerDiscountfilter.And(
                                a =>
                                    a.CountryId ==
                                    dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias
                                        .countryId);
                    }

                    if (
                        IsGuid(
                            dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.dealerId
                                .ToString()))
                    {
                        dealerDiscountfilter =
                            dealerDiscountfilter.And(
                                a =>
                                    a.DealerId ==
                                    dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias
                                        .dealerId);
                    }

                    if (
                        IsGuid(
                            dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias
                                .discounSchemeId.ToString()))
                    {
                        dealerDiscountfilter =
                            dealerDiscountfilter.And(
                                a =>
                                    a.DiscuntSchemeId ==
                                    dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias
                                        .discounSchemeId);
                    }

                    if (
                        IsGuid(
                            dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.makeId
                                .ToString()))
                    {
                        dealerDiscountfilter =
                            dealerDiscountfilter.And(
                                a =>
                                    a.MakeId ==
                                    dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.makeId);
                    }

                    if (
                        !string.IsNullOrEmpty(
                            dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias.itemType))
                    {
                        dealerDiscountfilter =
                            dealerDiscountfilter.And(
                                a =>
                                    a.PartOrLabour.ToString().ToLower() ==
                                    dealerDiscountSchemesSearchRequestDto.dealerDiscountSearchGridSearchCriterias
                                        .itemType.ToLower());
                    }

                    ISession session = EntitySessionManager.GetSession();
                    CommonEntityManager commonEm = new CommonEntityManager();
                    var filteredDealerDiscountData = session.Query<DealerDiscount>().Where(dealerDiscountfilter);


                    long totalRecords = filteredDealerDiscountData.Count();
                    var pagedDealerDiscountData = filteredDealerDiscountData.Skip(
                        (dealerDiscountSchemesSearchRequestDto.paginationOptionsDealerDiscountSearchGrid.pageNumber - 1) *
                        (dealerDiscountSchemesSearchRequestDto.paginationOptionsDealerDiscountSearchGrid.pageSize))
                        .Take(dealerDiscountSchemesSearchRequestDto.paginationOptionsDealerDiscountSearchGrid.pageSize);


                    response = new CommonGridResponseDto()
                    {
                        totalRecords = totalRecords,
                        data = pagedDealerDiscountData.Select(a => new
                        {
                            a.Id,
                            Type = a.PartOrLabour.ToLower() == "p" ? "Part" : "Labour Cost",
                            Country = commonEm.GetCountryNameById(a.CountryId),
                            Dealer = commonEm.GetDealerNameById(a.DealerId),
                            Make = a.MakeId == null ? "N/A" : commonEm.GetMakeNameById((Guid)a.MakeId),
                            From = a.StartDate.ToString("dd-MMM-yyyy"),
                            To = a.EndDate.ToString("dd-MMM-yyyy"),
                            Discount = a.DiscountPercentage.ToString("#.00") + " %",
                            Goodwill = a.GoodWillPercentage.ToString("#.00") + " %",
                            Active = a.IsApplicable ? "Yes" : "No",
                            Scheme = commonEm.GetDeakerDiscountSchemeById(a.DiscuntSchemeId)
                        }).ToArray()
                    };
                    return new JavaScriptSerializer().Serialize(response);
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object SaveDealerDiscount(DealerDiscountSaveRequestDto dealerDiscountSaveData)
        {
            object response = null;
            try
            {
                #region validation
                if (dealerDiscountSaveData == null)
                {
                    return "Request data invalid.";
                }

                if (dealerDiscountSaveData.startDate > dealerDiscountSaveData.endDate)
                {
                    return "Start date cannot be grater than end date.";
                }
                #endregion

                bool isUpdate = IsGuid(dealerDiscountSaveData.id.ToString());

                Expression<Func<DealerDiscount, bool>> dealerDiscountfilter =
                       PredicateBuilder.True<DealerDiscount>();
                dealerDiscountfilter = dealerDiscountfilter.And(a => a.CountryId == dealerDiscountSaveData.countryId &&
                    a.DealerId == dealerDiscountSaveData.dealerId &&
                    a.PartOrLabour.ToString().ToLower() == dealerDiscountSaveData.itemType.ToLower() &&
                    a.DiscuntSchemeId == dealerDiscountSaveData.discounSchemeId);

                if (IsGuid(dealerDiscountSaveData.makeId.ToString()) && dealerDiscountSaveData.itemType.ToLower() == "p")
                {
                    dealerDiscountfilter = dealerDiscountfilter.And(a => a.MakeId == dealerDiscountSaveData.makeId);
                }

                ISession session = EntitySessionManager.GetSession();
                List<DealerDiscount> dealerDiscounts =
                    session.Query<DealerDiscount>().Where(dealerDiscountfilter).ToList();

                if (dealerDiscounts.Count > 0)
                {
                    if (!isUpdate)
                    {
                        //date range validations
                        if (dealerDiscounts.Select(dealerDiscount => dealerDiscountSaveData.startDate <= dealerDiscount.EndDate &&
                                                                     dealerDiscount.StartDate <= dealerDiscountSaveData.endDate)
                                                                     .Any(overlap => overlap))
                        {
                            return "The date period overlapped with existing discount record.";
                        }
                    }
                    else
                    {
                        //skip update record and date range validation
                        if (dealerDiscounts.Where(a => a.Id != dealerDiscountSaveData.id)
                            .Select(dealerDiscount => dealerDiscountSaveData.startDate <= dealerDiscount.EndDate &&
                                    dealerDiscount.StartDate <= dealerDiscountSaveData.endDate).Any(overlap => overlap))
                        {
                            return "The date period overlapped with existing discount record.";
                        }
                    }
                }
                DealerDiscount dealerDiscountToSave = new DealerDiscount()
                {
                    Id = dealerDiscountSaveData.id,
                    DealerId = dealerDiscountSaveData.dealerId,
                    CountryId = dealerDiscountSaveData.countryId,
                    IsApplicable = dealerDiscountSaveData.isActive,
                    MakeId = dealerDiscountSaveData.makeId,
                    PartOrLabour = dealerDiscountSaveData.itemType,
                    EntryDate = DateTime.UtcNow,
                    StartDate = dealerDiscountSaveData.startDate,
                    DiscuntSchemeId = dealerDiscountSaveData.discounSchemeId,
                    EndDate = dealerDiscountSaveData.endDate,
                    DiscountPercentage = dealerDiscountSaveData.discountRate,
                    GoodWillPercentage = dealerDiscountSaveData.goodwillRate,
                    EntryBy = dealerDiscountSaveData.userId
                };

                if (!isUpdate)
                {
                    dealerDiscountToSave.Id = Guid.NewGuid();
                    if (dealerDiscountToSave.MakeId == Guid.Empty)
                    {
                        dealerDiscountToSave.MakeId = null;
                    }

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(dealerDiscountToSave);
                        session.Save(dealerDiscountToSave, dealerDiscountToSave.Id);
                        transaction.Commit();
                    }
                    response = "ok";

                }
                else
                {
                    if (dealerDiscountToSave.MakeId == Guid.Empty)
                    {
                        dealerDiscountToSave.MakeId = null;
                    }

                    session.Clear();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(dealerDiscountToSave);
                        session.SaveOrUpdate(dealerDiscountToSave);
                        transaction.Commit();
                    }
                    response = "ok";
                }

            }
            catch (Exception ex)
            {
                response = "Error occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetDealerDiscountById(Guid dealerDiscountId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                var dealerDiscount = session.Query<DealerDiscount>()
                    .FirstOrDefault(a => a.Id == dealerDiscountId);
                if (dealerDiscount != null)
                {
                    DealerDiscountDetailResponseDto dealerDiscountDetailResponseDto
                        = new DealerDiscountDetailResponseDto()
                        {
                            DealerDiscountId = dealerDiscount.Id,
                            DealerId = dealerDiscount.DealerId,
                            CountryId = dealerDiscount.CountryId,
                            MakeId = dealerDiscount.MakeId,
                            GoodWillRate = Math.Round(dealerDiscount.GoodWillPercentage * 100) / 100,
                            DiscountRate = Math.Round(dealerDiscount.DiscountPercentage * 100) / 100,
                            StartDate = dealerDiscount.StartDate.ToString("dd-MMM-yyyy"),
                            EndDate = dealerDiscount.EndDate.ToString("dd-MMM-yyyy"),
                            IsActive = dealerDiscount.IsApplicable,
                            ItemType = dealerDiscount.PartOrLabour,
                            SchemeId = dealerDiscount.DiscuntSchemeId
                        };
                    response = dealerDiscountDetailResponseDto;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal object GetDelerStaffLoacation(Guid dealerId)
        {
            ISession session = EntitySessionManager.GetSession();
            //DealerLocationsRespondDto res = new DealerLocationsRespondDto();
            object Response = null;

            try
            {
                // commented reason ##############################################
                //  after assign one user to one branch other branches not list to assign staff
                //##############################################################

                //DealerLocation DealerLocationEntities = session.Query<DealerLocation>().Where(a => a.DealerId == dealerId).FirstOrDefault();

                //DealerBranchStaff dealerBranchStaff = session.Query<DealerBranchStaff>().Where(a => a.BranchId == DealerLocationEntities.Id).FirstOrDefault();

                //if (dealerBranchStaff != null)
                //{

                    //DealerLocationsRespondDto lists = new DealerLocationsRespondDto();

                    //lists.DealerLocations = new List<DealerLocationRespondDto>();

                    //List<DealerLocation> result = session.Query<DealerLocation>().Where(a => a.Id == dealerBranchStaff.BranchId && a.DealerId == dealerId).ToList();


                //    foreach (var DealerLo in result)
                //    {
                //        DealerLocationRespondDto DealerLoc = new DealerLocationRespondDto()
                //        {

                //            Id = DealerLo.Id,
                //            CityId = DealerLo.CityId,
                //            DealerId = DealerLo.DealerId,
                //            HeadOfficeBranch = DealerLo.HeadOfficeBranch,
                //            Location = DealerLo.Location,
                //            SalesContactPerson = DealerLo.SalesContactPerson,
                //            SalesEmail = DealerLo.SalesEmail,
                //            SalesFax = DealerLo.SalesFax,
                //            SalesTelephone = DealerLo.SalesTelephone,
                //            ServiceContactPerson = DealerLo.ServiceContactPerson,
                //            ServiceEmail = DealerLo.ServiceEmail,
                //            ServiceFax = DealerLo.ServiceFax,
                //            ServiceTelephone = DealerLo.ServiceTelephone,
                //            EntryDateTime = DealerLo.EntryDateTime,
                //            EntryUser = DealerLo.EntryUser
                //        };
                //        lists.DealerLocations.Add(DealerLoc);
                //    }
                //    Response = lists;
                //}
                //else
                //{
                    DealerLocationsRespondDto lists = new DealerLocationsRespondDto();

                    lists.DealerLocations = new List<DealerLocationRespondDto>();

                    List<DealerLocation> result = session.Query<DealerLocation>().Where(a => a.DealerId == dealerId).ToList();
                    foreach (var DealerLo in result)
                    {
                        DealerLocationRespondDto DealerLoc = new DealerLocationRespondDto()
                        {

                            Id = DealerLo.Id,
                            CityId = DealerLo.CityId,
                            DealerId = DealerLo.DealerId,
                            HeadOfficeBranch = DealerLo.HeadOfficeBranch,
                            Location = DealerLo.Location,
                            SalesContactPerson = DealerLo.SalesContactPerson,
                            SalesEmail = DealerLo.SalesEmail,
                            SalesFax = DealerLo.SalesFax,
                            SalesTelephone = DealerLo.SalesTelephone,
                            ServiceContactPerson = DealerLo.ServiceContactPerson,
                            ServiceEmail = DealerLo.ServiceEmail,
                            ServiceFax = DealerLo.ServiceFax,
                            ServiceTelephone = DealerLo.ServiceTelephone,
                            EntryDateTime = DealerLo.EntryDateTime,
                            EntryUser = DealerLo.EntryUser
                        };
                        lists.DealerLocations.Add(DealerLoc);
                    }
                    Response = lists;
                //}
                return Response;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal object GetDealerStaffByDealerIdandBranchId(Guid DealerId, Guid BranchId)
        {
            //DealerStaffResponseDto Response = new DealerStaffResponseDto();
            object Response = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<object> responseList = new List<object>();
                List<DealerStaff> dealerStaffList = session.Query<DealerStaff>().Where(a => a.DealerId == DealerId).ToList();

                DealerStaffResponsedDto list = new DealerStaffResponsedDto();
                list.DealerStaff = new List<DealerStaffResponseDto>();

                foreach (var dealerStaff in dealerStaffList)
                {
                    List<DealerBranchStaff> dealerBranchStaff = session.Query<DealerBranchStaff>().Where(a => a.DealerStaffId == dealerStaff.Id && a.BranchId == BranchId).ToList();


                    if (dealerBranchStaff != null && dealerBranchStaff.Count()>0)
                    {
                        DealerStaffResponseDto DealerSt = new DealerStaffResponseDto()
                        {

                            DealerId = dealerStaff.DealerId,
                            Id = dealerStaff.Id,
                            UserId = dealerStaff.UserId
                        };
                        responseList.Add(DealerSt);
                    }

                }

                Response = responseList.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static object AddDealerLabourCharge(DealerLabourChargeSaveRequestDto dealerLabourChargeSaveSaveData , string UniqueDbName)
        {
            object response = null;

            try
            {
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();


                #region validation
                if (dealerLabourChargeSaveSaveData == null)
                {
                    return "Request data invalid.";
                }

                if (dealerLabourChargeSaveSaveData.StartDate > dealerLabourChargeSaveSaveData.EndDate)
                {
                    return "Start date cannot be grater than end date.";
                }

                if (!IsGuid(currentCurrencyPeriodId.ToString()))
                {
                    return "No currency period defined for today.";
                }

                if (!IsGuid(dealerLabourChargeSaveSaveData.CurrencyId.ToString()))
                {
                    return "Invalid currency selection for dealer.";
                }

                var conversion = EntityCacheData.GetCurrencyConversions(UniqueDbName)
                    .Where(a => a.CurrencyConversionPeriodId == currentCurrencyPeriodId && a.CurrencyId == dealerLabourChargeSaveSaveData.CurrencyId).FirstOrDefault();
                if (conversion == null)
                {
                    return "Selected currency not present in current currency conversion period.";
                }
                #endregion


                bool isUpdate = IsGuid(dealerLabourChargeSaveSaveData.Id.ToString());

                Expression<Func<DealerLabourCharge, bool>> dealerLabourChargefilter =
                      PredicateBuilder.True<DealerLabourCharge>();
                dealerLabourChargefilter = dealerLabourChargefilter.And(a => a.CountryId == dealerLabourChargeSaveSaveData.CountryId &&
                    a.DealerId == dealerLabourChargeSaveSaveData.DealerId &&
                    a.MakeId == dealerLabourChargeSaveSaveData.MakeId &&
                     dealerLabourChargeSaveSaveData.ModelId.ToString().Contains(a.ModelId.ToString()));

                ISession session = EntitySessionManager.GetSession();
                List<DealerLabourCharge> dealerLabourChargeEnterd = session.Query<DealerLabourCharge>().Where(dealerLabourChargefilter).ToList();

                if (dealerLabourChargeEnterd == null)
                {
                    return "Request data Alrady Enterd.";
                }

                Expression<Func<DealerLabourCharge, bool>> LabourChargefilter =
                      PredicateBuilder.True<DealerLabourCharge>();
                LabourChargefilter = LabourChargefilter.And(a => a.CountryId == dealerLabourChargeSaveSaveData.CountryId &&
                    a.DealerId == dealerLabourChargeSaveSaveData.DealerId &&
                    a.MakeId == dealerLabourChargeSaveSaveData.MakeId && dealerLabourChargeSaveSaveData.ModelId.ToString().Contains(a.ModelId.ToString()));

                List<DealerLabourCharge> dealerLabourCharge = session.Query<DealerLabourCharge>().Where(LabourChargefilter).ToList();


                if (dealerLabourCharge.Count > 0)
                {
                    if (!isUpdate)
                    {
                        //date range validations
                        if (dealerLabourCharge.Select(dealerDiscount => dealerLabourChargeSaveSaveData.StartDate <= dealerDiscount.EndDate &&
                                                                     dealerDiscount.StartDate <= dealerLabourChargeSaveSaveData.EndDate)
                                                                     .Any(overlap => overlap))
                        {
                            return "The date period overlapped with existing dealer labour charge record.";
                        }
                    }
                    else
                    {
                        //skip update record and date range validation
                        if (dealerLabourCharge.Where(a => a.Id != dealerLabourChargeSaveSaveData.Id)
                            .Select(dealerDiscount => dealerLabourChargeSaveSaveData.StartDate <= dealerDiscount.EndDate &&
                                    dealerDiscount.StartDate <= dealerLabourChargeSaveSaveData.EndDate).Any(overlap => overlap))
                        {
                            return "The date period overlapped with existing dealer labour charge record.";
                        }
                    }
                }

                if (!isUpdate)
                {
                    List<DealerLabourCharge> dealerLabourChargesList = new List<DealerLabourCharge>();
                    foreach (Guid modelId in dealerLabourChargeSaveSaveData.ModelId)
                    {
                        DealerLabourCharge dealerLabourChargeToSave = new DealerLabourCharge()
                        {
                            Id = dealerLabourChargeSaveSaveData.Id,
                            DealerId = dealerLabourChargeSaveSaveData.DealerId,
                            CountryId = dealerLabourChargeSaveSaveData.CountryId,
                            MakeId = dealerLabourChargeSaveSaveData.MakeId,
                            ModelId = modelId,
                            CurrencyId = dealerLabourChargeSaveSaveData.CurrencyId,
                            StartDate = dealerLabourChargeSaveSaveData.StartDate,
                            EndDate = dealerLabourChargeSaveSaveData.EndDate,
                            EntryDate = DateTime.UtcNow,
                            EntryBy = dealerLabourChargeSaveSaveData.userId,
                            CurrencyPeriodId = currentCurrencyPeriodId,
                            LabourChargeValue = currencyEm.ConvertToBaseCurrency(dealerLabourChargeSaveSaveData.LabourChargeValue,
                                                    dealerLabourChargeSaveSaveData.CurrencyId, currentCurrencyPeriodId)

                        };
                        dealerLabourChargesList.Add(dealerLabourChargeToSave);
                    }

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        foreach (DealerLabourCharge dealerLabourChargeToSave in dealerLabourChargesList)
                        {
                            dealerLabourChargeToSave.Id = Guid.NewGuid();
                            session.Evict(dealerLabourChargeToSave);
                            session.Save(dealerLabourChargeToSave, dealerLabourChargeToSave.Id);

                        }
                            transaction.Commit();
                    }
                    response = "ok";

                }
                else
                {

                    List<DealerLaborChargeUpdateContainer> dealerLabourChargesList = new List<DealerLaborChargeUpdateContainer>();
                    DealerLabourCharge baseDealerLaborCharge = session.Query<DealerLabourCharge>().Where(a=>a.Id== dealerLabourChargeSaveSaveData.Id).FirstOrDefault();

                    foreach (Guid modelId in dealerLabourChargeSaveSaveData.ModelId)
                    {
                        DealerLaborChargeUpdateContainer dealerLabourChargeSaveOrUpdateRequestDto = new DealerLaborChargeUpdateContainer();
                        Guid idValue = getDealerLabourChargeListWithSameData(baseDealerLaborCharge, modelId);
                        if (idValue==null || idValue==Guid.Empty) {
                            idValue = Guid.NewGuid();
                            dealerLabourChargeSaveOrUpdateRequestDto.save = true;
                            Expression<Func<DealerLabourCharge, bool>> lcF =
                            PredicateBuilder.True<DealerLabourCharge>();
                            lcF = lcF.And(a => a.CountryId == dealerLabourChargeSaveSaveData.CountryId &&
                                a.DealerId == dealerLabourChargeSaveSaveData.DealerId &&
                                a.MakeId == dealerLabourChargeSaveSaveData.MakeId &&
                                dealerLabourChargeSaveSaveData.ModelId.ToString().Contains(a.ModelId.ToString()));
                                int dealerLabourChargeCount = session.Query<DealerLabourCharge>().Where(LabourChargefilter).Count();
                            if (dealerLabourChargeCount > 0) {
                                return "Already Exist";
                            }
                        }


                        DealerLabourCharge dealerLabourChargeToSave = new DealerLabourCharge()
                        {
                            Id = idValue,
                            DealerId = dealerLabourChargeSaveSaveData.DealerId,
                            CountryId = dealerLabourChargeSaveSaveData.CountryId,
                            MakeId = dealerLabourChargeSaveSaveData.MakeId,
                            ModelId = modelId,
                            CurrencyId = dealerLabourChargeSaveSaveData.CurrencyId,
                            StartDate = dealerLabourChargeSaveSaveData.StartDate,
                            EndDate = dealerLabourChargeSaveSaveData.EndDate,
                            EntryDate = DateTime.UtcNow,
                            EntryBy = dealerLabourChargeSaveSaveData.userId,
                            CurrencyPeriodId = currentCurrencyPeriodId,
                            LabourChargeValue = currencyEm.ConvertToBaseCurrency(dealerLabourChargeSaveSaveData.LabourChargeValue,
                                                    dealerLabourChargeSaveSaveData.CurrencyId, currentCurrencyPeriodId)

                        };
                        dealerLabourChargeSaveOrUpdateRequestDto.DealerLabourCharge = dealerLabourChargeToSave;
                        dealerLabourChargesList.Add(dealerLabourChargeSaveOrUpdateRequestDto);

                    }
                    session.Clear();
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        foreach (DealerLaborChargeUpdateContainer conrainer in dealerLabourChargesList)
                        {
                            DealerLabourCharge labourCharge = conrainer.DealerLabourCharge;
                            session.Evict(labourCharge);
                            if (conrainer.save)
                            {
                                session.Save(labourCharge,labourCharge.Id);
                            }
                            else {
                                session.Update(labourCharge, labourCharge.Id);
                            }


                        }
                        transaction.Commit();
                    }
                    response = "ok";
                }
            }
            catch (Exception ex)
            {
                response = "Error occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static object SearchDealerLabourChargeSchemes(DealerLabourChargeSearchRequestDto dealerLabourChargeSearchRequestDto)
        {
            object response = null;

            try
            {
                if (dealerLabourChargeSearchRequestDto != null && dealerLabourChargeSearchRequestDto.paginationOptionsDealerLabourChargeSearchGrid != null)
                {
                    Expression<Func<DealerLabourCharge, bool>> filterDealerLabourCharge = PredicateBuilder.True<DealerLabourCharge>();

                    if (IsGuid(dealerLabourChargeSearchRequestDto.dealerLabourChargeSearchGridSearchCriterias.dealerId.ToString()))
                    {
                        filterDealerLabourCharge = filterDealerLabourCharge.And(a => a.DealerId ==
                                    dealerLabourChargeSearchRequestDto.dealerLabourChargeSearchGridSearchCriterias.dealerId);
                    }


                    ISession session = EntitySessionManager.GetSession();
                    CommonEntityManager commonEm = new CommonEntityManager();
                    CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                    Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                    var filteredDealerLabourCharge = session.Query<DealerLabourCharge>().Where(filterDealerLabourCharge);

                    long TotalRecords = filteredDealerLabourCharge.Count();
                    var DealerLabourChargeGridDetailsFilterd = filteredDealerLabourCharge.Skip((dealerLabourChargeSearchRequestDto.paginationOptionsDealerLabourChargeSearchGrid.pageNumber - 1) * dealerLabourChargeSearchRequestDto.paginationOptionsDealerLabourChargeSearchGrid.pageSize)
                    .Take(dealerLabourChargeSearchRequestDto.paginationOptionsDealerLabourChargeSearchGrid.pageSize)
                    .Select(a => new
                    {
                        Id = a.Id,
                        Country = commonEm.GetCountryNameById(a.CountryId),
                        Currencys = commonEm.GetCurrencyTypeByIdCode(a.CurrencyId),
                        Dealers = commonEm.GetDealerNameById(a.DealerId),
                        EndDate = a.EndDate.ToString("dd-MMM-yyyy"),
                        LabourChargeValue = currencyEm.ConvertFromBaseCurrency(a.LabourChargeValue, a.CurrencyId, currentCurrencyPeriodId),
                        Makes = commonEm.GetMakeNameById(a.MakeId),
                        Models = commonEm.GetModelNameById(a.ModelId),
                        StartDate = a.StartDate.ToString("dd-MMM-yyyy")
                    })
                    .ToArray();
                    response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = DealerLabourChargeGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }

        internal static object GetDealerLabourChargeById(Guid dealerLabourChargeId)
        {
            object response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                var dealerLabourCharge = session.Query<DealerLabourCharge>()
                    .FirstOrDefault(a => a.Id == dealerLabourChargeId);

                if (dealerLabourCharge != null)
                {
                    List<Guid> modelIdList = getModelIdsList(dealerLabourCharge);

                    DealerLabourChargeResponseDto dealerLabourChargeResponseDto
                        = new DealerLabourChargeResponseDto()
                        {
                            CurrencyId = dealerLabourCharge.CurrencyId,
                            DealerLabourChargeId = dealerLabourCharge.Id,
                            LabourChargeValue = currencyEm.ConvertFromBaseCurrency(dealerLabourCharge.LabourChargeValue, dealerLabourCharge.CurrencyId, currentCurrencyPeriodId),
                            DealerId = dealerLabourCharge.DealerId,
                            CountryId = dealerLabourCharge.CountryId,
                            MakeId = dealerLabourCharge.MakeId,
                            ModelId = modelIdList,
                            StartDate = dealerLabourCharge.StartDate.ToString("dd-MMM-yyyy"),
                            EndDate = dealerLabourCharge.EndDate.ToString("dd-MMM-yyyy"),

                        };

                    response = dealerLabourChargeResponseDto;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static List<Guid> getModelIdsList(DealerLabourCharge dealerLabourChargeResponseDto)
        {
            ISession session = EntitySessionManager.GetSession();

            List<Guid> modelList =  session.Query<DealerLabourCharge>().Where(a =>
                a.CountryId == dealerLabourChargeResponseDto.CountryId &&
                a.DealerId == dealerLabourChargeResponseDto.DealerId &&
                a.MakeId == dealerLabourChargeResponseDto.MakeId &&
                a.StartDate.Equals(dealerLabourChargeResponseDto.StartDate) &&
                a.EndDate.Equals(dealerLabourChargeResponseDto.EndDate) &&
                a.CurrencyId==dealerLabourChargeResponseDto.CurrencyId &&
                a.LabourChargeValue == dealerLabourChargeResponseDto.LabourChargeValue).Select(a=>a.ModelId).ToList();

            return modelList;
        }



        internal static Guid getDealerLabourChargeListWithSameData(DealerLabourCharge dealerLabourChargeResponseDto , Guid modelId)
        {
            ISession session = EntitySessionManager.GetSession();

            Guid id = session.Query<DealerLabourCharge>().Where(a =>
               a.CountryId == dealerLabourChargeResponseDto.CountryId &&
               a.DealerId == dealerLabourChargeResponseDto.DealerId &&
               a.MakeId == dealerLabourChargeResponseDto.MakeId &&
               a.ModelId == modelId &&
               a.StartDate.Equals(dealerLabourChargeResponseDto.StartDate) &&
               a.EndDate.Equals(dealerLabourChargeResponseDto.EndDate) &&
               a.CurrencyId == dealerLabourChargeResponseDto.CurrencyId ).Select(a=>a.Id).FirstOrDefault();

            return id;
        }

        internal static MakesResponseDto GetAllMakesByDealerId(Guid dealerId)
        {
            MakesResponseDto response = new MakesResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();


                // var query =
                //from DealerMakes in session.Query<DealerMakes>()
                //where DealerMakes.DealerId == dealerId
                //select new { DealerMakes = DealerMakes };
                //var result = query.ToList();
                IQueryable<DealerMakes> DealerMakesList = session.Query<DealerMakes>()
                    .Where(a => a.DealerId == dealerId);

                response.Makes = new List<MakeResponseDto>();

                foreach (var dealerMakesIds in DealerMakesList)
                {

                    Make data = session.Query<Make>().Where(a => a.Id == dealerMakesIds.MakeId).FirstOrDefault();
                    //makes = data.Where(a => a.Id == dealerMakesIds.DealerMakes.MakeId);

                    MakeResponseDto makeResponseDto = new MakeResponseDto()
                    {
                        Id = data.Id,
                        CommodityTypeId = data.CommodityTypeId,
                        IsActive = data.IsActive,
                        MakeCode = data.MakeCode,
                        MakeName = data.MakeName,
                        ManufacturerId = data.ManufacturerId,
                        WarantyGiven = data.WarantyGiven,
                        EntryUser = data.EntryUser,
                        IsMakeExists = data.IsMakeExists,
                        EntryDateTime = data.EntryDateTime

                    };

                    response.Makes.Add(makeResponseDto);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }
    }
}
