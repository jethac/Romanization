using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Japanese {
namespace Romanization
{
    public enum Kana
    {
        Hiragana,
        Katakana,
        NotKana
    }
    public class Util
    {
        private static int[] SmallKanaOffsets = { 
            0x0,
            0x2,
            0x4,
            0x6,
            0x8,
            0x22,
            0x42,
            0x44,
            0x46,
            0x48
        };

        private struct KanaRange
        {
            public char Start;
            public char End;
            public int Offset;
            public int PackCount;
        }
        private struct KanaRangeExt
        {
            public char Start;
            public char End;
            public char EndExtended;
        }

        private static Dictionary<char, KanaRange> ColumnMappings = new Dictionary<char, KanaRange>{
            {
                'あ',
                new KanaRange{
                    Start = 'ぁ',
                    End = 'お'
                }
            },
            {
                'か',
                new KanaRange{
                    Start = 'か',
                    End = 'ご',
                    PackCount = 2,
                    Offset = 0
                }
            },
            {
                'が',
                new KanaRange{
                    Start = 'か',
                    End = 'ご',
                    PackCount = 2,
                    Offset = 1
                }
            },
            {
                'さ',
                new KanaRange{
                    Start = 'さ',
                    End = 'ぞ',
                    PackCount = 2
                }
            },
            {
                'ざ',
                new KanaRange{
                    Start = 'さ',
                    End = 'ぞ',
                    PackCount = 2,
                    Offset = 1
                }
            },
            {
                'た',
                new KanaRange{
                    Start = 'た',
                    End = 'ど',
                    PackCount = 2,
                    Offset = 0
                }
            },
            {
                'だ',
                new KanaRange{
                    Start = 'た',
                    End = 'ど',
                    PackCount = 2,
                    Offset = 1
                }
            },
            {
                'な',
                new KanaRange{
                    Start = 'な',
                    End = 'の'
                }
            },
            {
                'は',
                new KanaRange{
                    Start = 'は',
                    End = 'ぽ',
                    PackCount = 3
                }
            },
            {
                'ば',
                new KanaRange{
                    Start = 'は',
                    End = 'ぽ',
                    PackCount = 3,
                    Offset = 1
                }
            },
            {
                'ぱ',
                new KanaRange{
                    Start = 'は',
                    End = 'ぽ',
                    PackCount = 3,
                    Offset = 2
                }
            },
            {
                'ま',
                new KanaRange{
                    Start = 'ま',
                    End = 'も'
                }
            },
            {
                'ら',
                new KanaRange{
                    Start = 'ら',
                    End = 'ろ'
                }
            },
            {
                'や',
                new KanaRange{
                    Start = 'ゃ',
                    End = 'よ'
                }
            },
            {
                'わ',
                new KanaRange{
                    Start = 'ゎ',
                    End = 'ん'
                }
            },
        };


        private static KanaRangeExt HiraganaRange = new KanaRangeExt
        {
            Start = (char)0x3041,
            End = (char)0x3093,
            EndExtended = (char)0x3096
        };
        private static KanaRangeExt KatakanaRange = new KanaRangeExt
        {
            Start = (char)0x30A1,
            End = (char)0x30F3,
            EndExtended = (char)0x30F6
        };

        public static Kana IsKana(char ch, bool extendedRange = false) {
            Kana rval = Kana.NotKana;

            if (IsHiragana(ch, extendedRange))
            {
                rval = Kana.Hiragana;
            }
            else if (IsKatakana(ch, extendedRange))
            {
                rval = Kana.Katakana;
            }

            return rval;
        }

        public static bool IsHiragana(char ch, bool extendedRange = false)
        {
            return IsInKanaRange(ch, ref HiraganaRange, extendedRange);
        }
        public static bool IsKatakana(char ch, bool extendedRange = false)
        {
            return IsInKanaRange(ch, ref KatakanaRange, extendedRange);
        }
        private static bool IsInKanaRange(char ch, ref KanaRangeExt range, bool extendedRange = false)
        {
            bool rval = ch >= range.Start && (ch <= range.End || (extendedRange && ch <= range.EndExtended));
            return rval;
        }

        public static char GetColumn(char ch) {
            char result = 'A';

            Kana type = IsKana(ch, false);
            if (type == Kana.NotKana) return result;

            int KatakanaOffset = (type == Kana.Katakana ? KatakanaRange.Start - HiraganaRange.Start : 0);

            bool found = false;
            foreach (KeyValuePair<char, KanaRange> kvp in ColumnMappings)
	        {
                if (kvp.Value.PackCount == 0)
                {
                    found =
                        ch >= kvp.Value.Start + KatakanaOffset &&
                        ch <= kvp.Value.End + KatakanaOffset;
                }
                else {
                    // Can't use a range check.
                    for (int i = (int)kvp.Value.Start + kvp.Value.Offset + KatakanaOffset; i <= (int)kvp.Value.End + KatakanaOffset; i += kvp.Value.PackCount)
                    {
                        found = (Char.Equals(ch, (char)i));
                        bool ta = Char.Equals(kvp.Key, 'た');
                        bool da = Char.Equals(kvp.Key, 'だ');

                        // Special case handling for ta/da
                        if (ta && Char.Equals((char)i, 'っ')) {
                            i -= 1; // don't retest
                        } else if (da && Char.Equals((char)i, 'つ')) {
                            i -= 1; // don't retest
                        }
                        if (found) break;
                    }
                }
                if (found)
                {
                    result = kvp.Key;
                    break;
                }
	        }

            return result;
        }
        public static bool IsSmallKana(char ch) {
            bool rval = false;

            Kana type = IsKana(ch, false);
            if (type == Kana.NotKana) return rval;

            int typeoffset = 0;
            if (type == Kana.Katakana) typeoffset = KatakanaRange.Start - HiraganaRange.Start;
            foreach (int offset in SmallKanaOffsets)
            {
                rval = Char.Equals(ch, (char)(HiraganaRange.Start + offset + typeoffset));
                if (rval)
                {
                    break;
                }
            }

            return rval;
        }

        /** Whether or not the given kana is a labial consonant (b-, p-, or m-); needed for
         *  modified Hepburn romanization.
         *  
         *  @param char ch  The character to test.
         *  @return bool    Whether the character is a labial consonant. Returns false if the
         *                  character isn't kana in the first place.
         */
        public static bool IsLabialConsonant(char ch)
        {
            bool rval = false;

            if (IsKana(ch) == Kana.NotKana) {
                return false;
            }
            char col = GetColumn(ch);
            rval = Char.Equals(col, 'ば') || Char.Equals(col, 'ぱ') || Char.Equals(col, 'ま');

            return rval;
        }
    
    }
}
}