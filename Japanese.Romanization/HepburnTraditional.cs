using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Japanese.Romanization
{
    /** Traditional Hepburn romanizer. Mostly complete for native japanese words;
     *  needs katakana support (but that shouldn't be hard).
     */
    public class HepburnTraditional : IRomanizer
    {
        public static Dictionary<char, string> mappings = new Dictionary<char, string>
        {
            {'あ', "a"},
            {'い', "i"},
            {'う', "u"},
            {'え', "e"},
            {'お', "o"},

            {'か', "ka"},
            {'き', "ki"},
            {'く', "ku"},
            {'け', "ke"},
            {'こ', "ko"},
            {'が', "ga"},
            {'ぎ', "gi"},
            {'ぐ', "gu"},
            {'げ', "ge"},
            {'ご', "go"},
            
            {'さ', "sa"},
            {'し', "shi"},
            {'す', "su"},
            {'せ', "se"},
            {'そ', "so"},
            {'ざ', "za"},
            {'じ', "ji"},
            {'ず', "zu"},
            {'ぜ', "ze"},
            {'ぞ', "zo"},
            
            {'た', "ta"},
            {'ち', "chi"},
            {'つ', "tsu"},
            {'て', "te"},
            {'と', "to"},
            {'だ', "da"},
            {'ぢ', "ji"},
            {'づ', "zu"},
            {'で', "de"},
            {'ど', "do"},

            {'な', "na"},
            {'に', "ni"},
            {'ぬ', "nu"},
            {'ね', "ne"},
            {'の', "no"},
            
            {'は', "ha"},
            {'ひ', "hi"},
            {'ふ', "hu"},
            {'へ', "he"},
            {'ほ', "ho"},
            {'ば', "ba"},
            {'び', "bi"},
            {'ぶ', "bu"},
            {'べ', "be"},
            {'ぼ', "bo"},
            {'ぱ', "pa"},
            {'ぴ', "pi"},
            {'ぷ', "pu"},
            {'ぺ', "pe"},
            {'ぽ', "po"},

            {'ま', "ma"},
            {'み', "mi"},
            {'む', "mu"},
            {'め', "me"},
            {'も', "mo"},

            {'や', "ya"},
            {'ゆ', "yu"},
            {'よ', "yo"},
            {'ら', "ra"},
            {'り', "ri"},
            {'る', "ru"},
            {'れ', "re"},
            {'ろ', "ro"},
            {'わ', "wa"},
            {'を', "wo"},
            {'ん', "n"}
        };
        
        private string[] LongVowelPermutations = {
            "uu",
            "oo",
            "ou"
                                                 };

        public string GetRomanized(WordDefinition wd)
        {
            return this.GetRomanized(wd.Phonetic, wd.Kanji, wd.Written);
        }
        public string GetRomanized(string hiragana) {
            return this.GetRomanized(hiragana, null, null);
        }
        private string GetRomanized(string hiragana, Dictionary<char, string> kanjimappings = null, string kanji = null)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.Clear();

            bool bUseKanjiHints = false;
            bUseKanjiHints = kanji != null && kanjimappings != null;

            #region kanji
            if (bUseKanjiHints) {
                // in kanji hint mode, split the hiragana string.
                // @todo: less dumb
                Dictionary<string, bool> substrings = new Dictionary<string, bool>();
                StringBuilder sb_substr = new StringBuilder(24);
                sb_substr.Clear();
                foreach (char ch in kanji)
                {
                    // is this a kanji?
                    bool bNotKana = Util.IsKana(ch) == Kana.NotKana;
                    if (bNotKana)
                    {
                        // this is a kanji.
                        // flush the substring stringbuilder and add the 
                        // contents to the substring list.
                        if (sb_substr.Length > 0) {
                            substrings.Add(sb_substr.ToString(), false);
                            sb_substr.Clear();
                        }

                        // add the reading for the kanji to the substring list.
                        substrings.Add(kanjimappings[ch], true);
                    }
                    else {
                        sb_substr.Append(ch);
                    }
                }
                if (sb_substr.Length > 0)
                {
                    substrings.Add(sb_substr.ToString(), false);
                    sb_substr.Clear();
                }

                // now, do the romaji calc.
                foreach (KeyValuePair<string, bool> kvp in substrings)
                {
                    string thisromaji = this.GetRomanized(kvp.Key);

                    if (kvp.Value && thisromaji.Length > 1)
                    {
                        // this is kanji
                        bool bLongEndingVowel =
                            LongVowelPermutations.Contains(
                                thisromaji.Substring(
                                    thisromaji.Length - 2,
                                    2
                                )
                            );
                        //Trace.WriteLine(String.Format("long ending vowel: {0} ({1})", bLongEndingVowel, thisromaji));

                        if (bLongEndingVowel) {
                            thisromaji = Util.WithMacron(thisromaji);
                        }
                    }

                    sb.Append(thisromaji);
                }

            }
            #endregion
            #region justkana
            else {
                for (int i = 0; i < hiragana.Length; i++)
                {
                    if (Char.Equals(hiragana[i], 'ん'))
                    {
                        if (i < hiragana.Length - 1 && Util.IsLabialConsonant(hiragana[i + 1]))
                        {
                            sb.Append("m");
                        }
                        else
                        {
                            sb.Append(mappings[hiragana[i]]);
                        }
                    }
                    else if (Util.IsSmallKana(hiragana[i]))
                    {
                        sb.Remove(sb.Length - 1, 1);

                        // wait, let's quickly see if the ending letter is j
                        if (Char.Equals('j', sb[sb.Length - 1]))
                        {
                            // omit the 'y'
                            sb.Append(mappings[(char)(hiragana[i] + 1)][1]);
                        }
                        else
                        {
                            sb.Append(mappings[(char)(hiragana[i] + 1)]);
                        }
                    }
                    else
                    {
                        sb.Append(mappings[hiragana[i]]);
                    }
                }

                // now that we've done all of that...
                // we have no word boundary information, so...
                if (!String.Equals("mizuumi", sb.ToString())) {
                    sb.Replace("uu", Util.WithMacron('u').ToString());
                    sb.Replace("ou", Util.WithMacron('o').ToString());
                    sb.Replace("oo", Util.WithMacron('o').ToString());
                }
            }
            #endregion

            return sb.ToString();
        }
    }
}
