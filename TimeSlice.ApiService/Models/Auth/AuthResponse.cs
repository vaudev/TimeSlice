namespace TimeSlice.ApiService.Models.Auth
{
    public class AuthResponse
    {
        public ApplicationUserDto User { get; set; }
        public bool Success { get; set; }
    }
}
