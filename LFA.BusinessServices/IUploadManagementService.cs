using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace TAS.Services
{
    public interface IUploadManagementService
    {
        object UploadAttachment(HttpPostedFile httpPostedFile, string Page, string Section, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);

        //object DownloadAttachment(string FileReferenceId, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);

        HttpResponseMessage DownloadAttachment(string FileReferenceId, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);

        object UploadScannedAttachment(byte[] document, string Page, string Section, string filename, string AttachmentType, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);
        
        object DeleteAttachments(List<Guid> attachmentIds, DataTransfer.Common.SecurityContext securityContext, DataTransfer.Common.AuditContext auditContext);
        object UploadAttachmentExternal(Guid TpaId,HttpPostedFile httpPostedFile, string page, string section);
    }
}
