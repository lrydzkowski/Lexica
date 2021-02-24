using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Words.Models;
using Lexica.Words.Validators;
using System.Collections.Generic;
using System.Linq;

namespace Lexica.Words.Services
{
    public class SetService : ISetService
    {
        public SetService(FileValidator fileValidator)
        {
            FileValidator = fileValidator;
        }

        public FileValidator FileValidator { get; set; }

        public OperationResult<Set?> Load(ISource fileSource)
        {
            return Load(new List<ISource> { fileSource });
        }

        public OperationResult<Set?> Load(List<ISource> filesSources)
        {
            var result = new OperationResult<Set?>(null);

            List<SetPath> setPaths = new List<SetPath>();
            List<Entry> entries = new List<Entry>();

            foreach (ISource fileSource in filesSources)
            {
                var setPath = new SetPath(fileSource.Path, fileSource.Name);
                string contents = fileSource.GetContents().Trim();

                result.Merge(FileValidator.Validate(contents, fileSource.Name));
                if (!result.Result)
                {
                    continue;
                }

                List<string> lines = contents.Split("\n").Select(x => x.Trim()).ToList();
                List<Entry> currentFileEntries = new List<Entry>();
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    string[] lineParts = line.Split(';');
                    List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList<string>();
                    List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList<string>();
                    currentFileEntries.Add(new Entry(setPath, i + 1, words, translations));
                }
                if (currentFileEntries.Count > 0)
                {
                    setPaths.Add(setPath);
                    entries.AddRange(currentFileEntries);
                }
            }

            if (result.Result)
            {
                var set = new Set(setPaths, entries);
                result.Data = set;
            }

            return result;
        }
    }
}
