declare var setMaxDigits: (n: number) => void;
declare class RSAKeyPair {
    constructor(e: string, n: string, m: string);
}
declare var encryptedString: (key: RSAKeyPair, value: string) => string;
declare var decryptedString: (key: RSAKeyPair, value: string) => string;
declare var RSAMAXLENGTH: number;
declare enum WayScriptRemotingMessageType {
    Result = 1,
    Notify = 2,
    SendSessionID = 3,
    InvokeError = 4,
    UploadFileBegined = 5,
    RSADecrptError = 6
}
declare var WEBSOCKET_Protocol: string;
declare class WayCookie {
    static setCookie(name: string, value: string): void;
    static getCookie(name: string): string;
}
declare class WayScriptRemotingUploadHandler {
    abort: boolean;
    offset: number;
}
declare class RSAInfo {
    Exponent: string;
    Modulus: string;
}
declare class WayScriptRemoting {
    static onBeforeInvoke: (name: string, parameters: any[]) => any;
    static onInvokeFinish: (name: string, parameters: any[]) => any;
    rsa: RSAInfo;
    classFullName: string;
    private _groupName;
    groupName: string;
    onerror: (err: any) => any;
    onconnect: () => any;
    private mDoConnected;
    private _onmessage;
    onmessage: (msg: any) => any;
    private socket;
    private socket_heart_timer;
    static ServerAddress: string;
    static ExistControllers: WayScriptRemoting[];
    constructor(remoteName: string);
    static getServerAddress(): void;
    static createRemotingController(remoteName: string): WayScriptRemoting;
    static getClassDefineScript(methods: any[]): string;
    private static createRemotingControllerAsync;
    private reCreateRSA;
    private _uploadFileWithHTTP;
    private arrayBufferToString;
    private sendFileWithHttp;
    private sendFile;
    uploadFile(fileElement: any, state: any, callback: (ret: any, totalSize: any, uploaded: any, err: any) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler;
    str2UTF8(str: any): any[];
    private encrypt;
    pageInvoke(name: string, parameters: any[], callback: (result: any, err: any, statusCode: any) => void, async?: boolean, useRsa?: boolean, returnUseRsa?: boolean): void;
    sendMessage(msg: string): void;
    private sendHeart;
    private connect;
}
declare class WayScriptRemotingChild extends WayScriptRemoting {
}
declare enum WayVirtualWebSocketStatus {
    none = 0,
    connected = 1,
    error = 2,
    closed = 3
}
declare class WayVirtualWebSocket {
    private guid;
    private url;
    private status;
    private errMsg;
    private lastMessage;
    private receiver;
    private _onopen;
    private _onmessage;
    private _onclose;
    private _onerror;
    private sendQueue;
    binaryType: string;
    onopen: (event: any) => void;
    onmessage: (event: any) => void;
    onclose: (event: any) => void;
    onerror: (event: any) => void;
    constructor(_url: string);
    close(): void;
    private init;
    send(data: any): void;
    private arrayBufferToString;
    private receiveChannelConnect;
    private sendHeart;
}
declare class WayScriptInvoker {
    url: string;
    async: boolean;
    onBeforeInvoke: () => any;
    onInvokeFinish: () => any;
    onCompleted: (result: any, err: any, statusCode: any) => any;
    private xmlHttp;
    constructor(_url: string);
    abort(): void;
    setTimeout(millseconds: number): void;
    Post(obj: any): void;
    Get(nameAndValues?: string[]): void;
    private xmlHttpStatusChanged;
    private createXMLHttp;
}
declare class WayHelper {
    static createWebSocket(url: string): WebSocket;
    static writePage(url: string): void;
    static downloadUrl(url: string): string;
    static addEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void;
    static removeEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void;
    static fireEvent(el: HTMLElement, eventName: string): void;
    static copyValue(target: any, source: any): void;
    static clone(obj: any): any;
}
