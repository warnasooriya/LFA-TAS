using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class CommodityEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<CommodityRespondDto> GetAllCommodities()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CommodityType>().ToList().Select(s => new CommodityRespondDto
            {
                CommodityTypeId = s.CommodityTypeId,
                CommodityCode = s.CommodityCode,
                CommonCode = s.CommonCode,
                CommodityTypeDescription = s.CommodityTypeDescription
            }).ToList();
        }

        internal List<CommodityCategory> GetCommodityCategoriesByCommodityTypeId(Guid commodityTypeID)
        {
            List<CommodityCategory> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<CommodityCategory> commodityCategoryData = session.Query<CommodityCategory>().Where(a => a.CommodityTypeId == commodityTypeID);
            entities = commodityCategoryData.ToList();
            return entities;
        }



        internal CommodityType GetCommodityById(Guid CommodityTypeId)
        {
            ISession session = EntitySessionManager.GetSession();
            CommodityType commodityType = session.Query<CommodityType>()
                .Where(a => a.CommodityTypeId == CommodityTypeId).FirstOrDefault();
            return commodityType;
        }

        internal static object GetCommodityTypebByCommodityCategoryId(Guid commodityCategoryId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommodityCategory commodityCategory = session.Query<CommodityCategory>()
                    .FirstOrDefault(a => a.CommodityCategoryId == commodityCategoryId);
                if (commodityCategory != null)
                {
                    CommodityType commodityType = session.Query<CommodityType>()
                   .FirstOrDefault(a => a.CommodityTypeId == commodityCategory.CommodityTypeId);
                    Response = commodityType;

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }
    }
}
