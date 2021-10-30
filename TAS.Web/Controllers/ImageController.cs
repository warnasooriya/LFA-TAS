using TAS.DataTransfer.Requests;
using TAS.Services;
using TAS.Web.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TAS.Web.Common;
using TAS.Services;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.DataTransfer.Requests;
using NLog;
using System.Reflection;

namespace TAS.Web.Controllers
{
    public class ImageController : ApiController
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        [HttpPost]
        public Guid UploadImage()
        {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    HttpPostedFile File = HttpContext.Current.Request.Files["file"];
                    Byte[] imgByte = null;
                    imgByte = new Byte[File.ContentLength];
                    File.InputStream.Read(imgByte, 0, File.ContentLength);
                    ImageRequestDto imageRequestDto = new ImageRequestDto();
                    imageRequestDto.DateUploaded = DateTime.UtcNow;
                    imageRequestDto.Description = File.ContentType;
                    imageRequestDto.ImageName = File.FileName;
                    imageRequestDto.Id = Guid.NewGuid();
                    imageRequestDto.ImageByte = imgByte;
                    imageRequestDto.ImageStatus = true;
                    IImageManagementService ImageManagementService = ServiceFactory.GetImageManagementService();
                    Guid ImageId = ImageManagementService.SaveImage(imageRequestDto,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    return ImageId;
                }
                else
                {
                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return Guid.Empty;
            }
            
        }

        [HttpPost]
        public string GetImageById(JObject data)
        {
            try
            {
                SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                IImageManagementService ImageManagementService = ServiceFactory.GetImageManagementService();

                if (data["ImageId"] != null && data["ImageId"].ToString() != "")
                {
                    ImageResponseDto ImageData = ImageManagementService.GetImageById(Guid.Parse(data["ImageId"].ToString()),
                    SecurityHelper.Context,
                    AuditHelper.Context);
                    return ImageData.DisplayImageSrc;
                }
                else return "";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "";
            }
            
        }

        [HttpPost]
        public string GetTPAImageById(JObject data)
        {
            IImageManagementService ImageManagementService = ServiceFactory.GetImageManagementService();

            //Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            Guid TPAId = new Guid();   // Temp new Guid. should assign the tpa id coming from jObject data
            TPAId = Guid.Parse(data["tpaId"].ToString());

            ImageResponseDto ImageData = ImageManagementService.GetTPAImageById(TPAId, Guid.Parse(data["ImageId"].ToString()),
            SecurityHelper.Context,
            AuditHelper.Context);
            return ImageData.DisplayImageSrc;
        }

        [HttpPost]
        public Guid UploadScannedImage() {
            try
            {
                if (HttpContext.Current.Request.Files.AllKeys.Any())
                {
                    SecurityHelper.Context.setToken(Request.Headers.Authorization.ToString());
                    HttpPostedFile File = HttpContext.Current.Request.Files["RemoteFile"];
                    Byte[] imgByte = null;
                    imgByte = new Byte[File.ContentLength];
                    File.InputStream.Read(imgByte, 0, File.ContentLength);
                    ImageRequestDto imageRequestDto = new ImageRequestDto();
                    imageRequestDto.DateUploaded = DateTime.UtcNow;
                    imageRequestDto.Description = File.ContentType;
                    imageRequestDto.ImageName = File.FileName;
                    imageRequestDto.Id = Guid.NewGuid();
                    imageRequestDto.ImageByte = imgByte;
                    imageRequestDto.ImageStatus = true;
                    IImageManagementService ImageManagementService = ServiceFactory.GetImageManagementService();
                    Guid ImageId = ImageManagementService.SaveImage(imageRequestDto,
                        SecurityHelper.Context,
                        AuditHelper.Context);
                    return ImageId;
                }
                else
                {
                    return Guid.Empty;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return Guid.Empty;
            }
        }
    }
}
