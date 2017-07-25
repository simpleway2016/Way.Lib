declare class JDataSource {
    buffer: JObserveObject[];
    private addFuncs;
    private removeFuncs;
    length: number;
    constructor();
    addEventListener(_type: string, listener: (sender, data, index: number) => any): number;
    removeEventListener(_type: string, index: number): void;
    loadMore(length: number): number;
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
    loadMore(length: number): number;
    loadAll(): void;
    clear(): void;
}
