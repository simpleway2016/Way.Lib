declare var setMaxDigits: (n: number) => void;
declare class RSAKeyPair { constructor(e: string,n:string, m: string); };
declare var encryptedString: (key: RSAKeyPair, value: string) => string;
window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

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

class WayBaseObject {

}

class WayScriptRemotingUploadHandler {
    abort: boolean = false;
    offset: number = 0;
}
class RSAInfo {
    Exponent: string;
    Modulus: string;
}
class WayScriptRemoting extends WayBaseObject {
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
        super();
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
        invoker.invoke(["m", "{'Action':'init' , 'ClassFullName':'" + remoteName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}"]);

        if (hasErr) {
            throw hasErr;
        }
        else if (result.err) {
            throw result.err;
        }

        var func;
        eval("func = " + result.text);

        var page = <WayScriptRemoting>new func(remoteName);
        page.rsa = result.rsa;

        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID);
        return page;
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
                    eval("func = " + result.text);

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

    uploadFile(fileElement: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler {
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
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "','FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
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
                        this.uploadFile(file, callback, handler);
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
                            this.uploadFile(file, callback, handler);
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
        if (value.length <= 58) {
            return encryptedString(key, value);
        }
        else {
            var result = "";
            var total = value.length;
            for (var i = 0; i < value.length; i += 58) {
                var text = value.substr(i, Math.min(58, total));
                total -= text.length;
                result += encryptedString(key, text);
            }
            return result;
        }
    }

    pageInvoke(name: string, parameters: any[], callback: any, async: boolean = true, useRsa: boolean=false) {
        try {
            if (WayScriptRemoting.onBeforeInvoke) {
                WayScriptRemoting.onBeforeInvoke(name, parameters);
            }

            var paramerStr = "";
            if (parameters) {
                parameters.forEach((p) => {
                    if (paramerStr.length > 0)
                        paramerStr += ",";

                    var itemstr = JSON.stringify(useRsa && p && typeof p == "string" ? this.encrypt(p) : p);
                    paramerStr += JSON.stringify(itemstr);
                });
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
                    eval("resultObj=" + ret);

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
            invoker.invoke(["m", "{'ClassFullName':'" + this.classFullName + "','MethodName':'" + name + "','Parameters':[" + paramerStr + "] , 'SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}"]);

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
        invoker.invoke(["mode", "init"]);
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
        invoker.invoke(["mode", "send", "data", data, "id", this.guid, "binaryType", this.binaryType]);
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
        this.receiver.invoke(["mode", "receive", "id", this.guid, "binaryType", this.binaryType]);
        setTimeout(() => this.sendHeart(), 30000);
    }

    private sendHeart(): void {
        if (this.status == WayVirtualWebSocketStatus.connected) {
            var invoker = new WayScriptInvoker(this.url);
            invoker.invoke(["mode", "heart", "id", this.guid]);
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
    method: string = "POST";
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
    invoke(nameAndValues: string[]): void {
        /*
        escape不编码字符有69个：*，+，-，.，/，@，_，0-9，a-z，A-Z

encodeURI不编码字符有82个：!，#，$，&，'，(，)，*，+，,，-，.，/，:，;，=，?，@，_，~，0-9，a-z，A-Z

encodeURIComponent不编码字符有71个：!， '，(，)，*，-，.，_，~，0-9，a-z，A-Z
        */
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        var p: string = "";
        if (nameAndValues) {
            for (var i = 0; i < nameAndValues.length; i += 2) {
                if (i > 0)
                    p += "&";
                p += nameAndValues[i] + "=" + (<any>window).encodeURIComponent(nameAndValues[i + 1], "utf-8");

            }
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

class WayTemplate {
    content: string;
    match: string;
    //匹配当前行的当前状态模式
    mode: string;
    constructor(_content: string, _match: string = null, _mode: string = "") {
        this.content = _content;
        this.match = _match;
        this.mode = _mode ? _mode : "";
    }
}



class WayHelper {
    //判断数组是否包含某个值
    static contains(arr, value): boolean {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == value)
                return true;
        }
        return false;
    }

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
        invoker.method = "GET";
        invoker.async = false;
        var errcount = 0;
        var result;
        invoker.onCompleted = (ret, err) => {
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
        }
        invoker.invoke([]);
        return result;
    }

    static findBindingElements(element: HTMLElement): any[] {
        var result = [];
        WayHelper.findInnerBindingElements(result, element);
        return result;
    }

    static findInnerBindingElements(result: any[], element: HTMLElement) {
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
        if (element.tagName.indexOf("Way") == 0 || (<any>element).WayControl) {
            return;
        }
        for (var i = 0; i < element.children.length; i++) {
            WayHelper.findInnerBindingElements(result, <any>element.children[i]);
        }
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

    static getDataForDiffent(originalData: any, currentData: any): any {
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
    }

    static replace(content: string, find: string, replace: string): string {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
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

class WayBindMemberConfig {
    elementMember: string;
    dataMember: string;
    element: HTMLElement;
    expressionString: string;
    dataMemberExp: any;
    constructor(_elementMember: string, _dataMember: string, _element: HTMLElement) {
        this.elementMember = _elementMember;
        this.dataMember = _dataMember;
        this.element = _element;
    }
}

class WayBindingElement extends WayBaseObject {
    element: HTMLElement;
    model: any;
    dataSource: any;
    configs: WayBindMemberConfig[] = [];
    expressionConfigs: WayBindMemberConfig[] = [];

    constructor(_element: HTMLElement, _model: any, _dataSource: any, expressionExp: RegExp, dataMemberExp: RegExp) {
        super();
        this.element = _element;
        this.model = _model;
        this.dataSource = _dataSource;
        
        var elements = WayHelper.findBindingElements(_element);
        for (var i = 0; i < elements.length; i++) {
            var ctrlEle = elements[i];
            this.initEle(ctrlEle, _dataSource, expressionExp, dataMemberExp);
        }
    }

    private initEle(ctrlEle: HTMLElement, _dataSource: any, expressionExp: RegExp, dataMemberExp: RegExp) {
        var databind = ctrlEle.getAttribute("databind");
        var _expressionString = ctrlEle.getAttribute("expression"); 
        var isWayControl = false;
        if ((<any>ctrlEle).WayControl) {
            ctrlEle = (<any>ctrlEle).WayControl;
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
                        (<any>ctrlEle).data = this.model;

                        if (_dataSource) {
                            var addevent = false;
                            if ((<any>ctrlEle).memberInChange && WayHelper.contains((<any>ctrlEle).memberInChange, eleMember))
                                addevent = true;
                            else if ( eleMember == "value" || eleMember == "checked")
                                addevent = true;

                            if (addevent) {
                                if (ctrlEle.addEventListener) {
                                    ctrlEle.addEventListener("change", () => { this.onvalueChanged(config); });
                                }
                                else {
                                    (<any>ctrlEle).attachEvent("onchange", () => { this.onvalueChanged(config); });
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

                if (!(<any>ctrlEle).expressionDatas) {
                    (<any>ctrlEle).expressionDatas = [];
                }

                (<any>ctrlEle).expressionDatas.push({ exp: dataMemberExp, data: _dataSource });
                var config = new WayBindMemberConfig(null, <any>datamembers, ctrlEle);
                config.expressionString = _expressionString;
                config.dataMemberExp = dataMemberExp;
                this.expressionConfigs.push(config);

            }
        }
    }
    

    doExpression(__config: WayBindMemberConfig) {
        var ___element = __config.element;
        var exp = __config.expressionString;
        var matches = exp.match(/[\W]?(this\.)/g);
        for (var i = 0; i < matches.length; i++) {
            var r = matches[i].replace("this.", "___element.");
            exp = exp.replace(matches[i], r);
        }

        for (var i = 0; i < (<any>__config.element).expressionDatas.length; i++) {
            var expItem = (<any>__config.element).expressionDatas[i];
            var matchs = exp.match(expItem.exp);
            if (matchs) {
                for (var j = 0; j < matchs.length; j++) {
                    var match = matchs[j];
                    var dataMember = match.substr(1);
                    exp = exp.replace(match, "__config.element.expressionDatas["+i+"].data." + dataMember);
                }
            }
        }

        eval(exp);
    }

    initEleValues(model): void{
        this.model = model;
        for (var i = 0; i < this.configs.length; i++) {

            eval("this.configs[i].element." + this.configs[i].elementMember + "=model." + this.configs[i].dataMember + ";");
        }
    }

    private onvalueChanged(fromWhichConfig: WayBindMemberConfig): void {
        try {
            if (this.configs.length == 0 || !this.model)
                return;//绑定已经移除了

            var model = this.model;
            var value = (<any>fromWhichConfig.element)[fromWhichConfig.elementMember];

            eval("model." + fromWhichConfig.dataMember + "=value;");
        }
        catch (e) {
            throw "WayBindingElement onvalueChanged error:" + e.message;
        }
    }

    getDataMembers(): string[] {
        var result: string[] = [];
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            result.push(config.dataMember);
        }
        return result;
    }

    onchange(itemIndex, name, value): void {
        try {
            for (var i = 0; i < this.configs.length; i++) {
                var config = this.configs[i];
                if (config.dataMember == name) {
                    if (eval("config.element." + config.elementMember + "!=value")) {
                        eval("config.element." + config.elementMember + "=value");
                        if (!(<any>config.element).element)//如果不是WayControl
                            WayHelper.fireEvent(config.element, 'change');
                    }
                }
            }

            for (var i = 0; i < this.expressionConfigs.length; i++) {
                var config = this.expressionConfigs[i];
                if (WayHelper.contains(config.dataMember , name) ) {
                    this.doExpression(config);
                }
            }
        }
        catch (e) {
        }
    }
}

class WayDataBindHelper {
    static bindings: WayBindingElement[] = [];

    static getObjectStr(obj: any, onchangeMembers: string[], parent: any): string {
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
                str += "get " + pro + "(){return item." + parent + pro + ";},"
                str += "set " + pro + "(v){if(item." + parent + pro + "!=v){item." + parent + pro + "=v;" + onchangeStr + "}}";
                index++;
            }
            str += "}";
            return str;
        }
        else {
            return JSON.stringify(obj);
        }
    }

    private static setSubObjectModel(model, rootModel): void {
        for (var p in model) {
            var value = model[p];

            if (value != null && typeof value == "object" && typeof value.getModel == "function") {

                var obj = model[p];
                obj.getModel = function () {
                    return rootModel;
                }

                WayDataBindHelper.setSubObjectModel(obj, rootModel);
            }
        }
    }

    static addPropertyToObject(model,obj,source, _itemIndex,  propertyName, fullMemberName, _onchange): void {
        var member = propertyName.split('.')[0];
        //var prototype = Object.getPrototypeOf(obj);
        if (eval("typeof obj." + member +" == \"undefined\"")) {
            if (member == propertyName) {//不是obj.name的模式
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
    }

    static cloneObjectForBind(obj: any, _itemIndex: any, onchangeMembers: string[], _onchange: any): any {
        if (obj.getSource && typeof obj.getSource == "function") {
           //需要增加没有的属性
            var prototype = Object.getPrototypeOf(obj);
            for (var i = 0; i < onchangeMembers.length; i++) {
                WayDataBindHelper.addPropertyToObject(obj, obj, obj.getSource(), _itemIndex, onchangeMembers[i], onchangeMembers[i], _onchange);
            }
            return obj;
        }

        var str = WayDataBindHelper.getObjectStr(obj, onchangeMembers, null);
        str = "result=(function(item,itemIndex,onProChange){ return " + str + ";})(obj,_itemIndex,_onchange);"
        var result;
        eval(str);
        WayDataBindHelper.setSubObjectModel(result, result);
        return result;
    }

    private static onchange(toCheckedEles, model, dataSource, itemIndex, name, value) {
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
    }

    static removeDataBind(element: HTMLElement) {
        for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
            if (WayDataBindHelper.bindings[i] != null && WayDataBindHelper.bindings[i].element == element) {
                WayDataBindHelper.bindings[i].configs = [];
                WayDataBindHelper.bindings[i] = null;
                //不要break，可能同一个element有多个绑定
            }
        }
    }

    //获取htmlElement里面所有用于绑定的字段名称
    static getBindingFields(element: HTMLElement,
        expressionExp: RegExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g,
        dataMemberExp: RegExp = /\@(\w|\.)+/g): string[] {
        if (typeof element == "string") {
            element = document.getElementById(<any>element);
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
    }
    //替换html里的变量
    static replaceHtmlFields(templateHtml, data): string {
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
    }

    static dataBind(element: any, data: any, tag: any = null,
        expressionExp: RegExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g,
        dataMemberExp: RegExp = /\@(\w|\.)+/g,
    doexpression:boolean = false): any {
        if (typeof element == "string") {
            element = document.getElementById(<any>element);
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
            //expression有可能包含$name @name两种变量，所以是否绑定后，马上执行一次doExpression，应该由调用者决定，因为只有所有涉及的model都绑定后，才可以执行
            for (var i = 0; i < bindingInfo.expressionConfigs.length; i++) {
                bindingInfo.doExpression(bindingInfo.expressionConfigs[i]);
            }
        }
        return model;
    }
}

class WayControlHelper {
    static getValue(ctrl: HTMLElement): string {
        switch (ctrl.tagName) {
            case "INPUT":
                if ((<any>ctrl).type == "checkbox")
                    return (<any>ctrl).checked;
                else
                    return (<any>ctrl).value;
            case "SELECT":
                return (<any>ctrl).value;
        }
        return "";
    }
    static setValue(ctrl: HTMLElement, value: any): void {
        switch (ctrl.tagName) {
            case "INPUT":
                if ((<any>ctrl).type == "checkbox")
                    (<any>ctrl).checked = value;
                else if (value && value.value)
                    (<any>ctrl).value = value.value;
                else
                    (<any>ctrl).value = value;
                break;
            case "SELECT":
                if (value && value.value)
                    (<any>ctrl).value = value.value;
                else
                    (<any>ctrl).value = value;
                break;
        }
    }
}

class WayPageInfo {
    PageIndex: number = 0;
    PageSize: number = 10;
    //正在看第几页,for pageMode
    ViewingPageIndex: number = 0;
}
interface IPageable {
    shouldLoadMorePage(): void;
    hasMorePage: boolean;
    pageMode: boolean;
}
class WayPager {
    scrollable: JQuery;
    control: IPageable;
    private scrollListener;
    constructor(_scrollable: JQuery, _ctrl: IPageable) {
        this.scrollable = _scrollable;
        this.control = _ctrl;
        this.scrollListener = () => { this.onscroll(); };
        WayHelper.addEventListener(_scrollable[0], "scroll", this.scrollListener, undefined);
    }



    private onscroll(): void {
        if (!this.control.hasMorePage || this.control.pageMode)
            return;

        var y = this.scrollable.scrollTop();
        var x = this.scrollable.scrollLeft();
        var height = this.scrollable.height();
        if (y + height > this.scrollable[0].scrollHeight * 0.86) {
            this.control.shouldLoadMorePage();
        }
    }
}

class WayProgressBar {
    private loading: any;
    color: string;
    private showRef: number = 0;
    private lastMouseDownLocation = null;
    private lastMouseDownTime = null;
    private timingNumber: number = 0;

    constructor(_color: string = "#FF2E82") {
        this.color = _color;

        if (document.body.addEventListener) {
            document.body.addEventListener("mousedown", (e) => { this.mousedown(e); });
        }
        else {
            (<any>document.body).attachEvent("onmousedown", (e) => { this.mousedown(e); });
        }
    }

    private mousedown(e) {
        e = e ? e : window.event;
        var x = e.clientX;
        var y = e.clientY;

        this.lastMouseDownLocation = { "x": x, "y": y };
        this.lastMouseDownTime = new Date().getTime();
    }

    private initLoading(): void {
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

                var cx = this.padding + 50,
                    cy = this.padding + 50,
                    _ = this._,
                    angle = (Math.PI / 180) * (point.progress * 360),
                    innerRadius = index === 1 ? 10 : 25;

                _.beginPath();
                _.moveTo(point.x, point.y);
                _.lineTo(
                    (Math.cos(angle) * innerRadius) + cx,
                    (Math.sin(angle) * innerRadius) + cy
                );
                _.closePath();
                _.stroke();

            },
            path: [
                ['arc', 50, 50, 40, 0, 360]
            ]
        };
        this.loading = new (<any>window).Sonic(pa);
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
    }

    show(centerElement: JQuery): void {
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
        this.timingNumber = setTimeout(() => {
            if (this.timingNumber) {
                this.timingNumber = 0;
                loadele.show();
                this.loading.play();
            }
        }, 1000);

    }
    hide(): void {

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
    }
}

class WayPopup {
    template: string = "<div style='background-color:#ffffff;color:red;font-size:12px;border:1px solid #cccccc;padding:2px;'>{0}</div>";
    container: JQuery;
    show(content: string, element: JQuery, direction: string): void {
        if (!this.container) {
            this.container = $(WayHelper.replace(this.template, "{0}", content));
            this.container.css({
                "position": "absolute",
                "visibility": "hidden",
                "z-index": 199,
            });
            this.container.click(() => {
                this.hide();
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
    }
    hide(): void {
        if (this.container) {
            this.container.css({
                "visibility": "hidden"
            });
        }
    }
}

class WayValidator {
    element: JQuery;
    popup: WayPopup;
    constructor(_element: HTMLElement) {
        this.element = $(_element);
        if (_element.addEventListener) {
            _element.addEventListener("change", () => { this.onvalueChanged(); });
        }
        else {
            (<any>_element).attachEvent("onchange", () => { this.onvalueChanged(); });
        }
        this.popup = new WayPopup();
    }

    validate(): boolean {
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
    }

    onvalueChanged(): void {
        this.validate();
    }
}

class WayDBContext {
    private remoting: WayScriptRemoting;
    //数据源
    datasource: any;
    constructor(controller: string, _datasource: string) {
        if (typeof controller == "object") {
            this.remoting = <any>controller;
        }
        else {
            this.remoting = WayScriptRemoting.createRemotingController(controller);
        }
        this.datasource = _datasource;
    }


    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async: boolean = true): void {
        searchModel = searchModel ? JSON.stringify(searchModel) : "";
        this.remoting.pageInvoke("GetDataSource", [pageinfo, this.datasource, bindFields, searchModel], (ret, err) => {
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
    }

    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async: boolean = true): void {
        var pageinfo = new WayPageInfo();
        pageinfo.PageIndex = 0;
        pageinfo.PageSize = 1;
        this.getDatas(pageinfo, bindFields, searchModel, (_data, _pkid, err) => {
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
    }

    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void {
        this.remoting.pageInvoke("SaveData", [this.datasource, JSON.stringify(data)], (idvalue: any, err: any) => {
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
    }
    count(searchModel: any, callback: (data: any, err: any) => void): void {

        searchModel = searchModel ? JSON.stringify(searchModel) : "";

        this.remoting.pageInvoke("Count", [this.datasource, searchModel], callback);
    }
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void {

        searchModel = searchModel ? JSON.stringify(searchModel) : "";

        this.remoting.pageInvoke("Sum", [this.datasource, fields, searchModel], callback);
    }
}


class WayGridView extends WayBaseObject implements IPageable {
    element: JQuery;
    private itemContainer: JQuery;
    private itemTemplates: WayTemplate[] = [];
    items: JQuery[] = [];
    //原始itemdata
    private originalItems = [];
    private bodyTemplateHtml: string;
    dbContext: WayDBContext;
    private pageinfo: WayPageInfo = new WayPageInfo();
    private pager: WayPager;
    private fieldExp: RegExp = /\{\@(\w|\.|\:)+\}/g;
    private loading: WayProgressBar = new WayProgressBar("#cccccc");
    private footerItem: JQuery;
    // 标识当前绑定数据的事物id
    private transcationID: number = 1;
    private primaryKey: string;
    hasMorePage: boolean;
    //设置，必须获取的字段(因为没有在模板中出现的字段，不会输出)
    dataMembers: string[] = [];

    //是否支持下拉刷新
    //下拉刷新必须定义body模板
    supportDropdownRefresh: boolean = false;

    //定义item._status的数据原型，可以修改此原型达到期望的目的
    itemStatusModel: any = { Selected: false };

    //是否使用翻页模式
    pageMode: boolean = false;
    //pageMode模式下，预先加载多少页数据
    preLoadNumForPageMode: number = 1;
    onViewPageIndexChange: (index: number) => void;

    //header模板
    header: WayTemplate;
    //footer模板
    footer: WayTemplate;
    //搜索条件model
    searchModel: any = {};
    allowEdit: boolean = false;
    //数据源
    _datasource: any;
    get datasource(): any {
        return this._datasource;
    }
    set datasource(_v: any) {
        if (typeof _v == "string" && _v.indexOf("[") == 0) {
            _v = JSON.parse(_v);
        }
        this._datasource = _v;
        this.dbContext.datasource = _v;
    }

    //用于自定义显示错误
    onerror: (err: any) => void;
    //在databind调用时触发
    onDatabind: () => void;
    //item创建后触发
    onCreateItem: (item: JQuery, model: string) => void;
    //在从服务器拉取数据，并创建item后触发
    onAfterCreateItems: (total: number, hasMore: boolean) => void;
    //item大小变化事件
    onItemSizeChanged: () => void;

    constructor(elementId: string, _pagesize: number = 10) {
        super();
        try {
            if (isNaN(_pagesize))
                _pagesize = 10;

            var controller = document.body.getAttribute("controller");
           
            this.dbContext = new WayDBContext(controller, null);
            if (typeof elementId == "string")
                this.element = $("#" + elementId);
            else if ((<any>elementId).tagName)
                this.element = $(<any>elementId);
            else
                this.element = <any>elementId;
            this.allowEdit = this.element.attr("allowedit") == "true";
            this.element.css(
                {
                    "overflow-y": "auto",
                    "-webkit-overflow-scrolling": "touch"
                });

            var isTouch = "ontouchstart" in this.element[0];
            if (!isTouch)
                this.supportDropdownRefresh = false;

            this.datasource = this.element.attr("datasource");

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
    //初始化下拉刷新事件
    private initRefreshEvent(touchEle: JQuery): void {

        var isTouch = "ontouchstart" in this.itemContainer[0];
        if (!isTouch)
            this.supportDropdownRefresh = false;

        var moving = false;
        var isTouchToRefresh = false;
        //先预设一下,否则有时候第一次设置touchEle会白屏
        touchEle.css("will-change", "transform");
        var point;

        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", (e) => {
            if (!this.supportDropdownRefresh || this.pageMode)
                return;

            isTouchToRefresh = false;
            if (this.element.scrollTop() > 0) {
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

        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", (e: TouchEvent) => {
            if (moving) {
                if (this.element.scrollTop() > 0) {
                    moving = false;
                    return;
                }
                e = e || <any>window.event;
                var y = isTouch ? e.touches[0].clientY : (<any>e).clientY;
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

        var touchoutFunc = (e: TouchEvent) => {

            if (moving) {
                moving = false;

                e = e || <any>window.event;

                var y = isTouch ? e.changedTouches[0].clientY : (<any>e).clientY;
                y = (y - point.y);

                isTouchToRefresh = (y > this.element.height() * 0.15);
                touchEle.css({
                    "transition": "transform 0.5s",
                    "-webkit-transform": "translate(0px,0px)",
                    "-moz-transform": "translate(0px,0px)",
                    "transform": "translate(0px,0px)"
                });
            }
        };

        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);

        this.element[0].ontouchcancel = () => {
            isTouchToRefresh = false;
            touchEle.css({
                "transition": "transform 0.5s",
                "-webkit-transform": "translate(0px,0px)",
                "-moz-transform": "translate(0px,0px)",
                "transform": "translate(0px,0px)"
            });
        };

        WayHelper.addEventListener(touchEle[0], "transitionend", (e) => {
            touchEle.css({
                "transition": "",
                "will-change": "auto"
            });
            if (isTouchToRefresh) {
                this.databind();
            }
        }, true);
    }


    private showLoading(): void {
        this.loading.show(this.element);
    }
    private hideLoading(): void {
        this.loading.hide();
    }

    getTemplateOuterHtml(element: any): string {
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
    }

    //添加item模板
    addItemTemplate(temp: WayTemplate): void {
        this.itemTemplates.push(temp);
    }
    //删除item模板
    removeItemTemplate(temp: WayTemplate): void {
        this.itemTemplates[this.itemTemplates.indexOf(temp)] = null;
    }

    private replace(content: string, find: string, replace: string): string {
        while (content.indexOf(find) >= 0) {
            content = content.replace(find, replace);
        }
        return content;
    }

    count(callback: (data: any, err: any) => void): void {
        this.showLoading();
        this.dbContext.count(this.searchModel, (data: any, err: any) => {
            this.hideLoading();
            callback(data, err);
        });
    }
    sum(fields: string[], callback: (data: any, err: any) => void): void {
        this.showLoading();
        this.dbContext.sum(fields, this.searchModel, (data: any, err: any) => {
            this.hideLoading();
            callback(data, err);
        });
    }

    save(itemIndex: number, callback: (idvalue: any, err: any) => void): void {
        if (this.allowEdit == false) {
            callback(null, "此WayGridView未设置为可编辑,请设置allowedit=\"true\"");
            return;
        }

        var item = this.items[itemIndex];
        var model = (<any>item).data;
        var data = this.originalItems[itemIndex];
        var changedData = WayHelper.getDataForDiffent(data, model);

        if (changedData) {

            if (this.primaryKey && this.primaryKey.length > 0) {
                eval("changedData." + this.primaryKey + "=model." + this.primaryKey + ";");
            }

            this.showLoading();
            this.dbContext.saveData(changedData, this.primaryKey, (data, err) => {
                this.hideLoading();
                if (err) {
                    callback(null, err);
                }
                else {
                    if (this.primaryKey && this.primaryKey.length > 0) {
                        callback(eval("changedData." + this.primaryKey), null);
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
    }


    private onErr(err: any): void {
        if (this.onerror) {
            this.onerror(err);
        }
        else
            throw err;
    }

    private contains(arr: string[], find: string): boolean {
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == find) {
                return true;
            }
        }
        return false;
    }

    private getBindFields(): string[] {
        var result: string[] = [];
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
    }



    //绑定数据
    databind(): void {
        if (this.pageMode) {//翻页模式
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
    }

    shouldLoadMorePage(): void {
        this.hasMorePage = false;//设为false，可以禁止期间被Pager再次调用
        var pageData;

        this.transcationID++;
        var mytranId = this.transcationID;
        if (typeof this.datasource == "string") {
            this.showLoading();
            this.dbContext.getDatas(this.pageinfo, this.getBindFields(), (this.searchModel.submitObject && typeof this.searchModel.submitObject == "function") ? this.searchModel.submitObject() : this.searchModel,
                (ret, pkid, err) => {
                this.hideLoading();

                if (mytranId != this.transcationID)
                    return;

                if (err) {
                    this.hasMorePage = true;
                    this.onErr(err);
                }
                else {
                    if (pkid != null) {
                        this.primaryKey = pkid;
                    }
                    pageData = ret;
                    this.bindDataToGrid(pageData);
                }
            });

        }
        else {
            pageData = this.getDataByPagesize(this.datasource);
            this.bindDataToGrid(pageData);
        }

    }

    private bindDataToGrid(pageData: any): void {
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
    }

    private getDataByPagesize(datas: any[]): any {
        if (datas.length <= this.pageinfo.PageSize)
            return datas;

        var result = [];
        var end = this.pageinfo.PageSize * (this.pageinfo.PageIndex + 1);
        for (var i = this.pageinfo.PageSize * this.pageinfo.PageIndex; i < end && i < datas.length; i++) {
            result.push(datas[i]);
        }
        return result;
    }

    //把两个table的td设为一样的宽度
    setSameWidthForTables(tableSource: JQuery, tableHeader: JQuery): void {
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
        for (var i = 0; i < (<any>tableHeader[0]).children[0].children.length - 1; i++) {
            var headerTD = $((<any>tableHeader[0]).children[0].children[i]);

            var colspan: any = headerTD.attr("colspan");
            if (!colspan || colspan == "")
                colspan = 1;
            else
                colspan = parseInt(colspan);

            var cellwidth = 0;
            for (var j = sourceIndex; j < sourceIndex + colspan; j++) {
                var sourceTD = $((<any>tableSource[0]).children[0].children[j]);
                cellwidth += sourceTD.width();
            }
            sourceIndex += colspan;
            headerTD.width(cellwidth);
        }

    }

    private findItemTemplate(data: any, mode: string = ""): WayTemplate {
        if (this.itemTemplates.length == 1)
            return this.itemTemplates[0];

        var expression: RegExp = /\@(\w|\.|\:)+/g;

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
    }

    //改变指定item为指定的mode
    changeMode(itemIndex: number, mode: string): JQuery {
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
    }


    //接受item数据的更新，如当前item的数据和很多input进行绑定，input值改变后，并且同步到数据库，
    //那么updateItemData方法就是同步本地GridView，否则调用changeMode，item显示的值还是原来的值
    acceptItemChanged(itemIndex: number) {
        var item = this.items[itemIndex];
        var mydata = (<any>item).data.getSource();
        this.originalItems[itemIndex] = WayHelper.clone(mydata);
    }

    //从服务器更新指定item的数据，并重新绑定
    rebindItemFromServer(itemIndex: number, mode: string, callback: (data: any, err: any) => void = null) {
        var searchmodel = {};
        var item = this.items[itemIndex];
        searchmodel[this.primaryKey] = (<any>item).data[this.primaryKey];
        this.dbContext.getDataItem(this.getBindFields(), searchmodel, (data: any, err: any) => {
            if (!err) {
                this.originalItems[itemIndex] = data;
                if (typeof mode == "undefined")
                    mode = (<any>item)._mode;
                this.changeMode(itemIndex, mode);
            }
            if (callback)
                callback(data, err);
        });
    }

    private replaceFromString(str: string, itemIndex, statusmodel, data): string{
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
    }

    private replaceVariable(container: HTMLElement, itemIndex, statusmodel, data): void {
        for (var i = 0; i < container.attributes.length; i++) {
            var attName = container.attributes[i].name;
            var attValue = container.getAttribute(attName);
            var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
            if (attValue != formatvalue) {
                container.setAttribute(attName, formatvalue);
            }
        }

        if (container.tagName.indexOf("Way") != 0 && !(<any>container).WayControl) {
            //如果不是WayControl，继续检查内容和子节点
            for (var i = 0; i < container.childNodes.length; i++) {
                var node = container.childNodes[i];
                if (node.nodeType == 3) {
                    //text
                    var attValue: string = (<any>node).data;
                    var formatvalue = this.replaceFromString(attValue, itemIndex, statusmodel, data);
                    if (attValue != formatvalue) {
                        (<any>node).data = formatvalue;
                    }
                }
                else if (node.nodeType == 1) {
                    //htmlelement
                    this.replaceVariable(<any>node, itemIndex, statusmodel, data);
                }
            }
        }
    }

    private createItem(itemIndex: any, mode: string = ""): JQuery {
        //把数据克隆一份
        var currentItemStatus;
        if (itemIndex < this.items.length) {
            currentItemStatus = (<any>this.items[itemIndex]).status;
        }
        var statusmodel = currentItemStatus ? currentItemStatus : this.itemStatusModel;

        var data = WayHelper.clone(this.originalItems[itemIndex]);
        var template = this.findItemTemplate(data, mode);
       
        var itemContent = template.content;

        var item = $(itemContent);
        this.replaceVariable(item[0], itemIndex, statusmodel, data);
        //把WayControl初始化
        for (var i = 0; i < item[0].children.length; i++) {
            checkToInitWayControl(<HTMLElement>item[0].children[i]);
        }
        
        var model = WayDataBindHelper.dataBind(item[0], data, itemIndex, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g);
        //创建status


        var myChangeFunc = statusmodel.onchange;
        var statusData = WayHelper.clone(statusmodel);

        (<any>item).status = WayDataBindHelper.dataBind(item[0], statusData, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g,true);
        if (typeof myChangeFunc == "function") {
            (<any>item).status.onchange = myChangeFunc;
        }

        //建立验证
        if (true) {
            var bindItemElements = item.find("*[databind]");

            var validators: WayValidator[] = [];

            for (var i = 0; i < bindItemElements.length; i++) {
                try {
                    validators.push(new WayValidator(bindItemElements[i]));
                }
                catch (e) {
                }
            }
            if (validators.length > 0) {
                (<any>item).validate = () => {
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
        (<any>item).data = model;
        (<any>item)._mode = mode;

        if (this.onCreateItem) {
            this.onCreateItem(item, mode);
        }

        return item;
    }

    private binddatas(datas: any[]): void {
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
    }

    private initForPageMode(): void {
        if (this.itemContainer[0] != this.element[0]) {
            this.itemContainer[0].parentElement.removeChild(this.itemContainer[0]);
        }
        this.itemContainer = $(document.createElement("DIV"));
        this.element[0].appendChild(this.itemContainer[0]);
        this.element.css(
            {
                "overflow-x": "hidden",
                "overflow-y": "hidden"
            });
        this.itemContainer.css(
            {
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

        WayHelper.addEventListener(this.element[0], isTouch ? "touchstart" : "mousedown", (e) => {
            isTouchToRefresh = false;
            e = e || window.event;
            this.itemContainer.css("will-change", "transform");
            point = {
                x: isTouch ? e.touches[0].clientX : e.clientX,
                y: isTouch ? e.touches[0].clientY : e.clientY,
                time: new Date().getTime()
            };

            moving = true;
        }, true);

        WayHelper.addEventListener(this.element[0], isTouch ? "touchmove" : "mousemove", (e: TouchEvent) => {
            if (moving) {

                e = e || <any>window.event;
                var x = isTouch ? e.touches[0].clientX : (<any>e).clientX;
                x = (x - point.x);
                if (x > 0 && this.pageinfo.ViewingPageIndex == 0) {
                    x /= 3;
                }
                else if (x < 0 && this.pageinfo.ViewingPageIndex == this.itemContainer[0].children.length - 1) {
                    x /= 3;
                }
                if (Math.abs(x) > 0) {
                    isTouchToRefresh = true;
                }

                x = "translate(" + (x - this.pageinfo.ViewingPageIndex * this.element.width()) + "px,0px)";
                this.itemContainer.css({
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

        var touchoutFunc = (e: TouchEvent) => {

            if (moving) {
                moving = false;

                e = e || <any>window.event;

                var x = isTouch ? e.changedTouches[0].clientX : (<any>e).clientX;
                x = (x - point.x);

                if (x != 0) {
                    if (x > this.element.width() / 3 || (x > this.element.width() / 10 && new Date().getTime() - point.time < 500)) {
                        if (this.pageinfo.ViewingPageIndex > 0) {
                            this.pageinfo.ViewingPageIndex--;
                        }
                    }
                    else if (-x > this.element.width() / 3 || (-x > this.element.width() / 10 && new Date().getTime() - point.time < 500)) {
                        if (this.pageinfo.ViewingPageIndex < this.itemContainer[0].children.length - 1) {
                            this.pageinfo.ViewingPageIndex++;
                        }
                    }

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

        this.element[0].ontouchcancel = () => {
            var desLocation = "translate(" + -this.pageinfo.ViewingPageIndex * this.element.width() + "px,0px)";

            this.itemContainer.css({
                "transition": "transform 0.5s",
                "-webkit-transform": desLocation,
                "-moz-transform": desLocation,
                "transform": desLocation
            });
        };

        WayHelper.addEventListener(this.element[0], isTouch ? "touchend" : "mouseup", touchoutFunc, undefined);


        WayHelper.addEventListener(this.itemContainer[0], "transitionend", (e) => {
            this.itemContainer.css({
                "transition": "",
            });
            if (this.onViewPageIndexChange) {
                this.onViewPageIndexChange(this.pageinfo.ViewingPageIndex);
            }
            if (this.pageinfo.ViewingPageIndex == this.itemContainer[0].children.length - 1 && this.hasMorePage) {
                this.shouldLoadMorePage();
            }
        }, true);
    }

    //设置当前观看那一页，执行这个方法，pageMode必须是true
    setViewPageIndex(index: number): void {
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
    }

    private binddatas_pageMode(datas: any[]): void {

        if (datas.length == 0)
            return;

        try {
            if (!this.bodyTemplateHtml) {
                this.bodyTemplateHtml = "<div></div>";
            }

            this.itemContainer.width(this.itemContainer.width() + this.element.width());

            var divContainer = $(this.bodyTemplateHtml);
            divContainer.css(
                {
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
    }
}

class WayDropDownList {
    memberInChange: ["text","value"];
    textElement: JQuery;
    actionElement: JQuery;
    element: JQuery;
    itemContainer: JQuery;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private isBindedGrid: boolean = false;
    private windowObj: JQuery;
    private maskLayer: JQuery;
    valueMember: string;
    textMember: string;
    private _value: string;

    get value(): string {
        return this._value;
    }
    set value(v: string) {
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
    }

    private _text: string;
    get text(): string {
        return this._text;
    }
    set text(v: string) {
        if (v != this._text) {
            this._text = v;
            this._value = this.getValueByText(v);

            this.fireEvent("change");
        }
    }

    onchange: any = null;

    constructor(elementid: string, datasource: any) {
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        (<any>this.element[0]).WayControl = this;
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
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");

        if (this.actionElement) {
            this.init();
            this.itemContainer[0].appendChild(this.element.find("script[_for='item']")[0]);
            this.grid = new WayGridView(<any>this.itemContainer[0], 20);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);
            
          
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
                    this.grid.searchModel.submitObject = () => {
                        var result;
                        eval("result = {" + this.textMember + ":" + JSON.stringify(this.grid.searchModel.text) + "}");
                        return result;
                    }
                    this.grid.searchModel.onchange = () => {
                        if (this.itemContainer.css("visibility") == "visible") {
                            this.grid.databind();
                            this.isBindedGrid = true;
                        }
                        else {
                            this.isBindedGrid = false;
                        }
                    }
                }
            }
        }
    }

    addEventListener(eventName: string, func: any) {
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
    }

    fireEvent(eventName: string) {
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
    }

  
    getTextByValue(value: string): string {
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = (<any>this.grid.items[i]).data;
            if (data.value == value) {
                return data.text;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.valueMember + ":" + JSON.stringify(value) + "}");
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, (data, err) => {
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
    }
    getValueByText(text: string): string {
        for (var i = 0; i < this.grid.items.length; i++) {
            var data = (<any>this.grid.items[i]).data;
            if (data.text == text) {
                return data.value;
            }
        }
        //find in server
        var model;
        var result;
        eval("model={" + this.textMember + ":" + JSON.stringify("equal:"+text) + "}");
        this.grid.dbContext.getDataItem([this.valueMember, this.textMember], model, (data, err) => {
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
    }
    private _onGridItemCreated(item: JQuery): void {
        (<any>item).status.Selected = (<any>item).data.value == this.value;
        item.click(() => {
            this.hideList();
            (<any>item).status.Selected = true;
            for (var i = 0; i < this.grid.items.length; i++) {
                if (this.grid.items[i] != item) {
                    (<any>this.grid.items[i]).status.Selected = false;
                }
            }
            this.value = (<any>item).data.value;
        });
    }

    private setText(text: string): void {
        if (this.textElement[0].tagName == "INPUT") {
            if (this.textElement.val() != text)
                this.textElement.val(text);
        }
        else {
            if (this.textElement.html() != text)
                this.textElement.html(text);
        }
    }

    private init(): void {
        this.itemContainer.css(
            {
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
            this.itemContainer.css("position","fixed");
            this.maskLayer = $("<div style='background-color:#000000;opacity:0.3;z-index:998;position:fixed;width:100%;height:100%;display:none;left:0;top:0;'></div>");
            document.body.appendChild(this.maskLayer[0]);
            this.itemContainer.css("height", "300px");
        }

        document.body.appendChild(this.itemContainer[0]);

        this.actionElement.click((e) => {
            e = e || <any>window.event;
            if (e.stopPropagation)
                e.stopPropagation();
            else
                e.cancelBubble = true;
            this.showList();
        });

        this.textElement.click((e) => {
            e = e || <any>window.event;
            if (e.stopPropagation)
                e.stopPropagation();
            else
                e.cancelBubble = true;
        });

        if (this.textElement[0].tagName == "INPUT") {
            if (this.isMobile) {

            }
            else {
                this.textElement.keyup(() => {
                     //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                    this.grid.searchModel.text = this.textElement.val();
                    if (this.itemContainer.css("visibility") != "visible") {
                        //如果没有显示，则主动显示
                        this.showList();
                    }
                });
            }

            this.textElement.change(() => {
                //触发onchange事件，如果list已经visible,事件里会触发grid.databind()
                this.grid.searchModel.text = this.textElement.val();
                this.text = this.grid.searchModel.text;
            });
        }

        $(document.documentElement).click(() => {
            this.hideList();
        });
    }
    //显示下拉列表
    showList(): void {
        
        if (this.maskLayer) {
            this.maskLayer.show();
        } 
        if (!this.isMobile) {
            var offset = this.textElement.offset();
            var y = (offset.top + this.textElement.outerHeight());

            this.itemContainer.css(
                {
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
            this.itemContainer.css(
                {
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
    }

    private setSelectedItemScrollIntoView() {
        if (this.value ) {
            for (var i = 0; i < this.grid.items.length; i++) {
                if ((<any>this.grid.items[i]).status.Selected) {
                    this.grid.items[i][0].scrollIntoView(false);
                    break;
                }
            }
        }
    }

    //隐藏显示下拉列表
    hideList(): void {
        if (this.maskLayer)
            this.maskLayer.hide();
        if (this.itemContainer.css("visibility") == "visible")
        {
            this.itemContainer.css("visibility","hidden");
        }
    }
}


class WayCheckboxList {
    memberInChange: [ "value"];
    element: JQuery;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private windowObj: JQuery;

    valueMember: string;
    textMember: string;
    private _value: any[] = [];

    get value(): any[] {
        return this._value;
    }
    set value(v: any[]) {
        if (!v)
            v = [];
        if (v != this._value) {
            this._value = v;
            this.checkGridItem();
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string, datasource: any) {
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        (<any>this.element[0]).WayControl = this;
        this.isMobile = "ontouchstart" in this.element[0];
        
        var itemtemplate = this.element.find("script[_for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");

        if (true) {
            this.grid = new WayGridView(<any>this.element[0], 0);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);

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
            this.grid.onAfterCreateItems = () => {
                this.checkGridItem();
            }
            this.grid.databind();
        }
    }

    private checkGridItem() {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = (<any>this.grid.items[j]).status;
            var data = (<any>this.grid.items[j]).data;

            status.Selected = WayHelper.contains(this._value, data.value);

        }
    }

    addEventListener(eventName: string, func: any) {
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
    }

    fireEvent(eventName: string) {
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
    }

    private rasieModelChange() {
        
        for (var k = 0; k < WayDataBindHelper.bindings.length;k ++) {
            var binding = WayDataBindHelper.bindings[k];
            if (<any>binding.element === this.element[0]) {
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
    }

    private _onGridItemCreated(item: JQuery): void {
        item.click(() => {
            (<any>item).status.Selected = !(<any>item).status.Selected;
            if ((<any>item).status.Selected)
            {
                this._value.push((<any>item).data.value);
                this.fireEvent("change");
                //这里只是数值发生变化，如果有model和自己绑定，触发一下model的onchange事件
                this.rasieModelChange();
            }
            else {
                for (var i = 0; i < this._value.length; i++) {
                    if (this._value[i] == (<any>item).data.value) {
                        this._value.splice(i, 1);
                        this.fireEvent("change");
                        this.rasieModelChange();
                        break;
                    }
                }
            }
        });
    }

}

class WayRadioList {
    memberInChange: ["value"];
    element: JQuery;
    private isMobile: boolean = false;
    private grid: WayGridView;
    private windowObj: JQuery;

    valueMember: string;
    textMember: string;
    private _value: string;

    get value(): string {
        return this._value;
    }
    set value(v: string) {
        if (v != this._value) {
            this._value = v;
            this.checkGridItem();
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string, datasource: any) {
        this.windowObj = $(window);
        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        (<any>this.element[0]).WayControl = this;
        this.isMobile = "ontouchstart" in this.element[0];

        var itemtemplate = this.element.find("script[_for='item']")[0];
        this.valueMember = this.element[0].getAttribute("valueMember");
        this.textMember = this.element[0].getAttribute("textMember");

        if (true) {
            this.grid = new WayGridView(<any>this.element[0], 0);
            this.grid.datasource = datasource;
            this.grid.onCreateItem = (item) => this._onGridItemCreated(item);

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
            this.grid.onAfterCreateItems = () => {
                this.checkGridItem();
            }
            this.grid.databind();
        }
    }

    private checkGridItem() {
        for (var j = 0; j < this.grid.items.length; j++) {
            var status = (<any>this.grid.items[j]).status;
            var data = (<any>this.grid.items[j]).data;

            status.Selected = this._value == data.value;

        }
    }

    addEventListener(eventName: string, func: any) {
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
    }

    fireEvent(eventName: string) {
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
    }

    private rasieModelChange() {

        for (var k = 0; k < WayDataBindHelper.bindings.length; k++) {
            var binding = WayDataBindHelper.bindings[k];
            if (<any>binding.element === this.element[0]) {
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
    }

    private _onGridItemCreated(item: JQuery): void {
        item.click(() => {
            this.value = (<any>item).data.value;
        });
    }

}


class WayButton {
    memberInChange: ["text"];
    element: JQuery;

    private onclickString: string;
    private internalModel: any = {text:null};
    get text(): string {
        return this.internalModel.text;
    }
    set text(v: string) {
        if (v != this.internalModel.text) {
            this.internalModel.text = v;
            this.fireEvent("change");
        }
    }


    onchange: any = null;



    constructor(elementid: string) {

        if (typeof elementid == "string")
            this.element = $("#" + elementid);
        else if ((<any>elementid).tagName)
            this.element = $(elementid);
        else
            this.element = <any>elementid;

        var _databind_internal = this.element.attr("_databind_internal");
        var databind = this.element.attr("databind");
        var _expression_internal = this.element.attr("_expression_internal");
        var expression = this.element.attr("expression");

        this.element.attr("databind", _databind_internal);
        this.element.attr("expression", _expression_internal);

        this.internalModel = WayDataBindHelper.dataBind(<any>this.element[0], { text: this.element.attr("text") }, null, /(\w|\.)+( )?\=( )?\@(\w|\.)+/g, /\@(\w|\.)+/g , true);
        this.element.attr("databind", databind);
        this.element.attr("expression", expression);


        (<any>this.element[0]).WayControl = this;
        this.onclickString = this.element.attr("onclick");
        this.element.attr("onclick", null);

        if (this.onclickString && this.onclickString.length > 0) {

            var matches = this.onclickString.match(/[\W]?(this\.)/g);
            for (var i = 0; i < matches.length; i++) {
                var r = matches[i].replace("this.", "___element.");
                this.onclickString = this.onclickString.replace(matches[i], r);
            }

            this.element.click(() => {
                var ___element = this;
                eval(this.onclickString);
            });
            
        }
    }

    addEventListener(eventName: string, func: any) {
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
    }

    fireEvent(eventName: string) {
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
    }

}

var checkToInitWayControl = (parentElement: HTMLElement) => {
    if (parentElement.tagName == "SCRIPT" || parentElement.tagName == "STYLE")
        return;

    if (parentElement.tagName.indexOf("Way")) {
        initWayControl(parentElement, null);
    }

    for (var i = 0; i < parentElement.children.length; i++) {
        var ele = parentElement.children[i];
        if (ele.tagName.indexOf("Way")) {
            initWayControl(<HTMLElement>ele, null);
        }
        else {
            checkToInitWayControl(<HTMLElement>ele);
        }
    }
}

var initWayControl = (virtualEle: HTMLElement, element: HTMLElement) => {
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

    if (replaceEleObj.attr("databind")) {
        replaceEleObj.attr("_databind_internal", replaceEleObj.attr("databind"));
    }
    if (replaceEleObj.attr("expression")) {
        replaceEleObj.attr("_expression_internal", replaceEleObj.attr("expression"));
    }
    for (var k = 0; k < virtualEle.attributes.length; k++) {
        replaceEleObj.attr(virtualEle.attributes[k].name, virtualEle.attributes[k].value)
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
            control = new WayDropDownList(<any>replaceEleObj, replaceEleObj.attr("datasource"));
            break;
        case "WAYCHECKBOXLIST":
            control = new WayCheckboxList(<any>replaceEleObj, replaceEleObj.attr("datasource"));
            break;
        case "WAYRADIOLIST":
            control = new WayRadioList(<any>replaceEleObj, replaceEleObj.attr("datasource"));
            break;
        case "WAYBUTTON":
            control = new WayButton(<any>replaceEleObj);
            break;
        case "WAYGRIDVIEW":
            replaceEleObj[0].innerHTML += virtualEle.innerHTML;
            control = new WayGridView(<any>replaceEleObj, parseInt(replaceEleObj.attr("_pagesize")));
            break;
        default:
            break;
    }

    if (control) {
        var idstr = replaceEleObj.attr("id");
        if (idstr && idstr.length > 0 && eval("!window." + idstr + " || !window." + idstr + ".WayControl")) {
            eval("window." + idstr + "=control;");
        }
    }
};

var _styles = $(WayHelper.downloadUrl("/templates/main.html"));
$(document).ready(() => {
    var body = $(document.body);
    var controllerName = body.attr("controller");
    if (!controllerName) {
        throw "<Body>没有定义controller";
    }
    else {
        (<any>window).controller = WayScriptRemoting.createRemotingController(controllerName);
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


