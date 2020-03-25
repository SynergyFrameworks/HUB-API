namespace Hub.Transactions.WebAPI.Data
{
    public interface INeedPath
    {
        ICanMigrate WithPath(string path);
    }
}