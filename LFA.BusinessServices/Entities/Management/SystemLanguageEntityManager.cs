using NLog;
using NHibernate;
using System;
using System.Reflection;
using System.Linq;
using NHibernate.Linq;
using TAS.DataTransfer.Responses;
using System.Collections.Generic;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class SystemLanguageEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public List<SystemLanguageResponseDto> GetSystemLanguages()
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<SystemLanguage> data = session.Query<SystemLanguage>();
                return data.Select(a => new SystemLanguageResponseDto { Id = a.Id, Language = a.Language, LanguageCode = a.LanguageCode }).ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }
    }
}
