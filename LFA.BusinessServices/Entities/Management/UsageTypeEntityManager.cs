using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Criterion;
using NHibernate.Linq;
using TAS.Services.Entities.Persistence;
using TAS.DataTransfer.Responses;

namespace TAS.Services.Entities.Management
{
    public class UsageTypeEntityManager
    {
        public List<UsageTypeResponseDto> GetAllUsageTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<UsageType>().Select(s => new UsageTypeResponseDto
            {
                Id = s.Id,
                UsageTypeName = s.UsageTypeName,
                UsageTypeCode = s.UsageTypeCode
            }).ToList();
        }
    }
}
