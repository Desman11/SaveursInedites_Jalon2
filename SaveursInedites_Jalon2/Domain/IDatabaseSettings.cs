namespace SaveursInedites_Jalon2.Domain
{
    public interface IDatabaseSettings
    {
        string ConnectionString { get; set; }
        DatabaseProviderName? DatabaseProviderName { get; set; }
    }
}