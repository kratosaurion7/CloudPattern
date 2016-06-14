using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace CloudPatterns.ExternalConfig
{
    public class MySQLSettingStore : ISettingStore
    {
        private MySqlConnection _db;
        private string _configTableName;
        private string _configKeyColumnName;
        private string _configValueColumnName;

        public MySQLSettingStore(MySqlConnection connection, string configTableName, string configKeyColumnName, string configValueColumnName)
        {
            _db = connection;
            _configTableName = configTableName;
            _configKeyColumnName = configKeyColumnName;
            _configValueColumnName = configValueColumnName;
        }

        public string Version { get; private set; }

        public Dictionary<string, string> FindAll()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            MySqlCommand cmd = _db.CreateCommand();
            cmd.CommandText = String.Format("SELECT {0}, {1} FROM {2}", _configKeyColumnName, _configValueColumnName, _configTableName);
            cmd.CommandType = System.Data.CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.HasRows)
            {
                string keyName = rdr.GetString(_configKeyColumnName);
                string value = rdr.GetString(_configValueColumnName);

                result.Add(keyName, value);

                rdr.Read();
            }

            rdr.Close();

            return result;
        }

        public string Get(string key)
        {
            MySqlCommand cmd = _db.CreateCommand();
            cmd.CommandText = String.Format("SELECT {0} FROM {1} WHERE {2} = {3}", _configValueColumnName, _configTableName, _configKeyColumnName, key);
            cmd.CommandType = System.Data.CommandType.Text;
            MySqlDataReader rdr = cmd.ExecuteReader();

            string result = "";

            if (rdr.HasRows)
            {
                result = rdr.GetString(_configKeyColumnName);
            }
            else
            {
                result = null;
            }

            rdr.Close();

            return result; ;
        }

        public void Update(string key, string value)
        {
        }
    }
}
