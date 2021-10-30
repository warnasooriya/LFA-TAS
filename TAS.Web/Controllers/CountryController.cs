
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class CountryController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
       // private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Country
        [HttpPost]
        public string AddCountry(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CountryRequestDto Country = data.ToObject<CountryRequestDto>();
                if (Country.Makes == null)
                    Country.Makes = new List<Guid>();
                if (Country.Modeles == null)
                    Country.Modeles = new List<Guid>();
                ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
                CountryRequestDto result = CountryManagementService.AddCountry(Country, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Country Added");
                if (result.CountryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Country failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Country failed!";
            }

        }

        [HttpPost]
        public string UpdateCountry(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CountryRequestDto Country = data.ToObject<CountryRequestDto>();
                ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
                CountryRequestDto result = CountryManagementService.UpdateCountry(Country, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Country Added");
                if (result.CountryInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add Country failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add Country failed!";
            }

        }

        [HttpPost]
        public object GetCountryById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();

            CountryResponseDto Country = CountryManagementService.GetCountryById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return Country;
        }

        [HttpPost]
        public object GetAllCountrys()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();

            CountriesResponseDto CountryData = CountryManagementService.GetAllCountries(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CountryData.Countries.OrderBy(c => c.CountryName).ToArray();
        }

        [HttpPost]
        public object GetAllActiveCountries()
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();

            CountriesResponseDto CountryData = CountryManagementService.GetAllCountries(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CountryData.Countries.OrderBy(c => c.CountryName).Where(a=>a.IsActive==true).ToArray();
        }

        [HttpPost]
        public object GetCountrysByMakeNModel(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            List<CountryResponseDto> CountryData = new List<CountryResponseDto>();
            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            Guid MakeId = Guid.Parse(data["MakeId"].ToString());
            Guid ModelId =Guid.Parse(data["ModelId"].ToString());
            foreach (var item in CountryManagementService.GetAllCountries(SecurityHelper.Context,AuditHelper.Context).Countries.FindAll(c=>c.IsActive==true))
            {
                CountryResponseDto cc = CountryManagementService.GetCountryById(item.Id, SecurityHelper.Context, AuditHelper.Context);
                if(cc.Makes.Contains(MakeId) && cc.Modeles.Contains(ModelId))
                {
                    CountryData.Add(item);
                }
            }
            return CountryData.OrderBy(c => c.CountryName).ToArray();
        }

        [HttpPost]
        public object GetAllCountrysByMakeNModelIds(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            List<Guid> ModelIdList = data["data"].ToObject<List<Guid>>();
            List<CountryResponseDto> CountryData = new List<CountryResponseDto>();
            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            Guid MakeId = Guid.Parse(data["MakeId"].ToString());


            foreach (var item in CountryManagementService.GetAllCountries(SecurityHelper.Context, AuditHelper.Context).Countries.FindAll(c => c.IsActive == true))
            {
                CountryResponseDto cc = CountryManagementService.GetCountryById(item.Id, SecurityHelper.Context, AuditHelper.Context);
                foreach(var ModelList in ModelIdList){
                    if (cc.Makes.Contains(MakeId) && cc.Modeles.Contains(ModelList))
                    {
                        CountryData.Add(item);
                    }
                }
            }
            return CountryData.OrderBy(c => c.CountryName).ToArray().Distinct();
        }



        [HttpPost]
        public object GetAllCountrysByMakeNModelIdsNew(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            List<Guid> ModelIdList = data["data"].ToObject<List<Guid>>();
            ICountryManagementService CountryManagementService = ServiceFactory.GetCountryManagementService();
            Guid MakeId = Guid.Parse(data["MakeId"].ToString());
           var response= CountryManagementService.GetAllCountrysByMakeNModelIdsNew(ModelIdList, MakeId, SecurityHelper.Context, AuditHelper.Context);
            return response;
        }

        #endregion

        #region TaxTypes
        [HttpPost]
        public object GetAllTaxTypes(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService TaxManagementService = ServiceFactory.GetTaxManagementService();

            TaxesResponseDto TaxData = TaxManagementService.GetAllTaxes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return TaxData.Taxes.ToArray();
        }

        [HttpPost]
        public string AddTax(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                TaxRequestDto Tax = data.ToObject<TaxRequestDto>(); //JsonConvert.DeserializeObject<List<CylinderCountRequestDto>>(data);
                ITaxManagementService taxManagementService = ServiceFactory.GetTaxManagementService();
                TaxRequestDto result = taxManagementService.AddTax(Tax, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CylinderCount Added");
                if (result.TaxInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add tax failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add tax failed!";
            }

        }

        [HttpPost]
        public string UpdateTax(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                TaxRequestDto Tax = data.ToObject<TaxRequestDto>();
                ITaxManagementService taxManagementService = ServiceFactory.GetTaxManagementService();
                TaxRequestDto result = taxManagementService.UpdateTax(Tax, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("Tax Added");
                if (result.TaxInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add tax failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add tax failed!";
            }



        }

        [HttpPost]
        public object GetTaxById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService taxManagementService = ServiceFactory.GetTaxManagementService();
            TaxResponseDto Tax = taxManagementService.GetTaxById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);

            return Tax;
        }
        [HttpPost]
        public object GetAllTaxTypesFromCountryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService TaxManagementService = ServiceFactory.GetTaxManagementService();
            List<CountryTaxesResponseDto> countryTaxes = TaxManagementService.GetCountryTaxesByCountryId(Guid.Parse(data["Id"].ToString()),
           SecurityHelper.Context,
           AuditHelper.Context).CountryTaxes.ToList();
           // List<CountryTaxesResponseDto> countryTaxes = ((List<CountryTaxesResponseDto>)GetAllCountryTaxessFromCountryId(data));
            List<CountryTaxDetails> result = new List<CountryTaxDetails>();
            foreach (var item in countryTaxes)
            {
                TaxResponseDto TaxData = TaxManagementService.GetTaxById(item.TaxTypeId,
                   SecurityHelper.Context,
                   AuditHelper.Context);
                CountryTaxDetails value = new CountryTaxDetails()
                {
                    CountryId = item.CountryId,
                    Id = item.Id,
                    IsOnPreviousTax = item.IsOnPreviousTax,
                    IsPercentage = item.IsPercentage,
                    MinimumValue = item.MinimumValue,
                    Tax = TaxData,
                    TaxTypeId = item.TaxTypeId,
                    TaxValue = item.TaxValue,
                    IndexVal = item.IndexVal
                };

                result.Add(value);
            }
            return result.OrderBy(t=> t.IndexVal).ToArray();
        }

        [HttpPost]
        public bool IsExsistingTaxByName(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService taxManagementService = ServiceFactory.GetTaxManagementService();
            bool isExsists = taxManagementService.IsExistingTaxName(Guid.Parse(data["Id"].ToString()), data["TaxName"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }

        [HttpPost]
        public bool IsExsistingTaxByCode(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService taxManagementService = ServiceFactory.GetTaxManagementService();
            bool isExsists = taxManagementService.IsExistingTaxCode(Guid.Parse(data["Id"].ToString()), data["TaxCode"].ToString(),
                SecurityHelper.Context,
                AuditHelper.Context);
            return isExsists;
        }
        [HttpPost]
        public object GetAllContactTaxes(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService TaxManagementService = ServiceFactory.GetTaxManagementService();

            ContractTaxesesResponseDto ContactTaxData = TaxManagementService.GetAllContactTaxes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return ContactTaxData.ContractTaxess.ToArray();
        }

        #endregion

        #region Country Taxes
        [HttpPost]
        public string AddCountryTaxes(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CountryTaxesRequestDto CountryTaxes = data.ToObject<CountryTaxesRequestDto>();
                ITaxManagementService CountryTaxesManagementService = ServiceFactory.GetTaxManagementService();
                CountryTaxesRequestDto result = CountryTaxesManagementService.AddCountryTaxes(CountryTaxes, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CountryTaxes Added");
                if (result.CountryTaxesInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CountryTaxes failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CountryTaxes failed!";
            }

        }

        [HttpPost]
        public string UpdateCountryTaxes(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CountryTaxesRequestDto CountryTaxes = data.ToObject<CountryTaxesRequestDto>();
                ITaxManagementService CountryTaxesManagementService = ServiceFactory.GetTaxManagementService();
                CountryTaxesRequestDto result = CountryTaxesManagementService.UpdateCountryTaxes(CountryTaxes, false, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CountryTaxes Added");
                if (result.CountryTaxesInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CountryTaxes failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CountryTaxes failed!";
            }

        }

        [HttpPost]
        public string DeleteCountryTaxes(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

                CountryTaxesRequestDto CountryTaxes = data.ToObject<CountryTaxesRequestDto>();
                ITaxManagementService CountryTaxesManagementService = ServiceFactory.GetTaxManagementService();
                CountryTaxesRequestDto result = CountryTaxesManagementService.UpdateCountryTaxes(CountryTaxes, true, SecurityHelper.Context, AuditHelper.Context);
                logger.Info("CountryTaxes Added");
                if (result.CountryTaxesInsertion)
                {
                    return "OK";
                }
                else
                {
                    return "Add CountryTaxes failed!";
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Add CountryTaxes failed!";
            }

        }

        [HttpPost]
        public object GetCountryTaxesById(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService CountryTaxesManagementService = ServiceFactory.GetTaxManagementService();

            CountryTaxesResponseDto CountryTaxes = CountryTaxesManagementService.GetCountryTaxesById(Guid.Parse(data["Id"].ToString()),
                SecurityHelper.Context,
                AuditHelper.Context);
            return CountryTaxes;
        }

        [HttpPost]
        public object GetAllCountryTaxessFromCountryId(JObject data)
        {
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            ITaxManagementService CountryTaxesManagementService = ServiceFactory.GetTaxManagementService();

            CountryTaxessResponseDto CountryTaxesData = CountryTaxesManagementService.GetAllCountryTaxes(
            SecurityHelper.Context,
            AuditHelper.Context);
            return CountryTaxesData.CountryTaxes.FindAll(t=>t.CountryId == Guid.Parse(data["Id"].ToString())).OrderBy(t=> t.IndexVal).ToArray();
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
        #endregion
    }
    public class CountryTaxDetails:CountryTaxesResponseDto
    {
        public TaxResponseDto Tax { get; set; }
    }
}