using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeSlice.ApiService.Data;

namespace TimeSlice.ApiService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : OwnerData
    {
        protected readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository( ApplicationDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T?> GetAsync( string ownerId, int? id )
        {
            if (id == null)
            {
                return null;
            }

            var entry = await _context.Set<T>().FindAsync( id );
            if( entry?.OwnerId != ownerId )
            {
                return null;
            }

            return entry;
        }

        public async Task<List<T>> GetAllAsync( string ownerId )
        {
            return await _context.Set<T>()
                .Where( x => x.OwnerId == ownerId )
                .ToListAsync();
        }

        public async Task<T> AddAsync( string ownerId, T entity )
        {
            await _context.AddAsync( entity );
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync( string ownerId, T entity )
        {
            var entry = await GetAsync(ownerId, entity.Id);
            if( entry == null )
            {
                return;
            }
            _context.Update( entity );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( string ownerId, int id )
        {
            var entity = await GetAsync( ownerId, id );
            if (entity == null)
            {
                return;
            }

            _context.Remove( entity );
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists( string ownerId, int id )
        {
            var entity = await GetAsync(ownerId, id );
            return entity != null;
        }
    }
}
