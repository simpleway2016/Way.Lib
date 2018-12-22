var JObserveObject = (function () {
    function JObserveObject(data, parent, parentname) {
        if (parent === void 0) { parent = null; }
        if (parentname === void 0) { parentname = null; }
        this._histories = [];
        this.__onchanges = [];
        if (data instanceof JObserveObject) {
            var old = data;
            this.addPropertyChangedListener(function (_model, _name, _value) {
                old.onPropertyChanged(_name, _value);
            });
            data = old.__data;
        }
        this.__data = data;
        this.__parent = parent;
        this.__parentName = parentname;
        for (var p in data) {
            this.__addProperty(p);
        }
    }
    JObserveObject.prototype.hasProperty = function (proname) {
        var checkname = proname;
        var index = proname.indexOf(".");
        if (index >= 0) {
            checkname = proname.substr(0, index);
        }
        var result = Object.getOwnPropertyDescriptor(this, checkname);
        if (result)
            result = true;
        else
            result = false;
        if (result && index >= 0) {
            if (this[checkname] instanceof JObserveObject) {
                result = this[checkname].hasProperty(proname.substr(index + 1));
            }
        }
        return result;
    };
    JObserveObject.prototype.addProperty = function (proName) {
        var checkname = proName;
        if (proName.indexOf(".") < 0) {
            if (Object.getOwnPropertyDescriptor(this, proName))
                return;
            this.__data[proName] = null;
        }
        else {
            var index = proName.indexOf(".");
            checkname = proName.substr(0, index);
            if (Object.getOwnPropertyDescriptor(this, checkname)) {
                if (this[checkname] instanceof JObserveObject) {
                    this[checkname].addProperty(proName.substr(index + 1));
                }
                return;
            }
            this.__data[checkname] = new JObserveObject({}, this, checkname);
            this.__data[checkname].addProperty(proName.substr(index + 1));
        }
        Object.defineProperty(this, checkname, {
            get: this.getFunc(checkname),
            set: this.setFunc(checkname),
            enumerable: true,
            configurable: true
        });
    };
    JObserveObject.prototype.getFunc = function (name) {
        return function () {
            return this.__data[name];
        };
    };
    JObserveObject.prototype.setFunc = function (checkname) {
        return function (value) {
            if (!Object.getOwnPropertyDescriptor(this.__data, checkname))
                throw new Error("不包含成员" + checkname);
            if (this.__data[checkname] !== value) {
                var original = this.__data[checkname];
                this.setHistory(checkname, original, value);
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
        };
    };
    JObserveObject.prototype.getChange = function () {
        var data = {};
        for (var i = 0; i < this._histories.length; i++) {
            var item = this._histories[i];
            data[item.name] = item.value;
        }
        this._histories = [];
        return data;
    };
    JObserveObject.prototype.setHistory = function (name, original, value) {
        for (var i = 0; i < this._histories.length; i++) {
            var item = this._histories[i];
            if (item.name == name) {
                if (item.original == value) {
                    this._histories.splice(i, 1);
                }
                else {
                    item.value = value;
                }
                return;
            }
        }
        var newitem = {};
        newitem.original = original;
        newitem.value = value;
        newitem.name = name;
        this._histories.push(newitem);
    };
    JObserveObject.prototype.__addProperty = function (proName) {
        var type = typeof this.__data[proName];
        if (type == "object" && this.__data[proName] && !(this.__data[proName] instanceof Array)) {
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
    };
    JObserveObject.prototype.addPropertyChangedListener = function (func) {
        this.__onchanges.push(func);
        return this.__onchanges.length - 1;
    };
    JObserveObject.prototype.removeListener = function (index) {
        this.__onchanges[index] = null;
    };
    JObserveObject.prototype.onPropertyChanged = function (proName, originalValue) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this, proName, originalValue);
            }
        }
    };
    return JObserveObject;
}());
