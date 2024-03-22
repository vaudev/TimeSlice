using Blazored.LocalStorage;
using TimeSlice.WebApp.Services.Base;

namespace TimeSlice.WebApp.Services.Timebox
{
	public class TimeboxService : GenericCrudService<TimeboxEntryDto>, ITimeboxService
    {
        public TimeboxService( ApiService client, ILocalStorageService localStorage ) : base( client, localStorage )
        {
        }

        protected override async Task InternalDeleteAsync( int id )
        {
            await _client.TimeboxDELETEAsync( id );
        }

        protected override async Task<ICollection<TimeboxEntryDto>> InternalGetAll()
        {
            return await _client.TimeboxAllAsync();
        }

        protected override async Task<TimeboxEntryDto> InternalGetAsync( int id )
        {
            return await _client.TimeboxGETAsync( id );
        }

        protected override async Task<TimeboxEntryDto> InternalPostAsync( TimeboxEntryDto dto )
        {
            return await _client.TimeboxPOSTAsync( dto );
        }

        protected override async Task InternalPutAsync( int id, TimeboxEntryDto dto )
        {
            await _client.TimeboxPUTAsync( id, dto );
        }
    }
}
