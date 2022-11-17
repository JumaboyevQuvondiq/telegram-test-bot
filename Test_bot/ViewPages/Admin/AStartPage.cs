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
        public static async Task StartPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
           

            ICategoryRespository categoryRespository = new CategoryRespository();


            List<string> strings = new List<string>();
            strings.Add("📁 kategoriyalar");
            strings.Add("🔍 Qidirish");
          



            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(strings, 2));

            await bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang", replyMarkup: replyKeyboard);




        }


    }
}
