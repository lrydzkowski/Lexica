using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lexica.Pronunciation
{
    public interface IPronunciation
    {
        public Task PlayAsync(string word);

        public Task PlayAsync(List<string> words);
    }
}
