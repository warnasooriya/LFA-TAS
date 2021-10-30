
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
    public class NRPCommissionTypesEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<NRPCommissionTypesResponseDto> GetNRPCommissionTypess()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<NRPCommissionTypes>().Select(NRPCommissionTypes => new NRPCommissionTypesResponseDto
            {
                Id = NRPCommissionTypes.Id,
                Name = NRPCommissionTypes.Name,
                IsActive = NRPCommissionTypes.IsActive,
                IsForTPA = NRPCommissionTypes.IsForTPA
            }).ToList();
        }

        public NRPCommissionTypesResponseDto GetNRPCommissionTypesById(Guid NRPCommissionTypesId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                NRPCommissionTypesResponseDto pDto = new NRPCommissionTypesResponseDto();

                var query =
                    from NRPCommissionTypes in session.Query<NRPCommissionTypes>()
                    where NRPCommissionTypes.Id == NRPCommissionTypesId
                    select new { NRPCommissionTypes = NRPCommissionTypes };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().NRPCommissionTypes.Id;
                    pDto.IsActive = result.First().NRPCommissionTypes.IsActive;
                    pDto.IsForTPA = result.First().NRPCommissionTypes.IsForTPA;
                    pDto.Name = result.First().NRPCommissionTypes.Name;

                    pDto.IsNRPCommissionTypesExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsNRPCommissionTypesExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        internal bool AddNRPCommissionTypes(NRPCommissionTypesRequestDto NRPCommissionTypes)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                NRPCommissionTypes pr = new Entities.NRPCommissionTypes();

                pr.Id = new Guid();
                pr.Name = NRPCommissionTypes.Name;
                pr.IsActive = NRPCommissionTypes.IsActive;
                pr.IsForTPA = NRPCommissionTypes.IsForTPA;

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

        internal bool UpdateNRPCommissionTypes(NRPCommissionTypesRequestDto NRPCommissionTypes)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                NRPCommissionTypes pr = new Entities.NRPCommissionTypes();

                pr.Id = NRPCommissionTypes.Id;
                pr.Name = NRPCommissionTypes.Name;
                pr.IsActive = NRPCommissionTypes.IsActive;
                pr.IsForTPA = NRPCommissionTypes.IsForTPA;

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

        public List<NRPCommissionContractMapping> GetNRPCommissionContractMappings()
        {
            List<NRPCommissionContractMapping> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<NRPCommissionContractMapping> NRPCommissionContractMappingData = session.Query<NRPCommissionContractMapping>();
            entities = NRPCommissionContractMappingData.ToList();
            return entities;
        }

        public NRPCommissionContractMappingResponseDto GetNRPCommissionContractMappingById(Guid NRPCommissionContractMappingId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                NRPCommissionContractMappingResponseDto pDto = new NRPCommissionContractMappingResponseDto();

                var query =
                    from NRPCommissionContractMapping in session.Query<NRPCommissionContractMapping>()
                    where NRPCommissionContractMapping.Id == NRPCommissionContractMappingId
                    select new { NRPCommissionContractMapping = NRPCommissionContractMapping };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().NRPCommissionContractMapping.Id;
                    pDto.Commission = result.First().NRPCommissionContractMapping.Commission;
                    pDto.ContractId = result.First().NRPCommissionContractMapping.ContractId;
                    pDto.IsPercentage = result.First().NRPCommissionContractMapping.IsPercentage;
                    pDto.NRPCommissionId = result.First().NRPCommissionContractMapping.NRPCommissionId;
                    pDto.IsOnNRP = result.First().NRPCommissionContractMapping.IsOnNRP;
                    pDto.IsOnGROSS = result.First().NRPCommissionContractMapping.IsOnGROSS;

                    pDto.IsNRPCommissionContractMappingExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsNRPCommissionContractMappingExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                NRPCommissionContractMapping pr = new Entities.NRPCommissionContractMapping();

                pr.Id = new Guid();
                pr.Commission = NRPCommissionContractMapping.Commission;
                pr.ContractId = NRPCommissionContractMapping.ContractId;
                pr.IsPercentage = NRPCommissionContractMapping.IsPercentage;
                pr.NRPCommissionId = NRPCommissionContractMapping.NRPCommissionId;
                pr.IsOnNRP = NRPCommissionContractMapping.IsOnNRP;
                pr.IsOnGROSS = NRPCommissionContractMapping.IsOnGROSS;

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

        internal bool UpdateNRPCommissionContractMapping(NRPCommissionContractMappingRequestDto NRPCommissionContractMapping)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                NRPCommissionContractMapping pr = new Entities.NRPCommissionContractMapping();

                pr.Id = NRPCommissionContractMapping.Id;
                pr.Commission = NRPCommissionContractMapping.Commission;
                pr.ContractId = NRPCommissionContractMapping.ContractId;
                pr.IsPercentage = NRPCommissionContractMapping.IsPercentage;
                pr.NRPCommissionId = NRPCommissionContractMapping.NRPCommissionId;
                pr.IsOnNRP = NRPCommissionContractMapping.IsOnNRP;
                pr.IsOnGROSS = NRPCommissionContractMapping.IsOnGROSS;

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
