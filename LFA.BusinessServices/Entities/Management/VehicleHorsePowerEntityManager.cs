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
	public class VehicleHorsePowerEntityManager
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();

		public List<VehicleHorsePower> GetVehicleHorsePowers()
		{
			List<VehicleHorsePower> entities = null;
			ISession session = EntitySessionManager.GetSession();
			IQueryable<VehicleHorsePower> VehicleHorsePowerData = session.Query<VehicleHorsePower>();
			entities = VehicleHorsePowerData.ToList();
			return entities;
		}


		public VehicleHorsePowerResponseDto GetVehicleHorsePowerById(Guid VehicleHorsePowerId)
		{
            try
            {
                ISession session = EntitySessionManager.GetSession();

                VehicleHorsePowerResponseDto pDto = new VehicleHorsePowerResponseDto();

                var query =
                    from VehicleHorsePower in session.Query<VehicleHorsePower>()
                    where VehicleHorsePower.Id == VehicleHorsePowerId
                    select new { VehicleHorsePower = VehicleHorsePower };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().VehicleHorsePower.Id;
                    pDto.HorsePower = result.First().VehicleHorsePower.HorsePower;
                    pDto.EntryDateTime = result.First().VehicleHorsePower.EntryDateTime;
                    pDto.EntryUser = result.First().VehicleHorsePower.EntryUser;

                    pDto.IsVehicleHorsePowerExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsVehicleHorsePowerExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
			
		}


		internal bool AddVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower)
		{
			try
			{
				
				ISession session = EntitySessionManager.GetSession();
				VehicleHorsePower pr = new Entities.VehicleHorsePower();

				pr.Id = new Guid();
				pr.HorsePower= VehicleHorsePower.HorsePower;
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

		internal bool UpdateVehicleHorsePower(VehicleHorsePowerRequestDto VehicleHorsePower)
		{
			try
			{
				ISession session = EntitySessionManager.GetSession();
				VehicleHorsePower pr = new Entities.VehicleHorsePower();

				pr.Id = VehicleHorsePower.Id;
				pr.HorsePower = VehicleHorsePower.HorsePower;
				pr.EntryDateTime = VehicleHorsePower.EntryDateTime;
				pr.EntryUser = VehicleHorsePower.EntryUser;

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

        internal int IsExsistingHorsePowerByHorsePower(Guid Id, string HorsePower)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from VehicleHorsePower in session.Query<VehicleHorsePower>()
                    where VehicleHorsePower.HorsePower == HorsePower
                    select new { VehicleHorsePower = VehicleHorsePower };

                return query.Count();
            }
            else
            {
                var query =
                    from VehicleHorsePower in session.Query<VehicleHorsePower>()
                    where VehicleHorsePower.Id != Id && VehicleHorsePower.HorsePower == HorsePower
                    select new { VehicleHorsePower = VehicleHorsePower };

                return query.Count();
            }
        }

        internal int IsExsistingHorsePowerByEntryUser(Guid Id, string EntryUser)
        {
            throw new NotImplementedException();
        }
    }
}
