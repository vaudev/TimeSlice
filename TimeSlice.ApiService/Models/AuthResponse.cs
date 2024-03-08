namespace BookStoreApp.API.Models.User
{
    public class AuthResponse
    {
        public string UserId { get; set; }
        public bool RequireConfirmation { get; set; }
        public bool Success { get; set; }
    }
}
