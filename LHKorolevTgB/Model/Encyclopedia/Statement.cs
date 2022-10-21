
using System.Collections.Generic;
using System;


namespace LHKorolevTgB.Model.Encyclopedia
{
    internal class Statement
    {
        public string Description { get; }
        public List<string> Keys { get; }
        public String PathToMedia { get; }
        public bool MedExist { get; }
        public Statement(string description, string pathToMedia, List<string> keys, bool medExist)
        {
            MedExist = medExist;
            Description = description;
            Keys = keys;
            PathToMedia = pathToMedia;
        }
    }
}
