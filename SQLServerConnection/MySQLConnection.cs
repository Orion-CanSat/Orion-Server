using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace OrionServer.SQLServerConnection
{
    public class MySQLConnection : SQLConnection
    {
        private MySqlConnection connection;

        public MySQLConnection(string DatabaseIP = "localhost", string DatabaseID = "root", string DatabasePassword = "", string DatabaseName = "Orion_Database"):
            base(DatabaseIP, DatabaseID, DatabasePassword, DatabaseName) { }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public override async Task<bool> Open(bool throwExceptionOnFailure = true)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (!this.IsOpen)
                this.Close();

            var mySqlConnectionStringBuilder = new MySqlConnectionStringBuilder()
            {
                Server = this.DatabaseIP,
                UserID = this.DatabaseID,
                Password = this.DatabasePassword,
                Database = this.DatabaseName
            };

            this.connection = new MySqlConnection(mySqlConnectionStringBuilder.ToString());
            
            try
            {
                this.connection.Open();
                this.IsOpen = true;
                return this.IsOpen;
            }
            catch (Exception e)
            {
                if (throwExceptionOnFailure)
#pragma warning disable CA2200 // Rethrow to preserve stack details
                    throw e;
#pragma warning restore CA2200 // Rethrow to preserve stack details
                return false;
            }
        }

        public override void Close()
        {
            if (!this.IsOpen)
                return;

            this.IsOpen = false;
            this.connection.Close();
        }

        public override async Task<bool> Insert<T>(T instance)
        {
            if (!IsOpen)
                return false;

            Type instanceType = typeof(T);
            PropertyInfo[] instanceTypeProperties = instanceType.GetProperties();
            object[] instancePropertiesValue = new object[instanceTypeProperties.Length];

            for (int i = 0; i < instanceTypeProperties.Length; i++)
                instancePropertiesValue[i] = instanceTypeProperties[i].GetValue(instance, null);

            var stringQuery = $"INSERT INTO `{instanceType.Name}` (`ID`";
            foreach (PropertyInfo pi in instanceTypeProperties)
                stringQuery += $", `{pi.Name}`";
            stringQuery += ") VALUES (NULL";
            foreach (PropertyInfo pi in instanceTypeProperties)
               stringQuery += $", @{pi.Name}";
            stringQuery += ");";

            MySqlCommand sqlcmd = new MySqlCommand(stringQuery, this.connection);

            for (int i = 0; i < instanceTypeProperties.Length; i++)
            {
                object value = instancePropertiesValue[i];
                string valueString;

                if (value is DateTime time)
                    valueString = time.ToString("yyyy-MM-dd HH:mm:ss");
                else if (value == null)
                    valueString = "NULL";
                else
                    valueString = value.ToString();

                sqlcmd.Parameters.AddWithValue($"@{instanceTypeProperties[i].Name}", value);
            }

            try
            {
                await sqlcmd.ExecuteNonQueryAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return true;
        }

        public override async Task ExecuteNonQueryAsync(string query)
        {
            if (!this.IsOpen)
                return;
            MySqlCommand sqlcmd = new MySqlCommand(query, this.connection);
            await sqlcmd.ExecuteNonQueryAsync();
        }

        ~MySQLConnection()
        {
            if (this.IsOpen)
            {
                this.Close();
                this.IsOpen = false;
            }
        }
    }
}
