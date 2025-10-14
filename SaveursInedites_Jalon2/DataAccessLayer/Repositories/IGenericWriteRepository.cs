namespace SaveursInedites_Jalon2.DataAccessLayer.Repositories
{
    public interface IGenericWriteRepository<TKey, TEntity>
    {
        Task<TEntity> CreateAsync(TEntity entity);
        Task<TEntity> ModifyAsync(TEntity entity);
        Task<bool> DeleteAsync(TKey key);
    }
}
