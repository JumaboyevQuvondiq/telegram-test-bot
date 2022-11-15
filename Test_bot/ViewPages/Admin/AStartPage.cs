using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;
using Test_bot.Respository;
using Test_bot.Services;
using Test_bot.models;

using Telegram.Bot.Types.Enums;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using Test_bot.ViewPages.ForUser;
using Telegram.Bot.Polling;

namespace Test_bot.ViewPages.User
{
    public static class AStartPage

    {
        static TelegramBotClient Bot = BotService.Bot;
        static ReceiverOptions receiverOptions = BotService.receiverOptions;


        public static async Task StartPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
           

            ICategoryRespository categoryRespository = new CategoryRespository();


            List<string> strings = new List<string>();
            strings.Add("test boshlash");
            strings.Add("mening testlarim");
            strings.Add("🔍 Qidirish");
            strings.Add("✉️ Xabar yuborish");



            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(strings, 2));

            await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang", replyMarkup: replyKeyboard);


            await bot.ReceiveAsync(updateHandler, Program.ErrorHandler, receiverOptions);


        }

        private static async Task updateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    if (update.Message.Text == "test boshlash")
                    {
                        await UCategoryPage.CategoryPage(bot, update, arg3);
                    }

                    else if (update.Message.Text == "mening testlarim")
                    {
                        // user ishlagan testlar ro`yxati
                        await UMyTestsPage.MyTestsPage(bot, update, arg3, update.Message.Chat.Id.ToString());

                    }
                    else if (update.Message.Text != "/start" && update.Message.Text != "🟰 Asosiy menu")
                    {
                        await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang");

                    }
                }
            }
        }




    }
}
