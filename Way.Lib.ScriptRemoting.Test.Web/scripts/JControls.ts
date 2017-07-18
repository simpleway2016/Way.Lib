//throw new Error('Method not implemented.');
//obj instanceof JControl
//属性的定义必须全小写，因为element.attribute[0].name就是全小写
//属性绑定<input databind="value=@name;className=$class;">
//属性直接替换<div>{@name}  {$text}</div>

window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
    removeListener(index: number);
}

class JElementHelper {
    //把dst换成source
    static replaceElement(source: HTMLElement, dst: HTMLElement) {
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
    }

    static getElement(html: string): HTMLElement {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return <HTMLElement>div.children[0];
    }

    static getJControlTypeName(tagName: string) {
        if (tagName == "JBUTTON")
            return "JButton";
        else if (tagName == "JLIST")
            return "JList";
        return false;
    }

    static initElements(container: HTMLElement) {
        for (var i = 0; i < container.children.length; i++) {
            var child = container.children[i];
            var classType = JElementHelper.getJControlTypeName(child.tagName);
            if (classType) {
                eval("new " + classType + "(child)");
            }
            else {
                JElementHelper.initElements(<HTMLElement>child);
            }
        }
    }
}

class JBindConfig
{
    dataPropertyName: string;
    elementPropertyName: string;
    constructor(dataPropertyName: string, elementPropertyName: string) {
        this.dataPropertyName = dataPropertyName;
        this.elementPropertyName = elementPropertyName;
    }
}


class JObserveObject implements INotifyPropertyChanged {

    __data;
    __parent: JObserveObject;
    __parentName: string;
    private __onchanges = [];

    constructor(data, parent: JObserveObject = null, parentname: string = null) {

        if (data instanceof JObserveObject) {
            var old: JObserveObject = data;
            this.addPropertyChangedListener((_model, _name, _value) => {
                old.onPropertyChanged(_name, _value);
            });
            //old发生变化，无法通知newModel，否则就进入死循环了
            data = old.__data;
        }

        this.__data = data;
        this.__parent = parent;
        this.__parentName = parentname;

        for (var p in data) {
            this.__addProperty(p);
        }
    }

    hasProperty(proname: string): boolean
    {
        var checkname = proname;
        var index = proname.indexOf(".");
        if (index >= 0)
        {
            checkname = proname.substr(0, index);
        }

        var result : any = Object.getOwnPropertyDescriptor(this, checkname);
        if (result)
            result = true;
        else
            result = false;

        if (result && index >= 0)
        {
            if (this[checkname] instanceof JObserveObject) {
                result = this[checkname].hasProperty(proname.substr(index + 1));
            }
        }
        return result;
    }
    addProperty(proName: string)
    {
        var checkname = proName;
        if (proName.indexOf(".") < 0) {

            if (Object.getOwnPropertyDescriptor(this, proName))//已经有这个属性
                return;

            this.__data[proName] = null;
        }
        else {
            var index = proName.indexOf(".");
            checkname = proName.substr(0, index);

            if (Object.getOwnPropertyDescriptor(this, checkname))
            {
                if (this[checkname] instanceof JObserveObject)
                {
                    (<JObserveObject>this[checkname]).addProperty(proName.substr(index + 1));
                }
                
                return;
            }
            this.__data[checkname] = new JObserveObject({}, this, checkname);
            (<JObserveObject>this.__data[checkname]).addProperty(proName.substr(index + 1));
        }

        
        Object.defineProperty(this, checkname, {
            get: this.getFunc(checkname),
            set: this.setFunc(checkname),
            enumerable: true,
            configurable: true
        });
       
    }

    private getFunc(name)
    {
        return function () {
            return this.__data[name];
        }
    }
    private setFunc(checkname:string) {
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
        }
    }
    private __addProperty(proName) {
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
    }

    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number {
        this.__onchanges.push(func);
        return this.__onchanges.length - 1;
    }

    removeListener(index: number) {
        this.__onchanges[index] = null;
    }

    onPropertyChanged( proName: string, originalValue: any) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this, proName, originalValue);
            }
        }
    }
}

