using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class CountryEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal List<CountryInfo> GetAllCountries()
        {
            List<CountryInfo> CountryInfos = new List<CountryInfo>();
            ISession session = EntitySessionManager.GetSession();

            var CountryData = session.Query<Country>().ToList();
            if (CountryData != null)
            {
                CountryInfos = CountryData.Select(s => new CountryInfo
                {
                    CountryName = s.CountryName,
                    CountryCode = s.CountryCode,
                    CurrencyId = s.CurrencyId,
                    IsActive = s.IsActive,
                    PhoneCode = s.PhoneCode,
                    Id = s.Id
                }).OrderBy(z => z.CountryName).ToList();
            }
            //foreach (var item in CountryData)
            //{
            //    CountryInfo c= new CountryInfo() {
            //    CountryName = item.CountryName,
            //    CountryCode = item.CountryCode,
            //    CurrencyId = item.CurrencyId,
            //    IsActive = item.IsActive,
            //    PhoneCode = item.PhoneCode,
            //    Id = item.Id
            //    };
            //    CountryInfos.Add(c);
            //};
            return CountryInfos;
        }


        internal List<CountryResponseDto> GetAllActiveCountries()
        {
            List<CountryResponseDto> CountryInfos = new List<CountryResponseDto>();
            ISession session = EntitySessionManager.GetSession();

            List<Country> countryList = session.Query<Country>().Where(a => a.IsActive == true).ToList();
            if (countryList != null)
            {
                CountryInfos = countryList.Select(s => new CountryInfo
                  {
                      CountryName = s.CountryName,
                      CountryCode = s.CountryCode,
                      CurrencyId = s.CurrencyId,
                      IsActive = s.IsActive,
                      PhoneCode = s.PhoneCode,
                      Id = s.Id
                  }).OrderBy(z => z.CountryName).
                  Select(s => new CountryResponseDto
                {
                    CountryName = s.CountryName,
                    IsActive = s.IsActive,
                    CountryCode = s.CountryCode,
                    CurrencyId = s.CurrencyId,
                    PhoneCode = s.PhoneCode,
                    Id = s.Id,
                    Makes = s.Makes,
                    Modeles = s.Models
                }).ToList();
            }
            return CountryInfos;
        }



        public CountryResponseDto GetCountryById(Guid CountryId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CountryResponseDto pDto = new CountryResponseDto();
                List<CountryMakes> makes = null;
                List<CountryModeles> modeles = null;

                var query =
                    from Country in session.Query<Country>()
                    where Country.Id == CountryId
                    select new { Country = Country };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    IQueryable<CountryMakes> mak = session.Query<CountryMakes>();
                    makes = mak.ToList().Count > 0 ? mak.ToList().FindAll(m => m.CountryId == CountryId) : null;

                    IQueryable<CountryModeles> mod = session.Query<CountryModeles>();
                    modeles = mod.ToList().Count > 0 ? mod.ToList().FindAll(m => m.CountryId == CountryId) : null;

                    pDto.Id = result.First().Country.Id;
                    pDto.CountryCode = result.First().Country.CountryCode;
                    pDto.CountryName = result.First().Country.CountryName;
                    pDto.IsActive = result.First().Country.IsActive;
                    pDto.CurrencyId = result.First().Country.CurrencyId;
                    pDto.PhoneCode = result.First().Country.PhoneCode;
                    pDto.Makes = makes == null ? null : makes.Select(m => m.MakeId).ToList();
                    pDto.Modeles = modeles == null ? null : modeles.Select(m => m.ModelId).ToList();

                    pDto.IsCountryExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsCountryExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal object GetCountryByMakeNModelIdsNew(Guid makeId, List<Guid> modelIds)
        {
            ISession session = EntitySessionManager.GetSession();
            var res =  (from c in session.Query<Country>()
                       join cm in session.Query<CountryMakes>() on c.Id equals cm.CountryId
                       join cmo in session.Query<CountryModeles>() on c.Id equals cmo.CountryId
                       where c.IsActive == true && cm.MakeId == makeId && modelIds.Contains(cmo.ModelId)
                       select new {
                           Id=c.Id ,
                           CountryCode =c.CountryCode,
                           CountryName = c.CountryName,
                           PhoneCode = c.PhoneCode,
                           CurrencyId =c.CurrencyId,
                           IsActive = c.IsActive,
                           IsCountryExists =true
                       }
                       ).Distinct().ToList();
            return res;
        }

        internal CountriesResponseDto GetAllDealerAvailableCountries()
        {
            CountriesResponseDto response = new CountriesResponseDto();
            response.Countries = new List<CountryResponseDto>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<Guid> dealerCountryList = session.Query<Dealer>()
                    .Select(a => a.CountryId).Distinct().ToList();
                IList<Country> availableCountries = session.QueryOver<Country>()
                    .WhereRestrictionOn(x => x.Id)
                    .IsIn(dealerCountryList)
                    .List<Country>();

                foreach (Country country in availableCountries)
                {
                    var countryData = new CountryResponseDto()
                    {
                        Id = country.Id,
                        CountryCode = country.CountryCode,
                        CountryName = country.CountryName,
                        CurrencyId = country.CurrencyId,
                        IsActive = country.IsActive,
                        IsCountryExists = true,
                        PhoneCode = country.PhoneCode
                    };
                    response.Countries.Add(countryData);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal bool AddCountry(CountryRequestDto Country)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Country pr = new Entities.Country();

                pr.Id = new Guid();
                pr.CountryCode = Country.CountryCode;
                pr.CountryName = Country.CountryName;
                pr.IsActive = Country.IsActive;
                pr.CurrencyId = Country.CurrencyId;
                pr.PhoneCode = Country.PhoneCode;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in Country.Makes)
                    {
                        CountryMakes cc = new Entities.CountryMakes();
                        cc.Id = new Guid();
                        cc.MakeId = item;
                        cc.CountryId = pr.Id;
                        session.SaveOrUpdate(cc);
                    }
                    foreach (var item in Country.Modeles)
                    {
                        CountryModeles cc = new Entities.CountryModeles();
                        cc.Id = new Guid();
                        cc.ModelId = item;
                        cc.CountryId = pr.Id;
                        session.SaveOrUpdate(cc);
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

        internal bool UpdateCountry(CountryRequestDto Country)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Country pr = new Entities.Country();

                pr.Id = Country.Id;
                pr.CountryCode = Country.CountryCode;
                pr.CountryName = Country.CountryName;
                pr.IsActive = Country.IsActive;
                pr.CurrencyId = Country.CurrencyId;
                pr.PhoneCode = Country.PhoneCode;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    //Add New Makes
                    if (Country.Makes != null)
                    {
                        foreach (var item in Country.Makes)
                        {
                            var query = from CountryMake in session.Query<CountryMakes>()
                                        where CountryMake.MakeId == item && CountryMake.CountryId == Country.Id
                                        select new { Id = CountryMake.Id };
                            if (query.ToList().Count == 0)
                            {
                                CountryMakes cc = new Entities.CountryMakes();
                                cc.Id = new Guid();
                                cc.MakeId = item;
                                cc.CountryId = Country.Id;
                                session.SaveOrUpdate(cc);
                            }
                        }
                    }
                    //Add New Modeles
                    if (Country.Modeles != null)
                    {
                        foreach (var item in Country.Modeles)
                        {
                            var query = from CountryModele in session.Query<CountryModeles>()
                                        where CountryModele.ModelId == item && CountryModele.CountryId == Country.Id
                                        select new { Id = CountryModele.Id };
                            if (query.ToList().Count == 0)
                            {
                                CountryModeles cc = new Entities.CountryModeles();
                                cc.Id = new Guid();
                                cc.ModelId = item;
                                cc.CountryId = Country.Id;
                                session.SaveOrUpdate(cc);
                            }
                        }
                    }
                    //Delete Removed Makes
                    var queryDeleted = from CountryMake in session.Query<CountryMakes>()
                                       where CountryMake.CountryId == Country.Id
                                       select CountryMake;
                    foreach (var item in queryDeleted.ToList())
                    {
                        if (Country.Makes.Contains(item.MakeId) == false)
                        {
                            session.Delete(item);
                        }
                    }
                    //Delete Removed Modeles
                    var queryDeletedM = from CountryModel in session.Query<CountryModeles>()
                                        where CountryModel.CountryId == Country.Id
                                        select CountryModel;
                    foreach (var item in queryDeletedM.ToList())
                    {
                        if (Country.Modeles.Contains(item.ModelId) == false)
                        {
                            session.Delete(item);
                        }
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


    }
}
