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
        private static char[] kana_ya = {
            'や',
            'ゆ',
            'よ'
        };
        private static int[] SmallKanaOffsets = { 0, 2, 4, 6, 8, 22, 42, 44, 46, 48 };


        private struct KanaRange
        {
            public char Start;
            public char End;
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
                    End = 'ご'
                }
            },
            {
                'さ',
                new KanaRange{
                    Start = 'さ',
                    End = 'ぞ'
                }
            },
            {
                'た',
                new KanaRange{
                    Start = 'た',
                    End = 'ど'
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
                    End = 'ぽ'
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

            bool found = false;
            foreach (KeyValuePair<char, KanaRange> kvp in ColumnMappings)
	        {
                found =
                    ch >= kvp.Value.Start + (type==Kana.Katakana? KatakanaRange.Start - HiraganaRange.Start : 0) &&
                    ch <= kvp.Value.End + (type == Kana.Katakana ? KatakanaRange.Start - HiraganaRange.Start : 0);
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
                break;
            }

            return rval;
        }
    }
}
}