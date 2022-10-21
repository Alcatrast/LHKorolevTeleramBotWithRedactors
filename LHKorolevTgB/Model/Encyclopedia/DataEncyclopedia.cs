using System;
using System.Collections.Generic;

namespace LHKorolevTgB.Model.Encyclopedia
{
    internal class DataEncyclopedia
    {
        public List<Statement> Statments { get; }
        public List<List<string>> Keys { get; }
        public List<string> MasterKeys { get; }

        public DataEncyclopedia(List<Statement> statments, List<string> masterKeys, List<List<string>> keys)
        {
            Statments = statments;
            Keys = keys;
            MasterKeys = masterKeys;
        }

        public List<Statement> GenerateEncyclopedia(List<string> keys)
        {
            List<Statement> filteredStatements = new List<Statement>();
            foreach (Statement it in Statments)
            {
                filteredStatements.Add(it);
            }
            List<int> indexs = new List<int>();


            for (int ki = 0; ki < keys.Count; ki++)
            {
                if (keys[ki] == MasterKeys[ki]) { continue; }

                for (int i = 0; i < filteredStatements.Count; i++)
                {
                    if (filteredStatements[i].Keys[ki] == keys[ki] == false) { indexs.Add(i); }
                }

                for (int i = indexs.Count - 1; i >= 0; i--)
                {
                    filteredStatements.RemoveAt(indexs[i]);
                }
                indexs.Clear();
            }

            if (filteredStatements.Count > 1)
            {
                filteredStatements = GenerateRandomOrderOfStatements(filteredStatements);
            }
            return filteredStatements;

        }
        private List<Statement> GenerateRandomOrderOfStatements(List<Statement> filteredStatements)
        {
            Random random = new Random();
            List<Statement> buffer = new List<Statement>();
            foreach (Statement it in filteredStatements)
            {
                buffer.Add(it);
            }

            List<Statement> result = new List<Statement>();
            for (int i = 0; i < filteredStatements.Count; i++)
            {
                int index = random.Next(0, buffer.Count);
                result.Add(buffer[index]);
                buffer.RemoveAt(index);
            }
            return result;
        }
    }
}
