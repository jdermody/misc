using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icbld.Models.Wordnet
{
    public enum PointerType
    {
        Unknown = 0,
        Antonym,
        Hypernym,
        InstanceHypernym,
        MemberHolonym,
        SubstanceHolonym,
        PartHolonym,
        MemberMeronym,
        SubstanceMeronym,
        PartMeronym,
        Attribute,
        DerivationallyRelatedForm,
        DomainOfSynset_category,
        DomainOfSynset_region,
        DomainOfSynset_usage,
        MemberOfDomain_category,
        MemberOfDomain_region,
        MemberOfDomain_usage,
        Entailment,
        Cause,
        AlsoSee,
        VerbGroup,
        SimiliarTo,
        ParticipleOfVerb,
        RelatedTo,
        InstanceRelatedTo,
        DerivedFromAdjective,
        Pertainym,
        MAX
    }
}
