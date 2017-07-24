declare var setMaxDigits: (n: number) => void;
declare class RSAKeyPair { constructor(e: string,n:string, m: string); };
declare var encryptedString: (key: RSAKeyPair, value: string) => string;
declare var decryptedString: (key: RSAKeyPair, value: string) => string;

enum WayScriptRemotingMessageType {
    Result = 1,
    Notify = 2,
    SendSessionID = 3,
    InvokeError = 4,
    UploadFileBegined = 5,
    RSADecrptError = 6,
}

class WayCookie {
    static setCookie(name: string, value: string): void {
        document.cookie = name + "=" + (<any>window).encodeURIComponent(value, "utf-8");

    }

    static getCookie(name: string): string {
        try {
            var cookieStr = document.cookie;
            if (cookieStr.length > 0) {
                var cookieArr = cookieStr.split(";"); //将cookie信息转换成数组
                for (var i = 0; i < cookieArr.length; i++) {
                    var cookieVal = cookieArr[i].split("="); //将每一组cookie(cookie名和值)也转换成数组
                    if (cookieVal[0].trim() == name) {
                        var v = cookieVal[1].trim();
                        if (v != "") {
                            return (<any>window).decodeURIComponent(v, "utf-8"); //返回需要提取的cookie值
                        }
                    }
                }
            }
        }
        catch (e) {
        }
        return "";
    }
}

class WayScriptRemotingUploadHandler {
    abort: boolean = false;
    offset: number = 0;
}
class RSAInfo {
    Exponent: string;
    Modulus: string;
}

class WayScriptRemoting {
    static onBeforeInvoke: (name: string, parameters: any[]) => any;
    static onInvokeFinish: (name: string, parameters: any[]) => any;

