using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CountryTaxesTaxessRetrievalUnitOfWork:UnitOfWork
    {
        public CountryTaxessResponseDto Result
        {
            get;
            private set;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            return true;
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;

        }

        public override void Execute()
        {

            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                TaxEntityManager TaxEntityManager = new TaxEntityManager();
                List<CountryTaxes> CountryTaxessEntities = TaxEntityManager.GetCountryTaxess();
                CountryTaxessResponseDto result = new CountryTaxessResponseDto();

                result.CountryTaxes = new List<CountryTaxesResponseDto>();
                var currencyEm = new CurrencyEntityManager();
                var countryEm = new CountryEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                //result = new List<SystemUserResponseDto>();
                foreach (CountryTaxes CountryTaxess in CountryTaxessEntities)
                {
                    CountryTaxesResponseDto CountryTaxessRespondDto = new CountryTaxesResponseDto();

                    var countrydetails = countryEm.GetCountryById(CountryTaxess.CountryId);
                    var currencyId = countrydetails.CurrencyId;

                    CountryTaxessRespondDto.IsOnPreviousTax = CountryTaxess.IsOnPreviousTax;
                    CountryTaxessRespondDto.IsPercentage = CountryTaxess.IsPercentage;
                    CountryTaxessRespondDto.MinimumValue = currencyEm.ConvertFromBaseCurrency(CountryTaxess.MinimumValue, currencyId, currentCurrencyPeriodId);
                    //currencyEm.ConvertFromBaseCurrency(result.First().CountryTaxes.MinimumValue, result.First().CountryTaxes.TpaCurrencyId, result.First().CountryTaxes.currencyPeriodId);
                    CountryTaxessRespondDto.TaxTypeId = CountryTaxess.TaxTypeId;
                    if (CountryTaxess.IsPercentage == true)
                    {
                        CountryTaxessRespondDto.TaxValue = CountryTaxess.TaxValue;
                    }
                    else
                    {
                        CountryTaxessRespondDto.TaxValue = currencyEm.ConvertFromBaseCurrency(CountryTaxess.TaxValue, currencyId, currentCurrencyPeriodId);
                    }
                    //CountryTaxessRespondDto.TaxValue = CountryTaxess.TaxValue;
                    CountryTaxessRespondDto.CountryId = CountryTaxess.CountryId;
                    CountryTaxessRespondDto.IsOnGross = CountryTaxess.IsOnGross;
                    CountryTaxessRespondDto.IsOnNRP = CountryTaxess.IsOnNRP;
                    CountryTaxessRespondDto.IndexVal = CountryTaxess.IndexVal;
                    CountryTaxessRespondDto.Id = CountryTaxess.Id;
                    CountryTaxessRespondDto.currencyPeriodId = currentCurrencyPeriodId;
                    CountryTaxessRespondDto.TpaCurrencyId = currencyId;
                    //CountryTaxessRespondDto.ConversionRate = CountryTaxess.ConversionRate;
                    decimal ConRate = CountryTaxess.ConversionRate;
                    CountryTaxessRespondDto.ConversionRate = currencyEm.ConvertFromBaseCurrency(ConRate, currencyId, currentCurrencyPeriodId);
                    result.CountryTaxes.Add(CountryTaxessRespondDto);
                }
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
