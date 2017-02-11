using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Tokenisation
{
    public class Token
    {
        public SimpleTokenType Type { get; set; }
        public int StartOffset { get; set; }
        public int Length { get; set; }
        public string Lemma { get; set; }
    }
}
