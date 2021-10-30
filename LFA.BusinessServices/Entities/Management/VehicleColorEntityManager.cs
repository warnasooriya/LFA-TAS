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
using System.Reflection;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class VehicleColorEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<VehicleColorResponseDto> GetVehicleColors()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<VehicleColor>().Select(VehicleColor => new VehicleColorResponseDto {
                Id = VehicleColor.Id,
                VehicleColorCode = VehicleColor.VehicleColorCode,
                Color = VehicleColor.Color,
                EntryDateTime = VehicleColor.EntryDateTime,
                EntryUser = VehicleColor.EntryUser
            }).ToList();
        }


        public VehicleColorResponseDto GetVehicleColorById(Guid VehicleColorId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleColorResponseDto pDto = new VehicleColorResponseDto();

                var query =
                    from vehicleColor in session.Query<VehicleColor>()
                    where vehicleColor.Id == VehicleColorId
                    select new { vehicleColor = vehicleColor };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().vehicleColor.Id;
                    pDto.VehicleColorCode = result.First().vehicleColor.VehicleColorCode;
                    pDto.Color = result.First().vehicleColor.Color;
                    pDto.EntryDateTime = result.First().vehicleColor.EntryDateTime;
                    pDto.EntryUser = result.First().vehicleColor.EntryUser;

                    pDto.IsVehicleColorExists = true;

                    return pDto;
                }
                else
                {
                    pDto.IsVehicleColorExists = false;

                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddVehicleColor(VehicleColorRequestDto VehicleColor)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                VehicleColor pr = new Entities.VehicleColor();

                pr.Id = new Guid();
                pr.VehicleColorCode = VehicleColor.VehicleColorCode;
				  pr.Color = VehicleColor.Color;
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

        internal bool UpdateVehicleColor(VehicleColorRequestDto VehicleColor)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                VehicleColor pr = new Entities.VehicleColor();

                pr.Id = VehicleColor.Id;
                pr.VehicleColorCode = VehicleColor.VehicleColorCode;
				 pr.Color = VehicleColor.Color;
                pr.EntryDateTime = VehicleColor.EntryDateTime;
                pr.EntryUser = VehicleColor.EntryUser;

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
