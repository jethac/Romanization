using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Japanese.Romanization;

namespace RomanizationTest
{
    [TestClass]
    public class UtilTests
    {
        [TestMethod]
        public void IsHiraganaTest()
        {
            char[] testArray = { 'a', 'z', 'あ', 'ぞ', 'ん', 'カ', 'ワ' };
            bool[] expectedResults = { false, false, true, true, true, false, false };

            for (int i = 0; i < testArray.Length; i++)
            {
                bool result = Util.IsHiragana(testArray[i], false);
                Assert.AreEqual(expectedResults[i], result);
            }
        }
        [TestMethod]
        public void IsKatakanaTest()
        {
            char[] testArray = { 'a', 'z', 'あ', 'ぞ', 'ん', 'カ', 'ワ' };
            bool[] expectedResults = { false, false, false, false, false, true, true };

            for (int i = 0; i < testArray.Length; i++)
            {
                bool result = Util.IsKatakana(testArray[i], false);
                Assert.AreEqual(expectedResults[i], result);
            }
        }
        [TestMethod]
        public void IsKanaTest()
        {
            char[] testArray = { 'a', 'z', 'あ', 'ぞ', 'ん', 'カ', 'ワ' };
            Kana[] expectedResults = {
                Kana.NotKana,
                Kana.NotKana,
                Kana.Hiragana,
                Kana.Hiragana,
                Kana.Hiragana,
                Kana.Katakana,
                Kana.Katakana
            };
            for (int i = 0; i < testArray.Length; i++)
            {
                Kana result = Util.IsKana(testArray[i], false);
                Assert.AreEqual(expectedResults[i], result);
            }
        }

        [TestMethod]
        public void ColumnDetectTest()
        {
            char[] testArray = { 'a', 'z', 'あ', 'ウ', 'ぎ', 'ク', 'と',};// 'デ', 'ン' };
            char[] expectedResults = {
                'A',
                'A',
                'あ',
                'あ',
                'が',
                'か',
                'た',
                //'だ',
                //'わ'
            };
            for (int i = 0; i < testArray.Length; i++)
            {
                char result = Util.GetColumn(testArray[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }
    }
}
