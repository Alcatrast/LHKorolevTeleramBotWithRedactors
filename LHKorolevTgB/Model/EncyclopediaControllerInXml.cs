using System;
using System.Collections.Generic;

namespace LHKorolevTgB.Model
{
    public class EncyclopediaControllerInXml
    {
        public DataEncyclopediaInXml dataEncyclopedia;
        public List<string> typesOfKeysRequest;
        public string originalCountOfStatements;
        public string responseForContinue;
        public string medResDirPath;// with last /\
     

        public EncyclopediaControllerInXml() { typesOfKeysRequest = new List<string>(); }
    }
    public class DataEncyclopediaInXml
    {
        public List<StatementInXml> Statments;
        public List<List<string>> Keys;
        public List<string> MasterKeys;
        public DataEncyclopediaInXml() { Statments = new List<StatementInXml>(); MasterKeys = new List<string>(); Keys = new List<List<string>>(); }
    }
    public class StatementInXml
    {
        public string Description;
        public List<string> Keys;
        public String PathToMedia;//without frst/\
        public bool medExist;
        public StatementInXml() { }
    }
}
