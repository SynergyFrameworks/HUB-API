using System.IO;
using System.Linq;
using Serilog;

namespace Hub.Transactions.WebAPI.Data
{
    public class Migrator : ICanMigrate, INeedPath
    {
        private readonly MigrationDestination _destination;
        private MigrationSource _source;
        private string _path;

        private Migrator(IMigrationDatabase database)
        {
            _destination = new MigrationDestination(database);
        }

        public static INeedPath ForDatabase(IMigrationDatabase database)
        {
            var migrator = new Migrator(database);
            
            return migrator;
        }

        public ICanMigrate WithPath(string path)
        {
            _path = path;

            return this;
        }

        public void Migrate()
        {
            _source = new MigrationSource(_path);

            if (_destination.VersionNumber < _source.VersionNumber)
            {
                Log.Information(
                    "{DatabaseType:l} migration required. Migrating: {DestinationVersion} ==> {SourceVersion}",
                    _destination.DatabaseType,
                    _destination.VersionNumber,
                    _source.VersionNumber);

                MigrateUp();
            }
            else if (_source.VersionNumber < _destination.VersionNumber)
            {
                //MigrateDown();
            }
            else
            {
                Log.Information("{DatabaseType:l} migration not required. Database is at current version: {Version}",
                    _destination.DatabaseType,
                    _destination.VersionNumber);
            }
        }

        private void MigrateDown()
        {
            var migrations = _destination.Migrations
                .Where(x => x.VersionNumber > _source.VersionNumber)
                .OrderByDescending(x => x.VersionNumber);

            foreach (var migration in migrations)
            {
                _destination.MigrateDown(migration);
            }
        }

        private void MigrateUp()
        {
            var upFiles = _source.Files
                .Where(x => x.Type == MigrationType.Up && x.VersionNumber > _destination.VersionNumber)
                .OrderBy(x => x.VersionNumber);

            foreach (var upFile in upFiles)
            {
                var upScript = File.ReadAllText(upFile.Path);
                var downFile = _source.Files.SingleOrDefault(x => x.Type == MigrationType.Down && x.VersionNumber == upFile.VersionNumber);
                var downScript = downFile == null ? string.Empty : File.ReadAllText(downFile.Path);
                var migration = new Migration
                {
                    VersionNumber = upFile.VersionNumber,
                    UpScript = upScript,
                    DownScript = downScript
                };

                _destination.MigrateUp(migration);
            }
        }
    }
}