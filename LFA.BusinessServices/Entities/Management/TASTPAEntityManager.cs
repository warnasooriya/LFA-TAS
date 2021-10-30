using TAS.DataTransfer.Requests;
using TAS.Services.Entities.Persistence;
using NHibernate;
using NHibernate.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using TAS.Services.Common;
using System.Reflection;
using NLog;

namespace TAS.Services.Entities.Management
{
    public class TASTPAEntityManager
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static TASTPA _tt { get; set; }

        internal static List<TASTPA> GetAllTPAs()
        {
            List<TASTPA> entities = null;
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASTPA> TPAData = session.Query<TASTPA>();
            entities = TPAData.ToList();
            return entities;
        }

        internal static List<TASTPA> GetTPADetailById(Guid tpaId)
        {
            List<TASTPA> entities = null;
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASTPA> TPAData = session.Query<TASTPA>().Where(a => a.Id == tpaId);
            entities = TPAData.ToList();
            return entities;
        }

        internal static List<TASTPA> GetTPADetailByName(string tpaName)
        {
            List<TASTPA> entities = null;
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASTPA> TPAData = session.Query<TASTPA>().Where(a => a.Name == tpaName);
            entities = TPAData.ToList();
            return entities;
        }



        internal static string SaveTPA(TASTPARequestDto TPA)
        {
            try
            {
                List<TASTPA> entities = null;
                ISession session = TASEntitySessionManager.GetSession();
                IQueryable<TASTPA> TPAData = session.Query<TASTPA>().Where(w => w.Name == TPA.Name);

                if (TPAData.Count() == 0)
                {


                    TASTPA tpa = new Entities.TASTPA();
                    tpa.Address = TPA.Address;
                    tpa.Banner = TPA.Banner;
                    tpa.DiscountDescription = TPA.DiscountDescription;
                    tpa.Id = Guid.NewGuid();
                    tpa.Logo = TPA.Logo;
                    tpa.Name = TPA.Name;
                    tpa.TelNumber = TPA.TelNumber;
                    tpa.OriginalTPAName = TPA.Name;

                    //string configPath = ConfigurationData.DataMappingFilePath;
                    //string filePath = configPath + @"Scripts\TPADBCreate.sql";

                    //string readText = File.ReadAllText(filePath);
                    var tpaName = TPA.Name.Trim();
                    if (tpaName != "")
                    {
                        string dbName = "";
                        if (tpaName.Contains(" "))
                        {
                            dbName = tpaName.Substring(0, tpaName.IndexOf(" "));// + tpaName.Substring(tpaName.IndexOf(" ") + 2, tpaName.Length - tpaName.IndexOf(" ") - 1).Substring(0, tpaName.Substring(tpaName.IndexOf(" ") + 2, tpaName.Length - tpaName.IndexOf(" ") - 1).IndexOf(" "));
                        }
                        else
                        {
                            dbName = tpaName;
                        }

                        string connectionString = ConfigurationData.DefaultConnectionStringFormat.Replace("[[dbName]]", dbName);
                        //string executeCommand =  readText.Replace("<<DBNAME>>", dbName).Replace("\n"," ").Replace("\r"," ");

                        using (ITransaction transaction = session.BeginTransaction())
                        {
                            tpa.DBName = dbName;
                            tpa.DBConnectionString = connectionString;
                            session.Save(tpa);
                            transaction.Commit();
                            _tt = tpa;
                        }

                        //db Create
                        System.Data.IDbCommand command = session.Connection.CreateCommand();
                        command.CommandText = "exec dbo.CreateTPADb '" + dbName + "'";
                        command.ExecuteNonQuery();

                        //Table Create
                        command = session.Connection.CreateCommand();
                        command.CommandText = "exec dbo.CreateTPATablesByDB '" + dbName + "'";
                        command.ExecuteNonQuery();

                        return connectionString;
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return "";
            }
        }

        internal static bool UpdateTPA(TASTPARequestDto TPA)
        {
            try
            {
                ISession session = TASEntitySessionManager.GetSession();
                TASTPA tpa = new Entities.TASTPA();
                tpa.Id = TPA.Id;
                tpa.Address = TPA.Address;
                tpa.Banner = TPA.Banner;
                tpa.DiscountDescription = TPA.DiscountDescription;
                tpa.Logo = TPA.Logo;
                tpa.Name = TPA.Name;
                tpa.TelNumber = TPA.TelNumber;


                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Update(tpa);
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

        internal static string GetTPAConnStringBydbName(string dbName)
        {
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASTPA> TPAData = session.Query<TASTPA>().Where(w => w.DBName == dbName);

            if (TPAData.FirstOrDefault() != null)
            {
                return TPAData.FirstOrDefault().DBConnectionString == null ? "" : TPAData.FirstOrDefault().DBConnectionString;
            }
            return "";
        }

        public static Guid GetTpaIdByName(string dbName)
        {
            ISession session = TASEntitySessionManager.GetSession();
            TASTPA TPAData = session.Query<TASTPA>().FirstOrDefault(w => w.DBName == dbName);

            if (TPAData != null)
            {
                return TPAData.Id;
            }
            return Guid.Empty;
        }

        internal static string GetTPAViewOnlyConnStringBydbName(string dbName)
        {
            ISession session = TASEntitySessionManager.GetSession();
            IQueryable<TASTPA> TPAData = session.Query<TASTPA>().Where(w => w.DBName == dbName);

            if (TPAData.FirstOrDefault() != null)
            {
                return TPAData.FirstOrDefault().DBConnectionStringViewOnly == null ? "" : TPAData.FirstOrDefault().DBConnectionStringViewOnly;
            }
            return "";
        }

        internal static bool TransferTPAinfo()
        {
            try
            {
                if (_tt != null)
                {
                    ISession session = TASEntitySessionManager.GetSession();
                    TPA tpa = new Entities.TPA();
                    tpa.Id = _tt.Id;
                    tpa.Name = _tt.Name;
                    tpa.TelNumber = _tt.TelNumber;
                    tpa.Address = _tt.Address;
                    tpa.Banner = _tt.Banner;
                    tpa.Logo = _tt.Logo;
                    tpa.DiscountDescription = _tt.DiscountDescription;
                    session.Save(tpa);

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                logger.Error(MethodBase.GetCurrentMethod().Name + ": exception: " + ex.Message + ", " + ex.InnerException);
                return false;
            }


        }
    }
}
