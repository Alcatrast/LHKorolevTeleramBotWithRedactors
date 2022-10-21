using LHKorolevTgB.Model.Encyclopedia;
using System;
using System.Collections.Generic;


namespace LHKorolevTgB.Controller
{
    internal class EncyclopediaController
    {
        public List<List<string>> NextResponses { get; private set; }
        public string Request { get; private set; }
        public string MediaRequest { get; private set; }

        private DataEncyclopedia dataEncyclopedia;
        private List<string> typesOfKeysRequest;
        private string originalCountOfStatements;
        private string responseForContinue;
        public string MedResDirPath { get; }


        private List<string> currentKeys;
        private int numberOfChosingKey;
        private int numberOfStatement;
        private int countOfStatement;
        private List<Statement> currenEncyclopedy;
        public bool EncyclopediaActivated { get; set; }
        private bool encyclopedyIsChoosen;

        public EncyclopediaController(DataEncyclopedia dataEncyclopedia, List<string> typesOfKeysRequest, string originalCountOfStatements, string responseForContinue, string medResDirPath)
        {
            this.dataEncyclopedia = dataEncyclopedia;
            this.typesOfKeysRequest = typesOfKeysRequest;
            this.originalCountOfStatements = originalCountOfStatements;
            this.responseForContinue = responseForContinue;
            MedResDirPath = medResDirPath;
    
            ResetController();
            
        }

        private List<List<string>> GenerateRsponseOfKeys(int num)
        {
            List<List<string>> res = new List<List<string>>();
            res.Add(new List<string> { dataEncyclopedia.MasterKeys[num] });
            foreach (string key in dataEncyclopedia.Keys[num])
            {
                res.Add(new List<string> { key });
            }
            return res;
        }

        private void ChangeStateInChoosing()
        {
            numberOfChosingKey++;
            if (numberOfChosingKey >= dataEncyclopedia.Keys.Count) { encyclopedyIsChoosen = true; }
        }

        private void WriteRequestOfKeys()
        {
            Request = typesOfKeysRequest[numberOfChosingKey];
            NextResponses = GenerateRsponseOfKeys(numberOfChosingKey);
            MediaRequest=string.Empty;
        }

        public void ResetController()
        {
            numberOfChosingKey = 0;
            numberOfStatement = 0;
            countOfStatement = int.Parse(originalCountOfStatements);
            NextResponses = new List<List<string>>() ;
            currentKeys = new List<string>();
            EncyclopediaActivated = false;
            encyclopedyIsChoosen = false;
            currenEncyclopedy = null;
        }

        public void ProcessingRespose(string response)
        {
            Request = "";
            NextResponses = new List<List<string>>();
            if (encyclopedyIsChoosen == false)
            {
                if (EncyclopediaActivated == false)
                {
                    EncyclopediaActivated = true;
                    WriteRequestOfKeys();
                }
                else
                {
                    if (dataEncyclopedia.Keys[numberOfChosingKey].Contains(response) || response == dataEncyclopedia.MasterKeys[numberOfChosingKey])
                    {
                        currentKeys.Add(response);
                        ChangeStateInChoosing();
                        if (encyclopedyIsChoosen == true)
                        {
                            currenEncyclopedy = dataEncyclopedia.GenerateEncyclopedia(currentKeys);
                            if (countOfStatement > currenEncyclopedy.Count) { countOfStatement = currenEncyclopedy.Count; }
                            if (countOfStatement == 0)
                            {
                                ResetController();
                                Request = "Такой вариант появится появится позже...";
                              
                                MediaRequest = string.Empty;
                                NextResponses = new List<List<string>>();

                            }
                            else
                            {
                                GetStatement();
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
                if (numberOfStatement < currenEncyclopedy.Count)
                {
                    GetStatement();
                }
            }
        }

        private void GetStatement()
        {
            Request = currenEncyclopedy[numberOfStatement].Description;

            if (currenEncyclopedy[numberOfStatement].MedExist)
            {

                MediaRequest = MedResDirPath + currenEncyclopedy[numberOfStatement].PathToMedia;
            }
            else { MediaRequest = string.Empty; }

            WritePossibleNextAnswers();
            numberOfStatement++;
            if (numberOfStatement >= currenEncyclopedy.Count) { this.ResetController(); }
        }

        private void WritePossibleNextAnswers()
        {
            NextResponses = new List<List<string>> { new List<string> { responseForContinue } };
        }
    }
}

