using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class Question
    {
        public int Id { get; set; } 
        public string Title { get; set; }   
        public char TrueAnswer { get; set; }  
        public string OptionA { get; set; } 
        public string OptionB { get; set; } 
        public string OptionC { get; set; } 
        public string OptionD { get; set; }
        public int ExamId { get; set; } 
    }
}
