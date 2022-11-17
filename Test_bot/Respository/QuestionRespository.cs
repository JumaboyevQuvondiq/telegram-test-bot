using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Test_bot.Constants;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;

namespace Test_bot.Respository
{
    public class QuestionRespository : IQuestionRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);

        public async Task<int> CreateAsync(Question Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO questions (title, true_answer, option_a, option_b, option_c, option_d, exam_id)" +
                $" VALUES(@title, @true_answer, @option_a, @option_b, @option_c, @option_d, @exam_id); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {
                        new NpgsqlParameter("title", Object.Title),
                        new NpgsqlParameter("true_answer", Object.TrueAnswer),
                        new NpgsqlParameter("option_a", Object.OptionA),
                        new NpgsqlParameter("option_b", Object.OptionB),
                        new NpgsqlParameter("option_c", Object.OptionC),
                        new NpgsqlParameter("option_d", Object.OptionD),
                        new NpgsqlParameter("exam_id", Object.ExamId)
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

        public async Task<int> DeleteAsync(Question Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from questions where id  = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", Object.Id) }
                };
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

        public async Task<IEnumerable<Question>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM questions;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<Question> questions = new List<Question>();
                while (await reader.ReadAsync())
                {
                    Question question = new Question()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        TrueAnswer = reader.GetChar(2),
                        OptionA = reader.GetString(3),
                        OptionB = reader.GetString(4),
                        OptionC = reader.GetString(5),
                        OptionD = reader.GetString(6),
                        ExamId = reader.GetInt32(7)

                    };
                    questions.Add(question);
                }
                return questions;
            }
            catch
            {
                return new List<Question>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

            public async Task<Question> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM questions where id = @id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", int.Parse(Id)) }
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return new Question()
                    {
                        Id = reader.GetInt32(0),
                        Title = reader.GetString(1),
                        TrueAnswer = reader.GetChar(2),
                        OptionA = reader.GetString(3),
                        OptionB = reader.GetString(4),
                        OptionC = reader.GetString(5),
                        OptionD = reader.GetString(6),
                        ExamId = reader.GetInt32(7)

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

        public async Task<int> UpdateAsync(Question Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "update questions " +
                                $"set title = @title, true_answer = @true_answer, option_a = @option_a, option_b = @option_b, option_c = @option_c,option_d = @option_d, exam_id = @exam_id" +
                                $" where id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters ={new NpgsqlParameter("title", Object.Title),
                                new NpgsqlParameter ("true_answer",Object.TrueAnswer),
                                new NpgsqlParameter("option_a", Object.OptionA),
                                new NpgsqlParameter("option_b", Object.OptionB),
                                new NpgsqlParameter("option_c",Object.OptionC),
                                new NpgsqlParameter("option_d", Object.OptionD),
                                new NpgsqlParameter("exam_id", Object.ExamId)
                                },



                };
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
    }
}
