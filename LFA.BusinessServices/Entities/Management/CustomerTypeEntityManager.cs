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
    public class CustomerTypeEntityManager
    {
        public List<CustomerTypeResponseDto> GetAllCustomerTypes()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<CustomerType>().Select(s => new CustomerTypeResponseDto
            {
                Id = s.Id,
                CustomerTypeName = s.CustomerTypeName,
                CustomerTypeDescription = s.CustomerTypeDescription
            }).ToList();

        }
    }
}
