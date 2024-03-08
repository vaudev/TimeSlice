using BookStoreApp.API.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.Encodings.Web;
using TimeSlice.ApiService.Data;
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

        public AuthController( ILogger<AuthController> logger,
            UserManager<ApplicationUser> userManager,
            IUserStore<ApplicationUser> userStore,
            IEmailSender<ApplicationUser> emailSender,
            SignInManager<ApplicationUser> signInManager)
        {
            _logger = logger;
            _userManager = userManager;
            _userStore = userStore;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route( "register" )]
        public async Task<ActionResult<AuthResponse>> Register(string email, string password, string returnUrl)
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync( user, email, CancellationToken.None );

            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync( user, email, CancellationToken.None );
            var result = await _userManager.CreateAsync( user, password);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError( error.Code, error.Description );
                }

                return BadRequest( ModelState );
            }

            var userId = await _userManager.GetUserIdAsync( user );
            var code = await _userManager.GenerateEmailConfirmationTokenAsync( user );
            code = WebEncoders.Base64UrlEncode( Encoding.UTF8.GetBytes( code ) );

            //var callbackUrl = _navigationManager.GetUriWithQueryParameters(
            //    _navigationManager.ToAbsoluteUri( "Account/ConfirmEmail" ).AbsoluteUri,
            //    new Dictionary<string, object?> { ["userId"] = userId, ["code"] = code, ["returnUrl"] = returnUrl } );

            //await _emailSender.SendConfirmationLinkAsync( user, email, HtmlEncoder.Default.Encode( callbackUrl ) );

            var response = new AuthResponse()
            {
                UserId = userId,
                RequireConfirmation = false,
                Success = true
            };

            if ( _userManager.Options.SignIn.RequireConfirmedAccount )
            {
                response.Success = false;
                response.RequireConfirmation = true;
                return response;
            }

            await _signInManager.SignInAsync( user, isPersistent: false );
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
