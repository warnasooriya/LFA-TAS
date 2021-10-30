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
    public class CylinderCountEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<CylinderCountResponseDto> GetCylinderCounts()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CylinderCount>().Select(s => new CylinderCountResponseDto
            {
                Id = s.Id,
                Count = s.Count,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();
        }


        public CylinderCountResponseDto GetCylinderCountById(Guid CylinderCountId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                CylinderCountResponseDto pDto = new CylinderCountResponseDto();

                var query =
                    from cylinderCount in session.Query<CylinderCount>()
                    where cylinderCount.Id == CylinderCountId
                    select new { cylinderCount = cylinderCount };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {

                    pDto.Id = result.First().cylinderCount.Id;
                    pDto.Count = result.First().cylinderCount.Count;
                    pDto.EntryDateTime = result.First().cylinderCount.EntryDateTime;
                    pDto.EntryUser = result.First().cylinderCount.EntryUser;

                    pDto.IsCylinderCountExists = true;

                    return pDto;
                }
                else
                {
                    pDto.IsCylinderCountExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal bool AddCylinderCount(CylinderCountRequestDto CylinderCount)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                CylinderCount pr = new Entities.CylinderCount();

                pr.Id = new Guid();
                pr.Count = CylinderCount.Count;
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

        internal bool UpdateCylinderCount(CylinderCountRequestDto CylinderCount)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CylinderCount pr = new Entities.CylinderCount();

                pr.Id = CylinderCount.Id;
                pr.Count = CylinderCount.Count;
                pr.EntryDateTime = CylinderCount.EntryDateTime;
                pr.EntryUser = CylinderCount.EntryUser;

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
