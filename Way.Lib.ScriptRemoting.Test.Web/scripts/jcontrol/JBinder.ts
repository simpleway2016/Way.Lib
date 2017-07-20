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
    protected bindingDataContext;
    control: any;
    disposed: boolean = false;
    rootControl: JControl;

    constructor(control)
    {
        this.control = control;

        var existed = false;
        AllJBinders.every((binder: JBinder) => {
            if (binder && binder.control == control && (<any>binder).constructor.name == (<any>this).constructor.name) {
                existed = true;
                return false;
            }
            return true;
        });
        if (existed)
            return;

        this.bindingDataContext = this.getDatacontext();
    }

    onPropertyChanged(sender, name: string, originalValue) {
    }

    getDatacontext(): INotifyPropertyChanged
    {
        if (this.control instanceof JControl)
        {
            this.rootControl = this.control;
            var data = (<JControl>this.control).datacontext;
            if (data == null && (<JControl>this.control).element)
            {
                var parent = (<JControl>this.control).element.parentElement;
                while (parent)
                {
                    if ((<any>parent).JControl)
                    {
                        this.rootControl = (<any>parent).JControl;
                        data = this.rootControl.datacontext;
                        if (data)
                            break;
                    }
                    else {
                        parent = parent.parentElement;
                    }
                }
            }

            return data;
        }
        else {
            var data;
            var parent = <HTMLElement>this.control;
            while (parent) {
                if ((<any>parent).JControl) {
                    this.rootControl = (<any>parent).JControl;
                    data = this.rootControl.datacontext;
                    if (data)
                        break;
                }
                else {
                    parent = parent.parentElement;
                }
            }
            return data;
        }
    }

    dispose() {
        this.disposed = true;
        this.rootControl = null;
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
    private controlListenIndex: number;

    constructor( control) {
        super(control);

       

        if (!this.bindingDataContext)
            return;

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


        var regexp = this.getRegexp();

        while (true) {
            var result = regexp.exec(databind);
            if (!result)
                break;
            var elementPropertyName = result[1];
            var dataPropertyName = result[2];
            this.configs.push(new JBindConfig(dataPropertyName, elementPropertyName));
            JBinder.addPropertyIfNotExist(this.bindingDataContext, dataPropertyName);

            databind = databind.substr(result.index + result[0].length);
        }   

        if (control instanceof JControl)
        {
            var jcontrol = <JControl>control;
            this.controlListenIndex = jcontrol.addPropertyChangedListener((s, n, o) => { this.onControlPropertyChanged(s, n, o); });
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
            this.updateValue();
        }
        
        this.bindChildren(this.control instanceof JControl ? this.control.element : this.control);
    }

    bindChildren(element: HTMLElement)
    {
        if (!element)
            return;

        for (var i = 0; i < element.children.length; i++)
        {
            var existed = false;
            AllJBinders.every((binder: JBinder) => {
                if (binder && binder.control == element.children[i] && (<any>binder).constructor.name == (<any>this).constructor.name)
                {
                    existed = true;
                    return false;
                }
                return true;
            });
            if (!existed)
            {
                var ele = <HTMLElement>element.children[i];
                var typename = (<any>this).constructor.name;
                eval("new " + typename + "(ele)");
            }
        }
    }

    updateValue() {
        var data = this.getDatacontext();

        for (var i = 0; i < this.configs.length; i++) {
            var config = this.configs[i];
            try {//防止属性是style.width这样的格式
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
            var data = self.getDatacontext();
            var control = self.control;
            try {
                eval("data." + config.dataPropertyName + " = control." + config.elementPropertyName);
            }
            catch (e) {
            }
        }
    }

    onPropertyChanged(  sender, name: string, originalValue) {
        if (this.disposed)
            return;

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
    }

    private onControlPropertyChanged(sender, name: string, originalValue) {
        try {
            if (this.disposed)
                return;

            var config = this.getConfigByElementProName(name);
            if (config) {
                var data = this.getDatacontext();
                var control = this.control;

                eval("data." + config.dataPropertyName + " = control." + name);
            }
        }
        catch (e) {
        }
    }

    dispose() {
        super.dispose();
        this.disposed = true;
        if (this.controlListenIndex)
        {
            (<JControl>this.control).removeListener(this.controlListenIndex);
            this.controlListenIndex = 0;
        }
        this.configs = [];
        
    }

    protected getRegexp(): RegExp
    {
        return /([\w|\.]+)[ ]?=[ ]?\@([\w|\.]+)/;
    }
}

class JControlBinder extends JDatacontextBinder {

    constructor(control) {
        super(control);

        
    }

    getDatacontext(): INotifyPropertyChanged {
        var element: HTMLElement;
        if (this.control instanceof JControl) {
            element = (<JControl>this.control).element.parentElement;
        }
        else {
            element = <HTMLElement>this.control;
        }

        var data;
        var parent = element;
        while (parent) {
            if ((<any>parent).JControl) {
                data = (<any>parent).JControl;
                this.rootControl = data;
                break;
            }
            else {
                parent = parent.parentElement;
            }
        }
        return data;
    }

    protected getRegexp(): RegExp {
        return /([\w|\.]+)[ ]?=[ ]?\$([\w|\.]+)/;
    }
}


class JDatacontextExpressionBinder extends JBinder {
    configs: JBindExpression[] = [];

    constructor(control) {
        super(control);

        if (!this.bindingDataContext)
            return;

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
            AllJBinders.push(this);
        }

        this.updateValue();
        this.bindChildren(this.control instanceof JControl ? this.control.element : this.control);
    }

    bindChildren(element: HTMLElement) {
        if (!element)
            return;
        for (var i = 0; i < element.children.length; i++) {
            var existed = false;
            AllJBinders.every((binder: JBinder) => {
                if (binder && binder.control == element.children[i] && (<any>binder).constructor.name == (<any>this).constructor.name) {
                    existed = true;
                    return false;
                }
                return true;
            });
            if (!existed) {
                var ele = <HTMLElement>element.children[i];
                var typename = (<any>this).constructor.name;
                eval("new " + typename + "(ele)");
            }
        }
    }

    updateValue() {

        for (var i = 0; i < this.configs.length; i++) {
            var exconfig = this.configs[i];
            if (exconfig) {
                var element = this.control;
                var data = this.getDatacontext();
                eval(exconfig.expression);
            }
        }
    }

    private handleExpression(expressionStr: string)
    {
        var original
        var expression_exp = this.getRegexp();

        var datacontext = this.getDatacontext();
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

            for (var i = 0; i < this.configs.length; i++)
            {
                if (!this.configs[i].expression)
                {
                    this.configs[i].expression = expressionStr;
                }
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
    

    onPropertyChanged( sender, name: string, originalValue) {
        if (this.disposed)
            return;

        var config = this.getConfigByDataProName(name);
        if (config)
        {
            var element = this.control;
            var data = sender;
            eval(config.expression);
        }
        
    }
    

    dispose() {
        super.dispose();
        this.disposed = true;

        this.configs = [];
    }

    protected getRegexp(): RegExp {
        return /\{1\}\.\@([\w|\.]+)/;
    }
}

class JControlExpressionBinder extends JDatacontextExpressionBinder {
    constructor(control) {
        super(control);
    }

    getDatacontext(): INotifyPropertyChanged {
        var element: HTMLElement;
        if (this.control instanceof JControl) {
            element = (<JControl>this.control).element;
        }
        else {
            element = <HTMLElement>this.control;
        }

        var data;
        var parent = (<HTMLElement>this.control).parentElement;
        while (parent) {
            if ((<any>parent).JControl) {
                data = (<any>parent).JControl;
                this.rootControl = data;
                break;
            }
            else {
                parent = parent.parentElement;
            }
        }
        return data;
    }

    protected getRegexp(): RegExp {
        return /\{1\}\.\$([\w|\.]+)/;
    }
}