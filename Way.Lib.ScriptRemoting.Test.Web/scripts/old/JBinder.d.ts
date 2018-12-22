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
declare class JBinder {
    protected bindingDataContext: any;
    control: any;
    disposed: boolean;
    rootControl: JControl;
    private _datacontext_listen_index;
    private _datacontext;
    readonly datacontext: any;
    constructor(control: any);
    static pushBinder(binder: any): void;
    onPropertyChanged(sender: any, name: string, originalValue: any): void;
    getDatacontext(): INotifyPropertyChanged;
    dispose(): void;
    static addPropertyIfNotExist(data: any, propertyName: any): void;
    static moveAttributeBindToDatabind(element: HTMLElement): void;
}
declare class JDatacontextBinder extends JBinder {
    configs: JBindConfig[];
    private controlListenIndex;
    constructor(control: any);
    bindChildren(element: HTMLElement): void;
    updateValue(): void;
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private listenElementEvent;
    onPropertyChanged(sender: any, name: string, originalValue: any): void;
    private onControlPropertyChanged;
    dispose(): void;
    protected getRegexp(): RegExp;
}
declare class JControlBinder extends JDatacontextBinder {
    constructor(control: any);
    getDatacontext(): INotifyPropertyChanged;
    protected getRegexp(): RegExp;
}
declare class JDatacontextExpressionBinder extends JBinder {
    configs: JBindExpression[];
    constructor(control: any);
    bindChildren(element: HTMLElement): void;
    updateValue(): void;
    private handleExpression;
    protected getConfigByDataProName(proname: string): JBindExpression;
    onPropertyChanged(sender: any, name: string, originalValue: any): void;
    dispose(): void;
    protected getRegexp(): RegExp;
}
declare class JControlExpressionBinder extends JDatacontextExpressionBinder {
    constructor(control: any);
    getDatacontext(): INotifyPropertyChanged;
    protected getRegexp(): RegExp;
}
