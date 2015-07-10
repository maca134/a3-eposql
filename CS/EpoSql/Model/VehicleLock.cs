#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class VehicleLock : Base
    {
        #region DBProps

        private long _owner;

        public virtual long Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }

        #endregion

        private static readonly Dictionary<long, VehicleLock> _objStore = new Dictionary<long, VehicleLock>();
        private static readonly object TypeLocker = new object();

        private static VehicleLock GetObject(long id)
        {
            lock (TypeLocker)
            {

                VehicleLock obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<VehicleLock>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new VehicleLock()
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
                        session.Delete<VehicleLock>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof (VehicleLock));
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
                    Logger.LogType.Error, typeof (VehicleLock));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 1)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (VehicleLock));
                return "[0,\"The request was malformed\"]";
            }

            var obj = GetObject(id);
            lock (obj.Locker)
            {
                obj._expiry = packet.Expiry;
                obj._owner = packet.Data.AsLong(0);
                obj.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating VehicleLock: {0}", id), Logger.LogType.Info, typeof (VehicleLock));
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
                    Logger.LogType.Error, typeof(VehicleLock));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\"]']",
                    obj._owner
                    );
            }
        }

        public static void Del(Request packet)
        {
            int id;
            try
            {
                id = Convert.ToInt32(packet.InstanceId.Split(':')[1]);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof(VehicleLock));
                return;
            }
            DeleteObject(id);
        }
    }
}