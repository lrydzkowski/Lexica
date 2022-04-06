using Lexica.Core.Extensions;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;

namespace Lexica.Words
{
    public class WordsSetOperator
    {
        private readonly Random randomGenerator = new();

        public WordsSetOperator(ISetService setService, List<ISource> filesSources)
        {
            SetService = setService;
            FilesSources = filesSources;
            LoadSet();
        }

        public WordsSetOperator(ISetService setService, ISource fileSource)
            : this(setService, new List<ISource>() { fileSource }) { }

        private ISetService SetService { get; set; }

        private List<ISource> FilesSources { get; set; }

        private OperationResult<Set?>? LastLoadSetOperationResult { get; set; }

        private Set? Set { get; set; }

        public bool LoadSet()
        {
            if (Set == null)
            {
                LastLoadSetOperationResult = SetService.Load(FilesSources);
                if (!LastLoadSetOperationResult.Result && LastLoadSetOperationResult.Errors?.Count > 0)
                {
                    throw new Exception(LastLoadSetOperationResult.Errors[0].Message);
                }
                Set = LastLoadSetOperationResult.Data;
            }
            return LastLoadSetOperationResult?.Result ?? true;
        }

        public bool Randomize()
        {
            if (Set == null)
            {
                return false;
            }
            Set.Entries.Shuffle();
            return true;
        }

        public Entry? GetEntry(string setNamespace, string setName, int lineNum)
        {
            if (Set != null)
            {
                for (int i = 0; i < Set.Entries.Count; i++)
                {
                    Entry entry = Set.Entries[i];
                    if (entry.SetPath.Namespace == setNamespace
                        && entry.SetPath.Name == setName
                        && entry.LineNum == lineNum)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Entry?> GetEntries(
            bool infiniteLoop = false,
            bool randomizeEachIteration = true,
            int sequenceMaxSize = -1)
        {
            if (Set == null)
            {
                yield return null;
            }
            else
            {
                if (randomizeEachIteration)
                {
                    Randomize();
                }
                for (int i = 0; i < Set.Entries.Count; i++)
                {
                    yield return Set.Entries[i];
                    if (IsTheLastElement(index: i, numberOfElements: Set.Entries.Count, sequenceMaxSize))
                    {
                        if (infiniteLoop)
                        {
                            i = -1;
                        }
                        else if (i == sequenceMaxSize)
                        {
                            break;
                        }
                        if (randomizeEachIteration)
                        {
                            Randomize();
                        }
                    }
                }
            }
        }

        private bool IsTheLastElement(int index, int numberOfElements, int sequenceMaxSize)
        {
            if (index == numberOfElements - 1 || index == sequenceMaxSize)
            {
                return true;
            }
            return false;
        }

        public int GetNumberOfEntries()
        {
            if (Set == null)
            {
                return 0;
            }
            return Set.Entries.Count;
        }

        public List<Entry> GetRandomEntries(int numOfEntries)
        {
            List<Entry> entries = new();
            if (Set == null)
            {
                return entries;
            }
            if (numOfEntries >= Set.Entries.Count)
            {
                return Set.Entries.GetRange(0, Set.Entries.Count);
            }
            List<int> drawnIndexes = new();
            for (int i = 0; i < numOfEntries; i++)
            {
                int drawnIndex;
                do
                {
                    drawnIndex = randomGenerator.Next(0, Set.Entries.Count - 1);
                }
                while (drawnIndexes.Contains(drawnIndex));
                drawnIndexes.Add(drawnIndex);
                entries.Add(Set.Entries[drawnIndex]);
            }
            return entries;
        }
    }
}