#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class PlayerData : Base
    {
        #region DBProps

        private string _name = "";

        public virtual string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        #endregion

        private static readonly Dictionary<long, PlayerData> _objStore = new Dictionary<long, PlayerData>();
        private static readonly object TypeLocker = new object();

        private static PlayerData GetObject(long id)
        {
            lock (TypeLocker)
            {
                PlayerData obj;
                if (_objStore.TryGetValue(id, out obj))
                {
                    return obj;
                }

                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<PlayerData>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    _objStore.Add(id, obj);
                    return obj;
                }
                obj = new PlayerData()
                {
                    Id = id
                };
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
                        session.Delete<PlayerData>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof (PlayerData));
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
                    Logger.LogType.Error, typeof (PlayerData));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 1)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (PlayerData));
                return "[0,\"The request was malformed\"]";
            }

            var playerdata = GetObject(id);
            lock (playerdata.Locker)
            {
                playerdata._expiry = packet.Expiry;
                playerdata._name = packet.Data.AsString(0);
                playerdata.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating PlayerData: {0}", id), Logger.LogType.Info, typeof (PlayerData));

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
                    Logger.LogType.Error, typeof(PlayerData));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\"]']",
                    obj._name
                    );
            }
        }
    }
}