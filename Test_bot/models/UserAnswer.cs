using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class UserAnswer
    { 
        public int Id { get; set; } 
        public int UserExamId { get; set; }
        public int QuestionId { get; set; } 
        public char Answer { get; set; }    

    }
}
