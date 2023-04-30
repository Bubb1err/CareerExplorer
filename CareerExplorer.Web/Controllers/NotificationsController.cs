using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Infrastructure.IServices;
using CareerExplorer.Shared;
using CareerExplorer.Web.Hubs;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace CareerExplorer.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MeetingNotification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        private readonly IEmailSender _emailSender;
        private readonly IRepository<AppUser> _appUserRepository;
        private readonly IRecommendVacanciesByEmailService _recommendVacanciesService;
        public NotificationsController(UserManager<IdentityUser> userManager, 
            IUnitOfWork unitOfWork,      
            IHubContext<NotificationHub> notificationHub,
            IEmailSender emailSender,
            IRecommendVacanciesByEmailService recommendVacanciesByEmailService)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _notificationRepository = _unitOfWork.GetRepository<MeetingNotification>();
            _notificationHub = notificationHub;
            _emailSender = emailSender;
            _appUserRepository = _unitOfWork.GetRepository<AppUser>();
            _recommendVacanciesService = recommendVacanciesByEmailService;
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Recruiter)]
        public async Task<IActionResult> CreateNotification(MeetingNotification notification)
        { 
            var sender = await _userManager.GetUserAsync(User);
            string senderId = sender.Id;
            notification.SenderId = senderId;
            await _notificationRepository.AddAsync(notification);
            await _unitOfWork.SaveAsync();
            return Ok();
        }
        [HttpGet]
        [Authorize(Roles =UserRoles.JobSeeker)]
        public async Task<IActionResult> GetInvitations()
        {
            var user = await _userManager.GetUserAsync(User);
            string userId = user.Id;
            ViewBag.UserId = userId;
            var invitations = _notificationRepository.GetAll(x => x.ReceiverId == userId);
            return View(invitations);

        }
        [HttpPost]
        public async Task<IActionResult> AcceptInvitation(int invitationId)
        {
            
            var user = await _userManager.GetUserAsync(User);
            var email = user.Email;
            if (invitationId == 0)
                return BadRequest();
            var invitation = _notificationRepository.GetFirstOrDefault(x => x.Id == invitationId);
            invitation.IsAccepted = true;
            await _unitOfWork.SaveAsync();

            var recruiterUser = _appUserRepository.GetFirstOrDefault(x => x.Id == invitation.SenderId, "RecruiterProfile");
            var recruiter = recruiterUser.RecruiterProfile;
            BackgroundJob.Schedule(()
                => SendNotification(invitation.ReceiverId, invitation.MeetingLink),
                invitation.Date - DateTime.Now);
            BackgroundJob.Schedule(() =>
            _emailSender.SendEmailAsync(email, "Notification",
            $"<p>You have a meeting</p><br/><a href={invitation.MeetingLink}>{invitation.MeetingLink}</a><br/><p>{recruiter.Name} " +
            $"{recruiter.Surname} {recruiter.Company}</p>"), invitation.Date - DateTime.Now);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Subscribe()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var appUser = _appUserRepository.GetFirstOrDefault(x => x.Id == user.Id, "JobSeekerProfile");
                if (appUser.JobSeekerProfile != null)
                {
                    appUser.JobSeekerProfile.IsSubscribedToNotification= true;
                    await _unitOfWork.SaveAsync();
                }
                else return BadRequest();
                RecurringJob.AddOrUpdate(() => 
                _recommendVacanciesService.SendVacanciesToUsersByEmail(TimeSpan.FromMinutes(15)), "*/15 * * * *");
                return Ok();
                
            }
            catch
            {
                return BadRequest();
            }
        }
        public async Task SendNotification(string receiverId, string content)
        {
            var hubContext = _notificationHub.Clients.User(receiverId);
            await hubContext.SendAsync("ReceiveNotification", receiverId, content);
        }
    }
}
