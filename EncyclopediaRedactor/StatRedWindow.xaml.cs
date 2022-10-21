using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Text;

namespace EncyclopediaRedactor
{
    /// <summary>
    /// Логика взаимодействия для StatRedWindow.xaml
    /// </summary>
    public partial class StatRedWindow : Window
    {
        public EncyclopediaControllerInXml encyclopedia;
        private int index;
        public StatRedWindow(EncyclopediaControllerInXml encyclopedia, int index)
        {
            InitializeComponent();
            this.encyclopedia = encyclopedia;
            this.index = index;
            RenderListOfListsOfKeys();
            RenderListOfChosenKeys();
            RenderImage();
            tbDescritpion.Text = this.encyclopedia.dataEncyclopedia.Statments[this.index].Description;
        }
        private void RenderImage()
        {
            if (encyclopedia.dataEncyclopedia.Statments[index].medExist==true)
            {
                //StreamReader sr = new StreamReader(encyclopedia.medResDirPath + encyclopedia.dataEncyclopedia.Statments[index].PathToMedia);
                //var ba = Encoding.ASCII.GetBytes(sr.ReadToEnd());
                //sr.Close();
                //imgOut.Source = LoadImage(ba);
                //////////////////////////
                buttonDelMedia.IsEnabled = true;
            }
            else
            {
               // imgOut.Source = new BitmapImage();
                //////////////////////////
                buttonDelMedia.IsEnabled = false;
            }
        }
        private void RenderListOfListsOfKeys()
        {
            listOfListsOfKeys.Items.Clear();
            ListBox list;
            for (int i = 0; i < encyclopedia.dataEncyclopedia.Keys.Count; i++)
            {
                list = new ListBox();
                foreach (string el in encyclopedia.dataEncyclopedia.Keys[i])
                {
                    var lb = new Label { Content = el };
                    lb.MouseDoubleClick += LbChoseKey_MouseDoubleClick;
                    list.Items.Add(lb);
                }
                listOfListsOfKeys.Items.Add(list);
            }
        }

        private void RenderListOfChosenKeys()
        {
            listOfChosenKeys.Items.Clear();
            foreach (string el in encyclopedia.dataEncyclopedia.Statments[index].Keys)
            {
                listOfChosenKeys.Items.Add(new TextBox { IsReadOnly = false, Text = el });
            }
        }

        private void LbChoseKey_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label lb)
            {
                int k = ((ListBox)(lb.Parent)).Items.IndexOf(lb);
                int t = ((ListBox)((ListBox)(lb.Parent)).Parent).Items.IndexOf(((ListBox)(lb.Parent)));
                encyclopedia.dataEncyclopedia.Statments[index].Keys[t] = encyclopedia.dataEncyclopedia.Keys[t][k];
                RenderListOfChosenKeys();
            }
        }

        bool isdel = false;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (isdel == false)
            {
                this.encyclopedia.dataEncyclopedia.Statments[this.index].Description = tbDescritpion.Text;
            }

        }

        private void buttonForDelete_Click(object sender, RoutedEventArgs e)
        {
            encyclopedia.dataEncyclopedia.Statments.RemoveAt(this.index);
            isdel = true;
            this.Close();
        }

        private void buttonChooseMedia_Click(object sender, RoutedEventArgs e)
        {
            var filePath = string.Empty;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = "c:\\";
            ofd.Filter = "All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.RestoreDirectory = true;

            if (ofd.ShowDialog() == true)
            {
                filePath = ofd.FileName;
            }
            if (filePath != string.Empty)
            {
                File.Copy(filePath, encyclopedia.medResDirPath + encyclopedia.dataEncyclopedia.Statments[index].PathToMedia, true);
              
                encyclopedia.dataEncyclopedia.Statments[index].medExist = true;
                RenderImage();
               
            }
        }

        private void buttonDelMedia_Click(object sender, RoutedEventArgs e)
        {
            if (encyclopedia.dataEncyclopedia.Statments[index].medExist == true)
            {
                try
                {
                    File.Delete(encyclopedia.medResDirPath + encyclopedia.dataEncyclopedia.Statments[index].PathToMedia);
                }
                catch { }
                encyclopedia.dataEncyclopedia.Statments[index].medExist = false;
                RenderImage();
            }
        }

        private static BitmapImage LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
