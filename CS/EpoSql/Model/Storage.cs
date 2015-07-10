#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Storage : Base
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

        private float _damage;

        public virtual float Damage
        {
            get { return _damage; }
            set { _damage = value; }
        }

        private string _inventory = "[]";

        public virtual string Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        private int _texture;

        public virtual int Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        private string _owners = "[]";

        public virtual string Owners
        {
            get { return _owners; }
            set { _owners = value; }
        }

        private int _parent = -1;

        public virtual int Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        #endregion

        private static readonly Dictionary<long, Storage> _objStore = new Dictionary<long, Storage>();
        private static readonly object TypeLocker = new object();

        private static Storage GetObject(long id)
        {
            lock (TypeLocker)
            {

                Storage obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Storage>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Storage()
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
                        session.Delete<Storage>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof (Storage));
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
                    Logger.LogType.Error, typeof (Storage));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count == 0)
            {
                DeleteObject(id);
                return "[1,\"Success\"]";
            }

            if (packet.Data.Count != 7)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (Storage));
                return "[0,\"The request was malformed\"]";
            }

            var storage = GetObject(id);
            lock (storage.Locker)
            {
                storage._expiry = packet.Expiry;
                storage._class = packet.Data.AsString(0);
                storage._position = ArmaArray.Serialize(packet.Data.AsArray(1));
                storage._damage = packet.Data.AsFloat(2);
                storage._inventory = ArmaArray.Serialize(packet.Data.AsArray(3));
                storage._texture = packet.Data.AsInt(4);
                storage._owners = ArmaArray.Serialize(packet.Data.AsArray(5));
                storage._parent = packet.Data.AsInt(6);
                storage.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating Storage: {0}", id), Logger.LogType.Info, typeof (Storage));

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
                    Logger.LogType.Error, typeof(Storage));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\",{1},{2},{3},{4},{5},{6}]']",
                    obj._class,
                    obj._position,
                    obj._damage,
                    obj._inventory,
                    obj._texture,
                    obj._owners,
                    obj._parent
                    );
            }
        }
    }
}