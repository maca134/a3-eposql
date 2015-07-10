#region usings

using System;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Log : Base
    {
        #region DBProps

        private string _type = "";

        public virtual string Type
        {
            get { return _type; }
            protected set { _type = value; }
        }

        private string _entry = "";

        public virtual string Entry
        {
            get { return _entry; }
            protected set { _entry = value; }
        }

        #endregion

        public static string Set(Request packet)
        {
            if (packet.Data.Count != 2)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (Log));
                return "[0,\"The request was malformed\"]";
            }

            var log = new Log {_type = packet.Data.AsString(0), _entry = packet.Data.AsString(1)};
            log.SaveOrUpdate();

            Logger.Log(String.Format("Adding log: {0}", log._type), Logger.LogType.Info, typeof (Log));

            return "[1,\"Success\"]";
        }
    }
}