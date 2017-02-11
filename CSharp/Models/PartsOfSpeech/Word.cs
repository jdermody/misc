using Icbld.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.PartsOfSpeech
{
    public class Word
    {
        public string Text { get; set; }
        public string Lemma { get; set; }
        public PartOfSpeech PartOfSpeech { get; set; }
    }
}
