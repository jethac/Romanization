using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime;
using System.Runtime.Serialization;

namespace Japanese
{
    [DataContract]
    public class WordDefinition
    {
        [DataMember]
        public string Written = "";

        [DataMember]
        public string Phonetic = "";

        [DataMember]
        public Dictionary<char, string> Kanji = new Dictionary<char, string>();

        public WordDefinition(string wr, string ph, Dictionary<char, string> kj) {
            this.Written = wr;
            this.Phonetic = ph;
            this.Kanji = kj;
        }
    }
}
