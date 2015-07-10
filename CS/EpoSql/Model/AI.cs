#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Ai : Base
    {
        #region DBProps

        private string _class = "";

        public virtual string Class
        {
            get { return _class; }
            set { _class = value; }
        }

        private string _home = "[]";

        public virtual string Home
        {
            get { return _home; }
            set { _home = value; }
        }

        private string _work = "[]";

        public virtual string Work
        {
            get { return _work; }
            set { _work = value; }
        }

        #endregion

        private static readonly Dictionary<long, Ai> _objStore = new Dictionary<long, Ai>();
        private static readonly object TypeLocker = new object();

        private static Ai GetObject(long id)
        {
            lock (TypeLocker)
            {

                Ai obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Ai>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Ai()
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
                        session.Delete<Ai>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Ai));
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
                    Logger.LogType.Error, typeof(Ai));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 3)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Ai));
                return "[0,\"The request was malformed\"]";
            }

            var ai = GetObject(id);
            lock (ai.Locker)
            {
                ai._expiry = packet.Expiry;
                ai._class = packet.Data.AsString(0);
                ai._home = ArmaArray.Serialize(packet.Data.AsArray(1));
                ai._work = ArmaArray.Serialize(packet.Data.AsArray(2));
                ai.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating ai: {0}", id), Logger.LogType.Info, typeof(Ai));
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
                    Logger.LogType.Error, typeof(Ai));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[\"{0}\",{1},{2}]']",
                    obj._class,
                    obj._home,
                    obj._work
                    );
            }
        }
    }
}