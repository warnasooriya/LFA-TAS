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
    public class RegionEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<RegionResponseDto> GetRegions()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Region>().Select(Region => new RegionResponseDto
            {
                Id = Region.Id,
                RegionCode = Region.RegionCode,
                RegionName = Region.RegionName
            }).ToList();
        }

        public RegionResponseDto GetRegionById(Guid RegionId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                RegionResponseDto pDto = new RegionResponseDto();

                var query =
                    from Region in session.Query<Region>()
                    where Region.Id == RegionId
                    select new { Region = Region };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Region.Id;
                    pDto.RegionCode = result.First().Region.RegionCode;
                    pDto.RegionName = result.First().Region.RegionName;

                    pDto.IsRegionExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsRegionExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddRegion(RegionRequestDto Region)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Region pr = new Entities.Region();

                pr.Id = new Guid();
                pr.RegionCode = Region.RegionCode;
                pr.RegionName = Region.RegionName;

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

        internal bool UpdateRegion(RegionRequestDto Region)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Region pr = new Entities.Region();

                pr.Id = Region.Id;
                pr.RegionCode = Region.RegionCode;
                pr.RegionName = Region.RegionName;

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
