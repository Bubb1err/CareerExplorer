using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.Data;
using CareerExplorer.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq;

namespace CareerExplorer.Web.Controllers
{
    public class ChatController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        private readonly IChatRepository _chatRepository;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRecruiterProfileRepository _recruiterRepository;
        private readonly AppDbContext _context;
        public ChatController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, AppDbContext context)
        {
            _userManager= userManager;
            _unitOfWork= unitOfWork;
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
            _chatRepository = (IChatRepository)_unitOfWork.GetRepository<Chat>();
            _appUserRepository = _unitOfWork.GetRepository<AppUser>();
            _recruiterRepository = (IRecruiterProfileRepository)_unitOfWork.GetRepository<Recruiter>();
            _context = context;
        }
        [Authorize]
        public async Task<IActionResult> GetChat(string? receiverId)
        {
            //getting sender(current user)
            var currentUser = await _userManager.GetUserAsync(User);
            var currentAppUser = _appUserRepository
                .GetFirstOrDefault(x => x.Id == currentUser.Id);
            string currentUserId = currentUser.Id;
            ViewBag.SenderId = currentUserId;

            //getting receiver
            var receiver = _appUserRepository
                .GetFirstOrDefault(x => x.Id == receiverId, "RecruiterProfile,JobSeekerProfile");
            ViewBag.ReceiverId = receiverId;
            if(receiver.JobSeekerProfile != null)
            {
                ViewBag.JobSeeker = _jobSeekerRepository.GetFirstOrDefault(x => x.UserId== receiverId, "Country,City,DesiredPosition");
            }
            else if(receiver.RecruiterProfile!= null)
            {
                ViewBag.Recruiter = _recruiterRepository.GetFirstOrDefault(x => x.UserId == receiverId);
            }

            var chat = _chatRepository
                .GetFirstOrDefault(x => x.Users.Contains(receiver) && x.Users.Contains(currentAppUser), "Messages");
            if(chat == null)
            {
                chat = new Chat
                {
                    Users = new List<AppUser> { currentAppUser, receiver },
                    Messages = new List<Message>()
                };

                await _chatRepository.AddAsync(chat);
                await _unitOfWork.SaveAsync();
                ViewBag.ChatId = chat.Id;
                return View(chat.Messages);
            }
            ViewBag.ChatId = chat.Id;
            return View(chat.Messages);
        }
        [Authorize(Roles = UserRoles.JobSeeker)]
        public async Task<IActionResult> GetJobSeekerChats()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var appUser = _appUserRepository
                .GetFirstOrDefault(x => x.Id == currentUser.Id);
            var chats = _chatRepository.GetJobSeekerChats(appUser).ToImmutableList();
            return View(chats);
        }
        public async Task<IActionResult> GetRecruiterChats()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var appUser = _appUserRepository
                .GetFirstOrDefault(x => x.Id == currentUser.Id);
            var chats = _chatRepository.GetRecruiterChats(appUser).ToImmutableList();
            return View(chats);
        }
        public async Task<IActionResult> DeleteChat(int chatId)
        {
            var chat = _chatRepository.GetFirstOrDefault(x => x.Id == chatId);
            if (chat == null) return BadRequest();
            _chatRepository.Remove(chat);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
    }
}
