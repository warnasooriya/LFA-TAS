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
    public class ItemStatusEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ItemStatusResponseDto> GetItemStatuss()
        {
            ISession session = EntitySessionManager.GetSession();
            return   session.Query<ItemStatus>().Select(s => new ItemStatusResponseDto
            {
                Id = s.Id,
                Status = s.Status,
                ItemStatusDescription = s.ItemStatusDescription,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();
        }

        public ItemStatusResponseDto GetItemStatusById(Guid ItemStatusId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ItemStatusResponseDto pDto = new ItemStatusResponseDto();

                var query =
                    from ItemStatus in session.Query<ItemStatus>()
                    where ItemStatus.Id == ItemStatusId
                    select new { ItemStatus = ItemStatus };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ItemStatus.Id;
                    pDto.Status = result.First().ItemStatus.Status;
                    pDto.ItemStatusDescription = result.First().ItemStatus.ItemStatusDescription;
                    pDto.EntryDateTime = result.First().ItemStatus.EntryDateTime;
                    pDto.EntryUser = result.First().ItemStatus.EntryUser;

                    pDto.IsItemStatusExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsItemStatusExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddItemStatus(ItemStatusRequestDto ItemStatus)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                ItemStatus pr = new Entities.ItemStatus();

                pr.Id = new Guid();
                pr.Status = ItemStatus.Status;
				 pr.ItemStatusDescription= ItemStatus.ItemStatusDescription;
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

        internal bool UpdateItemStatus(ItemStatusRequestDto ItemStatus)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ItemStatus pr = new Entities.ItemStatus();

                pr.Id = ItemStatus.Id;
                pr.Status = ItemStatus.Status;
				pr.ItemStatusDescription = ItemStatus.ItemStatusDescription;
                pr.EntryDateTime = ItemStatus.EntryDateTime;
                pr.EntryUser = ItemStatus.EntryUser;

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

        internal int GetExsistingItemStatusByStatus(Guid Id, string Status)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from ItemStatus in session.Query<ItemStatus>()
                    where ItemStatus.Status == Status
                    select new { ItemStatus = ItemStatus };

                return query.Count();
            }
            else
            {
                var query =
                    from ItemStatus in session.Query<ItemStatus>()
                    where ItemStatus.Id != Id && ItemStatus.Status == Status
                    select new { ItemStatus = ItemStatus };

                return query.Count();
            }
        }

        internal int GetExsistingItemStatusByDescription(Guid Id, string ItemStatusDescription)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from ItemStatus in session.Query<ItemStatus>()
                    where ItemStatus.ItemStatusDescription == ItemStatusDescription
                    select new { ItemStatus = ItemStatus };

                return query.Count();
            }
            else
            {
                var query =
                    from ItemStatus in session.Query<ItemStatus>()
                    where ItemStatus.Id != Id && ItemStatus.ItemStatusDescription == ItemStatusDescription
                    select new { ItemStatusDescription = ItemStatusDescription };

                return query.Count();
            }
        }
    }
}
