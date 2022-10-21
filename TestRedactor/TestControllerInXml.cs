
using System.Collections.Generic;


namespace TestRedactor
{
    public class TestControllerInXml
    {
        public DataTestInXml dataTest;
        public List<string> typesOfKeysRequest;
        public string originalCountOfQuestion;
        public string responseWithCorrectAnswer;
        public string responseWithUncorrectAnswer;
        public string responseOfScore;
        public TestControllerInXml() { typesOfKeysRequest = new List<string>(); }
    }
    public class DataTestInXml
    {
        public List<string> MasterKeys;
        public List<List<string>> Keys;
        public List<ContextInXml> Questions;
        public DataTestInXml() { Questions = new List<ContextInXml>(); MasterKeys = new List<string>(); Keys = new List<List<string>>(); }
    }
    public class ContextInXml
    {
        public List<string> Keys;
        public List<TestQuestionInXml> questions;
        public string description;
        public ContextInXml() { questions = new List<TestQuestionInXml>(); }
    }
    public class TestQuestionInXml
    {
        public string Question;
        public List<string> uncorrectAnswers;
        public string correctAnswer;
        public TestQuestionInXml() { uncorrectAnswers = new List<string>(); }
    }
}
