var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
;
var RSAMAXLENGTH = 110;
var WayScriptRemotingMessageType;
(function (WayScriptRemotingMessageType) {
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["Result"] = 1] = "Result";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["Notify"] = 2] = "Notify";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["SendSessionID"] = 3] = "SendSessionID";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["InvokeError"] = 4] = "InvokeError";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["UploadFileBegined"] = 5] = "UploadFileBegined";
    WayScriptRemotingMessageType[WayScriptRemotingMessageType["RSADecrptError"] = 6] = "RSADecrptError";
})(WayScriptRemotingMessageType || (WayScriptRemotingMessageType = {}));
var HTTP_Protocol = location.href.substr(0, location.href.indexOf(":"));
var WEBSOCKET_Protocol = "ws";
if (HTTP_Protocol == "https") {
    WEBSOCKET_Protocol = "wss";
}
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
var WayScriptRemoting = (function () {
    function WayScriptRemoting(remoteName) {
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
            if (!this._groupName)
                throw "请先设置groupName";
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
        var invoker = new WayScriptInvoker(HTTP_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
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
            m: JSON.stringify({
                Action: 'init',
                ClassFullName: remoteName,
                SessionID: WayCookie.getCookie("WayScriptRemoting")
            })
        });
        if (hasErr) {
            throw hasErr;
        }
        else if (result.err) {
            throw result.err;
        }
        var func;
        eval("func = " + WayScriptRemoting.getClassDefineScript(result.methods));
        var page = new func(remoteName);
        page.rsa = result.rsa;
        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID);
        return page;
    };
    WayScriptRemoting.getClassDefineScript = function (methods) {
        var text = "";
        text += ("(function (_super) {__extends(func, _super);function func() {_super.apply(this, arguments);");
        text += "this.server = {};";
        text += "var self = this;";
        for (var i = 0; i < methods.length; i++) {
            var m = methods[i];
            text += "this.server." + m.Method + " = function (";
            for (var j = 0; j < m.ParameterLength; j++) {
                text += "p" + j + ",";
            }
            text += "callback){_super.prototype.pageInvoke.call(self,'" + methods[i].Method + "',[";
            for (var j = 0; j < m.ParameterLength; j++) {
                text += "p" + j;
                if (j < m.ParameterLength - 1) {
                    text += ",";
                }
            }
            if (m.EncryptParameters || m.EncryptResult) {
                text += "] , callback,true," + m.EncryptParameters + "," + m.EncryptResult + " );};";
            }
            else {
                text += "] , callback );};";
            }
        }
        text += "}";
        text += "return func;}(WayScriptRemoting));";
        return text;
    };
    WayScriptRemoting.createRemotingControllerAsync = function (remoteName, callback) {
        WayScriptRemoting.getServerAddress();
        var ws = WayHelper.createWebSocket(WEBSOCKET_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
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
                    eval("func = " + WayScriptRemoting.getClassDefineScript(result.methods));
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
    WayScriptRemoting.prototype.reCreateRSA = function (callback) {
        var invoker = new WayScriptInvoker(HTTP_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_recreatersa?a=" + (new Date().getTime()));
        invoker.onCompleted = function (ret, err) {
            if (err) {
                callback(null, err);
            }
            else {
                var resultObj;
                eval("resultObj=" + ret);
                callback(resultObj, null);
            }
        };
        invoker.Post({
            m: JSON.stringify({
                SessionID: WayCookie.getCookie("WayScriptRemoting")
            })
        });
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
            var ws = WayHelper.createWebSocket(WEBSOCKET_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
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
    WayScriptRemoting.prototype.str2UTF8 = function (str) {
        var bytes = new Array();
        var len, c;
        len = str.length;
        for (var i = 0; i < len; i++) {
            c = str.charCodeAt(i);
            if (c >= 0x010000 && c <= 0x10FFFF) {
                bytes.push(((c >> 18) & 0x07) | 0xF0);
                bytes.push(((c >> 12) & 0x3F) | 0x80);
                bytes.push(((c >> 6) & 0x3F) | 0x80);
                bytes.push((c & 0x3F) | 0x80);
            }
            else if (c >= 0x000800 && c <= 0x00FFFF) {
                bytes.push(((c >> 12) & 0x0F) | 0xE0);
                bytes.push(((c >> 6) & 0x3F) | 0x80);
                bytes.push((c & 0x3F) | 0x80);
            }
            else if (c >= 0x000080 && c <= 0x0007FF) {
                bytes.push(((c >> 6) & 0x1F) | 0xC0);
                bytes.push((c & 0x3F) | 0x80);
            }
            else {
                bytes.push(c & 0xFF);
            }
        }
        return bytes;
    };
    WayScriptRemoting.prototype.encrypt = function (value) {
        setMaxDigits(129);
        value = JSON.stringify(this.str2UTF8(value));
        var key = new RSAKeyPair(this.rsa.Exponent, "", this.rsa.Modulus);
        if (value.length <= RSAMAXLENGTH) {
            return encryptedString(key, value);
        }
        else {
            var result = "";
            var total = value.length;
            for (var i = 0; i < value.length; i += RSAMAXLENGTH) {
                var text = value.substr(i, Math.min(RSAMAXLENGTH, total));
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
            var invoker = new WayScriptInvoker(HTTP_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=" + (new Date().getTime()));
            invoker.async = async;
            invoker.onCompleted = function (ret, err) {
                if (WayScriptRemoting.onInvokeFinish) {
                    WayScriptRemoting.onInvokeFinish(name, parameters);
                }
                if (!callback)
                    return;
                if (err) {
                    callback(null, err);
                }
                else {
                    var originalRet = ret;
                    var resultObj;
                    if (returnUseRsa && ret.indexOf("{") != 0) {
                        setMaxDigits(129);
                        var rsakey = new RSAKeyPair("", _this.rsa.Exponent, _this.rsa.Modulus);
                        try {
                            ret = decryptedString(rsakey, ret);
                            eval("ret=\"" + ret + "\"");
                        }
                        catch (e) {
                            _this.reCreateRSA(function (ret2, err) {
                                if (err)
                                    callback(null, err);
                                else {
                                    _this.rsa = ret2.rsa;
                                    callback(null, "服务器已处理完毕，因网络原因，无法正确显示结果");
                                }
                            });
                            return;
                        }
                    }
                    eval("resultObj=" + ret);
                    if (resultObj.sessionid && resultObj.sessionid.length > 0) {
                        WayCookie.setCookie("WayScriptRemoting", resultObj.sessionid);
                    }
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
            var parameJson;
            if (!parameters)
                parameJson = "[]";
            else
                parameJson = useRsa ? this.encrypt(JSON.stringify(parameters)) : JSON.stringify(parameters);
            invoker.Post({
                m: JSON.stringify({
                    ClassFullName: this.classFullName,
                    MethodName: name,
                    ParameterJson: parameJson,
                    SessionID: WayCookie.getCookie("WayScriptRemoting")
                })
            });
        }
        catch (e) {
            callback(null, e.message);
        }
    };
    WayScriptRemoting.prototype.sendHeart = function () {
        this.socket.send("{'Action':'w_heart','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
    };
    WayScriptRemoting.prototype.connect = function () {
        var _this = this;
        this.socket = WayHelper.createWebSocket(WEBSOCKET_Protocol + "://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        this.socket.onopen = function () {
            try {
                if (_this.onconnect) {
                    _this.onconnect();
                }
                _this.socket_heart_timer = setTimeout(function () { return _this.sendHeart(); }, 10000);
            }
            catch (e) {
            }
            _this.socket.send("{'GroupName':'" + _this._groupName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        this.socket.onmessage = function (evt) {
            var resultObj;
            eval("resultObj=" + evt.data);
            clearTimeout(_this.socket_heart_timer);
            _this.socket_heart_timer = setTimeout(function () { return _this.sendHeart(); }, 10000);
            if (_this._onmessage && resultObj.type == 1) {
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
    return WayScriptRemoting;
}());
WayScriptRemoting.ServerAddress = null;
WayScriptRemoting.ExistControllers = [];
var WayScriptRemotingChild = (function (_super) {
    __extends(WayScriptRemotingChild, _super);
    function WayScriptRemotingChild() {
        return _super !== null && _super.apply(this, arguments) || this;
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
var WayHelper = (function () {
    function WayHelper() {
    }
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
//# sourceMappingURL=WayScriptRemoting.js.map