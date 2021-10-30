using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using System.Security.Cryptography;
using NLog;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Web.Script.Serialization;

namespace TAS.Services.Entities.Management
{
    public class ManufacturerWarrantyEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ManufacturerWarranty> GetManufacturerWarranties()
        {
            List<ManufacturerWarranty> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ManufacturerWarranty> ManufacturerWarrantyData = session.Query<ManufacturerWarranty>();
            entities = ManufacturerWarrantyData.ToList();
            return entities;
        }

        public ManufacturerWarrantyResponseDto GetManufacturerWarrantyById(Guid ManufacturerWarrantyId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ManufacturerWarrantyResponseDto pDto = new ManufacturerWarrantyResponseDto();

                List<ManufacturerWarrantyDetails> MrWDetailsCInfor = null;
                List<ManufacturerWarrantyDetails> MrWDetailsCInfoccr = null;

                IQueryable<ManufacturerWarrantyDetails> MrWDetailsCInforData = session.Query<ManufacturerWarrantyDetails>()
                    .Where(a => a.ManufacturerWarrantyId == ManufacturerWarrantyId);

                MrWDetailsCInfor = MrWDetailsCInforData.Where(x => x.ManufacturerWarrantyId == ManufacturerWarrantyId).ToList();

                MrWDetailsCInfor.GroupBy(a => a.ModelId);

                var query =
                   from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
                   where ManufacturerWarranty.Id == ManufacturerWarrantyId
                   select new { ManufacturerWarranty = ManufacturerWarranty };

                var result = query.ToList();

                Guid makeid = result.First().ManufacturerWarranty.MakeId;

                Make make = session.Query<Make>().Where(a => a.Id == makeid).FirstOrDefault();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ManufacturerWarranty.Id;
                    pDto.MakeId = result.First().ManufacturerWarranty.MakeId;
                    pDto.CommodityTypeId = make.CommodityTypeId;
                    pDto.ApplicableFrom = result.First().ManufacturerWarranty.ApplicableFrom;
                    pDto.WarrantyName = result.First().ManufacturerWarranty.WarrantyName;
                    pDto.WarrantyMonths = result.First().ManufacturerWarranty.WarrantyMonths;
                    pDto.EntryDateTime = result.First().ManufacturerWarranty.EntryDateTime;
                    pDto.EntryUser = result.First().ManufacturerWarranty.EntryUser;
                    pDto.Countrys = MrWDetailsCInfor.Select(c => c.CountryId).Distinct().ToList();
                    pDto.Models = MrWDetailsCInfor.Select(c => c.ModelId).ToList();
                    pDto.IsManufacturerWarrantyExists = true;
                    pDto.WarrantyKm = result.First().ManufacturerWarranty.IsUnlimited ? "Unlimited" : result.First().ManufacturerWarranty.WarrantyKm.ToString();
                    pDto.IsUnlimited = result.First().ManufacturerWarranty.IsUnlimited;
                    return pDto;
                }
                else
                {
                    pDto.IsManufacturerWarrantyExists = false;
                    return null;
                }




            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ManufacturerWarranty pr = new Entities.ManufacturerWarranty();

