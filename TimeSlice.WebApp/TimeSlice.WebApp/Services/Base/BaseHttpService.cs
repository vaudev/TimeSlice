using Blazored.LocalStorage;
using System.Net.Http.Headers;
using TimeSlice.WebApp.Static;

namespace TimeSlice.WebApp.Services.Base
{
    public class BaseHttpService
    {
        protected readonly ApiService _client;
        protected readonly ILocalStorageService _localStorage;

        public BaseHttpService( ApiService client, ILocalStorageService localStorage )
        {
            _client = client;
            _localStorage = localStorage;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>( ApiException e )
        {
            if (e.StatusCode == 400)
            {
                return new Response<Guid>()
                {
                    Message = "Validation error occured",
                    ValidationErrors = e.Response
                };
            }
            else if (e.StatusCode == 404)
            {
                return new Response<Guid>()
                {
                    Message = "Not found",
                    ValidationErrors = e.Response
                };
            }

            if (e.StatusCode >= 200 && e.StatusCode <= 299)
            {
                return new Response<Guid>()
                {
                    Success = true
                };
            }

            return new Response<Guid>()
            {
                Message = "Something went wrong",
                ValidationErrors = e.Response
            };
        }

        public async Task GetBearerToken()
        {
            var token = await _localStorage.GetItemAsync<string>( Keys.AccessToken );
            if (token != null)
            {
                var authHeader = new AuthenticationHeaderValue( Keys.AuthScheme, token );

                // TODO: expose http client for default requestheaders.
                //_client.HttpClient.DefaultRequestHeaders.Authorization = authHeader;
            }
        }
    }
}
