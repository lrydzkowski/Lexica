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
        public ISetService SetService { get; private set; }

        public List<ISource> FilesSources { get; set; }

        public OperationResult<Set?>? LastLoadSetOperationResult { get; private set; }

        public Set? Set { get; private set; }

        public int Index { get; private set; }

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

        public void Reset()
        {
            Index = 0;
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

        public Entry? GetNextEntry()
        {
            LoadSet();
            if (Set == null)
            {
                return null;
            }
            if (Index > Set.Entries.Count - 1)
            {
                return null;
            }
            
            return Set.Entries[Index++];
        }
    }
}
