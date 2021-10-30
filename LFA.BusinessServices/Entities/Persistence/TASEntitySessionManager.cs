
using System.Web;
using NHibernate;
using NHibernate.Cfg;
using TAS.Services.Common;
using TAS.Core.Storage;


namespace TAS.Services.Entities.Persistence
{
        internal static class TASEntitySessionManager
        {
            private static ISessionFactory sessionFactory;

            private static int retryCnt = 0;

            private static void buildSessionFactory()
            {
                retryCnt += 1;
                try
                {
                    Configuration configuration = new Configuration();
                    string configPath = ConfigurationData.DataMappingFilePath;
                    string mappingPath = configPath + @"TASMappings\";

                    configuration.Configure(configPath + @"TAShibernate.cfg.xml");
                    configuration.AddFile(mappingPath + @"TASSystemUser.hbm.xml");
                    configuration.AddFile(mappingPath + @"TASTPA.hbm.xml");
                    configuration.AddFile(mappingPath + @"TASUserLogin.hbm.xml");
                    configuration.AddFile(mappingPath + @"TASMenu.hbm.xml");

                    sessionFactory = configuration.BuildSessionFactory();
                }
                catch (System.Exception e)
                {
                    if (retryCnt <= 3)
                    {
                        buildSessionFactory();
                    }
                }

            }
            public static void OpenSession()
            {

                if (sessionFactory == null)
                {
                    buildSessionFactory();
                }

                ISession session = sessionFactory.OpenSession();

                TASEntitySessionManager.SetSession(session);
            }

            public static ISession GetSession()
            {
                return StorageManager.GetData("session") as ISession;
            }

            internal static void SetSession(ISession session)
            {
                StorageManager.SetData("session", session);
            }

            public static void CloseSession()
            {
                ISession session = TASEntitySessionManager.GetSession();

                if (session != null)
                {
                    session.Close();
                    session.Dispose();
                }

                // TODO: check if session is getting disposed correctly
            }
        }
}