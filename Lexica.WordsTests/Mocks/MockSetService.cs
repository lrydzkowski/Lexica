using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.WordsTests.Mocks
{
    class MockSetService : ISetService
    {
        public Task<OperationResult> ChangePath(long setId, SetPath newPath)
        {
            throw new NotImplementedException();
        }

        public Task CreateSet(Set set)
        {
            throw new NotImplementedException();
        }

        public Task CreateSet(List<Set> sets)
        {
            throw new NotImplementedException();
        }

        public async Task<Set?> Get(long setId)
        {
            var source = new EmbeddedSource(
                "Resources/Importer/Embedded/Correct/example_set_1.txt",
                Assembly.GetExecutingAssembly()
            );
            List<Entry> entries = new List<Entry>();
            string contents = source.GetContents();
            List<string> rows = contents.Split("\n").ToList<string>();
            for (int i = 0; i < rows.Count; i++)
            {
                List<string> columns = rows[i].Split(';').ToList<string>();
                List<string> words = columns[0].Split(',').Select(x => x.Trim()).ToList<string>();
                List<string> translations = columns[1].Split(',').Select(x => x.Trim()).ToList<string>();
                entries.Add(new Entry(setId, i + 1, words, translations));
            }
            var setInfo = new SetInfo(setId, new SetPath("Others", "Other1"));
            var set = new Set(setInfo, entries);

            return set;
        }

        public async Task<Set?> Get(List<long> setIds)
        {
            return await Get(setIds[0]);
        }

        public Task<List<SetInfo>> GetInfoList()
        {
            throw new NotImplementedException();
        }

        public Task Remove(long setId)
        {
            throw new NotImplementedException();
        }
    }
}
