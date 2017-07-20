interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
    removeListener(index: number): any;
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
