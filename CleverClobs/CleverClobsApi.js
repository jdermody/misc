var CleverClobs;
(function (CleverClobs) {
    function getJson(url, auth, callback, onError) {
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
    function postText(url, auth, obj, callback, onError) {
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
    function sendDelete(url, auth, callback, onError) {
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
    function fail(callback) {
        console.log("There was a problem calling the ICBLD API");
        callback && callback(null);
    }
    var Api = (function () {
        function Api(auth) {
            this.auth = auth;
        }
        Api.prototype.wordnetSearch = function (query, callback) {
            if (query && query.length) {
                getJson(Api.BASE_URL + "wordnet/search?q=" + encodeURIComponent(query), this.auth, callback, function () { return fail(callback); });
            }
            else
                callback(null);
        };
        Api.prototype.get = function (sid, callback) {
            getJson(Api.BASE_URL + "wordnet/get/" + sid, this.auth, callback, function () { return fail(callback); });
        };
        Api.prototype.getExpanded = function (sid, callback) {
            getJson(Api.BASE_URL + "wordnet/expanded/" + sid, this.auth, callback, function () { return fail(callback); });
        };
        Api.prototype.getJobs = function (callback) {
            getJson(Api.BASE_URL + "job", this.auth, callback, function () { return fail(callback); });
        };
        Api.prototype.getJobStatus = function (job, callback) {
            getJson(Api.BASE_URL + "job/" + job.id + "/status", this.auth, callback, function () { return fail(callback); });
        };
        Api.prototype.deleteJob = function (job, callback) {
            sendDelete(Api.BASE_URL + "job/" + job.id, this.auth, callback, function () { return fail(null); });
        };
        Api.prototype.tokenise = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/tokenise";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.partsOfSpeech = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/pos";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.topicDetection = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/topicdetection";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.embedding = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/embedding";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.parseSyntax = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/syntax";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.analyseSentiment = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/sentiment";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype.analyseSemantics = function (text, callback) {
            var _this = this;
            var url = Api.BASE_URL + "job/semantic";
            postText(url, this.auth, text, function (job) { return _this._checkJobStatus(job, function () {
                getJson(url + "/" + job.id, _this.auth, function (results) { return callback(results); }, function () { return fail(callback); });
            }); }, function () { return fail(callback); });
        };
        Api.prototype._checkJobStatus = function (job, onComplete) {
            var _this = this;
            this.getJobStatus(job, function (result) {
                if (result === "complete")
                    onComplete();
                else
                    _this._queuePoll(job, onComplete);
            });
        };
        Api.prototype._queuePoll = function (job, onComplete) {
            var _this = this;
            setTimeout(function () { return _this._checkJobStatus(job, onComplete); }, Api.POLL_INTERVAL_MS);
        };
        Api.BASE_URL = "https://www.icbld.com/api/";
        Api.POLL_INTERVAL_MS = 500;
        return Api;
    })();
    CleverClobs.Api = Api;
})(CleverClobs || (CleverClobs = {}));
