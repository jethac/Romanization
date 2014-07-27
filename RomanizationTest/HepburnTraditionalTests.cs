using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Japanese.Romanization
{
    [TestClass]
    public class HepburnTraditionalTests
    {
        [TestMethod]
        public void RomanizeNLabial()
        {
            string[] testArray = {
                "にほんご",
                "せんぱい",
                "ばんざい"
            };
            string[] expectedResults = {
                "nihongo",
                "sempai",
                "banzai"
            };

            HepburnTraditional ht = new HepburnTraditional();
            for (int i = 0; i < testArray.Length; i++)
            {
                string result = ht.GetRomanized(testArray[i]);
                Assert.AreEqual(expectedResults[i], result);
            }
        }

        [TestMethod]
        public void RomanizeLongAA()
        {
            WordDefinition wd_1 = new Japanese.WordDefinition("邪悪", "じゃあく", new Dictionary<char, string>() { { '邪', "じゃ" }, { '悪', "あく" } });
            WordDefinition wd_2 = new Japanese.WordDefinition("お婆さん", "おばあさん", new Dictionary<char, string>() { { '婆', "ばあ" } });

            HepburnTraditional ht = new HepburnTraditional();
            Assert.AreEqual("jaaku", ht.GetRomanized(wd_1.Phonetic, wd_1.Kanji, wd_1.Written));
            Assert.AreEqual("obaasan", ht.GetRomanized(wd_2.Phonetic, wd_2.Kanji, wd_2.Written));
        }
    }
}
