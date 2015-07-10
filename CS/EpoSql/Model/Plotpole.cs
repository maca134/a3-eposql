#region usings

using System;
using System.Collections.Generic;
using EpoSql.Core;
using EpoSql.Util;

#endregion

namespace EpoSql.Model
{
    public class Plotpole : Base
    {
        #region DBProps

        private string _names = "[]";

        public virtual string Names
        {
            get { return _names; }
            set { _names = value; }
        }

        private string _pids = "[]";

        public virtual string Pids
        {
            get { return _pids; }
            set { _pids = value; }
        }

        #endregion

        private static readonly Dictionary<long, Plotpole> _objStore = new Dictionary<long, Plotpole>();
        private static readonly object TypeLocker = new object();

        private static Plotpole GetObject(long id)
        {
            lock (TypeLocker)
            {

                Plotpole obj;
                if (Manager.Settings.CacheData)
                {
                    if (_objStore.TryGetValue(id, out obj))
                    {
                        return obj;
                    }
                }
                using (var session = SessionFactory.OpenSession())
                {
                    obj = session.Get<Plotpole>(id);
                }
                if (obj != null)
                {
                    obj.IsNew = false;
                    if (Manager.Settings.CacheData)
                        _objStore.Add(id, obj);
                    return obj;
                }
                obj = new Plotpole()
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
                        session.Delete<Plotpole>(id);
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
            Logger.Log(String.Format("Deleting: {0}", id), Logger.LogType.Info, typeof(Plotpole));
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
                    Logger.LogType.Error, typeof(Plotpole));
                return "[0,\"Could not find ID\"]";
            }

            if (packet.Data.Count != 2)
            {
                Logger.Log(String.Format("Parsing error: {0}", packet.Data), Logger.LogType.Error,
                    typeof(Plotpole));
                return "[0,\"The request was malformed\"]";
            }

            var plotpole = GetObject(id);

            lock (plotpole.Locker)
            {
                plotpole._expiry = packet.Expiry;
                plotpole._names = ArmaArray.Serialize(packet.Data.AsArray(0));
                plotpole._pids = ArmaArray.Serialize(packet.Data.AsArray(1));
                plotpole.SaveOrUpdate();
            }

            Logger.Log(String.Format("Updating Plotpole: {0}", id), Logger.LogType.Info, typeof(Plotpole));

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
                    Logger.LogType.Error, typeof(Plotpole));
                return "[0,\"Could not find ID\"]";
            }

            var obj = GetObject(id);
            if (obj.IsNew)
                return "[1,'[]']";
            lock (obj.Locker)
            {
                return String.Format("[1,'[{0},{1}]']",
                    obj._names,
                    obj._pids
                    );
            }
        }
    }
}