using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;
using TAS.Services.Entities;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate.Linq;
using NLog;
using System.Reflection;



namespace TAS.Services.Entities.Management
{
    public class ImageEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        internal Guid SaveImage(ImageRequestDto ImageRequestDto)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Image image = new Image();
                image.Id = ImageRequestDto.Id;
                image.ImageByte = ImageRequestDto.ImageByte;
                image.ImageName = ImageRequestDto.ImageName;
                image.ImageStatus = ImageRequestDto.ImageStatus;
                image.Description = ImageRequestDto.Description;
                image.DateUploaded = ImageRequestDto.DateUploaded;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(image);
                    transaction.Commit();
                }

                return image.Id;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return Guid.Empty;
            }
        }

        internal DataTransfer.Responses.ImageResponseDto GetImageById(Guid imageId)
        {
            ISession session = EntitySessionManager.GetSession();

            DataTransfer.Responses.ImageResponseDto pDto = new DataTransfer.Responses.ImageResponseDto();

            var query =
                from Images in session.Query<Image>()
                where Images.Id == imageId
                select new { image = Images };


            var result = query.ToList();



            if (result != null && result.FirstOrDefault() != null)
            {
                pDto.Id = result.FirstOrDefault().image.Id;
                pDto.DisplayImageSrc = Convert.ToBase64String(result.First().image.ImageByte);

                return pDto;
            }
            else
            {
                return pDto;
            }
        }

        internal string GetImageBase64ById(Guid displayimage)
        {
            string response = string.Empty;
            ISession session = EntitySessionManager.GetSession();

         
            var query =
                from Images in session.Query<Image>()
                where Images.Id == displayimage
                select new { image = Images };


            var result = query.ToList();



            if (result != null && result.FirstOrDefault() != null)
            {

                response = Convert.ToBase64String(result.First().image.ImageByte);

             
            }
            return response;
        }
    }
}
