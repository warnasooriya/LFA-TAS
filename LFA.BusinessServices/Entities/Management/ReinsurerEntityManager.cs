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
using System.Web.Script.Serialization;

namespace TAS.Services.Entities.Management
{
    public class ReinsurerEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Reinsurer> GetReinsurers()
        {
            List<Reinsurer> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Reinsurer> ReinsurerData = session.Query<Reinsurer>();
            entities = ReinsurerData.ToList();
            return entities;
        }
        public ReinsurerResponseDto GetReinsurerById(Guid ReinsurerId)
        {
            ISession session = EntitySessionManager.GetSession();

            ReinsurerResponseDto pDto = new ReinsurerResponseDto();

            var query =
                from Reinsurer in session.Query<Reinsurer>()
                where Reinsurer.Id == ReinsurerId
                select new { Reinsurer = Reinsurer };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Reinsurer.Id;
                pDto.IsActive = result.First().Reinsurer.IsActive;
                pDto.ReinsurerCode = result.First().Reinsurer.ReinsurerCode;
                pDto.ReinsurerName = result.First().Reinsurer.ReinsurerName;
                pDto.EntryDateTime = result.First().Reinsurer.EntryDateTime;
                pDto.EntryUser = result.First().Reinsurer.EntryUser;
                pDto.CurrencyId = result.First().Reinsurer.CurrencyId;

                pDto.IsReinsurerExists = true;
                return pDto;
            }
            else
            {
                pDto.IsReinsurerExists = false;
                return pDto;
            }
        }
        internal bool AddReinsurer(ReinsurerRequestDto Reinsurer)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Reinsurer pr = new Entities.Reinsurer();

                pr.Id = new Guid();
                pr.IsActive = Reinsurer.IsActive;
                pr.ReinsurerCode = Reinsurer.ReinsurerCode;
                pr.ReinsurerName = Reinsurer.ReinsurerName;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");
                pr.CurrencyId = Reinsurer.CurrencyId;

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

        internal bool AddorUpdateReinsurerConsortiums(List<ReinsurerConsortiumRequestDto> reinsurerList)
        {
            bool status = false;
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    var deletebleDataList = session.Query<ReinsurerConsortium>().Where(a => a.ParentReinsurerId == reinsurerList.FirstOrDefault().ParentReinsurerId).ToList();
                    foreach (var delObj in deletebleDataList)
                    {
                        session.Delete(delObj);
                    }
                    session.Flush();
                    foreach (var reinsurer in reinsurerList)
                    {
                        ReinsurerConsortium pr = new Entities.ReinsurerConsortium();
                        pr.Id = Guid.NewGuid();
                        pr.ParentReinsurerId = reinsurer.ParentReinsurerId;
                        pr.ReinsurerId = reinsurer.ReinsurerId;
                        pr.NRPPercentage = reinsurer.NRPPercentage;
                        pr.ProfitSharePercentage = reinsurer.ProfitSharePercentage;
                        pr.RiskSharePercentage = reinsurer.RiskSharePercentage;
                        session.Save(pr, pr.Id);
                    }

                    transaction.Commit();
                    status = true;
                }
                catch (Exception ex)
                {
                    status = false;
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }
                return status;
            }

        }

        internal bool UpdateReinsurer(ReinsurerRequestDto Reinsurer)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Reinsurer pr = new Entities.Reinsurer();

                pr.Id = Reinsurer.Id;
                pr.IsActive = Reinsurer.IsActive;
                pr.ReinsurerCode = Reinsurer.ReinsurerCode;
                pr.ReinsurerName = Reinsurer.ReinsurerName;
                pr.EntryDateTime = Reinsurer.EntryDateTime;
                pr.EntryUser = Reinsurer.EntryUser;
                pr.CurrencyId = Reinsurer.CurrencyId;

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

        public List<ReinsurerConsortium> GetReinsurerConsortiums()
        {
            List<ReinsurerConsortium> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ReinsurerConsortium> ReinsurerConsortiumData = session.Query<ReinsurerConsortium>();
            entities = ReinsurerConsortiumData.ToList();
            return entities;
        }
        public ReinsurerConsortiumResponseDto GetReinsurerConsortiumById(Guid ReinsurerConsortiumId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ReinsurerConsortiumResponseDto pDto = new ReinsurerConsortiumResponseDto();

                var query =
                    from ReinsurerConsortium in session.Query<ReinsurerConsortium>()
                    where ReinsurerConsortium.Id == ReinsurerConsortiumId
                    select new { ReinsurerConsortium = ReinsurerConsortium };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().ReinsurerConsortium.Id;
                    pDto.ParentReinsurerId = result.First().ReinsurerConsortium.ParentReinsurerId;
                    pDto.ReinsurerId = result.First().ReinsurerConsortium.ReinsurerId;
                    pDto.NRPPercentage = result.First().ReinsurerConsortium.NRPPercentage;
                    pDto.ProfitSharePercentage = result.First().ReinsurerConsortium.ProfitSharePercentage;
                    pDto.RiskSharePercentage = result.First().ReinsurerConsortium.RiskSharePercentage;

                    pDto.IsReinsurerConsortiumExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsReinsurerConsortiumExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }
        internal bool AddReinsurerConsortium(ReinsurerConsortiumRequestDto ReinsurerConsortium)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();

                ReinsurerConsortium pr = new Entities.ReinsurerConsortium();

                pr.Id = new Guid();
                pr.ParentReinsurerId = ReinsurerConsortium.ParentReinsurerId;
                pr.ReinsurerId = ReinsurerConsortium.ReinsurerId;
                pr.NRPPercentage = ReinsurerConsortium.NRPPercentage;
                pr.ProfitSharePercentage = ReinsurerConsortium.ProfitSharePercentage;
                pr.RiskSharePercentage = ReinsurerConsortium.RiskSharePercentage;

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
        internal bool UpdateReinsurerConsortium(ReinsurerConsortiumRequestDto ReinsurerConsortium)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ReinsurerConsortium pr = new Entities.ReinsurerConsortium();

                pr.Id = ReinsurerConsortium.Id;
                pr.ParentReinsurerId = ReinsurerConsortium.ParentReinsurerId;
                pr.ReinsurerId = ReinsurerConsortium.ReinsurerId;
                pr.NRPPercentage = ReinsurerConsortium.NRPPercentage;
                pr.ProfitSharePercentage = ReinsurerConsortium.ProfitSharePercentage;
                pr.RiskSharePercentage = ReinsurerConsortium.RiskSharePercentage;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (pr.RiskSharePercentage == 0)
                    {
                        var ReinsurerConsortiumDeleted = from ReinsurerConsortiumd in session.Query<ReinsurerConsortium>()
                                                         where ReinsurerConsortiumd.Id == pr.Id
                                                         select ReinsurerConsortiumd;
                        var ReinsurerConsortiumList = ReinsurerConsortiumDeleted.ToList();
                        foreach (var item in ReinsurerConsortiumList)
                        {
                            session.Delete(item);
                        }
                    }
                    else
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
        internal static List<Guid> GetReinsurerStaffByReinsurerId(Guid ReinsurerId)
        {
            List<Guid> Result = new List<Guid>();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Guid ReinsurerUserTypeId = Guid.Empty;
                var userType = session.Query<UserType>()
                    .Where(a => a.Code.ToLower().StartsWith("ri")).FirstOrDefault();

                if (userType != null)
                    ReinsurerUserTypeId = userType.Id;

                IEnumerable<ReinsurerUser> reinsuereUsers = session.Query<ReinsurerUser>()
                    .Where(a => a.ReinsurerId == ReinsurerId);
                IEnumerable<SystemUser> systemUsers = session.Query<SystemUser>()
                    .Where(b => reinsuereUsers.Any(c => c.InternalUserId == b.LoginMapId) && b.UserTypeId == ReinsurerUserTypeId);

                Result = systemUsers.Select(a => a.LoginMapId).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                throw;
            }
            return Result;
        }
        internal static bool SaveReinsurerStaff(ReinsurerStaffAddRequestDto Request)
        {
            bool result = false;
            try
            {
                if (Request == null || Request.data == null
                    || Request.data.ReinusrerId == Guid.Empty)
                    return false;

                ISession session = EntitySessionManager.GetSession();
                //get reinsuere user type info
                Guid ReinsurerUserTypeId = Guid.Empty;
                var userType = session.Query<UserType>()
                    .Where(a => a.Code.ToLower().StartsWith("ri")).FirstOrDefault();

                if (userType != null)
                    ReinsurerUserTypeId = userType.Id;

                    //checking for existing mappings for reinsurer
                    IEnumerable<ReinsurerUser> ExistingReinsurerMappings = session.Query<ReinsurerUser>()
                        .Where(a => a.ReinsurerId == Request.data.ReinusrerId);
                    //remove if available
                    if (ExistingReinsurerMappings.Count() > 0)
                    {
                        foreach (var ReinsurerMapping in ExistingReinsurerMappings)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Delete(ReinsurerMapping);
                                transaction.Commit();
                            }
                        }

                    }

                foreach (ReinsurerStaff staff in Request.data.ReinsurerStaff)
                {
                    //checking for existing mappings for user
                    IEnumerable<ReinsurerUser> ExistingReinsurerUserMappings = session.Query<ReinsurerUser>()
                        .Where(a => a.InternalUserId == staff.UserId);
                    //remove if available
                    if (ExistingReinsurerUserMappings.Count() > 0)
                    {
                        foreach (var ReinsurerMapping in ExistingReinsurerUserMappings)
                        {
                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Delete(ReinsurerMapping);
                                transaction.Commit();
                            }
                        }

                    }


                    //adding new mappings
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        ReinsurerUser newStaffMApping = new ReinsurerUser()
                        {
                            Id = Guid.NewGuid(),
                            InternalUserId = staff.UserId,
                            ReinsurerId = staff.ReinusrerId
                        };
                        session.Save(newStaffMApping, newStaffMApping.Id);
                        transaction.Commit();
                    }

                    //update user type in system user
                    SystemUser systemUser = session.Query<SystemUser>()
                        .Where(a => a.LoginMapId == staff.UserId).FirstOrDefault();

                    if (systemUser != null)
                    {
                        systemUser.UserTypeId = ReinsurerUserTypeId;
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(systemUser, systemUser.Id);
                            transaction.Commit();
                        }
                    }

                }
                result = true;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return result;
        }

        internal static List<BordxReportColumnsByReinsureIdResponseDto> GetBordxReportColumnsByReinsureId(Guid ReinsureId)
        {
            ISession session = EntitySessionManager.GetSession();


            var arr = from r in session.Query<Reinsurer>()
                      from dc in session.Query<BordxReportColumns>()
                      where r.Id == ReinsureId
                      orderby dc.Sequance
                      select new
                      {
                          Reinsureid = r.Id,
                          r.ReinsurerName,
                          BordxReportColumnsId = dc.Id,
                          BordxReportDisplayName = dc.DisplayName,
                          Sequance = dc.Sequance,
                          HeaderId = dc.HeaderId,
                          IsAllowed = session.Query<ReinsureBordxReportColumns>().Count(dca => dca.ReinsureId == r.Id && dca.ColumnId == dc.Id && dca.IsAllowed == true) > 0 ? true : false,

                      };

            List<BordxReportColumnsByReinsureIdResponseDto> rmlmrList = new List<BordxReportColumnsByReinsureIdResponseDto>();

            BordxReportColumnsByReinsureIdResponseDto rmlmr;
            foreach (var item in arr)
            {
                rmlmr = new BordxReportColumnsByReinsureIdResponseDto();
                rmlmr.ColumnId = item.BordxReportColumnsId;
                rmlmr.DisplayName = item.BordxReportDisplayName;
                rmlmr.IsAllowed = item.IsAllowed;
                rmlmr.ReinsureId = item.Reinsureid;
                rmlmr.HeaderId = item.HeaderId;
                rmlmr.Sequance = item.Sequance;

                rmlmrList.Add(rmlmr);
            }

            return rmlmrList;
        }

        internal static bool AddOrUpdateReinsureBordxReportColumns(List<ReinsureBordxReportColumnsMappingResponseDto> insertion)
        {
            ISession session = EntitySessionManager.GetSession();

            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    var charts = session.Query<ReinsureBordxReportColumns>().Where(a => a.ReinsureId == insertion.First().ReinsureId);

                    foreach (var chartAcc in charts)
                    {
                        session.Delete(chartAcc);
                    }

                    ReinsureBordxReportColumns dcaccess;
                    //RoleMenuMapping rmm;
                    foreach (var item in insertion)
                    {
                        if (item.IsAllowed)
                        {
                            dcaccess = new Entities.ReinsureBordxReportColumns();
                            dcaccess.Id = Guid.NewGuid();
                            dcaccess.ReinsureId = item.ReinsureId;
                            dcaccess.ColumnId = item.ColumnId;
                            dcaccess.IsAllowed = item.IsAllowed;
                            dcaccess.HeaderId = item.HeaderId;
                            session.Save(dcaccess);
                        }

                    }

                    transaction.Commit();
                    return true;

                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            }
        }

        internal static List<ReinsureBordxByReinsureIdandYearResponseDto> GetAllReinsurerBordxByYearandReinsurerIdForGrid(Guid ReinsureId, int BordxYear)
        {
            ISession session = EntitySessionManager.GetSession();
            CommonEntityManager cem = new CommonEntityManager();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

            List< ClaimBordx> ClaimBordx = session.Query<ClaimBordx>().Where(a => a.BordxYear == BordxYear && a.Reinsurer == ReinsureId && a.IsConfirmed == true).ToList();

            var arr = ClaimBordx;

            //var arr = from c in session.Query<ClaimBordx>()
            //          where c.Id == ReinsureId && c.IsConfirmed == true && c.BordxYear == BordxYear
            //          select new
            //          {
            //              Reinsureid = c.Reinsurer,
            //              BordxId = c.Id,
            //              BordxMonth = c.Bordxmonth,
            //              BordxFromDate = c.Fromdate,
            //              BordxToDate = c.Todate,
            //              BordxNumber = c.BordxNumber
            //              //IsAllowed = session.Query<ReinsureBordxReportColumns>().Count(dca => dca.ReinsureId == r.Id && dca.ColumnId == dc.Id && dca.IsAllowed == true) > 0 ? true : false,

            //          };

            List<ReinsureBordxByReinsureIdandYearResponseDto> rmlmrList = new List<ReinsureBordxByReinsureIdandYearResponseDto>();

            ReinsureBordxByReinsureIdandYearResponseDto rmlmr;
            foreach (var item in arr)
            {
                rmlmr = new ReinsureBordxByReinsureIdandYearResponseDto();
                rmlmr.ClaimBordxFromDate = item.Fromdate.ToString("dd-MM-yyyy");
                rmlmr.ClaimBordxId = item.Id;
                rmlmr.ClaimBordxNo = item.BordxNumber;
                rmlmr.ClaimBordxTodate = item.Todate.ToString("dd-MM-yyyy");
                rmlmr.ClaimBordxMonth =  item.Bordxmonth;
                var claimValue = session.Query<ClaimBordxValueDetail>().Where(cbv => cbv.ClaimBordxId == item.Id).FirstOrDefault();

                var query =
                from Country in session.Query<Country>()
                 where Country.Id == claimValue.CountryId
                 select new { Country = Country };

                var result = query.FirstOrDefault();

                rmlmr.ClaimBordxValue = currencyEm.ConvertFromBaseCurrency(claimValue.USDValue, result.Country.CurrencyId, currentCurrencyPeriodId);

                var ClaimPaidAmount = session.Query<ClaimBordxPayment>().Where(cbp => cbp.ClaimBordxID == item.Id).Sum(a => a.BordxAmount);
                rmlmr.ClaimBordxPaidAmount = currencyEm.ConvertFromBaseCurrency(ClaimPaidAmount, result.Country.CurrencyId, currentCurrencyPeriodId);

                rmlmrList.Add(rmlmr);
            }

            return rmlmrList;
        }

        internal static object UserValidationReinsureBordxSubmission(Guid loggedInUserId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                SystemUser sysUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == loggedInUserId).FirstOrDefault();

                if (sysUser != null)
                {
                    UserType usertype = session.Query<UserType>().Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();

                    if (usertype.Code == "IU")
                    {
                        var data = new
                        {
                            status = "OK"
                        };

                        Response = data;
                        return Response;
                    }
                    else if (usertype.Code == "RI")
                    {
                        var data = new
                        {
                            status = "OK"
                        };
                        Response = data;
                        return Response;
                    }
                    else
                    {
                        var data = new
                        {
                            status = "You don't have access to this Page. Please contact Administrator"
                        };

                        Response = data;
                        return Response;
                    }
                }
                else
                {
                    var data = new
                    {
                        status = "You don't have access to this Page. Please contact Administrator"
                    };

                    Response = data;
                    return Response;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }
    }
}
