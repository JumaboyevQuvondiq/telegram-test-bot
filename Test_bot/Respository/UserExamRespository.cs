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
    public class UserExamRespository : IUserExamRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
        public async Task<int> CreateAsync(UserExam Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO user_exams ( user_id, exam_id, start_time, true_answer_count)" +
                $" VALUES( @user_id, @exam_id, @start_time, @true_answers); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {

                        new NpgsqlParameter("user_id", Object.userId),
                        new NpgsqlParameter("exam_id", Object.ExamId),
                        new NpgsqlParameter("start_time", Object.StartTime),
                        new NpgsqlParameter("true_answers", Object.TrueAnswerCount)
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

        public async Task<IEnumerable<UserExam>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM user_exams;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<UserExam> UserExams = new List<UserExam>();
                while (await reader.ReadAsync())
                {
                    UserExam user_exam = new UserExam()
                    {
                        id = reader.GetInt32(0),
                        userId = reader.GetString(1),
                        ExamId = reader.GetInt32(2),
                        StartTime = reader.GetDateTime(3),
                        TrueAnswerCount = reader.GetInt32(4)

                    };
                    UserExams.Add(user_exam);
                }
                return UserExams;
            }
            catch
            {
                return new List<UserExam>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<UserExam> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM user_exams where id = @id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", int.Parse(Id)) }
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return  new UserExam()
                    {
                        id = reader.GetInt32(0),
                        userId = reader.GetString(1),
                        ExamId = reader.GetInt32(2),
                        StartTime = reader.GetDateTime(3),
                        TrueAnswerCount = reader.GetInt32(4)

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

        public async Task<int> UpdateAsync(UserExam Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "update user_exams " +
                                $"set user_id = @user_id, exam_id = @exam_id, start_time = @start_time, true_answer_count = @true_answer" +
                                $" where id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters ={new NpgsqlParameter("user_id", Object.userId),
                                new NpgsqlParameter ("exam_id",Object.ExamId),
                                new NpgsqlParameter("start_time", Object.StartTime),
                                new NpgsqlParameter("true_answer", Object.TrueAnswerCount),
                                new NpgsqlParameter("Id", Object.id)
                                }

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
