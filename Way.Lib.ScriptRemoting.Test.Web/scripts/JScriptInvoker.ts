class JScriptInvoker {
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

class JHttpHelper {
    static writePage(url: string): void {
        document.write(JHttpHelper.downloadUrl(url));
    }

    static downloadUrl(url: string): string {
        var invoker = new JScriptInvoker(url);
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

}