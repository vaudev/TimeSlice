using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TimeSlice.WebApp.Static;

namespace TimeSlice.WebApp.Providers
{
    public class ApiAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _tokenStorage;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public ApiAuthenticationStateProvider( ILocalStorageService tokenStorage )
        {
            _tokenStorage = tokenStorage;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = new ClaimsPrincipal( new ClaimsIdentity() );

            var tokenString = await _tokenStorage.GetItemAsync<string>( Keys.AccessToken );
            if (tokenString == null)
            {
                return new AuthenticationState( user );
            }

            var jwtToken = _tokenHandler.ReadJwtToken( tokenString );
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return new AuthenticationState( user );
            }

            var claims = await GetClaims();

            user = new ClaimsPrincipal( new ClaimsIdentity( claims, "jwt" ) );

            return new AuthenticationState( user );
        }

        public async Task LoggedInAsync( string token )
        {
            await _tokenStorage.SetItemAsync( Keys.AccessToken, token );
            var claims = await GetClaims();
            var user = new ClaimsPrincipal( new ClaimsIdentity( claims, Keys.AuthType ) );
            var authState = Task.FromResult( new AuthenticationState( user ) );
            NotifyAuthenticationStateChanged( authState );
        }

        public async Task LoggedOut()
        {
            await _tokenStorage.RemoveItemAsync( Keys.AccessToken );
            var nobody = new ClaimsPrincipal( new ClaimsIdentity() );
            var authState = Task.FromResult( new AuthenticationState( nobody ) );
            NotifyAuthenticationStateChanged( authState );
        }

        private async Task<List<Claim>> GetClaims()
        {
            var savedToken = await _tokenStorage.GetItemAsync<string>( Keys.AccessToken );
            var tokenContent = _tokenHandler.ReadJwtToken( savedToken );
            var claims = tokenContent.Claims.ToList();
            claims.Add( new Claim( ClaimTypes.Name, tokenContent.Subject ) );
            return claims;
        }
    }
}
