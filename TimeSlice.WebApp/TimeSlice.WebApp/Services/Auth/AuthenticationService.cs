using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Linq;
using TimeSlice.WebApp.Providers;
using TimeSlice.WebApp.Services.Base;
using TimeSlice.WebApp.Static;

namespace TimeSlice.WebApp.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApiService _apiService;
        private readonly IMemoryCache _tokenStorage;
        private readonly ApiAuthenticationStateProvider _stateProvider;

        public AuthenticationService( ApiService httpClient, IMemoryCache tokenStorage, AuthenticationStateProvider stateProvider )
        {
            _apiService = httpClient;
            _tokenStorage = tokenStorage;
            _stateProvider = (ApiAuthenticationStateProvider) stateProvider;
        }

        public async Task<bool> RegisterOrLogin( string userName )
        {
            try
            {
                var result = await _apiService.RegisterorloginAsync( userName );

                if(result.Success)
                {
                    _tokenStorage.Set( Keys.AccessToken, result.Token, TimeSpan.FromHours(1) );
                    await _stateProvider.LoggedInAsync();
                }

                return result.Success;
            }
            catch (ApiException ex)
            {
                return false;
            }
        }

        public async Task Logout()
        {
            await _stateProvider.LoggedOut();
        }
    }
}
