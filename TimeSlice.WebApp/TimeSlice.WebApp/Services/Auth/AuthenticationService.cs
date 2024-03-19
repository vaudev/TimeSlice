using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Caching.Memory;
using TimeSlice.WebApp.Providers;
using TimeSlice.WebApp.Services.Base;
using TimeSlice.WebApp.Static;

namespace TimeSlice.WebApp.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApiService _apiService;
        private readonly ApiAuthenticationStateProvider _stateProvider;

        public AuthenticationService( ApiService httpClient, AuthenticationStateProvider stateProvider )
        {
            _apiService = httpClient;
            _stateProvider = (ApiAuthenticationStateProvider) stateProvider;
        }

        public async Task<bool> RegisterOrLogin( string userName )
        {
            try
            {
                var result = await _apiService.RegisterorloginAsync( userName );

                if(result.Success)
                {
                    await _stateProvider.LoggedInAsync( result.Token );
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
