declare class JControl implements INotifyPropertyChanged {
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    protected onPropertyChanged(proName: string, originalValue: any): void;
    protected onDatacontextPropertyChanged(datacontext: any, proName: string, originalValue: any): void;
    originalElement: HTMLElement;
    element: HTMLElement;
    onPropertyChangeds: any[];
    databind: string;
    expression: string;
    protected templates: HTMLElement[];
    protected templateMatchProNames: string[];
    protected currentTemplate: HTMLElement;
    private _datacontext_listen_index;
    private _datacontext;
    datacontext: any;
    private _parentJControl;
    parentJControl: JControl;
    private _id;
    id: string;
    private _onclick;
    onclick: any;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    private getFunc(name);
    private setFunc(attName);
    removeFromParent(): void;
    dispose(): void;
    addEventListener(type: string, listener: EventListenerOrEventListenerObject, useCapture?: boolean): void;
    removeEventListener(type: string, listener: EventListenerOrEventListenerObject): void;
    protected loadTemplates(): void;
    private checkDataContextPropertyExist();
    private reApplyTemplate(rootElement);
    resetParentJControl(): void;
    protected onTemplateApply(): void;
    protected getTemplate(): HTMLElement;
}
declare class JButton extends JControl {
    private _text;
    text: string;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class JPanel extends JControl {
    private _content;
    content: string;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class JTextbox extends JButton {
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class JListItem extends JControl {
    static StaticID: number;
    static StaticString: string;
    id: string;
    name: string;
    private _valueMember;
    valuemember: string;
    private _textMember;
    textmember: string;
    private _index;
    index: number;
    private _text;
    text: any;
    private _value;
    value: any;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class JList extends JControl {
    itemContainer: HTMLElement;
    protected itemControls: JListItem[];
    protected itemTemplates: any[];
    private _itemsource;
    itemsource: JDataSource;
    private _valueMember;
    valuemember: string;
    private _textMember;
    textmember: string;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    protected loadTemplates(): void;
    protected onTemplateApply(): void;
    protected bindItems(): void;
    private resetItemIndex();
    protected addItem(data: any): JListItem;
}
declare class JCheckboxList extends JList {
    private _checkedvalue;
    checkedvalue: any[];
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    protected addItem(data: any): JListItem;
    protected onItemDataChanged(sender: any, name: string, originalvalue: any): void;
}
declare class JRadioList extends JList {
    static StaticID: number;
    private itemid;
    private _checkedvalue;
    checkedvalue: any;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    protected addItem(data: any): JListItem;
    private _onItemDataChangedHanding;
    protected onItemDataChanged(sender: any, name: string, originalvalue: any): void;
}
declare class JDropdownList extends JList {
    private _selectedvalue;
    selectedvalue: any;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    protected bindItems(): void;
}
