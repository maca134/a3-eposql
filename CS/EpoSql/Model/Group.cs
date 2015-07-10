#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Group : Base
    {
        #region DBProps

        private string _groupName = "";

        public virtual string GroupName
        {
            get { return _groupName; }
            set { _groupName = value; }
        }

        private string _leaderName = "";

        public virtual string LeaderName
        {
            get { return _leaderName; }
            set { _leaderName = value; }
        }

        private int _level = 3;

        public virtual int Level
        {
            get { return _level; }
            protected set { _level = value; }
        }

        private string _mods = "";

        public virtual string Mods
        {
            get { return _mods; }
            set { _mods = value; }
        }

        private string _members = "";

        public virtual string Members
        {
            get { return _members; }
            set { _members = value; }
        }

        #endregion

        private static readonly Dictionary<long, Group> _objStore = new Dictionary<long, Group>();
        private static readonly object TypeLocker = new object();

        private static Group GetObject(long id)
        {
            lock (TypeLocker)
            {

                Group obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Group>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Group()
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
                        session.Delete<Group>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Group));
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
                    Logger.LogType.Error, typeof(Group));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 5)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Group));
                return "[0,\"The request was malformed\"]";
            }

            var group = GetObject(id);
            lock (group.Locker)
            {
                group._expiry = packet.Expiry;
                group._groupName = packet.Data.AsString(0);
                group._leaderName = packet.Data.AsString(1);
                group._level = packet.Data.AsInt(2);
                group._mods = ArmaArray.Serialize(packet.Data.AsArray(3));
                group._members = ArmaArray.Serialize(packet.Data.AsArray(4));
                group.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating Group: {0}", id), Logger.LogType.Info, typeof(Group));

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
                    Logger.LogType.Error, typeof(Group));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\",\"{1}\",{2},{3},{4}]']",
                    obj._groupName,
                    obj._leaderName,
                    obj._level,
                    obj._mods,
                    obj._members
                    );
            }
        }

        public static void Del(Request packet)
        {
            long id;
            try
            {
                id = Convert.ToInt64(packet.InstanceId);
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error getting id: {0} - {1}", ex.Message, packet.InstanceId),
                    Logger.LogType.Error, typeof(Group));
                return;
            }
            DeleteObject(id);
        }
    }
}