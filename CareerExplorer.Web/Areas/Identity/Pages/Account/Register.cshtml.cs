// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Azure.Identity;
using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Enums;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace CareerExplorer.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            AppDbContext context)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            _context = context;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            public string Role { get; set; } 
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            //adding roles if they don't exist
            //if one of them doesn't exist then we need to add all roles
            if (!_roleManager.RoleExistsAsync(UserRoles.Recruiter).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Recruiter)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.JobSeeker)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin)).GetAwaiter().GetResult();
                //seed admin
                var user = CreateUser();
                
                await _userStore.SetUserNameAsync(user, "admin@mail.com", CancellationToken.None);
                await _emailStore.SetEmailAsync(user, "admin@mail.com", CancellationToken.None);
                user.UserType = UserType.Admin;
                var result = await _userManager.CreateAsync(user, "Admin123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);
                    var userId = await _userManager.GetUserIdAsync(user);
                    await _context.Admins.AddAsync(new Admin()
                    {
                        UserId = userId
                    });
                    await _context.SaveChangesAsync();
                    var currentUser = _context.AppUsers.FirstOrDefault(x => x.Id == userId);
                    currentUser.AdminProfileId = _context.Admins.FirstOrDefault(x => x.UserId == userId).Id;
                    await _context.SaveChangesAsync();
                }
            }

            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.UserType = Input.Role == UserRoles.Recruiter ? UserType.Recruiter : UserType.JobSeeker;

                var result = await _userManager.CreateAsync(user, Input.Password);

                //todo: replace queries to context with repository queries
                if (result.Succeeded)
                {
                    var userId = await _userManager.GetUserIdAsync(user);
                    _logger.LogInformation("User created a new account with password.");

                    if(Input.Role == null)
                    {
                        await _userManager.AddToRoleAsync(user, UserRoles.JobSeeker);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, Input.Role);
                    }
                    //checking if user recruiter or jobseeker and create profile 
                    if (user.UserType == UserType.JobSeeker)
                    {
                        await _context.JobSeekers.AddAsync(new JobSeeker
                        {
                            UserId = userId
                        });
                        await _context.SaveChangesAsync();
                        var currentUser = _context.AppUsers.FirstOrDefault(x => x.Id == userId);
                        currentUser.JobSeekerProfileId = _context.JobSeekers.FirstOrDefault(x => x.UserId == userId).Id;
                        await _context.SaveChangesAsync();
                    }
                    else if (user.UserType == UserType.Recruiter)
                    {
                        await _context.Recruiters.AddAsync(new Recruiter
                        {
                            UserId = userId
                        });
                        await _context.SaveChangesAsync();
                        var currentUser = _context.AppUsers.FirstOrDefault(x => x.Id == userId);
                        currentUser.RecruiterProfileId = _context.Recruiters.FirstOrDefault(x => x.UserId == userId).Id;
                        await _context.SaveChangesAsync();
                    }


                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private AppUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<AppUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
