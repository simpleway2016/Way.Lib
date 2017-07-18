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
    JElementHelper.getElement = function (html) {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return div.children[0];
    };
    JElementHelper.getJControlTypeName = function (tagName) {
        if (tagName == "JBUTTON")
            return "JButton";
        else if (tagName == "JLIST")
            return "JList";
        return false;
    };
    JElementHelper.initElements = function (container) {
        for (var i = 0; i < container.children.length; i++) {
            var child = container.children[i];
            var classType = JElementHelper.getJControlTypeName(child.tagName);
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
                this.onPropertyChanged(checkname, value);
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
    function JControlDataBinder(data, jcontrol, expression) {
        var _this = this;
        this.configs = [];
        this.datacontext = data;
        this.control = jcontrol;
        this.expression = expression;
        var databind = jcontrol.databind;
        if (databind) {
            while (true) {
                var result = this.expression.exec(databind);
                if (!result)
                    break;
                var elementPropertyName = result[1];
                var dataPropertyName = result[4];
                this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
                JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                databind = databind.substr(result.index + result[0].length);
            }
        }
        this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener(function (s, n, o) { return _this.onPropertyChanged(s, n, o); });
        this.control.addPropertyChangedListener(function (s, n, o) { return _this.onControlPropertyChanged(s, n, o); });
        this.updateValue();
    }
    JControlDataBinder.prototype.dispose = function () {
        this.datacontext.removeListener(this.propertyChangedListenerIndex);
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
    JControlDataBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        if (this.configs.length == 0)
            return;
        if (!this.control.element.parentElement) {
            this.dispose();
            return;
        }
        try {
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("this.control." + config.elementPropertyName + " = this.datacontext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    };
    JControlDataBinder.prototype.onControlPropertyChanged = function (sender, name, originalValue) {
        try {
            var config = this.getConfigByElementProName(name);
            if (config) {
                eval("this.datacontext." + config.dataPropertyName + " = this.control." + name);
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
    };
    return JControlDataBinder;
}());
var JChildrenElementBinder = (function () {
    function JChildrenElementBinder(data, element, databind_exp, bindmyselft) {
        var _this = this;
        this.configs = [];
        this.expressionConfigs = [];
        this.children = [];
        this.disposed = false;
        this.datacontext = data;
        this.element = element;
        var expression_exp = /\{1\}\.([\w|\.]+)/;
        var databind = element.getAttribute("databind");
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
        var expressionStr = element.getAttribute("expression");
        if (expressionStr) {
            while (true) {
                var result = expression_exp.exec(expressionStr);
                if (!result)
                    break;
                var dataPropertyName = result[1];
                this.expressionConfigs.push(new JBindExpression(dataPropertyName, element.getAttribute("expression")));
                JChildrenElementBinder.addPropertyIfNotExist(data, dataPropertyName);
                expressionStr = expressionStr.substr(result.index + result[0].length);
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
                    child._templateBinder = new JChildrenElementBinder(data, child, databind_exp, false);
                    this.children.push(child._templateBinder);
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
                eval(config.expression.replace(/\{0\}/g, "element").replace(/\{1\}/g, "data"));
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
                eval(exconfig.expression.replace(/\{0\}/g, "element").replace(/\{1\}/g, "data"));
            }
        }
    };
    JChildrenElementBinder.prototype.init = function () {
        var _this = this;
        this.updateValue();
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                if (config.elementPropertyName == "value") {
                    var src = this;
                    this.element.addEventListener("change", function () {
                        try {
                            var config = _this.getConfigByElementProName("value");
                            if (config) {
                                eval("src.datacontext." + config.dataPropertyName + " = src.element." + config.elementPropertyName);
                            }
                        }
                        catch (e) {
                        }
                    }, false);
                }
            }
            catch (e) {
            }
        }
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
        this.datacontext = datacontext;
        if (templates) {
            for (var i = 0; i < templates.length; i++) {
                this.templates.push(templates[i]);
            }
        }
        else {
            this.loadTemplates();
            this.checkDataContextPropertyExist();
        }
        this.databind = this.originalElement.getAttribute("databind");
        for (var i = 0; i < this.originalElement.attributes.length; i++) {
            var attName = this.originalElement.attributes[i].name;
            if (attName == "id") {
                eval("window." + this.originalElement.attributes[i].value + "=this");
            }
            else if (attName == "databind") {
            }
            else {
                var finded = false;
                for (var myproname in this) {
                    if (myproname.toLowerCase() == attName) {
                        finded = true;
                        this[myproname] = this.originalElement.attributes[i].value;
                    }
                }
                if (!finded) {
                    eval("this._" + attName + "=this.originalElement.attributes[i].value");
                    Object.defineProperty(this, attName, {
                        get: function () {
                            return this["_" + attName];
                        },
                        set: function (value) {
                            if (this["_" + attName] != value) {
                                var oldvalue = this["_" + attName];
                                this["_" + attName] = value;
                                this.onPropertyChanged(attName, oldvalue);
                            }
                        },
                        enumerable: true,
                        configurable: true
                    });
                }
            }
        }
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
                        this.dataBinder = new JChildrenElementBinder(value, this.element, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, true);
                    }
                    if (value) {
                        this.controlDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/);
                    }
                    if (this.element) {
                        this.setChildrenDataContext(this.element, value);
                    }
                    this.checkDataContextPropertyExist();
                }
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
    JControl.prototype.dispose = function () {
        if (this.controlDataBinder) {
            this.controlDataBinder.dispose();
            this.controlDataBinder = null;
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
        if (alltemplates.length > 0) {
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
            if (JElementHelper.getJControlTypeName(this.element.tagName)) {
                throw new Error("不能把JControl作为模板的首个元素");
            }
            JElementHelper.replaceElement(this.element, rootElement);
            this.element.JControl = this;
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
            this.templateBinder = new JChildrenElementBinder(this, this.element, /([\w|\.]+)( )?=( )?\$([\w|\.]+)/, true);
            if (this.datacontext) {
                this.dataBinder = new JChildrenElementBinder(this.datacontext, this.element, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, true);
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
    function JButton(element) {
        var _this = _super.call(this, element) || this;
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
        var len = this.source.length;
        for (var i = len - 1; i >= index; i--) {
            this.source[i + 1] = this.source[i];
        }
        this.source[index] = data;
        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    };
    JDataSource.prototype.remove = function (data) {
        for (var i = 0; i < this.source.length; i++) {
            if (this.source[i] == data) {
                this.removeAt(i);
                break;
            }
        }
    };
    JDataSource.prototype.removeAt = function (index) {
        if (index < this.source.length && index >= 0) {
            var data = this.source[index];
            for (var i = index; i < this.source.length - 1; i++) {
                this.source[i] = this.source[i + 1];
            }
            this.source.length--;
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    };
    return JDataSource;
}());
var JList = (function (_super) {
    __extends(JList, _super);
    function JList(element) {
        return _super.call(this, element) || this;
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
            this.addItem(this.itemsource.source[i]);
        }
        this.itemsource.addEventListener("add", function (sender, data, index) {
            _this.addItem(data);
        });
        this.itemsource.addEventListener("remove", function (sender, data, index) {
            for (var i = 0; i < _this.itemControls.length; i++) {
                if (_this.itemControls[i] && _this.itemControls[i].datacontext == data) {
                    _this.itemContainer.removeChild(_this.itemControls[i].element);
                    _this.itemControls[i].dispose();
                    _this.itemControls[i] = null;
                    break;
                }
            }
        });
    };
    JList.prototype.addItem = function (data) {
        var div = document.createElement("DIV");
        this.itemContainer.appendChild(div);
        var jcontrol = new JControl(div, this.itemTemplates, data);
        this.itemControls.push(jcontrol);
    };
    return JList;
}(JControl));
if (document.addEventListener) {
    var templateHtml = JHttpHelper.downloadUrl("/templates/system.html");
    JElementHelper.SystemTemplateContainer = document.createElement("DIV");
    JElementHelper.SystemTemplateContainer.innerHTML = templateHtml;
    var style = JElementHelper.SystemTemplateContainer.querySelector("style");
    if (style) {
        document.head.appendChild(style);
    }
    document.addEventListener('DOMContentLoaded', function () { JElementHelper.initElements(document.body); }, false);
}
//# sourceMappingURL=JControls.js.map