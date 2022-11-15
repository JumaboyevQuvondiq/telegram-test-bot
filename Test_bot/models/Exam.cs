using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class Exam
    {
        public int Id { get; set; }
        public string title { get; set; }
        public int total_question { get; set; } 
        public string admin_id { get; set; }   
        public int category_id  { get; set; }

    }
}
