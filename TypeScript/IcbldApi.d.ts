declare module Models {
    export interface Continuation {
        id: string;
    }
    export const enum JobType {
        Tokenise = 0,
        WordEmbedding,
        PartsOfSpeech,
        SyntaxAnalysis,
        SentimentAnalysis,
        TopicDetection
    }
    export interface JobInfo {
        id: number;
        type: JobType;
        status: string;
        date: Date;
        hasCompleted: boolean;
        inputSize: number;
        outputSize: number;
        cost: number;
    }

    //--------------------------------------------------------------------------------------
    export const enum PartOfSpeech {
        unknown = 0,
        word,
        punctuation,
        number,
        name,
        interjection,
        symbol,
        article,
        conjunction,
        preposition,
        pronoun,
        noun,
        verb,
        adjective,
        adverb,
        possessiveNoun,
        possessivePronoun,
        possessiveName,
        possessiveMarker,
    }
    export const enum WordnetPartOfSpeech {
        unknown = 0,
        noun = 1,
        verb = 2,
        adjective = 3,
        adjectiveSatellite = 4,
        adverb = 5,
        punctuation = 6,
    }
    export const enum WordnetPointerType {
        unknown = 0,
        antonym = 1,
        hypernym = 2,
        instanceHypernym = 3,
        memberHolonym = 4,
        substanceHolonym = 5,
        partHolonym = 6,
        memberMeronym = 7,
        substanceMeronym = 8,
        partMeronym = 9,
        attribute = 10,
        derivationallyRelatedForm = 11,
        domainOfSynset_category = 12,
        domainOfSynset_region = 13,
        domainOfSynset_usage = 14,
        memberOfDomain_category = 15,
        memberOfDomain_region = 16,
        memberOfDomain_usage = 17,
        entailment = 18,
        cause = 19,
        alsoSee = 20,
        verbGroup = 21,
        similiarTo = 22,
        participleOfVerb = 23,
        relatedTo = 24,
        instanceRelatedTo = 25,
        derivedFromAdjective = 26,
        pertainym = 27,
    }
    export interface GlossWord {
        text: string;
        pos: PartOfSpeech;
        semanticIndex?: number;
        hasLeadingSpace: boolean;
    }
    export interface SensePointer {
        id: number;
        type: WordnetPointerType;
        source: number;
        target: number;
    }
    export interface VerbFrame {
        index: number;
        wordIndex: number;
    }
    export interface SenseIndex {
        partOfSpeech: WordnetPartOfSpeech;
        name: string[];
        classification: string[];
        definition: GlossWord[];
        example: string[];
        pointer: SensePointer[];
        verbFrame: VerbFrame[];
        id: number;
    }
    export interface ExpandedSensePointer extends SenseIndex {
        type: WordnetPointerType;
        source: number;
        target: number;
    }
    export interface ExpandedSenseIndex extends SenseIndex {
        expandedPointer: ExpandedSensePointer[];
    }
    export interface WordnetSearchResults {
        suggestion: string[];
        sense: SenseIndex[];
    }

    //--------------------------------------------------------------------------------------
    export interface WordEmbedding {
        word: string;
        score: number;
    }
    export interface Word {
        isPositive: boolean;
        text: string;
    }
    export interface WordEmbeddingResults {
        query: string;
        results: WordEmbedding[];
        wordList: Word[];
    }

    //--------------------------------------------------------------------------------------
    export const enum SimpleTokenType {
        None = 0,
        Word,
        Number,
        CurrencySymbol,
        Currency,
        Contraction,
        Punctuation,
    }
    export interface TokenResultsToken {
        type: SimpleTokenType;
        startOffset: number;
        length: number;
        lemma: string;
    }
    export interface TokenResults {
        text: string;
        tokens: TokenResultsToken[];
    }

    //--------------------------------------------------------------------------------------
    export interface DetectedTopic {
        topicId: number;
        title: string;
        senseIndex: number[];
    }
    export interface TopicDetectionResults {
        category: string[];
        topic: DetectedTopic[];
    }

    //--------------------------------------------------------------------------------------
    export interface POSWord {
        text: string;
        lemma: string;
        partOfSpeech: PartOfSpeech;
    }
    export interface POSSentence {
        words: POSWord[];
    }
    export interface PartOfSpeechResults {
        sentences: POSSentence[];
    }

    //--------------------------------------------------------------------------------------
    export const enum TokenType {
        none = 0,
        url,
        email,
        date,
        time,
        currency,
        numericPhrase,
        weight,
        length,
        temperature,
        speed,
        volume,
        area,
        person,
        malePerson,
        femalePerson,
        group,
        location,
        noun,
        nounPhrase,
        prepositionPhrase,
        subject,
        object,
        verb,
        presentSimple,
        presentContinuous,
        presentPerfect,
        presentPerfectContinuous,
        pastSimple,
        pastContinuous,
        pastPerfect,
        pastPerfectContinuous,
        futureSimple,
        futureContinuous,
        futurePerfect,
        futurePerfectContinuous,
        futureInPast,
        usedTo,
        wouldAlways,
    }
    export interface Token {
        type: TokenType;
        pos: PartOfSpeech;
        text: string;
        lemma: string;
        offset: number;
        isCapitalised: boolean;
        isPlural: boolean;
        isPassive: boolean;
        topicId: number;
        children: Token[];
    }
    export interface ParseResults {
        token: Token[];
    }

    //--------------------------------------------------------------------------------------
    export interface SentimentAnalysisWord
    {
        text: string;
        lemma: string;
        pos: PartOfSpeech;
        objectivity: number;
        sentiment: number;
    }
    export interface SentimentAnalysisSentence {
        words: SentimentAnalysisWord[];
    }
    export interface SentimentAnalysisResults {
        sentences: SentimentAnalysisSentence[];
    }
}