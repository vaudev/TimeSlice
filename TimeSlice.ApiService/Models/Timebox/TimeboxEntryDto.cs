using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Models.Timebox
{
    public class TimeboxCreateEntryDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }

        [Required]
        public string OwnerId { get; set; }
    }
}