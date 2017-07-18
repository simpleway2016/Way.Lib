var JScriptInvoker = (function () {
    function JScriptInvoker(_url) {
        this.async = true;
        if (_url) {
            this.url = _url;
        }
        else {
            this.url = window.location.href;
        }
    }
    JScriptInvoker.prototype.abort = function () {
        if (this.xmlHttp) {
            this.xmlHttp.abort();
        }
    };
    JScriptInvoker.prototype.setTimeout = function (millseconds) {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        this.xmlHttp.timeout = millseconds;
    };
    JScriptInvoker.prototype.Post = function (obj) {
        var _this = this;
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        if (this.onBeforeInvoke)
            this.onBeforeInvoke();
        this.xmlHttp.onreadystatechange = function () { return _this.xmlHttpStatusChanged(); };
        this.xmlHttp.onerror = function (e) {
            if (_this.onInvokeFinish)
                _this.onInvokeFinish();
            if (_this.onCompleted) {
                _this.onCompleted(null, "无法连接服务器");
            }
        };
        this.xmlHttp.ontimeout = function () {
            if (_this.onInvokeFinish)
                _this.onInvokeFinish();
            if (_this.onCompleted) {
                _this.onCompleted(null, "连接服务器超时");
            }
        };
        this.xmlHttp.open("POST", this.url, this.async);
        this.xmlHttp.setRequestHeader("Content-Type", "application/json");
        this.xmlHttp.send(JSON.stringify(obj));
    };
    JScriptInvoker.prototype.Get = function (nameAndValues) {
        var _this = this;
        if (nameAndValues === void 0) { nameAndValues = null; }
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        if (this.onBeforeInvoke)
            this.onBeforeInvoke();
        this.xmlHttp.onreadystatechange = function () { return _this.xmlHttpStatusChanged(); };
        this.xmlHttp.onerror = function (e) {
            if (_this.onInvokeFinish)
                _this.onInvokeFinish();
            if (_this.onCompleted) {
                _this.onCompleted(null, "无法连接服务器");
            }
        };
        this.xmlHttp.ontimeout = function () {
            if (_this.onInvokeFinish)
                _this.onInvokeFinish();
            if (_this.onCompleted) {
                _this.onCompleted(null, "连接服务器超时");
            }
        };
        var p = "";
        if (nameAndValues) {
            for (var i = 0; i < nameAndValues.length; i += 2) {
                if (i > 0)
                    p += "&";
                p += nameAndValues[i] + "=" + window.encodeURIComponent(nameAndValues[i + 1], "utf-8");
            }
        }
        var myurl = this.url;
        if (nameAndValues && nameAndValues.length > 0) {
            if (myurl.indexOf("?") < 0)
                myurl += "?";
            else
                myurl += "&";
        }
        myurl += p;
        this.xmlHttp.open("GET", myurl, this.async);
        this.xmlHttp.send(null);
    };
    JScriptInvoker.prototype.xmlHttpStatusChanged = function () {
        if (this.xmlHttp.readyState == 4) {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.xmlHttp.status == 200) {
                if (this.onCompleted) {
                    this.onCompleted(this.xmlHttp.responseText, null);
                }
            }
        }
    };
    JScriptInvoker.prototype.createXMLHttp = function () {
        var request = false;
        if (window.XMLHttpRequest) {
            request = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            try {
                request = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e1) {
                try {
                    request = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e2) {
                    request = false;
                }
            }
        }
        return request;
    };
    return JScriptInvoker;
}());
var JHttpHelper = (function () {
    function JHttpHelper() {
    }
    JHttpHelper.writePage = function (url) {
        document.write(JHttpHelper.downloadUrl(url));
    };
    JHttpHelper.downloadUrl = function (url) {
        var invoker = new JScriptInvoker(url);
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = function (ret, err) {
            if (err) {
                errcount++;
                if (errcount <= 1) {
                    invoker.Get();
                }
                else {
                    throw "无法打开网页：" + url;
                }
            }
            else {
                result = ret;
            }
        };
        invoker.Get();
        return result;
    };
    return JHttpHelper;
}());
//# sourceMappingURL=JScriptInvoker.js.map