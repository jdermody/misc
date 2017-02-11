using Icbld.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Icbld
{
    public class Api : IDisposable
    {
        readonly string _key;
        readonly HttpClient _client = new HttpClient();
        bool _isDisposed = false;

        #region Constants
        const string BASE_URL = "https://www.icbld.com/api/";
        const int DEFAULT_RETRY_COUNT = 10;
        const int DEFAULT_WAIT_MS = 500; 
        #endregion

        public Api(string key)
        {
            _key = key;
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", key);
        }

        #region Finalisation and Disposal
        ~Api()
        {
            _Dispose();
        }

        public void Dispose()
        {
            _Dispose();
            GC.SuppressFinalize(this);
        }

        void _Dispose()
        {
            if (!_isDisposed) {
                _client.Dispose();
                _isDisposed = true;
            }
        } 
        #endregion

        public string Key { get { return _key; } }

        public async Task<Models.Wordnet.SearchResults> WordnetSearch(string query)
        {
            return await _Fetch<Models.Wordnet.SearchResults>("wordnet/search?q=" + Uri.EscapeDataString(query));
        }

        public async Task<Models.Wordnet.SenseIndex> GetSenseIndex(uint senseIndex)
        {
            return await _Fetch<Models.Wordnet.SenseIndex>("wordnet/get/" + senseIndex.ToString());
        }

        public async Task<Models.Wordnet.ExpandedSenseIndex> GetExpandedSenseIndex(uint senseIndex)
        {
            return await _Fetch<Models.Wordnet.ExpandedSenseIndex>("wordnet/expanded/" + senseIndex.ToString());
        }

        public async Task<Models.Tokenisation.TokenResults> Tokenise(string text)
        {
            return await _PostJob(
                "job/tokenise/",
                Encoding.UTF8.GetBytes(text), 
                async jobInfo => await _Fetch<Models.Tokenisation.TokenResults>("job/tokenise/" + jobInfo.Id.ToString())
            );
        }

        public async Task<Models.PartsOfSpeech.PartOfSpeechResults> PartsOfSpeech(string text)
        {
            return await _PostJob(
                "job/pos/",
                Encoding.UTF8.GetBytes(text),
                async jobInfo => await _Fetch<Models.PartsOfSpeech.PartOfSpeechResults>("job/pos/" + jobInfo.Id.ToString())
            );
        }

        public async Task<Models.TopicDetection.TopicDetectionResults> TopicDetection(string text)
        {
            return await _PostJob(
                "job/topicdetection/",
                Encoding.UTF8.GetBytes(text),
                async jobInfo => await _Fetch<Models.TopicDetection.TopicDetectionResults>("job/topicdetection/" + jobInfo.Id.ToString())
            );
        }

        public async Task<Models.WordEmbedding.WordEmbeddingResults> WordEmbedding(string text)
        {
            return await _PostJob(
                "job/embedding/",
                Encoding.UTF8.GetBytes(text),
                async jobInfo => await _Fetch<Models.WordEmbedding.WordEmbeddingResults>("job/embedding/" + jobInfo.Id.ToString())
            );
        }

        public async Task<Models.Syntax.SyntaxResults> SyntaxAnalysis(string text)
        {
            return await _PostJob(
                "job/syntax/",
                Encoding.UTF8.GetBytes(text),
                async jobInfo => await _Fetch<Models.Syntax.SyntaxResults>("job/syntax/" + jobInfo.Id.ToString())
            );
        }

        public async Task<Models.Sentiment.SentimentAnalysisResults> SentimentAnalysis(string text)
        {
            return await _PostJob(
                "job/sentiment/",
                Encoding.UTF8.GetBytes(text),
                async jobInfo => await _Fetch<Models.Sentiment.SentimentAnalysisResults>("job/sentiment/" + jobInfo.Id.ToString())
            );
        }

        #region Helper Methods
        async Task<T> _PostJob<T>(string url, byte[] inputData, Func<JobInfo, Task<T>> callback)
            where T : class
        {
            var response = await _client.PostAsync(BASE_URL + url, new ByteArrayContent(inputData));
            var json = await response.Content.ReadAsStringAsync();
            var jobInfo = JsonConvert.DeserializeObject<JobInfo>(json);

            if (jobInfo.HasCompleted)
                return await callback(jobInfo);
            else {
                for (var i = 0; i < DEFAULT_RETRY_COUNT; i++) {
                    Thread.Sleep(DEFAULT_WAIT_MS);
                    var status = await _client.GetStringAsync(BASE_URL + "job/" + jobInfo.Id.ToString() + "/status");
                    if (status == "\"complete\"")
                        return await callback(jobInfo);
                }
                return null;
            }
        }

        async Task<T> _Fetch<T>(string url)
        {
            var json = await _client.GetStringAsync(BASE_URL + url);
            return JsonConvert.DeserializeObject<T>(json);
        } 
        #endregion
    }
}
