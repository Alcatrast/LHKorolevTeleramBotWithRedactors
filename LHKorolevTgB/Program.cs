using LHKorolevTgB.Controller;
using LHKorolevTgB.Controller.Builder;
using LHKorolevTgB.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;


namespace LHKorolevTgB
{
    class Program
    {
        private static string tokenFilePath = ConfigurationManager.AppSettings["BOT_TOKEN_FILE_PATH"];
        private static string testPath { get; } = ConfigurationManager.AppSettings["TEST_PATH"];
        private static string encPath { get; } = ConfigurationManager.AppSettings["ENCYCLOPEDIA_PATH"];
        private static string mmPath { get; } = ConfigurationManager.AppSettings["MAINMEDIA_PATH"];
        private static string mPath = ConfigurationManager.AppSettings["MAINMENU_PATH"];
        private static TelegramBotClient tbc;
        private static MainController mc;

        static void Main(string[] args)
        {
            string token = Encoding.UTF8.GetString(File.ReadAllBytes(tokenFilePath));
            //Console.WriteLine(token);
            tbc = new TelegramBotClient(token);
            Console.WriteLine("Bot initialized.");
            mc = ConstructMC();
            Console.WriteLine("Main controller ready");
            tbc.StartReceiving();
            tbc.OnMessage += OnMessageHandler;
            Console.WriteLine("Bot started...");
            Console.WriteLine("Enter \"STOP\" to stop bot.");
            string stp = "";
            do
            {
                stp = Console.ReadLine();
            }
            while (stp != "STOP");
            Console.WriteLine("Bot stopped.");
            tbc.StopReceiving();
            Console.ReadKey();
        }

        private static async void OnMessageHandler(object sender, MessageEventArgs e)
        {
            mc.ProcessingRequest(e.Message.Text);
            if (mc.MediaResponse == string.Empty)
            {
                await tbc.SendTextMessageAsync(e.Message.Chat.Id, mc.TextResponse, replyMarkup: GetButtons(mc.NextPossibleTextRequests));
            }
            else
            {
                try
                {
                    var FileUrl = mc.MediaResponse;
                    FileStream stream = System.IO.File.Open(FileUrl, FileMode.Open);
                    await tbc.SendPhotoAsync(e.Message.Chat.Id, stream, mc.TextResponse, replyMarkup: GetButtons(mc.NextPossibleTextRequests));
                    stream.Close();
                }
                catch
                {
                    await tbc.SendTextMessageAsync(e.Message.Chat.Id, mc.TextResponse, replyMarkup: GetButtons(mc.NextPossibleTextRequests));
                }
            }
        }

        private static IReplyMarkup GetButtons(List<List<string>> notbuttonLines)
        {
            List<List<KeyboardButton>> buttonLines = new List<List<KeyboardButton>>();
            List<KeyboardButton> buttons = new List<KeyboardButton>();
            foreach (var notbuttons in notbuttonLines)
            {
                foreach (var notbutton in notbuttons)
                {
                    buttons.Add(new KeyboardButton { Text = notbutton + " " });
                }
                buttonLines.Add(buttons);
                buttons = new List<KeyboardButton>();
            }
            return new ReplyKeyboardMarkup { Keyboard = buttonLines, ResizeKeyboard = true, OneTimeKeyboard = false };
        }

        private static TestController ConctructTC()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestControllerInXml));
            StreamReader rf = new StreamReader(testPath);
            TestControllerInXml testXml = (TestControllerInXml)serializer.Deserialize(rf);
            TestController testController = TestControllerBuilder.BuildFromXml(testXml);
            rf.Close();
            return testController;
        }

        private static EncyclopediaController ConstructEC()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
            StreamReader rf = new StreamReader(encPath);
            EncyclopediaControllerInXml encXml = (EncyclopediaControllerInXml)serializer.Deserialize(rf);
            EncyclopediaController encController = EncyclopediaControllerBuilder.BuildFromXml(encXml);
            rf.Close();
            return encController;
        }

        private static MainController ConstructMC()
        {
            string menutext = Encoding.UTF8.GetString(File.ReadAllBytes(mPath));
            return new MainController(ConstructEC(), ConctructTC(), "Открыть энциклопедию", "Решить тест", "!-Вернуться в меню-!", menutext, mmPath);
        }
    }
}
