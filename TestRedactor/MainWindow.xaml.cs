using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace TestRedactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String PATH;
        public TestControllerInXml testXml;

        public MainWindow()
        {
            InitializeComponent();
            InitVoid();
        }
        private void InitVoid()
        {
            testXml = new TestControllerInXml();
            testXml.dataTest = new DataTestInXml();
            testXml.dataTest.Questions = new List<ContextInXml>();
            testXml.typesOfKeysRequest = new List<string>();
            testXml.dataTest.MasterKeys.Add("Все категории");
            Render();
        }
        private void buttonAddContext_Click(object sender, RoutedEventArgs e)
        {
            SaveDOTBS();
            List<string> lst = new List<string>();
            for (int i = 0; i < testXml.typesOfKeysRequest.Count; i++) { lst.Add(""); }
            int numcon = testXml.dataTest.Questions.Count + 1;
            testXml.dataTest.Questions.Add(new ContextInXml { description = $"Новый контекст {numcon}.", Keys = lst, questions = new List<TestQuestionInXml>() });
            Render();
        }
        public void Render()
        {
            listOfSPQ.Items.Clear();
            Label lb;

            foreach (var question in testXml.dataTest.Questions)
            {
                lb = new Label { Content = question.description, };
                lb.MouseDoubleClick += Lb_MouseDoubleClick;
                listOfSPQ.Items.Add(lb);
            }
            tbCOQS.Text = testXml.originalCountOfQuestion;
            tbRCA.Text = testXml.responseOfScore;
            tbCA.Text = testXml.responseWithCorrectAnswer;
            tbUCA.Text = testXml.responseWithUncorrectAnswer;

        }
        private void SaveDOTBS()
        {
            testXml.originalCountOfQuestion = tbCOQS.Text;
            testXml.responseOfScore = tbRCA.Text;
            testXml.responseWithCorrectAnswer = tbCA.Text;
            testXml.responseWithUncorrectAnswer = tbUCA.Text;
        }
        private void Lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveDOTBS();
            if (sender is Label lb)
            {
                int indexOfContext = ((ListBox)(lb.Parent)).Items.IndexOf(lb);
                ContRedWindow crw = new ContRedWindow(testXml, indexOfContext);
                crw.ShowDialog();
                Render();
            }

        }

        private void buttonRedactKeys_Click(object sender, RoutedEventArgs e)
        {
            SaveDOTBS();
            KeysRedWindow krw = new KeysRedWindow(testXml);
            krw.ShowDialog();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }
        private string GetPathByOFD()
        {
            var filePath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\TelegramBotKorolev\Data";
            ofd.Filter = "test files (*.tes)|*.tes|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
            }
            return filePath;
        }

        public bool ConcatonateSecondToFifst(TestControllerInXml t1, TestControllerInXml t2)
        {
             if (t1.typesOfKeysRequest.Count ==t2.typesOfKeysRequest.Count)
            {
                for (int i = 0; i < t1.dataTest.Keys.Count; i++)
                {
                    foreach (string el in t2.dataTest.Keys[i])
                    {
                        if (t1.dataTest.Keys[i].Contains(el) == false) { t1.dataTest.Keys[i].Add(el); }
                    }
                }
                foreach (ContextInXml el in t2.dataTest.Questions)
                {
                    t1.dataTest.Questions.Add(el);
                }
                return true;
            }
            else { return false; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var filePath = GetPathByOFD();
            XmlSerializer serializer = new XmlSerializer(typeof(TestControllerInXml));
            try
            {
                StreamReader rf = new StreamReader(filePath);
                testXml = (TestControllerInXml)serializer.Deserialize(rf);
                rf.Close();
                this.PATH = filePath;
                miSave.IsEnabled = true;
            }
            catch { }
            Render();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            StreamWriter sw = new StreamWriter(PATH);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(TestControllerInXml));
            xmlSerializer.Serialize(sw, testXml);
            sw.Close();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Stream st;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\TelegramBotKorolev\Data";
            sfd.Filter = "test files (*.tes)|*.tes|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == true)
            {
                if ((st = sfd.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(st);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(TestControllerInXml));
                    xmlSerializer.Serialize(sw, testXml);
                    sw.Close();
                }
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            InitVoid();
            this.PATH = string.Empty;
            miSave.IsEnabled = false;
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            string filePath = GetPathByOFD();
            XmlSerializer serializer = new XmlSerializer(typeof(TestControllerInXml));
            try
            {
                StreamReader rf = new StreamReader(filePath);
                var impTestXml = (TestControllerInXml)serializer.Deserialize(rf);
                rf.Close();
                ConcatonateSecondToFifst(testXml, impTestXml);


            }
            catch { }
            Render();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveDOTBS();
            Render();
        }

    }
}
