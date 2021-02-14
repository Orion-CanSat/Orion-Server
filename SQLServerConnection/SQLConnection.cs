using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OrionServer.SQLServerConnection
{
    
    public class SQLConnection
    {
        internal string DatabaseIP = null;
        internal string DatabaseID = null;
        internal string DatabasePassword = null;
        internal string DatabaseName = null;
        internal bool IsOpen = false;

        public SQLConnection(string DatabaseIP = "localhost", string DatabaseID = "root", string DatabasePassword = "", string DatabaseName = "Orion_Database")
        {
            this.DatabaseIP = DatabaseIP;
            this.DatabaseID = DatabaseID;
            this.DatabasePassword = DatabasePassword;
            this.DatabaseName = DatabaseName;

            try
            {
                Task<bool> testConnectionTask = TestConnection();
                if (!testConnectionTask.IsCompleted)
                    testConnectionTask.Start();
                testConnectionTask.Wait();
                if (!testConnectionTask.Result)
                    throw new System.IO.IOException("Could not open connection to SQL Server");
            }
            catch (Exception e)
            {
#pragma warning disable CA2200 // Rethrow to preserve stack details
                throw e;
#pragma warning restore CA2200 // Rethrow to preserve stack details
            }
        }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<bool> Open(bool throwExceptionOnFailure = true) { if (throwExceptionOnFailure) { throw new Exception("Can not open Base SQLConnection Object"); } return false; }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual void Close() { }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task<bool> Insert<T>(T instance) { return false; }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public virtual async Task ExecuteNonQueryAsync(string query) { return; }
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        public void ExecuteNonQuery(string query)
        {
            Task executeNonQueryAsyncTask = this.ExecuteNonQueryAsync(query);
            executeNonQueryAsyncTask.RunSynchronously();
            executeNonQueryAsyncTask.Wait();
            return;
        }

        private async Task<bool> TestConnection() { if (await this.Open(false)) { this.Close(); return true; } else return false; }

        ~SQLConnection()
        {
            if (this.IsOpen)
            {
                this.Close();
                this.IsOpen = false;
            }
        }
    }
}
