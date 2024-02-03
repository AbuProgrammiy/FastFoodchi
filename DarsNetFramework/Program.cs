using System.Runtime.InteropServices;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;


namespace DarsNetFramework
{
    public class FastFoodManagment
    {
        public FastFoodManagment()
        {
             DirectoryAndFileCreate();
             Action().GetAwaiter().GetResult();
        }
        public long Admin=1268306946;
        //bot datalari saqlanadigan joyni tartblash uchun directory yasash
        public void DirectoryAndFileCreate()
        {
            if (!Directory.Exists("C:/FastFoodchi"))
            {
                Directory.CreateDirectory("C:/FastFoodchi");
                using (FileStream users = new FileStream("C:/FastFoodchi/usersChatId.txt", FileMode.Create)) { }
            }
        }
        public async Task Action()
        {

            var botClient = new TelegramBotClient("6526535333:AAHXVFBwruT-4zsnhbI_h2cVTRJXRr9cA7g");
            using CancellationTokenSource cts = new();

            ReceiverOptions receiverOptions = new()
            {
                AllowedUpdates = Array.Empty<UpdateType>() // receive all update types except ChatMember related updates
            };

            botClient.StartReceiving(
                updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: cts.Token
            );

            var me = await botClient.GetMeAsync();

            Console.WriteLine($"Start listening for @{me.Username}");
            Console.ReadLine();
            cts.Cancel();

            async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
            {
                if (update.Message is not { } message)
                    return;

                string[] usersId = System.IO.File.ReadAllLines("C:/FastFoodchi/usersChatId.txt");

                var chatId = message.Chat.Id;
                if (message.Type==MessageType.Text)
                {
                    Console.WriteLine($"Received a '{message.Text}' message in chat {chatId}.");
                    if(message.Text=="/start")
                    {
                        if(!usersId.Contains(chatId.ToString()))
                        {
                            ReplyKeyboardMarkup ContactShare = new(new[]
                            {
                                KeyboardButton.WithRequestContact("Share Contact"),
                            });

                            Message sentMessage = await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: "Botimizga hush kelibsiz",
                                replyMarkup: ContactShare,
                                cancellationToken: cancellationToken);
                        }
                    }
                    else if(message.Text=="/chatid")
                    {
                        await botClient.SendTextMessageAsync(
                                chatId: chatId,
                                text: $"Sizning chatIdingiz: {chatId}",
                                cancellationToken: cancellationToken);
                    }
                }

                // chat idisini bilish
                //if(message.T)
            }

            Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
            {
                var ErrorMessage = exception switch
                {
                    ApiRequestException apiRequestException
                        => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                    _ => exception.ToString()
                };

                Console.WriteLine(ErrorMessage);
                return Task.CompletedTask;
            }
        }
    }
    internal class Program
    {
        static void Main(string[] args)
        {
            FastFoodManagment start= new FastFoodManagment();   
        }
    }
}
