//throw new Error('Method not implemented.');
//obj instanceof JControl
//模板里，首个元素不能是JControl
//属性的定义必须全小写，因为element.attribute[0].name就是全小写
//属性绑定<input databind="value=@name;className=$class;">
//属性直接替换<div>{@name}  {$text}</div>
//表达式,expression="{0}.style.color={1}.@isSelected?'red':'black'"  {1}表示datacontext {1}.$是指向JControl属性的形式
//定义多个表达式 expression="***" expression1="**" expression2="***"，不要把 {1}.@isSelected {1}.$index 两种变量写在同一个表达式里面

//页面定义模板文件
//<body template="/templates/system.html">

//页面定义公用模板
/**
<header>
    <Key id="btntemplate">
        <script type="text/html">
            <input type="button" class="btn" databind="value=$text;className=$class" />
        </script>
    </Key>
</header>

<JButton template="btntemplate">
</JButton>
 */


window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any): number;
    removeListener(index: number);
}

class JElementHelper {
    static SystemTemplateContainer: HTMLElement;

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

    static getControlTypeName(tagname: string): string {
        for (var name in window) {
            if (name.toUpperCase() == tagname) {
                return name;
            }
        }
        return null;
    }

    static getElement(html: string): HTMLElement {
        var div = document.createElement("DIV");
        div.innerHTML = html;
        return <HTMLElement>div.children[0];
    }


