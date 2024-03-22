using AutoMapper;
using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSlice.ApiService.Data;
using TimeSlice.ApiService.Models.Timebox;
using TimeSlice.ApiService.Repositories.Timebox;
using TimeSlice.ApiService.Static;

namespace TimeSlice.ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TimeboxController : ControllerBase
    {
        private readonly ITimeboxRepository _repository;
        private IMapper _mapper;
        private ILogger<TimeboxController> _logger;
        private readonly ApplicationDbContext _context;

        public TimeboxController( ITimeboxRepository _repository, IMapper mapper, ILogger<TimeboxController> logger, ApplicationDbContext context)
        {
            this._repository = _repository;
            this._mapper = mapper;
            this._context = context;
            this._logger = logger;
        }

        // GET: api/Timebox
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TimeboxEntryDto>>> Get()
        {
            try
            {
                var entries = await _repository.GetAllAsync();
                return Ok( _mapper.Map<IEnumerable<TimeboxEntryDto>>( entries ) );
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, $"Error performing {nameof( Get )}" );
                return StatusCode( 500, Messages.Error500Message );
            }
        }

        // GET: api/Timebox/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TimeboxEntryDto>> Get(int id)
        {
            try
            {
                var entry = await _repository.GetAsync( id );
                if( entry == null )
                {
                    return NotFound();
                }

                return Ok( entry );
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, $"Error performing {nameof( Get )}" );
                return StatusCode( 500, Messages.Error500Message );
            }
        }

        // PUT: api/Timebox/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TimeboxEntryDto dto)
        {
            if (id != dto.Id)
            {
                _logger.LogWarning( $"Update ID invalid in {nameof( Put )} - ID: {id}" );
                return BadRequest();
            }

            var author = await _repository.GetAsync( id );

            if (author == null)
            {
                _logger.LogWarning( $"{nameof( TimeboxEntry )} record not found in {nameof( Put )} - ID: {id}" );
                return NotFound();
            }

            _mapper.Map( dto, author );

            try
            {
                await _repository.UpdateAsync( author );
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (await _repository.Exists( id ) == false)
                {
                    return NotFound();
                }
                else
                {
                    _logger.LogError( ex, $"Error performing {nameof( Get )}" );
                    return StatusCode( 500, Messages.Error500Message );
                }
            }

            return NoContent();
        }

        // POST: api/Timebox
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TimeboxEntryDto>> Post( TimeboxEntryDto dto )
        {
            try
            {
                var create = _mapper.Map<TimeboxCreateEntryDto>( dto );
                var data = _mapper.Map<TimeboxEntry>( create );
                await _repository.AddAsync( data );
                return CreatedAtAction( nameof( Post ), new { id = data.Id }, data );
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, $"Error performing {nameof( Post )}" );
                return StatusCode( 500, Messages.Error500Message );
            }
        }

        // DELETE: api/Timebox/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _repository.DeleteAsync( id );
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError( ex, $"Error performing {nameof( Delete )}" );
                return StatusCode( 500, Messages.Error500Message );
            }
        }
    }
}
