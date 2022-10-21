using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EncyclopediaRedactor
{
    /// <summary>
    /// Логика взаимодействия для KeysRedWindow.xaml
    /// </summary>
    public partial class KeysRedWindow : Window
    {
         public EncyclopediaControllerInXml test;

        int indexIntoTypes = 0;

        public KeysRedWindow(EncyclopediaControllerInXml test)
        {
            InitializeComponent();
            this.test = test;
            RenderLOT();
        }

        private void buttonAddType_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesOfTBs();
            int numt = test.typesOfKeysRequest.Count + 1;
            test.typesOfKeysRequest.Add($"Новый тип категорий {numt}...");
            test.dataEncyclopedia.MasterKeys.Add("Все категории");
            test.dataEncyclopedia.Keys.Add(new List<string>());
            RenderLOT();
        }

        private void buttonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesOfTBs();

            int numk = test.dataEncyclopedia.Keys[this.indexIntoTypes].Count + 1;
            this.test.dataEncyclopedia.Keys[this.indexIntoTypes].Add($"Новая категория {numk}");
            RenderLOC();

        }

        public void RenderLOC()
        {

            listOfCategories.Items.Clear();
            foreach (string el in this.test.dataEncyclopedia.Keys[this.indexIntoTypes])
            {
                var tb = new TextBox { Text = el };

                listOfCategories.Items.Add(tb);
            }
            textboxMasterCategory.Text = this.test.dataEncyclopedia.MasterKeys[this.indexIntoTypes];
        }

        public void RenderLOT()
        {
            if (test.typesOfKeysRequest.Count == 0) { buttonAddCategory.IsEnabled = false; } else { buttonAddCategory.IsEnabled = true; }
            listOfTypes.Items.Clear();
            foreach (string el in test.typesOfKeysRequest)
            {
                var tb = new TextBox { Text = el };
                tb.MouseDoubleClick += TbIT_MouseDoubleClick;
                listOfTypes.Items.Add(tb);
            }
        }

        private void TbIT_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SaveChangesOfTBs();
            if (sender is TextBox tb)
            {
                this.indexIntoTypes = ((ListBox)(tb.Parent)).Items.IndexOf(tb);
            }
            RenderLOT();
            RenderLOC();
        }

        public void SaveChangesOfTBs()
        {
            try
            {
                for (int i = 0; i < test.typesOfKeysRequest.Count; i++)
                {
                    test.typesOfKeysRequest[i] = ((TextBox)(listOfTypes.Items[i])).Text;
                }
                for (int i = 0; i < test.dataEncyclopedia.Keys[indexIntoTypes].Count; i++)
                {
                    test.dataEncyclopedia.Keys[indexIntoTypes][i] = ((TextBox)(listOfCategories.Items[i])).Text;
                }
                test.dataEncyclopedia.MasterKeys[indexIntoTypes] = textboxMasterCategory.Text;


                for (int i = 0; i < test.dataEncyclopedia.Keys[indexIntoTypes].Count; i++)
                {
                    if (test.dataEncyclopedia.Keys[indexIntoTypes][i] == "") { test.dataEncyclopedia.Keys[indexIntoTypes].RemoveAt(i); }
                }


                for (int i = 0; i < test.typesOfKeysRequest.Count; i++)
                {
                    if (test.typesOfKeysRequest[i] == "")
                    {
                        test.typesOfKeysRequest.RemoveAt(i);
                        test.dataEncyclopedia.MasterKeys.RemoveAt(i);
                        test.dataEncyclopedia.Keys.RemoveAt(i);
                    }
                }
            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveChangesOfTBs();
        }
    }
}
