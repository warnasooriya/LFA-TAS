using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using TAS.Services.Entities.Persistence;
using TAS.DataTransfer.Responses;
using TAS.DataTransfer.Requests;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class CommodityUsageTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<CommodityUsageTypeResponseDto> GetAllCommodityUsageTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CommodityUsageType>().Select(s => new CommodityUsageTypeResponseDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            }).ToList();
        }

        public CommodityUsageTypeResponseDto GetCommodityUsageTypeById(Guid CommodityUsageTypeId)
        {
            ISession session = EntitySessionManager.GetSession();

            CommodityUsageTypeResponseDto pDto = new CommodityUsageTypeResponseDto();

            var query =
                from CommodityUsageType in session.Query<CommodityUsageType>()
                where CommodityUsageType.Id == CommodityUsageTypeId
                select new { CommodityUsageType = CommodityUsageType };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {

                pDto.Id = result.First().CommodityUsageType.Id;
                pDto.Name = result.First().CommodityUsageType.Name;
                pDto.Description = result.First().CommodityUsageType.Description;

                pDto.IsCommodityUsageTypeExists = true;

                return pDto;
            }
            else
            {
                pDto.IsCommodityUsageTypeExists = false;

                return null;
            }
        }

        internal bool AddCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                CommodityUsageType pr = new Entities.CommodityUsageType();

                pr.Id = new Guid();
                pr.Name = CommodityUsageType.Name;
                pr.Description = CommodityUsageType.Description;

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

        internal bool UpdateCommodityUsageType(CommodityUsageTypeRequestDto CommodityUsageType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommodityUsageType pr = new Entities.CommodityUsageType();

                pr.Id = CommodityUsageType.Id;
                pr.Name = CommodityUsageType.Name;
                pr.Description = CommodityUsageType.Description;

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

        internal int GetExsistingCommodityUsageTypeByName(Guid Id, string Name)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from CommodityUsageType in session.Query<CommodityUsageType>()
                    where CommodityUsageType.Name == Name
                    select new { CommodityUsageType = CommodityUsageType };

                return query.Count();
            }
            else
            {
                var query =
                    from CommodityUsageType in session.Query<CommodityUsageType>()
                    where CommodityUsageType.Id != Id && CommodityUsageType.Name == Name
                    select new { CommodityUsageType = CommodityUsageType };

                return query.Count();
            }
        }

        internal int GetExsistingCommodityUsageTypeByDescription(Guid Id, string Description)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from CommodityUsageType in session.Query<CommodityUsageType>()
                    where CommodityUsageType.Description == Description
                    select new { CommodityUsageType = CommodityUsageType };

                return query.Count();
            }
            else
            {
                var query =
                    from CommodityUsageType in session.Query<CommodityUsageType>()
                    where CommodityUsageType.Id != Id && CommodityUsageType.Description == Description
                    select new { CommodityUsageType = CommodityUsageType };

                return query.Count();
            }
        }
    }
}
