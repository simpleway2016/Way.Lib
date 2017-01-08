var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
window.onerror = function (msg) {
    alert(msg);
};
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
        document.cookie = name + "=" + window.escape(value);
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
                            return window.unescape(v); //返回需要提取的cookie值
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
    WayScriptRemoting.prototype.pageInvoke = function (name, parameters, callback) {
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
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        var p = "";
        for (var i = 0; i < nameAndValues.length; i += 2) {
            if (i > 0)
                p += "&";
            p += nameAndValues[i] + "=" + window.escape(nameAndValues[i + 1]);
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
            if (myurl.indexOf("?") < 0)
                myurl += "?";
            else
                myurl += "&";
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
        return new WayVirtualWebSocket(url);
        //if ((<any>window).WebSocket) {
        //    return new WebSocket(url);
        //}
        //else {
        //    return <any>new WayVirtualWebSocket(url);
        //}
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
        this.element = _element;
        this.model = _model;
        this.dataSource = _dataSource;
        this.container = $(_element);
        var elements = this.container.find("*[_databind]");
        if (this.container[0].getAttribute("_databind")) {
            this.initEle(this.container[0], _dataSource, expressionExp, dataMemberExp);
        }
        for (var i = 0; i < elements.length; i++) {
            var ctrlEle = elements[i];
            this.initEle(ctrlEle, _dataSource, expressionExp, dataMemberExp);
        }
    }
    WayBindingElement.prototype.initEle = function (ctrlEle, _dataSource, expressionExp, dataMemberExp) {
        var _this = this;
        var _databind = ctrlEle.getAttribute("_databind");
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
                            eval("ctrlEle." + eleMember + "=_dataSource." + dataMember + ";");
                            if (eleMember == "value" || eleMember == "checked") {
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
    };
    WayBindingElement.prototype.onvalueChanged = function (fromWhichConfig) {
        try {
            if (this.configs.length == 0)
                return; //绑定已经移除了
            if (fromWhichConfig.elementMember == "value") {
                var model = this.model;
                eval("model." + fromWhichConfig.dataMember + "=" + JSON.stringify(fromWhichConfig.element.value) + ";");
            }
            else if (fromWhichConfig.elementMember == "checked") {
                var model = this.model;
                eval("model." + fromWhichConfig.dataMember + "=" + JSON.stringify(fromWhichConfig.element.checked) + ";");
            }
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
                    eval("config.element." + config.elementMember + "=" + JSON.stringify(value) + ";");
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
                if (pvalue != null && typeof pvalue == "object") {
                    str += pro + ":" + WayDataBindHelper.getObjectStr(pvalue, onchangeMembers, parent + pro + ".");
                    continue;
                }
                var onchangeStr;
                if (WayHelper.contains(onchangeMembers, parent + pro)) {
                    onchangeStr = "onProChange(true," + getmodelStr + ",item,itemIndex,'" + parent + pro + "',v);";
                }
                else {
                    onchangeStr = "onProChange(false," + getmodelStr + ",item,itemIndex,'" + parent + pro + "',v);";
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
    WayDataBindHelper.cloneObjectForBind = function (obj, _itemIndex, onchangeMembers, _onchange) {
        if (obj.getSource && typeof obj.getSource == "function") {
            obj = obj.getSource();
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
        if (toCheckedEles) {
            for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
                var binding = WayDataBindHelper.bindings[i];
                if (binding && binding.model == model) {
                    binding.onchange(itemIndex, name, value);
                    break;
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
    WayDataBindHelper.dataBind = function (element, data, tag, expressionExp, dataMemberExp) {
        if (tag === void 0) { tag = null; }
        if (expressionExp === void 0) { expressionExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g; }
        if (dataMemberExp === void 0) { dataMemberExp = /\@(\w|\.)+/g; }
        if (typeof element == "string") {
            element = document.getElementById(element);
        }
        var bindingInfo = new WayBindingElement(element, null, data, expressionExp, dataMemberExp);
        var onchangeMembers = bindingInfo.getDataMembers();
        var model = WayDataBindHelper.cloneObjectForBind(data, tag, onchangeMembers, WayDataBindHelper.onchange);
        var finded = false;
        bindingInfo.model = model;
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
            "transform": "scale(0.5)"
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
                    "position": "absolute"
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
            "top": y + "px",
            "position": "absolute"
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
    WayDBContext.prototype.getDatas = function (pageinfo, bindFields, searchModel, callback) {
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
        });
    };
    WayDBContext.prototype.getDataItem = function (bindFields, searchModel, callback) {
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
        });
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
    function WayGridView(elementId, controller, _pagesize) {
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
        //是否支持下拉刷新
        //下拉刷新必须定义body模板
        this.supportDropdownRefresh = false;
        //定义item._status的数据原型，可以修改此原型达到期望的目的
        this.itemStatusModel = { Selected: false };
        //是否使用翻页模式
        this.pageMode = false;
        //pageMode模式下，预先加载多少页数据
        this.preLoadNumForPageMode = 1;
        try {
            this.dbContext = new WayDBContext(controller, null);
            this.element = $("#" + elementId);
            this.element.css({
                "overflow-y": "auto",
                "-webkit-overflow-scrolling": "touch"
            });
            var isTouch = "ontouchstart" in this.element[0];
            if (!isTouch)
                this.supportDropdownRefresh = false;
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
                _this.search();
            }
        }, true);
    };
    WayGridView.prototype.search = function () {
        this.databind();
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
            this.dbContext.getDatas(this.pageinfo, this.getBindFields(), this.searchModel, function (ret, pkid, err) {
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
        this.hasMorePage = pageData.length >= this.pageinfo.PageSize;
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
        var expression = /\{[ ]?\$(\w+)[ ]?\}/g;
        var itemContent = template.content;
        while (true) {
            var r = expression.exec(template.content);
            if (!r)
                break;
            var proname = r[1];
            if (proname == "ItemIndex") {
                itemContent = itemContent.replace(r[0], itemIndex);
            }
            else {
                if (eval("typeof statusmodel." + proname + "=='undefined'") == false) {
                    itemContent = itemContent.replace(r[0], eval("statusmodel." + proname));
                }
                else {
                    itemContent = itemContent.replace(r[0], "null");
                }
            }
        }
        var match = itemContent.match(this.fieldExp);
        if (match) {
            for (var j = 0; j < match.length; j++) {
                var str = match[j].toString();
                var field = str.substr(2, str.length - 3);
                if (field.indexOf(":") > 0) {
                    field = field.substr(0, field.indexOf(":"));
                    itemContent = itemContent.replace(str, eval("data." + field + ".text"));
                }
                else {
                    var value = eval("data." + field);
                    if (value == null || typeof value == "undefined")
                        value = "";
                    if (typeof value == "object") {
                        if (typeof value.caption != "undefined") {
                            itemContent = itemContent.replace(str, value.caption);
                        }
                        else if (typeof value.value != "undefined") {
                            itemContent = itemContent.replace(str, value.value);
                        }
                        else {
                            itemContent = itemContent.replace(str, "");
                        }
                    }
                    else {
                        itemContent = itemContent.replace(str, value);
                    }
                }
            }
        }
        var item = $(itemContent);
        var model = WayDataBindHelper.dataBind(item[0], data, itemIndex, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g);
        //创建status
        var myChangeFunc = statusmodel.onchange;
        var statusData = WayHelper.clone(statusmodel);
        item._status = WayDataBindHelper.dataBind(item[0], statusData, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g);
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
//# sourceMappingURL=WayScriptRemoting.js.map