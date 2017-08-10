declare class JControl implements INotifyPropertyChanged {
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    protected onPropertyChanged(proName: string, originalValue: any): void;
    notifyDatacontextPropertyChanged(datacontext: any, proName: string, originalValue: any): void;
    static StaticID: number;
    static StaticString: string;
    originalElement: HTMLElement;
    element: HTMLElement;
    onPropertyChangeds: any[];
    databind: string;
    expression: string;
    protected templates: HTMLElement[];
    protected templateMatchProNames: string[];
    protected currentTemplate: HTMLElement;
    private _datacontext;
    datacontext: any;
    private _parentJControl;
    parentJControl: JControl;
    private _id;
    id: string;
    private _cid;
    cid: string;
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
    private _buffersize;
    buffersize: number;
    private _scrollController;
    private _loadonscroll;
    loadonscroll: boolean;
    itemContainer: HTMLElement;
    onLoading: () => any;
    onError: (err: string) => any;
    protected itemControls: JListItem[];
    protected itemTemplates: any[];
    private _addEventIndex;
    private _removeEventIndex;
    private _itemsource;
    itemsource: JDataSource;
    private _valueMember;
    valuemember: string;
    private _textMember;
    textmember: string;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
    rebind(): void;
    private clearItems();
    protected loadTemplates(): void;
    protected onTemplateApply(): void;
    loadMoreData(): void;
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
declare class JCheckbox extends JListItem {
    private _checked;
    checked: boolean;
    constructor(element: HTMLElement, templates?: any[], datacontext?: any);
}
declare class ScrollSourceManager {
    contentContainer: HTMLElement;
    list: JList;
    private listener;
    private _checkBufferSize;
    constructor(list: JList);
    dispose(): void;
    onListLoadData(): void;
    private onScroll();
}
