using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class GlossWord
    {
        public string Text { get; set; }
        public PartOfSpeech Pos { get; set; }
        public uint? SemanticIndex { get; set; }
        public bool HasLeadingSpace { get; set; }

        public override string ToString()
        {
            return $"{Text} - {Pos.ToString()}";
        }
    }
}
