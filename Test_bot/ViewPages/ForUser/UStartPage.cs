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
    public static class UStartPage

    {
       
        public static async Task StartPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            var Id = update.Message.Chat.Id.ToString();
            var FullName = update.Message.Chat.FirstName + " " + update.Message.Chat.LastName;
            while (FullName.Contains('\''))
            {
                FullName = FullName.Replace('\'', '`');
            }
            var Username =""+ update.Message.Chat.Username;
            models.User user = new models.User()
            {
                UserId = Id,
                FullName = FullName,
                UserName = Username

            };
            IUserRespository respository = new UserRespository();
            await respository.CreateAsync(user);

            ICategoryRespository categoryRespository = new CategoryRespository();


            List<string> strings = new List<string>();
            strings.Add("🖊 test boshlash");
            strings.Add("📝 mening testlarim");

            

            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(strings, 2));

            await bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang", replyMarkup: replyKeyboard);
           
                
             


        }



    }
}
