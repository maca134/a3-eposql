using EpoSql.Model;
using EpoSql.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace EpoSql.Core
{
    class ArmaParser
    {

        private Dictionary<string, Func<List<string>, string>> _cmdmap = new Dictionary<string, Func<List<string>, string>>();
        private Dictionary<string, Type> _tablemap = new Dictionary<string, Type>();
        private List<Task<string>> _responses = new List<Task<string>>();
        private readonly char _seperator = '\n';

        public ArmaParser()
        {
            _cmdmap.Add("GET", Get);
            _cmdmap.Add("SET", Set);
            _cmdmap.Add("DEL", Del);
            _cmdmap.Add("EXPIRE", Expire);
            _cmdmap.Add("RESPONSE", Response);

            _tablemap.Add("I", typeof(Model.Store));
            _tablemap.Add("PLAYERS", typeof(Model.Store));

            _tablemap.Add("PLAYER", typeof(Model.Player));
            _tablemap.Add("PLAYERSTATS", typeof(Model.Player));
            _tablemap.Add("PLAYERDATA", typeof(Model.PlayerData));
            _tablemap.Add("BANK", typeof(Model.Bank));

            _tablemap.Add("VEHICLE", typeof(Model.Vehicle));
            _tablemap.Add("VEHICLELOCK", typeof(Model.VehicleLock));
            _tablemap.Add("BUILDING", typeof(Model.Building));
            _tablemap.Add("STORAGE", typeof(Model.Storage));

            _tablemap.Add("AI", typeof(Model.AI));
            _tablemap.Add("AI_ITEMS", typeof(Model.AI_Items));
        }

        
    }
}
