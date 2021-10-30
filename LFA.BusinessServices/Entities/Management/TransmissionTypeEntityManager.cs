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
    public class TransmissionTypeEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<TransmissionTechnology> GetTransmissionTechnologies()
        {
            List<TransmissionTechnology> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<TransmissionTechnology> TransmissionTypeData = session.Query<TransmissionTechnology>();
            entities = TransmissionTypeData.ToList();
            return entities;
        }
        public List<TransmissionTypeResponseDto> GetTransmissionTypes()
        {
            List<TransmissionTypeResponseDto> transmissionTypeResponseDto = new List<TransmissionTypeResponseDto>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                IQueryable<TransmissionType> TransmissionTypeData = session.Query<TransmissionType>();
                IQueryable<TransmissionTechnology> TransmissionTechData = session.Query<TransmissionTechnology>();
                foreach (var item in TransmissionTypeData)
                {
                    List<string> techList = new List<string>();
                    var query =
                    from TransmissionTechnologyMap in session.Query<TransmissionTechnologyMap>()
                    where TransmissionTechnologyMap.TransmissionId == item.Id
                    select new { TransmissionTechnologyMap = TransmissionTechnologyMap };
                    foreach (var tec in query.ToList())
                    {
                        var query2 =
                  from TransmissionTechnology in session.Query<TransmissionTechnology>()
                  where TransmissionTechnology.Id == tec.TransmissionTechnologyMap.TransmissionTechnologyId
                  select new { TransmissionTechnology = TransmissionTechnology };
                        techList.Add(query2.ToList()[0].TransmissionTechnology.Name);
                    }
                    item.Technology = techList;

                    TransmissionTypeResponseDto transmissionTypeResponse = new TransmissionTypeResponseDto();
                    transmissionTypeResponse.TransmissionTypeCode = item.TransmissionTypeCode;
                    transmissionTypeResponse.Technology = item.Technology;
                    transmissionTypeResponse.Id = item.Id;
                    transmissionTypeResponse.EntryDateTime = item.EntryDateTime;
                    transmissionTypeResponse.EntryUser = item.EntryUser;
                    transmissionTypeResponseDto.Add(transmissionTypeResponse);
                }


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return transmissionTypeResponseDto;
        }

        public TransmissionTypeResponseDto GetTransmissionTypeById(Guid TransmissionTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                TransmissionTypeResponseDto pDto = new TransmissionTypeResponseDto();

                var query =
                    from TransmissionType in session.Query<TransmissionType>()
                    where TransmissionType.Id == TransmissionTypeId
                    select new { TransmissionType = TransmissionType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {

                    pDto.Id = result.First().TransmissionType.Id;
                    pDto.TransmissionTypeCode = result.First().TransmissionType.TransmissionTypeCode;
                    pDto.Technology = result.First().TransmissionType.Technology;
                    pDto.EntryDateTime = result.First().TransmissionType.EntryDateTime;
                    pDto.EntryUser = result.First().TransmissionType.EntryUser;

                    List<string> techList = new List<string>();
                    var query0 =
                    from TransmissionTechnologyMap in session.Query<TransmissionTechnologyMap>()
                    where TransmissionTechnologyMap.TransmissionId == pDto.Id
                    select new { TransmissionTechnologyMap = TransmissionTechnologyMap };
                    foreach (var tec in query0.ToList())
                    {
                        var query2 =
                          from TransmissionTechnology in session.Query<TransmissionTechnology>()
                          where TransmissionTechnology.Id == tec.TransmissionTechnologyMap.TransmissionTechnologyId
                          select new { TransmissionTechnology = TransmissionTechnology };
                        techList.Add(query2.ToList()[0].TransmissionTechnology.Name);
                    }
                    pDto.Technology = techList;

                    pDto.IsTransmissionTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsTransmissionTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddTransmissionType(TransmissionTypeRequestDto TransmissionType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                TransmissionType pr = new Entities.TransmissionType();

                pr.Id = new Guid();
                pr.TransmissionTypeCode = TransmissionType.TransmissionTypeCode;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    foreach (var item in TransmissionType.Technology)
                    {
                        TransmissionTechnologyMap prr = new Entities.TransmissionTechnologyMap();

                        var query2 =
                        from TransmissionTechnology in session.Query<TransmissionTechnology>()
                        where TransmissionTechnology.Id == Guid.Parse(item)
                        select new { TransmissionTechnology = TransmissionTechnology };

                        prr.Id = new Guid();
                        prr.TransmissionId = pr.Id;
                        prr.TransmissionTechnologyId = query2.ToList()[0].TransmissionTechnology.Id;
                        session.SaveOrUpdate(prr);
                    }
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

        internal bool UpdateTransmissionType(TransmissionTypeRequestDto TransmissionType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TransmissionType pr = new Entities.TransmissionType();

                pr.Id = TransmissionType.Id;
                pr.TransmissionTypeCode = TransmissionType.TransmissionTypeCode;
                pr.EntryDateTime = TransmissionType.EntryDateTime;
                pr.EntryUser = TransmissionType.EntryUser;

                IEnumerable<TransmissionTechnologyMap> techMap = session.Query<TransmissionTechnologyMap>()
                    .Where(a => a.TransmissionId == TransmissionType.Id);
                foreach (TransmissionTechnologyMap map in techMap)
                {
                    session.Delete(map);
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);


                    foreach (var item in TransmissionType.Technology)
                    {
                        TransmissionTechnologyMap prr = new Entities.TransmissionTechnologyMap();
                        //var query2 =
                        //from TransmissionTechnology in session.Query<TransmissionTechnology>()
                        //where TransmissionTechnology.Id == Guid.Parse(item)
                        //select new { TransmissionTechnology = TransmissionTechnology };
                        try
                        {
                            prr.TransmissionTechnologyId = Guid.Parse(item);
                        }
                        catch (Exception e) {
                            TransmissionTechnology transmissionTechnology = session.Query<TransmissionTechnology>().Where(a => a.Name.ToLower().Equals(item.ToLower())).FirstOrDefault();
                            prr.TransmissionTechnologyId = transmissionTechnology.Id;

                        }



                        prr.Id = new Guid();
                        prr.TransmissionId = pr.Id;


                        session.SaveOrUpdate(prr);
                    }
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
