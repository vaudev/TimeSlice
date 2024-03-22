using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Models.Auth
{
    public class ApplicationUserDto
    {
        public string UserName { get; set; }
        public string Id { get; set; }
    }
}
