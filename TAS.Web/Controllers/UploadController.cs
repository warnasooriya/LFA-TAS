using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Web;
using System.Web.Http;
using TAS.Services;
using TAS.Web.Common;

namespace TAS.Web.Controllers
{
    public class UploadController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        [HttpPost]
        public object UploadAttachment()
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                String Page = Request.Headers.GetValues("Page").FirstOrDefault().ToString();
                String Section = Request.Headers.GetValues("Section").FirstOrDefault().ToString();

                if (!HttpContext.Current.Request.Files.AllKeys.Any())
                    return "Failed";
                var httpPostedFile = HttpContext.Current.Request.Files["file"];

                IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();
                return UploadManagementService.UploadAttachment(httpPostedFile, Page, Section, SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Failed";
            }

        }

        [HttpPost]
        public object UploadAttachmentExternal()
        {
            try
            {

                String Page = Request.Headers.GetValues("Page").FirstOrDefault().ToString();
                String Section = Request.Headers.GetValues("Section").FirstOrDefault().ToString();
                Guid TpaId = Guid.Parse(Request.Headers.GetValues("TpaId").FirstOrDefault().ToString());

                if (!HttpContext.Current.Request.Files.AllKeys.Any())
                    return "Failed";
                var httpPostedFile = HttpContext.Current.Request.Files["file"];

                IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();
                return UploadManagementService.UploadAttachmentExternal(TpaId,httpPostedFile, Page, Section);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Failed";
            }

        }

        [HttpPost]
        public HttpResponseMessage DownloadAttachment(JObject data)
        {
            String FileReferenceId = data["fileRef"].ToString();
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
            IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();
            return UploadManagementService.DownloadAttachment(FileReferenceId, SecurityHelper.Context, AuditHelper.Context);
        }

        [HttpPost]
        public object DeleteAttachments(JObject data)
        {
            List<Guid> attachmentIds = JsonConvert.DeserializeObject<List<Guid>>(data["AttachmentIds"].ToString());
            SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());

            IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();
            return UploadManagementService.DeleteAttachments(attachmentIds, SecurityHelper.Context, AuditHelper.Context);
        }


        [HttpPost]
        public object UploadScannerAttachment() //Because Dynamsoft Image Scanner Sends file as "RemoteFile"
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                String Page = Request.Headers.GetValues("Page").FirstOrDefault().ToString();
                String Section = Request.Headers.GetValues("Section").FirstOrDefault().ToString();

                if (!HttpContext.Current.Request.Files.AllKeys.Any())
                    return "Failed";
                var httpPostedFile = HttpContext.Current.Request.Files["RemoteFile"];

                IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();
                return UploadManagementService.UploadAttachment(httpPostedFile, Page, Section, SecurityHelper.Context, AuditHelper.Context);

            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Failed";
            }

        }

        [HttpPost]
        public object UploadScannedAttachment(JObject data)
        {

            var finalAttachmentIdList = new List<string>();
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IUploadManagementService UploadManagementService = ServiceFactory.GetUploadManagementService();

                foreach (var item in data["ImageArray"]) {
                    String page = item["page"].ToString();
                    String section = item["section"].ToString();
                    String attachmentType = item["attachmentType"].ToString();
                    String documentName = item["documentName"].ToString();
                    Document document = null;
                    byte[] bytes = null;
                    using (System.IO.MemoryStream memoryStream = new System.IO.MemoryStream())
                    {
                        document = new Document(PageSize.LETTER, 0f, 0f, 10f, 10f);
                        PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                        document.Open();
                        foreach (var base64Str in item["encodedImage"])
                        {
                            string imageBase64 = base64Str.ToString();
                            byte[] imageBytes = Convert.FromBase64String(imageBase64);
                            iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(imageBytes);
                            PdfPTable table = new PdfPTable(1);
                            table.AddCell(new PdfPCell(image));
                            document.Add(table);
                        }
                        document.Close();
                        bytes = memoryStream.ToArray();
                        memoryStream.Close();
                    }

                   Object insertedId = UploadManagementService.UploadScannedAttachment(bytes, page, section, documentName, attachmentType, SecurityHelper.Context, AuditHelper.Context);
                   if (insertedId.ToString() != null) {
                       finalAttachmentIdList.Add(insertedId.ToString());
                   }
                }
                return finalAttachmentIdList;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "Failed";
            }

        }
    }
}
