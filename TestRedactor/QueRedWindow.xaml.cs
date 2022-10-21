using System.Windows;
using System.Windows.Controls;


namespace TestRedactor
{
    /// <summary>
    /// Interaction logic for QueRedWindow.xaml
    /// </summary>
    public partial class QueRedWindow : Window
    {
        TestControllerInXml test; int indexOfContext; int indexOfQuestion;
        public QueRedWindow(TestControllerInXml test, int indexOfContext, int indexOfQuestion)
        {
            InitializeComponent();
            this.test = test;
            this.indexOfContext = indexOfContext;
            this.indexOfQuestion = indexOfQuestion;
            Render();
        }

        private void Render()
        {
            listOfUncorrectAnswers.Items.Clear();
            foreach (string el in test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers)
            {
                TextBox tb = new TextBox { Text = el };
                listOfUncorrectAnswers.Items.Add(tb);
            }
            tbCorrectAnswer.Text = test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].correctAnswer;
            tbQuestion.Text = test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].Question;

            

        }

        private void btAddUnAn_Click(object sender, RoutedEventArgs e)
        {
            SaveTBS();
            int nq = test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers.Count + 1;
            test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers.Add($"Новый ответ {nq}.");
            Render();
        }
        private void SaveTBS()
        {
            for (int i = 0; i < test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers.Count; i++)
            {
                test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers[i] = ((TextBox)listOfUncorrectAnswers.Items[i]).Text;
            }
            for (int i = test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers.Count - 1; i >= 0; i--)
            {
                if (test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers[i] == "") { test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].uncorrectAnswers.RemoveAt(i); }
            }
            test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].correctAnswer = tbCorrectAnswer.Text;
            test.dataTest.Questions[indexOfContext].questions[indexOfQuestion].Question = tbQuestion.Text;

        }
        bool isDeleted = false;
        private void btDel_Click(object sender, RoutedEventArgs e)
        {
            test.dataTest.Questions[indexOfContext].questions.RemoveAt(indexOfQuestion);
            isDeleted = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (isDeleted == false)
            {
                SaveTBS();
            }
        }
    }
}
