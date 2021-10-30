
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Description;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class CurrencyManagementController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public string AddCurrencyConversion(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyConversionRequestDto CurrencyConversion = data.ToObject<CurrencyConversionRequestDto>();
                ICurrencyManagementService CurrencyConversionManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyConversionRequestDto result = CurrencyConversionManagementService.AddCurrencyConversion(CurrencyConversion, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyConversion Added");
                if (result.CurrencyConversionInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyConversion failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyConversion failed!";
            }
            
        }

        [HttpPost]
        public string UpdateCurrencyConversion(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyConversionRequestDto CurrencyConversion = data.ToObject<CurrencyConversionRequestDto>();
                ICurrencyManagementService CurrencyConversionManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyConversionRequestDto result = CurrencyConversionManagementService.UpdateCurrencyConversion(CurrencyConversion, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyConversion Added");
                if (result.CurrencyConversionInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyConversion failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyConversion failed!";
            }
            
        }
        [HttpPost]
        public string ValidateCurrencyPeriodOverlaps(CurrencyConversionPeriodRequestDto CurrencyConversionPeriodRequest)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ICurrencyManagementService CurrencyConversionManagementService = ServiceFactory.GetCurrencyManagementService();
            return CurrencyConversionManagementService.ValidateCurrencyPeriodOverlaps(CurrencyConversionPeriodRequest,
                SecurityHelper.Context,
                AuditHelper.Context);
        }
        [HttpPost]
        public object GetCurrencyConversionById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyConversionManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyConversionResponseDto CurrencyConversion = CurrencyConversionManagementService.GetCurrencyConversionById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CurrencyConversion;
        }

        [HttpPost]
        public object GetAllCurrencyConversions()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyConversionManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyConversionsResponseDto CurrencyConversionData = CurrencyConversionManagementService.GetCurrencyConversions(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyConversionData.CurrencyConversions.ToArray();
        }

        [HttpPost]
        public string AddCurrencyConversionPeriod(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyConversionPeriodRequestDto CurrencyConversionPeriod = data.ToObject<CurrencyConversionPeriodRequestDto>();
                ICurrencyManagementService CurrencyConversionPeriodManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyConversionPeriodRequestDto result = CurrencyConversionPeriodManagementService.AddCurrencyConversionPeriod(CurrencyConversionPeriod, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyConversionPeriod Added");
                if (result.CurrencyConversionPeriodInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyConversionPeriod failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyConversionPeriod failed!";
            }
            
        }

        [HttpPost]
        public string UpdateCurrencyConversionPeriod(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyConversionPeriodRequestDto CurrencyConversionPeriod = data.ToObject<CurrencyConversionPeriodRequestDto>();
                ICurrencyManagementService CurrencyConversionPeriodManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyConversionPeriodRequestDto result = CurrencyConversionPeriodManagementService.UpdateCurrencyConversionPeriod(CurrencyConversionPeriod, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyConversionPeriod Added");
                if (result.CurrencyConversionPeriodInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyConversionPeriod failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyConversionPeriod failed!";
            }
            
        }

        [HttpPost]
        public object GetCurrencyConversionPeriodById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyConversionPeriodManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyConversionPeriodResponseDto CurrencyConversionPeriod = CurrencyConversionPeriodManagementService.GetCurrencyConversionPeriodById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CurrencyConversionPeriod;
        }

        [HttpPost]
        public object GetAllCurrencyConversionPeriods()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyConversionPeriodManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyConversionPeriodsResponseDto CurrencyConversionPeriodData = CurrencyConversionPeriodManagementService.GetCurrencyConversionPeriods(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyConversionPeriodData.CurrencyConversionPeriods.ToArray();
        }

        [HttpPost]
        public object GetCurrencies()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyData.Currencies.ToArray();
        }

        [HttpPost]
        public object GetCurrencyById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrenciesResponseDto CurrencyData = CurrencyManagementService.GetAllCurrencies(
            SecurityHelper.Context,
            AuditHelper.Context);
            if (data["Id"] == null)
                return "N/A";
            return CurrencyData.Currencies.Find(c=>c.Id == Guid.Parse(data["Id"].ToString()));
        }

        [HttpPost]
        public string AddCurrencyEmail(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyEmailRequestDto CurrencyEmail = data.ToObject<CurrencyEmailRequestDto>();
                ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyEmailRequestDto result = CurrencyEmailManagementService.AddCurrencyEmail(CurrencyEmail, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyEmail Added");
                if (result.CurrencyEmailInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyEmail failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyEmail failed!";
            }
            
        }

        [HttpPost]
        public string UpdateCurrencyEmail(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CurrencyEmailRequestDto CurrencyEmail = data.ToObject<CurrencyEmailRequestDto>();
                ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();
                CurrencyEmailRequestDto result = CurrencyEmailManagementService.UpdateCurrencyEmail(CurrencyEmail, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CurrencyEmail Added");
                if (result.CurrencyEmailInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CurrencyEmail failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CurrencyEmail failed!";
            }
            
        }

        [HttpPost]
        public object GetCurrencyEmailById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyEmailResponseDto CurrencyEmail = CurrencyEmailManagementService.GetCurrencyEmailById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CurrencyEmail;
        }

        [HttpPost]
        public object GetAllCurrencyEmails()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();

            CurrencyEmailsResponseDto CurrencyEmailData = CurrencyEmailManagementService.GetCurrencyEmails(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CurrencyEmailData.CurrencyEmails.ToArray();
        }

        [HttpPost]
        public bool? CurrencyPeriodCheck()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();

            return CurrencyEmailManagementService.CurrencyPeriodCheck(
            SecurityHelper.Context,
            AuditHelper.Context);
        }

        [HttpPost]
        public bool? GetCurrencyRateAvailabilityByCurrencyId(JObject data)
        {
            Guid currencyId = Guid.Parse(data["Id"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            ICurrencyManagementService CurrencyEmailManagementService = ServiceFactory.GetCurrencyManagementService();

            return CurrencyEmailManagementService.GetCurrencyRateAvailabilityByCurrencyId(currencyId,
            SecurityHelper.Context,
            AuditHelper.Context);
        }

    }
}