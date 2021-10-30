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
	public class VehicleKiloWattEntityManager
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();

		public List<VehicleKiloWatt> GetVehicleKiloWatts()
		{
			List<VehicleKiloWatt> entities = null;
			ISession session = EntitySessionManager.GetSession();
			IQueryable<VehicleKiloWatt> VehicleKiloWattData = session.Query<VehicleKiloWatt>();
			entities = VehicleKiloWattData.ToList();
			return entities;
		}


		public VehicleKiloWattResponseDto GetVehicleKiloWattById(Guid VehicleKiloWattId)
		{
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleKiloWattResponseDto pDto = new VehicleKiloWattResponseDto();

                var query =
                    from VehicleKiloWatt in session.Query<VehicleKiloWatt>()
                    where VehicleKiloWatt.Id == VehicleKiloWattId
                    select new { VehicleKiloWatt = VehicleKiloWatt };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().VehicleKiloWatt.Id;
                    pDto.KiloWatt = result.First().VehicleKiloWatt.KiloWatt;
                    pDto.EntryDateTime = result.First().VehicleKiloWatt.EntryDateTime;
                    pDto.EntryUser = result.First().VehicleKiloWatt.EntryUser;

                    pDto.IsVehicleKiloWattExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVehicleKiloWattExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
			
		}


		internal bool AddVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt)
		{
			try
			{
				
				ISession session = EntitySessionManager.GetSession();
				VehicleKiloWatt pr = new Entities.VehicleKiloWatt();

				pr.Id = new Guid();
				pr.KiloWatt= VehicleKiloWatt.KiloWatt;
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

		internal bool UpdateVehicleKiloWatt(VehicleKiloWattRequestDto VehicleKiloWatt)
		{
			try
			{
				ISession session = EntitySessionManager.GetSession();
				VehicleKiloWatt pr = new Entities.VehicleKiloWatt();

				pr.Id = VehicleKiloWatt.Id;
				pr.KiloWatt = VehicleKiloWatt.KiloWatt;
				pr.EntryDateTime = VehicleKiloWatt.EntryDateTime;
				pr.EntryUser = VehicleKiloWatt.EntryUser;

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

        internal int IsExsistingVehicleKiloWattByKiloWatt(Guid Id, string KiloWatt)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from VehicleKiloWatt in session.Query<VehicleKiloWatt>()
                    where VehicleKiloWatt.KiloWatt == KiloWatt
                    select new { VehicleKiloWatt = VehicleKiloWatt };

                return query.Count();
            }
            else
            {
                var query =
                    from VehicleKiloWatt in session.Query<VehicleKiloWatt>()
                    where VehicleKiloWatt.Id != Id && VehicleKiloWatt.KiloWatt == KiloWatt
                    select new { VehicleKiloWatt = VehicleKiloWatt };

                return query.Count();
            }
        }

        internal int IsExsistingVehicleKiloWattByEntryUser(Guid Id, string EntryUser)
        {
            throw new NotImplementedException();
        }
    }
}
