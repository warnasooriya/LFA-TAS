using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class ClaimBatchingEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();


        public List<ClaimBatchTableResponseDto> GetAllClaimBatching()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<ClaimBatch>().Select(claimBatch => new ClaimBatchTableResponseDto {
                Id = claimBatch.Id,
                BatchNumber = claimBatch.BatchNumber,
                CountryId = Convert.ToString(GetCountryById(claimBatch.CountryId)),
                DealerId = Convert.ToString(GetDealerNameById(claimBatch.DealerId)),
                EntryBy = claimBatch.EntryBy,
                EntryDate = claimBatch.EntryDate,
                InsurerId = Convert.ToString(GetInsurerById(claimBatch.InsurerId)),
                ReinsurerId = Convert.ToString(GetReinsurerById(claimBatch.ReinsurerId))
            }).ToList();
        }

        public List<ClaimResponseDto> GetAllClaimDetailsIsBachingFalse()
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Claim>().Where(a => a.IsBatching == false).Select(claim => new ClaimResponseDto {
                Id = claim.Id,
                ClaimCountryId = claim.ClaimCountryId,
                ClaimNumber = claim.ClaimNumber,
                TotalClaimAmount = claim.TotalClaimAmount
            }).ToList();
        }

        public List<Claim> GetAllClaimDetails()
        {
            List<Claim> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Claim> ClaimData = session.Query<Claim>().Where(a => a.GroupId == null);
            entities = ClaimData.ToList();
            return entities;
        }

        public List<Claim> GetAllClaimDetailsByGroupID(Guid GroupId)
        {
            List<Claim> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Claim> ClaimData = session.Query<Claim>().Where(a => a.GroupId == GroupId);
            entities = ClaimData.ToList();
            return entities;
        }

        public ClaimBatchingResponseDto GetClaimBatchingById(Guid ClaimBatchingId)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBatchingResponseDto pDto = new ClaimBatchingResponseDto();

            var query =
                from ClaimBatch in session.Query<ClaimBatch>()
                where ClaimBatch.Id == ClaimBatchingId
                select new { ClaimBatch = ClaimBatch };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().ClaimBatch.Id;
                pDto.BatchNumber = result.First().ClaimBatch.BatchNumber;
                pDto.CountryId = result.First().ClaimBatch.CountryId;
                pDto.DealerId = result.First().ClaimBatch.DealerId;
                pDto.EntryBy = result.First().ClaimBatch.EntryBy;
                pDto.EntryDate = result.First().ClaimBatch.EntryDate;
                pDto.InsurerId = result.First().ClaimBatch.InsurerId;
                pDto.ReinsurerId = result.First().ClaimBatch.ReinsurerId;
                pDto.IsClaimBatchingExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }

        internal static object SaveClaimBatchGroup(ClaimBatchGroupSaveRequestDto claimBatchGroupSaveDetails)
        {
            GenericCodeMsgResponse response = new GenericCodeMsgResponse();
            if (claimBatchGroupSaveDetails == null || claimBatchGroupSaveDetails.data == null ||
                claimBatchGroupSaveDetails.data.ClaimBatchId == Guid.Empty || claimBatchGroupSaveDetails.data.SelectedClaims == null)
            {
                response.code = "error";
                response.msg = "Request data is invalid";
                return response;
            }

            try
            {
                ISession session = EntitySessionManager.GetSession();
                //Todo: validation against bordx
                bool isNewGroup = true;
                if (IsGuid(claimBatchGroupSaveDetails.data.GroupId.ToString()))
                    isNewGroup = false;
                List<ClaimGroupClaim> oldClaimGroupClaims = new List<ClaimGroupClaim>();
                IList<Claim> oldClaims = new List<Claim>();
                List<ClaimGroupClaim> newClaimGroupClaims = new List<ClaimGroupClaim>();
                IList<Claim> newClaims = new List<Claim>();
                //CurrencyCheck
                ClaimBatch claimBatchId = session.Query<ClaimBatch>().Where(a => a.Id == claimBatchGroupSaveDetails.data.ClaimBatchId).FirstOrDefault();
                Dealer dealerdata = session.Query<Dealer>().Where(b => b.Id == claimBatchId.DealerId).FirstOrDefault();
                var currencyEm = new CurrencyEntityManager();

                ClaimBatchGroup claimGroup = new ClaimBatchGroup();


                if (!isNewGroup)
                {
                    oldClaimGroupClaims = session.Query<ClaimGroupClaim>()
                        .Where(a => a.ClaimGroupId == claimBatchGroupSaveDetails.data.GroupId).ToList();

                    oldClaims = session.QueryOver<Claim>()
                    .WhereRestrictionOn(b => b.Id)
                    .IsIn(oldClaimGroupClaims.Select(a => a.ClaimId).ToArray())
                    .List<Claim>();

                    claimGroup = session.Query<ClaimBatchGroup>()
                        .FirstOrDefault(a => a.Id == claimBatchGroupSaveDetails.data.GroupId);
                    claimGroup.Comment = claimBatchGroupSaveDetails.data.Comment;
                }
                else
                {
                    claimGroup = new ClaimBatchGroup()
                    {
                        ClaimBatchId = claimBatchGroupSaveDetails.data.ClaimBatchId,
                        Comment = claimBatchGroupSaveDetails.data.Comment,
                        EntryBy = claimBatchGroupSaveDetails.requestedUserId,
                        EntryDate = DateTime.UtcNow,
                        GroupName = GetNextClaimGroupNumberById(claimBatchGroupSaveDetails.data.ClaimBatchId).ToString(),
                        Id = Guid.NewGuid(),
                        IsAllocatedForCheque = false,
                        IsGoodwill = claimBatchGroupSaveDetails.data.IsGoodwill,
                        //TotalAmount = claimBatchGroupSaveDetails.data.TotalAmount;//todo:conversion
                        TotalAmount = currencyEm.ConvertFromBaseCurrency(claimBatchGroupSaveDetails.data.TotalAmount,dealerdata.CurrencyId,dealerdata.CurrencyPeriodId)
                    };
                }

                newClaims = session.QueryOver<Claim>()
                      .WhereRestrictionOn(a => a.Id)
                      .IsIn(claimBatchGroupSaveDetails.data.SelectedClaims.Select(a => a.Id).ToArray())
                      .List<Claim>();

                foreach (var claimGroupClaim in claimBatchGroupSaveDetails.data.SelectedClaims)
                {
                    ClaimGroupClaim newClaimGroupClaim = new ClaimGroupClaim()
                    {
                        Id = Guid.NewGuid(),
                        //Amount = claimGroupClaim.Amount,//todo:conversion
                        Amount = currencyEm.ConvertFromBaseCurrency(claimGroupClaim.Amount,dealerdata.CurrencyId,dealerdata.CurrencyPeriodId),
                        ClaimGroupId = claimGroup.Id,
                        ClaimId = claimGroupClaim.Id,
                        Comment = claimGroupClaim.Comment,
                        EntryBy = claimBatchGroupSaveDetails.requestedUserId,
                        EntryDate = DateTime.UtcNow
                    };
                    newClaimGroupClaims.Add(newClaimGroupClaim);
                }




                using (ITransaction transaction = session.BeginTransaction())
                {

                    if (isNewGroup)
                    {
                        session.Save(claimGroup, claimGroup.Id);
                    }
                    else
                    {
                        session.Update(claimGroup, claimGroup.Id);
                    }

                    //remove existing
                    foreach (ClaimGroupClaim claimGroupClaim in oldClaimGroupClaims)
                    {
                        session.Delete(claimGroupClaim);
                    }
                    session.Evict(oldClaimGroupClaims);

                    //update old claims
                    foreach (Claim oldClaim in oldClaims)
                    {
                        var claimVal = claimBatchGroupSaveDetails.data.SelectedClaims.FirstOrDefault(a => a.Id == oldClaim.Id);
                        if (claimVal != null)
                        {
                            oldClaim.PaidAmount = claimVal.Amount;
                            oldClaim.PaidComment = claimVal.Comment;
                        }
                        oldClaim.IsBatching = false;
                        session.Update(oldClaim, oldClaim.Id);
                    }
                    session.Evict(oldClaims);

                    //add new claimgroupclaims
                    foreach (ClaimGroupClaim claimGroupClaim in newClaimGroupClaims)
                    {
                        session.Save(claimGroupClaim, claimGroupClaim.Id);
                    }
                    session.Evict(newClaimGroupClaims);

                    //update old claims
                    foreach (Claim oldClaim in newClaims)
                    {
                        var claimVal = claimBatchGroupSaveDetails.data.SelectedClaims.FirstOrDefault(a => a.Id == oldClaim.Id);
                        if(claimVal != null)
                        {
                            oldClaim.PaidAmount = claimVal.Amount;
                            oldClaim.PaidComment = claimVal.Comment;
                        }
                        oldClaim.IsBatching = true;
                        session.Update(oldClaim, oldClaim.Id);
                    }
                    session.Evict(newClaims);

                    transaction.Commit();
                }


                response.code = "ok";
                response.msg = "Claim Group successfully saved";

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.code = "error";
                response.msg = "Error occured while saving Claim Group";
            }
            return response;
        }

        internal static object GetAllAllocatedClaimsByGroupId(Guid claimBatchId, Guid claimBatchGroupId)

        {
            object response = new object();
            if (!IsGuid(claimBatchId.ToString()) || !IsGuid(claimBatchGroupId.ToString()))
                return response;
            try
            {
                ISession session = EntitySessionManager.GetSession();

                response = session.Query<ClaimGroupClaim>()
                    .Where(a => a.ClaimGroupId == claimBatchGroupId)
                    .Join(session.Query<Claim>(), b => b.ClaimId, c => c.Id, (b, c) => new { b, c })
                    .Select(d => new
                    {
                        d.c.Id,
                        d.c.TotalClaimAmount,
                        d.c.ClaimNumber,
                        d.b.Comment,
                        d.b.Amount,
                        Allocated = true
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllEligibleClaimsByBatchId(Guid claimBatchId)
        {
            object response = new object();
            if (!IsGuid(claimBatchId.ToString()))
                return response;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBatch claimBatch = session.Query<ClaimBatch>()
                    .FirstOrDefault(a => a.Id == claimBatchId);
                if (claimBatch == null)
                    return claimBatch;
                IEnumerable<Claim> filterdClaim = session.Query<Claim>()
                    .Where(a => a.PolicyDealerId == claimBatch.DealerId &&
                    a.PolicyCountryId == claimBatch.CountryId &&
                    a.IsApproved == true && a.IsBatching == false);
                IEnumerable<ReinsurerContract> filterdReinsurerContract = session.Query<ReinsurerContract>()
                    .Where(a => a.InsurerId == claimBatch.InsurerId &&
                    a.ReinsurerId == claimBatch.ReinsurerId);


                response = filterdReinsurerContract
                    .Join(session.Query<Contract>(), m => m.Id, n => n.ReinsurerContractId, (m, n) => new { m, n })
                    .Join(session.Query<Policy>(), o => o.n.Id, p => p.ContractId, (o, p) => new { o, p })
                    .Join(filterdClaim, q => q.p.Id, r => r.PolicyId, (q, r) => new { q, r })
                    .Join(session.Query<ClaimInvoiceEntryClaim>(), k => k.r.Id, l => l.ClaimId, (k, l) => new { k, l })
                    .Join(session.Query<ClaimInvoiceEntry>().Where(x => x.IsConfirm == true), w => w.l.ClaimInvoiceEntryId, z => z.Id, (w, z) => new { w,z})

                    .Select(s => new
                    {
                        s.w.k.r.Id,
                        TotalClaimAmount = Math.Round(s.w.k.r.AuthorizedAmount * s.w.k.r.ConversionRate * 100) / 100,
                        s.w.k.r.ClaimNumber,
                        Allocated = false
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static int GetNextClaimGroupNumberById(Guid claimBatchId)
        {
            int response = 1;
            if (!IsGuid(claimBatchId.ToString()))
                return response;

            ISession session = EntitySessionManager.GetSession();
            ClaimBatchGroup claimBatchGroup = session.Query<ClaimBatchGroup>()
                .Where(a => a.ClaimBatchId == claimBatchId)
                .OrderByDescending(a => a.EntryDate)
                .FirstOrDefault();
            if (claimBatchGroup == null)
                return response;

            int groupname = int.Parse(claimBatchGroup.GroupName);
            response = groupname + 1;
            return response;
        }

        internal static object GetClaimGroupsByBatchId(Guid claimBatchId)
        {
            object response = new object();
            if (!IsGuid(claimBatchId.ToString()))
                return response;

            ISession session = EntitySessionManager.GetSession();
            response = session.Query<ClaimBatchGroup>()
                .Where(a => a.ClaimBatchId == claimBatchId)
                .Select(a => new
                {
                    a.Id,
                    a.GroupName,
                    a.Comment,

                }).ToArray();
            return response;
        }

        internal object GetBatchNumbersListUnitOfWork(ClaimBatchNumberRequestDto claimBatchNumberRequestDto)
        {
            //expression builder for policy
            Expression<Func<ClaimBatch, bool>> filterClaimBatch = PredicateBuilder.True<ClaimBatch>();

            if (IsGuid(claimBatchNumberRequestDto.countryId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.CountryId == claimBatchNumberRequestDto.countryId);

            if (IsGuid(claimBatchNumberRequestDto.dealerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.DealerId == claimBatchNumberRequestDto.dealerId);

            if (IsGuid(claimBatchNumberRequestDto.insurerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.InsurerId == claimBatchNumberRequestDto.insurerId);

            if (IsGuid(claimBatchNumberRequestDto.reinsurerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.ReinsurerId == claimBatchNumberRequestDto.reinsurerId);


            ISession session = EntitySessionManager.GetSession();
            var claimBatches = session.Query<ClaimBatch>()
                .Where(filterClaimBatch)
                .OrderByDescending(a => a.EntryDate)
                .Select(a => new
                {
                    a.BatchNumber,
                    a.Id
                }).ToArray();
            return claimBatches;
        }

        internal object GetLast10BatchesBySearchCritera(ClaimBatchNumberRequestDto claimBatchNumberRequestDto)
        {
            //expression builder for policy
            Expression<Func<ClaimBatch, bool>> filterClaimBatch = PredicateBuilder.True<ClaimBatch>();

            if (IsGuid(claimBatchNumberRequestDto.countryId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.CountryId == claimBatchNumberRequestDto.countryId);

            if (IsGuid(claimBatchNumberRequestDto.dealerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.DealerId == claimBatchNumberRequestDto.dealerId);

            if (IsGuid(claimBatchNumberRequestDto.insurerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.InsurerId == claimBatchNumberRequestDto.insurerId);

            if (IsGuid(claimBatchNumberRequestDto.reinsurerId.ToString()))
                filterClaimBatch = filterClaimBatch.And(a => a.ReinsurerId == claimBatchNumberRequestDto.reinsurerId);

            ISession session = EntitySessionManager.GetSession();
            var claimBatches = session.Query<ClaimBatch>()
                .Where(filterClaimBatch)
                .OrderByDescending(a => a.EntryBy)
                .Take(10).Select(a => new
                {
                    a.Id,
                    a.BatchNumber,
                    a.EntryDate,
                    DealerId = GetDealerNameById(a.DealerId),
                    ReinsurerId = GetReinsurerById(a.ReinsurerId),
                    InsurerId = GetInsurerById(a.InsurerId),
                    CountryId = GetCountryById(a.CountryId)
                })
                .OrderByDescending(a => a.EntryDate)
                .ToArray();

            return claimBatches;
        }


        internal string GetNextBatchNumber(ClaimBatchNumberRequestDto claimBatchNumberRequestDto)
        {
            string response = string.Empty;
            if (claimBatchNumberRequestDto.countryId == Guid.Empty || claimBatchNumberRequestDto.dealerId == Guid.Empty ||
                claimBatchNumberRequestDto.insurerId == Guid.Empty || claimBatchNumberRequestDto.reinsurerId == Guid.Empty)
            {
                response = "invalid";
                return response;
            }
            ISession session = EntitySessionManager.GetSession();
            ClaimBatch claimBatch = session.Query<ClaimBatch>()
                .Where(a => a.CountryId == claimBatchNumberRequestDto.countryId && a.DealerId == claimBatchNumberRequestDto.dealerId &&
                a.InsurerId == claimBatchNumberRequestDto.insurerId && a.ReinsurerId == claimBatchNumberRequestDto.reinsurerId)
                .OrderByDescending(z => z.EntryDate).FirstOrDefault();

            if (claimBatch == null || claimBatch.Number == 0)
            {
                response = "00001";
                return response;
            }
            claimBatch.Number++;
            var newBatchNumber = claimBatch.Number.ToString().PadLeft(5, '0');
            return response = newBatchNumber;
        }

        internal string AddClaimBatch(ClaimBatchingRequestDto ClaimBatch)
        {
            string response = string.Empty;
            try
            {
                //validate claim batch details
                if (ClaimBatch == null || !IsGuid(ClaimBatch.CountryId.ToString()) ||
                !IsGuid(ClaimBatch.DealerId.ToString()) ||
                !IsGuid(ClaimBatch.InsurerId.ToString()) || !IsGuid(ClaimBatch.ReinsurerId.ToString()) ||
                string.IsNullOrEmpty(ClaimBatch.BatchNumber) || string.IsNullOrEmpty(ClaimBatch.Prefix))
                {
                    response = "Request data invalid.Please check all the mandatory fields.";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                //existing record validation
                ClaimBatch claimBatachExist = session.Query<ClaimBatch>()
                    .FirstOrDefault(a => a.CountryId == ClaimBatch.CountryId &&
                    a.DealerId == ClaimBatch.DealerId && a.InsurerId == ClaimBatch.InsurerId
                    && a.ReinsurerId == ClaimBatch.ReinsurerId && a.BatchNumber.ToLower() == ClaimBatch.BatchNumber.ToLower());

                if (claimBatachExist != null)
                {
                    response = "Batch Number already exist.";
                    return response;
                }
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ClaimBatch claimBatch = new ClaimBatch()
                    {
                        BatchNumber = ClaimBatch.Prefix + ClaimBatch.BatchNumber,
                        Number = Int32.Parse(ClaimBatch.BatchNumber),
                        CountryId = ClaimBatch.CountryId,
                        DealerId = ClaimBatch.DealerId,
                        EntryBy = ClaimBatch.EntryBy,
                        EntryDate = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        InsurerId = ClaimBatch.InsurerId,
                        ReinsurerId = ClaimBatch.ReinsurerId
                    };



                    session.Save(claimBatch);
                    transaction.Commit();
                }
                response = "ok";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response = "Error occured while saving claim batch";
            }
            return response;
        }

        private static object GetDealerNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Dealer>().Where(a => a.Id == guid).SingleOrDefault().DealerName;
        }

        private static object GetReinsurerById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Reinsurer>().Where(a => a.Id == guid).SingleOrDefault().ReinsurerName;
        }

        private static object GetInsurerById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Insurer>().Where(a => a.Id == guid).SingleOrDefault().InsurerFullName;
        }

        private static object GetCountryById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Country>().Where(a => a.Id == guid).SingleOrDefault().CountryName;
        }

        internal static object GetAllClaimBatchingForSearchGrid(ClaimBatchingSearchGridRequestDto ClaimBatchingSearchGridRequestDto)
        {
            if (ClaimBatchingSearchGridRequestDto != null && ClaimBatchingSearchGridRequestDto.paginationOptionsClaimBatchingSearchGrid != null)
            {
                Expression<Func<ClaimBatch, bool>> filterUser = PredicateBuilder.True<ClaimBatch>();

                //filterUser = filterUser.And(a => a.IsInvoiced == false);
                //if (!String.IsNullOrEmpty(  UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))
                if (!String.IsNullOrEmpty(ClaimBatchingSearchGridRequestDto.claimBatchingSearchGridSearchCriterias.BatchNumber))
                {
                    filterUser = filterUser.And(a => a.BatchNumber.ToString().Contains(ClaimBatchingSearchGridRequestDto.claimBatchingSearchGridSearchCriterias.BatchNumber.ToLower()));
                }


                ISession session = EntitySessionManager.GetSession();
                var filteredUser = session.Query<ClaimBatch>().Where(filterUser);

                long TotalRecords = filteredUser.Count();
                var customerGridDetailsFilterd = filteredUser.Skip((ClaimBatchingSearchGridRequestDto.paginationOptionsClaimBatchingSearchGrid.pageNumber - 1) * ClaimBatchingSearchGridRequestDto.paginationOptionsClaimBatchingSearchGrid.pageSize)
                .Take(ClaimBatchingSearchGridRequestDto.paginationOptionsClaimBatchingSearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    BatchNumber = a.BatchNumber,
                    DealerId = GetDealerNameById(a.DealerId),
                    ReinsurerId = GetReinsurerById(a.ReinsurerId),
                    InsurerId = GetInsurerById(a.InsurerId),
                    CountryId = GetCountryById(a.CountryId)
                    //IsInvoiced = a.IsInvoiced


                })
                .ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = customerGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);

            }
            else
            {
                return null;
            }
        }



        public List<ClaimBatchGroup> GetClaimBatchingGroup()
        {
            List<ClaimBatchGroup> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<ClaimBatchGroup> ClaimBatchGroupData = session.Query<ClaimBatchGroup>().OrderBy(a => a.GroupName);
            entities = ClaimBatchGroupData.ToList();
            return entities;
        }

        public ClaimBatchGroupResponseDto GetClaimBatchGroupById(Guid ClaimBatchGroupId)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBatchGroupResponseDto pDto = new ClaimBatchGroupResponseDto();

            var query =
                from ClaimBatchGroup in session.Query<ClaimBatchGroup>()
                where ClaimBatchGroup.Id == ClaimBatchGroupId
                select new { ClaimBatchGroup = ClaimBatchGroup };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().ClaimBatchGroup.Id;
                pDto.ClaimBatchId = result.First().ClaimBatchGroup.ClaimBatchId;
                pDto.EntryBy = result.First().ClaimBatchGroup.EntryBy;
                pDto.EntryDate = result.First().ClaimBatchGroup.EntryDate;
                pDto.GroupName = result.First().ClaimBatchGroup.GroupName;
                pDto.IsAllocatedForCheque = result.First().ClaimBatchGroup.IsAllocatedForCheque;
                pDto.IsClaimBatchGroupExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }
        internal static bool AddClaimBatchGroup(ClaimBatchGroupRequestDto ClaimBatchGroup)
        {
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    ClaimBatchGroup pr = new Entities.ClaimBatchGroup();

                    pr.Id = new Guid();
                    pr.ClaimBatchId = ClaimBatchGroup.ClaimBatchId;
                    pr.GroupName = ClaimBatchGroup.GroupName;
                    pr.IsAllocatedForCheque = ClaimBatchGroup.IsAllocatedForCheque;
                    pr.EntryBy = ClaimBatchGroup.EntryBy;
                    pr.EntryDate = DateTime.Today.ToUniversalTime();
                    pr.TotalAmount = ClaimBatchGroup.TotalAmount;
                    pr.Comment = ClaimBatchGroup.Comment;
                    pr.IsGoodwill = ClaimBatchGroup.IsGoodwill;

                    session.SaveOrUpdate(pr);
                    Claim cl;
                    ClaimGroupClaim cc;
                    foreach (var item in ClaimBatchGroup.ClaimGroupClaims)
                    {
                        cc = new Entities.ClaimGroupClaim();
                        //DealerMakes cc = new Entities.DealerMakes();
                        cc.Id = new Guid();
                        cc.ClaimId = item.ClaimId;
                        cc.Amount = item.Amount;
                        cc.ClaimGroupId = pr.Id;
                        DateTime dt = new DateTime();
                        dt = DateTime.Now.Date;
                        cc.EntryDate = dt;
                        cc.Comment = item.Comment;
                        session.SaveOrUpdate(cc);

                        cl = new Entities.Claim();
                        cl = session.Query<Claim>().FirstOrDefault(a => a.Id == item.ClaimId);
                        if (cl != null)
                        {
                            cl.IsBatching = true;
                            cl.GroupId = pr.Id;
                            session.SaveOrUpdate(cl);
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



        internal static object GetAllClaimBatchGroupById(Guid ClaimBatchGroupId)
        {
            object Response = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimBatchGroup claimBatchGroup = session.Query<ClaimBatchGroup>()
                    .Where(a => a.Id == ClaimBatchGroupId).FirstOrDefault();
                if (claimBatchGroup != null)
                    Response = claimBatchGroup.GroupName + "|" + claimBatchGroup.Comment;

                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        //internal bool UpdateClaimIsBatching(ClaimBatchGroupRequestDto ClaimBatchGroup)
        //{
        //    try
        //    {
        //        ISession session = EntitySessionManager.GetSession();
        //        var currencyEm = new CurrencyEntityManager();
        //        Claim pr = new Entities.Claim();

        //        foreach (var item in ClaimBatchGroup.ClaimId)
        //        {

        //            pr.Id = item;
        //            pr.IsBatching = true;
        //            pr.LastUpdatedDate = DateTime.Today.ToUniversalTime();
        //            pr.EntryDate = DateTime.Today.ToUniversalTime();
        //        }


        //        using (ITransaction transaction = session.BeginTransaction())
        //        {
        //            session.Update(pr);

        //            transaction.Commit();
        //        }

        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }
        //}

        internal static object GetClaimBatchGroupByName(string GroupName)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimBatchGroupResponseDto pDto = new ClaimBatchGroupResponseDto();

            var query =
                from ClaimBatchGroup in session.Query<ClaimBatchGroup>()
                where ClaimBatchGroup.GroupName == GroupName
                select new { ClaimBatchGroup = ClaimBatchGroup };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().ClaimBatchGroup.Id;
                pDto.ClaimBatchId = result.First().ClaimBatchGroup.ClaimBatchId;
                pDto.EntryBy = result.First().ClaimBatchGroup.EntryBy;
                pDto.EntryDate = result.First().ClaimBatchGroup.EntryDate;
                pDto.GroupName = result.First().ClaimBatchGroup.GroupName;
                pDto.IsAllocatedForCheque = result.First().ClaimBatchGroup.IsAllocatedForCheque;
                pDto.IsClaimBatchGroupExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }


        internal static object GetAllClaimGroupForSearchGrid(ClaimBatchGroupSearchGridRequestDto ClaimBatchGroupSearchGridRequestDto)
        {
            if (ClaimBatchGroupSearchGridRequestDto != null && ClaimBatchGroupSearchGridRequestDto.paginationOptionsClaimBatchGroupSearchGrid != null)
            {
                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();
                filterClaim = filterClaim.And(a => a.IsBatching == false);
                //if (!String.IsNullOrEmpty(  UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))
                if (!String.IsNullOrEmpty(ClaimBatchGroupSearchGridRequestDto.claimBatchGroupSearchGridSearchCriterias.GroupName))
                {
                    filterClaim = filterClaim.And(a => a.ClaimNumber.ToString().Contains(ClaimBatchGroupSearchGridRequestDto.claimBatchGroupSearchGridSearchCriterias.GroupName.ToLower()));
                }


                ISession session = EntitySessionManager.GetSession();
                var filteredUser = session.Query<Claim>().Where(filterClaim);

                long TotalRecords = filteredUser.Count();
                var customerGridDetailsFilterd = filteredUser.Skip((ClaimBatchGroupSearchGridRequestDto.paginationOptionsClaimBatchGroupSearchGrid.pageNumber - 1) * ClaimBatchGroupSearchGridRequestDto.paginationOptionsClaimBatchGroupSearchGrid.pageSize)
                .Take(ClaimBatchGroupSearchGridRequestDto.paginationOptionsClaimBatchGroupSearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    TotalClaimAmount = a.TotalClaimAmount,
                    ClaimNumber = a.ClaimNumber


                })
                .ToArray();
                var response = new CommonGridResponseDto()
                {
                    totalRecords = TotalRecords,
                    data = customerGridDetailsFilterd
                };
                return new JavaScriptSerializer().Serialize(response);

            }
            else
            {
                return null;
            }
        }


        internal static bool UpdateClaimBachGroup(ClaimBatchGroupRequestDto ClaimBatchGroup)
        {
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                try
                {
                    ClaimBatchGroup pr = new Entities.ClaimBatchGroup();
                    pr = session.Query<ClaimBatchGroup>().FirstOrDefault(a => a.Id == ClaimBatchGroup.Id);
                    pr.ClaimBatchId = ClaimBatchGroup.ClaimBatchId;
                    pr.GroupName = ClaimBatchGroup.GroupName;
                    pr.IsAllocatedForCheque = ClaimBatchGroup.IsAllocatedForCheque;
                    pr.EntryBy = ClaimBatchGroup.EntryBy;
                    pr.EntryDate = DateTime.Today.ToUniversalTime();
                    pr.TotalAmount = ClaimBatchGroup.TotalAmount;
                    pr.Comment = ClaimBatchGroup.Comment;
                    pr.IsGoodwill = ClaimBatchGroup.IsGoodwill;

                    session.SaveOrUpdate(pr);
                    Claim claimNewEntity;
                    List<Claim> ClaimList = session.Query<Claim>().Where(a => a.GroupId == ClaimBatchGroup.Id).ToList();
                    foreach (var item in ClaimList)
                    {
                        item.IsBatching = true;
                        item.GroupId = null;
                        session.SaveOrUpdate(item);
                    }

                    foreach (var item in ClaimBatchGroup.ClaimGroupClaims)
                    {
                        claimNewEntity = session.Query<Claim>().FirstOrDefault(a => a.Id == item.ClaimId);
                        if (claimNewEntity != null)
                        {
                            claimNewEntity.IsBatching = true;
                            claimNewEntity.GroupId = pr.Id;
                            session.SaveOrUpdate(claimNewEntity);
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
