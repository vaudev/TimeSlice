using Blazored.LocalStorage;

namespace TimeSlice.WebApp.Services.Base
{
    public abstract class GenericCrudService<T> : BaseHttpService, IGenericCrudService<T> where T : class
    {

        public GenericCrudService(ApiService client, ILocalStorageService localStorage ) : base( client, localStorage )
        {
        }

        public async Task<Response<int>> Create( T entry )
        {
            Response<int> response = new Response<int>();

            try
            {
                await GetBearerToken();
                var data = await InternalPostAsync( entry );
                response.Success = true;
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<int>( e );
            }

            return response;
        }

        public async Task<Response<int>> Delete( int id )
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await InternalDeleteAsync( id );
                response = new Response<int>()
                {
                    Success = true
                };
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<int>( e );
            }

            return response;
        }

        public async Task<Response<List<T>>> GetAll()
        {
            Response<List<T>> response;

            try
            {
                await GetBearerToken();
                var data = await InternalGetAll();
                response = new Response<List<T>>()
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<List<T>>( e );
            }

            return response;
        }

        public async Task<Response<T>> Get( int id )
        {
            Response<T> response = new Response<T>();

            try
            {
                await GetBearerToken();
                var data = await InternalGetAsync( id );
                response = new Response<T>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<T>( e );
            }

            return response;
        }

        public async Task<Response<int>> Update( int id, T entry )
        {
            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await InternalPutAsync( id, entry );
                response = new Response<int>()
                {
                    Success = true
                };
            }
            catch (ApiException e)
            {
                response = ConvertApiExceptions<int>( e );
            }

            return response;
        }

        protected abstract System.Threading.Tasks.Task<T> InternalPostAsync( T dto );
        protected abstract System.Threading.Tasks.Task InternalDeleteAsync( int id );
        protected abstract System.Threading.Tasks.Task<T> InternalGetAsync( int id );
        protected abstract System.Threading.Tasks.Task<System.Collections.Generic.ICollection<T>> InternalGetAll();
        protected abstract System.Threading.Tasks.Task InternalPutAsync( int id, T dto );

    }
}
