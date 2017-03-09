module CleverClobs {
    function getJson<T>(url: string, auth: string, callback: (obj: T) => void, onError: () => void) {
        var request = new XMLHttpRequest();
        request.open("GET", url, true);
        request.setRequestHeader("Authorization", "Basic " + auth);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400)
                callback(JSON.parse(request.response));
            else
                onError();
        };

        request.onerror = function () {
            onError();
        };

        request.send();
    }
    function postText<T>(url: string, auth: string, obj: any, callback: (obj: T) => void, onError: () => void) {
        var request = new XMLHttpRequest();
        request.open("POST", url, true);
        request.setRequestHeader("Authorization", "Basic " + auth);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400)
                callback(JSON.parse(request.response));
            else
                onError();
        };

        request.onerror = function () {
            onError();
        };

        request.send(obj);
    }
    function sendDelete(url: string, auth: string, callback: () => void, onError: () => void) {
        var request = new XMLHttpRequest();
        request.setRequestHeader("Authorization", "Basic " + auth);
        request.open("DELETE", url, true);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400)
                callback();
            else
                onError();
        };

        request.onerror = function () {
            onError();
        };

        request.send();
    }

    function fail(callback: (obj: any) => void) {
        console.log("There was a problem calling the ICBLD API");
        callback && callback(null);
    }

    export class Api {
        static BASE_URL = "https://www.icbld.com/api/";
        static POLL_INTERVAL_MS = 500;

        constructor(private auth: string) {
        }

        public wordnetSearch(query: string, callback: (result: Models.WordnetSearchResults) => void) {
            if (query && query.length) {
                getJson(Api.BASE_URL + "wordnet/search?q=" + encodeURIComponent(query), this.auth, callback, () => fail(callback));
            } else
                callback(null);
        }

        public get(sid: number, callback: (result: Models.SenseIndex) => void) {
            getJson(Api.BASE_URL + "wordnet/get/" + sid, this.auth, callback, () => fail(callback));
        }

        public getExpanded(sid: number, callback: (result: Models.ExpandedSenseIndex) => void) {
            getJson(Api.BASE_URL + "wordnet/expanded/" + sid, this.auth, callback, () => fail(callback));
        }

        public getJobs(callback: (result: Models.JobInfo[]) => void) {
            getJson(Api.BASE_URL + "job", this.auth, callback, () => fail(callback));
        }

        public getJobStatus(job: Models.JobInfo, callback: (result: string) => void) {
            getJson(Api.BASE_URL + "job/" + job.id + "/status", this.auth, callback, () => fail(callback));
        }

        public deleteJob(job: Models.JobInfo, callback?: () => void) {
            sendDelete(Api.BASE_URL + "job/" + job.id, this.auth, callback, () => fail(null));
        }

        public tokenise(text: string, callback: (results: Models.SimpleToken[]) => void) {
            var url = Api.BASE_URL + "job/tokenise";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.SimpleToken[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public partsOfSpeech(text: string, callback: (results: Models.Sentence[]) => void) {
            var url = Api.BASE_URL + "job/pos";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.Sentence[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public topicDetection(text: string, callback: (results: Models.TopicDetectionResults[]) => void) {
            var url = Api.BASE_URL + "job/topicdetection";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.TopicDetectionResults[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public embedding(text: string, callback: (results: Models.WordEmbeddingResults[]) => void) {
            var url = Api.BASE_URL + "job/embedding";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.WordEmbeddingResults[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public parseSyntax(text: string, callback: (results: Models.Token[]) => void) {
            var url = Api.BASE_URL + "job/syntax";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.Token[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public analyseSentiment(text: string, callback: (results: Models.Sentence[]) => void) {
            var url = Api.BASE_URL + "job/sentiment";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.Sentence[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        public analyseSemantics(text: string, callback: (results: Models.Token[]) => void) {
            var url = Api.BASE_URL + "job/semantic";
            postText(url, this.auth, text, (job: Models.JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: Models.Token[]) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        private _checkJobStatus(job: Models.JobInfo, onComplete: () => void) {
            this.getJobStatus(job, result => {
                if (result === "complete")
                    onComplete();
                else
                    this._queuePoll(job, onComplete);
            });
        }

        private _queuePoll(job: Models.JobInfo, onComplete: () => void) {
            setTimeout(() => this._checkJobStatus(job, onComplete), Api.POLL_INTERVAL_MS);
        }
    }
}