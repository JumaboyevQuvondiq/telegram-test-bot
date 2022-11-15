using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class AnswerPage
    {
        public string QuestionTitle { get; set; }
        public char TrueAnswer { get; set; }
        public char UserAnswer { get; set; }
    }
}
