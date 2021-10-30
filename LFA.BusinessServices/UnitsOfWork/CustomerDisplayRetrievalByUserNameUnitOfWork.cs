using TAS.DataTransfer.Responses;
using TAS.Services.Entities;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CustomerDisplayRetrievalByUserNameUnitOfWork : UnitOfWork
    {
        private string UniqueDbName = string.Empty;
        public CustomersResponseDto Result
        {
            get;
            private set;
        }
        public string tpaName
        {
            get;
            set;
        }

        public string customerUserName
        {
            get;
            set;
        }

        public Guid tpaId
        {
            get;
            set;
        }

        internal CustomerDisplayRetrievalByUserNameUnitOfWork(string customerUserName, string tpaName)
        {
            this.tpaName = tpaName;
            this.customerUserName = customerUserName;
        }

        //public CustomerDisplayRetrievalByUserNameUnitOfWork()
        //{
        //    this.tpaName = String.Empty;
        //}


        public override bool PreExecute() {
            try
            {
                TASEntitySessionManager.OpenSession();
                string dbName = UniqueDbName = TASTPAEntityManager.GetTPADetailByName(tpaName).FirstOrDefault().DBName;
                if (dbName != "")
                {

                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    var ret = TASTPAEntityManager.GetTPADetailByName(tpaName).FirstOrDefault();
                    string tasTpaConnString = ret.DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        this.tpaId = ret.Id;
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
                this.tpaId = Guid.Empty;
                return false;

            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                TASEntitySessionManager.CloseSession();
            }
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


                CustomerEntityManager CustomerEntityManager = new CustomerEntityManager();
                List<CustomerResponseDto> CustomerEntities = new List<CustomerResponseDto>();
                if (tpaId == Guid.Empty)
                {
                    CustomerEntities = EntityCacheData.GetCustomers(UniqueDbName);
                }
                else
                {
                    CustomerEntities = CustomerEntityManager.GetCustomersByUserName(customerUserName);

                }
                CustomersResponseDto result = new CustomersResponseDto();
                result.Customers = CustomerEntities;

                this.Result = result;
            }
            finally
            {
                //TASEntitySessionManager.CloseSession();
                EntitySessionManager.CloseSession();

            }

        }
    }
}
