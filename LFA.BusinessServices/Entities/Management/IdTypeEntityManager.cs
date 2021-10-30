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
    public class IdTypeEntityManager
    {
        public List<IdTypeResponseDto> GetAllIdTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return  session.Query<IdType>().Select(s => new IdTypeResponseDto { Id = s.Id, IdTypeDescription = s.IdTypeDescription, IdTypeName = s.IdTypeName }).ToList();

        }
    }
}
