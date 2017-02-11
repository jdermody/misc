module Icbld {
    function getJson<T>(url: string, auth: string, callback: (obj: T) => void, onError: () => void) {
        var request = new XMLHttpRequest();
        request.setRequestHeader("Authorization", "Basic " + auth);
        request.open("GET", url, true);

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
    function postJson<T>(url: string, auth: string, obj: any, callback: (obj: T) => void, onError: () => void) {
        var request = new XMLHttpRequest();
        request.setRequestHeader("Authorization", "Basic " + auth);
        request.open("POST", url, true);

        request.onload = function () {
            if (request.status >= 200 && request.status < 400)
                callback(JSON.parse(request.response));
            else
                onError();
        };

        request.onerror = function () {
            onError();
        };

        request.send(JSON.stringify(obj));
    }
    function deleteJson(url: string, auth: string, callback: () => void, onError: () => void) {
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

    function fail(callback: (obj:any) => void) {
        console.log("There was a problem calling the ICBLD API");
        callback && callback(null);
    }

    export class Api {
        static BASE_URL = "https://www.icbld.com/api/";
        static POLL_INTERVAL_MS = 500;

        constructor(private auth: string) {
        }

        public wordnetSearch(query: string, callback: (result: WordnetSearchResults) => void) {
            if (query && query.length) {
                getJson(Api.BASE_URL + "wordnet/search?q=" + encodeURIComponent(query), this.auth, callback, () => fail(callback));
            } else
                callback(null);
        }

        public get(sid: number, callback: (result: SenseIndex) => void) {
            getJson(Api.BASE_URL + "wordnet/get/" + sid, this.auth, callback, () => fail(callback));
        }

        public getExpanded(sid: number, callback: (result: ExpandedSenseIndex) => void) {
            getJson(Api.BASE_URL + "wordnet/expanded/" + sid, this.auth, callback, () => fail(callback));
        }

        public getJobs(callback: (result: JobInfo[]) => void) {
            getJson(Api.BASE_URL + "job/all", this.auth, callback, () => fail(callback));
        }

        public getJobStatus(job: JobInfo, callback: (result: string) => void) {
            getJson(Api.BASE_URL + "job/status/" + job.id, this.auth, callback, () => fail(callback));
        }

        public deleteJob(job: JobInfo, callback?: () => void) {
            deleteJson(Api.BASE_URL + "job/" + job.id, this.auth, callback, () => fail(null));
        }

        public tokenise(text: string, callback: (results: TokenResults) => void) {
            var url = Api.BASE_URL + "job/tokenise";
            postJson(url, this.auth, text, (job: JobInfo) => this._checkJobStatus(job, () => {
                getJson(url + "/" + job.id, this.auth, (results: TokenResults) => callback(results), () => fail(callback));
            }), () => fail(callback));
        }

        private _checkJobStatus(job: JobInfo, onComplete: () => void) {
            this.getJobStatus(job, result => {
                if (result === "complete")
                    onComplete();
                else
                    this._queuePoll(job, onComplete);
            });
        }

        private _queuePoll(job: JobInfo, onComplete: () => void) {
            setTimeout(() => this._checkJobStatus(job, onComplete), Api.POLL_INTERVAL_MS);
        }
    }
}