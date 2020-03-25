using System.Collections.Generic;
using System.Linq;

namespace Hub.Transactions.WebAPI.Data
{
    public class MigrationDestination
    {
        private readonly IMigrationDatabase _database;
        private readonly List<MigrationDTO> _migrations;
        private readonly int _versionNumber;

        public MigrationDestination(IMigrationDatabase database)
        {
            _database = database;
            _migrations = _database.GetMigrations();
            _versionNumber = _migrations.Any() ? _migrations.Max(x => x.VersionNumber) : 0;
        }

        public string DatabaseType => _database.GetType().Name.Replace("MigrationDatabase", string.Empty);

        public int VersionNumber
        {
            get { return _versionNumber; }
        }

        public List<MigrationDTO> Migrations
        {
            get { return _migrations; }
        }

        public void MigrateUp(Migration migration)
        {
            _database.ExecuteScript(migration.UpScript);
            _database.InsertMigration(migration);
        }

        public void MigrateDown(MigrationDTO migration)
        {
            _database.ExecuteScript(migration.DownScript);
            _database.RemoveMigration(migration.Id);
        }
    }
}