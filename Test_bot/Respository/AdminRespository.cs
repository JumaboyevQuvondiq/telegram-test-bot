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
    public class AdminRespository : IAdminRespository
    {
        private readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);

        public async Task<int> CreateAsync(Admin Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = "INSERT INTO admins (id, name)" +
                $" VALUES(@id, @name ); ";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = {
                                   new ("id" , Object.Id ),
                                   new ("name", Object.Name)
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

        public async Task<int> DeleteAsync(Admin Object)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"delete from admins where id  = @id";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id",Object.Id) }
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

        public async Task<IEnumerable<Admin>> GetAllAsync()
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM admins;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection);
                var reader = await command.ExecuteReaderAsync();
                List<Admin> Admins = new List<Admin>();
                while (await reader.ReadAsync())
                {
                    Admin user = new Admin()
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1)
                       
                    };
                    Admins.Add(user);
                }
                return Admins;
            }
            catch
            {
                return new List<Admin>();
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Admin> GetAsync(string Id)
        {
            try
            {
                await _connection.OpenAsync();
                string query = $"SELECT * FROM admins where id = @id;";
                NpgsqlCommand command = new NpgsqlCommand(query, _connection)
                {
                    Parameters = { new NpgsqlParameter("id",Id) }
                };
                var reader = await command.ExecuteReaderAsync();
                var result = await reader.ReadAsync();
                if (result)
                {
                    return new Admin()
                    {
                        Id = reader.GetString(0),
                        Name = reader.GetString(1),
                      
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
