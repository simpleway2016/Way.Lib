var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var JDataSource = (function () {
    function JDataSource() {
        this.buffer = [];
        this.addFuncs = [];
        this.removeFuncs = [];
    }
    JDataSource.prototype.addEventListener = function (_type, listener) {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            funcs.push(listener);
            return funcs.length - 1;
        }
    };
    JDataSource.prototype.removeEventListener = function (_type, index) {
        if (index >= 0) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            if (index < funcs.length) {
                funcs[index] = null;
            }
        }
    };
    JDataSource.prototype.loadMore = function (length, callback) {
        if (callback === void 0) { callback = null; }
    };
    JDataSource.prototype.loadData = function (skip, take, callback) {
        if (callback === void 0) { callback = null; }
    };
    JDataSource.prototype.loadAll = function () {
    };
    JDataSource.prototype.add = function (data) {
        this.buffer.push(data);
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, this.buffer.length - 1);
        }
    };
    JDataSource.prototype.insert = function (index, data) {
        this.buffer.splice(index, 0, data);
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    };
    JDataSource.prototype.remove = function (data) {
        var index = this.buffer.indexOf(data);
        this.removeAt(index);
    };
    JDataSource.prototype.removeAt = function (index) {
        if (index < this.buffer.length && index >= 0) {
            var data = this.buffer[index];
            this.buffer.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    };
    JDataSource.prototype.clear = function () {
        while (this.buffer.length > 0) {
            var index = this.buffer.length - 1;
            var data = this.buffer[index];
            this.buffer.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    };
    return JDataSource;
}());
var JArraySource = (function (_super) {
    __extends(JArraySource, _super);
    function JArraySource(data) {
        var _this = _super.call(this) || this;
        _this._currentPosition = 0;
        _this._array = data;
        _this.length = data.length;
        return _this;
    }
    JArraySource.prototype.loadData = function (skip, take, callback) {
        if (callback === void 0) { callback = null; }
        this.clear();
        this._currentPosition = skip;
        this.loadMore(take, callback);
    };
    JArraySource.prototype.loadMore = function (length, callback) {
        if (callback === void 0) { callback = null; }
        if (length <= 0)
            return 0;
        var count = 0;
        for (var i = this._currentPosition; i < this._array.length; i++) {
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
        if (callback) {
            callback(count, null);
        }
    };
    JArraySource.prototype.loadAll = function () {
        var count = 0;
        for (var i = this._currentPosition; i < this._array.length; i++) {
            var data = this._array[i];
            this.add(data);
            count++;
        }
        this._currentPosition += count;
    };
    JArraySource.prototype.clear = function () {
        _super.prototype.clear.call(this);
        this._currentPosition = 0;
    };
    return JArraySource;
}(JDataSource));
var JServerControllerSource = (function (_super) {
    __extends(JServerControllerSource, _super);
    function JServerControllerSource(controller, propertyName) {
        var _this = _super.call(this) || this;
        _this._skip = 0;
        _this._tranid = 0;
        _this._hasModeData = true;
        _this.length = -1;
        _this._controller = controller;
        _this._propertyName = propertyName;
        return _this;
    }
    JServerControllerSource.prototype.loadData = function (skip, take, callback) {
        if (callback === void 0) { callback = null; }
        this.clear();
        this._skip = skip;
        this.loadMore(take, callback);
    };
    JServerControllerSource.prototype.loadMore = function (length, cb) {
        if (cb === void 0) { cb = null; }
        if (length <= 0)
            return 0;
        if (!this._hasModeData) {
            if (cb) {
                cb(0, null);
            }
            return;
        }
        this._tranid++;
        this.loadDataFromServer(this._tranid, length, cb);
    };
    JServerControllerSource.prototype.getDatasourceLength = function (tranid, length, callback) {
        var _this = this;
        if (callback === void 0) { callback = null; }
        this._controller.server.GetDataLength(this._propertyName, "", function (ret, err) {
            if (err) {
                if (callback) {
                    callback(0, err);
                }
            }
            else {
                if (tranid == _this._tranid) {
                    _this.length = ret;
                    _this.loadDataFromServer(tranid, length, callback);
                }
            }
        });
    };
    JServerControllerSource.prototype.loadDataFromServer = function (tranid, length, callback) {
        var _this = this;
        if (callback === void 0) { callback = null; }
        if (this.length == -1) {
            this.getDatasourceLength(tranid, length, callback);
            return;
        }
        this._controller.server.LoadData(this._propertyName, this._skip, length, "", null, function (ret, err) {
            if (err) {
                if (callback) {
                    callback(0, err);
                }
            }
            else {
                if (tranid == _this._tranid) {
                    var count = 0;
                    for (var i = 0; i < ret.length; i++) {
                        var data = ret[i];
                        _this.add(new JObserveObject(data));
                        count++;
                    }
                    if (ret.length == 0) {
                        _this._hasModeData = false;
                    }
                    _this._skip += count;
                    if (callback) {
                        callback(count, null);
                    }
                }
            }
        });
    };
    JServerControllerSource.prototype.loadAll = function () {
        throw "不支持此方法";
    };
    JServerControllerSource.prototype.clear = function () {
        _super.prototype.clear.call(this);
        this._skip = 0;
        this._hasModeData = true;
    };
    return JServerControllerSource;
}(JDataSource));
