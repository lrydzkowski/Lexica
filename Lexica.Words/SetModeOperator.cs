using Lexica.Core.Extensions;
using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System.Collections.Generic;

namespace Lexica.Words
{
    public class SetModeOperator
    {
        public SetModeOperator(ISetService setService, List<ISource> filesSources)
        {
            SetService = setService;
            FilesSources = filesSources;
        }

        public SetModeOperator(ISetService setService, ISource fileSource)
        {
            SetService = setService;
            FilesSources = new List<ISource>() { fileSource };
        }

        private ISetService SetService { get; set; }

        private List<ISource> FilesSources { get; set; }

        private OperationResult<Set?>? LastLoadSetOperationResult { get; set; }

        private Set? Set { get; set; }

        public bool LoadSet()
        {
            if (Set == null)
            {
                LastLoadSetOperationResult = SetService.Load(FilesSources);
                Set = LastLoadSetOperationResult.Data;
            }
            return LastLoadSetOperationResult?.Result ?? true;
        }

        public bool Randomize()
        {
            bool result = LoadSet();
            if (Set != null)
            {
                Set.Entries.Shuffle();
            }
            return result;
        }

        public Entry? GetEntry(string setNamespace, string setName, int lineNum)
        {
            LoadSet();
            if (Set != null)
            {
                for (int i = 0; i < Set.Entries.Count; i++)
                {
                    Entry entry = Set.Entries[i];
                    if (    entry.SetPath.Namespace == setNamespace
                        &&  entry.SetPath.Name == setName
                        &&  entry.LineNum == lineNum)
                    {
                        return entry;
                    }
                }
            }
            return null;
        }

        public IEnumerable<Entry?> GetEntries(bool infiniteLoop = false, bool randomizeEachIteration = true)
        {
            LoadSet();
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
                    if (i == Set.Entries.Count - 1)
                    {
                        if (infiniteLoop)
                        {
                            i = 0;
                        }
                        if (randomizeEachIteration)
                        {
                            Randomize();
                        }
                    }
                }
            }
        }

        public int GetNumberOfEntries()
        {
            LoadSet();
            if (Set == null)
            {
                return 0;
            }
            return Set.Entries.Count;
        }
    }
}
