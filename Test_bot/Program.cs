using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;
using Test_bot.Respository;
using Test_bot.Services;
using Test_bot.ViewPages.User;
using User = Test_bot.models.User;

class Program

{
  

  

    public static Dictionary<string, int> categories;
    public  static Dictionary<string, int> exams;


    public static TelegramBotClient Bot = BotService.Bot;
    public static ReceiverOptions receiverOptions = BotService.receiverOptions;
    public static async Task Main()
    {

        
        ICategoryRespository categoryRespository = new CategoryRespository();
        categories = (await categoryRespository.GetAllAsync()).ToDictionary(x => x.Name, x => x.Id);
        IExamRespository examRespository = new ExamRespository();
         exams = (await examRespository.GetAllAsync()).ToDictionary(x => x.title, x => x.Id);
        Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);
        
        IAdminRespository adminRespository = new AdminRespository();
        
       
       
        Console.ReadKey();  
    }






    public static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }






    private static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
    {
        List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();

        if (update.Type == UpdateType.Message)
        {
            if (update.Message.Type == MessageType.Text)
            {
                if (update.Message.Text == "/start")
                {
                   
                     await UStartPage.StartPage(bot, update, arg3);

                }

            }
        }



      /*  //  Admin uchun
        else
        {


            if (await AdminHelper.IsAdmin(update.Message.Chat.Id.ToString()))
            {


                ICategoryRespository categoryRespository = new CategoryRespository();
                string str = "kategoriya qo'shish";


                List<string> strings = new List<string>();
                foreach (var Item in await categoryRespository.GetAllAsync())
                    strings.Add(Item.Name);
                keyboardButtons = PagesService.SortingPrint(strings, 2);
                keyboardButtons.Add(new KeyboardButton[] { str });
                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(keyboardButtons);

                await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Kategoriyani tanlang", replyMarkup: replyKeyboard);

            }

        }
           
        */
    }

}