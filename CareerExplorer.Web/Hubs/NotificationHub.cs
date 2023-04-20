using Azure.Identity;
using IdentityServer4.Models;
using Microsoft.AspNetCore.SignalR;

namespace CareerExplorer.Web.Hubs
{
    public class NotificationHub : Hub
    {
        //public async Task SendNotification(string userId, string content)
        //{
        //    Clients.User(userId).SendAsync("ReceiveNotification", content);
        //}
        public async Task ReceiveNotification(string receiverId, string content)
        {
            Console.WriteLine("sending notification3");
            await Clients.User(receiverId).SendAsync("ReceiveNotification", content);
        }

    }
}
