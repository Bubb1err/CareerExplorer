using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Enums;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using MagicVilla_VillaAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

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

        public AuthHelper(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            IUnitOfWork unitOfWork)
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
    }
}
