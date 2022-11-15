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
    public class ExamRespository : IExamRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
        public async Task<int> CreateAsync(Exam Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO exams ( title, total_question, admin_id, category_id)" +
                $" VALUES( @title, @questions, @admin_id, @category_id); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {
                       
                        new NpgsqlParameter("title", Object.title),
                        new NpgsqlParameter("questions", Object.total_question),
                        new NpgsqlParameter("admin_id", Object.admin_id),
                        new NpgsqlParameter("category_id", Object.category_id)
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

        public async Task<int> DeleteAsync(Exam Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from exams where id  = @id";
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

        public async Task<IEnumerable<Exam>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM exams;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<Exam> Exams = new List<Exam>();
                while (await reader.ReadAsync())
                {
                    Exam exam = new Exam()
                    {
                        Id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        total_question = reader.GetInt32(2),
                        admin_id = reader.GetString(3),
                        category_id = reader.GetInt32(4)

                    };
                    Exams.Add(exam);
                }
                return Exams;
            }
            catch
            {
                return new List<Exam>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Exam> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM exams where id = @id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id", int.Parse(Id)) }
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return  new Exam()
                    {
                        Id = reader.GetInt32(0),
                        title = reader.GetString(1),
                        total_question = reader.GetInt32(2),
                        admin_id = reader.GetString(3),
                        category_id = reader.GetInt32(4)

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

        public async Task<int> UpdateAsync(Exam Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "update exams " +
                                $"set title = @title, total_question = @questions, admin_id = @adminId, category_id= @categoryId" +
                                $" where id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters ={new NpgsqlParameter("title", Object.title),
                                new NpgsqlParameter ("questions",Object.total_question),
                                new NpgsqlParameter("adminId", Object.admin_id),
                                new NpgsqlParameter("categoryId",Object.category_id),
                                new NpgsqlParameter("Id", Object.Id)
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
