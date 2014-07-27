using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Japanese.Romanization
{
    public interface IRomanizer
    {
        string GetRomanized(string hiragana);
        string GetRomanized(string hiragana, Dictionary<char, string> kanjimappings, string kanji = null);
    }
}
