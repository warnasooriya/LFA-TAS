using TAS.DataTransfer.Common;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;

namespace TAS.Services.BusinessServices
{
    internal sealed class ImageManagementService:IImageManagementService
    {
        public Guid SaveImage(ImageRequestDto imageData, SecurityContext securityContext, AuditContext auditContext)
        {
            Guid ImageId = Guid.Empty;
            ImageInsertionUnitOfWork uow = new ImageInsertionUnitOfWork();
            uow.ImageRequestDto = imageData;
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            ImageId = uow.Result;
            return ImageId;
        }

        public ImageResponseDto GetTPAImageById(Guid TPAId, Guid ImageId, SecurityContext securityContext, AuditContext auditContext)
        {
            //Guid ImageId = Guid.Empty;
            TPAImageRetrievalUnitOfWork uow = new TPAImageRetrievalUnitOfWork(TPAId);
            uow.imageId = ImageId;
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            ImageResponseDto iRd = uow.Result;
            //ImageId = uow.Result;
            return iRd;
        }


        public ImageResponseDto GetImageById( Guid ImageId, SecurityContext securityContext, AuditContext auditContext)
        {
            //Guid ImageId = Guid.Empty;
            ImageRetrievalUnitOfWork uow = new ImageRetrievalUnitOfWork();
            uow.imageId = ImageId;
            uow.SecurityContext = securityContext;
            uow.AuditContext = auditContext;
            if (uow.PreExecute())
            {
                uow.Execute();
            }
            ImageResponseDto iRd = uow.Result;
            //ImageId = uow.Result;
            return iRd;
        }
    }
}
