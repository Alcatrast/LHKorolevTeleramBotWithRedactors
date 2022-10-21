using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TestRedactor
{
    /// <summary>
    /// Interaction logic for ContRedWindow.xaml
    /// </summary>
    public partial class ContRedWindow : Window
    {
        public TestControllerInXml test;
        private int index;
        public ContRedWindow(TestControllerInXml test, int index)
        {
            InitializeComponent();
            this.test = test;
            this.index = index;
            RenderListOfListsOfKeys();
            RenderListOfChosenKeys();
            RenderListOfQuestions();
            tbDescritpion.Text = this.test.dataTest.Questions[this.index].description;
        }

        private void RenderListOfListsOfKeys()
        {
            listOfListsOfKeys.Items.Clear();
            ListBox list;
            for (int i = 0; i < test.dataTest.Keys.Count; i++)
            {
                list = new ListBox();
                foreach (string el in test.dataTest.Keys[i])
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
            foreach (string el in test.dataTest.Questions[index].Keys)
            {
                listOfChosenKeys.Items.Add(new TextBox { IsReadOnly = false, Text = el });
            }
        }

        private void RenderListOfQuestions()
        {
            listOfQuestios.Items.Clear();
            foreach (var el in test.dataTest.Questions[index].questions)
            {
                Label nq = new Label { Content = el.Question };
                nq.MouseDoubleClick += ChQp_MouseDoubleClick;
                listOfQuestios.Items.Add(nq);
            }
        }

        private void ChQp_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label lb)
            {
                int indexOfQuestion=((ListBox)(lb.Parent)).Items.IndexOf(lb);
                QueRedWindow qrw = new QueRedWindow(test,this.index,indexOfQuestion);
                qrw.ShowDialog();
                RenderListOfQuestions();
            }
        }

        private void LbChoseKey_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label lb)
            {
                int k = ((ListBox)(lb.Parent)).Items.IndexOf(lb);
                int t = ((ListBox)((ListBox)(lb.Parent)).Parent).Items.IndexOf(((ListBox)(lb.Parent)));
                test.dataTest.Questions[index].Keys[t] = test.dataTest.Keys[t][k];
                RenderListOfChosenKeys();
            }
        }

        private void buttonAddQuestion_Click(object sender, RoutedEventArgs e)
        {
            int qn = test.dataTest.Questions[index].questions.Count + 1;
            test.dataTest.Questions[index].questions.Add(new TestQuestionInXml { Question = $"Новый вопрос {qn}?", uncorrectAnswers = new List<string>() });
            RenderListOfQuestions();
        }

        bool isdel = false;
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            if (isdel == false)
            {
                this.test.dataTest.Questions[this.index].description = tbDescritpion.Text;
            }

        }

        private void buttonForDelete_Click(object sender, RoutedEventArgs e)
        {
            test.dataTest.Questions.RemoveAt(this.index);
            isdel = true;
            this.Close();
        }

        private void tbDescritpion_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
