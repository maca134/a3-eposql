using EpoSql.Core;
using EpoSql.Util;
using System.Threading;

namespace CreateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            SessionFactory.Load();
            SessionFactory.BuildSchema();
            Logger.Log("Database Schema Built", Logger.LogType.Info, typeof(Program));
            Thread.Sleep(4000);
        }
    }
}
