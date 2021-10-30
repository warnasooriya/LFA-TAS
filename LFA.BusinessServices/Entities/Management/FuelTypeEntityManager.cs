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
    public class FuelTypeEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<FuelTypeResponseDto> GetFuelTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<FuelType>().Select(s => new FuelTypeResponseDto
            {
                FuelTypeId = s.FuelTypeId,
                FuelTypeCode = s.FuelTypeCode,
                FuelTypeDescription = s.FuelTypeDescription,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();
        }


        public FuelTypeResponseDto GetFuelTypeById(Guid FuelTypeId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                FuelTypeResponseDto pDto = new FuelTypeResponseDto();

                var query =
                    from fuelType in session.Query<FuelType>()
                    where fuelType.FuelTypeId == FuelTypeId
                    select new { fuelType = fuelType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {

                    pDto.FuelTypeId = result.First().fuelType.FuelTypeId;
                    pDto.FuelTypeCode = result.First().fuelType.FuelTypeCode;
                    pDto.FuelTypeDescription = result.First().fuelType.FuelTypeDescription;
                    pDto.EntryDateTime = result.First().fuelType.EntryDateTime;
                    pDto.EntryUser = result.First().fuelType.EntryUser;

                    pDto.IsFuelTypeExists = true;

                    return pDto;
                }
                else
                {
                    pDto.IsFuelTypeExists = false;

                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddFuelType(FuelTypeRequestDto FuelType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                FuelType pr = new Entities.FuelType();

                pr.FuelTypeId = new Guid();
                pr.FuelTypeCode = FuelType.FuelTypeCode;
				pr.FuelTypeDescription = FuelType.FuelTypeDescription;
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

        internal bool UpdateFuelType(FuelTypeRequestDto FuelType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FuelType pr = new Entities.FuelType();

                pr.FuelTypeId = FuelType.FuelTypeId;
                pr.FuelTypeCode = FuelType.FuelTypeCode;
				pr.FuelTypeDescription = FuelType.FuelTypeDescription;
                pr.EntryDateTime = FuelType.EntryDateTime;
                pr.EntryUser = FuelType.EntryUser;

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
