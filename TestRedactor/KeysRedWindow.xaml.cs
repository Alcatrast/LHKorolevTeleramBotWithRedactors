using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestRedactor
{
    /// <summary>
    /// Interaction logic for KeysRedWindow.xaml
    /// </summary>
    public partial class KeysRedWindow : Window
    {
        public TestControllerInXml test;

        int indexIntoTypes = 0;

        public KeysRedWindow(TestControllerInXml test)
        {
            InitializeComponent();
            this.test = test;
            RenderLOT();
            // RenderLOC();


        }

        private void buttonAddType_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesOfTBs();
            int numt = test.typesOfKeysRequest.Count + 1;
            test.typesOfKeysRequest.Add($"Новый тип категорий {numt}...");
            test.dataTest.MasterKeys.Add("Все категории");
            test.dataTest.Keys.Add(new List<string>());
            RenderLOT();
        }

        private void buttonAddCategory_Click(object sender, RoutedEventArgs e)
        {
            SaveChangesOfTBs();

            int numk = test.dataTest.Keys[this.indexIntoTypes].Count + 1;
            this.test.dataTest.Keys[this.indexIntoTypes].Add($"Новая категория {numk}");
            RenderLOC();

        }

        public void RenderLOC()
        {

            listOfCategories.Items.Clear();
            foreach (string el in this.test.dataTest.Keys[this.indexIntoTypes])
            {
                var tb = new TextBox { Text = el };

                listOfCategories.Items.Add(tb);
            }
            textboxMasterCategory.Text = this.test.dataTest.MasterKeys[this.indexIntoTypes];
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
                for (int i = 0; i < test.dataTest.Keys[indexIntoTypes].Count; i++)
                {
                    test.dataTest.Keys[indexIntoTypes][i] = ((TextBox)(listOfCategories.Items[i])).Text;
                }
                test.dataTest.MasterKeys[indexIntoTypes] = textboxMasterCategory.Text;


                for (int i = 0; i < test.dataTest.Keys[indexIntoTypes].Count; i++)
                {
                    if (test.dataTest.Keys[indexIntoTypes][i] == "") { test.dataTest.Keys[indexIntoTypes].RemoveAt(i); }
                }


                for (int i = 0; i < test.typesOfKeysRequest.Count; i++)
                {
                    if (test.typesOfKeysRequest[i] == "")
                    {
                        test.typesOfKeysRequest.RemoveAt(i);
                        test.dataTest.MasterKeys.RemoveAt(i);
                        test.dataTest.Keys.RemoveAt(i);
                    }
                }



                ////костыль
                //for (int i = test.dataTest.Keys.Count - 1; i >= 0; i--)
                //{
                //    if (test.dataTest.Keys[i].Count == 0) { test.dataTest.Keys.RemoveAt(i); }
                //}
                ////конец костыля

            }
            catch { }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveChangesOfTBs();
        }
    }
}
