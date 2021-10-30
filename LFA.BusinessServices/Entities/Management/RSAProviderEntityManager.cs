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
    public class RSAProviderEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<RSAProviderResponseDto> GetRSAProviders()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<RSAProvider>().Select(RSAProvider => new RSAProviderResponseDto
            {
                Id = RSAProvider.Id,
                ProviderCode = RSAProvider.ProviderCode,
                ProviderName = RSAProvider.ProviderName
            }).ToList();
        }

        public RSAProviderResponseDto GetRSAProviderById(Guid RSAProviderId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                RSAProviderResponseDto pDto = new RSAProviderResponseDto();

                var query =
                    from RSAProvider in session.Query<RSAProvider>()
                    where RSAProvider.Id == RSAProviderId
                    select new { RSAProvider = RSAProvider };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().RSAProvider.Id;
                    pDto.ProviderName = result.First().RSAProvider.ProviderName;
                    pDto.ProviderCode = result.First().RSAProvider.ProviderCode;

                    pDto.IsRSAProviderExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsRSAProviderExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddRSAProvider(RSAProviderRequestDto RSAProvider)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                RSAProvider pr = new Entities.RSAProvider();

                pr.Id = new Guid();
                pr.ProviderCode = RSAProvider.ProviderCode;
				 pr.ProviderName= RSAProvider.ProviderName;

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

        internal bool UpdateRSAProvider(RSAProviderRequestDto RSAProvider)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                RSAProvider pr = new Entities.RSAProvider();

                pr.Id = RSAProvider.Id;
                pr.ProviderCode = RSAProvider.ProviderCode;
                pr.ProviderName = RSAProvider.ProviderName;

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
