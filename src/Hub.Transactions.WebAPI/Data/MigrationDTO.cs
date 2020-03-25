namespace Hub.Transactions.WebAPI.Data
{
    public class MigrationDTO
    {
        public string Id { get; set; }

        public int VersionNumber { get; set; }

        public string UpScript { get; set; }

        public string DownScript { get; set; }
    }
}