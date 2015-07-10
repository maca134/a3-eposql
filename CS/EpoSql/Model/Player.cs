#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Player : Base
    {
        #region DBProps

        private string _worldspace = "[]";

        public virtual string WorldSpace
        {
            get { return _worldspace; }
            set { _worldspace = value; }
        }

        private string _medical = "[]";

        public virtual string Medical
        {
            get { return _medical; }
            set { _medical = value; }
        }

        private string _appearance = "[]";

        public virtual string Appearance
        {
            get { return _appearance; }
            set { _appearance = value; }
        }

        private string _vars = "[]";

        public virtual string Vars
        {
            get { return _vars; }
            set { _vars = value; }
        }

        private string _weapons = "[]";

        public virtual string Weapons
        {
            get { return _weapons; }
            set { _weapons = value; }
        }

        private string _assignedItems = "[]";

        public virtual string AssignedItems
        {
            get { return _assignedItems; }
            set { _assignedItems = value; }
        }

        private string _magazinesAmmo = "[]";

        public virtual string MagazinesAmmo
        {
            get { return _magazinesAmmo; }
            set { _magazinesAmmo = value; }
        }

        private string _itemsplayer = "[]";

        public virtual string ItemsPlayer
        {
            get { return _itemsplayer; }
            set { _itemsplayer = value; }
        }

        private string _weaponsplayer = "[]";

        public virtual string Weaponsplayer
        {
            get { return _weaponsplayer; }
            set { _weaponsplayer = value; }
        }

        private string _group = "";

        public virtual string Group
        {
            get { return _group; }
            set { _group = value; }
        }

        private bool _revive = true;

        public virtual bool Revive
        {
            get { return _revive; }
            set { _revive = value; }
        }

        private bool _dead;

        public virtual bool Dead
        {
            get { return _dead; }
            set { _dead = value; }
        }

        #endregion

        private static readonly Dictionary<long, Player> _objStore = new Dictionary<long, Player>();
        private static readonly object TypeLocker = new object();

        private static Player GetObject(long id)
        {
            lock (TypeLocker)
            {

                Player obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Player>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Player()
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
                        session.Delete<Player>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Player));
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
                    Logger.LogType.Error, typeof(Player));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Table == "PLAYERSTATS")
            {
                return UpdatePlayerStats(id, packet);
            }

            if (packet.Data.Count != 12)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Player));
                return "[0,\"The request was malformed\"]";
            }

            Logger.Log(String.Format("Updating Player: {0}", id), Logger.LogType.Info, typeof(Player));

            var player = GetObject(id);
            lock (player.Locker)
            {
                player._expiry = packet.Expiry;
                player._worldspace = ArmaArray.Serialize(packet.Data.AsArray(0));
                player._medical = ArmaArray.Serialize(packet.Data.AsArray(1));
                player._appearance = ArmaArray.Serialize(packet.Data.AsArray(2));
                player._vars = ArmaArray.Serialize(packet.Data.AsArray(4));
                player._weapons = ArmaArray.Serialize(packet.Data.AsArray(5));
                player._assignedItems = ArmaArray.Serialize(packet.Data.AsArray(6));
                player._magazinesAmmo = ArmaArray.Serialize(packet.Data.AsArray(7));
                player._itemsplayer = ArmaArray.Serialize(packet.Data.AsArray(8));
                player._weaponsplayer = ArmaArray.Serialize(packet.Data.AsArray(9));
                player._group = packet.Data.AsString(10);
                player._revive = packet.Data.AsBool(11);
                player.SaveOrUpdate();
            }
            return "[1,\"Success\"]";
        }

        private static string UpdatePlayerStats(long id, Request packet)
        {
            if (packet.Data.Count != 2)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Player));
                return "[0,\"The request was malformed\"]";
            }

            int key;
            int val;
            try
            {
                key = packet.Data.AsInt(0);
                val = packet.Data.AsInt(1);
            }
            catch
            {
                Logger.Log(String.Format("Packet error: {0}", String.Join(", ", packet)), Logger.LogType.Error,
                    typeof(Player));
                return "[0,\"The request was malformed\"]";
            }

            Logger.Log(String.Format("Updating PlayerStats: {0}", id), Logger.LogType.Info, typeof(Player));

            var player = GetObject(id);
            lock (player.Locker)
            {
                if (key == 0)
                {
                    player._dead = val == 1;
                }
                player.SaveOrUpdate();
            }
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
                return String.Format("[1,'[{0},{1},{2},[],{3},{4},{5},{6},{7},{8},\"{9}\",{10}]']",
                    obj._worldspace,
                    obj._medical,
                    obj._appearance,
                    obj._vars,
                    obj._weapons,
                    obj._assignedItems,
                    obj._magazinesAmmo,
                    obj._itemsplayer,
                    obj._weaponsplayer,
                    obj._group,
                    obj._revive
                    );
            }
        }

        public static string GetBit(Request packet)
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

            if (packet.Bit != 0)
            {
                Logger.Log(String.Format("Bit {0} is not valid", packet.Bit),
                    Logger.LogType.Error, typeof(Player));
                return "[0,\"Invalid bit\"]";
            }

            lock (obj.Locker)
            {
                return String.Format("[1,\"{0}\"]",
                    (obj._dead) ? "1" : "0"
                    );
            }
        }
    }
}