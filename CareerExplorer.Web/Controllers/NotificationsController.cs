using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using CareerExplorer.Shared;
using CareerExplorer.Web.Hubs;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.SignalR;
using NuGet.Protocol.Plugins;

namespace CareerExplorer.Web.Controllers
{
    public class NotificationsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<MeetingNotification> _notificationRepository;
        private readonly IHubContext<NotificationHub> _notificationHub;
        public NotificationsController(UserManager<IdentityUser> userManager, 
            IUnitOfWork unitOfWork,      
            IHubContext<NotificationHub> notificationHub)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _notificationRepository = _unitOfWork.GetRepository<MeetingNotification>();
            _notificationHub = notificationHub;
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
            if (invitationId == 0)
                return BadRequest();
            var invitation = _notificationRepository.GetFirstOrDefault(x => x.Id == invitationId);
            invitation.IsAccepted = true;
            await _unitOfWork.SaveAsync();
            BackgroundJob.Schedule(()
                => SendNotification(invitation.ReceiverId, invitation.MeetingLink),
                invitation.Date - DateTime.Now);
            return Ok();
        }
        public void SendNotification(string receiverId, string content)
        {
            var hubContext = _notificationHub.Clients.User(receiverId);
            hubContext.SendAsync("ReceiveNotification", receiverId, content);
        }
    }
}
