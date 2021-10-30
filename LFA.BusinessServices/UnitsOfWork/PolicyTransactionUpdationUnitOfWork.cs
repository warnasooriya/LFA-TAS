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

namespace TAS.Services.UnitsOfWork
{
    internal sealed class PolicyTransactionUpdationUnitOfWork : UnitOfWork
    {
        public PolicyTransactionRequestDto PolicyEndorsement;
        public PolicyTransactionResponseDto ExPr;
       
        public PolicyTransactionUpdationUnitOfWork(PolicyTransactionRequestDto PolicyEndorsement)
        {
            
            this.PolicyEndorsement = PolicyEndorsement;
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
                            PolicyEntityManager PolicyEndorsementEntityManager = new PolicyEntityManager();
                            var ce = PolicyEndorsementEntityManager.GetPolicyEndorsementById(PolicyEndorsement.Id);
                            if (ce.IsPolicyExists == true)
                            {
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
        //        PolicyEntityManager PolicyEndorsementEntityManager = new PolicyEntityManager();
        //        var ce = PolicyEndorsementEntityManager.GetPolicyEndorsementById(PolicyEndorsement.Id);
        //        if (ce.IsPolicyExists == true)
        //        {                   
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
                PolicyEntityManager PolicyEndorsementEntityManager = new PolicyEntityManager();
                //var ce = PolicyEndorsementEntityManager.GetPolicyEndorsementById(PolicyEndorsement.Id);
                //if (ce.IsPolicyEndorsementExists == true)
                //{
                    //bool result = PolicyEndorsementEntityManager.UpdatePolicyEndorsement(PolicyEndorsement);
                    //this.PolicyEndorsement.PolicyInsertion = result;
                //}
                //else
                //{
                //    this.PolicyEndorsement.PolicyEndorsementInsertion = false;
                //}

            }
            finally
            {
                EntitySessionManager.CloseSession();
            }
        }
    }
}
