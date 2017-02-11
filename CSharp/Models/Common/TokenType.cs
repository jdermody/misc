using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Common
{
    public enum TokenType
    {
        None = 0,
        Url,
        Email,
        Date,
        Time,
        Currency,
        NumericPhrase,

        Weight,
        Length,
        Temperature,
        Speed,
        Volume,
        Area,

        Person,
        MalePerson,
        FemalePerson,
        Group,
        Location,
        Noun,

        NounPhrase,
        PrepositionPhrase,

        Subject,
        Object,

        Verb,
        PresentSimple,
        PresentContinuous,
        PresentPerfect,
        PresentPerfectContinuous,
        PastSimple,
        PastContinuous,
        PastPerfect,
        PastPerfectContinuous,
        FutureSimple,
        FutureContinuous,
        FuturePerfect,
        FuturePerfectContinuous,
        FutureInPast,
        UsedTo,
        WouldAlways,
    }
}
