using PetaPoco;
using StackExchange.Profiling;
using StackExchange.Profiling.Data;
using System.Data;
using System.Data.Common;

namespace Hub.Transactions.WebAPI.Data
{
    public class ProfiledDatabase : Database
    {
        public ProfiledDatabase(IDbConnection connection) : base(connection) { }
        public ProfiledDatabase(string connectionString, string providerName) : base(connectionString, providerName) { }
        public ProfiledDatabase(string connectionString, DbProviderFactory dbProviderFactory) : base(connectionString, dbProviderFactory) { }

        public override IDbConnection OnConnectionOpened(IDbConnection connection)
        {
            return new ProfiledDbConnection((DbConnection)connection, MiniProfiler.Current);
        }
    }
}
