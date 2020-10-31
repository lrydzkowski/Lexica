using Lexica.Core.IO;
using Lexica.Core.Models;
using Lexica.Core.Validators;
using Lexica.Words.Models;
using Lexica.Words.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lexica.Words
{
    public class Importer
    {
        public ISetService SetService { get; }

        public IValidator<Set> SetValidator { get; }

        public IValidator<string, string> FileValidator { get; }

        public Importer(ISetService setService, IValidator<Set> setValidator, IValidator<string, string> fileValidator)
        {
            SetService = setService;
            SetValidator = setValidator;
            FileValidator = fileValidator;
        }

        public async Task<OperationResult> Import(IMultipleSource multipleSource)
        {
            var result = new OperationResult();

            List<ISource> sourceList = multipleSource.GetContents();
            if (sourceList.Count == 0)
            {
                result.AddError(
                    new Error(
                        (int)Words.Models.ErrorCodesEnum.NoImportSource,
                        "There are no files to import sets."
                    )
                );
                return result;
            }
            List<Set> sets = new List<Set>();
            foreach (var source in sourceList)
            {
                string contents = source.GetContents().Trim();

                result.Merge(FileValidator.Validate(contents, source.Name));
                if (!result.Result)
                {
                    continue;
                }

                List<string> lines = contents.Split("\n").Select(x => x.Trim()).ToList();
                List<Entry> entries = new List<Entry>();
                for (int i = 0; i < lines.Count; i++)
                {
                    string line = lines[i];
                    string[] lineParts = line.Split(';');
                    List<string> words = lineParts[0].Split(',').Select(x => x.Trim()).ToList<string>();
                    List<string> translations = lineParts[1].Split(',').Select(x => x.Trim()).ToList<string>();
                    entries.Add(new Entry(null, i + 1, words, translations));
                }
                if (entries.Count > 0)
                {
                    var setPath = new SetPath("Others", source.Name);
                    var setInfo = new SetInfo(null, setPath);
                    sets.Add(new Set(setInfo, entries));
                }
            }
            if (sets.Count > 0 && result.Result)
            {
                await SetService.CreateSet(sets);
            }

            return result;
        }
    }
}
