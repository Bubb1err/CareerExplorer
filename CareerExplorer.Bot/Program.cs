using CareerExplorer.Core.Entities;
using CareerExplorer.Core.Interfaces;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace CareerExplorer.Bot
{
    internal class Program
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IJobSeekerProfileRepository _jobSeekerRepository;
        public Program(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _jobSeekerRepository = (IJobSeekerProfileRepository)_unitOfWork.GetRepository<JobSeeker>();
        }

        static async Task Main(string[] args)
        {
            var botClient = new TelegramBotClient("6113390570:AAHqbeiI1QlHVw--oq-roY_qRF52u7sVIbg");
            //var webHookUrl = "https://ff4a-109-86-1-61.ngrok-free.app/Telegram/Post";
            //await botClient.SetWebhookAsync(webHookUrl);
            //await botClient.DeleteWebhookAsync();
            ////Console.WriteLine(botClient.GetWebhookInfoAsync());
            botClient.StartReceiving(HandleUpdateAsync, Error);
            Console.ReadKey();
        }

        private static Task UpdateHandler(ITelegramBotClient arg1, Update arg2, CancellationToken arg3)
        {
            throw new NotImplementedException();
        }

        private static Task Error(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
        {
            Console.WriteLine($"{arg2}");
            return Task.CompletedTask;
        }

        private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken arg3)
        {
            var message = update.Message;
            if(message != null)
            {
                if(message.Text.Contains("start"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Please enter your email");
                    return;
                }
                if(message.Text.Contains("@") && message.Text.EndsWith(".com"))
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Ok");
                    return;
                }
                if(message.Text == "Hello")
                {
                    await botClient.SendTextMessageAsync(message.Chat.Id, "Hi");
                    return;
                }
            }
        }
        public async Task<bool> UpdateUser(long chatId, string email)
        {
            var user = _jobSeekerRepository.GetFirstOrDefault(x => x.AppUser.Email== email);
            if(user == null)
            {
                return false;
            }
            user.TgChatId= chatId;
            await _unitOfWork.SaveAsync();
            return true;
        }
    }
}