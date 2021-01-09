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
            using (var transaction = context.Database.BeginTransaction())
            {
                foreach (var set in sets)
                {
                    var setRecord = new SetTable
                    {
                        Namespace = set.SetsInfo[0].Path.Namespace,
                        Name = set.SetsInfo[0].Path.Name
                    };
                    await context.AddAsync(setRecord);
                    await context.SaveChangesAsync();

                    int entryId = 1;
                    foreach (var entry in set.Entries)
                    {
                        foreach (var word in entry.Words)
                        {
                            foreach (var translation in entry.Translations)
                            {
                                var entryRecord = new EntryTable
                                {
                                    EntryId = entryId,
                                    SetId = setRecord.SetId,
                                    Word = word,
                                    Translation = translation
                                };
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

        public async Task<OperationResult> ChangePath(long setId, SetPath newPath)
        {
            using (var context = new LexicaContext())
            {
                SetTable setRecord = context.Sets.Where(x => x.SetId == setId).FirstOrDefault();
                if (setRecord == null)
                {
                    var error = new Error(
                        (int)EF.Models.ErrorCodesEnum.SetDoesntExist, 
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

        public async Task<Set?> Get(long setId)
        {
            return await Get(new List<long>() { setId });
        }

        public async Task<Set?> Get(List<long> setIds)
        {
            using (var context = new LexicaContext())
            {
                Set? set = null;
                List<SetInfo> setsInfo = new List<SetInfo>();
                List<Entry> entries = new List<Entry>();
                foreach (long setId in setIds)
                {
                    List<SetTable> setRecord = await context.Sets
                        .Where(x => x.SetId == setId)
                        .Include(x => x.Entries)
                        .AsNoTracking()
                        .ToListAsync();
                    SetInfo setInfo = setRecord.Select(
                        x => new SetInfo(
                            setId: x.SetId,
                            path: new SetPath(
                                setNamespace: x.Namespace,
                                name: x.Name
                            )
                        )
                    ).FirstOrDefault();
                    if (setInfo != null)
                    {
                        setsInfo.Add(setInfo);
                        entries.AddRange(
                            setRecord.SelectMany(
                                x => x.Entries.GroupBy(x => new { x.SetId, x.EntryId }).Select(
                                    x => new Entry(
                                        setId: x.Key.SetId,
                                        entryId: x.Key.EntryId,
                                        words: x.Select(x => x.Word).ToList(),
                                        translations: x.Select(x => x.Translation).ToList()
                                    )
                                ).ToList<Entry>()
                            ).ToList()
                        );
                    }
                }
                if (setsInfo.Count > 0)
                {
                    set = new Set(setsInfo, entries);
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
                    .Select(x => new SetInfo(
                        x.SetId,
                        new SetPath(
                            x.Namespace,
                            x.Name
                        )
                    ))
                    .ToListAsync();
                return list;
            }
        }

        public async Task Remove(long setId)
        {
            using (var context = new LexicaContext())
            {
                SetTable setRecord = await context.Sets
                    .Where(x => x.SetId == setId)
                    .Select(x => new SetTable { SetId = x.SetId })
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
