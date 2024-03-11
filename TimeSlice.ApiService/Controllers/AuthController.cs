using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using TimeSlice.ApiService.Configurations;
using TimeSlice.ApiService.Data;
using TimeSlice.ApiService.Models.Auth;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;

namespace TimeSlice.ApiService.Controllers
{
    [Route( "api/[controller]" )]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private ILogger<AuthController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly IEmailSender<ApplicationUser> _emailSender;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;

        public AuthController( ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IEmailSender<ApplicationUser> emailSender,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _signInManager = signInManager;
            _mapper = mapper;
        }


        [HttpPost]
        [Route( "registerorlogin" )]
        public async Task<ActionResult<AuthResponse>> RegisterOrLogin( string username )
        {
            var user = await _userManager.FindByNameAsync( username );

            // Automatically create a user if it doesn't exist
            if (user == null)
            {
                user = CreateUser();

                await _userStore.SetUserNameAsync( user, username, CancellationToken.None );

                var email = $"{username}@timeslice.com";
                var emailStore = GetEmailStore();
                await emailStore.SetEmailAsync( user, username, CancellationToken.None );

                var result = await _userManager.CreateAsync( user, "P@ssword1!" );

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError( error.Code, error.Description );
                    }

                    return BadRequest( ModelState );
                }
            }

            //var userId = await _userManager.GetUserIdAsync( user );
            var userDto = _mapper.Map<ApplicationUserDto>( user );

            var response = new AuthResponse()
            {
                User = userDto,
                Success = true
            };

            //await _signInManager.SignInAsync( user, isPersistent: false );
            return response;
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException( $"Can't create an instance of '{nameof( ApplicationUser )}'. " +
                    $"Ensure that '{nameof( ApplicationUser )}' is not an abstract class and has a parameterless constructor." );
            }
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException( "The default UI requires a user store with email support." );
            }
            return (IUserEmailStore<ApplicationUser>) _userStore;
        }

    }
}
