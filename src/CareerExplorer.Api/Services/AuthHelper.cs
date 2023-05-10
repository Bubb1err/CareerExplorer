using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Enums;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CareerExplorer.Api.Services
{
    internal sealed class AuthHelper : IAuthHelper
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAdminRepository _adminRepository;
        private readonly IRepository<AppUser> _userRepository;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IRepository<RefreshToken> _refreshTokenRepository;
        private readonly IConfiguration _config;
        private readonly IRepository<AppUser> _appUserRepository;

        public AuthHelper(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork,
            TokenValidationParameters tokenValidationParameters,
            IConfiguration config)
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
            _adminRepository = (IAdminRepository)_unitOfWork.GetRepository<Admin>();
            _userRepository = _unitOfWork.GetRepository<AppUser>();
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _recruiterRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _tokenValidationParameters = tokenValidationParameters;
            _refreshTokenRepository = _unitOfWork.GetRepository<RefreshToken>();
            _config = config;
            _appUserRepository = _unitOfWork.GetRepository<AppUser>();
        }
        public AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'.");
            }
        }
        public async Task CreateAdminUser()
        {
            var user = CreateUser();

            await _userStore.SetUserNameAsync(user, "admin@mail.com", CancellationToken.None);
            await _emailStore.SetEmailAsync(user, "admin@mail.com", CancellationToken.None);
            user.UserType = UserType.Admin;
            var result = await _userManager.CreateAsync(user, "Admin123!");
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                var userId = await _userManager.GetUserIdAsync(user);
                await _adminRepository.AddAsync(new Admin()
                {
                    UserId = userId
                });
                await _unitOfWork.SaveAsync();
                var currentUser = _userRepository.GetFirstOrDefault(x => x.Id == userId);
                currentUser.AdminProfileId = _adminRepository.GetFirstOrDefault(x => x.UserId == userId).Id;
                await _unitOfWork.SaveAsync();
            }
        }
        public async Task HandleUserRegistration(AppUser user, RegisterDTO registerDto)
        {
            var userId = await _userManager.GetUserIdAsync(user);

            if (registerDto.Role == null)
            {
                await _userManager.AddToRoleAsync(user, UserRoles.JobSeeker);
            }
            else
            {
                await _userManager.AddToRoleAsync(user, registerDto.Role);
            }
            //checking if user recruiter or jobseeker and create profile 
            if (user.UserType == UserType.JobSeeker)
            {
                await _jobSeekerRepository.AddAsync(new JobSeeker
                {
                    UserId = userId
                });
                await _unitOfWork.SaveAsync();
                var currentUser = _userRepository.GetFirstOrDefault(x => x.Id == userId);
                currentUser.JobSeekerProfileId = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId == userId).Id;
                await _unitOfWork.SaveAsync();
            }
            else if (user.UserType == UserType.Recruiter)
            {
                await _recruiterRepository.AddAsync(new Recruiter
                {
                    UserId = userId
                });
                await _unitOfWork.SaveAsync();
                var currentUser = _userRepository.GetFirstOrDefault(x => x.Id == userId);
                currentUser.RecruiterProfileId = _recruiterRepository.GetFirstOrDefault(x => x.UserId == userId).Id;
                await _unitOfWork.SaveAsync();
            }

        }
        public IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException();
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
        private async Task<AuthResultDTO> VerifyAndGenerateTokenAsync(TokenRequestDTO tokenRequestDto)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var storedToken = _refreshTokenRepository.GetFirstOrDefault(x => x.Token == tokenRequestDto.Token);
            var appUser = _appUserRepository.GetFirstOrDefault(x => x.Id == storedToken.UserId);
            try
            {
                var tokenCheckResult = jwtTokenHandler.ValidateToken(tokenRequestDto.Token,
                    _tokenValidationParameters, out var validateToken);

                return await GenerateJWTTokenAsync(appUser, storedToken);
            }
            catch (SecurityTokenExpiredException)
            {
                if (storedToken.DateExpired >= DateTime.UtcNow)
                {
                    return await GenerateJWTTokenAsync(appUser, storedToken);
                }
                else
                {
                    return await GenerateJWTTokenAsync(appUser, null);
                }
            }
        }
        public async Task<AuthResultDTO> GenerateJWTTokenAsync(AppUser user, RefreshToken rToken)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var authClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, roles.FirstOrDefault())
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config["JWT:Secret"]));
            var token = new JwtSecurityToken(issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                expires: DateTime.UtcNow.AddMinutes(15),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256));
            var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
            if (rToken != null)
            {
                var rTokenResponse = new AuthResultDTO()
                {
                    Token = jwtToken,
                    RefreshToken = rToken.Token,
                    ExpiresAt = token.ValidTo
                };
                return rTokenResponse;
            }
            var refreshToken = new RefreshToken()
            {
                JwtId = token.Id,
                IsRevoked = false,
                UserId = user.Id,
                DateAdded = DateTime.UtcNow,
                DateExpired = DateTime.UtcNow.AddMonths(6),
                Token = Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString(),
            };
            await _refreshTokenRepository.AddAsync(refreshToken);
            await _unitOfWork.SaveAsync();
            var result = new AuthResultDTO()
            {
                Token = jwtToken,
                RefreshToken = refreshToken.Token,
                ExpiresAt = token.ValidTo
            };
            return result;
        }
    }
}
