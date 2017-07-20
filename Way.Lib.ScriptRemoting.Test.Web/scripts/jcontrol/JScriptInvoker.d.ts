declare class JScriptInvoker {
    url: string;
    async: boolean;
    onBeforeInvoke: () => any;
    onInvokeFinish: () => any;
    onCompleted: (result: any, err: any) => any;
    private xmlHttp;
    constructor(_url: string);
    abort(): void;
    setTimeout(millseconds: number): void;
    Post(obj: any): void;
    Get(nameAndValues?: string[]): void;
    private xmlHttpStatusChanged();
    private createXMLHttp();
}
declare class JHttpHelper {
    static writePage(url: string): void;
    static downloadUrl(url: string): string;
}
