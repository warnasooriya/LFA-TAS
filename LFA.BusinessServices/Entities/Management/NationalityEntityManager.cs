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
    public class NationalityEntityManager
    {
        public List<NationalityResponseDto> GetAllNationalities()
        {
            List<Nationality> entities = null;
            ISession session = EntitySessionManager.GetSession();
            List<NationalityResponseDto> NationalityResponseDtoList = session.Query<Nationality>().Select(nationality => new NationalityResponseDto
            {
                Id= nationality.Id,
                NationalityName = nationality.NationalityName
        }).OrderBy(n => n.NationalityName).ToList();
            return NationalityResponseDtoList;
        }
    }
}
