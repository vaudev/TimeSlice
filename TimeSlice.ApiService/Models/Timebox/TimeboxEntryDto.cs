using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Models.Timebox
{
    public class TimeboxEntryDto : BaseDto
    {
        [Required]
        public string Caption { get; set; } = string.Empty;
    }
}