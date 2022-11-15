using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;
using Test_bot.Constants;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;

namespace Test_bot.Respository
{
    public class UserAnswersPageRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
        public async Task<IEnumerable<AnswerPage>> GetAllAsync(int id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "select questions.title, questions.true_answer, user_answers.answer from user_answers"+
                    $" join questions on questions.id = user_answers.question_id"+
                    $" where user_answers.user_exam_id = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {
                        new NpgsqlParameter("id", id)
                       
                                 }
                }
                ;
                var reader = await command.ExecuteReaderAsync();
                List<AnswerPage> answerPages = new List<AnswerPage>();
                while (await reader.ReadAsync())
                {
                    AnswerPage answerPage = new AnswerPage()
                    {
                        QuestionTitle = reader.GetString(0),
                        TrueAnswer = reader.GetChar(1),
                        UserAnswer = reader.GetChar(2)
                    };
                    answerPages.Add(answerPage);
                }
                return answerPages;


            }
            catch
            {
                return new List<AnswerPage>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
