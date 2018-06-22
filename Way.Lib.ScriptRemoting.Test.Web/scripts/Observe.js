function SetObserveProperty(obj, pro, initValue, arg, path) {
    Object.defineProperty(obj, "_$" + pro, {
        configurable: false,
        writable: true,
        enumerable: false
    });

    if (arg && arg.deep === true && typeof initValue === "object") {
        if (path.length > 0)
            path += ".";
        obj["_$" + pro] = Observe(initValue, arg, path + pro);
    }
    else {
        obj["_$" + pro] = initValue;
    }

    Object.defineProperty(obj, pro, {
        configurable: true,
        enumerable: true,
        get: function () {
            return this["_$" + pro];
        },
        set: function (newValue) {
            if (this["_$" + pro] !== newValue) {
                var oldvalue = this["_$" + pro];

                if (arg && arg.deep === true && typeof newValue === "object") {
                    if (path.length > 0)
                        path += ".";
                    newValue = Observe(newValue, arg, path + pro);
                }

                this["_$" + pro] = newValue
                if (this.onchange) {
                    this.onchange({
                        sender: this,
                        propertyName: pro,
                        path: path,
                        newValue: newValue,
                        oldValue: oldvalue
                    });
                }
            }
        }
    });
}

/**
 * 把对象设置为Observe类型，支持onchange(e)事件
 * @param data
 * @param arg arg对象，包括onchange deep
 */
function Observe(data, arg, path) {
    if (path === undefined)
        path = "";
    if (Array.isArray(data)) {
        var arr = [];
        for (var i = 0; i < data.length; i++) {
            arr[i] = Observe(data[i], arg, path + "[" + i + "]");
        }
        return arr;
    }
    else {
        var obj = {};
        for (var pro in data) {
            SetObserveProperty(obj, pro, data[pro], arg, path);
        }
        if (arg && arg.onchange && typeof arg.onchange === "function") {
            obj.onchange = arg.onchange;
        }
        return obj;
    }
}