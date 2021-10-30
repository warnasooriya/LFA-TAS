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
    public class DealTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<DealTypeResponseDto> GetDealTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<DealType>().Select(DealType => new DealTypeResponseDto
            {
                Id = DealType.Id,
                Name = DealType.Name
            }).ToList(); ;
        }

        public DealTypeResponseDto GetDealTypeById(Guid DealTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                DealTypeResponseDto pDto = new DealTypeResponseDto();

                var query =
                    from DealType in session.Query<DealType>()
                    where DealType.Id == DealTypeId
                    select new { DealType = DealType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().DealType.Id;
                    pDto.Name = result.First().DealType.Name;

                    pDto.IsDealTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsDealTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddDealType(DealTypeRequestDto DealType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                DealType pr = new Entities.DealType();

                pr.Id = new Guid();
                pr.Name = DealType.Name;

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

        internal bool UpdateDealType(DealTypeRequestDto DealType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                DealType pr = new Entities.DealType();

                pr.Id = DealType.Id;
                pr.Name = DealType.Name;

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
