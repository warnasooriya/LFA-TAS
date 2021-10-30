using NHibernate;
using NHibernate.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;

namespace TAS.Services.Entities.Management
{
    public class BrokerEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal static bool SaveBroker(BrokerRequestDto Broker)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Broker broker = new Entities.Broker();
                broker.Id = Guid.NewGuid();
                broker.Name = Broker.Name;
                broker.Code = Broker.Code;
                broker.CountryId = Broker.CountryId;
                broker.BrokerStatus = Broker.BrokerStatus;
                broker.TelNumber = Broker.TelNumber;
                broker.Address = Broker.Address;
                 
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(broker);
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


        internal static bool UpdateBroker(BrokerRequestDto Broker)
        {
            try
            {
                ISession session = EntitySessionManager.GetSession();
                Broker broker = new Entities.Broker();
                broker.Id = Broker.Id;
                broker.Name = Broker.Name;
                broker.Code = Broker.Code;
                broker.CountryId = Broker.CountryId;
                broker.BrokerStatus = Broker.BrokerStatus;
                broker.TelNumber = Broker.TelNumber;
                broker.Address = Broker.Address;

                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(broker);
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
        public List<Broker> GetAllBrokers()
        {
            List<Broker> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Broker> BrokerData = session.Query<Broker>();
            entities = BrokerData.ToList();
            return entities;
        }
        public List<Broker> GetBrokerDetailsByBrokerId(Guid BrokerId)
        {
            List<Broker> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Broker> BrokerData = session.Query<Broker>().Where(a => a.Id == BrokerId);
            entities = BrokerData.ToList();
            return entities;
        }

        public List<Broker> GetAllBrokersByCountry(Guid countryid)
        {
            List<Broker> entities = null;
            ISession session = EntitySessionManager.GetSession();
            IQueryable<Broker> BrokerData = session.Query<Broker>().Where(a => a.CountryId == countryid);
            entities = BrokerData.ToList();
            return entities;
        }




        internal bool IsExsistsBrokerCode(Guid Id, string BrokerName, string BrokerCode)
        {
            ISession session = EntitySessionManager.GetSession();
            if (BrokerName != null)
            {
                var query =
                   from Broker in session.Query<Broker>()
                   where Broker.Id != Id && Broker.Name == BrokerName && Broker.Code == BrokerCode
                   select new { Broker = Broker };

                int total = query.Count();
                if (total > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var query =
                  from Broker in session.Query<Broker>()
                  where Broker.Id != Id && Broker.Code == BrokerCode
                  select new { Broker = Broker };

                int total = query.Count();
                if (total > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                } 
            }
         
        }
    }
}
