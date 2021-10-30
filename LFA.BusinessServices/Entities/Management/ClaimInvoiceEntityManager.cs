using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;
using Amazon.Runtime.Internal;
using NHibernate.Engine;
using NHibernate.Hql.Ast.ANTLR;
using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class ClaimInvoiceEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public ClaimInvoiceResponseDto GetClaimInvoiceEntryById(Guid ClaimInvoiceEntryId)
        {
            ISession session = EntitySessionManager.GetSession();
            ClaimInvoiceResponseDto pDto = new ClaimInvoiceResponseDto();
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();

            var query =
                from ClaimInvoiceEntry in session.Query<ClaimInvoiceEntry>()
                where ClaimInvoiceEntry.Id == ClaimInvoiceEntryId
                select new { ClaimInvoiceEntry = ClaimInvoiceEntry };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().ClaimInvoiceEntry.Id;
                pDto.ConversionRate = result.First().ClaimInvoiceEntry.ConversionRate;
                pDto.CurrencyId = result.First().ClaimInvoiceEntry.CurrencyId;
                pDto.CurrencyPeriodId = result.First().ClaimInvoiceEntry.CurrencyPeriodId;
                pDto.DealerId = result.First().ClaimInvoiceEntry.DealerId;
                pDto.EntryBy = result.First().ClaimInvoiceEntry.EntryBy;
                pDto.EntryDateTime = result.First().ClaimInvoiceEntry.EntryDateTime;
                pDto.InvoiceAmount = currencyEm.ConvertFromBaseCurrency(result.First().ClaimInvoiceEntry.InvoiceAmount, result.First().ClaimInvoiceEntry.CurrencyId, result.First().ClaimInvoiceEntry.CurrencyPeriodId);
                pDto.InvoiceDate = result.First().ClaimInvoiceEntry.InvoiceDate;
                pDto.InvoiceNumber = result.First().ClaimInvoiceEntry.InvoiceNumber;
                pDto.InvoiceReceivedDate = result.First().ClaimInvoiceEntry.InvoiceReceivedDate;
                pDto.UserAttachmentId = result.First().ClaimInvoiceEntry.UserAttachmentId;
                pDto.IsConfirm = result.First().ClaimInvoiceEntry.IsConfirm;
                pDto.IsClaimInvoiceExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }

        internal bool ConfirmClaimInvoiceUpdate(ClaimInvoiceEntryRequestDto claimInvoice)
        {
            ISession session = EntitySessionManager.GetSession();
            try
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    ClaimInvoiceEntry claimInvoiceEntry = session.Query<ClaimInvoiceEntry>().Where(a => a.Id == claimInvoice.Id).FirstOrDefault();
                    claimInvoiceEntry.IsConfirm = true;
                    session.Update(claimInvoiceEntry);
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

        internal static object ClaimInvoiceDataRetrivalForDashboard(Guid claimSubmittedDealerId)
        {

            ClaimInvoiceDashboardResponseDto response = new ClaimInvoiceDashboardResponseDto();
            response.data = new List<ClaimInvoiceData>();
            response.status = "ok";

            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<Claim> dashboardClaimData = session.Query<Claim>()
                    .Where(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId)
                    .ToList();

                List<Claim> dashboardapprovedAllClaimData = session.Query<Claim>()
                    .Where(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId && a.IsApproved == true)
                    .ToList();
                if(dashboardapprovedAllClaimData.Count != 0)
                {
                    var ClaimInvoiceData = new ClaimInvoiceData()
                    {
                        label = "Approved All Claims",
                        value = dashboardapprovedAllClaimData.Count

    };
                    response.data.Add(ClaimInvoiceData);
                }

                List<Claim> dashboardnotapprovedAllClaimData = session.Query<Claim>()
                    .Where(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId && a.IsApproved == false)
                    .ToList();

                if (dashboardnotapprovedAllClaimData.Count != 0)
                {
                    var ClaimInvoiceData = new ClaimInvoiceData()
                    {
                        label = "Not Approved All Claims",
                        value = dashboardnotapprovedAllClaimData.Count
                    };
                    response.data.Add(ClaimInvoiceData);
                }

                List<Claim> dashboardInvoiceAllClaimData = session.Query<Claim>()
                    .Where(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId && a.IsApproved == true && a.IsInvoiced == true)
                    .ToList();

                if (dashboardInvoiceAllClaimData.Count != 0)
                {
                    var ClaimInvoiceData = new ClaimInvoiceData()
                    {
                        label = "Invoiced Claims",
                        value = dashboardInvoiceAllClaimData.Count
                    };
                    response.data.Add(ClaimInvoiceData);
                }

                List<Claim> dashboardNotInvoiceAllClaimData = session.Query<Claim>()
                    .Where(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId && a.IsApproved == true && a.IsInvoiced == false)
                    .ToList();

                if (dashboardNotInvoiceAllClaimData.Count != 0)
                {
                    var ClaimInvoiceData = new ClaimInvoiceData()
                    {
                        label = "Invoiced Claims",
                        value = dashboardNotInvoiceAllClaimData.Count
                    };
                    response.data.Add(ClaimInvoiceData);
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.status = "Error";
            }
            return response;
        }

        internal static object GetAllClaimItemByClaimId(Guid claimId)
        {
            object Response = null;
            //object error = null;

            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Decimal LabourAmount = 0;
                Decimal SundryAmount = 0;
                Decimal PartAmount = 0;
                Decimal TotalLabourAmount = 0;
                Decimal TotalSundryAmount = 0;
                Decimal TotalPartAmount = 0;

                List<ClaimTax> claimTaxes = session.Query<ClaimTax>().Where(a => a.ClaimId == claimId).ToList();
                ClaimEnteredItemDetailsResponseDto ClaimEnteredItem = new ClaimEnteredItemDetailsResponseDto();

                Claim claim = session.Query<Claim>().Where(a => a.Id == claimId).FirstOrDefault();
                List<ClaimItem> claimItem = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                ClaimInvoiceEntryClaim claimInvoiceEntryClaim = session.Query<ClaimInvoiceEntryClaim>()
                    .Where(b => b.ClaimId == claimId).FirstOrDefault();
                ClaimInvoiceEntry claimInvoiceEntry = new ClaimInvoiceEntry();
                if (claimInvoiceEntryClaim == null)
                {
                   ClaimEnteredItem.error = "Not assign to any invoice.";
                    return ClaimEnteredItem;
                }
                else
                {
                    claimInvoiceEntry = session.Query<ClaimInvoiceEntry>()
                        .Where(c => c.Id == claimInvoiceEntryClaim.ClaimInvoiceEntryId).FirstOrDefault();
                }

                foreach(var claimItemList in claimItem)
                {

                    if (claimItemList.ItemName == "Labour Charge")
                    {
                        LabourAmount = LabourAmount + claimItemList.TotalPrice;
                    }
                    else if (claimItemList.ItemName == "Sundry")
                    {
                        SundryAmount = SundryAmount + claimItemList.TotalPrice;
                    }
                    else
                    {
                        PartAmount = PartAmount + claimItemList.TotalPrice;
                    }
                }

                if (claimTaxes.Count != 0)
                {

                    foreach (var tax in claimTaxes)
                    {
                        Decimal LabourAmountTax = 0;
                        Decimal SundryAmountTax = 0;
                        Decimal PartAmountTax = 0;
                        CountryTaxes countryTaxes = session.Query<CountryTaxes>().Where(b => b.TaxTypeId == tax.CountyTaxId).FirstOrDefault();

                        if (countryTaxes.IsPercentage == true)
                        {
                            SundryAmountTax = (SundryAmount * countryTaxes.TaxValue) / 100;
                            LabourAmountTax = (LabourAmount * countryTaxes.TaxValue) / 100;
                            PartAmountTax =(PartAmount * countryTaxes.TaxValue) / 100;
                        }
                        else {
                            if(SundryAmount != 0)
                            {
                                SundryAmountTax = countryTaxes.TaxValue;
                            }else if(LabourAmount != 0)
                            {
                                LabourAmountTax = countryTaxes.TaxValue;
                            }else if(PartAmount != 0)
                            {
                                PartAmountTax = countryTaxes.TaxValue;
                            }


                        }
                         TotalLabourAmount = TotalLabourAmount + LabourAmountTax;
                         TotalSundryAmount = TotalSundryAmount + SundryAmountTax;
                         TotalPartAmount = TotalPartAmount + PartAmountTax;
                    }
                    ClaimEnteredItem = new ClaimEnteredItemDetailsResponseDto()
                    {
                        ClaimId = claim.Id,
                        ClaimApprovedAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        InvoiceAmount = currencyEm.ConvertFromBaseCurrency(claimInvoiceEntry.InvoiceAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        SundryAmount = currencyEm.ConvertFromBaseCurrency(SundryAmount + TotalSundryAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        LabourAmount = currencyEm.ConvertFromBaseCurrency(LabourAmount + TotalLabourAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        PartAmount = currencyEm.ConvertFromBaseCurrency(PartAmount + TotalPartAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        ClaimNo = claim.ClaimNumber
                    };
                }
                else
                {
                    ClaimEnteredItem = new ClaimEnteredItemDetailsResponseDto()
                    {
                        ClaimId = claim.Id,
                        ClaimApprovedAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        InvoiceAmount = currencyEm.ConvertFromBaseCurrency(claimInvoiceEntry.InvoiceAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        SundryAmount = currencyEm.ConvertFromBaseCurrency(SundryAmount ,claim.ClaimCurrencyId,claim.CurrencyPeriodId),
                        LabourAmount = currencyEm.ConvertFromBaseCurrency(LabourAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId) ,
                        PartAmount = currencyEm.ConvertFromBaseCurrency(PartAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId),
                        ClaimNo = claim.ClaimNumber
                    };


                }



                Response = ClaimEnteredItem;
            }
            catch(Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;
        }

        internal object AddClaimAjusmentInvoiceEntry(Guid claimId, decimal adjustPartAmount, decimal adjustLabourAmount, decimal adjustSundryAmount)
        {
            object Response = null;
            try
            {
                ClaimInvoiceEntry pr = new Entities.ClaimInvoiceEntry();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                List<ClaimInvoiceEntryClaim> p = new List<Entities.ClaimInvoiceEntryClaim>();
                ISession session = EntitySessionManager.GetSession();

                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);
                claim.PartadjustedValue = currencyEm.ConvertToBaseCurrency(adjustPartAmount, claim.ClaimCurrencyId, currentCurrencyPeriodId);
                claim.LabouradjustedValue = currencyEm.ConvertToBaseCurrency(adjustLabourAmount, claim.ClaimCurrencyId, currentCurrencyPeriodId);
                claim.SundryadjustedValue = currencyEm.ConvertToBaseCurrency(adjustSundryAmount, claim.ClaimCurrencyId, currentCurrencyPeriodId);
                claim.IsBatching = true;
                claim.IsInvoiced = true;

                Decimal PaidAmount = adjustPartAmount + adjustLabourAmount + adjustSundryAmount;
                claim.PaidAmount = currencyEm.ConvertToBaseCurrency(PaidAmount, claim.ClaimCurrencyId, currentCurrencyPeriodId);

                using (ITransaction transaction = session.BeginTransaction())
                {

                        if (claim != null)
                        {
                            session.SaveOrUpdate(claim);
                        }

                    transaction.Commit();
                }

                return Response = "OK";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);

            }

            return Response;
        }

        internal static object GetAllSubmittedInvoiceClaimByDealerId(Guid claimSubmittedDealerId,string invoiceNumber , string claimNumber)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                Expression<Func<Claim, bool>> claimFilter = PredicateBuilder.True<Claim>();
                Expression<Func<ClaimInvoiceEntry, bool>> claimInvoiceEntry = PredicateBuilder.True<ClaimInvoiceEntry>();
                claimFilter = claimFilter.And(a => a.ClaimSubmittedDealerId == claimSubmittedDealerId && a.IsApproved == true && a.IsInvoiceSubmit == true
                //&& a.IsBatching == false
                );

                if (!String.IsNullOrEmpty(claimNumber))
                {
                    claimFilter = claimFilter.And(a => a.ClaimNumber.ToLower().Contains(claimNumber.ToLower()));
                }


                var ress = session.Query<Claim>()
                    .Where(claimFilter)
                    .Join(session.Query<ClaimInvoiceEntryClaim>(), b => b.Id, c => c.ClaimId, (b, c) => new { b, c })
                    .Join(session.Query<ClaimInvoiceEntry>(), d => d.c.ClaimInvoiceEntryId, e => e.Id, (d, e) => new { d, e })

                    .OrderBy(x => x.d.b.ClaimNumber)
                    .Select(g => new
                    {
                        g.d.b.Id,
                        g.d.b.ClaimNumber,
                        g.d.b.LastUpdatedDate,
                        g.d.b.TotalClaimAmount,
                        TotalClaimAmount2 = currencyEm.ConvertFromBaseCurrency(g.d.b.TotalClaimAmount, g.d.b.ClaimCurrencyId, currentCurrencyPeriodId),
                        g.d.b.IsInvoiced,
                        g.d.b.IsInvoiceSubmit,
                        ClaimCurrencyCode = cem.GetCurrencyCodeById(g.d.b.ClaimCurrencyId),
                        g.e.InvoiceReceivedDate,
                        g.e.InvoiceDate,
                        g.e.InvoiceNumber,
                        InvoiceAmount = currencyEm.ConvertFromBaseCurrency(g.e.InvoiceAmount, g.d.b.ClaimCurrencyId, currentCurrencyPeriodId),
                        g.e.IsConfirm,
                        ClaimInvoiceEntryId = g.e.Id,
                        g.d.b.IsBatching

                    }).Where(a=>a.IsConfirm==false).ToArray().OrderBy(a => a.InvoiceNumber);


                if (!String.IsNullOrEmpty(invoiceNumber))
                {
                    Response = ress.Where(a=>a.InvoiceNumber.ToLower().Contains(invoiceNumber.ToLower()));
                }
                else {
                    Response = ress;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        private static object GetDealerNameById(Guid guid)
        {
            ISession session = EntitySessionManager.GetSession();
            return session.Query<Dealer>().Where(a => a.Id == guid).SingleOrDefault().DealerName;
        }

        internal bool AddClaimInvoiceEntry(ClaimInvoiceEntryRequestDto ClaimInvoiceEntry)
        {
            ISession session = EntitySessionManager.GetSession();
            //using (ITransaction transaction = session.BeginTransaction())
            //{
                try
                {
                    ClaimInvoiceEntry pr = new Entities.ClaimInvoiceEntry();
                    CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                    Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                    List<ClaimInvoiceEntryClaim> p = new List<Entities.ClaimInvoiceEntryClaim>();

                    pr.Id = new Guid();
                    pr.ConversionRate = currencyEm.GetConversionRate(ClaimInvoiceEntry.CurrencyId, currentCurrencyPeriodId);
                    pr.CurrencyId = ClaimInvoiceEntry.CurrencyId;
                    pr.CurrencyPeriodId = currentCurrencyPeriodId;
                    pr.DealerId = ClaimInvoiceEntry.DealerId;
                    pr.EntryBy = ClaimInvoiceEntry.EntryBy;
                    pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                    pr.InvoiceAmount = currencyEm.ConvertToBaseCurrency(ClaimInvoiceEntry.InvoiceAmount, ClaimInvoiceEntry.CurrencyId, currentCurrencyPeriodId);
                    pr.InvoiceDate = ClaimInvoiceEntry.InvoiceDate;
                    pr.InvoiceNumber = ClaimInvoiceEntry.InvoiceNumber;
                    pr.InvoiceReceivedDate = ClaimInvoiceEntry.InvoiceReceivedDate;
                    pr.UserAttachmentId = ClaimInvoiceEntry.UserAttachmentId;
                    pr.IsConfirm = ClaimInvoiceEntry.IsConfirm;




                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.SaveOrUpdate(pr);

                        Claim claim;

                        foreach (var item in ClaimInvoiceEntry.claims)
                        {
                            ClaimInvoiceEntryClaim cc = new Entities.ClaimInvoiceEntryClaim();
                            //DealerMakes cc = new Entities.DealerMakes();
                            cc.Id = new Guid();
                            cc.ClaimId = item.Id;
                            cc.ClaimInvoiceEntryId = pr.Id;
                            session.SaveOrUpdate(cc);

                            claim = session.Query<Claim>().FirstOrDefault(a => a.Id == item.Id);

                            if (claim != null)
                            {
                                claim.IsInvoiced = false;
                                claim.IsInvoiceSubmit = true;
                            session.SaveOrUpdate(claim);
                            }
                        }
                        transaction.Commit();
                    }


                    return true;
                }
                catch (Exception ex)
                {
                   // transaction.Rollback();
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                    return false;
                }
            //}
        }


        internal static object GetAllClaimByClaimSubmittedDealerId(Guid ClaimSubmittedDealerId,string ClaimNumber)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();


                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();

                filterClaim = filterClaim.And(a => a.ClaimSubmittedDealerId == ClaimSubmittedDealerId && a.IsApproved == true && a.IsInvoiceSubmit == false);
                if (!String.IsNullOrEmpty(ClaimNumber))
                {
                    filterClaim = filterClaim.And(a => a.ClaimNumber.ToLower().Contains(ClaimNumber.ToLower()));
                }

                List<Claim> ClaimList = session.Query<Claim>()
                    .Where(filterClaim).ToList();
                if (ClaimList != null && ClaimList.Count() > 0)
                {
                    Response = ClaimList
                        .OrderBy(a => a.ClaimNumber)
                        .Select(a => new
                        {
                            a.Id,
                            a.ClaimNumber,
                            a.LastUpdatedDate,
                            TotalClaimAmount = a.AuthorizedAmount,
                            TotalClaimAmount2 = currencyEm.ConvertFromBaseCurrency(a.AuthorizedAmount, a.ClaimCurrencyId, currentCurrencyPeriodId),
                            a.IsInvoiced,
                            a.IsInvoiceSubmit,
                            ClaimCurrencyCode = cem.GetCurrencyCodeById(a.ClaimCurrencyId),
                            InvoiceNumber = "N/A"
                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetAllClaimInvoiceEntryForSearchGrid(ClaimInvoiceEntrySearchGridRequestDto ClaimInvoiceEntrySearchGridRequestDto)
        {
            if (ClaimInvoiceEntrySearchGridRequestDto != null && ClaimInvoiceEntrySearchGridRequestDto.paginationOptionsClaimInvoiceEntrySearchGrid != null)
            {
                Expression<Func<ClaimInvoiceEntry, bool>> filterUser = PredicateBuilder.True<ClaimInvoiceEntry>();
                // filterUser = filterUser.And(a => a.IsActive == true);
                //if (!String.IsNullOrEmpty(  UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))
                if (!String.IsNullOrEmpty(ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.dealer))
                {
                    filterUser = filterUser.And(a => a.DealerId.ToString().Contains(ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.dealer.ToLower()));
                }
                if (!String.IsNullOrEmpty(ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.invoiceDate.ToString())
                && ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.invoiceDate != DateTime.MinValue)
                {
                    filterUser = filterUser.And(a => a.InvoiceReceivedDate >= ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.invoiceDate);
                }
                if (!String.IsNullOrEmpty(ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.invoiceNo))
                {
                    filterUser = filterUser.And(a => a.InvoiceNumber.ToLower().Contains(ClaimInvoiceEntrySearchGridRequestDto.ClaimInvoiceEntrySearchGridSearchCriterias.invoiceNo.ToLower()));
                }

                ISession session = EntitySessionManager.GetSession();
                var filteredUser = session.Query<ClaimInvoiceEntry>().Where(filterUser);
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                long TotalRecords = filteredUser.Count();
                var customerGridDetailsFilterd = filteredUser.Skip((ClaimInvoiceEntrySearchGridRequestDto.paginationOptionsClaimInvoiceEntrySearchGrid.pageNumber - 1) * ClaimInvoiceEntrySearchGridRequestDto.paginationOptionsClaimInvoiceEntrySearchGrid.pageSize)
                .Take(ClaimInvoiceEntrySearchGridRequestDto.paginationOptionsClaimInvoiceEntrySearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    InvoiceNumber = a.InvoiceNumber,
                    InvoiceReceivedDate = a.InvoiceReceivedDate,
                    DealerId = GetDealerNameById(a.DealerId),
                    InvoiceAmount = currencyEm.ConvertFromBaseCurrency(a.InvoiceAmount,a.CurrencyId,a.CurrencyPeriodId),
                    InvoiceDate = a.InvoiceDate.ToString("dd-MMM-yyyy")
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

        internal static object GetAllClaimInvoiceEntryForSearchGrid(ClaimInvoiceEntryClaimSearchGridRequestDto ClaimInvoiceEntryClaimSearchGridRequestDto)
        {
            if (ClaimInvoiceEntryClaimSearchGridRequestDto != null && ClaimInvoiceEntryClaimSearchGridRequestDto.paginationOptionsClaimInvoiceEntryClaimSearchGrid != null)
            {
                Expression<Func<Claim, bool>> filterUser = PredicateBuilder.True<Claim>();
                filterUser = filterUser.And(a => a.IsInvoiced == false && a.IsApproved == true);
                //if (!String.IsNullOrEmpty(  UserSearchGridRequestDto.UserSearchGridSearchCriterias.eMail))

                if (ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.DealerId != Guid.Empty)
                {
                    filterUser = filterUser.And(a => a.ClaimSubmittedDealerId == ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.DealerId);
                }
                if (!String.IsNullOrEmpty(ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.ClaimNumber))
                {
                    filterUser = filterUser.And(a => a.ClaimNumber.ToString().Contains(ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.ClaimNumber.ToLower()));
                }
                if (!String.IsNullOrEmpty(ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.LastUpdatedDate.ToString())
                && ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.LastUpdatedDate != DateTime.MinValue)
                {
                    filterUser = filterUser.And(a => a.LastUpdatedDate >= ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.LastUpdatedDate);
                }
                if (!String.IsNullOrEmpty(ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.TotalClaimAmount.ToString()))
                {
                    filterUser = filterUser.And(a => a.TotalClaimAmount >= ClaimInvoiceEntryClaimSearchGridRequestDto.claimInvoiceEntryClaimSearchGridSearchCriterias.TotalClaimAmount);
                }

                ISession session = EntitySessionManager.GetSession();
                var filteredUser = session.Query<Claim>().Where(filterUser);

                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();

                long TotalRecords = filteredUser.Count();
                var customerGridDetailsFilterd = filteredUser.Skip((ClaimInvoiceEntryClaimSearchGridRequestDto.paginationOptionsClaimInvoiceEntryClaimSearchGrid.pageNumber - 1) * ClaimInvoiceEntryClaimSearchGridRequestDto.paginationOptionsClaimInvoiceEntryClaimSearchGrid.pageSize)
                .Take(ClaimInvoiceEntryClaimSearchGridRequestDto.paginationOptionsClaimInvoiceEntryClaimSearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    ClaimNumber = a.ClaimNumber,
                    TotalClaimAmount2 = currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount,a.ClaimCurrencyId,currentCurrencyPeriodId),
                    LastUpdatedDate = a.LastUpdatedDate.ToString("dd-MMM-yyyy"),
                    //IsInvoiced = a.IsInvoiced,
                    TotalClaimAmount = a.TotalClaimAmount,
                    ClaimCurrencyCode = cem.GetCurrencyCodeById(a.ClaimCurrencyId)


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

        internal bool UpdatePart(ClaimInvoiceEntryRequestDto ClaimInvoice)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimInvoiceEntry pr = new Entities.ClaimInvoiceEntry();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentCurrencyPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                List<ClaimInvoiceEntryClaim> oldClaimInvoiceEntryClaim = new List<ClaimInvoiceEntryClaim>();
                IList<Claim> oldClaims = new List<Claim>();

                pr.Id = ClaimInvoice.Id;
                pr.ConversionRate = currencyEm.GetConversionRate(ClaimInvoice.CurrencyId, currentCurrencyPeriodId);
                pr.CurrencyId = ClaimInvoice.CurrencyId;
                pr.CurrencyPeriodId = currentCurrencyPeriodId;
                pr.DealerId = ClaimInvoice.DealerId;
                pr.EntryBy = ClaimInvoice.EntryBy;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.InvoiceAmount = currencyEm.ConvertToBaseCurrency(ClaimInvoice.InvoiceAmount, ClaimInvoice.CurrencyId, currentCurrencyPeriodId);
                pr.InvoiceDate = ClaimInvoice.InvoiceDate;
                pr.InvoiceNumber = ClaimInvoice.InvoiceNumber;
                pr.InvoiceReceivedDate = ClaimInvoice.InvoiceReceivedDate;
                pr.UserAttachmentId = ClaimInvoice.UserAttachmentId;
                pr.IsConfirm = ClaimInvoice.IsConfirm;

                oldClaimInvoiceEntryClaim = session.Query<ClaimInvoiceEntryClaim>()
                        .Where(a => a.ClaimInvoiceEntryId == ClaimInvoice.Id).ToList();

                oldClaims = session.QueryOver<Claim>()
                 .WhereRestrictionOn(b => b.Id)
                 .IsIn(oldClaimInvoiceEntryClaim.Select(a => a.ClaimId).ToArray())
                 .List<Claim>();

                    using (ITransaction transaction = session.BeginTransaction())
                {
                    //remove exciting
                    foreach (ClaimInvoiceEntryClaim claimInvoiceEntryClaim in oldClaimInvoiceEntryClaim)
                    {
                        session.Delete(claimInvoiceEntryClaim);
                    }
                    //update old claims
                    foreach (Claim oldClaim in oldClaims)
                    {
                        oldClaim.IsInvoiced = false;
                        oldClaim.IsInvoiceSubmit = false;
                        session.Update(oldClaim, oldClaim.Id);
                    }
                    //add new claimgroupclaims
                    foreach (var item in ClaimInvoice.claims)
                    {
                        Claim claim = new Claim();
                        ClaimInvoiceEntryClaim cc = new Entities.ClaimInvoiceEntryClaim();
                        cc.Id = new Guid();
                        cc.ClaimId = item.Id;
                        cc.ClaimInvoiceEntryId = pr.Id;
                        session.SaveOrUpdate(cc);
                        claim = session.Query<Claim>().FirstOrDefault(a => a.Id == item.Id);
                        if (claim != null)
                        {
                            claim.IsInvoiced = true;
                            claim.IsInvoiceSubmit = true;
                            session.Update(claim, claim.Id);
                        }
                    }
                    session.Update(pr);
                    //session.Evict(ClaimInvoice.claims);


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
