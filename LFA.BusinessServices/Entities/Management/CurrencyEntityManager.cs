using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using TAS.Services.Entities.Persistence;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using System.Text.RegularExpressions;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class CurrencyEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<CurrencyResponseDto> GetAllCurrencies()
        {

            ISession session = EntitySessionManager.GetSession();
            return  session.Query<Currency>().Select(s=> new CurrencyResponseDto
            {
                CurrencyName = s.CurrencyName,
                Code = s.Code,
                Symbol = s.Symbol,
                Country = s.Country,
                Id = s.Id
            }).ToList(); ;
        }

        public List<CurrencyConversionResponseDto> GetCurrencyConversions()
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<CurrencyConversions>().Select(CurrencyConversion=>new CurrencyConversionResponseDto {
                Id = CurrencyConversion.Id,
                Rate = CurrencyConversion.Rate,
                CurrencyId = CurrencyConversion.CurrencyId,
                CurrencyConversionPeriodId = CurrencyConversion.CurrencyConversionPeriodId,
                Comment = CurrencyConversion.Comment
            }).ToList();
        }

        public CurrencyConversionResponseDto GetCurrencyConversionById(Guid CurrencyConversionId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                CurrencyConversionResponseDto pDto = new CurrencyConversionResponseDto();

                var query =
                    from CurrencyConversion in session.Query<CurrencyConversions>()
                    where CurrencyConversion.Id == CurrencyConversionId
                    select new { CurrencyConversion = CurrencyConversion };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().CurrencyConversion.Id;
                    pDto.Rate = result.First().CurrencyConversion.Rate;
                    pDto.CurrencyConversionPeriodId = result.First().CurrencyConversion.CurrencyConversionPeriodId;
                    pDto.CurrencyId = result.First().CurrencyConversion.CurrencyId;
                    pDto.Comment = result.First().CurrencyConversion.Comment;

                    pDto.IsCurrencyConversionExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCurrencyConversionExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                if (session.Query<CurrencyConversions>().Count(a => a.CurrencyConversionPeriodId == CurrencyConversion.CurrencyConversionPeriodId && a.CurrencyId == CurrencyConversion.CurrencyId) == 0)
                {

                    CurrencyConversions pr = new Entities.CurrencyConversions();

                    pr.Id = new Guid();
                    pr.CurrencyId = CurrencyConversion.CurrencyId;
                    pr.Rate = CurrencyConversion.Rate;
                    pr.CurrencyConversionPeriodId = CurrencyConversion.CurrencyConversionPeriodId;
                    pr.Comment = CurrencyConversion.Comment;



                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(pr);
                        transaction.Commit();
                    }
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal List<CurrencyConversionUtilDto> getConversionRatesForMultipleCIds(List<CurrencyConversionUtilDto> currencyDealerList)
        {
            ISession session = EntitySessionManager.GetSession();
            List<String> currencyList = currencyDealerList.Select(a => a.DealerCurrencyId.ToString().ToLower()).Distinct().ToList();
            List<String> CurrencyPeriodIdList = currencyDealerList.Select(a => a.currencyPeriodId.ToString().ToLower()).Distinct().ToList();
            return session.Query<CurrencyConversions>()
                .Where(a => currencyList.Contains(a.CurrencyId.ToString().ToLower()) && CurrencyPeriodIdList.Contains(a.CurrencyConversionPeriodId.ToString().ToLower()))
                .Join(session.Query<Currency>(), b => b.CurrencyId, c => c.Id, (b, c) => new { b, c })
                .Select(ss => new CurrencyConversionUtilDto {
                    currencyPeriodId = ss.b.CurrencyConversionPeriodId,
                    DealerCurrencyId = ss.b.CurrencyId,
                    ConversionRate = ss.b.Rate ,
                    currencyCode = ss.c.Code
                }).ToList();
        }

        internal bool UpdateCurrencyConversion(CurrencyConversionRequestDto CurrencyConversion)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversions pr = new Entities.CurrencyConversions();

                pr.Id = CurrencyConversion.Id;
                pr.CurrencyId = CurrencyConversion.CurrencyId;
                pr.Rate = CurrencyConversion.Rate;
                pr.CurrencyConversionPeriodId = CurrencyConversion.CurrencyConversionPeriodId;
                pr.Comment = CurrencyConversion.Comment;

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

        public List<CurrencyConversionPeriods> GetCurrencyConversionPeriods()
        {
            List<CurrencyConversionPeriods> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CurrencyConversionPeriods> CurrencyConversionPeriodData = session.Query<CurrencyConversionPeriods>();
            entities = CurrencyConversionPeriodData.ToList();
            return entities;
        }

        public CurrencyConversionPeriodResponseDto GetCurrencyConversionPeriodById(Guid CurrencyConversionPeriodId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CurrencyConversionPeriodResponseDto pDto = new CurrencyConversionPeriodResponseDto();

                var query =
                    from CurrencyConversionPeriod in session.Query<CurrencyConversionPeriods>()
                    where CurrencyConversionPeriod.Id == CurrencyConversionPeriodId
                    select new { CurrencyConversionPeriod = CurrencyConversionPeriod };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().CurrencyConversionPeriod.Id;
                    pDto.FromDate = result.First().CurrencyConversionPeriod.FromDate;
                    pDto.ToDate = result.First().CurrencyConversionPeriod.ToDate;
                    pDto.Description = result.First().CurrencyConversionPeriod.Description;

                    pDto.IsCurrencyConversionPeriodExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCurrencyConversionPeriodExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                CurrencyConversionPeriods pr = new Entities.CurrencyConversionPeriods();

                pr.Id = new Guid();
                pr.FromDate = CurrencyConversionPeriod.FromDate;
                pr.ToDate = CurrencyConversionPeriod.ToDate;
                pr.Description = CurrencyConversionPeriod.Description;

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

        internal bool UpdateCurrencyConversionPeriod(CurrencyConversionPeriodRequestDto CurrencyConversionPeriod)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversionPeriods pr = new Entities.CurrencyConversionPeriods();

                pr.Id = CurrencyConversionPeriod.Id;
                pr.FromDate = CurrencyConversionPeriod.FromDate;
                pr.ToDate = CurrencyConversionPeriod.ToDate;
                pr.Description = CurrencyConversionPeriod.Description;

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

        public List<CurrencyEmail> GetCurrencyEmails()
        {
            List<CurrencyEmail> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CurrencyEmail> CurrencyEmailData = session.Query<CurrencyEmail>();
            entities = CurrencyEmailData.ToList();
            return entities;
        }

        public CurrencyEmailResponseDto GetCurrencyEmailById(Guid CurrencyEmailId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CurrencyEmailResponseDto pDto = new CurrencyEmailResponseDto();

                var query =
                    from CurrencyEmail in session.Query<CurrencyEmail>()
                    where CurrencyEmail.Id == CurrencyEmailId
                    select new { CurrencyEmail = CurrencyEmail };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().CurrencyEmail.Id;
                    pDto.AdminEmail = result.First().CurrencyEmail.AdminEmail;
                    pDto.FirstDurationType = result.First().CurrencyEmail.FirstDurationType;
                    pDto.FirstEmailDuration = result.First().CurrencyEmail.FirstEmailDuration;
                    pDto.FirstMailBody = result.First().CurrencyEmail.FirstMailBody;
                    pDto.FirstMailSubject = result.First().CurrencyEmail.FirstMailSubject;
                    pDto.LastDurationType = result.First().CurrencyEmail.LastDurationType;
                    pDto.LastEmailDuration = result.First().CurrencyEmail.LastEmailDuration;
                    pDto.LastMailBody = result.First().CurrencyEmail.LastMailBody;
                    pDto.LastMailSubject = result.First().CurrencyEmail.LastMailSubject;
                    pDto.SecoundDurationType = result.First().CurrencyEmail.SecoundDurationType;
                    pDto.SecoundEmailDuration = result.First().CurrencyEmail.SecoundEmailDuration;
                    pDto.SecoundMailBody = result.First().CurrencyEmail.SecoundMailBody;
                    pDto.SecoundMailSubject = result.First().CurrencyEmail.SecoundMailSubject;
                    pDto.TPAEmail = result.First().CurrencyEmail.TPAEmail;

                    pDto.IsCurrencyEmailExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCurrencyEmailExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                CurrencyEmail pr = new Entities.CurrencyEmail();

                pr.Id = new Guid();
                pr.AdminEmail = CurrencyEmail.AdminEmail;
                pr.FirstDurationType = CurrencyEmail.FirstDurationType;
                pr.FirstEmailDuration = CurrencyEmail.FirstEmailDuration;
                pr.FirstMailBody = CurrencyEmail.FirstMailBody;
                pr.FirstMailSubject = CurrencyEmail.FirstMailSubject;
                pr.LastDurationType = CurrencyEmail.LastDurationType;
                pr.LastEmailDuration = CurrencyEmail.LastEmailDuration;
                pr.LastMailBody = CurrencyEmail.LastMailBody;
                pr.LastMailSubject = CurrencyEmail.LastMailSubject;
                pr.SecoundDurationType = CurrencyEmail.SecoundDurationType;
                pr.SecoundEmailDuration = CurrencyEmail.SecoundEmailDuration;
                pr.SecoundMailBody = CurrencyEmail.SecoundMailBody;
                pr.SecoundMailSubject = CurrencyEmail.SecoundMailSubject;
                pr.TPAEmail = CurrencyEmail.TPAEmail;

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

        internal bool UpdateCurrencyEmail(CurrencyEmailRequestDto CurrencyEmail)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEmail pr = new Entities.CurrencyEmail();

                pr.Id = CurrencyEmail.Id;
                pr.AdminEmail = CurrencyEmail.AdminEmail;
                pr.FirstDurationType = CurrencyEmail.FirstDurationType;
                pr.FirstEmailDuration = CurrencyEmail.FirstEmailDuration;
                pr.FirstMailBody = CurrencyEmail.FirstMailBody;
                pr.FirstMailSubject = CurrencyEmail.FirstMailSubject;
                pr.LastDurationType = CurrencyEmail.LastDurationType;
                pr.LastEmailDuration = CurrencyEmail.LastEmailDuration;
                pr.LastMailBody = CurrencyEmail.LastMailBody;
                pr.LastMailSubject = CurrencyEmail.LastMailSubject;
                pr.SecoundDurationType = CurrencyEmail.SecoundDurationType;
                pr.SecoundEmailDuration = CurrencyEmail.SecoundEmailDuration;
                pr.SecoundMailBody = CurrencyEmail.SecoundMailBody;
                pr.SecoundMailSubject = CurrencyEmail.SecoundMailSubject;
                pr.TPAEmail = CurrencyEmail.TPAEmail;

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

        internal static string CurrencyPeriodOverlapsValidate(CurrencyConversionPeriodRequestDto CurrencyConversionPeriodRequest)
        {
            String Response = String.Empty;
            try
            {



                ISession session = EntitySessionManager.GetSession();
                IEnumerable<CurrencyConversionPeriods> currencyPeriods;
                if (CurrencyConversionPeriodRequest.Id != Guid.Empty)
                {
                    currencyPeriods = session.Query<CurrencyConversionPeriods>()
                       .Where(a =>
                           ((a.FromDate <= CurrencyConversionPeriodRequest.FromDate && a.ToDate >= CurrencyConversionPeriodRequest.FromDate)
                           ||
                           (a.FromDate <= CurrencyConversionPeriodRequest.ToDate && a.ToDate >= CurrencyConversionPeriodRequest.ToDate))
                           &&
                           a.Id != CurrencyConversionPeriodRequest.Id
                           );
                }
                else
                {
                    currencyPeriods = session.Query<CurrencyConversionPeriods>()
                   .Where(a =>
                       (a.FromDate <= CurrencyConversionPeriodRequest.FromDate && a.ToDate >= CurrencyConversionPeriodRequest.FromDate)
                       ||
                       (a.FromDate <= CurrencyConversionPeriodRequest.ToDate && a.ToDate >= CurrencyConversionPeriodRequest.ToDate)
                       );
                }


                if (currencyPeriods.Count() > 0)
                {
                    if (currencyPeriods.FirstOrDefault().Description != null)
                    {
                        Response = "From date or To date overlaps with another currency conversion period discribed as " + currencyPeriods.FirstOrDefault().Description;
                    }
                    else
                    {
                        Response = "From date or To date overlaps with another currency conversion period.";
                    }
                }



                bool isUpdate = IsGuid(CurrencyConversionPeriodRequest.Id.ToString());

                List<CurrencyConversionPeriods> currencyConversionPeriods =
                    session.Query<CurrencyConversionPeriods>().ToList();

                if (currencyConversionPeriods.Count > 0)
                {
                    if (!isUpdate)
                    {
                        //date range validations
                        if (currencyConversionPeriods.Select(currencyConversion => CurrencyConversionPeriodRequest.FromDate <= currencyConversion.ToDate &&
                                                                     currencyConversion.FromDate <= CurrencyConversionPeriodRequest.ToDate)
                                                                     .Any(overlap => overlap))
                        {
                            return Response = "The date period overlapped with existing currency conversion period.";
                        }
                    }
                    else
                    {
                        //skip update record and date range validation
                        if (currencyConversionPeriods.Where(a => a.Id != CurrencyConversionPeriodRequest.Id)
                            .Select(currencyConversion => CurrencyConversionPeriodRequest.FromDate <= currencyConversion.ToDate &&
                                    currencyConversion.FromDate <= CurrencyConversionPeriodRequest.ToDate).Any(overlap => overlap))
                        {
                            return Response = "The date period overlapped with existing currency conversion period.";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                Response = "Error Occured while validating currency conversion period.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static bool? CurrencyPeriodCheckUnit()
        {
            try
            {
                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<CurrencyConversionPeriods> currencyPeriods = session.Query<CurrencyConversionPeriods>()
                    .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday);
                if (currencyPeriods.Count() > 0)
                    return true;
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal Guid GetCurrentCurrencyPeriodId()
        {
            Guid Response = Guid.Empty;
            try
            {
                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<CurrencyConversionPeriods> currencyPeriods = session.Query<CurrencyConversionPeriods>()
                    .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday);
                if (currencyPeriods.Count() > 0)
                {
                    Response = currencyPeriods.FirstOrDefault().Id;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal decimal ConvertToBaseCurrency(decimal value, Guid currencyId)
        {
            Decimal Response = value;
            try
            {
                if (value == Decimal.Zero)
                    return value;

                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();

                Currency dealerCurrency = session.Query<Currency>().Where(a => a.Id == currencyId).FirstOrDefault();
                if (dealerCurrency != null && dealerCurrency.Code.ToLower().StartsWith("usd"))
                {
                    Response = value;
                    return Response;
                }

                CurrencyConversionPeriods currencyPeriod = session.Query<CurrencyConversionPeriods>()
                   .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday).FirstOrDefault();
                if (currencyPeriod != null)
                {
                    CurrencyConversions currencyConversion = session.Query<CurrencyConversions>()
                        .Where(a => a.CurrencyConversionPeriodId == currencyPeriod.Id && a.CurrencyId == currencyId).FirstOrDefault();
                    if (currencyConversion != null)
                    {
                        Response = (value / currencyConversion.Rate);
                    }
                    else
                    {
                        //specified currency is not defined in the period
                        //saving it as usd
                        throw new Exception("Specified dealer currency is not present in the conversions");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
            return Response;

        }
        internal decimal ConvertToBaseCurrency(decimal value, Guid currencyId, Guid currencyPeriodId)
        {
            Decimal Response = value;
            try
            {
                if (value == Decimal.Zero)
                    return value;

                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();

                Currency dealerCurrency = session.Query<Currency>().FirstOrDefault(a => a.Id == currencyId);
                if (dealerCurrency != null && dealerCurrency.Code.ToLower().StartsWith("usd"))
                {
                    Response = value;
                    return Response;
                }

                CurrencyConversionPeriods currencyPeriod = session
                   .Query<CurrencyConversionPeriods>().FirstOrDefault(a => a.Id == currencyPeriodId);
                if (currencyPeriod != null)
                {
                    CurrencyConversions currencyConversion = session
                        .Query<CurrencyConversions>().FirstOrDefault(a => a.CurrencyConversionPeriodId == currencyPeriod.Id && a.CurrencyId == currencyId);
                    if (currencyConversion != null)
                    {
                        Response = (value / currencyConversion.Rate);
                    }
                    else
                    {
                        //specified currency is not defined in the period
                        //saving it as usd
                        throw new Exception("Specified dealer currency is not present in the conversions");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
            return Response;

        }
        public decimal ConvertFromBaseCurrency(decimal value, Guid currencyId)
        {
            Decimal Response = value;
            try
            {
                if (value == Decimal.Zero)
                    return value;

                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();

                Currency dealerCurrency = session.Query<Currency>().Where(a => a.Id == currencyId).FirstOrDefault();
                if (dealerCurrency != null && dealerCurrency.Code.ToLower().StartsWith("usd"))
                {
                    Response = value;
                    return Math.Round(Response * 100) / 100;
                }

                CurrencyConversionPeriods currencyPeriod = session.Query<CurrencyConversionPeriods>()
                   .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday).FirstOrDefault();
                if (currencyPeriod != null)
                {
                    CurrencyConversions currencyConversion = session.Query<CurrencyConversions>()
                        .Where(a => a.CurrencyConversionPeriodId == currencyPeriod.Id && a.CurrencyId == currencyId).FirstOrDefault();
                    if (currencyConversion != null)
                    {
                        Response = (value * currencyConversion.Rate);
                    }
                    else
                    {
                        //specified currency is not defined in the period
                        //saving it as usd
                        throw new Exception("Specified dealer currency is not present in the conversions");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
            return Math.Round(Response * 100) / 100;

        }


        public decimal ConvertFromBaseCurrency(decimal value, Guid currencyId, Guid currencyPeriodId)
        {
            Decimal Response = value;
            try
            {
                if (value == Decimal.Zero)
                    return value;


                ISession session = EntitySessionManager.GetSession();

                Currency dealerCurrency = session.Query<Currency>().Where(a=>a.Id==currencyId).FirstOrDefault();
                if (dealerCurrency != null && dealerCurrency.Code.ToLower().StartsWith("usd"))
                {
                    Response = value;
                    return Math.Round(Response * 100) / 100;
                }

                CurrencyConversionPeriods currencyPeriod = session
                   .Query<CurrencyConversionPeriods>().Where(a=>a.Id==currencyPeriodId).FirstOrDefault();
                if (currencyPeriod != null)
                {
                    CurrencyConversions currencyConversion = session
                        .Query<CurrencyConversions>().Where(a => a.CurrencyConversionPeriodId == currencyPeriod.Id && a.CurrencyId == currencyId).FirstOrDefault();
                    if (currencyConversion != null)
                    {
                        Response = (value * currencyConversion.Rate);
                    }
                    else
                    {
                        //specified currency is not defined in the period
                        //saving it as usd
                        throw new Exception("Specified dealer currency is not present in the conversions");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                //throw;
            }
            return Math.Round(Response * 100) / 100;

        }


        public decimal ConvertFromBaseCurrencyMultiple(decimal value, Guid currencyId, Guid currencyPeriodId ,List<CurrencyConversionUtilDto> currencyConversions )
        {
            Decimal Response = value;
            try
            {
                if (value == Decimal.Zero)
                    return value;

                CurrencyConversionUtilDto  currencyConversionUtilDto = currencyConversions.Where(a => a.DealerCurrencyId == currencyId && a.currencyPeriodId == currencyPeriodId).FirstOrDefault();
                if (currencyConversionUtilDto == null)
                    return value;

                if (currencyConversionUtilDto.currencyCode != null && currencyConversionUtilDto.currencyCode.ToLower().StartsWith("usd"))
                {
                    Response = value;
                    return Math.Round(Response * 100) / 100;
                }
                if (currencyConversionUtilDto.currencyPeriodId != null)
                {

                    if (currencyConversionUtilDto.ConversionRate != null)
                    {
                        Response = (value * currencyConversionUtilDto.ConversionRate);
                    }
                    else
                    {
                        throw new Exception("Specified dealer currency is not present in the conversions");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                //throw;
            }
            return Math.Round(Response * 100) / 100;

        }


        internal static bool? CurrencyRateAvailabilityCheck(Guid currencyId)
        {
            bool? Response = false;
            try
            {
                DateTime utcToday = DateTime.UtcNow;
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversionPeriods currencyPeriod = session.Query<CurrencyConversionPeriods>()
                    .Where(a => a.FromDate <= utcToday && a.ToDate >= utcToday).FirstOrDefault();
                if (currencyPeriod != null)
                {
                    CurrencyConversions currencyConversion = session.Query<CurrencyConversions>()
                        .Where(a => a.CurrencyId == currencyId && a.CurrencyConversionPeriodId == currencyPeriod.Id).FirstOrDefault();
                    if (currencyConversion != null && currencyConversion.Rate > 0)
                    {
                        Response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = null;
            }
            return Response;
        }

        internal decimal GetConversionRate(Guid BaseCurrencyId, Guid BaseCurrencyPeriodId)
        {
            decimal response = (decimal)1;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversions curversion = session.Query<CurrencyConversions>()
                    .Where(a => a.CurrencyConversionPeriodId == BaseCurrencyPeriodId && a.CurrencyId == BaseCurrencyId).FirstOrDefault();
                if (curversion != null)
                {
                    response = curversion.Rate;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }


        internal decimal GetConversionRate(Guid BaseCurrencyId, Guid BaseCurrencyPeriodId, bool withException)
        {
            decimal response = decimal.One;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversions conversion = session
                    .Query<CurrencyConversions>().FirstOrDefault(a => a.CurrencyConversionPeriodId == BaseCurrencyPeriodId && a.CurrencyId == BaseCurrencyId);
                if (conversion != null)
                {
                    response = conversion.Rate;
                }
                else
                {
                    Currency currency = session.Query<Currency>().FirstOrDefault(a => a.Id == BaseCurrencyId);
                    if (currency != null && currency.Code.ToLower().Contains("usd"))
                    {
                        response = decimal.One;
                        return response;
                    }
                    else
                    {
                        throw new Exception("Currency not exist in the system");
                    }
                }

            }
            catch (Exception ex)
            {

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }

            return response;
        }

        internal decimal GetLocalCurrencyConversionRate(Guid ContractId, Guid relevantCurrencyPeriodId)
        {
            decimal Response = decimal.Zero;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Contract contract = session.Query<Contract>()
                        .Where(a => a.Id == ContractId).FirstOrDefault();

                ReinsurerContract reinsurerContract = session.Query<ReinsurerContract>().Where(a => a.Id == contract.ReinsurerContractId).FirstOrDefault();
                Country country = session.Query<Country>()
                        .Where(a => a.Id == reinsurerContract.CountryId).FirstOrDefault();

                Currency currency = session.Query<Currency>().Where(a => a.Id == country.CurrencyId).FirstOrDefault();
                Guid currencyPeriodId = relevantCurrencyPeriodId;
                CurrencyConversions conversion = session.Query<CurrencyConversions>()
                    .Where(a => a.CurrencyConversionPeriodId == currencyPeriodId && a.CurrencyId == country.CurrencyId).FirstOrDefault();
                Response = conversion.Rate;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal string GetCurrentUSDRateByCountryId(Guid countryId)
        {
            string Response = String.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Guid conversionPrriodId = GetCurrentCurrencyPeriodId();
                Country country = session.Query<Country>()
                    .Where(a => a.Id == countryId).FirstOrDefault();
                if (country == null || !IsGuid(country.CurrencyId.ToString()))
                    return Response;

                CurrencyConversions conversion = session.Query<CurrencyConversions>()
                    .Where(a => a.CurrencyConversionPeriodId == conversionPrriodId && a.CurrencyId == country.CurrencyId).FirstOrDefault();

                Currency currency = session.Query<Currency>().Where(a => a.Id == country.CurrencyId).FirstOrDefault();

                if (conversion == null)
                    return Response;

                Response = conversion.Rate.ToString() + '-' + currency.Code;

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

        internal bool CheckConversionRate(Guid CurrencyId, Guid currentCurrecyPeriodId)
        {
            bool Response = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyConversions curversion = session.Query<CurrencyConversions>()
                    .Where(a => a.CurrencyConversionPeriodId == currentCurrecyPeriodId && a.CurrencyId == CurrencyId).FirstOrDefault();
                if (curversion != null)
                {
                    Response = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }
    }
}
