using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Test_bot.Services
{
    public static class BotService
    {
        public static TelegramBotClient Bot = new TelegramBotClient
        ("5644180790:AAEkVRwLxaEcoLD8Yvyi7yp_qTwxOIoteOg");
        public static ReceiverOptions receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = new UpdateType[]
               {
                UpdateType.Message,
                UpdateType.EditedMessage,
                UpdateType.CallbackQuery,
                UpdateType.InlineQuery
               }
        };
    }
}
