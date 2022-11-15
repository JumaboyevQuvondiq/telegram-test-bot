using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Test_bot.Constants;
using Test_bot.Interfaces.IServices;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;

namespace Test_bot.Respository
{
    public class UserRespository : IUserRespository
    {
        private readonly NpgsqlConnection  _connection = new NpgsqlConnection (DbConstant.CONNECTION);
        public async Task<int> CreateAsync(User user)
        {

            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO public.users(user_id, full_name, username)" +
                $" VALUES(@Id, @name, @username); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection) 
                {
                    Parameters = {new NpgsqlParameter("Id",user.UserId),
                                  new NpgsqlParameter("name", user.FullName),
                                  new NpgsqlParameter("username", user.UserName)
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

        public async Task<int> DeleteAsync(string id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from users where user_id ilike '%@Id%'";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection) 
                { 
                    Parameters = {new NpgsqlParameter("Id",id) } 
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

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM users;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<User> users = new List<User>();
                while (await reader.ReadAsync())
                {
                    User user = new User()
                    {
                        UserId = reader.GetInt32(0).ToString(),
                        FullName = reader.GetString(1),
                        UserName = reader.GetString(2)
                    };
                    users.Add(user);
                }
                return users;
            }
            catch
            {
                return new List<User>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<User?> GetAsync(string id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM users where user_id like '%@Id%';";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {new NpgsqlParameter("Id", id)}
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return new User()
                    {
                        UserId = reader.GetString(0),
                        FullName = reader.GetString(1),
                        UserName = reader.GetString(2)
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

        public async Task<int> UpdateAsync(User user)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "update users " +
                                $"set full_name = @name, username = @userName," +
                                $" where user_id = '@userId'";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters= {new NpgsqlParameter("full_name",user.FullName),
                                 new NpgsqlParameter("username",user.UserName),
                                 new NpgsqlParameter("userId",user.UserId)}
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
