#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Vehicle : Base
    {
        #region DBProps

        private string _class = "";

        public virtual string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private string _position = "[]";

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

        private string _hitPointDamage = "";

        public virtual string HitPointDamage
        {
            get { return _hitPointDamage; }
            set { _hitPointDamage = value; }
        }

        private float _fuel;

        public virtual float Fuel
        {
            get { return _fuel; }
            set { _fuel = value; }
        }

        private string _inventory = "[]";

        public virtual string Inventory
        {
            get { return _inventory; }
            set { _inventory = value; }
        }

        private string _magazinesAmmo = "[]";

        public virtual string MagazinesAmmo
        {
            get { return _magazinesAmmo; }
            set { _magazinesAmmo = value; }
        }

        private int _texture;

        public virtual int Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        #endregion

        private static readonly Dictionary<long, Vehicle> _objStore = new Dictionary<long, Vehicle>();
        private static readonly object TypeLocker = new object();

        private static Vehicle GetObject(long id)
        {
            lock (TypeLocker)
            {
                Vehicle obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Vehicle>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Vehicle()
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
                        session.Delete<Vehicle>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Vehicle));
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
                    Logger.LogType.Error, typeof(Vehicle));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count == 0)
            {
                DeleteObject(id);
                return "[1,\"Success\"]";
            }

            if (packet.Data.Count != 8)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Vehicle));
                return "[0,\"The request was malformed\"]";
            }

            var vehicle = GetObject(id);
            lock (vehicle.Locker)
            {
                vehicle._expiry = packet.Expiry;
                vehicle._class = packet.Data.AsString(0);
                vehicle._position = ArmaArray.Serialize(packet.Data.AsArray(1));
                vehicle._damage = packet.Data.AsFloat(2);
                vehicle._hitPointDamage = ArmaArray.Serialize(packet.Data.AsArray(3));
                vehicle._fuel = packet.Data.AsFloat(4);
                vehicle._inventory = ArmaArray.Serialize(packet.Data.AsArray(5));
                vehicle._magazinesAmmo = ArmaArray.Serialize(packet.Data.AsArray(6));
                vehicle._texture = packet.Data.AsInt(7);
                vehicle.SaveOrUpdate();
            }
            Logger.Log(String.Format("Updating Vehicle: {0}", id), Logger.LogType.Info, typeof(Vehicle));

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
                    Logger.LogType.Error, typeof(Vehicle));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";

            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\",{1},{2},{3},{4},{5},{6},{7}]']",
                    obj._class,
                    obj._position,
                    obj._damage,
                    obj._hitPointDamage,
                    obj._fuel,
                    obj._inventory,
                    obj._magazinesAmmo,
                    obj._texture
                    );
            }
        }
    }
}