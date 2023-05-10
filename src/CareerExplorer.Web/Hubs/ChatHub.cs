using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace CareerExplorer.Web.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Message> _messageRepository;
        private readonly IRepository<Chat> _chatRepository;
        private readonly IEmailSender _emailSender;
        public ChatHub(IUnitOfWork unitOfWork,
            IEmailSender emailSender)
        {
            _unitOfWork= unitOfWork;
            _messageRepository= _unitOfWork.GetRepository<Message>();
            _chatRepository= _unitOfWork.GetRepository<Chat>();
            _emailSender = emailSender;
        }
        public async Task SendMessage(int chatId, string senderId, string receiverId, string content)
        {
            var message = new Message
            {
                ChatId = chatId,
                SenderId = senderId,
                ReceiverId = receiverId,
                Text = content,
                TimeSent = DateTime.UtcNow
            };
            await _messageRepository.AddAsync(message);
            await _unitOfWork.SaveAsync();
            var chat = _chatRepository.GetFirstOrDefault(x => x.Id== chatId, "Users");
            foreach (var user in chat.Users)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            }
            var messageText = message.Text;
            string senderEmail = chat.Users.FirstOrDefault(x => x.Id == senderId).Email;
            string receiverEmail = chat.Users.FirstOrDefault(x => x.Id == receiverId).Email;

            await Clients.Group(chatId.ToString()).SendAsync("ReceiveMessage", messageText, senderId);
            if (senderEmail != null && receiverEmail != null)
            {
                await _emailSender.SendEmailAsync(receiverEmail, $"You have a new message form {senderEmail}", messageText);
            }
        }
    }
}
