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
    public class PremiumBasedOnEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<PremiumBasedOnResponseDto> GetPremiumBasedOns()
        {
             ISession session = EntitySessionManager.GetSession();
            return session.Query<PremiumBasedOn>().Select(PremiumBasedOn => new PremiumBasedOnResponseDto {
                Id = PremiumBasedOn.Id,
                Code = PremiumBasedOn.Code,
                Description = PremiumBasedOn.Description,
                EntryDateTime = PremiumBasedOn.EntryDateTime,
                EntryUser = PremiumBasedOn.EntryUser
            }).ToList();
        }


        public PremiumBasedOnResponseDto GetPremiumBasedOnById(Guid PremiumBasedOnId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                PremiumBasedOnResponseDto pDto = new PremiumBasedOnResponseDto();

                var query =
                    from PremiumBasedOn in session.Query<PremiumBasedOn>()
                    where PremiumBasedOn.Id == PremiumBasedOnId
                    select new { PremiumBasedOn = PremiumBasedOn };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().PremiumBasedOn.Id;
                    pDto.Code = result.First().PremiumBasedOn.Code;
                    pDto.Description = result.First().PremiumBasedOn.Description;
                    pDto.EntryDateTime = result.First().PremiumBasedOn.EntryDateTime;
                    pDto.EntryUser = result.First().PremiumBasedOn.EntryUser;

                    pDto.IsPremiumBasedOnExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsPremiumBasedOnExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal bool AddPremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                PremiumBasedOn pr = new Entities.PremiumBasedOn();

                pr.Id = new Guid();
                pr.Code = PremiumBasedOn.Code;
				 pr.Description= PremiumBasedOn.Description;
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

        internal bool UpdatePremiumBasedOn(PremiumBasedOnRequestDto PremiumBasedOn)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PremiumBasedOn pr = new Entities.PremiumBasedOn();

                pr.Id = PremiumBasedOn.Id;
                pr.Code = PremiumBasedOn.Code;
                pr.Description = PremiumBasedOn.Description;
                pr.EntryDateTime = PremiumBasedOn.EntryDateTime;
                pr.EntryUser = PremiumBasedOn.EntryUser;

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
