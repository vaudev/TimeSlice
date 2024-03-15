namespace TimeSlice.WebApp.Services.Auth
{
    public interface IAuthenticationService
    {
        Task<bool> RegisterOrLogin( string userName );
        Task Logout();
    }
}
