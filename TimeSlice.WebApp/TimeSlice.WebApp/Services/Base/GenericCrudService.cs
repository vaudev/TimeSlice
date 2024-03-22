using Blazored.LocalStorage;
using TimeSlice.WebApp.Services.Auth;

namespace TimeSlice.WebApp.Services.Base
{
    public abstract class GenericCrudService<T> : BaseHttpService, IGenericCrudService<T> where T : class
    {
        private readonly IAuthenticationService _authService;

        public GenericCrudService(ApiService client, ILocalStorageService localStorage, IAuthenticationService authService ) : base( client, localStorage )
        {
            _authService = authService;
        }

        private void AssertOwner()
        {
            if (GetOwnerId() == string.Empty)
            {
                throw new Exception( "No owner is set for this service. Please set the owner before calling any CRUD operations." );                
            }
        }

        public async Task<Response<int>> Create( T entry )
        {
            AssertOwner();

            Response<int> response = new Response<int>();

            try
            {
                await GetBearerToken();
                var data = await InternalPostAsync( GetOwnerId(), entry );
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
            AssertOwner();

            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await InternalDeleteAsync( GetOwnerId(), id );
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
            AssertOwner();

            Response<List<T>> response;

            try
            {
                await GetBearerToken();
                var data = await InternalGetAll( GetOwnerId() );
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
            AssertOwner();

            Response<T> response = new Response<T>();

            try
            {
                await GetBearerToken();
                var data = await InternalGetAsync( GetOwnerId(), id );
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

        public async Task<Response<int>> Update(int id, T entry )
        {
            AssertOwner();

            Response<int> response = new();

            try
            {
                await GetBearerToken();
                await InternalPutAsync( GetOwnerId(), id, entry );
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

        private string GetOwnerId()
        {
            return _authService.GetOwnerId();
        }

        protected abstract System.Threading.Tasks.Task<T> InternalPostAsync( string ownerId, T dto );
        protected abstract System.Threading.Tasks.Task InternalDeleteAsync( string ownerId,  int id );
        protected abstract System.Threading.Tasks.Task<T> InternalGetAsync( string ownerId, int id );
        protected abstract System.Threading.Tasks.Task<System.Collections.Generic.ICollection<T>> InternalGetAll( string ownerId);
        protected abstract System.Threading.Tasks.Task InternalPutAsync( string ownerId, int id, T dto );
    }
}
