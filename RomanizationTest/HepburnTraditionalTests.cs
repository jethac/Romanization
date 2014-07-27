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
            Assert.AreEqual("jaaku", ht.GetRomanized(wd_1));
            Assert.AreEqual("obaasan", ht.GetRomanized(wd_2));
        }

        [TestMethod]
        public void RomanizeLongII()
        {
            WordDefinition wd_1 = new Japanese.WordDefinition("お兄さん", "おにいさん", new Dictionary<char, string>() { { '兄', "にい" } });
            WordDefinition wd_2 = new Japanese.WordDefinition("灰色", "はいいろ", new Dictionary<char, string>() { { '灰', "はい" }, { '色', "いろ" } });

            HepburnTraditional ht = new HepburnTraditional();
            Assert.AreEqual("oniisan", ht.GetRomanized(wd_1.Phonetic));
            Assert.AreEqual("haiiro", ht.GetRomanized(wd_2.Phonetic));
        }

        [TestMethod]
        public void RomanizeLongUU()
        {
            WordDefinition wd_1 = new Japanese.WordDefinition("食う", "くう", new Dictionary<char, string>() { { '食', "く" } });
            WordDefinition wd_2 = new Japanese.WordDefinition("数学", "すうがく", new Dictionary<char, string>() { { '数', "すう" }, { '学', "がく" } });
            string wd_3 = "ぐうたら";
            WordDefinition wd_4 = new Japanese.WordDefinition("湖", "みずうみ", new Dictionary<char, string>() { { '湖', "みずうみ" } });

            HepburnTraditional ht = new HepburnTraditional();
            Assert.AreEqual("kuu", ht.GetRomanized(wd_1));
            Assert.AreEqual("sūgaku", ht.GetRomanized(wd_2));
            Assert.AreEqual("gūtara", ht.GetRomanized(wd_3));
            Assert.AreEqual("mizuumi", ht.GetRomanized(wd_4));
        }
        [TestMethod]
        public void RomanizeLongOO()
        {
            WordDefinition wd_1 = new Japanese.WordDefinition("小躍り", "こおどり", new Dictionary<char, string>() { { '小', "こ" }, { '躍', "おど" } });
            WordDefinition wd_2 = new Japanese.WordDefinition("氷", "こおり", new Dictionary<char, string>() { { '氷', "こおり" } });


            HepburnTraditional ht = new HepburnTraditional();
            Assert.AreEqual("koodori", ht.GetRomanized(wd_1));
            Assert.AreEqual("kōri", ht.GetRomanized(wd_2));
        }
    }
}
