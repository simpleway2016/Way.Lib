declare class JDataSource {
    buffer: JObserveObject[];
    private addFuncs;
    private removeFuncs;
    length: number;
    constructor();
    addEventListener(_type: string, listener: (sender, data, index: number) => any): number;
    removeEventListener(_type: string, index: number): void;
    loadMore(length: number, callback?: (count: number, err: string) => any): void;
    loadData(skip: number, take: number, callback?: (count: number, err: string) => any): void;
    loadAll(): void;
    add(data: JObserveObject): void;
    insert(index: number, data: JObserveObject): void;
    remove(data: JObserveObject): void;
    removeAt(index: number): void;
    clear(): void;
}
declare class JArraySource extends JDataSource {
    private _array;
    private _currentPosition;
    constructor(data: JObserveObject[]);
    loadData(skip: number, take: number, callback?: (count: number, err: string) => any): void;
    loadMore(length: number, callback?: (count: number, err: string) => any): number;
    loadAll(): void;
    clear(): void;
}
declare class JServerControllerSource extends JDataSource {
    private _propertyName;
    private _controller;
    private _skip;
    private _tranid;
    constructor(controller: WayScriptRemoting, propertyName: string);
    loadData(skip: number, take: number, callback?: (count: number, err: string) => any): void;
    loadMore(length: number, cb?: (count: number, err: string) => any): number;
    private getDatasourceLength(tranid, length, callback?);
    private loadDataFromServer(tranid, length, callback?);
    loadAll(): void;
    clear(): void;
}