    static initElements(container: HTMLElement) {
        if (!container || !container.children)//防止#text
            return;

        for (var i = 0; i < container.children.length; i++) {
            var child = container.children[i];
            var classType = JElementHelper.getControlTypeName(child.tagName);
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
class JBindExpression {
    dataPropertyName: string;
    expression: string;
    constructor(dataPropertyName: string, expression: string) {
        this.dataPropertyName = dataPropertyName;
        this.expression = expression;
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

    configs: JBindConfig[] = [];
    expressionConfigs: JBindExpression[] = [];
    expresion_replace_reg: RegExp;
    private propertyChangedListenerIndex: number;
    private expressionPropertyChangedListenerIndex: number;

    constructor(data: INotifyPropertyChanged, jcontrol: JControl, databind_exp: RegExp, expression_exp: RegExp) {
        this.datacontext = data;
        this.control = jcontrol;
        this.expresion_replace_reg = expression_exp;

        var databind: string = jcontrol.databind;
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

        for (var attname in jcontrol)
        {
            if (attname == "expression" || /expression[0-9]+/.exec(attname))
            {
                var expressionStr: string = jcontrol[attname];
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
            this.expressionPropertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onExpressionPropertyChanged(s, n, o));
            this.control.addPropertyChangedListener((s, n: string, o) => this.onExpressionPropertyChanged(s, n, o));
        }

        if (this.configs.length > 0)
        {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));
            this.control.addPropertyChangedListener((s, n: string, o) => this.onControlPropertyChanged(s, n, o));
        }
        
        this.updateValue();

    }

    dispose()
    {
        if (this.propertyChangedListenerIndex)
        {
            this.datacontext.removeListener(this.propertyChangedListenerIndex);
            this.propertyChangedListenerIndex = 0;
        }
           

        if (this.expressionPropertyChangedListenerIndex)
        {
            this.datacontext.removeListener(this.expressionPropertyChangedListenerIndex);
            this.expressionPropertyChangedListenerIndex = 0;
        }
        
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

    protected getExpressionConfigByDataProName(proname: string) {
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var config = this.expressionConfigs[i];
            if (config.dataPropertyName == proname)
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
            var self = this;
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("self.control." + config.elementPropertyName + " = self.datacontext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    }
    private onControlPropertyChanged(sender, name: string, originalValue) {
        try {
            var self = this;
            var config = this.getConfigByElementProName(name);
            if (config) {
                eval("self.datacontext." + config.dataPropertyName + " = self.control." + name);
            }
        }
        catch (e) {
        }
    }

    private onExpressionPropertyChanged(sender, name: string, originalValue) {

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
    }

   
}

//处理如 value=$text 为模板内元素绑定使用
class JChildrenElementBinder
{
    element: HTMLElement;
    datacontext: INotifyPropertyChanged;

    configs: JBindConfig[] = [];
    expressionConfigs: JBindExpression[] = [];
    children: JChildrenElementBinder[] = [];

    private disposed: boolean = false;
    private propertyChangedListenerIndex: number;
    private propertyExpressionChangedListenerIndex: number;
    private expresion_replace_reg: RegExp;

    constructor(data: INotifyPropertyChanged, element: HTMLElement, forDataContext: boolean, bindmyselft: boolean) {
        var databind_exp = /([\w|\.]+)( )?=( )?\@([\w|\.]+)/;
        var expression_exp = /\{1\}.\@([\w|\.]+)/;
        if (!forDataContext)
        {
            databind_exp = /([\w|\.]+)( )?=( )?\$([\w|\.]+)/;
            expression_exp = /\{1\}.\$([\w|\.]+)/;
        }

        this.datacontext = data;
        this.expresion_replace_reg = expression_exp;
        this.element = element;
        var databind: string = element.getAttribute("databind");
        for (var i = 0; i < element.attributes.length; i++)
        {
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

        if (databind)
        {
            while (true)
            {
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
        for (var i = 0; i < allattributes.length; i++)
        {
            if (allattributes[i].name == "expression" || /expression([0-9]+)/.exec(allattributes[i].name))
            {
                var expressionStr: string = allattributes[i].value;
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
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));
        }
        if (this.expressionConfigs.length > 0) {
            this.propertyExpressionChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onExpressionPropertyChanged(s, n, o));
        }

        this.init();

        if (bindmyselft || !(<any>element).JControl) {
            //如果不是JControl，则继续把子元素绑定
            for (var i = 0; i < element.children.length; i++) {
                var child: any = element.children[i];
                if (child.tagName != "SCRIPT")
                {
                    if (!child.JControl) {
                        child._templateBinder = new JChildrenElementBinder(data, child, forDataContext, false);
                        this.children.push(child._templateBinder);
                    }
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
        this.datacontext.removeListener(this.propertyExpressionChangedListenerIndex);
        this.configs = [];
        this.expressionConfigs = [];
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
    protected getExpressionConfigByDataProName(proname: string) {
        for (var i = 0; i < this.expressionConfigs.length; i++) {
            var config = this.expressionConfigs[i];
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

    private onExpressionPropertyChanged(sender, name: string, originalValue) {
        if (this.disposed)
            return;

        try {
            var config = this.getExpressionConfigByDataProName(name);
            if (config) {
                var element = this.element;
                var data = this.datacontext;
                var r;
                var expression = config.expression.replace(/\{0\}\./g, "element.");
                while (r = this.expresion_replace_reg.exec(expression))
                {
                    expression = expression.replace(r[0], "data." + r[1]);
                }
                eval(expression);
            }
        }
        catch (e) {
        }
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
            if (config) {
                try {
                    //防止属性是style.width这样的格式
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

        for (var i = 0; i < this.expressionConfigs.length; i++)
        {
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
    }

    init() {
        this.updateValue();
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {
                if (config.elementPropertyName == "value" || config.elementPropertyName == "checked")
                {
                    this.element.addEventListener("change", this.listenElementEvent(this , config), false);
                }
            }
            catch (e)
            {
            }
        }
    }

    private listenElementEvent(self, config: JBindConfig)
    {
        return function ()
        {
            try {
                eval("self.datacontext." + config.dataPropertyName + " = self.element." + config.elementPropertyName);
            }
            catch (e) {
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
    expression: string;

    protected templates: HTMLElement[] = [];
    //Template模板里面，match包含的属性名
    protected templateMatchProNames :string[] = [];

    protected currentTemplate: HTMLElement;
    protected templateBinder: JChildrenElementBinder;
    protected dataBinder: JChildrenElementBinder;
    //for自己的@属性
    protected controlDataBinder: JControlDataBinder;
    //for上级$属性
    protected containerDataBinder: JControlDataBinder;

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
            var original = this._datacontext;
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
                    this.dataBinder = new JChildrenElementBinder(value, this.element,true,true);
                }

                if (value) {
                    //JControlDataBinder用于把<JControl>标签和datacontext绑定
                    this.controlDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\@([\w|\.]+)/, /\{1\}.\@([\w|\.]+)/);
                }
                if (this.element) {
                    this.setChildrenDataContext(this.element, value);
                }

                this.checkDataContextPropertyExist();
            }
            this.onPropertyChanged("datacontext", original);
        }
    }

    private _parentJControl: JControl;
    get parentJControl(): JControl
    {
        return this._parentJControl;
    }
    set parentJControl(value: JControl)
    {
        if (this._parentJControl != value)
        {
            this._parentJControl = value;
            if (this.containerDataBinder)
            {
                this.containerDataBinder.dispose();
                this.containerDataBinder = null;
            }
            this.containerDataBinder = new JControlDataBinder(value, this, /([\w|\.]+)( )?=( )?\$([\w|\.]+)/, /\{1\}.\$([\w|\.]+)/);
        }
    }

    private _id: string;
    get id(): string {
        return this._id;
    }
    set id(value: string)
    {
        if (this._id != value)
        {
            var original = this._id;
            if (original)
            {
                eval("if(window." + original + "==this) {window." + original + "=undefined;}"); 
            }
            this._id = value;
            eval("window." + value + "=this");
            this.onPropertyChanged("id", original);
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
        

        //加载所有模板
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
                            this[<string>myproname] = this.originalElement.attributes[i].value;
                        }
                        break;
                    }
                }
                if (!finded)
                {
                    //如果属性没有定义，动态添加一个属性
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

     
        if (this.originalElement.getAttribute("datacontext") && this.originalElement.getAttribute("datacontext").length > 0)
        {
            this.datacontext = this.originalElement.getAttribute("datacontext");
        }
        else {
            this.datacontext = datacontext;
        }
        this.checkDataContextPropertyExist();

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

    private getFunc(name) {
        return function () {
            return this["_" + name];
        }
    }
    private setFunc(attName: string) {
        return function (value) {
            if (this["_" + attName] != value) {
                var oldvalue = this["_" + attName];
                this["_" + attName] = value;
                this.onPropertyChanged(attName, oldvalue);
            }
        }
    }

    dispose()
    {
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
        var keyTemplate;
        if (alltemplates.length > 0)
        {
            
        }
        else if (this.originalElement.getAttribute("template") && this.originalElement.getAttribute("template").length > 0
            && (keyTemplate = document.querySelector("#" + this.originalElement.getAttribute("template"))))
        {
            alltemplates = keyTemplate.querySelectorAll("script");
        }
        else {
            //获取当前类名
            var typename = (<any>this).constructor.name;
            //use system templates
            alltemplates = <any>JElementHelper.SystemTemplateContainer.querySelector(typename).children;
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

        //如果template is null，那么忽略这次变更
        if (template && template != this.currentTemplate) {
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
            if (JElementHelper.getControlTypeName(this.element.tagName)) {
                throw new Error("不能把JControl作为模板的首个元素");
            }
            (<any>this.element).JControl = this;
            JElementHelper.replaceElement(this.element, rootElement);
            //查找上级JControl
            if (true)
            {
                var parent = this.element.parentElement;
                while (parent) {
                    if ((<any>parent).JControl) {
                        this.parentJControl = (<any>parent).JControl;
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
                //如果自己没有数据源，查看parent的数据源
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

            this.templateBinder = new JChildrenElementBinder(this, this.element, false , true);
            if (this.datacontext) {
                this.dataBinder = new JChildrenElementBinder(this.datacontext, this.element,true,true);
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
    
    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }
}
class JTextbox extends JButton {
    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
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
        this.source.splice(index, 0, data);

        for (var i = 0; i < this.addFuncs.length; i++) {
            this.addFuncs[i](this, data, index);
        }
    }

    remove(data: JObserveObject) {
        //for (var i = 0; i < this.source.length; i++)
        //{
        //    if (this.source[i] == data)
        //    {
        //        this.removeAt(i);
        //        break;
        //    }
        //}
        var index = this.source.indexOf(data);
        this.removeAt(index);
    }
    removeAt(index: number) {
        if (index < this.source.length && index >= 0)
        {
            var data = this.source[index];
            //for (var i = index; i < this.source.length - 1; i++) {
            //    this.source[i] = this.source[i + 1];
            //}
            //this.source.length--;
            
            this.source.splice(index, 1);
            for (var j = 0; j < this.removeFuncs.length; j++) {
                this.removeFuncs[j](this, data, index);
            }
        }
    }
}

class JListItem extends JControl
{
    static StaticID: number = 1;
    static StaticString: string = "JListItem_";

    id: string;
    name: string;

    private _valueMember: string;
    get valuemember(): string {
        return this._valueMember;
    }
    set valuemember(value: string) {
        if (value != this._valueMember) {
            var original = this._valueMember;
            this._valueMember = value;
            if (this.datacontext) {
                var self = this;
                eval("self.value=self.datacontext." + value);
            }
            this.onPropertyChanged("valuemember", original);
        }
    }

    private _textMember: string;
    get textmember(): string {
        return this._textMember;
    }
    set textmember(value: string) {
        if (value != this._textMember) {
            var original = this._textMember;
            this._textMember = value;
            if (this.datacontext)
            {
                var self = this;
                eval("self.text=self.datacontext." + value);
            }
            this.onPropertyChanged("textmember", original);
        }
    }

    private _index: number;
    get index(): number {
        return this._index;
    }
    set index(value: number) {
        if (value != this._index)
        {
            var original = this._index;
            this._index = value;
            this.onPropertyChanged("index", original);
        }
    }

    private _text;
    get text() {
        return this._text;
    }
    set text(value) {
        if (value != this._text) {
            var original = this._text;
            this._text = value;
            this.onPropertyChanged("text", original);
        }
    }

    private _value;
    get value() {
        return this._value;
    }
    set value(value) {
        if (value != this._value) {
            var original = this._value;
            this._value = value;
            this.onPropertyChanged("value", original);
        }
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
        this.id = JListItem.StaticString + JListItem.StaticID++;
        if (JListItem.StaticID >= 100000)
        {
            JListItem.StaticString += "E_";
            JListItem.StaticID = 1;
        }
        this.onPropertyChanged("id", undefined);
    }
}

class JList extends JControl {

    itemContainer: HTMLElement;
    protected itemControls: JListItem[];
    protected itemTemplates: any[];

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


    private _valueMember: string;
    get valuemember(): string {
        return this._valueMember;
    }
    set valuemember(value: string) {
        if (value != this._valueMember) {
            var original = this._valueMember;
            this._valueMember = value;
            if (this.itemControls) {
                for (var i = 0; i < this.itemControls.length; i++)
                {
                    if (this.itemControls[i])
                    {
                        this.itemControls[i].valuemember = value;
                    }
                }
            }
            this.onPropertyChanged("valuemember", original);
        }
    }

    private _textMember: string;
    get textmember(): string {
        return this._textMember;
    }
    set textmember(value: string) {
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
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
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
            var item = this.addItem(this.itemsource.source[i]);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
        }
        this.resetItemIndex();

        this.itemsource.addEventListener("add", (sender, data, index) => {
            var item = this.addItem(data);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
            this.resetItemIndex();
        });

        this.itemsource.addEventListener("remove", (sender, data, index) => {
            for (var i = 0; i < this.itemControls.length; i++)
            {
                if (this.itemControls[i] && this.itemControls[i].datacontext == data)
                {
                    this.itemContainer.removeChild(this.itemControls[i].element);
                    this.itemControls[i].dispose();
                    //delete
                    this.itemControls.splice(i, 1);
                    break;
                }
            }
            this.resetItemIndex();
        });
    }

    private resetItemIndex()
    {
        var index = 0;
        for (var i = 0; i < this.itemControls.length; i++)
        {
            if (this.itemControls[i])
            {
                this.itemControls[i].index = index;
                index++;
            }
        }
    }

    protected addItem(data): JListItem
    {
        var div = document.createElement("DIV");
        this.itemContainer.appendChild(div);

        var jcontrol = new JListItem(div, this.itemTemplates, data);
        this.itemControls.push(jcontrol);
        return jcontrol;
    }
}


class JCheckboxList extends JList
{
    private _checkedvalue: any[]=[];
    get checkedvalue(): any[] {
        return this._checkedvalue;
    }
    set checkedvalue(value: any[]) {
        if (value != this._checkedvalue) {
            var original = this._checkedvalue;
            this._checkedvalue = value;

            if (this.valuemember && this.valuemember.length > 0)
            {
                this.itemControls.forEach((control: JListItem, index: number, array) => {
                    var itemvalue;
                    eval("itemvalue=control.datacontext." + this.valuemember);
                    if (this._checkedvalue.indexOf(itemvalue) >= 0) {
                        control.datacontext.checked = true;
                    }
                    else {
                        control.datacontext.checked = false;
                    }
                });
            }

            this.onPropertyChanged("checkedvalue", original);
        }
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }

    protected addItem(data): JListItem {
        var item = super.addItem(data);
        if (typeof data.checked == "undefined")
            data.checked = false;

        if (data instanceof JObserveObject)
        {
            (<JObserveObject>data).addPropertyChangedListener((s, name, o) => { this.onItemDataChanged(s, name, o); });
        }
        return item;
    }

    protected onItemDataChanged(sender, name: string, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0)
        {
           //先确定sender目前还属于item里面
            for (var i = 0; i < this.itemControls.length; i++)
            {
                if (this.itemControls[i] && this.itemControls[i].datacontext == sender)
                {
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
    }
}

class JRadioList extends JList
{
    static StaticID: number = 1;

    private itemid: number;

    private _checkedvalue = null;
    get checkedvalue() {
        return this._checkedvalue;
    }
    set checkedvalue(value) {
        if (value != this._checkedvalue) {
            var original = this._checkedvalue;
            this._checkedvalue = value;

            if (this.valuemember && this.valuemember.length > 0) {
                this.itemControls.forEach((control: JListItem, index: number, array) => {
                    var itemvalue;
                    eval("itemvalue=control.datacontext." + this.valuemember);
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
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }

    protected addItem(data): JListItem {
        if (!this.itemid)
            this.itemid = (JRadioList.StaticID++);

        var item = super.addItem(data);
        item.name = "JRadioListItem_" + this.itemid;
        if (typeof data.checked == "undefined")
            data.checked = false;

        if (data instanceof JObserveObject) {
            (<JObserveObject>data).addPropertyChangedListener((s, name, o) => { this.onItemDataChanged(s, name, o); });
        }
        return item;
    }

    protected onItemDataChanged(sender, name: string, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0) {
            //先确定sender目前还属于item里面
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

            //如果都没有checked
            this.checkedvalue = null;
        }
    }
}

class JDropdownList extends JList {

    private _selectedvalue = null;
    get selectedvalue() {
        return this._selectedvalue;
    }
    set selectedvalue(value) {
        if (value != this._selectedvalue) {
            var original = this._selectedvalue;
            this._selectedvalue = value;            
            this.onPropertyChanged("selectedvalue", original);
        }
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }

    protected bindItems()
    {
        this.selectedvalue = null;
        super.bindItems();
        if (this.itemControls && this.valuemember && this.valuemember.length > 0 && this.itemControls.length > 0)
        {
            var data = this.itemControls[0].datacontext;
            var src = this;
            eval("src.selectedvalue = data." + this.valuemember);
            
        }
    }
}

if (document.addEventListener) {
    function removeElement(element) {
        if (!element.children)
            return;//可能是#text

        if (element.JControl)
        {
            (<JControl>element.JControl).dispose();
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


        //监视document.body子元素变动事件，新加入的element，转换JControl，这个监听是个异步事件模式
        var MutationObserver = (<any>window).MutationObserver ||
            (<any>window).WebKitMutationObserver ||
            (<any>window).MozMutationObserver;

        var mutationObserverSupport = !!MutationObserver;
        if (mutationObserverSupport) {
            try {
                var options = {
                    'childList': true,
                    subtree: true,
                };
                var callback = function (records) {//MutationRecord
                    records.map(function (record) {
                        if (record.addedNodes) {
                            for (var i = 0; i < record.addedNodes.length; i++) {
                                //转换JControl
                                if (!record.addedNodes[i].JControl) {
                                    JElementHelper.initElements(record.addedNodes[i].parentElement);
                                }
                            }
                        }

                        if (record.removedNodes) {
                            for (var i = 0; i < record.removedNodes.length; i++) {
                                //dispose相关JControl
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