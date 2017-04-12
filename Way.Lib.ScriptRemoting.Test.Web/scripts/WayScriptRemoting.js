var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
;
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
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["RSADecrptError"] = 6] = "RSADecrptError";
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
                var cookieArr = cookieStr.split(";");
                for (var i = 0; i < cookieArr.length; i++) {
                    var cookieVal = cookieArr[i].split("=");
                    if (cookieVal[0].trim() == name) {
                        var v = cookieVal[1].trim();
                        if (v != "") {
                            return window.decodeURIComponent(v, "utf-8");
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
var RSAInfo = (function () {
    function RSAInfo() {
    }
    return RSAInfo;
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
        invoker.Post({
            m: {
                Action: 'init',
                ClassFullName: remoteName,
                SessionID: WayCookie.getCookie("WayScriptRemoting")
            }
        });
        if (hasErr) {
            throw hasErr;
        }
        else if (result.err) {
            throw result.err;
        }
        var func;
        eval("func = " + result.text);
        var page = new func(remoteName);
        page.rsa = result.rsa;
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
            ws.onerror = null;
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
                    page.rsa = result.rsa;
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
    WayScriptRemoting.prototype._uploadFileWithHTTP = function (fileElement, state, callback, handler) {
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
            this.pageInvoke("UploadFileWithHTTP", [file.name, state, size, handler.offset], function (ret, err) {
                if (err) {
                    if (callback) {
                        callback(null, 0, 0, err);
                    }
                }
                else {
                    _this.sendFileWithHttp(ret, state, file, reader, size, handler.offset, 10240, callback, handler);
                }
            });
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
    WayScriptRemoting.prototype.arrayBufferToString = function (data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }
        return str;
    };
    WayScriptRemoting.prototype.sendFileWithHttp = function (tranid, state, file, reader, size, start, len, callback, handler) {
        var _this = this;
        if (start + len > size) {
            len = size - start;
        }
        var blob = file.slice(start, start + len);
        reader.onload = function () {
            var filedata = reader.result;
            if (filedata.byteLength > 0) {
                start += filedata.byteLength;
                if (handler.abort) {
                }
                else {
                    try {
                        filedata = _this.arrayBufferToString(filedata);
                        _this.pageInvoke("GettingFileDataWithHttp", [tranid, filedata], function (ret, err) {
                            if (err) {
                                if (err.indexOf("tranid not exist") >= 0) {
                                    _this._uploadFileWithHTTP(file, state, callback, handler);
                                }
                                else {
                                    setTimeout(function () { _this.sendFileWithHttp(tranid, state, file, reader, size, start, len, callback, handler); }, 1000);
                                }
                            }
                            else {
                                handler.offset = ret.offset;
                                if (callback) {
                                    callback(ret.size == ret.offset ? "ok" : "", ret.size, ret.offset, null);
                                }
                                if (ret.offset < ret.size) {
                                    _this.sendFileWithHttp(tranid, state, file, reader, size, ret.offset, len, callback, handler);
                                }
                            }
                        });
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
    WayScriptRemoting.prototype.uploadFile = function (fileElement, state, callback, handler) {
        var _this = this;
        if (!window.WebSocket) {
            return this._uploadFileWithHTTP(fileElement, state, callback, null);
        }
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
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "',State:" + JSON.stringify(state) + ",'FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + _this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
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
                        _this.uploadFile(file, state, callback, handler);
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
                        if (handler.abort == false) {
                            _this.uploadFile(file, state, callback, handler);
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
    WayScriptRemoting.prototype.encrypt = function (value) {
        setMaxDigits(129);
        value = window.encodeURIComponent(value, "utf-8");
        var key = new RSAKeyPair(this.rsa.Exponent, "", this.rsa.Modulus);
        if (value.length <= 110) {
            return encryptedString(key, value);
        }
        else {
            var result = "";
            var total = value.length;
            for (var i = 0; i < value.length; i += 110) {
                var text = value.substr(i, Math.min(110, total));
                total -= text.length;
                result += encryptedString(key, text);
            }
            return result;
        }
    };
    WayScriptRemoting.prototype.pageInvoke = function (name, parameters, callback, async, useRsa, returnUseRsa) {
        var _this = this;
        if (async === void 0) { async = true; }
        if (useRsa === void 0) { useRsa = false; }
        if (returnUseRsa === void 0) { returnUseRsa = false; }
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
                    if (returnUseRsa && ret.indexOf("{") != 0) {
                        setMaxDigits(129);
                        var rsakey = new RSAKeyPair("", _this.rsa.Exponent, _this.rsa.Modulus);
                        ret = decodeURIComponent(decryptedString(rsakey, ret));
                    }
                    eval("resultObj=" + ret);
                    if (resultObj.type == WayScriptRemotingMessageType.Result) {
                        callback(resultObj.result, null);
                    }
                    else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                        callback(null, resultObj.result);
                    }
                    else if (resultObj.type == WayScriptRemotingMessageType.RSADecrptError) {
                        _this.rsa = resultObj.result;
                        _this.pageInvoke(name, parameters, callback, async, useRsa);
                    }
                }
            };
            if (useRsa) {
                paramerStr = "\"" + this.encrypt(paramerStr) + "\"";
            }
            invoker.Post({ m: "{'ClassFullName':'" + this.classFullName + "','MethodName':'" + name + "','Parameters':[" + paramerStr + "] , 'SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}" });
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
    WayScriptRemoting.ServerAddress = null;
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
        invoker.Post({ "mode": "init" });
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
        invoker.Post({
            "mode": "send",
            "data": data,
            "id": this.guid,
            "binaryType": this.binaryType
        });
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
                _this.lastMessage = result;
                if (_this._onmessage && _this.status == WayVirtualWebSocketStatus.connected) {
                    _this._onmessage({ data: _this.lastMessage });
                }
                if (_this.status == WayVirtualWebSocketStatus.connected) {
                    _this.receiveChannelConnect();
                }
            }
        };
        this.receiver.Post({
            "mode": "receive",
            "id": this.guid,
            "binaryType": this.binaryType
        });
        setTimeout(function () { return _this.sendHeart(); }, 30000);
    };
    WayVirtualWebSocket.prototype.sendHeart = function () {
        var _this = this;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            var invoker = new WayScriptInvoker(this.url);
            invoker.Post({
                "mode": "heart",
                "id": this.guid
            });
            setTimeout(function () { return _this.sendHeart(); }, 30000);
        }
    };
    return WayVirtualWebSocket;
}());
var WayScriptInvoker = (function () {
    function WayScriptInvoker(_url) {
        this.async = true;
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
    WayScriptInvoker.prototype.Post = function (obj) {
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
    WayScriptInvoker.prototype.Get = function (nameAndValues) {
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
    return WayScriptInvoker;
}());
var WayTemplate = (function () {
    function WayTemplate(_content, _match, mode) {
        if (_match === void 0) { _match = null; }
        if (mode === void 0) { mode = ""; }
        this.content = _content;
        this.match = _match;
        this.mode = mode ? mode : "";
    }
    return WayTemplate;
}());
var WayHelper = (function () {
    function WayHelper() {
    }
    WayHelper.contains = function (arr, value) {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value)
                return true;
        }
        return false;
    };
    WayHelper.getPropertyName = function (obj, index) {
        var i = 0;
        for (var p in obj) {
            if (i == index)
                return p;
            i++;
        }
        return null;
    };
    WayHelper.createWebSocket = function (url) {
        if (window.WebSocket) {
            return new WebSocket(url);
        }
        else {
            return new WayVirtualWebSocket(url);
        }
    };
    WayHelper.setTouchFireClickEvent = function (element, handler) {
        if (!("ontouchstart" in element))
            return;
        var touchPoint;
        var canBeClick;
        element.addEventListener("touchstart", function (e) {
            canBeClick = true;
            touchPoint = {
                x: e.touches[0].clientX,
                y: e.touches[0].clientY,
                time: new Date().getTime()
            };
        });
        element.addEventListener("touchmove", function (e) {
            var x = e.touches[0].clientX;
            var y = e.touches[0].clientY;
            if (Math.abs(x - touchPoint.x) > window.innerWidth / 15 || Math.abs(y - touchPoint.y) > window.innerHeight / 15) {
                canBeClick = false;
            }
        });
        element.addEventListener("touchend", function (e) {
            if (canBeClick && (new Date().getTime() - touchPoint.time) < 300) {
                e.preventDefault();
                e.stopPropagation();
                if (handler) {
                    handler();
                }
                else {
                    if (element.fireEvent)
                        element.fireEvent("click");
                    else
                        element.click();
                }
            }
            canBeClick = false;
        });
    };
    WayHelper.writePage = function (url) {
        document.write(WayHelper.downloadUrl(url));
    };
    WayHelper.downloadUrl = function (url) {
        var invoker = new WayScriptInvoker(url);
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
    WayHelper.findBindingElements = function (element) {
        var result = [];
        WayHelper.findInnerBindingElements(result, element);
        return result;
    };
    WayHelper.findInnerBindingElements = function (result, element) {
        var attr = element.getAttribute("databind");
        if (attr && attr.length > 0) {
            result.push(element);
        }
        else {
            attr = element.getAttribute("expression");
            if (attr && attr.length > 0) {
                result.push(element);
            }
        }
        if (element.tagName.indexOf("Way") == 0 || element.WayControl) {
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
    WayHelper.fireEvent = function (el, eventName) {
        if (eventName.indexOf("on") == 0)
            eventName = eventName.substr(2);
        var evt;
        if (document.createEvent) {
            evt = document.createEvent("HTMLEvents");
            evt.initEvent(eventName, true, true);
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
var WayObserveObject = (function () {
    function WayObserveObject(data, parent, parentname) {
        if (parent === void 0) { parent = null; }
        if (parentname === void 0) { parentname = null; }
        this.__onchanges = [];
        this.__objects = {};
        if (data instanceof WayObserveObject) {
            var old = data;
            this.addEventListener("change", function (_model, _name, _value) {
                old.__changed(_name, _value);
            });
            data = old.__data;
        }
        this.__data = data;
        this.__parent = parent;
        this.__parentName = parentname;
        for (var p in data) {
            this.__addProperty(p);
        }
    }
    WayObserveObject.prototype.addNewProperty = function (proName, value) {
        this.__data[proName] = value;
        var type = typeof value;
        if (value == null || value instanceof Array || type != "object") {
            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName, value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;
                            }
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    };
    WayObserveObject.prototype.__addProperty = function (proName) {
        var type = typeof this.__data[proName];
        if (type == "object" && !(this.__data[proName] instanceof Array)) {
            this[proName] = new WayObserveObject(this.__data[proName], this, proName);
        }
        else if (type != "function") {
            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName, value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;
                            }
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    };
    WayObserveObject.prototype.addEventListener = function (name, func) {
        if (name == "change") {
            this.__onchanges.push(func);
        }
    };
    WayObserveObject.prototype.removeEventListener = function (name, func) {
        if (name == "change") {
            for (var i = 0; i < this.__onchanges.length; i++) {
                if (this.__onchanges[i] == func) {
                    this.__onchanges[i] = null;
                }
            }
        }
    };
    WayObserveObject.prototype.__changed = function (name, value) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this, name, value);
            }
        }
    };
    return WayObserveObject;
}());
var WayBindingElement = (function (_super) {
    __extends(WayBindingElement, _super);
    function WayBindingElement(_element, _model, expressionExp, dataMemberExp) {
        _super.call(this);
        this.configs = [];
        this.expressionConfigs = [];
        this.element = _element;
        this.model = _model;
        var elements = WayHelper.findBindingElements(_element);
        for (var i = 0; i < elements.length; i++) {
            var ctrlEle = elements[i];
            this.initEle(ctrlEle, expressionExp, dataMemberExp);
        }
    }
    WayBindingElement.prototype.initEle = function (ctrlEle, expressionExp, dataMemberExp) {
        var _this = this;
        var databind = ctrlEle.getAttribute("databind");
        var _expressionString = ctrlEle.getAttribute("expression");
        var isWayControl = false;
        if (ctrlEle.WayControl) {
            ctrlEle = ctrlEle.WayControl;
            isWayControl = true;
        }
        if (databind) {
            var matchs = databind.match(expressionExp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    if (match && match.indexOf("=") > 0) {
                        var eleMember = match.match(/(\w|\.)+( )?\=/g)[0];
                        eleMember = eleMember.match(/(\w|\.)+/g)[0];
                        var dataMember = match.match(dataMemberExp)[0];
                        dataMember = dataMember.substr(1);
                        if (this.model) {
                            var fields = dataMember.split('.');
                            var findingObj = this.model;
                            for (var k = 0; k < fields.length; k++) {
                                var field = fields[k];
                                if (field.length == 0)
                                    break;
                                if (k < fields.length - 1) {
                                    var isUndefined = eval("typeof findingObj." + field + "!='object'");
                                    if (isUndefined) {
                                        findingObj.addNewProperty(field, new WayObserveObject({}, findingObj, field));
                                    }
                                    findingObj = findingObj[field];
                                }
                                else {
                                    var isUndefined = eval("typeof findingObj." + field + "=='undefined'");
                                    if (isUndefined) {
                                        findingObj.addNewProperty(field, null);
                                    }
                                }
                            }
                        }
                        if (this.model) {
                            var isObject = this.model[dataMember] && typeof this.model[dataMember] == 'object' && typeof this.model[dataMember]["value"] != "undefined";
                            if (isObject) {
                                dataMember += ".value";
                            }
                        }
                        var config = new WayBindMemberConfig(eleMember, dataMember, ctrlEle);
                        this.configs.push(config);
                        ctrlEle.data = this.model;
                        if (this.model) {
                            var addevent = false;
                            if (isWayControl) {
                                if (ctrlEle.memberInChange && WayHelper.contains(ctrlEle.memberInChange, eleMember))
                                    addevent = true;
                            }
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
                ctrlEle.expressionDatas.push({ exp: dataMemberExp, data: this.model.__data });
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
                return;
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
    WayDataBindHelper.removeDataBind = function (element) {
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] != null && WayDataBindHelper.bindings[i].element == element) {
                WayDataBindHelper.bindings[i].model.removeEventListener("change", WayDataBindHelper.bindings[i]._changefunc);
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
            }
        }
    };
    WayDataBindHelper.getBindingFields = function (element, expressionExp, dataMemberExp) {
        if (expressionExp === void 0) { expressionExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g; }
        if (dataMemberExp === void 0) { dataMemberExp = /\@(\w|\.)+/g; }
        if (typeof element == "string") {
            element = document.getElementById(element);
        }
        var bindingInfo = new WayBindingElement(element, null, expressionExp, dataMemberExp);
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
        else if (element.element && element.element instanceof jQuery) {
            element = element.element[0];
        }
        var model = null;
        if (!data)
            data = {};
        if (data instanceof WayObserveObject) {
            model = data;
            data = model.__data;
        }
        else {
            model = new WayObserveObject(data);
        }
        var bindingInfo = new WayBindingElement(element, model, expressionExp, dataMemberExp);
        bindingInfo._changefunc = function (datamodel, proname, value) {
            bindingInfo.onchange(tag, proname, value);
        };
        model.addEventListener("change", bindingInfo._changefunc);
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
            this.control.shouldLoadMorePage(-1);
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
                var x, y;
                x = this.lastMouseDownLocation.x - 50;
                y = this.lastMouseDownLocation.y;
                loadele.css({
                    "left": x + "px",
                    "top": y + "px",
                });
            }
            return;
        }
        this.showRef++;
        this.timingNumber = setTimeout(function () {
            if (_this.timingNumber) {
                _this.timingNumber = 0;
                var offset = centerElement.offset();
                var x, y;
                if (_this.lastMouseDownTime && false) {
                    if (new Date().getTime() - _this.lastMouseDownTime < 1000) {
                        x = _this.lastMouseDownLocation.x - 50;
                        y = _this.lastMouseDownLocation.y + 30;
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
var WayControlBase = (function (_super) {
    __extends(WayControlBase, _super);
    function WayControlBase() {
        _super.apply(this, arguments);
    }
    return WayControlBase;
}(WayBaseObject));
var WayGridView = (function (_super) {
    __extends(WayGridView, _super);
    function WayGridView(elementId, configElement) {
        _super.call(this);
        this.memberInChange = [];
        this.itemTemplates = [];
        this.items = [];
        this.originalItems = [];
        this.pageinfo = new WayPageInfo();
        this.fieldExp = /\{\@(\w|\.|\:)+\}/g;
        this.loading = new WayProgressBar("#cccccc");
        this.transcationID = 1;
        this.dataMembers = [];
        this.supportDropdownRefresh = false;
        this.itemStatusModel = { Selected: false };
        this.pageMode = false;
        this.preloadedMaxPageIndex = 0;
        this.preLoadNumForPageMode = 1;
        this.searchModel = {};
        this.allowEdit = false;
        this.initedPageMode = false;
        try {
            var controller = document.body.getAttribute("controller");
            this.dbContext = new WayDBContext(controller, null);
            if (typeof elementId == "string")
                this.element = $("#" + elementId);
            else if (elementId.tagName)
                this.element = $(elementId);
            else
                this.element = elementId;
            var configElementObj;
            if (configElement)
                configElementObj = $(configElement);
            else
                configElementObj = this.element;
            if (!this.element[0].WayControl) {
                this.element[0].WayControl = this;
            }
            var searchid = this.element.attr("searchid");
            if (searchid && searchid.length > 0) {
                try {
                    this.searchModel = WayDataBindHelper.dataBind(searchid, {});
                }
                catch (e) {
                }
            }
            this.allowEdit = this.element.attr("allowedit") == "true";
            this.element.css({
                "overflow-y": "auto",
                "-webkit-overflow-scrolling": "touch"
            });
            var isTouch = "ontouchstart" in this.element[0];
            if (!isTouch)
                this.supportDropdownRefresh = false;
            this.datasource = this.element.attr("datasource");
            this.pagesize = parseInt(this.element.attr("pagesize"));
            if (isNaN(this.pagesize)) {
                this.pagesize = 10;
            }
            this.pager = new WayPager(this.element, this);
            var bodyTemplate = configElementObj.find("script[for='body']");
            var templates = configElementObj.find("script");
            this.itemContainer = this.element;
            if (bodyTemplate.length > 0) {
                this.bodyTemplateHtml = bodyTemplate[0].innerHTML;
                this.itemContainer = $(this.bodyTemplateHtml);
                this.element[0].appendChild(this.itemContainer[0]);
                this.initRefreshEvent(this.itemContainer);
            }
            else {
                this.supportDropdownRefresh = false;
            }
            if (this.itemContainer[0].children.length > 0 && this.itemContainer[0].children[0].tagName == "TBODY") {
                this.itemContainer = $(this.itemContainer[0].children[0]);
            }
            for (var i = 0; i < templates.length; i++) {
                var templateItem = templates[i];
                templateItem.parentElement.removeChild(templateItem);
                var _forWhat = templateItem.getAttribute("for");
                if (_forWhat == "header") {
                    this.header = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_forWhat == "footer") {
                    this.footer = new WayTemplate(this.getTemplateOuterHtml(templateItem));
                }
                else if (_forWhat == "item") {
                    var mode = templateItem.getAttribute("mode");
                    var match = templateItem.getAttribute("match");
                    var temp = new WayTemplate(this.getTemplateOuterHtml(templateItem), match, mode);
                    this.addItemTemplate(temp);
                }
            }
        }
        catch (e) {
            throw "WayGridView构造函数错误，" + e.message;
        }
    }
    Object.defineProperty(WayGridView.prototype, "pagesize", {
        get: function () {
            return this.pageinfo.PageSize;
        },
        set: function (value) {
            this.pageinfo.PageSize = value;
        },
        enumerable: true,
        configurable: true
    });
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
    WayGridView.prototype.initRefreshEvent = function (touchEle) {
        var _this = this;
        var isTouch = "ontouchstart" in this.itemContainer[0];
        if (!isTouch)
            this.supportDropdownRefresh = false;
        var moving = false;
        var isTouchToRefresh = false;
        $(touchEle[0].parentElement).css({
            "transform-style": "preserve-3d",
            "-webkit-transform-style": "preserve-3d",
            "-moz-transform-style": "preserve-3d",
        });
        touchEle.css({
            "transition-property": "transform",
            "-moz-transition-property": "-moz-transform",
            "-webkit-transition-property": "-webkit-transform",
        });
        var point;
        WayHelper.addEventListener(touchEle[0], isTouch ? "touchstart" : "mousedown", function (e) {
            if (!_this.supportDropdownRefresh || _this.pageMode)
                return;
            isTouchToRefresh = false;
            if (_this.element.scrollTop() > 0) {
                return;
            }
            if (!isTouch) {
                if (window.captureEvents) {
                    window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
                }
                else
                    _this.element[0].setCapture();
            }
            e = e || window.event;
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY
            };
            moving = true;
        }, true);
        WayHelper.addEventListener(touchEle[0], isTouch ? "touchmove" : "mousemove", function (e) {
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
                    y = "translate3d(0," + y + "px,0)";
                    touchEle.css({
                        "-webkit-transform": y,
                        "-moz-transform": y,
                        "-o-transform": y,
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
                if (!isTouch) {
                    if (window.releaseEvents) {
                        window.releaseEvents(Event.MOUSEMOVE | Event.MOUSEUP);
                    }
                    else
                        _this.element[0].releaseCapture();
                }
                e = e || window.event;
                var y = isTouch ? e.changedTouches[0].clientY : e.clientY;
                y = (y - point.y);
                isTouchToRefresh = (y > _this.element.height() * 0.15);
                var desLocation = "translate3d(0,0,0)";
                touchEle.css({
                    "-moz-transition": "-moz-transform 0.5s",
                    "-webkit-transition": "-webkit-transform 0.5s",
                    "-o-transition": "-o-transform 0.5s",
                    "transition": "transform 0.5s",
                    "-moz-transform": desLocation,
                    "-webkit-transform": desLocation,
                    "-o-transform": desLocation,
                    "transform": desLocation
                });
            }
        };
        WayHelper.addEventListener(touchEle[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);
        var touchcancelFunc = function () {
            isTouchToRefresh = false;
            var desLocation = "translate3d(0,0,0)";
            touchEle.css({
                "-moz-transition": "-moz-transform 0.5s",
                "-webkit-transition": "-webkit-transform 0.5s",
                "-o-transition": "-o-transform 0.5s",
                "transition": "transform 0.5s",
                "-moz-transform": desLocation,
                "-webkit-transform": desLocation,
                "-o-transform": desLocation,
                "transform": desLocation
            });
        };
        touchEle[0].ontouchcancel = touchcancelFunc;
        var transitionendFunc = function (e) {
            touchEle.css({
                "-moz-transition": "",
                "-webkit-transition": "",
                "-o-transition": "",
                "transition": "",
            });
            if (isTouchToRefresh) {
                _this.databind();
            }
        };
        WayHelper.addEventListener(touchEle[0], "transitionend", transitionendFunc, true);
        WayHelper.addEventListener(touchEle[0], "webkitTransitionEnd", transitionendFunc, true);
    };
    WayGridView.prototype.showLoading = function (centerElement) {
        this.loading.show(centerElement);
    };
    WayGridView.prototype.hideLoading = function () {
        this.loading.hide();
    };
    WayGridView.prototype.getTemplateOuterHtml = function (element) {
        return element.innerHTML;
    };
    WayGridView.prototype.addItemTemplate = function (temp) {
        this.itemTemplates.push(temp);
    };
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
        this.showLoading(this.element);
        this.dbContext.count((this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data, function (data, err) {
            _this.hideLoading();
            callback(data, err);
        });
    };
    WayGridView.prototype.sum = function (fields, callback) {
        var _this = this;
        this.showLoading(this.element);
        this.dbContext.sum(fields, (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data, function (data, err) {
            _this.hideLoading();
            callback(data, err);
        });
    };
    WayGridView.prototype.save = function (itemIndex, callback) {
        var _this = this;
        if (this.allowEdit == false) {
            callback(null, "此WayGridView未设置为可编辑,请设置allowedit=\"true\"");
            return;
        }
        var item = this.items[itemIndex];
        var model = item.data;
        var data = this.originalItems[itemIndex];
        var changedData = WayHelper.getDataForDiffent(data, model);
        if (changedData) {
            if (this.primaryKey && this.primaryKey.length > 0) {
                eval("changedData." + this.primaryKey + "=model." + this.primaryKey + ";");
            }
            this.showLoading(this.element);
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
    WayGridView.prototype.databind = function () {
        if (!this._datasource || (typeof this._datasource == "string" && this._datasource.length == 0))
            return;
        if (this.pageMode) {
            this.initForPageMode();
        }
        this.footerItem = null;
        for (var i = 0; i < this.items.length; i++) {
            WayDataBindHelper.removeDataBind(this.items[i][0]);
        }
        this.items = [];
        this.originalItems = [];
        while (this.itemContainer[0].children.length > 0) {
            this.itemContainer[0].removeChild(this.itemContainer[0].children[0]);
        }
        if (this.pageMode) {
            var x = "translate3d(0,0,0)";
            this.itemContainer.css({
                "-webkit-transform": x,
                "-moz-transform": x,
                "transform": x
            });
        }
        if (this.header) {
            var headerObj = $(this.header.content);
            this.itemContainer.append(headerObj);
        }
        if (this.onDatabind) {
            this.onDatabind();
        }
        this.hasMorePage = true;
        this.pageinfo.PageIndex = 0;
        this.shouldLoadMorePage(0);
    };
    WayGridView.prototype.shouldLoadMorePage = function (pageindex) {
        var _this = this;
        if (pageindex == -1)
            pageindex = this.pageinfo.PageIndex;
        this.hasMorePage = false;
        var pageData;
        this.transcationID++;
        var mytranId = this.transcationID;
        var info = new WayPageInfo();
        info.PageSize = this.pageinfo.PageSize;
        info.PageIndex = pageindex;
        if (typeof this.datasource == "string") {
            this.showLoading(this.element);
            this.dbContext.getDatas(info, this.getBindFields(), (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel.__data, function (ret, pkid, err) {
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
                    _this.bindDataToGrid(pageData, pageindex);
                }
            });
        }
        else {
            pageData = this.pageinfo.PageSize > 0 ? this.getDataByPagesize(this.datasource, info) : this.datasource;
            this.bindDataToGrid(pageData, pageindex);
        }
    };
    WayGridView.prototype.bindDataToGrid = function (pageData, pageindex) {
        this.binddatas(pageData, pageindex);
        if (!this.pageMode) {
            this.pageinfo.PageIndex = pageindex + 1;
        }
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
                this.preLoadPage();
            }
            else {
                if (this.element[0].scrollHeight <= this.element.height() * 1.1) {
                    this.shouldLoadMorePage(this.pageinfo.PageIndex);
                }
            }
        }
    };
    WayGridView.prototype.getDataByPagesize = function (datas, pageinfo) {
        if (datas.length <= pageinfo.PageSize)
            return datas;
        var result = [];
        var end = pageinfo.PageSize * (pageinfo.PageIndex + 1);
        for (var i = pageinfo.PageSize * pageinfo.PageIndex; i < end && i < datas.length; i++) {
            result.push(datas[i]);
        }
        return result;
    };
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
    WayGridView.prototype.changeMode = function (itemIndex, mode) {
        try {
            var item = this.items[itemIndex];
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
    WayGridView.prototype.acceptItemChanged = function (itemIndex) {
        var item = this.items[itemIndex];
        var mydata = item.data.getSource();
        this.originalItems[itemIndex] = WayHelper.clone(mydata);
    };
    WayGridView.prototype.rebindItemFromServer = function (itemIndex, mode, callback) {
        var _this = this;
        if (callback === void 0) { callback = null; }
        var searchmodel = {};
        var item = this.items[itemIndex];
        searchmodel[this.primaryKey] = item.data[this.primaryKey];
        this.dbContext.getDataItem(this.getBindFields(), searchmodel, function (data, err) {
            if (!err) {
                _this.originalItems[itemIndex] = data;
                if (typeof mode == "undefined")
                    mode = item.mode;
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
                    var value = eval("data." + field + ".text");
                    if (!value)
                        value = "";
                    result = result.replace(str, value);
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
        if (container.tagName.indexOf("Way") != 0 && !container.WayControl) {
            for (var i = 0; i < container.childNodes.length; i++) {
                var node = container.childNodes[i];
                if (node.nodeType == 3) {
                    var attValue = node.data;
                    var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
                    if (attValue != formatvalue) {
                        node.data = formatvalue;
                    }
                }
                else if (node.nodeType == 1) {
                    this.replaceVariable(node, itemIndex, statusmodel, data);
                }
            }
        }
    };
    WayGridView.prototype.createItem = function (itemIndex, mode) {
        if (mode === void 0) { mode = ""; }
        var status;
        if (itemIndex < this.items.length) {
            status = this.items[itemIndex].status;
        }
        else {
            status = new WayObserveObject(WayHelper.clone(this.itemStatusModel));
        }
        var data = WayHelper.clone(this.originalItems[itemIndex]);
        var template = this.findItemTemplate(data, mode);
        var itemContent = template.content;
        var item = $(itemContent);
        this.replaceVariable(item[0], itemIndex, status, data);
        for (var i = 0; i < item[0].children.length; i++) {
            checkToInitWayControl(item[0].children[i]);
        }
        var model = WayDataBindHelper.dataBind(item[0], data, itemIndex, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g);
        item.status = WayDataBindHelper.dataBind(item[0], status, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g, true);
        item.data = model;
        item.mode = mode;
        if (this.onCreateItem) {
            this.onCreateItem(item, mode);
        }
        return item;
    };
    WayGridView.prototype.addItem = function (data) {
        this.originalItems.push(data);
        var itemindex = this.items.length;
        var item = this.createItem(itemindex);
        if (this.footerItem) {
            item.insertBefore(this.footerItem);
        }
        else {
            this.itemContainer.append(item);
        }
        this.items.push(item);
        return item;
    };
    WayGridView.prototype.binddatas = function (datas, pageindex) {
        if (this.pageMode) {
            this.binddatas_pageMode(datas, pageindex);
            return;
        }
        try {
            for (var i = 0; i < datas.length; i++) {
                this.addItem(datas[i]);
            }
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
        if (this.initedPageMode)
            return;
        this.initedPageMode = true;
        if (this.itemContainer[0] != this.element[0]) {
            this.itemContainer[0].parentElement.removeChild(this.itemContainer[0]);
        }
        this.itemContainer = $(document.createElement("DIV"));
        this.element[0].appendChild(this.itemContainer[0]);
        this.element.css({
            "overflow": "hidden"
        });
        this.element.css({
            "transform-style": "preserve-3d",
            "-webkit-transform-style": "preserve-3d",
            "-moz-transform-style": "preserve-3d",
        });
        this.itemContainer.css({
            "height": "100%",
            "position": "relative",
            "transition-property": "transform",
            "-moz-transition-property": "-moz-transform",
            "-webkit-transition-property": "-webkit-transform",
        });
        var isTouch = "ontouchstart" in this.itemContainer[0];
        var point;
        var moving;
        this.element[0].ontouchstart = null;
        this.element[0].ontouchend = null;
        this.element[0].ontouchmove = null;
        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", function (e) {
            e = e || window.event;
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY,
                left: -_this.pageinfo.ViewingPageIndex * _this.element.width(),
                time: new Date().getTime(),
            };
            if (!isTouch) {
                if (window.captureEvents) {
                    window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
                }
                else
                    _this.element[0].setCapture();
            }
            moving = true;
        }, true);
        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", function (e) {
            if (moving) {
                e = e || window.event;
                var x = isTouch ? e.touches[0].clientX : e.clientX;
                x = x - point.x;
                if (x > 0 && _this.pageinfo.ViewingPageIndex == 0) {
                    x /= 3;
                }
                else if (x < 0 && _this.pageinfo.ViewingPageIndex == _this.preloadedMaxPageIndex) {
                    x /= 3;
                }
                x = "translate3d(" + (point.left + x) + "px,0,0)";
                _this.itemContainer.css({
                    "-webkit-transform": x,
                    "-moz-transform": x,
                    "transform": x
                });
                if (Math.abs(x) > 0) {
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
                if (!isTouch) {
                    if (window.releaseEvents) {
                        window.releaseEvents(Event.MOUSEMOVE | Event.MOUSEUP);
                    }
                    else
                        _this.element[0].releaseCapture();
                }
                e = e || window.event;
                var x = isTouch ? e.changedTouches[0].clientX : e.clientX;
                x = (x - point.x);
                if (x != 0) {
                    if (x > _this.element.width() / 3 || (x > _this.element.width() / 18 && new Date().getTime() - point.time < 500)) {
                        if (_this.pageinfo.ViewingPageIndex > 0) {
                            _this.pageinfo.ViewingPageIndex--;
                        }
                    }
                    else if (-x > _this.element.width() / 3 || (-x > _this.element.width() / 18 && new Date().getTime() - point.time < 500)) {
                        if (_this.pageinfo.ViewingPageIndex != _this.preloadedMaxPageIndex) {
                            _this.pageinfo.ViewingPageIndex++;
                        }
                    }
                    var desLocation = "translate3d(" + -_this.pageinfo.ViewingPageIndex * _this.element.width() + "px,0,0)";
                    _this.itemContainer.css({
                        "-moz-transition": "-moz-transform 0.5s",
                        "-webkit-transition": "-webkit-transform 0.5s",
                        "-o-transition": "-o-transform 0.5s",
                        "transition": "transform 0.5s",
                        "-moz-transform": desLocation,
                        "-webkit-transform": desLocation,
                        "-o-transform": desLocation,
                        "transform": desLocation
                    });
                }
            }
        };
        this.element[0].ontouchcancel = function () {
            var desLocation = "translate3d(" + -_this.pageinfo.ViewingPageIndex * _this.element.width() + "px,0,0)";
            _this.itemContainer.css({
                "-moz-transition": "-moz-transform 0.5s",
                "-webkit-transition": "-webkit-transform 0.5s",
                "-o-transition": "-o-transform 0.5s",
                "transition": "transform 0.5s",
                "-moz-transform": desLocation,
                "-webkit-transform": desLocation,
                "-o-transform": desLocation,
                "transform": desLocation
            });
        };
        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);
        var transitionendFunc = function (e) {
            _this.itemContainer.css({
                "-moz-transition": "",
                "-webkit-transition": "",
                "-o-transition": "",
                "transition": "",
            });
            if (_this.onViewPageIndexChange) {
                _this.onViewPageIndexChange(_this.pageinfo.ViewingPageIndex);
            }
            _this.preLoadPage();
        };
        WayHelper.addEventListener(this.itemContainer[0], "transitionend", transitionendFunc, true);
        WayHelper.addEventListener(this.itemContainer[0], "webkitTransitionEnd", transitionendFunc, true);
    };
    WayGridView.prototype.preLoadPage = function () {
        for (var j = this.pageinfo.ViewingPageIndex - this.preLoadNumForPageMode; j < this.pageinfo.ViewingPageIndex + this.preLoadNumForPageMode + 1; j++) {
            if (j < 0)
                continue;
            var index = j;
            for (var i = 0; i < this.itemContainer[0].children.length; i++) {
                if (this.itemContainer[0].children[i].pageIndex == index) {
                    index = -1;
                    break;
                }
            }
            if (index >= 0)
                this.shouldLoadMorePage(index);
        }
        this.preloadedMaxPageIndex = 0;
        for (var i = 0; i < this.itemContainer[0].children.length; i++) {
            if (Math.abs(this.itemContainer[0].children[i].pageIndex - this.pageinfo.ViewingPageIndex) > 1) {
                this.itemContainer[0].removeChild(this.itemContainer[0].children[i]);
                i--;
            }
            else {
                if (this.itemContainer[0].children[i].pageIndex > this.preloadedMaxPageIndex) {
                    this.preloadedMaxPageIndex = this.itemContainer[0].children[i].pageIndex;
                }
            }
        }
    };
    WayGridView.prototype.setViewPageIndex = function (index) {
        if (this.pageMode) {
            if (index >= 0) {
                this.pageinfo.ViewingPageIndex = index;
                var desLocation = "translate3d(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0,0)";
                this.itemContainer.css({
                    "transition": "transform 0.5s",
                    "-webkit-transform": desLocation,
                    "-moz-transform": desLocation,
                    "transform": desLocation
                });
            }
        }
    };
    WayGridView.prototype.binddatas_pageMode = function (datas, pageindex) {
        if (datas.length == 0)
            return;
        try {
            if (!this.bodyTemplateHtml) {
                this.bodyTemplateHtml = "<div></div>";
            }
            var width = this.element.width();
            var divContainer = $(this.bodyTemplateHtml);
            divContainer[0].pageIndex = pageindex;
            if (pageindex > this.preloadedMaxPageIndex)
                this.preloadedMaxPageIndex = pageindex;
            divContainer.css({
                "position": "absolute",
                "width": width + "px",
                "height": this.element.height() + "px",
                "left": width * pageindex + "px",
                "top": "0px",
            });
            this.itemContainer.append(divContainer);
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
}(WayControlBase));
var WayDropDownList = (function (_super) {
    __extends(WayDropDownList, _super);
    function WayDropDownList(elementid, configElement) {
        var _this = this;
        _super.call(this);
        this.memberInChange = ["text", "value"];
        this.isMobile = false;
        this.isBindedGrid = false;
        this.onchange = null;
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        if (!this.element[0].WayControl) {
            this.element[0].WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        this.selectonly = this.element.attr("selectonly") === "true";
        var textele = this.element.find("*[istext]");
        if (textele.length > 0) {
            this.textElement = $(textele[0]);
        }
        if (this.selectonly && this.textElement && this.textElement[0].tagName == "INPUT") {
            this.textElement.attr("readonly", "readonly");
        }
        var actionEle = this.element.find("*[isaction]");
        if (actionEle.length > 0) {
            this.actionElement = $(actionEle[0]);
        }
        this.itemContainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);
        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");
        var datasource = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }
        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }
        if (this.actionElement) {
            this.init();
            this.itemContainer[0].appendChild(this.element.find("script[for='item']")[0]);
            this.grid = new WayGridView(this.itemContainer[0], null);
            this.grid.pagesize = 20;
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
                    this.textElement.attr("databind", "value=@text");
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
        var valueattr = this.element.attr("value");
        if (valueattr) {
            this.value = valueattr;
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
                for (var i = 0; i < this.grid.items.length; i++) {
                    if (this.grid.items[i].data.value == this._value) {
                        this.grid.items[i].status.Selected = true;
                    }
                    else {
                        this.grid.items[i].status.Selected = false;
                    }
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
                this.setText(v);
                this._value = this.getValueByText(v);
                for (var i = 0; i < this.grid.items.length; i++) {
                    if (this.grid.items[i].data.value == this._value) {
                        this.grid.items[i].status.Selected = true;
                    }
                    else {
                        this.grid.items[i].status.Selected = false;
                    }
                }
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
        var _this = this;
        if (this.grid.datasource instanceof Array) {
            for (var i = 0; i < this.grid.datasource.length; i++) {
                if (this.grid.datasource[i][this.valueMember] == value)
                    return this.grid.datasource[i][this.textMember];
            }
            return null;
        }
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = this.grid.items[i].data;
            if (data.value == value) {
                return data.text;
            }
        }
        var model;
        var result;
        eval("model={" + this.valueMember + ":" + JSON.stringify(value) + "}");
        this.grid.showLoading(this.textElement ? this.textElement : this.element);
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, function (data, err) {
            _this.grid.hideLoading();
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
        var _this = this;
        if (this.grid.datasource instanceof Array) {
            for (var i = 0; i < this.grid.datasource.length; i++) {
                if (this.grid.datasource[i][this.textMember] == text)
                    return this.grid.datasource[i][this.valueMember];
            }
            return null;
        }
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = this.grid.items[i].data;
            if (data.text == text) {
                return data.value;
            }
        }
        var model;
        var result;
        eval("model={" + this.textMember + ":" + JSON.stringify("equal:" + text) + "}");
        this.grid.showLoading(this.textElement ? this.textElement : this.element);
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, function (data, err) {
            _this.grid.hideLoading();
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
        item.status.Selected = item.data.value == this.value;
        item.click(function () {
            _this.hideList();
            _this.value = item.data.value;
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
        if (this.selectonly) {
            this.element.click(function (e) {
                e = e || window.event;
                if (e.stopPropagation)
                    e.stopPropagation();
                else
                    e.cancelBubble = true;
                _this.showList();
            });
        }
        else {
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
        }
        if (this.textElement[0].tagName == "INPUT") {
            if (this.isMobile) {
            }
            else {
                this.textElement.keyup(function () {
                    _this.grid.searchModel.text = _this.textElement.val();
                    if (_this.itemContainer.css("visibility") != "visible") {
                        _this.showList();
                    }
                });
            }
            this.textElement.change(function () {
                _this.grid.searchModel.text = _this.textElement.val();
                _this.text = _this.grid.searchModel.text;
            });
        }
        $(document.documentElement).click(function () {
            _this.hideList();
        });
    };
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
                "position": "fixed",
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
                if (this.grid.items[i].status.Selected) {
                    this.grid.items[i][0].scrollIntoView(false);
                    break;
                }
            }
        }
    };
    WayDropDownList.prototype.hideList = function () {
        if (this.maskLayer)
            this.maskLayer.hide();
        if (this.itemContainer.css("visibility") == "visible") {
            this.itemContainer.css("visibility", "hidden");
        }
    };
    return WayDropDownList;
}(WayControlBase));
var WayCheckboxList = (function (_super) {
    __extends(WayCheckboxList, _super);
    function WayCheckboxList(elementid) {
        var _this = this;
        _super.call(this);
        this.memberInChange = ["value"];
        this.isMobile = false;
        this._value = [];
        this.onchange = null;
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        if (!this.element[0].WayControl) {
            this.element[0].WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");
        var datasource = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }
        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }
        if (true) {
            this.grid = new WayGridView(this.element[0], null);
            this.grid.pagesize = 0;
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
            var status = this.grid.items[j].status;
            var data = this.grid.items[j].data;
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
                        if (typeof binding.model.__changed == "function") {
                            binding.model.__changed(config.dataMember, this._value);
                        }
                    }
                }
            }
        }
    };
    WayCheckboxList.prototype._onGridItemCreated = function (item) {
        var _this = this;
        item.click(function () {
            item.status.Selected = !item.status.Selected;
            if (item.status.Selected) {
                _this._value.push(item.data.value);
                _this.fireEvent("change");
                _this.rasieModelChange();
            }
            else {
                for (var i = 0; i < _this._value.length; i++) {
                    if (_this._value[i] == item.data.value) {
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
}(WayControlBase));
var WayRadioList = (function (_super) {
    __extends(WayRadioList, _super);
    function WayRadioList(elementid) {
        var _this = this;
        _super.call(this);
        this.memberInChange = ["value"];
        this.isMobile = false;
        this.onchange = null;
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        if (!this.element[0].WayControl) {
            this.element[0].WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        var itemtemplate = this.element.find("script[for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");
        var datasource = this.element.attr("datasource");
        if (datasource && datasource.length > 0 && datasource.substr(0, 1) == "[") {
            eval("datasource=" + datasource);
        }
        if (!this.valueMember || this.valueMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("value" in datasource[0])
                    this.valueMember = "value";
                else {
                    this.valueMember = WayHelper.getPropertyName(datasource[0], 1);
                    if (!this.valueMember)
                        this.valueMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.valueMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].value=datasource[i]." + this.valueMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("value" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].value=datasource[i]." + this.valueMember);
            }
        }
        if (!this.textMember || this.textMember.length == 0) {
            if (datasource && datasource instanceof Array && datasource.length > 0) {
                if ("text" in datasource[0])
                    this.textMember = "text";
                else {
                    this.textMember = WayHelper.getPropertyName(datasource[0], 0);
                    if (this.textMember) {
                        for (var i = 0; i < datasource.length; i++) {
                            eval("datasource[i].text=datasource[i]." + this.textMember);
                        }
                    }
                }
            }
        }
        else if (datasource && datasource instanceof Array && datasource.length > 0 && !("text" in datasource[0])) {
            for (var i = 0; i < datasource.length; i++) {
                eval("datasource[i].text=datasource[i]." + this.textMember);
            }
        }
        if (true) {
            this.grid = new WayGridView(this.element[0], null);
            this.grid.pagesize = 0;
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
            var status = this.grid.items[j].status;
            var data = this.grid.items[j].data;
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
                        if (typeof binding.model.__changed == "function") {
                            binding.model.__changed(config.dataMember, this._value);
                        }
                    }
                }
            }
        }
    };
    WayRadioList.prototype._onGridItemCreated = function (item) {
        var _this = this;
        item.click(function () {
            _this.value = item.data.value;
        });
    };
    return WayRadioList;
}(WayControlBase));
var WayRelateListDatasource = (function () {
    function WayRelateListDatasource() {
        this.loop = false;
    }
    return WayRelateListDatasource;
}());
var WayRelateList = (function (_super) {
    __extends(WayRelateList, _super);
    function WayRelateList(elementid, virtualEle) {
        _super.call(this);
        this.memberInChange = ["value"];
        this.onchange = null;
        this.configs = [];
        this._value = [];
        this.windowObj = _windowObj;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        if (!this.element[0].WayControl) {
            this.element[0].WayControl = this;
        }
        this.isMobile = "ontouchstart" in this.element[0];
        this.textElement = $(this.element.find("*[istext='true']")[0]);
        for (var i = 0; i < virtualEle.children.length; i++) {
            var configEle = virtualEle.children[i];
            if (configEle.tagName == "CONFIG") {
                var config = new WayRelateListDatasource();
                config.datasource = configEle.getAttribute("datasource");
                config.relateMember = configEle.getAttribute("relateMember");
                config.textMember = configEle.getAttribute("textMember");
                config.valueMember = configEle.getAttribute("valueMember");
                config.loop = configEle.getAttribute("loop") == "true";
                this.configs.push(config);
            }
        }
        this.init();
    }
    Object.defineProperty(WayRelateList.prototype, "text", {
        get: function () {
            return this._text;
        },
        set: function (v) {
            if (v != this._text) {
                this._text = v;
                if (this.textElement[0].tagName == "INPUT") {
                    this.textElement.val(v);
                }
                else {
                    this.textElement.html(v);
                }
                this.fireEvent("change");
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(WayRelateList.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            if (v != this._value) {
                this._value = v;
                var text = "";
                for (var i = 0; i < v.length; i++) {
                    var config = i < this.configs.length ? this.configs[i] : this.configs[this.configs.length - 1];
                    var grid = null;
                    if (this.listContainer[0].children.length > i) {
                        grid = this.listContainer[0].children[i].WayControl;
                    }
                    if (text.length > 0)
                        text += "/";
                    text += this.getTextByValue(config, grid, v[i]);
                }
                this.text = text;
            }
        },
        enumerable: true,
        configurable: true
    });
    WayRelateList.prototype.addEventListener = function (eventName, func) {
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
    WayRelateList.prototype.fireEvent = function (eventName) {
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
    WayRelateList.prototype.init = function () {
        var _this = this;
        if (this.isMobile) {
            var scrollConainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);
            this.listContainer = $(document.createElement("DIV"));
            this.listContainer.css({
                height: "100%",
                "-webkit-box-sizing": "border-box",
                "-moz-box-sizing": "border-box",
                "box-sizing": "border-box",
            });
            scrollConainer.css({
                "-webkit-overflow-scrolling": "touch",
                "overflow-x": "auto",
                "overflow-y": "hidden",
                width: this.windowObj.innerWidth() * 0.9 + "px",
                height: this.windowObj.innerHeight() * 0.9 + "px",
                "margin-left": this.windowObj.innerWidth() * 0.05 + "px",
                "margin-top": this.windowObj.innerHeight() * 0.05 + "px",
            });
            this.maskLayer = $("<div style='background-color:rgba(0, 0, 0,0.3);z-index:998;position:fixed;width:100%;height:100%;visibility:hidden;left:0;top:0;'></div>");
            this.maskLayer.click(function (e) {
                e = e || window.event;
                var srcElement = e.target || e.srcElement;
                if (srcElement == _this.maskLayer[0]) {
                    _this.hideList();
                }
            });
            document.body.appendChild(this.maskLayer[0]);
            this.maskLayer[0].appendChild(scrollConainer[0]);
            scrollConainer[0].appendChild(this.listContainer[0]);
        }
        else {
            this.listContainer = $(this.element.find("script[for='itemContainer']")[0].innerHTML);
            this.listContainer.css({
                "visibility": "hidden",
                height: "300px",
                "z-index": 999,
                position: "absolute",
                "-webkit-box-sizing": "border-box",
                "-moz-box-sizing": "border-box",
                "box-sizing": "border-box",
            });
            document.body.appendChild(this.listContainer[0]);
            $(document.documentElement).click(function (e) {
                e = e || window.event;
                var srcEle = (e.target || e.srcElement);
                while (srcEle && srcEle.tagName != "BODY") {
                    if (srcEle == _this.listContainer[0] || srcEle == _this.element[0]) {
                        return;
                    }
                    srcEle = srcEle.parentElement;
                }
                _this.hideList();
            });
        }
        this.element.click(function () {
            _this.showList();
        });
    };
    WayRelateList.prototype.showList = function () {
        var container = this.maskLayer ? this.maskLayer : this.listContainer;
        if (container.css("visibility") == "hidden") {
            if (!this.isMobile) {
                var offset = this.element.offset();
                var y = (offset.top + this.element.outerHeight());
                this.listContainer.css({
                    left: offset.left + "px",
                    top: y + "px",
                });
                if (y + this.listContainer.outerHeight() > document.body.scrollTop + this.windowObj.innerHeight()) {
                    y = offset.top - this.listContainer.outerHeight();
                    if (y >= 0) {
                        this.listContainer.css("top", y + "px");
                    }
                }
                if (offset.left + this.listContainer.outerWidth() > document.body.scrollLeft + this.windowObj.innerWidth()) {
                    this.listContainer.css("left", (document.body.scrollLeft + this.windowObj.innerWidth() - this.listContainer.outerWidth()) + "px");
                }
            }
            else {
                this.maskLayer.show();
            }
            container.css("visibility", "visible");
            this.loadList();
        }
    };
    WayRelateList.prototype.hideList = function () {
        var container = this.maskLayer ? this.maskLayer : this.listContainer;
        if (container.css("visibility") != "hidden") {
            container.css("visibility", "hidden");
        }
    };
    WayRelateList.prototype.checkWidth = function () {
        var minWidth = this.textElement.width();
        if (this.isMobile)
            minWidth = this.windowObj.innerWidth() * 0.9;
        var contentWidth = 0;
        for (var i = 0; i < this.listContainer[0].children.length; i++) {
            var ele = $(this.listContainer[0].children[i]);
            contentWidth += ele.outerWidth();
        }
        if (contentWidth < minWidth) {
            this.listContainer.css("width", "");
            var lastObj = this.listContainer.children().last();
            lastObj.width(lastObj.width() + minWidth - contentWidth);
        }
        else if (this.isMobile) {
            this.listContainer.width(contentWidth);
            this.listContainer[0].parentElement.scrollLeft = 100000;
        }
        else {
            this.listContainer.width(contentWidth + 1);
        }
    };
    WayRelateList.prototype.loadList = function () {
        if (this.listContainer[0].children.length == 0) {
            var config = this.configs[0];
            this.loadConfigList(config, 0, {});
        }
        else {
            var searchModel = {};
            for (var i = 0; i < this._value.length; i++) {
                if (i < this.listContainer[0].children.length) {
                    var grid = this.listContainer[0].children[i].WayControl;
                    for (var j = 0; j < grid.items.length; j++) {
                        var item = grid.items[j];
                        if (item.data.value == this._value[i]) {
                            if (!item.status.Selected) {
                                item.status.Selected = true;
                                while (this.listContainer[0].children.length > i + 1) {
                                    this.listContainer[0].removeChild(this.listContainer[0].children[this.listContainer[0].children.length - 1]);
                                }
                            }
                            item[0].scrollIntoView();
                        }
                        else {
                            item.status.Selected = false;
                        }
                    }
                }
                else {
                    var config = i < this.configs.length ? this.configs[i] : this.configs[this.configs.length - 1];
                    eval("searchModel={" + config.relateMember + ":" + JSON.stringify(this._value[i - 1]) + "}");
                    this.loadConfigList(config, i, searchModel);
                }
            }
        }
    };
    WayRelateList.prototype.loadConfigList = function (config, configIndex, searchModel) {
        var _this = this;
        while (this.listContainer[0].children.length > configIndex) {
            this.listContainer[0].removeChild(this.listContainer[0].children[this.listContainer[0].children.length - 1]);
        }
        this.listContainer.children().last().css("width", "");
        var div = $(document.createElement("DIV"));
        div.attr("datasource", config.datasource);
        div.css({
            "height": "100%",
            "overflow-x": "hidden",
            "overflow-y": "auto",
            "min-width": "100px",
            "float": "left",
            "-webkit-box-sizing": "border-box",
            "-moz-box-sizing": "border-box",
            "box-sizing": "border-box",
        });
        if (configIndex > 0) {
            div.css({
                "border-left": "1px solid #ccc",
            });
        }
        div.html("<script for='item' type='text/ html'>" + this.element.find("script[for='item']")[0].innerHTML + "</script>");
        this.listContainer.append(div);
        var grid = new WayGridView(div, null);
        grid.pagesize = 0;
        grid.searchModel = searchModel;
        if (config.textMember) {
            grid.dataMembers.push(config.textMember + "->text");
        }
        if (config.valueMember) {
            grid.dataMembers.push(config.valueMember + "->value");
        }
        grid.onAfterCreateItems = function (total, hasmore) {
            _this.checkWidth();
        };
        grid.onCreateItem = function (item) {
            item.status.Selected = item.data.value == _this._value[configIndex];
            if (item.status.Selected) {
                item[0].scrollIntoView(false);
                var nextConfig;
                if (config.loop)
                    nextConfig = config;
                else if (configIndex < _this.configs.length - 1)
                    nextConfig = _this.configs[configIndex + 1];
                if (nextConfig) {
                    var term;
                    eval("term={" + nextConfig.relateMember + ":" + JSON.stringify(item.data.value) + "}");
                    _this.loadConfigList(nextConfig, configIndex + 1, term);
                }
            }
            item.click(function () {
                if (!item.status.Selected) {
                    for (var i = 0; i < grid.items.length; i++) {
                        grid.items[i].status.Selected = (grid.items[i] === item);
                    }
                    var nextConfig;
                    if (config.loop)
                        nextConfig = config;
                    else if (configIndex < _this.configs.length - 1)
                        nextConfig = _this.configs[configIndex + 1];
                    if (nextConfig) {
                        var term;
                        eval("term={" + nextConfig.relateMember + ":" + JSON.stringify(item.data.value) + "}");
                        _this.loadConfigList(nextConfig, configIndex + 1, term);
                    }
                    else {
                        _this.hideList();
                    }
                    _this.showCurrentText();
                }
                else {
                    _this.hideList();
                }
            });
        };
        grid.databind();
    };
    WayRelateList.prototype.getTextByValue = function (config, grid, value) {
        var dbcontext;
        if (grid) {
            for (var i = 0; i < grid.items.length; i++) {
                var data = grid.items[i].data;
                if (data.value == value) {
                    return data.text;
                }
            }
            dbcontext = grid.dbContext;
        }
        else {
            var controller = document.body.getAttribute("controller");
            dbcontext = new WayDBContext(controller, config.datasource);
        }
        var model;
        var result;
        eval("model={" + config.valueMember + ":" + JSON.stringify(value) + "}");
        dbcontext.getDataItem([config.valueMember, config.textMember], model, function (data, err) {
            if (err) {
                throw err;
            }
            else if (data) {
                result = data;
            }
        }, false);
        if (result) {
            return result[config.textMember];
        }
        return null;
    };
    WayRelateList.prototype.showCurrentText = function () {
        var text = "";
        while (this._value.length > 0)
            this._value.pop();
        for (var i = 0; i < this.listContainer[0].children.length; i++) {
            var grid = this.listContainer[0].children[i].WayControl;
            for (var j = 0; j < grid.items.length; j++) {
                if (grid.items[j].status.Selected) {
                    if (text.length > 0)
                        text += "/";
                    text += grid.items[j].data.text;
                    this._value.push(grid.items[j].data.value);
                }
            }
        }
        this.text = text;
    };
    return WayRelateList;
}(WayControlBase));
var WayButton = (function (_super) {
    __extends(WayButton, _super);
    function WayButton(elementid) {
        var _this = this;
        _super.call(this);
        this.memberInChange = ["text"];
        this.internalModel = { text: null };
        this.onchange = null;
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if (elementid.tagName)
            this.element = $(elementid);
        else
            this.element = elementid;
        var _databind_internal = this.element.attr("_databind_internal");
        var databind = this.element.attr("databind");
        var _expression_internal = this.element.attr("_expression_internal");
        var expression = this.element.attr("expression");
        this.element.attr("databind", _databind_internal);
        this.element.attr("expression", _expression_internal);
        this.internalModel = WayDataBindHelper.dataBind(this.element[0], { text: this.element.attr("text") }, null, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g, true);
        this.element.attr("databind", databind);
        this.element.attr("expression", expression);
        if (!this.element[0].WayControl) {
            this.element[0].WayControl = this;
        }
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
}(WayControlBase));
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
    if (element === void 0) { element = null; }
    var mytemplate = virtualEle.getAttribute("template");
    if (mytemplate && mytemplate.length > 0) {
        var templates = $(document.body).find("script[id='" + mytemplate + "']");
        if (templates && templates.length > 0) {
            element = templates[0];
        }
    }
    for (var i = 0; i < virtualEle.children.length; i++) {
        if (virtualEle.children[i].tagName == "SCRIPT" && virtualEle.children[i].getAttribute("for") == "template") {
            element = virtualEle.children[i];
            virtualEle.removeChild(element);
            break;
        }
    }
    if (!element) {
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
    var controlType = virtualEle.tagName;
    var replaceEleObj = $(element.innerHTML);
    for (var k = 0; k < element.attributes.length; k++) {
        replaceEleObj.attr(element.attributes[k].name, element.attributes[k].value);
    }
    checkToInitWayControl(replaceEleObj[0]);
    var style1 = virtualEle.getAttribute("style");
    var style2 = replaceEleObj.attr("style");
    if (style1) {
        if (!style2)
            style2 = "";
        replaceEleObj.attr("style", style2 + ";" + style1);
        virtualEle.removeAttribute("style");
    }
    if (replaceEleObj.attr("databind")) {
        replaceEleObj.attr("_databind_internal", replaceEleObj.attr("databind"));
    }
    if (replaceEleObj.attr("expression")) {
        replaceEleObj.attr("_expression_internal", replaceEleObj.attr("expression"));
    }
    for (var k = 0; k < virtualEle.attributes.length; k++) {
        replaceEleObj.attr(virtualEle.attributes[k].name, virtualEle.attributes[k].value);
    }
    if (virtualEle == virtualEle.parentElement.children[virtualEle.parentElement.children.length - 1]) {
        var parent = virtualEle.parentElement;
        parent.removeChild(virtualEle);
        parent.appendChild(replaceEleObj[0]);
    }
    else {
        var nextlib = virtualEle.nextElementSibling;
        var parent = virtualEle.parentElement;
        parent.removeChild(virtualEle);
        parent.insertBefore(replaceEleObj[0], nextlib);
    }
    var control = null;
    var typeFunctionName = null;
    for (var name in window) {
        if (name.toLowerCase() == controlType.toLowerCase()) {
            typeFunctionName = name;
            break;
        }
    }
    if (typeFunctionName) {
        eval("control = new " + typeFunctionName + "(replaceEleObj,virtualEle)");
    }
    if (control) {
        var idstr = replaceEleObj.attr("id");
        if (idstr && idstr.length > 0) {
            var exists = false;
            for (var k = 0; k < _allWayControlNames.length; k++) {
                if (_allWayControlNames[k] == idstr) {
                    exists = true;
                    break;
                }
            }
            if (!exists) {
                eval("window." + idstr + "=control;");
            }
        }
    }
};
var _allWayControlNames = [];
var _styles;
var _bodyObj;
var _windowObj = $(window);
$(document).ready(function () {
    _bodyObj = $(document.body);
    var controllerName = _bodyObj.attr("controller");
    if (!controllerName || controllerName.length == 0) {
    }
    else {
        window.controller = WayScriptRemoting.createRemotingController(controllerName);
    }
    var _controlTemplatePath = _bodyObj.attr("controlTemplate");
    if (!_controlTemplatePath || _controlTemplatePath.length == 0)
        _controlTemplatePath = "/templates/main.html";
    _styles = $(WayHelper.downloadUrl(_controlTemplatePath));
    for (var i = 0; i < _styles.length; i++) {
        var element = _styles[i];
        if (element.tagName == "STYLE") {
            document.body.appendChild(element);
        }
        else if (element.children) {
            var controlType = element.tagName;
            while (element.children.length > 1) {
                element.removeChild(element.children[1]);
            }
            var controlEles = _bodyObj.find(controlType);
            for (var j = 0; j < controlEles.length; j++) {
                var virtualEle = controlEles[j];
                initWayControl(virtualEle, element);
            }
        }
    }
});
//# sourceMappingURL=WayScriptRemoting.js.map