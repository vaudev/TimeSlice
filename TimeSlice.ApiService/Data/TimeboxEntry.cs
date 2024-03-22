﻿using System.ComponentModel.DataAnnotations;

namespace TimeSlice.ApiService.Data
{
    public class TimeboxEntry
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public DateTime DateStart { get; set; }

        [Required]
        public DateTime DateEnd { get; set; }
    }
}
