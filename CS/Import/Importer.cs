using EpoSql.Core;
using EpoSql.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Import
{
    public class Importer
    {
        private static Redis _client = null;
        public static Redis GetClient(ApplicationArguments options)
        {
            if (_client != null)
                return _client;

            _client = new Redis(options.Host, options.Port);
            _client.Password = options.Password;

            try
            {
                _client.Db = options.DbID;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Setting database: " + ex.Message);
                Environment.Exit(1);
            }
            return _client;
        }

        public static void Run(ApplicationArguments options)
        {
            ImportItems(options);
            ImportPlayerStats(options);
        }

        private static void ImportItems(ApplicationArguments options)
        {
            Redis redis = GetClient(options);
            string[] ikeyp = new string[] { "Building", "Plotpole", "Storage", "Vehicle", "AI", "AI_Items", "Bank", "Group", "Player", "PlayerData", "PlayerData" };

            foreach (string ik in ikeyp)
            {
                string[] keys = new string[] { };
                try
                {
                    keys = redis.GetKeys(String.Format("{0}:*", ik));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error Getting Keys: " + ex.Message);
                    continue;
                }
                foreach (string key in keys)
                {
                    try
                    {
                        int ttl = redis.TimeToLive(key);
                        string rstring = Encoding.UTF8.GetString(redis.Get(key));
                        Manager.Invoke(String.Format("SET\n{0}\n{1}\n{2}\n{3}", ik, key.Replace(ik + ":", ""), ttl, rstring), 10000);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(String.Format("Error getting key {0}: {1}", key, ex.Message));
                    }
                }

            }
        }

        private static void ImportPlayerStats(ApplicationArguments options)
        {
            Redis redis = GetClient(options);
            string[] keys = new string[] { };
            try
            {
                keys = redis.GetKeys("PlayerStats:*");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Getting Keys: " + ex.Message);
                return;
            }
            foreach (string key in keys)
            {
                byte[] d = redis.Get(key);
                int ttl;
                if (d[0] == 128)
                {
                    ttl = redis.TimeToLive(key);
                    Manager.Invoke(String.Format("SET\nPlayerStats\n{0}\n{1}\n[0,1]\n", key.Replace("PlayerStats:", ""), ttl), 10000);
                }
                else
                {
                    ttl = redis.TimeToLive(key);
                    Manager.Invoke(String.Format("SET\nPlayerStats\n{0}\n{1}\n[0,0]\n", key.Replace("PlayerStats:", ""), ttl), 10000);
                }
            }
        }
    }
}
