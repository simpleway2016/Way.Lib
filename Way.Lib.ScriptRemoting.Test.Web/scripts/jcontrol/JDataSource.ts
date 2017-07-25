class JDataSource {
    buffer: JObserveObject[] = [];
    private addFuncs: ((sender, data, index: number) => any)[] = [];
    private removeFuncs: ((sender, data, index: number) => any)[] = [];

    length: number;

    constructor() {

    }

    addEventListener(_type: string, listener: (sender, data, index: number) => any): number {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            funcs.push(listener);
            return funcs.length - 1;
        }
    }

    removeEventListener(_type: string, index: number) {
        if (index >= 0) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            if (index < funcs.length) {
                funcs[index] = null;
            }

        }
    }

    //加载length条数据到source，返回实际加载的数量
    loadMore(length: number):number {
        return 0;
    }
    //加载剩余所有数据
    loadAll() {

    }

    add(data: JObserveObject) {
        this.buffer.push(data);
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, this.buffer.length - 1);
        }
    }

    insert(index: number, data: JObserveObject) {
        this.buffer.splice(index, 0, data);

        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    }

    remove(data: JObserveObject) {
        var index = this.buffer.indexOf(data);
        this.removeAt(index);
    }
    removeAt(index: number) {
        if (index < this.buffer.length && index >= 0) {
            var data = this.buffer[index];

            this.buffer.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    }
    clear()
    {
        while (this.buffer.length > 0)
        {
            var index = this.buffer.length - 1;
            var data = this.buffer[index];

            this.buffer.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    }
}

class JArraySource extends JDataSource {
    private _array : any[];
    private _currentPosition: number = 0;

    constructor(data: JObserveObject[]) {
        super();
        this._array = data;
        this.length = data.length;
    }

    loadMore(length: number): number {
        if (length <= 0)
            return 0;

        var count = 0;
        for (var i = this._currentPosition; i < this._array.length; i++)
        {
            var data = this._array[i];
            this.add(data);
            count++;
            if (count == length)
                break;
        }
        this._currentPosition += count;
        return count;
    }
    loadAll() {
        var count = 0;
        for (var i = this._currentPosition; i < this._array.length; i++) {
            var data = this._array[i];
            this.add(data);
            count++;
        }
        this._currentPosition += count;
    }
    clear()
    {
        super.clear();
        this._currentPosition = 0;
    }
}