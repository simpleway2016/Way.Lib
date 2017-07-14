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
var JObserveObject = (function () {
    function JObserveObject(data, parent, parentname) {
        if (parent === void 0) { parent = null; }
        if (parentname === void 0) { parentname = null; }
        this.__onchanges = [];
        this.__objects = {};
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
    JObserveObject.prototype.addNewProperty = function (proName, value) {
        this.__data[proName] = value;
        var type = typeof value;
        if (value == null || value instanceof Array || type != "object") {
            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName, value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;
                            }
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    };
    JObserveObject.prototype.__addProperty = function (proName) {
        var type = typeof this.__data[proName];
        if (type == "object" && !(this.__data[proName] instanceof Array)) {
            this[proName] = new JObserveObject(this.__data[proName], this, proName);
        }
        else if (type != "function") {
            Object.defineProperty(this, proName, {
                get: function () {
                    return this.__data[proName];
                },
                set: function (value) {
                    if (this.__data[proName] != value) {
                        this.__data[proName] = value;
                        this.__changed(proName, value);
                        if (this.__parent) {
                            var curparent = this.__parent;
                            var pname = this.__parentName;
                            while (curparent) {
                                proName = pname + "." + proName;
                                curparent.__changed(proName, value);
                                pname = curparent.__parentName;
                                curparent = curparent.__parent;
                            }
                        }
                    }
                },
                enumerable: true,
                configurable: true
            });
        }
    };
    JObserveObject.prototype.addPropertyChangedListener = function (func) {
        this.__onchanges.push(func);
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
var JDataBinder = (function () {
    function JDataBinder(data, element, expression, bindmyselft) {
        var _this = this;
        this.expression = /(\w+)( )?=( )?\$(\w+)/;
        this.configs = [];
        this.dataContext = data;
        this.element = element;
        this.expression = expression;
        var databind = element.getAttribute("databind");
        if (databind) {
            while (true) {
                var result = this.expression.exec(databind);
                if (!result)
                    break;
                var elementPropertyName = result[1];
                var dataPropertyName = result[4];
                this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
                databind = databind.substr(result.index + result[0].length);
            }
        }
        this.dataContext.addPropertyChangedListener(function (s, n, o) { return _this.onPropertyChanged(s, n, o); });
        this.init();
        if (bindmyselft || !element._JControl) {
            for (var i = 0; i < element.children.length; i++) {
                var child = element.children[i];
                if (child.tagName != "SCRIPT") {
                    child._templateBinder = new JDataBinder(data, child, expression, false);
                }
            }
        }
    }
    JDataBinder.prototype.getConfigByDataProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    };
    JDataBinder.prototype.getConfigByElementProName = function (proname) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    };
    JDataBinder.prototype.onPropertyChanged = function (sender, name, originalValue) {
        try {
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("this.element." + config.elementPropertyName + " = this.dataContext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    };
    JDataBinder.prototype.updateValue = function () {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                eval("this.element." + config.elementPropertyName + " = this.dataContext." + config.dataPropertyName);
            }
            catch (e) {
            }
        }
    };
    JDataBinder.prototype.init = function () {
        var _this = this;
        this.updateValue();
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                if (config.elementPropertyName == "value") {
                    this.element.addEventListener("change", function () {
                        try {
                            var config = _this.getConfigByElementProName("value");
                            if (config) {
                                _this.dataContext[config.dataPropertyName] = _this.element[config.elementPropertyName];
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
    return JDataBinder;
}());
var JControl = (function () {
    function JControl(element) {
        this.onPropertyChangeds = [];
        for (var i = 0; i < element.attributes.length; i++) {
            var attName = element.attributes[i].name;
            if (attName == "id") {
                eval("window." + element.attributes[i].value + "=this");
            }
            else {
                for (var myproname in this) {
                    if (myproname.toLowerCase() == attName) {
                        this[myproname] = element.attributes[i].value;
                    }
                }
            }
        }
        var template = this.getTemplate(element, "Template");
        if (template) {
            this.element = JElementHelper.getElement(template.innerHTML);
            if (JElementHelper.getJControlTypeName(this.element.tagName)) {
                throw new Error("不能把JControl作为模板的首个元素");
            }
            JElementHelper.replaceElement(this.element, element);
            this.element._JControl = this;
            this.element.addEventListener("click", this.onclick, false);
            JElementHelper.initElements(this.element);
            this.templateBinder = new JDataBinder(this, this.element, /(\w+)( )?=( )?\$(\w+)/, true);
            if (this.dataContext) {
                this.dataBinder = new JDataBinder(this.dataContext, this.element, /(\w+)( )?=( )?\@(\w+)/, true);
            }
        }
    }
    JControl.prototype.addPropertyChangedListener = function (func) {
        this.onPropertyChangeds.push(func);
    };
    JControl.prototype.onPropertyChanged = function (proName, originalValue) {
        for (var i = 0; i < this.onPropertyChangeds.length; i++) {
            if (this.onPropertyChangeds[i]) {
                this.onPropertyChangeds[i](this, proName, originalValue);
            }
        }
    };
    Object.defineProperty(JControl.prototype, "dataContext", {
        get: function () {
            return this._dataContext;
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
            if (this._dataContext != value) {
                this._dataContext = value;
                if (this.dataBinder) {
                    this.dataBinder.dataContext = value;
                    this.dataBinder.updateValue();
                }
                else {
                    if (this.element) {
                        this.dataBinder = new JDataBinder(value, this.element, /(\w+)( )?=( )?\@(\w+)/, true);
                    }
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
                value = function () { eval(_value); };
            }
            else {
                value = _value;
            }
            if (value != this._onclick) {
                var originalValue = this._onclick;
                this._onclick = value;
                if (this.element) {
                    this.element.addEventListener("click", value, false);
                }
            }
        },
        enumerable: true,
        configurable: true
    });
    JControl.prototype.getTemplate = function (element, forwhat) {
        var template = element.querySelector("script[for='" + forwhat + "']");
        return template;
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
if (document.addEventListener) {
    document.addEventListener('DOMContentLoaded', function () { JElementHelper.initElements(document.body); }, false);
}
//# sourceMappingURL=JControls.js.map