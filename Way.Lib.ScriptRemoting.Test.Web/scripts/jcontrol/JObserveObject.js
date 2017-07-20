var JObserveObject = (function () {
    function JObserveObject(data, parent, parentname) {
        if (parent === void 0) { parent = null; }
        if (parentname === void 0) { parentname = null; }
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
        };
    };
    JObserveObject.prototype.__addProperty = function (proName) {
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
var JDataSource = (function () {
    function JDataSource(data) {
        this.addFuncs = [];
        this.removeFuncs = [];
        this.source = data;
    }
    JDataSource.prototype.addEventListener = function (_type, listener) {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            funcs.push(listener);
        }
    };
    JDataSource.prototype.removeEventListener = function (_type, listener) {
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
    };
    JDataSource.prototype.add = function (data) {
        this.source.push(data);
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, this.source.length - 1);
        }
    };
    JDataSource.prototype.insert = function (index, data) {
        this.source.splice(index, 0, data);
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    };
    JDataSource.prototype.remove = function (data) {
        var index = this.source.indexOf(data);
        this.removeAt(index);
    };
    JDataSource.prototype.removeAt = function (index) {
        if (index < this.source.length && index >= 0) {
            var data = this.source[index];
            this.source.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    };
    return JDataSource;
}());
//# sourceMappingURL=JObserveObject.js.map