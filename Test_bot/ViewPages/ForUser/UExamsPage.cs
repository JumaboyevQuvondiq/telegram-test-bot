using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Test_bot.Interfaces.Respositories;
using Test_bot.Respository;
using Test_bot.Services;
using Test_bot.models;
using Telegram.Bot.Types.Enums;
using Test_bot.ViewPages.User;
using Telegram.Bot.Polling;

namespace Test_bot.ViewPages.ForUser
{
    public class UExamsPage
    {
        
        public static List<string> strings = new List<string>();
        public static string category;
        public  static async Task UExamPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {


            var category_id = Program.categories[update.Message.Text];
            category = update.Message.Text;
            IExamRespository examRespository = new ExamRespository();
           
            var exams_list = (await examRespository.GetAllAsync()).Where(x=> (x.category_id) == category_id).Select(x=>x.title).ToList();
            exams_list.Add("⬅️ Orqaga");
            exams_list.Add("🟰 Asosiy menu");
            var keyboardButtons = PagesService.SortingPrint(exams_list, 1);
             strings = exams_list;
            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(keyboardButtons);


            await bot.SendTextMessageAsync(update.Message.Chat.Id, "Testni tanlang", replyMarkup: replyKeyboard);


  
        }

        
    }
}
