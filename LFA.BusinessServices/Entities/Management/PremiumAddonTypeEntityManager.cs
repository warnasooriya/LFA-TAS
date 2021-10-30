using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Persistence;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class PremiumAddonTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<PremiumAddonTypeResponseDto> GetPremiumAddonTypes()
        {
            var session = EntitySessionManager.GetSession();
            return session.Query<PremiumAddonType>().Select(PremiumAddonType => new PremiumAddonTypeResponseDto {
                Id = PremiumAddonType.Id,
                CommodityTypeId = PremiumAddonType.CommodityTypeId,
                Description = PremiumAddonType.Description,
                EntryDateTime = PremiumAddonType.EntryDateTime,
                EntryUser = PremiumAddonType.EntryUser,
                IndexNo = PremiumAddonType.IndexNo,
            }).ToList();
        }
        public List<PremiumAddonType> GetPremiumAddonTypesBycommodityTypeId(Guid CommodityTypeId)
        {
            List<PremiumAddonType> entities = null;
            var session = EntitySessionManager.GetSession();
            var PremiumAddonTypeData = session.Query<PremiumAddonType>().Where(a=>a.CommodityTypeId== CommodityTypeId);
            entities = PremiumAddonTypeData.ToList();
            return entities;
        }

        public List<PremiumAddonType> GetPremiumAddonTypesforVariant()
        {
            List<PremiumAddonType> entities = null;
            var session = EntitySessionManager.GetSession();
            var PremiumAddonTypeData = session.Query<PremiumAddonType>().Where(a => a.IsApplicableforVariant == true);
            entities = PremiumAddonTypeData.ToList();
            return entities;
        }

        public PremiumAddonTypeResponseDto GetPremiumAddonTypeById(Guid PremiumAddonTypeId)
        {
            try
            {
                var session = EntitySessionManager.GetSession();

                var pDto = new PremiumAddonTypeResponseDto();

                var query =
                    from PremiumAddonType in session.Query<PremiumAddonType>()
                    where PremiumAddonType.Id == PremiumAddonTypeId
                    select new { PremiumAddonType };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().PremiumAddonType.Id;
                    pDto.CommodityTypeId = result.First().PremiumAddonType.CommodityTypeId;
                    pDto.Description = result.First().PremiumAddonType.Description;
                    pDto.EntryDateTime = result.First().PremiumAddonType.EntryDateTime;
                    pDto.EntryUser = result.First().PremiumAddonType.EntryUser;
                    pDto.IndexNo = result.First().PremiumAddonType.IndexNo;

                    pDto.IsPremiumAddonTypeExists = true;
                    return pDto;
                }
                pDto.IsPremiumAddonTypeExists = false;
                return null;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddPremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType)
        {
            try
            {
                var session = EntitySessionManager.GetSession();
                var pr = new PremiumAddonType();

                pr.Id = new Guid();
                pr.CommodityTypeId = PremiumAddonType.CommodityTypeId;
                pr.IndexNo = PremiumAddonType.IndexNo;
                pr.Description = PremiumAddonType.Description;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (var transaction = session.BeginTransaction())
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

        internal bool UpdatePremiumAddonType(PremiumAddonTypeRequestDto PremiumAddonType)
        {
            try
            {
                var session = EntitySessionManager.GetSession();
                var pr = new PremiumAddonType();

                pr.Id = PremiumAddonType.Id;
                pr.CommodityTypeId = PremiumAddonType.CommodityTypeId;
                pr.Description = PremiumAddonType.Description;
                pr.EntryDateTime = PremiumAddonType.EntryDateTime;
                pr.EntryUser = PremiumAddonType.EntryUser;

                using (var transaction = session.BeginTransaction())
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