using System;
using System.Collections.Generic;

namespace LHKorolevTgB.Model.Test
{
    internal class TestQuestion
    {
        public string Question { get; private set; }
        private List<string> uncorrectAnswers;
        private string correctAnswer;

        public TestQuestion(string question, List<string> uncorrectAnswers, string correctAnswer)
        {
            Question = question;
            this.uncorrectAnswers = uncorrectAnswers;
            this.correctAnswer = correctAnswer;
        }
        private List<string> GenerateRandomOrderOfAnswers()
        {
            Random random = new Random();
            List<string> buffer = new List<string>();
            foreach (string it in uncorrectAnswers)
            {
                buffer.Add(it);
            }
            buffer.Add(correctAnswer);
            List<string> result = new List<string>();

            for (int i = 0; i < uncorrectAnswers.Count + 1; i++)
            {
                int index = random.Next(0, buffer.Count);
                result.Add(buffer[index]);
                buffer.RemoveAt(index);
            }
            return result;
        }

        public List<string> GetAnswers() { return GenerateRandomOrderOfAnswers(); }
        public bool IsCorrectAnswer(string answer) { return answer == correctAnswer; }

    }
}
