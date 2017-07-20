class JBindConfig {
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

class JBinder
{
    datacontext: INotifyPropertyChanged;
    control: any;
    disposed: boolean = false;

    constructor(data: INotifyPropertyChanged,control)
    {
        this.datacontext = data;
        this.control = control;
    }

    dispose() {
        this.disposed = true;
    }

    static addPropertyIfNotExist(data, propertyName) {
        if (data instanceof JObserveObject) {
            var observeData = <JObserveObject>data;
            if (observeData.hasProperty(propertyName) == false) {
                observeData.addProperty(propertyName);
            }
        }
    }

    //移动element里面 text={bind $text}这样的标签到databind=""里面
    static moveAttributeBindToDatabind(element: HTMLElement)
    {
        if (!element.attributes)
            return;
        var databind = element.getAttribute("databind");
        if (!databind)
            databind = "";

        for (var i = 0; i < element.attributes.length; i++)
        {
            var att = element.attributes[i];
            if (att.name == "databind")
                continue;

            var name = att.name;
            //console.log(name);
            if (name == "class")
                name = "className";
            else if (name == "innerhtml")
                name = "innerHTML";

            var r;
            if (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(att.value))
            {
                element.attributes.removeNamedItem(att.name);
                i--;
                databind += ";" + name + "=$" + r[1];
            }
            else if (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(att.value)) {
                element.attributes.removeNamedItem(att.name);
                i--;
                databind += ";" + name + "=@" + r[1];
            }
        }

        element.setAttribute("databind", databind);
    }
}


class JDatacontextBinder extends JBinder
{
    configs: JBindConfig[] = [];
    protected propertyChangedListenerIndex: number = 0;
    protected controlPropertyChangedListenerIndex: number = 0;

    constructor(data: INotifyPropertyChanged, control) {
        super(data, control);

        var databind;
        if (control instanceof JControl)
        {
            //JControl在构造函数实现了 moveAttributeBindToDatabind功能
            databind = (<JControl>control).databind;
        }
        else 
        {
            JBinder.moveAttributeBindToDatabind(<HTMLElement>control);
            databind = (<HTMLElement>control).getAttribute("databind");
        }

        if (!databind)
            return;

        var regexp = this.getRegexp();

        while (true) {
            var result = regexp.exec(databind);
            if (!result)
                break;
            var elementPropertyName = result[1];
            var dataPropertyName = result[2];
            this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
            JBinder.addPropertyIfNotExist(data, dataPropertyName);

            databind = databind.substr(result.index + result[0].length);
        }   

        if (this.configs.length > 0)
        {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o))
        }
        else if (control instanceof JControl)
        {
            this.controlPropertyChangedListenerIndex =
                (<JControl>control).addPropertyChangedListener((s, n: string, o) => this.onControlPropertyChanged(s, n, o));
        }
        else {
            var element = <HTMLElement>control;
            this.configs.forEach((config: JBindConfig) => {
                if (config.elementPropertyName == "value" || config.elementPropertyName == "checked") {
                    element.addEventListener("change", this.listenElementEvent(this, config), false);
                }
            });
        }

        if (this.configs.length > 0)
        {
            AllJBinders.push(this);
        }

        this.updateValue();
        this.bindChildren();
    }

    updateValue() {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {//防止属性是style.width这样的格式
                var value;
                var data = this.datacontext;
                var control = this.control;

                eval("value=data." + config.dataPropertyName);
                if (value) {
                    eval("control." + config.elementPropertyName + " = value");
                }

            }
            catch (e) {
            }
        }
    }

    protected bindChildren()
    {
        if (this.control instanceof JControl) {
            var jcontrol = <JControl>this.control;
            if (jcontrol.element)
            {
                AllJBinders.push(new JDatacontextBinder(this.datacontext, jcontrol.element));
            }
            
        }
        else {
            var element = <HTMLElement>this.control;
            for (var i = 0; i < element.children.length; i++) {
                AllJBinders.push(new JDatacontextBinder(this.datacontext, element.children[i]));
            }
        }
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

    private listenElementEvent(self: JBinder, config: JBindConfig) {
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
        }
    }

    private onPropertyChanged(sender, name: string, originalValue) {
        if (this.disposed)
            return;

        try {
            var src = this;
            var config = this.getConfigByDataProName(name);
            if (config) {
                var data = src.datacontext;
                var control = src.control;

                eval("control." + config.elementPropertyName + " = data." + config.dataPropertyName);
            }
        }
        catch (e) {
        }
    }

    private onControlPropertyChanged(sender, name: string, originalValue) {
        try {
            if (this.disposed)
                return;

            var self = this;
            var config = this.getConfigByElementProName(name);
            if (config) {
                var data = self.datacontext;
                var control = self.control;

                eval("data." + config.dataPropertyName + " = control." + name);
            }
        }
        catch (e) {
        }
    }

    dispose() {
        super.dispose();
        this.disposed = true;
        if (this.propertyChangedListenerIndex) {
            this.datacontext.removeListener(this.propertyChangedListenerIndex);
            this.propertyChangedListenerIndex = 0;
        }

        if (this.control instanceof JControl && this.controlPropertyChangedListenerIndex) {
            (<JControl>this.control).removeListener(this.controlPropertyChangedListenerIndex);
            this.controlPropertyChangedListenerIndex = 0;
        }
        this.configs = [];
        
    }

    protected getRegexp(): RegExp
    {
        return /([\w|\.]+)[ ]?=[ ]?\@([\w|\.]+)/;
    }
}

