using Lexica.Core.Models;
using Lexica.EF.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.EF.Services
{
    class SetService : ISetService
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
                        setRecord.Namespace = set.Path.Namespace;
                        setRecord.Name = set.Path.Name;
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
                    var error = new Error("SetDoesntExist", string.Format("Set with id {0} doesn't exist.", setId));
                    return new OperationResult(false, error);
                }
                setRecord.Namespace = newPath.Namespace;
                setRecord.Name = newPath.Name;
                await context.SaveChangesAsync();
            }
            return new OperationResult(true);
        }

        public Task<Set> Get(long setId)
        {
            throw new NotImplementedException();
        }

        public Task<Set> Get(List<long> setIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<Set>> GetList(bool includeEntries = false)
        {
            throw new NotImplementedException();
        }

        public Task<OperationResult> Remove(long setId)
        {
            throw new NotImplementedException();
        }
    }
}
