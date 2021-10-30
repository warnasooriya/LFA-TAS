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
    public class CommodityCategoryEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<CommodityCategoryResponseDto> GetCommodityCategories(Guid CommodityTypeId)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CommodityCategory>().Where(c => c.CommodityTypeId == CommodityTypeId)
                .Select(s => new CommodityCategoryResponseDto
            {
                CommodityCategoryId = s.CommodityCategoryId,
                CommodityCategoryCode = s.CommodityCategoryCode,
                CommodityCategoryDescription = s.CommodityCategoryDescription,
                Length = s.Length
            }).ToList();


        }

        public CommodityCategoryResponseDto GetCommodityCategoryById(Guid CommodityCategoryId,Guid CommodityTypeId)
        {
            ISession session = EntitySessionManager.GetSession();

            CommodityCategoryResponseDto pDto = new CommodityCategoryResponseDto();

            var query =
                from CommodityCategory in session.Query<CommodityCategory>()
                where CommodityCategory.CommodityCategoryId == CommodityCategoryId
                select new { CommodityCategory = CommodityCategory};

                var result = query.ToList();

            if (result != null && result.Count > 0)
            {
                pDto.CommodityCategoryId = result.First().CommodityCategory.CommodityCategoryId;
                pDto.CommodityCategoryDescription = result.First().CommodityCategory.CommodityCategoryDescription;
                pDto.CommodityCategoryCode = result.First().CommodityCategory.CommodityCategoryCode;
                pDto.Length = result.First().CommodityCategory.Length;

                pDto.IsCommodityCategoryExists = true;
                return pDto;
            }
            else
            {
                pDto.IsCommodityCategoryExists = false;
                return null;
            }
        }

        internal List<CommodityCategory> GetCommodityCategoriesByCommodityTypeId(Guid commodityTypeID)
        {
            List<CommodityCategory> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CommodityCategory> commodityCategoryData = session.Query<CommodityCategory>().Where(a => a.CommodityTypeId == commodityTypeID);
            entities = commodityCategoryData.ToList();
            return entities;
        }

        internal bool AddCommodityCategory(CommodityCategoryRequestDto CommodityCategory)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommodityCategory pr = new Entities.CommodityCategory();

                pr.CommodityCategoryId = new Guid();

                pr.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
                pr.CommodityCategoryCode = CommodityCategory.CommodityCategoryCode;
                pr.CommodityTypeId = CommodityCategory.CommodityTypeId;
                pr.Length = CommodityCategory.Length;

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

        internal bool UpdateCommodityCategory(CommodityCategoryRequestDto CommodityCategory)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommodityCategory pr = new Entities.CommodityCategory();

                pr.CommodityCategoryId = CommodityCategory.CommodityCategoryId;
                pr.CommodityCategoryDescription = CommodityCategory.CommodityCategoryDescription;
                pr.CommodityCategoryCode = CommodityCategory.CommodityCategoryCode;
                pr.CommodityTypeId = CommodityCategory.CommodityTypeId;
                pr.Length = CommodityCategory.Length;

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

        internal int GetExsistingCommodityCategoryByDescription(Guid CommodityCategoryId, string CommodityCategoryDescription)
        {
            ISession session = EntitySessionManager.GetSession();

            if (CommodityCategoryId == Guid.Empty)
            {
                var query =
                    from CommodityCategory in session.Query<CommodityCategory>()
                    where CommodityCategory.CommodityCategoryDescription == CommodityCategoryDescription
                    select new { CommodityCategory = CommodityCategory };

                return query.Count();
            }
            else
            {
                var query =
                    from CommodityCategory in session.Query<CommodityCategory>()
                    where CommodityCategory.CommodityCategoryId != CommodityCategoryId && CommodityCategory.CommodityCategoryDescription == CommodityCategoryDescription
                    select new { CommodityCategory = CommodityCategory };

                return query.Count();
            }
        }

        internal int GetExsistingCommodityCategoryByCode(Guid CommodityCategoryId, string CommodityCategoryCode)
        {
            ISession session = EntitySessionManager.GetSession();

            if (CommodityCategoryId == Guid.Empty)
            {
                var query =
                    from CommodityCategory in session.Query<CommodityCategory>()
                    where CommodityCategory.CommodityCategoryCode == CommodityCategoryCode
                    select new { CommodityCategory = CommodityCategory };

                return query.Count();
            }
            else
            {
                var query =
                    from CommodityCategory in session.Query<CommodityCategory>()
                    where CommodityCategory.CommodityCategoryId != CommodityCategoryId && CommodityCategory.CommodityCategoryCode == CommodityCategoryCode
                    select new { CommodityCategory = CommodityCategory };

                return query.Count();
            }
        }
    }
}
