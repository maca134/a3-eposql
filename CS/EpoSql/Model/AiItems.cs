#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class AiItems : Base
    {
        #region DBProps

        private string _items = "[]";

        public virtual string Items
        {
            get { return _items; }
            set { _items = value; }
        }

        private string _count = "[]";

        public virtual string Count
        {
            get { return _count; }
            set { _count = value; }
        }

        #endregion

        private static readonly Dictionary<long, AiItems> _objStore = new Dictionary<long, AiItems>();
        private static readonly object TypeLocker = new object();

        private static AiItems GetObject(long id)
        {
            lock (TypeLocker)
            {

                AiItems obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<AiItems>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new AiItems()
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
                        session.Delete<AiItems>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof (AiItems));
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
                    Logger.LogType.Error, typeof (AiItems));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 2)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof (AiItems));
                return "[0,\"The request was malformed\"]";
            }

            var aiitem = GetObject(id);
            lock (aiitem.Locker)
            {
                aiitem._expiry = packet.Expiry;
                aiitem._items = ArmaArray.Serialize(packet.Data.AsArray(0));
                aiitem._count = ArmaArray.Serialize(packet.Data.AsArray(1));
                aiitem.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating ai_items: {0}", id), Logger.LogType.Info, typeof (AiItems));
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
                    Logger.LogType.Error, typeof(AiItems));
                return "[0,\"Could not find ID\"]";
            }
            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[{0},{1}]']",
                    obj._items,
                    obj._count
                    );
            }
        }
    }




}