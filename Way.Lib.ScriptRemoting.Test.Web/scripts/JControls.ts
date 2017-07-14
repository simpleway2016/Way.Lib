//throw new Error('Method not implemented.');
//obj instanceof JControl

window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

interface INotifyPropertyChanged {
    addPropertyChangedListener(onPropertyChanged: (sender, proName: string, originalValue) => any );
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
    private __objects = {};

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

    addNewProperty(proName, value) {
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
    }


    private __addProperty(proName) {
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
    }

    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any) {
        this.__onchanges.push(func);
    }

    onPropertyChanged( proName: string, originalValue: any) {
        for (var i = 0; i < this.__onchanges.length; i++) {
            if (this.__onchanges[i]) {
                this.__onchanges[i](this, proName, originalValue);
            }
        }
    }
}


//处理如 value=$text 为模板内元素绑定使用
class JDataBinder
{
    element: HTMLElement;
    dataContext: INotifyPropertyChanged;
    expression: RegExp = /(\w+)( )?=( )?\$(\w+)/;
    configs: JBindConfig[] = [];

    constructor(data: INotifyPropertyChanged, element: HTMLElement, expression: RegExp , bindmyselft:boolean) {
        this.dataContext = data;
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
                this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName) );
                databind = databind.substr(result.index + result[0].length);
            }           
        }

        this.dataContext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));
        this.init();

        if (bindmyselft || !(<any>element)._JControl) {
            //如果不是JControl，则继续把子元素绑定
            for (var i = 0; i < element.children.length; i++) {
                var child: any = element.children[i];
                if (child.tagName != "SCRIPT")
                {
                    child._templateBinder = new JDataBinder(data, child, expression, false);
                }
                
            }
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

    private onPropertyChanged(sender,name:string,originalValue) {
        try {
            var config = this.getConfigByDataProName(name);
            if (config) {
                eval("this.element." + config.elementPropertyName + " = this.dataContext." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    }

    updateValue() {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {//防止属性是style.width这样的格式
                eval("this.element." + config.elementPropertyName + " = this.dataContext." + config.dataPropertyName);
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
                    this.element.addEventListener("change", () => {
                        try {
                            var config = this.getConfigByElementProName("value");
                            if (config) {
                                this.dataContext[config.dataPropertyName] = this.element[config.elementPropertyName];
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
    addPropertyChangedListener(func: (sender: any, proName: string, originalValue: any) => any) {
        this.onPropertyChangeds.push(func);
    }
    protected onPropertyChanged(proName: string, originalValue: any)
    {
        for (var i = 0; i < this.onPropertyChangeds.length; i++) {
            if (this.onPropertyChangeds[i]) {
                this.onPropertyChangeds[i](this, proName, originalValue);
            }
        }
    }

    element: HTMLElement;
    onPropertyChangeds = [];
    protected templateBinder: JDataBinder;
    protected dataBinder: JDataBinder;

    private _dataContext: any;
    get dataContext()
    {
        return this._dataContext;
    }
    set dataContext(value)
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
        if (this._dataContext != value)
        {
            this._dataContext = value;
            if (this.dataBinder) {
                this.dataBinder.dataContext = value;
                this.dataBinder.updateValue();
            }
            else {
                if (this.element)
                {
                    this.dataBinder = new JDataBinder(value, this.element, /(\w+)( )?=( )?\@(\w+)/, true);
                }
                
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
            value = () => { eval(<string>_value) };
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
    }

    constructor(element: HTMLElement) {

        for (var i = 0; i < element.attributes.length; i++)
        {
            var attName = element.attributes[i].name;
            if (attName == "id") {
                eval("window." + element.attributes[i].value + "=this");
            }
            else {
                for (var myproname in this) {
                    if (myproname.toLowerCase() == attName) {
                        this[<string>myproname] = element.attributes[i].value;
                    }
                }
            }
        }

        var template = this.getTemplate(element , "Template");
        if (template) {
            this.element = JElementHelper.getElement(template.innerHTML);
            if (JElementHelper.getJControlTypeName(this.element.tagName))
            {
                throw new Error("不能把JControl作为模板的首个元素");
            }
            JElementHelper.replaceElement(this.element, element);
            (<any>this.element)._JControl = this;
            this.element.addEventListener("click", this.onclick, false);
            
            //把this.element里面的JControl初始化
            JElementHelper.initElements(this.element);

            this.templateBinder = new JDataBinder(this, this.element, /(\w+)( )?=( )?\$(\w+)/, true);
            if (this.dataContext) {
                this.dataBinder = new JDataBinder(this.dataContext, this.element, /(\w+)( )?=( )?\@(\w+)/, true);
            }
        }
    }

    protected getTemplate(element: HTMLElement, forwhat: string): HTMLElement {
        var template = element.querySelector("script[for='" + forwhat + "']");
        return <HTMLElement>template;
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

if (document.addEventListener) {
    document.addEventListener('DOMContentLoaded', function () { JElementHelper.initElements(document.body);}, false);
}
