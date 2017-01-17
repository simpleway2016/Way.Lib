var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
window.onerror = function (errorMessage, scriptURI, lineNumber) {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
};
if (true) {
    try {
        var obj = {};
        Object.defineProperty(obj, "test", {
            get: function () {
                return null;
            },
            set: function (value) {
            },
            enumerable: true,
            configurable: true
        });
    }
    catch (e) {
        throw "浏览器不支持defineProperty";
    }
}
var WayScriptRemotingMessageType;
(function (WayScriptRemotingMessageType) {
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["Result"] = 1] = "Result";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["Notify"] = 2] = "Notify";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["SendSessionID"] = 3] = "SendSessionID";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["InvokeError"] = 4] = "InvokeError";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["UploadFileBegined"] = 5] = "UploadFileBegined";
})(WayScriptRemotingMessageType || (WayScriptRemotingMessageType = {}));
var WayCookie = (function () {
    function WayCookie() {
    }
    WayCookie.setCookie = function (name, value) {
        document.cookie = name + "=" + window.encodeURIComponent(value, "utf-8");
    };
    WayCookie.getCookie = function (name) {
        try {
            var cookieStr = document.cookie;
            if (cookieStr.length > 0) {
                var cookieArr = cookieStr.split(";"); //将cookie信息转换成数组
                for (var i = 0; i < cookieArr.length; i++) {
                    var cookieVal = cookieArr[i].split("="); //将每一组cookie(cookie名和值)也转换成数组
                    if (cookieVal[0].trim() == name) {
                        var v = cookieVal[1].trim();
                        if (v != "") {
                            return window.decodeURIComponent(v, "utf-8"); //返回需要提取的cookie值
                        }
                    }
                }
            }
        }
        catch (e) {
        }
        return "";
    };
    return WayCookie;
}());
var WayBaseObject = (function () {
    function WayBaseObject() {
    }
    return WayBaseObject;
}());
var WayScriptRemotingUploadHandler = (function () {
    function WayScriptRemotingUploadHandler() {
        this.abort = false;
        this.offset = 0;
    }
    return WayScriptRemotingUploadHandler;
}());
var WayScriptRemoting = (function (_super) {
    __extends(WayScriptRemoting, _super);
    function WayScriptRemoting(remoteName) {
        _super.call(this);
        this.mDoConnected = false;
        this.classFullName = remoteName;
        WayScriptRemoting.getServerAddress();
    }
    Object.defineProperty(WayScriptRemoting.prototype, "groupName", {
        get: function () {
            return this.groupName;
        },
        set: function (value) {
            this._groupName = value;
            if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
                this.mDoConnected = true;
                this.connect();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayScriptRemoting.prototype, "onmessage", {
        //长连接接收到信息触发
        get: function () {
            return this._onmessage;
        },
        set: function (func) {
            this._onmessage = func;
            if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
                this.mDoConnected = true;
                this.connect();
            }
        },
        enumerable: true,
        configurable: true
    });
    WayScriptRemoting.getServerAddress = function () {
        if (WayScriptRemoting.ServerAddress == null) {
            var host, port;
            var href = location.href;
            var index = href.indexOf("://");
            href = href.substr(index + 3);
            var index = href.indexOf("/");
            if (index > 0) {
                href = href.substr(0, index);
            }
            href = href.split(':');
            host = href[0];
            if (href.length > 1) {
                port = href[1];
                WayScriptRemoting.ServerAddress = host + ":" + port;
            }
            else {
                WayScriptRemoting.ServerAddress = host;
            }
        }
    };
    WayScriptRemoting.createRemotingController = function (remoteName) {
        if (!remoteName) {
            return new WayScriptRemoting(null);
        }
        for (var i = 0; i < WayScriptRemoting.ExistControllers.length; i++) {
            if (WayScriptRemoting.ExistControllers[i].classFullName == remoteName)
                return WayScriptRemoting.ExistControllers[i];
        }
        WayScriptRemoting.getServerAddress();
        var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
        invoker.async = false;
        var result;
        var hasErr = null;
        invoker.onCompleted = function (ret, err) {
            if (err) {
                hasErr = err;
            }
            else {
                eval("result=" + ret);
            }
        };
        invoker.invoke(["m", "{'Action':'init' , 'ClassFullName':'" + remoteName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}"]);
        if (hasErr) {
            throw hasErr;
        }
        else if (result.err) {
            throw result.err;
        }
        var func;
        eval("func = " + result.text);
        var page = new func(remoteName);
        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID);
        return page;
    };
    WayScriptRemoting.createRemotingControllerAsync = function (remoteName, callback) {
        WayScriptRemoting.getServerAddress();
        var ws = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        ws.onopen = function () {
            ws.send("{'Action':'init' , 'ClassFullName':'" + remoteName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        ws.onmessage = function (evt) {
            ws.onerror = null; //必须把它设置为null，否则关闭时，会触发onerror
            ws.send("{'Action':'exit'}");
            var result;
            eval("result=" + evt.data);
            if (result.err) {
                callback(null, result.err);
            }
            else {
                try {
                    var func;
                    eval("func = " + result.text);
                    var page = new func(remoteName);
                    WayCookie.setCookie("WayScriptRemoting", result.SessionID);
                    callback(page, null);
                }
                catch (e) {
                    callback(null, e.message);
                }
            }
        };
        ws.onerror = function (evt) {
            callback(null, "无法连接服务器");
        };
    };
    WayScriptRemoting.prototype.sendFile = function (ws, file, reader, size, start, len, callback, handler) {
        var _this = this;
        if (ws.binaryType != "arraybuffer")
            return;
        if (start + len > size) {
            len = size - start;
        }
        var blob = file.slice(start, start + len);
        reader.onload = function () {
            var filedata = reader.result;
            if (filedata.byteLength > 0) {
                start += filedata.byteLength;
                if (ws.binaryType != "arraybuffer")
                    return;
                if (ws.readyState == WebSocket.CLOSED || handler.abort) {
                    if (ws.readyState != WebSocket.CLOSED) {
                        ws.close();
                    }
                }
                else {
                    try {
                        ws.send(filedata);
                        if (start < size) {
                            _this.sendFile(ws, file, reader, size, start, len, callback, handler);
                        }
                    }
                    catch (e) {
                    }
                }
            }
        };
        reader.onerror = function () {
            if (callback) {
                try {
                    callback("", null, null, "读取文件发生错误");
                }
                catch (e) { }
            }
        };
        reader.readAsArrayBuffer(blob);
    };
    WayScriptRemoting.prototype.uploadFile = function (fileElement, callback, handler) {
        var _this = this;
        try {
            var file;
            if (typeof fileElement == "string") {
                fileElement = document.getElementById(fileElement);
            }
            if (fileElement.files) {
                file = fileElement.files[0];
            }
            else {
                file = fileElement;
            }
            var reader = new FileReader();
            var size = file.size;
            var errored = false;
            var finished = false;
            if (!handler) {
                handler = new WayScriptRemotingUploadHandler();
            }
            var ws = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
            var initType = ws.binaryType;
            ws.onopen = function () {
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "','FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + _this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
            };
            ws.onmessage = function (evt) {
                var resultObj;
                eval("resultObj=" + evt.data);
                if (resultObj.type == WayScriptRemotingMessageType.UploadFileBegined) {
                    ws.binaryType = "arraybuffer";
                    _this.sendFile(ws, file, reader, size, handler.offset, 102400, callback, handler);
                }
                else if (resultObj.type == WayScriptRemotingMessageType.Result) {
                    if (errored)
                        return;
                    if (resultObj.result == "ok") {
                        finished = true;
                        ws.binaryType = initType;
                        ws.send("{'Action':'exit'}");
                        if (callback) {
                            callback("ok", size, size, false);
                        }
                    }
                    else {
                        //计算服务器接收进度
                        if (callback) {
                            handler.offset = parseInt(resultObj.result);
                            try {
                                callback("", size, parseInt(resultObj.result), false);
                            }
                            catch (e) { }
                        }
                    }
                }
                else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                    ws.binaryType = initType;
                    errored = true;
                    if (callback) {
                        try {
                            callback(null, null, null, resultObj.result);
                        }
                        catch (e) { }
                    }
                    ws.close();
                }
            };
            ws.onclose = function () {
                ws.onerror = null;
                if (!finished) {
                    if (handler.abort == false) {
                        _this.uploadFile(file, callback, handler);
                    }
                }
            };
            ws.onerror = function () {
                ws.onclose = null;
                if (handler.offset == 0) {
                    if (callback && !finished) {
                        try {
                            callback(null, null, null, "网络错误");
                        }
                        catch (e) { }
                    }
                }
                else {
                    if (!finished) {
                        //续传
                        if (handler.abort == false) {
                            _this.uploadFile(file, callback, handler);
                        }
                    }
                }
            };
            return handler;
        }
        catch (e) {
            if (callback) {
                try {
                    callback(null, null, null, e.message);
                }
                catch (e) { }
            }
        }
    };
    WayScriptRemoting.prototype.pageInvoke = function (name, parameters, callback, async) {
        if (async === void 0) { async = true; }
        try {
            if (WayScriptRemoting.onBeforeInvoke) {
                WayScriptRemoting.onBeforeInvoke(name, parameters);
            }
            var paramerStr = "";
            if (parameters) {
                parameters.forEach(function (p) {
                    if (paramerStr.length > 0)
                        paramerStr += ",";
                    var itemstr = JSON.stringify(p);
                    paramerStr += JSON.stringify(itemstr);
                });
            }
            var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
            invoker.async = async;
            invoker.onCompleted = function (ret, err) {
                if (WayScriptRemoting.onInvokeFinish) {
                    WayScriptRemoting.onInvokeFinish(name, parameters);
                }
                if (err) {
                    callback(null, err);
                }
                else {
                    var resultObj;
                    eval("resultObj=" + ret);
                    if (resultObj.type == WayScriptRemotingMessageType.Result) {
                        callback(resultObj.result, null);
                    }
                    else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                        callback(null, resultObj.result);
                    }
                }
            };
            invoker.invoke(["m", "{'ClassFullName':'" + this.classFullName + "','MethodName':'" + name + "','Parameters':[" + paramerStr + "] , 'SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}"]);
        }
        catch (e) {
            callback(null, e.message);
        }
    };
    WayScriptRemoting.prototype.connect = function () {
        var _this = this;
        this.socket = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        this.socket.onopen = function () {
            try {
                if (_this.onconnect) {
                    _this.onconnect();
                }
            }
            catch (e) {
            }
            _this.socket.send("{'GroupName':'" + _this._groupName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        this.socket.onmessage = function (evt) {
            var resultObj;
            eval("resultObj=" + evt.data);
            if (_this._onmessage) {
                _this._onmessage(resultObj.msg);
            }
        };
        this.socket.onclose = function (evt) {
            _this.socket.onerror = null;
            try {
                if (_this.onerror) {
                    _this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(function () { _this.connect(); }, 1000);
        };
        this.socket.onerror = function (evt) {
            _this.socket.onclose = null;
            try {
                if (_this.onerror) {
                    _this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(function () { _this.connect(); }, 1000);
        };
    };
    WayScriptRemoting.ServerAddress = null; //"localhost:9090";
    WayScriptRemoting.ExistControllers = [];
    return WayScriptRemoting;
}(WayBaseObject));
var WayScriptRemotingChild = (function (_super) {
    __extends(WayScriptRemotingChild, _super);
    function WayScriptRemotingChild() {
        _super.apply(this, arguments);
    }
    return WayScriptRemotingChild;
}(WayScriptRemoting));
var WayVirtualWebSocketStatus;
(function (WayVirtualWebSocketStatus) {
    WayVirtualWebSocketStatus[WayVirtualWebSocketStatus["none"] = 0] = "none";
    WayVirtualWebSocketStatus[WayVirtualWebSocketStatus["connected"] = 1] = "connected";
    WayVirtualWebSocketStatus[WayVirtualWebSocketStatus["error"] = 2] = "error";
    WayVirtualWebSocketStatus[WayVirtualWebSocketStatus["closed"] = 3] = "closed";
})(WayVirtualWebSocketStatus || (WayVirtualWebSocketStatus = {}));
var WayVirtualWebSocket = (function () {
    function WayVirtualWebSocket(_url) {
        this.status = WayVirtualWebSocketStatus.none;
        this.sendQueue = [];
        this.binaryType = "string";
        var exp = /http[s]?\:\/\/[\w|\:]+[\/]?/;
        var httpstr = exp.exec(window.location.href)[0];
        var exp2 = /ws\:\/\/[\w|\:]+[\/]?/;
        var wsstr = exp2.exec(_url)[0];
        this.url = _url.replace(wsstr, httpstr);
        if (this.url.indexOf("?") > 0) {
            this.url += "&";
        }
        else {
            this.url += "?";
        }
        this.url += "WayVirtualWebSocket=1";
        this.init();
    }
    Object.defineProperty(WayVirtualWebSocket.prototype, "onopen", {
        get: function () {
            return this._onopen;
        },
        set: function (value) {
            this._onopen = value;
            if (this.status == WayVirtualWebSocketStatus.connected) {
                if (this._onopen) {
                    this._onopen({});
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayVirtualWebSocket.prototype, "onmessage", {
        get: function () {
            return this._onmessage;
        },
        set: function (value) {
            this._onmessage = value;
            if (this.status == WayVirtualWebSocketStatus.connected) {
                if (this._onmessage) {
                    this._onmessage({ data: this.lastMessage });
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayVirtualWebSocket.prototype, "onclose", {
        get: function () {
            return this._onclose;
        },
        set: function (value) {
            this._onclose = value;
            if (this.status == WayVirtualWebSocketStatus.closed) {
                if (this._onclose) {
                    this._onclose({});
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayVirtualWebSocket.prototype, "onerror", {
        get: function () {
            return this._onerror;
        },
        set: function (value) {
            this._onerror = value;
            if (this.status == WayVirtualWebSocketStatus.error) {
                if (this._onerror) {
                    this._onerror({});
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    WayVirtualWebSocket.prototype.close = function () {
        this.status = WayVirtualWebSocketStatus.closed;
        this.receiver.abort();
        if (this._onclose) {
            this._onclose({});
        }
    };
    WayVirtualWebSocket.prototype.init = function () {
        var _this = this;
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = function (result, err) {
            if (err) {
                _this.status = WayVirtualWebSocketStatus.error;
                _this.errMsg = err;
                if (_this._onerror) {
                    _this._onerror({ data: _this.errMsg });
                }
            }
            else {
                _this.guid = result;
                _this.status = WayVirtualWebSocketStatus.connected;
                if (_this._onopen) {
                    _this._onopen({});
                }
                _this.receiveChannelConnect();
            }
        };
        invoker.invoke(["mode", "init"]);
    };
    WayVirtualWebSocket.prototype.send = function (data) {
        var _this = this;
        if (this.sendQueue.length > 0) {
            this.sendQueue.push(data);
            return;
        }
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = function (result, err) {
            if (err) {
                _this.status = WayVirtualWebSocketStatus.error;
                _this.errMsg = err;
                if (_this._onerror) {
                    _this._onerror({ data: _this.errMsg });
                }
                _this.sendQueue = [];
            }
            else {
                _this.sendQueue.pop();
            }
        };
        if (this.binaryType == "arraybuffer") {
            data = this.arrayBufferToString(data);
        }
        invoker.invoke(["mode", "send", "data", data, "id", this.guid, "binaryType", this.binaryType]);
    };
    WayVirtualWebSocket.prototype.arrayBufferToString = function (data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }
        return str;
    };
    WayVirtualWebSocket.prototype.receiveChannelConnect = function () {
        var _this = this;
        this.receiver = new WayScriptInvoker(this.url);
        this.receiver.setTimeout(0);
        this.receiver.onCompleted = function (result, err) {
            if (err) {
                _this.status = WayVirtualWebSocketStatus.error;
                _this.errMsg = err;
                if (_this._onerror) {
                    _this._onerror({ data: _this.errMsg });
                }
            }
            else {
                //if (this.binaryType == "arraybuffer") {
                //    var arr = result.split('%');
                //    result = new ArrayBuffer(arr.length - 1);
                //    var intArr = new Uint8Array(result);
                //    for (var i = 1; i < arr.length; i++) {
                //        intArr[i - 1] = parseInt(arr[i] , 16);
                //    }
                //}
                _this.lastMessage = result;
                if (_this._onmessage && _this.status == WayVirtualWebSocketStatus.connected) {
                    _this._onmessage({ data: _this.lastMessage });
                }
                if (_this.status == WayVirtualWebSocketStatus.connected) {
                    _this.receiveChannelConnect();
                }
            }
        };
        this.receiver.invoke(["mode", "receive", "id", this.guid, "binaryType", this.binaryType]);
        setTimeout(function () { return _this.sendHeart(); }, 30000);
    };
    WayVirtualWebSocket.prototype.sendHeart = function () {
        var _this = this;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            var invoker = new WayScriptInvoker(this.url);
            invoker.invoke(["mode", "heart", "id", this.guid]);
            setTimeout(function () { return _this.sendHeart(); }, 30000);
        }
    };
    return WayVirtualWebSocket;
}());
var WayScriptInvoker = (function () {
    function WayScriptInvoker(_url) {
        this.async = true;
        this.method = "POST";
        if (_url) {
            this.url = _url;
        }
        else {
            this.url = window.location.href;
        }
    }
    WayScriptInvoker.prototype.abort = function () {
        if (this.xmlHttp) {
            this.xmlHttp.abort();
        }
    };
    WayScriptInvoker.prototype.setTimeout = function (millseconds) {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        this.xmlHttp.timeout = millseconds;
    };
    WayScriptInvoker.prototype.invoke = function (nameAndValues) {
        var _this = this;
        /*
        escape不编码字符有69个：*，+，-，.，/，@，_，0-9，a-z，A-Z

encodeURI不编码字符有82个：!，#，$，&，'，(，)，*，+，,，-，.，/，:，;，=，?，@，_，~，0-9，a-z，A-Z

encodeURIComponent不编码字符有71个：!， '，(，)，*，-，.，_，~，0-9，a-z，A-Z
        */
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        var p = "";
        if (nameAndValues) {
            for (var i = 0; i < nameAndValues.length; i += 2) {
                if (i > 0)
                    p += "&";
                p += nameAndValues[i] + "=" + window.encodeURIComponent(nameAndValues[i + 1], "utf-8");
            }
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
        if (this.method == "POST") {
            this.xmlHttp.open("POST", this.url, this.async);
            this.xmlHttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            this.xmlHttp.send(p); //null,对ff浏览器是必须的
        }
        else {
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
        }
    };
    WayScriptInvoker.prototype.xmlHttpStatusChanged = function () {
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
    WayScriptInvoker.prototype.createXMLHttp = function () {
        var request = false;
        // Microsoft browsers
        if (window.XMLHttpRequest) {
            request = new XMLHttpRequest();
        }
        else if (window.ActiveXObject) {
            try {
                //Internet Explorer
                request = new ActiveXObject("Msxml2.XMLHTTP");
            }
            catch (e1) {
                try {
                    //Internet Explorer
                    request = new ActiveXObject("Microsoft.XMLHTTP");
                }
                catch (e2) {
                    request = false;
                }
            }
        }
        return request;
    };
    return WayScriptInvoker;
}());
var WayTemplate = (function () {
    function WayTemplate(_content, _match, _mode) {
        if (_match === void 0) { _match = null; }
        if (_mode === void 0) { _mode = ""; }
        this.content = _content;
        this.match = _match;
        this.mode = _mode ? _mode : "";
    }
    return WayTemplate;
}());
var WayHelper = (function () {
    function WayHelper() {
    }
    //判断数组是否包含某个值
    WayHelper.contains = function (arr, value) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value)
                return true;
        }
        return false;
    };
    WayHelper.createWebSocket = function (url) {
        if (window.WebSocket) {
            return new WebSocket(url);
        }
        else {
            return new WayVirtualWebSocket(url);
        }
    };
    WayHelper.writePage = function (url) {
        document.write(WayHelper.downloadUrl(url));
    };
    WayHelper.downloadUrl = function (url) {
        var invoker = new WayScriptInvoker(url);
        invoker.method = "GET";
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = function (ret, err) {
            if (err) {
                errcount++;
                if (errcount <= 1) {
                    invoker.invoke([]);
                }
                else {
                    throw "无法打开网页：" + url;
                }
            }
            else {
                result = ret;
            }
        };
        invoker.invoke([]);
        return result;
    };
    WayHelper.findBindingElements = function (element) {
        var result = [];
        WayHelper.findInnerBindingElements(result, element);
        return result;
    };
    WayHelper.findInnerBindingElements = function (result, element) {
        var attr = element.getAttribute("_databind");
        if (attr && attr.length > 0) {
            result.push(element);
        }
        else {
            attr = element.getAttribute("_expression");
            if (attr && attr.length > 0) {
                result.push(element);
            }
        }
        if (element.tagName.indexOf("Way") == 0 || element._WayControl) {
            return;
        }
        for (var i = 0; i < element.children.length; i++) {
            WayHelper.findInnerBindingElements(result, element.children[i]);
        }
    };
    WayHelper.addEventListener = function (element, eventName, listener, useCapture) {
        if (element.addEventListener) {
            element.addEventListener(eventName, listener, useCapture);
        }
        else {
            element.attachEvent("on" + eventName, listener);
        }
    };
    WayHelper.removeEventListener = function (element, eventName, listener, useCapture) {
        if (element.removeEventListener) {
            element.removeEventListener(eventName, listener, useCapture);
        }
        else {
            element.detachEvent("on" + eventName, listener);
        }
    };
    //触发htmlElement相关事件，如：fireEvent(myDiv , "click");
    WayHelper.fireEvent = function (el, eventName) {
        if (eventName.indexOf("on") == 0)
            eventName = eventName.substr(2);
        var evt;
        if (document.createEvent) {
            evt = document.createEvent("HTMLEvents");
            // 3个参数：事件类型，是否冒泡，是否阻止浏览器的默认行为  
            evt.initEvent(eventName, true, true);
            //evt.initMouseEvent(eventName, true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            el.dispatchEvent(evt);
        }
        else if (el.fireEvent) {
            el.fireEvent('on' + eventName);
        }
    };
    WayHelper.getDataForDiffent = function (originalData, currentData) {
        var result = null;
        for (var p in originalData) {
            var mydata = originalData[p];
            var curdata = currentData[p];
            if (mydata != null && typeof mydata == "object") {
                var dif = WayHelper.getDataForDiffent(mydata, curdata);
                if (dif) {
                    if (!result) {
                        result = {};
                    }
                    eval("result." + p + "=dif;");
                }
            }
            else if (mydata != curdata) {
                if (!result) {
                    result = {};
                }
                eval("result." + p + "=curdata;");
            }
        }
        return result;
    };
    WayHelper.replace = function (content, find, replace) {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
    };
    WayHelper.copyValue = function (target, source) {
        for (var pro in target) {
            var originalvalue = target[pro];
            if (originalvalue != null && typeof originalvalue == "object") {
                WayHelper.copyValue(originalvalue, source[pro]);
            }
            else {
                target[pro] = source[pro];
            }
        }
    };
    WayHelper.clone = function (obj) {
        var o;
        if (obj != null && typeof obj == "object") {
            if (obj === null) {
                o = null;
            }
            else {
                if (obj instanceof Array) {
                    o = [];
                    for (var i = 0, len = obj.length; i < len; i++) {
                        o.push(WayHelper.clone(obj[i]));
                    }
                }
                else {
                    o = {};
                    for (var j in obj) {
                        o[j] = WayHelper.clone(obj[j]);
                    }
                }
            }
        }
        else {
            o = obj;
        }
        return o;
    };
    return WayHelper;
}());
var WayBindMemberConfig = (function () {
    function WayBindMemberConfig(_elementMember, _dataMember, _element) {
        this.elementMember = _elementMember;
        this.dataMember = _dataMember;
        this.element = _element;
    }
    return WayBindMemberConfig;
}());
var WayBindingElement = (function (_super) {
    __extends(WayBindingElement, _super);
    function WayBindingElement(_element, _model, _dataSource, expressionExp, dataMemberExp) {
        _super.call(this);
        this.configs = [];
        this.expressionConfigs = [];
        this.element = _element;
        this.model = _model;
        this.dataSource = _dataSource;
        var elements = WayHelper.findBindingElements(_element);
        for (var i = 0; i < elements.length; i++) {
            var ctrlEle = elements[i];
            this.initEle(ctrlEle, _dataSource, expressionExp, dataMemberExp);
        }
    }
    WayBindingElement.prototype.initEle = function (ctrlEle, _dataSource, expressionExp, dataMemberExp) {
        var _this = this;
        var _databind = ctrlEle.getAttribute("_databind");
        var _expressionString = ctrlEle.getAttribute("_expression");
        var isWayControl = false;
        if (ctrlEle._WayControl) {
            ctrlEle = ctrlEle._WayControl;
            isWayControl = true;
        }
        if (_databind) {
            var matchs = _databind.match(expressionExp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    if (match && match.indexOf("=") > 0) {
                        var eleMember = match.match(/(\w|\.)+( )?\=/g)[0];
                        eleMember = eleMember.match(/(\w|\.)+/g)[0];
                        var dataMember = match.match(dataMemberExp)[0];
                        dataMember = dataMember.substr(1);
                        //检查data.member是否存在
                        if (_dataSource) {
                            var fields = dataMember.split('.');
                            var findingObj = _dataSource;
                            for (var k = 0; k < fields.length; k++) {
                                var field = fields[k];
                                if (field.length == 0)
                                    break;
                                if (k < fields.length - 1) {
                                    var isUndefined = eval("typeof findingObj." + field + "!='object'");
                                    if (isUndefined) {
                                        eval("findingObj." + field + "={};");
                                    }
                                    findingObj = eval("findingObj." + field);
                                }
                                else {
                                    var isUndefined = eval("typeof findingObj." + field + "=='undefined'");
                                    if (isUndefined) {
                                        eval("findingObj." + field + "=null;");
                                    }
                                    findingObj = eval("findingObj." + field);
                                }
                            }
                        }
                        if (_dataSource) {
                            var isObject = eval("_dataSource." + dataMember + " && typeof _dataSource." + dataMember + "=='object' && typeof _dataSource." + dataMember + ".value!='undefined'");
                            if (isObject) {
                                dataMember += ".value";
                            }
                        }
                        var config = new WayBindMemberConfig(eleMember, dataMember, ctrlEle);
                        this.configs.push(config);
                        ctrlEle._data = this.model;
                        if (_dataSource) {
                            var addevent = false;
                            if (ctrlEle.memberInChange && WayHelper.contains(ctrlEle.memberInChange, eleMember))
                                addevent = true;
                            else if (eleMember == "value" || eleMember == "checked")
                                addevent = true;
                            if (addevent) {
                                if (ctrlEle.addEventListener) {
                                    ctrlEle.addEventListener("change", function () { _this.onvalueChanged(config); });
                                }
                                else {
                                    ctrlEle.attachEvent("onchange", function () { _this.onvalueChanged(config); });
                                }
                            }
                        }
                    }
                }
            }
        }
        if (_expressionString) {
            var matchs = _expressionString.match(dataMemberExp);
            if (matchs) {
                var datamembers = [];
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    datamembers.push(match.substr(1));
                }
                if (!ctrlEle.expressionDatas) {
                    ctrlEle.expressionDatas = [];
                }
                ctrlEle.expressionDatas.push({ exp: dataMemberExp, data: _dataSource });
                var config = new WayBindMemberConfig(null, datamembers, ctrlEle);
                config.expressionString = _expressionString;
                config.dataMemberExp = dataMemberExp;
                this.expressionConfigs.push(config);
            }
        }
    };
    WayBindingElement.prototype.doExpression = function (__config) {
        var ___element = __config.element;
        var exp = __config.expressionString;
        var matches = exp.match(/[\W]?(this\.)/g);
        for (var i = 0; i < matches.length; i++) {
            var r = matches[i].replace("this.", "___element.");
            exp = exp.replace(matches[i], r);
        }
        for (var i = 0; i < __config.element.expressionDatas.length; i++) {
            var expItem = __config.element.expressionDatas[i];
            var matchs = exp.match(expItem.exp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    var dataMember = match.substr(1);
                    exp = exp.replace(match, "__config.element.expressionDatas[" + i + "].data." + dataMember);
                }
            }
        }
        eval(exp);
    };
    WayBindingElement.prototype.initEleValues = function (model) {
        this.model = model;
        for (var i = 0; i < this.configs.length; i++) {
            eval("this.configs[i].element." + this.configs[i].elementMember + "=model." + this.configs[i].dataMember + ";");
        }
    };
    WayBindingElement.prototype.onvalueChanged = function (fromWhichConfig) {
        try {
            if (this.configs.length == 0 || !this.model)
                return; //绑定已经移除了
            var model = this.model;
            var value = fromWhichConfig.element[fromWhichConfig.elementMember];
            eval("model." + fromWhichConfig.dataMember + "=value;");
        }
        catch (e) {
            throw "WayBindingElement onvalueChanged error:" + e.message;
        }
    };
    WayBindingElement.prototype.getDataMembers = function () {
        var result = [];
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            result.push(config.dataMember);
        }
        return result;
    };
    WayBindingElement.prototype.onchange = function (itemIndex, name, value) {
        try {
            for (var i = 0; i < this.configs.length; i++) {
                var config = this.configs[i];
                if (config.dataMember == name) {
                    if (eval("config.element." + config.elementMember + "!=value")) {
                        eval("config.element." + config.elementMember + "=value");
                        if (!config.element.element)
                            WayHelper.fireEvent(config.element, 'change');
                    }
                }
            }
            for (var i = 0; i < this.expressionConfigs.length; i++) {
                var config = this.expressionConfigs[i];
                if (WayHelper.contains(config.dataMember, name)) {
                    this.doExpression(config);
                }
            }
        }
        catch (e) {
        }
    };
    return WayBindingElement;
}(WayBaseObject));
var WayDataBindHelper = (function () {
    function WayDataBindHelper() {
    }
    WayDataBindHelper.getObjectStr = function (obj, onchangeMembers, parent) {
        var getmodelStr;
        if (!parent) {
            parent = "";
            getmodelStr = "this";
        }
        else {
            getmodelStr = "this.getModel()";
        }
        if (obj != null && typeof obj == "object") {
            var str = "{";
            if (parent == "") {
                str += "getSource:function(){return item;}";
            }
            else {
                str += "getModel:function(){return null;}";
            }
            var index = 0;
            for (var pro in obj) {
                var pvalue = obj[pro];
                if (typeof pvalue == "function") {
                    continue;
                }
                str += ",";
                if (pvalue != null && typeof pvalue == "object" && !(pvalue instanceof Array)) {
                    str += pro + ":" + WayDataBindHelper.getObjectStr(pvalue, onchangeMembers, parent + pro + ".");
                    continue;
                }
                var onchangeStr;
                if (WayHelper.contains(onchangeMembers, parent + pro)) {
                    onchangeStr = "onProChange(true," + getmodelStr + ",item,itemIndex,'" + parent + pro + "',v);";
                }
                else {
                    //onchangeStr = "onProChange(false," + getmodelStr + ",item,itemIndex,'" + parent + pro + "',v);";
                    onchangeStr = "onProChange(true," + getmodelStr + ",item,itemIndex,'" + parent + pro + "',v);";
                }
                str += "get " + pro + "(){return item." + parent + pro + ";},";
                str += "set " + pro + "(v){if(item." + parent + pro + "!=v){item." + parent + pro + "=v;" + onchangeStr + "}}";
                index++;
            }
            str += "}";
            return str;
        }
        else {
            return JSON.stringify(obj);
        }
    };
    WayDataBindHelper.setSubObjectModel = function (model, rootModel) {
        for (var p in model) {
            var value = model[p];
            if (value != null && typeof value == "object" && typeof value.getModel == "function") {
                var obj = model[p];
                obj.getModel = function () {
                    return rootModel;
                };
                WayDataBindHelper.setSubObjectModel(obj, rootModel);
            }
        }
    };
    WayDataBindHelper.addPropertyToObject = function (model, obj, source, _itemIndex, propertyName, fullMemberName, _onchange) {
        var member = propertyName.split('.')[0];
        //var prototype = Object.getPrototypeOf(obj);
        if (eval("typeof obj." + member + " == \"undefined\"")) {
            if (member == propertyName) {
                //直接defineProperty (obj) 即可，不要defineProperty (prototype)，用prototype，jquery会报错（其原因不解）
                Object.defineProperty(obj, member, {
                    get: function () {
                        return eval("source." + fullMemberName);
                    },
                    set: function (value) {
                        if (eval("source." + fullMemberName + "!=value")) {
                            eval("source." + fullMemberName + "=value");
                            _onchange(true, model, _itemIndex, source, fullMemberName, value);
                        }
                    },
                    enumerable: true,
                    configurable: true
                });
                return;
            }
            else {
                obj[member] = {};
            }
        }
        if (member != propertyName) {
            propertyName = propertyName.substr(member.length + 1);
            WayDataBindHelper.addPropertyToObject(model, obj[member], source, _itemIndex, propertyName, fullMemberName, _onchange);
        }
    };
    WayDataBindHelper.cloneObjectForBind = function (obj, _itemIndex, onchangeMembers, _onchange) {
        if (obj.getSource && typeof obj.getSource == "function") {
            //需要增加没有的属性
            var prototype = Object.getPrototypeOf(obj);
            for (var i = 0; i < onchangeMembers.length; i++) {
                WayDataBindHelper.addPropertyToObject(obj, obj, obj.getSource(), _itemIndex, onchangeMembers[i], onchangeMembers[i], _onchange);
            }
            return obj;
        }
        var str = WayDataBindHelper.getObjectStr(obj, onchangeMembers, null);
        str = "result=(function(item,itemIndex,onProChange){ return " + str + ";})(obj,_itemIndex,_onchange);";
        var result;
        eval(str);
        WayDataBindHelper.setSubObjectModel(result, result);
        return result;
    };
    WayDataBindHelper.onchange = function (toCheckedEles, model, dataSource, itemIndex, name, value) {
        if (typeof model.onchange == "function") {
            model.onchange(model, itemIndex, name, value);
        }
        else if (model.onchange && typeof model.onchange.length != "undefined") {
            for (var i = 0; i < model.onchange.length; i++) {
                model.onchange[i](model, itemIndex, name, value);
            }
        }
        if (toCheckedEles) {
            for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
                var binding = WayDataBindHelper.bindings[i];
                if (binding && binding.model == model) {
                    binding.onchange(itemIndex, name, value);
                }
            }
        }
    };
    WayDataBindHelper.removeDataBind = function (element) {
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] != null && WayDataBindHelper.bindings[i].element == element) {
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
            }
        }
    };
    //获取htmlElement里面所有用于绑定的字段名称
    WayDataBindHelper.getBindingFields = function (element, expressionExp, dataMemberExp) {
        if (expressionExp === void 0) { expressionExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g; }
        if (dataMemberExp === void 0) { dataMemberExp = /\@(\w|\.)+/g; }
        if (typeof element == "string") {
            element = document.getElementById(element);
        }
        var bindingInfo = new WayBindingElement(element, null, null, expressionExp, dataMemberExp);
        var onchangeMembers = bindingInfo.getDataMembers();
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] == bindingInfo) {
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
                break;
            }
        }
        return onchangeMembers;
    };
    //替换html里的变量
    WayDataBindHelper.replaceHtmlFields = function (templateHtml, data) {
        var expression = /\{\@([\w|\.]+)\}/g;
        var html = templateHtml;
        while (true) {
            var r = expression.exec(templateHtml);
            if (!r)
                break;
            try {
                html = html.replace(r[0], eval("data." + r[1]));
            }
            catch (e) {
                html = html.replace(r[0], "");
            }
        }
        return html;
    };
    WayDataBindHelper.dataBind = function (element, data, tag, expressionExp, dataMemberExp, doexpression) {
        if (tag === void 0) { tag = null; }
        if (expressionExp === void 0) { expressionExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g; }
        if (dataMemberExp === void 0) { dataMemberExp = /\@(\w|\.)+/g; }
        if (doexpression === void 0) { doexpression = false; }
        if (typeof element == "string") {
            element = document.getElementById(element);
        }
        else if (element.element && element.element.length) {
            //is jquery
            element = element.element[0];
        }
        var model = null;
        if (!data)
            data = {};
        else if (data.getSource && typeof data.getSource == "function") {
            model = data;
            data = model.getSource();
        }
        var bindingInfo = new WayBindingElement(element, null, data, expressionExp, dataMemberExp);
        var onchangeMembers = bindingInfo.getDataMembers();
        model = WayDataBindHelper.cloneObjectForBind(model ? model : data, tag, onchangeMembers, WayDataBindHelper.onchange);
        var finded = false;
        bindingInfo.initEleValues(model);
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] == null) {
                finded = true;
                WayDataBindHelper.bindings[i] = bindingInfo;
                break;
            }
        }
        if (!finded) {
            WayDataBindHelper.bindings.push(bindingInfo);
        }
        if (doexpression) {
            //_expression有可能包含$name @name两种变量，所以是否绑定后，马上执行一次doExpression，应该由调用者决定，因为只有所有涉及的model都绑定后，才可以执行
            for (var i = 0; i < bindingInfo.expressionConfigs.length; i++) {
                bindingInfo.doExpression(bindingInfo.expressionConfigs[i]);
            }
        }
        return model;
    };
    WayDataBindHelper.bindings = [];
    return WayDataBindHelper;
}());
var WayControlHelper = (function () {
    function WayControlHelper() {
    }
    WayControlHelper.getValue = function (ctrl) {
        switch (ctrl.tagName) {
            case "INPUT":
                if (ctrl.type == "checkbox")
                    return ctrl.checked;
                else
                    return ctrl.value;
            case "SELECT":
                return ctrl.value;
        }
        return "";
    };
    WayControlHelper.setValue = function (ctrl, value) {
        switch (ctrl.tagName) {
            case "INPUT":
                if (ctrl.type == "checkbox")
                    ctrl.checked = value;
                else if (value && value.value)
                    ctrl.value = value.value;
                else
                    ctrl.value = value;
                break;
            case "SELECT":
                if (value && value.value)
                    ctrl.value = value.value;
                else
                    ctrl.value = value;
                break;
        }
    };
    return WayControlHelper;
}());
var WayPageInfo = (function () {
    function WayPageInfo() {
        this.PageIndex = 0;
        this.PageSize = 10;
        //正在看第几页,for pageMode
        this.ViewingPageIndex = 0;
    }
    return WayPageInfo;
}());
var WayPager = (function () {
    function WayPager(_scrollable, _ctrl) {
        var _this = this;
        this.scrollable = _scrollable;
        this.control = _ctrl;
        this.scrollListener = function () { _this.onscroll(); };
        WayHelper.addEventListener(_scrollable[0], "scroll", this.scrollListener, undefined);
    }
    WayPager.prototype.onscroll = function () {
        if (!this.control.hasMorePage || this.control.pageMode)
            return;
        var y = this.scrollable.scrollTop();
        var x = this.scrollable.scrollLeft();
        var height = this.scrollable.height();
        if (y + height > this.scrollable[0].scrollHeight * 0.86) {
            this.control.shouldLoadMorePage();
        }
    };
    return WayPager;
}());
var WayProgressBar = (function () {
    function WayProgressBar(_color) {
        var _this = this;
        if (_color === void 0) { _color = "#FF2E82"; }
        this.showRef = 0;
        this.lastMouseDownLocation = null;
        this.lastMouseDownTime = null;
        this.timingNumber = 0;
        this.color = _color;
        if (document.body.addEventListener) {
            document.body.addEventListener("mousedown", function (e) { _this.mousedown(e); });
        }
        else {
            document.body.attachEvent("onmousedown", function (e) { _this.mousedown(e); });
        }
    }
    WayProgressBar.prototype.mousedown = function (e) {
        e = e ? e : window.event;
        var x = e.clientX;
        var y = e.clientY;
        this.lastMouseDownLocation = { "x": x, "y": y };
        this.lastMouseDownTime = new Date().getTime();
    };
    WayProgressBar.prototype.initLoading = function () {
        var pa = {
            width: 100,
            height: 100,
            stepsPerFrame: 1,
            trailLength: 1,
            pointDistance: .05,
            strokeColor: this.color,
            fps: 20,
            setup: function () {
                this._.lineWidth = 4;
            },
            step: function (point, index) {
                var cx = this.padding + 50, cy = this.padding + 50, _ = this._, angle = (Math.PI / 180) * (point.progress * 360), innerRadius = index === 1 ? 10 : 25;
                _.beginPath();
                _.moveTo(point.x, point.y);
                _.lineTo((Math.cos(angle) * innerRadius) + cx, (Math.sin(angle) * innerRadius) + cy);
                _.closePath();
                _.stroke();
            },
            path: [
                ['arc', 50, 50, 40, 0, 360]
            ]
        };
        this.loading = new window.Sonic(pa);
        $(this.loading.canvas).css({
            "-webkit-transform": "scale(0.5)",
            "-moz-transform": "scale(0.5)",
            "-ms-transform": "scale(0.5)",
            "transform": "scale(0.5)",
            "z-index": 99999,
            "position": "absolute"
        });
        $(this.loading.canvas).hide();
        document.body.appendChild(this.loading.canvas);
    };
    WayProgressBar.prototype.show = function (centerElement) {
        var _this = this;
        if (!this.loading) {
            this.initLoading();
        }
        var loadele = $(this.loading.canvas);
        if (this.showRef > 0) {
            this.showRef++;
            if (this.lastMouseDownTime && new Date().getTime() - this.lastMouseDownTime < 1000) {
                x = this.lastMouseDownLocation.x - 50;
                y = this.lastMouseDownLocation.y;
                loadele.css({
                    "left": x + "px",
                    "top": y + "px",
                });
            }
            return;
        }
        var offset = centerElement.offset();
        var x, y;
        if (this.lastMouseDownTime) {
            if (new Date().getTime() - this.lastMouseDownTime < 1000) {
                x = this.lastMouseDownLocation.x - 50;
                y = this.lastMouseDownLocation.y + 30;
            }
        }
        else {
            x = offset.left + (centerElement.width() - loadele.width()) / 2;
            y = offset.top + (centerElement.height() - loadele.height()) / 2;
        }
        loadele.css({
            "left": x + "px",
            "top": y + "px"
        });
        this.showRef++;
        this.timingNumber = setTimeout(function () {
            if (_this.timingNumber) {
                _this.timingNumber = 0;
                loadele.show();
                _this.loading.play();
            }
        }, 1000);
    };
    WayProgressBar.prototype.hide = function () {
        if (this.showRef > 0)
            this.showRef--;
        if (this.showRef > 0)
            return;
        if (this.timingNumber) {
            clearTimeout(this.timingNumber);
            this.timingNumber = 0;
        }
        this.loading.stop();
        $(this.loading.canvas).hide();
    };
    return WayProgressBar;
}());
var WayPopup = (function () {
    function WayPopup() {
        this.template = "<div style='background-color:#ffffff;color:red;font-size:12px;border:1px solid #cccccc;padding:2px;'>{0}</div>";
    }
    WayPopup.prototype.show = function (content, element, direction) {
        var _this = this;
        if (!this.container) {
            this.container = $(WayHelper.replace(this.template, "{0}", content));
            this.container.css({
                "position": "absolute",
                "visibility": "hidden",
                "z-index": 199,
            });
            this.container.click(function () {
                _this.hide();
            });
            document.body.appendChild(this.container[0]);
        }
        var x = 0;
        var y = 0;
        var offset = element.offset();
        if (direction == "[right]") {
            x = offset.left + element.outerWidth() + 1;
            y = offset.top;
        }
        else if (direction == "[left]") {
            x = offset.left - this.container.outerWidth() - 1;
            y = offset.top;
        }
        else if (direction == "[top]") {
            x = offset.left;
            y = offset.top - this.container.outerHeight() - 1;
        }
        else if (direction == "[bottom]") {
            x = offset.left;
            y = offset.top + element.outerHeight() + 1;
        }
        this.container.css({
            "left": x + "px",
            "top": y + "px",
            "visibility": "visible"
        });
    };
    WayPopup.prototype.hide = function () {
        if (this.container) {
            this.container.css({
                "visibility": "hidden"
            });
        }
    };
    return WayPopup;
}());
var WayValidator = (function () {
    function WayValidator(_element) {
        var _this = this;
        this.element = $(_element);
        if (_element.addEventListener) {
            _element.addEventListener("change", function () { _this.onvalueChanged(); });
        }
        else {
            _element.attachEvent("onchange", function () { _this.onvalueChanged(); });
        }
        this.popup = new WayPopup();
    }
    WayValidator.prototype.validate = function () {
        var showed = false;
        for (var i = 0; i < this.element[0].attributes.length; i++) {
            var name = this.element[0].attributes[i].name;
            if (name.indexOf("_verify") == 0 && name.indexOf("_msg") < 0) {
                try {
                    var expression = this.element[0].attributes[i].value;
                    expression = WayHelper.replace(expression, "{0}", JSON.stringify(WayControlHelper.getValue(this.element[0])));
                    var result = eval(expression);
                    if (result == true) {
                        var msg = this.element[0].getAttribute(name + "_msg");
                        var match = msg.match(/\[(\w)+\]/g);
                        var direction;
                        if (match) {
                            direction = match[0];
                            msg = msg.substr(direction.length);
                        }
                        else {
                            direction = "[left]";
                        }
                        showed = true;
                        this.popup.show(msg, this.element, direction);
                        break;
                    }
                }
                catch (e) {
                    throw "数据验证发生错误，" + e.message;
                }
            }
        }
        if (showed == false) {
            this.popup.hide();
        }
        return !showed;
    };
    WayValidator.prototype.onvalueChanged = function () {
        this.validate();
    };
    return WayValidator;
}());
var WayDBContext = (function () {
    function WayDBContext(controller, _datasource) {
        if (typeof controller == "object") {
            this.remoting = controller;
        }
        else {
            this.remoting = WayScriptRemoting.createRemotingController(controller);
        }
        this.datasource = _datasource;
    }
    WayDBContext.prototype.getDatas = function (pageinfo, bindFields, searchModel, callback, async) {
        if (async === void 0) { async = true; }
        searchModel = searchModel ? JSON.stringify(searchModel) : "";
        this.remoting.pageInvoke("GetDataSource", [pageinfo, this.datasource, bindFields, searchModel], function (ret, err) {
            if (err) {
                callback(null, null, err);
            }
            else {
                var pkidDic = ret[ret.length - 1];
                var pkid = null;
                if (pkidDic.pkid != "") {
                    pkid = pkidDic.pkid;
                }
                ret.length--;
                callback(ret, pkid, null);
            }
        }, async);
    };
    WayDBContext.prototype.getDataItem = function (bindFields, searchModel, callback, async) {
        if (async === void 0) { async = true; }
        var pageinfo = new WayPageInfo();
        pageinfo.PageIndex = 0;
        pageinfo.PageSize = 1;
        this.getDatas(pageinfo, bindFields, searchModel, function (_data, _pkid, err) {
            if (err) {
                callback(null, err);
            }
            else {
                if (_data.length > 0) {
                    callback(_data[0], null);
                }
                else {
                    callback(null, null);
                }
            }
        }, async);
    };
    WayDBContext.prototype.saveData = function (data, primaryKey, callback) {
        this.remoting.pageInvoke("SaveData", [this.datasource, JSON.stringify(data)], function (idvalue, err) {
            if (err) {
                callback(data, err);
            }
            else {
                if (primaryKey) {
                    eval("data." + primaryKey + "=" + JSON.stringify(idvalue) + ";");
                }
                callback(data, err);
            }
        });
    };
    WayDBContext.prototype.count = function (searchModel, callback) {
        searchModel = searchModel ? JSON.stringify(searchModel) : "";
        this.remoting.pageInvoke("Count", [this.datasource, searchModel], callback);
    };
    WayDBContext.prototype.sum = function (fields, searchModel, callback) {
        searchModel = searchModel ? JSON.stringify(searchModel) : "";
        this.remoting.pageInvoke("Sum", [this.datasource, fields, searchModel], callback);
    };
    return WayDBContext;
}());
var WayGridView = (function (_super) {
    __extends(WayGridView, _super);
    function WayGridView(elementId, _pagesize) {
        if (_pagesize === void 0) { _pagesize = 10; }
        _super.call(this);
        this.itemTemplates = [];
        this.items = [];
        //原始itemdata
        this.originalItems = [];
        this.pageinfo = new WayPageInfo();
        this.fieldExp = /\{\@(\w|\.|\:)+\}/g;
        this.loading = new WayProgressBar("#cccccc");
        // 标识当前绑定数据的事物id
        this.transcationID = 1;
        //设置，必须获取的字段(因为没有在模板中出现的字段，不会输出)
        this.dataMembers = [];
        //是否支持下拉刷新
        //下拉刷新必须定义body模板
        this.supportDropdownRefresh = false;
        //定义item._status的数据原型，可以修改此原型达到期望的目的
        this.itemStatusModel = { Selected: false };
        //是否使用翻页模式
        this.pageMode = false;
        //pageMode模式下，预先加载多少页数据
        this.preLoadNumForPageMode = 1;
        //搜索条件model
        this.searchModel = {};
        this.allowEdit = false;
        try {
            if (isNaN(_pagesize))
                _pagesize = 10;
            var controller = document.body.getAttribute("_controller");
            this.dbContext = new WayDBContext(controller, null);
            if (typeof elementId == "string")
                this.element = $("#" + elementId);
            else if (elementId.tagName)
                this.element = $(elementId);
            else
                this.element = elementId;
            this.allowEdit = this.element.attr("_allowedit") == "true";
            this.element.css({
                "overflow-y": "auto",
                "-webkit-overflow-scrolling": "touch"
            });
            var isTouch = "ontouchstart" in this.element[0];
            if (!isTouch)
                this.supportDropdownRefresh = false;
            this.datasource = this.element.attr("_datasource");
            this.pager = new WayPager(this.element, this);
            this.pageinfo.PageSize = _pagesize;
            var bodyTemplate = this.element.find("script[_for='body']");
            var templates = this.element.find("script");
            this.itemContainer = this.element;
            if (bodyTemplate.length > 0) {
                this.bodyTemplateHtml = bodyTemplate[0].innerHTML;
                this.itemContainer = $(this.bodyTemplateHtml);
                this.element[0].appendChild(this.itemContainer[0]);
                this.initRefreshEvent(this.itemContainer);
            }
            else {
                //没有body模板，则不支持下拉刷新
                this.supportDropdownRefresh = false;
            }
            if (this.itemContainer[0].children.length > 0 && this.itemContainer[0].children[0].tagName == "TBODY") {
                this.itemContainer = $(this.itemContainer[0].children[0]);
            }
            for (var i = 0; i < templates.length; i++) {
                var templateItem = templates[i];
                templateItem.parentElement.removeChild(templateItem);
                var _for = templateItem.getAttribute("_for");
                if (_for == "header") {
                    this.header = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_for == "footer") {
                    this.footer = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_for == "item") {
                    var mode = templateItem.getAttribute("_mode");
                    var match = templateItem.getAttribute("_match");
                    var temp = new WayTemplate(this.getTemplateOuterHtml(templateItem), match, mode);
                    this.addItemTemplate(temp);
                }
            }
        }
        catch (e) {
            throw "WayGridView构造函数错误，" + e.message;
        }
    }
    Object.defineProperty(WayGridView.prototype, "datasource", {
        get: function () {
            return this._datasource;
        },
        set: function (_v) {
            if (typeof _v == "string" && _v.indexOf("[") == 0) {
                _v = JSON.parse(_v);
            }
            this._datasource = _v;
            this.dbContext.datasource = _v;
        },
        enumerable: true,
        configurable: true
    });
    //初始化下拉刷新事件
    WayGridView.prototype.initRefreshEvent = function (touchEle) {
        var _this = this;
        var isTouch = "ontouchstart" in this.itemContainer[0];
        if (!isTouch)
            this.supportDropdownRefresh = false;
        var moving = false;
        var isTouchToRefresh = false;
        //先预设一下,否则有时候第一次设置touchEle会白屏
        touchEle.css("will-change", "transform");
        var point;
        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", function (e) {
            if (!_this.supportDropdownRefresh || _this.pageMode)
                return;
            isTouchToRefresh = false;
            if (_this.element.scrollTop() > 0) {
                return;
            }
            e = e || window.event;
            touchEle.css("will-change", "transform");
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY
            };
            moving = true;
        }, true);
        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", function (e) {
            if (moving) {
                if (_this.element.scrollTop() > 0) {
                    moving = false;
                    return;
                }
                e = e || window.event;
                var y = isTouch ? e.touches[0].clientY : e.clientY;
                y = (y - point.y);
                if (y > 0) {
                    isTouchToRefresh = true;
                    y = "translate(0px," + y + "px)";
                    touchEle.css({
                        "-webkit-transform": y,
                        "-moz-transform": y,
                        "transform": y
                    });
                }
                if (isTouchToRefresh) {
                    if (e.stopPropagation) {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    else
                        window.event.cancelBubble = true;
                }
            }
        }, true);
        var touchoutFunc = function (e) {
            if (moving) {
                moving = false;
                e = e || window.event;
                var y = isTouch ? e.changedTouches[0].clientY : e.clientY;
                y = (y - point.y);
                isTouchToRefresh = (y > _this.element.height() * 0.15);
                touchEle.css({
                    "transition": "transform 0.5s",
                    "-webkit-transform": "translate(0px,0px)",
                    "-moz-transform": "translate(0px,0px)",
                    "transform": "translate(0px,0px)"
                });
            }
        };
        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);
        this.element[0].ontouchcancel = function () {
            isTouchToRefresh = false;
            touchEle.css({
                "transition": "transform 0.5s",
                "-webkit-transform": "translate(0px,0px)",
                "-moz-transform": "translate(0px,0px)",
                "transform": "translate(0px,0px)"
            });
        };
        WayHelper.addEventListener(touchEle[0], "transitionend", function (e) {
            touchEle.css({
                "transition": "",
                "will-change": "auto"
            });
            if (isTouchToRefresh) {
                _this.databind();
            }
        }, true);
    };
    WayGridView.prototype.showLoading = function () {
        this.loading.show(this.element);
    };
    WayGridView.prototype.hideLoading = function () {
        this.loading.hide();
    };
    WayGridView.prototype.getTemplateOuterHtml = function (element) {
        return element.innerHTML;
        //var ctrl: JQuery = $(element);
        //ctrl.css("display", "");
        //ctrl.removeAttr("_for");
        //ctrl.removeAttr("_match");
        //ctrl.removeAttr("_mode");
        //var html = "<" + ctrl[0].tagName + " ";
        //for (var i = 0; i < ctrl[0].attributes.length; i++) {
        //    html += ctrl[0].attributes[i].name + "=" + JSON.stringify(ctrl[0].attributes[i].value) + " ";
        //}
        //html += ">" + element.innerHTML + "</" + ctrl[0].tagName + ">";
        //return html;
    };
    //添加item模板
    WayGridView.prototype.addItemTemplate = function (temp) {
        this.itemTemplates.push(temp);
    };
    //删除item模板
    WayGridView.prototype.removeItemTemplate = function (temp) {
        this.itemTemplates[this.itemTemplates.indexOf(temp)] = null;
    };
    WayGridView.prototype.replace = function (content, find, replace) {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
    };
    WayGridView.prototype.count = function (callback) {
        var _this = this;
        this.showLoading();
        this.dbContext.count(this.searchModel, function (data, err) {
            _this.hideLoading();
            callback(data, err);
        });
    };
    WayGridView.prototype.sum = function (fields, callback) {
        var _this = this;
        this.showLoading();
        this.dbContext.sum(fields, this.searchModel, function (data, err) {
            _this.hideLoading();
            callback(data, err);
        });
    };
    WayGridView.prototype.save = function (itemIndex, callback) {
        var _this = this;
        if (this.allowEdit == false) {
            callback(null, "此WayGridView未设置为可编辑,请设置_allowedit=\"true\"");
            return;
        }
        var item = this.items[itemIndex];
        var model = item._data;
        var data = this.originalItems[itemIndex];
        var changedData = WayHelper.getDataForDiffent(data, model);
        if (changedData) {
            if (this.primaryKey && this.primaryKey.length > 0) {
                eval("changedData." + this.primaryKey + "=model." + this.primaryKey + ";");
            }
            this.showLoading();
            this.dbContext.saveData(changedData, this.primaryKey, function (data, err) {
                _this.hideLoading();
                if (err) {
                    callback(null, err);
                }
                else {
                    if (_this.primaryKey && _this.primaryKey.length > 0) {
                        callback(eval("changedData." + _this.primaryKey), null);
                    }
                    else {
                        callback(null, null);
                    }
                }
            });
        }
        else {
            //没有值变化
            var idvalue;
            if (this.primaryKey) {
                eval("idvalue=model." + this.primaryKey + ";");
            }
            callback(idvalue, null);
        }
    };
    WayGridView.prototype.onErr = function (err) {
        if (this.onerror) {
            this.onerror(err);
        }
        else
            throw err;
    };
    WayGridView.prototype.contains = function (arr, find) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == find) {
                return true;
            }
        }
        return false;
    };
    WayGridView.prototype.getBindFields = function () {
        var result = [];
        for (var i = 0; i < this.itemTemplates.length; i++) {
            var template = this.itemTemplates[i];
            var itemContent = template.content;
            var match = itemContent.match(/\@(\w|\.|\:)+/g);
            if (match) {
                for (var j = 0; j < match.length; j++) {
                    var str = match[j].toString();
                    var field = str.substr(1, str.length - 1);
                    if (!this.contains(result, field)) {
                        result.push(field);
                    }
                }
            }
        }
        if (this.primaryKey && this.primaryKey.length > 0) {
            result.push(this.primaryKey);
        }
        for (var i = 0; i < this.dataMembers.length; i++) {
            var field = this.dataMembers[i];
            if (!this.contains(result, field)) {
                result.push(field);
            }
        }
        return result;
    };
    //绑定数据
    WayGridView.prototype.databind = function () {
        if (this.pageMode) {
            this.initForPageMode();
        }
        this.footerItem = null;
        //清除内容
        for (var i = 0; i < this.items.length; i++) {
            //消除绑定
            WayDataBindHelper.removeDataBind(this.items[i][0]);
        }
        this.items = [];
        this.originalItems = [];
        while (this.itemContainer[0].children.length > 0) {
            this.itemContainer[0].removeChild(this.itemContainer[0].children[0]);
        }
        //bind header
        if (this.header) {
            var headerObj = $(this.header.content);
            this.itemContainer.append(headerObj);
        }
        if (this.onDatabind) {
            this.onDatabind();
        }
        this.hasMorePage = true;
        this.pageinfo.PageIndex = 0;
        this.shouldLoadMorePage();
    };
    WayGridView.prototype.shouldLoadMorePage = function () {
        var _this = this;
        this.hasMorePage = false; //设为false，可以禁止期间被Pager再次调用
        var pageData;
        this.transcationID++;
        var mytranId = this.transcationID;
        if (typeof this.datasource == "string") {
            this.showLoading();
            this.dbContext.getDatas(this.pageinfo, this.getBindFields(), (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel, function (ret, pkid, err) {
                _this.hideLoading();
                if (mytranId != _this.transcationID)
                    return;
                if (err) {
                    _this.hasMorePage = true;
                    _this.onErr(err);
                }
                else {
                    if (pkid != null) {
                        _this.primaryKey = pkid;
                    }
                    pageData = ret;
                    _this.bindDataToGrid(pageData);
                }
            });
        }
        else {
            pageData = this.getDataByPagesize(this.datasource);
            this.bindDataToGrid(pageData);
        }
    };
    WayGridView.prototype.bindDataToGrid = function (pageData) {
        this.binddatas(pageData);
        this.pageinfo.PageIndex++;
        this.hasMorePage = this.pageinfo.PageSize > 0 && pageData.length >= this.pageinfo.PageSize;
        if (this.onAfterCreateItems) {
            try {
                this.onAfterCreateItems(this.items.length, this.hasMorePage);
            }
            catch (e) {
            }
        }
        if (this.hasMorePage) {
            if (this.pageMode) {
                //翻页模式
                //预加载
                if (this.preLoadNumForPageMode < 1)
                    this.preLoadNumForPageMode = 1;
                if (this.pageinfo.PageIndex <= this.preLoadNumForPageMode) {
                    this.shouldLoadMorePage();
                }
            }
            else {
                if (this.element[0].scrollHeight <= this.element.height() * 1.1) {
                    this.shouldLoadMorePage();
                }
            }
        }
    };
    WayGridView.prototype.getDataByPagesize = function (datas) {
        if (datas.length <= this.pageinfo.PageSize)
            return datas;
        var result = [];
        var end = this.pageinfo.PageSize * (this.pageinfo.PageIndex + 1);
        for (var i = this.pageinfo.PageSize * this.pageinfo.PageIndex; i < end && i < datas.length; i++) {
            result.push(datas[i]);
        }
        return result;
    };
    //把两个table的td设为一样的宽度
    WayGridView.prototype.setSameWidthForTables = function (tableSource, tableHeader) {
        while (tableSource[0].tagName != "TABLE") {
            tableSource = $(tableSource[0].children[0]);
        }
        while (tableHeader[0].tagName != "TABLE") {
            tableHeader = $(tableHeader[0].children[0]);
        }
        if (tableSource[0].children.length > 0 && tableSource[0].children[0].tagName == "TBODY") {
            tableSource = $(tableSource[0].children[0]);
        }
        if (tableHeader[0].children.length > 0 && tableHeader[0].children[0].tagName == "TBODY") {
            tableHeader = $(tableHeader[0].children[0]);
        }
        var sourceIndex = 0;
        for (var i = 0; i < tableHeader[0].children[0].children.length - 1; i++) {
            var headerTD = $(tableHeader[0].children[0].children[i]);
            var colspan = headerTD.attr("colspan");
            if (!colspan || colspan == "")
                colspan = 1;
            else
                colspan = parseInt(colspan);
            var cellwidth = 0;
            for (var j = sourceIndex; j < sourceIndex + colspan; j++) {
                var sourceTD = $(tableSource[0].children[0].children[j]);
                cellwidth += sourceTD.width();
            }
            sourceIndex += colspan;
            headerTD.width(cellwidth);
        }
    };
    WayGridView.prototype.findItemTemplate = function (data, mode) {
        if (mode === void 0) { mode = ""; }
        if (this.itemTemplates.length == 1)
            return this.itemTemplates[0];
        var expression = /\@(\w|\.|\:)+/g;
        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (!itemTemplalte || itemTemplalte.mode != mode)
                continue;
            if (itemTemplalte.match) {
                var matchStr = itemTemplalte.match;
                var match = matchStr.match(expression);
                if (match) {
                    for (var j = 0; j < match.length; j++) {
                        var str = match[j].toString();
                        var field = str.substr(1, str.length - 1);
                        matchStr = matchStr.replace(str, JSON.stringify(eval("data." + field)));
                    }
                    if (eval(matchStr) == true) {
                        return itemTemplalte;
                    }
                }
            }
        }
        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (itemTemplalte && itemTemplalte.mode == mode)
                return itemTemplalte;
        }
        for (var i = 0; i < this.itemTemplates.length; i++) {
            var itemTemplalte = this.itemTemplates[i];
            if (itemTemplalte)
                return itemTemplalte;
        }
        return null;
    };
    //改变指定item为指定的mode
    WayGridView.prototype.changeMode = function (itemIndex, mode) {
        try {
            var item = this.items[itemIndex];
            //移除数据绑定
            WayDataBindHelper.removeDataBind(item[0]);
            var newItem = this.createItem(itemIndex, mode);
            this.items[itemIndex] = newItem;
            var pre = item.prev();
            var parent = item.parent();
            item.remove();
            if (pre.length == 0) {
                parent.prepend(newItem);
            }
            else {
                newItem.insertAfter(pre);
            }
            if (this.onItemSizeChanged) {
                this.onItemSizeChanged();
            }
            return newItem;
        }
        catch (e) {
            throw "changeMode error:" + e.message;
        }
    };
    //接受item数据的更新，如当前item的数据和很多input进行绑定，input值改变后，并且同步到数据库，
    //那么updateItemData方法就是同步本地GridView，否则调用changeMode，item显示的值还是原来的值
    WayGridView.prototype.acceptItemChanged = function (itemIndex) {
        var item = this.items[itemIndex];
        var mydata = item._data.getSource();
        this.originalItems[itemIndex] = WayHelper.clone(mydata);
    };
    //从服务器更新指定item的数据，并重新绑定
    WayGridView.prototype.rebindItemFromServer = function (itemIndex, mode, callback) {
        var _this = this;
        if (callback === void 0) { callback = null; }
        var searchmodel = {};
        var item = this.items[itemIndex];
        searchmodel[this.primaryKey] = item._data[this.primaryKey];
        this.dbContext.getDataItem(this.getBindFields(), searchmodel, function (data, err) {
            if (!err) {
                _this.originalItems[itemIndex] = data;
                if (typeof mode == "undefined")
                    mode = item._mode;
                _this.changeMode(itemIndex, mode);
            }
            if (callback)
                callback(data, err);
        });
    };
    WayGridView.prototype.replaceFromString = function (str, itemIndex, statusmodel, data) {
        var expression = /\{[ ]?\$(\w+)[ ]?\}/g;
        var result = str;
        while (true) {
            var r = expression.exec(str);
            if (!r)
                break;
            var proname = r[1];
            if (proname == "ItemIndex") {
                result = result.replace(r[0], itemIndex);
            }
            else {
                if (eval("typeof statusmodel." + proname + "=='undefined'") == false) {
                    result = result.replace(r[0], eval("statusmodel." + proname));
                }
                else {
                    result = result.replace(r[0], "null");
                }
            }
        }
        var match = result.match(this.fieldExp);
        if (match) {
            for (var j = 0; j < match.length; j++) {
                var str = match[j].toString();
                var field = str.substr(2, str.length - 3);
                if (field.indexOf(":") > 0) {
                    field = field.substr(0, field.indexOf(":"));
                    result = result.replace(str, eval("data." + field + ".text"));
                }
                else {
                    var value = eval("data." + field);
                    if (value == null || typeof value == "undefined")
                        value = "";
                    if (typeof value == "object") {
                        if (typeof value.caption != "undefined") {
                            result = result.replace(str, value.caption);
                        }
                        else if (typeof value.value != "undefined") {
                            result = result.replace(str, value.value);
                        }
                        else {
                            result = result.replace(str, "");
                        }
                    }
                    else {
                        result = result.replace(str, value);
                    }
                }
            }
        }
        return result;
    };
    WayGridView.prototype.replaceVariable = function (container, itemIndex, statusmodel, data) {
        for (var i = 0; i < container.attributes.length; i++) {
            var attName = container.attributes[i].name;
            var attValue = container.getAttribute(attName);
            var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
            if (attValue != formatvalue) {
                container.setAttribute(attName, formatvalue);
            }
        }
        if (container.tagName.indexOf("Way") != 0) {
            //如果不是WayControl，继续检查内容和子节点
            for (var i = 0; i < container.childNodes.length; i++) {
                var node = container.childNodes[i];
                if (node.nodeType == 3) {
                    //text
                    var attValue = node.data;
                    var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
                    if (attValue != formatvalue) {
                        node.data = formatvalue;
                    }
                }
                else if (node.nodeType == 1) {
                    //htmlelement
                    this.replaceVariable(node, itemIndex, statusmodel, data);
                }
            }
        }
    };
    WayGridView.prototype.createItem = function (itemIndex, mode) {
        if (mode === void 0) { mode = ""; }
        //把数据克隆一份
        var currentItemStatus;
        if (itemIndex < this.items.length) {
            currentItemStatus = this.items[itemIndex]._status;
        }
        var statusmodel = currentItemStatus ? currentItemStatus : this.itemStatusModel;
        var data = WayHelper.clone(this.originalItems[itemIndex]);
        var template = this.findItemTemplate(data, mode);
        var itemContent = template.content;
        var item = $(itemContent);
        this.replaceVariable(item[0], itemIndex, statusmodel, data);
        //把WayControl初始化
        for (var i = 0; i < item[0].children.length; i++) {
            checkToInitWayControl(item[0].children[i]);
        }
        var model = WayDataBindHelper.dataBind(item[0], data, itemIndex, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g);
        //创建status
        var myChangeFunc = statusmodel.onchange;
        var statusData = WayHelper.clone(statusmodel);
        item._status = WayDataBindHelper.dataBind(item[0], statusData, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g, true);
        if (typeof myChangeFunc == "function") {
            item._status.onchange = myChangeFunc;
        }
        //建立验证
        if (true) {
            var bindItemElements = item.find("*[_databind]");
            var validators = [];
            for (var i = 0; i < bindItemElements.length; i++) {
                try {
                    validators.push(new WayValidator(bindItemElements[i]));
                }
                catch (e) {
                }
            }
            if (validators.length > 0) {
                item.validate = function () {
                    var passed = true;
                    for (var i = 0; i < validators.length; i++) {
                        if (!validators[i].validate()) {
                            passed = false;
                        }
                    }
                    return passed;
                };
            }
        }
        ////////////
        item._data = model;
        item._mode = mode;
        if (this.onCreateItem) {
            this.onCreateItem(item, mode);
        }
        return item;
    };
    WayGridView.prototype.binddatas = function (datas) {
        if (this.pageMode) {
            this.binddatas_pageMode(datas);
            return;
        }
        try {
            //bind items
            for (var i = 0; i < datas.length; i++) {
                this.originalItems.push(datas[i]);
                var itemindex = this.items.length;
                var item = this.createItem(itemindex);
                if (this.footerItem) {
                    item.insertBefore(this.footerItem);
                }
                else {
                    this.itemContainer.append(item);
                }
                this.items.push(item);
            }
            //bind footer
            if (!this.footerItem && this.footer) {
                this.footerItem = $(this.footer.content);
                this.itemContainer.append(this.footerItem);
            }
            if (this.onItemSizeChanged) {
                this.onItemSizeChanged();
            }
        }
        catch (e) {
            this.onErr("GridView.databind error:" + e.message);
        }
    };
    WayGridView.prototype.initForPageMode = function () {
        var _this = this;
        if (this.itemContainer[0] != this.element[0]) {
            this.itemContainer[0].parentElement.removeChild(this.itemContainer[0]);
        }
        this.itemContainer = $(document.createElement("DIV"));
        this.element[0].appendChild(this.itemContainer[0]);
        this.element.css({
            "overflow-x": "hidden",
            "overflow-y": "hidden"
        });
        this.itemContainer.css({
            "height": "100%",
            "width": "0px",
            "will-change": "transform"
        });
        var isTouch = "ontouchstart" in this.itemContainer[0];
        var point;
        var moving;
        var isTouchToRefresh = false;
        this.element[0].ontouchstart = null;
        this.element[0].ontouchend = null;
        this.element[0].ontouchmove = null;
        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", function (e) {
            isTouchToRefresh = false;
            e = e || window.event;
            _this.itemContainer.css("will-change", "transform");
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY,
                time: new Date().getTime()
            };
            moving = true;
        }, true);
        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", function (e) {
            if (moving) {
                e = e || window.event;
                var x = isTouch ? e.touches[0].clientX : e.clientX;
                x = (x - point.x);
                if (x > 0 && _this.pageinfo.ViewingPageIndex == 0) {
                    x /= 3;
                }
                else if (x < 0 && _this.pageinfo.ViewingPageIndex == _this.itemContainer[0].children.length - 1) {
                    x /= 3;
                }
                if (Math.abs(x) > 0) {
                    isTouchToRefresh = true;
                }
                x = "translate(" + (x - _this.pageinfo.ViewingPageIndex * _this.element.width()) + "px,0px)";
                _this.itemContainer.css({
                    "-webkit-transform": x,
                    "-moz-transform": x,
                    "transform": x
                });
                if (isTouchToRefresh) {
                    if (e.stopPropagation) {
                        e.stopPropagation();
                        e.preventDefault();
                    }
                    else
                        window.event.cancelBubble = true;
                }
            }
        }, true);
        var touchoutFunc = function (e) {
            if (moving) {
                moving = false;
                e = e || window.event;
                var x = isTouch ? e.changedTouches[0].clientX : e.clientX;
                x = (x - point.x);
                if (x != 0) {
                    if (x > _this.element.width() / 3 || (x > _this.element.width() / 10 && new Date().getTime() - point.time < 500)) {
                        if (_this.pageinfo.ViewingPageIndex > 0) {
                            _this.pageinfo.ViewingPageIndex--;
                        }
                    }
                    else if (-x > _this.element.width() / 3 || (-x > _this.element.width() / 10 && new Date().getTime() - point.time < 500)) {
                        if (_this.pageinfo.ViewingPageIndex < _this.itemContainer[0].children.length - 1) {
                            _this.pageinfo.ViewingPageIndex++;
                        }
                    }
                    var desLocation = "translate(" + -_this.pageinfo.ViewingPageIndex * _this.element.width() + "px,0px)";
                    _this.itemContainer.css({
                        "transition": "transform 0.5s",
                        "-webkit-transform": desLocation,
                        "-moz-transform": desLocation,
                        "transform": desLocation
                    });
                }
            }
        };
        this.element[0].ontouchcancel = function () {
            var desLocation = "translate(" + -_this.pageinfo.ViewingPageIndex * _this.element.width() + "px,0px)";
            _this.itemContainer.css({
                "transition": "transform 0.5s",
                "-webkit-transform": desLocation,
                "-moz-transform": desLocation,
                "transform": desLocation
            });
        };
        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);
        WayHelper.addEventListener(this.itemContainer[0], "transitionend", function (e) {
            _this.itemContainer.css({
                "transition": "",
            });
            if (_this.onViewPageIndexChange) {
                _this.onViewPageIndexChange(_this.pageinfo.ViewingPageIndex);
            }
            if (_this.pageinfo.ViewingPageIndex == _this.itemContainer[0].children.length - 1 && _this.hasMorePage) {
                _this.shouldLoadMorePage();
            }
        }, true);
    };
    //设置当前观看那一页，执行这个方法，pageMode必须是true
    WayGridView.prototype.setViewPageIndex = function (index) {
        if (this.pageMode) {
            if (index >= 0 && index != this.pageinfo.ViewingPageIndex && index < this.itemContainer[0].children.length) {
                this.pageinfo.ViewingPageIndex = index;
                var desLocation = "translate(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0px)";
                this.itemContainer.css({
                    "transition": "transform 0.5s",
                    "-webkit-transform": desLocation,
                    "-moz-transform": desLocation,
                    "transform": desLocation
                });
            }
        }
    };
    WayGridView.prototype.binddatas_pageMode = function (datas) {
        if (datas.length == 0)
            return;
        try {
            if (!this.bodyTemplateHtml) {
                this.bodyTemplateHtml = "<div></div>";
            }
            this.itemContainer.width(this.itemContainer.width() + this.element.width());
            var divContainer = $(this.bodyTemplateHtml);
            divContainer.css({
                "width": this.element.width() + "px",
                "height": this.element.height() + "px",
                "float": "left",
            });
            this.itemContainer.append(divContainer);
            //bind items
            for (var i = 0; i < datas.length; i++) {
                this.originalItems.push(datas[i]);
                var itemindex = this.items.length;
                var item = this.createItem(itemindex);
                divContainer.append(item);
                this.items.push(item);
            }
        }
        catch (e) {
            this.onErr("GridView.databind error:" + e.message);
        }
    };
    return WayGridView;
}(WayBaseObject));
var WayDropDownList = (function () {
    function WayDropDownList(elementid, datasource) {
        var _this = this;
        this.isMobile = false;
        this.isBindedGrid = false;
        this.onchange = null;
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        this.element[0]._WayControl = this;
        this.isMobile = "ontouchstart" in this.element[0];
        //this.isMobile = true;
        var textele = this.element.find("*[_istext]");
        if (textele.length > 0) {
            this.textElement = $(textele[0]);
        }
        var actionEle = this.element.find("*[_isaction]");
        if (actionEle.length > 0) {
            this.actionElement = $(actionEle[0]);
        }
        this.itemContainer = $(this.element.find("script[_for='itemContainer']")[0].innerHTML);
        var itemtemplate = this.element.find("script[_for='item']")[0];
        this.valueMember = this.element[0].getAttribute("_valueMember");
        this.textMember = this.element[0].getAttribute("_textMember");
        if (this.actionElement) {
            this.init();
            this.itemContainer[0].appendChild(this.element.find("script[_for='item']")[0]);
            this.grid = new WayGridView(this.itemContainer[0], 20);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = function (item) { return _this._onGridItemCreated(item); };
            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");
                if (this.textElement[0].tagName == "INPUT") {
                    this.textElement.attr("_databind", "value=@text");
                    this.grid.searchModel = WayDataBindHelper.dataBind(this.textElement[0], {});
                    this.grid.searchModel.submitObject = function () {
                        var result;
                        eval("result = {" + _this.textMember + ":" + JSON.stringify(_this.grid.searchModel.text) + "}");
                        return result;
                    };
                    this.grid.searchModel.onchange = function () {
                        if (_this.itemContainer.css("visibility") == "visible") {
                            _this.grid.databind();
                            _this.isBindedGrid = true;
                        }
                        else {
                            _this.isBindedGrid = false;
                        }
                    };
                }
            }
        }
    }
    Object.defineProperty(WayDropDownList.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            if (v != this._value) {
                this._value = v;
                this._text = this.getTextByValue(v);
                if (this._text) {
                    this.setText(this._text);
                }
                else {
                    this._text = "";
                    this.setText("");
                }
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayDropDownList.prototype, "text", {
        get: function () {
            return this._text;
        },
        set: function (v) {
            if (v != this._text) {
                this._text = v;
                this._value = this.getValueByText(v);
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    WayDropDownList.prototype.addEventListener = function (eventName, func) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }
            this.onchange.push(func);
        }
    };
    WayDropDownList.prototype.fireEvent = function (eventName) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    };
    WayDropDownList.prototype.getTextByValue = function (value) {
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = this.grid.items[i]._data;
            if (data.value == value) {
                return data.text;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.valueMember + ":" + JSON.stringify(value) + "}");
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, function (data, err) {
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        if (result) {
            return result[this.textMember];
        }
        return null;
    };
    WayDropDownList.prototype.getValueByText = function (text) {
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = this.grid.items[i]._data;
            if (data.text == text) {
                return data.value;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.textMember + ":" + JSON.stringify("equal:" + text) + "}");
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, function (data, err) {
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        if (result) {
            return result[this.valueMember];
        }
        return null;
    };
    WayDropDownList.prototype._onGridItemCreated = function (item) {
        var _this = this;
        item._status.Selected = item._data.value == this.value;
        item.click(function () {
            _this.hideList();
            item._status.Selected = true;
            for (var i = 0; i < _this.grid.items.length; i++) {
                if (_this.grid.items[i] != item) {
                    _this.grid.items[i]._status.Selected = false;
                }
            }
            _this.value = item._data.value;
        });
    };
    WayDropDownList.prototype.setText = function (text) {
        if (this.textElement[0].tagName == "INPUT") {
            if (this.textElement.val() != text)
                this.textElement.val(text);
        }
        else {
            if (this.textElement.html() != text)
                this.textElement.html(text);
        }
    };
    WayDropDownList.prototype.init = function () {
        var _this = this;
        this.itemContainer.css({
            "position": "absolute",
            "z-index": 999,
            "overflow-x": "hidden",
            "overflow-y": "auto",
            "visibility": "hidden"
        });
        if (!this.isMobile) {
            var cssHeight = this.itemContainer.css("height");
            if (!cssHeight || cssHeight == "" || cssHeight == "0px") {
                this.itemContainer.css("height", "300px");
            }
        }
        else {
            this.itemContainer.css("position", "fixed");
            this.maskLayer = $("<div style='background-color:#000000;opacity:0.3;z-index:998;position:fixed;width:100%;height:100%;display:none;left:0;top:0;'></div>");
            document.body.appendChild(this.maskLayer[0]);
            this.itemContainer.css("height", "300px");
        }
        document.body.appendChild(this.itemContainer[0]);
        this.actionElement.click(function (e) {
            e = e || window.event;
            if (e.stopPropagation)
                e.stopPropagation();
            else
                e.cancelBubble = true;
            _this.showList();
        });
        this.textElement.click(function (e) {
            e = e || window.event;
            if (e.stopPropagation)
                e.stopPropagation();
            else
                e.cancelBubble = true;
        });
        if (this.textElement[0].tagName == "INPUT") {
            if (this.isMobile) {
            }
            else {
                this.textElement.keyup(function () {
                    //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                    _this.grid.searchModel.text = _this.textElement.val();
                    if (_this.itemContainer.css("visibility") != "visible") {
                        //如果没有显示，则主动显示
                        _this.showList();
                    }
                });
            }
            this.textElement.change(function () {
                //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                _this.grid.searchModel.text = _this.textElement.val();
                _this.text = _this.grid.searchModel.text;
            });
        }
        $(document.documentElement).click(function () {
            _this.hideList();
        });
    };
    //显示下拉列表
    WayDropDownList.prototype.showList = function () {
        if (this.maskLayer) {
            this.maskLayer.show();
        }
        if (!this.isMobile) {
            var offset = this.textElement.offset();
            var y = (offset.top + this.textElement.outerHeight());
            this.itemContainer.css({
                width: this.textElement.outerWidth() + "px",
                left: offset.left + "px",
                top: y + "px",
            });
            if (y + this.itemContainer.outerHeight() > document.body.scrollTop + this.windowObj.innerHeight()) {
                y = offset.top - this.itemContainer.outerHeight();
                if (y >= 0) {
                    this.itemContainer.css("top", y + "px");
                }
            }
            if (offset.left + this.itemContainer.outerWidth() > document.body.scrollLeft + this.windowObj.innerWidth()) {
                this.itemContainer.css("left", (document.body.scrollLeft + this.windowObj.innerWidth() - this.itemContainer.outerWidth()) + "px");
            }
        }
        else {
            this.itemContainer.css({
                width: this.windowObj.innerWidth() * 0.9 + "px",
                height: this.windowObj.innerHeight() * 0.9 + "px",
                left: this.windowObj.innerWidth() * 0.05 + "px",
                top: this.windowObj.innerHeight() * 0.05 + "px",
            });
        }
        if (this.itemContainer.css("visibility") != "visible") {
            this.itemContainer.css("visibility", "visible");
            if (this.isBindedGrid) {
                this.setSelectedItemScrollIntoView();
            }
        }
        if (!this.isBindedGrid) {
            this.grid.databind();
            this.isBindedGrid = true;
        }
    };
    WayDropDownList.prototype.setSelectedItemScrollIntoView = function () {
        if (this.value) {
            for (var i = 0; i < this.grid.items.length; i++) {
                if (this.grid.items[i]._status.Selected) {
                    this.grid.items[i][0].scrollIntoView(false);
                    break;
                }
            }
        }
    };
    //隐藏显示下拉列表
    WayDropDownList.prototype.hideList = function () {
        if (this.maskLayer)
            this.maskLayer.hide();
        if (this.itemContainer.css("visibility") == "visible") {
            this.itemContainer.css("visibility", "hidden");
        }
    };
    return WayDropDownList;
}());
var WayCheckboxList = (function () {
    function WayCheckboxList(elementid, datasource) {
        var _this = this;
        this.isMobile = false;
        this._value = [];
        this.onchange = null;
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        this.element[0]._WayControl = this;
        this.isMobile = "ontouchstart" in this.element[0];
        var itemtemplate = this.element.find("script[_for='item']")[0];
        this.valueMember = this.element[0].getAttribute("_valueMember");
        this.textMember = this.element[0].getAttribute("_textMember");
        if (true) {
            this.grid = new WayGridView(this.element[0], 0);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = function (item) { return _this._onGridItemCreated(item); };
            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");
            }
            this.grid.onAfterCreateItems = function () {
                _this.checkGridItem();
            };
            this.grid.databind();
        }
    }
    Object.defineProperty(WayCheckboxList.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            if (!v)
                v = [];
            if (v != this._value) {
                this._value = v;
                this.checkGridItem();
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    WayCheckboxList.prototype.checkGridItem = function () {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = this.grid.items[j]._status;
            var data = this.grid.items[j]._data;
            status.Selected = WayHelper.contains(this._value, data.value);
        }
    };
    WayCheckboxList.prototype.addEventListener = function (eventName, func) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }
            this.onchange.push(func);
        }
    };
    WayCheckboxList.prototype.fireEvent = function (eventName) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    };
    WayCheckboxList.prototype.rasieModelChange = function () {
        for (var k = 0; k < WayDataBindHelper.bindings.length; k++) {
            var binding = WayDataBindHelper.bindings[k];
            if (binding.element === this.element[0]) {
                for (var m = 0; m < binding.configs.length; m++) {
                    var config = binding.configs[m];
                    if (config.elementMember == "value") {
                        if (typeof binding.model.onchange == "function") {
                            binding.model.onchange(binding.model, null, config.dataMember, this._value);
                        }
                        else if (binding.model.onchange && typeof binding.model.onchange.length != "undefined") {
                            for (var i = 0; i < binding.model.onchange.length; i++) {
                                binding.model.onchange[i](binding.model, null, config.dataMember, this._value);
                            }
                        }
                    }
                }
            }
        }
    };
    WayCheckboxList.prototype._onGridItemCreated = function (item) {
        var _this = this;
        item.click(function () {
            item._status.Selected = !item._status.Selected;
            if (item._status.Selected) {
                _this._value.push(item._data.value);
                _this.fireEvent("change");
                //这里只是数值发生变化，如果有model和自己绑定，触发一下model的onchange事件
                _this.rasieModelChange();
            }
            else {
                for (var i = 0; i < _this._value.length; i++) {
                    if (_this._value[i] == item._data.value) {
                        _this._value.splice(i, 1);
                        _this.fireEvent("change");
                        _this.rasieModelChange();
                        break;
                    }
                }
            }
        });
    };
    return WayCheckboxList;
}());
var WayRadioList = (function () {
    function WayRadioList(elementid, datasource) {
        var _this = this;
        this.isMobile = false;
        this.onchange = null;
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        this.element[0]._WayControl = this;
        this.isMobile = "ontouchstart" in this.element[0];
        var itemtemplate = this.element.find("script[_for='item']")[0];
        this.valueMember = this.element[0].getAttribute("_valueMember");
        this.textMember = this.element[0].getAttribute("_textMember");
        if (true) {
            this.grid = new WayGridView(this.element[0], 0);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = function (item) { return _this._onGridItemCreated(item); };
            if (!this.valueMember || this.valueMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.valueMember + "->value");
            }
            if (!this.textMember || this.textMember == "") {
            }
            else {
                this.grid.dataMembers.push(this.textMember + "->text");
            }
            this.grid.onAfterCreateItems = function () {
                _this.checkGridItem();
            };
            this.grid.databind();
        }
    }
    Object.defineProperty(WayRadioList.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            if (v != this._value) {
                this._value = v;
                this.checkGridItem();
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    WayRadioList.prototype.checkGridItem = function () {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = this.grid.items[j]._status;
            var data = this.grid.items[j]._data;
            status.Selected = this._value == data.value;
        }
    };
    WayRadioList.prototype.addEventListener = function (eventName, func) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }
            this.onchange.push(func);
        }
    };
    WayRadioList.prototype.fireEvent = function (eventName) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    };
    WayRadioList.prototype.rasieModelChange = function () {
        for (var k = 0; k < WayDataBindHelper.bindings.length; k++) {
            var binding = WayDataBindHelper.bindings[k];
            if (binding.element === this.element[0]) {
                for (var m = 0; m < binding.configs.length; m++) {
                    var config = binding.configs[m];
                    if (config.elementMember == "value") {
                        if (typeof binding.model.onchange == "function") {
                            binding.model.onchange(binding.model, null, config.dataMember, this._value);
                        }
                        else if (binding.model.onchange && typeof binding.model.onchange.length != "undefined") {
                            for (var i = 0; i < binding.model.onchange.length; i++) {
                                binding.model.onchange[i](binding.model, null, config.dataMember, this._value);
                            }
                        }
                    }
                }
            }
        }
    };
    WayRadioList.prototype._onGridItemCreated = function (item) {
        var _this = this;
        item.click(function () {
            _this.value = item._data.value;
        });
    };
    return WayRadioList;
}());
var WayButton = (function () {
    function WayButton(elementid) {
        var _this = this;
        this.internalModel = { text: null };
        this.onchange = null;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        var _databind_internal = this.element.attr("_databind_internal");
        var _databind = this.element.attr("_databind");
        var _expression_internal = this.element.attr("_expression_internal");
        var _expression = this.element.attr("_expression");
        this.element.attr("_databind", _databind_internal);
        this.element.attr("_expression", _expression_internal);
        this.internalModel = WayDataBindHelper.dataBind(this.element[0], { text: this.element.attr("_text") }, null, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g, true);
        this.element.attr("_databind", _databind);
        this.element.attr("_expression", _expression);
        this.element[0]._WayControl = this;
        this.onclickString = this.element.attr("onclick");
        this.element.attr("onclick", null);
        if (this.onclickString && this.onclickString.length > 0) {
            var matches = this.onclickString.match(/[\W]?(this\.)/g);
            for (var i = 0; i < matches.length; i++) {
                var r = matches[i].replace("this.", "___element.");
                this.onclickString = this.onclickString.replace(matches[i], r);
            }
            this.element.click(function () {
                var ___element = _this;
                eval(_this.onclickString);
            });
        }
    }
    Object.defineProperty(WayButton.prototype, "text", {
        get: function () {
            return this.internalModel.text;
        },
        set: function (v) {
            if (v != this.internalModel.text) {
                this.internalModel.text = v;
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    WayButton.prototype.addEventListener = function (eventName, func) {
        if (eventName == "change") {
            if (!this.onchange) {
                this.onchange = [];
            }
            else if (typeof this.onchange == "function") {
                var arr = [];
                arr.push(this.onchange);
                this.onchange = arr;
            }
            this.onchange.push(func);
        }
    };
    WayButton.prototype.fireEvent = function (eventName) {
        if (eventName == "change") {
            if (this.onchange && typeof this.onchange == "function") {
                this.onchange();
            }
            else if (this.onchange) {
                for (var i = 0; i < this.onchange.length; i++) {
                    this.onchange[i]();
                }
            }
        }
    };
    return WayButton;
}());
var checkToInitWayControl = function (parentElement) {
    if (parentElement.tagName == "SCRIPT" || parentElement.tagName == "STYLE")
        return;
    if (parentElement.tagName.indexOf("Way")) {
        initWayControl(parentElement, null);
    }
    for (var i = 0; i < parentElement.children.length; i++) {
        var ele = parentElement.children[i];
        if (ele.tagName.indexOf("Way")) {
            initWayControl(ele, null);
        }
        else {
            checkToInitWayControl(ele);
        }
    }
};
var initWayControl = function (virtualEle, element) {
    if (element == null) {
        for (var i = 0; i < _styles.length; i++) {
            var _styEle = _styles[i];
            if (_styEle.tagName == virtualEle.tagName) {
                element = _styEle;
                break;
            }
        }
    }
    if (!element)
        return;
    var controlType = element.tagName;
    var replaceEleObj = $(element.innerHTML);
    checkToInitWayControl(replaceEleObj[0]);
    var style1 = virtualEle.getAttribute("style");
    var style2 = replaceEleObj.attr("style");
    if (style1) {
        if (!style2)
            style2 = "";
        replaceEleObj.attr("style", style2 + ";" + style1);
        virtualEle.removeAttribute("style");
    }
    if (replaceEleObj.attr("_databind")) {
        replaceEleObj.attr("_databind_internal", replaceEleObj.attr("_databind"));
    }
    if (replaceEleObj.attr("_expression")) {
        replaceEleObj.attr("_expression_internal", replaceEleObj.attr("_expression"));
    }
    for (var k = 0; k < virtualEle.attributes.length; k++) {
        replaceEleObj.attr(virtualEle.attributes[k].name, virtualEle.attributes[k].value);
    }
    if (virtualEle == virtualEle.parentElement.children[virtualEle.parentElement.children.length - 1]) {
        virtualEle.parentElement.removeChild(virtualEle);
        virtualEle.parentElement.appendChild(replaceEleObj[0]);
    }
    else {
        var nextlib = virtualEle.nextSibling;
        virtualEle.parentElement.insertBefore(replaceEleObj[0], nextlib);
    }
    var control = null;
    switch (controlType) {
        case "WAYDROPDOWNLIST":
            control = new WayDropDownList(replaceEleObj, replaceEleObj.attr("_datasource"));
            break;
        case "WAYCHECKBOXLIST":
            control = new WayCheckboxList(replaceEleObj, replaceEleObj.attr("_datasource"));
            break;
        case "WAYRADIOLIST":
            control = new WayRadioList(replaceEleObj, replaceEleObj.attr("_datasource"));
            break;
        case "WAYBUTTON":
            control = new WayButton(replaceEleObj);
            break;
        case "WAYGRIDVIEW":
            replaceEleObj[0].innerHTML += virtualEle.innerHTML;
            control = new WayGridView(replaceEleObj, parseInt(replaceEleObj.attr("_pagesize")));
            break;
        default:
            break;
    }
    if (control) {
        var idstr = replaceEleObj.attr("id");
        if (idstr && idstr.length > 0 && eval("!window." + idstr + " || !window." + idstr + "._WayControl")) {
            eval("window." + idstr + "=control;");
        }
    }
};
var _styles = $(WayHelper.downloadUrl("/templates/main.html"));
$(document).ready(function () {
    var body = $(document.body);
    var controllerName = body.attr("_controller");
    if (!controllerName) {
        throw "<Body>没有定义_controller";
    }
    else {
        window._controller = WayScriptRemoting.createRemotingController(controllerName);
    }
    for (var i = 0; i < _styles.length; i++) {
        var element = _styles[i];
        if (element.tagName == "STYLE") {
            document.body.appendChild(element);
        }
        else {
            var controlType = element.tagName;
            var controlEles = body.find(controlType);
            for (var j = 0; j < controlEles.length; j++) {
                var virtualEle = controlEles[j];
                initWayControl(virtualEle, element);
            }
        }
    }
});
//# sourceMappingURL=WayScriptRemoting.js.map