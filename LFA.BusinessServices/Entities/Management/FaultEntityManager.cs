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
using TAS.DataTransfer.Requests;
using NLog;
using System.Reflection;
using System.Text.RegularExpressions;
using TAS.Services.Common;
using System.Linq.Expressions;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace TAS.Services.Entities.Management
{
    public class FaultEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #region FaultCategory
        public List<FaultCategoryResponseDto> GetAllFaultCategorys()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<FaultCategory>().Select(Fault => new FaultCategoryResponseDto {
                FaultCategoryCode = Fault.FaultCategoryCode,
                FaultCategoryName = Fault.FaultCategoryName,
                Id = Fault.Id,
            }).ToList();
        }

        public FaultCategoryResponseDto GetFaultCategoryById(Guid FaultCategoryId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultCategoryResponseDto pDto = new FaultCategoryResponseDto();

                var query =
                    from FaultCategory in session.Query<FaultCategory>()
                    where FaultCategory.Id == FaultCategoryId
                    select new { FaultCategory = FaultCategory };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().FaultCategory.Id;
                    pDto.FaultCategoryCode = result.First().FaultCategory.FaultCategoryCode;
                    pDto.FaultCategoryName = result.First().FaultCategory.FaultCategoryName;
                    pDto.IsActive = result.First().FaultCategory.IsActive;
                    return pDto;
                }
                else
                {
                    pDto.IsActive = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal bool AddFaultCategory(FaultCategoryRequestDto FaultCategory)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultCategory pr = new Entities.FaultCategory();

                pr.Id = new Guid();
                pr.FaultCategoryCode = FaultCategory.FaultCategoryCode;
                pr.FaultCategoryName = FaultCategory.FaultCategoryName;
                pr.IsActive = true;


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

        internal static object SearchFaultsByCriterias(FaultSearchRequestDto faultSearchData)
        {
            object response = new object();
            try
            {
                if (faultSearchData == null || faultSearchData.searchDetails == null)
                    return response;

                ISession session = EntitySessionManager.GetSession();

                IEnumerable<Guid> faultAreaIds = null, faultCategoryIds = null;
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultAreaCode))
                {
                    faultAreaIds = session.Query<FaultArea>()
                        .Where(a => a.FaultAreaCode.ToLower().Contains(faultSearchData.searchDetails.faultAreaCode.ToLower()))
                        .Select(a => a.Id);
                }

                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultCategoryCode))
                {
                    faultCategoryIds = session.Query<FaultCategory>()
                        .Where(a => a.FaultCategoryCode.ToLower().Contains(faultSearchData.searchDetails.faultCategoryCode.ToLower()))
                        .Select(a => a.Id);
                }
                

                //expression builder for fault
                Expression<Func<Fault, bool>> filterFault = PredicateBuilder.True<Fault>();
                filterFault = filterFault.And(a => a.IsActive);
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultCode))
                    filterFault = filterFault.And(a => a.FaultCode.ToLower().Contains(faultSearchData.searchDetails.faultCode.ToLower()));
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultDescription))
                    filterFault = filterFault.And(a => a.FaultName.ToLower().Contains(faultSearchData.searchDetails.faultDescription.ToLower()));
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultAreaCode))
                    filterFault = filterFault.And(a => faultAreaIds.Any(b => b == a.FaultAreaId));
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultCategoryCode))
                    filterFault = filterFault.And(a => faultCategoryIds.Any(b => b == a.FaultCategoryId));
                if (IsGuid(faultSearchData.searchDetails.categoryCodeId.ToString()))                
                    filterFault = filterFault.And(a => a.FaultCategoryId == faultSearchData.searchDetails.categoryCodeId);
                if (IsGuid(faultSearchData.searchDetails.faultAreaId.ToString()))
                    filterFault = filterFault.And(a => a.FaultAreaId == faultSearchData.searchDetails.faultAreaId);
                if (!string.IsNullOrEmpty(faultSearchData.searchDetails.faultName))
                    filterFault = filterFault.And(a => a.FaultName.ToLower().Contains(faultSearchData.searchDetails.faultName.ToLower()));

                //query
                var searchFaults = session.Query<Fault>().Where(filterFault);
                var filteredDataObject = searchFaults
                    .Skip((faultSearchData.page - 1) * faultSearchData.pageSize)
                    .Take(faultSearchData.pageSize)
                    .Join(session.Query<FaultArea>(), fault => fault.FaultAreaId, area => area.Id, (fault, area) => new { fault, area })
                    .Join(session.Query<FaultCategory>(), fault => fault.fault.FaultCategoryId, category => category.Id, (fault, category) => new { fault, category })
                    .Select(a => new
                    {
                        a.fault.fault.Id,
                        a.fault.fault.FaultCode,
                        a.fault.fault.FaultName,
                        a.fault.area.FaultAreaCode,
                        a.category.FaultCategoryCode
                    })
                    .ToArray();


                var gridDataObject = new CommonGridResponseDto()
                {
                    data = filteredDataObject,
                    totalRecords = searchFaults.Count()
                };
                response = new JavaScriptSerializer().Serialize(gridDataObject);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal bool UpdateFaultCategory(FaultCategoryRequestDto FaultCategory)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultCategory pr = new Entities.FaultCategory();

                pr.Id = FaultCategory.Id;
                pr.FaultCategoryCode = FaultCategory.FaultCategoryCode;
                pr.FaultCategoryName = FaultCategory.FaultCategoryName;
                pr.IsActive = true;

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

        #endregion

        #region FaultArea
        public List<FaultAreaResponseDto> GetAllFaultAreas()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<FaultArea>().Select(Fault => new FaultAreaResponseDto {
                FaultAreaCode = Fault.FaultAreaCode,
                FaultAreaName = Fault.FaultAreaName,
                Id = Fault.Id
            }).ToList();
        }

        public FaultAreaResponseDto GetFaultAreaById(Guid FaultAreaId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultAreaResponseDto pDto = new FaultAreaResponseDto();

                var query =
                    from FaultArea in session.Query<FaultArea>()
                    where FaultArea.Id == FaultAreaId
                    select new { FaultArea = FaultArea };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().FaultArea.Id;
                    pDto.FaultAreaCode = result.First().FaultArea.FaultAreaCode;
                    pDto.FaultAreaName = result.First().FaultArea.FaultAreaName;
                    pDto.IsActive = result.First().FaultArea.IsActive;
                    return pDto;
                }
                else
                {
                    pDto.IsActive = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }

        internal bool AddFaultArea(FaultAreaRequestDto FaultArea)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultArea pr = new Entities.FaultArea();

                pr.Id = new Guid();
                pr.FaultAreaCode = FaultArea.FaultAreaCode;
                pr.FaultAreaName = FaultArea.FaultAreaName;
                pr.IsActive = true;


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

        internal bool UpdateFaultArea(FaultAreaRequestDto FaultArea)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultArea pr = new Entities.FaultArea();

                pr.Id = FaultArea.Id;
                pr.FaultAreaCode = FaultArea.FaultAreaCode;
                pr.FaultAreaName = FaultArea.FaultAreaName;
                pr.IsActive = true;

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

        #endregion

        #region Fault
        public List<FaultResponseDto> GetAllFaults()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Fault>().Select(Fault => new FaultResponseDto {
                FaultCode = Fault.FaultCode,
                FaultName = Fault.FaultName,
                FaultCategoryId = Fault.FaultCategoryId,
                FaultAreaId = Fault.FaultAreaId,
                Id = Fault.Id,
                FaultAreaName = GetFaultAreaById(Fault.FaultAreaId).FaultAreaName,
                FaultCategoryName = GetFaultCategoryById(Fault.FaultCategoryId).FaultCategoryName,
            }).ToList();
        }

        internal static object GetAllCauseOfFailuresByFaultId(Guid faultId)
        {
            object response = new object();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                response = session.Query<FaultCauseOfFailure>()
                    .Where(a => a.FaultId == faultId)
                    .ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }
        public FaultResponseDto GetFaultById(Guid FaultId)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();
                FaultResponseDto pDto = new FaultResponseDto();

                var query =
                    from Fault in session.Query<Fault>()
                    where Fault.Id == FaultId
                    select new { Fault = Fault };

                var CFquery =
                    from FaultCauseOfFailure in session.Query<FaultCauseOfFailure>()
                    where FaultCauseOfFailure.FaultId == FaultId
                    select new { CauseOfFailure = FaultCauseOfFailure.CauseOfFailure };


                var CF = new List<string>();

                var CFresult = CFquery.ToList();

                foreach (var x in CFresult)
                {
                    CF.Add(x.CauseOfFailure);
                }


                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().Fault.Id;
                    pDto.FaultCode = result.First().Fault.FaultCode;
                    pDto.FaultName = result.First().Fault.FaultName;
                    pDto.FaultAreaId = result.First().Fault.FaultAreaId;
                    pDto.FaultCategoryId = result.First().Fault.FaultCategoryId;
                    pDto.IsActive = result.First().Fault.IsActive;
                    pDto.CFailures = CF.ToArray();
                    return pDto;
                }
                else
                {
                    pDto.IsActive = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }

        }

        internal static object ValidateFaultCode(string faultCode, Guid faultCategoryId, Guid faultAreaId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                ISession session = EntitySessionManager.GetSession();
              
                response.code = "ok";

                Fault fault = session.Query<Fault>().Where(a => a.FaultCode == faultCode && a.FaultCategoryId == faultCategoryId && a.FaultAreaId == faultAreaId).FirstOrDefault();

                if(fault == null)
                {
                    response.code = "error";
                    response.msg = "fault code already in database.";
                }
                else
                {
                    response.code = "ok";
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }

        internal bool AddFault(FaultRequestDto Fault)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Fault pr = new Entities.Fault();

                pr.Id = new Guid();
                pr.FaultCode = Fault.FaultCode;
                pr.FaultName = Fault.FaultName;
                pr.FaultAreaId = Fault.FaultAreaId;
                pr.FaultCategoryId = Fault.FaultCategoryId;
                pr.IsActive = true;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }
                AddCauseOfFailure(Fault, pr.Id);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal void AddCauseOfFailure(FaultRequestDto Fault, Guid ID)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();


                foreach (string i in Fault.CFailures)
                {
                    FaultCauseOfFailure cf = new Entities.FaultCauseOfFailure();

                    cf.FaultId = ID;
                    cf.CauseOfFailure = i;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(cf);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

        }

        internal bool UpdateFault(FaultRequestDto Fault)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Fault pr = new Entities.Fault();

                pr.Id = Fault.Id;
                pr.FaultCode = Fault.FaultCode;
                pr.FaultName = Fault.FaultName;
                pr.FaultAreaId = Fault.FaultAreaId;
                pr.FaultCategoryId = Fault.FaultCategoryId;
                pr.IsActive = true;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);
                    transaction.Commit();
                }
                UpdateCauseOfFailure(Fault);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }
        }

        internal void UpdateCauseOfFailure(FaultRequestDto Fault)
        {

            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<FaultCauseOfFailure> LCF = session.Query<FaultCauseOfFailure>()
                            .Where(a => a.FaultId == Fault.Id).ToList();
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (FaultCauseOfFailure item in LCF)
                    {
                        session.Delete(item);
                    }
                    transaction.Commit();
                }

                foreach (string i in Fault.CFailures)
                {
                    FaultCauseOfFailure cfn = new Entities.FaultCauseOfFailure();

                    cfn.FaultId = Fault.Id;
                    cfn.CauseOfFailure = i;

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(cfn);
                        transaction.Commit();
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

        }

        internal static List<FaultCauseOfFailureDto> GetAllFaultCauseOfFailures()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<FaultCauseOfFailure>().Select(FaultCauseOfFailure => new FaultCauseOfFailureDto {
                CauseOfFailure = FaultCauseOfFailure.CauseOfFailure,
                FaultId = FaultCauseOfFailure.FaultId,
                Id = FaultCauseOfFailure.Id
            }).ToList();
        }
        #endregion

        internal static object SearchFaultsGridPagine(FaultSearchRequestDto faultSearchData)
        {
            object response = new object();
            try
            {

                ISession session = EntitySessionManager.GetSession();
                //query
                var searchFaults = session.Query<Fault>();
                var filteredDataObject = searchFaults
                    .Skip((faultSearchData.page - 1) * faultSearchData.pageSize)
                    .Take(faultSearchData.pageSize)
                    .Join(session.Query<FaultArea>(), fault => fault.FaultAreaId, area => area.Id, (fault, area) => new { fault, area })
                    .Join(session.Query<FaultCategory>(), fault => fault.fault.FaultCategoryId, category => category.Id, (fault, category) => new { fault, category })
                    .Select(a => new
                    {
                        Id = a.fault.fault.Id,
                        FaultCode = a.fault.fault.FaultCode,
                        FaultName = a.fault.fault.FaultName,
                        FaultAreaCode = a.fault.area.FaultAreaName,
                        FaultCategoryCode = a.category.FaultCategoryName
                    })
                    .ToArray();


                var gridDataObject = new CommonGridResponseDto()
                {
                    data = filteredDataObject,
                    totalRecords = searchFaults.Count()
                };
                response = new JavaScriptSerializer().Serialize(gridDataObject);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
                return false;
            Regex isGuid = new Regex(@"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$", RegexOptions.Compiled);

            if (candidate != null)
            {
                if (isGuid.IsMatch(candidate))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
