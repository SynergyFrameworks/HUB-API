namespace Hub.Transactions.WebAPI.Data
{
    public class MigrationFile
    {
        public int VersionNumber { get; set; }

        public MigrationType Type { get; set; }

        public string Path { get; set; }
    }
}