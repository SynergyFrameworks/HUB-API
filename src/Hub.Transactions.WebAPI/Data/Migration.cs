using System;

namespace Hub.Transactions.WebAPI.Data
{
    public class Migration
    {
        public Migration()
        {
            Id = Guid.NewGuid().ToString();
            DateCreated = DateTime.Now;
        }

        public string Id { get; private set; }

        public int VersionNumber { get; set; }

        public string UpScript { get; set; }

        public string DownScript { get; set; }

        public DateTime DateCreated { get; set; }
    }
}