class JControlBinder extends JDatacontextBinder {

    constructor(data: INotifyPropertyChanged, control) {
        super(data, control);

        
    }

    protected bindChildren() {
        if (this.control instanceof JControl) {
            var jcontrol = <JControl>this.control;
            if (jcontrol.element) {
                AllJBinders.push(new JControlBinder(jcontrol, jcontrol.element));
            }
        }
        else {
            var element = <HTMLElement>this.control;
            for (var i = 0; i < element.children.length; i++) {
                AllJBinders.push(new JControlBinder(this.datacontext, element.children[i]));
            }
        }
    }

    protected getRegexp(): RegExp {
        return /([\w|\.]+)[ ]?=[ ]?\$([\w|\.]+)/;
    }
}


class JDatacontextExpressionBinder extends JBinder {
    configs: JBindExpression[] = [];
    protected propertyChangedListenerIndex: number = 0;

    constructor(data: INotifyPropertyChanged, control) {
        super(data, control);

        var expressionStr;
        if (control instanceof JControl) {
            for (var pro in control) {
                if (pro == "expression" || /expression([0-9]+)/.exec(pro)) {
                    this.handleExpression( control[pro] );
                }
            }
        }
        else {
            var element = <HTMLElement>control;
            var allattributes = element.attributes;
            for (var i = 0; i < allattributes.length; i++) {
                if (allattributes[i].name == "expression" || /expression([0-9]+)/.exec(allattributes[i].name)) {
                    this.handleExpression(allattributes[i].value);

                }
            }
        }
        if (this.configs.length > 0) {
            this.propertyChangedListenerIndex = this.datacontext.addPropertyChangedListener((s, n: string, o) => this.onPropertyChanged(s, n, o));

            AllJBinders.push(this);
        }

        this.updateValue();
        this.bindChildren();
    }

    updateValue() {

        for (var i = 0; i < this.configs.length; i++) {
            var exconfig = this.configs[i];
            if (exconfig) {
                var element = this.control;
                var data = this.datacontext;
                eval(exconfig.expression);
            }
        }
    }

    private handleExpression(expressionStr: string)
    {
        var original
        var expression_exp = this.getRegexp();

        expressionStr = expressionStr.replace(/\{0\}\./g, "element.");
        if (expressionStr) {
            while (true) {
                var result = expression_exp.exec(expressionStr);
                if (!result)
                    break;
                var dataPropertyName = result[1];
                this.configs.push(new JBindExpression(dataPropertyName, null));
                JBinder.addPropertyIfNotExist(this.datacontext, dataPropertyName);

                expressionStr = expressionStr.replace(result[0], "data." + dataPropertyName);
            }

            for (var i = 0; i < this.configs.length; i++)
            {
                this.configs[i].expression = expressionStr;
            }
        }
    }

    protected bindChildren() {
        if (this.control instanceof JControl) {
            var jcontrol = <JControl>this.control;
            if (jcontrol.element)
                AllJBinders.push(new JDatacontextExpressionBinder(this.datacontext, jcontrol.element));
        }
        else {
            var element = <HTMLElement>this.control;
            for (var i = 0; i < element.children.length; i++) {
                AllJBinders.push(new JDatacontextExpressionBinder(this.datacontext, element.children[i]));
            }
        }
    }

    protected getConfigByDataProName(proname: string) {
        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            if (config.dataPropertyName == proname)
                return config;
        }
        return null;
    }
    
    
    private onPropertyChanged(sender, name: string, originalValue) {
        if (this.disposed)
            return;

        var config = this.getConfigByDataProName(name);
        if (config)
        {
            var element = this.control;
            var data = this.datacontext;
            eval(config.expression);
        }
        
    }
    

    dispose() {
        super.dispose();
        this.disposed = true;
        if (this.propertyChangedListenerIndex) {
            this.datacontext.removeListener(this.propertyChangedListenerIndex);
            this.propertyChangedListenerIndex = 0;
        }

        this.configs = [];
    }

    protected getRegexp(): RegExp {
        return /\{1\}\.\@([\w|\.]+)/;
    }
}

class JControlExpressionBinder extends JDatacontextExpressionBinder {
    constructor(data: INotifyPropertyChanged, control) {
        super(data, control);
    }

    protected getRegexp(): RegExp {
        return /\{1\}\.\$([\w|\.]+)/;
    }
}