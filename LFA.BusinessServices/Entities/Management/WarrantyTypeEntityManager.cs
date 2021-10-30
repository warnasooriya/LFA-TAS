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
    public class WarrantyTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<WarrantyTypeResponseDto> GetWarrantyTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<WarrantyType>().Select(WarrantyType => new WarrantyTypeResponseDto
            {
                Id = WarrantyType.Id,
                WarrantyTypeDescription = WarrantyType.WarrantyTypeDescription,
                EntryDateTime = WarrantyType.EntryDateTime,
                EntryUser = WarrantyType.EntryUser
            }).ToList();
        }

        public WarrantyTypeResponseDto GetWarrantyTypeById(Guid WarrantyTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                WarrantyTypeResponseDto pDto = new WarrantyTypeResponseDto();

                var query =
                    from WarrantyType in session.Query<WarrantyType>()
                    where WarrantyType.Id == WarrantyTypeId
                    select new { WarrantyType = WarrantyType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().WarrantyType.Id;
                    pDto.WarrantyTypeDescription = result.First().WarrantyType.WarrantyTypeDescription;
                    pDto.EntryDateTime = result.First().WarrantyType.EntryDateTime;
                    pDto.EntryUser = result.First().WarrantyType.EntryUser;

                    pDto.IsWarrantyTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsWarrantyTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }


        internal bool AddWarrantyType(WarrantyTypeRequestDto WarrantyType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                WarrantyType pr = new Entities.WarrantyType();

                pr.Id = new Guid();
				pr.WarrantyTypeDescription= WarrantyType.WarrantyTypeDescription;
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

        internal bool UpdateWarrantyType(WarrantyTypeRequestDto WarrantyType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                WarrantyType pr = new Entities.WarrantyType();

                pr.Id = WarrantyType.Id;
				pr.WarrantyTypeDescription = WarrantyType.WarrantyTypeDescription;
                pr.EntryDateTime = WarrantyType.EntryDateTime;
                pr.EntryUser = WarrantyType.EntryUser;

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
