using Npgsql;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Test_bot.Constants;
using Test_bot.models;
using User = Test_bot.models.User;

namespace Test_bot.Services
{
    public  class FindService
    {
        private static readonly NpgsqlConnection _connection = new NpgsqlConnection(DbConstant.CONNECTION);
      
        public static List<User> UserSearch(string searchText)
        {
            _connection.Open();
            List<User> users = new List<User>();
            string query = $"SELECT user_id, full_name, username  FROM users " +
                $"WHERE user_id like '%{searchText}%' union " +
                $"SELECT user_id, full_name, username FROM users " +
                $"WHERE full_name like '%{searchText}%' union " +
                $"SELECT user_id, full_name, username FROM users " +
                $"WHERE username like '%@text%'";
            NpgsqlCommand npgsqlCommand = new NpgsqlCommand(query, _connection)
            {
                Parameters = {
                new NpgsqlParameter("text", searchText)
                }
            };

            var reader = npgsqlCommand.ExecuteReader();

            while (reader.Read())
            {
                User user = new User()
                {
                    UserId = reader.GetString(0),
                    FullName = reader.GetString(1),                   
                    UserName = reader.GetString(2)
                   
                };

                users.Add(user);
            }

            _connection.Close();

            return users;
        }


        public static List<KeyboardButton[]> SearchButtons(int n)
        {
            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
            if (n == 0)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🟰 Asosiy menu" });
            }
            else if (n == 1)
            {
                keyboardButtons.Add(new KeyboardButton[] { "◀️", "▶️" });
                keyboardButtons.Add(new KeyboardButton[] { "🟰 Asosiy menu" });
            }
            else if (n == 2)
            {
                keyboardButtons.Add(new KeyboardButton[] { "🔴", "▶️" });
                keyboardButtons.Add(new KeyboardButton[] { "🟰 Asosiy menu" });
            }
            else if (n == 3)
            {
                keyboardButtons.Add(new KeyboardButton[] { "◀️", "🔴" });
                keyboardButtons.Add(new KeyboardButton[] { "🟰 Asosiy menu" });
            }

            return keyboardButtons;
        }


        public static string SearchResult(List<User> user, int page, int pages)
        {
            var userr = user.Skip(page).Take(pages).ToList();
            int count = page == 0 ? 1 : page + 1;
            string matn = "";
            foreach (var item in userr)
            {
                matn = matn + $"<b>{count})\n" +
                        $"🆔 Id - {item.UserId}\n" +
                        $"👤 Foydalanuvchi - {item.FullName}\n" +
                        $"🌐 Username - @{item.UserName}</b>\n\n";
                count++;
            }

            return matn;
        }
    }
}
