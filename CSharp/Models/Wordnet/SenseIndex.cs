using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public class SenseIndex
    {
        public PartOfSpeech PartOfSpeech { get; set; }
        public List<string> Name { get; set; }
        public List<string> Classification { get; set; }
        public List<GlossWord> Definition { get; set; }
        public List<string> Example { get; set; }
        public List<SensePointer> Pointer { get; set; }
        public List<VerbFrame> VerbFrame { get; set; }
        public string UniqueId { get; set; }

        public uint Id { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var word in Definition) {
                if (word.HasLeadingSpace)
                    sb.Append(' ');
                sb.Append(word.Text);
            }
            return $"{String.Join(", ", Name)} - {sb.ToString()}";
        }
    }
}
