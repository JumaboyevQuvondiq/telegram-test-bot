using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class UserExam
    {
        public int id { get; set; }
        public string userId { get; set; } 
        public int ExamId { get; set; } 
        public DateTime StartTime { get; set; }
        public int TrueAnswerCount { get; set; }

    }
}
