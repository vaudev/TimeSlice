using Blazored.LocalStorage;
using TimeSlice.WebApp.Services.Auth;
using TimeSlice.WebApp.Services.Base;

namespace TimeSlice.WebApp.Services.Timebox
{
	public class TimeboxService : GenericCrudService<TimeboxEntryDto>, ITimeboxService
    {
        public TimeboxService( ApiService client, ILocalStorageService localStorage, IAuthenticationService authService ) : base( client, localStorage, authService )
        {
        }

        protected override async Task InternalDeleteAsync( string ownerId, int id )
        {
            await _client.TimeboxDELETEAsync(ownerId, id );
        }

        protected override async Task<ICollection<TimeboxEntryDto>> InternalGetAll(string ownerId )
        {
            return await _client.TimeboxAllAsync(ownerId );
        }

        protected override async Task<TimeboxEntryDto> InternalGetAsync(string ownerId, int id )
        {
            return await _client.TimeboxGETAsync(ownerId, id );
        }

        protected override async Task<TimeboxEntryDto> InternalPostAsync( string ownerId, TimeboxEntryDto dto )
        {
            return await _client.TimeboxPOSTAsync( ownerId, dto );
        }

        protected override async Task InternalPutAsync( string ownerId, int id, TimeboxEntryDto dto )
        {
            await _client.TimeboxPUTAsync( ownerId, id, dto );
        }
    }
}
