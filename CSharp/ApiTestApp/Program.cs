using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiTestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(async () =>
            {
                var api = new Icbld.Api("fb4cb54264f44a969321c1b88ebc2516");

                var text = File.ReadAllText(@"D:\dump\bach.txt");
                var tokenisation = await api.Tokenise(text);

                var syntaxAnalysis = await api.SyntaxAnalysis("This is a test");
                var pos = await api.PartsOfSpeech("This is a test");
                var sentiment = await api.SentimentAnalysis("I hate chocolate");
                var topic = await api.TopicDetection("User interface design has been a topic of considerable research, including on its aesthetics. Standards have been developed as far back as the 1980s for defining the usability of software products.");
                var embedding = await api.WordEmbedding("bondi");

                var results = await api.WordnetSearch("test");
                for (var i = 0; i < 2; i++) {
                    if (i == 0) {
                        var senseIndex = await api.GetSenseIndex(results.Sense[0].Id);
                    }
                    else {
                        var expandedSenseIndex = await api.GetExpandedSenseIndex(results.Sense[1].Id);
                    }
                }
            }).GetAwaiter().GetResult();
        }
    }
}
