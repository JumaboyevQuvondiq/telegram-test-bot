using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class User
    {
        public string UserId { get; set; } = default!;
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
