using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Test_bot.Constants;
using Test_bot.models;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Polling;
using Test_bot.Services;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.Enums;
using System.Net.Sockets;
using Test_bot.Respository;

namespace Test_bot.ViewPages.ForUser
{
    public class UMyTestsPage
    {
        private static readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
        static ReceiverOptions receiverOptions = BotService.receiverOptions;
        static List<ExamInfo> Exams = new List<ExamInfo>();    
        public static async Task MyTestsPage(ITelegramBotClient bot, Update update, CancellationToken arg3, string UserId)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "select user_exams.id, exams.title, exams.total_question, user_exams.true_answer_count, user_exams.start_time from user_exams" +
                    $" join exams on  exams.id = user_exams.exam_id" +
                    $" where  user_exams.user_id = @id";
                NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", UserId) }
                };
                var reader = await npgsqlCommand.ExecuteReaderAsync();
                 Exams = new List<ExamInfo>();
                while (await reader.ReadAsync())
                {
                    ExamInfo exam = new ExamInfo()
                    {
                        id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        questions_count = reader.GetInt32(2),
                        true_answer_count = reader.GetInt32(3),
                        start_time = reader.GetDateTime(4)

                    };

                    Exams.Add(exam);
                    Exams = Exams.OrderByDescending(x => x.start_time).ToList();
                }
                List<List<InlineKeyboardButton>> inlineKeyboardButtons = new List<List<InlineKeyboardButton>>();
                inlineKeyboardButtons.Add(new List<InlineKeyboardButton>());
                int index = 0;
                var str = "<b>Mening testlarim:</b>\n";
                for (int i = 0; i < Exams.Count; i++)
                {
                    str += $"{i + 1}) {Exams[i].title}:  to`g`ri javoblar {Exams[i].true_answer_count}/{Exams[i].questions_count}   {Exams[i].start_time}\n\n";
                    if ((i+1)%5 == 0)
                    {
                        index++;
                        inlineKeyboardButtons.Add(new List<InlineKeyboardButton>());
                    }
                    inlineKeyboardButtons[index].Add(InlineKeyboardButton.WithCallbackData(text: (i + 1).ToString(), callbackData: (i + 1).ToString()));
                }

                await bot.SendTextMessageAsync(update.Message.Chat.Id, str, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(inlineKeyboardButtons));
                await bot.ReceiveAsync(updateHandler, Program.ErrorHandler, receiverOptions);
            }
            catch { }
            finally
            {
                _connection.Close();
            }
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
                       

                    }
                }
            }
            else if (update.Type == UpdateType.CallbackQuery)
            {
                var index = int.Parse(update.CallbackQuery.Data)-1;
                // ExamInfo funksiyaga ketadi
                var str = $"<b>{Exams[index].title}  {Exams[index].true_answer_count}/{Exams[index].questions_count}  {Exams[index].start_time}</b>\n";
                UserAnswersPageRespository userAnswersPageRespository = new UserAnswersPageRespository();
                 var result =  await userAnswersPageRespository.GetAllAsync(Exams[index].id);
                str += PagesService.AnswerPage(result.ToList());

               await  bot.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, str, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
                
        }
    }
           
}
