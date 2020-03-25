using PetaPoco;
using System.Collections.Generic;
using System.Linq;

namespace Hub.Transactions.WebAPI.Data
{
    public class PostgreSQLMigrationDatabase : IMigrationDatabase
    {
        private readonly IDatabase _db;

        public PostgreSQLMigrationDatabase(IDatabase db)
        {
            _db = db;
        }

        public List<MigrationDTO> GetMigrations()
        {
            CreateMigrationsTableIfDoesNotExist();

            const string sql = "SELECT id AS Id, " +
                               "version_number AS VersionNumber, " +
                               "up_script AS UpScript, " +
                               "down_script AS DownScript " +
                               "FROM cs_migrations";

            var migrations = _db.Query<MigrationDTO>(sql).ToList();

            return migrations;
        }

        private void CreateMigrationsTableIfDoesNotExist()
        {
            const string sql = "CREATE TABLE IF NOT EXISTS public.cs_migrations (" +
                               "id CHAR(36) NOT NULL, " +
                               "version_number INTEGER NOT NULL, " +
                               "up_script TEXT, " +
                               "down_script TEXT, " +
                               "date_created TIMESTAMP WITHOUT TIME ZONE, " +
                               "CONSTRAINT cs_migrations_id_pk PRIMARY KEY (id))";

            _db.Execute(sql);
        }

        public void ExecuteScript(string script)
        {
            _db.Execute(script);
        }

        public void InsertMigration(Migration migration)
        {
            var poco = new
            {
                id = migration.Id,
                version_number = migration.VersionNumber,
                up_script = migration.UpScript,
                down_script = migration.DownScript,
                date_created = migration.DateCreated.ToUniversalTime()
            };
            
            _db.Insert("cs_migrations", "id", false, poco);
        }

        public void RemoveMigration(string id)
        {
            _db.Delete("cs_migrations", "id", null, id);
        }
    }
}