                pr.Id = new Guid();
                pr.MakeId = ManufacturerWarranty.MakeId;
                //  pr.ModelId = ManufacturerWarranty.ModelId;
                pr.ApplicableFrom = ManufacturerWarranty.ApplicableFrom;
                //  pr.CountryId = ManufacturerWarranty.CountryId;
                pr.WarrantyKm = ManufacturerWarranty.WarrantyKm;
                pr.WarrantyName = ManufacturerWarranty.WarrantyName;
                pr.WarrantyMonths = ManufacturerWarranty.WarrantyMonths;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.IsUnlimited = ManufacturerWarranty.IsUnlimited;
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);

                    foreach (var item in ManufacturerWarranty.Models)
                    {

                        foreach (var item2 in ManufacturerWarranty.Countrys)
                        {
                            ManufacturerWarrantyDetails mwd = new Entities.ManufacturerWarrantyDetails();
                            mwd.Id = new Guid();
                            mwd.CountryId = item2;
                            mwd.ManufacturerWarrantyId = pr.Id;
                            mwd.ModelId = item;
                            session.SaveOrUpdate(mwd);
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

        internal bool UpdateManufacturerWarranty(ManufacturerWarrantyRequestDto ManufacturerWarranty)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ManufacturerWarranty pr = new Entities.ManufacturerWarranty();

                List<ManufacturerWarrantyDetails> manufacturerWarrantyDetails = session.Query<ManufacturerWarrantyDetails>()
                   .Where(a => a.ManufacturerWarrantyId == ManufacturerWarranty.Id).ToList();

                foreach (var MWDetails in manufacturerWarrantyDetails)
                {
                    session.Delete(MWDetails);
                }



                pr.Id = ManufacturerWarranty.Id;
                pr.MakeId = ManufacturerWarranty.MakeId;
                //   pr.ModelId = ManufacturerWarranty.ModelId;
                pr.ApplicableFrom = ManufacturerWarranty.ApplicableFrom;
                //   pr.CountryId = ManufacturerWarranty.CountryId;
                pr.WarrantyKm = ManufacturerWarranty.WarrantyKm;
                pr.WarrantyName = ManufacturerWarranty.WarrantyName;
                pr.WarrantyMonths = ManufacturerWarranty.WarrantyMonths;
                pr.EntryDateTime = ManufacturerWarranty.EntryDateTime;
                pr.EntryUser = ManufacturerWarranty.EntryUser;
                pr.IsUnlimited = ManufacturerWarranty.IsUnlimited;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    foreach (var item in ManufacturerWarranty.Models)
                    {

                        foreach (var item2 in ManufacturerWarranty.Countrys)
                        {
                            ManufacturerWarrantyDetails mwd = new Entities.ManufacturerWarrantyDetails();
                            mwd.Id = new Guid();
                            mwd.CountryId = item2;
                            mwd.ManufacturerWarrantyId = pr.Id;
                            mwd.ModelId = item;
                            session.SaveOrUpdate(mwd);
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


        internal static object SearchManufacturerWarrantySchemes(ManufacturerWarrentySearchRequestDto manufacturerWarrentySearchRequestDto)
        {
            object response = null;
            try
            {

                if (manufacturerWarrentySearchRequestDto != null && manufacturerWarrentySearchRequestDto.paginationOptionsManufacturerWarrentySearchGrid != null)
                {
                    ISession session = EntitySessionManager.GetSession();
                    CommonEntityManager commonEm = new CommonEntityManager();



                    Expression<Func<ManufacturerWarranty, bool>> filterManufacturerWarranty = PredicateBuilder.True<ManufacturerWarranty>();

                    if (IsGuid(manufacturerWarrentySearchRequestDto.manufacturerWarrentySearchGridSearchCriterias.MakeId.ToString()))
                    {
                        filterManufacturerWarranty = filterManufacturerWarranty.And(a => a.MakeId ==
                            manufacturerWarrentySearchRequestDto.manufacturerWarrentySearchGridSearchCriterias.MakeId);
                    }



                    if (manufacturerWarrentySearchRequestDto.CommodityTypeId != null && manufacturerWarrentySearchRequestDto.CommodityTypeId != Guid.Empty)
                    {
                        Guid ComId = manufacturerWarrentySearchRequestDto.CommodityTypeId;
                        IEnumerable<Make> Makes = session.Query<Make>().Where(a => a.CommodityTypeId == ComId);
                        filterManufacturerWarranty = filterManufacturerWarranty.And(a => Makes.Any(b => b.Id == a.MakeId));
                    }

                    var filteredManufacturerWarrantyData = session.Query<ManufacturerWarranty>().Where(filterManufacturerWarranty);


                    long totalRecords = filteredManufacturerWarrantyData.Count();

                    var ManufacturerWarrantyGridDetailsFilterd = filteredManufacturerWarrantyData.Skip((manufacturerWarrentySearchRequestDto.paginationOptionsManufacturerWarrentySearchGrid.pageNumber - 1) * manufacturerWarrentySearchRequestDto.paginationOptionsManufacturerWarrentySearchGrid.pageSize)
                    .Take(manufacturerWarrentySearchRequestDto.paginationOptionsManufacturerWarrentySearchGrid.pageSize)
                    .Select(a => new
                    {

                        a.Id,
                        a.WarrantyMonths,
                        Makes = commonEm.GetMakeNameById(a.MakeId),
                        a.WarrantyKm,
                        a.WarrantyName,
                        ApplicableFrom = a.ApplicableFrom.ToString("dd/MM/yyy"),
                        a.IsUnlimited,
                    })
                    .ToArray();

                    response = new CommonGridResponseDto()
                    {
                        totalRecords = totalRecords,
                        data = ManufacturerWarrantyGridDetailsFilterd
                    };

                    return new JavaScriptSerializer().Serialize(response);
                }
                return response;
            }
            catch (Exception ex)
            {

                return response = null;

            }
        }


        internal static ManufacturerWarrantyDetailsResponseDto GetManufacturerWarrantyByModelandCountry(Guid ModelId, Guid CountryId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ManufacturerWarrantyDetailsResponseDto pDto = new ManufacturerWarrantyDetailsResponseDto();

                var query =
                    from ManufacturerWarrantyDetails in session.Query<ManufacturerWarrantyDetails>()
                    where ManufacturerWarrantyDetails.CountryId == CountryId && ManufacturerWarrantyDetails.ModelId == ModelId
                    select new { ManufacturerWarrantyDetails = ManufacturerWarrantyDetails };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ManufacturerWarrantyDetails.Id;
                    pDto.CountryId = result.First().ManufacturerWarrantyDetails.CountryId;
                    pDto.ManufacturerWarrantyId = result.First().ManufacturerWarrantyDetails.ManufacturerWarrantyId;
                    pDto.ModelId = result.First().ManufacturerWarrantyDetails.ModelId;

                    pDto.ManufacturerWarrantyDetailsExists = true;
                    return pDto;
                }
                else
                {
                    pDto.ManufacturerWarrantyDetailsExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal static object GetManufacturerWarrantyByModelMakeandCountry(Guid ModelId, Guid CountryId, Guid MakeId)
        {
            try
            {
                {
                    ISession session = EntitySessionManager.GetSession();

                    var ManufacturerWarrantyDetails = session.Query<ManufacturerWarrantyDetails>()
                        .Join(session.Query<ManufacturerWarranty>(), a => a.ManufacturerWarrantyId, b => b.Id, (a, b) => new { a, b })
                        .Where(a => a.b.MakeId == MakeId && a.a.ModelId == ModelId && a.a.CountryId == CountryId)
                         .Select(x => new
                         {

                             x.b.WarrantyMonths,
                             x.a.ManufacturerWarrantyId
                         }).ToArray();

                    return ManufacturerWarrantyDetails;


                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal static ManufacturerWarrantyResponseDto GetManufacturerWarrantyByContarctId(Guid mwdId, Guid MakeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ManufacturerWarrantyResponseDto pDto = new ManufacturerWarrantyResponseDto();

                var query =
               from ManufacturerWarranty in session.Query<ManufacturerWarranty>()
               where ManufacturerWarranty.Id == mwdId && ManufacturerWarranty.MakeId == MakeId
               select new { ManufacturerWarranty = ManufacturerWarranty };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ManufacturerWarranty.Id;
                    pDto.WarrantyKm = result.First().ManufacturerWarranty.IsUnlimited ? "Unlimited" : result.First().ManufacturerWarranty.WarrantyKm.ToString();
                    pDto.WarrantyMonths = result.First().ManufacturerWarranty.WarrantyMonths;
                    pDto.WarrantyName = result.First().ManufacturerWarranty.WarrantyName;
                    pDto.IsUnlimited = result.First().ManufacturerWarranty.IsUnlimited;
                    pDto.IsManufacturerWarrantyExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsManufacturerWarrantyExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
    }
}
