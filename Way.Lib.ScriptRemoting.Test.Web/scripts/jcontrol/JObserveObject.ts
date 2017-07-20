


class JObserveObject implements INotifyPropertyChanged {

    __data;
    __parent: JObserveObject;
    __parentName: string;
    private __onchanges = [];

    constructor(data, parent: JObserveObject = null, parentname: string = null) {

        if (data instanceof JObserveObject) {
            var old: JObserveObject = data;
            this.addPropertyChangedListener((_model, _name, _value) => {
                old.onPropertyChanged(_name, _value);
            });
            //old发生变化，无法通知newModel，否则就进入死循环了
            data = old.__data;
        }

        this.__data = data;
        this.__parent = parent;
        this.__parentName = parentname;

        for (var p in data) {
            this.__addProperty(p);
        }
    }

    hasProperty(proname: string): boolean
    {
        var checkname = proname;
        var index = proname.indexOf(".");
        if (index >= 0)
        {
            checkname = proname.substr(0, index);
        }

        var result : any = Object.getOwnPropertyDescriptor(this, checkname);
        if (result)
            result = true;
        else
            result = false;

        if (result && index >= 0)
        {
            if (this[checkname] instanceof JObserveObject) {
                result = this[checkname].hasProperty(proname.substr(index + 1));
            }
        }
        return result;
    }
    addProperty(proName: string)
    {
        var checkname = proName;
        if (proName.indexOf(".") < 0) {

            if (Object.getOwnPropertyDescriptor(this, proName))//已经有这个属性
                return;

            this.__data[proName] = null;
        }
        else {
            var index = proName.indexOf(".");
            checkname = proName.substr(0, index);

            if (Object.getOwnPropertyDescriptor(this, checkname))
            {
                if (this[checkname] instanceof JObserveObject)
                {
                    (<JObserveObject>this[checkname]).addProperty(proName.substr(index + 1));
                }
                
                return;
            }
            this.__data[checkname] = new JObserveObject({}, this, checkname);
            (<JObserveObject>this.__data[checkname]).addProperty(proName.substr(index + 1));
        }

        
        Object.defineProperty(this, checkname, {
            get: this.getFunc(checkname),
            set: this.setFunc(checkname),
            enumerable: true,
            configurable: true
        });
       
    }

    private getFunc(name)
    {
        return function () {
            return this.__data[name];
        }
    }
    private setFunc(checkname:string) {
        return function (value) {
            if (!Object.getOwnPropertyDescriptor(this.__data, checkname))
                throw new Error("不包含成员" + checkname);
            if (this.__data[checkname] != value) {
                var original = this.__data[checkname];

                this.__data[checkname] = value;
                this.onPropertyChanged(checkname, original);
                if (this.__parent) {
                    var curparent = this.__parent;
                    var pname = this.__parentName;

                    var path = checkname;
                    while (curparent) {
                        path = pname + "." + path;
                        curparent.onPropertyChanged(path, original);
                        pname = curparent.__parentName;
                        curparent = curparent.__parent;

                    }
                }

            }
        }
    }
    private __addProperty(proName) {
        var type = typeof this.__data[proName];
        if (type == "object" && !(this.__data[proName] instanceof Array)) {
            this[proName] = new JObserveObject(this.__data[proName], this, proName);
        }
        else if (type != "function") {

            Object.defineProperty(this, proName, {
                get: this.getFunc(proName),
                set: this.setFunc(proName),
                enumerable: true,
                configurable: true
            });
        }
    }

    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number {
        this.__onchanges.push(func);
        return this.__onchanges.length - 1;
    }

    removeListener(index: number) {
        this.__onchanges[index] = null;
    }

    onPropertyChanged( proName: string, originalValue: any) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this, proName, originalValue);
            }
        }
    }
}


class JDataSource
{
    source: JObserveObject[];
    private addFuncs: ((sender, data, index: number)=>any)[] = [];
    private removeFuncs: ((sender, data,index:number) => any)[] = [];

    constructor(data: JObserveObject[])
    {
        this.source = data;
    }

    addEventListener(_type: string, listener: (sender, data, index: number) => any) {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            funcs.push(listener);
        }
    }

    removeEventListener(_type: string, listener: (sender, data, index: number) => any) {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            for (var i = 0; i < funcs.length; i++) {
                if (funcs[i] == listener) {
                    funcs[i] = null;
                }
            }
        }
    }

    add(data: JObserveObject)
    {
        this.source.push(data);
        for (var i = 0; i < this.addFuncs.length; i++)
        {
            this.addFuncs[i](this, data, this.source.length - 1);
        }
    }

    insert(index: number, data: JObserveObject) {
        this.source.splice(index, 0, data);

        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    }

    remove(data: JObserveObject) {
        //for (var i = 0; i < this.source.length; i++)
        //{
        //    if (this.source[i] == data)
        //    {
        //        this.removeAt(i);
        //        break;
        //    }
        //}
        var index = this.source.indexOf(data);
        this.removeAt(index);
    }
    removeAt(index: number) {
        if (index < this.source.length && index >= 0)
        {
            var data = this.source[index];
            //for (var i = index; i < this.source.length - 1; i++) {
            //    this.source[i] = this.source[i + 1];
            //}
            //this.source.length--;
            
            this.source.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    }
}
