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
        static TelegramBotClient Bot = BotService.Bot;
        static ReceiverOptions receiverOptions = BotService.receiverOptions;
        public static List<string> strings = new List<string>();
        public static string category;
        public static async Task UExamPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
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


            await Program.Bot.SendTextMessageAsync(update.Message.Chat.Id, "Testni tanlang", replyMarkup: replyKeyboard);


            if (Program.exams.Keys.Contains(update.Message.Text))
            {
                await Program.Bot.SendTextMessageAsync(update.Message.Chat.Id, "Testlar hozircha tayyor emas");
            }


            update.Message.Text = "test boshlash";

            await bot.ReceiveAsync(updateHandler, Program.ErrorHandler, receiverOptions);
        }

        private static async Task updateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    ICategoryRespository categoryRespository = new CategoryRespository();
                    List<string> categories = new List<string>();
                    foreach (var item in await categoryRespository.GetAllAsync())
                    {

                        categories.Add(item.Name);
                    }


                    if (update.Message.Text == "⬅️ Orqaga")
                    {
                        update.Message.Text = "test boshlash";
                        await UCategoryPage.CategoryPage(bot, update, arg3);
                    }
                    else if (update.Message.Text == "🟰 Asosiy menu")
                    {

                        await UStartPage.StartPage(bot, update, arg3);
                    }
                    else if (strings.Contains(update.Message.Text))
                    {
                        List<string> strings2 = new List<string>();
                        strings2.Add("🔙 Orqaga");
                        strings2.Add("🟰 Asosiy menu");
                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "test boshlandi", replyMarkup: new ReplyKeyboardMarkup(PagesService.SortingPrint(strings2, 2)) { ResizeKeyboard = true});
                        await UTestPage.TestPage(bot, update, arg3);
                    }
                    else if (update.Message.Text == "/start")
                    {
                        await UStartPage.StartPage(bot, update, arg3);

                    }
                    

                    else if (!categories.Contains(update.Message.Text) && update.Message.Text != "🔙 Orqaga")
                    {
                        await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang");
                    }

                }
            }
        }
    }
}
