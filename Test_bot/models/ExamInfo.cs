using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test_bot.models
{
    public class ExamInfo
    {
        public int id { get; set; }
        public string title { get; set; }
        public int true_answer_count { get; set; }
        public int questions_count { get; set; }    
        public DateTime start_time { get; set; }
    }
}
