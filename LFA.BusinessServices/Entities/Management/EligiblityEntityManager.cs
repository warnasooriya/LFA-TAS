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
    public class EligibilityEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Eligibility> GetEligibilitys()
        {
            List<Eligibility> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Eligibility> EligibilityData = session.Query<Eligibility>();
            entities = EligibilityData.ToList();
            return entities;
        }

        public EligibilityResponseDto GetEligibilityById(Guid EligibilityId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                EligibilityResponseDto pDto = new EligibilityResponseDto();

                var query =
                    from Eligibility in session.Query<Eligibility>()
                    where Eligibility.Id == EligibilityId
                    select new { Eligibility = Eligibility };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Eligibility.Id;
                    pDto.ContractId = result.First().Eligibility.ContractId;
                    pDto.AgeTo = result.First().Eligibility.AgeTo;
                    pDto.AgeFrom = result.First().Eligibility.AgeFrom;
                    pDto.MileageTo = result.First().Eligibility.MileageTo;
                    pDto.MileageFrom = result.First().Eligibility.MileageFrom;
                    pDto.MonthsFrom = result.First().Eligibility.MonthsFrom;
                    pDto.MonthsTo = result.First().Eligibility.MonthsTo;

                    pDto.PlusMinus = result.First().Eligibility.PlusMinus;
                    pDto.Premium = result.First().Eligibility.Premium;
                    pDto.IsPercentage = result.First().Eligibility.IsPercentage;
                    pDto.EntryDateTime = result.First().Eligibility.EntryDateTime;
                    pDto.EntryUser = result.First().Eligibility.EntryUser;

                    pDto.IsEligibilityExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsEligibilityExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }            
        }

        internal bool AddEligibility(EligibilityRequestDto Eligibility)
        {
            try
            {
                
                ISession session = EntitySessionManager.GetSession();
                Eligibility pr = new Entities.Eligibility();

                pr.Id = new Guid();
                pr.ContractId = Eligibility.ContractId;
                pr.AgeFrom = Eligibility.AgeFrom;
                pr.AgeTo = Eligibility.AgeTo;
                pr.MileageFrom = Eligibility.MileageFrom;
                pr.MileageTo = Eligibility.MileageTo;
                pr.MonthsFrom = Eligibility.MonthsFrom;
                pr.MonthsTo = Eligibility.MonthsTo;

                pr.PlusMinus = Eligibility.PlusMinus;
                pr.Premium = Eligibility.Premium;
                pr.IsPercentage = Eligibility.IsPercentage;
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

        internal bool UpdateEligibility(EligibilityRequestDto Eligibility)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Eligibility pr = new Entities.Eligibility();

                pr.Id = Eligibility.Id;
                pr.ContractId = Eligibility.ContractId;
                pr.AgeFrom = Eligibility.AgeFrom;
                pr.AgeTo = Eligibility.AgeTo;
                pr.MileageTo = Eligibility.MileageTo;
                pr.MileageFrom = Eligibility.MileageFrom;
                pr.MonthsFrom = Eligibility.MonthsFrom;
                pr.MonthsTo = Eligibility.MonthsTo;

                pr.PlusMinus = Eligibility.PlusMinus;
                pr.Premium = Eligibility.Premium;
                pr.IsPercentage = Eligibility.IsPercentage;
                pr.EntryDateTime = Eligibility.EntryDateTime;
                pr.EntryUser = Eligibility.EntryUser;

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
