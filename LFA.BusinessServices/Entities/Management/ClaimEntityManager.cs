using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlTypes;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using TAS.DataTransfer.Exceptions;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Common.Notification;
using TAS.Services.Common.Transformer;
using TAS.Services.Entities.Persistence;
using TAS.Services.Reports.ClaimAuthorization;
using TAS.Services.Reports.ClaimChequeStatement;

namespace TAS.Services.Entities.Management
{
    public class ClaimEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public object UserValidationClaimSubmission(Guid loggedInUserId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                SystemUser sysUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == loggedInUserId).FirstOrDefault();
                if (sysUser == null)
                {
                    var data = new
                    {
                        status = "You don't have access to this Page. Please contact Administrator",
                        userType = "",
                        dealerCurrencyId = "",
                        dealerCurrencyCode = "",
                        dealerHourlyRate = 0.00

                    };
                    Response = data;
                    return Response;
                }
                UserType userType = session.Query<UserType>()
                    .Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();
                if (userType == null)
                {
                    var data = new
                    {
                        status = "You don't have access to this Page. Please contact Administrator",
                        userType = "",
                        dealerCurrencyId = "",
                        dealerCurrencyCode = "",
                        dealerHourlyRate = 0.00
                    };
                    Response = data;
                    return Response;
                }

                if (userType.Code.ToLower() == "du")
                {
                    DealerStaff dealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == loggedInUserId).FirstOrDefault();
                    if (dealerStaff == null)
                    {
                        var data = new
                        {
                            status = "You haven't assigned to any dealer.",
                            userType = "",
                            dealerCurrencyId = "",
                            dealerCurrencyCode = "",
                            dealerHourlyRate = 0.00

                        };
                        Response = data;
                        return Response;
                    }


                    Dealer dealer = session.Query<Dealer>()
                        .Where(a => a.Id == dealerStaff.DealerId).FirstOrDefault();

                    if (dealer == null)
                    {
                        var data = new
                        {
                            status = "You haven't assigned to any dealer.",
                            userType = "",
                            dealerCurrencyId = "",
                            dealerCurrencyCode = "",
                            dealerHourlyRate = 0.00

                        };
                        Response = data;
                        return Response;
                    }
                    else
                    {
                        if (IsGuid(dealer.CurrencyId.ToString()))
                        {
                            var data = new
                            {
                                status = "ok",
                                userType = userType.Code,
                                dealerCurrencyId = dealer.CurrencyId,
                                dealerCurrencyCode = cem.GetCurrencyTypeByIdCode(dealer.CurrencyId),
                                dealerHourlyRate = Math.Round(dealer.ManHourRate * dealer.ConversionRate * 100) / 100,
                                dealerId = dealer.Id

                            };
                            Response = data;
                            return Response;
                        }
                        else
                        {
                            var data = new
                            {
                                status = "Currency is not set on the dealer you are assigned (" + dealer.DealerName + ").",
                                userType = "",
                                dealerCurrencyId = "",
                                dealerCurrencyCode = "",
                                dealerHourlyRate = 0.00

                            };
                            Response = data;
                            return Response;
                        }
                    }
                }
                else
                {
                    var data = new
                    {
                        status = "You have to logged in as a dealer to access this page.",
                        userType = "",
                        dealerCurrencyId = "",
                        dealerCurrencyCode = "",
                        dealerHourlyRate = 0.00

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



        internal static object getClaimAuthorizationFormforTYER(Guid loggedInUserId, Guid claimId, string tpaName, string dbConnectionString)
        {
            object response = null;
            try
            {
                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + tpaName.ToLower() + "\\ClaimAuthorizationOther.sql")
                    );
                string ReportLocation = isExist ? tpaName.ToLower() : "Default";

                ISession session = EntitySessionManager.GetSession();

                String Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath +
                        "\\ClaimAuthorization\\" + ReportLocation + "\\ClaimAuthorizationOther.sql"));
                Query = Query

                    .Replace("{claimId}", claimId.ToString());

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = "ClaimAuthorizationOther",
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + ReportLocation
                };
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(ReportDataQuery, ReportDataQuery.Id);
                    transaction.Commit();
                }

                response = reportKey;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object getClaimAuthorizationFormforCycle(Guid loggedInUserId, Guid claimId, string tpaName, string dbConnectionString)
        {
            object response = null;
            try
            {
                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + tpaName.ToLower() + "\\ClaimAuthorization.sql")
                    );
                string ReportLocation = isExist ? tpaName.ToLower() : "Default";
                ISession session = EntitySessionManager.GetSession();

                String Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath +
                        "\\ClaimAuthorization\\" + ReportLocation + "\\ClaimAuthorization.sql"));


                Query = Query

                    .Replace("{claimId}", claimId.ToString());

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = "ClaimAuthorization",
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + ReportLocation
                };
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(ReportDataQuery, ReportDataQuery.Id);
                    transaction.Commit();
                }

                response = reportKey;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        internal static object getClaimAuthorizationFormforCycle1(Guid loggedInUserId, Guid claimId, string tpaName, string dbConnectionString)
        {
            object response = null;
            try
            {
                bool isExist = File.Exists(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + tpaName.ToLower() + "\\ClaimAuthorization1.sql")
                    );
                string ReportLocation = isExist ? tpaName.ToLower() : "Default";
                ISession session = EntitySessionManager.GetSession();

                String Query = File.ReadAllText(
                    System.Web.HttpContext.Current.Server.MapPath(
                        ConfigurationData.ReportsPath +
                        "\\ClaimAuthorization\\" + ReportLocation + "\\ClaimAuthorization1.sql"));


                Query = Query

                    .Replace("{claimId}", claimId.ToString());

                //putting in the db so report viewer can view and read it

                string reportKey = Guid.NewGuid().ToString();
                ReportDataQuery ReportDataQuery = new ReportDataQuery()
                {
                    Id = Guid.NewGuid(),
                    ReportKey = Guid.Parse(reportKey),
                    ReportCode = "ClaimAuthorization",
                    ReportDbConnStr = dbConnectionString,
                    ReportQuery = Query,
                    ReportDirectory = ConfigurationData.ReportsPath + "\\ClaimAuthorization\\" + ReportLocation
                };
                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Save(ReportDataQuery, ReportDataQuery.Id);
                    transaction.Commit();
                }

                response = reportKey;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }


        internal static byte[] ClaimAuthorizationFormExport(Guid claimId, Guid userId, string tpaName)
        {
            byte[] Response = null;
            try
            {
                #region validate
                if (!IsGuid(claimId.ToString()) || !IsGuid(userId.ToString()) || string.IsNullOrEmpty(tpaName))
                {
                    return Response;
                }
                #endregion

                ISession session = EntitySessionManager.GetSession();

                SystemUser systemUser = session.Query<SystemUser>()
                    .FirstOrDefault(a => a.LoginMapId == userId);
                if (systemUser != null)
                {
                    UserType userType = session.Query<UserType>()
                        .FirstOrDefault(a => a.Id == systemUser.UserTypeId);
                    if (userType != null)
                    {
                        Response = new ClaimAuthorizationReport().Generate(claimId, userId, userType.Code, tpaName);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object ClaimInformationRequest(ClaimInformationRequestDto claimInformationRequestDto, Guid tpaId)
        {
            GenericCodeMsgResponse Response = new GenericCodeMsgResponse();
            try
            {
                //validation
                if (claimInformationRequestDto == null ||
                    !IsGuid(claimInformationRequestDto.claimId.ToString()) ||
                    claimInformationRequestDto.informationMsg == string.Empty)
                {
                    Response.code = "error";
                    Response.msg = "Input data is empty";
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                bool isClaimSubmission = false;
                ClaimSubmission claimSubmission = new ClaimSubmission();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimInformationRequestDto.claimId);

                if (claim == null)
                {
                    claimSubmission = session.Query<ClaimSubmission>()
                        .FirstOrDefault(a => a.Id == claimInformationRequestDto.claimId);
                    if (claimSubmission == null)
                    {
                        Response.code = "error";
                        Response.msg = "Input claim is invalid";
                        return Response;
                    }
                    else
                    {
                        isClaimSubmission = true;
                    }
                }

                ClaimComment claimComment;
                if (isClaimSubmission)
                {
                    claimComment = new ClaimComment()
                    {
                        ByTPA = true,
                        ClaimId = claimSubmission.Id,
                        Comment = claimInformationRequestDto.informationMsg,
                        EntryDateTime = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        PolicyId = claimSubmission.PolicyId,
                        Seen = false,
                        SeenDateTime = SqlDateTime.MinValue.Value,
                        SentFrom = claimInformationRequestDto.loggedInUserId,
                        SentTo = claimSubmission.ClaimSubmittedBy
                    };

                }
                else
                {
                    claimComment = new ClaimComment()
                    {
                        ByTPA = true,
                        ClaimId = claim.Id,
                        Comment = claimInformationRequestDto.informationMsg,
                        EntryDateTime = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        PolicyId = claim.PolicyId,
                        Seen = false,
                        SeenDateTime = SqlDateTime.MinValue.Value,
                        SentFrom = claimInformationRequestDto.loggedInUserId,
                        SentTo = claim.ClaimSubmittedBy
                    };
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    //save claim comment
                    session.Save(claimComment, claimComment.Id);

                    //update claim
                    if (isClaimSubmission)
                    {
                        claimSubmission.StatusId = new CommonEntityManager().GetClaimStatusIdByCode("REQ");
                        session.Update(claimSubmission, claimSubmission.Id);
                    }
                    else
                    {
                        claim.StatusId = new CommonEntityManager().GetClaimStatusIdByCode("REQ");
                        session.Update(claim, claim.Id);
                    }

                    transaction.Commit();
                }

                //set notification
                try
                {
                    CommonEntityManager commonEntityManager = new CommonEntityManager();
                    var notificationDto = new PushNotificationsRequestDto()
                    {
                        generatedTime = DateTime.UtcNow,
                        message = GenerateClaimNotificationTextByClaimNoAndClaimStatus(claim.ClaimNumber, "REQ"),
                        link = ConfigurationData.BaseUrl + "app/claim/listing/" + claim.Id,
                        messageFrom = commonEntityManager.GetUserNameById(claimInformationRequestDto.loggedInUserId),
                        profilePic = commonEntityManager.GetProfilePictureByUserId(claimInformationRequestDto.loggedInUserId),
                        userDetails = new List<UserDetail>()
                        {
                            new UserDetail()
                            {
                                tpaId = tpaId,
                                userId = isClaimSubmission?claimSubmission.ClaimSubmittedBy:claim.ClaimSubmittedBy
                            }
                        }
                    };
                    Task.Run(async () => await NotificationEntityManager.PushNotificationSender(notificationDto));

                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }


                Response.code = "success";

            }
            catch (Exception ex)
            {
                Response.code = "error";
                Response.msg = "Error occured while saving/";

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetAllClaimHistoryDetailsByClaimId(Guid claimId, Guid loggedInUserId)
        {
            AllClaimHistoryDetailsResponseDto response = new AllClaimHistoryDetailsResponseDto();
            try
            {
                if (!IsGuid(claimId.ToString()) || !IsGuid(claimId.ToString()))
                {
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                //user type filtering
                SystemUser sysUser = session.Query<SystemUser>()
                   .Where(a => a.LoginMapId == loggedInUserId).FirstOrDefault();
                if (sysUser == null)
                {
                    return response;
                }

                UserType userType = session.Query<UserType>()
                    .FirstOrDefault(a => a.Id == sysUser.UserTypeId);
                if (userType == null)
                {
                    return response;
                }

                GetPolicyDetailsForViewInClaimRequestDto claimReqData = new GetPolicyDetailsForViewInClaimRequestDto() { claimId = claimId };
                if (userType.Code.ToLower() != "du")
                {
                    response.assessmentAndClaimHistory = GetClaimDetailsForView(claimReqData);
                }

                Guid policyId = Guid.Empty;
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimReqData.claimId);
                if (claim == null)
                {
                    ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                        .FirstOrDefault(a => a.Id == claimReqData.claimId);
                    if (claimSubmission != null)
                    {
                        policyId = claimSubmission.PolicyId;
                    }
                }
                else
                {
                    policyId = claim.PolicyId;
                }

                if (userType.Code.ToLower() != "du")
                {
                    response.claimNotes = GetClaimNotesPolicyId(policyId);
                }

                response.claimComments = GetClaimPendingCommentsByClaimId(claimId, claim != null);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static ClaimCommentsResponseDto GetClaimPendingCommentsByClaimId(Guid claimId, bool isClaimAvailable)
        {
            ClaimCommentsResponseDto Response = new ClaimCommentsResponseDto();
            try
            {
                if (isClaimAvailable)
                {
                    Response.Comments = new List<ClaimCommentResponseDto>();
                    ISession session = EntitySessionManager.GetSession();
                    session.Query<ClaimComment>().Where(a => a.ClaimId == claimId)
                    .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                    .Join(session.Query<Claim>(), o => o.m.ClaimId, p => p.ClaimSubmissionId, (o, p) => new { o, p })
                    .Join(session.Query<InternalUser>(), q => q.o.m.SentFrom.ToString(), r => r.Id, (q, r) => new { q, r })
                    .OrderByDescending(or => or.q.o.m.EntryDateTime)
                    .ToList().ForEach(z => Response.Comments.Add(new ClaimCommentResponseDto()
                    {
                        ClaimNo = z.q.p.ClaimNumber,
                        EntryDateTime = z.q.o.m.EntryDateTime.ToString("dd-MMM-yyyy"),
                        Id = z.q.o.m.Id,
                        Comment = z.q.o.m.Comment,
                        PolicyNo = z.q.o.n.PolicyNo,
                        SentFrom = z.r.FirstName + " " + z.r.LastName,
                        ByTPA = z.q.o.m.ByTPA
                    }));
                }
                else
                {
                    Response.Comments = new List<ClaimCommentResponseDto>();
                    ISession session = EntitySessionManager.GetSession();
                    session.Query<ClaimComment>().Where(a => a.ClaimId == claimId)
                    .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                    .Join(session.Query<ClaimSubmission>(), o => o.m.ClaimId, p => p.Id, (o, p) => new { o, p })
                    .Join(session.Query<InternalUser>(), q => q.o.m.SentFrom.ToString(), r => r.Id, (q, r) => new { q, r })
                    .OrderByDescending(or => or.q.o.m.EntryDateTime)
                    .ToList().ForEach(z => Response.Comments.Add(new ClaimCommentResponseDto()
                    {
                        ClaimNo = z.q.p.ClaimNumber,
                        EntryDateTime = z.q.o.m.EntryDateTime.ToString("dd-MMM-yyyy"),
                        Id = z.q.o.m.Id,
                        Comment = z.q.o.m.Comment,
                        PolicyNo = z.q.o.n.PolicyNo,
                        SentFrom = z.r.FirstName + " " + z.r.LastName,
                        ByTPA = z.q.o.m.ByTPA
                    }));
                }



                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetAllOriginalPolicyDetailsByClaimId(Guid claimId)
        {
            ClaimOriginalPolicyDataResponseDto response = new ClaimOriginalPolicyDataResponseDto();
            if (!IsGuid(claimId.ToString()))
            {
                return response;
            }

            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                ClaimSubmission claimSubmission = session
                    .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);
                if (claimSubmission == null && claim == null)
                {
                    return response;
                }

                bool fromClaim = claim != null;
                var policyId = fromClaim ? claim.PolicyId : claimSubmission.PolicyId;

                Policy policy = session.Query<Policy>()
                    .FirstOrDefault(a => a.Id == policyId);
                if (policy == null)
                {
                    return response;
                }

                Product product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();

                if (product.Productcode == "TYRE")
                {

                    var policyData = session.Query<Policy>().Where(a => a.Id == policyId)
                   .Join(session.Query<Customer>(), m => m.CustomerId, n => n.Id, (m, n) => new { m, n })
                   .Join(session.Query<Contract>(), o => o.m.ContractId, p => p.Id, (o, p) => new { o, p })
                   .Join(session.Query<ReinsurerContract>(), q => q.p.ReinsurerContractId, r => r.Id, (q, r) => new { q, r })
                   .Join(session.Query<Insurer>(), s => s.r.InsurerId, t => t.Id, (s, t) => new { s, t })
                   .Join(session.Query<Reinsurer>(), u => u.s.r.ReinsurerId, v => v.Id, (u, v) => new { u, v })
                   .Join(session.Query<UsageType>(), w => w.u.s.q.o.n.UsageTypeId, x => x.Id, (w, x) => new { w, x })
                   .Join(session.Query<Dealer>(), k => k.w.u.s.q.o.m.DealerId, l => l.Id, (k, l) => new { k, l })
                   .Select(y => new
                   {
                       y.k.x.UsageTypeName,
                       y.k.w.v.ReinsurerName,
                       y.k.w.u.t.InsurerShortName,
                       y.k.w.u.s.r.UWYear,
                       CustomerName = y.k.w.u.s.q.o.n.FirstName + " " + y.k.w.u.s.q.o.n.LastName,
                       y.k.w.u.s.q.o.n.MobileNo,
                       y.l.DealerName,
                       y.k.w.u.s.q.o.m.PolicyNo
                   });
                    var ExtendedWarrentyData = new PolicyEntityManager().GetPolicyById2(policy.PolicyBundleId) as PolicyEntityManager.PolicyViewData;
                    decimal conversionRate = ExtendedWarrentyData.Vehicle != null ?
                        ExtendedWarrentyData.Vehicle.ConversionRate : ExtendedWarrentyData.BAndW.ConversionRate;
                    string currencyCode = commonEm.GetCurrencyCodeById(ExtendedWarrentyData.Vehicle != null ?
                        ExtendedWarrentyData.Vehicle.DealerCurrencyId : ExtendedWarrentyData.BAndW.DealerCurrencyId);
                    //string makeModel = commonEm.GetMakeNameById(ExtendedWarrentyData.Vehicle != null ?
                    //   ExtendedWarrentyData.BAndW.MakeId : ExtendedWarrentyData.Vehicle.MakeId) + " - " +
                    //    commonEm.GetModelNameById(ExtendedWarrentyData.Vehicle != null ?
                    //   ExtendedWarrentyData.BAndW.ModelId : ExtendedWarrentyData.Vehicle.ModelId);

                    InvoiceCodeDetails invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(b => b.Id == invoiceCodeDetails.InvoiceCodeId).FirstOrDefault();
                    CustomerEnterdInvoiceDetails CEID = session.Query<CustomerEnterdInvoiceDetails>().Where(c => c.InvoiceCodeId == invoiceCode.Id).FirstOrDefault();

                    string makeModel = commonEm.GetAdditionalMakeNameById(CEID.AdditionalDetailsMakeId) + " - " +
                        commonEm.GetAdditionalModelNameById(CEID.AdditionalDetailsModelId);
                    string ModelYear = CEID.AdditionalDetailsModelYear.ToString();

                    if (policyData != null && policyData.Count() > 0)
                    {
                        decimal mwkm = decimal.Zero;
                        var mwRes = decimal.TryParse(ExtendedWarrentyData.MWKM, out mwkm);
                        response = new ClaimOriginalPolicyDataResponseDto()
                        {
                            Cedent = policyData.FirstOrDefault().InsurerShortName,
                            CustomerName = policyData.FirstOrDefault().CustomerName,
                            MobileNumber = policyData.FirstOrDefault().MobileNo,
                            PolicyDealer = policyData.FirstOrDefault().DealerName,
                            PolicyNumber = policyData.FirstOrDefault().PolicyNo,
                            UWYear = policyData.FirstOrDefault().UWYear,
                            PolicyType = policyData.FirstOrDefault().UsageTypeName,
                            Reinsurer = policyData.FirstOrDefault().ReinsurerName,
                            CutoffMileage = ExtendedWarrentyData.KMCutOff.ToString("N0", CultureInfo.InvariantCulture) + " km",
                            ExtEnd = ExtendedWarrentyData.PolicyEndDate.ToString("dd-MMM-yyyy"),
                            ExtMileage = ExtendedWarrentyData.ExtKM.ToString("N0", CultureInfo.InvariantCulture) + " km",
                            ExtStart = ExtendedWarrentyData.PolicyStartDate.ToString("dd-MMM-yyyy"),
                            MWEnd = ExtendedWarrentyData.MWEnddate.ToString("dd-MMM-yyyy"),
                            MWMileage = mwRes ? mwkm.ToString("N0", CultureInfo.InvariantCulture) + " km" : "Unlimited",
                            MWStart = ExtendedWarrentyData.MWStartDate.ToString("dd-MMM-yyyy"),
                            MWMonths = ExtendedWarrentyData.MWMonths.ToString(),
                            EXTMonths = ExtendedWarrentyData.ExtMonths.ToString(),
                            ClaimNumber = claim == null ? "n/a" : claim.ClaimNumber,
                            CoBuyerName = "",
                            CoverageType = commonEm.GetCoverTypeNameById(ExtendedWarrentyData.CoverTypeId),
                            MakeModel = makeModel,
                            VIN = ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.VINNo : ExtendedWarrentyData.BAndW.SerialNo,
                            ModelYear = ModelYear,
                            CylinderCount = commonEm.GetCyllinderCountValueById(ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.CylinderCountId : Guid.Empty),
                            EngineCapacity = commonEm.GetEngineCapacityNameById(ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.EngineCapacityId : Guid.Empty),
                            PlateNumber = ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.PlateNo : "",
                            ProdMonth = "",
                            SalePrice = ExtendedWarrentyData.Vehicle != null ?
                                ExtendedWarrentyData.Vehicle.DealerPrice.ToString("N", CultureInfo.InvariantCulture) + " " + currencyCode
                                : ExtendedWarrentyData.BAndW.DealerPrice.ToString("N", CultureInfo.InvariantCulture) + " " + currencyCode,

                        };
                    }
                }
                else
                {
                    var policyData = session.Query<Policy>().Where(a => a.Id == policyId)
                   .Join(session.Query<Customer>(), m => m.CustomerId, n => n.Id, (m, n) => new { m, n })
                   .Join(session.Query<Contract>(), o => o.m.ContractId, p => p.Id, (o, p) => new { o, p })
                   .Join(session.Query<ReinsurerContract>(), q => q.p.ReinsurerContractId, r => r.Id, (q, r) => new { q, r })
                   .Join(session.Query<Insurer>(), s => s.r.InsurerId, t => t.Id, (s, t) => new { s, t })
                   .Join(session.Query<Reinsurer>(), u => u.s.r.ReinsurerId, v => v.Id, (u, v) => new { u, v })
                   .Join(session.Query<UsageType>(), w => w.u.s.q.o.n.UsageTypeId, x => x.Id, (w, x) => new { w, x })
                   .Join(session.Query<Dealer>(), k => k.w.u.s.q.o.m.DealerId, l => l.Id, (k, l) => new { k, l })
                   .Select(y => new
                   {
                       y.k.x.UsageTypeName,
                       y.k.w.v.ReinsurerName,
                       y.k.w.u.t.InsurerShortName,
                       y.k.w.u.s.r.UWYear,
                       CustomerName = y.k.w.u.s.q.o.n.FirstName + " " + y.k.w.u.s.q.o.n.LastName,
                       y.k.w.u.s.q.o.n.MobileNo,
                       y.l.DealerName,
                       y.k.w.u.s.q.o.m.PolicyNo
                   });
                    var ExtendedWarrentyData = new PolicyEntityManager().GetPolicyById2(policy.PolicyBundleId) as PolicyEntityManager.PolicyViewData;
                    decimal conversionRate = ExtendedWarrentyData.Vehicle != null ?
                        ExtendedWarrentyData.Vehicle.ConversionRate : ExtendedWarrentyData.BAndW.ConversionRate;
                    string currencyCode = commonEm.GetCurrencyCodeById(ExtendedWarrentyData.Vehicle != null ?
                        ExtendedWarrentyData.Vehicle.DealerCurrencyId : ExtendedWarrentyData.BAndW.DealerCurrencyId);

                    string makeModel;

                    if (ExtendedWarrentyData.Vehicle != null)
                    {
                        makeModel = commonEm.GetMakeNameById(ExtendedWarrentyData.Vehicle.MakeId) + " - " + commonEm.GetModelNameById(ExtendedWarrentyData.Vehicle.ModelId);
                    }
                    else {

                        makeModel = commonEm.GetMakeNameById(ExtendedWarrentyData.BAndW.MakeId) + " - " + commonEm.GetModelNameById(ExtendedWarrentyData.BAndW.ModelId);

                    }


                    if (policyData != null && policyData.Count() > 0)
                    {
                        decimal mwkm = decimal.Zero;
                        var mwRes = decimal.TryParse(ExtendedWarrentyData.MWKM, out mwkm);
                        response = new ClaimOriginalPolicyDataResponseDto()
                        {
                            Cedent = policyData.FirstOrDefault().InsurerShortName,
                            CustomerName = policyData.FirstOrDefault().CustomerName,
                            MobileNumber = policyData.FirstOrDefault().MobileNo,
                            PolicyDealer = policyData.FirstOrDefault().DealerName,
                            PolicyNumber = policyData.FirstOrDefault().PolicyNo,
                            UWYear = policyData.FirstOrDefault().UWYear,
                            PolicyType = policyData.FirstOrDefault().UsageTypeName,
                            Reinsurer = policyData.FirstOrDefault().ReinsurerName,
                            CutoffMileage = ExtendedWarrentyData.KMCutOff.ToString("N0", CultureInfo.InvariantCulture) + " km",
                            ExtEnd = ExtendedWarrentyData.ExtEndDate.ToString("dd-MMM-yyyy"),
                            ExtMileage = ExtendedWarrentyData.ExtKM.ToString("N0", CultureInfo.InvariantCulture) + " km",
                            ExtStart = ExtendedWarrentyData.ExtStartDate.ToString("dd-MMM-yyyy"),
                            MWEnd = ExtendedWarrentyData.MWEnddate.ToString("dd-MMM-yyyy"),
                            MWMileage = mwRes ? mwkm.ToString("N0", CultureInfo.InvariantCulture) + " km" : "Unlimited",
                            MWStart = ExtendedWarrentyData.MWStartDate.ToString("dd-MMM-yyyy"),
                            MWMonths = ExtendedWarrentyData.MWMonths.ToString(),
                            EXTMonths = ExtendedWarrentyData.ExtMonths.ToString(),
                            ClaimNumber = claim == null ? "n/a" : claim.ClaimNumber,
                            CoBuyerName = "",
                            CoverageType = commonEm.GetCoverTypeNameById(ExtendedWarrentyData.CoverTypeId),
                            MakeModel = makeModel,
                            VIN = ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.VINNo : ExtendedWarrentyData.BAndW.SerialNo,
                            ModelYear = ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.ModelYear : ExtendedWarrentyData.BAndW.ModelYear,
                            CylinderCount = commonEm.GetCyllinderCountValueById(ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.CylinderCountId : Guid.Empty),
                            EngineCapacity = commonEm.GetEngineCapacityNameById(ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.EngineCapacityId : Guid.Empty),
                            PlateNumber = ExtendedWarrentyData.Vehicle != null ? ExtendedWarrentyData.Vehicle.PlateNo : "",
                            ProdMonth = "",
                            SalePrice = ExtendedWarrentyData.Vehicle != null ?
                                ExtendedWarrentyData.Vehicle.DealerPrice.ToString("N", CultureInfo.InvariantCulture) + " " + currencyCode
                                : ExtendedWarrentyData.BAndW.DealerPrice.ToString("N", CultureInfo.InvariantCulture) + " " + currencyCode,

                        };
                    }
                }


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GoodwillAuthorizationByUserId(Guid userId)
        {
            bool Response = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response = session.Query<SystemUser>().Where(a => a.LoginMapId == userId)
                    .Join(session.Query<UserRole>(), m => m.RoleId, n => n.RoleId, (m, n) => new { m, n })
                    .FirstOrDefault().n.IsClaimAuthorized;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object SaveAttachmentsToClaim(SaveAttachmentsToClaimRequestDto saveAttachmentsToClaimRequestDto)
        {
            GenericCodeMsgResponse Response = new GenericCodeMsgResponse();
            try
            {
                if (saveAttachmentsToClaimRequestDto == null ||
                    saveAttachmentsToClaimRequestDto.claimId == Guid.Empty ||
                    saveAttachmentsToClaimRequestDto.docIds == null)
                {
                    Response.code = "error";
                    Response.msg = "Request data incorrect.";
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                List<ClaimAttachment> claimAttachments = new List<ClaimAttachment>();
                foreach (Guid docId in saveAttachmentsToClaimRequestDto.docIds)
                {
                    ClaimAttachment claimAttachment = new ClaimAttachment()
                    {
                        ClaimId = saveAttachmentsToClaimRequestDto.claimId,
                        UserAttachmentId = docId,
                        Id = Guid.NewGuid(),
                        DateOfAttachment = DateTime.UtcNow,
                    };
                    claimAttachments.Add(claimAttachment);
                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (var attachment in claimAttachments)
                    {
                        session.Evict(attachment);
                        session.Save(attachment, attachment.Id);
                    }

                    transaction.Commit();
                }
                Response.code = "success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetAllClaimStatus()
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response = session.Query<ClaimStatusCode>()
                      .OrderBy(a => a.DisplayOrder)
                    .Select(a => new
                    {
                        a.Description,
                        a.Id,
                        a.StatusCode
                    }).ToArray();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object AddNewPartByDealer(AddNewPartByDealerRequestDto addNewPartRequestDto)
        {
            GenericCodeMsgResponse response = new GenericCodeMsgResponse();
            try
            {
                //validation
                if (addNewPartRequestDto == null || !IsGuid(addNewPartRequestDto.CommodityId.ToString()) ||
                    !IsGuid(addNewPartRequestDto.DealerId.ToString()) || !IsGuid(addNewPartRequestDto.MakeId.ToString()) ||
                    !IsGuid(addNewPartRequestDto.PartAreaId.ToString()) || string.IsNullOrEmpty(addNewPartRequestDto.PartCode) ||
                    string.IsNullOrEmpty(addNewPartRequestDto.PartName) || string.IsNullOrEmpty(addNewPartRequestDto.PartNumber))
                {
                    response.code = "error";
                    response.msg = "Request data invalid.";
                    return response;
                }
                //currency validation
                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                CommonEntityManager commonEntityManager = new CommonEntityManager();

                ISession session = EntitySessionManager.GetSession();
                Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == addNewPartRequestDto.DealerId);
                if (dealer == null)
                {
                    response.code = "error";
                    response.msg = "Dealer not found in the database.";
                    return response;
                }
                else if (dealer.CurrencyId == Guid.Empty)
                {
                    response.code = "error";
                    response.msg = "Dealer currency not set.";
                    return response;
                }
                else if (dealer.CountryId == Guid.Empty)
                {
                    response.code = "error";
                    response.msg = "Dealer country not set.";
                    return response;
                }

                Guid currentCurrencyPreiodId = currencyEntityManager.GetCurrentCurrencyPeriodId();
                if (currentCurrencyPreiodId == Guid.Empty)
                {
                    response.code = "error";
                    response.msg = "No currency period is defined for today.";
                    return response;
                }

                bool rate = currencyEntityManager.CheckConversionRate(dealer.CurrencyId, currentCurrencyPreiodId);
                if (!rate)
                {
                    response.code = "error";
                    response.msg = "Dealer currency (" + commonEntityManager.GetCurrencyCodeById(dealer.CurrencyId) + " is not found in current conversion period.";
                    return response;
                }

                //if exist validation
                Part existingPart = session.Query<Part>()
                    .FirstOrDefault(a => a.MakeId == addNewPartRequestDto.MakeId && addNewPartRequestDto.PartAreaId == addNewPartRequestDto.PartAreaId &&
                    a.CommodityId == addNewPartRequestDto.CommodityId && a.PartCode.ToLower() == addNewPartRequestDto.PartCode.ToLower());
                if (existingPart != null)
                {
                    response.code = "error";
                    response.msg = "Entered part already exist.";
                    return response;
                }

                Part newPart = new Part()
                {
                    Id = Guid.NewGuid(),
                    AllocatedHours = addNewPartRequestDto.AllocatedHours,
                    ApplicableForAllModels = addNewPartRequestDto.ApplicableForAllModels,
                    CommodityId = addNewPartRequestDto.CommodityId,
                    EntryDateTime = DateTime.UtcNow,
                    IsActive = addNewPartRequestDto.IsActive,
                    MakeId = addNewPartRequestDto.MakeId,
                    PartAreaId = addNewPartRequestDto.PartAreaId,
                    PartCode = addNewPartRequestDto.PartCode,
                    PartName = addNewPartRequestDto.PartName,
                    PartNumber = addNewPartRequestDto.PartNumber,
                    EntryBy = addNewPartRequestDto.EntryBy
                };

                PartPrice newPartPrice = new PartPrice()
                {
                    ConversionRate = currencyEntityManager.GetConversionRate(dealer.CurrencyId, currentCurrencyPreiodId),
                    CountryId = dealer.CountryId,
                    CurrencyId = dealer.CurrencyId,
                    CurrencyPeriodId = currentCurrencyPreiodId,
                    DealerId = dealer.Id,
                    Id = Guid.NewGuid(),
                    PartId = newPart.Id,
                    Price = currencyEntityManager.ConvertToBaseCurrency(addNewPartRequestDto.Price, dealer.CurrencyId, currentCurrencyPreiodId)
                };

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(newPart);
                    session.Save(newPart, newPart.Id);

                    session.Evict(newPartPrice);
                    session.Save(newPartPrice, newPartPrice.Id);

                    transaction.Commit();
                }
                response.code = "ok";

            }
            catch (Exception ex)
            {
                response.code = "error";
                response.msg = "Error occured while saving part details.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object ValidateVinNumber(string vinSerialNumer, Guid commodityTypeId, Guid dealerId,Guid productId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                #region "Validation"
                if (string.IsNullOrEmpty(vinSerialNumer))
                {
                    response.code = "error";
                    response.msg = "Invalid VIN/Serial.";
                    return response;
                }
                if (!IsGuid(commodityTypeId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Commodity Type.";
                    return response;
                }
                if (!IsGuid(dealerId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Dealer.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                string commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);
                List<Guid> allowedMakes = session.Query<DealerMakes>()
                   .Where(a => a.DealerId == dealerId)
                   .Select(a => a.MakeId).ToList();

                //create objects for further use
                VehicleDetails vehicleDetails = new VehicleDetails();
                BrownAndWhiteDetails brownAndWhiteDetails = new BrownAndWhiteDetails();
                OtherItemDetails otherItemDetails = new OtherItemDetails();
                YellowGoodDetails yellowGoodDetails = new YellowGoodDetails();
                Guid makeId, modelId, commodityCategoryId;
                string plateNo;
                //looking for make and vin match
                if (commodityCode.ToLower() == "a")
                {
                    vehicleDetails = session.Query<VehicleDetails>()
                        .FirstOrDefault(a => a.VINNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (vehicleDetails == null)
                    {
                        response.code = "error";
                        response.msg = "VIN Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == vehicleDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered vehicle's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = vehicleDetails.MakeId;
                    modelId = vehicleDetails.ModelId;
                    commodityCategoryId = vehicleDetails.CategoryId;
                    plateNo = vehicleDetails.PlateNo;
                }
                else if (commodityCode.ToLower() == "b")
                {
                    vehicleDetails = session.Query<VehicleDetails>()
                        .FirstOrDefault(a => a.VINNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (vehicleDetails == null)
                    {
                        response.code = "error";
                        response.msg = "VIN Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == vehicleDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered vehicle's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = vehicleDetails.MakeId;
                    modelId = vehicleDetails.ModelId;
                    commodityCategoryId = vehicleDetails.CategoryId;
                }
                else if (commodityCode.ToLower() == "e")
                {
                    brownAndWhiteDetails = session.Query<BrownAndWhiteDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (brownAndWhiteDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == brownAndWhiteDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = brownAndWhiteDetails.MakeId;
                    modelId = brownAndWhiteDetails.ModelId;
                    commodityCategoryId = brownAndWhiteDetails.CategoryId;

                }
                else if (commodityCode.ToLower() == "o")
                {
                    otherItemDetails = session.Query<OtherItemDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (otherItemDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == otherItemDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = otherItemDetails.MakeId;
                    modelId = otherItemDetails.ModelId;
                    commodityCategoryId = otherItemDetails.CategoryId;
                }
                else if (commodityCode.ToLower() == "y")
                {
                    yellowGoodDetails = session.Query<YellowGoodDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (yellowGoodDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == yellowGoodDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }

                    makeId = yellowGoodDetails.MakeId;
                    modelId = yellowGoodDetails.ModelId;
                    commodityCategoryId = yellowGoodDetails.CategoryId;
                }
                else
                {
                    response.code = "error";
                    response.msg = "Invalid commodity code.";
                    return response;
                }

                //looking for policy details
                List<VehiclePolicy> vehiclePolicy = new List<VehiclePolicy>();
                List<BAndWPolicy> brownAndWhitePolicy = new List<BAndWPolicy>();
                List<OtherItemPolicy> otherItemPolicy = new List<OtherItemPolicy>();
                List<YellowGoodPolicy> yellowGoodPolicy = new List<YellowGoodPolicy>();
                List<Guid> eligiblePolicyNubers = new List<Guid>();
                IList<Policy> eligiblePolicies = new List<Policy>();

                if (commodityCode.ToLower() == "a")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.VehicleId == vehicleDetails.Id)
                         .Join(session.Query<Policy>(), b => b.PolicyId, c => c.Id, (b, c) => new { b, c })
                         .Where(p=> p.c.ProductId==productId)
                         .Select(s=>s.b)
                        .ToList();
                    if (vehiclePolicy == null || vehiclePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd vehicle hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(vehiclePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "b")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.VehicleId == vehicleDetails.Id)
                         .Join(session.Query<Policy>(), b => b.PolicyId, c => c.Id, (b, c) => new { b, c })
                         .Where(p => p.c.ProductId == productId)
                         .Select(s => s.b)
                        .ToList();
                    if (vehiclePolicy == null || vehiclePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd vehicle hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(vehiclePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "e")
                {
                    brownAndWhitePolicy = session.Query<BAndWPolicy>()
                        .Where(a => a.BAndWId == brownAndWhiteDetails.Id)
                         .Join(session.Query<Policy>(), b => b.PolicyId, c => c.Id, (b, c) => new { b, c })
                         .Where(p => p.c.ProductId == productId)
                         .Select(s => s.b)
                        .ToList();
                    if (brownAndWhitePolicy == null || brownAndWhitePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(brownAndWhitePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "o")
                {
                    otherItemPolicy = session.Query<OtherItemPolicy>()
                       .Where(a => a.OtherItemId == otherItemDetails.Id)
                       .Join(session.Query<Policy>(), b => b.PolicyId, c => c.Id, (b, c) => new { b, c })
                         .Where(p => p.c.ProductId == productId)
                         .Select(s => s.b)
                       .ToList();
                    if (otherItemPolicy == null || otherItemPolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(otherItemPolicy.Select(a => a.PolicyId));
                }
                else
                {
                    yellowGoodPolicy = session.Query<YellowGoodPolicy>()
                      .Where(a => a.YellowGoodId == yellowGoodDetails.Id)
                      .Join(session.Query<Policy>(), b => b.PolicyId, c => c.Id, (b, c) => new { b, c })
                         .Where(p => p.c.ProductId == productId)
                         .Select(s => s.b)
                      .ToList();
                    if (yellowGoodPolicy == null || yellowGoodPolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(yellowGoodPolicy.Select(a => a.PolicyId));
                }

                //policy validation
                if (eligiblePolicyNubers.Any())
                {
                    eligiblePolicies = session.QueryOver<Policy>()
                        .WhereRestrictionOn(a => a.Id)
                        .IsIn(eligiblePolicyNubers)
                        .List<Policy>();
                    //policy status validation
                    eligiblePolicies = eligiblePolicies
                        .Where(a => a.IsApproved == true && a.IsPolicyCanceled != true)
                        .ToList();
                    if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any active policy.";
                        return response;
                    }
                }
                else
                {
                    response.code = "error";
                    response.msg = "Enterd Item hasn't associated with any policy.";
                    return response;
                }


                #endregion
                response.code = "ok";
                var vehiclepolicyinfo = vehiclePolicy.FirstOrDefault();
                var Policyinfo = session.Query<Policy>()
                      .FirstOrDefault(a => a.Id == vehiclepolicyinfo.PolicyId);
                var Productinfo = session.Query<Product>()
                       .FirstOrDefault(a => a.Id== Policyinfo.ProductId);
                var ProductTypeinfo = session.Query<ProductType>()
                       .FirstOrDefault(a => a.Id == Productinfo.ProductTypeId);
                PolicyEntityManager pem = new PolicyEntityManager();

                Contract contract = new Contract();
                ContractInsuaranceLimitation contractInsuaranceLimitation = new ContractInsuaranceLimitation();
                ContractExtensions contractExtensions = new ContractExtensions();
                InsuaranceLimitation insuaranceLimitation = new InsuaranceLimitation();
                List<LoanInstallmentILOEResponseDto> loanInstallmentILOEResponseDto = new List<LoanInstallmentILOEResponseDto>();

                int loneinstallmentValue = 0;
                //int LoneInstallment = 0;
                //List<LoneInstallment> lones = new List<LoneInstallment>();

                if (eligiblePolicies != null)
                {
                    contract = session.Query<Contract>().Where(a => a.Id == eligiblePolicies.First().ContractId).FirstOrDefault();
                    contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == contract.Id).FirstOrDefault();
                    contractExtensions = session.Query<ContractExtensions>()
                        .Where(a => a.ContractInsuanceLimitationId == contractInsuaranceLimitation.Id).FirstOrDefault();
                    insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .Where(a => a.Id == contractInsuaranceLimitation.InsuaranceLimitationId).FirstOrDefault();

                    if (insuaranceLimitation != null)
                    {
                        DateTime PolicySoldDate = eligiblePolicies.FirstOrDefault().PolicySoldDate;
                        DateTime TodayDate = DateTime.Now;
                        //int Months = (PolicySoldDate.Year - TodayDate.Year) * 12 + TodayDate.Month - PolicySoldDate.Month;
                        TimeSpan span = TodayDate - PolicySoldDate;
                        int Months = (TodayDate.Month - PolicySoldDate.Month) + 12 * (TodayDate.Year - PolicySoldDate.Year);
                        int installmetCount = 0;

                        if(Months >= 6)
                            installmetCount = 6;
                        else
                            installmetCount = Months;


                        if (Months == 1)
                        {
                            LoanInstallmentILOEResponseDto loanInstallmenteDto = new LoanInstallmentILOEResponseDto()
                            {
                                Id = 1,
                                Installment = Months
                            };
                            loanInstallmentILOEResponseDto.Add(loanInstallmenteDto);
                        }
                        else
                        {

                            int loneMonth = Months;
                            int IdCount = 0;
                            for (int i = 0; i < installmetCount; i++)
                            {
                                    IdCount = IdCount + 1;
                                    loneMonth = loneMonth + 1;
                                    LoanInstallmentILOEResponseDto loanInstallmenteDto = new LoanInstallmentILOEResponseDto()
                                    {
                                        Id = IdCount,
                                        Installment = loneMonth
                                    };
                                    loanInstallmentILOEResponseDto.Add(loanInstallmenteDto);
                            }
                        }

                    }

                }


                if (ProductTypeinfo.Code != "ILOE")
                {

                    response.obj = eligiblePolicies.Select(a => new
                    {
                        a.Id,
                        a.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(a.ProductId),
                        CommodityCategoryId = commodityCategoryId,
                        MakeId = makeId,
                        ModelId = modelId,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(a.CustomerId),
                        plateNo = vehicleDetails.PlateNo,
                        Milage=a.HrsUsedAtPolicySale
                    }).ToArray();
                }
                else {

                    response.obj = eligiblePolicies.Select(a => new
                    {
                        a.Id,
                        a.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(a.ProductId),
                        CommodityCategoryId = commodityCategoryId,
                        MakeId = makeId,
                        ModelId = modelId,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(a.CustomerId),
                        LoneInstallmentValue = loanInstallmentILOEResponseDto,
                        ClaimLimitation = contract.ClaimLimitation * contract.ConversionRate,
                        CalculateEMI = pem.GetEMIValue(vehicleDetails.DealerPrice * vehicleDetails.ConversionRate, Policyinfo.ContractId),
                        Milage = a.HrsUsedAtPolicySale
                    }).ToArray();

                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }



        internal static object ValidateVinSerialNumberAndGetPolicyDetails(string vinSerialNumer, Guid commodityTypeId, Guid dealerId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                #region "Validation"
                if (string.IsNullOrEmpty(vinSerialNumer))
                {
                    response.code = "error";
                    response.msg = "Invalid VIN/Serial.";
                    return response;
                }
                if (!IsGuid(commodityTypeId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Commodity Type.";
                    return response;
                }
                if (!IsGuid(dealerId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Dealer.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                string commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);
                List<Guid> allowedMakes = session.Query<DealerMakes>()
                   .Where(a => a.DealerId == dealerId)
                   .Select(a => a.MakeId).ToList();

                //create objects for further use
                VehicleDetails vehicleDetails = new VehicleDetails();
                BrownAndWhiteDetails brownAndWhiteDetails = new BrownAndWhiteDetails();
                OtherItemDetails otherItemDetails = new OtherItemDetails();
                YellowGoodDetails yellowGoodDetails = new YellowGoodDetails();
                Contract contract = new Contract();
                Guid makeId, modelId, commodityCategoryId;
                //looking for make and vin match
                if (commodityCode.ToLower() == "a")
                {
                    vehicleDetails = session.Query<VehicleDetails>()
                        .FirstOrDefault(a => a.VINNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (vehicleDetails == null)
                    {
                        response.code = "error";
                        response.msg = "VIN Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == vehicleDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered vehicle's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = vehicleDetails.MakeId;
                    modelId = vehicleDetails.ModelId;
                    commodityCategoryId = vehicleDetails.CategoryId;
                }
                else if (commodityCode.ToLower() == "b")
                {
                    vehicleDetails = session.Query<VehicleDetails>()
                        .FirstOrDefault(a => a.VINNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (vehicleDetails == null)
                    {
                        response.code = "error";
                        response.msg = "VIN Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == vehicleDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered vehicle's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = vehicleDetails.MakeId;
                    modelId = vehicleDetails.ModelId;
                    commodityCategoryId = vehicleDetails.CategoryId;
                }
                else if (commodityCode.ToLower() == "e")
                {
                    brownAndWhiteDetails = session.Query<BrownAndWhiteDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (brownAndWhiteDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == brownAndWhiteDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = brownAndWhiteDetails.MakeId;
                    modelId = brownAndWhiteDetails.ModelId;
                    commodityCategoryId = brownAndWhiteDetails.CategoryId;

                }
                else if (commodityCode.ToLower() == "o")
                {
                    otherItemDetails = session.Query<OtherItemDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (otherItemDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == otherItemDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }
                    makeId = otherItemDetails.MakeId;
                    modelId = otherItemDetails.ModelId;
                    commodityCategoryId = otherItemDetails.CategoryId;
                }
                else if (commodityCode.ToLower() == "y")
                {
                    yellowGoodDetails = session.Query<YellowGoodDetails>()
                        .FirstOrDefault(a => a.SerialNo.ToLower().Trim() == vinSerialNumer.ToLower().Trim());
                    if (yellowGoodDetails == null)
                    {
                        response.code = "error";
                        response.msg = "Serial Number not found in the database.";
                        return response;
                    }

                    if (!allowedMakes.Any(a => a == yellowGoodDetails.MakeId))
                    {
                        response.code = "error";
                        response.msg = "Entered item's make not allowed for logged in dealer.";
                        return response;
                    }

                    makeId = yellowGoodDetails.MakeId;
                    modelId = yellowGoodDetails.ModelId;
                    commodityCategoryId = yellowGoodDetails.CategoryId;
                }
                else
                {
                    response.code = "error";
                    response.msg = "Invalid commodity code.";
                    return response;
                }

                //looking for policy details
                List<VehiclePolicy> vehiclePolicy = new List<VehiclePolicy>();
                List<BAndWPolicy> brownAndWhitePolicy = new List<BAndWPolicy>();
                List<OtherItemPolicy> otherItemPolicy = new List<OtherItemPolicy>();
                List<YellowGoodPolicy> yellowGoodPolicy = new List<YellowGoodPolicy>();
                List<Guid> eligiblePolicyNubers = new List<Guid>();
                IList<Policy> eligiblePolicies = new List<Policy>();

                if (commodityCode.ToLower() == "a")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.VehicleId == vehicleDetails.Id).ToList();
                    if (vehiclePolicy == null || vehiclePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd vehicle hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(vehiclePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "b")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.VehicleId == vehicleDetails.Id).ToList();
                    if (vehiclePolicy == null || vehiclePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd vehicle hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(vehiclePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "e")
                {
                    brownAndWhitePolicy = session.Query<BAndWPolicy>()
                        .Where(a => a.BAndWId == brownAndWhiteDetails.Id).ToList();
                    if (brownAndWhitePolicy == null || brownAndWhitePolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(brownAndWhitePolicy.Select(a => a.PolicyId));
                }
                else if (commodityCode.ToLower() == "o")
                {
                    otherItemPolicy = session.Query<OtherItemPolicy>()
                       .Where(a => a.OtherItemId == otherItemDetails.Id).ToList();
                    if (otherItemPolicy == null || otherItemPolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(otherItemPolicy.Select(a => a.PolicyId));
                }
                else
                {
                    yellowGoodPolicy = session.Query<YellowGoodPolicy>()
                      .Where(a => a.YellowGoodId == yellowGoodDetails.Id).ToList();
                    if (yellowGoodPolicy == null || yellowGoodPolicy.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(yellowGoodPolicy.Select(a => a.PolicyId));
                }

                //policy validation
                if (eligiblePolicyNubers.Any())
                {
                    eligiblePolicies = session.QueryOver<Policy>()
                        .WhereRestrictionOn(a => a.Id)
                        .IsIn(eligiblePolicyNubers)
                        .List<Policy>();
                    //policy status validation
                    eligiblePolicies = eligiblePolicies
                        .Where(a => a.IsApproved == true && a.IsPolicyCanceled != true)
                        .ToList();
                    if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any active policy.";
                        return response;
                    }
                }
                else
                {
                    response.code = "error";
                    response.msg = "Enterd Item hasn't associated with any policy.";
                    return response;
                }

                ContractInsuaranceLimitation contractInsuaranceLimitation = new ContractInsuaranceLimitation();
                ContractExtensions contractExtensions = new ContractExtensions();
                InsuaranceLimitation insuaranceLimitation = new InsuaranceLimitation();
                List<int> vs = new List<int>();
                //int LoneInstallment = 0;
                //List<LoneInstallment> lones = new List<LoneInstallment>();

                if (eligiblePolicies != null)
                {
                    contract = session.Query<Contract>().Where(a => a.Id == eligiblePolicies.First().ContractId).FirstOrDefault();
                    contractInsuaranceLimitation = session.Query<ContractInsuaranceLimitation>().Where(a => a.ContractId == contract.Id).FirstOrDefault();
                    contractExtensions = session.Query<ContractExtensions>()
                        .Where(a => a.ContractInsuanceLimitationId == contractInsuaranceLimitation.Id).FirstOrDefault();
                    insuaranceLimitation = session.Query<InsuaranceLimitation>()
                        .Where(a => a.Id == contractInsuaranceLimitation.InsuaranceLimitationId).FirstOrDefault();

                    if(insuaranceLimitation != null)
                    {
                        DateTime PolicySoldDate = eligiblePolicies.FirstOrDefault().PolicySoldDate;
                        DateTime TodayDate = DateTime.Now;
                        int Months = ((PolicySoldDate.Year - TodayDate.Year) * 12) + PolicySoldDate.Month - TodayDate.Month;

                        if(Months == 0)
                        {
                            int value1 = Months + 1;
                            vs.Add(value1);
                        }
                        else
                        {
                            int value1 = Months;
                            vs.Add(value1);
                        }

                    }

                }


                #endregion
                response.code = "ok";
                var vehiclepolicyinfo = vehiclePolicy.FirstOrDefault();
                var Policyinfo = session.Query<Policy>()
                      .FirstOrDefault(a => a.Id == vehiclepolicyinfo.PolicyId);
                var Productinfo = session.Query<Product>()
                       .FirstOrDefault(a => a.Id == Policyinfo.ProductId);
                var ProductTypeinfo = session.Query<ProductType>()
                       .FirstOrDefault(a => a.Id == Productinfo.ProductTypeId);
                PolicyEntityManager pem = new PolicyEntityManager();

                if (ProductTypeinfo.Code != "ILOE")
                {

                    response.obj = eligiblePolicies.Select(a => new
                    {
                        a.Id,
                        a.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(a.ProductId),
                        CommodityCategoryId = commodityCategoryId,
                        MakeId = makeId,
                        ModelId = modelId,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(a.CustomerId),

                        //GetEMI = pem.GetEMIValue(a.FinanceAmount * vehicleDetails.ConversionRate ,a.ContractId)
                    }).ToArray();
                }
                else
                {

                    response.obj = eligiblePolicies.Select(a => new
                    {
                        a.Id,
                        a.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(a.ProductId),
                        CommodityCategoryId = commodityCategoryId,
                        MakeId = makeId,
                        ModelId = modelId,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(a.CustomerId),
                        LoneInstallment = vs,
                        CalculateEMI = pem.GetEMIValue(vehicleDetails.DealerPrice * vehicleDetails.ConversionRate, Policyinfo.ContractId)
                    }).ToArray();

                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }


        internal static object ProductSearchOnClaimSubmissioniloe(ProductSearchInClaimSubmissionIloeRequestDto ProductSearchInClaimSubmissionIloeRequestDto)
        {
            object Response = null;
            try
            {
                if (ProductSearchInClaimSubmissionIloeRequestDto == null
                    || !IsGuid(ProductSearchInClaimSubmissionIloeRequestDto.userId.ToString()))
                {
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                if (ProductSearchInClaimSubmissionIloeRequestDto.userType.ToLower() == "du")
                {
                    DealerStaff DealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == ProductSearchInClaimSubmissionIloeRequestDto.userId).FirstOrDefault();
                    if (DealerStaff == null)
                    {
                        Response = "You haven't assign to any Dealer";
                        return Response;
                    }


                    List<Guid> dealerMakesIdList = session.Query<DealerMakes>()
                        .Where(a => a.DealerId == DealerStaff.DealerId).Select(a => a.MakeId).ToList();
                    if (!dealerMakesIdList.Any())
                    {
                        Response = "No make is assigned to your Dealer. ";
                        return Response;
                    }

                    #region dynamic column filtering
                    //make filtering
                    IEnumerable<ContractExtensionMake> extensionsToMakes = session.Query<ContractExtensionMake>()
                        .Where(a => dealerMakesIdList.Contains(a.MakeId));
                    IEnumerable<ContractExtensions> contractExtensions = session.Query<ContractExtensions>()
                        .Where(a => extensionsToMakes.Any(b => b.ContractExtensionId == a.Id));
                    List<Guid> contractIds = contractExtensions.Select(a => a.Id).Distinct().ToList();

                    //customer filtering
                    List<Guid> CustomerIds = new List<Guid>();
                    if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName)
                        || !String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName))
                    {
                        IEnumerable<Customer> customers;
                        if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName))
                        {
                            customers = session.Query<Customer>()
                              .Where(a => a.FirstName.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName.ToLower()) ||
                               a.LastName.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName.ToLower()));
                        }
                        else
                        {
                            customers = session.Query<Customer>()
                              .Where(a => a.FirstName.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName.ToLower()) ||
                               a.LastName.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName.ToLower()));
                        }

                        CustomerIds = customers.Select(a => a.Id).Distinct().ToList();
                    }

                    //serial/vin filtering
                    List<Guid> poilicyIds = new List<Guid>();
                    if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.serialNo))
                    {
                        List<Guid> vehicleIds = session.Query<VehicleDetails>()
                            .Where(a => a.VINNo.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.serialNo.ToLower()))
                            .Select(a => a.Id).ToList();

                        var policyIdsAuto = session.Query<VehiclePolicy>()
                            .Where(a => vehicleIds.Contains(a.VehicleId)).Select(a => a.PolicyId).ToList();
                        if (policyIdsAuto.Count() > 0)
                        {
                            poilicyIds.AddRange(policyIdsAuto);
                        }

                    }

                    #endregion dynamic column filtering

                    #region policy data expression building
                    //policy filtering
                    Expression<Func<Policy, bool>> filterByPolicy = PredicateBuilder.True<Policy>();
                    filterByPolicy = filterByPolicy.And(a => a.IsPolicyCanceled != true && a.IsPolicyRenewed != true);
                    if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.policyNo))
                    {
                        filterByPolicy = filterByPolicy.And(a => a.PolicyNo.ToLower().Contains(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.policyNo.ToLower()));
                    }
                    filterByPolicy = filterByPolicy.And(a => a.ProductId == ProductSearchInClaimSubmissionIloeRequestDto.productId);
                    filterByPolicy = filterByPolicy.And(a => contractIds.Contains(a.ContractInsuaranceLimitationId));

                    if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.customerName))
                    {
                        filterByPolicy = filterByPolicy.And(a => CustomerIds.Contains(a.CustomerId));
                    }

                    if (!String.IsNullOrEmpty(ProductSearchInClaimSubmissionIloeRequestDto.productSearchGridSearchCriterias.serialNo))
                    {
                        filterByPolicy = filterByPolicy.And(a => poilicyIds.Contains(a.Id));
                    }

                    #endregion policy data expression building


                    //main filter for policy table
                    var policyGridDetails = from policy in session.Query<Policy>().Where(filterByPolicy)
                                            join vehiclePolicy in session.Query<VehiclePolicy>() on policy.Id equals vehiclePolicy.PolicyId
                                            join vehiclalDetails in session.Query<VehicleDetails>() on vehiclePolicy.VehicleId equals vehiclalDetails.Id
                                            select new { policy = policy, vehiclePolicy = vehiclePolicy, vehiclalDetails = vehiclalDetails };

                    //grid paging setup & joining other tables
                    long TotalRecords = policyGridDetails.Count();
                    var policyGridDetailsFilterd = policyGridDetails.OrderByDescending(a => a.policy.EntryDateTime).Skip((ProductSearchInClaimSubmissionIloeRequestDto.paginationOptionsProductSearchGrid.pageNumber - 1) * (ProductSearchInClaimSubmissionIloeRequestDto.paginationOptionsProductSearchGrid.pageSize))
                    .Take(ProductSearchInClaimSubmissionIloeRequestDto.paginationOptionsProductSearchGrid.pageSize);

                    PolicyEntityManager pem = new PolicyEntityManager();
                    CommonEntityManager cem = new CommonEntityManager();
                    var fullPolicyGridDetailsFilterd = policyGridDetailsFilterd
                     .Select(a => new
                     {
                         Id = a.policy.Id,
                         CommodityTypeId = a.policy.CommodityTypeId,
                         CommodityType = cem.GetCommodityTypeNameById(a.policy.CommodityTypeId),
                         PolicyNo = a.policy.PolicyNo,
                         SerialNo = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(a.policy.Id, a.policy.CommodityTypeId, "serial"),
                         CommodityCategoryId = cem.GetCommodityCategoryIdByContractId(a.policy.ContractId),
                         CustomerName = cem.GetCustomerNameById(a.policy.CustomerId),
                         CalculateEMI = pem.GetEMIValue(a.policy.FinanceAmount * a.vehiclalDetails.ConversionRate, a.policy.ContractId)
                     }).ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = fullPolicyGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);
                }
                else
                {
                    Response = "Dealer's users can only access this page.";
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object PolicySearchOnClaimSubmission(PolicySearchInClaimSubmissionRequestDto PolicySearchInClaimSubmissionRequestDto)
        {
            object Response = null;
            try
            {
                if (PolicySearchInClaimSubmissionRequestDto == null
                    || !IsGuid(PolicySearchInClaimSubmissionRequestDto.userId.ToString()))
                {
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                if (PolicySearchInClaimSubmissionRequestDto.userType.ToLower() == "du")
                {
                    DealerStaff DealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == PolicySearchInClaimSubmissionRequestDto.userId).FirstOrDefault();
                    if (DealerStaff == null)
                    {
                        Response = "You haven't assign to any Dealer";
                        return Response;
                    }


                    List<Guid> dealerMakesIdList = session.Query<DealerMakes>()
                        .Where(a => a.DealerId == DealerStaff.DealerId).Select(a => a.MakeId).ToList();
                    if (!dealerMakesIdList.Any())
                    {
                        Response = "No make is assigned to your Dealer. ";
                        return Response;
                    }

                    #region dynamic column filtering
                    //make filtering
                    IEnumerable<ContractExtensionMake> extensionsToMakes = session.Query<ContractExtensionMake>()
                        .Where(a => dealerMakesIdList.Contains(a.MakeId));
                    IEnumerable<ContractExtensions> contractExtensions = session.Query<ContractExtensions>()
                        .Where(a => extensionsToMakes.Any(b => b.ContractExtensionId == a.Id));
                    List<Guid> contractIds = contractExtensions.Select(a => a.Id).Distinct().ToList();

                    //customer filtering
                    List<Guid> CustomerIds = new List<Guid>();
                    if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName)
                        || !String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber))
                    {
                        IEnumerable<Customer> customers;
                        if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName)
                            && String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber))
                        {
                            customers = session.Query<Customer>()
                                .Where(a =>
                                PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower().Contains(a.FirstName.ToLower()) ||
                                PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower().Contains(a.LastName.ToLower()) ||
                                a.FirstName.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower()) ||
                                a.LastName.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower()));
                        }
                        else if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName)
                           && !String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber))
                        {
                            customers = session.Query<Customer>()
                               .Where(a =>
                               (PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower().Contains(a.FirstName.ToLower()) ||
                               PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower().Contains(a.LastName.ToLower()) ||
                               a.FirstName.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower()) ||
                               a.LastName.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower()))
                               && a.MobileNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber.ToLower()));
                        }
                        else
                        {
                            customers = session.Query<Customer>()
                              .Where(a => a.MobileNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber.ToLower()));
                        }

                        CustomerIds = customers.Select(a => a.Id).Distinct().ToList();
                    }

                    //serial/vin filtering
                    List<Guid> poilicyIds = new List<Guid>();
                    if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo))
                    {
                        List<Guid> vehicleIds = session.Query<VehicleDetails>()
                            .Where(a => a.VINNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()))
                            .Select(a => a.Id).ToList();
                        List<Guid> electronicIds = session.Query<BrownAndWhiteDetails>()
                            .Where(a => a.SerialNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()))
                            .Select(a => a.Id).ToList();
                        List<Guid> otherItemIds = session.Query<OtherItemDetails>()
                            .Where(a => a.SerialNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()))
                            .Select(a => a.Id).ToList();
                        List<Guid> yellowGoodIds = session.Query<YellowGoodDetails>()
                            .Where(a => a.SerialNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo.ToLower()))
                            .Select(a => a.Id).ToList();

                        var policyIdsAuto = session.Query<VehiclePolicy>()
                            .Where(a => vehicleIds.Contains(a.VehicleId)).Select(a => a.PolicyId).ToList();
                        if (policyIdsAuto.Count() > 0)
                        {
                            poilicyIds.AddRange(policyIdsAuto);
                        }

                        var policyIdsElec = session.Query<BAndWPolicy>()
                           .Where(a => electronicIds.Contains(a.BAndWId)).Select(a => a.PolicyId).ToList();
                        if (policyIdsElec.Count() > 0)
                        {
                            poilicyIds.AddRange(policyIdsElec);
                        }

                        var policyIdsOther = session.Query<OtherItemPolicy>()
                           .Where(a => otherItemIds.Contains(a.OtherItemId)).Select(a => a.PolicyId).ToList();
                        if (policyIdsOther.Count() > 0)
                        {
                            poilicyIds.AddRange(policyIdsOther);
                        }

                        var policyIdsYellowGood = session.Query<YellowGoodPolicy>()
                           .Where(a => yellowGoodIds.Contains(a.YellowGoodId)).Select(a => a.PolicyId).ToList();
                        if (policyIdsYellowGood.Count() > 0)
                        {
                            poilicyIds.AddRange(policyIdsYellowGood);
                        }
                    }

                    #endregion dynamic column filtering

                    #region policy data expression building
                    //policy filtering
                    Expression<Func<Policy, bool>> filterByPolicy = PredicateBuilder.True<Policy>();
                    filterByPolicy = filterByPolicy.And(a => a.IsApproved == true && a.IsPolicyCanceled != true && a.IsPolicyRenewed != true);
                    if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.policyNo))
                    {
                        filterByPolicy = filterByPolicy.And(a => a.PolicyNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.policyNo.ToLower()));
                    }

                    filterByPolicy = filterByPolicy.And(a => contractIds.Contains(a.ContractId));

                    if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName))
                    {
                        filterByPolicy = filterByPolicy.And(a => CustomerIds.Contains(a.CustomerId));
                    }

                    if (!String.IsNullOrEmpty(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.serialNo))
                    {
                        filterByPolicy = filterByPolicy.And(a => poilicyIds.Contains(a.Id));
                    }

                    #endregion policy data expression building


                    //main filter for policy table
                    var policyGridDetails = session.Query<Policy>().Where(filterByPolicy);
                    //grid paging setup & joining other tables
                    long TotalRecords = policyGridDetails.Count();
                    var policyGridDetailsFilterd = policyGridDetails.Skip((PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageNumber - 1) * (PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageSize))
                    .Take(PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageSize);

                    #region "get data object to join query"
                    //IEnumerable<CommodityType> commodityType = session.Query<CommodityType>();
                    //IEnumerable<Customer> customer = session.Query<Customer>();
                    //IEnumerable<Dealer> dealer = session.Query<Dealer>();
                    //IEnumerable<Product> product = session.Query<Product>();

                    //IEnumerable<VehiclePolicy> vehiclePolicy = session.Query<VehiclePolicy>();
                    //IEnumerable<BAndWPolicy> electronicPolicy = session.Query<BAndWPolicy>();
                    //IEnumerable<OtherItemPolicy> otherPolicy = session.Query<OtherItemPolicy>();
                    //IEnumerable<YellowGoodPolicy> yellowGoodPolicy = session.Query<YellowGoodPolicy>();


                    //IEnumerable<VehicleDetails> vehicleDetails = session.Query<VehicleDetails>();
                    //IEnumerable<BrownAndWhiteDetails> electronicDetails = session.Query<BrownAndWhiteDetails>();
                    //IEnumerable<OtherItemDetails> otherItemDetails = session.Query<OtherItemDetails>();
                    //IEnumerable<YellowGoodDetails> yellowGoodDetails = session.Query<YellowGoodDetails>();

                    #endregion "get data object to join query"
                    CommonEntityManager cem = new CommonEntityManager();
                    var fullPolicyGridDetailsFilterd = policyGridDetailsFilterd
                     //.ToList()
                     // .Join(commodityType, a => a.CommodityTypeId, b => b.CommodityTypeId, (a, b) => new { a, b })
                     // .Join(customer, c => c.a.CustomerId, d => d.Id, (c, d) => new { c, d })
                     // .Join(dealer, e => e.c.a.DealerId, f => f.Id, (e, f) => new { e, f })
                     // .Join(product, g => g.e.c.a.ProductId, h => h.Id, (g, h) => new { g, h })
                     // .Join(vehiclePolicy, i => i.g.e.c.a.Id, j => j.PolicyId, (i, j) => new { i, j })
                     // .Join(electronicPolicy, k => k.i.g.e.c.a.Id, l => l.PolicyId, (k, l) => new { k, l })
                     // .Join(otherPolicy, m => m.k.i.g.e.c.a.Id, n => n.PolicyId, (m, n) => new { m, n })
                     // .Join(yellowGoodPolicy, o => o.m.k.i.g.e.c.a.Id, p => p.PolicyId, (o, p) => new { o, p })
                     // .Join(vehicleDetails, q => q.o.m.k.j.VehicleId, r => r.Id, (q, r) => new { q, r })
                     // .Join(electronicDetails, s => s.q.o.m.l.BAndWId, t => t.Id, (s, t) => new { s, t })
                     // .Join(otherItemDetails, u => u.s.q.o.n.OtherItemId, v => v.Id, (u, v) => new { u, v })
                     // .Join(yellowGoodDetails, w => w.u.s.q.p.YellowGoodId, x => x.Id, (w, x) => new { w, x })
                     .Select(a => new
                     {
                         Id = a.Id,
                         CommodityTypeId = a.CommodityTypeId,
                         CommodityType = cem.GetCommodityTypeNameById(a.CommodityTypeId),
                         PolicyNo = a.PolicyNo,
                         SerialNo = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(a.Id, a.CommodityTypeId, "serial"),
                         Dealer = cem.GetDealerNameById(a.DealerId),
                         DealerId = a.DealerId,
                         CustomerId = a.CustomerId,
                         MakeId = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(a.Id, a.CommodityTypeId, "make"),
                         ModelId = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(a.Id, a.CommodityTypeId, "model"),
                         CommodityCategoryId = cem.GetCommodityCategoryIdByContractId(a.ContractId),
                         CustomerName = cem.GetCustomerNameById(a.CustomerId)
                     }).ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = fullPolicyGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);
                }
                else
                {
                    Response = "Dealer's users can only access this page.";
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object PolicySearchOnClaimSubmissionNew(PolicySearchInClaimSubmissionRequestDto PolicySearchInClaimSubmissionRequestDto)
        {
            object Response = null;
            try
            {
                if (PolicySearchInClaimSubmissionRequestDto == null
                    || !IsGuid(PolicySearchInClaimSubmissionRequestDto.userId.ToString()))
                {
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                if (PolicySearchInClaimSubmissionRequestDto.userType.ToLower() == "du")
                {
                    DealerStaff DealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == PolicySearchInClaimSubmissionRequestDto.userId).FirstOrDefault();
                    if (DealerStaff == null)
                    {
                        Response = "You haven't assign to any Dealer";
                        return Response;
                    }

                    var policyGridDetails = (from icd in session.Query<InvoiceCodeDetails>()
                                         join ic in session.Query<InvoiceCode>() on icd.InvoiceCodeId equals ic.Id
                                         join po in session.Query<Policy>() on icd.PolicyId equals po.Id
                                         join c in session.Query<Customer>() on po.CustomerId equals c.Id
                                         where
                                         po.IsApproved==true && po.IsPolicyCanceled != true && po.IsPolicyRenewed !=true && po.DealerId == DealerStaff.DealerId
                                         && ic.PlateNumber.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.plateNo.ToLower())
                                         && po.PolicyNo.ToLower().Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.policyNo.ToLower())
                                         && c.MobileNo.Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.mobileNumber)
                                         select new
                                         {
                                             PolicyNo = po.PolicyNo,
                                             CustomerName = c.FirstName.ToLower() + " " + c.LastName.ToLower(),
                                             MobileNo = c.MobileNo,
                                             PlateNo = ic.PlateNumber

                                         }).Where(a=> a.CustomerName.Contains(PolicySearchInClaimSubmissionRequestDto.policySearchGridSearchCriterias.customerName.ToLower())).ToList();

                    long TotalRecords = policyGridDetails.Count();
                    var policyGridDetailsFilterd = policyGridDetails.Skip((PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageNumber - 1) * (PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageSize))
                    .Take(PolicySearchInClaimSubmissionRequestDto.paginationOptionsPolicySearchGrid.pageSize);
                    var fullPolicyGridDetailsFilterd = policyGridDetailsFilterd.ToArray();
                    var response = new CommonGridResponseDto()
                    {
                        totalRecords = TotalRecords,
                        data = fullPolicyGridDetailsFilterd
                    };
                    return new JavaScriptSerializer().Serialize(response);
                }
                else
                {
                    Response = "Dealer's users can only access this page.";
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;

        }

        internal static object GetPolicyDetailsByPolicyId(Guid policyId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Policy policy = session
                    .Query<Policy>().FirstOrDefault(a => a.Id == policyId);
                if (policy != null)
                {
                    var policyDetails = new
                    {
                        CustomerName = cem.GetCustomerNameById(policy.CustomerId),
                        CommodityType = cem.GetCommodityTypeNameById(policy.CommodityTypeId),
                        InsuaranceProductName = cem.GetProductNameById(policy.ProductId),
                        PolicyNo = policy.PolicyNo,
                        StartDate = policy.PolicyStartDate.ToString("dd-MMM-yyyy"),
                        EndDate = policy.PolicyEndDate.ToString("dd-MMM-yyyy"),
                        //todo
                    };
                    Response = policyDetails;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        private static bool IsGuid(string candidate)
        {
            if (candidate == "00000000-0000-0000-0000-000000000000")
            {
                return false;
            }

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

        internal static object GetAllPartAreasByComodityCategoryId(Guid commodityCategoryId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                List<PartArea> partAreaList = session.Query<PartArea>()
                    .Where(a => a.CommodityCategoryId == commodityCategoryId).ToList();
                if (partAreaList != null && partAreaList.Count() > 0)
                {
                    Response = partAreaList
                        .OrderBy(a => a.PartAreaName)
                        .Select(a => new
                        {
                            a.Id,
                            a.PartAreaCode,
                            a.PartAreaName
                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetPartsByPartAreaAndMakeId(Guid partAreaId, Guid makeId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                List<Part> partList = session.Query<Part>()
                    .Where(a => a.MakeId == makeId && a.PartAreaId == partAreaId).ToList();
                if (partList != null && partList.Count() > 0)
                {
                    Response = partList
                        .OrderBy(a => a.PartName)
                        .Select(a => new
                        {
                            a.Id,
                            a.PartName,
                            a.PartCode,
                            a.PartNumber,
                            a.ApplicableForAllModels,
                            a.AllocatedHours


                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object ValidatePartInformation(Guid dealerId, Guid makeId, Guid partId, Guid modelId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Part part = session.Query<Part>()
                    .Where(a => a.Id == partId).FirstOrDefault();
                if (part != null)
                {
                    PartPrice partPrice = session
                        .Query<PartPrice>().FirstOrDefault(a => a.PartId == partId && a.DealerId == dealerId);
                    PartModel partModel = session
                        .Query<PartModel>().FirstOrDefault(a => a.PartId == partId && a.ModelId == modelId);
                    List<PartSuggestion> partSuggestionsList = session.Query<PartSuggestion>()
                        .Where(a => a.PartId == partId).ToList();
                    var status = new
                    {
                        priceComment = partPrice == null ? "" : "ok",
                        price = partPrice == null ? (decimal)0.00 : currencyEm.ConvertFromBaseCurrency(partPrice.Price, partPrice.CurrencyId, partPrice.CurrencyPeriodId),
                        modelComment = part.ApplicableForAllModels == true ? "ok" : (partModel == null ? "" : "ok"),
                        make = cem.GetMakeNameById(makeId),
                        model = cem.GetModelNameById(modelId),
                        isRelatedPartsAvailable = partSuggestionsList.Count > 0 ? true : false,
                        allocatedHours = part.AllocatedHours == null ? 0 : part.AllocatedHours
                    };
                    Response = status;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal static object GetAllServiceHistoryByPolicyId(Guid policyId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Policy policy = session
                    .Query<Policy>().FirstOrDefault(a => a.Id == policyId);
                if (policy == null)
                {
                    PolicyBundle bundle = session.Query<PolicyBundle>()
                        .FirstOrDefault(a => a.Id == policyId);
                    if (bundle != null)
                    {
                        policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == bundle.Id);
                    }
                    else
                    {
                        return Response;
                    }
                }

                CommodityType commodityType = session
                    .Query<CommodityType>().FirstOrDefault(a => a.CommodityTypeId == policy.CommodityTypeId);
                if (commodityType == null)
                {
                    return Response;
                }

                string commodityCode = commodityType.CommodityCode;
                if (commodityCode.ToLower() == "a")
                { //automobile
                    VehiclePolicy vehiclePolicy = session
                        .Query<VehiclePolicy>().FirstOrDefault(a => a.PolicyId == policy.Id);
                    if (vehiclePolicy != null)
                    {
                        List<VehicleServiceHistory> vehicleServiceHistory = session.Query<VehicleServiceHistory>()
                            .Where(a => a.VehicleId == vehiclePolicy.VehicleId).ToList();
                        if (vehicleServiceHistory != null && vehicleServiceHistory.Count() > 0)
                        {
                            Response = vehicleServiceHistory.OrderByDescending(a => a.ServiceNumber).Select(a => new
                            {
                                a.Id,
                                a.Milage,
                                a.Remarks,
                                ServiceDate = a.ServiceDate.ToString("dd-MMM-yyyy"),
                                a.ServiceNumber,
                                canEdit = true//complete later
                            }).ToArray();
                        }
                    }

                }
                else if (commodityCode.ToLower() == "b")
                { //automobile
                    VehiclePolicy vehiclePolicy = session
                        .Query<VehiclePolicy>().FirstOrDefault(a => a.PolicyId == policy.Id);
                    if (vehiclePolicy != null)
                    {
                        List<VehicleServiceHistory> vehicleServiceHistory = session.Query<VehicleServiceHistory>()
                            .Where(a => a.VehicleId == vehiclePolicy.VehicleId).ToList();
                        if (vehicleServiceHistory != null && vehicleServiceHistory.Count() > 0)
                        {
                            Response = vehicleServiceHistory.OrderByDescending(a => a.ServiceNumber).Select(a => new
                            {
                                a.Id,
                                a.Milage,
                                a.Remarks,
                                ServiceDate = a.ServiceDate.ToString("dd-MMM-yyyy"),
                                a.ServiceNumber,
                                canEdit = true//complete later
                            }).ToArray();
                        }
                    }

                }
                else if (commodityCode.ToLower() == "o")
                { //automobile
                    OtherItemPolicy otherItemPolicy = session
                        .Query<OtherItemPolicy>().FirstOrDefault(a => a.PolicyId == policy.Id);
                    if (otherItemPolicy != null)
                    {
                        List<OtherItemServiceHistory> vehicleServiceHistory = session.Query<OtherItemServiceHistory>()
                            .Where(a => a.OtherItemId == otherItemPolicy.OtherItemId).ToList();
                        if (vehicleServiceHistory != null && vehicleServiceHistory.Count() > 0)
                        {
                            Response = vehicleServiceHistory.OrderByDescending(a => a.ServiceNumber).Select(a => new
                            {
                                a.Id,
                                a.Milage,
                                a.Remarks,
                                ServiceDate = a.ServiceDate.ToString("dd-MMM-yyyy"),
                                a.ServiceNumber,
                                canEdit = true//complete later
                            }).ToArray();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static string AddServiceHistory(Guid policyId, ServiceHistoryRequestDto serviceData)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Policy policy = session.Query<Policy>()
                    .Where(a => a.Id == policyId).FirstOrDefault();
                if (policy == null)
                {
                    PolicyBundle bundle = session.Query<PolicyBundle>()
                        .FirstOrDefault(a => a.Id == policyId);
                    if (bundle != null)
                    {
                        policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == bundle.Id);
                    }
                    else
                    {
                        return Response;
                    }
                }
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                string commodityCode = commodityType.CommodityCode;
                if (commodityCode.ToLower() == "a")
                { //automobile

                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (vehiclePolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {
                            VehicleServiceHistory serviceHistory = new VehicleServiceHistory()
                            {
                                Id = serviceData.Id,
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                VehicleId = vehiclePolicy.VehicleId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Update(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                        else
                        {
                            VehicleServiceHistory servisHistoryExist = session.Query<VehicleServiceHistory>()
                           .Where(a => a.VehicleId == vehiclePolicy.VehicleId && a.ServiceNumber == serviceData.serviceNumber).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                Response = "Service Number already exist";
                                return Response;
                            }

                            VehicleServiceHistory serviceHistory = new VehicleServiceHistory()
                            {
                                Id = Guid.NewGuid(),
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                VehicleId = vehiclePolicy.VehicleId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Save(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                    }
                }
                else if (commodityCode.ToLower() == "b")
                { //automobile

                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (vehiclePolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {
                            VehicleServiceHistory serviceHistory = new VehicleServiceHistory()
                            {
                                Id = serviceData.Id,
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                VehicleId = vehiclePolicy.VehicleId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Update(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                        else
                        {
                            VehicleServiceHistory servisHistoryExist = session.Query<VehicleServiceHistory>()
                           .Where(a => a.VehicleId == vehiclePolicy.VehicleId && a.ServiceNumber == serviceData.serviceNumber).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                Response = "Service Number already exist";
                                return Response;
                            }

                            VehicleServiceHistory serviceHistory = new VehicleServiceHistory()
                            {
                                Id = Guid.NewGuid(),
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                VehicleId = vehiclePolicy.VehicleId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Save(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                    }
                }
                else if (commodityCode.ToLower() == "o")
                { //automobile

                    OtherItemPolicy otherItemPolicy = session.Query<OtherItemPolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (otherItemPolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {
                            OtherItemServiceHistory serviceHistory = new OtherItemServiceHistory()
                            {
                                Id = serviceData.Id,
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                OtherItemId = otherItemPolicy.OtherItemId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Update(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                        else
                        {
                            OtherItemServiceHistory servisHistoryExist = session.Query<OtherItemServiceHistory>()
                           .Where(a => a.OtherItemId == otherItemPolicy.OtherItemId && a.ServiceNumber == serviceData.serviceNumber).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                Response = "Service Number already exist";
                                return Response;
                            }

                            OtherItemServiceHistory serviceHistory = new OtherItemServiceHistory()
                            {
                                Id = Guid.NewGuid(),
                                Milage = serviceData.milage,
                                Remarks = serviceData.remarks,
                                ServiceDate = serviceData.serviceDate,
                                ServiceNumber = serviceData.serviceNumber,
                                OtherItemId = otherItemPolicy.OtherItemId
                            };


                            using (ITransaction transaction = session.BeginTransaction())
                            {
                                session.Evict(serviceHistory);
                                session.Save(serviceHistory, serviceHistory.Id);
                                transaction.Commit();
                            }
                            Response = "ok";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal static string DeleteServiceHistory(Guid policyId, ServiceHistoryRequestDto serviceData)
        {
            string Response = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                Policy policy = session.Query<Policy>()
                    .Where(a => a.Id == policyId).FirstOrDefault();
                if (policy == null)
                {
                    PolicyBundle bundle = session.Query<PolicyBundle>()
                        .FirstOrDefault(a => a.Id == policyId);
                    if (bundle != null)
                    {
                        policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == bundle.Id);
                    }
                    else
                    {
                        return Response;
                    }
                }
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                if (commodityType == null)
                {
                    return Response;
                }

                string commodityCode = commodityType.CommodityCode;
                if (commodityCode.ToLower() == "a")
                { //automobile

                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (vehiclePolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {

                            VehicleServiceHistory servisHistoryExist = session.Query<VehicleServiceHistory>()
                           .Where(a => a.Id == serviceData.Id).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                using (ITransaction transaction = session.BeginTransaction())
                                {
                                    session.Evict(servisHistoryExist);
                                    session.Delete(servisHistoryExist);
                                    transaction.Commit();
                                }
                                Response = "ok";
                            }


                        }
                    }
                }
                if (commodityCode.ToLower() == "b")
                { //automobile

                    VehiclePolicy vehiclePolicy = session.Query<VehiclePolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (vehiclePolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {

                            VehicleServiceHistory servisHistoryExist = session.Query<VehicleServiceHistory>()
                           .Where(a => a.Id == serviceData.Id).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                using (ITransaction transaction = session.BeginTransaction())
                                {
                                    session.Evict(servisHistoryExist);
                                    session.Delete(servisHistoryExist);
                                    transaction.Commit();
                                }
                                Response = "ok";
                            }


                        }
                    }
                }
                else if (commodityCode.ToLower() == "o")
                { //automobile

                    OtherItemPolicy otherItemPolicy = session.Query<OtherItemPolicy>()
                        .Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    if (otherItemPolicy != null)
                    {
                        if (serviceData.Id != Guid.Empty)
                        {

                            OtherItemServiceHistory servisHistoryExist = session.Query<OtherItemServiceHistory>()
                           .Where(a => a.Id == serviceData.Id).FirstOrDefault();
                            if (servisHistoryExist != null)
                            {
                                using (ITransaction transaction = session.BeginTransaction())
                                {
                                    session.Evict(servisHistoryExist);
                                    session.Delete(servisHistoryExist);
                                    transaction.Commit();
                                }
                                Response = "ok";
                            }


                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }


        internal static object GetAllRelatedPartsByPartId(Guid partId, Guid dealerId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                List<PartSuggestion> partSuggestions = session.Query<PartSuggestion>()
                    .Where(a => a.PartId == partId).ToList();
                List<object> responsePartList = new List<object>();
                if (partSuggestions != null && partSuggestions.Count > 0)
                {

                    foreach (var part in partSuggestions)
                    {
                        Part newPart = session.Query<Part>()
                            .Where(a => a.Id == part.SuggestedPartId).FirstOrDefault();
                        if (newPart != null)
                        {
                            PartPrice partPrice = session.Query<PartPrice>()
                            .Where(a => a.PartId == newPart.Id && a.DealerId == dealerId).FirstOrDefault();
                            var partdata = new
                            {
                                partId = part.SuggestedPartId,
                                priceComment = partPrice == null ? "" : "ok",
                                price = partPrice == null ? (decimal)0.00 : currencyEm.ConvertFromBaseCurrency(partPrice.Price, partPrice.CurrencyPeriodId, partPrice.CurrencyId),
                                partAreaId = newPart.PartAreaId,
                                partCode = newPart.PartCode,
                                partNumber = newPart.PartNumber,
                                partName = newPart.PartName,
                                qty = part.Quantity,
                                totalPrice = (part.Quantity) * (partPrice == null ? (decimal)0.00 : currencyEm.ConvertFromBaseCurrency(partPrice.Price, partPrice.CurrencyPeriodId, partPrice.CurrencyId)),
                                itemType = "PART",
                                allocatedHours = newPart.AllocatedHours,
                                remarks = ""
                            };
                            responsePartList.Add(partdata);
                        }
                    };

                }

                Response = responsePartList.ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }



        public PartAreaResponseDto GetPartAreaById(Guid PartAreaId)
        {
            ISession session = EntitySessionManager.GetSession();
            PartAreaResponseDto pDto = new PartAreaResponseDto();

            var query =
                from PartArea in session.Query<PartArea>()
                where PartArea.Id == PartAreaId
                select new { PartArea = PartArea };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().PartArea.Id;
                pDto.CommodityTypeId = result.First().PartArea.CommodityTypeId;
                pDto.CommodityCategoryId = result.First().PartArea.CommodityCategoryId;
                pDto.PartAreaCode = result.First().PartArea.PartAreaCode;
                pDto.PartAreaName = result.First().PartArea.PartAreaName;
                pDto.IsPartAreaExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }

        public PartResponseDto GetPartByPartAreaId(Guid PartAreaId)
        {
            ISession session = EntitySessionManager.GetSession();
            PartResponseDto pDto = new PartResponseDto();

            var query =
                from Part in session.Query<Part>()
                where Part.PartAreaId == PartAreaId
                select new { Part = Part };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().Part.Id;
                pDto.PartAreaId = result.First().Part.PartAreaId;
                pDto.MakeId = result.First().Part.MakeId;
                pDto.CommodityId = result.First().Part.CommodityId;
                pDto.PartCode = result.First().Part.PartCode;
                pDto.PartName = result.First().Part.PartName;
                pDto.PartNumber = result.First().Part.PartNumber;
                pDto.IsActive = result.First().Part.IsActive;
                pDto.ApplicableForAllModels = result.First().Part.ApplicableForAllModels;
                //   pDto.EntryDateTime = result.First().Part.EntryDateTime;
                pDto.AllocatedHours = result.First().Part.AllocatedHours;
                //  pDto.EntryBy = result.First().Part.EntryBy;
                //  pDto.IsPartExists = true;
                return pDto;
            }
            else
            {

                return null;
            }
        }

        internal bool AddPartArea(PartAreaRequestDto PartArea)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PartArea pr = new Entities.PartArea();

                pr.Id = new Guid();
                pr.CommodityTypeId = PartArea.CommodityTypeId;
                pr.CommodityCategoryId = PartArea.CommodityCategoryId;
                pr.PartAreaCode = PartArea.PartAreaCode;
                pr.PartAreaName = PartArea.PartAreaName;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<PartAreaResponseDto> GetAllPartArea()
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                return session.Query<PartArea>().Select(partarea => new PartAreaResponseDto {
                    Id = partarea.Id,
                    CommodityTypeId = partarea.CommodityTypeId,
                    CommodityCategoryId = partarea.CommodityCategoryId,
                    PartAreaCode = partarea.PartAreaCode,
                    PartAreaName = partarea.PartAreaName
                }).ToList();

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }


        }

        internal bool UpdatePartArea(PartAreaRequestDto PartArea)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PartArea pr = new Entities.PartArea();

                pr.Id = PartArea.Id;
                pr.CommodityTypeId = PartArea.CommodityTypeId;
                pr.CommodityCategoryId = PartArea.CommodityCategoryId;
                pr.PartAreaCode = PartArea.PartAreaCode;
                pr.PartAreaName = PartArea.PartAreaName;

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


        public PartResponseDto GetPartById(Guid PartId)
        {
            PartResponseDto Response = new PartResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Part part = session.Query<Part>()
                    .Where(a => a.Id == PartId).FirstOrDefault();
                if (part == null)
                {
                    return Response;
                }

                List<PartPrice> partPrices = session.Query<PartPrice>()
                    .Where(a => a.PartId == PartId).ToList();
                List<PartPriceReq> partPriceListResp = new List<PartPriceReq>();
                CommonEntityManager cem = new CommonEntityManager();
                int partPriceId = 0;
                foreach (PartPrice partPrice in partPrices)
                {
                    partPriceId++;
                    PartPriceReq partPriceResp = new PartPriceReq()
                    {
                        CountryId = partPrice.CountryId,
                        CountryName = cem.GetCountryNameById(partPrice.CountryId),
                        CurrencyName = cem.GetCurrencyTypeByIdCode(partPrice.CurrencyId),
                        DealerId = partPrice.DealerId,
                        DealerName = cem.GetDealerNameById(partPrice.DealerId),
                        Id = partPriceId,
                        Price = Math.Round(partPrice.Price * partPrice.ConversionRate * 100) / 100
                    };
                    partPriceListResp.Add(partPriceResp);
                }


                Response = new PartResponseDto()
                {
                    AllocatedHours = part.AllocatedHours,
                    ApplicableForAllModels = part.ApplicableForAllModels,
                    CommodityId = part.CommodityId,
                    Id = part.Id,
                    IsActive = part.IsActive,
                    MakeId = part.MakeId,
                    PartAreaId = part.PartAreaId,
                    PartCode = part.PartCode,
                    PartName = part.PartName,
                    PartNumber = part.PartNumber,
                    PartPrices = partPriceListResp
                };

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static string AddPart(PartRequestDto partData)
        {
            string Response = string.Empty;
            try
            {
                //validate part details
                if (partData == null || partData.Part == null)
                {
                    return "Request data is empty.";
                }

                if (!IsGuid(partData.Part.CommodityId.ToString()) || !IsGuid(partData.Part.CommodityId.ToString())
                     || !IsGuid(partData.Part.MakeId.ToString()))
                {
                    return "Request data is incomplete.";
                }
                //part price duplicates validation
                int duplicatesCount = partData.PartPrice.GroupBy(i => new { i.CountryId, i.DealerId })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).Count();
                if (duplicatesCount > 0)
                {
                    return "Part prices contains duplicate values.";
                }

                CommonEntityManager commonEM = new CommonEntityManager();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();

                //Part No Duplicate validation
                int prtNoCount = 0;
                List<Part> partN = session.Query<Part>().ToList();
                foreach (var prt in partN)
                {
                    if (partData.Part.PartNumber == prt.PartNumber)
                    {
                        prtNoCount++;
                        break;
                    }
                }

                if (prtNoCount > 0)
                {
                    return "DuplicatePrtNo";
                }

                Guid currentCurrecyPeriodId = currencyEM.GetCurrentCurrencyPeriodId();
                if (currentCurrecyPeriodId == Guid.Empty)
                {
                    return "No currency period defined for today.";
                }

                Guid newPartId = Guid.NewGuid();
                Part Part = new Part()
                {
                    Id = newPartId,
                    AllocatedHours = partData.Part.AllocatedHours,
                    ApplicableForAllModels = partData.Part.ApplicableForAllModels,
                    CommodityId = partData.Part.CommodityId,
                    EntryBy = partData.UserId,
                    EntryDateTime = DateTime.UtcNow,
                    IsActive = partData.Part.IsActive,
                    MakeId = partData.Part.MakeId,
                    PartAreaId = partData.Part.PartAreaId,
                    PartCode = partData.Part.PartCode,
                    PartName = partData.Part.PartName,
                    PartNumber = partData.Part.PartNumber
                };


                List<PartPrice> partPriceList = new List<PartPrice>();
                foreach (PartPriceReq partPrice in partData.PartPrice)
                {
                    if (IsGuid(partPrice.DealerId.ToString()))
                    {
                        Dealer dealer = session.Query<Dealer>()
                            .Where(a => a.Id == partPrice.DealerId).FirstOrDefault();
                        if (dealer != null)
                        {
                            bool checkConversionRate = currencyEM.CheckConversionRate(dealer.CurrencyId, currentCurrecyPeriodId);
                            if (!checkConversionRate)
                            {
                                return "Conversion rate not found for dealer(" + dealer.DealerName + "'s) currency.";
                            }

                            PartPrice PartPrice = new PartPrice()
                            {
                                ConversionRate = currencyEM.GetConversionRate(dealer.CurrencyId, currentCurrecyPeriodId),
                                CountryId = partPrice.CountryId,
                                CurrencyId = dealer.CurrencyId,
                                CurrencyPeriodId = currentCurrecyPeriodId,
                                DealerId = dealer.Id,
                                Id = Guid.NewGuid(),
                                PartId = newPartId,
                                Price = currencyEM.ConvertToBaseCurrency(partPrice.Price, dealer.CurrencyId, currentCurrecyPeriodId)
                            };
                            partPriceList.Add(PartPrice);
                        }
                    }
                }

                //saving details
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(Part);
                    session.Save(Part, Part.Id);

                    foreach (PartPrice PartPrice in partPriceList)
                    {
                        session.Evict(PartPrice);
                        session.Save(PartPrice, PartPrice.Id);
                    }

                    transaction.Commit();
                }
                Response = "ok";


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while saving part.";
            }
            return Response;
        }

        internal string UpdatePart(PartRequestDto partData)
        {
            string Response = string.Empty;
            try
            {
                //validate part details
                if (partData == null || partData.Part == null)
                {
                    return "Request data is empty.";
                }

                if (!IsGuid(partData.Part.Id.ToString()))
                {
                    return "No part selected for update.";
                }

                if (!IsGuid(partData.Part.CommodityId.ToString()) || !IsGuid(partData.Part.CommodityId.ToString())
                     || !IsGuid(partData.Part.MakeId.ToString()))
                {
                    return "Request data is incomplete.";
                }
                //part price duplicates validation
                int duplicatesCount = partData.PartPrice.GroupBy(i => new { i.CountryId, i.DealerId })
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key).Count();
                if (duplicatesCount > 0)
                {
                    return "Part prices contains duplicate values.";
                }

                CommonEntityManager commonEM = new CommonEntityManager();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();
                ISession session = EntitySessionManager.GetSession();

                session.Clear();
                //Part No Duplicate validation
                int prtNoCount = 0;
                List<Part> partN = session.Query<Part>().ToList();
                string OldpartN = session.Query<Part>()
                    .Where(a => a.Id == partData.Part.Id).FirstOrDefault().PartNumber;
                if (OldpartN != partData.Part.PartNumber)
                {
                    foreach (var prt in partN)
                    {
                        if (partData.Part.PartNumber == prt.PartNumber)
                        {
                            prtNoCount++;
                            break;
                        }
                    }
                }

                if (prtNoCount > 0)
                {
                    return "DuplicatePrtNo";
                }

                Guid currentCurrecyPeriodId = currencyEM.GetCurrentCurrencyPeriodId();
                if (currentCurrecyPeriodId == Guid.Empty)
                {
                    return "No currency period defined for today.";
                }

                Part Part = new Part()
                {
                    Id = partData.Part.Id,
                    AllocatedHours = partData.Part.AllocatedHours,
                    ApplicableForAllModels = partData.Part.ApplicableForAllModels,
                    CommodityId = partData.Part.CommodityId,
                    EntryBy = partData.UserId,
                    EntryDateTime = DateTime.UtcNow,
                    IsActive = partData.Part.IsActive,
                    MakeId = partData.Part.MakeId,
                    PartAreaId = partData.Part.PartAreaId,
                    PartCode = partData.Part.PartCode,
                    PartName = partData.Part.PartName,
                    PartNumber = partData.Part.PartNumber
                };

                session.Clear();
                //old price details
                List<PartPrice> partPriceListOld = session.Query<PartPrice>()
                    .Where(a => a.PartId == partData.Part.Id).ToList();
                //setup new price details
                List<PartPrice> partPriceList = new List<PartPrice>();
                foreach (PartPriceReq partPrice in partData.PartPrice)
                {
                    if (IsGuid(partPrice.DealerId.ToString()))
                    {
                        Dealer dealer = session.Query<Dealer>()
                            .Where(a => a.Id == partPrice.DealerId).FirstOrDefault();
                        if (dealer != null)
                        {
                            bool checkConversionRate = currencyEM.CheckConversionRate(dealer.CurrencyId, currentCurrecyPeriodId);
                            if (!checkConversionRate)
                            {
                                return "Conversion rate not found for dealer(" + dealer.DealerName + "'s) currency.";
                            }

                            PartPrice PartPrice = new PartPrice()
                            {
                                ConversionRate = currencyEM.GetConversionRate(dealer.CurrencyId, currentCurrecyPeriodId),
                                CountryId = partPrice.CountryId,
                                CurrencyId = dealer.CurrencyId,
                                CurrencyPeriodId = currentCurrecyPeriodId,
                                DealerId = dealer.Id,
                                Id = Guid.NewGuid(),
                                PartId = partData.Part.Id,
                                Price = currencyEM.ConvertToBaseCurrency(partPrice.Price, dealer.CurrencyId, currentCurrecyPeriodId)
                            };
                            partPriceList.Add(PartPrice);
                        }
                    }
                }

                //saving details
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(Part);
                    session.Update(Part, Part.Id);

                    foreach (PartPrice partPriceOld in partPriceListOld)
                    {
                        session.Evict(partPriceOld);
                        session.Delete(partPriceOld);
                    }


                    foreach (PartPrice PartPrice in partPriceList)
                    {
                        session.Evict(PartPrice);
                        session.Save(PartPrice, PartPrice.Id);
                    }

                    transaction.Commit();
                }
                Response = "ok";


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while saving part.";
            }
            return Response;
        }

        public List<Part> GetAllPartAreaByMakeId(Guid MakeId)
        {
            List<Part> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Part> PartData = session.Query<Part>();
            entities = PartData.ToList();

            return entities.FindAll(c => c.MakeId == MakeId);
        }


        internal static object GetAllPriceForSearchGrid(PriceSearchGridRequestDto PriceSearchGridRequestDto)
        {
            if (PriceSearchGridRequestDto == null || PriceSearchGridRequestDto.paginationOptionsPriceSearchGrid == null)
            {
                return null;
            }

            Expression<Func<PartPrice, bool>> filterPrice = PredicateBuilder.True<PartPrice>();

            if (IsGuid(PriceSearchGridRequestDto.priceSearchGridSearchCriterias.dealerid.ToString()))
            {
                filterPrice = filterPrice.And(a => a.DealerId == PriceSearchGridRequestDto.priceSearchGridSearchCriterias.dealerid);
            }

            ISession session = EntitySessionManager.GetSession();
            var FilterdPriceDetails = session.Query<PartPrice>().Where(filterPrice);
            long TotalRecords = FilterdPriceDetails.Count();
            var PriceDetailsForGrid = FilterdPriceDetails.Skip((PriceSearchGridRequestDto.paginationOptionsPriceSearchGrid.pageNumber - 1) * PriceSearchGridRequestDto.paginationOptionsPriceSearchGrid.pageSize)
            .Take(PriceSearchGridRequestDto.paginationOptionsPriceSearchGrid.pageSize)
            .Select(a => new
            {
                a.DealerId,
                a.CountryId,
                a.CurrencyId,
                a.CurrencyPeriodId,
                a.Price,
                a.Id
            }).ToArray();
            var response = new CommonGridResponseDto()
            {
                totalRecords = TotalRecords,
                data = PriceDetailsForGrid
            };
            return new JavaScriptSerializer().Serialize(response);

        }

        internal static object SubmitClaim(ClaimSubmissionRequestDto claimSubmissionData)
        {
            object response = null;
            try
            {
                //validate claim information
                string result = ClaimDataValidationOnSave(claimSubmissionData);
                if (result != "ok")
                {
                    return result;
                }

                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentConversionPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentConversionPeriodId.ToString()))
                {
                    return "Currenct currency period is not set.";
                }

                ISession session = EntitySessionManager.GetSession();
                Policy policy = session
                    .Query<Policy>().FirstOrDefault(a => a.Id == claimSubmissionData.policyId);
                if (policy == null)
                {
                    return "Invalid policy selection.";
                }
                CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();


                // check policy risk period with Failure  date
                if (claimSubmissionData.policyDetails.failureDate.ToString("yyyy-MM-dd") != "1753-01-01")
                {
                    Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    bool validateContractPeriod = true;
                    if (!contract.IsActive)
                    {
                        validateContractPeriod = false;
                        return "Contract Is Not Active Status";
                    }

                    if (policy.PolicyStartDate.AddDays(1) <= claimSubmissionData.policyDetails.failureDate && policy.PolicyEndDate.AddDays(1) >= claimSubmissionData.policyDetails.failureDate)
                    {
                        validateContractPeriod = true;
                    }
                    else
                    {
                        validateContractPeriod = false;
                        string errorMsg = "Risk Period Mismatch with Failure Date \n  [   Contract Period    : " + policy.PolicyStartDate.AddDays(1).ToString("dd-MMM-yyyy") + " to " + policy.PolicyEndDate.AddDays(1).ToString("dd-MMM-yyyy") + " ] ";
                        return errorMsg;
                    }
                }

                if (CommodityType.CommodityCode == "A")
                {
                    // check Failure mileage less than premium mileage
                    decimal PremiumMileage  = session.Query<ContractInsuaranceLimitation>()
                         .Join(session.Query<InsuaranceLimitation>(), b => b.InsuaranceLimitationId, c => c.Id, (b, c) => new { b, c })
                         .Where(x => x.b.ContractId == policy.ContractId).Select(s=> s.c.Km).FirstOrDefault();
                    if (PremiumMileage > 0 && PremiumMileage + Decimal.Parse(policy.HrsUsedAtPolicySale) < claimSubmissionData.policyDetails.failureMileage) {
                        return "Failure mileage above risk termination .";
                    }

                    // check Failure mileage less than previous claim
                    if (!CheckClaimMileageValidation(policy.PolicyBundleId, claimSubmissionData.policyDetails.failureMileage))
                    {
                        return "Failure Mileage cannot less than previous submitted  claim .";
                    }

                }


                Dealer claimDealer = session
                    .Query<Dealer>().FirstOrDefault(a => a.Id == claimSubmissionData.dealerId);
                if (claimDealer == null)
                {
                    return "Logged in dealer is invalid.";
                }

                if (claimSubmissionData.claimMileage <= 0)
                {
                    return "Invalid claim mileage.";
                }

                if (claimSubmissionData.claimDate < SqlDateTime.MinValue.Value)
                {
                    return "Invalid claim date.Claim date should be greater than " + SqlDateTime.MinValue.Value.ToString("dd-MMM-yyyy");
                }

                //new parts and part price setup
                List<Part> newParts = new List<Part>();
                List<PartPrice> newPartPrices = new List<PartPrice>();

                foreach (var item in claimSubmissionData.claimItemList)
                {
                    if (item.partId == Guid.Empty && item.itemType.ToLower() == "p")
                    {
                        Part part = new Part()
                        {
                            Id = Guid.NewGuid(),
                            AllocatedHours = 0,
                            ApplicableForAllModels = true,
                            CommodityId = policy.CommodityTypeId,
                            EntryBy = claimSubmissionData.requestedUserId,
                            EntryDateTime = DateTime.UtcNow,
                            IsActive = true,
                            MakeId = claimSubmissionData.policyDetails.makeId,
                            PartAreaId = item.partAreaId,
                            PartCode = item.itemName,
                            PartName = item.itemName,
                            PartNumber = item.itemNumber
                        };
                        newParts.Add(part);

                        PartPrice partPrice = new PartPrice()
                        {
                            DealerId = claimSubmissionData.dealerId,
                            CountryId = claimDealer.CountryId,
                            CurrencyId = claimDealer.CurrencyId,
                            ConversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId),
                            CurrencyPeriodId = currentConversionPeriodId,
                            Id = Guid.NewGuid(),
                            PartId = part.Id,
                            Price = currencyEm.ConvertToBaseCurrency(item.unitPrice, claimDealer.CurrencyId, currentConversionPeriodId)
                        };
                        newPartPrices.Add(partPrice);
                        //setup item info

                        item.partId = part.Id;
                    }
                }


                //claim header
                ClaimSubmission claimRequest = DBDTOTransformer.Instance.ConvertClaimDataToClaimSubmissionRequest(
                    claimDealer, claimSubmissionData, policy, currentConversionPeriodId);
                //claim detail
                List<ClaimSubmissionItem> claimItemList = DBDTOTransformer.Instance.ConvertClaimDataToClaimItemList(
                    claimSubmissionData, claimRequest.Id, claimDealer, currentConversionPeriodId);
                List<PartPrice> partPricesList = new List<PartPrice>();

                List<ClaimSubmissionAttachment> clamAttachmentList = new List<ClaimSubmissionAttachment>();
                foreach (Guid docId in claimSubmissionData.attachmentIds)
                {
                    ClaimSubmissionAttachment attchment = new ClaimSubmissionAttachment()
                    {
                        ClaimSubmissionId = claimRequest.Id,
                        Id = Guid.NewGuid(),
                        UserAttachmentId = docId,
                        DateOfAttachment = DateTime.UtcNow
                    };
                    clamAttachmentList.Add(attchment);
                }
                session.Clear();

                var Productinfo = session.Query<Product>()
                       .FirstOrDefault(a => a.Id == policy.ProductId);
                var ProductTypeinfo = session.Query<ProductType>()
                       .FirstOrDefault(a => a.Id == Productinfo.ProductTypeId);

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(claimRequest);

                    //save new parts & prices
                    foreach (Part part in newParts)
                    {
                        session.Evict(part);
                        session.Save(part, part.Id);
                    }
                    foreach (PartPrice partPrice in newPartPrices)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);
                    }

                    session.Save(claimRequest, claimRequest.Id);

                    foreach (ClaimSubmissionItem claimItem in claimItemList)
                    {
                        session.Evict(claimItem);
                        session.Save(claimItem, claimItem.Id);

                        if (ProductTypeinfo.Code != "ILOE")
                        {
                            //update delaer price if not found
                            if (IsGuid(claimItem.PartId.ToString()))
                            {
                                PartPrice partPrice = session
                                    .Query<PartPrice>().FirstOrDefault(a => a.PartId == claimItem.PartId
                                        && a.DealerId == claimDealer.Id && a.CountryId == claimDealer.CountryId);
                                if (partPrice == null)
                                {
                                    partPrice = new PartPrice()
                                    {
                                        Id = Guid.NewGuid(),
                                        ConversionRate = claimRequest.ConversionRate,
                                        CountryId = claimDealer.CountryId,
                                        CurrencyId = claimRequest.ClaimCurrencyId,
                                        CurrencyPeriodId = claimRequest.CurrencyPeriodId,
                                        DealerId = claimDealer.Id,
                                        PartId = Guid.Parse(claimItem.PartId.ToString()),
                                        Price = claimItem.UnitPrice
                                    };
                                    partPricesList.Add(partPrice);
                                }
                            }
                        }

                    }
                    //save doc ids
                    foreach (ClaimSubmissionAttachment claimAttachment in clamAttachmentList)
                    {
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment, claimAttachment.Id);
                    }

                    //update Part Prices
                    foreach (PartPrice partPrice in partPricesList)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);
                    }

                    transaction.Commit();
                }
                response = "ok";

                //if(response == "ok")
                //{
                //    ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                //                .Where(a => a.Id == claimRequest.Id).FirstOrDefault();
                //    UserEntityManager ce = new UserEntityManager();
                //    List<String> EmailList = new List<string>();
                //    EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                //    new GetMyEmail().ClaimApprovedEmail(EmailList, ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).FirstName);
                //}
            }

            catch (Exception ex)
            {
                response = "Error occured while saving claim request.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private string ClaimTireDataValidationOnSave(ClaimOtherTireUpdateRequestDto claimSubmissionData)
        {
            string result = "ok";
            try
            {
                if (claimSubmissionData == null)
                {
                    result = "Request data is empty";
                }
                else
                {
                    if (!IsGuid(claimSubmissionData.dealerId.ToString()) || !IsGuid(claimSubmissionData.policyId.ToString())
                        || !IsGuid(claimSubmissionData.requestedUserId.ToString()))
                    {
                        result = "Requested data is incorrect";
                    }
                    else
                    {
                        if (claimSubmissionData.claimItemList == null || !claimSubmissionData.claimItemList.Any())
                        {
                            result = "Claim items not found.";
                        }
                        else
                        {
                            if (claimSubmissionData.policyDetails == null)
                            {
                                result = "Please fill all the mandetory fields.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;
        }
        private static string ClaimDataValidationOnSave(ClaimSubmissionRequestDto claimSubmissionData)
        {
            string result = "ok";
            try
            {
                if (claimSubmissionData == null)
                {
                    result = "Request data is empty";
                }
                else
                {
                    if (!IsGuid(claimSubmissionData.dealerId.ToString()) || !IsGuid(claimSubmissionData.policyId.ToString())
                        || !IsGuid(claimSubmissionData.requestedUserId.ToString()))
                    {
                        result = "Requested data is incorrect";
                    }
                    else
                    {
                        if (claimSubmissionData.claimItemList == null || !claimSubmissionData.claimItemList.Any())
                        {
                            result = "Claim items not found.";
                        }
                        else
                        {
                            if (claimSubmissionData.policyDetails == null)
                            {
                                result = "Please fill all the mandetory fields.";
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return result;
        }

        internal static object GetAllPartAreasByCommodityTypeId(Guid CommodityTypeId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                List<PartArea> partAreaList = session.Query<PartArea>()
                    .Where(a => a.CommodityTypeId == CommodityTypeId).ToList();
                if (partAreaList != null && partAreaList.Count() > 0)
                {
                    Response = partAreaList
                        .OrderBy(a => a.PartAreaName)
                        .Select(a => new
                        {
                            a.Id,
                            CommodityCategory = cem.GetCommodityCategoryNameById(a.CommodityCategoryId),
                            a.PartAreaCode,
                            a.PartAreaName
                        })
                        .OrderBy(a => a.CommodityCategory)
                        .ThenBy(a => a.PartAreaCode)
                        .ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetPartsByMakePartArea(Guid PartAreaId, Guid MakeId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                List<Part> partAreaList = session.Query<Part>()
                    .Where(a => a.MakeId == MakeId && a.PartAreaId == PartAreaId).ToList();
                if (partAreaList != null && partAreaList.Count() > 0)
                {
                    Response = partAreaList
                        .OrderBy(a => a.PartName)
                        .Select(a => new
                        {
                            a.Id,
                            a.PartName,
                            a.PartNumber,
                            a.PartCode,
                            a.ApplicableForAllModels
                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object GetPartSuggestionsByPartId(Guid partId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<PartSuggestion> partSuggestionList = session.Query<PartSuggestion>()
                    .Where(a => a.PartId == partId).ToList();
                if (partSuggestionList != null && partSuggestionList.Count() > 0)
                {
                    Response = partSuggestionList
                        .Select(a => new
                        {
                            a.Quantity,
                            a.SuggestedPartId

                        }).ToArray();
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static string SavePartSuggestion(PartSuggestionRequestDto PartSuggestionRequest)
        {
            string Response = "";
            try
            {
                //validation
                if (PartSuggestionRequest == null || PartSuggestionRequest.partSuggestion == null)
                {
                    return "Request data is empty";
                }

                if (!IsGuid(PartSuggestionRequest.partSuggestion.PartId.ToString()))
                {
                    return "Request data is incorrect";
                }

                ISession session = EntitySessionManager.GetSession();
                List<PartSuggestion> existingPartSuggestionList = session.Query<PartSuggestion>()
                    .Where(a => a.PartId == PartSuggestionRequest.partSuggestion.PartId).ToList();
                //saving details
                using (ITransaction transaction = session.BeginTransaction())
                {
                    foreach (PartSuggestion PartSuggestion in existingPartSuggestionList)
                    {
                        session.Evict(PartSuggestion);
                        session.Delete(PartSuggestion);
                    }

                    foreach (var partSuggestion in PartSuggestionRequest.relatedParts)
                    {
                        if (partSuggestion.isSelected)
                        {
                            PartSuggestion suggestion = new PartSuggestion()
                            {
                                Id = Guid.NewGuid(),
                                PartId = PartSuggestionRequest.partSuggestion.PartId,
                                Quantity = partSuggestion.quantity,
                                SuggestedPartId = partSuggestion.Id
                            };
                            session.Evict(partSuggestion);
                            session.Save(suggestion, suggestion.Id);
                        }
                    }
                    transaction.Commit();
                }
                Response = "ok";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while saving part suggestion.";
            }
            return Response;
        }

        internal object UserValidationClaimListing(Guid loggedInUserId)
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                SystemUser sysUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == loggedInUserId).FirstOrDefault();
                if (sysUser == null)
                {
                    var data = new
                    {
                        status = "You don't have access to this Page. Please contact Administrator",
                    };
                    Response = data;
                    return Response;
                }
                UserType userType = session.Query<UserType>()
                    .Where(a => a.Id == sysUser.UserTypeId).FirstOrDefault();
                if (userType == null)
                {
                    var data = new
                    {
                        status = "You don't have access to this Page. Please contact Administrator",
                    };
                    Response = data;
                    return Response;
                }

                if (userType.Code.ToLower() == "du")
                {
                    DealerStaff dealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == loggedInUserId).FirstOrDefault();
                    if (dealerStaff == null)
                    {
                        var data = new
                        {
                            status = "You haven't assigned to any dealer.",
                        };
                        Response = data;
                        return Response;
                    }


                    Dealer dealer = session.Query<Dealer>()
                        .Where(a => a.Id == dealerStaff.DealerId).FirstOrDefault();

                    if (dealer == null)
                    {
                        var data = new
                        {
                            status = "You haven't assigned to any dealer.",

                        };
                        Response = data;
                        return Response;
                    }
                    else
                    {
                        if (IsGuid(dealer.CurrencyId.ToString()))
                        {
                            var data = new
                            {
                                status = "ok",
                                type = "du"
                            };
                            Response = data;
                            return Response;
                        }
                        else
                        {
                            var data = new
                            {
                                status = "Currency is not set on the dealer you are assigned (" + dealer.DealerName + ").",
                            };
                            Response = data;
                            return Response;
                        }
                    }
                }
                else if (userType.Code.ToLower() == "iu")
                {
                    var data = new
                    {
                        status = "ok",
                        type = "iu"
                    };
                    Response = data;
                    return Response;
                }
                else
                {
                    var data = new
                    {
                        status = "You are not allowed to access this page.",
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

        internal static object GetAllClaimDashboardStatus()
        {
            ClaimDashboardResponseDto response = new ClaimDashboardResponseDto();
            response.data = new List<ClaimDashboardData>();
            response.status = "Ok";

            try
            {
                ISession session = EntitySessionManager.GetSession();

                IEnumerable<ClaimStatusCode> claimStatus = session.Query<ClaimStatusCode>();
                List<ClaimSubmission> claimSubmissionList = session.Query<ClaimSubmission>().ToList();
                List<Claim> claimList = session.Query<Claim>().ToList();

                #region Submitted
                var getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "SUB").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                var getAllClaims = claimList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "SUB").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                var getAllSubmittedClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if(getAllSubmittedClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "Submitted Claims",
                        value = getAllSubmittedClaims.Count

                    };
                    response.data.Add(ClaimDashboardData);
                }

                #endregion

                #region Approved

                getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "APP").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                 getAllClaims = claimList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "APP").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                var getAllApprovedClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if (getAllApprovedClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "Approved Claims",
                        value = getAllApprovedClaims.Count
                    };
                    response.data.Add(ClaimDashboardData);
                }

                #endregion

                #region Rejected
                getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "REJ").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                getAllClaims = claimList
                   .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                   .Where(a => a.n.StatusCode == "REJ").Select(
                   cv => new
                   {
                       cv.m.ClaimNumber
                   }
                   );

                var getAllRejectedClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if (getAllRejectedClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "Rejected Claims",
                        value = getAllRejectedClaims.Count
                    };
                    response.data.Add(ClaimDashboardData);
                }
                #endregion

                #region OnReview
                getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "REV").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                getAllClaims = claimList
                   .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                   .Where(a => a.n.StatusCode == "REV").Select(
                   cv => new
                   {
                       cv.m.ClaimNumber
                   }
                   );

                var getAllOnReviewClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if (getAllOnReviewClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "On Review Claims",
                        value = getAllOnReviewClaims.Count
                    };
                    response.data.Add(ClaimDashboardData);
                }
                #endregion

                #region OnHold
                getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "HOL").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                getAllClaims = claimList
                   .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                   .Where(a => a.n.StatusCode == "HOL").Select(
                   cv => new
                   {
                       cv.m.ClaimNumber
                   }
                   );

                var getAllOnHoldClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if (getAllOnHoldClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "On Review Claims",
                        value = getAllOnHoldClaims.Count
                    };
                    response.data.Add(ClaimDashboardData);
                }
                #endregion

                #region RequestingInfo
                getAllClaimSubmission = claimSubmissionList
                    .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                    .Where(a => a.n.StatusCode == "REQ").Select(
                    cv => new
                    {
                        cv.m.ClaimNumber
                    }
                    );

                getAllClaims = claimList
                   .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                   .Where(a => a.n.StatusCode == "REQ").Select(
                   cv => new
                   {
                       cv.m.ClaimNumber
                   }
                   );

                var getAllRequestingInfoClaims = getAllClaimSubmission.Union(getAllClaims).ToList();

                if (getAllRequestingInfoClaims.Count != 0)
                {
                    var ClaimDashboardData = new ClaimDashboardData()
                    {
                        label = "Requesting Info Claims",
                        value = getAllRequestingInfoClaims.Count
                    };
                    response.data.Add(ClaimDashboardData);
                }
                #endregion

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.status = "Error";
            }


            return response;
        }

        internal object GetAllSubmittedClaimsByUserId(ClaimListRequestDto claimListRequestDto)
        {
            object response = null;
            try
            {
                if (claimListRequestDto == null)
                {
                    return response;
                }

                Guid dealerId = Guid.Empty;
                Boolean isSearch = false;
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEntityManager = new CommonEntityManager();
                if (claimListRequestDto.userType.ToLower() == "du")
                {
                    DealerStaff dealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == claimListRequestDto.loggedInUserId).FirstOrDefault();
                    if (dealerStaff == null)
                    {
                        return response;
                    }

                    dealerId = dealerStaff.DealerId;
                }
                //expression builder
                Expression<Func<ClaimSubmission, bool>> filterClaimSubmissions = PredicateBuilder.True<ClaimSubmission>();

                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();

                if (IsGuid(dealerId.ToString()))
                {
                    filterClaimSubmissions = filterClaimSubmissions.And(a => a.ClaimSubmittedDealerId == dealerId);
                    filterClaim = filterClaim.And(a => a.ClaimSubmittedDealerId == dealerId);
                }

                //searchcriterias
                if (claimListRequestDto.claimSearch != null)
                {


                    if (IsGuid(claimListRequestDto.claimSearch.claimDealerId.ToString()))
                    {
                        filterClaimSubmissions = filterClaimSubmissions.And(a => a.ClaimSubmittedDealerId == claimListRequestDto.claimSearch.claimDealerId);
                        filterClaim = filterClaim.And(a => a.ClaimSubmittedDealerId == claimListRequestDto.claimSearch.claimDealerId);
                        isSearch = true;

                    }
                    if (IsGuid(claimListRequestDto.claimSearch.commodityTypeId.ToString()))
                    {
                        filterClaimSubmissions = filterClaimSubmissions.And(a => a.CommodityTypeId == claimListRequestDto.claimSearch.commodityTypeId);
                        filterClaim = filterClaim.And(a => a.CommodityTypeId == claimListRequestDto.claimSearch.commodityTypeId);
                        isSearch = true;
                    }
                    if (IsGuid(claimListRequestDto.claimSearch.makeId.ToString()))
                    {
                        filterClaimSubmissions = filterClaimSubmissions.And(a => a.MakeId == claimListRequestDto.claimSearch.makeId);
                        filterClaim = filterClaim.And(a => a.MakeId == claimListRequestDto.claimSearch.makeId);
                        isSearch = true;
                    }
                    if (IsGuid(claimListRequestDto.claimSearch.statusId.ToString()))
                    {
                        filterClaimSubmissions = filterClaimSubmissions.And(a => a.StatusId == claimListRequestDto.claimSearch.statusId);
                        filterClaim = filterClaim.And(a => a.StatusId == claimListRequestDto.claimSearch.statusId);
                        isSearch = true;

                    }
                    if (!string.IsNullOrEmpty(claimListRequestDto.claimSearch.policyNo))
                    {
                        filterClaimSubmissions = filterClaimSubmissions.And(a => a.PolicyNumber.ToLower().Contains(claimListRequestDto.claimSearch.policyNo.ToLower()));
                        filterClaim = filterClaim.And(a => a.PolicyNumber.ToLower().Contains(claimListRequestDto.claimSearch.policyNo.ToLower()));
                        isSearch = true;
                    }
                    if (!string.IsNullOrEmpty(claimListRequestDto.claimSearch.claimNo))
                    {


                        if (claimListRequestDto.claimSearch.claimNo == "all")
                        {
                            isSearch = false;

                        }
                        else
                        {
                            filterClaimSubmissions = filterClaimSubmissions.And(a => a.Wip.ToLower().Contains(claimListRequestDto.claimSearch.claimNo.ToLower()));
                            filterClaim = filterClaim.And(a => a.ClaimNumber.ToLower().Contains(claimListRequestDto.claimSearch.claimNo.ToLower()));
                            isSearch = true;
                        }
                    }


                    //   session.Query<ClaimComment>().Where(a => a.ClaimId == ClaimSubmissionId)
                    //.Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                    //.Join(session.Query<Claim>(), o => o.m.ClaimId, p => p.ClaimSubmissionId, (o, p) => new { o, p })
                    //.Join(session.Query<InternalUser>(), q => q.o.m.SentFrom.ToString(), r => r.Id, (q, r) => new { q, r })
                    //.OrderByDescending(or => or.q.o.m.EntryDateTime)

                }

                var claimsubFilterWithVin = session.Query<ClaimSubmission>().Where(filterClaimSubmissions);
                var claimsfilterWithVin = session.Query<Claim>().Where(filterClaim);

                IQueryable<VehicleDetails> vehicle;
                IQueryable<VehiclePolicy> vehiclePolicy;

                IEnumerable<ClaimStatusCode> claimStatus = session.Query<ClaimStatusCode>();
                IEnumerable<CommodityType> commodityType = session.Query<CommodityType>();
                IEnumerable<Dealer> dealer = session.Query<Dealer>();
                IEnumerable<Make> make = session.Query<Make>();
                IEnumerable<Model> model = session.Query<Model>();
                IEnumerable<Currency> currency = session.Query<Currency>();
                IEnumerable<ClaimItem> claimItem = session.Query<ClaimItem>();
                IEnumerable<ClaimSubmissionItem> claimSubmissionItem = session.Query<ClaimSubmissionItem>();
                IEnumerable<ClaimSubmission> claimSubmissionTmp = session.Query<ClaimSubmission>();
                IEnumerable<Claim> claimTmp = session.Query<Claim>();

                if (!string.IsNullOrEmpty(claimListRequestDto.claimSearch.vinNo))
                {
                    vehicle = session.Query<VehicleDetails>().Where(a => a.VINNo.ToLower().Contains(claimListRequestDto.claimSearch.vinNo.ToLower()));
                    vehiclePolicy = session.Query<VehiclePolicy>().Where(b => vehicle.Any(c => c.Id == b.VehicleId));
                    claimsubFilterWithVin = claimsubFilterWithVin.Where(d => vehiclePolicy.Any(e => e.PolicyId == d.PolicyId));
                    claimsfilterWithVin = claimsfilterWithVin.Where(d => vehiclePolicy.Any(e => e.PolicyId == d.PolicyId));

                    var claimSubmissions = claimsubFilterWithVin
                  .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                  .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                  .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                  .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                  .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                  .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                  //.Where(a => a.k.i.q.o.c.n.StatusCode == "")
                  //.OrderByDescending(or => or.k.i.q.o.c.m.EntryDate)
                  .Select(z => new
                  {
                      z.k.i.q.o.c.m.Id,
                      CommodityType = z.k.i.q.o.d.CommodityTypeDescription,
                      z.k.i.q.o.c.m.PolicyNumber,
                      z.k.i.q.o.c.m.ClaimNumber,
                      ClaimDealer = z.k.i.q.p.DealerName,
                      Make = z.k.i.r.MakeName,
                      Model = z.k.j.ModelName,
                      ApprovedAmount = "N/A",
                      ClaimAmount = Math.Round(z.k.i.q.o.c.m.TotalClaimAmount * z.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + z.l.Code,
                      Status = z.k.i.q.o.c.n.StatusCode,
                      Date = z.k.i.q.o.c.m.EntryDate,
                      VINNo = z.k.i.q.o.c.m.VINNo.ToString(),
                      ClaimSubmittedBy = commonEntityManager.GetUserNameById( z.k.i.q.o.c.m.ClaimSubmittedBy),
                      isEndorsed=false,
                      z.k.i.q.o.c.m.Wip
                  }).ToArray()
                  .Where(b => string.IsNullOrEmpty(b.ClaimNumber)).ToArray();

                    var claims = claimsfilterWithVin
                        .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                        .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                        .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                        .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                        .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                        .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                        .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                        //.OrderByDescending(or => or.s.k.i.q.o.c.m.EntryDate)
                        .Select(yz => new
                        {
                            yz.s.k.i.q.o.c.m.Id,
                            CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                            yz.s.k.i.q.o.c.m.PolicyNumber,
                            yz.s.k.i.q.o.c.m.ClaimNumber,
                            ClaimDealer = yz.s.k.i.q.p.DealerName,
                            Make = yz.s.k.i.r.MakeName,
                            Model = yz.s.k.j.ModelName,
                            ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.AuthorizedAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                            ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.TotalClaimAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                            Status = yz.s.k.i.q.o.c.n.StatusCode,
                            Date = yz.s.k.i.q.o.c.m.EntryDate,
                            VINNo = yz.t.VINNo.ToString(),
                            ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.ClaimSubmittedBy),
                            isEndorsed = yz.s.k.i.q.o.c.m.IsEndorsed,
                            yz.t.Wip
                        })//.OrderByDescending(a => a.PolicyNumber)
                        .ToArray();
                    //var claimsEndosed = session.Query<Claim>()
                    //   .Where(filterClaim)
                    //   .Join(claimTmp, z => z.NewClaimId, y => y.Id, (z, y) => new { z, y })
                    //   .Join(claimStatus, m => m.y.StatusId, n => n.Id, (m, n) => new { m, n })
                    //   .Join(commodityType, c => c.m.y.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                    //   .Join(dealer, o => o.c.m.y.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                    //   .Join(make, q => q.o.c.m.y.MakeId, r => r.Id, (q, r) => new { q, r })
                    //   .Join(model, i => i.q.o.c.m.y.ModelId, j => j.Id, (i, j) => new { i, j })
                    //   .Join(currency, k => k.i.q.o.c.m.y.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                    //   .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.z.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                    //   //.OrderByDescending(or => or.s.k.i.q.o.c.m.y.EntryDate)
                    //   .Select(yz => new
                    //   {
                    //       yz.s.k.i.q.o.c.m.y.Id,
                    //       CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                    //       yz.s.k.i.q.o.c.m.y.PolicyNumber,
                    //       yz.s.k.i.q.o.c.m.y.ClaimNumber,
                    //       ClaimDealer = yz.s.k.i.q.p.DealerName,
                    //       Make = yz.s.k.i.r.MakeName,
                    //       Model = yz.s.k.j.ModelName,
                    //       ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.y.AuthorizedAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                    //       ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.y.TotalClaimAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                    //       Status = yz.s.k.i.q.o.c.n.StatusCode,
                    //       Date = yz.s.k.i.q.o.c.m.y.EntryDate,
                    //       VINNo = yz.t.VINNo.ToString(),
                    //       ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.y.ClaimSubmittedBy),
                    //       isEndorsed=yz.s.k.i.q.o.c.m.y.IsEndorsed,
                    //       yz.t.Wip
                    //   })//.OrderByDescending(a => a.PolicyNumber)
                    //   .ToArray();
                    var unionResult = claimSubmissions.Union(claims).OrderByDescending(a => a.Date).Select(a => new
                    {
                        a.Id,
                        a.CommodityType,
                        a.PolicyNumber,
                        a.ClaimNumber,
                        a.ClaimDealer,
                        a.Make,
                        a.Model,
                        a.ApprovedAmount,
                        a.ClaimAmount,
                        a.Status,
                        a.VINNo,
                        Date = a.Date.ToString("dd-MMM-yyyy HH:mm:ss"),
                        a.ClaimSubmittedBy,
                        a.isEndorsed,
                        a.Wip
                    });
                    var totalRecords = unionResult.Count();
                    var claimDetailsFilterd = unionResult
                        .Skip((claimListRequestDto.page - 1) * claimListRequestDto.pageSize)
                        .Take(claimListRequestDto.pageSize);
                    var gridResponse = new CommonGridResponseDto()
                    {
                        totalRecords = totalRecords,
                        data = claimDetailsFilterd
                    };
                    response = new JavaScriptSerializer().Serialize(gridResponse);

                }

                else
                {
                    if (isSearch)
                    {
                        var claimSubmissions = session.Query<ClaimSubmission>()
                       .Where(filterClaimSubmissions)
                       .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                       .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                       .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                       .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                       .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                       .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                       //.OrderByDescending(or => or.k.i.q.o.c.m.EntryDate)
                       .Select(z => new
                       {
                           z.k.i.q.o.c.m.Id,
                           CommodityType = z.k.i.q.o.d.CommodityTypeDescription,
                           z.k.i.q.o.c.m.PolicyNumber,
                           z.k.i.q.o.c.m.ClaimNumber,
                           ClaimDealer = z.k.i.q.p.DealerName,
                           Make = z.k.i.r.MakeName,
                           Model = z.k.j.ModelName,
                           ApprovedAmount = "N/A",
                           ClaimAmount = Math.Round(z.k.i.q.o.c.m.TotalClaimAmount * z.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + z.l.Code,
                           Status = z.k.i.q.o.c.n.StatusCode,
                           Date = z.k.i.q.o.c.m.EntryDate,
                           VINNo = z.k.i.q.o.c.m.VINNo.ToString(),
                           ClaimSubmittedBy = commonEntityManager.GetUserNameById(z.k.i.q.o.c.m.ClaimSubmittedBy),
                           isEndorsed = false,
                           z.k.i.q.o.c.m.Wip,
                           GoodWill=false
                       }).ToArray()
                       .Where(b => string.IsNullOrEmpty(b.ClaimNumber)).ToArray();

                        var claims = session.Query<Claim>()
                            .Where(filterClaim)
                            .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                            .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                            .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                            .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                            .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                            .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                            .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                            //.OrderByDescending(or => or.s.k.i.q.o.c.m.EntryDate)
                            .Select(yz => new
                            {
                                yz.s.k.i.q.o.c.m.Id,
                                CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                yz.s.k.i.q.o.c.m.PolicyNumber,
                                yz.s.k.i.q.o.c.m.ClaimNumber,
                                ClaimDealer = yz.s.k.i.q.p.DealerName,
                                Make = yz.s.k.i.r.MakeName,
                                Model = yz.s.k.j.ModelName,
                                ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.AuthorizedAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.TotalClaimAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                Status = yz.s.k.i.q.o.c.n.StatusCode,
                                Date = yz.s.k.i.q.o.c.m.EntryDate,
                                VINNo = yz.t.VINNo.ToString(),
                                ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.ClaimSubmittedBy),
                                isEndorsed = yz.s.k.i.q.o.c.m.IsEndorsed,
                                yz.t.Wip,
                                GoodWill = yz.s.k.i.q.o.c.m.IsGoodwill
                            })//.OrderByDescending(a => a.PolicyNumber)
                            .ToArray();
                        var claimsEndosed = session.Query<Claim>()
                           .Where(filterClaim)
                           .Join(claimTmp, z => z.NewClaimId, y => y.Id, (z, y) => new { z, y })
                           .Join(claimStatus, m => m.y.StatusId, n => n.Id, (m, n) => new { m, n })
                           .Join(commodityType, c => c.m.y.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                           .Join(dealer, o => o.c.m.y.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                           .Join(make, q => q.o.c.m.y.MakeId, r => r.Id, (q, r) => new { q, r })
                           .Join(model, i => i.q.o.c.m.y.ModelId, j => j.Id, (i, j) => new { i, j })
                           .Join(currency, k => k.i.q.o.c.m.y.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                           .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.z.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                           //.OrderByDescending(or => or.s.k.i.q.o.c.m.y.EntryDate)
                           .Select(yz => new
                           {
                               yz.s.k.i.q.o.c.m.y.Id,
                               CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                               yz.s.k.i.q.o.c.m.y.PolicyNumber,
                               yz.s.k.i.q.o.c.m.y.ClaimNumber,
                               ClaimDealer = yz.s.k.i.q.p.DealerName,
                               Make = yz.s.k.i.r.MakeName,
                               Model = yz.s.k.j.ModelName,
                               ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.y.AuthorizedAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                               ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.y.TotalClaimAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                               Status = yz.s.k.i.q.o.c.n.StatusCode,
                               Date = yz.s.k.i.q.o.c.m.y.EntryDate,
                               VINNo = yz.t.VINNo.ToString(),
                               ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.y.ClaimSubmittedBy),
                               isEndorsed = yz.s.k.i.q.o.c.m.y.IsEndorsed,
                               yz.t.Wip,
                               GoodWill = yz.s.k.i.q.o.c.m.y.IsGoodwill
                           })//.OrderByDescending(a => a.PolicyNumber)
                           .ToArray();
                        var unionResult = claimSubmissions.Union(claims).Union(claimsEndosed).OrderByDescending(a => a.Date).Select(a => new
                        {
                            a.Id,
                            a.CommodityType,
                            a.PolicyNumber,
                            a.ClaimNumber,
                            a.ClaimDealer,
                            a.Make,
                            a.Model,
                            a.ApprovedAmount,
                            a.ClaimAmount,
                            a.Status,
                            a.VINNo,
                            Date = a.Date.ToString("dd-MMM-yyyy HH:mm:ss"),
                            a.ClaimSubmittedBy,
                            a.isEndorsed,
                            a.Wip,
                            a.GoodWill
                        });

                        var totalRecords = unionResult.Count();
                        var claimDetailsFilterd = unionResult
                            .Skip((claimListRequestDto.page - 1) * claimListRequestDto.pageSize)
                            .Take(claimListRequestDto.pageSize);
                        var gridResponse = new CommonGridResponseDto()
                        {
                            totalRecords = totalRecords,
                            data = claimDetailsFilterd
                        };
                        response = new JavaScriptSerializer().Serialize(gridResponse);
                    }
                    else {

                        if (claimListRequestDto.claimSearch.claimNo == "all")
                        {


                            var claims = session.Query<Claim>()
                                .Where(filterClaim)
                                .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                                .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                                .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                                .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                                .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                                .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                                .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                                //.OrderByDescending(or => or.s.k.i.q.o.c.m.EntryDate)
                                .Select(yz => new
                                {
                                    yz.s.k.i.q.o.c.m.Id,
                                    CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                    yz.s.k.i.q.o.c.m.PolicyNumber,
                                    yz.s.k.i.q.o.c.m.ClaimNumber,
                                    ClaimDealer = yz.s.k.i.q.p.DealerName,
                                    Make = yz.s.k.i.r.MakeName,
                                    Model = yz.s.k.j.ModelName,
                                    ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.AuthorizedAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.TotalClaimAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    Status = yz.s.k.i.q.o.c.n.StatusCode,
                                    Date = yz.s.k.i.q.o.c.m.EntryDate.ToString("dd-MMM-yyyy"),
                                    VINNo = yz.t.VINNo.ToString(),
                                    ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.ClaimSubmittedBy),
                                    GoodWill =yz.s.k.i.q.o.c.m.IsGoodwill,
                                    isEndorsed = yz.s.k.i.q.o.c.m.IsEndorsed,
                                    yz.t.Wip
                                })//.OrderByDescending(a => a.PolicyNumber)
                            .Where(b => b.Status != "Approved" ).ToArray();


                            var totalRecords = claims.Count();
                            var claimDetailsFilterd = claims
                                .Skip((claimListRequestDto.page - 1) * claimListRequestDto.pageSize)
                                .Take(claimListRequestDto.pageSize);
                            var gridResponse = new CommonGridResponseDto()
                            {
                                totalRecords = totalRecords,
                                data = claimDetailsFilterd
                            };
                            response = new JavaScriptSerializer().Serialize(gridResponse);
                        }

                        else
                        {

                            var claimSubmissions = session.Query<ClaimSubmission>()
                      .Where(filterClaimSubmissions)
                      .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                      .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                      .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                      .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                      .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                      .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                      //.OrderByDescending(or => or.k.i.q.o.c.m.EntryDate)
                      .Select(z => new
                      {
                          z.k.i.q.o.c.m.Id,
                          CommodityType = z.k.i.q.o.d.CommodityTypeDescription,
                          z.k.i.q.o.c.m.PolicyNumber,
                          z.k.i.q.o.c.m.ClaimNumber,
                          ClaimDealer = z.k.i.q.p.DealerName,
                          Make = z.k.i.r.MakeName,
                          Model = z.k.j.ModelName,
                          ApprovedAmount = "N/A",
                          ClaimAmount = Math.Round(z.k.i.q.o.c.m.TotalClaimAmount * z.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + z.l.Code,
                          Status = z.k.i.q.o.c.n.StatusCode,
                          Date = z.k.i.q.o.c.m.EntryDate,
                          VINNo = z.k.i.q.o.c.m.VINNo.ToString(),
                          ClaimSubmittedBy = commonEntityManager.GetUserNameById(z.k.i.q.o.c.m.ClaimSubmittedBy),
                          isEndorsed = false,
                           z.k.i.q.o.c.m.Wip,
                          GoodWill=false

                      }).ToArray()
                      .Where(b => string.IsNullOrEmpty(b.ClaimNumber) && (b.Status != "Approved" && b.Status != "Rejected")).ToArray();

                            var claims = session.Query<Claim>()
                                .Where(filterClaim)
                                .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                                .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                                .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                                .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                                .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                                .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                                .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                                //.OrderByDescending(or => or.s.k.i.q.o.c.m.EntryDate)
                                .Select(yz => new
                                {
                                    yz.s.k.i.q.o.c.m.Id,
                                    CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                    yz.s.k.i.q.o.c.m.PolicyNumber,
                                    yz.s.k.i.q.o.c.m.ClaimNumber,
                                    ClaimDealer = yz.s.k.i.q.p.DealerName,
                                    Make = yz.s.k.i.r.MakeName,
                                    Model = yz.s.k.j.ModelName,
                                    ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.AuthorizedAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.TotalClaimAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    Status = yz.s.k.i.q.o.c.n.StatusCode,
                                    Date = yz.s.k.i.q.o.c.m.EntryDate,
                                    VINNo = yz.t.VINNo.ToString(),
                                    ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.ClaimSubmittedBy),
                                    isEndorsed = yz.s.k.i.q.o.c.m.IsEndorsed,
                                    yz.t.Wip,
                                    GoodWill = yz.s.k.i.q.o.c.m.IsGoodwill
                                })//.OrderByDescending(a => a.PolicyNumber)

                            .Where(b => b.Status != "Approved" && b.Status != "Rejected").ToArray();
                            var claimsEndosed = session.Query<Claim>()
                               .Where(filterClaim)
                               .Join(claimTmp, z => z.NewClaimId, y => y.Id, (z, y) => new { z, y })
                               .Join(claimStatus, m => m.y.StatusId, n => n.Id, (m, n) => new { m, n })
                               .Join(commodityType, c => c.m.y.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                               .Join(dealer, o => o.c.m.y.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                               .Join(make, q => q.o.c.m.y.MakeId, r => r.Id, (q, r) => new { q, r })
                               .Join(model, i => i.q.o.c.m.y.ModelId, j => j.Id, (i, j) => new { i, j })
                               .Join(currency, k => k.i.q.o.c.m.y.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                               .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.z.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                               //.OrderByDescending(or => or.s.k.i.q.o.c.m.y.EntryDate)
                               .Select(yz => new
                               {
                                   yz.s.k.i.q.o.c.m.y.Id,
                                   CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                   yz.s.k.i.q.o.c.m.y.PolicyNumber,
                                   yz.s.k.i.q.o.c.m.y.ClaimNumber,
                                   ClaimDealer = yz.s.k.i.q.p.DealerName,
                                   Make = yz.s.k.i.r.MakeName,
                                   Model = yz.s.k.j.ModelName,
                                   ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.y.AuthorizedAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                   ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.y.TotalClaimAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                   Status = yz.s.k.i.q.o.c.n.StatusCode,
                                   Date = yz.s.k.i.q.o.c.m.y.EntryDate,
                                   VINNo = yz.t.VINNo.ToString(),
                                   ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.y.ClaimSubmittedBy),
                                   isEndorsed = yz.s.k.i.q.o.c.m.y.IsEndorsed,
                                   yz.t.Wip,
                                   GoodWill = yz.s.k.i.q.o.c.m.y.IsGoodwill
                               })//.OrderByDescending(a => a.PolicyNumber)
                               .Where(b => b.Status != "Approved" && b.Status != "Rejected").ToArray();
                            var unionResult = claimSubmissions.Union(claims).Union(claimsEndosed).OrderByDescending(a => a.Date).Select(a => new
                            {
                                a.Id,
                                a.CommodityType,
                                a.PolicyNumber,
                                a.ClaimNumber,
                                a.ClaimDealer,
                                a.Make,
                                a.Model,
                                a.ApprovedAmount,
                                a.ClaimAmount,
                                a.Status,
                                a.VINNo,
                                Date = a.Date.ToString("dd-MMM-yyyy HH:mm:ss"),
                                a.ClaimSubmittedBy,
                                a.isEndorsed,
                                a.Wip,
                                a.GoodWill
                            });
                            var totalRecords = unionResult.Count();
                            var claimDetailsFilterd = unionResult
                                .Skip((claimListRequestDto.page - 1) * claimListRequestDto.pageSize)
                                .Take(claimListRequestDto.pageSize);
                            var gridResponse = new CommonGridResponseDto()
                            {
                                totalRecords = totalRecords,
                                data = claimDetailsFilterd
                            };
                            response = new JavaScriptSerializer().Serialize(gridResponse);
                        }



                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal object GetAllSubmittedClaimsForEditByUserId(ClaimListRequestDto claimListRequestDto)
        {
            object response = null;
            try
            {
                if (claimListRequestDto == null)
                {
                    return response;
                }

                Guid dealerId = Guid.Empty;
                Boolean isSearch = false;
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEntityManager = new CommonEntityManager();
                if (claimListRequestDto.userType.ToLower() == "du")
                {
                    DealerStaff dealerStaff = session.Query<DealerStaff>()
                        .Where(a => a.UserId == claimListRequestDto.loggedInUserId).FirstOrDefault();
                    if (dealerStaff == null)
                    {
                        return response;
                    }

                    dealerId = dealerStaff.DealerId;
                }
                //expression builder
                Expression<Func<ClaimSubmission, bool>> filterClaimSubmissions = PredicateBuilder.True<ClaimSubmission>();

                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();

                if (IsGuid(dealerId.ToString()))
                {
                    filterClaimSubmissions = filterClaimSubmissions.And(a => a.ClaimSubmittedDealerId == dealerId);
                    filterClaim = filterClaim.And(a => a.ClaimSubmittedDealerId == dealerId);
                }

                //searchcriterias


                var claimsubFilterWithVin = session.Query<ClaimSubmission>().Where(filterClaimSubmissions);
                var claimsfilterWithVin = session.Query<Claim>().Where(filterClaim);



                IEnumerable<ClaimStatusCode> claimStatus = session.Query<ClaimStatusCode>();
                IEnumerable<CommodityType> commodityType = session.Query<CommodityType>();
                IEnumerable<Dealer> dealer = session.Query<Dealer>();
                IEnumerable<Make> make = session.Query<Make>();
                IEnumerable<Model> model = session.Query<Model>();
                IEnumerable<Currency> currency = session.Query<Currency>();
                IEnumerable<ClaimItem> claimItem = session.Query<ClaimItem>();
                IEnumerable<ClaimSubmissionItem> claimSubmissionItem = session.Query<ClaimSubmissionItem>();
                IEnumerable<ClaimSubmission> claimSubmissionTmp = session.Query<ClaimSubmission>();
                IEnumerable<Claim> claimTmp = session.Query<Claim>();


                     var claimSubmissions = session.Query<ClaimSubmission>()
                      .Where(filterClaimSubmissions)
                      .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                      .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                      .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                      .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                      .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                      .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                      //.OrderByDescending(or => or.k.i.q.o.c.m.EntryDate)
                      .Where(a => a.k.i.q.o.c.n.StatusCode == "SUB")
                      .Select(z => new
                      {
                          z.k.i.q.o.c.m.Id,
                          CommodityType = z.k.i.q.o.d.CommodityTypeDescription,
                          z.k.i.q.o.c.m.PolicyNumber,
                          z.k.i.q.o.c.m.ClaimNumber,
                          ClaimDealer = z.k.i.q.p.DealerName,
                          Make = z.k.i.r.MakeName,
                          Model = z.k.j.ModelName,
                          ApprovedAmount = "N/A",
                          ClaimAmount = Math.Round(z.k.i.q.o.c.m.TotalClaimAmount * z.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + z.l.Code,
                          Status = z.k.i.q.o.c.n.Description,
                          Date = z.k.i.q.o.c.m.EntryDate,
                          VINNo = z.k.i.q.o.c.m.VINNo.ToString(),
                          ClaimSubmittedBy = commonEntityManager.GetUserNameById(z.k.i.q.o.c.m.ClaimSubmittedBy),

                      }).ToArray()
                      .Where(b => string.IsNullOrEmpty(b.ClaimNumber) && (b.Status != "Approved" && b.Status != "Rejected")).ToArray();

                            var claims = session.Query<Claim>()
                                .Where(filterClaim)
                                .Join(claimStatus, m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                                .Join(commodityType, c => c.m.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                                .Join(dealer, o => o.c.m.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                                .Join(make, q => q.o.c.m.MakeId, r => r.Id, (q, r) => new { q, r })
                                .Join(model, i => i.q.o.c.m.ModelId, j => j.Id, (i, j) => new { i, j })
                                .Join(currency, k => k.i.q.o.c.m.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                                .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                                //.OrderByDescending(or => or.s.k.i.q.o.c.m.EntryDate)
                                .Where(a => a.s.k.i.q.o.c.n.StatusCode == "SUB")
                                .Select(yz => new
                                {
                                    yz.s.k.i.q.o.c.m.Id,
                                    CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                    yz.s.k.i.q.o.c.m.PolicyNumber,
                                    yz.s.k.i.q.o.c.m.ClaimNumber,
                                    ClaimDealer = yz.s.k.i.q.p.DealerName,
                                    Make = yz.s.k.i.r.MakeName,
                                    Model = yz.s.k.j.ModelName,
                                    ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.AuthorizedAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.TotalClaimAmount * yz.s.k.i.q.o.c.m.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                    Status = yz.s.k.i.q.o.c.n.Description,
                                    Date = yz.s.k.i.q.o.c.m.EntryDate,
                                    VINNo = yz.t.VINNo.ToString(),
                                    ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.ClaimSubmittedBy),
                                })//.OrderByDescending(a => a.PolicyNumber)
                            .Where(b => b.Status != "Approved" && b.Status != "Rejected").ToArray();
                            var claimsEndosed = session.Query<Claim>()
                               .Where(filterClaim)
                               .Join(claimTmp, z => z.NewClaimId, y => y.Id, (z, y) => new { z, y })
                               .Join(claimStatus, m => m.y.StatusId, n => n.Id, (m, n) => new { m, n })
                               .Join(commodityType, c => c.m.y.CommodityTypeId, d => d.CommodityTypeId, (c, d) => new { c, d })
                               .Join(dealer, o => o.c.m.y.ClaimSubmittedDealerId, p => p.Id, (o, p) => new { o, p })
                               .Join(make, q => q.o.c.m.y.MakeId, r => r.Id, (q, r) => new { q, r })
                               .Join(model, i => i.q.o.c.m.y.ModelId, j => j.Id, (i, j) => new { i, j })
                               .Join(currency, k => k.i.q.o.c.m.y.ClaimCurrencyId, l => l.Id, (k, l) => new { k, l })
                               .Join(claimSubmissionTmp, s => s.k.i.q.o.c.m.z.ClaimSubmissionId, t => t.Id, (s, t) => new { s, t })
                               //.OrderByDescending(or => or.s.k.i.q.o.c.m.y.EntryDate)
                               .Where(a => a.s.k.i.q.o.c.n.StatusCode == "SUB")
                               .Select(yz => new
                               {
                                   yz.s.k.i.q.o.c.m.y.Id,
                                   CommodityType = yz.s.k.i.q.o.d.CommodityTypeDescription,
                                   yz.s.k.i.q.o.c.m.y.PolicyNumber,
                                   yz.s.k.i.q.o.c.m.y.ClaimNumber,
                                   ClaimDealer = yz.s.k.i.q.p.DealerName,
                                   Make = yz.s.k.i.r.MakeName,
                                   Model = yz.s.k.j.ModelName,
                                   ApprovedAmount = Math.Round(yz.s.k.i.q.o.c.m.y.AuthorizedAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                   ClaimAmount = Math.Round(yz.s.k.i.q.o.c.m.y.TotalClaimAmount * yz.s.k.i.q.o.c.m.y.ConversionRate, 2).ToString("N", CultureInfo.CreateSpecificCulture("en-US")) + " " + yz.s.l.Code,
                                   Status = yz.s.k.i.q.o.c.n.Description,
                                   Date = yz.s.k.i.q.o.c.m.y.EntryDate,
                                   VINNo = yz.t.VINNo.ToString(),
                                   ClaimSubmittedBy = commonEntityManager.GetUserNameById(yz.s.k.i.q.o.c.m.y.ClaimSubmittedBy),
                               })//.OrderByDescending(a => a.PolicyNumber)
                               .Where(b => b.Status != "Approved" && b.Status != "Rejected").ToArray();
                            var unionResult = claimSubmissions.Union(claims).Union(claimsEndosed).OrderByDescending(a => a.Date).Select(a => new
                            {
                                a.Id,
                                a.CommodityType,
                                a.PolicyNumber,
                                a.ClaimNumber,
                                a.ClaimDealer,
                                a.Make,
                                a.Model,
                                a.ApprovedAmount,
                                a.ClaimAmount,
                                a.Status,
                                a.VINNo,
                                Date = a.Date.ToString("dd-MMM-yyyy"),
                                a.ClaimSubmittedBy
                            });
                            var totalRecords = unionResult.Count();
                            var claimDetailsFilterd = unionResult
                                .Skip((claimListRequestDto.page - 1) * claimListRequestDto.pageSize)
                                .Take(claimListRequestDto.pageSize);
                            var gridResponse = new CommonGridResponseDto()
                            {
                                totalRecords = totalRecords,
                                data = claimDetailsFilterd
                            };
                            response = new JavaScriptSerializer().Serialize(gridResponse);


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal object GetClaimRequestDetailsByClaimId(Guid claimId)
        {
            object response = null;
            try
            {
                if (!IsGuid(claimId.ToString()))
                {
                    return response;
                }

                ClaimRequestDetailsResponseDto claimDetails = new ClaimRequestDetailsResponseDto();

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ClaimSubmission claimSubmission = session
                   .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);

                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);
                if (claim != null) {
                    var claimStatusCode = cem.GetClaimStatusCodeById(claim.StatusId);
                    if (claimStatusCode == "REV" || claimStatusCode == "UPD")
                    {
                        claimId = claim.ClaimSubmissionId;
                        claimSubmission = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);

                        // remove already approved or rejected claim parts
                        List<Guid> claimSubmitionIds = session.Query<ClaimSubmissionItem>().Where(a => a.ClaimSubmissionId == claimId).Select(b=>b.Id).ToList();
                        List<ClaimItem> claimItemList = session.Query<ClaimItem>().Where(a => a.ClaimId==claim.Id ).ToList();

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            foreach (ClaimItem ci in claimItemList) {
                            if (!claimSubmitionIds.Contains(ci.Id)) {
                                session.Delete(ci);
                            }
                            }
                            transaction.Commit();
                        }

                        claim = null;
                    }
                }


                if (claimSubmission == null && claim == null)
                {
                    return response;
                }

                bool fromClaim = claim != null;
                if (claim != null)
                {
                    Policy policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claim.PolicyId);
                    Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    CommodityType commodityType = session.Query<CommodityType>().Where(b => b.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                    List<CountryTaxes> countryTaxesList = session.Query<CountryTaxes>().Where(a => a.CountryId == claim.PolicyCountryId).ToList();
                    claimDetails.CountryTaxes = new List<CountryTaxesDe>().OrderBy(a => a.TaxName).ToList();
                    Product product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                    claimDetails.ProductCode = product.Productcode;
                    foreach (var countryTaxes in countryTaxesList)
                    {
                        List<TaxTypes> taxTypesList = session.Query<TaxTypes>().Where(a => a.Id == countryTaxes.TaxTypeId).ToList();

                        foreach (TaxTypes taxTypes in taxTypesList)
                        {
                            CountryTaxesDe taxesDe = new CountryTaxesDe()
                            {
                                TaxName = taxTypes.TaxName,
                                TaxTypesId = taxTypes.Id,
                                TaxValue = countryTaxes.TaxValue,
                                MinimumValue = countryTaxes.MinimumValue,
                                IsPercentage = countryTaxes.IsPercentage
                            };
                            claimDetails.CountryTaxes.Add(taxesDe);

                        }


                    }
                    List<ClaimItem> claimItems = session.Query<ClaimItem>()
                       .Where(a => a.ClaimId == claimId).OrderBy(a => a.ItemName).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>().OrderByDescending(a => a.itemName).ToList();
                    int claimItemseq = 0;
                    foreach (ClaimItem claimItem in claimItems)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice = Math.Round(claimItem.TotalPrice * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claim.ClaimCurrencyId,
                            //    claim.CurrencyPeriodId),
                            unitPrice = Math.Round(claimItem.UnitPrice * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claim.ClaimCurrencyId,
                            //    claim.CurrencyPeriodId),
                            authorizedAmount = claimItem.IsApproved == true ? (Math.Round(claimItem.AuthorizedAmount * claimItem.ConversionRate * 100) / 100) : (claimItem.IsApproved == false ? 0 : 0),
                            //currencyEm.ConvertFromBaseCurrency(claimItem.AuthorizedAmount, claim.ClaimCurrencyId,
                            //        claim.CurrencyPeriodId),
                            faultId = claimItem.FaultId,
                            faultName = cem.GetFaultNameById(claimItem.FaultId),
                            status = claimItem.IsApproved == true ? "Approved" : (claimItem.IsApproved == false ? "Rejected" : ""),
                            statusCode = claimItem.IsApproved == true ? "A" : (claimItem.IsApproved == false ? "R" : ""),
                            discountAmount = Math.Round(claimItem.DiscountAmount * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claim.ClaimCurrencyId,
                            //        claim.CurrencyPeriodId),
                            goodWillAmount = Math.Round(claimItem.GoodWillAmount * claimItem.ConversionRate * 100) / 100,
                            //    currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claim.ClaimCurrencyId,
                            //claim.CurrencyPeriodId),
                            comment = claimItem.TpaComment,
                            isDiscountPercentage = claimItem.IsDiscountPercentage,
                            isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                            totalGrossPrice = Math.Round(claimItem.UnitPrice * claimItem.Quantity * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), claim.ClaimCurrencyId,
                            //        claim.CurrencyPeriodId),
                            discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate :
                            Math.Round(claimItem.DiscountRate * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, claim.ClaimCurrencyId,
                            //        claim.CurrencyPeriodId),
                            goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate :
                            Math.Round(claimItem.GoodWillRate * claimItem.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, claim.ClaimCurrencyId,
                            //claim.CurrencyPeriodId),
                            parentId = claimItem.ParentId,
                            discountSchemeCode = claimItem.DiscountSchemeCode,
                            discountSchemeId = claimItem.DiscountSchemeId,
                            rejectionTypeId = claimItem.RejectionTypeId,
                            causeOfFailureId = claimItem.CauseOfFailureId,
                            UnUsedTireDepth = cem.getUnusedTyreDepthByClaimItemId(claimItem.Id , commodityType.CommodityTypeDescription)

                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claim.ClaimNumber;
                    claimDetails.DealerName = cem.GetDealerNameById(claim.ClaimSubmittedDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claim.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claim.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount,
                    claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.AuthorizedClaimAmount = claim.IsApproved == true ? (Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100) : (claim.IsApproved == false ? 0 : 0);
                    //currencyEm.ConvertFromBaseCurrency(claim.AuthorizedAmount,
                    //claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.TotalDiscountAmount = Math.Round((claim.TotalGrossClaimAmount - claim.TotalClaimAmount) * claim.ConversionRate * 100) / 100;
                    //currencyEm.ConvertFromBaseCurrency(
                    //(claim.TotalGrossClaimAmount - claim.TotalClaimAmount), claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claim.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claim.PolicyCountryId);
                    claimDetails.ClaimDate = claim.ClaimDate.ToString("dd-MMM-yyyy");
                    claimDetails.ClaimMileage = claim.ClaimMileageKm.ToString();
                    var claimStatusCode = cem.GetClaimStatusCodeById(claim.StatusId);
                    claimDetails.ClaimStatus = claimStatusCode;
                    claimDetails.IsGoodwillClaim = claim.IsGoodwill;
                    claimDetails.PolicyId = claim.PolicyId;
                    //colmplaint
                    claimDetails.Complaint = new Complaint
                    {
                        customer = claim.CustomerComplaint,
                        dealer = claim.DealerComment,
                        conclution = claim.Conclution,
                        engineer = claim.EngineerComment
                    };
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claim.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claim.CommodityCategoryId;
                    claimDetails.MakeId = claim.MakeId;
                    claimDetails.ModelId = claim.ModelId;
                    claimDetails.DealerId = claim.ClaimSubmittedDealerId;
                    claimDetails.PolicyId = claim.PolicyId;
                    claimDetails.PolicyDealerId = claim.PolicyDealerId;
                    claimDetails.PolicyCountryId = claim.PolicyCountryId;

                    claimDetails.ClaimDate = claim.ClaimDate.ToString("dd/MMM/yyyy");
                    claimDetails.CustomerName = cem.GetCustomerNameById(claim.CustomerId);
                    claimDetails.CustomeMobileNo = cem.GetCustomerMobileNumberById(claim.CustomerId);
                    claimDetails.totalLiability = Convert.ToDecimal(Math.Round(contract.LiabilityLimitation * claim.ConversionRate * 100) / 100);
                    //show dealer submitted data in claim view popup
                    claimSubmission = session
                    .Query<ClaimSubmission>().FirstOrDefault(a => a.ClaimNumber.ToLower().Trim() == claim.ClaimNumber.ToLower().Trim());


                    var claimComments = session.Query<ClaimComment>().Where(a => a.ClaimId == claim.Id).OrderByDescending(b => b.EntryDateTime).FirstOrDefault();
                    if (claimComments != null) {
                        claimDetails.lastComment = claimComments.Comment;
                    }
                    if (claimSubmission != null)
                    {

                        //claimDetails.CustomerName = cem.GetCustomerNameById(claimSubmission.CustomerId);
                        claimDetails.VINNO = claimSubmission.VINNo;
                        claimDetails.PlateNo = claimSubmission.PlateNo;
                        claimDetails.LastServiceMileage = claimSubmission.LastServiceMileage.ToString("N0", CultureInfo.InvariantCulture);
                        claimDetails.LastServiceDate = claimSubmission.LastServiceDate.ToString("dd/MMM/yyyy");
                        claimDetails.RepairCenter = claimSubmission.RepairCenter;
                        claimDetails.RepairCenterLocation = claimSubmission.RepairCenterLocation;
                        claimDetails.Make = cem.GetMakeNameById(claimSubmission.MakeId);
                        claimDetails.Model = cem.GetModelNameById(claimSubmission.ModelId);
                        claimDetails.CustomerNameCS = claimSubmission.CustomerName;
                        claimDetails.DealerCode = cem.GetDealerCodeById(claimSubmission.PolicyDealerId);
                        claimDetails.Wip = claimSubmission.Wip;
                        claimDetails.FailureDate = claimSubmission.FailureDate.ToString("dd/MMM/yyyy"); ;
                        claimDetails.FailureMilege = claimSubmission.FailureMileage;
                        claimDetails.ClaimAmount = (Math.Round(claimSubmission.TotalClaimAmount* claimSubmission.ConversionRate*100)/100);
                    }
                    response = claimDetails;
                }
                else
                {
                    Policy policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claimSubmission.PolicyId);
                    Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    CommodityType commodityType = session.Query<CommodityType>().Where(b => b.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                    List<CountryTaxes> countryTaxesList = session.Query<CountryTaxes>().Where(a => a.CountryId == claimSubmission.PolicyCountryId).ToList();
                    claimDetails.CountryTaxes = new List<CountryTaxesDe>().OrderBy(a => a.TaxName).ToList();

                    Product product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                    claimDetails.ProductCode = product.Productcode;

                    foreach (var countryTaxes in countryTaxesList)
                    {
                        List<TaxTypes> taxTypesList = session.Query<TaxTypes>().Where(a => a.Id == countryTaxes.TaxTypeId).ToList();

                        foreach (TaxTypes taxTypes in taxTypesList)
                        {
                            CountryTaxesDe taxesDe = new CountryTaxesDe()
                            {
                                TaxName = taxTypes.TaxName,
                                TaxTypesId = taxTypes.Id,
                                TaxValue = countryTaxes.TaxValue,
                                MinimumValue = countryTaxes.MinimumValue,
                                IsPercentage = countryTaxes.IsPercentage
                            };
                            claimDetails.CountryTaxes.Add(taxesDe);

                        }


                    }
                    //reading claim item information
                    List<ClaimSubmissionItem> claimItemList = session.Query<ClaimSubmissionItem>()
                        .Where(a => a.ClaimSubmissionId == claimId).OrderByDescending(a => a.ItemName).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>().OrderByDescending(a => a.itemName).ToList();
                    int claimItemseq = 0;
                    foreach (ClaimSubmissionItem claimItem in claimItemList)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice = Math.Round(claimItem.TotalPrice * claimSubmission.ConversionRate * 100) / 100,
                            parentId = claimItem.ParentId,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claimSubmission.ClaimCurrencyId,
                            //    claimSubmission.CurrencyPeriodId),
                            unitPrice = Math.Round(claimItem.UnitPrice * claimSubmission.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claimSubmission.ClaimCurrencyId,
                            //    claimSubmission.CurrencyPeriodId),
                            discountAmount = Math.Round(claimItem.DiscountAmount * claimSubmission.ConversionRate * 100) / 100,
                            //currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claimSubmission.ClaimCurrencyId,
                            //claimSubmission.CurrencyPeriodId),
                            goodWillAmount = Math.Round(claimItem.GoodWillAmount * claimSubmission.ConversionRate * 100) / 100,
                            UnUsedTireDepth = cem.getUnusedTyreDepthByClaimItemId(claimItem.Id, commodityType.CommodityTypeDescription)
                            //currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId)
                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claimSubmission.ClaimNumber;
                    claimDetails.Wip = claimSubmission.Wip;
                    claimDetails.DealerName = cem.GetDealerNameById(claimSubmission.ClaimSubmittedDealerId);
                    claimDetails.DealerCode = cem.GetDealerCodeById(claimSubmission.PolicyDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claimSubmission.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claimSubmission.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = Math.Round(claimSubmission.TotalClaimAmount * claimSubmission.ConversionRate * 100) / 100;
                    //currencyEm.ConvertFromBaseCurrency(claimSubmission.TotalClaimAmount,
                    //claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId);
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claimSubmission.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claimSubmission.PolicyCountryId);
                    //colmplaint
                    claimDetails.Complaint = new Complaint();
                    claimDetails.Complaint.customer = claimSubmission.CustomerComplaint;
                    claimDetails.Complaint.dealer = claimSubmission.DealerComment;
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claimSubmission.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claimSubmission.CommodityCategoryId;
                    claimDetails.MakeId = claimSubmission.MakeId;
                    claimDetails.ModelId = claimSubmission.ModelId;
                    claimDetails.DealerId = claimSubmission.ClaimSubmittedDealerId;
                    claimDetails.PolicyDealerId = claimSubmission.PolicyDealerId;
                    claimDetails.PolicyCountryId = claimSubmission.PolicyCountryId;
                    claimDetails.ClaimDate = claimSubmission.ClaimDate.ToString("dd/MMM/yyyy");
                    claimDetails.CustomerName = cem.GetCustomerNameById(claimSubmission.CustomerId);
                    claimDetails.CustomeMobileNo = claimSubmission.MobileNo;
                    claimDetails.VINNO = claimSubmission.VINNo;
                    claimDetails.PlateNo = claimSubmission.PlateNo;
                    claimDetails.LastServiceMileage = claimSubmission.LastServiceMileage.ToString("N0", CultureInfo.InvariantCulture); ;
                    claimDetails.LastServiceDate = claimSubmission.LastServiceDate.ToString("dd/MMM/yyyy");
                    claimDetails.RepairCenter = claimSubmission.RepairCenter;
                    claimDetails.RepairCenterLocation = claimSubmission.RepairCenterLocation;
                    claimDetails.Make = cem.GetMakeNameById(claimSubmission.MakeId);
                    claimDetails.Model = cem.GetModelNameById(claimSubmission.ModelId);
                    claimDetails.CustomerNameCS = claimSubmission.CustomerName;
                    claimDetails.PolicyId = claimSubmission.PolicyId;
                    claimDetails.ClaimStatus = cem.GetClaimStatusCodeById(claimSubmission.StatusId);
                    claimDetails.ClaimMileage = claimSubmission.ClaimMileage.ToString();
                    claimDetails.FailureDate = claimSubmission.FailureDate.ToString("dd/MMM/yyyy"); ;
                    claimDetails.FailureMilege = claimSubmission.FailureMileage;
                    claimDetails.totalLiability = Convert.ToDecimal( Math.Round(contract.LiabilityLimitation * claimSubmission.ConversionRate * 100) / 100);
                    response = claimDetails;
                }


            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal object GetClaimDetailsForEditOtherTire(Guid claimId, Guid userId)
        {
            ClaimDetailsForEditOtherTireResponseDto claimDetailsForEditOtherTireResponseDto = new ClaimDetailsForEditOtherTireResponseDto();
            try
            {
                claimDetailsForEditOtherTireResponseDto.Status = "ok";

                #region "validation"
                //request validation
                if (!IsGuid(claimId.ToString()) || !IsGuid(userId.ToString()))
                {
                    claimDetailsForEditOtherTireResponseDto.Status = "Request data invalid";
                    return claimDetailsForEditOtherTireResponseDto;
                }
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                    .Where(a => a.Id == claimId).FirstOrDefault();
                if (claimSubmission == null)
                {
                    claimDetailsForEditOtherTireResponseDto.Status = "Request data invalid";
                    return claimDetailsForEditOtherTireResponseDto;
                }
                //claim status validation
                ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                    .Where(a => a.Id == claimSubmission.StatusId)
                    .FirstOrDefault();

                if (claimStatus != null)
                {
                    if (claimStatus.StatusCode != "SUB" && claimStatus.StatusCode != "REQ")
                    {
                        claimDetailsForEditOtherTireResponseDto.Status = "Selected claim cannot be edited.Its in " +
                                                                claimStatus.Description + " state.";
                        return claimDetailsForEditOtherTireResponseDto;
                    }
                }
                else
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": No Claim status found for claimId-" +
                                 claimSubmission.Id);
                    claimDetailsForEditOtherTireResponseDto.Status = "Error in claim status codes.";
                    return claimDetailsForEditOtherTireResponseDto;
                }


                //user validation
                SystemUser systemUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == userId).FirstOrDefault();
                if (systemUser == null)
                {
                    claimDetailsForEditOtherTireResponseDto.Status = "Requested user not found in the system";
                    return claimDetailsForEditOtherTireResponseDto;
                }

                UserType userType = session.Query<UserType>()
                    .FirstOrDefault(a => a.Id == systemUser.UserTypeId);
                if (userType == null || userType.Code.ToUpper() != "DU")
                {
                    claimDetailsForEditOtherTireResponseDto.Status = "Requested user not allowed to perform update claim.";
                    return claimDetailsForEditOtherTireResponseDto;
                }

                DealerStaff dealerStaff = session.Query<DealerStaff>()
                    .Where(a => a.UserId == userId && a.DealerId == claimSubmission.ClaimSubmittedDealerId)
                    .FirstOrDefault();
                if (dealerStaff == null)
                {
                    claimDetailsForEditOtherTireResponseDto.Status = "Requested claim not belongs to logged in user's dealer.";
                    return claimDetailsForEditOtherTireResponseDto;
                }

                #endregion

                claimDetailsForEditOtherTireResponseDto.ClaimDetails = GetClaimRequestDetailsByClaimId(claimId) as ClaimRequestDetailsResponseDto;

                claimDetailsForEditOtherTireResponseDto.ClaimDetailsTire = GetClaimRequestDetailsOtherTireByClaimId(claimDetailsForEditOtherTireResponseDto.ClaimDetails.VINNO, claimId) as ClaimRequestDetailsOtherTireResponseDto;

                claimDetailsForEditOtherTireResponseDto.ClaimId = claimId;
                claimDetailsForEditOtherTireResponseDto.PolicyDetails = new PolicyDetails()
                {
                    DealerId = claimSubmission.ClaimSubmittedDealerId,
                    CommodityTypeId = claimSubmission.CommodityTypeId,
                    CommodityCategoryId = claimSubmission.CommodityCategoryId,
                    CustomerId = claimSubmission.CustomerId,
                    MakeId = claimSubmission.MakeId,
                    ModelId = claimSubmission.ModelId,
                    PolicyId = claimSubmission.PolicyId,
                    CustomerComplaint = claimSubmission.CustomerComplaint,
                    DealerComment = claimSubmission.DealerComment,
                    ClaimMileage = claimSubmission.ClaimMileage
                };
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return claimDetailsForEditOtherTireResponseDto;
        }

        private ClaimRequestDetailsOtherTireResponseDto GetClaimRequestDetailsOtherTireByClaimId(string VINNO, Guid claimId)
        {
            ClaimRequestDetailsOtherTireResponseDto response = new ClaimRequestDetailsOtherTireResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(a => a.Code == VINNO).FirstOrDefault();

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();

                List<RequestDetailsOtherTire> RequestDetailsOtherTire = new List<RequestDetailsOtherTire>();

                List<ClaimRequestDetailsOtherTireResponseDto> ClaimRequest = new List<ClaimRequestDetailsOtherTireResponseDto>();
                foreach (var ICD in invoiceCodeDetails)
                {
                    List<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>()
                    .Where(a => a.InvoiceCodeDetailId == ICD.Id).ToList();

                    foreach (var invoiceCD in invoiceCodeTireDetails)
                    {
                        List<ClaimSubmission> claimSubmission = session.Query<ClaimSubmission>().Where(a => a.PolicyId == ICD.PolicyId && a.Id == claimId).ToList();

                        foreach (var claimSub in claimSubmission)
                        {
                            List<ClaimSubmissionItem> claimSubmissionItem = session.Query<ClaimSubmissionItem>()
                            .Where(b => b.ClaimSubmissionId == claimSub.Id).ToList();


                            foreach (var CSItem in claimSubmissionItem)
                            {
                                ClaimItemTireDetails claimItemTireDetails = session.Query<ClaimItemTireDetails>()
                            .Where(a => a.ClaimItemId == CSItem.Id && a.InvoiceCodeTireId == invoiceCD.Id).FirstOrDefault();

                                if (claimItemTireDetails != null)
                                {
                                    RequestDetailsOtherTire c = new RequestDetailsOtherTire()
                                    {
                                        UnUsedTireDepth = claimItemTireDetails.UnUsedTireDepth,
                                        InvoiceCodeDetailsId = ICD.Id,
                                        InvoiceCodeId = invoiceCode.Id,
                                        InvoiceCodeTireDetailsId = invoiceCD.Id,
                                        Position = invoiceCD.Position,
                                        SerialNumber = invoiceCD.SerialNumber,
                                        claimSubmissionItemId = CSItem.Id,
                                        claimSubmissionId = claimSub.Id,
                                        RemarkPosition = CSItem.Remark,
                                    };
                                    RequestDetailsOtherTire.Add(c);
                                }

                            }

                        }
                    }



                }
                response.requestDetailsOtherTires = RequestDetailsOtherTire;
                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;
        }



        internal object GetClaimDetailsForEdit(Guid claimId, Guid userId)
        {
            ClaimDetailsForEditResponseDto claimDetailsForEditResponseDto = new ClaimDetailsForEditResponseDto();
            try
            {
                claimDetailsForEditResponseDto.Status = "ok";
                #region "validation"
                //request validation
                if (!IsGuid(claimId.ToString()) || !IsGuid(userId.ToString()))
                {
                    claimDetailsForEditResponseDto.Status = "Request data invalid";
                    return claimDetailsForEditResponseDto;
                }
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                    .Where(a => a.Id == claimId).FirstOrDefault();

                if (claimSubmission == null) {
                    Claim claim = session.Query<Claim>()
                    .Where(a => a.Id == claimId).FirstOrDefault();
                    if (claim != null) {
                        claimSubmission = session.Query<ClaimSubmission>().Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                        claimId = claim.ClaimSubmissionId;
                        claimSubmission.StatusId = claim.StatusId;
                    }
                }

                if (claimSubmission == null)
                {
                    claimDetailsForEditResponseDto.Status = "Request data invalid";
                    return claimDetailsForEditResponseDto;
                }
                //claim status validation
                ClaimStatusCode claimStatus = session.Query<ClaimStatusCode>()
                    .Where(a => a.Id == claimSubmission.StatusId)
                    .FirstOrDefault();

                if (claimStatus != null)
                {
                    if (claimStatus.StatusCode != "SUB" && claimStatus.StatusCode != "REQ" && claimStatus.StatusCode != "INP")
                    {
                        claimDetailsForEditResponseDto.Status = "Selected claim cannot be edited.Its in " +
                                                                claimStatus.Description + " state.";
                        return claimDetailsForEditResponseDto;
                    }
                }
                else
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": No Claim status found for claimId-" +
                                 claimSubmission.Id);
                    claimDetailsForEditResponseDto.Status = "Error in claim status codes.";
                    return claimDetailsForEditResponseDto;
                }


                //user validation
                SystemUser systemUser = session.Query<SystemUser>()
                    .Where(a => a.LoginMapId == userId).FirstOrDefault();
                if (systemUser == null)
                {
                    claimDetailsForEditResponseDto.Status = "Requested user not found in the system";
                    return claimDetailsForEditResponseDto;
                }

                UserType userType = session.Query<UserType>()
                    .FirstOrDefault(a => a.Id == systemUser.UserTypeId);
                if (userType == null || userType.Code.ToUpper() != "DU")
                {
                    claimDetailsForEditResponseDto.Status = "Requested user not allowed to perform update claim.";
                    return claimDetailsForEditResponseDto;
                }

                DealerStaff dealerStaff = session.Query<DealerStaff>()
                    .Where(a => a.UserId == userId && a.DealerId == claimSubmission.ClaimSubmittedDealerId)
                    .FirstOrDefault();
                if (dealerStaff == null)
                {
                    claimDetailsForEditResponseDto.Status = "Requested claim not belongs to logged in user's dealer.";
                    return claimDetailsForEditResponseDto;
                }

                #endregion

                //getting claim details
                claimDetailsForEditResponseDto.ClaimDetails = GetClaimRequestDetailsByClaimId(claimId) as ClaimRequestDetailsResponseDto;

                claimDetailsForEditResponseDto.ClaimId = claimId;
                claimDetailsForEditResponseDto.PolicyDetails = new PolicyDetails()
                {
                    DealerId = claimSubmission.ClaimSubmittedDealerId,
                    CommodityTypeId = claimSubmission.CommodityTypeId,
                    CommodityCategoryId = claimSubmission.CommodityCategoryId,
                    CustomerId = claimSubmission.CustomerId,
                    MakeId = claimSubmission.MakeId,
                    ModelId = claimSubmission.ModelId,
                    PolicyId = claimSubmission.PolicyId,
                    CustomerComplaint = claimSubmission.CustomerComplaint,
                    DealerComment = claimSubmission.DealerComment
                };
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return claimDetailsForEditResponseDto;
        }


        internal object UpdateOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimData, Guid tpaId)
        {
            object Response = null;
            try
            {
                #region validation
                //validation
                if (claimData == null)
                {
                    return "Claim update request is invalid.";
                }

                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentConversionPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentConversionPeriodId.ToString()))
                {
                    return "Currenct currency period is not set.";
                }

                ISession session = EntitySessionManager.GetSession();
                Policy policy = session.Query<Policy>()
                    .Where(a => a.Id == claimData.policyId).FirstOrDefault();
                if (policy == null)
                {
                    return "Invalid policy selection.";
                }



                Dealer claimDealer = session.Query<Dealer>()
                    .Where(a => a.Id == claimData.dealerId).FirstOrDefault();
                if (claimDealer == null)
                {
                    return "Logged in dealer is invalid.";
                }

                decimal TirePriceUTDValuation = Convert.ToDecimal("0.00");
                string UnUsedTireDepth = "";
                decimal TirePricePercentage = Convert.ToDecimal("0.00");
                decimal LegalTreadDepth = Convert.ToDecimal("0.00");

                if (claimData.InvoiceCodeId == Guid.Empty) {
                    claimData.InvoiceCodeId = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == policy.Id).FirstOrDefault().InvoiceCodeId;
                }

                ClaimSubmission claim = session.Query<ClaimSubmission>().Where(a => a.Id == claimData.Id).FirstOrDefault();
                //claim.LastServiceDate = DateTime.Parse("1999-12-30");
                InvoiceCode invoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == claimData.InvoiceCodeId).FirstOrDefault();
                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == invoiceCode.Id).ToList();

                foreach (var ICodeDetails in invoiceCodeDetails)
                {
                    foreach (var tyre in claimData.tyreDetails)
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                        Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == tyre.ArticleNo).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {

                            List<Claim> claimda = session.Query<Claim>().Where(d => d.PolicyId == policy.Id).ToList();

                            foreach (var claimList in claimda)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    return "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }

                        }
                        else
                        {
                            return "Serial number not valid for front tyre.";
                        }
                    }
                }


                if (claim == null)
                {
                    return "Submitted claim not found.";
                }

                List<ClaimSubmissionAttachment> clamAttachmentList = claimData.attachmentIds.Select(docId => new ClaimSubmissionAttachment()
                {
                    ClaimSubmissionId = claim.Id,
                    Id = Guid.NewGuid(),
                    UserAttachmentId = docId
                }).ToList();

                #endregion

                decimal conversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId);
                //update claim related data
                //claim.CustomerComplaint = claimData.OtherTireDetails.customerComplaint;
                claim.DealerComment = claimData.OtherTireDetails.dealerComment;
                claim.LastUpdatedBy = claimData.requestedUserId;
                claim.LastUpdatedDate = DateTime.UtcNow;
                claim.TotalClaimAmount = currencyEm.ConvertToBaseCurrency(claimData.totalClaimAmount,
                    claimDealer.CurrencyId, currentConversionPeriodId);
                claim.ConversionRate = conversionRate;
                claim.ClaimMileage = claimData.policyDetails.failureMileage;
                claim.ClaimDate = claimData.policyDetails.failureDate;
                claim.MobileNo = claimData.policyDetails.mobileNo;

                List<Part> newParts = new List<Part>();
                List<PartPrice> newPartPrices = new List<PartPrice>();
                List<ClaimSubmissionItem> listClaimSubmissionItem = new List<ClaimSubmissionItem>();

                double NoofdatesinPolicy = (DateTime.UtcNow - policy.PolicySoldDate).TotalDays;



                List<ClaimItemTireDetails> ClaimItemTireDetailList = new List<ClaimItemTireDetails>();
                List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();



                ClaimItemType claimItemType = session.Query<ClaimItemType>().Where(a => a.ItemDescription == "Part").FirstOrDefault();

                foreach (var ICD in invoiceCodeDetails)
                {
                    List<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().Where(a => a.InvoiceCodeDetailId == ICD.Id).ToList();


                    AvailableTireSizesPattern availableTireSizepattern = session.Query<AvailableTireSizesPattern>()
                        .Where(a => a.Id == invoiceCodeTireDetails.FirstOrDefault().AvailableTireSizesPatternId).FirstOrDefault();


                    AvailableTireSizes availableTireSizes = session.Query<AvailableTireSizes>().Where(a => a.CrossSection == invoiceCodeTireDetails.FirstOrDefault().CrossSection &&
                                                                               a.Diameter == invoiceCodeTireDetails.FirstOrDefault().Diameter &&
                                                                               a.LoadSpeed == invoiceCodeTireDetails.FirstOrDefault().LoadSpeed &&
                                                                               a.Width == invoiceCodeTireDetails.FirstOrDefault().Width &&
                                                                               a.Id == availableTireSizepattern.AvailableTireSizesId).FirstOrDefault();


                    CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>().Where(a => a.InvoiceCodeId == ICD.InvoiceCodeId).FirstOrDefault();

                    Decimal DrivanKM = claimData.policyDetails.failureMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;
                    PartArea partArea = session.Query<PartArea>().FirstOrDefault();
                    if (invoiceCodeTireDetails != null)
                    {

                        foreach (var ICTDetais in invoiceCodeTireDetails)
                        {
                            foreach (var tyreObj in claimData.tyreDetails)
                            {
                                if (ICTDetais.Position == tyreObj.Position)
                                {

                                    UnUsedTireDepth = tyreObj.UnUsedTireDepth;
                                    LegalTreadDepth = (Convert.ToDecimal(tyreObj.UnUsedTireDepth));
                                    TirePricePercentage = (LegalTreadDepth / availableTireSizes.OriginalTireDepth) * 100;

                                    foreach (var UTDValuation in tireUTDValuation)
                                    {
                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                if (TirePricePercentage >= (UTDValuation.MinUTD * 100) && TirePricePercentage < (UTDValuation.MaxUTD * 100))
                                                {
                                                    TirePriceUTDValuation = UTDValuation.ClaimPercentage * currencyEm.ConvertFromBaseCurrency(availableTireSizes.TirePrice, claimDealer.CurrencyId, currentConversionPeriodId);
                                                }

                                            }
                                            else
                                            {
                                                TirePriceUTDValuation = Convert.ToDecimal("0");
                                            }

                                        }
                                        else
                                        {
                                            TirePriceUTDValuation = currencyEm.ConvertFromBaseCurrency(availableTireSizes.TirePrice, claimDealer.CurrencyId, currentConversionPeriodId);
                                        }
                                    }

                                    Part part = new Part()
                                    {
                                        Id = Guid.NewGuid(),
                                        AllocatedHours = 0,
                                        ApplicableForAllModels = true,
                                        CommodityId = policy.CommodityTypeId,
                                        EntryBy = claimData.requestedUserId,
                                        EntryDateTime = DateTime.UtcNow,
                                        IsActive = true,
                                        MakeId = claimData.policyDetails.makeId,
                                        PartCode = ICTDetais.SerialNumber,
                                        PartAreaId = partArea.Id,
                                        PartName = ICTDetais.Position + " -" + ICTDetais.ArticleNumber + " " + availableTireSizepattern.Pattern,
                                        PartNumber = ICTDetais.SerialNumber
                                    };

                                    newParts.Add(part);

                                    PartPrice partPrice = new PartPrice()
                                    {
                                        DealerId = claimData.dealerId,
                                        CountryId = claimDealer.CountryId,
                                        CurrencyId = claimDealer.CurrencyId,
                                        ConversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId),
                                        CurrencyPeriodId = currentConversionPeriodId,
                                        Id = Guid.NewGuid(),
                                        PartId = part.Id,
                                        Price = availableTireSizes.TirePrice,
                                        //Price = currencyEm.ConvertToBaseCurrency(TirePriceUTDValuation, claimDealer.CurrencyId, currentConversionPeriodId)
                                    };
                                    newPartPrices.Add(partPrice);

                                    ClaimSubmissionItem claimSubmissionItem = new ClaimSubmissionItem()
                                    {
                                        ClaimItemTypeId = claimItemType.Id,
                                        ClaimSubmissionId = claim.Id,
                                        DiscountAmount = Convert.ToDecimal("0.00"),
                                        DiscountRate = Convert.ToDecimal("0.00"),
                                        GoodWillAmount = Convert.ToDecimal("0.00"),
                                        GoodWillRate = Convert.ToDecimal("0.00"),
                                        Id = Guid.NewGuid(),
                                        IsDiscountPercentage = false,
                                        IsGoodWillPercentage = false,
                                        ItemCode = ICTDetais.SerialNumber,
                                        ItemName = ICTDetais.Position + " -" + ICTDetais.ArticleNumber + " " + availableTireSizepattern.Pattern,
                                        ParentId = Guid.Empty,
                                        PartId = part.Id,
                                        Quantity = 1,
                                        Remark = ICTDetais.Position,
                                        TotalGrossPrice = availableTireSizes.TirePrice,
                                        TotalPrice = availableTireSizes.TirePrice,
                                        UnitPrice = currencyEm.ConvertToBaseCurrency(TirePriceUTDValuation, claimDealer.CurrencyId, currentConversionPeriodId)
                                        //UnitPrice = availableTireSizes.TirePrice,
                                    };

                                    listClaimSubmissionItem.Add(claimSubmissionItem);

                                    //UnUsedTireDepth = claimSubmissionOtherTireData.OtherTireDetails.unusedTyreDepthBackRight;
                                    ClaimItemTireDetails claimItemTireDetails = new ClaimItemTireDetails()
                                    {
                                        Id = Guid.NewGuid(),
                                        InvoiceCodeTireId = ICTDetais.Id,
                                        ClaimItemId = claimSubmissionItem.Id,
                                        UnUsedTireDepth = Convert.ToDecimal(UnUsedTireDepth)
                                    };
                                    ClaimItemTireDetailList.Add(claimItemTireDetails);
                                }
                            }
                        }
                    }

                }

                using (ITransaction transaction = session.BeginTransaction())
                {


                    //delete before insert items
                    List<ClaimSubmissionItem> existingClaimItems = session.Query<ClaimSubmissionItem>()
                        .Where(a => a.ClaimSubmissionId == claim.Id).ToList();
                    List<ClaimSubmissionAttachment> existingAttachments = session.Query<ClaimSubmissionAttachment>()
                      .Where(a => a.ClaimSubmissionId == claim.Id).ToList();


                    foreach (ClaimSubmissionItem existingClaimItem in existingClaimItems)
                    {

                        ClaimItemTireDetails claimItemTireDetails = session.Query<ClaimItemTireDetails>().Where(a => a.ClaimItemId == existingClaimItem.Id).FirstOrDefault();
                        session.Delete(claimItemTireDetails);

                        session.Delete(existingClaimItem);

                        PartPrice partPrice = session.Query<PartPrice>().Where(a => a.PartId == existingClaimItem.PartId).FirstOrDefault();
                        session.Delete(partPrice);


                        Part part = session.Query<Part>().Where(a => a.Id == existingClaimItem.PartId).FirstOrDefault();
                        session.Delete(part);


                    }


                    foreach (ClaimSubmissionAttachment existingAttachment in existingAttachments)
                    {
                        session.Delete(existingAttachment);
                    }

                    //insert new data



                    foreach (Part part in newParts)
                    {
                        session.Evict(part);
                        session.Save(part, part.Id);
                    }

                    foreach (PartPrice partPrice in newPartPrices)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);

                    }

                    session.Evict(claim);
                    session.Update(claim, claim.Id);

                    foreach (ClaimSubmissionItem claimItem in listClaimSubmissionItem)
                    {
                        session.Evict(claimItem);
                        session.Save(claimItem, claimItem.Id);
                    }
                    //save doc ids

                    foreach (ClaimSubmissionAttachment claimAttachment in clamAttachmentList)
                    {
                        claimAttachment.DateOfAttachment = DateTime.Today;
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment, claimAttachment.Id);
                    }


                    foreach (ClaimItemTireDetails claimItemTire in ClaimItemTireDetailList)
                    {
                        session.Evict(claimItemTire);
                        session.Save(claimItemTire, claimItemTire.Id);
                    }

                    transaction.Commit();
                }

                Response = "ok";



            }
            catch (Exception ex)
            {
                Response = "Error occured while update claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }




        internal object UpdateClaim(ClaimUpdateRequestDto claimData, Guid tpaId)
        {
            object Response = null;
            try
            {
                #region validation
                //validation
                if (claimData == null || !IsGuid(claimData.id.ToString()))
                {
                    return "Claim update request is invalid.";
                }
                string result = ClaimDataValidationOnSave(claimData);
                if (result != "ok")
                {
                    return result;
                }

                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentConversionPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentConversionPeriodId.ToString()))
                {
                    return "Currenct currency period is not set.";
                }

                ISession session = EntitySessionManager.GetSession();
                Policy policy = session.Query<Policy>()
                    .Where(a => a.Id == claimData.policyId).FirstOrDefault();
                if (policy == null)
                {
                    return "Invalid policy selection.";
                }

                Dealer claimDealer = session.Query<Dealer>()
                    .Where(a => a.Id == claimData.dealerId).FirstOrDefault();
                if (claimDealer == null)
                {
                    return "Logged in dealer is invalid.";
                }

                ClaimSubmission claim = session.Query<ClaimSubmission>()
                    .Where(a => a.Id == claimData.id).FirstOrDefault();

                if (claim == null)
                {
                    return "Submitted claim not found.";
                }



                CommodityType CommodityType = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                // validate mileage less than  policy sale mileage
                if (CommodityType.CommodityCode == "A" && CommodityType.CommodityTypeDescription != "Tyre") {

                    decimal PremiumMileage = session.Query<ContractInsuaranceLimitation>()
                        .Join(session.Query<InsuaranceLimitation>(), b => b.InsuaranceLimitationId, c => c.Id, (b, c) => new { b, c })
                        .Where(x => x.b.ContractId == policy.ContractId).Select(s => s.c.Km).FirstOrDefault();

                    if (PremiumMileage > 0 && PremiumMileage + Decimal.Parse(policy.HrsUsedAtPolicySale) < claimData.policyDetails.failureMileage)
                    {
                        return "Failure mileage above risk termination .";
                    }

                    if (Decimal.Parse(policy.HrsUsedAtPolicySale) > claimData.policyDetails.failureMileage) {
                        return "mileage cannot lower than policy added mileage.";
                    }


                }
                #endregion

                decimal conversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId);
                //update claim related data
                claim.CustomerComplaint = claimData.complaint.customer;
                claim.DealerComment = claimData.complaint.dealer;
                claim.LastUpdatedBy = claimData.requestedUserId;
                claim.LastUpdatedDate = DateTime.UtcNow;
                claim.CustomerName = claimData.policyDetails.customerName;
                claim.PlateNo = claimData.policyDetails.plateNumber;
                claim.TotalClaimAmount = currencyEm.ConvertToBaseCurrency(claimData.totalClaimAmount,
                claimDealer.CurrencyId, currentConversionPeriodId);
                claim.ConversionRate = conversionRate;
                claim.FailureMileage = claimData.policyDetails.failureMileage;
                claim.FailureDate = claimData.policyDetails.failureDate;
                claim.LastServiceDate = claimData.policyDetails.lastServiceDate;
                claim.LastServiceMileage = claimData.policyDetails.lastServiceMileage;
                bool particialApprovedClaim = false;
                Claim cm = new Claim();
                if (claim.StatusId != new CommonEntityManager().GetClaimStatusIdByCode("REQ") &&
                    claim.StatusId != new CommonEntityManager().GetClaimStatusIdByCode("SUB"))
                {
                    cm = session.Query<Claim>().Where(a => a.ClaimSubmissionId == claim.Id).FirstOrDefault();
                    if (cm != null)
                    {
                        claim.StatusId = cm.StatusId;
                        particialApprovedClaim = true;
                    }
                }

                if (claim.StatusId == new CommonEntityManager().GetClaimStatusIdByCode("REQ"))
                {
                    claim.StatusId = new CommonEntityManager().GetClaimStatusIdByCode("UPD");
                    //request user details
                    var commentDetails = session.Query<ClaimComment>()
                        .OrderByDescending(x => x.SeenDateTime)
                        .FirstOrDefault(a => a.ClaimId == claim.Id && a.ByTPA == true);
                    CommonEntityManager commonEntityManager = new CommonEntityManager();
                    if (commentDetails != null)
                    {
                        var InternalUser = session.Query<InternalUser>()
                        .FirstOrDefault(a => a.Id == commentDetails.SentFrom.ToString());

                        if (InternalUser != null)
                        {
                            PushNotificationsRequestDto pushNotifications = new PushNotificationsRequestDto()
                            {
                                generatedTime = DateTime.UtcNow,
                                link = ConfigurationData.BaseUrl + "app/claim/listing/" + claim.Id,
                                message = "Dealer has updated the claim request (policy Number - "
                                    + claim.PolicyNumber + ") " + "as you requested.",
                                messageFrom = commonEntityManager.GetUserNameById(commentDetails.SentFrom),
                                profilePic = commonEntityManager.GetProfilePictureByUserId(Guid.Parse(InternalUser.Id)),
                                userDetails = new List<UserDetail>()
                                {
                                    new UserDetail()
                                    {
                                        tpaId = tpaId,
                                        userId =Guid.Parse(InternalUser.Id)
                                    }
                                }
                            };

                            Task.Run(async () => await NotificationEntityManager.PushNotificationSender(pushNotifications));
                        }
                    }
                }
                else if (claim.StatusId == new CommonEntityManager().GetClaimStatusIdByCode("SUB"))
                {
                }
                else
                {
                    return "Selected claim is request not eligible to edit.";
                }

                //claim detail
                List<ClaimSubmissionItem> claimItemList = DBDTOTransformer.Instance.ConvertClaimDataToClaimItemList(
                    claimData, claim.Id, claimDealer, currentConversionPeriodId);
                List<PartPrice> partPricesList = new List<PartPrice>();

                List<ClaimSubmissionAttachment> clamAttachmentList = claimData.attachmentIds.Select(docId => new ClaimSubmissionAttachment()
                {
                    ClaimSubmissionId = claim.Id,
                    Id = Guid.NewGuid(),
                    UserAttachmentId = docId,
                    DateOfAttachment = DateTime.UtcNow
                }).ToList();

                bool isClaimSubmission = false;

                if (claim == null)
                {

                }
                else
                {
                    isClaimSubmission = true;
                }

                ClaimComment claimCom = session.Query<ClaimComment>().Where(a => a.PolicyId == claimData.policyId && a.ClaimId == claimData.id).FirstOrDefault();

                ClaimComment claimComment = null;
                if (claimCom != null)
                {
                    if (isClaimSubmission)
                    {
                        claimComment = new ClaimComment()
                        {
                            ByTPA = true,
                            ClaimId = claim.Id,
                            Comment = claimData.commentDealer,
                            EntryDateTime = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                            PolicyId = claimData.policyId,
                            Seen = false,
                            SeenDateTime = SqlDateTime.MinValue.Value,
                            SentFrom = claimData.requestedUserId,
                            SentTo = claimCom.SentFrom
                        };

                    }
                    else
                    {
                        claimComment = new ClaimComment()
                        {
                            ByTPA = true,
                            ClaimId = claim.Id,
                            Comment = claimData.commentDealer,
                            EntryDateTime = DateTime.UtcNow,
                            Id = Guid.NewGuid(),
                            PolicyId = claimData.policyId,
                            Seen = false,
                            SeenDateTime = SqlDateTime.MinValue.Value,
                            SentFrom = claimData.requestedUserId,
                            SentTo = claimCom.SentFrom
                        };
                    }
                }

                using (ITransaction transaction = session.BeginTransaction())
                {

                    session.Evict(claim);
                    session.Update(claim, claim.Id);
                    if (particialApprovedClaim && cm!=null) {
                        cm.StatusId = claim.StatusId;
                        cm.FailureDate = claim.FailureDate;
                        cm.ClaimMileageKm = claim.FailureMileage;
                        session.Evict(cm);
                        session.Update(cm);
                    }

                    //save claim comment
                    if (claimComment != null)
                    session.Save(claimComment, claimComment.Id);


                    //delete before insert items
                    List<ClaimSubmissionItem> existingClaimItems = session.Query<ClaimSubmissionItem>()
                        .Where(a => a.ClaimSubmissionId == claim.Id).ToList();
                    List<ClaimSubmissionAttachment> existingAttachments = session.Query<ClaimSubmissionAttachment>()
                      .Where(a => a.ClaimSubmissionId == claim.Id).ToList();

                    foreach (ClaimSubmissionItem existingClaimItem in existingClaimItems)
                    {
                        session.Evict(existingClaimItem);
                        session.Delete(existingClaimItem);
                    }

                    foreach (ClaimSubmissionAttachment existingAttachment in existingAttachments)
                    {
                        session.Evict(existingAttachment);
                        session.Delete(existingAttachment);
                    }

                    //insert new data
                    foreach (ClaimSubmissionItem claimItem in claimItemList)
                    {
                        session.Evict(claimItem);
                        session.Save(claimItem);

                        //update delaer price if not found
                        if (IsGuid(claimItem.PartId.ToString()))
                        {
                            PartPrice partPrice = new PartPrice();
                            partPrice = session.Query<PartPrice>()
                                .Where(a => a.PartId == claimItem.PartId
                                    && a.DealerId == claimDealer.Id && a.CountryId == claimDealer.CountryId).FirstOrDefault();
                            if (partPrice == null)
                            {
                                partPrice = new PartPrice()
                                {
                                    Id = Guid.NewGuid(),
                                    ConversionRate = claim.ConversionRate,
                                    CountryId = claimDealer.CountryId,
                                    CurrencyId = claim.ClaimCurrencyId,
                                    CurrencyPeriodId = claim.CurrencyPeriodId,
                                    DealerId = claimDealer.Id,
                                    PartId = Guid.Parse(claimItem.PartId.ToString()),
                                    Price = claimItem.UnitPrice
                                };
                                partPricesList.Add(partPrice);
                            }
                        }
                    }
                    //save doc ids
                    foreach (ClaimSubmissionAttachment claimAttachment in clamAttachmentList)
                    {
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment, claimAttachment.Id);
                    }

                    //update Part Prices
                    foreach (PartPrice partPrice in partPricesList)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);
                    }

                    transaction.Commit();
                }
                Response = "ok";


            }
            catch (Exception ex)
            {
                Response = "Error occured while update claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;
        }

        internal List<PartRejectionTypesResponseDto> GetAllPartRejectioDescription()
        {
            List<PartRejectionTypesResponseDto> entities = null;
            ISession session = EntitySessionManager.GetSession();
            try
            {
                entities = session.Query<PartRejectionType>().Select(partrejectionType => new PartRejectionTypesResponseDto
                {
                    Id = partrejectionType.Id,
                    Code = partrejectionType.Code,
                    Description = partrejectionType.Description,
                    UserId = partrejectionType.UserId,
                }).ToList();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return entities;
        }

        public PartRejectionTypesResponseDto GetPartRejectionTypeById(Guid PartRejectionTypeId)
        {
            ISession session = EntitySessionManager.GetSession();
            PartRejectionTypesResponseDto pDto = new PartRejectionTypesResponseDto();

            var query =
                from PartRejectionType in session.Query<PartRejectionType>()
                where PartRejectionType.Id == PartRejectionTypeId
                select new { PartRejectionType = PartRejectionType };

            var result = query.ToList();

            if (result != null && result.Count > 0)
            {
                pDto.Id = result.First().PartRejectionType.Id;
                pDto.Code = result.First().PartRejectionType.Code;
                pDto.Description = result.First().PartRejectionType.Description;
                pDto.UserId = result.First().PartRejectionType.UserId;
                pDto.IsPartRejectionTypesExists = true;
                return pDto;
            }
            else
            {
                return null;
            }
        }

        internal bool AddPartRejection(PartRejectionTypeRequestDto PartRejectionType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PartRejectionType pr = new Entities.PartRejectionType();

                pr.Id = new Guid();
                pr.Code = PartRejectionType.Code;
                pr.Description = PartRejectionType.Description;
                //pr.UserId = PartRejectionType.UserId;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
                    transaction.Commit();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal bool UpdatePartRejectioDescription(PartRejectionTypeRequestDto PartRejectionType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PartRejectionType pr = new Entities.PartRejectionType();

                pr.Id = PartRejectionType.Id;
                pr.Code = PartRejectionType.Code;
                pr.Description = PartRejectionType.Description;
                pr.UserId = PartRejectionType.UserId;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(pr);

                    transaction.Commit();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #region < ClaimChequePayment >

        internal bool AddClaimChequePayment(ClaimChequePaymentRequestDto claimChequeData)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                bool isSaved = false;
                Guid claimChequePaymentId;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    try
                    {
                        ClaimChequePayment ccpd = new Entities.ClaimChequePayment();

                        ccpd.ChequeNo = claimChequeData.ChequeNo;
                        ccpd.ChequeDate = claimChequeData.ChequeDate;
                        ccpd.IssuedDate = claimChequeData.IssuedDate;
                        ccpd.ChequeAmount = claimChequeData.ChequeAmount;
                        ccpd.EntryDate = DateTime.Today.ToUniversalTime();
                        ccpd.EntryBy = claimChequeData.EntryBy;
                        ccpd.CountryId = claimChequeData.CountryId;
                        ccpd.DealerId = claimChequeData.DealerId;
                        ccpd.Comment = claimChequeData.Comment;

                        session.SaveOrUpdate(ccpd);
                        claimChequePaymentId = ccpd.Id;

                        foreach (ClaimChequePaymentDetailRequestDto data in claimChequeData.ClaimChequePaymentDetails)
                        {
                            ClaimChequePaymentDetail cpd = new Entities.ClaimChequePaymentDetail();
                            cpd.ClaimBatchGroupId = data.ClaimBatchGroupId;
                            cpd.ClaimChequePaymentId = claimChequePaymentId;
                            cpd.EntryDate = DateTime.Today.ToUniversalTime();
                            cpd.EntryBy = claimChequeData.EntryBy;
                            session.SaveOrUpdate(cpd);

                            ClaimBatchGroup cbg = new ClaimBatchGroup();

                            cbg = session.Query<ClaimBatchGroup>()
                                .FirstOrDefault(a => a.Id == data.ClaimBatchGroupId);

                            if (cbg != null)
                            {
                                cbg.IsAllocatedForCheque = true;
                                session.SaveOrUpdate(cbg);
                            }

                        }
                        transaction.Commit();
                        isSaved = true;
                    }
                    catch
                    {
                        transaction.Rollback();
                    }
                }

                return isSaved;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal object GetClaimListForClaimProcess_old(ClaimRetirevalForProcessRequestDto claimRequestData)
        {
            object Response = null;
            try
            {
                #region validate

                if (claimRequestData == null || !IsGuid((claimRequestData.loggedInUserId.ToString())))
                {
                    Response = "Request data incomplete";
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                SystemUser sysUser = session
                    .Query<SystemUser>().FirstOrDefault(a => a.LoginMapId == claimRequestData.loggedInUserId);
                if (sysUser == null)
                {
                    Response = "Request user incorrect";
                    return Response;
                }

                UserType userType = session.Query<UserType>().FirstOrDefault(a => a.Id == sysUser.UserTypeId);

                if (userType == null)
                {
                    Response = "Request user type incorrect";
                    return Response;
                }
                if (userType.Code.ToLower().Trim() != "iu")
                {
                    Response = "Access only for internal users";
                    return Response;
                }
                #endregion

                Guid approvedStatus = commonEm.GetClaimStatusIdByCode("APP");
                Guid rejectedStatus = commonEm.GetClaimStatusIdByCode("REJ");
                Guid inProgressStatus = commonEm.GetClaimStatusIdByCode("REV");

                //processing claims
                IEnumerable<Claim> claims = session.Query<Claim>()
                    .Where(a => a.StatusId != approvedStatus && a.StatusId != rejectedStatus && a.ExamineBy == claimRequestData.loggedInUserId);
                //submitted claims
                IEnumerable<ClaimSubmission> submittedClaims = session.Query<ClaimSubmission>()
                    .Where(a => a.StatusId != approvedStatus && a.StatusId != rejectedStatus &&a.StatusId != inProgressStatus);

                var claimData = claims.Select(a => new
                {
                    a.Id,
                    a.PolicyId,
                    CommodityType = commonEm.GetCommodityTypeNameById(a.CommodityTypeId),
                    a.PolicyNumber,
                    a.ClaimNumber,
                    ClaimDealer = commonEm.GetDealerNameById(a.ClaimSubmittedDealerId),
                    Make = commonEm.GetMakeNameById(a.MakeId),
                    Model = commonEm.GetModelNameById(a.ModelId),
                    ClaimAmount =
                        currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount, a.ClaimCurrencyId, a.CurrencyPeriodId) +
                        " " +
                        commonEm.GetCurrencyTypeByIdCode(a.ClaimCurrencyId),
                    Status = commonEm.GetClaimStatusCodeById(a.StatusId),
                    Date = a.EntryDate.ToString("dd-MMM-yyyy")
                }).Union(submittedClaims.Select(a => new
                {
                    a.Id,
                    a.PolicyId,
                    CommodityType = commonEm.GetCommodityTypeNameById(a.CommodityTypeId),
                    a.PolicyNumber,
                    a.ClaimNumber,
                    ClaimDealer = commonEm.GetDealerNameById(a.ClaimSubmittedDealerId),
                    Make = commonEm.GetMakeNameById(a.MakeId),
                    Model = commonEm.GetModelNameById(a.ModelId),
                    ClaimAmount =
                        currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount, a.ClaimCurrencyId, a.CurrencyPeriodId) +
                        " " +
                        commonEm.GetCurrencyTypeByIdCode(a.ClaimCurrencyId),
                    Status = commonEm.GetClaimStatusCodeById(a.StatusId),
                    Date = a.EntryDate.ToString("dd-MMM-yyyy")
                }).OrderByDescending(a => a.PolicyNumber));
                var claimDataEvaluated = claimData.ToList();
                var claimDataFiltered = claimDataEvaluated.Skip((claimRequestData.page - 1) * claimRequestData.pageSize)
                    .Take(claimRequestData.pageSize).ToArray();
                var gridResponse = new CommonGridResponseDto()
                {
                    totalRecords = claimDataEvaluated.Count(),
                    data = claimDataFiltered
                };
                Response = new JavaScriptSerializer().Serialize(gridResponse);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;

        }

        internal object GetClaimListForClaimProcess(ClaimRetirevalForProcessRequestDto claimRequestData)
        {
            object Response = null;
            try
            {
                #region validate

                if (claimRequestData == null || !IsGuid((claimRequestData.loggedInUserId.ToString())))
                {
                    Response = "Request data incomplete";
                    return Response;
                }

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                SystemUser sysUser = session
                    .Query<SystemUser>().FirstOrDefault(a => a.LoginMapId == claimRequestData.loggedInUserId);
                if (sysUser == null)
                {
                    Response = "Request user incorrect";
                    return Response;
                }

                UserType userType = session.Query<UserType>().FirstOrDefault(a => a.Id == sysUser.UserTypeId);

                if (userType == null)
                {
                    Response = "Request user type incorrect";
                    return Response;
                }
                if (userType.Code.ToLower().Trim() != "iu")
                {
                    Response = "Access only for internal users";
                    return Response;
                }
                #endregion

                List<ClaimProcessDataGridResponseDto> policyGridDetailsFilterd = session.CreateSQLQuery("exec GetClaimListForClaimProcess :skip,:take,:loggedInUserId ")
                       .SetInt32("skip", (claimRequestData.page - 1) * claimRequestData.pageSize)
                       .SetInt32("take", claimRequestData.pageSize)
                       .SetGuid("loggedInUserId", claimRequestData.loggedInUserId)
                       .SetResultTransformer(Transformers.AliasToBean<ClaimProcessDataGridResponseDto>()).List<ClaimProcessDataGridResponseDto>().ToList();

                int res = session.CreateSQLQuery("exec GetClaimListForClaimProcessRowCount :loggedInUserId")
                    .SetGuid("loggedInUserId", claimRequestData.loggedInUserId)
                    .UniqueResult<int>();
                var totalRecords = long.Parse(res.ToString());

                var listedData = policyGridDetailsFilterd.Select(a => new
                {
                    a.Id,
                    a.PolicyId,
                    a.CommodityType,
                    a.PolicyNumber,
                    a.ClaimNumber,
                    a.ClaimDealer,
                    a.Make,
                    a.Model,
                    ClaimAmount =
                        currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount, a.ClaimCurrencyId, a.CurrencyPeriodId) +
                        " " +
                        a.Currency,
                    a.Status,
                    a.Date
                });

                    var gridResponse = new CommonGridResponseDto()
                {
                    totalRecords = totalRecords,
                    data = listedData
                    };
                Response = new JavaScriptSerializer().Serialize(gridResponse);
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return Response;

        }

        public List<ClaimChequePaymentDetailResponseDto> GetPendingClaimGroups(Guid countryId, Guid dealerId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<ClaimChequePaymentDetailResponseDto> list = null;

                ClaimChequePaymentDetailResponseDto ccDto = new ClaimChequePaymentDetailResponseDto();

                var query = from claimbatch in session.Query<ClaimBatch>()
                            where claimbatch.CountryId == countryId && claimbatch.DealerId == dealerId
                            join claimBatchGroup in session.Query<ClaimBatchGroup>() on claimbatch.Id equals claimBatchGroup.ClaimBatchId
                            where claimBatchGroup.IsAllocatedForCheque == false
                            join claimGroupClaim in session.Query<ClaimGroupClaim>() on claimBatchGroup.Id equals claimGroupClaim.ClaimGroupId
                            join claim in session.Query<Claim>() on claimGroupClaim.ClaimId equals claim.Id
                            group new { claim.TotalClaimAmount }
                                by new { claimBatchGroup.Id, claimBatchGroup.GroupName, claimbatch.BatchNumber, claimbatch.EntryDate } into g
                            select new { BatchGroupId = g.Key.Id, GroupName = g.Key.GroupName, BatchNumber = g.Key.BatchNumber, BatchDate = g.Key.EntryDate, Amount = g.Sum(x => x.TotalClaimAmount) };

                var result = query.ToList();
                if (result != null && result.Count > 0)
                {
                    list = new List<ClaimChequePaymentDetailResponseDto>();
                    foreach (var data in result)
                    {
                        ccDto = new ClaimChequePaymentDetailResponseDto();

                        ccDto.ClaimBatchGroupId = data.BatchGroupId;
                        ccDto.BatchNumber = data.BatchNumber;
                        ccDto.BatchDate = data.BatchDate;
                        ccDto.Amount = data.Amount;
                        ccDto.GroupName = data.GroupName;

                        list.Add(ccDto);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
                return null;
            }
        }

        internal bool ChequeNoIsExists(string chequeNo)
        {
            try
            {
                bool retVal = false;
                ISession session = EntitySessionManager.GetSession();
                if (session.Query<ClaimChequePayment>().Count(a => a.ChequeNo == chequeNo) > 0) { retVal = true; }
                return retVal;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
                return true;
            }
        }

        internal static object GetAllChequesForSearchGrid(ChequeSearchGridRequestDto chequeSearchGridRequestDto)
        {
            if (chequeSearchGridRequestDto != null && chequeSearchGridRequestDto.paginationOptionsSearchGrid != null)
            {
                Expression<Func<ClaimChequePayment, bool>> filterCheque = PredicateBuilder.True<ClaimChequePayment>();
                //filterCheque = filterCheque.And(a => a.IsActive == true);
                if (!String.IsNullOrEmpty(chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeNo))
                {
                    filterCheque = filterCheque.And(a => a.ChequeNo.ToLower().Contains(chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeNo.ToLower()));
                }
                if ((chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeDate != null && chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeDate.ToString() != "1/1/0001 12:00:00 AM"))
                {
                    filterCheque = filterCheque.And(a => a.ChequeDate == chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeDate);
                }
                if (chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeAmount != 0)
                {
                    filterCheque = filterCheque.And(a => a.ChequeAmount == chequeSearchGridRequestDto.searchGridSearchCriterias.ChequeAmount);
                }
                ISession session = EntitySessionManager.GetSession();
                var filteredCheques = session.Query<ClaimChequePayment>().Where(filterCheque);

                long TotalRecords = filteredCheques.Count();
                var customerGridDetailsFilterd = filteredCheques.Skip((chequeSearchGridRequestDto.paginationOptionsSearchGrid.pageNumber - 1) * chequeSearchGridRequestDto.paginationOptionsSearchGrid.pageSize)
                .Take(chequeSearchGridRequestDto.paginationOptionsSearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    ChequeNo = a.ChequeNo,
                    ChequeDate = a.ChequeDate.ToString("dd-MMM-yyyy"),
                    ChequeAmount = a.ChequeAmount,
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

        public byte[] GetChequeAttachmentByChequePaymentIdId(Guid chequePaymentId, string dbName)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                ClaimChequePayment cheueData = session.Query<ClaimChequePayment>().FirstOrDefault(a => a.Id == chequePaymentId);

                if (cheueData != null)
                {
                    ClaimChequeReport ccr = new ClaimChequeReport();
                    return ccr.Generate(dbName, "AEW", chequePaymentId);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return null;
        }

        #endregion


        internal object AddClamItem(ClamItemRequestDto clamItemRequest)
        {
            AddClaimItemResponseDto claimAddResponseDto = new AddClaimItemResponseDto();
            try
            {
                claimAddResponseDto.Status = "ok";
                ISession session = EntitySessionManager.GetSession();
                var claimType = String.Empty;
                #region "validation"
                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == clamItemRequest.claimId);
                if (claim == null)
                {
                    var dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == clamItemRequest.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(clamItemRequest.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    claimAddResponseDto.Status = "Invalid claim or policy selection";
                    return claimAddResponseDto;
                }

                #endregion

                if (claimType == "New")
                {
                    claimAddResponseDto = SaveTpaEnteredClaim(clamItemRequest);
                    claimAddResponseDto.IsReload = true;
                }
                else if (claimType == "DealerSubmitted")
                {
                    claimAddResponseDto = SaveDealerSubmittedClaimWithItem(clamItemRequest,null);
                    claimAddResponseDto.IsReload = true;
                }
                else
                {
                    claimAddResponseDto = UpdateNewClaimWithItem(clamItemRequest);
                    claimAddResponseDto.IsReload = true;
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return claimAddResponseDto;
        }

        private AddClaimItemResponseDto UpdateNewClaimWithItem(ClamItemRequestDto clamItemRequest)
        {
            var respones = new AddClaimItemResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == clamItemRequest.claimId);
                if (claim != null)
                {
                    if (!IsGuid(clamItemRequest.faultId.ToString()))
                    {
                        clamItemRequest.faultId = null;
                    }

                    ClaimItem claimItem =
                        session.Query<ClaimItem>().FirstOrDefault(a => a.Id == clamItemRequest.serverId);
                    if (claimItem != null)
                    {
                        claimItem.AuthorizedAmount = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.authorizedAmt, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.DiscountAmount = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.discountAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.IsApproved = clamItemRequest.status.ToLower() == "a" ? true : false;
                        claimItem.TotalGrossPrice = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.totalGrossPrice, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.TotalPrice = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.totalPrice, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.DiscountRate = clamItemRequest.isDiscountPercentage ? clamItemRequest.discountRate : currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.discountRate, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.GoodWillAmount = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.goodWillAmount, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.GoodWillRate = clamItemRequest.isGoodWillPercentage ? clamItemRequest.goodWillRate : currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.goodWillRate, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.FaultId = clamItemRequest.faultId;
                        claimItem.Quantity = clamItemRequest.qty;
                        claimItem.UnitPrice = currencyEntityManager.ConvertToBaseCurrency(
                            clamItemRequest.unitPrice, claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                        claimItem.TpaComment = clamItemRequest.comment;

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Update(claimItem, claimItem.Id);
                            List<ClaimItem> claimItems = session.Query<ClaimItem>()
                                .Where(a => a.ClaimId == clamItemRequest.claimId).ToList();

                            claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                            claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                            claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);

                            session.Update(claim, claim.Id);

                            transaction.Commit();
                        }
                        respones.Status = "ok";
                        respones.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                        respones.ClaimNo = claim.ClaimNumber;
                        respones.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                        respones.ClaimId = claim.Id;
                        respones.IsReload = false;
                    }
                    else
                    {
                        respones.Status = "Selected claim item is invalid";
                        return respones;
                    }
                }
                else
                {
                    respones.Status = "Invalid claim selection";
                    return respones;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                               ex.InnerException);
            }
            return respones;
        }

        private AddClaimItemResponseDto SaveDealerSubmittedClaimWithItem(ClamItemRequestDto clamItemRequest, PartWithClaimRequestDto partDetails)
        {
            var response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Claim newClaim = DBDTOTransformer.Instance.DealerClaimToTpaClaim(clamItemRequest);
                //old claim details
                ClaimSubmission oldClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == clamItemRequest.claimId);

                List<ClaimSubmissionItem> oldClaimItems = session.Query<ClaimSubmissionItem>().Where(a => a.ClaimSubmissionId == clamItemRequest.claimId).ToList();
                List<ClaimSubmissionAttachment> oldClaimAttachments = session.Query<ClaimSubmissionAttachment>().Where(a => a.ClaimSubmissionId == clamItemRequest.claimId).ToList();


                List<ClaimItem> claimItems = DBDTOTransformer.Instance.DealerClaimItemsToTpaClaimItems(clamItemRequest,
                    newClaim);

                if (claimItems.Count == 0 && partDetails!=null) {
                    ClaimItem claimItem = DBDTOTransformer.Instance.ClaimItemRequestToNewClaimItem(clamItemRequest, newClaim);
                    claimItems.Add(claimItem);
                }



                //update claim info
                newClaim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                newClaim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                newClaim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);
                newClaim.ApprovedDate = DateTime.UtcNow;

                List<ClaimAttachment> claimAttachments =
                    DBDTOTransformer.Instance.DealerClaimAttachementsToClaimAttachemnts(clamItemRequest, newClaim);

                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var newClaimNumber =
                        session.Query<Claim>().Count(a => a.PolicyCountryId == newClaim.PolicyCountryId) + 1;
                    var countryCode = new CommonEntityManager().GetCountryCodeById(newClaim.PolicyCountryId);
                    newClaim.ClaimNumber = countryCode.ToUpper() + "/" +
                                           Convert.ToString(newClaimNumber)
                                               .PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0');

                    session.Save(newClaim, newClaim.Id);

                    foreach (ClaimItem claimItem in claimItems)
                    {
                        session.Evict(claimItem);
                        session.Save(claimItem, claimItem.Id);
                    }

                    foreach (ClaimAttachment claimAttachment in claimAttachments)
                    {
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment, claimAttachment.Id);
                    }

                    //update old claimdetails
                    oldClaim.StatusId = newClaim.StatusId;
                    oldClaim.ClaimNumber = newClaim.ClaimNumber;
                    session.Evict(oldClaim);
                    session.Update(oldClaim, oldClaim.Id);

                    transaction.Commit();
                }
                //setting up response
                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(newClaim.AuthorizedAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimNo = newClaim.ClaimNumber;
                response.RequestedAmount = Math.Round(newClaim.TotalClaimAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimId = newClaim.Id;

            }
            catch (CurrencyPeriodNotSetException ex)
            {
                response.Status = "No Currency Period is set for today.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured while saving new claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;
        }

        private AddClaimItemResponseDto SaveTpaEnteredClaim(ClamItemRequestDto clamItemRequest)
        {
            var response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                PolicyBundle policyBundle = session.Query<PolicyBundle>().FirstOrDefault(a => a.Id == clamItemRequest.policyId);
                Policy policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == policyBundle.Id);

                if (policy == null)
                {
                    response.Status = "Invalid policy selection";
                    return response;
                }

                Claim newClaim = DBDTOTransformer.Instance.ClaimItemRequestToNewClaim(clamItemRequest, policy);
                ClaimItem newClaimItem = DBDTOTransformer.Instance.ClaimItemRequestToNewClaimItem(clamItemRequest, newClaim);

                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    var newClaimNumber =
                        session.Query<Claim>().Count(a => a.PolicyCountryId == newClaim.PolicyCountryId) + 1;
                    var countryCode = new CommonEntityManager().GetCountryCodeById(newClaim.PolicyCountryId);
                    newClaim.ClaimNumber = countryCode.ToUpper() + "/" + Convert.ToString(newClaimNumber).PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0');
                   // newClaim.AuthorizedAmount = newClaimItem.AuthorizedAmount;
                    newClaim.ConversionRate = newClaimItem.ConversionRate;
                    newClaim.CurrencyPeriodId = newClaimItem.CurrencyPeriodId;
                    newClaim.TotalClaimAmount = newClaimItem.TotalPrice;
                    newClaim.TotalGrossClaimAmount = newClaimItem.TotalGrossPrice;
                    newClaim.ApprovedDate = SqlDateTime.MinValue.Value;
                    newClaim.ClaimDate = DateTime.UtcNow;

                    session.Save(newClaim, newClaim.Id);
                    session.Save(newClaimItem, newClaimItem.Id);

                    transaction.Commit();
                }
                //setting up response
                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(newClaim.AuthorizedAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimNo = newClaim.ClaimNumber;
                response.RequestedAmount = Math.Round(newClaim.TotalClaimAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimId = newClaim.Id;

            }
            catch (Exception ex)
            {
                response.Status = "Error Occured while saving new claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;
        }

        internal object ValidateClaimProcessRequest(ValidateClaimProcessRequestDto validateClaimProcessRequest)
        {
            ValidateClaimProcessResponseDto response = new ValidateClaimProcessResponseDto();
            response.status = "ok";
            try
            {
                var claimType = String.Empty;
                CommonEntityManager commonEm = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission dealerClaim = new ClaimSubmission();
                int claimProcessingSafeTime = ConfigurationData.ClaimProcessingSafeTime;
                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == validateClaimProcessRequest.claimId);

                if (claim == null)
                {
                    dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == validateClaimProcessRequest.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(validateClaimProcessRequest.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (!string.IsNullOrEmpty(claimType))
                {
                    if (claimType == "New")
                    {
                        //seems ok

                    }
                    else if (claimType == "DealerSubmitted")
                    {
                        var claimStatus = commonEm.GetClaimStatusCodeById(dealerClaim.StatusId);
                        if (claimStatus.ToUpper() == "SUB" || claimStatus.ToUpper() == "INP"|| claimStatus.ToUpper() == "REV" || claimStatus.ToUpper() == "UPD")
                        {
                            //check whether someone has opened same claim
                            ClaimProcessingStat claimProcessingStat = session.Query<ClaimProcessingStat>()
                                .OrderByDescending(a => a.EntryDate)
                                .FirstOrDefault(a => a.ClaimId == validateClaimProcessRequest.claimId &&
                                                     a.UserId != validateClaimProcessRequest.loggedInUserId &&
                                                     a.IsAssignedToOther != true &&
                                                     a.TimeCompleted > DateTime.Parse(SqlDateTime.MinValue.ToString()) &&
                                                     a.IsActive);
                            if (claimProcessingStat == null)
                            {
                                ClaimProcessingStat newClaimProcessingStat = new ClaimProcessingStat()
                                {
                                    Id = Guid.NewGuid(),
                                    AssignedUserName = "",
                                    IsAssignedToOther = false,
                                    ClaimId = validateClaimProcessRequest.claimId,
                                    TimeCompleted = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                                    TimeOpened = DateTime.UtcNow,
                                    TimeStarted = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                                    UserId = validateClaimProcessRequest.loggedInUserId,
                                    UserName = commonEm.GetUserNameById(validateClaimProcessRequest.loggedInUserId),
                                    IsActive = true,
                                    EntryDate = DateTime.UtcNow
                                };
                                if (claim != null)
                                {
                                    claim.StatusId = commonEm.GetClaimStatusIdByCode("INP");
                                }
                                    dealerClaim.StatusId= commonEm.GetClaimStatusIdByCode("INP");
                                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                                {
                                    session.Save(newClaimProcessingStat, newClaimProcessingStat.Id);
                                    if (claim != null)
                                    {
                                        session.Update(claim, validateClaimProcessRequest.claimId);
                                    }
                                    session.Update(dealerClaim, validateClaimProcessRequest.claimId);
                                    transaction.Commit();
                                }
                                response.policyId = dealerClaim.PolicyId;

                            }
                            else if ((DateTime.UtcNow - claimProcessingStat.TimeOpened).Minutes > claimProcessingSafeTime)
                            {
                                //opened and not yet processed + not in safe time
                                response.status = "warning";
                                response.msg = "Selected claim has recently opened by " + commonEm.GetUserNameById(claimProcessingStat.UserId) +
                                    " and only the first user who review claim items will be able to process claim.";
                                response.statId = claimProcessingStat.Id;
                                response.policyId = dealerClaim.PolicyId;
                            }
                            else
                            {
                                //recently opened and in safe time
                                response.status = "error";
                                response.msg = "Selected claim is currently viewing by " + commonEm.GetUserNameById(claimProcessingStat.UserId);
                            }


                        }
                        else
                        {
                            response.status = "error";
                            response.msg = "Selected claim has already been processed by " + commonEm.GetUserNameById((Guid)dealerClaim.LastUpdatedBy);
                        }

                    }
                    else if (claimType == "Existing")
                    {
                        var claimStatus = commonEm.GetClaimStatusCodeById(claim.StatusId);
                        if (claimStatus.ToUpper() == "SUB" || claimStatus.ToUpper() == "INP" || claimStatus.ToUpper() == "REV" || claimStatus.ToUpper() == "UPD" || claimStatus.ToUpper() == "PEN")
                        {
                            //check whether someone has opened same claim
                            ClaimProcessingStat claimProcessingStat = session.Query<ClaimProcessingStat>()
                                .OrderByDescending(a => a.EntryDate)
                                .FirstOrDefault(a => a.ClaimId == validateClaimProcessRequest.claimId &&
                                                     a.UserId != validateClaimProcessRequest.loggedInUserId &&
                                                     a.IsAssignedToOther != true &&
                                                     a.TimeCompleted > DateTime.Parse(SqlDateTime.MinValue.ToString()) &&
                                                     a.IsActive);
                            if (claimProcessingStat == null)
                            {
                                ClaimProcessingStat newClaimProcessingStat = new ClaimProcessingStat()
                                {
                                    Id = Guid.NewGuid(),
                                    AssignedUserName = "",
                                    IsAssignedToOther = false,
                                    ClaimId = validateClaimProcessRequest.claimId,
                                    TimeCompleted = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                                    TimeOpened = DateTime.UtcNow,
                                    TimeStarted = DateTime.Parse(SqlDateTime.MinValue.ToString()),
                                    UserId = validateClaimProcessRequest.loggedInUserId,
                                    UserName = commonEm.GetUserNameById(validateClaimProcessRequest.loggedInUserId),
                                    IsActive = true,
                                    EntryDate = DateTime.UtcNow
                                };
                                if (claim != null)
                                {
                                    claim.StatusId = commonEm.GetClaimStatusIdByCode("INP");
                                }
                                dealerClaim.StatusId = commonEm.GetClaimStatusIdByCode("INP");
                                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                                {
                                    session.Save(newClaimProcessingStat, newClaimProcessingStat.Id);
                                    if (claim != null)
                                    {
                                        session.Update(claim, validateClaimProcessRequest.claimId);
                                    }
                                    else
                                    {
                                        session.Update(dealerClaim, validateClaimProcessRequest.claimId);
                                    }

                                    transaction.Commit();
                                }
                                response.policyId = claim.PolicyId;

                            }
                            else if ((DateTime.UtcNow - claimProcessingStat.TimeOpened).Minutes > claimProcessingSafeTime)
                            {
                                //opened and not yet processed + not in safe time
                                response.status = "warning";
                                response.msg = "Selected claim has recently opened by " + commonEm.GetUserNameById(claimProcessingStat.UserId) +
                                    " and only the first user who review claim items will be able to process claim.";
                                response.statId = claimProcessingStat.Id;
                                response.policyId = claim.PolicyId;
                            }
                            else
                            {
                                //recently opened and in safe time
                                response.status = "error";
                                response.msg = "Selected claim is currently viewing by " + commonEm.GetUserNameById(claimProcessingStat.UserId);
                            }
                        }
                        else
                        {
                            response.status = "error";
                            response.msg = "Selected claim has already been processed by " + commonEm.GetUserNameById(claim.ExamineBy);
                        }

                    }
                }
                else
                {
                    response.status = "error";
                    response.msg = "Invalid claim selection";
                }

            }
            catch (Exception ex)
            {

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;

        }

        internal object SavePartWithClaim(PartWithClaimRequestDto partDetails)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            try
            {

                #region "validate request"

                if (!IsGuid(partDetails.claimId.ToString()) && !IsGuid(partDetails.policyId.ToString()))
                {
                    response.Status = "Invalid claim or policy selection.";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                string claimType = string.Empty;


                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == partDetails.claimId);
                if (claim == null) {
                    claim = session.Query<Claim>().FirstOrDefault(a => a.ClaimSubmissionId == partDetails.claimId);
                    if (claim != null) {
                        partDetails.claimId = claim.Id;
                    }

                }

                if (claim == null)
                {
                    var dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == partDetails.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(partDetails.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    response.Status = "Invalid claim or policy selection";
                    return response;
                }

                if (partDetails.part.serverId == Guid.Empty) {
                    partDetails.part.serverId = Guid.NewGuid();
                }

                #endregion
                var claimInfo = new ClamItemRequestDto()
                {
                    claimId = partDetails.claimId,
                    authorizedAmt = partDetails.part.authorizedAmount,
                    comment = partDetails.part.remark,
                    dealerId = commonEm.GetDealerIdByClaimId(partDetails.claimId),
                    discountAmount = partDetails.part.discountAmount,
                    discountRate = partDetails.part.discountValue,
                    entryBy = partDetails.loggedInUserId,
                    faultId = partDetails.part.fault,
                    goodWillAmount = partDetails.part.goodWillAmount,
                    goodWillRate = partDetails.part.goodWillValue,
                    isDiscountPercentage = partDetails.part.discountType.ToLower() == "p" ? true : false,
                    isGoodWillPercentage = partDetails.part.goodWillType.ToLower() == "p" ? true : false,
                    itemName = partDetails.part.partName,
                    itemNumber = partDetails.part.partNumber,
                    itemType = partDetails.part.partName.ToLower() == "sundry" ? "S" : "P",
                    parentId = 0,
                    partAreaId = partDetails.part.partAreaId,
                    partId = partDetails.part.partId,
                    policyId = commonEm.GetPolicyIdByClaimId(partDetails.claimId),
                    qty = partDetails.part.partQty,
                    remarks = partDetails.part.remark,
                    serverId = partDetails.part.serverId,
                    totalGrossPrice = (partDetails.part.unitPrice * partDetails.part.partQty),
                    totalPrice = (partDetails.part.unitPrice * partDetails.part.partQty)
                    - (partDetails.part.discountAmount + partDetails.part.goodWillAmount),
                    unitPrice = partDetails.part.unitPrice,
                    status = partDetails.part.itemStatus,
                    discountSchemeId = commonEm.GetDealerDiscountSchemeIdByCode(partDetails.part.partDiscountScheme),
                    rejectionTypeId = partDetails.part.rejectionTypeId == Guid.Empty ? null : partDetails.part.rejectionTypeId,
                    causeOfFailureId = partDetails.part.causeOfFailureId,

                };

                if (partDetails.isUpdate && claimType == "Existing")
                {
                    response = UpdateClaimWithItem(claimInfo, partDetails);
                }
                else
                {
                    if (claimType == "DealerSubmitted")
                    {
                        response = SaveDealerSubmittedClaimWithItem(claimInfo, partDetails);
                        response.IsReload = true;
                    }
                    else if (claimType == "New")
                    {
                        claimInfo.policyId = partDetails.policyId;
                        claimInfo.dealerId = partDetails.dealerId;
                        response = SaveTpaEnteredClaim(claimInfo);
                        response.IsReload = true;
                    }
                    else
                    {
                        response = AddNewItemForExistingClaim(claimInfo);
                        response.IsReload = true;
                    }
                    if (partDetails.labour != null && partDetails.labour.nettAmount != decimal.Zero)
                    {
                        var labourChargeSave = new AddLabourChargeToClaimRequestDto()
                        {
                            claimId = response.ClaimId,
                            loggedInUserId = partDetails.loggedInUserId,
                            labourCharge = new LabourChargeData()
                            {
                                discountAmount = partDetails.labour.discountAmount,
                                goodWillAmount = partDetails.labour.goodWillAmount,
                                partId = Guid.Empty,
                                hourlyRate = partDetails.labour.hourlyRate,
                                hours = partDetails.labour.hours,
                                goodWillType = partDetails.labour.goodWillType,
                                totalAmount = partDetails.labour.totalAmount,
                                goodWillValue = partDetails.labour.goodWillValue,
                                discountType = partDetails.labour.discountType,
                                chargeType = partDetails.labour.chargeType,
                                discountValue = partDetails.labour.discountValue,
                                description = partDetails.labour.description,
                                parentId = GetParentIdByPartId(response.ClaimId, partDetails.part.partId),
                                authorizedAmount = partDetails.labour.authorizedAmount,
                                discountSchemeId =
                                    commonEm.GetDealerDiscountSchemeIdByCode(partDetails.labour.labourDiscountScheme),
                                discountSchemeName = partDetails.labour.labourDiscountScheme
                            },
                            dealerId = partDetails.dealerId
                        };

                        var resp = SaveClaimWithLabourCharge(labourChargeSave) as AddClaimItemResponseDto;
                        if (resp == null || resp.Status.ToLower() != "ok")
                        {
                            response.Status = "Error Occured while savaing labour charge.";
                        }
                        else
                        {
                            response.AuthorizedAmount = resp.AuthorizedAmount;
                        }
                    }
                }

                response.ServerId = partDetails.part.serverId;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static ClaimLiabilityValidateResDto ValidateClaimLiability(ProcessClaimRequestDto processClaimRequestDto,Guid contractId, Guid ClaimCurrencyId , Guid PolicyId)
        {
            CurrencyEntityManager currencyEm = new CurrencyEntityManager();
            ClaimLiabilityValidateResDto resDto =  new ClaimLiabilityValidateResDto();
            ISession session = EntitySessionManager.GetSession();
            resDto.Status = true;
            try
            {
                // checking per claim liability limitation
                decimal authAmount = currencyEm.ConvertToBaseCurrency(processClaimRequestDto.athorizedAmount, ClaimCurrencyId);
                var claimLiability = session.Query<Contract>().Where(a => a.Id == contractId).Select(s => new { s.Id, s.ClaimLimitation, s.LiabilityLimitation }).FirstOrDefault();
                if (claimLiability != null && claimLiability.ClaimLimitation > 0 && authAmount > claimLiability.ClaimLimitation)
                {
                    resDto.Status = false;
                    resDto.message = "Exceeded Per Claim Liability Limitation";
                    return resDto;
                }
                // checking Total Liability Limitation
                if (claimLiability != null && claimLiability.LiabilityLimitation > 0)
                {
                    decimal previousApprovedClaimAmount = 0;
                    var  previousApprovedClaimAmountList = session.Query<Claim>().Where(a => a.PolicyId == PolicyId && a.IsApproved == true).Select(s => s.AuthorizedAmount).ToList();

                    if (previousApprovedClaimAmountList != null && previousApprovedClaimAmountList.Count()>0) {
                        previousApprovedClaimAmount = previousApprovedClaimAmountList.Sum();
                    }
                    if ((previousApprovedClaimAmount + authAmount) > claimLiability.LiabilityLimitation)
                    {
                        resDto.Status = false;
                        resDto.message = "Exceeded Total Liability Limitation";
                        return resDto;
                    }
                }
            }
            catch (Exception ex)
            {

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return resDto;
        }

        private AddClaimItemResponseDto UpdateClaimWithItem(ClamItemRequestDto claimInfo, PartWithClaimRequestDto partDetails)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            try
            {
                CommonEntityManager commonEm = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimItem claimItem = session.Query<ClaimItem>()
                    .FirstOrDefault(a => a.Id == claimInfo.serverId);

                ClaimItem claimItemLaboour = session.Query<ClaimItem>()
                    .FirstOrDefault(a => a.ParentId == claimInfo.serverId);

                if (claimItem != null)
                {
                    //delete old claim item
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(claimItem);
                        session.Delete(claimItem);
                        if (claimItemLaboour != null)
                        {
                            session.Evict(claimItemLaboour);
                            session.Delete(claimItemLaboour);
                        }
                        transaction.Commit();
                    }
                }

                //insert new claim item
                //claimInfo.serverId = Guid.Empty;
                response = AddNewItemForExistingClaim(claimInfo);
                //insert labour data
                if (partDetails.labour != null && partDetails.labour.nettAmount != decimal.Zero)
                {
                    var labourChargeSave = new AddLabourChargeToClaimRequestDto()
                    {
                        claimId = partDetails.claimId,
                        loggedInUserId = partDetails.loggedInUserId,
                        labourCharge = new LabourChargeData()
                        {
                            discountAmount = partDetails.labour.discountAmount,
                            goodWillAmount = partDetails.labour.goodWillAmount,
                            partId = Guid.Empty,
                            hourlyRate = partDetails.labour.hourlyRate,
                            hours = partDetails.labour.hours,
                            goodWillType = partDetails.labour.goodWillType,
                            totalAmount = partDetails.labour.totalAmount,
                            goodWillValue = partDetails.labour.goodWillValue,
                            discountType = partDetails.labour.discountType,
                            chargeType = partDetails.labour.chargeType,
                            discountValue = partDetails.labour.discountValue,
                            description = partDetails.labour.description,
                            parentId = GetParentIdByPartId(response.ClaimId, partDetails.part.partId),
                            authorizedAmount = partDetails.labour.authorizedAmount,
                            discountSchemeId =
                                commonEm.GetDealerDiscountSchemeIdByCode(partDetails.labour.labourDiscountScheme),
                            discountSchemeName = partDetails.labour.labourDiscountScheme
                        },
                        dealerId = partDetails.dealerId
                    };

                    var resp = SaveClaimWithLabourCharge(labourChargeSave) as AddClaimItemResponseDto;
                    if (resp == null || resp.Status.ToLower() != "ok")
                    {
                        response.Status = "Error Occured while savaing labour charge.";
                    }
                    else
                    {
                        response.AuthorizedAmount = resp.AuthorizedAmount;
                    }
                }

                //update claim details
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimInfo.claimId);
                List<ClaimItem> claimItems = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);
                //claim.ApprovedDate =DateTime.UtcNow ;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(claim);
                    session.Update(claim, claim.Id);
                    transaction.Commit();
                }

                response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                response.ClaimId = claim.Id;
                response.ClaimNo = claim.ClaimNumber;
                response.IsReload = false;
                response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                response.Status = "ok";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                response.Status = "Error occured while updating the claim";
            }
            return response;
        }

        private Guid GetParentIdByPartId(Guid claimId, Guid partId)
        {
            Guid response = Guid.Empty;

            try
            {
                if (IsGuid(claimId.ToString()) && IsGuid(partId.ToString()))
                {
                    ISession session = EntitySessionManager.GetSession();
                    ClaimItem claimItem = session.Query<ClaimItem>().OrderByDescending(a => a.SeqId)
                        .FirstOrDefault(a => a.ClaimId == claimId && a.PartId == partId);
                    if (claimItem != null)
                    {
                        response = claimItem.Id;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private AddClaimItemResponseDto AddNewItemForExistingClaim(ClamItemRequestDto claimInfo)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimInfo.claimId);
                if (claim != null)
                {
                    claimInfo.dealerId = claim.ClaimSubmittedDealerId;
                    ClaimItem claimItem = DBDTOTransformer.Instance.ClaimItemRequestToNewClaimItem(claimInfo, claim);

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(claimItem, claimItem.Id);

                        List<ClaimItem> claimItems = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                        claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                        claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                        claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);

                        claim.ApprovedDate=DateTime.UtcNow;
                        session.Update(claim, claim.Id);
                        transaction.Commit();
                    }
                    //setting up response
                    response.Status = "ok";
                    response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimNo = claim.ClaimNumber;
                    response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimId = claim.Id;
                    response.IsReload = false;

                }
                else
                {
                    response.Status = "Invalid claim selection";
                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetPolicyDetailsByPolicyIdForClaimProcess(Guid policyId)
        {
            PolicyDataResponseForClaimProcessDto response = new PolicyDataResponseForClaimProcessDto();
            response.Status = "ok";
            try
            {
                #region "validation"

                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                PolicyBundle policyBundle = session.Query<PolicyBundle>().FirstOrDefault(a => a.Id == policyId);
                Policy policy = new Policy();
                if (policyBundle == null)
                {
                    policy = session.Query<Policy>().FirstOrDefault(a => a.Id == policyId);
                }
                else
                {
                    policy = session.Query<Policy>().FirstOrDefault(a => a.PolicyBundleId == policyBundle.Id);
                }

                if (policy == null)
                {
                    response.Status = "Invlaid Policy Selction";
                    return response;
                }

                #endregion

                Dealer dealer = session.Query<Dealer>().FirstOrDefault(a => a.Id == policy.DealerId);
                Customer customer = session.Query<Customer>().FirstOrDefault(a => a.Id == policy.CustomerId);

                response.CustomerName = cem.GetCustomerNameById(policy.CustomerId);
                response.CustomerTel = customer.MobileNo;
                response.Make = cem.GetMakeNameById(Guid.Parse(cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "make")));
                response.Model = cem.GetModelNameById(Guid.Parse(cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "model")));
                response.Serial = cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "serial");
                response.PolicyCountry = cem.GetCountryNameById(dealer.CountryId);
                response.PolicyNo = policy.PolicyNo;
                response.PolicyDealer = dealer.DealerName;
                response.PolicyDealerId = dealer.Id;
                response.CommodityCategoryId = cem.GetCommodityCategoryIdByContractId(policy.ContractId);
                response.ClaimDealers = cem.GetAllDealersByMakeId(Guid.Parse(cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id, policy.CommodityTypeId, "make")));
                response.MakeId =
                    Guid.Parse(cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id,
                        policy.CommodityTypeId, "make"));
                response.ModelId =
                    Guid.Parse(cem.GetSerialNumberMakeIdModelIdByPolicyIdCommodityTypeId(policy.Id,
                        policy.CommodityTypeId, "model"));
                response.PolicyCountryId = dealer.CountryId;
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object SaveClaimEngineerComment(SaveClaimEngineerCommentRequestDto clamEngineerRequest)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                #region "validation"

                if (!IsGuid(clamEngineerRequest.claimId.ToString()) && !IsGuid(clamEngineerRequest.dealerId.ToString())
                    && !IsGuid(clamEngineerRequest.policyId.ToString()))
                {
                    response.Status = "Request data is invalid.";
                    return response;
                }


                ClaimSubmission dealerClaim = new ClaimSubmission();
                string claimType = string.Empty;
                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == clamEngineerRequest.claimId);
                if (claim == null)
                {
                    dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == clamEngineerRequest.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(clamEngineerRequest.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    response.Status = "Request data is invalid.";
                    return response;
                }
                #endregion

                if (claimType == "Existing")
                {
                    response = UpdateClaimEngineerCommentsOnClaim(clamEngineerRequest);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static AddClaimItemResponseDto UpdateClaimEngineerCommentsOnClaim(SaveClaimEngineerCommentRequestDto clamEngineerRequest)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {

                ISession session = EntitySessionManager.GetSession();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == clamEngineerRequest.claimId);

                claim.EngineerComment = clamEngineerRequest.engineer;
                claim.Conclution = clamEngineerRequest.conclution;
                claim.LastUpdatedDate = DateTime.UtcNow;
                claim.LastUpdatedBy = clamEngineerRequest.loggedInUserId;
                claim.ClaimDate = claim.ClaimDate <= SqlDateTime.MinValue.Value
                    ? SqlDateTime.MinValue.Value
                    : claim.ClaimDate;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(claim);
                    transaction.Commit();
                }

                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                response.ClaimNo = claim.ClaimNumber;
                response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                response.ClaimId = claim.Id;
                response.IsReload = false;
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object SaveClaimWithLabourCharge(AddLabourChargeToClaimRequestDto addLabourChargeToClaimRequestDto)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {
                #region "validation"
                ISession session = EntitySessionManager.GetSession();
                if (!IsGuid(addLabourChargeToClaimRequestDto.claimId.ToString()))
                {
                    response.Status = "Invalid claim selection";
                    return response;
                }

                Claim claim =
                    session.Query<Claim>().FirstOrDefault(a => a.Id == addLabourChargeToClaimRequestDto.claimId);
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == addLabourChargeToClaimRequestDto.claimId);

                string claimType = string.Empty;
                if (claim == null)
                {
                    if (claimSubmission != null)
                    {
                        claimType = "New";
                    }
                }
                else
                {
                    claimType = "Excisting";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    response.Status = "Invalid claim selection";
                    return response;
                }


                #endregion
                if (claimType == "New")
                {

                    Claim newClaim = DBDTOTransformer.Instance.ClaimSubmissionToClaim(claimSubmission);
                    List<ClaimItem> newClaimItems = DBDTOTransformer.Instance.ClaimSubmissionItemsToClaimItems(claimSubmission, addLabourChargeToClaimRequestDto);
                    List<ClaimSubmissionAttachment> oldClaimAttachments = session.Query<ClaimSubmissionAttachment>().Where(a => a.ClaimSubmissionId == addLabourChargeToClaimRequestDto.claimId).ToList();
                    //reuse exisiting method
                    ClamItemRequestDto clamItemRequest = new ClamItemRequestDto() { claimId = addLabourChargeToClaimRequestDto.claimId };
                    List<ClaimAttachment> claimAttachments =
                        DBDTOTransformer.Instance.DealerClaimAttachementsToClaimAttachemnts(clamItemRequest, newClaim);

                    newClaim.AuthorizedAmount = newClaimItems.Where(a => a.IsApproved == true).Sum(a => a.AuthorizedAmount);
                    newClaim.TotalGrossClaimAmount = newClaimItems.Sum(a => a.TotalGrossPrice);
                    newClaim.TotalClaimAmount = newClaimItems.Sum(a => a.TotalPrice);

                    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        var newClaimNumber =
                            session.Query<Claim>().Count(a => a.PolicyCountryId == newClaim.PolicyCountryId) + 1;
                        var countryCode = new CommonEntityManager().GetCountryCodeById(newClaim.PolicyCountryId);
                        newClaim.ClaimNumber = countryCode.ToUpper() + "/" + Convert.ToString(newClaimNumber).PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0');

                        session.Save(newClaim, newClaim.Id);

                        foreach (ClaimItem newClaimItem in newClaimItems)
                        {
                            newClaimItem.ClaimId = newClaim.Id;
                            session.Evict(newClaimItem);
                            session.Save(newClaimItem, newClaimItem.Id);
                        }

                        foreach (ClaimAttachment claimAttachment in claimAttachments)
                        {
                            session.Evict(claimAttachment);
                            session.Save(claimAttachment, claimAttachment.Id);
                        }

                        //update old claimdetails
                        claimSubmission.ClaimNumber = newClaim.ClaimNumber;
                        session.Evict(claimSubmission);
                        session.Update(claimSubmission, claimSubmission.Id);

                        transaction.Commit();
                    }
                    //setting up response
                    response.Status = "ok";
                    response.AuthorizedAmount = Math.Round(newClaim.AuthorizedAmount * newClaim.ConversionRate * 100) / 100;
                    response.ClaimNo = newClaim.ClaimNumber;
                    response.RequestedAmount = Math.Round(newClaim.TotalClaimAmount * newClaim.ConversionRate * 100) / 100;
                    response.ClaimId = newClaim.Id;
                    response.IsReload = true;
                }
                else
                {
                    ClaimItem clamItem = DBDTOTransformer.Instance.LabourChargeToClaimItem(claim, addLabourChargeToClaimRequestDto);

                    //claim.AuthorizedAmount += clamItem.AuthorizedAmount;
                    //claim.TotalGrossClaimAmount += clamItem.TotalGrossPrice;
                    //claim.TotalClaimAmount += clamItem.TotalPrice;


                    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {

                        session.Evict(clamItem);
                        session.Save(clamItem, clamItem.Id);


                        List<ClaimItem> claimItems = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                        claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                        claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                        claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);

                        session.Evict(claim);
                        session.Update(claim);


                        transaction.Commit();
                    }

                    //setting up response
                    response.Status = "ok";
                    response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimNo = claim.ClaimNumber;
                    response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimId = claim.Id;
                    response.IsReload = true;
                }


            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object ProcessClaim(ProcessClaimRequestDto claimProcessRequest, Guid tpaId)
        {
            AddClaimItemResponseDto response = new AddClaimItemResponseDto();
            response.Status = "ok";
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimProcessRequest.claimId);
                ClaimSubmission claimsubmission= session.Query<ClaimSubmission>().FirstOrDefault(a => a.ClaimNumber == claim.ClaimNumber);
                Policy policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claim.PolicyId);
                CommodityType commodityType = session.Query<CommodityType>()
                    .Where(a => a.CommodityTypeId == policy.CommodityTypeId).FirstOrDefault();
                Product product = session.Query<Product>().Where(a => a.Id == policy.ProductId).FirstOrDefault();
                ProductType ProductType = session.Query<ProductType>().Where(a => a.Id == product.ProductTypeId).FirstOrDefault();
                VehicleDetails vehicleDetails = new VehicleDetails();
                VehiclePolicy vehiclePolicy = new VehiclePolicy();
                OtherItemPolicy otherItemPolicy = new OtherItemPolicy();
                OtherItemDetails otherItemDetails = new OtherItemDetails();

                if (commodityType.CommodityCode == "A")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    vehicleDetails = session.Query<VehicleDetails>().Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                }

                if (commodityType.CommodityCode == "B")
                {
                    vehiclePolicy = session.Query<VehiclePolicy>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();
                    vehicleDetails = session.Query<VehicleDetails>().Where(a => a.Id == vehiclePolicy.VehicleId).FirstOrDefault();
                }

                if(commodityType.CommodityCode == "O")
                {
                    otherItemPolicy = session.Query<OtherItemPolicy>().Where(a => a.PolicyId ==
                    policy.Id).FirstOrDefault();
                    otherItemDetails = session.Query<OtherItemDetails>().Where(a => a.Id == otherItemPolicy.OtherItemId).FirstOrDefault();
                }


                #region Validation
                if (claim == null)
                {
                    response.Status = "Invalid claim selection.";
                    return response;
                }
                if (ProductType.Code != "ILOE")
                {
                    if (string.IsNullOrEmpty(claim.EngineerComment) || string.IsNullOrEmpty(claim.Conclution))
                    {
                        response.Status = "Please save both engineer comment & conclusion.";
                        return response;
                    }
                }
                if (claimProcessRequest.claimMileage <= 0)
                {
                    response.Status = "Invalid claim mileage/hrs.";
                    return response;
                }
                bool isCommentAvailable = false;
                if (claimProcessRequest.status == "REJ" || claimProcessRequest.status == "REQ" || claimProcessRequest.status == "PEN")
                {
                    if (string.IsNullOrEmpty(claimProcessRequest.comment.Trim()))
                    {
                        response.Status = "Please enter comment.";
                        return response;
                    }
                    else
                    {
                        isCommentAvailable = true;
                    }
                }

                // validate Mileage
                if (claimProcessRequest.status == "APP") {
                    int milageVerification = session.Query<Claim>()
                        .Where(p => p.PolicyId == claim.PolicyId && p.ClaimMileageKm > claimProcessRequest.claimMileage)
                        .Join(session.Query<ClaimStatusCode>(), m => m.StatusId, n => n.Id, (m, n) => new { m, n })
                        .Where(w =>
                            w.n.StatusCode == "PID" ||
                            w.n.StatusCode == "PRC" ||
                            w.n.StatusCode == "REV" ||
                            w.n.StatusCode == "APP" ||
                            w.n.StatusCode == "PIP"
                        ).Select(se => new { Milage = se.m.ClaimMileageKm })
                        .Count();


                    if (milageVerification > 0)
                    {
                        response.Status = "mismatch claim mileage/hrs with previous claim";
                        return response;
                    }


                    // validate failure Date between contract start date and end date;
                    Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();
                    bool validateContractPeriod = true;
                    if (!contract.IsActive)
                    {
                        validateContractPeriod = false;
                        response.Status = "Contract Is Not Active Status";
                        return response;
                    }

                    if (claim.FailureDate.ToString("yyyy-MM-dd") != "1753-01-01") {
                        if (contract.StartDate <= claim.FailureDate && contract.EndDate >= claim.FailureDate)
                        {
                            validateContractPeriod = true;
                        }
                        else
                        {
                            validateContractPeriod = false;
                            response.Status = "Contract Expired !   Contract Period    : \n " + contract.StartDate.ToString("dd-MMM-yyyy") + " to " + contract.EndDate.ToString("dd-MMM-yyyy") + "  ";
                            return response;
                        }
                    }

                }

                // validate per claim liability & total Claim Liability
                if (claimProcessRequest.status == "APP")
                {
                    ClaimLiabilityValidateResDto validateObj = ValidateClaimLiability(claimProcessRequest,policy.ContractId, claimsubmission.ClaimCurrencyId, policy.Id);
                    if (!validateObj.Status) {
                        response.Status = validateObj.message;
                        return response;
                    }
                }

                    #endregion

                    claim.StatusId = cem.GetClaimStatusIdByCode(claimProcessRequest.status);
                claimsubmission.StatusId= cem.GetClaimStatusIdByCode(claimProcessRequest.status);
                claim.LastUpdatedDate = DateTime.UtcNow;
                claim.IsApproved = claimProcessRequest.status == "APP" ? true : false;
                claim.ClaimDate = claimProcessRequest.claimDate < SqlDateTime.MinValue.Value ? SqlDateTime.MinValue.Value : claimProcessRequest.claimDate;
                claim.ClaimMileageKm = claimProcessRequest.claimMileage;
                claim.LastUpdatedBy = claimProcessRequest.loggedInUserId;
                claim.AuthorizedAmount = claimProcessRequest.athorizedAmount;
                if (claimProcessRequest.status == "APP")
                {
                    claim.ApprovedBy = claimProcessRequest.loggedInUserId;
                    claim.ApprovedDate = DateTime.UtcNow;
                }
                //set up uploaded ids
                List<ClaimAttachment> claimList = claimProcessRequest.policyDocIds.Select(attachmentId => new ClaimAttachment()
                {
                    Id = Guid.NewGuid(),
                    ClaimId = claim.Id,
                    UserAttachmentId = attachmentId
                }).ToList();

                //add claim comments if available
                ClaimComment claimComment = new ClaimComment();
                if (isCommentAvailable)
                {
                    claimComment = new ClaimComment()
                    {
                        ByTPA = true,
                        ClaimId = claim.Id,
                        Id = Guid.NewGuid(),
                        Comment = claimProcessRequest.comment,
                        EntryDateTime = DateTime.UtcNow,
                        PolicyId = claim.PolicyId,
                        Seen = false,
                        SeenDateTime = SqlDateTime.MinValue.Value,
                        SentFrom = claimProcessRequest.loggedInUserId,
                        SentTo = claim.ClaimSubmittedBy
                    };
                }

                List<TaxTypes> taxTypes = session.Query<TaxTypes>().ToList();
                List<ClaimTax> claimTax = new List<ClaimTax>();
                //string taxIds;


                foreach (Guid taxIds in claimProcessRequest.claimtaxIds)
                {
                    CountryTaxes countryTaxes = session.Query<CountryTaxes>().Where(a => a.TaxTypeId == taxIds).FirstOrDefault();

                    if(countryTaxes.IsPercentage == true)
                    {
                        ClaimTax Tax = new ClaimTax()
                        {
                            Id = Guid.Empty,
                            ClaimId = claim.Id,
                            CountyTaxId = countryTaxes.TaxTypeId,
                            Amount = currencyEm.ConvertToBaseCurrency(((claimProcessRequest.athorizedAmountbefortax * countryTaxes.TaxValue) / 100), claimsubmission.ClaimCurrencyId)
                        };
                        claimTax.Add(Tax);
                    }
                    else
                    {
                        ClaimTax Tax = new ClaimTax()
                        {
                            Id = Guid.Empty,
                            ClaimId = claim.Id,
                            CountyTaxId = countryTaxes.TaxTypeId,
                            Amount = currencyEm.ConvertToBaseCurrency(countryTaxes.TaxValue, claimsubmission.ClaimCurrencyId)
                        };
                        claimTax.Add(Tax);
                    }

                }

                using (ITransaction transaction = session.BeginTransaction())
                {
                    if (isCommentAvailable)
                    {
                        session.Save(claimComment, claimComment.Id);
                    }

                    foreach (ClaimAttachment claimAttachment in claimList)
                    {
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment);
                    }

                    foreach(ClaimTax claimtx in claimTax)
                    {
                        session.Save(claimtx);
                    }

                    session.Update(claim);
                    session.Update(claimsubmission);

                    transaction.Commit();
                }

                //sendout notifications to update user hence process successfull
                try
                {
                    CommonEntityManager commonEntityManager = new CommonEntityManager();
                    var notificationDto = new PushNotificationsRequestDto()
                    {
                        generatedTime = DateTime.UtcNow,
                        message = GenerateClaimNotificationTextByClaimNoAndClaimStatus(claim.ClaimNumber, claimProcessRequest.status),
                        link = ConfigurationData.BaseUrl + "app/claim/listing/" + claim.Id,
                        messageFrom = commonEntityManager.GetUserNameById(claimProcessRequest.loggedInUserId),
                        profilePic = commonEntityManager.GetProfilePictureByUserId(claimProcessRequest.loggedInUserId),
                        userDetails = new List<UserDetail>()
                        {
                            new UserDetail()
                            {
                                tpaId = tpaId,
                                userId = claim.ClaimSubmittedBy
                            }

                        }
                    };
                    Task.Run(async () => await NotificationEntityManager.PushNotificationSender(notificationDto));

                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }

                List<ClaimItem> claimItems = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                List<ClaimTax> claimTaxes = session.Query<ClaimTax>().Where(b => b.ClaimId == claim.Id).ToList();

                claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount) + claimTaxes.Sum(b => b.Amount);
                claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice) + claimTaxes.Sum(b => b.Amount);
                claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice) + claimTaxes.Sum(b => b.Amount);
                claim.ApprovedDate = DateTime.UtcNow;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Evict(claim);
                    session.Update(claim, claim.Id);
                    transaction.Commit();
                }

                response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                response.ClaimId = claim.Id;
                response.ClaimNo = claim.ClaimNumber;
                response.IsReload = false;
                response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                response.Status = "ok";

                if(response.Status == "ok")
                {
                    if (commodityType.CommodityCode == "A")
                    {
                        if (claimProcessRequest.status == "APP")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                            new GetMyEmail().ClaimApprovedEmail(EmailList, ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).FirstName, vehicleDetails.VINNo, claim.ClaimNumber);
                        }
                        else if (claimProcessRequest.status == "REJ")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                            new GetMyEmail().ClaimRejectedEmail(EmailList, ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).FirstName, vehicleDetails.VINNo, claim.ClaimNumber);
                        }
                    }
                    else if (commodityType.CommodityCode == "B")
                    {
                        if (claimProcessRequest.status == "APP")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                            new GetMyEmail().ClaimApprovedEmail(EmailList, ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).FirstName, vehicleDetails.VINNo, claim.ClaimNumber);
                        }
                        else if (claimProcessRequest.status == "REJ")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                            new GetMyEmail().ClaimRejectedEmail(EmailList, ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).FirstName, vehicleDetails.VINNo, claim.ClaimNumber);
                        }
                    }
                    else if(commodityType.CommodityCode == "O")
                    {
                        if (claimProcessRequest.status == "APP")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);

                            new GetMyEmail().TyreClaimApprovedEmail(EmailList, claim.PolicyNumber);
                        }
                        else if (claimProcessRequest.status == "REJ")
                        {
                            ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                                .Where(a => a.Id == claim.ClaimSubmissionId).FirstOrDefault();
                            UserEntityManager ce = new UserEntityManager();
                            List<String> EmailList = new List<string>();
                            EmailList.Add(ce.GetUserById(claimSubmission.ClaimSubmittedBy.ToString()).Email);
                            new GetMyEmail().TyreClaimRejection(EmailList, claim.PolicyNumber);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static string GenerateClaimNotificationTextByClaimNoAndClaimStatus(string claimNumber, string status)
        {
            string response = string.Empty;
            switch (status)
            {
                case "APP":
                    response = "Claim - " + claimNumber + " has been approved."; break;
                case "REJ":
                    response = "Claim -  " + claimNumber + " has been rejected. Please review comments for more information."; break;
                case "HOL":
                    response = "Claim -  " + claimNumber + " has been kept on hold."; break;
                case "REQ":
                    response = "Claim -  " + claimNumber + " needed further details. Please review comments for more information."; break;
                default:
                    break;
            }
            return response;
        }

        internal static object GetPolicyDetailsForView(GetPolicyDetailsForViewInClaimRequestDto getPolicyDetailsForViewRequest)
        {

            PolicyDetailsForClaimViewResponseDto response = new PolicyDetailsForClaimViewResponseDto();
            try
            {
                #region "validation"
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission dealerClaim = new ClaimSubmission();
                string claimType = string.Empty;
                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == getPolicyDetailsForViewRequest.claimId);
                if (claim == null)
                {
                    dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == getPolicyDetailsForViewRequest.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(getPolicyDetailsForViewRequest.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    return response;
                }

                #endregion
                Guid policyId = Guid.Empty;
                if (claimType == "New")
                {
                    policyId = getPolicyDetailsForViewRequest.policyId;
                }
                else if (claimType == "DealerSubmitted")
                {
                    policyId = dealerClaim.PolicyId;
                }
                else
                {
                    policyId = claim.PolicyId;
                }

                PolicyBundle policyBundle = session.Query<PolicyBundle>()
                    .FirstOrDefault(a => a.Id == policyId);
                Policy policy = new Policy();
                if (policyBundle == null)
                {
                    policy = session.Query<Policy>()
                    .FirstOrDefault(a => a.Id == policyId);
                }
                else
                {
                    policy = session.Query<Policy>()
                     .FirstOrDefault(a => a.PolicyBundleId == policyBundle.Id);
                }

                String Query = File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath(ConfigurationData.QueryPath + "\\PolicySummaryViewInClaim.sql"));
                Query = Query.Replace("{PolicyId}", policy.Id.ToString());
                var data = session.CreateSQLQuery(Query).SetResultTransformer(Transformers.AliasToBean<PolicyDetailsForClaimViewResponseDto>())
                .List<PolicyDetailsForClaimViewResponseDto>().FirstOrDefault();
                List<Claim> claims = session.Query<Claim>()
                   .Where(a => a.PolicyId == policyId && a.IsApproved==true).ToList();
                var sumOfAmount= decimal.Zero;
                if (claims.Count > 0)
                {
                    foreach (Claim claimItem in claims)
                    {

                        sumOfAmount = sumOfAmount + Math.Round(claimItem.AuthorizedAmount * claim.ConversionRate * 100) / 100;

                    }
                }
                //extra formatting :(
                var saleAmount = decimal.Zero;
                if(data.salePrice != null)
                {
                    var Unit = data.salePrice.Split(' ')[1];
                    var canParse = decimal.TryParse(data.salePrice.Split(' ')[0], out saleAmount);
                    if (canParse)
                    {
                        data.salePrice = (Math.Round(saleAmount * 100) / 100).ToString("N", CultureInfo.InvariantCulture);
                        data.salePrice += " " + Unit;
                    }
                }


                //setup formatting
                data.s_manfWarrentyEndDate = data.manfWarrentyEndDate.ToString("dd-MMM-yyyy");
                data.s_extensionStartDate = data.extensionStartDate.ToString("dd-MMM-yyyy");
                data.s_manfWarrentyStartDate = data.manfWarrentyStartDate.ToString("dd-MMM-yyyy");
                data.s_extensionEndDate = data.extensionEndDate.ToString("dd-MMM-yyyy");

                if(data.manfWarrentyMilage != null)
                    data.manfWarrentyMilage = decimal.Parse(data.manfWarrentyMilage).ToString("N0", CultureInfo.InvariantCulture);


                if (data.extensionMilage != "Unlimited")
                {
                    data.extensionMilage = decimal.Parse(data.extensionMilage).ToString("N0", CultureInfo.InvariantCulture);
                }
                if (data.cutoff != "Unlimited")
                {
                    data.cutoff = decimal.Parse(data.cutoff).ToString("N0", CultureInfo.InvariantCulture);
                }
                data.sumOfAuthorizedClaimedAmount = sumOfAmount;
                response = data;
            }
            catch (Exception ex)
            {

                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetClaimDetailsForView(GetPolicyDetailsForViewInClaimRequestDto getPolicyDetailsForViewInClaimRequestDto)
        {
            ClaimDetailsForClaimViewResponseDto response = new ClaimDetailsForClaimViewResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                #region validation
                ClaimSubmission dealerClaim = new ClaimSubmission();
                string claimType = string.Empty;
                var claim = session.Query<Claim>().FirstOrDefault(a => a.Id == getPolicyDetailsForViewInClaimRequestDto.claimId);
                if (claim == null)
                {
                    dealerClaim = session.Query<ClaimSubmission>().FirstOrDefault(a => a.Id == getPolicyDetailsForViewInClaimRequestDto.claimId);
                    if (dealerClaim == null)
                    {
                        claimType = IsGuid(getPolicyDetailsForViewInClaimRequestDto.policyId.ToString()) ? "New" : "";
                    }
                    else
                    {
                        claimType = "DealerSubmitted";
                    }
                }
                else
                {
                    claimType = "Existing";
                }

                if (string.IsNullOrEmpty(claimType))
                {
                    return response;
                }
                #endregion

                response.AssessmentData = new List<AssessmentData>();
                response.ClaimData = new List<ClaimData>();

                Guid policyId = Guid.Empty;
                if (claimType == "New")
                {
                    policyId = getPolicyDetailsForViewInClaimRequestDto.policyId;
                }
                else if (claimType == "DealerSubmitted")
                {
                    policyId = dealerClaim.PolicyId;
                }
                else
                {
                    policyId = claim.PolicyId;
                }

                List<Claim> claims = session.Query<Claim>()
                    .Where(a => a.PolicyId == policyId).OrderByDescending(a => a.EntryDate).ToList();

                claims.RemoveAll(a => a.Id == getPolicyDetailsForViewInClaimRequestDto.claimId);

                foreach (Claim claimObj in claims)
                {

                    ClaimItem claimItem = session.Query<ClaimItem>().Where(a => a.ClaimId == claimObj.Id).FirstOrDefault();
                    Policy policy = session.Query<Policy>().Where(a => a.Id == claimObj.PolicyId).FirstOrDefault();
                    Contract contract = session.Query<Contract>().Where(a => a.Id == policy.ContractId).FirstOrDefault();

                    response.AssessmentData.Add(new AssessmentData()
                    {
                        claimNumber = claimObj.ClaimNumber,
                        failureDate = claimObj.FailureDate,
                        milage = claimObj.ClaimMileageKm.ToString(),
                        fault = cem.GetFaultNameById(claimItem.FaultId),
                        approvedDate = claimObj.ApprovedDate,
                        perClaimLiability = contract.ClaimLimitation.ToString(),
                        totalLiability = contract.LiabilityLimitation.ToString(),
                        status = cem.GetClaimStatusCodeById(claimObj.StatusId),
                        conclution = claimObj.Conclution,
                        s_claimDate = claimObj.EntryDate.ToString("dd-MMM-yyyy"),

                        customerComplaint = claimObj.CustomerComplaint,
                        dealerComment = claimObj.DealerComment,
                        engineerAssesment = claimObj.EngineerComment,


                    });

                    response.ClaimData.Add(new ClaimData()
                    {
                        failureDate = claimObj.FailureDate.ToString("dd-MMM-yyyy"),
                        fault = cem.GetFaultNameById(claimItem.FaultId),
                        approvedDate = claimObj.ApprovedDate.ToString("dd-MMM-yyyy"),
                        perClaimLiability = cem.GetCurrencyTypeByIdCode(claimObj.ClaimCurrencyId) + " " +(Math.Round(contract.ClaimLimitation * claimObj.ConversionRate * 100) / 100)  ,

                        totalLiability = cem.GetCurrencyTypeByIdCode(claimObj.ClaimCurrencyId) + " " +(Math.Round(contract.LiabilityLimitation * claimObj.ConversionRate * 100) / 100) ,
                        milage = claimObj.ClaimMileageKm.ToString(),
                        claimId = claimObj.Id,
                        status = cem.GetClaimStatusCodeById(claimObj.StatusId),
                        s_status = cem.GetClaimStatusNameById(claimObj.StatusId),
                        authorizedAmount = cem.GetCurrencyTypeByIdCode(claimObj.ClaimCurrencyId) + " " + (Math.Round(claimObj.AuthorizedAmount * claimObj.ConversionRate * 100) / 100),
                        claimNo = claimObj.ClaimNumber,
                        comments = claimObj.DealerComment,
                        s_date = claimObj.EntryDate.ToString("dd-MMM-yyyy"),
                        requestedAmount = cem.GetCurrencyTypeByIdCode(claimObj.ClaimCurrencyId) + " " +(Math.Round(claimObj.TotalClaimAmount * claimObj.ConversionRate * 100) / 100) ,
                    });
                }

                return response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetAllClaimForSearchGrid(ClaimSearchGridRequestDto ClaimSearchGridRequestDto)
        {
            if (ClaimSearchGridRequestDto != null && ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid != null)
            {
                ISession session = EntitySessionManager.GetSession();
                List<Guid> claimBordxDetail = session.Query<ClaimBordxDetail>()
                        .Select(a => a.ClaimId).ToList();

                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();
                filterClaim = filterClaim.And(a => claimBordxDetail.Contains(a.Id));
                //IQueryable<ClaimBordxDetail> claimBordxDetail;


                //var filteredClaim = session.Query<Claim>().Where(filterClaim);

                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                if (!String.IsNullOrEmpty(ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.claimNumber))
                {
                    filterClaim = filterClaim.And(a => a.ClaimNumber.ToLower().Contains(ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.claimNumber.ToLower()));

                }
                if (IsGuid(ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.commodityTypeId.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.CommodityTypeId == ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.commodityTypeId);
                }
                if (IsGuid(ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.customerId.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.CustomerId == ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.customerId);
                }
                if (IsGuid(ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.policyId.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.PolicyId == ClaimSearchGridRequestDto.claimSearchGridSearchCriterias.policyId);
                }


                //ISession session = EntitySessionManager.GetSession();
                var filteredClaim = session.Query<Claim>().Where(filterClaim);

                //filteredClaim = filteredClaim.Where(b => claimBordxDetail.Any(c => c == b.Id && c.IsBatching == true));

                long TotalRecords = filteredClaim.Count();
                var customerGridDetailsFilterd = filteredClaim.Skip((ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageNumber - 1) * ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageSize)
                .Take(ClaimSearchGridRequestDto.paginationOptionsClaimSearchGrid.pageSize)
                .Select(a => new
                {
                    Id = a.Id,
                    PolicyId = a.PolicyId,
                    CommodityType = cem.GetCommodityTypeNameById(a.CommodityTypeId),
                    PolicyNumber = a.PolicyNumber,
                    ClaimNumber = a.ClaimNumber,
                    ClaimDealer = cem.GetDealerNameById(a.ClaimSubmittedDealerId),
                    Make = cem.GetMakeNameById(a.MakeId),
                    Model = cem.GetModelNameById(a.ModelId),
                    ClaimAmount = currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount, a.ClaimCurrencyId, a.CurrencyPeriodId) +
                        " " +
                        cem.GetCurrencyTypeByIdCode(a.ClaimCurrencyId),
                    Date = a.EntryDate

                })
                .ToArray()
                .OrderBy(a => a.ClaimNumber);
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

        internal object GetClaimDetilsByClaimId(Guid claimId)
        {
            object response = null;
            try
            {
                if (!IsGuid(claimId.ToString()))
                {
                    return response;
                }

                ClaimRequestDetailsResponseDto claimDetails = new ClaimRequestDetailsResponseDto();

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ClaimSubmission claimSubmission = session
                    .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);
                if (claimSubmission == null && claim == null)
                {
                    return response;
                }

                bool fromClaim = claim != null;
                if (claim != null)
                {
                    List<ClaimItem> claimItems = session.Query<ClaimItem>()
                       .Where(a => a.ClaimId == claimId).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>();
                    int claimItemseq = 0;
                    foreach (ClaimItem claimItem in claimItems)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            unitPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            authorizedAmount = currencyEm.ConvertFromBaseCurrency(claimItem.AuthorizedAmount, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            faultId = claimItem.FaultId,
                            faultName = cem.GetFaultNameById(claimItem.FaultId),
                            status = claimItem.IsApproved == true ? "Approved" : (claimItem.IsApproved == false ? "Rejected" : ""),
                            statusCode = claimItem.IsApproved == true ? "A" : (claimItem.IsApproved == false ? "R" : ""),
                            discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claim.ClaimCurrencyId,
                        claim.CurrencyPeriodId),
                            comment = claimItem.TpaComment,
                            isDiscountPercentage = claimItem.IsDiscountPercentage,
                            isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                            totalGrossPrice = currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate : currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate : currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId)

                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claim.ClaimNumber;
                    claimDetails.DealerName = cem.GetDealerNameById(claim.ClaimSubmittedDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claim.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claim.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount,
                        claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.AuthorizedClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.AuthorizedAmount,
                        claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claim.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claim.PolicyCountryId);
                    //colmplaint
                    claimDetails.Complaint = new Complaint
                    {
                        customer = claim.CustomerComplaint,
                        dealer = claim.DealerComment,
                        conclution = claim.Conclution,
                        engineer = claim.EngineerComment
                    };
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claim.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claim.CommodityCategoryId;
                    claimDetails.MakeId = claim.MakeId;
                    claimDetails.ModelId = claim.ModelId;
                    claimDetails.DealerId = claim.ClaimSubmittedDealerId;
                    claimDetails.PolicyId = claim.PolicyId;
                    response = claimDetails;
                }
                else
                {
                    //reading claim item information
                    List<ClaimSubmissionItem> claimItemList = session.Query<ClaimSubmissionItem>()
                        .Where(a => a.ClaimSubmissionId == claimId).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>();
                    int claimItemseq = 0;
                    foreach (ClaimSubmissionItem claimItem in claimItemList)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            unitPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claimSubmission.ClaimCurrencyId,
                            claimSubmission.CurrencyPeriodId),
                            goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId)
                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claimSubmission.ClaimNumber;
                    claimDetails.DealerName = cem.GetDealerNameById(claimSubmission.ClaimSubmittedDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claimSubmission.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claimSubmission.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claimSubmission.TotalClaimAmount,
                        claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId);
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claimSubmission.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claimSubmission.PolicyCountryId);
                    //colmplaint
                    claimDetails.Complaint = new Complaint();
                    claimDetails.Complaint.customer = claimSubmission.CustomerComplaint;
                    claimDetails.Complaint.dealer = claimSubmission.DealerComment;
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claimSubmission.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claimSubmission.CommodityCategoryId;
                    claimDetails.MakeId = claimSubmission.MakeId;
                    claimDetails.ModelId = claimSubmission.ModelId;
                    claimDetails.DealerId = claimSubmission.ClaimSubmittedDealerId;
                    //claimDetails.PolicyId = claimSubmission.PolicyId;
                    response = claimDetails;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetDealerDiscountByClaimDetails(DealerDiscountForClaimRequestDto dealerDiscountRequestData)
        {
            DealerDiscountByClaimDetailsResponseDto response = new DealerDiscountByClaimDetailsResponseDto();
            response.Status = "ok";
            try
            {
                #region validation
                if (dealerDiscountRequestData.policyId == Guid.Empty && dealerDiscountRequestData.claimId == Guid.Empty)
                {
                    response.Status = "Please select a policy or claim first.";
                    return response;
                }
                #endregion

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                List<DealerDiscount> dealerDiscounts = new List<DealerDiscount>();
                if (dealerDiscountRequestData.type.ToLower() == "l")
                {
                    dealerDiscounts = session.Query<DealerDiscount>()
                                     .Where(a => a.IsApplicable == true && a.CountryId == dealerDiscountRequestData.countryId &&
                                         a.DealerId == dealerDiscountRequestData.dealerId && a.DiscuntSchemeId == dealerDiscountRequestData.schemeId &&
                                         a.PartOrLabour.ToLower() == "l").ToList();
                }
                else
                {
                    dealerDiscounts = session.Query<DealerDiscount>()
                  .Where(a => a.IsApplicable == true && a.CountryId == dealerDiscountRequestData.countryId &&
                      a.DealerId == dealerDiscountRequestData.dealerId && a.DiscuntSchemeId == dealerDiscountRequestData.schemeId &&
                      a.MakeId == dealerDiscountRequestData.makeId && a.PartOrLabour.ToLower() == "p").ToList();
                }

                if (!dealerDiscounts.Any())
                {
                    response.Status = "No dealer discount found.";
                    return response;
                }

                string discountScheme = commonEm.GetDiscountSchemeCodeById(dealerDiscountRequestData.schemeId);
                if (discountScheme == "POLICY")
                {
                    var policySoldDate = DateTime.MinValue;
                    if (dealerDiscountRequestData.policyId != Guid.Empty)
                    {
                        PolicyBundle policyB =
                            session.Query<PolicyBundle>()
                                .FirstOrDefault(a => a.Id == dealerDiscountRequestData.policyId);
                        if (policyB != null)
                        {
                            policySoldDate = policyB.PolicySoldDate;
                        }
                        else
                        {
                            Policy policy =
                                session.Query<Policy>().FirstOrDefault(a => a.Id == dealerDiscountRequestData.policyId);
                            if (policy != null)
                            {
                                policySoldDate = policy.PolicySoldDate;
                            }
                        }
                    }
                    else
                    {
                        Claim claim =
                            session.Query<Claim>().FirstOrDefault(a => a.Id == dealerDiscountRequestData.claimId);
                        PolicyBundle policyB =
                           session.Query<PolicyBundle>()
                               .FirstOrDefault(a => a.Id == claim.PolicyId);
                        if (policyB != null)
                        {
                            policySoldDate = policyB.PolicySoldDate;
                        }
                        else
                        {
                            Policy policy =
                                session.Query<Policy>().FirstOrDefault(a => a.Id == claim.PolicyId);
                            if (policy != null)
                            {
                                policySoldDate = policy.PolicySoldDate;
                            }
                        }
                    }


                    if (policySoldDate != DateTime.MinValue)
                    {
                        var dealerDiscount =
                            dealerDiscounts
                             .OrderByDescending(o => o.EntryDate)
                             .FirstOrDefault(a => a.StartDate <= policySoldDate && a.EndDate >= policySoldDate);
                        if (dealerDiscount != null)
                        {
                            response = new DealerDiscountByClaimDetailsResponseDto()
                            {
                                Status = "ok",
                                GoodWillRate = dealerDiscount.GoodWillPercentage,
                                DiscountRate = dealerDiscount.DiscountPercentage
                            };
                            return response;
                        }
                        else
                        {
                            response.Status = "No dealer discount found.";
                        }
                    }
                    else
                    {
                        response.Status = "No dealer discount found.";
                    }
                }
                else if (discountScheme == "CLAIM")
                {
                    var claimDate = DateTime.MinValue;

                    Claim claim =
                        session.Query<Claim>().FirstOrDefault(a => a.Id == dealerDiscountRequestData.claimId);
                    if (claim != null)
                    {
                        claimDate = claim.EntryDate;
                    }
                    else
                    {
                        ClaimSubmission claimSub =
                            session.Query<ClaimSubmission>()
                                .FirstOrDefault(a => a.Id == dealerDiscountRequestData.claimId);
                        if (claimSub != null)
                        {
                            claimDate = claimSub.EntryDate;
                        }
                    }


                    if (claimDate != DateTime.MinValue)
                    {
                        var dealerDiscount =
                            dealerDiscounts
                                .OrderByDescending(o=> o.EntryDate)
                                .FirstOrDefault(a => a.StartDate <= claimDate && a.EndDate >= claimDate);
                        if (dealerDiscount != null)
                        {
                            response = new DealerDiscountByClaimDetailsResponseDto()
                            {
                                Status = "ok",
                                GoodWillRate = dealerDiscount.GoodWillPercentage,
                                DiscountRate = dealerDiscount.DiscountPercentage
                            };
                            return response;
                        }
                        else
                        {
                            response.Status = "No dealer discount found.";
                        }
                    }
                    else
                    {
                        response.Status = "No dealer discount found.";
                    }
                }

            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private AddClaimEndorsementItemResponseDto AddNewItemForExistingClaimEndorsement(ClaimEndorsementItemRequestDto claimInfo)
        {
            AddClaimEndorsementItemResponseDto response = new AddClaimEndorsementItemResponseDto();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                ClaimEndorsement claim = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.Id == claimInfo.claimId);
                if (claim != null)
                {
                    claimInfo.dealerId = claim.ClaimSubmittedDealerId;
                    claimInfo.status = "a";
                    ClaimEndorsementItem claimItem = DBDTOTransformer.Instance.ClaimItemRequestToNewClaimEndorsementItem(claimInfo, claim);

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Save(claimItem, claimItem.Id);

                        //claim.TotalClaimAmount += claimItem.TotalPrice;
                        //claim.TotalGrossClaimAmount += claimItem.TotalGrossPrice;
                        //claim.AuthorizedAmount += claimItem.AuthorizedAmount;

                        List<ClaimItem> claimItems = session.Query<ClaimItem>().Where(a => a.ClaimId == claim.Id).ToList();
                        claim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                        claim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                        claim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);

                        session.Update(claim, claim.Id);
                        transaction.Commit();
                    }
                    //setting up response
                    response.Status = "ok";
                    response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimNo = claim.ClaimNumber;
                    response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                    response.ClaimId = claim.Id;
                    response.IsReload = false;

                }
                else
                {
                    response.Status = "Invalid claim selection";
                    return response;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object ProcessClaimEndorsement(ProcessClaimEndorsementRequestDto claimEndorsementProcessRequest)
        {
            AddClaimEndorsementItemResponseDto response = new AddClaimEndorsementItemResponseDto();
            response.Status = "ok";
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimEndorsement claim = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.Id == claimEndorsementProcessRequest.claimId);

                string claimStatusCode = claimEndorsementProcessRequest.status == "A"
                    ? "APP"
                    : (claimEndorsementProcessRequest.status == "R" ? "REJ" : "HOL");

                claim.StatusId = cem.GetClaimStatusIdByCode(claimStatusCode);
                claim.LastUpdatedDate = DateTime.UtcNow;
                claim.IsApproved = claimEndorsementProcessRequest.status == "A" ? true : false;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(claim);
                    transaction.Commit();
                }

                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(claim.AuthorizedAmount * claim.ConversionRate * 100) / 100;
                response.ClaimNo = claim.ClaimNumber;
                response.RequestedAmount = Math.Round(claim.TotalClaimAmount * claim.ConversionRate * 100) / 100;
                response.ClaimId = claim.Id;
                response.IsReload = false;
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static AddClaimEndorsementItemResponseDto SaveTpaEnteredClaimEndorsement(ClaimEndorsementItemRequestDto claimEndorsementInfo)
        {
            var response = new AddClaimEndorsementItemResponseDto();
            response.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Policy policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claimEndorsementInfo.policyId);
                PolicyBundle policyBundle = session.Query<PolicyBundle>().FirstOrDefault(a => a.Id == policy.PolicyBundleId);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimEndorsementInfo.claimId);
                ClaimEndorsement claimEndorsement = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.ClaimId == claimEndorsementInfo.claimId);
                ClaimItem claimItem = session.Query<ClaimItem>().FirstOrDefault(a => a.ClaimId == claimEndorsementInfo.claimId);

                if (policy == null)
                {
                    response.Status = "Invalid policy selection";
                    return response;
                }

                ClaimEndorsement newClaimEndorsement = DBDTOTransformer.Instance.ClaimEndorsementItemRequestToNewClaim(claimEndorsementInfo, policy);
                ClaimEndorsementItem newClaimEndorsementItem = DBDTOTransformer.Instance.ClaimEndorsementItemRequestToNewClaimItem(claimEndorsementInfo, newClaimEndorsement);

                using (ITransaction transaction = session.BeginTransaction())
                {
                    newClaimEndorsement.ClaimNumber = claim.ClaimNumber;
                    newClaimEndorsement.AuthorizedAmount = newClaimEndorsementItem.AuthorizedAmount;
                    newClaimEndorsement.ConversionRate = newClaimEndorsementItem.ConversionRate;
                    newClaimEndorsement.CurrencyPeriodId = newClaimEndorsementItem.CurrencyPeriodId;
                    newClaimEndorsement.TotalClaimAmount = newClaimEndorsementItem.TotalPrice;
                    newClaimEndorsement.TotalGrossClaimAmount = newClaimEndorsementItem.TotalGrossPrice;
                    session.Clear();
                    if (claimEndorsement == null)
                    {
                        session.Save(newClaimEndorsement, newClaimEndorsement.Id);
                        session.Save(newClaimEndorsementItem, newClaimEndorsementItem.Id);
                    }
                    else
                    {
                        session.Update(newClaimEndorsement);
                        session.Save(newClaimEndorsementItem, newClaimEndorsementItem.Id);

                    }

                    transaction.Commit();
                }
                //setting up response
                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(newClaimEndorsement.AuthorizedAmount * newClaimEndorsement.ConversionRate * 100) / 100;
                response.ClaimNo = newClaimEndorsement.ClaimNumber;
                response.RequestedAmount = Math.Round(newClaimEndorsement.TotalClaimAmount * newClaimEndorsement.ConversionRate * 100) / 100;
                response.ClaimId = newClaimEndorsement.Id;

            }
            catch (Exception ex)
            {
                response.Status = "Error Occured while saving new claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;
        }

        internal object SaveClaimEndorsement(SaveClaimEndorsementProcessRequestDto SaveClaimEndorsementProcessReques)
        {
            AddClaimEndorsementItemResponseDto response = new AddClaimEndorsementItemResponseDto();

            try
            {

                #region "validate request"

                if (!IsGuid(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId.ToString()) && !IsGuid(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId.ToString()))
                {
                    response.Status = "Invalid claim or policy selection.";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager commonEm = new CommonEntityManager();
                string claimEndorsement = string.Empty;
                var claimEndorsementItem = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.ClaimId == SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId);



                if (claimEndorsementItem == null)
                {
                    claimEndorsement = IsGuid(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId.ToString()) ? "New" : "";
                }
                else if (claimEndorsementItem.IsRejected == false && claimEndorsementItem.EndorsementIsApproved == false)
                {
                    claimEndorsement = IsGuid(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId.ToString()) ? "New" : "";
                }
                else
                {
                    claimEndorsement = "Existing";
                }


                if (string.IsNullOrEmpty(claimEndorsement))
                {
                    response.Status = "Invalid claim or policy selection";
                    return response;
                }

                #endregion

                foreach (var ClaimList in SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.ClaimList)
                {
                    var claimEndorsementInfo = new ClaimEndorsementItemRequestDto()
                    {

                        authorizedAmt = ClaimList.authorizedAmt,
                        claimId = SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId,
                        comment = ClaimList.comment,
                        dealerId = SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.dealerId,
                        discountAmount = ClaimList.discountAmount,
                        discountRate = ClaimList.discountRate,
                        entryBy = SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.loggedInUserId,
                        faultId = ClaimList.faultId,
                        goodWillAmount = ClaimList.goodWillAmount,
                        goodWillRate = ClaimList.goodWillRate,
                        id = ClaimList.id,
                        isDiscountPercentage = ClaimList.isDiscountPercentage,
                        isGoodWillPercentage = ClaimList.isGoodWillPercentage,
                        itemName = ClaimList.itemName,
                        itemNumber = ClaimList.itemNumber,
                        itemType = ClaimList.itemType,
                        parentId = 0,
                        partAreaId = ClaimList.partAreaId,
                        partId = ClaimList.partId,
                        policyId = commonEm.GetPolicyIdByClaimId(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.claimId),
                        qty = ClaimList.partQty,
                        remarks = ClaimList.remark,
                        serverId = ClaimList.serverId ?? Guid.Empty,
                        status = ClaimList.status,
                        totalGrossPrice = ClaimList.totalGrossPrice,
                        totalPrice = ClaimList.totalPrice,
                        unitPrice = ClaimList.unitPrice,
                        discountType = "",
                        goodWillType = "",
                        hourlyRate = Convert.ToDecimal("0.0"),
                        hours = Convert.ToDecimal("0.0"),
                    };

                    if (claimEndorsement == "New")
                    {
                        claimEndorsementInfo.policyId = SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.policyId;
                        claimEndorsementInfo.dealerId = SaveClaimEndorsementProcessReques.ClaimEndorsementDetails.dealerId;
                        claimEndorsementInfo.status = "a";
                        response = SaveTpaEnteredClaimEndorsementList(claimEndorsementInfo);
                        response.IsReload = true;
                    }
                    else
                    {

                        response.Status = "Already Inserted Claim Endorsement Details ";
                        //claimEndorsementInfo.serverId = Guid.Empty;
                        //response = AddNewItemForExistingClaimEndorsement(claimEndorsementInfo);
                        //response.IsReload = true;
                    }
                }


                response = claimStatusUpdate(SaveClaimEndorsementProcessReques.ClaimEndorsementDetails);


                return response;
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured while saving new claim endorsement.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }
            return response;
        }

        private AddClaimEndorsementItemResponseDto SaveTpaEnteredClaimEndorsementList(ClaimEndorsementItemRequestDto claimEndorsementInfo)
        {
            var response = new AddClaimEndorsementItemResponseDto();
            response.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Policy policy = session.Query<Policy>().FirstOrDefault(a => a.Id == claimEndorsementInfo.policyId);
                PolicyBundle policyBundle = session.Query<PolicyBundle>().FirstOrDefault(a => a.Id == policy.PolicyBundleId);
                ClaimEndorsement claimEndorsement = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.ClaimId == claimEndorsementInfo.claimId);
                ClaimEndorsement newClaim = DBDTOTransformer.Instance.ClaimEndorsementToTpaClaim(claimEndorsementInfo, policy);
                ClaimEndorsementItem claimItems = DBDTOTransformer.Instance.ClaimEndorsementItemsToTpaClaimItems(claimEndorsementInfo, newClaim);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimEndorsementInfo.claimId);

                ////update claim in
                newClaim.AuthorizedAmount = claimItems.AuthorizedAmount;
                newClaim.TotalClaimAmount = claimItems.TotalPrice;
                newClaim.TotalGrossClaimAmount = claimItems.TotalGrossPrice;



                using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                {

                    var countryCode = new CommonEntityManager().GetCountryCodeById(newClaim.PolicyCountryId);
                    newClaim.ClaimNumber = claim.ClaimNumber;

                    session.Clear();
                    if (claimEndorsement == null)
                    {
                        session.Save(newClaim, newClaim.Id);
                        session.Evict(claimItems);
                        session.Save(claimItems, claimItems.Id);
                    }
                    else
                    {
                        // session.Update(newClaim);
                        session.Evict(claimItems);
                        session.Save(claimItems, claimItems.Id);
                    }

                    transaction.Commit();
                }
                //setting up response
                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(newClaim.AuthorizedAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimNo = newClaim.ClaimNumber;
                response.RequestedAmount = Math.Round(newClaim.TotalClaimAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimId = newClaim.Id;

            }

            catch (Exception ex)
            {
                response.Status = "Error Occured while saving new claim.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " +
                             ex.InnerException);
            }

            return response;

        }

        private AddClaimEndorsementItemResponseDto claimStatusUpdate(ClaimEndorsement_ claimEndorsement_)
        {
            AddClaimEndorsementItemResponseDto response = new AddClaimEndorsementItemResponseDto();
            response.Status = "ok";
            try
            {
                CommonEntityManager cem = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimEndorsement claimEndorsement = session.Query<ClaimEndorsement>().FirstOrDefault(a => a.ClaimId == claimEndorsement_.claimId);

                string claimStatusCode = claimEndorsement_.status == "A"
                    ? "APP"
                    : (claimEndorsement_.status == "R" ? "REJ" : "HOL");

                Guid newstatusId = cem.GetClaimStatusIdByCode(claimStatusCode);

                claimEndorsement.StatusId = newstatusId;
                claimEndorsement.Conclution = claimEndorsement_.assesmentDetails.conclution;
                claimEndorsement.EngineerComment = claimEndorsement_.assesmentDetails.engineer;
                claimEndorsement.LastUpdatedBy = claimEndorsement_.loggedInUserId;
                claimEndorsement.LastUpdatedDate = DateTime.UtcNow;
                claimEndorsement.IsApproved = claimEndorsement_.status == "A" ? true : false;
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(claimEndorsement);
                    transaction.Commit();
                }

                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(claimEndorsement.AuthorizedAmount * claimEndorsement.ConversionRate * 100) / 100;
                response.ClaimNo = claimEndorsement.ClaimNumber;
                response.RequestedAmount = Math.Round(claimEndorsement.TotalClaimAmount * claimEndorsement.ConversionRate * 100) / 100;
                response.ClaimId = claimEndorsement.Id;
                response.IsReload = false;
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private AddClaimEndorsementItemResponseDto SaveClaimEndorsementWithLabour(ClaimEndorsementItemRequestDto claimEndorsementLInfo)
        {
            var response = new AddClaimEndorsementItemResponseDto();

            try
            {
                ISession session = EntitySessionManager.GetSession();
                Claim claimSubmission = session.Query<Claim>().FirstOrDefault(a => a.Id == claimEndorsementLInfo.claimId);

                ClaimEndorsement newClaim = DBDTOTransformer.Instance.ClaimSubmissionToClaimEndorsement(claimSubmission);
                List<ClaimEndorsementItem> newClaimItems = DBDTOTransformer.Instance.ClaimSubmissionItemsToClaimEndorsementItem(claimSubmission, claimEndorsementLInfo);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimEndorsementLInfo.claimId);

                using (ITransaction transaction = session.BeginTransaction())
                {

                    newClaim.ClaimNumber = claim.ClaimNumber;

                    session.Save(newClaim, newClaim.Id);

                    foreach (ClaimEndorsementItem newClaimItem in newClaimItems)
                    {
                        session.Clear();
                        newClaimItem.ClaimId = claimSubmission.Id;
                        session.Evict(newClaimItem);
                        session.Save(newClaimItem, newClaimItem.Id);
                    }

                    transaction.Commit();
                }
                //setting up response
                response.Status = "ok";
                response.AuthorizedAmount = Math.Round(newClaim.AuthorizedAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimNo = newClaim.ClaimNumber;
                response.RequestedAmount = Math.Round(newClaim.TotalClaimAmount * newClaim.ConversionRate * 100) / 100;
                response.ClaimId = newClaim.Id;
                response.IsReload = true;

            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }


        internal static object GetAllClaimForClaimInquirySearchGrid(ClaimInquirySearchGridRequestDto ClaimInquirySearchGridRequestDto)
        {
            if (ClaimInquirySearchGridRequestDto != null && ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid != null)
            {
                ISession session = EntitySessionManager.GetSession();
                // List<Guid> claimBordxDetail = session.Query<ClaimBordxDetail>().Select(a => a.ClaimId).ToList();
                // Expression<Func<Claim, bool>> filterPolicy = PredicateBuilder.True<Claim>();

                Expression<Func<Claim, bool>> filterClaim = PredicateBuilder.True<Claim>();
                Expression<Func<ClaimSubmission, bool>> filterClaimSub = PredicateBuilder.True<ClaimSubmission>();
                //filterClaim = filterClaim.And(a => claimBordxDetail.Contains(a.Id));
                //IQueryable<ClaimBordxDetail> claimBordxDetail;


                //var filteredClaim = session.Query<Claim>().Where(filterClaim);

                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
              //  filterClaimSub = filterClaimSub.And(a => string.IsNullOrEmpty(a.ClaimNumber));

                if (!String.IsNullOrEmpty(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.claimNumber))
                {
                    filterClaim = filterClaim.And(a => a.ClaimNumber.ToLower().Contains(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.claimNumber.ToLower()));

                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.status.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.StatusId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.status);
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.dealerId.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.ClaimSubmittedDealerId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.dealerId);
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.customerId.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.CustomerId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.customerId);
                }
                if (!String.IsNullOrEmpty(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.policyId))
                {
                    filterClaim = filterClaim.And(a => a.PolicyNumber.ToLower().Contains(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.policyId.ToLower()));
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.country.ToString()))
                {
                    filterClaim = filterClaim.And(a => a.ClaimCountryId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.country);
                }

                if (!String.IsNullOrEmpty(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.claimNumber))
                {
                    filterClaimSub = filterClaimSub.And(a => a.ClaimNumber.ToLower().Contains(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.claimNumber.ToLower()));

                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.status.ToString()))
                {
                    filterClaimSub = filterClaimSub.And(a => a.StatusId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.status);
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.dealerId.ToString()))
                {
                    filterClaimSub = filterClaimSub.And(a => a.ClaimSubmittedDealerId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.dealerId);
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.customerId.ToString()))
                {
                    filterClaimSub = filterClaimSub.And(a => a.CustomerId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.customerId);
                }
                if (!String.IsNullOrEmpty(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.policyId))
                {
                    filterClaimSub = filterClaimSub.And(a => a.PolicyNumber.ToLower().Contains(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.policyId.ToLower()));
                }
                if (IsGuid(ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.country.ToString()))
                {
                    filterClaimSub = filterClaimSub.And(a => a.ClaimCountryId == ClaimInquirySearchGridRequestDto.claimInquirySearchGridSearchCriterias.country);
                }

                //ISession session = EntitySessionManager.GetSession();
                var filteredClaim = session.Query<Claim>().Where(filterClaim).Select(z=>new {
                    z.Id,
                    z.PolicyId,
                    z.CommodityTypeId,
                    z.PolicyNumber,
                    ClaimNumber = z.ClaimNumber,
                    z.ClaimSubmittedDealerId,
                    z.MakeId,
                    z.ModelId,
                    z.TotalClaimAmount,
                    z.ClaimCurrencyId,
                    z.CurrencyPeriodId,
                    z.EntryDate

                }).ToArray();

                filterClaimSub = filterClaimSub.And(b => b.ClaimNumber ==null || b.ClaimNumber==String.Empty || b.ClaimNumber.Trim().Length==0 );
                var filteredClaimSub = session.Query<ClaimSubmission>().Where(filterClaimSub).Select(z => new {
                    z.Id,
                    z.PolicyId,
                    z.CommodityTypeId,
                    z.PolicyNumber,
                    ClaimNumber = z.Wip,
                    z.ClaimSubmittedDealerId,
                    z.MakeId,
                    z.ModelId,
                    z.TotalClaimAmount,
                    z.ClaimCurrencyId,
                    z.CurrencyPeriodId,
                    z.EntryDate
                }).ToArray();
                //.Where(b => string.IsNullOrEmpty(b.ClaimNumber)).ToArray();

                //filteredClaim = filteredClaim.Where(b => claimBordxDetail.Any(c => c == b.Id && c.IsBatching == true));


                //var claims = filteredClaim
                //  .Select(z => new
                //  {
                //      Id = z.Id,
                //      PolicyId = z.PolicyId,
                //      CommodityType = cem.GetCommodityTypeNameById(z.CommodityTypeId),
                //      PolicyNumber = z.PolicyNumber,
                //      serialnumber = getSerialNumberByPolicyId(z.PolicyId),
                //      ClaimNumber = z.ClaimNumber,
                //      ClaimDealer = cem.GetDealerNameById(z.ClaimSubmittedDealerId),
                //      Make = cem.GetMakeNameById(z.MakeId),
                //      Model = cem.GetModelNameById(z.ModelId),
                //      ClaimAmount = currencyEm.ConvertFromBaseCurrency(z.TotalClaimAmount, z.ClaimCurrencyId, z.CurrencyPeriodId) +
                //        " " +
                //        cem.GetCurrencyTypeByIdCode(z.ClaimCurrencyId),
                //      Date = z.EntryDate
                //  }).ToArray();



                //var claimSubmissions = filteredClaimSub
                //  .Select(z => new
                //  {
                //      Id = z.Id,
                //      PolicyId = z.PolicyId,
                //      CommodityType = cem.GetCommodityTypeNameById(z.CommodityTypeId),
                //      PolicyNumber = z.PolicyNumber,
                //      serialnumber = getSerialNumberByPolicyId(z.PolicyId),
                //      ClaimNumber = z.Wip,
                //      ClaimDealer = cem.GetDealerNameById(z.ClaimSubmittedDealerId),
                //      Make = cem.GetMakeNameById(z.MakeId),
                //      Model = cem.GetModelNameById(z.ModelId),
                //      ClaimAmount = currencyEm.ConvertFromBaseCurrency(z.TotalClaimAmount, z.ClaimCurrencyId, z.CurrencyPeriodId) +
                //        " " +
                //        cem.GetCurrencyTypeByIdCode(z.ClaimCurrencyId),
                //      Date = z.EntryDate
                //  }).ToArray().Where(b => string.IsNullOrEmpty(b.ClaimNumber)).ToArray(); ;



                //long TotalRecords = filteredClaim.Count()+ filteredClaimSub.Count();
                //var customerGridDetailsFilterd = filteredClaim.Skip((ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageNumber - 1) * ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageSize)
                //.Take(ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageSize)
                //.Select(a => new
                //{
                //    Id = a.Id,
                //    PolicyId = a.PolicyId,
                //    CommodityType = cem.GetCommodityTypeNameById(a.CommodityTypeId),
                //    PolicyNumber = a.PolicyNumber,
                //    serialnumber = getSerialNumberByPolicyId(a.PolicyId),
                //    ClaimNumber = a.ClaimNumber,
                //    ClaimDealer = cem.GetDealerNameById(a.ClaimSubmittedDealerId),
                //    Make = cem.GetMakeNameById(a.MakeId),
                //    Model = cem.GetModelNameById(a.ModelId),
                //    ClaimAmount = currencyEm.ConvertFromBaseCurrency(a.TotalClaimAmount, a.ClaimCurrencyId, a.CurrencyPeriodId) +
                //        " " +
                //        cem.GetCurrencyTypeByIdCode(a.ClaimCurrencyId),
                //    Date = a.EntryDate

                //})
                //.ToArray();



                var unionResult = filteredClaim.Union(filteredClaimSub).OrderByDescending(a => a.EntryDate);
                var totalRecords = unionResult.Count();
                var claimDetailsFilterd = unionResult.Skip
                    ((ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageNumber - 1) * ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageSize)
                .Take(ClaimInquirySearchGridRequestDto.paginationOptionsClaimInquirySearchGrid.pageSize).Select(z=>new {
                    Id = z.Id,
                    PolicyId = z.PolicyId,
                    CommodityType = cem.GetCommodityTypeNameById(z.CommodityTypeId),
                    PolicyNumber = z.PolicyNumber,
                    serialnumber = getSerialNumberByPolicyId(z.PolicyId),
                    ClaimNumber = z.ClaimNumber,
                    ClaimDealer = cem.GetDealerNameById(z.ClaimSubmittedDealerId),
                    Make = cem.GetMakeNameById(z.MakeId),
                    Model = cem.GetModelNameById(z.ModelId),
                    ClaimAmount = currencyEm.ConvertFromBaseCurrency(z.TotalClaimAmount, z.ClaimCurrencyId, z.CurrencyPeriodId) +
                            " " +
                            cem.GetCurrencyTypeByIdCode(z.ClaimCurrencyId),
                    Date = z.EntryDate
                });
                var gridResponse = new CommonGridResponseDto()
                {
                    totalRecords = totalRecords,
                    data = claimDetailsFilterd
                };
                var response = new JavaScriptSerializer().Serialize(gridResponse);
                return response;



                //var response = new CommonGridResponseDto()
                //{
                //    totalRecords = TotalRecords,
                //    data = customerGridDetailsFilterd
                //};
                //return new JavaScriptSerializer().Serialize(response);

            }
            else
            {
                return null;
            }
        }

        private static object getSerialNumberByPolicyId(Guid PolicyId)
        {
            ISession session = EntitySessionManager.GetSession();

            Policy policy = session.Query<Policy>().Where(a => a.Id == PolicyId).FirstOrDefault();
            if (policy == null)
            {
                return "Not Found";
            }

            CommodityType commodity = session.Query<CommodityType>().Where(a => a.CommodityTypeId == policy.CommodityTypeId)
                .FirstOrDefault();
            if (commodity == null)
            {
                return "Not Found";
            }

            if (commodity.CommodityCode == "A")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (commodity.CommodityCode == "B")
            {
                Guid vehicleId = Guid.Empty;
                if (session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault() != null)
                {
                    vehicleId = session.Query<VehiclePolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().VehicleId;
                }
                else
                {
                    return "VIN Not found";
                }
                if (vehicleId != Guid.Empty &&
                    session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).Count() > 0)
                {
                    return session.Query<VehicleDetails>().Where(a => a.Id == vehicleId).FirstOrDefault().VINNo;
                }
                else
                {
                    return "VIN Not found";
                }
            }
            else if (commodity.CommodityCode == "E")
            {
                Guid BnWId = session.Query<BAndWPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().BAndWId;
                if (BnWId != Guid.Empty)
                {
                    return session.Query<BrownAndWhiteDetails>().Where(a => a.Id == BnWId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (commodity.CommodityCode == "Y")
            {
                Guid YgId = session.Query<YellowGoodPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().YellowGoodId;
                if (YgId != Guid.Empty)
                {
                    return session.Query<YellowGoodDetails>().Where(a => a.Id == YgId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else if (commodity.CommodityCode == "O")
            {
                Guid OId = session.Query<OtherItemPolicy>().Where(a => a.PolicyId == PolicyId).FirstOrDefault().OtherItemId;
                if (OId != Guid.Empty)
                {
                    return session.Query<OtherItemDetails>().Where(a => a.Id == OId).FirstOrDefault().SerialNo;
                }
                else
                {
                    return "N/A";
                }
            }
            else
            {
                return "N/A";
            }
        }

        internal object GetClaimDetailsforInquiryByClaimId(Guid claimId)
        {
            object response = null;
            try
            {
                if (!IsGuid(claimId.ToString()))
                {
                    return response;
                }

                ClaimforInquiryRequestDetailsResponseDto claimDetails = new ClaimforInquiryRequestDetailsResponseDto();

                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                ClaimSubmission claimSubmission = session
                    .Query<ClaimSubmission>().FirstOrDefault(a => a.Id == claimId);
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);
                ClaimGroupClaim ClaimGroupClaim = session.Query<ClaimGroupClaim>().FirstOrDefault(a => a.ClaimId == claimId);

                if (claimSubmission == null && claim == null)
                {
                    return response;
                }

                bool fromClaim = claim != null;
                if (claim != null)
                {
                    List<ClaimItem> claimItems = session.Query<ClaimItem>()
                       .Where(a => a.ClaimId == claimId).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>();
                    int claimItemseq = 0;
                    foreach (ClaimItem claimItem in claimItems)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            unitPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            authorizedAmount = currencyEm.ConvertFromBaseCurrency(claimItem.AuthorizedAmount, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            faultId = claimItem.FaultId,
                            faultName = cem.GetFaultNameById(claimItem.FaultId),
                            status = claimItem.IsApproved == true ? "Approved" : (claimItem.IsApproved == false ? "Rejected" : ""),
                            statusCode = claimItem.IsApproved == true ? "A" : (claimItem.IsApproved == false ? "R" : ""),
                            discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claim.ClaimCurrencyId,
                        claim.CurrencyPeriodId),
                            comment = claimItem.TpaComment,
                            isDiscountPercentage = claimItem.IsDiscountPercentage,
                            isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                            totalGrossPrice = currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate : currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, claim.ClaimCurrencyId,
                                    claim.CurrencyPeriodId),
                            goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate : currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, claim.ClaimCurrencyId,
                            claim.CurrencyPeriodId),
                            parentId = claimItem.ParentId,
                            discountSchemeCode = claimItem.DiscountSchemeCode,
                            discountSchemeId = claimItem.DiscountSchemeId,
                            rejectionTypeId = claimItem.RejectionTypeId

                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claim.ClaimNumber;
                    claimDetails.DealerName = cem.GetDealerNameById(claim.ClaimSubmittedDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claim.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claim.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.TotalClaimAmount,
                        claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.AuthorizedClaimAmount = currencyEm.ConvertFromBaseCurrency(claim.AuthorizedAmount,
                        claim.ClaimCurrencyId, claim.CurrencyPeriodId);
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claim.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claim.PolicyCountryId);
                    claimDetails.PaidAmount = claim.PaidAmount;
                    claimDetails.EntryDate = claim.EntryDate.ToString("dd-MMM-yyyy");
                    claimDetails.ApprovedDate = claim.ApprovedDate.ToString("dd-MMM-yyyy");
                    claimDetails.ApprovedBy = cem.GetUserNameById(claim.ApprovedBy);

                    //colmplaint
                    claimDetails.Complaint = new Complaint
                    {
                        customer = claim.CustomerComplaint,
                        dealer = claim.DealerComment,
                        conclution = claim.Conclution,
                        engineer = claim.EngineerComment
                    };
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claim.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claim.CommodityCategoryId;
                    claimDetails.MakeId = claim.MakeId;
                    claimDetails.ModelId = claim.ModelId;
                    claimDetails.DealerId = claim.ClaimSubmittedDealerId;
                    claimDetails.PolicyId = claim.PolicyId;
                    claimDetails.PolicyDealerId = claim.PolicyDealerId;
                    claimDetails.PolicyCountryId = claim.PolicyCountryId;


                    ClaimInvoiceEntryClaim claimInvoiceEntryClaim = session.
                        Query<ClaimInvoiceEntryClaim>().FirstOrDefault(a => a.ClaimId == claimId);
                    //ClaimBatchGroup claimBatchGroup = session.Query<ClaimBatchGroup>().FirstOrDefault(a => a.Id == ClaimGroupClaim.ClaimGroupId);
                    //ClaimBordxDetail claimBordxDetail = session.Query<ClaimBordxDetail>().FirstOrDefault(a => a.ClaimId == claimId);

                    // reading Invoice details
                    if (claimInvoiceEntryClaim != null)
                    {
                        ClaimInvoiceEntry claimInvoiceEntry = session.Query<ClaimInvoiceEntry>().FirstOrDefault(a => a.Id == claimInvoiceEntryClaim.ClaimInvoiceEntryId);

                        claimDetails.InvoiceDate = claimInvoiceEntry.InvoiceDate.ToString("dd-MMM-yyyy");
                        claimDetails.InvoiceReceivedDate = claimInvoiceEntry.InvoiceReceivedDate.ToString("dd-MMM-yyyy");
                        claimDetails.InvoiceNumber = claimInvoiceEntry.InvoiceNumber;
                    }
                    else
                    {
                        claimDetails.InvoceWarning = "Data Not Found.";
                    }
                    //if (claimBatchGroup != null)
                    //{
                    //    ClaimBatch claimBatch = session.Query<ClaimBatch>().FirstOrDefault(a => a.Id == claimBatchGroup.ClaimBatchId);
                    //    // reading Claim Batch Details
                    //    claimDetails.GroupName = claimBatchGroup.GroupName;
                    //    claimDetails.BatchEntryDate = claimBatch.EntryDate.ToString("dd-MMM-yyyy");
                    //    claimDetails.BatchNumber = claimBatch.BatchNumber;
                    //}
                    //else
                    //{
                    //    claimDetails.BatchGroupWarning = "Data Not Found.";
                    //}

                    //if (claimBordxDetail != null)
                    //{
                    //    ClaimBordx claimBordx = session.Query<ClaimBordx>().FirstOrDefault(a => a.Id == claimBordxDetail.ClaimBordxId);
                    //    // reading Claim Bordx Details
                    //    claimDetails.Insurer = cem.GetInsurerNameById(claimBordx.Insurer);
                    //    claimDetails.Reinsurer = cem.GetReinsurerNameById(claimBordx.Reinsurer);
                    //    claimDetails.Bordxmonth = claimBordx.Bordxmonth;
                    //    claimDetails.BordxYear = claimBordx.BordxYear;
                    //    claimDetails.BordxNumber = claimBordx.BordxNumber;
                    //}
                    //else
                    //{
                    //    claimDetails.BordxWarning = "Data Not Found.";
                    //}
                    response = claimDetails;
                }
                else {


                    List<ClaimSubmissionItem> claimItems = session.Query<ClaimSubmissionItem>()
                       .Where(a => a.ClaimSubmissionId == claimId).ToList();
                    claimDetails.ClaimItemList = new List<ClaimItemList>();
                    int claimItemseq = 0;
                    foreach (ClaimSubmissionItem claimItem in claimItems)
                    {
                        claimItemseq++;
                        ClaimItemList claimItemObj = new ClaimItemList()
                        {
                            partAreaId = cem.GetPartAreaIdByPartId(claimItem.PartId),
                            serverId = claimItem.Id,
                            partId = claimItem.PartId,
                            id = claimItemseq,
                            itemName = claimItem.ItemName,
                            itemNumber = claimItem.ItemCode,
                            itemType = cem.GetClaimTypeCodeById(claimItem.ClaimItemTypeId),
                            qty = claimItem.Quantity,
                            remarks = claimItem.Remark,
                            totalPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.TotalPrice, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            unitPrice =
                                currencyEm.ConvertFromBaseCurrency(claimItem.UnitPrice, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            authorizedAmount = 0,
                            faultId = Guid.Empty,
                            faultName = " ",
                            status = "Pending",
                            statusCode = "",
                            discountAmount = currencyEm.ConvertFromBaseCurrency(claimItem.DiscountAmount, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            goodWillAmount = currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillAmount, claimSubmission.ClaimCurrencyId,
                        claimSubmission.CurrencyPeriodId),
                            comment = claimItem.Remark,
                            isDiscountPercentage = claimItem.IsDiscountPercentage,
                            isGoodWillPercentage = claimItem.IsGoodWillPercentage,
                            totalGrossPrice = currencyEm.ConvertFromBaseCurrency((claimItem.UnitPrice * claimItem.Quantity), claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            discountRate = claimItem.IsDiscountPercentage ? claimItem.DiscountRate : currencyEm.ConvertFromBaseCurrency(claimItem.DiscountRate, claimSubmission.ClaimCurrencyId,
                                    claimSubmission.CurrencyPeriodId),
                            goodWillRate = claimItem.IsGoodWillPercentage ? claimItem.GoodWillRate : currencyEm.ConvertFromBaseCurrency(claimItem.GoodWillRate, claimSubmission.ClaimCurrencyId,
                            claimSubmission.CurrencyPeriodId),
                            parentId = claimItem.ParentId,
                            discountSchemeCode = "",
                            discountSchemeId = Guid.Empty,
                            rejectionTypeId = Guid.Empty

                        };
                        claimDetails.ClaimItemList.Add(claimItemObj);
                    }
                    //reading attachments
                    claimDetails.Attachments = new AttachmentEntityManager().GetAttachmentsByClaimRequestId(claimId, fromClaim);
                    //other details
                    claimDetails.ClaimNumber = claimSubmission.Wip;
                    claimDetails.DealerName = cem.GetDealerNameById(claimSubmission.ClaimSubmittedDealerId);
                    claimDetails.Id = claimId;
                    claimDetails.PolicyNumber = claimSubmission.PolicyNumber;
                    claimDetails.RequestedUser = cem.GetUserNameById(claimSubmission.ClaimSubmittedBy);
                    claimDetails.TotalClaimAmount = currencyEm.ConvertFromBaseCurrency(claimSubmission.TotalClaimAmount,
                        claimSubmission.ClaimCurrencyId, claimSubmission.CurrencyPeriodId);
                    claimDetails.AuthorizedClaimAmount = 0;
                    claimDetails.CurrencyCode = cem.GetCurrencyTypeByIdCode(claimSubmission.ClaimCurrencyId);
                    claimDetails.Country = cem.GetCountryNameById(claimSubmission.PolicyCountryId);
                    claimDetails.PaidAmount = 0;
                    claimDetails.EntryDate = claimSubmission.EntryDate.ToString("dd-MMM-yyyy");
                    claimDetails.ApprovedDate = "";
                    claimDetails.ApprovedBy = "";

                    //colmplaint
                    claimDetails.Complaint = new Complaint
                    {
                        customer = claimSubmission.CustomerComplaint,
                        dealer = claimSubmission.DealerComment,
                        conclution = "",
                        engineer = ""
                    };
                    //reading service history details
                    claimDetails.ServiceHistory = GetAllServiceHistoryByPolicyId(claimSubmission.PolicyId);
                    //essential data
                    claimDetails.CommodityCategoryId = claimSubmission.CommodityCategoryId;
                    claimDetails.MakeId = claimSubmission.MakeId;
                    claimDetails.ModelId = claimSubmission.ModelId;
                    claimDetails.DealerId = claimSubmission.ClaimSubmittedDealerId;
                    claimDetails.PolicyId = claimSubmission.PolicyId;
                    claimDetails.PolicyDealerId = claimSubmission.PolicyDealerId;
                    claimDetails.PolicyCountryId = claimSubmission.PolicyCountryId;




                    claimDetails.InvoceWarning = "Data Not Found.";


                    response = claimDetails;

                }




            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetExcitingClaimEndorsementDetails(Guid claimId)
        {
            object response = null;
            if (!IsGuid(claimId.ToString()))
            {
                return response;
            }

            ExcitingCliamEndrosmentDataResponseDto CliamEndrosmentData = new ExcitingCliamEndrosmentDataResponseDto();
            CliamEndrosmentData.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimId);

                if (claim != null)
                {
                    List<Claim> claimItems = session.Query<Claim>()
                      .Where(a => a.Id == claim.Id).ToList();
                    CliamEndrosmentData.ExcitingCliamEndrosmentData = new List<ExcitingCliamEndrosment>();

                    foreach (Claim claimItem in claimItems)
                    {
                        ExcitingCliamEndrosment claimItemObj = new ExcitingCliamEndrosment()
                        {

                            ClaimSubmittedBy = cem.GetUserNameById(claimItem.ClaimSubmittedBy),
                            EntryDate = claimItem.EntryDate.ToString("dd-MMM-yyyy"),
                            Status = cem.GetClaimStatusCodeById(claimItem.StatusId),
                            CliamEndrosmentId = claimItem.Id,
                            CliamId = claimItem.Id

                        };
                        CliamEndrosmentData.ExcitingCliamEndrosmentData.Add(claimItemObj);
                    }
                    return response = CliamEndrosmentData;
                }
            }
            catch (Exception ex)
            {
                CliamEndrosmentData.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object GetOldandNewClaimEndorsementDetails(Guid cliamEndrosmentId, Guid cliamId)
        {
            CliamEndrosmentDataNewAndOldResponseDto response = new CliamEndrosmentDataNewAndOldResponseDto();
            response.Status = "ok";
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CommonEntityManager cem = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == cliamId && a.IsEndorsed == true);

                Claim newClaim = null;
                if (claim != null)
                {
                    newClaim = session.Query<Claim>().FirstOrDefault(a => a.Id == claim.NewClaimId);
                }

                if (claim != null && claim.NewClaimId != null)
                {
                    //Guid cliamEndrosmentId = claim.NewClaimId;
                    Guid newClaimId = claim.NewClaimId;

                    response.oldCliamEndrosment = DBDTOTransformer.Instance.OldClaimInquiryEnrolmentDetails(claim, cliamEndrosmentId);
                }

                if (newClaim != null && claim.NewClaimId != null)
                {
                    //  Guid cliamEndrosmentId = claim.NewClaimId;
                    response.newCliamEndrosment = DBDTOTransformer.Instance.NewClaimInquiryEnrolmentDetails(newClaim, cliamEndrosmentId);
                }
            }
            catch (Exception ex)
            {
                response.Status = "Error Occured.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object ValidateClaimIdOnEndorsement(ValidateClaimProcessRequestDto claimIdValidateRequest)
        {
            ValidateClaimProcessResponseDto response = new ValidateClaimProcessResponseDto();
            string status = string.Empty;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Claim claim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimIdValidateRequest.claimId);
                if (claim == null)
                {
                    response.msg = "Selected claim is not eligible to endorse.";
                    return response;
                }


                SystemUser sysUser = session.Query<SystemUser>()
                    .FirstOrDefault(a => a.LoginMapId == claimIdValidateRequest.loggedInUserId);
                if (sysUser == null)
                {
                    response.msg = "You are not allowed to perform this action.";
                    return response;
                }

                UserType userType = session
                    .Query<UserType>().FirstOrDefault(a => a.Id == sysUser.UserTypeId);
                if (userType == null)
                {
                    status = "You are not allowed to perform this action.";
                }
                else if (userType.Code.ToLower() == "iu")
                {
                    status = "ok";
                }
                else
                {
                    status = "You are not allowed to perform this action.";
                }

                if (status == "ok")
                {
                    response.msg = status;
                    response.policyId = claim.PolicyId;
                }
                else
                {
                    response.msg = status;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static object EndorseClaim(ClaimEndorseRequestDto claimEndorseRequest)
        {
            GenericCodeMsgResponse response = new GenericCodeMsgResponse();
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEM = new CurrencyEntityManager();

                try
                {
                    CurrencyEntityManager currencyEm = new CurrencyEntityManager();

                    #region validate
                    if (claimEndorseRequest == null || !IsGuid(claimEndorseRequest.claimId.ToString()))
                    {
                        response.code = "error";
                        response.msg = "Invalid endorsement request";
                        return response;
                    }
                    else
                    {
                        //claim date validation
                        if (claimEndorseRequest.claimDate < SqlDateTime.MinValue.Value)
                        {
                            response.code = "error";
                            response.msg = "Claim date cannot be less than " + SqlDateTime.MinValue.Value.ToString("dd-MMM-yyyy");
                            return response;
                        }
                        //mileafe validation
                        if (claimEndorseRequest.claimMileage == 0)
                        {
                            response.code = "error";
                            response.msg = "Claim mileage should be grater than zero.";
                            return response;
                        }
                        //status validation

                        if (claimEndorseRequest.status.ToUpper() == "NA")
                        {
                            response.code = "error";
                            response.msg = "Please select a claim status.";
                            return response;
                        }
                    }

                    Claim oldClaim = session.Query<Claim>().FirstOrDefault(a => a.Id == claimEndorseRequest.claimId);
                    if (oldClaim == null)
                    {
                        response.code = "error";
                        response.msg = "Invalid claim selection";
                        return response;
                    }
                    #endregion

                    #region creation new claim

                    string claimStatusCode = claimEndorseRequest.status == "A"
                        ? "APP"
                        : claimEndorseRequest.status == "R" ? "REJ" : "HOL";
                    Guid newClaimId = Guid.NewGuid();
                    Claim newClaim = new Claim()
                    {
                        Id = newClaimId,
                        ApprovedBy = claimEndorseRequest.loggedInUserId,
                        ApprovedDate = DateTime.UtcNow,
                        ClaimCountryId = oldClaim.ClaimCountryId,
                        ClaimCurrencyId = oldClaim.ClaimCurrencyId,
                        ClaimDate = claimEndorseRequest.claimDate,
                        ClaimMileageKm = claimEndorseRequest.claimMileage,
                        ClaimSubmittedBy = claimEndorseRequest.loggedInUserId,
                        ClaimSubmittedDealerId = oldClaim.ClaimSubmittedDealerId,
                        CommodityCategoryId = oldClaim.CommodityCategoryId,
                        CommodityTypeId = oldClaim.CommodityTypeId,
                        Conclution = claimEndorseRequest.compalinData.conclution,
                        ConversionRate = oldClaim.ConversionRate,
                        CurrencyPeriodId = oldClaim.CurrencyPeriodId,
                        CustomerComplaint = claimEndorseRequest.compalinData.customer,
                        CustomerId = oldClaim.CustomerId,
                        DealerComment = claimEndorseRequest.compalinData.dealer,
                        EngineerComment = claimEndorseRequest.compalinData.engineer,
                        EntryDate = DateTime.UtcNow,
                        ExamineBy = claimEndorseRequest.loggedInUserId,
                        IsApproved = true,
                        IsBatching = false,
                        IsGoodwill = claimEndorseRequest.isGoodwillClaim,
                        IsEndorsed = false,
                        IsInvoiced = false,
                        GroupId = null,
                        LastUpdatedBy = null,
                        LastUpdatedDate = SqlDateTime.MinValue.Value,
                        MakeId = oldClaim.MakeId,
                        ModelId = oldClaim.ModelId,
                        NewClaimId = Guid.Empty,
                        PaidAmount = 0,
                        PaidComment = string.Empty,
                        PolicyCountryId = oldClaim.PolicyCountryId,
                        PolicyDealerId = oldClaim.PolicyDealerId,
                        PolicyId = oldClaim.PolicyId,
                        PolicyNumber = oldClaim.PolicyNumber,
                        StatusId = new CommonEntityManager().GetClaimStatusIdByCode(claimStatusCode),
                        FailureDate = oldClaim.FailureDate,


                    };
                    #endregion

                    #region claim items creation
                    List<ClaimItem> claimItems = new List<ClaimItem>();
                    foreach (DataTransfer.Requests.ClaimItem claimItemDto in claimEndorseRequest.claimItems)
                    {
                        ClaimItem currentClaimItem;
                        if (claimItemDto.partName.ToUpper() == "SUNDRY")
                        {
                            currentClaimItem = new ClaimItem()
                            {
                                Id = Guid.NewGuid(),
                                AuthorizedAmount =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.authorizedAmount, newClaim.ClaimCurrencyId,
                                          newClaim.CurrencyPeriodId),
                                TotalGrossPrice =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.nettAmount, newClaim.ClaimCurrencyId,
                                          newClaim.CurrencyPeriodId),
                                ParentId = Guid.Empty,
                                DiscountRate = decimal.Zero,
                                GoodWillRate = decimal.Zero,
                                ClaimId = newClaimId,
                                TotalPrice =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.nettAmount, newClaim.ClaimCurrencyId,
                                          newClaim.CurrencyPeriodId),
                                CurrencyPeriodId = newClaim.CurrencyPeriodId,
                                DiscountAmount = decimal.Zero,
                                IsApproved = claimItemDto.itemStatus.ToLower() == "a" ? true : false,
                                DiscountSchemeCode = claimItemDto.partDiscountScheme,
                                RejectionTypeId = claimItemDto.rejectionTypeId,
                                FaultId = null,
                                ConversionRate = oldClaim.ConversionRate,
                                ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("L"),
                                DiscountSchemeId = null,
                                IsDiscountPercentage = false,
                                IsGoodWillPercentage = false,
                                GoodWillAmount = decimal.Zero,
                                TpaComment = claimItemDto.remark,
                                Quantity = decimal.One,
                                UnitPrice = currencyEm.ConvertToBaseCurrency(claimItemDto.nettAmount, newClaim.ClaimCurrencyId,
                                        newClaim.CurrencyPeriodId),
                                Remark = claimItemDto.remark,
                                ItemCode = claimItemDto.partNumber,
                                ItemName = claimItemDto.partName,
                                PartId = null
                            };
                            claimItems.Add(currentClaimItem);
                        }
                        else
                        {
                            //this is a part combind with labour
                            currentClaimItem = new ClaimItem()
                            {
                                Id = Guid.NewGuid(),
                                AuthorizedAmount =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.authorizedAmount, newClaim.ClaimCurrencyId,
                                        newClaim.CurrencyPeriodId),
                                TotalGrossPrice =
                                        currencyEm.ConvertToBaseCurrency((claimItemDto.unitPrice * claimItemDto.partQty),
                                            newClaim.ClaimCurrencyId,
                                        newClaim.CurrencyPeriodId),
                                ParentId = Guid.Empty,
                                DiscountRate =
                                    claimItemDto.discountType != "F"
                                        ? currencyEm.ConvertToBaseCurrency(claimItemDto.discountValue, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId)
                                        : claimItemDto.discountValue,
                                GoodWillRate =
                                    claimItemDto.goodWillType != "F"
                                        ? currencyEm.ConvertToBaseCurrency(claimItemDto.goodWillValue, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId)
                                        : claimItemDto.goodWillValue,
                                ClaimId = newClaimId,
                                TotalPrice =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.nettAmount, newClaim.ClaimCurrencyId,
                                        newClaim.CurrencyPeriodId),
                                CurrencyPeriodId = newClaim.CurrencyPeriodId,
                                DiscountAmount =
                                    currencyEm.ConvertToBaseCurrency(claimItemDto.discountAmount, newClaim.ClaimCurrencyId,
                                    newClaim.CurrencyPeriodId),
                                IsApproved = claimItemDto.itemStatus.ToLower() == "a" ? true : false,
                                DiscountSchemeCode = claimItemDto.partDiscountScheme,
                                RejectionTypeId = claimItemDto.rejectionTypeId,
                                FaultId = claimItemDto.faultId,
                                ConversionRate = oldClaim.ConversionRate,
                                ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("P"),
                                DiscountSchemeId =
                                    claimItemDto.partDiscountScheme == "NA"
                                        ? null
                                        : new CommonEntityManager().GetDealerDiscountSchemeIdByCode(
                                            claimItemDto.partDiscountScheme),
                                IsDiscountPercentage = claimItemDto.discountType != "F",
                                IsGoodWillPercentage = claimItemDto.goodWillType != "F",
                                GoodWillAmount =
                                    currencyEm.ConvertToBaseCurrency(claimItemDto.goodWillAmount, newClaim.ClaimCurrencyId,
                                    newClaim.CurrencyPeriodId),
                                TpaComment = claimItemDto.remark,
                                Quantity = claimItemDto.partQty,
                                UnitPrice = currencyEm.ConvertToBaseCurrency(claimItemDto.unitPrice, newClaim.ClaimCurrencyId,
                                    newClaim.CurrencyPeriodId),
                                Remark = claimItemDto.remark,
                                ItemCode = claimItemDto.partNumber,
                                ItemName = claimItemDto.partName,
                                PartId = claimItemDto.partId
                            };
                            claimItems.Add(currentClaimItem);
                            //get labour charge too
                            if (claimItemDto.l_totalAmount > 0)
                            {
                                var claimItemForLabour = new ClaimItem()
                                {
                                    Id = Guid.NewGuid(),
                                    AuthorizedAmount = decimal.Zero,
                                    TotalGrossPrice =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.l_totalAmount, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId),
                                    ParentId = currentClaimItem.Id,
                                    DiscountRate =
                                        claimItemDto.l_discountType != "F"
                                            ? currencyEm.ConvertToBaseCurrency(claimItemDto.l_discountValue,
                                                newClaim.ClaimCurrencyId,
                                                newClaim.CurrencyPeriodId)
                                            : claimItemDto.discountValue,
                                    GoodWillRate =
                                        claimItemDto.l_goodWillType != "F"
                                            ? currencyEm.ConvertToBaseCurrency(claimItemDto.l_goodWillValue,
                                                newClaim.ClaimCurrencyId,
                                                newClaim.CurrencyPeriodId)
                                            : claimItemDto.goodWillValue,
                                    ClaimId = newClaimId,
                                    TotalPrice =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.l_nettAmount, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId),
                                    CurrencyPeriodId = newClaim.CurrencyPeriodId,
                                    DiscountAmount =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.l_discountAmount, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId),
                                    IsApproved = true,
                                    DiscountSchemeCode = claimItemDto.l_labourDiscountScheme,
                                    RejectionTypeId = null,
                                    FaultId = null,
                                    ConversionRate = oldClaim.ConversionRate,
                                    ClaimItemTypeId = new CommonEntityManager().GetClaimTypeIdByClaimTypeCode("L"),
                                    DiscountSchemeId =
                                        claimItemDto.l_labourDiscountScheme == "NA"
                                            ? null
                                            : new CommonEntityManager().GetDealerDiscountSchemeIdByCode(
                                                claimItemDto.l_labourDiscountScheme),
                                    IsDiscountPercentage = claimItemDto.l_discountType != "F",
                                    IsGoodWillPercentage = claimItemDto.l_goodWillType != "F",
                                    GoodWillAmount =
                                        currencyEm.ConvertToBaseCurrency(claimItemDto.l_goodWillAmount, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId),
                                    TpaComment = claimItemDto.l_description,
                                    Quantity = claimItemDto.l_hours,
                                    UnitPrice =
                                        currencyEm.ConvertToBaseCurrency(
                                            claimItemDto.l_chargeType == "F"
                                                ? claimItemDto.l_nettAmount
                                                : claimItemDto.l_hourlyRate, newClaim.ClaimCurrencyId,
                                            newClaim.CurrencyPeriodId),
                                    Remark = claimItemDto.l_description,
                                    ItemCode = claimItemDto.l_chargeType == "F" ? "Fixed" : "Hourly",
                                    ItemName = "Labour Charge",
                                    PartId = null

                                };
                                claimItems.Add(claimItemForLabour);
                            }
                        }
                    }
                    #endregion

                    #region claim attachment creation
                    List<ClaimAttachment> claimAttachments = claimEndorseRequest.policyDocIds.Select(atachmentId => new ClaimAttachment()
                    {
                        Id = Guid.NewGuid(),
                        ClaimId = newClaimId,
                        UserAttachmentId = atachmentId
                    }).ToList();

                    #endregion

                    using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
                    {
                        //updating existing claim
                        session.Clear();
                        oldClaim.IsEndorsed = true;
                        oldClaim.NewClaimId = newClaimId;
                        session.Update(oldClaim, oldClaim.Id);

                        //saving new claim details
                        newClaim.AuthorizedAmount = claimItems.Sum(a => a.AuthorizedAmount);
                        newClaim.TotalClaimAmount = claimItems.Sum(a => a.TotalPrice);
                        newClaim.TotalGrossClaimAmount = claimItems.Sum(a => a.TotalGrossPrice);
                        var newClaimNumber =
                                session.Query<Claim>().Count(a => a.PolicyCountryId == newClaim.PolicyCountryId) + 1;
                        var countryCode = new CommonEntityManager().GetCountryCodeById(newClaim.PolicyCountryId);
                        newClaim.ClaimNumber = countryCode.ToUpper() + "/" +
                                               Convert.ToString(newClaimNumber)
                                                   .PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0');

                        session.Evict(newClaim);
                        session.Save(newClaim, newClaim.Id);

                        foreach (ClaimItem claimItem in claimItems)
                        {
                            session.Evict(claimItem);
                            session.Save(claimItem, claimItem.Id);
                        }

                        foreach (ClaimAttachment attachment in claimAttachments)
                        {
                            session.Evict(attachment);
                            session.Save(attachment, attachment.Id);
                        }
                        transaction.Commit();
                    }
                    response.code = "success";
                    response.msg = "Claim successfully endorsed. New claim number - " + newClaim.ClaimNumber;
                }
                catch (Exception ex)
                {
                    response.code = "error";
                    response.msg = "Error occured while endorsing the claim.";
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        public static object ValidatePolicyNumberOnClaimSubmission(string policyNumber)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                #region validation
                if (string.IsNullOrEmpty(policyNumber))
                {
                    response.code = "error";
                    response.msg = "Policy Number cannot be empty";
                    return response;
                }
                ISession session = EntitySessionManager.GetSession();
                Policy policy =
                    session.Query<Policy>().FirstOrDefault(a => a.PolicyNo.ToLower() == policyNumber.ToLower());

                if (policy == null || policy.Id == Guid.Empty)
                {
                    response.code = "error";
                    response.msg = "Policy Number not found.";
                    return response;
                }
                #endregion
                var vinSerial = PolicyEntityManager.GetVINSerialNumnerByPolicyId(policy.CommodityTypeId, policy.Id);
                if (string.IsNullOrEmpty(vinSerial))
                {
                    response.code = "error";
                    response.msg = "VIN or Serial Number not found.";
                }
                else
                {
                    response.code = "ok";
                    response.msg = vinSerial;
                    dynamic obj = new ExpandoObject();
                    obj.policyId = policy.Id;
                    obj.commodityCategoryId =
                        new CommonEntityManager().GetCommodityCategoryIdByContractId(policy.ContractId);
                    response.obj = obj;

                }
            }
            catch (Exception ex)
            {
                response.code = "error";
                response.msg = "Error occured while reading policy information.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        internal static string AddNotes(NotesRequestDto noteData)
        {
            string Response = string.Empty;
            try
            {
                //validate Notes details
                if (noteData == null)
                {
                    return "Request data is empty.";
                }

                ISession session = EntitySessionManager.GetSession();
                ClaimNotes cn = new Entities.ClaimNotes();

                //cn.Id = Guid.NewGuid();
                cn.ClaimId = noteData.ClaimId;
                cn.EntryDateTime = DateTime.Today.ToUniversalTime();
                cn.Note = noteData.Note;
                cn.PolicyId = noteData.PolicyId;
                cn.SubmittedUserId = noteData.SubmittedUserId;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(cn);
                    noteData.Id = Guid.NewGuid();
                    transaction.Commit();
                }

                Response = "ok";

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while saving Notes.";
            }
            return Response;
        }


        internal static object GetAllClaimRejectionTypes()
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response = session.Query<ClaimRejectionType>()
                    .Select(a => new
                    {
                        a.Id,
                        a.Description
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);

            }
            return Response;
        }


        internal static object RejectClaimRequest(ClaimRejectionRequestDto claimRequest)
        {
            GenericCodeMsgResponse Response = new GenericCodeMsgResponse();
            Response.code = "ERROR";
            try
            {
                //validation
                if (claimRequest == null || !IsGuid(claimRequest.claimId.ToString()))
                {
                    Response.msg = "Input data is invalid";

                }
                if (!IsGuid(claimRequest.rejectionTypeId.ToString()) || string.IsNullOrEmpty(claimRequest.rejectionComment.Trim()))
                {
                    Response.msg = "Please enter all mandetory fields";
                }
                CommonEntityManager commonEntityManager = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                    .FirstOrDefault(a => a.Id == claimRequest.claimId);
                if (claimSubmission != null)
                {
                    //update claim status and rejection type
                    claimSubmission.RejectionTypeId = claimRequest.rejectionTypeId;
                    claimSubmission.StatusId = commonEntityManager.GetClaimStatusIdByCode("RWP");
                    //add new comment
                    ClaimComment claimComment = new ClaimComment()
                    {
                        ByTPA = true,
                        ClaimId = claimRequest.claimId,
                        Comment = claimRequest.rejectionComment,
                        EntryDateTime = DateTime.UtcNow,
                        Id = Guid.NewGuid(),
                        PolicyId = claimSubmission.PolicyId,
                        Seen = false,
                        SeenDateTime = SqlDateTime.MinValue.Value,
                        SentFrom = claimRequest.loggedInUserId,
                        SentTo = claimSubmission.ClaimSubmittedBy
                    };

                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Evict(claimSubmission);
                        session.Update(claimSubmission, claimSubmission.Id);

                        session.Save(claimComment, claimComment.Id);
                        transaction.Commit();
                    }
                    Response.code = "OK";
                }
                else
                {
                    Claim claim = session.Query<Claim>()
                   .FirstOrDefault(a => a.Id == claimRequest.claimId);
                    if (claim == null)
                    {
                        Response.msg = "Selected claim is invalid";
                    }
                    else
                    {
                        Response.msg = "Please reject this claim from processing screen.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response.msg = "Error occured while rejecting the claim";
            }
            return Response;
        }

        internal static object ViewClaimUpdateStatus(Guid claimId)
        {
            GenericCodeMsgResponse Response = new GenericCodeMsgResponse();
            Response.code = "ERROR";
            try
            {
                //validation
                if (!IsGuid(claimId.ToString()))
                {
                    Response.msg = "Input data is invalid";

                }

                CommonEntityManager commonEntityManager = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                    .FirstOrDefault(a => a.Id == claimId);
                if (claimSubmission != null)
                {
                    //update claim status and rejection type
                    if (claimSubmission.StatusId == commonEntityManager.GetClaimStatusIdByCode("SUB"))
                    {
                        claimSubmission.StatusId = commonEntityManager.GetClaimStatusIdByCode("INP");
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Evict(claimSubmission);
                            session.Update(claimSubmission, claimSubmission.Id);
                            transaction.Commit();
                        }
                    }



                    Response.code = "OK";
                }
                else
                {
                    Claim claim = session.Query<Claim>()
                   .FirstOrDefault(a => a.Id == claimId);
                    if (claim == null)
                    {
                        Response.msg = "Selected claim is invalid";
                    }
                    else
                    {
                        Response.msg = "Please reject this claim from processing screen.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response.msg = "Error occured while rejecting the claim";
            }
            return Response;
        }
        internal static object UpdateClaimUpdateStatus(Guid claimId)
        {
            GenericCodeMsgResponse Response = new GenericCodeMsgResponse();
            Response.code = "ERROR";
            try
            {
                //validation
                if (!IsGuid(claimId.ToString()))
                {
                    Response.msg = "Input data is invalid";

                }

                CommonEntityManager commonEntityManager = new CommonEntityManager();
                ISession session = EntitySessionManager.GetSession();
                ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                    .FirstOrDefault(a => a.Id == claimId);
                if (claimSubmission != null)
                {
                    //update claim status and rejection type
                    if (claimSubmission.StatusId == commonEntityManager.GetClaimStatusIdByCode("INP"))
                    {
                        claimSubmission.StatusId = commonEntityManager.GetClaimStatusIdByCode("SUB");
                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            session.Evict(claimSubmission);
                            session.Update(claimSubmission, claimSubmission.Id);
                            transaction.Commit();
                        }
                    }



                    Response.code = "OK";
                }
                else
                {
                    Claim claim = session.Query<Claim>()
                   .FirstOrDefault(a => a.Id == claimId);
                    if (claim == null)
                    {
                        Response.msg = "Selected claim is invalid";
                    }
                    else
                    {
                        Response.msg = "Please reject this claim from processing screen.";
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response.msg = "Error occured while rejecting the claim";
            }
            return Response;
        }

        internal static NotesResponseDto GetClaimNotesPolicyId(Guid policyBundleId)
        {
            NotesResponseDto Response = new NotesResponseDto();
            try
            {

                Response.Notes = new List<NoteResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                session.Query<ClaimNotes>().Where(a => a.PolicyId == policyBundleId)
                .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                .Join(session.Query<Claim>(), o => o.m.ClaimId, p => p.Id, (o, p) => new { o, p })
                .Join(session.Query<InternalUser>(), q => q.o.m.SubmittedUserId.ToString(), r => r.Id, (q, r) => new { q, r })
                .OrderByDescending(or => or.q.o.m.EntryDateTime)
                .ToList().ForEach(z => Response.Notes.Add(new NoteResponseDto()
                {
                    ClaimNo = z.q.p.ClaimNumber,
                    EntryDateTime = z.q.o.m.EntryDateTime.ToString("dd-MMM-yyyy"),
                    Id = z.q.o.m.Id,
                    Note = z.q.o.m.Note,
                    PolicyNo = z.q.o.n.PolicyNo,
                    SubmittedUser = z.r.FirstName + " " + z.r.LastName
                }));

                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static string AddComments(ClaimCommentRequestDto commentData)
        {
            string Response = string.Empty;
            try
            {
                //validate Notes details
                if (commentData == null)
                {
                    return "Request data is empty.";
                }

                ISession session = EntitySessionManager.GetSession();
                ClaimComment cn = new Entities.ClaimComment();

                //cn.Id = Guid.NewGuid();
                cn.ClaimId = commentData.ClaimId;
                cn.EntryDateTime = DateTime.Today.ToUniversalTime();
                cn.Comment = commentData.Comment;
                cn.PolicyId = commentData.PolicyId;
                cn.ByTPA = commentData.ByTPA;
                cn.Seen = commentData.Seen;
                cn.SeenDateTime = SqlDateTime.MinValue.Value;
                cn.SentFrom = commentData.SentFrom;
                cn.SentTo = commentData.SentTo;




                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(cn);
                    commentData.Id = Guid.NewGuid();
                    transaction.Commit();
                }

                Response = "ok";

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                Response = "Error occured while saving Comment.";
            }
            return Response;
        }

        internal static ClaimCommentsResponseDto GetClaimPendingCommentsByPolicyId(Guid policyBundleId)
        {
            ClaimCommentsResponseDto Response = new ClaimCommentsResponseDto();
            try
            {

                Response.Comments = new List<ClaimCommentResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                session.Query<ClaimComment>().Where(a => a.PolicyId == policyBundleId)
                .Join(session.Query<Policy>(), m => m.PolicyId, n => n.Id, (m, n) => new { m, n })
                .Join(session.Query<ClaimSubmission>(), o => o.m.ClaimId, p => p.Id, (o, p) => new { o, p })
                .Join(session.Query<InternalUser>(), q => q.o.m.SentFrom.ToString(), r => r.Id, (q, r) => new { q, r })
                .OrderByDescending(or => or.q.o.m.EntryDateTime)
                .ToList().ForEach(z => Response.Comments.Add(new ClaimCommentResponseDto()
                {
                    ClaimNo = z.q.p.ClaimNumber,
                    EntryDateTime = z.q.o.m.EntryDateTime.ToString("dd-MMM-yyyy"),
                    Id = z.q.o.m.Id,
                    Comment = z.q.o.m.Comment,
                    PolicyNo = z.q.o.n.PolicyNo,
                    SentFrom = z.r.FirstName + " " + z.r.LastName,
                    ByTPA = z.q.o.m.ByTPA
                }));

                return Response;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal static object ValidateInvoiceCode(string invoiceCode, Guid commodityTypeId, Guid dealerId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                #region "Validation"
                if (string.IsNullOrEmpty(invoiceCode))
                {
                    response.code = "error";
                    response.msg = "Invalid Invoice Code.";
                    return response;
                }
                if (!IsGuid(commodityTypeId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Commodity Type.";
                    return response;
                }
                if (!IsGuid(dealerId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Dealer.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                string commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);
                CommodityCategory CommodityCategory = session.Query<CommodityCategory>()
                    .Where(a => a.CommodityTypeId == commodityTypeId).FirstOrDefault();
                List<Guid> allowedMakes = session.Query<DealerMakes>()
                   .Where(a => a.DealerId == dealerId)
                   .Select(a => a.MakeId).ToList();

                InvoiceCode InvoiceCode = session.Query<InvoiceCode>().Where(a => a.Code == invoiceCode).FirstOrDefault();

                if (InvoiceCode == null)
                {
                    response.code = "error";
                    response.msg = "Invalid Invoice Code.";
                    return response;
                }

                InvoiceCodeDetails InvoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == InvoiceCode.Id).FirstOrDefault();

                IList<Policy> eligiblePolicies = new List<Policy>();

                List<Guid> eligiblePolicyNubers = new List<Guid>();
                IList<InvoiceCodeDetails> InvoiceCodeD = new List<InvoiceCodeDetails>();
               // eligiblePolicies = session.Query<Policy>().Where(a => a.Id == InvoiceCodeDetails.PolicyId).ToList();

                if (commodityCode.ToLower() == "o")
                {
                    InvoiceCodeD = session.Query<InvoiceCodeDetails>()
                        .Where(a => a.InvoiceCodeId == InvoiceCode.Id).ToList();
                    if (InvoiceCodeD == null || InvoiceCodeD.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Invoice Code hasn't associated with any policy.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(InvoiceCodeD.Select(e => (e.PolicyId.Value)));
                }

                if (InvoiceCode != null)
                {
                    DateTime TodayDate = DateTime.Today;
                    double ExpireDate = (TodayDate - InvoiceCode.GeneratedDate).TotalDays;

                    if (ExpireDate >= 30)
                    {
                        response.code = "error";
                        response.msg = "Invoice code genatated date is expire.";
                        return response;
                    }

                }



                if (InvoiceCodeDetails.IsPolicyApproved != true)
                {
                    response.code = "error";
                    response.msg = "Entered invoice code hasn't associated with any approved policy.";
                    return response;
                }

                //policy validation
                if (eligiblePolicyNubers.Any())
                {
                    eligiblePolicies = session.QueryOver<Policy>()
                        .WhereRestrictionOn(a => a.Id)
                        .IsIn(eligiblePolicyNubers)
                        .List<Policy>();
                    //policy status validation
                    eligiblePolicies = eligiblePolicies
                        .Where(a => a.IsApproved == true && a.IsPolicyCanceled != true)
                        .ToList();
                    if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any active policy.";
                        return response;
                    }
                }
                else
                {
                    response.code = "error";
                    response.msg = "Enterd Item hasn't associated with any policy.";
                    return response;
                }

                if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                {
                    response.code = "error";
                    response.msg = "Entered  invoice code hasn't associated with any active policy.";
                    return response;
                }

                //DealerInvoiceTireDetails   DealerInvoiceTireDetails = session.Query<DealerInvoiceTireDetails>().

                #endregion
                response.code = "ok";

                var eligiblePoliciess = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == InvoiceCode.Id)
                    .Join(session.Query<InvoiceCodeTireDetails>(), b => b.Id, c => c.InvoiceCodeDetailId, (b, c) => new { b, c })
                    .Join(session.Query<CustomerEnterdInvoiceDetails>(), g => g.b.InvoiceCodeId, h => h.InvoiceCodeId, (g, h) => new { g, h })
                    .Join(session.Query<Policy>(), d => d.g.b.PolicyId, e => e.Id, (d, e) => new { d, e })
                    .Select(xy => new
                    {
                        xy.e.Id,
                        xy.e.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(xy.e.ProductId),
                        CommodityCategoryId = CommodityCategory.CommodityCategoryId,
                        MakeId = InvoiceCodeDetails.MakeId,
                        ModelId = InvoiceCodeDetails.ModelId,
                        xy.d.g.c.Position,
                        InvoiceCode.PlateNumber,
                        xy.d.g.b.TireQuantity,
                        InvoiceCodeId = InvoiceCode.Id,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(xy.e.CustomerId),
                        xy.e.IsApproved,
                        xy.e.IsPolicyCanceled,
                        xy.d.g.c.ArticleNumber,
                        xy.d.g.c.SerialNumber,
                        InvoiceCodeDetailsId = xy.d.g.c.Id,
                        ICDTirePosition = xy.d.g.b.Position,
                        Milage = xy.d.h.AdditionalDetailsMileage
                        //InvoiceCodeId = xy.b.Id,

                    }).Where(v => v.IsApproved == true && v.IsPolicyCanceled != true).OrderByDescending(n => n.PolicyNo).ToArray();

                response.obj = eligiblePoliciess;

                //var result = eligiblePoliciess.GroupBy(i => i.Id)
                // .Select(group =>
                //       new
                //       {
                //           Key = group.Key,
                //           Items = group.OrderByDescending(x => x.PolicyNo)
                //       })
                // .Select(g => g.Items.First());

                //response.obj = result;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }

        internal static object GetTyreDetailsByPolicyNumber(string policyNumber, Guid commodityTypeId, Guid dealerId)
        {
            GenericCodeMsgObjResponse response = new GenericCodeMsgObjResponse();
            try
            {
                #region "Validation"
                if (string.IsNullOrEmpty(policyNumber))
                {
                    response.code = "error";
                    response.msg = "Invalid Service Contract No.";
                    return response;
                }
                if (!IsGuid(commodityTypeId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Commodity Type.";
                    return response;
                }
                if (!IsGuid(dealerId.ToString()))
                {
                    response.code = "error";
                    response.msg = "Invalid Dealer.";
                    return response;
                }

                ISession session = EntitySessionManager.GetSession();
                string commodityCode = new CommonEntityManager().GetCommodityTypeUniqueCodeById(commodityTypeId);
                CommodityCategory CommodityCategory = session.Query<CommodityCategory>()
                    .Where(a => a.CommodityTypeId == commodityTypeId).FirstOrDefault();
                List<Guid> allowedMakes = session.Query<DealerMakes>()
                   .Where(a => a.DealerId == dealerId)
                   .Select(a => a.MakeId).ToList();

                Policy policy = session.Query<Policy>().Where(a => a.PolicyNo == policyNumber).FirstOrDefault();

                if (policy == null)
                {
                    response.code = "error";
                    response.msg = "Invalid Service Contract No.";
                    return response;
                }

                InvoiceCodeDetails InvoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == policy.Id).FirstOrDefault();

                InvoiceCode InvoiceCode = session.Query<InvoiceCode>().Where(a => a.Id == InvoiceCodeDetails.InvoiceCodeId).FirstOrDefault();

                IList<Policy> eligiblePolicies = new List<Policy>();

                List<Guid> eligiblePolicyNubers = new List<Guid>();
                IList<InvoiceCodeDetails> InvoiceCodeD = new List<InvoiceCodeDetails>();
                // eligiblePolicies = session.Query<Policy>().Where(a => a.Id == InvoiceCodeDetails.PolicyId).ToList();

                if (commodityCode.ToLower() == "o")
                {
                    InvoiceCodeD = session.Query<InvoiceCodeDetails>()
                        .Where(a => a.InvoiceCodeId == InvoiceCode.Id).ToList();
                    if (InvoiceCodeD == null || InvoiceCodeD.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Entered Service Contract Number hasn't associated with any Contract.";
                        return response;
                    }

                    eligiblePolicyNubers.AddRange(InvoiceCodeD.Select(e => (e.PolicyId.Value)));
                }

                //if (InvoiceCode != null)
                //{
                //    DateTime TodayDate = DateTime.Today;
                //    double ExpireDate = (TodayDate - InvoiceCode.GeneratedDate).TotalDays;

                //    if (ExpireDate >= 30)
                //    {
                //        response.code = "error";
                //        response.msg = "Invoice code genatated date is expire.";
                //        return response;
                //    }

                //}



                if (InvoiceCodeDetails.IsPolicyApproved != true)
                {
                    response.code = "error";
                    response.msg = "Entered Service Contract Number hasn't associated with any approved Contract.";
                    return response;
                }

                //policy validation
                if (eligiblePolicyNubers.Any())
                {
                    eligiblePolicies = session.QueryOver<Policy>()
                        .WhereRestrictionOn(a => a.Id)
                        .IsIn(eligiblePolicyNubers)
                        .List<Policy>();
                    //policy status validation
                    eligiblePolicies = eligiblePolicies
                        .Where(a => a.IsApproved == true && a.IsPolicyCanceled != true)
                        .ToList();
                    if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                    {
                        response.code = "error";
                        response.msg = "Enterd Item hasn't associated with any active Contract.";
                        return response;
                    }
                }
                else
                {
                    response.code = "error";
                    response.msg = "Enterd Item hasn't associated with any Contract.";
                    return response;
                }

                if (eligiblePolicies == null || eligiblePolicies.Count == 0)
                {
                    response.code = "error";
                    response.msg = "Entered  Service Contract Number hasn't associated with any active policy.";
                    return response;
                }

                //DealerInvoiceTireDetails   DealerInvoiceTireDetails = session.Query<DealerInvoiceTireDetails>().

                #endregion
                response.code = "ok";

                var eligiblePoliciess = session.Query<InvoiceCodeDetails>().Where(a => a.InvoiceCodeId == InvoiceCode.Id)
                    .Join(session.Query<InvoiceCodeTireDetails>(), b => b.Id, c => c.InvoiceCodeDetailId, (b, c) => new { b, c })
                    .Join(session.Query<CustomerEnterdInvoiceDetails>(), g => g.b.InvoiceCodeId, h => h.InvoiceCodeId, (g, h) => new { g, h })
                    .Join(session.Query<Policy>(), d => d.g.b.PolicyId, e => e.Id, (d, e) => new { d, e })
                    .Select(xy => new
                    {
                        xy.e.Id,
                        xy.e.PolicyNo,
                        ProductName = new CommonEntityManager().GetProductNameById(xy.e.ProductId),
                        CommodityCategoryId = CommodityCategory.CommodityCategoryId,
                        MakeId = InvoiceCodeDetails.MakeId,
                        ModelId = InvoiceCodeDetails.ModelId,
                        xy.d.g.c.Position,
                        InvoiceCode.PlateNumber,
                        xy.d.g.b.TireQuantity,
                        InvoiceCodeId = InvoiceCode.Id,
                        CustomerName = new CommonEntityManager().GetCustomerNameById(xy.e.CustomerId),
                        CustomerDetails = new CommonEntityManager().getCustomerById(xy.e.CustomerId),
                        xy.e.IsApproved,
                        xy.e.IsPolicyCanceled,
                        xy.d.g.c.ArticleNumber,
                        xy.d.g.c.SerialNumber,
                        InvoiceCodeDetailsId = xy.d.g.c.Id,
                        ICDTirePosition = xy.d.g.b.Position,
                        Milage = xy.d.h.AdditionalDetailsMileage,
                        MobileNumber = new CommonEntityManager().GetCustomerMobileNumberById(xy.e.CustomerId),
                        ClaimInformation = new CommonEntityManager().GetClaimInformation(xy.e.Id, xy.d.g.c.SerialNumber , xy.d.g.c.Position)
                        //InvoiceCodeId = xy.b.Id,

                    }).Where(v => v.IsApproved == true && v.IsPolicyCanceled != true).OrderByDescending(n => n.PolicyNo).ToArray();

                response.obj = eligiblePoliciess;

                //var result = eligiblePoliciess.GroupBy(i => i.Id)
                // .Select(group =>
                //       new
                //       {
                //           Key = group.Key,
                //           Items = group.OrderByDescending(x => x.PolicyNo)
                //       })
                // .Select(g => g.Items.First());

                //response.obj = result;

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }


            return response;
        }


        internal static object GetAllCustomerComplaints()
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response = session.Query<CustomerComplaint>()
                    .Select(a => new
                    {
                        a.Id,
                        a.Complaint,
                        a.ComplaintCode
                    }).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);

            }
            return Response;
        }

        internal static object GetAllDealerComments()
        {
            object Response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Response = session.Query<DealerComment>()
                    .Select(a => new
                    {
                        a.Id,
                        a.CommentCode,
                        a.Comment,
                        a.IsRejectionType
                    }).OrderByDescending(a => a.CommentCode).ToArray();
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);

            }
            return Response;
        }



        internal static object SubmitOtherTireClaim(ClaimSubmissionOtherTireRequestDto claimSubmissionOtherTireData)
        {
            object response = null;
            try
            {
                //validate claim information
                CommonEntityManager CommonEntityManager = new CommonEntityManager();
                CurrencyEntityManager currencyEm = new CurrencyEntityManager();
                Guid currentConversionPeriodId = currencyEm.GetCurrentCurrencyPeriodId();
                if (!IsGuid(currentConversionPeriodId.ToString()))
                    return "Currenct currency period is not set.";

                //decimal TirePrice = Convert.ToDecimal("0.00");

                ISession session = EntitySessionManager.GetSession();
                Policy policy = session
                    .Query<Policy>().FirstOrDefault(a => a.Id == claimSubmissionOtherTireData.policyId);
                if (policy == null)
                    return "Invalid policy selection.";

                //var ClaimValidation = ClaimEntityManager.OtherTYERClaimValidation(claimSubmissionOtherTireData.OtherTireDetails, claimSubmissionOtherTireData.InvoiceCodeId, policy.Id);
                //if (ClaimValidation != "")
                //{
                //    return ClaimValidation;
                //}

                // validate Previous Claim Mileage
                if (!CheckClaimMileageValidation(policy.PolicyBundleId, claimSubmissionOtherTireData.policyDetails.failureMileage)) {
                    return "Failure Mileage cannot less than previous submitted  claim .";
                }

                List<Part> newParts = new List<Part>();
                List<PartPrice> newPartPrices = new List<PartPrice>();
                List<ClaimSubmissionItem> listClaimSubmissionItem = new List<ClaimSubmissionItem>();

                decimal TirePriceUTDValuation = Convert.ToDecimal("0.00");
                string UnUsedTireDepth = "";
                decimal TirePricePercentage = Convert.ToDecimal("0.00");
                decimal LegalTreadDepth = Convert.ToDecimal("0.00");
                double NoofdatesinPolicy = (DateTime.UtcNow - policy.PolicySoldDate).TotalDays;


                Dealer claimDealer = session
                    .Query<Dealer>().Where(a => a.Id == claimSubmissionOtherTireData.dealerId).FirstOrDefault();
                //Dealer claimDealer = session
                //    .Query<Dealer>().FirstOrDefault(a => a.Id == claimSubmissionOtherTireData.dealerId);

                if (claimDealer == null)
                    return "Logged in dealer is invalid.";

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == claimSubmissionOtherTireData.InvoiceCodeId && a.PolicyId == policy.Id).ToList();

                ClaimItemType claimItemType = session.Query<ClaimItemType>().Where(a => a.ItemDescription == "Part").FirstOrDefault();

                ClaimSubmission claimRequest = DBDTOTransformer.Instance.ConvertClaimDataToClaimSubmissionTireRequest(
                    claimDealer, claimSubmissionOtherTireData, policy, currentConversionPeriodId);



                AvailableTireSizesPattern availableTireSizesPattern = new AvailableTireSizesPattern();
                List<AvailableTireSizesPattern> availableTireSizesPatternList = new List<AvailableTireSizesPattern>();
                List<InvoiceCodeTireDetails> invoiceCodeTireDetails = new List<InvoiceCodeTireDetails>();
                List<ClaimItemTireDetails> ClaimItemTireDetailList = new List<ClaimItemTireDetails>();

                PartArea partArea = session.Query<PartArea>().FirstOrDefault();

                foreach (var ICD in invoiceCodeDetails)
                {
                    invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().Where(a => a.InvoiceCodeDetailId == ICD.Id).ToList();

                    CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>().Where(a => a.InvoiceCodeId == ICD.InvoiceCodeId).FirstOrDefault();

                    Decimal DrivanKM = claimRequest.ClaimMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;

                    List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();

                    //AvailableTireSizesPattern availableTireSizepattern = session.Query<AvailableTireSizesPattern>()
                    //    .Where(a => a.Id == invoiceCodeTireDetails.FirstOrDefault().AvailableTireSizesPatternId).FirstOrDefault();

                    //AvailableTireSizes availableTireSizes = session.Query<AvailableTireSizes>().Where(a => a.CrossSection == invoiceCodeTireDetails.FirstOrDefault().CrossSection &&
                    //                                                           a.Diameter == invoiceCodeTireDetails.FirstOrDefault().Diameter &&
                    //                                                           a.LoadSpeed == invoiceCodeTireDetails.FirstOrDefault().LoadSpeed &&
                    //                                                           a.Width == invoiceCodeTireDetails.FirstOrDefault().Width &&
                    //                                                           a.Id == availableTireSizepattern.AvailableTireSizesId).FirstOrDefault();

                        foreach (var ICTDetais in invoiceCodeTireDetails)
                        {
                            foreach (var tyreObj in claimSubmissionOtherTireData.tyreDetails)
                                {
                                if (ICTDetais.Position == tyreObj.Position) {

                                AvailableTireSizesPattern availableTireSizepattern = (from am in session.Query<ArticleMapping>()
                                                                                      join ats in session.Query<AvailableTireSizes>() on am.AvailableTireSizeId equals ats.Id
                                                                                      join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                                                                                      where am.ArticleNo == tyreObj.ArticleNo
                                                                                      select new AvailableTireSizesPattern { Id=atsp.Id , AvailableTireSizesId= atsp.AvailableTireSizesId, Pattern=atsp.Pattern}).FirstOrDefault();

                                AvailableTireSizes availableTireSizes = (from am in session.Query<ArticleMapping>()
                                            join ats in session.Query<AvailableTireSizes>() on am.AvailableTireSizeId equals ats.Id
                                            join atsp in session.Query<AvailableTireSizesPattern>() on ats.Id equals atsp.AvailableTireSizesId
                                            where am.ArticleNo == tyreObj.ArticleNo
                                            select new AvailableTireSizes {
                                                Id = ats.Id, TirePrice = ats.TirePrice,  OriginalTireDepth = ats.OriginalTireDepth,
                                                Width=ats.Width,CrossSection=ats.CrossSection,
                                                Diameter= ats.Diameter,
                                                LoadSpeed=ats.LoadSpeed,
                                                CurrencyRate =ats.CurrencyRate ,
                                                CurrencyId=ats.CurrencyId,
                                                CurrencyPeriodId =ats.CurrencyPeriodId,
                                            }).FirstOrDefault();

                                UnUsedTireDepth = tyreObj.UnUsedTireDepth;
                                    LegalTreadDepth = (Convert.ToDecimal(tyreObj.UnUsedTireDepth));
                                    TirePricePercentage = (LegalTreadDepth / availableTireSizes.OriginalTireDepth) * 100;

                                    //foreach (var UTDValuation in tireUTDValuation)
                                    //{
                                    //    if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                    //    {
                                    //        if (LegalTreadDepth > Convert.ToDecimal("3"))
                                    //        {
                                    //            if (TirePricePercentage >= (UTDValuation.MinUTD * 100) && TirePricePercentage < (UTDValuation.MaxUTD * 100))
                                    //            {
                                    //                TirePriceUTDValuation = UTDValuation.ClaimPercentage * currencyEm.ConvertFromBaseCurrency(availableTireSizes.TirePrice, claimDealer.CurrencyId, currentConversionPeriodId);
                                    //            }

                                    //        }
                                    //        else
                                    //        {

                                    //            TirePriceUTDValuation = Convert.ToDecimal("0");
                                    //        }

                                    //    }
                                    //    else
                                    //    {
                                    //        TirePriceUTDValuation = currencyEm.ConvertFromBaseCurrency(availableTireSizes.TirePrice, claimDealer.CurrencyId, currentConversionPeriodId);
                                    //    }
                                    //}

                                TirePriceUTDValuation = currencyEm.ConvertFromBaseCurrency(availableTireSizes.TirePrice, claimDealer.CurrencyId, currentConversionPeriodId);
                                // Claim Limitation Criteria Considering - Premium Setup Page Entered Year Wise Claim Price Calculation
                                Decimal unitPrice = currencyEm.ConvertToBaseCurrency(TirePriceUTDValuation, claimDealer.CurrencyId, currentConversionPeriodId);

                                // check is available claim calculation criteria
                                int claimCriteriaCount = session.Query<ClaimCriteria>().Where(a => a.ContractId == policy.ContractId).Count();
                                if (claimCriteriaCount > 0)
                                {
                                    // check claim submission date matching criteria
                                    DateTime policyStartDate = policy.PolicyStartDate;
                                    DateTime PolicyEndDate = policy.PolicyEndDate;
                                    TimeSpan span = claimRequest.ClaimDate - policyStartDate;
                                    DateTime zeroTime = new DateTime(1, 1, 1);
                                    int years = (zeroTime + span).Year;

                                    ClaimCriteria claimCriteria = session.Query<ClaimCriteria>().Where(a => a.ContractId == policy.ContractId && a.Year == years).FirstOrDefault();
                                    if (claimCriteria != null)
                                    {
                                        unitPrice = unitPrice / 100 * claimCriteria.Presentage;
                                    }

                                }
                                Part part = new Part()
                                    {
                                        Id = Guid.NewGuid(),
                                        AllocatedHours = 0,
                                        ApplicableForAllModels = true,
                                        CommodityId = policy.CommodityTypeId,
                                        EntryBy = claimSubmissionOtherTireData.requestedUserId,
                                        EntryDateTime = DateTime.UtcNow,
                                        IsActive = true,
                                        MakeId = claimSubmissionOtherTireData.policyDetails.makeId,
                                        PartAreaId = partArea.Id,
                                        PartCode = ICTDetais.SerialNumber,
                                        PartName = ICTDetais.Position + " -" + ICTDetais.ArticleNumber + " " + availableTireSizepattern.Pattern,
                                        PartNumber = ICTDetais.SerialNumber
                                    };

                                    newParts.Add(part);

                                    PartPrice partPrice = new PartPrice()
                                    {
                                        DealerId = claimSubmissionOtherTireData.dealerId,
                                        CountryId = claimDealer.CountryId,
                                        CurrencyId = claimDealer.CurrencyId,
                                        ConversionRate = currencyEm.GetConversionRate(claimDealer.CurrencyId, currentConversionPeriodId),
                                        CurrencyPeriodId = currentConversionPeriodId,
                                        Id = Guid.NewGuid(),
                                        PartId = part.Id,
                                        Price = unitPrice,
                                        //Price = currencyEm.ConvertToBaseCurrency(TirePriceUTDValuation, claimDealer.CurrencyId, currentConversionPeriodId)
                                    };
                                    newPartPrices.Add(partPrice);


                                ClaimSubmissionItem claimSubmissionItem = new ClaimSubmissionItem()
                                    {
                                        ClaimItemTypeId = claimItemType.Id,
                                        ClaimSubmissionId = claimRequest.Id,
                                        DiscountAmount = Convert.ToDecimal("0.00"),
                                        DiscountRate = Convert.ToDecimal("0.00"),
                                        GoodWillAmount = Convert.ToDecimal("0.00"),
                                        GoodWillRate = Convert.ToDecimal("0.00"),
                                        Id = Guid.NewGuid(),
                                        IsDiscountPercentage = false,
                                        IsGoodWillPercentage = false,
                                        ItemCode = ICTDetais.SerialNumber,
                                        ItemName = ICTDetais.Position +" -" + ICTDetais.ArticleNumber + " " + availableTireSizepattern.Pattern,
                                        ParentId = Guid.Empty,
                                        PartId = part.Id,
                                        Quantity = 1,
                                        Remark = ICTDetais.Position,
                                        TotalGrossPrice = unitPrice,
                                        TotalPrice = unitPrice,
                                        UnitPrice = unitPrice
                                        //UnitPrice = availableTireSizes.TirePrice,
                                    };

                                    listClaimSubmissionItem.Add(claimSubmissionItem);

                                    //UnUsedTireDepth = claimSubmissionOtherTireData.OtherTireDetails.unusedTyreDepthBackRight;
                                    ClaimItemTireDetails claimItemTireDetails = new ClaimItemTireDetails()
                                    {
                                        Id = Guid.NewGuid(),
                                        InvoiceCodeTireId = ICTDetais.Id,
                                        ClaimItemId = claimSubmissionItem.Id,
                                        UnUsedTireDepth = Convert.ToDecimal(UnUsedTireDepth)
                                    };
                                    ClaimItemTireDetailList.Add(claimItemTireDetails);
                                }
                            }
                        }

                }

                decimal TotalClaimAmountSUM = 0;

                foreach (var TotalItemValue in listClaimSubmissionItem)
                {
                    TotalClaimAmountSUM += TotalItemValue.TotalPrice;
                }

                if (claimSubmissionOtherTireData.claimDate < SqlDateTime.MinValue.Value)
                    return "Invalid claim date.Claim date should be greater than " + SqlDateTime.MinValue.Value.ToString("dd-MMM-yyyy");



                List<PartPrice> partPricesList = new List<PartPrice>();

                List<ClaimSubmissionAttachment> clamAttachmentList = new List<ClaimSubmissionAttachment>();
                foreach (Guid docId in claimSubmissionOtherTireData.attachmentIds)
                {
                    ClaimSubmissionAttachment attchment = new ClaimSubmissionAttachment()
                    {
                        ClaimSubmissionId = claimRequest.Id,
                        Id = Guid.NewGuid(),
                        UserAttachmentId = docId,
                        DateOfAttachment = DateTime.UtcNow
                    };
                    clamAttachmentList.Add(attchment);
                }

                DealerComment dealerComment = session.Query<DealerComment>()
                    .Where(a => a.Comment == claimSubmissionOtherTireData.OtherTireDetails.dealerComment).FirstOrDefault();

                if(dealerComment.IsRejectionType == true)
                {
                    claimRequest.RejectionTypeId = dealerComment.Id;
                    claimRequest.StatusId = CommonEntityManager.GetClaimStatusIdByCode("REJ");
                }

                session.Clear();


                using (ITransaction transaction = session.BeginTransaction())
                {
                    claimRequest.TotalClaimAmount = TotalClaimAmountSUM;

                    var newWip =session.Query<ClaimSubmission>().Count(a => a.ClaimSubmittedDealerId == claimRequest.ClaimSubmittedDealerId) + 1;
                    var DealerCode = new CommonEntityManager().GetDealerCodeById(claimRequest.ClaimSubmittedDealerId);
                    claimRequest.Wip = DealerCode.ToUpper() + "/" + Convert.ToString(newWip).PadLeft(Convert.ToInt32(ConfigurationData.ClaimNumberFormatPadding), '0');

                    session.Evict(claimRequest);

                    //save new parts & prices
                    foreach (Part part in newParts)
                    {
                        session.Evict(part);
                        session.Save(part, part.Id);
                    }
                    foreach (PartPrice partPrice in newPartPrices)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);
                    }

                    session.Save(claimRequest, claimRequest.Id);

                    foreach (ClaimSubmissionItem claimItem in listClaimSubmissionItem)
                    {
                        session.Evict(claimItem);
                        //session.Save(claimItem);
                        session.Save(claimItem, claimItem.Id);

                        ////update delaer price if not found
                        //if (IsGuid(claimItem.PartId.ToString()))
                        //{
                        //    PartPrice partPrice = session
                        //        .Query<PartPrice>().FirstOrDefault(a => a.PartId == claimItem.PartId
                        //            && a.DealerId == claimDealer.Id && a.CountryId == claimDealer.CountryId);
                        //    if (partPrice == null)
                        //    {
                        //        partPrice = new PartPrice()
                        //        {
                        //            Id = Guid.NewGuid(),
                        //            ConversionRate = claimRequest.ConversionRate,
                        //            CountryId = claimDealer.CountryId,
                        //            CurrencyId = claimRequest.ClaimCurrencyId,
                        //            CurrencyPeriodId = claimRequest.CurrencyPeriodId,
                        //            DealerId = claimDealer.Id,
                        //            PartId = Guid.Parse(claimItem.PartId.ToString()),
                        //            Price = claimItem.UnitPrice
                        //        };
                        //        partPricesList.Add(partPrice);
                        //    }
                        //}
                    }
                    //save doc ids
                    foreach (ClaimSubmissionAttachment claimAttachment in clamAttachmentList)
                    {
                        session.Evict(claimAttachment);
                        session.Save(claimAttachment, claimAttachment.Id);
                    }

                    //update Part Prices
                    foreach (PartPrice partPrice in partPricesList)
                    {
                        session.Evict(partPrice);
                        session.Save(partPrice, partPrice.Id);
                    }

                    foreach (ClaimItemTireDetails claimItemTire in ClaimItemTireDetailList)
                    {
                        session.Evict(claimItemTire);
                        session.Save(claimItemTire, claimItemTire.Id);
                    }

                    transaction.Commit();

                }
                response = "ok";
                // send Notification Email For Tyre Claim Submission
                try
                {
                    List<string> toEmailList = new List<string>();
                    UserEntityManager ce = new UserEntityManager();
                    UserResponseDto userResponseDto = ce.GetUserById(claimRequest.ClaimSubmittedBy.ToString());
                    toEmailList.Add(userResponseDto.Email);

                    if (claimSubmissionOtherTireData.Reject)
                    {
                        new GetMyEmail().TyreClaimRejection(toEmailList, claimRequest.PolicyNumber);
                    }
                    else
                    {
                        new GetMyEmail().TyreClaimSubmition(toEmailList, userResponseDto.FirstName, claimRequest.Wip);
                    }
                }
                catch (Exception ex)
                {
                    logger.Error(MethodBase.GetCurrentMethod().Name + ": exception - Claim Submission Email: " + ex.Message + ", " + ex.InnerException);
                }

            }

            catch (Exception ex)
            {
                response = "Error occured while saving claim request.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return response;
        }

        private static bool CheckClaimMileageValidation(Guid policyBundleId , decimal falilMilege)
        {
            bool status = false;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                List<Guid> policyIds = session.Query<Policy>()
                                       .Where(a => a.PolicyBundleId == policyBundleId).Select(s=>s.Id).ToList();

               decimal failierMilage =  session.Query<ClaimSubmission>().Where(a => policyIds.Contains(a.PolicyId)).OrderByDescending(o=>o.EntryDate).Select(s => s.FailureMileage).FirstOrDefault();
                if (falilMilege >= failierMilage) {
                    status = true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return status;
        }

        private static string OtherTYERClaimEditValidation(DealerInvoiceTireDetail otherTireDetails, Guid InvoiceCodeId, Guid PolicyId)
        {
            String ErrorMgs = "";

            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == InvoiceCodeId && a.PolicyId == PolicyId).ToList();

                foreach (var ICodeDetails in invoiceCodeDetails)
                {
                    if (otherTireDetails.seriaFrontlLeft != null && otherTireDetails.seriaFrontlLeft != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                        Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.seriaFrontlLeft).FirstOrDefault();

                        if (invoiceCodeTireDetails != null)
                        {
                            // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                            //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                            // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }

                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for front tyre.";
                        }
                    }
                    if (otherTireDetails.serialBackLeft != null && otherTireDetails.serialBackLeft != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialBackLeft).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                            // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                            //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                            // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                    if (otherTireDetails.serialBackRight != null && otherTireDetails.serialBackRight != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialBackRight).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                            // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                            //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                            // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                    if (otherTireDetails.serialFrontRight != null && otherTireDetails.serialFrontRight != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialFrontRight).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                            // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                            //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                            // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                }
                return ErrorMgs;

            }
            catch (Exception ex)
            {
                ErrorMgs = "Error occured while validate Article number.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return ErrorMgs;
        }

        private static object OtherTYERClaimValidation(DealerInvoiceTireDetail otherTireDetails, Guid InvoiceCodeId, Guid PolicyId)
        {

            String ErrorMgs = "";
            try
            {
                ISession session = EntitySessionManager.GetSession();

                List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>()
                    .Where(a => a.InvoiceCodeId == InvoiceCodeId && a.PolicyId == PolicyId).ToList();

                foreach (var ICodeDetails in invoiceCodeDetails)
                {
                    if (otherTireDetails.seriaFrontlLeft != null && otherTireDetails.seriaFrontlLeft != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                        Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.seriaFrontlLeft).FirstOrDefault();

                        if (invoiceCodeTireDetails != null)
                        {
                           // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                           //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                           // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                                foreach (var claimList in claim)
                                {
                                    ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();



                                    if (claimItem.IsApproved == true)
                                    {
                                        ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                    }
                                }

                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for front tyre.";
                        }
                    }
                    if (otherTireDetails.serialBackLeft != null && otherTireDetails.serialBackLeft != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialBackLeft).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                           // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                           //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                           // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                    if (otherTireDetails.serialBackRight != null && otherTireDetails.serialBackRight != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialBackRight).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                           // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                           //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                           // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                    if (otherTireDetails.serialFrontRight != null && otherTireDetails.serialFrontRight != "")
                    {
                        InvoiceCodeTireDetails invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().
                                    Where(a => a.InvoiceCodeDetailId == ICodeDetails.Id && a.SerialNumber == otherTireDetails.serialFrontRight).FirstOrDefault();
                        if (invoiceCodeTireDetails != null)
                        {
                           // ClaimSubmission claimSubmission = session.Query<ClaimSubmission>()
                           //.Where(c => c.PolicyId == PolicyId).FirstOrDefault();
                           // ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(b => b.ClaimSubmissionId == claimSubmission.Id).FirstOrDefault();

                            List<Claim> claim = session.Query<Claim>().Where(d => d.PolicyId == PolicyId).ToList();

                            foreach (var claimList in claim)
                            {
                                ClaimItem claimItem = session.Query<ClaimItem>().Where(e => e.ClaimId == claimList.Id).FirstOrDefault();

                                if (claimItem.IsApproved == true)
                                {
                                    ErrorMgs = "We have found a previous approved claim for the tyre. Only one claim allowed per tyre.";
                                }
                            }
                        }
                        else
                        {
                            ErrorMgs = "Article number not valid for back tyre.";
                        }
                    }
                }
                return ErrorMgs;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }

        }

        internal static UTDResponseDto GetUDTvalueClaimId(Guid _claimId, Guid _policyId, Guid _partId)
        {
            UTDResponseDto response = null;
            try
            {
                ISession session = EntitySessionManager.GetSession();
                CurrencyEntityManager currencyEntityManager = new CurrencyEntityManager();
                CommonEntityManager commonEntityManager = new CommonEntityManager();

                Decimal OriginalTireDepth = Convert.ToDecimal("0.00");
                Decimal UnsedTireDepth = Convert.ToDecimal("0.00");
                Decimal LegalTreadDepth = Convert.ToDecimal("0.00");
                int PercentageValue = 0;
                int OriginalPercentageValue = 0;
                Decimal CustomerPaybleValue = Convert.ToDecimal("0.00");
                bool iswithinonemonth = false;
                Decimal SystemEnterdTireValue = Convert.ToDecimal("0.00");

                Policy policy = session
                    .Query<Policy>().FirstOrDefault(a => a.Id == _policyId);
                Part part = session.Query<Part>().Where(a => a.Id == _partId).FirstOrDefault();
                PartPrice partprice = session.Query<PartPrice>().Where(a => a.PartId == _partId).FirstOrDefault();

                Claim claim = session.Query<Claim>().Where(a => a.Id == _claimId && a.PolicyId == _policyId).FirstOrDefault();

                if (claim == null)
                {
                    List<ClaimSubmission> ClaimSubmission = session.Query<ClaimSubmission>().Where(a => a.Id == _claimId).ToList();

                    foreach (var CSub in ClaimSubmission)
                    {
                        ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(a => a.ClaimSubmissionId == CSub.Id).FirstOrDefault();

                        Decimal OriginalTirePrice = currencyEntityManager.ConvertFromBaseCurrency(partprice.Price, CSub.ClaimCurrencyId, CSub.CurrencyPeriodId);
                        Decimal CalculateUDT = currencyEntityManager.ConvertFromBaseCurrency(claimSubmissionItem.UnitPrice, CSub.ClaimCurrencyId, CSub.CurrencyPeriodId);
                        double NoofdatesinPolicy = (DateTime.UtcNow - policy.PolicySoldDate).TotalDays;
                        List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == _policyId).ToList();


                        foreach (var ICD in invoiceCodeDetails)
                        {

                            CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                                .Where(a => a.InvoiceCodeId == ICD.InvoiceCodeId).FirstOrDefault();

                            decimal DrivanKM = CSub.ClaimMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;

                            List<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().Where(a => a.InvoiceCodeDetailId == ICD.Id).ToList();
                            foreach (var ICTDetails in invoiceCodeTireDetails)
                            {
                                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>().Where(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id && a.InvoiceCodeTireDetailId == ICTDetails.Id).FirstOrDefault();

                                AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                                    .Where(a => a.Id == ICTDetails.AvailableTireSizesPatternId).FirstOrDefault();

                                AvailableTireSizes availableTireSizes = session.Query<AvailableTireSizes>().Where(b => b.CrossSection == ICTDetails.CrossSection &&
                                                                                b.Diameter == ICTDetails.Diameter && b.LoadSpeed == ICTDetails.LoadSpeed &&
                                                                                b.Width == ICTDetails.Width
                                                                                && b.Id == availableTireSizesPattern.AvailableTireSizesId).FirstOrDefault();

                                //List<AvailableTireSizes> availableTire = session.Query<AvailableTireSizes>()
                                //                        .Where(a => a.CrossSection == ICTDetails.CrossSection
                                //                        && a.Diameter == ICTDetails.Diameter
                                //                        && a.LoadSpeed.ToLower().Trim() == ICTDetails.LoadSpeed
                                //                        && a.Width == ICTDetails.Width
                                //                        && a.Id == availableTireSizesPattern.AvailableTireSizesId).ToList();


                                //List<AvailableTireSizesPattern> availableTireSizesPatternList = new List<AvailableTireSizesPattern>();
                                //AvailableTireSizes availableTireSizes = new AvailableTireSizes();



                                ClaimItemTireDetails claimItemTireDetails = session.Query<ClaimItemTireDetails>()
                                    .Where(a => a.InvoiceCodeTireId == ICTDetails.Id && a.ClaimItemId == claimSubmissionItem.Id).FirstOrDefault();

                                CustomerPaybleValue = customerEnterdInvoiceTireDetails.PurchasedPrice * availableTireSizes.CurrencyRate;
                                SystemEnterdTireValue = availableTireSizes.TirePrice * availableTireSizes.CurrencyRate;


                                if (claimItemTireDetails != null)
                                {
                                    if (claimSubmissionItem.Remark == ICTDetails.Position)
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        // LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FL" && ICTDetails.Position == "BR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FL" && ICTDetails.Position == "BL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FR" && ICTDetails.Position == "BL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                                iswithinonemonth = true;
                                            }
                                        }
                                        else
                                        {
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FR" && ICTDetails.Position == "BR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                                iswithinonemonth = true;
                                            }
                                        }
                                        else
                                        {
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BR" && ICTDetails.Position == "FL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }


                                    }
                                    else if (claimSubmissionItem.Remark == "BR" && ICTDetails.Position == "FR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);


                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                foreach (var UTDValuation in tireUTDValuation)
                                                {

                                                    PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BL" && ICTDetails.Position == "FR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BL" && ICTDetails.Position == "FL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }


                                    }
                                }


                            }
                            response = new UTDResponseDto();
                            response.OriginalTireDepth = OriginalTireDepth;
                            response.OriginalTirePrice = OriginalTirePrice;
                            response.UnsedTireDepth = LegalTreadDepth;
                            response.PercentageValue = PercentageValue;
                            response.WithinMonth = iswithinonemonth;

                            response.CalculateUDT = CalculateUDT;
                            //Dec/*i*/mal CustomerValue = customerEnterdInvoiceTireDetails



                            response.PurchasedPrice = CustomerPaybleValue;
                            Decimal WarrentyCredit = Convert.ToDecimal(100 - OriginalPercentageValue);

                            if (CalculateUDT == 0)
                            {
                                response.CustomerValue = CustomerPaybleValue;
                                response.OriginalPercentageValue = 0;
                            }
                            else
                            {
                                response.CustomerValue = (CustomerPaybleValue * WarrentyCredit) / 100;
                                response.OriginalPercentageValue = OriginalPercentageValue;
                            }



                            response.PolicyStartDate = policy.PolicyStartDate;
                            response.FailureDate = CSub.ClaimDate;
                            var NoofDatetoFaliarDate = (CSub.ClaimDate - policy.PolicyStartDate).TotalDays;
                            response.NoofDatetoFaliarDate = Convert.ToInt32(NoofDatetoFaliarDate);
                            response.KMatPolicySale = customerEnterdInvoiceDetails.AdditionalDetailsMileage;
                            response.KMatFailureDate = CSub.ClaimMileage;
                            response.DrivanKM = CSub.ClaimMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;
                        }

                    }
                }
                else
                {
                    List<ClaimSubmission> ClaimSubmission = session.Query<ClaimSubmission>().Where(a => a.Id == claim.ClaimSubmissionId).ToList();

                    foreach (var CSub in ClaimSubmission)
                    {
                        ClaimSubmissionItem claimSubmissionItem = session.Query<ClaimSubmissionItem>().Where(a => a.ClaimSubmissionId == CSub.Id).FirstOrDefault();

                        Decimal OriginalTirePrice = currencyEntityManager.ConvertFromBaseCurrency(partprice.Price, CSub.ClaimCurrencyId, CSub.CurrencyPeriodId);
                        Decimal CalculateUDT = currencyEntityManager.ConvertFromBaseCurrency(claimSubmissionItem.UnitPrice, CSub.ClaimCurrencyId, CSub.CurrencyPeriodId);
                        double NoofdatesinPolicy = (DateTime.UtcNow - policy.PolicySoldDate).TotalDays;
                        List<InvoiceCodeDetails> invoiceCodeDetails = session.Query<InvoiceCodeDetails>().Where(a => a.PolicyId == _policyId).ToList();


                        foreach (var ICD in invoiceCodeDetails)
                        {

                            CustomerEnterdInvoiceDetails customerEnterdInvoiceDetails = session.Query<CustomerEnterdInvoiceDetails>()
                                .Where(a => a.InvoiceCodeId == ICD.InvoiceCodeId).FirstOrDefault();

                            decimal DrivanKM = CSub.ClaimMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;

                            List<InvoiceCodeTireDetails> invoiceCodeTireDetails = session.Query<InvoiceCodeTireDetails>().Where(a => a.InvoiceCodeDetailId == ICD.Id).ToList();
                            foreach (var ICTDetails in invoiceCodeTireDetails)
                            {
                                CustomerEnterdInvoiceTireDetails customerEnterdInvoiceTireDetails = session.Query<CustomerEnterdInvoiceTireDetails>().Where(a => a.CustomerEnterdInvoiceId == customerEnterdInvoiceDetails.Id && a.InvoiceCodeTireDetailId == ICTDetails.Id).FirstOrDefault();



                                AvailableTireSizesPattern availableTireSizesPattern = session.Query<AvailableTireSizesPattern>()
                                    .Where(a => a.Id == ICTDetails.AvailableTireSizesPatternId).FirstOrDefault();

                                AvailableTireSizes availableTireSizes = session.Query<AvailableTireSizes>().Where(b => b.CrossSection == ICTDetails.CrossSection &&
                                                                                b.Diameter == ICTDetails.Diameter && b.LoadSpeed == ICTDetails.LoadSpeed &&
                                                                                b.Width == ICTDetails.Width
                                                                                && b.Id == availableTireSizesPattern.AvailableTireSizesId).FirstOrDefault();
                                ClaimItemTireDetails claimItemTireDetails = session.Query<ClaimItemTireDetails>()
                                    .Where(a => a.InvoiceCodeTireId == ICTDetails.Id && a.ClaimItemId == claimSubmissionItem.Id).FirstOrDefault();

                                CustomerPaybleValue = customerEnterdInvoiceTireDetails.PurchasedPrice * availableTireSizes.CurrencyRate;



                                if (claimItemTireDetails != null)
                                {
                                    if (claimSubmissionItem.Remark == ICTDetails.Position)
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        // LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FL" && ICTDetails.Position == "BR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FL" && ICTDetails.Position == "BL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FR" && ICTDetails.Position == "BL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                                iswithinonemonth = true;
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "FR" && ICTDetails.Position == "BR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                                iswithinonemonth = true;
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BR" && ICTDetails.Position == "FL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }


                                    }
                                    else if (claimSubmissionItem.Remark == "BR" && ICTDetails.Position == "FR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;

                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);


                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                foreach (var UTDValuation in tireUTDValuation)
                                                {

                                                    PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BL" && ICTDetails.Position == "FR")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                            //OriginalPercentageValue = Convert.ToInt32("100");
                                            //iswithinonemonth = true;
                                        }

                                    }
                                    else if (claimSubmissionItem.Remark == "BL" && ICTDetails.Position == "FL")
                                    {
                                        UnsedTireDepth = claimItemTireDetails.UnUsedTireDepth;
                                        OriginalTireDepth = availableTireSizes.OriginalTireDepth;


                                        List<TireUTDValuation> tireUTDValuation = session.Query<TireUTDValuation>().ToList();
                                        LegalTreadDepth = (claimItemTireDetails.UnUsedTireDepth);
                                        //LegalTreadDepth = (availableTireSizes.OriginalTireDepth - claimItemTireDetails.UnUsedTireDepth) - Convert.ToDecimal("1.6");

                                        if (DrivanKM > 2000 || NoofdatesinPolicy > 30)
                                        {
                                            if (LegalTreadDepth > Convert.ToDecimal("3"))
                                            {
                                                PercentageValue = Convert.ToInt32((LegalTreadDepth / OriginalTireDepth) * 100);

                                                foreach (var UTDValuation in tireUTDValuation)
                                                {
                                                    if (PercentageValue >= (UTDValuation.MinUTD * 100) && PercentageValue < (UTDValuation.MaxUTD * 100))
                                                    {
                                                        OriginalPercentageValue = Convert.ToInt32(UTDValuation.ClaimPercentage * 100);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                OriginalPercentageValue = Convert.ToInt32("0");
                                            }
                                        }
                                        else
                                        {
                                            OriginalPercentageValue = Convert.ToInt32("100");
                                            iswithinonemonth = true;
                                        }


                                    }
                                }


                            }
                            response = new UTDResponseDto();
                            response.OriginalTireDepth = OriginalTireDepth;
                            response.OriginalTirePrice = OriginalTirePrice;
                            response.UnsedTireDepth = LegalTreadDepth;
                            response.PercentageValue = PercentageValue;
                            response.WithinMonth = iswithinonemonth;

                            response.CalculateUDT = CalculateUDT;
                            //Dec/*i*/mal CustomerValue = customerEnterdInvoiceTireDetails



                            response.PurchasedPrice = CustomerPaybleValue;
                            Decimal WarrentyCredit = Convert.ToDecimal(100 - OriginalPercentageValue);

                            if (CalculateUDT == 0)
                            {
                                response.CustomerValue = CustomerPaybleValue;
                                response.OriginalPercentageValue = 0;
                            }
                            else
                            {
                                response.CustomerValue = (CustomerPaybleValue * WarrentyCredit) / 100;
                                response.OriginalPercentageValue = OriginalPercentageValue;
                            }



                            response.PolicyStartDate = policy.PolicyStartDate;
                            response.FailureDate = CSub.ClaimDate;
                            var NoofDatetoFaliarDate = (CSub.ClaimDate - policy.PolicyStartDate).TotalDays;
                            response.NoofDatetoFaliarDate = Convert.ToInt32(NoofDatetoFaliarDate);
                            response.KMatPolicySale = customerEnterdInvoiceDetails.AdditionalDetailsMileage;
                            response.KMatFailureDate = CSub.ClaimMileage;
                            response.DrivanKM = CSub.ClaimMileage - customerEnterdInvoiceDetails.AdditionalDetailsMileage;
                        }

                    }
                }

                //ClaimSubmission claimSubmissions = session.Query<ClaimSubmission>().Where(a => a.Id)












            }
            catch (Exception ex)
            {
                //response = "Error occured while saving claim request.";
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return response;
        }
    }
}
