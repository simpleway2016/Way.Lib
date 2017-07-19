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

            var r;
            if (r = /\{bind[ ]+\$([\w|\.]+)\}/.exec(att.value))
            {
                att.value = "";
                databind += ";" + att.name + "=$" + r[1];
            }
            else if (r = /\{bind[ ]+\@([\w|\.]+)\}/.exec(att.value)) {
                att.value = "";
                databind += ";" + att.name + "=@" + r[1];
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

        this.bindChildren();
    }

    protected bindChildren()
    {
        if (this.control instanceof JControl) {
            var jcontrol = <JControl>this.control;
            AllJBinders.push(new JDatacontextBinder(this.datacontext, jcontrol.element));
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

            try {
                eval("self.datacontext." + config.dataPropertyName + " = self.element." + config.elementPropertyName);
            }
            catch (e) {
            }
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

    private onControlPropertyChanged(sender, name: string, originalValue) {
        try {
            if (this.disposed)
                return;

            var self = this;
            var config = this.getConfigByElementProName(name);
            if (config) {
                eval("self.datacontext." + config.dataPropertyName + " = self.control." + name);
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
            AllJBinders.push(new JControlBinder(jcontrol, jcontrol.element));
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