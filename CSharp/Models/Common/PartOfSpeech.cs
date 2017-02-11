using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Common
{
    public enum PartOfSpeech
    {
        Unknown = 0,
        Word,
        Punctuation,
        Number,
        Name,
        Interjection,
        Symbol,

        Article,
        Conjunction,
        Preposition,
        Pronoun,

        Noun,
        Verb,
        Adjective,
        Adverb,

        PossessiveNoun,
        PossessivePronoun,
        PossessiveName,
        PossessiveMarker,
        MAX
    };
}
