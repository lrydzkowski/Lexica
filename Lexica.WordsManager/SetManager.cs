using Lexica.WordsManager.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lexica.WordsManager
{
    class SetManager
    {
        public List<Set> Sets { get; private set; }

        private List<List<int>> RandomOrder = null;

        public void Randomize()
        {
            RandomOrder = new List<List<int>>();
            int k = 0;
            for (int i = 0; i < Sets.Count; i++)
            {
                RandomOrder[i] = new List<int>();
                for (int j = 0; j < Sets[i].Entries.Count; j++)
                {
                    RandomOrder[k] = new List<int>() { i, j };
                }
            }

        }
    }
}
