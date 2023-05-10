using CareerExplorer.Api.DTO;
using CareerExplorer.Core.Entities;
using Microsoft.AspNetCore.Identity;

namespace CareerExplorer.Api.Services
{
   public  interface IAuthHelper
   {
        AppUser CreateUser();
        Task CreateAdminUser();
        Task HandleUserRegistration(AppUser user, RegisterDTO registerDto);
        IUserEmailStore<IdentityUser> GetEmailStore();
        Task<AuthResultDTO> GenerateJWTTokenAsync(AppUser user, RefreshToken rToken);
   }
}
