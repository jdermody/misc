using Icbld.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Syntax
{
    public class Token
    {
        public TokenType Type { get; set; }
        public PartOfSpeech POS { get; set; }
        public string Text { get; set; }
        public string Lemma { get; set; }
        public uint Offset { get; set; }
        public bool IsCapitalised { get; set; }
        public bool IsPlural { get; set; }
        public bool IsPassive { get; set; }
        public uint? TopicId { get; set; }
        public Token[] Children { get; set; }
    }
}
