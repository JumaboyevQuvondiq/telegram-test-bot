using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Test_bot.Interfaces.Respositories;
using Test_bot.Respository;

namespace Test_bot.Services
{
    public static class AdminHelper
    {
        public static async Task<bool> IsAdmin(string id)
        {
            IAdminRespository adminRespository = new AdminRespository();
            var result = adminRespository.GetAllAsync();
            var list = result.Result.ToList().Select(x => x.Id).ToList();
            return  list.Contains(id);
        }
    }
}
