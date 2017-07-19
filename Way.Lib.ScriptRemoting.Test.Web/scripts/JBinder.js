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
    function JBinder(data, control) {
        this.datacontext = data;
        this.control = control;
    }
    JBinder.prototype.dispose = function () {
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
            var r;
            if (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(att.value)) {
                att.value = "";
                databind += ";" + att.name + "=$" + r[1];
            }
            else if (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(att.value)) {
                att.value = "";
                databind += ";" + att.name + "=@" + r[1];
            }
        }
        element.setAttribute("databind", databind);
    };
    return JBinder;
}());
var JDatacontextBinder = (function (_super) {
    __extends(JDatacontextBinder, _super);
    function JDatacontextBinder(data, control) {
        var _this = _super.call(this, data, control) || this;
        _this.configs = [];
        _this.disposed = false;
        _this.propertyChangedListenerIndex = 0;
        _this.controlPropertyChangedListenerIndex = 0;
        var databind;
        if (control instanceof JControl) {
            databind = control.databind;
        }
        else {
            JBinder.moveAttributeBindToDatabind(control);
            databind = control.getAttribute("databind");
        }
        if (!databind)
            return _this;
        var regexp = _this.getRegexp();
        while (true) {
            var result = regexp.exec(databind);
            if (!result)
                break;
            var elementPropertyName = result[1];
            var dataPropertyName = result[2];
            _this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
            JBinder.addPropertyIfNotExist(data, dataPropertyName);
            databind = databind.substr(result.index + result[0].length);
        }
        if (_this.configs.length > 0) {
            _this.propertyChangedListenerIndex = _this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onPropertyChanged(s, n, o); });
        }
        else if (control instanceof JControl) {
            _this.controlPropertyChangedListenerIndex =
                control.addPropertyChangedListener(function (s, n, o) { return _this.onControlPropertyChanged(s, n, o); });
        }
        else {
            var element = control;
            _this.configs.forEach(function (config) {
                if (config.elementPropertyName == "value" || config.elementPropertyName == "checked") {
                    element.addEventListener("change", _this.listenElementEvent(_this, config), false);
                }
            });
        }
        _this.bindChildren();
        return _this;
    }
    JDatacontextBinder.prototype.bindChildren = function () {
        if (this.control instanceof JControl) {
            var jcontrol = this.control;
            AllJBinders.push(new JDatacontextBinder(this.datacontext, jcontrol.element));
        }
        else {
            var element = this.control;
            for (var i = 0; i < element.children.length; i++) {
                AllJBinders.push(new JDatacontextBinder(this.datacontext, element.children[i]));
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
            try {
                eval("self.datacontext." + config.dataPropertyName + " = self.element." + config.elementPropertyName);
            }
            catch (e) {
            }
        };
    };
    JDatacontextBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        if (this.disposed)
            return;
        try {
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("this.element." + config.elementPropertyName + " = this.datacontext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    };
    JDatacontextBinder.prototype.onControlPropertyChanged = function (sender, name, originalValue) {
        try {
            var self = this;
            var config = this.getConfigByElementProName(name);
            if (config) {
                eval("self.datacontext." + config.dataPropertyName + " = self.control." + name);
            }
        }
        catch (e) {
        }
    };
    JDatacontextBinder.prototype.dispose = function () {
        _super.prototype.dispose.call(this);
        this.disposed = true;
        if (this.propertyChangedListenerIndex) {
            this.datacontext.removeListener(this.propertyChangedListenerIndex);
            this.propertyChangedListenerIndex = 0;
        }
        if (this.control instanceof JControl && this.controlPropertyChangedListenerIndex) {
            this.control.removeListener(this.controlPropertyChangedListenerIndex);
            this.controlPropertyChangedListenerIndex = 0;
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
    function JControlBinder(data, control) {
        return _super.call(this, data, control) || this;
    }
    JControlBinder.prototype.bindChildren = function () {
        if (this.control instanceof JControl) {
            var jcontrol = this.control;
            AllJBinders.push(new JControlBinder(jcontrol, jcontrol.element));
        }
        else {
            var element = this.control;
            for (var i = 0; i < element.children.length; i++) {
                AllJBinders.push(new JControlBinder(this.datacontext, element.children[i]));
            }
        }
    };
    JControlBinder.prototype.getRegexp = function () {
        return /([\w|\.]+)[ ]?=[ ]?\$([\w|\.]+)/;
    };
    return JControlBinder;
}(JDatacontextBinder));
//# sourceMappingURL=JBinder.js.map