using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using TAS.Services.Entities.Persistence;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class TaxEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<TaxResponseDto> GetAllTaxes()
        {
            List<TaxTypes> entities = null;
            ISession session = EntitySessionManager.GetSession();
            return session.Query<TaxTypes>().Select(Tax => new TaxResponseDto
            {
                TaxName = Tax.TaxName,
                TaxCode = Tax.TaxCode,
                Id = Tax.Id
            }).ToList();
        }

        public TaxResponseDto GetTaxById(Guid TaxId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();
                TaxResponseDto pDto = new TaxResponseDto();

                var query =
                    from Tax in session.Query<TaxTypes>()
                    where Tax.Id == TaxId
                    select new { Tax = Tax };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Tax.Id;
                    pDto.TaxCode = result.First().Tax.TaxCode;
                    pDto.TaxName = result.First().Tax.TaxName;
                    pDto.OnNRP = result.First().Tax.OnNRP;
                    pDto.OnGross = result.First().Tax.OnGross;

                    pDto.IsTaxExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsTaxExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddTax(TaxRequestDto Tax)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TaxTypes pr = new Entities.TaxTypes();

                pr.Id = new Guid();
                pr.TaxCode = Tax.TaxCode;
                pr.TaxName = Tax.TaxName;
                pr.OnGross = Tax.OnGross;
                pr.OnNRP = Tax.OnNRP;

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

        internal List<CountryTaxesResponseDto> GetCountryTaxesByCountryId(Guid countryId)
        {

            List<CountryTaxes> CountryTaxessEntities = GetCountryTaxByCountryId(countryId);
            List<CountryTaxesResponseDto> countryTaxes = new List<CountryTaxesResponseDto>();
            var currencyEm = new CurrencyEntityManager();
            var countryEm = new CountryEntityManager();
            Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
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
                countryTaxes.Add(CountryTaxessRespondDto);
            }
            return countryTaxes;
        }

        internal bool UpdateTax(TaxRequestDto Tax)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TaxTypes pr = new Entities.TaxTypes();

                pr.Id = Tax.Id;
                pr.TaxCode = Tax.TaxCode;
                pr.TaxName = Tax.TaxName;
                pr.OnGross = Tax.OnGross;
                pr.OnNRP = Tax.OnNRP;

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

        public List<CountryTaxes> GetCountryTaxess()
        {
            List<CountryTaxes> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CountryTaxes> CountryTaxesData = session.Query<CountryTaxes>();
            entities = CountryTaxesData.ToList();
            return entities;
        }

        public List<CountryTaxes> GetCountryTaxByCountryId(Guid countryId)
        {
            List<CountryTaxes> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CountryTaxes> CountryTaxesData = session.Query<CountryTaxes>().Where(a=>a.CountryId==countryId);
            entities = CountryTaxesData.ToList();
            return entities;
        }

        public CountryTaxesResponseDto GetCountryTaxesById(Guid CountryTaxesId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CountryTaxesResponseDto pDto = new CountryTaxesResponseDto();

                var query =
                    from CountryTaxes in session.Query<CountryTaxes>()
                    where CountryTaxes.Id == CountryTaxesId
                    select new { CountryTaxes = CountryTaxes };

                var currencyEm = new CurrencyEntityManager();
                var countryEm = new CountryEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    var countrydetails = countryEm.GetCountryById(result.First().CountryTaxes.CountryId);
                    var currencyId = countrydetails.CurrencyId;

                    pDto.Id = result.First().CountryTaxes.Id;
                    pDto.IsOnPreviousTax = result.First().CountryTaxes.IsOnPreviousTax;
                    pDto.IsPercentage = result.First().CountryTaxes.IsPercentage;
                    pDto.MinimumValue = result.First().CountryTaxes.MinimumValue;
                    // pDto.MinimumValue = currencyEm.ConvertFromBaseCurrency(result.First().CountryTaxes.MinimumValue,result.First().CountryTaxes.TpaCurrencyId,result.First().CountryTaxes.currencyPeriodId);
                    pDto.TaxTypeId = result.First().CountryTaxes.TaxTypeId;

                    pDto.CountryId = result.First().CountryTaxes.CountryId;
                    pDto.IsOnGross = result.First().CountryTaxes.IsOnGross;
                    pDto.IsOnNRP = result.First().CountryTaxes.IsOnNRP;
                    pDto.IndexVal = result.First().CountryTaxes.IndexVal;
                    //decimal ConRate = result.First().CountryTaxes.ConversionRate;
                    //pDto.ConversionRate = currencyEm.ConvertFromBaseCurrency(ConRate, result.First().CountryTaxes.TpaCurrencyId, currentCurrencyPeriodId);
                    pDto.ConversionRate = result.First().CountryTaxes.ConversionRate;
                    pDto.currencyPeriodId = result.First().CountryTaxes.currencyPeriodId;
                    pDto.TpaCurrencyId = result.First().CountryTaxes.TpaCurrencyId;
                    //if (result.First().CountryTaxes.IsPercentage == true)
                    //{
                    pDto.TaxValue = result.First().CountryTaxes.TaxValue;
                    //}
                    //else
                    //{
                    //    pDto.TaxValue = currencyEm.ConvertFromBaseCurrency(result.First().CountryTaxes.TaxValue, result.First().CountryTaxes.TpaCurrencyId, result.First().CountryTaxes.currencyPeriodId);
                    //}
                    pDto.IsCountryTaxesExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCountryTaxesExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddCountryTaxes(CountryTaxesRequestDto CountryTaxes)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CountryTaxes pr = new Entities.CountryTaxes();
                var currencyEm = new CurrencyEntityManager();
                var countryEm = new CountryEntityManager();

                var  countrydetails =  countryEm.GetCountryById(CountryTaxes.CountryId);
                var currencyId =  countrydetails.CurrencyId;
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                pr.Id = new Guid();
                pr.CountryId = CountryTaxes.CountryId;
                pr.IsOnPreviousTax = CountryTaxes.IsOnPreviousTax;
                pr.IsPercentage = CountryTaxes.IsPercentage;
                if (CountryTaxes.IsPercentage == true)
                {
                    pr.TaxValue = CountryTaxes.TaxValue;
                }
                else
                {
                    pr.TaxValue = currencyEm.ConvertToBaseCurrency(CountryTaxes.TaxValue, currencyId, currentCurrencyPeriodId);
                }
                pr.IsOnGross = CountryTaxes.IsOnGross;
                pr.IsOnNRP = CountryTaxes.IsOnNRP;
                pr.TaxTypeId = CountryTaxes.TaxTypeId;
                pr.MinimumValue = currencyEm.ConvertToBaseCurrency(CountryTaxes.MinimumValue, currencyId, currentCurrencyPeriodId);
                pr.IndexVal = CountryTaxes.IndexVal;
                pr.currencyPeriodId = currentCurrencyPeriodId;
                pr.TpaCurrencyId = currencyId;
                pr.ConversionRate = currencyEm.GetConversionRate(currencyId, currentCurrencyPeriodId);


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

        internal bool UpdateCountryTaxes(CountryTaxesRequestDto CountryTaxes)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CountryTaxes pr = new Entities.CountryTaxes();
                var currencyEm = new CurrencyEntityManager();
                var countryEm = new CountryEntityManager();

                var countrydetails = countryEm.GetCountryById(CountryTaxes.CountryId);
                var currencyId = countrydetails.CurrencyId;
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                pr.Id = CountryTaxes.Id;
               // pr.TaxValue = CountryTaxes.TaxValue;
                pr.CountryId = CountryTaxes.CountryId;
                pr.IsOnPreviousTax = CountryTaxes.IsOnPreviousTax;
                pr.IsPercentage = CountryTaxes.IsPercentage;
                if (CountryTaxes.IsPercentage == true)
                {
                    pr.TaxValue = CountryTaxes.TaxValue;
                }
                else
                {
                    pr.TaxValue = currencyEm.ConvertToBaseCurrency(CountryTaxes.TaxValue, currencyId, currentCurrencyPeriodId);
                }
                pr.IsOnGross = CountryTaxes.IsOnGross;
                pr.IsOnNRP = CountryTaxes.IsOnNRP;
                pr.TaxTypeId = CountryTaxes.TaxTypeId;
                pr.MinimumValue = currencyEm.ConvertToBaseCurrency(CountryTaxes.MinimumValue,currencyId,currentCurrencyPeriodId);
                pr.IndexVal = CountryTaxes.IndexVal;
                pr.currencyPeriodId = currentCurrencyPeriodId;
                pr.TpaCurrencyId = currencyId;
                pr.ConversionRate = currencyEm.GetConversionRate(currencyId, currentCurrencyPeriodId);

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

        internal bool DeleteCountryTaxes(CountryTaxesRequestDto CountryTaxes)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<CountryTaxes> countryTaxes = session.Query<CountryTaxes>().ToList();
                session.Clear();

                foreach (var ct in countryTaxes)
                {
                    List<ContractTaxMapping> contractTaxes = session.Query<ContractTaxMapping>().Where(a => a.CountryTaxId == ct.Id).ToList();

                    if (contractTaxes.Count > 0)
                    {
                        return false;
                    }
                    else
                    {
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            var Deleted = from CountryTaxe in session.Query<CountryTaxes>()
                                          where CountryTaxe.Id == CountryTaxes.Id
                                          select CountryTaxe;
                            var list = Deleted.ToList();
                            foreach (var item in list)
                            {
                                session.Delete(item);
                            }
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

        internal int GetExsistingTaxByTaxName(Guid Id, string TaxName)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from Tax in session.Query<TaxTypes>()
                    where Tax.TaxName == TaxName
                    select new { Tax = Tax };

                return query.Count();
            }
            else
            {
                var query =
                    from Tax in session.Query<TaxTypes>()
                    where Tax.Id != Id && Tax.TaxName == TaxName
                    select new { Tax = Tax };

                return query.Count();
            }

        }

        internal int GetExsistingTaxByTaxCode(Guid Id, string TaxCode)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from Tax in session.Query<TaxTypes>()
                    where Tax.TaxCode == TaxCode
                    select new { Tax = Tax };

                return query.Count();
            }
            else
            {
                var query =
                    from Tax in session.Query<TaxTypes>()
                    where Tax.Id != Id && Tax.TaxCode == TaxCode
                    select new { Tax = Tax };

                return query.Count();
            }

        }

        public List<ContractTaxesResponseDto> GetAllContactTaxes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ContractTaxes>().Select(Tax => new ContractTaxesResponseDto {
                ContractId = Tax.ContractId,
                CountryTaxesId = Tax.CountryTaxId,
                Id = Tax.Id
            }).ToList();
        }
    }
}
