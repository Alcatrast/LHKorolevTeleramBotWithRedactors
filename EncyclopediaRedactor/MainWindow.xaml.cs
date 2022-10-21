using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml.Serialization;

namespace EncyclopediaRedactor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        String PATH;
        public EncyclopediaControllerInXml encyclopediaXml;

        public MainWindow()
        {
            InitializeComponent();
            InitVoid();
        }
        private void InitVoid()
        {
            encyclopediaXml = new EncyclopediaControllerInXml();
            encyclopediaXml.dataEncyclopedia = new DataEncyclopediaInXml();
            encyclopediaXml.dataEncyclopedia.Statments = new List<StatementInXml>();
            encyclopediaXml.typesOfKeysRequest = new List<string>();
            encyclopediaXml.dataEncyclopedia.MasterKeys.Add("Все категории");
            Render();
        }
        private void buttonAddStatement_Click(object sender, RoutedEventArgs e)
        {
            SaveDOTBS();
            List<string> lst = new List<string>();
            for (int i = 0; i < encyclopediaXml.typesOfKeysRequest.Count; i++) { lst.Add(""); }
            int numcon = encyclopediaXml.dataEncyclopedia.Statments.Count + 1;
            encyclopediaXml.dataEncyclopedia.Statments.Add(new StatementInXml { Description = $"Новый контекст {numcon}.", Keys = lst, medExist = false, PathToMedia = numcon.ToString() });
            Render();
        }
        public void Render()
        {
            listOfSPQ.Items.Clear();
            Label lb;

            foreach (var statement in encyclopediaXml.dataEncyclopedia.Statments)
            {
                lb = new Label { Content = statement.Description, };
                lb.MouseDoubleClick += Lb_MouseDoubleClick;
                listOfSPQ.Items.Add(lb);
            }
            tbCOSS.Text = encyclopediaXml.originalCountOfStatements;
            tbRFC.Text = encyclopediaXml.responseForContinue;
            tbMRDP.Text = encyclopediaXml.medResDirPath;
            //tbEOSEM.Text = encyclopediaXml.errorOfExistStatementMessage;

        }
        private void SaveDOTBS()
        {
            encyclopediaXml.originalCountOfStatements = tbCOSS.Text;
            encyclopediaXml.responseForContinue = tbRFC.Text;
            encyclopediaXml.medResDirPath = tbMRDP.Text;
           // encyclopediaXml.errorOfExistStatementMessage=tbEOSEM.Text;
        }
        private void Lb_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveDOTBS();
            if (sender is Label lb)
            {
                int indexOfStatement = ((ListBox)(lb.Parent)).Items.IndexOf(lb);
                StatRedWindow srw = new StatRedWindow(encyclopediaXml, indexOfStatement);
                srw.ShowDialog();
                Render();
            }

        }

        private void buttonRedactKeys_Click(object sender, RoutedEventArgs e)
        {
            SaveDOTBS();
            KeysRedWindow krw = new KeysRedWindow(encyclopediaXml);
            krw.ShowDialog();

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {


        }
        private string GetPathByOFD()
        {
            var filePath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"C:\TelegramBotKorolev\Data\main_enc";
            ofd.Filter = "encyclopedia files (*.enc)|*.enc|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
            }
            return filePath;
        }

        public bool ConcatonateSecondToFifst(EncyclopediaControllerInXml e1, EncyclopediaControllerInXml e2)
        {
            if (e1.typesOfKeysRequest.Count == e2.typesOfKeysRequest.Count)
            {
                for (int i = 0; i < e1.dataEncyclopedia.Keys.Count; i++)
                {
                    foreach (string el in e2.dataEncyclopedia.Keys[i])
                    {
                        if (e1.dataEncyclopedia.Keys[i].Contains(el) == false) { e1.dataEncyclopedia.Keys[i].Add(el); }
                    }
                }
                foreach (StatementInXml el in e2.dataEncyclopedia.Statments)
                {
                    e1.dataEncyclopedia.Statments.Add(el);
                }
                return true;
            }
            else { return false; }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var filePath = GetPathByOFD();
            XmlSerializer serializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
            try
            {
                StreamReader rf = new StreamReader(filePath);
                encyclopediaXml = (EncyclopediaControllerInXml)serializer.Deserialize(rf);
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
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
            xmlSerializer.Serialize(sw, encyclopediaXml);
            sw.Close();
        }

        private void SaveAs_Click(object sender, RoutedEventArgs e)
        {
            Stream st;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\TelegramBotKorolev\Data\main_enc";
            sfd.Filter = "encyclopedia files (*.enc)|*.enc|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == true)
            {
                if ((st = sfd.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(st);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
                    xmlSerializer.Serialize(sw, encyclopediaXml);
                    sw.Close();
                }
            }
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {

            InitVoid();
            Stream st;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = @"C:\TelegramBotKorolev\Data\main_enc";
            sfd.Filter = "encyclopedia files (*.enc)|*.enc|All files (*.*)|*.*";
            sfd.FilterIndex = 1;
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == true)
            {
                if ((st = sfd.OpenFile()) != null)
                {
                    StreamWriter sw = new StreamWriter(st);
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
                    xmlSerializer.Serialize(sw, encyclopediaXml);
                    this.PATH = string.Empty;//////////// хз  
                    sw.Close();
                }
            }
            
            miSave.IsEnabled = true;
        }
        private void Import_Click(object sender, RoutedEventArgs e)
        {
            string filePath = GetPathByOFD();
            XmlSerializer serializer = new XmlSerializer(typeof(EncyclopediaControllerInXml));
            try
            {
                StreamReader rf = new StreamReader(filePath);
                var impTestXml = (EncyclopediaControllerInXml)serializer.Deserialize(rf);
                rf.Close();
                ConcatonateSecondToFifst(encyclopediaXml, impTestXml);


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
