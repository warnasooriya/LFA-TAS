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
using System.Reflection;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class ReinsurerContractEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<ReinsurerContract> GetReinsurerContracts()
        {
            List<ReinsurerContract> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ReinsurerContract> ReinsurerContractData = session.Query<ReinsurerContract>();
            entities = ReinsurerContractData.ToList();
            return entities;
        }

        public ReinsurerContractResponseDto GetReinsurerContractById(Guid ReinsurerContractId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ReinsurerContractResponseDto pDto = new ReinsurerContractResponseDto();

                var query =
                    from ReinsurerContract in session.Query<ReinsurerContract>()
                    where ReinsurerContract.Id == ReinsurerContractId
                    select new { ReinsurerContract = ReinsurerContract };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ReinsurerContract.Id;
                    pDto.InsurerId = result.First().ReinsurerContract.InsurerId;
                    pDto.LinkContractId = result.First().ReinsurerContract.LinkContractId;
                    pDto.ReinsurerId = result.First().ReinsurerContract.ReinsurerId;
                    pDto.CommodityTypeId = result.First().ReinsurerContract.CommodityTypeId;
                    pDto.ContractNo = result.First().ReinsurerContract.ContractNo;
                    pDto.CountryId = result.First().ReinsurerContract.CountryId;
                    pDto.FromDate = result.First().ReinsurerContract.FromDate;
                    pDto.ToDate = result.First().ReinsurerContract.ToDate;
                    pDto.UWYear = result.First().ReinsurerContract.UWYear;
                    pDto.EntryDateTime = result.First().ReinsurerContract.EntryDateTime;
                    pDto.EntryUser = result.First().ReinsurerContract.EntryUser;
                    pDto.IsActive = result.First().ReinsurerContract.IsActive;
                    pDto.BrokerId = result.First().ReinsurerContract.BrokerId;
                    pDto.IsReinsurerContractExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsReinsurerContractExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }            
        }

        internal bool AddReinsurerContract(ReinsurerContractRequestDto ReinsurerContract)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ReinsurerContract pr = new Entities.ReinsurerContract();

                ReinsurerContract ReinsurerContractResult = session
                    .Query<ReinsurerContract>().FirstOrDefault(a => a.ContractNo == ReinsurerContract.ContractNo);

                if (ReinsurerContractResult==null)
                {

                    pr.Id = new Guid();
                    pr.ReinsurerId = ReinsurerContract.ReinsurerId;
                    pr.LinkContractId = ReinsurerContract.LinkContractId;
                    pr.InsurerId = ReinsurerContract.InsurerId;
                    pr.CommodityTypeId = ReinsurerContract.CommodityTypeId;
                    pr.ContractNo = ReinsurerContract.ContractNo;
                    pr.CountryId = ReinsurerContract.CountryId;
                    pr.FromDate = ReinsurerContract.FromDate;
                    pr.ToDate = ReinsurerContract.ToDate;
                    pr.UWYear = ReinsurerContract.UWYear;
                    pr.IsActive = ReinsurerContract.IsActive;
                    pr.BrokerId = ReinsurerContract.BrokerId;
                    pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                    pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(pr);
                        transaction.Commit();
                    }
                    return true;
                }
                else {

                    return false;

                }
                
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal bool UpdateReinsurerContract(ReinsurerContractRequestDto ReinsurerContract)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ReinsurerContract pr = new Entities.ReinsurerContract();

                pr.Id = ReinsurerContract.Id;
                pr.ReinsurerId = ReinsurerContract.ReinsurerId;
                pr.LinkContractId = ReinsurerContract.LinkContractId;
                pr.InsurerId = ReinsurerContract.InsurerId;
                pr.CommodityTypeId = ReinsurerContract.CommodityTypeId;
                pr.ContractNo = ReinsurerContract.ContractNo;
                pr.CountryId = ReinsurerContract.CountryId;
                pr.FromDate = ReinsurerContract.FromDate;
                pr.ToDate = ReinsurerContract.ToDate;
                pr.UWYear = ReinsurerContract.UWYear;
                pr.IsActive = ReinsurerContract.IsActive;
                pr.BrokerId = ReinsurerContract.BrokerId;
                pr.EntryDateTime = ReinsurerContract.EntryDateTime;
                pr.EntryUser = ReinsurerContract.EntryUser;

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

        public ReinsurerContractResponseDto GetReinsurerContractByContractId(Guid ContractId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ReinsurerContractResponseDto pDto = new ReinsurerContractResponseDto();

                var contarct =
                    from Contract in session.Query<Contract>()
                    where Contract.Id == ContractId
                    select new { Contract = Contract };

                var CC = contarct.ToList();

                var query =
                    from ReinsurerContract in session.Query<ReinsurerContract>()
                    where ReinsurerContract.Id == CC.FirstOrDefault().Contract.ReinsurerContractId
                    select new { ReinsurerContract = ReinsurerContract };

                var result = query.ToList();

                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ReinsurerContract.Id;
                    pDto.InsurerId = result.First().ReinsurerContract.InsurerId;
                    pDto.LinkContractId = result.First().ReinsurerContract.LinkContractId;
                    pDto.ReinsurerId = result.First().ReinsurerContract.ReinsurerId;
                    pDto.CommodityTypeId = result.First().ReinsurerContract.CommodityTypeId;
                    pDto.ContractNo = result.First().ReinsurerContract.ContractNo;
                    pDto.CountryId = result.First().ReinsurerContract.CountryId;
                    pDto.FromDate = result.First().ReinsurerContract.FromDate;
                    pDto.ToDate = result.First().ReinsurerContract.ToDate;
                    pDto.UWYear = result.First().ReinsurerContract.UWYear;
                    pDto.EntryDateTime = result.First().ReinsurerContract.EntryDateTime;
                    pDto.EntryUser = result.First().ReinsurerContract.EntryUser;
                    pDto.IsActive = result.First().ReinsurerContract.IsActive;
                    pDto.IsReinsurerContractExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsReinsurerContractExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }  
        }
    }
}
