using Lexica.Core.Extensions;
using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class SetModeOperator
    {
        public ISetService SetService { get; private set; }

        public List<long> SetIds { get; private set; }

        public Set? Set { get; private set; }

        public int Index { get; private set; }

        public SetModeOperator(ISetService setService, long setId) : this(setService, new List<long> { setId }) { }

        public SetModeOperator(ISetService setService, List<long> setIds)
        {
            SetService = setService;
            SetIds = setIds;
        }

        public async Task LoadSet()
        {
            if (Set == null)
            {
                Set = await SetService.Get(SetIds);
            }
        }

        public async Task Randomize()
        {
            await LoadSet();
            if (Set != null)
            {
                Set.Entries.Shuffle();
            }            
        }

        public void Reset()
        {
            Index = 0;
        }

        public async Task<Entry?> GetEntry(long setId, int entryId)
        {
            await LoadSet();
            if (Set != null)
            {
                for (int i = 0; i < Set.Entries.Count; i++)
                {
                    Entry entry = Set.Entries[i];
                    if (entry.SetId == setId && entry.EntryId == entryId)
                    {
                        return entry;
                    }
                }
            }

            return null;
        }

        public async Task<Entry?> GetNextEntry()
        {
            await LoadSet();
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
