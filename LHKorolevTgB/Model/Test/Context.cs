using System;
using System.Collections.Generic;

namespace LHKorolevTgB.Model.Test
{
    internal class Context
    {
        public List<string> Keys { get; private set; }
        private List<TestQuestion> questions;
        private string description;
        public Context(List<string> keys, List<TestQuestion> questions, string description)
        {
            Keys = keys;
            this.questions = questions;
            this.description = description;
        }
        public TestQuestion GetTestQuestion()
        {
            Random random = new Random();
            return questions[random.Next(0, questions.Count)];
        }
        public string GetFeedback(bool isCorrectAnswer)
        {
            if (isCorrectAnswer) { return null; } else { return description; }
        }


    }
}
