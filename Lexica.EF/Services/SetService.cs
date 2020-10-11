using Lexica.Core.Models;
using Lexica.EF.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Services
{
    public class SetService : ISetService
    {
        public async Task CreateSet(Set set)
        {
            await CreateSet(new List<Set>() { set });
        }

        public async Task CreateSet(List<Set> sets)
        {
            using (var context = new LexicaContext())
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    foreach (var set in sets)
                    {
                        var setRecord = new SetTable();
                        setRecord.Namespace = set.SetsInfo[0].Path.Namespace;
                        setRecord.Name = set.SetsInfo[0].Path.Name;
                        await context.AddAsync(setRecord);
                        await context.SaveChangesAsync();

                        int entryId = 1;
                        foreach (var entry in set.Entries)
                        {
                            foreach (var word in entry.Words)
                            {
                                foreach (var translation in entry.Translations)
                                {
                                    var entryRecord = new EntryTable();
                                    entryRecord.EntryId = entryId;
                                    entryRecord.SetId = setRecord.Id;
                                    entryRecord.Word = word;
                                    entryRecord.Translation = translation;
                                    await context.AddAsync(entryRecord);
                                }
                            }
                            entryId++;
                        }
                        await context.SaveChangesAsync();
                    }

                    await transaction.CommitAsync();
                }
            }
        }

        public async Task<OperationResult> ChangePath(long setId, SetPath newPath)
        {
            using (var context = new LexicaContext())
            {
                SetTable setRecord = context.Sets.Where(x => x.Id == setId).FirstOrDefault();
                if (setRecord == null)
                {
                    var error = new Error(
                        (int)ErrorEnum.SetDoesntExist, 
                        $"Set with id {setId} doesn't exist."
                    );
                    return new OperationResult(false, error);
                }
                setRecord.Namespace = newPath.Namespace;
                setRecord.Name = newPath.Name;
                await context.SaveChangesAsync();
            }
            return new OperationResult(true);
        }

        public async Task<Set> Get(long setId)
        {
            return await Get(new List<long>() { setId });
        }

        public async Task<Set> Get(List<long> setIds)
        {
            using (var context = new LexicaContext())
            {
                Set set = null;
                foreach (long setId in setIds)
                {
                    List<SetTable> setRecord = await context.Sets
                        .Where(x => x.Id == setId)
                        .Include(x => x.Entries)
                        .AsNoTracking()
                        .ToListAsync();
                    set = setRecord.Select(x => new Set() {
                        SetsInfo = new List<SetInfo>() {
                            new SetInfo()
                            {
                                SetId = x.Id,
                                Path = new SetPath()
                                {
                                    Namespace = x.Namespace,
                                    Name = x.Name
                                }
                            }
                        },
                        Entries = x.Entries.GroupBy(x => new { x.SetId, x.EntryId }).Select(x => new Entry()
                        {
                            SetId = x.Key.SetId,
                            EntryId = x.Key.EntryId,
                            Words = x.Select(x => x.Word).ToList(),
                            Translations = x.Select(x => x.Translation).ToList()
                        }).ToList<Entry>()
                    })
                    .FirstOrDefault();
                }
                return set;
            }
        }

        public async Task<List<SetInfo>> GetInfoList()
        {
            using (var context = new LexicaContext())
            {
                List<SetInfo> list = await context.Sets
                    .AsNoTracking()
                    .Select(x => new SetInfo()
                    {
                        SetId = x.Id,
                        Path = new SetPath()
                        {
                            Namespace = x.Namespace,
                            Name = x.Name
                        }
                    })
                    .ToListAsync();
                return list;
            }
        }

        public async Task Remove(long setId)
        {
            using (var context = new LexicaContext())
            {
                SetTable setRecord = await context.Sets
                    .Where(x => x.Id == setId)
                    .Select(x => new SetTable { Id = x.Id })
                    .FirstOrDefaultAsync();
                if (setRecord != null)
                {
                    context.Remove(setRecord);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}
