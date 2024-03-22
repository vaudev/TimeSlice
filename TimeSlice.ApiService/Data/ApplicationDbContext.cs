using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TimeSlice.ApiService.Data
{
	public class ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ) : IdentityDbContext<ApplicationUser>( options )
    {
        public DbSet<TimeboxEntry> TimeboxEntries => Set<TimeboxEntry>();
    }
}
