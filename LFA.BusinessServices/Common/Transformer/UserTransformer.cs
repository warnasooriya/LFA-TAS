using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;

namespace TAS.Services.Common.Transformer
{
    public class UserTransformer
    {
        public UserResponseDto Transform(Object userObject)
        {
            UserResponseDto _Resposnse = new UserResponseDto();
            try
            {
                var objType = userObject.GetType();
                if (objType == typeof(InternalUser))
                {
                    UserEntityManager userEm = new UserEntityManager();
                    InternalUser InternalUser = (InternalUser)userObject;
                    _Resposnse = new UserResponseDto()
                    {
                        Id = InternalUser.Id,
                        Address1 = InternalUser.Address1,
                        Address2 = InternalUser.Address2,
                        Address3 = InternalUser.Address3,
                        Address4 = InternalUser.Address4,
                        CountryId = InternalUser.CountryId,
                        DateOfBirth = InternalUser.DateOfBirth,
                        DLIssueDate = InternalUser.DLIssueDate,
                        Email = InternalUser.Email,
                        FirstName = InternalUser.FirstName,
                        Gender = InternalUser.Gender,
                        IDNo = InternalUser.IDNo,
                        IDTypeId = InternalUser.IDTypeId,
                        InternalExtension = InternalUser.InternalExtension,
                        IsActive = InternalUser.IsActive,
                        LastName = InternalUser.LastName,
                        MobileNo = InternalUser.MobileNo,
                        NationalityId = InternalUser.NationalityId,
                        OtherTelNo = InternalUser.OtherTelNo,
                        Password = InternalUser.Password,
                        ProfilePicture = InternalUser.ProfilePicture,
                        EntryDate = InternalUser.EntryDate,
                        EntryBy = InternalUser.EntryBy,
                        IsDealerAccount = InternalUser.IsDealerAccount,
                        ReinsurerId = userEm.GetReinsuereIdByUserId(InternalUser.Id)
                    };
                }
                else if (objType == typeof(Customer))
                {
                    Customer Customer = (Customer)userObject;
                    _Resposnse = new UserResponseDto()
                    {
                        Id = Customer.Id.ToString(),
                        Address1 = Customer.Address1,
                        Address2 = Customer.Address2,
                        Address3 = Customer.Address3,
                        Address4 = Customer.Address4,
                        CountryId = Customer.CountryId,
                        DateOfBirth = Customer.DateOfBirth,
                        DLIssueDate = Customer.DLIssueDate,
                        Email = Customer.Email,
                        FirstName = Customer.FirstName,
                        Gender = Customer.Gender,
                        IDNo = Customer.IDNo,
                        IDTypeId = Customer.IDTypeId,
                        IsActive = Customer.IsActive,
                        LastName = Customer.LastName,
                        MobileNo = Customer.MobileNo,
                        NationalityId = Customer.NationalityId,
                        OtherTelNo = Customer.OtherTelNo,
                        Password = Customer.Password,
                      //  ProfilePicture = Customer.ProfilePicture,
                        EntryDate = (DateTime)Customer.EntryDateTime,
                        EntryBy = Customer.EntryUserId,
                        ReinsurerId = Guid.Empty
                    };
                }

            }
            catch (Exception)
            {
            }
            return _Resposnse;

        }
    }
}
