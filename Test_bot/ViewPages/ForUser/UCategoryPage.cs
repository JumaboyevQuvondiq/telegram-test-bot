using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot;
using Test_bot.Services;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Test_bot.Interfaces.Respositories;
using Test_bot.Respository;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.Enums;
using Test_bot.ViewPages.User;

namespace Test_bot.ViewPages.ForUser
{
    public class UCategoryPage
    {
        
        public static async Task CategoryPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {


            ICategoryRespository categoryRespository = new CategoryRespository();


            List<string> strings = new List<string>();
            foreach (var Item in await categoryRespository.GetAllAsync())
                strings.Add(Item.Name);
            strings.Add("🟰 Asosiy menu");

            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(strings, 2));

            await bot.SendTextMessageAsync(update.Message.Chat.Id, "Kategoriya tanlang", replyMarkup: replyKeyboard);

          
        
        }

    }
}