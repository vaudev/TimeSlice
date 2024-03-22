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
        private ClaimsPrincipal? _user = null;

        public ApiAuthenticationStateProvider( ILocalStorageService tokenStorage )
        {
            _tokenStorage = tokenStorage;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public string GetOwnerId()
        {
            if( _user == null )
            {
                return string.Empty;
            }

            var uidClaim = _user.Claims.FirstOrDefault( c => c.Type == "uid" );
            return uidClaim?.Value ?? string.Empty;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            _user = new ClaimsPrincipal( new ClaimsIdentity() );

            var tokenString = await _tokenStorage.GetItemAsync<string>( Keys.AccessToken );
            if (tokenString == null)
            {
                return new AuthenticationState( _user );
            }

            var jwtToken = _tokenHandler.ReadJwtToken( tokenString );
            if (jwtToken.ValidTo < DateTime.UtcNow)
            {
                return new AuthenticationState( _user );
            }

            var claims = await GetClaims();

            _user = new ClaimsPrincipal( new ClaimsIdentity( claims, "jwt" ) );

            return new AuthenticationState( _user );
        }

        public async Task LoggedInAsync( string token )
        {
            await _tokenStorage.SetItemAsync( Keys.AccessToken, token );
            var claims = await GetClaims();
            _user = new ClaimsPrincipal( new ClaimsIdentity( claims, Keys.AuthType ) );
            var authState = Task.FromResult( new AuthenticationState( _user ) );
            NotifyAuthenticationStateChanged( authState );
        }

        public async Task LoggedOut()
        {
            await _tokenStorage.RemoveItemAsync( Keys.AccessToken );
            _user = new ClaimsPrincipal( new ClaimsIdentity() );
            var authState = Task.FromResult( new AuthenticationState( _user ) );
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
