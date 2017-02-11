using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Tokenisation
{
    public enum SimpleTokenType
    {
        None,
        Word,
        Number,
        CurrencySymbol,
        Currency,
        Contraction,
        Punctuation,
    }
}
