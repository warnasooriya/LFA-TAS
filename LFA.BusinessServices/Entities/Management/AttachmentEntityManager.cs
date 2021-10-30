using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class AttachmentEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public object GetDocumentTypesByPageName(String pageName)
        {
            ISession session = EntitySessionManager.GetSession();
            IEnumerable<AttachmentSection> AttachmentSections = session.Query<AttachmentSection>()
                .Where(a => a.AttachmentScreenName.ToLower() == pageName.ToLower());

            IEnumerable<AttachmentType> AttachmentTypes = session.Query<AttachmentType>()
                .Where(b => AttachmentSections.Any(c => c.Id == b.AttachmentSectionId));
            return AttachmentSections.Join(AttachmentTypes,
                x => x.Id,
                y => y.AttachmentSectionId,
                (x, y) => new
                {
                    x.AttachmentSectionCode,
                    y.AttachmentTypeCode,
                    y.Id,
                    y.AttachmentTypeDescription
                }).ToArray();

        }

        internal static object UploadAttachment(HttpPostedFile httpPostedFile, string Page, string Section, string DbName,string userId)
        {
            String FileType = httpPostedFile.FileName.Split('@').Last().ToString();
            String FileName = httpPostedFile.FileName.Split('@').First().ToString();
            String FileExtention = FileName.Split('.').Last().ToString();
            decimal ContentLength = Math.Ceiling(decimal.Parse(httpPostedFile.ContentLength.ToString()) / decimal.Parse("1024"));
            Guid PurposedFileId = Guid.NewGuid();
            Guid LoggedUserId = Guid.Empty;
            Guid.TryParse(userId, out LoggedUserId);
            //aws
            AwsS3Helper awsS3 = new AwsS3Helper();
            if (!awsS3.IsExistBucket(DbName.ToLower()))
            {
                var response = awsS3.CreateBucket(DbName.ToLower());
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    return "Failed";
            }

            var result = awsS3.CreateObject(PurposedFileId + "." + FileExtention, DbName.ToLower(), httpPostedFile.ContentType, httpPostedFile.InputStream);
            if (result.HttpStatusCode != HttpStatusCode.OK)
                return "Failed";
            var userAttachment = new UserAttachment()
            {
                Id = PurposedFileId,
                AttachmentFileName = FileName,
                AttachmentFileType = httpPostedFile.ContentType,
                AttachmentSectionId = GetAttachmentSectionByPageAndSectionCode(Page, Section),
                AttachmentSizeKB = ContentLength,
                AttachmentTypeId = GetAttachmentTypeByAttachmentSectionIdAndTypeCode(GetAttachmentSectionByPageAndSectionCode(Page, Section), FileType),
                FileServerReference = PurposedFileId.ToString() + "." + FileExtention,
                UploadedDateTime = DateTime.UtcNow,
                UserDefinedName = "",
                UserId = LoggedUserId
            };

            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(userAttachment, PurposedFileId);
                transaction.Commit();
            }
            return PurposedFileId.ToString();
        }

        internal AttachmentsByUsersResponseDto GetAttachmentsByPolicyIdByUserType(Guid policyBundleId)
        {
            AttachmentsByUsersResponseDto Response = new AttachmentsByUsersResponseDto();
            try
            {

                Response.AdminAttachments = new List<AttachmentResponseDto>();
                Response.DealerAttachments = new List<AttachmentResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<PolicyAttachment> policyAttachments = session.Query<PolicyAttachment>()
                    .Where(a => a.PolicyBundleId == policyBundleId);
                IEnumerable<UserAttachment> userAttachments = session.Query<UserAttachment>();
                IEnumerable<AttachmentSection> attachmentSection = session.Query<AttachmentSection>();
                IEnumerable<AttachmentType> attachementType = session.Query<AttachmentType>();

                var Attachments = policyAttachments.Join(userAttachments,
                    a => a.UserAttachmentId,
                    b => b.Id,
                    (a, b) => new { a, b })
                    .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });

                var attachmentList = new List<AttachmentResponseDto>();
                foreach (var item in Attachments)
                {
                    var attachment = new AttachmentResponseDto()
                    {
                        AttachmentSection = item.e.d.AttachmentSectionCode,
                        AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                        AttachmentType = item.e.c.b.AttachmentFileType,
                        FileName = item.e.c.b.AttachmentFileName,
                        FileServerRef = item.e.c.b.FileServerReference,
                        Id = item.e.c.b.Id,
                        DocumentType = item.f.AttachmentTypeDescription
                    };

                    string userType = "";
                   SystemUser systemUser = session.Query<SystemUser>().Where(a=>a.LoginMapId== item.e.c.b.UserId).FirstOrDefault();
                    if (systemUser != null) {
                        userType = session.Query<UserType>().FirstOrDefault(u => u.Id == systemUser.UserTypeId).Code;
                    }
                    if (systemUser != null && userType == "DU")
                    {
                        Response.DealerAttachments.Add(attachment);
                    }
                    else {
                        Response.AdminAttachments.Add(attachment);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        private static Guid GetAttachmentTypeByAttachmentSectionIdAndTypeCode(Guid AttachmentSectionId, string FileType)
        {
           ISession session = EntitySessionManager.GetSession();
            var AttachmentType = session.Query<AttachmentType>().Where(a => a.AttachmentSectionId == AttachmentSectionId
                && a.AttachmentTypeCode.ToLower() == FileType.ToLower()).FirstOrDefault();
            if (AttachmentType != null)
            {
                return AttachmentType.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        private static Guid GetAttachmentSectionByPageAndSectionCode(string Page, string Section)
        {
            ISession session = EntitySessionManager.GetSession();
            var AttachmentSection = session.Query<AttachmentSection>().Where(a => a.AttachmentScreenName.ToLower() == Page.ToLower()
                && a.AttachmentSectionCode.ToLower() == Section.ToLower()).FirstOrDefault();
            if (AttachmentSection != null)
            {
                return AttachmentSection.Id;
            }
            else
            {
                return Guid.Empty;
            }
        }

        internal AttachmentsResponseDto GetAttachmentsByPolicyId(Guid policyBundleId)
        {
            AttachmentsResponseDto Response = new AttachmentsResponseDto();
            try
            {

                Response.Attachments = new List<AttachmentResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<PolicyAttachment> policyAttachments = session.Query<PolicyAttachment>()
                    .Where(a => a.PolicyBundleId == policyBundleId);
                IEnumerable<UserAttachment> userAttachments = session.Query<UserAttachment>();
                IEnumerable<AttachmentSection> attachmentSection = session.Query<AttachmentSection>();
                IEnumerable<AttachmentType> attachementType = session.Query<AttachmentType>();

                var Attachments = policyAttachments.Join(userAttachments,
                    a => a.UserAttachmentId,
                    b => b.Id,
                    (a, b) => new { a, b })
                    .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });

                var attachmentList = new List<AttachmentResponseDto>();
                foreach (var item in Attachments)
                {
                    var attachment = new AttachmentResponseDto()
                    {
                        AttachmentSection = item.e.d.AttachmentSectionCode,
                        AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                        AttachmentType = item.e.c.b.AttachmentFileType,
                        FileName = item.e.c.b.AttachmentFileName,
                        FileServerRef = item.e.c.b.FileServerReference,
                        Id = item.e.c.b.Id,
                        DocumentType = item.f.AttachmentTypeDescription
                    };
                    Response.Attachments.Add(attachment);
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;

        }

        internal static HttpResponseMessage DownloadAttachment(string FileReferenceId, string DBName)
        {

            //  Guid fileId = Guid.Parse(FileReferenceId.Split('.')[0].ToString());
            ISession session = EntitySessionManager.GetSession();
            var file = session.Query<UserAttachment>().Where(a => a.FileServerReference == FileReferenceId).FirstOrDefault();
            String FileName = "Attachment";
            if (file != null)
            {
                FileName = file.AttachmentFileName;
            }
            AwsS3Helper awsS3 = new AwsS3Helper();
            var Response = awsS3.DownloadObject(FileReferenceId, DBName);
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new StreamContent(Response.ResponseStream);
            result.Content.Headers.ContentType =
                new MediaTypeHeaderValue("application/octet-stream");
            result.Content.Headers.Add("x-filename", FileName);
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")

            {
                FileName = FileName
            };
            return result;
        }

        internal static object DeleteAttachment(List<Guid> attachmentIds, string dbName)
        {
            try
            {
                AwsS3Helper awsS3 = new AwsS3Helper();
                foreach (Guid attachmentId in attachmentIds)
                {
                    ISession session = EntitySessionManager.GetSession();
                    PolicyAttachment policyAttachment = session.Query<PolicyAttachment>()
                        .Where(a => a.UserAttachmentId == attachmentId).FirstOrDefault();
                    UserAttachment userAttachment = session.Query<UserAttachment>()
                        .Where(a => a.Id == attachmentId).FirstOrDefault();

                    var response = awsS3.DeleteObject(userAttachment.FileServerReference, dbName.ToLower());
                    //if (response.HttpStatusCode == HttpStatusCode.OK)
                    //{
                    using (ITransaction transaction = session.BeginTransaction())
                    {
                        session.Delete(userAttachment);
                        session.Delete(policyAttachment);
                        transaction.Commit();
                    }
                    //}
                }
                return "Success";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Failed";
            }


        }



        internal AttachmentsResponseDto GetAttachmentsByClaimRequestId(Guid claimId, bool fromClaim)
        {
            AttachmentsResponseDto Response = new AttachmentsResponseDto();
            try
            {
                Response.Attachments = new List<AttachmentResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<UserAttachment> userAttachments = session.Query<UserAttachment>();
                IEnumerable<AttachmentSection> attachmentSection = session.Query<AttachmentSection>();
                IEnumerable<AttachmentType> attachementType = session.Query<AttachmentType>();
                if (fromClaim)
                {
                    var claimAttachments = session.Query<ClaimAttachment>()
              .Where(a => a.ClaimId == claimId);
                    var attachmentsClaim = claimAttachments.Join(userAttachments,
                        a => a.UserAttachmentId,
                        b => b.Id,
                        (a, b) => new { a, b })
                        .Join(attachmentSection,
                        c => c.b.AttachmentSectionId,
                        d => d.Id,
                        (c, d) => new { c, d })
                        .Join(attachementType,
                        e => e.c.b.AttachmentTypeId,
                        f => f.Id,
                        (e, f) => new { e, f });
                    foreach (var item in attachmentsClaim)
                    {
                        var attachment = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            DateOfAttachment= item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription
                        };
                        Response.Attachments.Add(attachment);
                    }
                }
                else
                {
                    var claimSubmissionAttachments = session.Query<ClaimSubmissionAttachment>()
                  .Where(a => a.ClaimSubmissionId == claimId);
                    var attachmentsClaimSubmission = claimSubmissionAttachments.Join(userAttachments,
                  a => a.UserAttachmentId,
                  b => b.Id,
                  (a, b) => new { a, b })
                  .Join(attachmentSection,
                  c => c.b.AttachmentSectionId,
                  d => d.Id,
                  (c, d) => new { c, d })
                  .Join(attachementType,
                  e => e.c.b.AttachmentTypeId,
                  f => f.Id,
                  (e, f) => new { e, f });
                    foreach (var item in attachmentsClaimSubmission)
                    {
                        var attachment = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            DateOfAttachment = item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription
                        };
                        Response.Attachments.Add(attachment);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }

            return Response;

        }

        internal static object UploadScannedAttachment(byte[] pdfdocument, string Page, string Section,string Filename,string AttachmentType,string DbName , string userId)
        {
            Guid LoggedUserId = Guid.Empty;
            Guid.TryParse(userId, out LoggedUserId);

            String FileExtention = "pdf";
            Guid PurposedFileId = Guid.NewGuid();
            AwsS3Helper awsS3 = new AwsS3Helper();
            if (!awsS3.IsExistBucket(DbName.ToLower()))
            {
                var response = awsS3.CreateBucket(DbName.ToLower());
                if (response.HttpStatusCode != HttpStatusCode.OK)
                    return "Failed";
            }
            MemoryStream streamobject = new MemoryStream(pdfdocument);

            var result = awsS3.CreateObject(PurposedFileId + "." + FileExtention, DbName.ToLower(), "application/pdf", streamobject);
            if (result.HttpStatusCode != HttpStatusCode.OK)
                return "Failed";

            var userAttachment = new UserAttachment()
            {
                Id = PurposedFileId,
                AttachmentFileName = Filename + ".pdf",
                AttachmentFileType = "application/pdf",
                AttachmentSectionId = GetAttachmentSectionByPageAndSectionCode(Page, Section),
                AttachmentSizeKB = pdfdocument.Length / (1024),
                AttachmentTypeId = GetAttachmentTypeByAttachmentSectionIdAndTypeCode(GetAttachmentSectionByPageAndSectionCode(Page, Section), AttachmentType),
                FileServerReference = PurposedFileId.ToString() + "." + FileExtention,
                UploadedDateTime = DateTime.UtcNow,
                UserDefinedName = "",
                UserId= LoggedUserId
            };
            ISession session = EntitySessionManager.GetSession();
            using (ITransaction transaction = session.BeginTransaction())
            {
                session.Save(userAttachment, PurposedFileId);
                transaction.Commit();
            }
            return PurposedFileId.ToString();
        }
        internal string GetClaimNumberByClaimId(Guid Id)
        {
            ISession session = EntitySessionManager.GetSession();
            var query =
                from Claim in session.Query<Claim>()
                where Claim.Id == Id
                select new { Claim = Claim };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                return result.First().Claim.ClaimNumber;
            }
            else
            {

                return null;
            }

        }

        internal string GetClaimSubNumberByClaimId(Guid Id)
        {
            ISession session = EntitySessionManager.GetSession();
            var query =
                from ClaimSubmission in session.Query<ClaimSubmission>()
                where ClaimSubmission.Id == Id
                select new { ClaimSubmission = ClaimSubmission };

            var result = query.ToList();


            if (result != null && result.Count > 0)
            {
                return result.First().ClaimSubmission.Wip;
            }
            else
            {

                return null;
            }

        }

        internal AttachmentsResponseDto GetClaimAndPolicyAttachmentsByPolicyId(Guid policyBundleId)
        {
            AttachmentsResponseDto Response = new AttachmentsResponseDto();
            try
            {

                Response.Attachments = new List<AttachmentResponseDto>();
                ISession session = EntitySessionManager.GetSession();
                IEnumerable<Policy> policy = session.Query<Policy>().Where(a => a.Id == policyBundleId);
                IEnumerable<Claim> claim = session.Query<Claim>().Where(a => a.PolicyId == policy.FirstOrDefault().Id);
                IEnumerable<ClaimSubmission> claimSubmission = session.Query<ClaimSubmission>().Where(a => a.PolicyId == policy.FirstOrDefault().Id);

                Policy policys = session.Query<Policy>().Where(a => a.Id == policyBundleId).FirstOrDefault();
                Claim claims = session.Query<Claim>().Where(a => a.PolicyId == policys.Id).FirstOrDefault();
                ClaimSubmission claimSubmissions = session.Query<ClaimSubmission>().Where(a => a.PolicyId == policys.Id).FirstOrDefault();

                IEnumerable<PolicyAttachment> policyAttachments = session.Query<PolicyAttachment>()
                    .Where(a => a.PolicyBundleId == policyBundleId);

                IEnumerable<ClaimAttachment> claimAttachement = session.Query<ClaimAttachment>()
                    .Where(a => a.ClaimId == claim.FirstOrDefault().Id);
                IEnumerable<ClaimSubmissionAttachment> claimSubmissionAttachment = session.Query<ClaimSubmissionAttachment>()
                    .Where(a => a.ClaimSubmissionId == claimSubmission.FirstOrDefault().Id);

                IEnumerable<UserAttachment> userAttachments = session.Query<UserAttachment>();
                IEnumerable<AttachmentSection> attachmentSection = session.Query<AttachmentSection>();
                IEnumerable<AttachmentType> attachementType = session.Query<AttachmentType>();


                var Attachments = policyAttachments.Join(userAttachments,
                    a => a.UserAttachmentId,
                    b => b.Id,
                    (a, b) => new { a, b })
                    .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });



                var attachmentList = new List<AttachmentResponseDto>();
                foreach (var item in Attachments)
                {
                    var attachment = new AttachmentResponseDto()
                    {
                        AttachmentSection = item.e.d.AttachmentSectionCode,
                        AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                        AttachmentType = item.e.c.b.AttachmentFileType,
                        FileName = item.e.c.b.AttachmentFileName,
                        FileServerRef = item.e.c.b.FileServerReference,
                        Id = item.e.c.b.Id,
                        DocumentType = item.f.AttachmentTypeDescription
                    };
                    Response.Attachments.Add(attachment);
                }

                if (claims != null)
                {
                    var claimAttachments = claimAttachement.Join(userAttachments,
                     a => a.UserAttachmentId,
                     b => b.Id,
                     (a, b) => new { a, b })
                     .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });

                    foreach (var item in claimAttachments)
                    {
                        var calimattachments = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription,
                            DateOfAttachment = item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            ClaimNumber = GetClaimNumberByClaimId(item.e.c.a.ClaimId)

                        };
                        Response.Attachments.Add(calimattachments);
                    }
                }

                if (claimSubmissions != null)
                {
                    var claimSubmissionAttachments = claimSubmissionAttachment.Join(userAttachments,
                     a => a.UserAttachmentId,
                     b => b.Id,
                     (a, b) => new { a, b })
                     .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });


                    foreach (var item in claimSubmissionAttachments)
                    {
                        var claimSubmissionAttach = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription,
                            DateOfAttachment = item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            ClaimNumber = GetClaimSubNumberByClaimId(item.e.c.a.ClaimSubmissionId)
                        };
                        Response.Attachments.Add(claimSubmissionAttach);
                    }

                }



            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
            }
            return Response;
        }

        internal AttachmentsResponseDto GetClaimAndPolicyAttachmentsByClaimId(Guid claimId)
        {
            AttachmentsResponseDto Response = new AttachmentsResponseDto();
            try
            {

                Response.Attachments = new List<AttachmentResponseDto>();
                ISession session = EntitySessionManager.GetSession();

                IEnumerable<UserAttachment> userAttachments = session.Query<UserAttachment>();
                IEnumerable<AttachmentSection> attachmentSection = session.Query<AttachmentSection>();
                IEnumerable<AttachmentType> attachementType = session.Query<AttachmentType>();

                Claim claims = session.Query<Claim>().Where(a => a.Id == claimId).FirstOrDefault();
                ClaimSubmission claimSubmissions = session.Query<ClaimSubmission>()
                    .Where(a => a.Id == claimId).FirstOrDefault();

                if(claims != null)
                {
                    Policy policys = session.Query<Policy>().Where(a => a.Id == claims.PolicyId).FirstOrDefault();
                    IEnumerable<PolicyAttachment> policyAttachments = session.Query<PolicyAttachment>()
                   .Where(a => a.PolicyBundleId == policys.PolicyBundleId);
                    IEnumerable<ClaimAttachment> claimAttachement = session.Query<ClaimAttachment>()
                   .Where(a => a.ClaimId == claims.Id);

                    var Attachments = policyAttachments.Join(userAttachments,
                    a => a.UserAttachmentId,
                    b => b.Id,
                    (a, b) => new { a, b })
                    .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });



                    var attachmentList = new List<AttachmentResponseDto>();
                    foreach (var item in Attachments)
                    {
                        var attachment = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription
                        };
                        Response.Attachments.Add(attachment);
                    }

                    var claimAttachments = claimAttachement.Join(userAttachments,
                     a => a.UserAttachmentId,
                     b => b.Id,
                     (a, b) => new { a, b })
                     .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });

                    foreach (var item in claimAttachments)
                    {
                        var calimattachments = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription,
                            DateOfAttachment = item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            ClaimNumber = GetClaimNumberByClaimId(item.e.c.a.ClaimId)

                        };
                        Response.Attachments.Add(calimattachments);
                    }
                }                      

                if (claimSubmissions != null)
                {

                    Policy policys = session.Query<Policy>().Where(a => a.Id == claimSubmissions.PolicyId).FirstOrDefault();
                    IEnumerable<PolicyAttachment> policyAttachments = session.Query<PolicyAttachment>()
                   .Where(a => a.PolicyBundleId == policys.PolicyBundleId);
                    IEnumerable<ClaimSubmissionAttachment> claimSubmissionAttachment = session.Query<ClaimSubmissionAttachment>()
                        .Where(a => a.ClaimSubmissionId == claimSubmissions.Id);

                    var Attachments = policyAttachments.Join(userAttachments,
                    a => a.UserAttachmentId,
                    b => b.Id,
                    (a, b) => new { a, b })
                    .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });



                    var attachmentList = new List<AttachmentResponseDto>();
                    foreach (var item in Attachments)
                    {
                        var attachment = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription
                        };
                        Response.Attachments.Add(attachment);
                    }


                    var claimSubmissionAttachments = claimSubmissionAttachment.Join(userAttachments,
                     a => a.UserAttachmentId,
                     b => b.Id,
                     (a, b) => new { a, b })
                     .Join(attachmentSection,
                    c => c.b.AttachmentSectionId,
                    d => d.Id,
                    (c, d) => new { c, d })
                    .Join(attachementType,
                    e => e.c.b.AttachmentTypeId,
                    f => f.Id,
                    (e, f) => new { e, f });


                    foreach (var item in claimSubmissionAttachments)
                    {
                        var claimSubmissionAttach = new AttachmentResponseDto()
                        {
                            AttachmentSection = item.e.d.AttachmentSectionCode,
                            AttachmentSizeKB = item.e.c.b.AttachmentSizeKB.ToString(),
                            AttachmentType = item.e.c.b.AttachmentFileType,
                            FileName = item.e.c.b.AttachmentFileName,
                            FileServerRef = item.e.c.b.FileServerReference,
                            Id = item.e.c.b.Id,
                            DocumentType = item.f.AttachmentTypeDescription,
                            DateOfAttachment = item.e.c.a.DateOfAttachment.Date.ToString("dd-MMM-yyyy"),
                            ClaimNumber = GetClaimSubNumberByClaimId(item.e.c.a.ClaimSubmissionId)
                        };
                        Response.Attachments.Add(claimSubmissionAttach);
                    }

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
