declare var setMaxDigits: (n: number) => void;
declare class RSAKeyPair {
    constructor(e: string, n: string, m: string);
}
declare var encryptedString: (key: RSAKeyPair, value: string) => string;
declare var decryptedString: (key: RSAKeyPair, value: string) => string;
declare enum WayScriptRemotingMessageType {
    Result = 1,
    Notify = 2,
    SendSessionID = 3,
    InvokeError = 4,
    UploadFileBegined = 5,
    RSADecrptError = 6,
}
declare class WayCookie {
    static setCookie(name: string, value: string): void;
    static getCookie(name: string): string;
}
declare class WayBaseObject {
}
declare class WayScriptRemotingUploadHandler {
    abort: boolean;
    offset: number;
}
declare class RSAInfo {
    Exponent: string;
    Modulus: string;
}
declare class WayScriptRemoting extends WayBaseObject {
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
    static ServerAddress: string;
    static ExistControllers: WayScriptRemoting[];
    constructor(remoteName: string);
    static getServerAddress(): void;
    static createRemotingController(remoteName: string): WayScriptRemoting;
    static getClassDefineScript(methods: any[]): string;
    private static createRemotingControllerAsync(remoteName, callback);
    private _uploadFileWithHTTP(fileElement, state, callback, handler);
    private arrayBufferToString(data);
    private sendFileWithHttp(tranid, state, file, reader, size, start, len, callback, handler);
    private sendFile(ws, file, reader, size, start, len, callback, handler);
    uploadFile(fileElement: any, state: any, callback: (ret, totalSize, uploaded, err) => any, handler: WayScriptRemotingUploadHandler): WayScriptRemotingUploadHandler;
    private encrypt(value);
    pageInvoke(name: string, parameters: any[], callback: any, async?: boolean, useRsa?: boolean, returnUseRsa?: boolean): void;
    private connect();
}
declare class WayScriptRemotingChild extends WayScriptRemoting {
}
declare enum WayVirtualWebSocketStatus {
    none = 0,
    connected = 1,
    error = 2,
    closed = 3,
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
    private init();
    send(data: any): void;
    private arrayBufferToString(data);
    private receiveChannelConnect();
    private sendHeart();
}
declare class WayScriptInvoker {
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
declare class WayTemplate {
    content: string;
    match: string;
    mode: string;
    constructor(_content: string, _match?: string, mode?: string);
}
declare class WayHelper {
    static contains(arr: any, value: any): boolean;
    static getPropertyName(obj: any, index: number): string;
    static createWebSocket(url: string): WebSocket;
    static setTouchFireClickEvent(element: any, handler: any): void;
    static writePage(url: string): void;
    static downloadUrl(url: string): string;
    static findBindingElements(element: HTMLElement): any[];
    static findInnerBindingElements(result: any[], element: HTMLElement): void;
    static addEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void;
    static removeEventListener(element: HTMLElement, eventName: string, listener: any, useCapture: any): void;
    static fireEvent(el: HTMLElement, eventName: string): void;
    static getDataForDiffent(originalData: any, currentData: any): any;
    static replace(content: string, find: string, replace: string): string;
    static copyValue(target: any, source: any): void;
    static clone(obj: any): any;
}
declare class WayBindMemberConfig {
    elementMember: string;
    dataMember: string;
    element: HTMLElement;
    expressionString: string;
    dataMemberExp: any;
    constructor(_elementMember: string, _dataMember: string, _element: HTMLElement);
}
declare class WayObserveObject {
    __data: any;
    __parent: WayObserveObject;
    __parentName: string;
    private __onchanges;
    private __objects;
    constructor(data: any, parent?: WayObserveObject, parentname?: string);
    addNewProperty(proName: any, value: any): void;
    private __addProperty(proName);
    addEventListener(name: string, func: (model, name, value) => void): void;
    removeEventListener(name: string, func: any): void;
    __changed(name: string, value: any): void;
}
declare class WayBindingElement extends WayBaseObject {
    element: HTMLElement;
    model: WayObserveObject;
    configs: WayBindMemberConfig[];
    expressionConfigs: WayBindMemberConfig[];
    constructor(_element: HTMLElement, _model: any, expressionExp: RegExp, dataMemberExp: RegExp);
    private initEle(ctrlEle, expressionExp, dataMemberExp);
    doExpression(__config: WayBindMemberConfig): void;
    initEleValues(model: any): void;
    private onvalueChanged(fromWhichConfig);
    getDataMembers(): string[];
    onchange(itemIndex: any, name: any, value: any): void;
}
declare class WayDataBindHelper {
    static bindings: WayBindingElement[];
    static removeDataBind(element: HTMLElement): void;
    static getBindingFields(element: HTMLElement, expressionExp?: RegExp, dataMemberExp?: RegExp): string[];
    static replaceHtmlFields(templateHtml: any, data: any): string;
    static dataBind(element: any, data: any, tag?: any, expressionExp?: RegExp, dataMemberExp?: RegExp, doexpression?: boolean): any;
}
declare class WayControlHelper {
    static getValue(ctrl: HTMLElement): string;
    static setValue(ctrl: HTMLElement, value: any): void;
}
declare class WayPageInfo {
    PageIndex: number;
    PageSize: number;
    ViewingPageIndex: number;
}
interface IPageable {
    shouldLoadMorePage(pageindex: number): void;
    hasMorePage: boolean;
    pageMode: boolean;
}
declare class WayPager {
    scrollable: JQuery;
    control: IPageable;
    private scrollListener;
    constructor(_scrollable: JQuery, _ctrl: IPageable);
    private onscroll();
}
declare class WayProgressBar {
    private loading;
    color: string;
    private showRef;
    private lastMouseDownLocation;
    private lastMouseDownTime;
    private timingNumber;
    constructor(_color?: string);
    private mousedown(e);
    private initLoading();
    show(centerElement: JQuery): void;
    hide(): void;
}
declare class WayPopup {
    template: string;
    container: JQuery;
    show(content: string, element: JQuery, direction: string): void;
    hide(): void;
}
declare class WayDataSource {
    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async?: boolean): void;
    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async?: boolean): void;
    count(searchModel: any, callback: (data: any, err: any) => void): void;
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void;
    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void;
}
declare class WayArrayDataSource extends WayDataSource {
    private _data;
    constructor(data: any[]);
    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async?: boolean): void;
    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async?: boolean): void;
    count(searchModel: any, callback: (data: any, err: any) => void): void;
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void;
    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void;
}
declare class WayDBContext extends WayDataSource {
    private remoting;
    private datasource;
    constructor(controller: string, _datasource: string);
    getDatas(pageinfo: WayPageInfo, bindFields: any, searchModel: any, callback: (_data: any, _pkid: any, err: any) => void, async?: boolean): void;
    getDataItem(bindFields: any, searchModel: any, callback: (data: any, err: any) => void, async?: boolean): void;
    saveData(data: any, primaryKey: string, callback: (data: any, err: any) => void): void;
    count(searchModel: any, callback: (data: any, err: any) => void): void;
    sum(fields: string[], searchModel: any, callback: (data: any, err: any) => void): void;
}
declare class WayControlBase extends WayBaseObject {
}
declare class WayGridView extends WayControlBase implements IPageable {
    memberInChange: any[];
    element: JQuery;
    private itemContainer;
    private itemTemplates;
    items: JQuery[];
    private originalItems;
    private bodyTemplateHtml;
    dbContext: WayDataSource;
    private pageinfo;
    pagesize: number;
    private pager;
    private fieldExp;
    private loading;
    private footerItem;
    private transcationID;
    private primaryKey;
    hasMorePage: boolean;
    dataMembers: string[];
    supportDropdownRefresh: boolean;
    itemStatusModel: any;
    pageMode: boolean;
    private preloadedMaxPageIndex;
    preLoadNumForPageMode: number;
    onViewPageIndexChange: (index: number) => void;
    header: WayTemplate;
    footer: WayTemplate;
    searchModel: any;
    allowEdit: boolean;
    _datasource: any;
    datasource: any;
    onerror: (err: any) => void;
    onDatabind: () => void;
    onCreateItem: (item: JQuery, model: string) => void;
    onAfterCreateItems: (total: number, hasMore: boolean) => void;
    onItemSizeChanged: () => void;
    constructor(elementId: string, configElement: HTMLElement);
    private initRefreshEvent(touchEle);
    showLoading(centerElement: JQuery): void;
    hideLoading(): void;
    getTemplateOuterHtml(element: any): string;
    addItemTemplate(temp: WayTemplate): void;
    removeItemTemplate(temp: WayTemplate): void;
    private replace(content, find, replace);
    count(callback: (data: any, err: any) => void): void;
    sum(fields: string[], callback: (data: any, err: any) => void): void;
    save(itemIndex: number, callback: (idvalue: any, err: any) => void): void;
    private onErr(err);
    private contains(arr, find);
    private getBindFields();
    databind(): void;
    shouldLoadMorePage(pageindex: number): void;
    private bindDataToGrid(pageData, pageindex);
    private getDataByPagesize(datas, pageinfo);
    setSameWidthForTables(tableSource: JQuery, tableHeader: JQuery): void;
    private findItemTemplate(data, mode?);
    changeMode(itemIndex: number, mode: string): JQuery;
    acceptItemChanged(itemIndex: number): void;
    rebindItemFromServer(itemIndex: number, mode: string, callback?: (data: any, err: any) => void): void;
    private replaceFromString(str, itemIndex, statusmodel, data);
    private replaceVariable(container, itemIndex, statusmodel, data);
    private createItem(itemIndex, mode?);
    addItem(data: any): JQuery;
    private binddatas(datas, pageindex);
    private initedPageMode;
    private initForPageMode();
    private preLoadPage();
    setViewPageIndex(index: number): void;
    private binddatas_pageMode(datas, pageindex);
}
declare class WayDropDownList extends WayControlBase {
    memberInChange: any[];
    textElement: JQuery;
    actionElement: JQuery;
    element: JQuery;
    itemContainer: JQuery;
    selectonly: boolean;
    datasource: any;
    private isMobile;
    private grid;
    private isBindedGrid;
    private windowObj;
    private maskLayer;
    valueMember: string;
    textMember: string;
    private _value;
    value: string;
    private _text;
    text: string;
    onchange: any;
    constructor(elementid: string, configElement: HTMLElement);
    databind(): void;
    addEventListener(eventName: string, func: any): void;
    fireEvent(eventName: string): void;
    getTextByValue(value: string): string;
    getValueByText(text: string): string;
    private _onGridItemCreated(item);
    private setText(text);
    private init();
    showList(): void;
    private setSelectedItemScrollIntoView();
    hideList(): void;
}
declare class WayCheckboxList extends WayControlBase {
    memberInChange: any[];
    element: JQuery;
    datasource: any;
    private isMobile;
    private grid;
    private windowObj;
    valueMember: string;
    textMember: string;
    private _value;
    value: any[];
    onchange: any;
    constructor(elementid: string);
    databind(): void;
    private checkGridItem();
    addEventListener(eventName: string, func: any): void;
    fireEvent(eventName: string): void;
    private rasieModelChange();
    private _onGridItemCreated(item);
}
declare class WayRadioList extends WayControlBase {
    memberInChange: any[];
    element: JQuery;
    datasource: any;
    private isMobile;
    private grid;
    private windowObj;
    valueMember: string;
    textMember: string;
    private _value;
    value: string;
    onchange: any;
    constructor(elementid: string);
    databind(): void;
    private checkGridItem();
    addEventListener(eventName: string, func: any): void;
    fireEvent(eventName: string): void;
    private rasieModelChange();
    private _onGridItemCreated(item);
}
declare class WayRelateListDatasource {
    datasource: string;
    relateMember: string;
    textMember: string;
    valueMember: string;
    loop: boolean;
}
declare class WayRelateList extends WayControlBase {
    memberInChange: any[];
    element: JQuery;
    textElement: JQuery;
    onchange: any;
    private maskLayer;
    private isMobile;
    private windowObj;
    private configs;
    private listContainer;
    private _text;
    text: string;
    private _value;
    value: string[];
    addEventListener(eventName: string, func: any): void;
    fireEvent(eventName: string): void;
    constructor(elementid: string, virtualEle: HTMLElement);
    private init();
    showList(): void;
    hideList(): void;
    private checkWidth();
    private loadList();
    private loadConfigList(config, configIndex, searchModel);
    getTextByValue(config: WayRelateListDatasource, grid: WayGridView, value: string): string;
    private showCurrentText();
}
declare class WayButton extends WayControlBase {
    memberInChange: any[];
    element: JQuery;
    private onclickString;
    private internalModel;
    text: string;
    onchange: any;
    constructor(elementid: string);
    addEventListener(eventName: string, func: any): void;
    fireEvent(eventName: string): void;
}
declare var checkToInitWayControl: (parentElement: HTMLElement) => void;
declare var initWayControl: (virtualEle: HTMLElement, element?: HTMLElement) => void;
declare var _allWayControlNames: any[];
declare var _styles: JQuery;
declare var _bodyObj: JQuery;
declare var _windowObj: JQuery;
