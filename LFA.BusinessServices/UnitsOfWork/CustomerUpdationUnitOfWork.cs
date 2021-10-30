using TAS.DataTransfer.Requests;
using TAS.DataTransfer.Responses;
using TAS.Services.Entities.Management;
using TAS.Services.Entities.Persistence;
using TAS.Services.UnitsOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAS.Services.Common;
using TAS.Caching;

namespace TAS.Services.UnitsOfWork
{
    internal sealed class CustomerUpdationUnitOfWork : UnitOfWork
    {
        public CustomerRequestDto Customer;
        public CustomerResponseDto ExPr;
        private string UniqueDbName = string.Empty;

        public CustomerUpdationUnitOfWork(CustomerRequestDto Customer)
        {

            this.Customer = Customer;
        }
        public override bool PreExecute()
        {
            try
            {
                Common.JWTHelper JWTHelper = new Common.JWTHelper(SecurityContext);
                Dictionary<string, object> str = JWTHelper.DecodeAuthenticationToken();
                string dbName = UniqueDbName = str.FirstOrDefault(f => f.Key == "dbName").Value.ToString();
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
                            CustomerEntityManager CustomerEntityManager = new CustomerEntityManager();
                            var ce = CustomerEntityManager.GetCustomerById(Guid.Parse(Customer.Id));
                            if (ce.IsCustomerExists == true)
                            {
                                Customer.EntryDateTime = ce.EntryDateTime;
                                Customer.EntryUserId = ce.EntryUserId;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
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

        //public override bool PreExecute()
        //{
        //    try
        //    {
        //        EntitySessionManager.OpenSession();
        //        CustomerEntityManager CustomerEntityManager = new CustomerEntityManager();
        //        var ce = CustomerEntityManager.GetCustomerById(Guid.Parse(Customer.Id));
        //        if (ce.IsCustomerExists == true)
        //        {
        //            Customer.EntryDateTime = ce.EntryDateTime;
        //            Customer.EntryUserId = ce.EntryUserId;
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    finally
        //    {
        //        EntitySessionManager.CloseSession();

        //    }

        //}
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
                //var ce = CustomerEntityManager.GetCustomerById(Customer.CustomerId, Customer.CommodityTypeId);
                //if (ce.IsCustomerExists == true)
                //{
                    bool result = CustomerEntityManager.UpdateCustomer(Customer);
                if (result)
                {
                    /* remove cache */
                    ICache cache = CacheFactory.GetCache();
                    cache.Remove(UniqueDbName + "_Customers");
                }
                this.Customer.CustomerInsertion = result;
                //}
                //else
                //{
                //    this.Customer.CustomerInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
