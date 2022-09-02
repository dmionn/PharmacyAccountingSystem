using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyAccountingSystem
{
    internal class DatabaseContext
    {

        public DatabaseContext()
        {
            dosmth();
        }

        private void dosmth()
        {
            SQLiteConnection sqlite_conn;
            sqlite_conn = new SQLiteConnection(LoadConnectionString());
            try
            {
                sqlite_conn.Open();

                SQLiteCommand sqlite_cmd;
                sqlite_cmd = sqlite_conn.CreateCommand();
                sqlite_cmd.CommandText = "INSERT INTO Products (Name) VALUES ('новый продукт3');";
                sqlite_cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

            }

            sqlite_conn.Close();
        }

        private string LoadConnectionString(string id = "Default")
            => ConfigurationManager.ConnectionStrings[id].ConnectionString;

    }
}
