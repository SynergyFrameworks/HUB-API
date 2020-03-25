using System.Collections.Generic;

namespace Hub.Transactions.WebAPI.Data
{
    public interface IMigrationDatabase
    {
        List<MigrationDTO> GetMigrations();
        void ExecuteScript(string script);
        void InsertMigration(Migration migration);
        void RemoveMigration(string id);
    }
}