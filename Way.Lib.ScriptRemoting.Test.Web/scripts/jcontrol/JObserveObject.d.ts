interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
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
    private getFunc(name);
    private setFunc(checkname);
    getChange(): any;
    private setHistory(name, original, value);
    private __addProperty(proName);
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number;
    removeListener(index: number): void;
    onPropertyChanged(proName: string, originalValue: any): void;
}