    rsa: RSAInfo;
    classFullName: string;
    private _groupName: string;
    get groupName(): string {
        return this.groupName;
    }
    set groupName(value: string) {
        this._groupName = value;
        if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
            this.mDoConnected = true;
            this.connect();
        }
    }

    //当长连接异常时触发
    onerror: (err: any) => any;
    //当长连接正常连上时触发
    onconnect: () => any;

    private mDoConnected: boolean = false;
    private _onmessage: (msg: any) => any;
    //长连接接收到信息触发
    get onmessage(): (msg: any) => any {
        return this._onmessage;
    }
    set onmessage(func: (msg: any) => any) {
        this._onmessage = func;
        if (!this.mDoConnected && this._groupName && this._groupName.length > 0) {
            this.mDoConnected = true;
            this.connect();
        }
    }

    private socket: WebSocket;

    static ServerAddress: string = null;//"localhost:9090";
    static ExistControllers: WayScriptRemoting[] = [];
    constructor(remoteName: string) {
        this.classFullName = remoteName;
        WayScriptRemoting.getServerAddress();
    }

    static getServerAddress(): void {
        if (WayScriptRemoting.ServerAddress == null) {
            var host, port;
            var href: any = location.href;
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
    }

    static createRemotingController(remoteName: string): WayScriptRemoting {
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
        invoker.onCompleted = (ret, err) => {
            if (err) {
                hasErr = err;
            }
            else {
                eval("result=" + ret);

            }
        };
        invoker.Post({
            m: JSON.stringify( {
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
        eval("func = " + WayScriptRemoting.getClassDefineScript(<any[]>result.methods));

        var page = <WayScriptRemoting>new func(remoteName);
        page.rsa = result.rsa;

        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID);
        return page;
    }

    static getClassDefineScript(methods: any[]): string {
        var text: string = "";
        text += ("(function (_super) {__extends(func, _super);function func() {_super.apply(this, arguments);");
        text += "this.server = {};";
        text += "var self = this;";
        for (var i = 0; i < methods.length; i++) {
            var m = methods[i];
           
            text += "this.server." + m.Method + " = function (";
            for (var j = 0; j < m.ParameterLength; j++)
            {
                text += "p" + j + ",";
            }
            text += "callback){_super.prototype.pageInvoke.call(self,'" + methods[i].Method + "',[";
            for (var j = 0; j < m.ParameterLength; j++)
            {
                text += "p" + j;
                if (j < m.ParameterLength - 1) {
                    text += ",";
                }
            }

            if (m.EncryptParameters || m.EncryptResult) {
                text += "] , callback,true," + m.EncryptParameters + "," + m.EncryptResult +" );};";
            }
            else {
                text += "] , callback );};";
            }
        }
        text += "}";
        text += "return func;}(WayScriptRemoting));";
       return text;
    }

    private static createRemotingControllerAsync(remoteName: string, callback: (obj: WayScriptRemoting, err: string) => void): void {
        WayScriptRemoting.getServerAddress();
        var ws = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        ws.onopen = () => {
            ws.send("{'Action':'init' , 'ClassFullName':'" + remoteName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        ws.onmessage = (evt) => {
            ws.onerror = null;//必须把它设置为null，否则关闭时，会触发onerror
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

                    var page = <WayScriptRemoting>new func(remoteName);
                    page.rsa = result.rsa;
                    WayCookie.setCookie("WayScriptRemoting", result.SessionID)
                    callback(page, null);
                }
                catch (e) {
                    callback(null, e.message);
                }
            }
        };
        ws.onerror = (evt: any) => {
            callback(null, "无法连接服务器");
        };
    }

    private _uploadFileWithHTTP(fileElement: any, state: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler {
        try {

            var file: File;
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

            this.pageInvoke("UploadFileWithHTTP", [file.name, state, size, handler.offset], (ret, err) => {
                if (err) {
                    if (callback) {
                        callback(null, 0, 0, err);
                    }
                }
                else {

                    this.sendFileWithHttp(ret, state,file, reader, size, handler.offset, 10240, callback, handler);
                }
            });
            return handler;
        }
        catch (e) {
            if (callback) {
                try { callback(null, null, null, e.message); } catch (e) { }
            }
        }
    }
    private arrayBufferToString(data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }

        return str;
    }
    private sendFileWithHttp(tranid: string, state: any,file: File, reader: FileReader, size: number,
        start: number, len: number, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler) {

        if (start + len > size) {
            len = size - start;
        }

        var blob = file.slice(start, start + len);
        reader.onload = () => {
            var filedata: ArrayBuffer = reader.result;
            
            if (filedata.byteLength > 0) {
                start += filedata.byteLength;
                if ( handler.abort) {
                   
                }
                else {
                    try {
                        filedata = <any>this.arrayBufferToString(filedata);
                        this.pageInvoke("GettingFileDataWithHttp", [tranid, filedata], (ret, err) => {
                            if (err) {
                                if (err.indexOf("tranid not exist") >= 0) {
                                    this._uploadFileWithHTTP(file, state, callback, handler);
                                }
                                else {
                                    setTimeout(() => { this.sendFileWithHttp(tranid, state, file, reader, size, start, len, callback, handler); } , 1000);
                                   
                                }
                            }
                            else {
                                handler.offset = ret.offset;
                                if (callback) {
                                    callback(ret.size == ret.offset ? "ok":"", ret.size, ret.offset, null);
                                }
                                if (ret.offset < ret.size) {
                                    this.sendFileWithHttp(tranid, state, file, reader, size, ret.offset, len, callback, handler);
                                }
                            }
                        });

                        
                    }
                    catch (e) {
                    }
                }
            }
        };
        reader.onerror = () => {
            if (callback) {
                try { callback("", null, null, "读取文件发生错误"); } catch (e) { }
            }
        }
        reader.readAsArrayBuffer(blob);
    }

    private sendFile(ws: WebSocket, file: File, reader: FileReader, size: number,
        start: number, len: number, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler) {

        if (ws.binaryType != "arraybuffer")
            return;

        if (start + len > size) {
            len = size - start;
        }

        var blob = file.slice(start, start + len);
        reader.onload = () => {
            var filedata: ArrayBuffer = reader.result;
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
                            this.sendFile(ws, file, reader, size, start, len, callback, handler);
                        }
                    }
                    catch (e) {
                    }
                }
            }
        };
        reader.onerror = () => {
            if (callback) {
                try { callback("", null, null, "读取文件发生错误"); } catch (e) { }
            }
        }
        reader.readAsArrayBuffer(blob);
    }

    uploadFile(fileElement: any, state: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler {
        if (!(<any>window).WebSocket) {
            return this._uploadFileWithHTTP(fileElement , state , callback, null);
        }
        try {

            var file: File;
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
            ws.onopen = () => {
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "',State:" + JSON.stringify(state)+ ",'FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
            };
            ws.onmessage = (evt) => {
                var resultObj;
                eval("resultObj=" + evt.data);

                if (resultObj.type == WayScriptRemotingMessageType.UploadFileBegined) {
                    ws.binaryType = "arraybuffer";
                    this.sendFile(ws, file, reader, size, handler.offset, 102400, callback, handler);
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
                            try { callback("", size, parseInt(resultObj.result), false); } catch (e) { }

                        }
                    }
                }
                else if (resultObj.type == WayScriptRemotingMessageType.InvokeError) {
                    ws.binaryType = initType;
                    errored = true;
                    if (callback) {
                        try { callback(null, null, null, resultObj.result); } catch (e) { }
                    }
                    ws.close();
                }
            };
            ws.onclose = () => {
                ws.onerror = null;
                if (!finished) {
                    if (handler.abort == false) {
                        this.uploadFile(file, state, callback, handler);
                    }
                }
            }
            ws.onerror = () => {
                ws.onclose = null;
                if (handler.offset == 0) {
                    if (callback && !finished) {
                        try { callback(null, null, null, "网络错误"); } catch (e) { }
                    }
                }
                else {
                    if (!finished) {
                        //续传
                        if (handler.abort == false) {
                            this.uploadFile(file, state, callback, handler);
                        }

                    }
                }
            };

            return handler;
        }
        catch (e) {
            if (callback) {
                try { callback(null, null, null, e.message); } catch (e) { }
            }
        }
    }

    private encrypt(value: string): string {
        setMaxDigits(129);
        value = (<any>window).encodeURIComponent(value, "utf-8");

        var key = new RSAKeyPair(this.rsa.Exponent, "", this.rsa.Modulus);
        if (value.length <= 110) {//只有110是正常，试过120都发生异常，但是c#里面的算法，128都可以
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
    }

    pageInvoke(name: string, parameters: any[], callback: any, async: boolean = true, useRsa: boolean=false , returnUseRsa:boolean = false) {
        try {
            if (WayScriptRemoting.onBeforeInvoke) {
                WayScriptRemoting.onBeforeInvoke(name, parameters);
            }

            
            var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
            invoker.async = async;
            invoker.onCompleted = (ret, err) => {
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
                        var rsakey = new RSAKeyPair("", this.rsa.Exponent, this.rsa.Modulus);
                        try {
                            ret = decryptedString(rsakey, ret);
                        }
                        catch (e)
                        {
                            throw "RSA decrypted error，" + e.messsage;
                        }
                        ret = decodeURIComponent(ret);
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
                        this.rsa = resultObj.result;
                        this.pageInvoke(name,parameters, callback, async, useRsa);
                    }
                }
            };

            for (var i = 0; i < parameters.length; i++) {
                parameters[i] = JSON.stringify(parameters[i]);
                if (useRsa) {
                    parameters[i] = this.encrypt(parameters[i]);
                }
            }
            invoker.Post({
                m: JSON.stringify({
                    ClassFullName: this.classFullName,
                    MethodName: name,
                    Parameters: parameters,
                    SessionID: WayCookie.getCookie("WayScriptRemoting")
                })
            });
            
        }
        catch (e) {
            callback(null, e.message);
        }

    }

    private connect(): void {
        this.socket = WayHelper.createWebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
        this.socket.onopen = () => {
            try {
                if (this.onconnect) {
                    this.onconnect();
                }
            }
            catch (e) {
            }
            this.socket.send("{'GroupName':'" + this._groupName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
        };
        this.socket.onmessage = (evt: any) => {
            var resultObj;
            eval("resultObj=" + evt.data);

            if (this._onmessage) {
                this._onmessage(resultObj.msg);
            }
        }
        this.socket.onclose = (evt: CloseEvent) => {
            this.socket.onerror = null;

            try {
                if (this.onerror) {
                    this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(() => { this.connect(); }, 1000);
        }
        this.socket.onerror = (evt: Event) => {
            this.socket.onclose = null;
            try {
                if (this.onerror) {
                    this.onerror("无法连接服务器");
                }
            }
            catch (e) {
            }
            setTimeout(() => { this.connect(); }, 1000);
        }
    }


}

class WayScriptRemotingChild extends WayScriptRemoting {
}

enum WayVirtualWebSocketStatus {
    none = 0,
    connected = 1,
    error = 2,
    closed = 3
}

class WayVirtualWebSocket {
    private guid: string;
    private url: string;
    private status: WayVirtualWebSocketStatus = WayVirtualWebSocketStatus.none;
    private errMsg: string;
    private lastMessage: any;
    private receiver: WayScriptInvoker;
    private _onopen: (event: any) => void;
    private _onmessage: (event: any) => void;
    private _onclose: (event: any) => void;
    private _onerror: (event: any) => void;
    private sendQueue: any[] = [];
    binaryType: string = "string";

    get onopen(): (event: any) => void {
        return this._onopen;
    }
    set onopen(value: (event: any) => void) {
        this._onopen = value;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            if (this._onopen) {
                this._onopen({});
            }
        }
    }

    get onmessage(): (event: any) => void {
        return this._onmessage;
    }
    set onmessage(value: (event: any) => void) {
        this._onmessage = value;
        if (this.status == WayVirtualWebSocketStatus.connected) {
            if (this._onmessage) {
                this._onmessage({ data: this.lastMessage });
            }
        }
    }

    get onclose(): (event: any) => void {
        return this._onclose;
    }
    set onclose(value: (event: any) => void) {
        this._onclose = value;
        if (this.status == WayVirtualWebSocketStatus.closed) {
            if (this._onclose) {
                this._onclose({});
            }
        }
    }

    get onerror(): (event: any) => void {
        return this._onerror;
    }
    set onerror(value: (event: any) => void) {
        this._onerror = value;
        if (this.status == WayVirtualWebSocketStatus.error) {
            if (this._onerror) {
                this._onerror({});
            }
        }
    }

    constructor(_url: string) {
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

    close(): void {
        this.status = WayVirtualWebSocketStatus.closed;
        this.receiver.abort();
        if (this._onclose) {
            this._onclose({});
        }
    }
    private init(): void {
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
                }
            }
            else {
                this.guid = result;
                this.status = WayVirtualWebSocketStatus.connected;
                if (this._onopen) {
                    this._onopen({});
                }
                this.receiveChannelConnect();
            }
        };
        invoker.Post({ "mode": "init" });
    }

    send(data): void {
        if (this.sendQueue.length > 0) {
            this.sendQueue.push(data);
            return;
        }
        var invoker = new WayScriptInvoker(this.url);
        invoker.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
                }
                this.sendQueue = [];
            }
            else {
                this.sendQueue.pop();
            }
        }
        if (this.binaryType == "arraybuffer") {
            data = this.arrayBufferToString(data);

        }
        invoker.Post({
            "mode": "send",
            "data": data,
            "id": this.guid,
            "binaryType": this.binaryType
        });
    }

    private arrayBufferToString(data) {
        var array = new Uint8Array(data);
        var str = "";
        for (var i = 0, len = array.length; i < len; ++i) {
            str += "%" + array[i].toString(16);
        }

        return str;
    }

    private receiveChannelConnect(): void {
        this.receiver = new WayScriptInvoker(this.url);
        this.receiver.setTimeout(0);
        this.receiver.onCompleted = (result, err) => {
            if (err) {
                this.status = WayVirtualWebSocketStatus.error;
                this.errMsg = err;
                if (this._onerror) {
                    this._onerror({ data: this.errMsg });
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
                this.lastMessage = result;
                if (this._onmessage && this.status == WayVirtualWebSocketStatus.connected) {
                    this._onmessage({ data: this.lastMessage });
                }
                if (this.status == WayVirtualWebSocketStatus.connected) {
                    this.receiveChannelConnect();
                }
            }
        };
        this.receiver.Post({
            "mode": "receive",
            "id": this.guid,
            "binaryType": this.binaryType
        });
        setTimeout(() => this.sendHeart(), 30000);
    }

    private sendHeart(): void {
        if (this.status == WayVirtualWebSocketStatus.connected) {
            var invoker = new WayScriptInvoker(this.url);
            invoker.Post({
                "mode": "heart",
                "id": this.guid
            });
            setTimeout(() => this.sendHeart(), 30000);
        }
    }
}

