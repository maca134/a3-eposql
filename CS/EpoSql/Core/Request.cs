#region usings

using System;
using System.Linq;

#endregion

namespace EpoSql.Core
{
    public class Request
    {
        public enum Commands
        {
            GetSync,
            GetAsync,
            GetBit,
            Set,
            Del,
            Expire,
            Response,
            Unknown
        }

        private readonly Commands _command;

        public Commands Command
        {
            get { return _command; }
        }

        private readonly string _table;

        public string Table
        {
            get { return _table; }
        }

        private readonly string _instanceId;

        public string InstanceId
        {
            get { return _instanceId; }
        }

        private readonly int _expiry = -1;

        public int Expiry
        {
            get { return _expiry; }
        }

        private readonly int _bit;
        public int Bit
        {
            get { return _bit; }
        }

        private readonly int _tid;
        public int Tid
        {
            get { return _tid; }
        }

        private readonly int _part;
        public int Part
        {
            get { return _part; }
        }

        private readonly ArmaArray _data;

        public ArmaArray Data
        {
            get { return _data; }
        }

        public Request(string request)
        {
            var parts = request.Split(Manager.Seperator);

            switch (parts[0].ToUpper())
            {
                case "SET":
                    _command = Commands.Set;
                    break;
                case "GETSYNC":
                    _command = Commands.GetSync;
                    break;
                case "GETASYNC":
                    _command = Commands.GetAsync;
                    break;
                case "DEL":
                    _command = Commands.Del;
                    break;
                case "EXPIRE":
                    _command = Commands.Expire;
                    break;
                case "GETBIT":
                    _command = Commands.GetBit;
                    break;
                case "RESPONSE":
                    _command = Commands.Response;
                    break;
                default:
                    throw new RequestException("Unknown packet type");
                    break;
            }

            try
            {
                switch (_command)
                {
                    case Commands.Set:
                        _table = parts[1].ToUpper();
                        _instanceId = parts[2];
                        _expiry = Convert.ToInt32(parts[3]);
                        _data = ArmaArray.Unserialize(parts[4]);
                        break;

                    case Commands.GetSync:
                    case Commands.GetAsync:
                    case Commands.Del:
                        _table = parts[1].ToUpper();
                        _instanceId = parts[2];
                        break;

                    case Commands.Expire:
                        _table = parts[1].ToUpper();
                        _instanceId = parts[2];
                        _expiry = Convert.ToInt32(parts[3]);
                        break;

                    case Commands.GetBit:
                        _table = parts[1].ToUpper();
                        _instanceId = parts[2];
                        _bit = Convert.ToInt32(parts[3]);
                        break;

                    case Commands.Response:
                        _tid = Convert.ToInt32(parts[1]);
                        _part = Convert.ToInt32(parts[2]);
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new RequestException(String.Format("Request Error: {0} - {1}", ex.Message, request));
            }
        }
    }
}