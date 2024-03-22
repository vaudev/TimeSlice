using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Data
{
    public class OwnerData
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string OwnerId { get; set; }
    }
}
