
window.onerror = (msg) => {
    alert(msg);
}

enum WayScriptRemotingMessageType {
    Result = 1,
    Notify = 2,
    SendSessionID = 3,
    InvokeError = 4,
}

class WayCookie {
    static setCookie(name: string, value: string): void {
        document.cookie = name + "=" + (<any>window).escape(value);

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
                            return (<any>window).unescape(v); //返回需要提取的cookie值
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

class WayScriptRemoting extends WayBaseObject {
    static onBeforeInvoke: (name: string, parameters: any[]) => any;
    static onInvokeFinish: (name: string, parameters: any[]) => any;

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
        WayScriptRemoting.ExistControllers.push(page);
        WayCookie.setCookie("WayScriptRemoting", result.SessionID)
        return page;
    }

    private static createRemotingControllerAsync(remoteName: string, callback: (obj: WayScriptRemoting, err: string) => void): void {
        WayScriptRemoting.getServerAddress();
        var ws = new WebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
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
            var ws = new WebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
            var initType = ws.binaryType;
            ws.onopen = () => {
                ws.send("{'Action':'UploadFile','FileName':'" + file.name + "','FileSize':" + size + ",'Offset':" + handler.offset + ",'ClassFullName':'" + this.classFullName + "','SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}");
                ws.binaryType = "arraybuffer";
                this.sendFile(ws, file, reader, size, handler.offset, 102400, callback, handler);
            };
            ws.onmessage = (evt) => {
                var resultObj;
                eval("resultObj=" + evt.data);

                if (resultObj.type == WayScriptRemotingMessageType.Result) {
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

    pageInvoke(name: string, parameters: any[], callback: any) {
        try {
            if (WayScriptRemoting.onBeforeInvoke) {
                WayScriptRemoting.onBeforeInvoke(name, parameters);
            }

            var paramerStr = "";
            if (parameters) {
                parameters.forEach((p) => {
                    if (paramerStr.length > 0)
                        paramerStr += ",";
                    var itemstr = JSON.stringify(p);
                    paramerStr += JSON.stringify(itemstr);
                });
            }

            var invoker = new WayScriptInvoker("http://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_invoke?a=1");
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
                }
            };
            invoker.invoke(["m", "{'ClassFullName':'" + this.classFullName + "','MethodName':'" + name + "','Parameters':[" + paramerStr + "] , 'SessionID':'" + WayCookie.getCookie("WayScriptRemoting") + "'}"]);
            
        }
        catch (e) {
            callback(null, e.message);
        }

    }

    private connect(): void {
        this.socket = new WebSocket("ws://" + WayScriptRemoting.ServerAddress + "/wayscriptremoting_socket");
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

class WayScriptInvoker {
    url: string;
    async: boolean = true;
    onBeforeInvoke: () => any;
    onInvokeFinish: () => any;
    onCompleted: (result: any, err: any) => any;
    method: string = "POST";
    private xmlHttp: any;

    constructor(_url: string) {
        if (_url) {
            this.url = _url;
        }
        else {
            this.url = window.location.href;
        }

    }

    invoke(nameAndValues: string[]): void {
        if (!this.xmlHttp) {
            this.xmlHttp = this.createXMLHttp();
        }
        var p: string = "";
        for (var i = 0; i < nameAndValues.length; i += 2) {
            if (i > 0)
                p += "&";
            p += nameAndValues[i] + "=" + (<any>window).escape(nameAndValues[i + 1]);

        }
        if (this.onBeforeInvoke)
            this.onBeforeInvoke();

        this.xmlHttp.onreadystatechange = () => this.xmlHttpStatusChanged();
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
            else {
                if (this.onCompleted) {
                    this.onCompleted(null, "无法连接服务器");
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

    //触发htmlElement相关事件，如：fireEvent(myDiv , "click");
    static fireEvent(el: HTMLElement, eventName:string):void {
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
    container: JQuery;
    configs: WayBindMemberConfig[] = [];


    constructor(_element: HTMLElement, _model: any, _dataSource: any, expressionExp: RegExp, dataMemberExp: RegExp) {
        super();
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

    private initEle(ctrlEle: HTMLElement, _dataSource: any, expressionExp: RegExp, dataMemberExp: RegExp) {
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
                        (<any>ctrlEle)._data = this.model;

                        if (_dataSource) {
                            eval("ctrlEle." + eleMember + "=_dataSource." + dataMember + ";");

                            if (eleMember == "value" || eleMember == "checked") {
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
    }


    private onvalueChanged(fromWhichConfig: WayBindMemberConfig): void {

        try {
            if (this.configs.length == 0)
                return;//绑定已经移除了

            if (fromWhichConfig.elementMember == "value") {
                var model = this.model;
                eval("model." + fromWhichConfig.dataMember + "=" + JSON.stringify((<any>fromWhichConfig.element).value) + ";");
            }
            else if (fromWhichConfig.elementMember == "checked") {
                var model = this.model;
                eval("model." + fromWhichConfig.dataMember + "=" + JSON.stringify((<any>fromWhichConfig.element).checked) + ";");
            }
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
                    eval("config.element." + config.elementMember + "=" + JSON.stringify(value) + ";");
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

    static cloneObjectForBind(obj: any, _itemIndex: any, onchangeMembers: string[], _onchange: any): any {
        if (obj.getSource && typeof obj.getSource == "function") {
            obj = obj.getSource();
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
        if (toCheckedEles) {
            for (var i = 0; i < WayDataBindHelper.bindings.length; i++) {
                var binding = WayDataBindHelper.bindings[i];
                if (binding && binding.model == model) {
                    binding.onchange(itemIndex, name, value);
                    break;
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
    static replaceHtmlFields(templateHtml, data):string
	{
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

    static dataBind(element: HTMLElement, data: any, tag: any = null,
        expressionExp: RegExp = /(\w|\.)+( )?\=( )?\@(\w|\.)+/g,
        dataMemberExp: RegExp = /\@(\w|\.)+/g): any {
        if (typeof element == "string") {
            element = document.getElementById(<any>element);
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
}
interface IPageable {
    shouldLoadMorePage(): void;
    hasMorePage: boolean;
}
class WayPager {
    scrollable: JQuery;
    control: IPageable;
    constructor(_scrollable: JQuery, _ctrl: IPageable) {
        this.scrollable = _scrollable;
        this.control = _ctrl;

        _scrollable.scroll(() => { this.onscroll(); });
    }

    private onscroll(): void {
        if (!this.control.hasMorePage)
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
            "transform": "scale(0.5)"
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


    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void): void {
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
        });
    }

    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void): void {
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
        });
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

    private dbContext: WayDBContext;
    private pageinfo: WayPageInfo = new WayPageInfo();
    private pager: WayPager;
    private fieldExp: RegExp = /\{\@(\w|\.|\:)+\}/g;
    private loading: WayProgressBar = new WayProgressBar("#cccccc");
    private footerItem: JQuery;
    // 标识当前绑定数据的事物id
    private transcationID: number = 1;
    private primaryKey: string;
    hasMorePage: boolean;

    //定义item._status的数据原型，可以修改此原型达到期望的目的
    itemStatusModel: any = { Selected: false };

    //header模板
    header: WayTemplate;
    //footer模板
    footer: WayTemplate;
    //搜索条件model
    searchModel: any;
    //数据源
    _datasource: any;
    get datasource(): any {
        return this._datasource;
    }
    set datasource(_v: any) {
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

    constructor(elementId: string, controller: string, _pagesize: number = 10) {
        super();
        try {
            this.dbContext = new WayDBContext(controller, null)
            this.element = $("#" + elementId);
            this.pager = new WayPager(this.element, this);
            this.pageinfo.PageSize = _pagesize;
            var bodyTemplate = this.element.find("script[_for='body']");
            var templates = this.element.find("script");

            this.itemContainer = this.element;
            if (bodyTemplate.length > 0) {
                this.itemContainer = $(bodyTemplate[0].innerHTML);
                this.element[0].appendChild(this.itemContainer[0]);
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

    search(): void {
        this.databind();
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
        var item = this.items[itemIndex];
        var model = (<any>item)._data;
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
        return result;
    }

    //绑定数据
    databind(): void {

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

        this.transcationID++;
        var mytranId = this.transcationID;
        if (typeof this.datasource == "function") {
            this.showLoading();
            this.datasource((ret, err) => {
                this.hideLoading();
                if (mytranId != this.transcationID)
                    return;

                if (err) {
                    this.onErr(err);
                }
                else {
                    this.binddatas(ret);
                }
            });
        }
        else if (typeof this.datasource == "string") {
            this.showLoading();
            this.dbContext.getDatas(this.pageinfo, this.getBindFields(), this.searchModel, (ret, pkid, err) => {
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

                    this.binddatas(ret);
                    this.pageinfo.PageIndex++;
                    this.hasMorePage = ret.length >= this.pageinfo.PageSize;

                    if (this.onAfterCreateItems) {
                        try {
                            this.onAfterCreateItems(this.items.length, this.hasMorePage);
                        }
                        catch (e) {
                        }
                    }

                    if (this.hasMorePage && this.element[0].scrollHeight <= this.element.height() * 1.1) {
                        this.shouldLoadMorePage();
                    }
                }
            });

        }
        else {
            this.binddatas(this.datasource);
        }
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
        var mydata = (<any>item)._data.getSource();
        this.originalItems[itemIndex] = WayHelper.clone(mydata);
    }

    //从服务器更新指定item的数据，并重新绑定
    rebindItemFromServer(itemIndex: number, mode: string, callback: (data: any, err: any) => void) {
        var searchmodel = {};
        var item = this.items[itemIndex];
        searchmodel[this.primaryKey] = (<any>item)._data[this.primaryKey];
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

    private createItem(itemIndex: any, mode: string = ""): JQuery {
        //把数据克隆一份
        var currentItemStatus;
        if (itemIndex < this.items.length) {
            currentItemStatus = (<any>this.items[itemIndex])._status;
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

        (<any>item)._status = WayDataBindHelper.dataBind(item[0], statusData, itemIndex, /(\w|\.)+( )?\=( )?\$(\w|\.)+/g, /\$(\w|\.)+/g);
        if (typeof myChangeFunc == "function") {
            (<any>item)._status.onchange = myChangeFunc;
        }

        //建立验证
        if (true) {
            var bindItemElements = item.find("*[_databind]");

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
        (<any>item)._data = model;
        (<any>item)._mode = mode;

        if (this.onCreateItem) {
            this.onCreateItem(item, mode);
        }

        return item;
    }

    private binddatas(datas: any[]): void {
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
}


