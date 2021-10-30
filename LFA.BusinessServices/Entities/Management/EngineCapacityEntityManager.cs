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
    public class EngineCapacityEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<EngineCapacityResponseDto> GetEngineCapacities()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<EngineCapacity>().Select(s => new EngineCapacityResponseDto
            {
                Id = s.Id,
                EngineCapacityNumber = s.EngineCapacityNumber,
                MesureType = s.MesureType,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList(); ;
        }


        public EngineCapacityResponseDto GetEngineCapacityById(Guid EngineCapacityId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();

                EngineCapacityResponseDto pDto = new EngineCapacityResponseDto();

                var query =
                    from engineCapacity in session.Query<EngineCapacity>()
                    where engineCapacity.Id == EngineCapacityId
                    select new { engineCapacity = engineCapacity };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().engineCapacity.Id;
                    pDto.EngineCapacityNumber = result.First().engineCapacity.EngineCapacityNumber;
                    pDto.MesureType = result.First().engineCapacity.MesureType;
                    pDto.EntryDateTime = result.First().engineCapacity.EntryDateTime;
                    pDto.EntryUser = result.First().engineCapacity.EntryUser;


                    pDto.IsEngineCapacityExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsEngineCapacityExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddEngineCapacity(EngineCapacityRequestDto EngineCapacity)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                EngineCapacity pr = new Entities.EngineCapacity();

                pr.Id = new Guid();
                pr.EngineCapacityNumber = EngineCapacity.EngineCapacityNumber;
				pr.MesureType = EngineCapacity.MesureType;
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

        internal bool UpdateEngineCapacity(EngineCapacityRequestDto EngineCapacity)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                EngineCapacity pr = new Entities.EngineCapacity();

                pr.Id = EngineCapacity.Id;
                pr.EngineCapacityNumber = EngineCapacity.EngineCapacityNumber;
				pr.MesureType = EngineCapacity.MesureType;
                pr.EntryDateTime = EngineCapacity.EntryDateTime;
                pr.EntryUser = EngineCapacity.EntryUser;

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

        internal int GetExsistingEngineCapacityByEngineCapacityNumber(Guid Id, decimal EngineCapacityNumber)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from EngineCapacity in session.Query<EngineCapacity>()
                    where EngineCapacity.EngineCapacityNumber == EngineCapacityNumber
                    select new { EngineCapacity = EngineCapacity };

                return query.Count();
            }
            else
            {
                var query =
                    from EngineCapacity in session.Query<EngineCapacity>()
                    where EngineCapacity.Id != Id && EngineCapacity.EngineCapacityNumber == EngineCapacityNumber
                    select new { EngineCapacity = EngineCapacity };

                return query.Count();
            }
        }

        internal int GetExsistingEngineCapacityByMesureType(Guid Id, string MesureType)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from EngineCapacity in session.Query<EngineCapacity>()
                    where EngineCapacity.MesureType == MesureType
                    select new { EngineCapacity = EngineCapacity };

                return query.Count();
            }
            else
            {
                var query =
                    from EngineCapacity in session.Query<EngineCapacity>()
                    where EngineCapacity.Id != Id && EngineCapacity.MesureType == MesureType
                    select new { EngineCapacity = EngineCapacity };

                return query.Count();
            }
        }
    }
}
