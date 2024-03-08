using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Data
{
    public class TimeboxEntry
    {
        public int Id { get; set; }

        [Required]
        public string Caption { get; set; } = string.Empty; 
    }
}
