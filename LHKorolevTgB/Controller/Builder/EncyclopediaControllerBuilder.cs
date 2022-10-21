using System.Collections.Generic;
using LHKorolevTgB.Model.Encyclopedia;
using LHKorolevTgB.Model;

namespace LHKorolevTgB.Controller.Builder
{
    class EncyclopediaControllerBuilder
    {
        public static EncyclopediaController BuildFromXml(EncyclopediaControllerInXml encyxml)
        {
            return new EncyclopediaController(BDE(encyxml.dataEncyclopedia), encyxml.typesOfKeysRequest, encyxml.originalCountOfStatements, encyxml.responseForContinue, encyxml.medResDirPath);
        }

        private static Statement BS(StatementInXml sixml)
        {
            return new Statement(sixml.Description, sixml.PathToMedia, sixml.Keys, sixml.medExist);
        }

        private static DataEncyclopedia BDE(DataEncyclopediaInXml dexml)
        {
            List<Statement> ls = new List<Statement>();
            foreach (StatementInXml el in dexml.Statments)
            {
                ls.Add(BS(el));
            }
            return new DataEncyclopedia(ls, dexml.MasterKeys, dexml.Keys);
        }
    }
}

