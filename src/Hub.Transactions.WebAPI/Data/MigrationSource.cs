using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Hub.Transactions.WebAPI.Data
{
    public class MigrationSource
    {
        private readonly string _path;

        public MigrationSource(string path)
        {
            _path = path;
            Files = GetMigrationFiles();
            VersionNumber = GetVersionNumber();
        }

        private int GetVersionNumber()
        {
            var files = Files.Where(x => x.Type == MigrationType.Up).ToList();
            var versionNumber = files.Any() ? files.Max(x => x.VersionNumber) : 0;

            return versionNumber;
        }

        public int VersionNumber { get; private set; }

        public List<MigrationFile> Files {get; private set; }

        private List<MigrationFile> GetMigrationFiles()
        {
            var migrationFiles = new List<MigrationFile>();
            var files = Directory.Exists(_path) ? Directory.GetFiles(_path) : new string[0];

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var match = Regex.Match(fileName, @"(\d+)\.(up|down)\.(js|sql)", RegexOptions.IgnoreCase);

                if (match.Success == false)
                {
                    continue;
                }

                var version = int.Parse(match.Groups[1].Value);
                var type = (MigrationType) Enum.Parse(typeof (MigrationType), match.Groups[2].Value, true);
                var migrationFile = new MigrationFile
                {
                    VersionNumber = version,
                    Path = file,
                    Type = type
                };

                migrationFiles.Add(migrationFile);
            }

            return migrationFiles;
        }
    }
}