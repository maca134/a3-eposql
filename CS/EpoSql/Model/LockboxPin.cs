#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class LockboxPin : Base
    {
        private string _pin = "";

        public virtual string Pin
        {
            get { return _pin; }
            set { _pin = value; }
        }


        private static readonly Dictionary<long, LockboxPin> _objStore = new Dictionary<long, LockboxPin>();
        private static readonly object TypeLocker = new object();

        private static LockboxPin GetObject(long id)
        {
            lock (TypeLocker)
            {

                LockboxPin obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<LockboxPin>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new LockboxPin()
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
                        session.Delete<LockboxPin>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(LockboxPin));
        }


        public static string Set(Request packet)
        {
            int id;
            try
            {
                id = Convert.ToInt32(packet.InstanceId.Split(':')[1]);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof(Building));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 1)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(LockboxPin));
                return "[0,\"The request was malformed\"]";
            }

            var lockboxpin = GetObject(id);

            lock (lockboxpin.Locker)
            {
                lockboxpin._expiry = packet.Expiry;
                lockboxpin._pin = packet.Data.AsString(0);
                lockboxpin.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating LockboxPin: {0}", id), Logger.LogType.Info, typeof(LockboxPin));

            return "[1,\"Success\"]";
        }

        public static string Get(Request packet)
        {
            int id;
            try
            {
                id = Convert.ToInt32(packet.InstanceId.Split(':')[1]);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof(Building));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\"]']",
                    obj._pin
                    );
            }
        }
    }
}