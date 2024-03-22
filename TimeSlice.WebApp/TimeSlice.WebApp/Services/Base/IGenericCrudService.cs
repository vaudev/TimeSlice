namespace TimeSlice.WebApp.Services.Base
{
    public interface IGenericCrudService<T> where T : class
    {
        Task<Response<List<T>>> GetAll();
        Task<Response<int>> Create( T entry );
        Task<Response<T>> Get( int id );
        Task<Response<int>> Update( int id, T entry );
        Task<Response<int>> Delete( int id );
    }
}
