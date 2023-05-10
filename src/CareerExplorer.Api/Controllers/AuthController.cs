using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using CareerExplorer.Core.Enums;
using CareerExplorer.Shared;
using CareerExplorer.Api.DTO;
using System.Net;
using CareerExplorer.Api.Services;
using CareerExplorer.Web;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Core.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;

namespace CareerExplorer.Api.Controllers
{
    [EnableCors("DefaultPolicy")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        protected APIResponse _response;
        private readonly IAuthHelper _authHelper;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AuthController(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            RoleManager<IdentityRole> roleManager,
            IAuthHelper authHelper,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _response = new();
            _authHelper = authHelper;
            _emailStore= _authHelper.GetEmailStore();
            _unitOfWork= unitOfWork;
            _appUserRepository = _unitOfWork.GetRepository<AppUser>();
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
        [HttpPost("api/login")]
        public async Task<ActionResult<APIResponse>> Login([FromBody] LoginDTO loginDto)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest("Please,provide all required fields.");
                var userExist = _appUserRepository.GetFirstOrDefault(x => x.Email == loginDto.Email);
                if (userExist != null && await _userManager.CheckPasswordAsync(userExist, loginDto.Password))
                {
                    var tokenValue = await _authHelper.GenerateJWTTokenAsync(userExist, null);
                    _response.IsSuccess = true;
                    _response.Result = tokenValue;
                    _response.StatusCode = HttpStatusCode.OK;
                    return Ok(_response);
                }
            }
            catch(Exception ex)
            {
               _response.IsSuccess = false;
                _response.Errors = new List<string> { ex.Message };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
            return Unauthorized();
        }
    }
}
