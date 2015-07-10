#region usings

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using EpoSql.Model;
using EpoSql.Util;

#endregion

namespace EpoSql.Core
{
    public class Manager
    {
        private static Manager _instance;

        public static Manager Instance
        {
            get { return _instance ?? (_instance = new Manager()); }
        }

        private static Settings _settings;

        public static Settings Settings
        {
            get { return _settings ?? (_settings = new Settings()); }
        }

        public static string Invoke(string input, int maxoutput)
        {
            return Instance._invoke(input, maxoutput);
        }

        public const char Seperator = '\n';

        private int _maxOutput = -1;

        private readonly Dictionary<string, Type> _tablemap = new Dictionary<string, Type>();
        private Dictionary<int, Task<string>> _tasks = new Dictionary<int, Task<string>>();
        private int _taskPointer;

        private Manager()
        {
            _taskPointer = 0;
            if (Settings.ShowConsole)
            {
                ConsoleHelper.CreateConsole();
            }
            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Logger.Log("Manager Started", Logger.LogType.Info, typeof(Manager));
            SessionFactory.Load();

            _tablemap.Add("I", typeof(Store));
            _tablemap.Add("PLAYERS", typeof(Store));
            _tablemap.Add("LOG", typeof(Log));

            _tablemap.Add("PLAYER", typeof(Player));
            _tablemap.Add("PLAYERSTATS", typeof(Player));
            _tablemap.Add("PLAYERDATA", typeof(PlayerData));
            _tablemap.Add("BANK", typeof(Bank));
            _tablemap.Add("GROUP", typeof(Group));

            _tablemap.Add("VEHICLE", typeof(Vehicle));
            _tablemap.Add("VEHICLELOCK", typeof(VehicleLock));
            _tablemap.Add("BUILDING", typeof(Building));
            _tablemap.Add("STORAGE", typeof(Storage));
            _tablemap.Add("PLOTPOLE", typeof(Plotpole));
            _tablemap.Add("LOCKBOXPIN", typeof(LockboxPin));

            _tablemap.Add("AI", typeof(Ai));
            _tablemap.Add("AI_ITEMS", typeof(AiItems));
        }

        private string _invoke(string packet, int maxoutput)
        {
            if (_maxOutput == -1)
                _maxOutput = maxoutput - 100;

            if (packet.StartsWith("LOAD"))
                return "[1,\"Done\"]";

            if (_settings.LogRequests)
                Logger.Log(String.Format("Request: {0} | Output Size: {1}", packet.Replace("\n", ", "), _maxOutput),
                    Logger.LogType.Info, typeof(Manager));

            var response = _parse(packet);

            if (response.Length > _maxOutput)
            {
                var tid = Interlocked.Increment(ref _taskPointer);
                var task = new Task<string>(() => { return response; });
                var parts = (int)Math.Ceiling((double)response.Length / (double)_maxOutput);

                _tasks.Add(tid, task);
                task.Start();
                return String.Format("[2,{0},{1}]", tid, parts);
            }

            if (_settings.LogRequests)
                Logger.Log(String.Format("Response: {0}", response), Logger.LogType.Info, typeof(Manager));

            return response;
        }

