using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class TPABranchEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal static List<TPABranch> GetAllTPABranches()
        {
            List<TPABranch> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<TPABranch> TPAData = session.Query<TPABranch>();
            entities = TPAData.ToList();
            return entities;
        }

        internal static List<TPABranch> GetTPABranchesByTPAId(Guid TPAId)
        {
            List<TPABranch> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<TPABranch> TPAData = session.Query<TPABranch>().Where(a => a.TpaId == TPAId);
            entities = TPAData.ToList();
            return entities;
        }

        internal static bool SaveTPABranch(DataTransfer.Requests.TPABranchRequestDto TPABranchData)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TPABranch tpaBranch = new Entities.TPABranch();
                tpaBranch.Id = TPABranchData.Id;
                tpaBranch.Address = TPABranchData.Address;
                tpaBranch.BranchCode = TPABranchData.BranchCode;
                tpaBranch.BranchName = TPABranchData.BranchName;
                tpaBranch.CityId = TPABranchData.CityId;
                tpaBranch.ContryId = TPABranchData.ContryId;
                tpaBranch.TimeZone = TPABranchData.TimeZone;
                tpaBranch.IsHeadOffice = TPABranchData.IsHeadOffice;
                tpaBranch.State = TPABranchData.State;
                tpaBranch.TpaId = TPABranchData.TpaId;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (TPABranchData.IsHeadOffice)
                    {
                        List<TPABranch> existingTPABranchList = session.Query<TPABranch>()
                            .Where(a => a.TpaId == TPABranchData.TpaId).ToList();
                        foreach (TPABranch branch in existingTPABranchList)
                        {
                            branch.IsHeadOffice = false;
                            session.Update(branch, branch.Id);
                        }
                        session.Flush();
                    }
                    session.Save(tpaBranch);
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

        internal static bool UpdateTPABranch(TPABranchRequestDto TPABranchData)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                TPABranch tpaBranch = new Entities.TPABranch();
                tpaBranch.Id = TPABranchData.Id;
                tpaBranch.Address = TPABranchData.Address;
                tpaBranch.BranchCode = TPABranchData.BranchCode;
                tpaBranch.BranchName = TPABranchData.BranchName;
                tpaBranch.CityId = TPABranchData.CityId;
                tpaBranch.ContryId = TPABranchData.ContryId;
                tpaBranch.TimeZone = TPABranchData.TimeZone;
                tpaBranch.IsHeadOffice = TPABranchData.IsHeadOffice;
                tpaBranch.State = TPABranchData.State;
                tpaBranch.TpaId = TPABranchData.TpaId;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (TPABranchData.IsHeadOffice)
                    {
                        List<TPABranch> existingTPABranchList = session.Query<TPABranch>()
                            .Where(a => a.TpaId == TPABranchData.TpaId && a.Id != TPABranchData.Id).ToList();
                        foreach (TPABranch branch in existingTPABranchList)
                        {
                            branch.IsHeadOffice = false;
                            session.Update(branch, branch.Id);
                        }
                    }
                    session.Update(tpaBranch, tpaBranch.Id);
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

        internal TPABranchesResponseDto GetTPABranchesBySystemUserId(Guid loggedUserId)
        {
            TPABranchesResponseDto Response = new TPABranchesResponseDto();
            try
            {
                Response.TPABranches = new List<TPABranchResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IQueryable<UserBranch> AssignedUserBranchs = session.Query<UserBranch>().Where(a => a.InternalUserId == loggedUserId);
                IQueryable<TPABranch> AssignedTpaBranches = session.Query<TPABranch>().Where(b => AssignedUserBranchs.Any(c => c.TPABranchId == b.Id));
                if (AssignedTpaBranches.Count() == 0)
                    return Response;
                foreach (TPABranch branch in AssignedTpaBranches.ToList())
                {
                    TPABranchResponseDto dtoBranch = new TPABranchResponseDto()
                    {
                        Id = branch.Id,
                        BranchCode = branch.BranchCode,
                        BranchName = branch.BranchName
                    };
                    Response.TPABranches.Add(dtoBranch);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal static List<TimeZoneResponseDtos> GetAllTimeZones()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Entities.TimeZone>().OrderBy(x => x.Sequence).Select( timezone=> new TimeZoneResponseDtos
            {
                Id = timezone.Id,
                Sequence = timezone.Sequence,
                NameofTimeZone = timezone.NameofTimeZone,
                Time = timezone.Time
            }).ToList();
        }

        internal int GetExsistingTpaBranchByName(Guid Id, string BranchName)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from TPABranch in session.Query<TPABranch>()
                    where TPABranch.BranchName == BranchName
                    select new { TPABranch = TPABranch };

                return query.Count();
            }
            else
            {
                var query =
                    from TPABranch in session.Query<TPABranch>()
                    where TPABranch.Id != Id && TPABranch.BranchName == BranchName
                    select new { BranchName = BranchName };

                return query.Count();
            }
        }

        internal static int GetExsistingTpaBranchByCode(Guid Id, string BranchCode)
        {
            ISession session = EntitySessionManager.GetSession();

            if (Id == Guid.Empty)
            {
                var query =
                    from TPABranch in session.Query<TPABranch>()
                    where TPABranch.BranchCode == BranchCode
                    select new { TPABranch = TPABranch };

                return query.Count();
            }
            else
            {
                var query =
                    from TPABranch in session.Query<TPABranch>()
                    where TPABranch.Id != Id && TPABranch.BranchCode == BranchCode
                    select new { TPABranch = TPABranch };

                return query.Count();
            }
        }
    }
}
