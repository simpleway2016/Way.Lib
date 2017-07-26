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
var JControl = (function () {
    function JControl(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = this;
        this.onPropertyChangeds = [];
        this.templates = [];
        this.templateMatchProNames = [];
        this.cid = JControl.StaticString + JControl.StaticID++;
        if (JControl.StaticID >= 100000) {
            JControl.StaticString += "E_";
            JControl.StaticID = 1;
        }
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
        this.reApplyTemplate(this.element ? this.element : this.originalElement);
        if (!this.datacontext) {
            new JDatacontextBinder(this);
            new JDatacontextExpressionBinder(this);
        }
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
        var _this = this;
        for (var i = 0; i < this.onPropertyChangeds.length; i++) {
            if (this.onPropertyChangeds[i]) {
                this.onPropertyChangeds[i](this, proName, originalValue);
            }
        }
        AllJBinders.forEach(function (binder) {
            if (binder && binder.rootControl == _this && binder.constructor.name.indexOf("Control") >= 0) {
                binder.onPropertyChanged(_this, proName, originalValue);
            }
        });
    };
    JControl.prototype.onDatacontextPropertyChanged = function (datacontext, proName, originalValue) {
        var _this = this;
        AllJBinders.forEach(function (binder) {
            if (binder && binder.rootControl == _this && binder.constructor.name.indexOf("Datacontext") >= 0) {
                binder.onPropertyChanged(datacontext, proName, originalValue);
            }
        });
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
                if (this._datacontext_listen_index) {
                    this._datacontext.removeListener(this._datacontext_listen_index);
                    this._datacontext_listen_index = 0;
                }
                var original = this._datacontext;
                this._datacontext = value;
                AllJBinders.forEach(function (binder, index) {
                    if (binder && binder.rootControl == _this && binder.constructor.name.indexOf("Datacontext") >= 0) {
                        AllJBinders[index].dispose();
                        delete AllJBinders[index];
                        AllJBinders[index] = null;
                    }
                });
                new JDatacontextBinder(this);
                new JDatacontextExpressionBinder(this);
                if (value) {
                    this.checkDataContextPropertyExist();
                    this._datacontext_listen_index = value.addPropertyChangedListener(function (sender, name, oldvalue) { return _this.onDatacontextPropertyChanged(sender, name, oldvalue); });
                }
                this.onPropertyChanged("datacontext", original);
                this.checkDataContextPropertyExist();
                this.reApplyTemplate(this.element ? this.element : this.originalElement);
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
            var _this = this;
            if (this._parentJControl != value) {
                AllJBinders.forEach(function (binder, index) {
                    if (binder && binder.control == _this && binder.constructor.name.indexOf("Control") >= 0) {
                        AllJBinders[index].dispose();
                        delete AllJBinders[index];
                        AllJBinders[index] = null;
                    }
                });
                this._parentJControl = value;
                if (value) {
                    new JControlBinder(this);
                    new JControlExpressionBinder(this);
                }
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
    Object.defineProperty(JControl.prototype, "cid", {
        get: function () {
            return this._cid;
        },
        set: function (value) {
            if (this._cid != value) {
                var original = this._cid;
                this._cid = value;
                this.onPropertyChanged("cid", original);
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
    JControl.prototype.removeFromParent = function () {
        if (this.element) {
            this.element.parentElement.removeChild(this.element);
        }
    };
    JControl.prototype.dispose = function () {
        this.parentJControl = null;
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
            this.resetParentJControl();
            JElementHelper.replaceElement(this.element, rootElement);
            if (this.onclick) {
                this.addEventListener("click", this.onclick, false);
            }
            this.onTemplateApply();
        }
    };
    JControl.prototype.resetParentJControl = function () {
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
    };
    JControl.prototype.onTemplateApply = function () {
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
JControl.StaticID = 1;
JControl.StaticString = "JC_";
var JButton = (function (_super) {
    __extends(JButton, _super);
    function JButton(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        return _super.call(this, element, templates, datacontext) || this;
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
var JPanel = (function (_super) {
    __extends(JPanel, _super);
    function JPanel(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        return _super.call(this, element, templates, datacontext) || this;
    }
    Object.defineProperty(JPanel.prototype, "content", {
        get: function () {
            return this._content;
        },
        set: function (value) {
            if (value != this._content) {
                var originalValue = this._content;
                this._content = value;
                this.onPropertyChanged("content", originalValue);
            }
        },
        enumerable: true,
        configurable: true
    });
    return JPanel;
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
var JListItem = (function (_super) {
    __extends(JListItem, _super);
    function JListItem(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        return _super.call(this, element, templates, datacontext) || this;
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
var JList = (function (_super) {
    __extends(JList, _super);
    function JList(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        if (!_this.buffersize) {
            _this.buffersize = 20;
        }
        if (_this.buffersize && _this.itemsource) {
            _this.bindItems();
        }
        return _this;
    }
    Object.defineProperty(JList.prototype, "itemsource", {
        get: function () {
            return this._itemsource;
        },
        set: function (value) {
            if (this._addEventIndex && this._itemsource && this._itemsource instanceof JDataSource) {
                this._itemsource.removeEventListener("add", this._addEventIndex);
                this._itemsource.removeEventListener("remove", this._removeEventIndex);
            }
            this._addEventIndex = 0;
            this._removeEventIndex = 0;
            if (value && typeof value == "string") {
                if (value.indexOf("[") == 0) {
                    try {
                        eval("value=" + value);
                    }
                    catch (e) {
                        return;
                    }
                }
                else {
                    var typeConfig = value.split(',');
                    this._itemsource = new JServerControllerSource(typeConfig[0].controller(), typeConfig[1]);
                    if (this.buffersize) {
                        this.bindItems();
                    }
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
                this.clearItems();
                this._itemsource = value;
                if (this.buffersize) {
                    this.bindItems();
                }
                return;
            }
            else {
                throw new Error("itemsource必须是数组或者JDataSource");
            }
            this.clearItems();
            this._itemsource = new JArraySource(value);
            if (this.buffersize) {
                this.bindItems();
            }
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
    JList.prototype.clearItems = function () {
        if (this.itemControls) {
            for (var i = 0; i < this.itemControls.length; i++) {
                if (this.itemControls[i]) {
                    this.itemControls[i].dispose();
                }
            }
            this.itemControls.length = 0;
        }
    };
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
    };
    JList.prototype.loadMoreData = function () {
        var _this = this;
        if (this.itemsource) {
            if (this.itemsource instanceof JArraySource) {
                this.itemsource.loadAll();
            }
            else {
                if (this.onLoading)
                    this.onLoading();
                this.itemsource.loadMore(this.buffersize, function (count, err) {
                    if (err) {
                        if (_this.onError)
                            _this.onError(err);
                    }
                });
            }
        }
    };
    JList.prototype.bindItems = function () {
        var _this = this;
        if (!this.itemContainer)
            return;
        this.loadMoreData();
        this.itemControls = [];
        for (var i = 0; i < this.itemsource.buffer.length; i++) {
            var item = this.addItem(this.itemsource.buffer[i]);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
        }
        this.resetItemIndex();
        this._addEventIndex = this.itemsource.addEventListener("add", function (sender, data, index) {
            var item = _this.addItem(data);
            item.textmember = _this.textmember;
            item.valuemember = _this.valuemember;
            _this.resetItemIndex();
        });
        this._removeEventIndex = this.itemsource.addEventListener("remove", function (sender, data, index) {
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
        return _super.call(this, element, templates, datacontext) || this;
    }
    Object.defineProperty(JCheckboxList.prototype, "checkedvalue", {
        get: function () {
            if (!this._checkedvalue) {
                this._checkedvalue = [];
            }
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
        JBinder.addPropertyIfNotExist(data, "checked");
        if (typeof data.checked == "undefined" || data.checked == null)
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
        _this._onItemDataChangedHanding = false;
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
                if (!this._onItemDataChangedHanding) {
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
        JBinder.addPropertyIfNotExist(data, "checked");
        if (typeof data.checked == "undefined" || data.checked == null)
            data.checked = false;
        if (data instanceof JObserveObject) {
            data.addPropertyChangedListener(function (s, name, o) { _this.onItemDataChanged(s, name, o); });
        }
        return item;
    };
    JRadioList.prototype.onItemDataChanged = function (sender, name, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0) {
            if (this._onItemDataChangedHanding)
                return;
            this._onItemDataChangedHanding = true;
            if (sender[name]) {
                for (var i = 0; i < this.itemControls.length; i++) {
                    if (this.itemControls[i] && this.itemControls[i].datacontext == sender) {
                        var value;
                        var data = this.itemControls[i].datacontext;
                        eval("value = data." + this.valuemember);
                        this.checkedvalue = value;
                    }
                    else if (this.itemControls[i]) {
                        this.itemControls[i].datacontext["checked"] = false;
                    }
                }
            }
            else {
                for (var i = 0; i < this.itemControls.length; i++) {
                    if (this.itemControls[i] && this.itemControls[i].datacontext["checked"]) {
                        var value;
                        var data = this.itemControls[i].datacontext;
                        eval("value = data." + this.valuemember);
                        this.checkedvalue = value;
                        this._onItemDataChangedHanding = false;
                        return;
                    }
                }
            }
            this._onItemDataChangedHanding = false;
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
var JCheckbox = (function (_super) {
    __extends(JCheckbox, _super);
    function JCheckbox(element, templates, datacontext) {
        if (templates === void 0) { templates = null; }
        if (datacontext === void 0) { datacontext = null; }
        var _this = _super.call(this, element, templates, datacontext) || this;
        _this._checked = false;
        return _this;
    }
    Object.defineProperty(JCheckbox.prototype, "checked", {
        get: function () {
            return this._checked;
        },
        set: function (value) {
            if (this._checked != value) {
                var original = this._checked;
                this._checked = value;
                this.onPropertyChanged("checked", original);
            }
        },
        enumerable: true,
        configurable: true
    });
    return JCheckbox;
}(JListItem));
//# sourceMappingURL=JControls.js.map