using Microsoft.VisualBasic;
using System;
using System.Security.Cryptography.X509Certificates;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using Test_bot.Interfaces.Respositories;
using Test_bot.models;
using Test_bot.Respository;
using Test_bot.Services;
using Test_bot.ViewPages.ForUser;
using Test_bot.ViewPages.User;
using User = Test_bot.models.User;

class Program

{
  

  

    public static Dictionary<string, int> categories;
    public  static Dictionary<string, int> exams;
    public static bool addCotegory = false;
    public static bool AddTest = false;
    public static int category_Id;
    public static int test_id;
    public static bool addQuestion = false;
    public static bool isFind = false;
    public static int page = 0;


    public static TelegramBotClient Bot = BotService.Bot;
    public static ReceiverOptions receiverOptions = BotService.receiverOptions;
    public static async Task Main()
    {

        
        ICategoryRespository categoryRespository = new CategoryRespository();
        categories = (await categoryRespository.GetAllAsync()).ToDictionary(x => x.Name, x => x.Id);
        IExamRespository examRespository = new ExamRespository();
         exams = (await examRespository.GetAllAsync()).ToDictionary(x => x.title, x => x.Id);
        Bot.StartReceiving(UpdateHandler, ErrorHandler, receiverOptions);
        
        IAdminRespository adminRespository = new AdminRespository();
        
       
       
        Console.ReadKey();  
    }






    public static Task ErrorHandler(ITelegramBotClient arg1, Exception arg2, CancellationToken arg3)
    {
        throw new NotImplementedException();
    }






