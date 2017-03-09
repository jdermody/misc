declare module CleverClobs {
    export module Models {
        // The type of job
        export const enum JobType {
            // Tokenisation
            tokenise = 0,

            // Word embedding
            wordEmbedding,

            // Parts of speech
            partsOfSpeech,

            // Syntax analysis
            syntaxAnalysis,

            // Sentiment analysis
            sentimentAnalysis,

            // Topic detection
            topicDetection,

            // Semantic analysis
            semanticAnalysis,

        }
        // Part of speech tag
        export const enum PartOfSpeech {
            // Not known
            unknown = 0,

            // A word
            word,

            // A punctuation character
            punctuation,

            // A number
            number,

            // A given name
            name,

            // Uh, huh etc
            interjection,

            // A symbol
            symbol,

            // An article (the, a, etc)
            article,

            // A conjunction(and, or, etc)
            conjunction,

            // A preposition (for, to, etc)
            preposition,

            // A pronoun (he, she, etc)
            pronoun,

            // A noun
            noun,

            // A verb
            verb,

            // An adjective
            adjective,

            // An adverb
            adverb,

            // A possessive noun (the dog's, a rabbit's, etc)
            possessiveNoun,

            // A posssessive pronoun (her, his, etc)
            possessivePronoun,

            // A possessive name (John's, Julia's, etc)
            possessiveName,

            // A possessive marker (')
            possessiveMarker,

            // Maximum value for POS
            max,

        }
        // The type of a token
        export const enum TokenType {
            // Nothing special
            none = 0,

            // A url
            url,

            // An email address
            email,

            // A date
            date,

            // A time
            time,

            // A currency symbol
            currency,

            // A number
            number,

            // Punctuation characters
            punctuation,

            // A numeric phrase
            numericPhrase,

            // A weight (e.g. 5kgs)
            weight,

            // A length (e.g. 2km)
            length,

            // A temperature (e.g. 5c)
            temperature,

            // A speed (e.g. 100kmh)
            speed,

            // A volume
            volume,

            // An area
            area,

            // A person
            person,

            // A male person
            malePerson,

            // A female person
            femalePerson,

            // An organisation or group
            group,

            // A location
            location,

            // A noun
            noun,

            // A noun phrase
            nounPhrase,

            // A preposition phrase
            prepositionPhrase,

            // A verb phrase subject
            subject,

            // A verb phrase object
            object,

            // A verb
            verb,

            // A present tense verb phrase
            presentSimple,

            // A present continuous tense verb phrase
            presentContinuous,

            // A present perfect tense verb phrase
            presentPerfect,

            // A present perfect continuous tense verb phrase
            presentPerfectContinuous,

            // A past tense verb phrase
            pastSimple,

            // A past continuous tense verb phrase
            pastContinuous,

            // A past perfect tense verb phrase
            pastPerfect,

            // A past perfect continuous tense verb phrase
            pastPerfectContinuous,

            // A future tense verb phrase
            futureSimple,

            // A future continuous tense verb phrase
            futureContinuous,

            // A future perfect tense verb phrase
            futurePerfect,

            // A future perfect continuous tense verb phrase
            futurePerfectContinuous,

            // A future in past tense verb phrase
            futureInPast,

            // A "used to" verb phrase
            usedTo,

            // A "would always" verb phrase
            wouldAlways,

            // Infinitive verb phrase
            infinitive,

        }
        // WordNet sense pointer type
        export const enum PointerType {
            // Not known
            unknown = 0,

            // Antonym
            antonym,

            // A more generic term
            hypernym,

            // A specific instance of a more generic term
            instanceHypernym,

            // Holonym ("a part of") by member
            memberHolonym,

            // Holonym ("a part of") by substance
            substanceHolonym,

            // Holonym ("a part of")
            partHolonym,

            // Meronym ("is a part of") by member
            memberMeronym,

            // Meronym ("is a part of") by substance
            substanceMeronym,

            // Meronym ("is a part of")
            partMeronym,

            // A noun for which adjectives express values. The noun weight is an attribute, for which the adjectives light and heavy express values.
            attribute,

            // Terms in different syntactic categories that have the same root form and are semantically related.
            derivationallyRelatedForm,

            // Related by category
            domainOfSynset_category,

            // Related by region
            domainOfSynset_region,

            // Related by usage
            domainOfSynset_usage,

            // Related by category
            memberOfDomain_category,

            // Related by region
            memberOfDomain_region,

            // Related by usage
            memberOfDomain_usage,

            // A verb X entails Y if X cannot be done unless Y is, or has been, done.
            entailment,

            // Verb causation
            cause,

            // Also see
            alsoSee,

            // Verb group
            verbGroup,

            // Similiar to
            similiarTo,

            // Participle Of Verb
            participleOfVerb,

            // Hyponym - a "type of" relationship
            relatedTo,

            // Hyponym ("type of") relationship based on a specific instance
            instanceRelatedTo,

            // Derived from adjective
            derivedFromAdjective,

            // A relational adjective. Adjectives that are pertainyms are usually defined by such phrases as "of or pertaining to" and do not have antonyms. A pertainym can point to a noun or another pertainym.
            pertainym,

            // Maximum index
            max,

        }
        // Part of speech for WordNet sense indices
        export const enum WordnetPartOfSpeech {
            // Not knowne
            unknown = 0,

            // Noun
            noun,

            // Verb
            verb,

            // Adjective
            adjective,

            // Adjective satellite. See https://wordnet.princeton.edu/wordnet/man/wngloss.7WN.html
            adjectiveSatellite,

            // Adverb
            adverb,

            // Punctuation
            punctuation,

        }

        // Information about a lodged job
        export interface JobInfo {
            // Job id
            id: number,

            // Job type
            type: JobType,

            // Job completion status
            status: string,

            // Job lodgement date
            date: Date,

            // True if the job has completed
            hasCompleted: boolean,

            // Size in characters of the job input
            inputSize: number,

            // Size in bytes of the job's output
            outputSize: number,

            // Cost in credits to execute the job
            cost: number,
        }

        // A ranked word
        export interface RankedWord {
            // The word text
            word: string,

            // The word's score
            score: number,
        }

        // Parsed sentence
        export interface Sentence {
            // List of words
            words: Word[],
        }

        // Simple tokenisation token
        export interface SimpleToken {
            // Token type
            type: TokenType,

            // Token text
            text: string,

            // Token lemma (optional)
            lemma: string,
        }

        // A recursive token within a parse tree
        export interface Token {
            // Token type
            type: TokenType,

            // Token part of speech
            pos: PartOfSpeech,

            // Token text
            text: string,

            // Base form of word (optional)
            lemma: string,

            // Equivalent to an ordered id - the offset of the token within the text
            offset: number,

            // True if the token is capitalised
            isCapitalised: boolean,

            // True if the token is a plural noun
            isPlural: boolean,

            // True if the token is a passive verb
            isPassive: boolean,

            // True if the token is a question
            isQuestion: boolean,

            // Topic id (if any) associated with the token
            topicId?: number,

            // List of the token's children (optional)
            children: Token[],

            // List of possible WordNet sense indices (if any)
            senseIndex: number[],

            // List of referents, indexed by token offset (optional)
            referent: number[],
        }

        // Topic detection results
        export interface TopicDetectionResults {
            // List of ranked likely topic domains
            category: string[],

            // List of topics found in the text
            topic: TopicInfo[],
        }

        // Topic information
        export interface TopicInfo {
            // Topic id
            topicId: number,

            // Name of the topic
            title: string,

            // Related sense indices
            sense: number[],

            // List of the topic's categories
            category: string[],
        }

        // A parsed word
        export interface Word {
            // Word text
            text: string,

            // Base form of word (optional)
            lemma: string,

            // Word part of speech
            pos: PartOfSpeech,

            // The strength of sentiment - from 0 (low) to 1 (high)
            sentimentStrength: number,

            // The type of sentiment expressed - from 0 (negative) to 1 (positive)
            sentimentPolarity: number,
        }

        // Results from executing a word embedding query
        export interface WordEmbeddingResults {
            // List of related words
            word: RankedWord[],

            // Word embedding vector
            vector: number[],
        }

        // A sense index whose sense pointers have been expanded to include the sense indices they refer to
        export interface ExpandedSenseIndex {
            // List of expanded sense pointers
            expandedPointer: ExpandedSensePointer[],
        }

        // A sense pointer that has been expanded to include the sense index it points to
        export interface ExpandedSensePointer {
            // Pointer type
            type: PointerType,

            // Source word index (0 for all words)
            source: number,

            // Target word index (0 for all words)
            target: number,
        }

        // A word within a sense index's definition
        export interface GlossWord {
            // Word text
            text: string,

            // Part of speech
            pos: PartOfSpeech,

            // (Optional) sense index id
            semanticIndex?: number,

            // True if the word has a leading space
            hasLeadingSpace: boolean,
        }

        // WordNet sense index
        export interface SenseIndex {
            // Part of speech
            partOfSpeech: WordnetPartOfSpeech,

            // List of words in the sense index's name
            name: string[],

            // List of words that specify the sense index domain (optional)
            classification: string[],

            // List of words in the definition
            definition: GlossWord[],

            // List of example sentences (optional)
            example: string[],

            // List of pointers to related sense indices
            pointer: SensePointer[],

            // List of verb frames (optional)
            verbFrame: VerbFrame[],

            // Unique id
            uniqueId: string,

            // Sense index id
            id: number,
        }

        // Pointer to a sense index
        export interface SensePointer {
            // Sense index id
            id: number,

            // Type of pointer
            type: PointerType,

            // Source word index (0 for all words)
            source: number,

            // Target word index (0 for all words)
            target: number,
        }

        // A verb frame
        export interface VerbFrame {
            // Verb frame index. See https://wordnet.princeton.edu/man/wninput.5WN.html#toc4
            index: number,

            // Word index that the frame refers to (0 for all words)
            wordIndex: number,
        }

        // WordNet search results
        export interface WordnetSearchResults {
            // List of suggested words if nothing was found
            suggestion: string[],

            // List of matching sense indices
            sense: SenseIndex[],
        }

    }
}