class JControlDataBinder
{
    control: JControl;
    datacontext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[] = [];
    private propertyChangedListenerIndex: number;

    constructor(data: INotifyPropertyChanged, jcontrol: JControl, expression: RegExp) {
        this.datacontext = data;
        this.control = jcontrol;
        this.expression = expression;

        var databind: string = jcontrol.databind;
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

        this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));
        this.control.addPropertyChangedListener((s, n: string, o) => this.onControlPropertyChanged(s, n, o));
        this.updateValue();

    }

    dispose()
    {
        this.datacontext.removeListener(this.propertyChangedListenerIndex);
        this.configs = [];
    }

    protected getConfigByDataProName(proname: string) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    }

    protected getConfigByElementProName(proname: string) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    }

    private onPropertyChanged(sender, name: string, originalValue) {
        if (this.configs.length == 0)
            return;

        if (!this.control.element.parentElement)
        {
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
    }
    private onControlPropertyChanged(sender, name: string, originalValue) {
        try {
            var config = this.getConfigByElementProName(name);
            if (config) {
                eval("this.datacontext." + config.dataPropertyName + " = this.control." + name);
            }
        }
        catch (e) {
        }
    }

    updateValue() {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {//防止属性是style.width这样的格式
                var value;
                eval("value=this.datacontext." + config.dataPropertyName);
                if (value) {
                    eval("this.control." + config.elementPropertyName + " = value");
                }

            }
            catch (e) {
            }
        }
    }

   
}

//处理如 value=$text 为模板内元素绑定使用
class JChildrenElementBinder
{
    element: HTMLElement;
    datacontext: INotifyPropertyChanged;
    expression: RegExp;
    configs: JBindConfig[] = [];
    children: JChildrenElementBinder[] = [];

    private disposed: boolean = false;
    private propertyChangedListenerIndex: number;

    constructor(data: INotifyPropertyChanged, element: HTMLElement, expression: RegExp , bindmyselft:boolean) {
        this.datacontext = data;
        this.element = element;
        this.expression = expression;

        var databind: string =  element.getAttribute("databind");
        if (databind)
        {
            while (true)
            {
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

        if (this.configs.length > 0) {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));
        }
        this.init();

        if (bindmyselft || !(<any>element).JControl) {
            //如果不是JControl，则继续把子元素绑定
            for (var i = 0; i < element.children.length; i++) {
                var child: any = element.children[i];
                if (child.tagName != "SCRIPT")
                {
                    child._templateBinder = new JChildrenElementBinder(data, child, expression, false);
                    this.children.push(child._templateBinder);
                }
                
            }
        }
    }

    static addPropertyIfNotExist(data, propertyName)
    {
        if (data instanceof JObserveObject)
        {
            var observeData = <JObserveObject>data;
            if (observeData.hasProperty(propertyName) == false)
            {
                observeData.addProperty(propertyName);
            }
        }
    }

    dispose()
    {
        this.disposed = true;
        this.datacontext.removeListener(this.propertyChangedListenerIndex);
        this.configs = [];
        for (var i = 0; i < this.children.length; i++)
        {
            this.children[i].dispose();
        }
    }

    protected getConfigByDataProName(proname: string)
    {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    }

    protected getConfigByElementProName(proname: string) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.elementPropertyName == proname)
                return config;
        }
        return null;
    }

    private onPropertyChanged(sender, name: string, originalValue) {
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
    }

    updateValue() {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {//防止属性是style.width这样的格式
                var value;
                eval("value=this.datacontext." + config.dataPropertyName);
                if (value)
                {
                    eval("this.element." + config.elementPropertyName + " = value");
                }
                
            }
            catch (e) {
            }
        }
    }

    init() {
        this.updateValue();
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                if (config.elementPropertyName == "value" )
                {
                    var src = this;
                    this.element.addEventListener("change", () => {
                        try {
                            var config = this.getConfigByElementProName("value");
                            if (config) {
                                eval("src.datacontext." + config.dataPropertyName + " = src.element." + config.elementPropertyName);
                            }
                        }
                        catch (e) {
                        }
                    }, false);
                }
            }
            catch (e)
            {
            }
        }
    }
}

