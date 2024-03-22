using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using TimeSlice.WebApp.Providers;
using TimeSlice.WebApp.Services.Base;
using TimeSlice.WebApp.Services.Timebox;

namespace TimeSlice.WebApp.Services.Auth
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly ApiService _apiService;
        private readonly ApiAuthenticationStateProvider _stateProvider;
        private string _ownerId = string.Empty;

        public AuthenticationService( ApiService httpClient, AuthenticationStateProvider stateProvider )
        {
            _apiService = httpClient;
            _stateProvider = (ApiAuthenticationStateProvider) stateProvider;
            _stateProvider.AuthenticationStateChanged += OnAuthenticationStateChanged;
            _ownerId = _stateProvider.GetOwnerId();
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

        private void OnAuthenticationStateChanged( Task<AuthenticationState> task )
        {
            _ownerId = _stateProvider.GetOwnerId();
        }

        public string GetOwnerId()
        {
            return _ownerId;
        }
    }
}
