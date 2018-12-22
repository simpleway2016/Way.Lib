var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var JBindConfig = (function () {
    function JBindConfig(dataPropertyName, elementPropertyName) {
        this.dataPropertyName = dataPropertyName;
        this.elementPropertyName = elementPropertyName;
    }
    return JBindConfig;
}());
var JBindExpression = (function () {
    function JBindExpression(dataPropertyName, expression) {
        this.dataPropertyName = dataPropertyName;
        this.expression = expression;
    }
    return JBindExpression;
}());
var JBinder = (function () {
    function JBinder(control) {
        var _this = this;
        this.disposed = false;
        this.control = control;
        AllJBinders.forEach(function (binder, index) {
            if (binder && binder.control == control && binder.constructor == _this.constructor) {
                AllJBinders[index].dispose();
                delete AllJBinders[index];
                AllJBinders[index] = null;
            }
        });
        this.bindingDataContext = this.datacontext;
        if (this.bindingDataContext) {
            this._datacontext_listen_index =
                this.bindingDataContext.addPropertyChangedListener(function (sender, name, oldvalue) { return _this.onPropertyChanged(sender, name, oldvalue); });
        }
    }
    Object.defineProperty(JBinder.prototype, "datacontext", {
        get: function () {
            if (!this._datacontext) {
                this._datacontext = this.getDatacontext();
            }
            return this._datacontext;
        },
        enumerable: true,
        configurable: true
    });
    JBinder.pushBinder = function (binder) {
        for (var i = 0; i < AllJBinders.length; i++) {
            if (!AllJBinders[i]) {
                AllJBinders[i] = binder;
                return;
            }
        }
        AllJBinders.push(binder);
    };
    JBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
    };
    JBinder.prototype.getDatacontext = function () {
        if (this.control instanceof JControl) {
            this.rootControl = this.control;
            var data = this.control.datacontext;
            if (!data && this.control.element) {
                this.rootControl = this.control.element.getDataContextControl();
                if (this.rootControl) {
                    data = this.rootControl.datacontext;
                }
            }
            return data;
        }
        else {
            this.rootControl = this.control.getDataContextControl();
            var data;
            if (this.rootControl) {
                data = this.rootControl.datacontext;
            }
            return data;
        }
    };
    JBinder.prototype.dispose = function () {
        this._datacontext.removeListener(this._datacontext_listen_index);
        this._datacontext = null;
        this.disposed = true;
        this.rootControl = null;
    };
    JBinder.addPropertyIfNotExist = function (data, propertyName) {
        if (data instanceof JObserveObject) {
            var observeData = data;
            if (observeData.hasProperty(propertyName) == false) {
                observeData.addProperty(propertyName);
            }
        }
    };
    JBinder.moveAttributeBindToDatabind = function (element) {
        if (!element.attributes)
            return;
        var databind = element.getAttribute("databind");
        if (!databind)
            databind = "";
        for (var i = 0; i < element.attributes.length; i++) {
            var att = element.attributes[i];
            if (att.name == "databind")
                continue;
            var name = att.name;
            if (name == "class")
                name = "className";
            else if (name == "innerhtml")
                name = "innerHTML";
            var r;
            if (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(att.value)) {
                var nowLen = element.attributes.length;
                element.removeAttribute(att.name);
                if (element.attributes.length < nowLen) {
                    i--;
                }
                else {
                    element.setAttribute(att.name, "");
                }
                databind += ";" + name + "=$" + r[1];
            }
            else if (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(att.value)) {
                var nowLen = element.attributes.length;
                element.removeAttribute(att.name);
                if (element.attributes.length < nowLen) {
                    i--;
                }
                else {
                    element.setAttribute(att.name, "");
                }
                databind += ";" + name + "=@" + r[1];
            }
        }
        element.setAttribute("databind", databind);
    };
    return JBinder;
}());
var JDatacontextBinder = (function (_super) {
    __extends(JDatacontextBinder, _super);
    function JDatacontextBinder(control) {
        var _this = _super.call(this, control) || this;
        _this.configs = [];
        if (!_this.bindingDataContext)
            return _this;
        var databind;
        if (control instanceof JControl) {
            databind = control.databind;
        }
        else {
            JBinder.moveAttributeBindToDatabind(control);
            databind = control.getAttribute("databind");
        }
        var regexp = _this.getRegexp();
        while (true) {
            var result = regexp.exec(databind);
            if (!result)
                break;
            var elementPropertyName = result[1];
            var dataPropertyName = result[2];
            _this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
            JBinder.addPropertyIfNotExist(_this.bindingDataContext, dataPropertyName);
            databind = databind.substr(result.index + result[0].length);
        }
        if (control instanceof JControl) {
            var jcontrol = control;
            _this.controlListenIndex = jcontrol.addPropertyChangedListener(function (s, n, o) { _this.onControlPropertyChanged(s, n, o); });
        }
        else {
            var element = control;
            _this.configs.forEach(function (config) {
                if (config.elementPropertyName == "value" || config.elementPropertyName == "checked") {
                    element.addEventListener("change", _this.listenElementEvent(_this, config), false);
                }
            });
        }
        if (_this.configs.length > 0) {
            JBinder.pushBinder(_this);
            _this.updateValue();
        }
        _this.bindChildren(_this.control instanceof JControl ? _this.control.element : _this.control);
        return _this;
    }
    JDatacontextBinder.prototype.bindChildren = function (element) {
        var _this = this;
        if (!element)
            return;
        for (var i = 0; i < element.children.length; i++) {
            var existed = false;
            AllJBinders.every(function (binder) {
                if (binder && binder.control == element.children[i] && binder.constructor == _this.constructor) {
                    existed = true;
                    return false;
                }
                return true;
            });
            if (!existed) {
                var ele = element.children[i];
                var myconstructor = this.constructor;
                if (ele.JControl) {
                    eval("new myconstructor(ele.JControl)");
                }
                else {
                    eval("new myconstructor(ele)");
                }
            }
        }
    };
    JDatacontextBinder.prototype.updateValue = function () {
        var data = this.datacontext;
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                var value;
                var control = this.control;
                eval("value=data." + config.dataPropertyName);
                if (value) {
                    eval("control." + config.elementPropertyName + " = value");
                }
            }
            catch (e) {
            }
        }
    };
    JDatacontextBinder.prototype.getConfigByDataProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JDatacontextBinder.prototype.getConfigByElementProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    };
    JDatacontextBinder.prototype.listenElementEvent = function (self, config) {
        return function () {
            if (self.disposed)
                return;
            var data = self.datacontext;
            var control = self.control;
            try {
                eval("data." + config.dataPropertyName + " = control." + config.elementPropertyName);
            }
            catch (e) {
            }
        };
    };
    JDatacontextBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        if (this.disposed)
            return;
        if (this.control instanceof JControl) {
            this.control.notifyDatacontextPropertyChanged(sender, name, originalValue);
        }
        try {
            var config = this.getConfigByDataProName(name);
            if (config) {
                var data = sender;
                var control = this.control;
                eval("control." + config.elementPropertyName + " = data." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    };
    JDatacontextBinder.prototype.onControlPropertyChanged = function (sender, name, originalValue) {
        try {
            if (this.disposed)
                return;
            var config = this.getConfigByElementProName(name);
            if (config) {
                var data = this.datacontext;
                var control = this.control;
                eval("data." + config.dataPropertyName + " = control." + name);
            }
        }
        catch (e) {
        }
    };
    JDatacontextBinder.prototype.dispose = function () {
        _super.prototype.dispose.call(this);
        this.disposed = true;
        if (this.controlListenIndex) {
            this.control.removeListener(this.controlListenIndex);
            this.controlListenIndex = 0;
        }
        this.configs = [];
    };
    JDatacontextBinder.prototype.getRegexp = function () {
        return /([\w|\.]+)[ ]?=[ ]?\@([\w|\.]+)/;
    };
    return JDatacontextBinder;
}(JBinder));
var JControlBinder = (function (_super) {
    __extends(JControlBinder, _super);
    function JControlBinder(control) {
        return _super.call(this, control) || this;
    }
    JControlBinder.prototype.getDatacontext = function () {
        var element;
        if (this.control instanceof JControl) {
            element = this.control.element.parentElement;
        }
        else {
            element = this.control;
        }
        this.rootControl = element.getContainer();
        return this.rootControl;
    };
    JControlBinder.prototype.getRegexp = function () {
        return /([\w|\.]+)[ ]?=[ ]?\$([\w|\.]+)/;
    };
    return JControlBinder;
}(JDatacontextBinder));
var JDatacontextExpressionBinder = (function (_super) {
    __extends(JDatacontextExpressionBinder, _super);
    function JDatacontextExpressionBinder(control) {
        var _this = _super.call(this, control) || this;
        _this.configs = [];
        if (!_this.bindingDataContext)
            return _this;
        var expressionStr;
        if (control instanceof JControl) {
            for (var pro in control) {
                if (pro == "expression" || /expression([0-9]+)/.exec(pro)) {
                    _this.handleExpression(control[pro]);
                }
            }
        }
        else {
            var element = control;
            var allattributes = element.attributes;
            for (var i = 0; i < allattributes.length; i++) {
                if (allattributes[i].name == "expression" || /expression([0-9]+)/.exec(allattributes[i].name)) {
                    _this.handleExpression(allattributes[i].value);
                }
            }
        }
        if (_this.configs.length > 0) {
            JBinder.pushBinder(_this);
        }
        _this.updateValue();
        _this.bindChildren(_this.control instanceof JControl ? _this.control.element : _this.control);
        return _this;
    }
    JDatacontextExpressionBinder.prototype.bindChildren = function (element) {
        var _this = this;
        if (!element)
            return;
        for (var i = 0; i < element.children.length; i++) {
            var existed = false;
            AllJBinders.every(function (binder) {
                if (binder && binder.control == element.children[i] && binder.constructor == _this.constructor) {
                    existed = true;
                    return false;
                }
                return true;
            });
            if (!existed) {
                var ele = element.children[i];
                var myconstructor = this.constructor;
                if (ele.JControl) {
                    eval("new myconstructor(ele.JControl)");
                }
                else {
                    eval("new myconstructor(ele)");
                }
            }
        }
    };
    JDatacontextExpressionBinder.prototype.updateValue = function () {
        for (var i = 0; i < this.configs.length; i++) {
            var exconfig = this.configs[i];
            if (exconfig) {
                var element = this.control;
                var data = this.datacontext;
                eval(exconfig.expression);
            }
        }
    };
    JDatacontextExpressionBinder.prototype.handleExpression = function (expressionStr) {
        var original;
        var expression_exp = this.getRegexp();
        var datacontext = this.datacontext;
        expressionStr = expressionStr.replace(/\{0\}\./g, "element.");
        if (expressionStr) {
            while (true) {
                var result = expression_exp.exec(expressionStr);
                if (!result)
                    break;
                var dataPropertyName = result[1];
                this.configs.push(new JBindExpression(dataPropertyName, null));
                JBinder.addPropertyIfNotExist(datacontext, dataPropertyName);
                expressionStr = expressionStr.replace(result[0], "data." + dataPropertyName);
            }
            for (var i = 0; i < this.configs.length; i++) {
                if (!this.configs[i].expression) {
                    this.configs[i].expression = expressionStr;
                }
            }
        }
    };
    JDatacontextExpressionBinder.prototype.getConfigByDataProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JDatacontextExpressionBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        if (this.disposed)
            return;
        var config = this.getConfigByDataProName(name);
        if (config) {
            var element = this.control;
            var data = sender;
            eval(config.expression);
        }
    };
    JDatacontextExpressionBinder.prototype.dispose = function () {
        _super.prototype.dispose.call(this);
        this.disposed = true;
        this.configs = [];
    };
    JDatacontextExpressionBinder.prototype.getRegexp = function () {
        return /\{1\}\.\@([\w|\.]+)/;
    };
    return JDatacontextExpressionBinder;
}(JBinder));
var JControlExpressionBinder = (function (_super) {
    __extends(JControlExpressionBinder, _super);
    function JControlExpressionBinder(control) {
        return _super.call(this, control) || this;
    }
    JControlExpressionBinder.prototype.getDatacontext = function () {
        var element;
        if (this.control instanceof JControl) {
            element = this.control.element.parentElement;
        }
        else {
            element = this.control;
        }
        var data;
        var parent = this.control;
        while (parent) {
            if (parent.JControl) {
                data = parent.JControl;
                this.rootControl = data;
                break;
            }
            else {
                parent = parent.parentElement;
            }
        }
        return data;
    };
    JControlExpressionBinder.prototype.getRegexp = function () {
        return /\{1\}\.\$([\w|\.]+)/;
    };
    return JControlExpressionBinder;
}(JDatacontextExpressionBinder));
window.bindElement = function (element, datacontext) {
    for (var i = 0; i < element.children.length; i++) {
        var node = element.children[i];
        if (node.JControl) {
            if (!node.JControl.datacontext) {
                node.JControl.datacontext = datacontext;
            }
            continue;
        }
        else {
            window.bindElement(node, datacontext);
        }
    }
};
