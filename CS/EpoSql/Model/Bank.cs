#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Bank : Base
    {
        #region DBProps

        private int _amount;

        public virtual int Amount
        {
            get { return _amount; }
            set { _amount = value; }
        }

        #endregion

        private static readonly Dictionary<long, Bank> _objStore = new Dictionary<long, Bank>();
        private static readonly object TypeLocker = new object();

        private static Bank GetObject(long id)
        {
            lock (TypeLocker)
            {

                Bank obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Bank>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Bank()
                {
                    Id = id
                };
                if (Manager.Settings.CacheData)
                    _objStore.Add(id, obj);
                return obj;
            }
        }

        private static void DeleteObject(long id)
        {
            lock (TypeLocker)
            {
                try
                {
                    using (var session = SessionFactory.OpenSession())
                    {
                        session.Delete<Bank>(id);
                    }
                }
                catch
                {
                    // ignored
                }
                try
                {
                    _objStore.Remove(id);
                }
                catch
                {
                    // ignored
                }
            }
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof (Bank));
        }

        public static string Set(Request packet)
        {
            long id;
            try
            {
                id = Convert.ToInt64(packet.InstanceId);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof (Bank));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 1)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (Bank));
                return "[0,\"The request was malformed\"]";
            }
            var bank = GetObject(id);
            lock (bank.Locker)
            {
                bank._expiry = packet.Expiry;
                bank._amount = packet.Data.AsInt(0);
                bank.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating Bank: {0}", id), Logger.LogType.Debug, typeof (Bank));
            return "[1,\"Success\"]";
        }

        public static string Get(Request packet)
        {
            long id;
            try
            {
                id = Convert.ToInt64(packet.InstanceId);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof(Player));
                return "[0,\"Could not find ID\"]";
            }
            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[{0}]']",
                    obj._amount
                    );
            }
        }
    }
}