using AutoMapper;
using TimeSlice.ApiService.Data;

namespace TimeSlice.ApiService.Repositories.Timebox
{
    public class TimeboxRepository : GenericRepository<TimeboxEntry>, ITimeboxRepository
    {
        public TimeboxRepository( ApplicationDbContext context, IMapper mapper ) : base( context, mapper )
        {
        }
    }
}
