using System;
using System.Collections.Generic;

namespace LHKorolevTgB.Model.Test
{
    internal class DataTest
    {
        public List<string> MasterKeys { get; private set; }
        public List<List<string>> Keys { get; private set; }
        public List<Context> Questions { get; private set; }

        public DataTest(List<Context> questions, List<string> masterKeys, List<List<string>> keys)
        {
            Questions = questions;
            MasterKeys = masterKeys;
            Keys = keys;
        }
        public List<Context> GenerateTest(List<string> keys)
        {
            List<Context> filteredQuestions = new List<Context>();
            foreach (Context it in Questions)
            {
                filteredQuestions.Add(it);
            }
            List<int> indexs = new List<int>();


            for (int ki = 0; ki < keys.Count; ki++)
            {
                if (keys[ki] == MasterKeys[ki]) { continue; }

                for (int i = 0; i < filteredQuestions.Count; i++)
                {
                    if (filteredQuestions[i].Keys[ki] == keys[ki] == false) { indexs.Add(i); }
                }

                for (int i = indexs.Count - 1; i >= 0; i--)
                {
                    filteredQuestions.RemoveAt(indexs[i]);
                }
                indexs.Clear();
            }
            if (filteredQuestions.Count > 1)
            {
                filteredQuestions = GenerateRandomOrderOfQuestions(filteredQuestions);
            }
            return filteredQuestions;

        }
        private List<Context> GenerateRandomOrderOfQuestions(List<Context> filteredQuestions)
        {
            Random random = new Random();
            List<Context> buffer = new List<Context>();
            foreach (Context it in filteredQuestions)
            {
                buffer.Add(it);
            }

            List<Context> result = new List<Context>();
            for (int i = 0; i < filteredQuestions.Count; i++)
            {
                int index = random.Next(0, buffer.Count);
                result.Add(buffer[index]);
                buffer.RemoveAt(index);
            }
            return result;
        }
    }
}
