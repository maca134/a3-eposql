#region usings

using System;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Store : Base
    {
        #region DBProps

        private string _key = "";

        public virtual string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _data = "";

        public virtual string Data
        {
            get { return _data; }
            set { _data = value; }
        }

        #endregion

        private static readonly object TypeLocker = new object();

        public static string Set(Request packet)
        {
            var id = packet.InstanceId;
            lock (TypeLocker)
            {
                var store = new Store {_key = id, _expiry = packet.Expiry, _data = packet.Data.ToString()};
                store.SaveOrUpdate();
            }
            Logger.Log(String.Format("Saving store: {0}", packet.Data), Logger.LogType.Info, typeof (Store));
            return "[1,\"Success\"]";
        }
    }
}