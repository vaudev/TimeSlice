namespace TimeSlice.ApiService.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T?> GetAsync( string ownerId, int? id );
        Task<List<T>> GetAllAsync( string ownerId );
        Task<T> AddAsync( string ownerId, T entity );
        Task UpdateAsync( string ownerId, T entity );
        Task DeleteAsync( string ownerId, int id );
        Task<bool> Exists( string ownerId, int id );
    }
}
