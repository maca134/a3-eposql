#region usings

using System;
using System.IO;
using EpoSql.Model;
using EpoSql.Util;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Dialect;
using NHibernate.Tool.hbm2ddl;

#endregion

namespace EpoSql.Core
{
    public static class SessionFactory
    {
        private static ISessionFactory _factory;
        private static FluentConfiguration _config;

        public static void Load()
        {
            _config = Fluently.Configure();

            if (Manager.Settings.Driver == Settings.DbDriver.Sqlite)
            {
                var dbfile = Path.Combine(Manager.Settings.BasePath, Manager.Settings.SqliteFile);
                var basepath = Path.GetDirectoryName(dbfile);
                if (basepath != null && !Directory.Exists(basepath))
                {
                    try
                    {
                        Directory.CreateDirectory(basepath);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("Could not create directory {0}: {1}", basepath, ex.Message),
                            Logger.LogType.Error, typeof (SessionFactory));
                    }
                }
                Logger.Log(String.Format("Opening Database {0}", dbfile), Logger.LogType.Info, typeof (SessionFactory));

                var sqlcfg = SQLiteConfiguration.Standard.UsingFile(dbfile);
                //sqlcfg.ShowSql();
                //sqlcfg.FormatSql();
                _config.Database(sqlcfg);
            }
            if (Manager.Settings.Driver == Settings.DbDriver.Mysql)
            {
                var sqlcfg = MySQLConfiguration.Standard.ConnectionString(
                    String.Format(
                        "Server={0}; Port={1}; Database={2}; Uid={3}; Pwd={4};",
                        Manager.Settings.MysqlHost,
                        Manager.Settings.MysqlPort,
                        Manager.Settings.MysqlDatabase,
                        Manager.Settings.MysqlUser,
                        Manager.Settings.MysqlPassword
                        ));
                sqlcfg.Dialect<MySQL55InnoDBDialect>();
                //sqlcfg.ShowSql();
                //sqlcfg.FormatSql();
                _config.Database(sqlcfg);
            }
            _config.Mappings(m => m.FluentMappings.AddFromAssemblyOf<Vehicle>());
            _factory = _config.BuildSessionFactory();
#if DEBUG
            //BuildSchema();
#endif
        }

        public static ISession OpenSession()
        {
            return _factory.OpenSession();
        }

        public static void Kill()
        {
            _factory.Dispose();
        }

        public static void BuildSchema()
        {
            try
            {
                new SchemaExport(_config.BuildConfiguration()).Create(false, true);
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, Logger.LogType.Error, typeof (SessionFactory));
            }
        }
    }
}