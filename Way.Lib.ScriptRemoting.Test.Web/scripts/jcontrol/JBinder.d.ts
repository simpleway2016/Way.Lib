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
    datacontext: INotifyPropertyChanged;
    control: any;
    disposed: boolean;
    constructor(data: INotifyPropertyChanged, control: any);
    dispose(): void;
    static addPropertyIfNotExist(data: any, propertyName: any): void;
    static moveAttributeBindToDatabind(element: HTMLElement): void;
}
declare class JDatacontextBinder extends JBinder {
    configs: JBindConfig[];
    protected propertyChangedListenerIndex: number;
    protected controlPropertyChangedListenerIndex: number;
    constructor(data: INotifyPropertyChanged, control: any);
    updateValue(): void;
    protected bindChildren(): void;
    protected getConfigByDataProName(proname: string): JBindConfig;
    protected getConfigByElementProName(proname: string): JBindConfig;
    private listenElementEvent(self, config);
    private onPropertyChanged(sender, name, originalValue);
    private onControlPropertyChanged(sender, name, originalValue);
    dispose(): void;
    protected getRegexp(): RegExp;
}
declare class JControlBinder extends JDatacontextBinder {
    constructor(data: INotifyPropertyChanged, control: any);
    protected bindChildren(): void;
    protected getRegexp(): RegExp;
}
declare class JDatacontextExpressionBinder extends JBinder {
    configs: JBindExpression[];
    protected propertyChangedListenerIndex: number;
    constructor(data: INotifyPropertyChanged, control: any);
    updateValue(): void;
    private handleExpression(expressionStr);
    protected bindChildren(): void;
    protected getConfigByDataProName(proname: string): JBindExpression;
    private onPropertyChanged(sender, name, originalValue);
    dispose(): void;
    protected getRegexp(): RegExp;
}
declare class JControlExpressionBinder extends JDatacontextExpressionBinder {
    constructor(data: INotifyPropertyChanged, control: any);
    protected getRegexp(): RegExp;
}
