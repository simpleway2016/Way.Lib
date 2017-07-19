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
window.onerror = function (errorMessage, scriptURI, lineNumber) {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
};
var JElementHelper = (function () {
    function JElementHelper() {
    }
    JElementHelper.replaceElement = function (source, dst) {
        if (dst == dst.parentElement.children[dst.parentElement.children.length - 1]) {
            var parent = dst.parentElement;
            parent.removeChild(dst);
            parent.appendChild(source);
        }
        else {
            var nextlib = dst.nextElementSibling;
            var parent = dst.parentElement;
            parent.removeChild(dst);
            parent.insertBefore(source, nextlib);
        }
    };
    JElementHelper.getControlTypeName = function (tagname) {
        for (var name in window) {
            if (name.toUpperCase() == tagname) {
                return name;
            }
        }
        return null;
    };
    JElementHelper.getElement = function (html) {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return div.children[0];
    };
    JElementHelper.initElements = function (container) {
        if (!container || !container.children)
            return;
        for (var i = 0; i < container.children.length; i++) {
            var child = container.children[i];
            var classType = JElementHelper.getControlTypeName(child.tagName);
            if (classType) {
                eval("new " + classType + "(child)");
            }
            else {
                JElementHelper.initElements(child);
            }
        }
    };
    return JElementHelper;
}());
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
var JControlDataBinder = (function () {
    function JControlDataBinder(data, jcontrol, databind_exp, expression_exp) {
        var _this = this;
        this.configs = [];
        this.expressionConfigs = [];
        this.datacontext = data;
        this.control = jcontrol;
        this.expresion_replace_reg = expression_exp;
        var databind = jcontrol.databind;
        if (databind) {
            while (true) {
                var result = databind_exp.exec(databind);
                if (!result)
                    break;
                var elementPropertyName = result[1];
                var dataPropertyName = result[4];
                this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
                JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                databind = databind.substr(result.index + result[0].length);
            }
        }
        for (var attname in jcontrol) {
            if (attname == "expression" || /expression[0-9]+/.exec(attname)) {
                var expressionStr = jcontrol[attname];
                if (expressionStr) {
                    while (true) {
                        var result = expression_exp.exec(expressionStr);
                        if (!result)
                            break;
                        var dataPropertyName = result[1];
                        this.expressionConfigs.push(new JBindExpression(dataPropertyName, jcontrol[attname]));
                        JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                        expressionStr = expressionStr.substr(result.index + result[0].length);
                    }
                }
            }
        }
        if (this.expressionConfigs.length > 0) {
            this.expressionPropertyChangedListenerIndex = this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onExpressionPropertyChanged(s, n, o); });
            this.control.addPropertyChangedListener(function (s, n, o) { return _this.onExpressionPropertyChanged(s, n, o); });
        }
        if (this.configs.length > 0) {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onPropertyChanged(s, n, o); });
            this.control.addPropertyChangedListener(function (s, n, o) { return _this.onControlPropertyChanged(s, n, o); });
        }
        this.updateValue();
    }
    JControlDataBinder.prototype.dispose = function () {
        if (this.propertyChangedListenerIndex) {
            this.datacontext.removeListener(this.propertyChangedListenerIndex);
            this.propertyChangedListenerIndex = 0;
        }
        if (this.expressionPropertyChangedListenerIndex) {
            this.datacontext.removeListener(this.expressionPropertyChangedListenerIndex);
            this.expressionPropertyChangedListenerIndex = 0;
        }
        this.configs = [];
    };
    JControlDataBinder.prototype.getConfigByDataProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JControlDataBinder.prototype.getConfigByElementProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    };
    JControlDataBinder.prototype.getExpressionConfigByDataProName = function (proname) {
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var config = this.expressionConfigs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JControlDataBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        if (this.configs.length == 0)
            return;
        if (!this.control.element.parentElement) {
            this.dispose();
            return;
        }
        try {
            var self = this;
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("self.control." + config.elementPropertyName + " = self.datacontext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    };
    JControlDataBinder.prototype.onControlPropertyChanged = function (sender, name, originalValue) {
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
    JControlDataBinder.prototype.onExpressionPropertyChanged = function (sender, name, originalValue) {
        try {
            var config = this.getExpressionConfigByDataProName(name);
            if (config) {
                var element = this.control;
                var data = this.datacontext;
                var r;
                var expression = config.expression.replace(/\{0\}\./g, "element.");
                while (r = this.expresion_replace_reg.exec(expression)) {
                    expression = expression.replace(r[0], "data." + r[1]);
                }
                eval(expression);
            }
        }
        catch (e) {
        }
    };
    JControlDataBinder.prototype.updateValue = function () {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                var value;
                eval("value=this.datacontext." + config.dataPropertyName);
                if (value) {
                    eval("this.control." + config.elementPropertyName + " = value");
                }
            }
            catch (e) {
            }
        }
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var exconfig = this.expressionConfigs[i];
            if (exconfig) {
                var element = this.control;
                var data = this.datacontext;
                var r;
                var expression = exconfig.expression.replace(/\{0\}\./g, "element.");
                while (r = this.expresion_replace_reg.exec(expression)) {
                    expression = expression.replace(r[0], "data." + r[1]);
                }
                eval(expression);
            }
        }
    };
    return JControlDataBinder;
}());
var JChildrenElementBinder = (function () {
    function JChildrenElementBinder(data, element, forDataContext, bindmyselft) {
        var _this = this;
        this.configs = [];
        this.expressionConfigs = [];
        this.children = [];
        this.disposed = false;
        var databind_exp = /([\w|\.]+)( )?=( )?\@([\w|\.]+)/;
        var expression_exp = /\{1\}.\@([\w|\.]+)/;
        if (!forDataContext) {
            databind_exp = /([\w|\.]+)( )?=( )?\$([\w|\.]+)/;
            expression_exp = /\{1\}.\$([\w|\.]+)/;
        }
        this.datacontext = data;
        this.expresion_replace_reg = expression_exp;
        this.element = element;
        var databind = element.getAttribute("databind");
        for (var i = 0; i < element.attributes.length; i++) {
            var r;
            if (!forDataContext && (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(element.attributes[i].value))) {
                if (!databind)
                    databind = "";
                var attname = element.attributes[i].name;
                if (attname == "class")
                    attname = "className";
                element.attributes[i].value = "";
                databind += ";" + attname + "=$" + r[1];
            }
            else if (forDataContext && (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(element.attributes[i].value))) {
                if (!databind)
                    databind = "";
                var attname = element.attributes[i].name;
                if (attname == "class")
                    attname = "className";
                element.attributes[i].value = "";
                databind += ";" + attname + "=@" + r[1];
            }
        }
        if (databind) {
            while (true) {
                var result = databind_exp.exec(databind);
                if (!result)
                    break;
                var elementPropertyName = result[1];
                var dataPropertyName = result[4];
                this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
                JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                databind = databind.substr(result.index + result[0].length);
            }
        }
        var allattributes = element.attributes;
        for (var i = 0; i < allattributes.length; i++) {
            if (allattributes[i].name == "expression" || /expression([0-9]+)/.exec(allattributes[i].name)) {
                var expressionStr = allattributes[i].value;
                if (expressionStr) {
                    while (true) {
                        var result = expression_exp.exec(expressionStr);
                        if (!result)
                            break;
                        var dataPropertyName = result[1];
                        this.expressionConfigs.push(new JBindExpression(dataPropertyName, allattributes[i].value));
                        JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                        expressionStr = expressionStr.substr(result.index + result[0].length);
                    }
                }
            }
        }
        if (this.configs.length > 0) {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onPropertyChanged(s, n, o); });
        }
        if (this.expressionConfigs.length > 0) {
            this.propertyExpressionChangedListenerIndex = this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onExpressionPropertyChanged(s, n, o); });
        }
        this.init();
        if (bindmyselft || !element.JControl) {
            for (var i = 0; i < element.children.length; i++) {
                var child = element.children[i];
                if (child.tagName != "SCRIPT") {
                    if (!child.JControl) {
                        child._templateBinder = new JChildrenElementBinder(data, child, forDataContext, false);
                        this.children.push(child._templateBinder);
                    }
                }
            }
        }
    }
    JChildrenElementBinder.addPropertyIfNotExist = function (data, propertyName) {
        if (data instanceof JObserveObject) {
            var observeData = data;
            if (observeData.hasProperty(propertyName) == false) {
                observeData.addProperty(propertyName);
            }
        }
    };
    JChildrenElementBinder.prototype.dispose = function () {
        this.disposed = true;
        this.datacontext.removeListener(this.propertyChangedListenerIndex);
        this.datacontext.removeListener(this.propertyExpressionChangedListenerIndex);
        this.configs = [];
        this.expressionConfigs = [];
        for (var i = 0; i < this.children.length; i++) {
            this.children[i].dispose();
        }
    };
    JChildrenElementBinder.prototype.getConfigByDataProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JChildrenElementBinder.prototype.getExpressionConfigByDataProName = function (proname) {
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var config = this.expressionConfigs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JChildrenElementBinder.prototype.getConfigByElementProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    };
    JChildrenElementBinder.prototype.onExpressionPropertyChanged = function (sender, name, originalValue) {
        if (this.disposed)
            return;
        try {
            var config = this.getExpressionConfigByDataProName(name);
            if (config) {
                var element = this.element;
                var data = this.datacontext;
                var r;
                var expression = config.expression.replace(/\{0\}\./g, "element.");
                while (r = this.expresion_replace_reg.exec(expression)) {
                    expression = expression.replace(r[0], "data." + r[1]);
                }
                eval(expression);
            }
        }
        catch (e) {
        }
    };
    JChildrenElementBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
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
    JChildrenElementBinder.prototype.updateValue = function () {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config) {
                try {
                    var value;
                    eval("value=this.datacontext." + config.dataPropertyName);
                    if (value) {
                        eval("this.element." + config.elementPropertyName + "=value");
                    }
                }
                catch (e) {
                }
            }
        }
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var exconfig = this.expressionConfigs[i];
            if (exconfig) {
                var element = this.element;
                var data = this.datacontext;
                var r;
                var expression = exconfig.expression.replace(/\{0\}\./g, "element.");
                while (r = this.expresion_replace_reg.exec(expression)) {
                    expression = expression.replace(r[0], "data." + r[1]);
                }
                eval(expression);
            }
        }
    };
    JChildrenElementBinder.prototype.init = function () {
        this.updateValue();
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                if (config.elementPropertyName == "value" || config.elementPropertyName == "checked") {
                    this.element.addEventListener("change", this.listenElementEvent(this, config), false);
                }
            }
            catch (e) {
            }
        }
    };
    JChildrenElementBinder.prototype.listenElementEvent = function (self, config) {
        return function () {
            try {
                eval("self.datacontext." + config.dataPropertyName + " = self.element." + config.elementPropertyName);
            }
            catch (e) {
            }
        };
    };
    return JChildrenElementBinder;
}());
var JControl = (function () {
    function JControl(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = this;
        this.onPropertyChangeds = [];
        this.templates = [];
        this.templateMatchProNames = [];
        this.originalElement = element;
        if (templates) {
            for (var i = 0; i < templates.length; i++) {
                this.templates.push(templates[i]);
            }
        }
        else {
            this.loadTemplates();
        }
        this.databind = this.originalElement.getAttribute("databind");
        for (var i = 0; i < this.originalElement.attributes.length; i++) {
            var attName = this.originalElement.attributes[i].name;
            if (attName == "databind") {
            }
            else if (attName == "datacontext") {
            }
            else {
                var finded = false;
                for (var myproname in this) {
                    if (myproname.toLowerCase() == attName) {
                        finded = true;
                        var r;
                        if (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(this.originalElement.attributes[i].value)) {
                            if (!this.databind)
                                this.databind = "";
                            this.databind += ";" + myproname + "=$" + r[1];
                        }
                        else if (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(this.originalElement.attributes[i].value)) {
                            if (!this.databind)
                                this.databind = "";
                            this.databind += ";" + myproname + "=@" + r[1];
                        }
                        else {
                            this[myproname] = this.originalElement.attributes[i].value;
                        }
                        break;
                    }
                }
                if (!finded) {
                    eval("this._" + attName + "=this.originalElement.attributes[i].value");
                    Object.defineProperty(this, attName, {
                        get: this.getFunc(attName),
                        set: this.setFunc(attName),
                        enumerable: true,
                        configurable: true
                    });
                }
            }
        }
        if (this.originalElement.getAttribute("datacontext") && this.originalElement.getAttribute("datacontext").length > 0) {
            this.datacontext = this.originalElement.getAttribute("datacontext");
        }
        else {
            this.datacontext = datacontext;
        }
        this.checkDataContextPropertyExist();
        this.reApplyTemplate(this.originalElement);
        this.addPropertyChangedListener(function (s, name, value) {
            for (var i = 0; i < _this.templateMatchProNames.length; i++) {
                if (_this.templateMatchProNames[i] == "$" + name) {
                    _this.reApplyTemplate(_this.element);
                    break;
                }
            }
        });
    }
    JControl.prototype.addPropertyChangedListener = function (func) {
        this.onPropertyChangeds.push(func);
        return this.onPropertyChangeds.length - 1;
    };
    JControl.prototype.removeListener = function (index) {
        this.onPropertyChangeds[index] = null;
    };
    JControl.prototype.onPropertyChanged = function (proName, originalValue) {
        for (var i = 0; i < this.onPropertyChangeds.length; i++) {
            if (this.onPropertyChangeds[i]) {
                this.onPropertyChangeds[i](this, proName, originalValue);
            }
        }
    };
    Object.defineProperty(JControl.prototype, "datacontext", {
        get: function () {
            return this._datacontext;
        },
        set: function (value) {
            var _this = this;
            if (value && typeof value == "string") {
                try {
                    eval("value=" + value);
                }
                catch (e) {
                    return;
                }
            }
            if (value && !value.addPropertyChangedListener) {
                value = new JObserveObject(value);
                value.addPropertyChangedListener(function (s, name, value) {
                    for (var i = 0; i < _this.templateMatchProNames.length; i++) {
                        if (_this.templateMatchProNames[i] == "@" + name) {
                            _this.reApplyTemplate(_this.element);
                            break;
                        }
                    }
                });
            }
            if (this._datacontext != value) {
                var original = this._datacontext;
                this._datacontext = value;
                if (this.controlDataBinder) {
                    this.controlDataBinder.dispose();
                    this.controlDataBinder = null;
                }
                if (this.dataBinder) {
                    this.dataBinder.dispose();
                    this.dataBinder = null;
                }
                if (value) {
                    if (this.element) {
                        this.dataBinder = new JChildrenElementBinder(value, this.element, true, true);
                    }
                    if (value) {
                        this.controlDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, /\{1\}.\@([\w|\.]+)/);
                    }
                    if (this.element) {
                        this.setChildrenDataContext(this.element, value);
                    }
                    this.checkDataContextPropertyExist();
                }
                this.onPropertyChanged("datacontext", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JControl.prototype, "parentJControl", {
        get: function () {
            return this._parentJControl;
        },
        set: function (value) {
            if (this._parentJControl != value) {
                this._parentJControl = value;
                if (this.containerDataBinder) {
                    this.containerDataBinder.dispose();
                    this.containerDataBinder = null;
                }
                this.containerDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\$([\w|\.]+)/, /\{1\}.\$([\w|\.]+)/);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JControl.prototype, "id", {
        get: function () {
            return this._id;
        },
        set: function (value) {
            if (this._id != value) {
                var original = this._id;
                if (original) {
                    eval("if(window." + original + "==this) {window." + original + "=undefined;}");
                }
                this._id = value;
                eval("window." + value + "=this");
                this.onPropertyChanged("id", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JControl.prototype, "onclick", {
        get: function () {
            return this._onclick;
        },
        set: function (_value) {
            var value;
            if (_value && typeof _value == "string") {
                if (_value.length > 0) {
                    value = function () { eval(_value); };
                }
                else {
                    value = null;
                }
            }
            else {
                value = _value;
            }
            if (value != this._onclick) {
                var originalValue = this._onclick;
                this._onclick = value;
                this.addEventListener("click", value, false);
            }
        },
        enumerable: true,
        configurable: true
    });
    JControl.prototype.getFunc = function (name) {
        return function () {
            return this["_" + name];
        };
    };
    JControl.prototype.setFunc = function (attName) {
        return function (value) {
            if (this["_" + attName] != value) {
                var oldvalue = this["_" + attName];
                this["_" + attName] = value;
                this.onPropertyChanged(attName, oldvalue);
            }
        };
    };
    JControl.prototype.dispose = function () {
        if (this.controlDataBinder) {
            this.controlDataBinder.dispose();
            this.controlDataBinder = null;
        }
        if (this.containerDataBinder) {
            this.containerDataBinder.dispose();
            this.containerDataBinder = null;
        }
        if (this.dataBinder) {
            this.dataBinder.dispose();
            this.dataBinder = null;
        }
        if (this.templateBinder) {
            this.templateBinder.dispose();
            this.templateBinder = null;
        }
    };
    JControl.prototype.addEventListener = function (type, listener, useCapture) {
        if (this.element && listener) {
            this.element.addEventListener(type, listener, useCapture);
        }
    };
    JControl.prototype.removeEventListener = function (type, listener) {
        if (this.element && listener) {
            this.element.removeEventListener(type, listener);
        }
    };
    JControl.prototype.loadTemplates = function () {
        var alltemplates = this.originalElement.querySelectorAll("script");
        var keyTemplate;
        if (alltemplates.length > 0) {
        }
        else if (this.originalElement.getAttribute("template") && this.originalElement.getAttribute("template").length > 0
            && (keyTemplate = document.querySelector("#" + this.originalElement.getAttribute("template")))) {
            alltemplates = keyTemplate.querySelectorAll("script");
        }
        else {
            var typename = this.constructor.name;
            alltemplates = JElementHelper.SystemTemplateContainer.querySelector(typename).children;
        }
        for (var i = 0; i < alltemplates.length; i++) {
            this.templates.push(alltemplates[i]);
            var match = alltemplates[i].getAttribute("match");
            if (match) {
                var reg = /\@[\w|\.]+/;
                while (true) {
                    var result = reg.exec(match);
                    if (!result)
                        break;
                    var name = result[0];
                    this.templateMatchProNames.push(name);
                    match = match.substr(result.index + result[0].length);
                }
                match = alltemplates[i].getAttribute("match");
                reg = /\$[\w|\.]+/;
                while (true) {
                    var result = reg.exec(match);
                    if (!result)
                        break;
                    var name = result[0];
                    this.templateMatchProNames.push(name);
                    match = match.substr(result.index + result[0].length);
                }
            }
        }
    };
    JControl.prototype.checkDataContextPropertyExist = function () {
        if (this.datacontext && this.datacontext instanceof JObserveObject) {
            var ob = this.datacontext;
            for (var i = 0; i < this.templateMatchProNames.length; i++) {
                if (this.templateMatchProNames[i].indexOf("@") == 0) {
                    var name = this.templateMatchProNames[i].substr(1);
                    if (!ob.hasProperty(name)) {
                        ob.addProperty(name);
                    }
                }
            }
        }
    };
    JControl.prototype.reApplyTemplate = function (rootElement) {
        var template = this.getTemplate();
        if (template && template != this.currentTemplate) {
            this.currentTemplate = template;
            var html = template.innerHTML;
            var reg = /\{\@([\w|\.]+)\}/;
            if (reg) {
                var result;
                while (result = reg.exec(html)) {
                    var name = result[1];
                    var value = "";
                    if (this.datacontext) {
                        try {
                            eval("value=this.datacontext." + name);
                        }
                        catch (e) { }
                        if (typeof (value) == "undefined")
                            value = "";
                    }
                    html = html.replace(result[0], value);
                }
                reg = /\{\$([\w|\.]+)\}/;
                while (result = reg.exec(html)) {
                    var name = result[1];
                    var value = "";
                    try {
                        eval("value=this." + name);
                    }
                    catch (e) { }
                    if (typeof (value) == "undefined")
                        value = "";
                    html = html.replace(result[0], value);
                }
            }
            this.element = JElementHelper.getElement(html);
            if (JElementHelper.getControlTypeName(this.element.tagName)) {
                throw new Error("不能把JControl作为模板的首个元素");
            }
            this.element.JControl = this;
            JElementHelper.replaceElement(this.element, rootElement);
            if (true) {
                var parent = this.element.parentElement;
                while (parent) {
                    if (parent.JControl) {
                        this.parentJControl = parent.JControl;
                        break;
                    }
                    else {
                        parent = parent.parentElement;
                    }
                }
            }
            if (this.onclick) {
                this.addEventListener("click", this.onclick, false);
            }
            if (!this.datacontext) {
                var parent = this.element.parentElement;
                while (parent && !this.datacontext) {
                    if (parent.JControl) {
                        this.datacontext = parent.JControl.datacontext;
                        break;
                    }
                    else {
                        parent = parent.parentElement;
                    }
                }
            }
            if (this.templateBinder) {
                this.templateBinder.dispose();
                this.templateBinder = null;
            }
            if (this.dataBinder) {
                this.dataBinder.dispose();
                this.dataBinder = null;
            }
            JElementHelper.initElements(this.element);
            this.templateBinder = new JChildrenElementBinder(this, this.element, false, true);
            if (this.datacontext) {
                this.dataBinder = new JChildrenElementBinder(this.datacontext, this.element, true, true);
            }
            this.onTemplateApply();
        }
    };
    JControl.prototype.onTemplateApply = function () {
    };
    JControl.prototype.setChildrenDataContext = function (element, datacontext) {
        for (var i = 0; i < element.children.length; i++) {
            var child = element.children[i];
            if (child.JControl) {
                if (!child.JControl.datacontext) {
                    child.JControl.datacontext = datacontext;
                }
            }
            else {
                this.setChildrenDataContext(child, datacontext);
            }
        }
    };
    JControl.prototype.getTemplate = function () {
        if (this.templates.length == 0)
            return null;
        var result = null;
        for (var i = 0; i < this.templates.length; i++) {
            if (this.templates[i]) {
                var match = this.templates[i].getAttribute("match");
                if (!match || match.length == 0) {
                    result = this.templates[i];
                }
                else {
                    var reg = /\@([\w|\.]+)/;
                    while (true) {
                        var r = reg.exec(match);
                        if (!r)
                            break;
                        var name = r[1];
                        match = match.replace(r[0], "this.datacontext." + name);
                    }
                    reg = /\$([\w|\.]+)/;
                    while (true) {
                        var r = reg.exec(match);
                        if (!r)
                            break;
                        var name = r[1];
                        match = match.replace(r[0], "this." + name);
                    }
                    var execResult = false;
                    try {
                        eval("execResult=(" + match + ")");
                    }
                    catch (e) { }
                    if (execResult) {
                        return this.templates[i];
                    }
                }
            }
        }
        return result;
    };
    return JControl;
}());
var JButton = (function (_super) {
    __extends(JButton, _super);
    function JButton(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this._text = "";
        return _this;
    }
    Object.defineProperty(JButton.prototype, "text", {
        get: function () {
            return this._text;
        },
        set: function (value) {
            if (value != this._text) {
                var originalValue = this._text;
                this._text = value;
                this.onPropertyChanged("text", originalValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    return JButton;
}(JControl));
var JTextbox = (function (_super) {
    __extends(JTextbox, _super);
    function JTextbox(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        return _super.call(this, element, templates, datacontext) || this;
    }
    return JTextbox;
}(JButton));
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
var JListItem = (function (_super) {
    __extends(JListItem, _super);
    function JListItem(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this.id = JListItem.StaticString + JListItem.StaticID++;
        if (JListItem.StaticID >= 100000) {
            JListItem.StaticString += "E_";
            JListItem.StaticID = 1;
        }
        _this.onPropertyChanged("id", undefined);
        return _this;
    }
    Object.defineProperty(JListItem.prototype, "valuemember", {
        get: function () {
            return this._valueMember;
        },
        set: function (value) {
            if (value != this._valueMember) {
                var original = this._valueMember;
                this._valueMember = value;
                if (this.datacontext) {
                    var self = this;
                    eval("self.value=self.datacontext." + value);
                }
                this.onPropertyChanged("valuemember", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JListItem.prototype, "textmember", {
        get: function () {
            return this._textMember;
        },
        set: function (value) {
            if (value != this._textMember) {
                var original = this._textMember;
                this._textMember = value;
                if (this.datacontext) {
                    var self = this;
                    eval("self.text=self.datacontext." + value);
                }
                this.onPropertyChanged("textmember", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JListItem.prototype, "index", {
        get: function () {
            return this._index;
        },
        set: function (value) {
            if (value != this._index) {
                var original = this._index;
                this._index = value;
                this.onPropertyChanged("index", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JListItem.prototype, "text", {
        get: function () {
            return this._text;
        },
        set: function (value) {
            if (value != this._text) {
                var original = this._text;
                this._text = value;
                this.onPropertyChanged("text", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JListItem.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (value) {
            if (value != this._value) {
                var original = this._value;
                this._value = value;
                this.onPropertyChanged("value", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    return JListItem;
}(JControl));
JListItem.StaticID = 1;
JListItem.StaticString = "JListItem_";
var JList = (function (_super) {
    __extends(JList, _super);
    function JList(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        return _super.call(this, element, templates, datacontext) || this;
    }
    Object.defineProperty(JList.prototype, "itemsource", {
        get: function () {
            return this._itemsource;
        },
        set: function (value) {
            if (value && typeof value == "string") {
                try {
                    eval("value=" + value);
                }
                catch (e) {
                    return;
                }
            }
            if (value instanceof Array) {
                for (var i = 0; i < value.length; i++) {
                    if (value[i] && !(value[i] instanceof JObserveObject)) {
                        value[i] = new JObserveObject(value[i]);
                    }
                }
            }
            else if (value instanceof JDataSource) {
                if (this.itemContainer) {
                    this.itemContainer.innerHTML = "";
                }
                this._itemsource = value;
                this.bindItems();
                return;
            }
            else {
                throw new Error("itemsource必须是数组或者JDataSource");
            }
            if (this.itemContainer) {
                this.itemContainer.innerHTML = "";
            }
            this._itemsource = new JDataSource(value);
            this.bindItems();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JList.prototype, "valuemember", {
        get: function () {
            return this._valueMember;
        },
        set: function (value) {
            if (value != this._valueMember) {
                var original = this._valueMember;
                this._valueMember = value;
                if (this.itemControls) {
                    for (var i = 0; i < this.itemControls.length; i++) {
                        if (this.itemControls[i]) {
                            this.itemControls[i].valuemember = value;
                        }
                    }
                }
                this.onPropertyChanged("valuemember", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(JList.prototype, "textmember", {
        get: function () {
            return this._textMember;
        },
        set: function (value) {
            if (value != this._textMember) {
                var original = this._textMember;
                this._textMember = value;
                if (this.itemControls) {
                    for (var i = 0; i < this.itemControls.length; i++) {
                        if (this.itemControls[i]) {
                            this.itemControls[i].textmember = value;
                        }
                    }
                }
                this.onPropertyChanged("textmember", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    JList.prototype.loadTemplates = function () {
        _super.prototype.loadTemplates.call(this);
        this.itemTemplates = [];
        for (var i = 0; i < this.templates.length; i++) {
            if (this.templates[i].getAttribute("for") == "item") {
                this.itemTemplates.push(this.templates[i]);
                this.templates[i] = null;
            }
        }
    };
    JList.prototype.onTemplateApply = function () {
        _super.prototype.onTemplateApply.call(this);
        this.itemContainer = this.element.id == "itemContainer" ? this.element : this.element.querySelector("*[id='itemContainer']");
        this.bindItems();
    };
    JList.prototype.bindItems = function () {
        var _this = this;
        if (!this.itemContainer)
            return;
        this.itemControls = [];
        for (var i = 0; i < this.itemsource.source.length; i++) {
            var item = this.addItem(this.itemsource.source[i]);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
        }
        this.resetItemIndex();
        this.itemsource.addEventListener("add", function (sender, data, index) {
            var item = _this.addItem(data);
            item.textmember = _this.textmember;
            item.valuemember = _this.valuemember;
            _this.resetItemIndex();
        });
        this.itemsource.addEventListener("remove", function (sender, data, index) {
            for (var i = 0; i < _this.itemControls.length; i++) {
                if (_this.itemControls[i] && _this.itemControls[i].datacontext == data) {
                    _this.itemContainer.removeChild(_this.itemControls[i].element);
                    _this.itemControls[i].dispose();
                    _this.itemControls.splice(i, 1);
                    break;
                }
            }
            _this.resetItemIndex();
        });
    };
    JList.prototype.resetItemIndex = function () {
        var index = 0;
        for (var i = 0; i < this.itemControls.length; i++) {
            if (this.itemControls[i]) {
                this.itemControls[i].index = index;
                index++;
            }
        }
    };
    JList.prototype.addItem = function (data) {
        var div = document.createElement("DIV");
        this.itemContainer.appendChild(div);
        var jcontrol = new JListItem(div, this.itemTemplates, data);
        this.itemControls.push(jcontrol);
        return jcontrol;
    };
    return JList;
}(JControl));
var JCheckboxList = (function (_super) {
    __extends(JCheckboxList, _super);
    function JCheckboxList(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this._checkedvalue = [];
        return _this;
    }
    Object.defineProperty(JCheckboxList.prototype, "checkedvalue", {
        get: function () {
            return this._checkedvalue;
        },
        set: function (value) {
            var _this = this;
            if (value != this._checkedvalue) {
                var original = this._checkedvalue;
                this._checkedvalue = value;
                if (this.valuemember && this.valuemember.length > 0) {
                    this.itemControls.forEach(function (control, index, array) {
                        var itemvalue;
                        eval("itemvalue=control.datacontext." + _this.valuemember);
                        if (_this._checkedvalue.indexOf(itemvalue) >= 0) {
                            control.datacontext.checked = true;
                        }
                        else {
                            control.datacontext.checked = false;
                        }
                    });
                }
                this.onPropertyChanged("checkedvalue", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    JCheckboxList.prototype.addItem = function (data) {
        var _this = this;
        var item = _super.prototype.addItem.call(this, data);
        if (typeof data.checked == "undefined")
            data.checked = false;
        if (data instanceof JObserveObject) {
            data.addPropertyChangedListener(function (s, name, o) { _this.onItemDataChanged(s, name, o); });
        }
        return item;
    };
    JCheckboxList.prototype.onItemDataChanged = function (sender, name, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0) {
            for (var i = 0; i < this.itemControls.length; i++) {
                if (this.itemControls[i] && this.itemControls[i].datacontext == sender) {
                    var original = this.checkedvalue;
                    if (sender[name]) {
                        var self = this;
                        eval("self.checkedvalue.push(sender." + this.valuemember + ")");
                    }
                    else {
                        var value;
                        eval("value = sender." + this.valuemember);
                        var index = this.checkedvalue.indexOf(value);
                        if (index >= 0) {
                            this.checkedvalue.splice(index, 1);
                        }
                    }
                    this.onPropertyChanged("checkedvalue", original);
                    break;
                }
            }
        }
    };
    return JCheckboxList;
}(JList));
var JRadioList = (function (_super) {
    __extends(JRadioList, _super);
    function JRadioList(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this._checkedvalue = null;
        return _this;
    }
    Object.defineProperty(JRadioList.prototype, "checkedvalue", {
        get: function () {
            return this._checkedvalue;
        },
        set: function (value) {
            var _this = this;
            if (value != this._checkedvalue) {
                var original = this._checkedvalue;
                this._checkedvalue = value;
                if (this.valuemember && this.valuemember.length > 0) {
                    this.itemControls.forEach(function (control, index, array) {
                        var itemvalue;
                        eval("itemvalue=control.datacontext." + _this.valuemember);
                        if (itemvalue == value) {
                            control.datacontext.checked = true;
                        }
                        else {
                            control.datacontext.checked = false;
                        }
                    });
                }
                this.onPropertyChanged("checkedvalue", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    JRadioList.prototype.addItem = function (data) {
        var _this = this;
        if (!this.itemid)
            this.itemid = (JRadioList.StaticID++);
        var item = _super.prototype.addItem.call(this, data);
        item.name = "JRadioListItem_" + this.itemid;
        if (typeof data.checked == "undefined")
            data.checked = false;
        if (data instanceof JObserveObject) {
            data.addPropertyChangedListener(function (s, name, o) { _this.onItemDataChanged(s, name, o); });
        }
        return item;
    };
    JRadioList.prototype.onItemDataChanged = function (sender, name, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0) {
            for (var i = 0; i < this.itemControls.length; i++) {
                if (this.itemControls[i]) {
                    if (this.itemControls[i].datacontext["checked"]) {
                        var value;
                        var data = this.itemControls[i].datacontext;
                        eval("value = data." + this.valuemember);
                        this.checkedvalue = value;
                        return;
                    }
                }
            }
            this.checkedvalue = null;
        }
    };
    return JRadioList;
}(JList));
JRadioList.StaticID = 1;
var JDropdownList = (function (_super) {
    __extends(JDropdownList, _super);
    function JDropdownList(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this._selectedvalue = null;
        return _this;
    }
    Object.defineProperty(JDropdownList.prototype, "selectedvalue", {
        get: function () {
            return this._selectedvalue;
        },
        set: function (value) {
            if (value != this._selectedvalue) {
                var original = this._selectedvalue;
                this._selectedvalue = value;
                this.onPropertyChanged("selectedvalue", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    JDropdownList.prototype.bindItems = function () {
        this.selectedvalue = null;
        _super.prototype.bindItems.call(this);
        if (this.itemControls && this.valuemember && this.valuemember.length > 0 && this.itemControls.length > 0) {
            var data = this.itemControls[0].datacontext;
            var src = this;
            eval("src.selectedvalue = data." + this.valuemember);
        }
    };
    return JDropdownList;
}(JList));
if (document.addEventListener) {
    function removeElement(element) {
        if (!element.children)
            return;
        if (element.JControl) {
            element.JControl.dispose();
        }
        for (var i = 0; i < element.children.length; i++) {
            removeElement(element.children[i]);
        }
    }
    document.addEventListener('DOMContentLoaded', function () {
        var bodytemplate = document.body.getAttribute("template");
        if (!bodytemplate || bodytemplate.length == 0) {
            bodytemplate = "/templates/system.html";
        }
        var templateHtml = JHttpHelper.downloadUrl(bodytemplate);
        JElementHelper.SystemTemplateContainer = document.createElement("DIV");
        JElementHelper.SystemTemplateContainer.innerHTML = templateHtml;
        var style = JElementHelper.SystemTemplateContainer.querySelector("style");
        if (style) {
            document.head.appendChild(style);
        }
        JElementHelper.initElements(document.body);
        var MutationObserver = window.MutationObserver ||
            window.WebKitMutationObserver ||
            window.MozMutationObserver;
        var mutationObserverSupport = !!MutationObserver;
        if (mutationObserverSupport) {
            try {
                var options = {
                    'childList': true,
                    subtree: true,
                };
                var callback = function (records) {
                    records.map(function (record) {
                        if (record.addedNodes) {
                            for (var i = 0; i < record.addedNodes.length; i++) {
                                if (!record.addedNodes[i].JControl) {
                                    JElementHelper.initElements(record.addedNodes[i].parentElement);
                                }
                            }
                        }
                        if (record.removedNodes) {
                            for (var i = 0; i < record.removedNodes.length; i++) {
                                removeElement(record.removedNodes[i]);
                            }
                        }
                    });
                };
                var observer = new MutationObserver(callback);
                observer.observe(document.body, options);
            }
            catch (e) {
                alert(e.message);
            }
        }
    }, false);
}
//# sourceMappingURL=JControls.js.map