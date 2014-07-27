using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Japanese.Romanization
{
    /** Traditional Hepburn romanizer. Currently very, *very* naive and incomplete.
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

        public string GetRomanized(string hiragana) {
            return this.GetRomanized(hiragana, null, null);
        }
        public string GetRomanized(string hiragana, Dictionary<char, string> kanjimappings = null, string kanji = null)
        {
            StringBuilder sb = new StringBuilder(1024);
            sb.Clear();

            bool bUseKanjiHints = false;
            bUseKanjiHints = kanji != null && kanjimappings != null;

            if (bUseKanjiHints) {

            } else {
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
                        sb.Append(mappings[(char)(hiragana[i] + 1)]);
                    }
                    else
                    {
                        sb.Append(mappings[hiragana[i]]);
                    }
                }
            }

            return sb.ToString();
        }
    }
}
