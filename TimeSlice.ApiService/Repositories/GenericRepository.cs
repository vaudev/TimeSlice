using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TimeSlice.ApiService.Data;

namespace TimeSlice.ApiService.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public GenericRepository( ApplicationDbContext context, IMapper mapper )
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<T?> GetAsync( int? id )
        {
            if (id == null)
            {
                return null;
            }

            return await _context.Set<T>().FindAsync( id );
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> AddAsync( T entity )
        {
            await _context.AddAsync( entity );
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync( T entity )
        {
            _context.Update( entity );
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync( int id )
        {
            var entity = await GetAsync( id );
            if (entity == null)
            {
                return;
            }

            _context.Remove( entity );
            await _context.SaveChangesAsync();
        }

        public async Task<bool> Exists( int id )
        {
            var entity = await GetAsync( id );
            return entity != null;
        }
    }
}