    public static async Task UpdateHandler(ITelegramBotClient bot, Update update, CancellationToken arg3)
    {
        List<KeyboardButton[]> keyboardButtons = new List<KeyboardButton[]>();

        if (update.Type == UpdateType.Message)
        {
            if (await AdminHelper.IsAdmin(update.Message.Chat.Id.ToString()))
            {
                if(update.Message.Type == MessageType.Text)
                {
                    ICategoryRespository categoryRespository = new CategoryRespository();
                    IExamRespository examRespository = new ExamRespository();


                    var text  = update.Message.Text;
                    
                    if (text == "📁 kategoriyalar")
                    {
                        

                        List<string> strings = new List<string>();
                        strings.Add("🟰 Asosiy menu");
                        strings.Add("📁 kategoriya qo'shish");
                        categories = (await categoryRespository.GetAllAsync()).ToDictionary(x => x.Name, x => x.Id);
                        foreach (var Item in  categories.Keys)
                            strings.Add(Item.ToString());
                        ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(strings, 2));

                        await bot.SendTextMessageAsync(update.Message.Chat.Id, "Kategoriya tanlang", replyMarkup: replyKeyboard);


                    }

                    else if (text == "/start" || text == "🟰 Asosiy menu")
                    {
                        categories = (await categoryRespository.GetAllAsync()).ToDictionary(x => x.Name, x => x.Id);
                        addQuestion = false;
                        exams = (await examRespository.GetAllAsync()).ToDictionary(x => x.title, x => x.Id);
                        await AStartPage.StartPage(bot, update, arg3);

                    }

                    // ketegoriya qo'shish
                    else if (text == "📁 kategoriya qo'shish" || addCotegory == true)
                    {
                        try
                        {
                            if (!addCotegory)
                            {
                                addCotegory = true;
                               
                                List<string> menu = new List<string>();
                                menu.Add("🟰 Asosiy menu");
                                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menu, 1));
                                await bot.SendTextMessageAsync(update.Message.Chat.Id, "📁 Kategoriya nomini kiriting", replyMarkup: replyKeyboard);
                            }
                            else
                            {
                                addCotegory = false;
                                Category category = new Category()
                                {
                                    Id = 1,
                                    count_tests = 0,
                                    Name = text.Trim()
                                };

                               
                               var result =  await  categoryRespository.CreateAsync(category);
                                if (result==1)
                                 await bot.SendTextMessageAsync(update.Message.Chat.Id, "📁 Kategoriya qoshildi ✅");
                                else
                                {
                                    await bot.SendTextMessageAsync(update.Message.Chat.Id, "📁 Kategoriya qoshilmadi ❌");
                                }
                                await AStartPage.StartPage(bot, update, arg3);
                                
                            }
                        }
                        catch { }
                    }

                    // exam page
                    else if (categories.Keys.Contains(text))
                    {
                        try
                        {
                           
                              
                                 category_Id = categories[text];
                                var exams_list = (await examRespository.GetAllAsync()).Where(x => (x.category_id) == category_Id).Select(x => x.title).ToList();
                                List<string> menuExamPage = new List<string>();
                                menuExamPage.Add("📚 Test qo'shish");
                                menuExamPage.Add("🟰 Asosiy menu");
                                foreach (var item in exams_list)
                                {
                                    menuExamPage.Add(item);
                                }
                                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menuExamPage, 2)) { ResizeKeyboard = true };

                                await bot.SendTextMessageAsync(update.Message.Chat.Id, "📚 Testni tanglang", replyMarkup: replyKeyboard);
                            
                        
                        }
                        catch
                        {

                        }
                    }
                    else if (text == "📚 Test qo'shish" || AddTest)
                    {
                        try
                        {
                            if (!AddTest)
                            {
                                AddTest = true;
                               
                               
                                List<string> menuExamPage = new List<string>();
                             
                                menuExamPage.Add("🟰 Asosiy menu");
                              
                                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menuExamPage, 1)) { ResizeKeyboard = true };

                                await bot.SendTextMessageAsync(update.Message.Chat.Id, "📚 Test nomini kiriting", replyMarkup: replyKeyboard);
                            }

                            else
                            {
                                AddTest = false;
                                Exam exam = new Exam()
                                {
                                    admin_id = update.Message.Chat.Id.ToString(),
                                    title = text.Trim(),
                                    total_question = 0,
                                    category_id = category_Id


                                };
                               

                                var result = await examRespository.CreateAsync(exam);
                                if (result == 1)
                                    await bot.SendTextMessageAsync(update.Message.Chat.Id, "📚 Test qoshildi ✅");
                                else
                                {
                                    await bot.SendTextMessageAsync(update.Message.Chat.Id, "📚 Test qoshilmadi ❌");
                                }
                                await AStartPage.StartPage(bot, update, arg3);

                            }

                        }
                        catch
                        {

                        }
                    }

                    //Testga kirish

                    else if (exams.Keys.Contains(text))
                    {
                        try
                        {
                        
                                test_id = exams[text];
                                List<string> menuExamPage = new List<string>();
                                menuExamPage.Add("📘 Testga savol va javoblarni qo'shish");
                                menuExamPage.Add("🟰 Asosiy menu");
                              
                                ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menuExamPage, 2)) { ResizeKeyboard = true };

                                await bot.SendTextMessageAsync(update.Message.Chat.Id, "Testga savol va javoblarni qo`shasizmi? ", replyMarkup: replyKeyboard);

                        }
                        catch
                        {

                        }

                    }

                    // testga savol va jovoblarni qo'shish

                    else  if (text == "📘 Testga savol va javoblarni qo'shish" || addQuestion)
                    {
                        try
                        {
                            if (!addQuestion)
                            {
                                addQuestion = true; 
                                IQuestionRespository questionRespository = new QuestionRespository();
                                var question = await questionRespository.GetAsync("1");
                                string str = $"{question.Title}\nA) {question.OptionA}\nB) {question.OptionB}\nC) {question.OptionC}\nD) {question.OptionD}\n*A";
                                await bot.SendTextMessageAsync(update.Message.Chat.Id, "❗️ Eslatma:\nTestni savol va javoblarini quyidagicha yuborishingizni so'raymiz 😊");
                                await bot.SendTextMessageAsync(update.Message.Chat.Id, str);
                            }
                            else
                            {
                                if(text.StringToQuestion().Title != String.Empty)
                                {
                                    var question = text.StringToQuestion();
                                    question.ExamId = test_id;
                                    IQuestionRespository questionRespository = new QuestionRespository();
                                    var result = await questionRespository.CreateAsync(question);
                                    
                                        if (result == 1)
                                            await bot.SendTextMessageAsync(update.Message.Chat.Id, "📘 Testga savol va javoblarni qo'shildi ✅");
                                        else
                                        {
                                            await bot.SendTextMessageAsync(update.Message.Chat.Id, "📘 Testga savol va javoblarni qo'shilmadi ❌");
                                        }
                                    
                                }
                            }

                        }
                        catch { }
                    }
                    else if (text == "🔍 Qidirish")
                    {
                        isFind = true;
                        List<string> menuExamPage = new List<string>();

                        menuExamPage.Add("🟰 Asosiy menu");

                        ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menuExamPage, 1)) { ResizeKeyboard = true };
                        await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                            text: $"<b>Qidirish uchun foydalanuvchining Telegram idsini, Usernameni yoki Ismini yozib yuboring: </b>",
                            parseMode: ParseMode.Html,
                            replyMarkup:replyKeyboard);
                       
                    }// Qidirish 
                    else if (isFind)
                    {

                            int n;
                            string matn = "";
                            string first = "";
                            var result = text != "▶️" && text != "◀️" && text != "🔴" ? FindService.UserSearch(text) : FindService.UserSearch(first);
                            int pages = (result.Count() + 9) / 10;

                            if (text == "🔴")
                            {
                                await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                    text: $"Page topilmadi",
                                    parseMode: ParseMode.Html);
                            }
                            else if (text == "▶️")
                            {
                                page++;
                                matn = FindService.SearchResult(result, page * 10, 10);
                                if (pages == 1)
                                {
                                    n = 0;
                                }
                                else if (page == 0)
                                {
                                    n = 2;
                                }
                                else if (page == pages - 1)
                                {
                                    n = 3;
                                }
                                else
                                {
                                    n = 1;
                                }
                                ReplyKeyboardMarkup search_manu =
                                new ReplyKeyboardMarkup(FindService.SearchButtons(n));

                                await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                    text: $"{matn}",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: search_manu);
                            }
                            else if (text == "◀️")
                            {
                                page--;
                                matn = FindService.SearchResult(result, page * 10, 10);
                                if (pages == 1)
                                {
                                    n = 0;
                                }
                                else if (page == 0)
                                {
                                    n = 2;
                                }
                                else if (page == pages - 1)
                                {
                                    n = 3;
                                }
                                else
                                {
                                    n = 1;
                                }
                                ReplyKeyboardMarkup search_manu =
                                new ReplyKeyboardMarkup(FindService.SearchButtons(n));

                                await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                    text: $"{matn}",
                                    parseMode: ParseMode.Html,
                                    replyMarkup: search_manu);
                            }
                            else if (result.Count() > 0)
                            {
                                first = text;
                                matn = FindService.SearchResult(result, page * 10, 10);
                                if (pages == 1)
                                {
                                    n = 0;
                                }
                                else if (page == 0)
                                {
                                    n = 2;
                                }
                                else if (page == pages - 1)
                                {
                                    n = 3;
                                }
                                else
                                {
                                    n = 1;
                                }
                                ReplyKeyboardMarkup search_manu =
                                new ReplyKeyboardMarkup(FindService.SearchButtons(n));

                            
        

                                        await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                        text: $"{matn}",
                                        parseMode: ParseMode.Html,
                                        replyMarkup: search_manu);
                            }
                            else
                            {
                            List<string> menuExamPage = new List<string>();

                            menuExamPage.Add("🟰 Asosiy menu");

                            ReplyKeyboardMarkup replyKeyboard = new ReplyKeyboardMarkup(PagesService.SortingPrint(menuExamPage, 1)) { ResizeKeyboard = true };
                            await Bot.SendTextMessageAsync(chatId: update.Message.Chat.Id,
                                    text: $"<b>Topilmadi!</b>",
                                    parseMode: ParseMode.Html,
                                    replyMarkup:replyKeyboard);
                            }
                    }
                }


            }

 

            else  if (update.Message.Type == MessageType.Text)
            {
                if (update.Message.Text == "/start" || update.Message.Text == "🟰 Asosiy menu")
                {
                    ICategoryRespository categoryRespository = new CategoryRespository();
                    categories = (await categoryRespository.GetAllAsync()).ToDictionary(x => x.Name, x => x.Id);
                    await UStartPage.StartPage(bot, update, arg3);

                }
               else if (update.Message.Text == "🖊 test boshlash" || update.Message.Text == "⬅️ Orqaga")
                {
                    await UCategoryPage.CategoryPage(bot, update, arg3);
                } 
             
                else if (categories.Keys.Contains(update.Message.Text))
                {
                  
                    await UExamsPage.UExamPage(bot, update, arg3);    
                } 
                else if (update.Message.Text == "🔙 Orqaga")
                {
                    update.Message.Text = UExamsPage.category;
                    await UExamsPage.UExamPage(bot, update, arg3);
                }

                else if (UExamsPage.strings.Contains(update.Message.Text))
                {
                    List<string> strings2 = new List<string>();
                    strings2.Add("🔙 Orqaga");
                    strings2.Add("🟰 Asosiy menu");
                    await bot.SendTextMessageAsync(update.Message.Chat.Id, "test boshlandi", replyMarkup: new ReplyKeyboardMarkup(PagesService.SortingPrint(strings2, 2)) { ResizeKeyboard = true });
                    UTestPage uTestPage = new UTestPage();
                    await uTestPage.TestPage(bot, update, arg3);
                }

                
                else if (update.Message.Text == "📝 mening testlarim")
                {
                    // user ishlagan testlar ro`yxati
                    await UMyTestsPage.MyTestsPage(bot, update, arg3, update.Message.Chat.Id.ToString());

                }


            }
        }



        else if (update.Type == UpdateType.CallbackQuery)
        {
            if (int.TryParse(update.CallbackQuery.Data, out int son))
            {
                var index = int.Parse(update.CallbackQuery.Data) - 1;
                // ExamInfo funksiyaga ketadi
                var str = $"<b>{UMyTestsPage.Exams[index].title}  {UMyTestsPage.Exams[index].true_answer_count}/{UMyTestsPage.Exams[index].questions_count}  {UMyTestsPage.Exams[index].start_time}</b>\n";
                UserAnswersPageRespository userAnswersPageRespository = new UserAnswersPageRespository();
                var result = await userAnswersPageRespository.GetAllAsync(UMyTestsPage.Exams[index].id);
                str += PagesService.AnswerPage(result.ToList());

                await bot.SendTextMessageAsync(update.CallbackQuery.Message.Chat.Id, str, Telegram.Bot.Types.Enums.ParseMode.Html);
            }
            
        }


    }

}