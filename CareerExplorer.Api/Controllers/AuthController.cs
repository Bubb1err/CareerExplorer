using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CareerExplorer.Core.Enums;
using CareerExplorer.Shared;
using CareerExplorer.Api.DTO;
using System.Net;
using CareerExplorer.Api.Services;
using CareerExplorer.Web;

namespace CareerExplorer.Api.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        protected APIResponse _response;
        private readonly IAuthHelper _authHelper;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IAuthHelper authHelper)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _response = new();
            _authHelper = authHelper;
            _emailStore= _authHelper.GetEmailStore();
        }
        [HttpPost]
        [Route("api/register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<APIResponse>> Register([FromBody]RegisterDTO registerDto)
        {
            if (!_roleManager.RoleExistsAsync(UserRoles.Recruiter).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Recruiter)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.JobSeeker)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).GetAwaiter().GetResult();
                await _authHelper.CreateAdminUser();
            }

            var user = _authHelper.CreateUser();

            await _userStore.SetUserNameAsync(user, registerDto.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, registerDto.Email, CancellationToken.None);
            user.UserType = registerDto.Role == UserRoles.Recruiter ? UserType.Recruiter : UserType.JobSeeker;

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (result.Succeeded)
            {
                await _authHelper.HandleUserRegistration(user, registerDto);
                await _signInManager.SignInAsync(user, isPersistent: false);

                _response.IsSuccess = true;
                _response.Result = "User created successfully";
                _response.StatusCode = HttpStatusCode.Created;
                return Ok(_response);
            }
            _response.IsSuccess = false;
            _response.Errors = new List<string> { "An error occured while creating a user." };
            _response.StatusCode = HttpStatusCode.BadRequest;
            return BadRequest(_response);
        }
        [HttpPost]
        [Route("api/login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginDto)
        {
            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, 
                loginDto.Password, true, lockoutOnFailure: false); 
            if (result.Succeeded)
            {
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "User logged in successfully";
                return _response;
            }
            _response.IsSuccess = false;
            _response.Errors = new List<string> { "An error occured while trying to log in." };
            _response.StatusCode = HttpStatusCode.BadRequest;
            return _response;
        }
        [HttpPost]
        [Route("api/logout")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _response.IsSuccess = true;
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = "User logged out successfully.";
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.StatusCode= HttpStatusCode.BadRequest;
                _response.Errors = new List<string> { ex.Message };
            }
            return _response;
        }
    }
}
