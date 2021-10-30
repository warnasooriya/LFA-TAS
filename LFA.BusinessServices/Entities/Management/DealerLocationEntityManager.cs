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
using NHibernate.Mapping;

namespace TAS.Services.Entities.Management
{
    public class DealerLocationEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal List<DealerLocationRespondDto> GetAllDealerLocations()
        {

            ISession session = EntitySessionManager.GetSession();
             return  session.Query<DealerLocation>().Select(s => new DealerLocationRespondDto
            {
                Id = s.Id,
                CityId = s.CityId,
                DealerId = s.DealerId,
                HeadOfficeBranch = s.HeadOfficeBranch,
                Location = s.Location,
                SalesContactPerson = s.SalesContactPerson,
                DealerAddress = s.DealerAddress,
                SalesEmail = s.SalesEmail,
                SalesFax = s.SalesFax,
                SalesTelephone = s.SalesTelephone,
                ServiceContactPerson = s.ServiceContactPerson,
                ServiceEmail = s.ServiceEmail,
                ServiceFax = s.ServiceFax,
                ServiceTelephone = s.ServiceTelephone,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();

        }


        internal object GetAllDealerLocationsByUser(Guid userId)
        {

            ISession session = EntitySessionManager.GetSession();
            var DealerLocationData = (from dbs in session.Query<DealerBranchStaff>()
                                                             join dl in session.Query<DealerLocation>() on dbs.BranchId equals dl.Id
                                                             join ds in session.Query<DealerStaff>() on dbs.DealerStaffId equals ds.Id
                                                             where ds.UserId == userId
                                      select new
                                                             {
                                                                 Id = dl.Id,
                                                                 Location = dl.Location

                                                             }).ToList();
            return DealerLocationData;
        }

        public DealerLocationRespondDto GetDealerLocationById(Guid DealerLocationId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                DealerLocationRespondDto pDto = new DealerLocationRespondDto();

                var query =
                    from DealerLocation in session.Query<DealerLocation>()
                    where DealerLocation.Id == DealerLocationId
                    select new { DealerLocation = DealerLocation };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().DealerLocation.Id;
                    pDto.CityId = result.First().DealerLocation.CityId;
                    pDto.DealerId = result.First().DealerLocation.DealerId;
                    pDto.DealerAddress = result.First().DealerLocation.DealerAddress;
                    pDto.TpaBranchId = result.First().DealerLocation.TpaBranchId;
                    pDto.HeadOfficeBranch = result.First().DealerLocation.HeadOfficeBranch;
                    pDto.Location = result.First().DealerLocation.Location;
                    pDto.LocationCode = result.First().DealerLocation.LocationCode;
                    pDto.SalesContactPerson = result.First().DealerLocation.SalesContactPerson;
                    pDto.SalesEmail = result.First().DealerLocation.SalesEmail;
                    pDto.SalesFax = result.First().DealerLocation.SalesFax;
                    pDto.SalesTelephone = result.First().DealerLocation.SalesTelephone;
                    pDto.ServiceContactPerson = result.First().DealerLocation.ServiceContactPerson;
                    pDto.ServiceEmail = result.First().DealerLocation.ServiceEmail;
                    pDto.ServiceFax = result.First().DealerLocation.ServiceFax;
                    pDto.ServiceTelephone = result.First().DealerLocation.ServiceTelephone;
                    pDto.EntryDateTime = result.First().DealerLocation.EntryDateTime;
                    pDto.EntryUser = result.First().DealerLocation.EntryUser;

                    pDto.IsDealerLocationExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsDealerLocationExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal bool AddDealerLocation(DealerLocationRequestDto DealerLocation)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                DealerLocation pr = new Entities.DealerLocation();
                pr.Id = new Guid();
                pr.CityId = DealerLocation.CityId;
                pr.TpaBranchId = DealerLocation.TpaBranchId;
                pr.DealerId = DealerLocation.DealerId;
                pr.HeadOfficeBranch = DealerLocation.HeadOfficeBranch;
                pr.Location = DealerLocation.Location;
                pr.LocationCode = DealerLocation.LocationCode;
                pr.DealerAddress = DealerLocation.DealerAddress;
                pr.SalesContactPerson = DealerLocation.SalesContactPerson;
                pr.SalesEmail = DealerLocation.SalesEmail;
                pr.SalesFax = DealerLocation.SalesFax;
                pr.SalesTelephone = DealerLocation.SalesTelephone;
                pr.ServiceContactPerson = DealerLocation.ServiceContactPerson;
                pr.ServiceEmail = DealerLocation.ServiceEmail;
                pr.ServiceFax = DealerLocation.ServiceFax;
                pr.ServiceTelephone = DealerLocation.ServiceTelephone;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");



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

        internal bool UpdateDealerLocation(DealerLocationRequestDto DealerLocation)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                DealerLocation pr = new Entities.DealerLocation();

                pr.Id = DealerLocation.Id;
                pr.CityId = DealerLocation.CityId;
                pr.TpaBranchId = DealerLocation.TpaBranchId;
                pr.DealerId = DealerLocation.DealerId;
                pr.HeadOfficeBranch = DealerLocation.HeadOfficeBranch;
                pr.Location = DealerLocation.Location;
                pr.LocationCode = DealerLocation.LocationCode;
                pr.DealerAddress = DealerLocation.DealerAddress;
                pr.SalesContactPerson = DealerLocation.SalesContactPerson;
                pr.SalesEmail = DealerLocation.SalesEmail;
                pr.SalesFax = DealerLocation.SalesFax;
                pr.SalesTelephone = DealerLocation.SalesTelephone;
                pr.ServiceContactPerson = DealerLocation.ServiceContactPerson;
                pr.ServiceEmail = DealerLocation.ServiceEmail;
                pr.ServiceFax = DealerLocation.ServiceFax;
                pr.ServiceTelephone = DealerLocation.ServiceTelephone;
                pr.EntryDateTime = DealerLocation.EntryDateTime;
                pr.EntryUser = DealerLocation.EntryUser;

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

        internal static List<DealerBranchStaff> DealerBranchStaff()
        {
            List<DealerBranchStaff> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<DealerBranchStaff> DealerLocationData = session.Query<DealerBranchStaff>();
            entities = DealerLocationData.ToList();
            return entities;
        }
    }
}
