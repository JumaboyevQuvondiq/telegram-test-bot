using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.Constants;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;

namespace Test_bot.Respository
{
    public class UserAnswerRespository : IUserAnswerRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
        public async Task<int> CreateAsync(UserAnswer Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO user_answers ( user_exam_id, question_id, answer)" +
                $" VALUES( @user_exam_id, @question_id, @answer); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {

                        new NpgsqlParameter("user_exam_id", Object.UserExamId),
                        new NpgsqlParameter("question_id", Object.QuestionId),
                        new NpgsqlParameter("answer", Object.Answer)
                            }
                }
                ;
                int result = await command.ExecuteNonQueryAsync();
                return result;
            }
            catch
            {
                return 0;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<UserAnswer> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM user_answers where id = @id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", int.Parse(Id)) }
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return new UserAnswer()
                    {
                        Id = reader.GetInt32(0),
                        UserExamId = reader.GetInt32(1),
                        QuestionId = reader.GetInt32(2),
                        Answer = reader.GetChar(3)

                    };
                }
                else return null;
            }
            catch
            {
                return null;
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
