interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): any;
}
declare class JObserveObject implements INotifyPropertyChanged {
    __data: any;
    __parent: JObserveObject;
    __parentName: string;
    private _histories;
    private __onchanges;
    constructor(data: any, parent?: JObserveObject, parentname?: string);
    hasProperty(proname: string): boolean;
    addProperty(proName: string): void;
    private getFunc;
    private setFunc;
    getChange(): any;
    private setHistory;
    private __addProperty;
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    onPropertyChanged(proName: string, originalValue: any): void;
}
