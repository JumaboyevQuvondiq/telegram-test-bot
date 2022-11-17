using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Test_bot.models;

namespace Test_bot.Services
{
    public static class PagesService
    {
        public static List<KeyboardButton[]> SortingPrint(List<string> strings, int row)

        {
            List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();
            List<KeyboardButton> keyboardButtons2 = new List<KeyboardButton>();

            for (int i = 1; i <= strings.Count; i++)
            {
                keyboardButtons2.Add(new KeyboardButton(strings[i - 1]));
                if (i % row == 1) continue;
                else
                {
                    keyboardButtons.Add(keyboardButtons2.ToArray());
                    keyboardButtons2 = new List<KeyboardButton>();
                }
            }
            keyboardButtons.Add(keyboardButtons2.ToArray());

            return keyboardButtons;
        }

        public static InlineKeyboardMarkup InlineMarkup()
        {
            InlineKeyboardMarkup inlineKeyboard = new(new[]
                         {

                         new []
                            {
                                 InlineKeyboardButton.WithCallbackData(text: "A", callbackData: "A"),
                                InlineKeyboardButton.WithCallbackData(text: "B", callbackData: "B"),
                                     InlineKeyboardButton.WithCallbackData(text: "C", callbackData: "C"),
                                 InlineKeyboardButton.WithCallbackData(text: "D", callbackData: "D"),
                             }
                        });
            return inlineKeyboard ; 
        }

        public static string AnswerPage(List<AnswerPage> list)
        {
            var truee = " ✅";
            var falsee = " ❌";
            var str = "";

            for (int i = 0; i < list.Count; i++)
            {
                var result = list[i].UserAnswer == list[i].TrueAnswer ? (list[i].TrueAnswer + truee) : list[i].UserAnswer + falsee;
                str += $"{i + 1}) {list[i].QuestionTitle}\nYour answer: {result}\n";
            }
            return str;
        }

        public static Question StringToQuestion(this string str)
        {
            Question question = new Question();
            try
            {
                int length = str.IndexOf("A)");
                var title = str.Substring(0, length).Trim();
               
                length = str.IndexOf("B)")-str.IndexOf("A)")-2;
                var OptionA = str.Substring(str.IndexOf("A)") + 2, length).Trim();
               
                length = str.IndexOf("C)") - str.IndexOf("B)") - 2;
                var OptionB = str.Substring(str.IndexOf("B)") + 2,length).Trim();
              
                length = str.IndexOf("D)") - str.IndexOf("C)") - 2;
                var OptionC = str.Substring(str.IndexOf("C)") + 2, length).Trim();
              
                length = str.IndexOf("*") - str.IndexOf("D)") - 2;
                var OptionD = str.Substring(str.IndexOf("D)") + 2,length).Trim();
               
                var true_answer = str[str.IndexOf("*") + 1];

                question.Title = title;
                question.OptionA = OptionA;
                question.OptionB = OptionB;
                question.OptionC = OptionC;
                question.OptionD = OptionD;
                question.TrueAnswer = true_answer;
                return question;

            }
            catch
            {
                return new Question();
            }
        }

    }
}
