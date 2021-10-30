using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Entities;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CustomerLoginAuthUnitOfWork : UnitOfWork
    {
        private string UniqueDbName = string.Empty;
        public CustomersResponseDto Result
        {
            get;
            private set;
        }
        public Guid customerID
        {
            get;
            set;
        }

        public Guid tpaId
        {
            get;
            set;
        }


        internal CustomerLoginAuthUnitOfWork(Guid customerID, Guid tpaId)
        {
            this.customerID = customerID;
            this.tpaId = tpaId;
        }

        public CustomerLoginAuthUnitOfWork()
        {
            this.customerID = Guid.Empty;
            this.tpaId = Guid.Empty;
        }

        public override bool PreExecute() {
            try
            {

                TASEntitySessionManager.OpenSession();
                string  dbName = UniqueDbName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBName;

                if (dbName != "")
                {
                    tpaName = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().Name;
                    //string tasTpaConnString = TASTPAEntityManager.GetTPAViewOnlyConnStringBydbName(dbName);
                    string tasTpaConnString = TASTPAEntityManager.GetTPADetailById(tpaId).FirstOrDefault().DBConnectionStringViewOnly;

                    if (tasTpaConnString != "")
                    {
                        dbConnectionString = tasTpaConnString;
                        return true;
                    }
                }
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
                    CustomerEntities = CustomerEntityManager.GetCustomerlistById(customerID);
                }
                CustomersResponseDto result = new CustomersResponseDto();
                result.Customers = CustomerEntities;
                this.Result = result;
            }
            finally
            {
                EntitySessionManager.CloseSession();

            }
        }


        public string tpaName { get; set; }
        public string customerName { get; set; }
    }
}
