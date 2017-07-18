interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
    removeListener(index: number): any;
}
declare class JElementHelper {
    static replaceElement(source: HTMLElement, dst: HTMLElement): void;
    static getElement(html: string): HTMLElement;
    static getJControlTypeName(tagName: string): false | "JButton" | "JList";
    static initElements(container: HTMLElement): void;
}
declare class JBindConfig {
    dataPropertyName: string;
    elementPropertyName: string;
    constructor(dataPropertyName: string, elementPropertyName: string);
}
declare class JObserveObject implements INotifyPropertyChanged {
    __data: any;
    __parent: JObserveObject;
    __parentName: string;
    private __onchanges;
    constructor(data: any, parent?: JObserveObject, parentname?: string);
    private __addProperty(proName);
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    onPropertyChanged(proName: string, originalValue: any): void;
}
declare class JControlDataBinder {
    control: JControl;
    datacontext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[];
    private propertyChangedListenerIndex;
    constructor(data: INotifyPropertyChanged, jcontrol: JControl, expression: RegExp);
    dispose(): void;
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private onPropertyChanged(sender, name, originalValue);
    private onControlPropertyChanged(sender, name, originalValue);
    updateValue(): void;
}
declare class JChildrenElementBinder {
    element: HTMLElement;
    datacontext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[];
    children: JChildrenElementBinder[];
    private disposed;
    private propertyChangedListenerIndex;
    constructor(data: INotifyPropertyChanged, element: HTMLElement, expression: RegExp, bindmyselft: boolean);
    dispose(): void;
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private onPropertyChanged(sender, name, originalValue);
    updateValue(): void;
    init(): void;
}
declare class JControl implements INotifyPropertyChanged {
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    protected onPropertyChanged(proName: string, originalValue: any): void;
    originalElement: HTMLElement;
    element: HTMLElement;
    onPropertyChangeds: any[];
    databind: string;
    protected templates: HTMLElement[];
    protected templateMatchProNames: any[];
    protected currentTemplate: HTMLElement;
    protected templateBinder: JChildrenElementBinder;
    protected dataBinder: JChildrenElementBinder;
    protected controlDataBinder: JControlDataBinder;
    private _datacontext;
    datacontext: any;
    private _onclick;
    onclick: any;
    constructor(element: HTMLElement);
    addEventListener(type: string, listener: EventListenerOrEventListenerObject, useCapture?: boolean): void;
    removeEventListener(type: string, listener: EventListenerOrEventListenerObject): void;
    protected loadTemplates(): void;
    protected reApplyTemplate(rootElement: HTMLElement): void;
    protected setChildrenDataContext(element: HTMLElement, datacontext: any): void;
    protected getTemplate(): HTMLElement;
}
declare class JButton extends JControl {
    private _text;
    text: string;
    constructor(element: HTMLElement);
}
declare class JDataSource {
    source: JObserveObject[];
    private onAddFuncs;
    private onRemoveFuncs;
    constructor(data: JObserveObject[]);
    addEventListener(type: string, listener: (sender, data, index: number) => any): void;
    removeEventListener(type: string, listener: (sender, data, index: number) => any): void;
    add(data: JObserveObject): void;
    insert(index: number, data: JObserveObject): void;
    remove(data: JObserveObject): void;
    removeAt(index: number): void;
}
declare class JList extends JControl {
    private _itemsource;
    itemsource: JDataSource;
    constructor(element: HTMLElement);
    protected loadTemplates(): void;
}
