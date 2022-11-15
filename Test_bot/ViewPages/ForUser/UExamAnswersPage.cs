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

namespace Test_bot.ViewPages.ForUser
{
    public class UExamAnswersPage
    {
        private static readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);

        public static async Task ExamAnswerPage(ITelegramBotClient bot, Update update, ExamInfo examInfo)
        {
            try
            {
               await _connection.OpenAsync();
                var str = $"<b>{examInfo.title}:  to'g'ri javoblar: {examInfo.true_answer_count}/{examInfo.questions_count}   {examInfo.start_time}\n\n </b>";

                var query = "select ";


            }
            catch
            {

            }
            finally
            {
                _connection.Close();
            }

        }
    }
}
