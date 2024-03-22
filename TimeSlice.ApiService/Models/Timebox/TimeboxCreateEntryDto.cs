using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Models.Timebox
{
    public class TimeboxEntryDto : BaseDto
    {
        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }
    }
}