        private string _parse(string packet)
        {
            Request request;
            try
            {
                request = new Request(packet);
            }
            catch (RequestException ex)
            {
                Logger.Log(String.Format("Error parsing request: {0}", ex.Message), Logger.LogType.Error,
                    typeof(Manager));
                return "[0,\"Request is malformed\"]";
            }
            var response = "[1,\"Done\"]";
            switch (request.Command)
            {
                case Request.Commands.Set:
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Set(request);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(String.Format("Set Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                                typeof(Manager));
                        }
                    });
                    response = "[1,\"Done\"]";
                    break;

                case Request.Commands.GetAsync:
                    var t = Task<string>.Factory.StartNew(() =>
                    {
                        try
                        {
                            return Get(request);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(String.Format("GetAsync Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                                typeof(Manager));
                            return "[0,\"Error\"]";
                        }
                    });
                    var tid = Interlocked.Increment(ref _taskPointer);
                    _tasks.Add(tid, t);
                    response = String.Format("[1,{0}]", tid);
                    break;

                case Request.Commands.GetSync:
                    try
                    {
                        response = Get(request);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("GetSync Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                            typeof(Manager));
                        response = "[0,\"Error\"]";
                    }
                    break;

                case Request.Commands.GetBit:
                    try
                    {
                        response = GetBit(request);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("GetBit Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                            typeof(Manager));
                        response = "[0,\"Error\"]";
                    }
                    break;

                case Request.Commands.Del:
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Del(request);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(String.Format("Del Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                                typeof(Manager));
                        }
                    });
                    break;

                case Request.Commands.Expire:
                    Task.Factory.StartNew(() =>
                    {
                        try
                        {
                            Expire(request);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log(String.Format("Expire Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                                typeof(Manager));
                        }
                    });
                    break;

                case Request.Commands.Response:
                    try
                    {
                        response = Response(request);
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("Response Error: {0} - {1}", ex.Message, packet), Logger.LogType.Error,
                            typeof(Manager));
                        response = "[0,\"Error\"]";
                    }
                    break;
            }
            return response;
        }

        private void Set(Request packet)
        {
            if (!_tablemap.ContainsKey(packet.Table))
            {
                throw new Exception(String.Format("Unknown table: {0}", packet.Table));
            }
            var cls = _tablemap[packet.Table];

            var m = cls.GetMethod("Set", BindingFlags.Public | BindingFlags.Static);
            if (m != null)
            {
                m.Invoke(null, new object[] { packet });
            }
            else
            {
                throw new Exception(String.Format("The requested object does not implement Set"));
            }
        }

        private string Get(Request packet)
        {
            if (!_tablemap.ContainsKey(packet.Table))
            {
                throw new Exception(String.Format("Unknown table: {0}", packet.Table));
            }
            var cls = _tablemap[packet.Table];

            var m = cls.GetMethod("Get", BindingFlags.Public | BindingFlags.Static);
            if (m != null)
            {
                return (m.Invoke(null, new object[] { packet })) as string;
            }
            throw new Exception(String.Format("The requested object does not implement Get"));
        }

        private string GetBit(Request packet)
        {
            if (!_tablemap.ContainsKey(packet.Table))
            {
                throw new Exception(String.Format("Unknown table: {0}", packet.Table));
            }
            var cls = _tablemap[packet.Table];

            var m = cls.GetMethod("GetBit", BindingFlags.Public | BindingFlags.Static);
            if (m != null)
            {
                return (m.Invoke(null, new object[] { packet })) as string;
            }
            throw new Exception(String.Format("The requested object does not implement GetBit"));
        }

        private void Expire(Request packet)
        {
            if (!_tablemap.ContainsKey(packet.Table))
            {
                throw new Exception(String.Format("Unknown table: {0}", packet.Table));
            }
            var cls = _tablemap[packet.Table];

            var m = cls.GetMethod("Expire", BindingFlags.Public | BindingFlags.Static);
            if (m != null)
            {
                m.Invoke(null, new object[] { packet });
            }
            else
            {
                throw new Exception(String.Format("The requested object does not implement Expire"));
            }
        }

        private void Del(Request packet)
        {
            if (!_tablemap.ContainsKey(packet.Table))
            {
                throw new Exception(String.Format("Unknown table: {0}", packet.Table));
            }
            var cls = _tablemap[packet.Table];

            var m = cls.GetMethod("Del", BindingFlags.Public | BindingFlags.Static);
            if (m != null)
            {
                m.Invoke(null, new object[] {packet});
            }
            else
            {
                throw new Exception(String.Format("The requested object does not implement Del"));
            }
        }

        private string Response(Request packet)
        {
            Task<string> task;
            if (!_tasks.TryGetValue(packet.Tid, out task))
            {
                throw new Exception(String.Format("Could not find task with id: {0}", packet.Tid));
            }

            if (!task.IsCompleted)
            {
                return "WAIT";
            }

            var response = task.Result;
            if (response.Length < _maxOutput)
            {
                _tasks.Remove(packet.Tid);
                return response;
            }

            var parts = (int)Math.Ceiling((double)response.Length / (double)_maxOutput);
            var part = packet.Part;

            if (part >= parts)
            {
                throw new Exception(String.Format("Trying to get part that is out of bounds: {0}", String.Join(", ", packet)));
            }
            var start = _maxOutput * part;
            var length = _maxOutput;

            if ((start + length) >= response.Length)
            {
                length = response.Length - start;
                _tasks.Remove(packet.Tid);
            }

            return response.Substring(start, length);
        }

        private static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            Kill();
        }

        private static void Kill()
        {
            Logger.Log("Stopping Manager", Logger.LogType.Info, typeof(Manager));
            SessionFactory.Kill();
            Logger.Log("Stopped Manager", Logger.LogType.Info, typeof(Manager));
        }
    }
}