class WayScriptInvoker {
    url: string;
    async: boolean = true;
    onBeforeInvoke: () => any;
    onInvokeFinish: () => any;
    onCompleted: (result: any, err: any) => any;
    private xmlHttp: XMLHttpRequest;

    constructor(_url: string) {
        if (_url) {
            this.url = _url;
        }
        else {
            this.url = window.location.href;
        }

    }
    abort(): void {
        if (this.xmlHttp) {
            this.xmlHttp.abort();
        }
    }
    setTimeout(millseconds: number): void {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        this.xmlHttp.timeout = millseconds;

    }

    Post(obj) {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }

        if (this.onBeforeInvoke)
            this.onBeforeInvoke();

        this.xmlHttp.onreadystatechange = () => this.xmlHttpStatusChanged();
        this.xmlHttp.onerror = (e) => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "无法连接服务器");
            }
        }
        this.xmlHttp.ontimeout = () => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "连接服务器超时");
            }
        }
        this.xmlHttp.open("POST", this.url, this.async);
        this.xmlHttp.setRequestHeader("Content-Type", "application/json");
        this.xmlHttp.send(JSON.stringify(obj)); //null,对ff浏览器是必须的
    }

    Get(nameAndValues: string[] = null): void {
       
        /*
               escape不编码字符有69个：*，+，-，.，/，@，_，0-9，a-z，A-Z

        encodeURI不编码字符有82个：!，#，$，&，'，(，)，*，+，,，-，.，/，:，;，=，?，@，_，~，0-9，a-z，A-Z

        encodeURIComponent不编码字符有71个：!， '，(，)，*，-，.，_，~，0-9，a-z，A-Z
               */
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }

        if (this.onBeforeInvoke)
            this.onBeforeInvoke();

        this.xmlHttp.onreadystatechange = () => this.xmlHttpStatusChanged();
        this.xmlHttp.onerror = (e) => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "无法连接服务器");
            }
        }
        this.xmlHttp.ontimeout = () => {
            if (this.onInvokeFinish)
                this.onInvokeFinish();
            if (this.onCompleted) {
                this.onCompleted(null, "连接服务器超时");
            }
        }

        var p: string = "";
        if (nameAndValues) {
            for (var i = 0; i < nameAndValues.length; i += 2) {
                if (i > 0)
                    p += "&";
                p += nameAndValues[i] + "=" + (<any>window).encodeURIComponent(nameAndValues[i + 1], "utf-8");

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
    }

    private xmlHttpStatusChanged(): void {
        if (this.xmlHttp.readyState == 4) {

            if (this.onInvokeFinish)
                this.onInvokeFinish();

            if (this.xmlHttp.status == 200) {
                if (this.onCompleted) {
                    this.onCompleted(this.xmlHttp.responseText, null);
                }
            }
        }
    }

    private createXMLHttp(): any {
        var request: any = false;

        // Microsoft browsers
        if ((<any>window).XMLHttpRequest) {
            request = new XMLHttpRequest();
        }
        else if ((<any>window).ActiveXObject) {

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
    }
}

class WayHelper {
    static createWebSocket(url: string): WebSocket {
        if ((<any>window).WebSocket) {
            return new WebSocket(url);
        }
        else {
            return <any>new WayVirtualWebSocket(url);
        }
    }

    static writePage(url: string): void {
        document.write(WayHelper.downloadUrl(url));
    }

    static downloadUrl(url: string): string {
        var invoker = new WayScriptInvoker(url);
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = (ret, err) => {
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
        }
        invoker.Get();
        return result;
    }


    static addEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void {
        if (element.addEventListener) {
            element.addEventListener(eventName, listener, useCapture);
        }
        else {
            (<any>element).attachEvent("on" + eventName, listener);
        }
    }
    static removeEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void {
        if (element.removeEventListener) {
            element.removeEventListener(eventName, listener, useCapture);
        }
        else {
            (<any>element).detachEvent("on" + eventName, listener);
        }
    }
    //触发htmlElement相关事件，如：fireEvent(myDiv , "click");
    static fireEvent(el: HTMLElement, eventName: string): void {
        if (eventName.indexOf("on") == 0)
            eventName = eventName.substr(2);
        var evt;
        if (document.createEvent) { // DOM Level 2 standard 
            evt = document.createEvent("HTMLEvents");
            // 3个参数：事件类型，是否冒泡，是否阻止浏览器的默认行为  
            evt.initEvent(eventName, true, true);
            //evt.initMouseEvent(eventName, true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
            el.dispatchEvent(evt);
        }
        else if ((<any>el).fireEvent) { // IE 
            (<any>el).fireEvent('on' + eventName);
        }
    }
    
    static copyValue(target: any, source: any): void {
        for (var pro in target) {
            var originalvalue = target[pro];
            if (originalvalue != null && typeof originalvalue == "object") {
                WayHelper.copyValue(originalvalue, source[pro]);
            }
            else {
                target[pro] = source[pro];
            }
        }
    }

    static clone(obj: any): any {
        var o;
        if (obj != null && typeof obj == "object") {
            if (obj === null) {
                o = null;
            } else {
                if (obj instanceof Array) {
                    o = [];
                    for (var i = 0, len = obj.length; i < len; i++) {
                        o.push(WayHelper.clone(obj[i]));
                    }
                } else {
                    o = {};
                    for (var j in obj) {
                        o[j] = WayHelper.clone(obj[j]);
                    }
                }
            }
        } else {
            o = obj;
        }
        return o;
    }
}

