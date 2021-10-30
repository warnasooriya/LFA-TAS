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
    public class RSAAnualPremiumEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<RSAAnualPremium> GetRSAAnualPremiums()
        {
            List<RSAAnualPremium> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<RSAAnualPremium> RSAAnualPremiumData = session.Query<RSAAnualPremium>();
            entities = RSAAnualPremiumData.ToList();
            return entities;
        }

        public RSAAnualPremiumResponseDto GetRSAAnualPremiumById(Guid RSAAnualPremiumId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                RSAAnualPremiumResponseDto pDto = new RSAAnualPremiumResponseDto();

                var query =
                    from RSAAnualPremium in session.Query<RSAAnualPremium>()
                    where RSAAnualPremium.Id == RSAAnualPremiumId
                    select new { RSAAnualPremium = RSAAnualPremium };
                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().RSAAnualPremium.Id;
                    pDto.ContractExtensionId = result.First().RSAAnualPremium.ContractExtensionId;
                    pDto.Year = result.First().RSAAnualPremium.Year;
                    pDto.Value = result.First().RSAAnualPremium.Value;
                    pDto.IsRSAAnualPremiumExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsRSAAnualPremiumExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
            
        }

        internal bool AddRSAAnualPremium(RSAAnualPremiumRequestDto RSAAnualPremium)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                RSAAnualPremium pr = new Entities.RSAAnualPremium();

                pr.Id = new Guid();
                pr.ContractExtensionId = RSAAnualPremium.ContractExtensionId;
                pr.Year = RSAAnualPremium.Year;
                pr.Value = RSAAnualPremium.Value;

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

        internal bool UpdateRSAAnualPremium(RSAAnualPremiumRequestDto RSAAnualPremium)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                RSAAnualPremium pr = new Entities.RSAAnualPremium();

                pr.Id = RSAAnualPremium.Id;
                pr.ContractExtensionId = RSAAnualPremium.ContractExtensionId;
                pr.Year = RSAAnualPremium.Year;
                pr.Value = RSAAnualPremium.Value;

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
