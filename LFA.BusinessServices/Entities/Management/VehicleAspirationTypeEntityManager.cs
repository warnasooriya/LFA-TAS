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

namespace TAS.Services.Entities.Management
{
    public class VehicleAspirationTypeEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<VehicleAspirationTypeResponseDto> GetVehicleAspirationTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<VehicleAspirationType>().Select(s => new VehicleAspirationTypeResponseDto
            {
                Id = s.Id,
                AspirationTypeCode = s.AspirationTypeCode,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser

            }).ToList();
        }


        public VehicleAspirationTypeResponseDto GetVehicleAspirationTypeById(Guid VehicleAspirationTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleAspirationTypeResponseDto pDto = new VehicleAspirationTypeResponseDto();

                var query =
                    from VehicleAspirationType in session.Query<VehicleAspirationType>()
                    where VehicleAspirationType.Id == VehicleAspirationTypeId
                    select new { VehicleAspirationType = VehicleAspirationType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().VehicleAspirationType.Id;
                    pDto.AspirationTypeCode = result.First().VehicleAspirationType.AspirationTypeCode;
                    pDto.EntryDateTime = result.First().VehicleAspirationType.EntryDateTime;
                    pDto.EntryUser = result.First().VehicleAspirationType.EntryUser;

                    pDto.IsVehicleAspirationTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVehicleAspirationTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                VehicleAspirationType pr = new Entities.VehicleAspirationType();

                pr.Id = new Guid();
				pr.AspirationTypeCode= VehicleAspirationType.AspirationTypeCode;
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

        internal bool UpdateVehicleAspirationType(VehicleAspirationTypeRequestDto VehicleAspirationType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                VehicleAspirationType pr = new Entities.VehicleAspirationType();

                pr.Id = VehicleAspirationType.Id;
				pr.AspirationTypeCode = VehicleAspirationType.AspirationTypeCode;
                pr.EntryDateTime = VehicleAspirationType.EntryDateTime;
                pr.EntryUser = VehicleAspirationType.EntryUser;

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

        internal int GetExsistingVehicleAspirationTypeByAspirationTypeCode(Guid Id, string AspirationTypeCode)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from VehicleAspirationType in session.Query<VehicleAspirationType>()
                    where VehicleAspirationType.AspirationTypeCode == AspirationTypeCode
                    select new { VehicleAspirationType = VehicleAspirationType };

                return query.Count();
            }
            else
            {
                var query =
                    from VehicleAspirationType in session.Query<VehicleAspirationType>()
                    where VehicleAspirationType.Id != Id && VehicleAspirationType.AspirationTypeCode == AspirationTypeCode
                    select new { VehicleAspirationType = VehicleAspirationType };

                return query.Count();
            }
        }

        internal int GetExsistingVehicleAspirationTypeByEntryUser(Guid Id, string EntryUser)
        {
            throw new NotImplementedException();
        }

        internal int IsExsistingAspirationTypesByCode(Guid Id, string AspirationTypeCode)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from VehicleAspirationType in session.Query<VehicleAspirationType>()
                    where VehicleAspirationType.AspirationTypeCode == AspirationTypeCode
                    select new { VehicleAspirationType = VehicleAspirationType };

                return query.Count();
            }
            else
            {
                var query =
                    from VehicleAspirationType in session.Query<VehicleAspirationType>()
                    where VehicleAspirationType.Id != Id && VehicleAspirationType.AspirationTypeCode == AspirationTypeCode
                    select new { VehicleAspirationType = VehicleAspirationType };

                return query.Count();
            }
        }

        internal int IsExsistingAspirationTypesByEntryUser(Guid Id, string EntryUser)
        {
            throw new NotImplementedException();
        }
    }
}
