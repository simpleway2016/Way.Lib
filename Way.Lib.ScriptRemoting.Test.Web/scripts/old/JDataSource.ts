﻿class JDataSource {
    buffer: JObserveObject[] = [];
    private addFuncs: ((sender, data, index: number) => any)[] = [];
    private removeFuncs: ((sender, data, index: number) => any)[] = [];
    //数据总数
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
    loadMore(length: number, callback: (count: number, err: string) => any = null)  {

    }
    //加载某一页数据，之前加载数据会被清除
    loadData(skip: number, take: number, callback: (count: number, err: string) => any = null)
    {

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

    loadData(skip: number, take: number, callback: (count: number, err: string) => any = null) {
        this.clear();
        this._currentPosition = skip;
        this.loadMore(take, callback);
    }

    loadMore(length: number, callback: (count: number, err: string) => any = null) {
        if (length <= 0)
            return 0;

        var count = 0;
        for (var i = this._currentPosition; i < this._array.length; i++)
        {
            var data = this._array[i];
            if (data instanceof JObserveObject) {
                this.add(data);
            }
            else {
                this.add(new JObserveObject(data));
            }
            count++;
            if (count == length)
                break;
        }
        this._currentPosition += count;
        if (callback)
        {
            callback(count, null);
        }
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

class JServerControllerSource extends JDataSource {
    private _propertyName: string;
    private _controller: any;
    private _skip: number = 0;
    private _tranid: number = 0;
    private _hasModeData: boolean = true;

    constructor(controller: WayScriptRemoting , propertyName:string) {
        super();
        this.length = -1;
        this._controller = controller;
        this._propertyName = propertyName;
    }
    //加载某一页数据，之前加载数据会被清除
    loadData(skip: number, take: number, callback: (count: number, err: string) => any = null) {
        this.clear();
        this._skip = skip;
        this.loadMore(take, callback);
    }
    loadMore(length: number, cb: ( count:number, err: string) => any = null) {
        if (length <= 0)
            return 0;
        if (!this._hasModeData)
        {
            if (cb)
            {
                cb(0, null);
            }
            return;
        }
        this._tranid++;

        this.loadDataFromServer(this._tranid, length, cb);
    }

    private getDatasourceLength(tranid: number, length: number, callback: (count: number, err: string) => any = null)
    {
        this._controller.server.GetDataLength(this._propertyName, "", (ret: number, err) => {
            if (err) {
                if (callback) {
                    callback(0, err);
                }
            }
            else {
                if (tranid == this._tranid) {
                    this.length = ret;
                    this.loadDataFromServer(tranid, length, callback);
                }

            }
        });
    }

    private loadDataFromServer(tranid: number, length: number, callback: (count: number, err: string) => any = null)
    {
        if (this.length == -1)
        {
            this.getDatasourceLength(tranid, length, callback);
            return;
        }

        this._controller.server.LoadData(this._propertyName, this._skip, length, "",null, (ret : any[], err) => {
            if (err) {
                if (callback) {
                    callback(0, err);
                }
            }
            else {
                if (tranid == this._tranid) {
                    var count = 0;
                    for (var i = 0; i < ret.length; i++) {
                        var data = ret[i];
                        this.add(new JObserveObject(data));
                        count++;
                    }
                    if (ret.length == 0)
                    {
                        this._hasModeData = false;
                    }
                    this._skip += count;
                    if (callback) {
                        callback(count, null);
                    }
                }

            }
        });
    }

    loadAll() {
        throw "不支持此方法";
    }
    clear() {
        super.clear();
        this._skip = 0;
        this._hasModeData = true;
    }
}