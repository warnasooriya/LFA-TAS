using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using TAS.DataTransfer.Common;
using TAS.Services.UnitsOfWork;

namespace TAS.Services.BusinessServices
{
    public class UploadManagementService : IUploadManagementService
    {
        public object UploadAttachment(HttpPostedFile httpPostedFile, string Page, string Section, SecurityContext securityContext, AuditContext auditContext)
        {
            AttachmentUploadUnitOfWork uow = new AttachmentUploadUnitOfWork(httpPostedFile, Page, Section);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object UploadAttachmentExternal(Guid TpaId,HttpPostedFile httpPostedFile, string page, string section)
        {
            AttachmentUploadExternalUnitOfWork uow = new AttachmentUploadExternalUnitOfWork(TpaId,httpPostedFile, page, section);
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public HttpResponseMessage DownloadAttachment(string FileReferenceId, SecurityContext securityContext, AuditContext auditContext)
        {
            AttachmentDownloadUnitOfWork uow = new AttachmentDownloadUnitOfWork(FileReferenceId);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }

        public object DeleteAttachments(List<Guid> attachmentIds, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext)
        {
            AttachmentDeleteUnitOfWork uow = new AttachmentDeleteUnitOfWork(attachmentIds);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
        }




        public object UploadScannedAttachment(byte[] document, string Page, string Section,string filename ,
            string AttachmentType,SecurityContext securityContext, AuditContext auditContext)
        {
            AttachmentScannerUploadUnitOfWork uow = new AttachmentScannerUploadUnitOfWork(document, Page, Section, filename, AttachmentType);
            uow.AuditContext = auditContext;
            uow.SecurityContext = securityContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            return uow.Result;
         
        }
    }
}
