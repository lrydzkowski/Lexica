using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Pronunciation
{
    public interface IPronunciation
    {
        public Task<bool> AudioExists(string word);

        public Task<bool> AudioExists(List<string> words);

        public Task PlayAsync(string word);

        public Task PlayAsync(List<string> words);
    }
}
