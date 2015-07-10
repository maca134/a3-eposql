#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Building : Base
    {
        #region DBProps

        private string _class = "";

        public virtual string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private string _position = "";

        public virtual string Position
        {
            get { return _position; }
            set { _position = value; }
        }

        private int _storageId = -1;

        public virtual int StorageId
        {
            get { return _storageId; }
            set { _storageId = value; }
        }

        private long _playerId;

        public virtual long PlayerId
        {
            get { return _playerId; }
            set { _playerId = value; }
        }

        private int _texture;

        public virtual int Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private float _damage;

        public virtual float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        #endregion

        private static readonly Dictionary<long, Building> _objStore = new Dictionary<long, Building>();
        private static readonly object TypeLocker = new object();

        private static Building GetObject(long id)
        {
            lock (TypeLocker)
            {

                Building obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Building>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Building()
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
                        session.Delete<Building>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Building));
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

            if (packet.Data.Count == 0)
            {
                DeleteObject(id);
                return "[1,\"Success\"]";
            }
            if (packet.Data.Count < 2)
            {
                Logger.Log(String.Format("Error parsing: {0}", packet.InstanceId), Logger.LogType.Error,
                    typeof(Building));
                return "[0,\"The request was malformed\"]";
            }
            var building = GetObject(id);
            lock (building.Locker)
            {
                building._expiry = packet.Expiry;
                building._class = packet.Data.AsString(0);
                building._position = ArmaArray.Serialize(packet.Data.AsArray(1));

                switch (packet.Data.Count)
                {
                    case 2:
                        break;

                    case 5:
                        building._storageId = packet.Data.AsInt(2);
                        building._playerId = packet.Data.AsLong(3);
                        building._texture = packet.Data.AsInt(4);
                        break;

                    case 6:
                        building._storageId = packet.Data.AsInt(2);
                        building._playerId = packet.Data.AsLong(3);
                        building._texture = packet.Data.AsInt(4);
                        building._damage = packet.Data.AsFloat(5);
                        break;

                    default:
                        Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                            typeof(Building));
                        return "[0,\"The request was malformed\"]";
                }
                building.SaveOrUpdate();
                Logger.Log(String.Format("Updating Building: {0}", id), Logger.LogType.Info, typeof(Building));
            }
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
                return String.Format("[1,'[\"{0}\",{1},\"{2}\",\"{3}\",{4},{5}]',{6}]",
                    obj._class,
                    obj._position,
                    obj._storageId,
                    obj._playerId,
                    obj._texture,
                    obj._damage,
                    (obj.Expiry - DateTime.Now).TotalSeconds
                    );
            }
        }

        public static void Expire(Request packet)
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
                return;
            }
            var obj = GetObject(id);
            lock (obj.Locker)
            {
                obj._expiry = packet.Expiry;
            }
            Logger.Log(String.Format("Expiry reset: {0}", id), Logger.LogType.Info, typeof(Building));
        }
    }
}