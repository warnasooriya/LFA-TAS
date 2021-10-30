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
    public class VehicleBodyTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<VehicleBodyTypeResponseDto> GetVehicleBodyTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<VehicleBodyType>().Select(s => new VehicleBodyTypeResponseDto
            {
                Id = s.Id,
                VehicleBodyTypeCode = s.VehicleBodyTypeCode,
                VehicleBodyTypeDescription = s.VehicleBodyTypeDescription,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();
        }


        public VehicleBodyTypeResponseDto GetVehicleBodyTypeById(Guid VehicleBodyTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleBodyTypeResponseDto pDto = new VehicleBodyTypeResponseDto();

                var query =
                    from vehicleBodyType in session.Query<VehicleBodyType>()
                    where vehicleBodyType.Id == VehicleBodyTypeId
                    select new { vehicleBodyType = vehicleBodyType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().vehicleBodyType.Id;
                    pDto.VehicleBodyTypeDescription = result.First().vehicleBodyType.VehicleBodyTypeDescription;
                    pDto.VehicleBodyTypeCode = result.First().vehicleBodyType.VehicleBodyTypeCode;
                    pDto.EntryDateTime = result.First().vehicleBodyType.EntryDateTime;
                    pDto.EntryUser = result.First().vehicleBodyType.EntryUser;

                    pDto.IsVehicleBodyTypeExists = true;

                    return pDto;
                }
                else
                {
                    pDto.IsVehicleBodyTypeExists = false;

                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                VehicleBodyType pr = new Entities.VehicleBodyType();

                pr.Id = new Guid();
                pr.VehicleBodyTypeDescription = VehicleBodyType.VehicleBodyTypeDescription;
				 pr.VehicleBodyTypeCode = VehicleBodyType.VehicleBodyTypeCode;
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

        internal bool UpdateVehicleBodyType(VehicleBodyTypeRequestDto VehicleBodyType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                VehicleBodyType pr = new Entities.VehicleBodyType();

                pr.Id = VehicleBodyType.Id;
                pr.VehicleBodyTypeDescription = VehicleBodyType.VehicleBodyTypeDescription;
				 pr.VehicleBodyTypeCode = VehicleBodyType.VehicleBodyTypeCode;
                pr.EntryDateTime = VehicleBodyType.EntryDateTime;
                pr.EntryUser = VehicleBodyType.EntryUser;

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
    }
}
