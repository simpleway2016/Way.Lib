interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
    removeListener(index: number): any;
}
declare class JElementHelper {
    static SystemTemplateContainer: HTMLElement;
    static replaceElement(source: HTMLElement, dst: HTMLElement): void;
    static getControlTypeName(tagname: string): string;
    static getElement(html: string): HTMLElement;
    static initElements(container: HTMLElement): void;
}
declare class JBindConfig {
    dataPropertyName: string;
    elementPropertyName: string;
    constructor(dataPropertyName: string, elementPropertyName: string);
}
declare class JBindExpression {
    dataPropertyName: string;
    expression: string;
    constructor(dataPropertyName: string, expression: string);
}
declare class JObserveObject implements INotifyPropertyChanged {
    __data: any;
    __parent: JObserveObject;
    __parentName: string;
    private __onchanges;
    constructor(data: any, parent?: JObserveObject, parentname?: string);
    hasProperty(proname: string): boolean;
    addProperty(proName: string): void;
    private getFunc(name);
    private setFunc(checkname);
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
    configs: JBindConfig[];
    expressionConfigs: JBindExpression[];
    children: JChildrenElementBinder[];
    private disposed;
    private propertyChangedListenerIndex;
    private propertyExpressionChangedListenerIndex;
    constructor(data: INotifyPropertyChanged, element: HTMLElement, databind_exp: RegExp, bindmyselft: boolean);
    static addPropertyIfNotExist(data: any, propertyName: any): void;
    dispose(): void;
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getExpressionConfigByDataProName(proname: string): JBindExpression;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private onExpressionPropertyChanged(sender, name, originalValue);
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
    protected templateMatchProNames: string[];
    protected currentTemplate: HTMLElement;
    protected templateBinder: JChildrenElementBinder;
    protected dataBinder: JChildrenElementBinder;
    protected controlDataBinder: JControlDataBinder;
    private _datacontext;
    datacontext: any;
    private _onclick;
    onclick: any;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    dispose(): void;
    addEventListener(type: string, listener: EventListenerOrEventListenerObject, useCapture?: boolean): void;
    removeEventListener(type: string, listener: EventListenerOrEventListenerObject): void;
    protected loadTemplates(): void;
    private checkDataContextPropertyExist();
    private reApplyTemplate(rootElement);
    protected onTemplateApply(): void;
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
    private addFuncs;
    private removeFuncs;
    constructor(data: JObserveObject[]);
    addEventListener(_type: string, listener: (sender, data, index: number) => any): void;
    removeEventListener(_type: string, listener: (sender, data, index: number) => any): void;
    add(data: JObserveObject): void;
    insert(index: number, data: JObserveObject): void;
    remove(data: JObserveObject): void;
    removeAt(index: number): void;
}
declare class JListItem extends JControl {
    private _index;
    index: number;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class JList extends JControl {
    itemContainer: HTMLElement;
    private itemControls;
    private itemTemplates;
    private _itemsource;
    itemsource: JDataSource;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    protected loadTemplates(): void;
    protected onTemplateApply(): void;
    protected bindItems(): void;
    private resetItemIndex();
    private addItem(data);
}
declare class JCheckboxList extends JList {
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
