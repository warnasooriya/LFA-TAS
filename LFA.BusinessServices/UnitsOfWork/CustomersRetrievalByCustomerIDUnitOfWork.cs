using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.DataTransfer.Responses;
using TAS.Services.Common;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CustomersRetrievalByCustomerIDUnitOfWork : UnitOfWork
    {
        public Guid Id;
        public CustomerResponseDto Result
        {
            get;
            private set;
        }

        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
                if (dbName != "")
                {
                    TASEntitySessionManager.OpenSession();
                    string tasTpaConnString = TASTPAEntityManager.GetTPAConnStringBydbName(dbName);
                    TASEntitySessionManager.CloseSession();
                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        EntitySessionManager.OpenSession(dbConnectionString);
                        if (JWTHelper.checkTokenValidity(Convert.ToInt32(ConfigurationData.tasTokenValidTime.ToString())))
                        {
                            return true;
                        }
                        EntitySessionManager.CloseSession();
                    }
                }
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
            return false;

        }

        public override void Execute()
        {
            try
            {
                //temp section can remove  //**  lines once preexecute jwt and db name working fine
                if (dbConnectionString != null)   //**
                {     //**
                    EntitySessionManager.OpenSession(dbConnectionString);
                }     //**
                else     //**
                {     //**
                    EntitySessionManager.OpenSession();     //**
                }     //**
                CustomerEntityManager customerEntityManager = new CustomerEntityManager();
                this.Result = customerEntityManager.GetCustomerDataById(Id);
                //CustomerResponseDto result = new CustomerResponseDto();
                //CustomerResponseDto result = null;
                //if (Customer != null)
                //{
                //    result = new CustomerResponseDto();
                //    result.FirstName = Customer.FirstName;
                //    result.LastName = Customer.LastName;
                //    result.UserName = Customer.UserName;
                //    result.Password = Customer.Password;
                //    result.NationalityId = Customer.NationalityId;
                //    result.CountryId = Customer.CountryId;
                //    result.DateOfBirth = Customer.DateOfBirth;
                //    result.MobileNo = Customer.MobileNo;
                //    result.OtherTelNo = Customer.OtherTelNo;
                //    result.CustomerTypeId = Customer.CustomerTypeId;
                //    result.UsageTypeId = Customer.UsageTypeId;
                //    result.Gender = Customer.Gender;
                //    result.Address1 = Customer.Address1;
                //    result.Address2 = Customer.Address2;
                //    result.Address3 = Customer.Address3;
                //    result.Address4 = Customer.Address4;
                //    result.IDNo = Customer.IDNo;
                //    result.IDTypeId = Customer.IDTypeId;
                //    result.CityId = Customer.CityId;
                //    result.DLIssueDate = Customer.DLIssueDate;
                //    result.Email = Customer.UserName;
                //    result.IsActive = Customer.IsActive;
                //    result.BusinessName = Customer.BusinessName;
                //    result.BusinessAddress1 = Customer.BusinessAddress1;
                //    result.BusinessAddress2 = Customer.BusinessAddress2;
                //    result.BusinessAddress3 = Customer.BusinessAddress3;
                //    result.BusinessAddress4 = Customer.BusinessAddress4;
                //    result.BusinessTelNo = Customer.BusinessTelNo;
                //    result.EntryDateTime = Customer.EntryDateTime;
                //    result.EntryUserId = Customer.EntryUserId;
                //    result.OccupationId = Customer.OccupationId;
                //    result.TitleId = Customer.TitleId;
                //    result.MaritalStatusId = Customer.MaritalStatusId;
                //    result.PostalCode = Customer.PostalCode;
                //    result.Id = Customer.Id.ToString();
                //}
                //this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }

    }
}
