#region usings

using System;
using System.IO;
using System.Reflection;
using EpoSql.Util;

#endregion

namespace EpoSql.Core
{
    public class Settings
    {
        public enum DbDriver
        {
            Sqlite,
            Mysql
        }

        private readonly IniParser _ini;

        public Settings()
        {
            var filename = Assembly.GetExecutingAssembly().GetName().Name + ".ini";
            var settingsFile = Path.Combine(BasePath, filename);
            if (!File.Exists(settingsFile))
            {
                Logger.Log(String.Format("Settings file {0} does not exist. Creating default file.", settingsFile),
                    Logger.LogType.Warn, typeof (Settings));
                using (File.Create(settingsFile))
                {
                }
            }
            Logger.Log(String.Format("Loading settings from {0}", settingsFile), typeof (Settings));
            _ini = new IniParser(settingsFile);
            LoadSettings();
        }

        ~Settings()
        {
            SaveSettings();
        }

        public string BasePath
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
        }

        private bool _showconsole;

        public bool ShowConsole
        {
            get { return _showconsole; }
        }

        private bool _logRequests;

        public bool LogRequests
        {
            get { return _logRequests; }
        }

        private bool _cacheData;

        public bool CacheData
        {
            get { return _cacheData; }
        }

        private DbDriver _driver = DbDriver.Sqlite;

        public DbDriver Driver
        {
            get { return _driver; }
        }

        private string _mysqlHost = "localhost";

        public string MysqlHost
        {
            get { return _mysqlHost; }
        }

        private int _mysqlPort = 3306;

        public int MysqlPort
        {
            get { return _mysqlPort; }
        }

        private string _mysqlUser = "root";

        public string MysqlUser
        {
            get { return _mysqlUser; }
        }

        private string _mysqlPassword = "";

        public string MysqlPassword
        {
            get { return _mysqlPassword; }
        }

        private string _mysqlDatabase = "";

        public string MysqlDatabase
        {
            get { return _mysqlDatabase; }
        }

        private string _sqlitefile = @"db\eposql.db";

        public string SqliteFile
        {
            get { return _sqlitefile; }
        }

        public void LoadSettings()
        {
            try
            {
                _showconsole = _ini.GetBoolSetting("General", "ShowConsole");
            }
            catch
            {
                _showconsole = false;
            }

            try
            {
                _logRequests = _ini.GetBoolSetting("General", "LogRequests");
            }
            catch
            {
                _logRequests = false;
            }

            try
            {
                _cacheData = _ini.GetBoolSetting("General", "CacheData", true);
            }
            catch
            {
                _cacheData = true;
            }

            try
            {
                var driver = _ini.GetSetting("General", "Driver", "sqlite").ToLower();
                switch (driver)
                {
                    case "sqlite":
                        _driver = DbDriver.Sqlite;
                        break;
                    case "mysql":
                        _driver = DbDriver.Mysql;
                        break;
                    default:
                        Logger.Log(String.Format("Unknown driver {0}", _driver), typeof (Settings));
                        _driver = DbDriver.Sqlite;
                        break;
                }
            }
            catch
            {
                _driver = DbDriver.Sqlite;
            }

            try
            {
                _mysqlHost = _ini.GetSetting("MySQL", "Host", "localhost");
            }
            catch
            {
                _mysqlHost = "localhost";
            }

            try
            {
                _mysqlPort = Convert.ToInt16(_ini.GetSetting("MySQL", "Port", "3306"));
            }
            catch
            {
                _mysqlPort = 3306;
            }

            try
            {
                _mysqlUser = _ini.GetSetting("MySQL", "User", "root");
            }
            catch
            {
                _mysqlUser = "root";
            }

            try
            {
                _mysqlPassword = _ini.GetSetting("MySQL", "Password");
            }
            catch
            {
                _mysqlPassword = "";
            }

            try
            {
                _mysqlDatabase = _ini.GetSetting("MySQL", "Database");
            }
            catch
            {
                _mysqlDatabase = "";
            }

            try
            {
                _sqlitefile = _ini.GetSetting("Sqlite", "DBFile", @"db\eposql.db");
            }
            catch
            {
                _sqlitefile = "";
            }

            SaveSettings();
        }

        public void SaveSettings()
        {
            _ini.SetSetting("General", "ShowConsole", _showconsole.ToString());
            _ini.SetSetting("General", "LogRequests", _logRequests.ToString());
            _ini.SetSetting("General", "CacheData", _cacheData.ToString());
            _ini.SetSetting("General", "Driver", _driver.ToString());
            _ini.SetSetting("MySQL", "Host", _mysqlHost);
            _ini.SetSetting("MySQL", "Port", _mysqlPort.ToString());
            _ini.SetSetting("MySQL", "User", _mysqlUser);
            _ini.SetSetting("MySQL", "Password", _mysqlPassword);
            _ini.SetSetting("MySQL", "Database", _mysqlDatabase);
            _ini.SetSetting("Sqlite", "DBFile", _sqlitefile);

            _ini.Save();
            Logger.Log("Settings Saved", Logger.LogType.Info, typeof (Manager));
        }
    }
}