class JControl implements INotifyPropertyChanged {
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any): number {
        this.onPropertyChangeds.push(func);
        return this.onPropertyChangeds.length - 1;
    }
    removeListener(index: number) {
        this.onPropertyChangeds[index] = null;
    }

    protected onPropertyChanged(proName: string, originalValue: any)
    {
        for (var i = 0; i < this.onPropertyChangeds.length; i++) {
            if (this.onPropertyChangeds[i]) {
                this.onPropertyChangeds[i](this, proName, originalValue);
            }
        }
    }

    originalElement: HTMLElement;
    element: HTMLElement;
    onPropertyChangeds = [];
    databind: string;
    protected templates: HTMLElement[] = [];
    //Template模板里面，match包含的属性名
    protected templateMatchProNames :string[] = [];

    protected currentTemplate: HTMLElement;
    protected templateBinder: JChildrenElementBinder;
    protected dataBinder: JChildrenElementBinder;
    protected controlDataBinder: JControlDataBinder;

    private _datacontext: any;
    get datacontext()
    {
        return this._datacontext;
    }
    set datacontext(value)
    {

        if (value && typeof value == "string")
        {
            try {
                eval("value="+value);
            }
            catch(e){
                return;
            }
        }

        if (value && !value.addPropertyChangedListener) {
            value = new JObserveObject(value);
            (<JObserveObject>value).addPropertyChangedListener((s, name: string, value) => {
                for (var i = 0; i < this.templateMatchProNames.length; i++)
                {
                    if (this.templateMatchProNames[i] == "@" + name)
                    {
                        //需要重新加载模板
                        this.reApplyTemplate(this.element);
                        break;
                    }
                }
            });
        }

        if (this._datacontext != value)
        {
            this._datacontext = value;
            if (this.controlDataBinder)
            {
                this.controlDataBinder.dispose();
                this.controlDataBinder = null;
            }
            if (this.dataBinder) {
                this.dataBinder.dispose();
                this.dataBinder = null;
            }

            if (value)
            {
                if (this.element) {
                    //JChildrenElementBinder用于把模板里所有子元素和datacontext绑定
                    this.dataBinder = new JChildrenElementBinder(value, this.element, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, true);
                }

                if (value) {
                    //JControlDataBinder用于把<JControl>标签和datacontext绑定
                    this.controlDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/);
                }
                if (this.element) {
                    this.setChildrenDataContext(this.element, value);
                }

                this.checkDataContextPropertyExist();
            }
            
        }
    }

    private _onclick;
    get onclick() {
        return this._onclick;
    }
    set onclick(_value) {
        var value;
        if (_value && typeof _value == "string") {
            if (_value.length > 0) {
                value = () => { eval(<string>_value) };
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
    }
    
    constructor(element: HTMLElement,templates:any[] = null,datacontext = null) {
        this.originalElement = element;
        this.datacontext = datacontext;

        //加载所有模板
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
                        this[<string>myproname] = this.originalElement.attributes[i].value;
                    }
                }
                if (!finded)
                {
                    //如果属性没有定义，动态添加一个属性
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

        this.addPropertyChangedListener((s, name: string, value) => {
            for (var i = 0; i < this.templateMatchProNames.length; i++) {
                if (this.templateMatchProNames[i] == "$" + name) {
                    //需要重新加载模板
                    this.reApplyTemplate(this.element);
                    break;
                }
            }
        });
    }

    dispose()
    {
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
    }

    addEventListener(type: string, listener: EventListenerOrEventListenerObject, useCapture?: boolean) {
        if (this.element && listener) {
            this.element.addEventListener(type, listener, useCapture);
        }
    }
    removeEventListener(type: string, listener: EventListenerOrEventListenerObject) {
        if (this.element && listener) {
            this.element.removeEventListener(type, listener);
        }
    }
    protected loadTemplates()
    {
        var alltemplates = this.originalElement.querySelectorAll("script");
        for (var i = 0; i < alltemplates.length; i++)
        {
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
        
    }

    //检查模板里的match变量，是否在datacontext已经定义，没有定义要添加定义
    private checkDataContextPropertyExist()
    {
        if (this.datacontext && this.datacontext instanceof JObserveObject)
        {
            var ob = <JObserveObject>this.datacontext;
            for (var i = 0; i < this.templateMatchProNames.length; i++) {
                if (this.templateMatchProNames[i].indexOf("@") == 0)
                {
                    var name = this.templateMatchProNames[i].substr(1);
                    if (!ob.hasProperty(name))
                    {
                        ob.addProperty(name);
                    }
                }
            }
        }
       
    }

    //重新应用模板
    private reApplyTemplate(rootElement: HTMLElement)
    {
        var template = this.getTemplate();
        if (template != this.currentTemplate) {
            this.currentTemplate = template;
            var html = template.innerHTML;
            //替换变量
            var reg = /\{\@([\w|\.]+)\}/;
            if (reg) {
                var result;
                while (result = reg.exec(html)) {
                    var name = result[1];
                    var value = "";
                    if (this.datacontext)
                    {
                        try {
                            eval("value=this.datacontext." + name);
                        } catch (e) { }
                       
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
                    } catch (e) { }
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
            (<any>this.element).JControl = this;

            if (this.onclick) {
                this.addEventListener("click", this.onclick, false);
            }

            if (!this.datacontext) {
                var parent = this.element.parentElement;
                while (parent && !this.datacontext) {
                    if ((<any>parent).JControl) {
                        this.datacontext = (<JControl>(<any>parent).JControl).datacontext;
                        break;
                    }
                    else {
                        parent = parent.parentElement;
                    }
                }
            }

            if (this.templateBinder)
            {
                this.templateBinder.dispose();
                this.templateBinder = null;
            }
            if (this.dataBinder) {
                this.dataBinder.dispose();
                this.dataBinder = null;
            }

            //把this.element里面的JControl初始化
            JElementHelper.initElements(this.element);

            this.templateBinder = new JChildrenElementBinder(this, this.element, /([\w|\.]+)( )?=( )?\$([\w|\.]+)/, true);
            if (this.datacontext) {
                this.dataBinder = new JChildrenElementBinder(this.datacontext, this.element, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, true);
            }

            this.onTemplateApply();
        }
    }

    protected onTemplateApply()
    {

    }

    protected setChildrenDataContext(element: HTMLElement, datacontext) {
        for (var i = 0; i < element.children.length; i++) {
            var child = element.children[i];
            if ((<any>child).JControl) {
                if (!(<JControl>(<any>child).JControl).datacontext) {
                    (<JControl>(<any>child).JControl).datacontext = datacontext;
                }
            }
            else {
                this.setChildrenDataContext(<HTMLElement>child, datacontext);
            }
        }
    }

    protected getTemplate(): HTMLElement {
        if (this.templates.length == 0)
            return null;
        var result = null;
        for (var i = 0; i < this.templates.length; i++)
        {
            if (this.templates[i])
            {
                var match = this.templates[i].getAttribute("match");
                if (!match || match.length == 0) {
                    result = this.templates[i];
                }
                else {
                    var reg = /\@(\w+)/;
                    while (true) {
                        var r = reg.exec(match);
                        if (!r)
                            break;
                        var name = r[1];
                        match = match.replace(r[0], "this.datacontext." + name);
                    }

                    reg = /\$(\w+)/;
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
                    catch (e)
                    {}
                    
                    if (execResult) {
                        return this.templates[i];
                    }
                }
            }
        }
        return result;
    }
}

class JButton extends JControl{
    private _text: string = "";
    get text():string {
        return this._text;
    }
    set text(value: string) {
        if (value != this._text)
        {
            var originalValue = this._text;
            this._text = value;
            this.onPropertyChanged("text", originalValue);
        }
    }
    
    constructor(element: HTMLElement) {
        super(element);
    }
}

class JDataSource
{
    source: JObserveObject[];
    private addFuncs: ((sender, data, index: number)=>any)[] = [];
    private removeFuncs: ((sender, data,index:number) => any)[] = [];

    constructor(data: JObserveObject[])
    {
        this.source = data;
    }

    addEventListener(_type: string, listener: (sender, data, index: number) => any) {
        if (listener) {
            var funcs;
            var src = this;
            eval("funcs=src." + _type + "Funcs");
            funcs.push(listener);
        }
    }

    removeEventListener(_type: string, listener: (sender, data, index: number) => any) {
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
    }

    add(data: JObserveObject)
    {
        this.source.push(data);
        for (var i = 0; i < this.addFuncs.length; i++)
        {
            this.addFuncs[i](this, data, this.source.length - 1);
        }
    }

    insert(index: number, data: JObserveObject) {
        var len = this.source.length;
        for (var i = len - 1; i >= index; i--)
        {
            this.source[i + 1] = this.source[i];
        }
        this.source[index] = data;

        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    }

    remove(data: JObserveObject) {
        for (var i = 0; i < this.source.length; i++)
        {
            if (this.source[i] == data)
            {
                this.removeAt(i);
                break;
            }
        }
    }
    removeAt(index: number) {
        if (index < this.source.length && index >= 0)
        {
            var data = this.source[index];
            for (var i = index; i < this.source.length - 1; i++) {
                this.source[i] = this.source[i + 1];
            }
            this.source.length--;
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    }
}

class JList extends JControl {

    itemContainer: HTMLElement;
    private itemControls: JControl[];
    private itemTemplates: any[];

    private _itemsource: JDataSource;
    get itemsource() {
        return this._itemsource;
    }
    set itemsource(value) {
        if (value && typeof value == "string") {
            try {
                eval("value=" + value);
            }
            catch (e) {
                return;
            }
        }
        if (value instanceof Array)
        {
            for (var i = 0; i < value.length; i++)
            {
                if (value[i] && !(value[i] instanceof JObserveObject))
                {
                    value[i] = new JObserveObject(value[i]);
                }
            }
        }
        else if (value instanceof JDataSource)
        {
            if (this.itemContainer)
            {
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
    }

    constructor(element: HTMLElement) {
        super(element);

    }

    protected loadTemplates()
    {
        super.loadTemplates();
        this.itemTemplates = [];
        for (var i = 0; i < this.templates.length; i++)
        {
            if (this.templates[i].getAttribute("for") == "item")
            {
                this.itemTemplates.push(this.templates[i]);
                this.templates[i] = null;
            }
        }
    }

    protected onTemplateApply()
    {
        super.onTemplateApply();
        this.itemContainer = this.element.id == "itemContainer" ? this.element : <HTMLElement>this.element.querySelector("*[id='itemContainer']");
        this.bindItems();
    }

    protected bindItems()
    {

        if (!this.itemContainer)
            return;

        this.itemControls = [];
        for (var i = 0; i < this.itemsource.source.length; i++)
        {
            this.addItem(this.itemsource.source[i]);
        }

        this.itemsource.addEventListener("add", (sender, data, index) => {
            this.addItem(data);
        });

        this.itemsource.addEventListener("remove", (sender, data, index) => {
            for (var i = 0; i < this.itemControls.length; i++)
            {
                if (this.itemControls[i] && this.itemControls[i].datacontext == data)
                {
                    this.itemContainer.removeChild(this.itemControls[i].element);
                    this.itemControls[i].dispose();
                    this.itemControls[i] = null;
                    break;
                }
            }
        });
    }

    private addItem(data)
    {
        var div = document.createElement("DIV");
        this.itemContainer.appendChild(div);

        var jcontrol = new JControl(div, this.itemTemplates, data);
        this.itemControls.push(jcontrol);
    }
}

if (document.addEventListener) {
    document.addEventListener('DOMContentLoaded', function () { JElementHelper.initElements(document.body);}, false);
}
