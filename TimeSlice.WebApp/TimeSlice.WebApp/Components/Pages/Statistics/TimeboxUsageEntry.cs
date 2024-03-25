using System.ComponentModel.DataAnnotations;

namespace TimeSlice.WebApp.Components.Pages.Statistics
{
    public class TimeboxUsageEntry
    {
        public string Code { get; set; } = string.Empty;
        public TimeSpan Duration { get; set; }
    }
}
