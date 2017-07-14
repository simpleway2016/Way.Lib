interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): any;
}
declare class JElementHelper {
    static replaceElement(source: HTMLElement, dst: HTMLElement): void;
    static getElement(html: string): HTMLElement;
    static getJControlTypeName(tagName: string): false | "JButton";
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
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): void;
    onPropertyChanged(proName: string, originalValue: any): void;
}
declare class JControlDataBinder {
    control: JControl;
    dataContext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[];
    constructor(data: INotifyPropertyChanged, jcontrol: JControl, expression: RegExp);
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private onPropertyChanged(sender, name, originalValue);
    private onControlPropertyChanged(sender, name, originalValue);
    updateValue(): void;
}
declare class JChildrenElementBinder {
    element: HTMLElement;
    dataContext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[];
    constructor(data: INotifyPropertyChanged, element: HTMLElement, expression: RegExp, bindmyselft: boolean);
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private onPropertyChanged(sender, name, originalValue);
    updateValue(): void;
    init(): void;
}
declare class JControl implements INotifyPropertyChanged {
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): void;
    protected onPropertyChanged(proName: string, originalValue: any): void;
    element: HTMLElement;
    onPropertyChangeds: any[];
    databind: string;
    protected templateBinder: JChildrenElementBinder;
    protected dataBinder: JChildrenElementBinder;
    protected controlDataBinder: JControlDataBinder;
    private _dataContext;
    dataContext: any;
    private _onclick;
    onclick: any;
    constructor(element: HTMLElement);
    protected setChildrenDataContext(element: HTMLElement, dataContext: any): void;
    protected getTemplate(element: HTMLElement, forwhat: string): HTMLElement;
}
declare class JButton extends JControl {
    private _text;
    text: string;
    constructor(element: HTMLElement);
}
