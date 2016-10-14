using System;

namespace LeadGenerator
{
    public class DBExtension
    {
        private string _connectionString = "";

        public DBExtension(string connectionString)
        {
            _connectionString = connectionString;
        }

        public string ConnectionString { get { return _connectionString; } }
    }
}
