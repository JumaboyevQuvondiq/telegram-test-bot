using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Test_bot.Constants;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;

namespace Test_bot.Respository
{
    public class CategoryRespository : ICategoryRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);

        public async Task<int> CreateAsync(Category category)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO categories(category_name)" +
                $" VALUES(@name); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {new NpgsqlParameter("name",category.Name)}
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

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from categories where id  = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters={new NpgsqlParameter("Id",id)}
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

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM categories;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<Category> categories = new List<Category>();
                while (await reader.ReadAsync())
                {
                    Category user = new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        count_tests= reader.GetInt32(2)
                    };
                    categories.Add(user);
                }
                return categories;
            }
            catch
            {
                return new List<Category>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Category> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM categories where id = @Id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("Id",Id) } 
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return new Category()
                    {
                        Id = reader.GetInt32(0),
                        Name = reader.GetString(1),
                        count_tests = reader.GetInt32(2)
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

        public async Task<int> UpdateAsync(Category category)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "update categories " +
                                $"set category_name = @name"+
                                $" where id = @Id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters={new NpgsqlParameter("name", category.Name),
                                new NpgsqlParameter ("Id",category.Id)},
                    


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
