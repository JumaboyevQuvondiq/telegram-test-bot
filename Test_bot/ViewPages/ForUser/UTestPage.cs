using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types;
using Telegram.Bot;
using Test_bot.Interfaces.Respositories;
using Test_bot.Respository;
using Test_bot.Services;
using Npgsql.Internal.TypeHandlers.DateTimeHandlers;
using Microsoft.VisualBasic;
using Telegram.Bot.Types.Enums;
using Test_bot.ViewPages.User;
using Telegram.Bot.Polling;
using Test_bot.models;

namespace Test_bot.ViewPages.ForUser
{
    public class UTestPage
    {
        static TelegramBotClient Bot = BotService.Bot;
        static ReceiverOptions receiverOptions = BotService.receiverOptions;
        public static List<string> strings = new List<string>();
        static char TrueAnswer;
        static int count_true_answer = 0;
        static List<Question> questions;
        static UserExam userExam;
        static int index = 0;
       

        public static async Task TestPage(ITelegramBotClient bot, Update update, CancellationToken arg3)
        {
            

            var Exam_id = Program.exams[update.Message.Text];
            IQuestionRespository questionRespository = new QuestionRespository();
             userExam = new UserExam()
            {
               
                userId = update.Message.Chat.Id.ToString(),
                ExamId = Exam_id,
                StartTime = DateTime.Now,   
                TrueAnswerCount = 0,

            };
            IUserExamRespository userExamRespository = new UserExamRespository();
            await userExamRespository.CreateAsync(userExam);
            var user_exams = (await userExamRespository.GetAllAsync()).ToList();
            userExam.id = user_exams[user_exams.Count-1].id;
            
            index = 0;


             questions = (await questionRespository.GetAllAsync()).Where(x => (x.ExamId) == Exam_id).Select(x => x).ToList();

            if (questions.Count > 0)
                await test(bot, update, arg3, questions[0]);
            else
            {
                
               await bot.SendTextMessageAsync(update.Message.Chat.Id, "Uzr. Hozircha test tayyor emas");
                await bot.ReceiveAsync(updateHandler, Program.ErrorHandler, receiverOptions);
            }

        }

        private static async Task updateHandler(ITelegramBotClient arg1, Update update, CancellationToken arg3)
        {
            if (update.Type == UpdateType.Message)
            {
                if (update.Message.Type == MessageType.Text)
                {
                    if (update.Message.Text.ToUpper()[0] == TrueAnswer)
                    {
                        count_true_answer+=1;
                    }
                    else if (update.Message.Text == "/start")
                    {
                        await UStartPage.StartPage(arg1, update, arg3);

                    }
                    else if (update.Message.Text == "🔙 Orqaga")
                    {
                        update.Message.Text = UExamsPage.category;
                        await UExamsPage.UExamPage(arg1,update,arg3);
                    }
                    else if (update.Message.Text == "🟰 Asosiy menu")
                    {
                      
                        await UStartPage.StartPage(arg1, update, arg3);
                    }
                    else if (!Program.exams.Keys.Contains(update.Message.Text))
                    {
                        await Bot.SendTextMessageAsync(update.Message.Chat.Id, "Menyudan tanlang");
                    }
                }
            }


            else if (update.Type == UpdateType.CallbackQuery)
            {
                IUserAnswerRespository userAnswerRespository = new UserAnswerRespository();
                UserAnswer userAnswer = new UserAnswer() { 
                    UserExamId = userExam.id,
                    QuestionId = questions[index].Id,
                    Answer = update.CallbackQuery.Data[0]
                };
                await userAnswerRespository.CreateAsync(userAnswer);

                if (update.CallbackQuery.Data.ToUpper()[0] == TrueAnswer)
                {

                    userExam.TrueAnswerCount++;
                    

                    Console.WriteLine(update.CallbackQuery.Data);
                    
                }
                else
                {

                }
                if (index < questions.Count - 1)
                {
                    index++;
                    await test(arg1, update, arg3, questions[index]);
                }
                else
                {
                    IUserExamRespository userExamRespository = new UserExamRespository();
                    await userExamRespository.UpdateAsync(userExam);
                    await arg1.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, "test yakunlandi");
                }

                
                Console.WriteLine(count_true_answer);
               

            }

        }

        static async Task test(ITelegramBotClient bot, Update update, CancellationToken arg3, Question question)
        {
            if (update.Type == UpdateType.Message)
            {
                string str = $"<b>{question.Title}</b>\n\nA) {question.OptionA}\nA) {question.OptionB}\nA) {question.OptionC}\nA) {question.OptionD}";
                await bot.SendTextMessageAsync(update.Message.Chat.Id, str, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: PagesService.InlineMarkup());
                TrueAnswer = question.TrueAnswer;
                update.Message = null;
                await bot.ReceiveAsync(updateHandler, Program.ErrorHandler, receiverOptions);

            }
            else if (update.Type == UpdateType.CallbackQuery) 
            {
                string str = $"<b>{question.Title}</b>\n\nA) {question.OptionA}\nA) {question.OptionB}\nA) {question.OptionC}\nA) {question.OptionD}";
                await bot.EditMessageTextAsync(update.CallbackQuery.Message.Chat.Id, update.CallbackQuery.Message.MessageId, str, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: PagesService.InlineMarkup());
               // await bot.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, str, Telegram.Bot.Types.Enums.ParseMode.Html, replyMarkup: PagesService.InlineMarkup());
                TrueAnswer = question.TrueAnswer;
               
            }
            
        }


    }
}
