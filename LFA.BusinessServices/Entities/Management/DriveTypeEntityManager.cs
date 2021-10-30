using TAS.Services.Entities.Persistence;
using NHibernate;
using TAS.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;

using NHibernate.Criterion;
using TAS.Services.Common;
using System.Security.Cryptography;
using NLog;
using System.Reflection;

namespace TAS.Services.Entities.Management
{
    public class DriveTypeEntityManager
    {

        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<DriveTypeResponseDto> GetDriveTypes()
        {
            List<DriveType> entities = null;
            ISession session = EntitySessionManager.GetSession();
           return  session.Query<DriveType>().Select(s => new DriveTypeResponseDto
            {
                Id = s.Id,
                Type = s.Type,
                DriveTypeDescription = s.DriveTypeDescription,
                EntryDateTime = s.EntryDateTime,
                EntryUser = s.EntryUser
            }).ToList();
        }


        public DriveTypeResponseDto GetDriveTypeById(Guid DriveTypeId)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();

                DriveTypeResponseDto pDto = new DriveTypeResponseDto();

                var query =
                    from driveType in session.Query<DriveType>()
                    where driveType.Id == DriveTypeId
                    select new { driveType = driveType };

                var result = query.ToList();


                if (result != null && result.Count > 0)
                {
                    pDto.Id = result.First().driveType.Id;
                    pDto.Type = result.First().driveType.Type;
                    pDto.DriveTypeDescription = result.First().driveType.DriveTypeDescription;
                    pDto.EntryDateTime = result.First().driveType.EntryDateTime;
                    pDto.EntryUser = result.First().driveType.EntryUser;

                    pDto.IsDriveTypeExists = true;
                    return pDto;
                }
                else
                {
                    pDto.IsDriveTypeExists = false;
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return null;
            }
        }


        internal bool AddDriveType(DriveTypeRequestDto DriveType)
        {
            try
            {

                ISession session = EntitySessionManager.GetSession();
                DriveType pr = new Entities.DriveType();

                pr.Id = new Guid();
                pr.Type = DriveType.Type;
				 pr.DriveTypeDescription= DriveType.DriveTypeDescription;
                pr.EntryDateTime = DateTime.Today.ToUniversalTime();
                pr.EntryUser = Guid.Parse("ba56ec84-1fe0-4385-abe4-182f62caa050");



                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(pr);
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

        internal bool UpdateDriveType(DriveTypeRequestDto DriveType)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                DriveType pr = new Entities.DriveType();

                pr.Id = DriveType.Id;
                pr.Type = DriveType.Type;
				pr.DriveTypeDescription = DriveType.DriveTypeDescription;
                pr.EntryDateTime = DriveType.EntryDateTime;
                pr.EntryUser = DriveType.EntryUser;

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
    }
}
