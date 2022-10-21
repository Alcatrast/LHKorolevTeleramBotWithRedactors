using System;
using System.Collections.Generic;
using LHKorolevTgB.Model.Test;


namespace LHKorolevTgB.Controller
{
    internal class TestController
    {
        public List<List<string>> NextResponses { get; private set; }
        public string Request { get; private set; }

        private DataTest dataTest;
        private List<string> typesOfKeysRequest;
        private List<string> currentKeys;
        private string originalCountOfQuestion;
        private string responseWithCorrectAnswer;
        private string responseWithUncorrectAnswer;
        private string responseOfScore;

        private int numberOfChosingKey;
        private int numberOfQuestion;
        private int countOfQuestion;

        private TestQuestion currentQuestion;
        private List<Context> currentTest;
        public bool TestIsStarted { get; private set; }
        private bool testIsChoosen;
        private int score;
        private bool currentAnswerIsCorrect;

        public TestController(DataTest dataTest, string originalCountOfQuestion, List<string> typesOfKeysRequest, string responseWithCorrectAnswer, string responseWithUncorrectAnswer, string responseOfScore)
        {
            this.dataTest = dataTest;
            this.typesOfKeysRequest = typesOfKeysRequest;
            this.originalCountOfQuestion = originalCountOfQuestion;
            this.responseWithCorrectAnswer = responseWithCorrectAnswer;
            this.responseWithUncorrectAnswer = responseWithUncorrectAnswer;
            this.responseOfScore = responseOfScore;
            ResetController();
        }

        public void ProcessingRespose(string response)
        {
            Request = "";
            NextResponses = new List<List<string>>();
            if (testIsChoosen == false)
            {
                if (TestIsStarted == false)
                {
                    TestIsStarted = true;
                    WriteRequestOfKeys();
                }
                else
                {

                    if (dataTest.Keys[numberOfChosingKey].Contains(response) || response == dataTest.MasterKeys[numberOfChosingKey])
                    {
                        currentKeys.Add(response);
                        ChangeStateInChoosing();
                        if (testIsChoosen == true)
                        {

                            currentTest = dataTest.GenerateTest(currentKeys);
                            if (countOfQuestion > currentTest.Count) { countOfQuestion = currentTest.Count; }
                            if (countOfQuestion == 0) {
                            ResetController();
                                Request = "Такой вариант появится позже...";
                                NextResponses= new List<List<string>>();    
                            
                            }
                            else
                            {
                                currentQuestion = currentTest[numberOfQuestion].GetTestQuestion();
                                Request = currentQuestion.Question;
                                WritePossibleNextAnswers();
                            }
                        }
                        else
                        {
                            WriteRequestOfKeys();
                        }
                    }
                    else
                    {
                        WriteRequestOfKeys();
                    }
                }
            }
            else
            {
                currentAnswerIsCorrect = currentQuestion.IsCorrectAnswer(response);
                if (currentAnswerIsCorrect == true)
                {
                    score++;
                    Request += responseWithCorrectAnswer + "\n\n";
                }
                else { Request += responseWithUncorrectAnswer + "\n\n" + currentTest[numberOfQuestion].GetFeedback(false) + "\n\n"; }
                numberOfQuestion++;
                if (numberOfQuestion >= currentTest.Count == false)
                {
                    currentQuestion = currentTest[numberOfQuestion].GetTestQuestion();
                    Request += currentQuestion.Question;
                    WritePossibleNextAnswers();
                }
                else
                {
                    Request += responseOfScore + ": " + score.ToString() + "/" + countOfQuestion.ToString();
                    NextResponses = null;
                    ResetController();
                }
            }
        }

        private List<List<string>> GenerateRequestOfKeys(int num)
        {
            List<List<string>> res = new List<List<string>>();
            res.Add(new List<string> { dataTest.MasterKeys[num] });
            foreach (string key in dataTest.Keys[num])
            {
                res.Add(new List<string> { key });
            }
            return res;
        }

        private void ChangeStateInChoosing()
        {
            numberOfChosingKey++;
            if (numberOfChosingKey >= dataTest.Keys.Count) { testIsChoosen = true; }
        }

        private void WriteRequestOfKeys()
        {
            Request = typesOfKeysRequest[numberOfChosingKey];
            NextResponses = GenerateRequestOfKeys(numberOfChosingKey);
        }

        private void WritePossibleNextAnswers()
        {
            List<List<string>> possibleAnswers = new List<List<string>>();
            List<string> lineAnswers = new List<string>();
            List<string> answers = currentQuestion.GetAnswers();
            for (int i = 0; i < answers.Count; i++)
            {
                lineAnswers.Add(answers[i]);
                if (i % 2 == 1 || i == answers.Count - 1)
                {
                    possibleAnswers.Add(lineAnswers);
                    lineAnswers = new List<string>();
                }
            }
            NextResponses = possibleAnswers;
        }

        public void ResetController()
        {
            numberOfChosingKey = 0;
            numberOfQuestion = 0;
            score = 0;
            countOfQuestion = int.Parse(originalCountOfQuestion);
            NextResponses = new List<List<string>>();
            currentKeys = new List<string>();
            TestIsStarted = false;
            currentAnswerIsCorrect = false;
            testIsChoosen = false;
            currentQuestion = null;
            currentTest = null;
        }
    }
}