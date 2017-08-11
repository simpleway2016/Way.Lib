//throw new Error('Method not implemented.');
//obj instanceof JControl
//获取类名  obj.constructor.name
//模板里，首个元素不能是JControl
//htmlElement.getContainer() 可以获取所属JControl
//属性的定义必须全小写，因为element.attribute[0].name就是全小写
//属性绑定<input databind="value=@name;className=$class;">
//属性直接替换<div>{@name}  {$text}</div>
//@表示datacontext里面的属性，如果当前元素是htmlElement，则表示该htmlElement所属JControl的datacontext
//$表示上级JControl的属性
//表达式,expression="{0}.style.color={1}.@isSelected?'red':'black'"  {1}表示datacontext {1}.$是指向JControl属性的形式
//定义多个表达式 expression="{0}.value={1}.@count + '(个)'" expression1="{0}.style.color={1}.$color" expression2="***"，@$符号不能同时出现在一个expression当中
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

        //AllJBinders.forEach((binder: JBinder) => {
        //    if (binder && binder.rootControl == this && (<any>binder).constructor.name.indexOf("Control") >= 0) {
        //        binder.onPropertyChanged(this, proName, originalValue);
        //    }
        //});
    }
    notifyDatacontextPropertyChanged(datacontext, proName: string, originalValue: any) {
        for (var i = 0; i < this.templateMatchProNames.length; i++) {
            if (this.templateMatchProNames[i] == "@" + proName) {
                //需要重新加载模板
                this.reApplyTemplate(this.element);
                break;
            }
        }

        //AllJBinders.forEach((binder: JBinder) => {
        //    if (binder && binder.rootControl == this && (<any>binder).constructor.name.indexOf("Datacontext") >= 0) {
        //        binder.onPropertyChanged(datacontext, proName, originalValue);
        //    }
        //});
       
    }

    static StaticID: number = 1;
    static StaticString: string = "JC_";
    
    originalElement: HTMLElement;
    element: HTMLElement;
    onPropertyChangeds = [];
    databind: string;
    expression: string;

    protected templates: HTMLElement[] = [];
    //Template模板里面，match包含的属性名
    protected templateMatchProNames :string[] = [];

    protected currentTemplate: HTMLElement;

    private _enable: boolean = true;
    get enable(): boolean {
        return this._enable;
    }
    set enable(value: boolean) {
        if (this._enable !== value)
        {
            var original = this._enable;
            if (typeof value == "string") {
                this._enable = (value == "true");
            }
            else {
                this._enable = value;
            }
            if (this._enable)
            {
                (<any>this.element).disabled = false;
            }
            else {
                (<any>this.element).disabled = true;
            }
            this.onPropertyChanged("enable", original);
        }
    }

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
        }

        if (this._datacontext != value)
        {
           
            var original = this._datacontext;
            this._datacontext = value;

            AllJBinders.forEach((binder: JBinder, index: number) => {
                if (binder && binder.rootControl == this && (<any>binder).constructor.name.indexOf("Datacontext") >= 0) {
                    AllJBinders[index].dispose();
                    delete AllJBinders[index];
                    AllJBinders[index] = null;
                }
            });

            new JDatacontextBinder(this);
            new JDatacontextExpressionBinder(this);


            this.onPropertyChanged("datacontext", original);

            this.checkDataContextPropertyExist();
            this.reApplyTemplate(this.element ? this.element : this.originalElement);
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

            AllJBinders.forEach((binder: JBinder, index: number) => {
                if (binder && binder.control == this && (<any>binder).constructor.name.indexOf("Control") >= 0) {
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

    private _cid: string;
    //页面唯一id
    get cid(): string {
        return this._cid;
    }
    set cid(value: string) {
        if (this._cid != value) {
            var original = this._cid;
            this._cid = value;
            this.onPropertyChanged("cid", original);
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
    
    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        this.cid = JControl.StaticString + JControl.StaticID++;
        if (JControl.StaticID >= 100000) {
            JControl.StaticString += "E_";
            JControl.StaticID = 1;
        }

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
        //这里要手动绑定一次，虽然this.datacontext变化后会调用this.reApplyTemplate，但如果一开始是null，就不会调用了，所有这里要调用一次
        this.reApplyTemplate(this.element ? this.element : this.originalElement);

        if (!this.datacontext)
        {
            //如果this.datacontext == null，表示一开始datacontext虽然new JDatacontextBinder(this);，但因为this.element==null，这个绑定动作没有意义，
            //所以这里再次绑定
            new JDatacontextBinder(this);
            new JDatacontextExpressionBinder(this);
        }

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
            if (this["_" + attName] !== value) {
                var oldvalue = this["_" + attName];
                this["_" + attName] = value;
                this.onPropertyChanged(attName, oldvalue);
            }
        }
    }

    removeFromParent()
    {
        if (this.element)
        {
            this.element.parentElement.removeChild(this.element);
        }
    }

    dispose()
    {
        this.parentJControl = null;
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
            //查找上级JControl
            this.resetParentJControl();

            JElementHelper.replaceElement(this.element, rootElement);
            

            if (this.onclick) {
                this.addEventListener("click", this.onclick, false);
            }

            
             
            this.onTemplateApply();
        }
    }

    resetParentJControl()
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

    protected onTemplateApply()
    {

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
    private _text;
    get text(): string {
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

class JPanel extends JControl {
    private _content;
    get content(): string {
        return this._content;
    }
    set content(value: string) {
        if (value != this._content) {
            var originalValue = this._content;
            this._content = value;
            this.onPropertyChanged("content", originalValue);
        }
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }
}

class JTextbox extends JButton {
    private _type: string;
    get type(): string {
        return this._type;
    }
    set type(value: string)
    {
        if (this._type !== value)
        {
            var originalValue = this._type;
            this._type = value;
            this.onPropertyChanged("type", originalValue);
        }
    }
    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);

        if (!this.type)
        {
            this.type = "text";
        }
    }
}

class JListItem extends JControl
{   

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
        
    }
}

class JList extends JControl {

    //每次执行loadMoreData，加载多少条数据，Array类型的数据源，不受此属性影响，一次性加载所有数据
    private _buffersize: number;
    get buffersize() {
        return this._buffersize;
    }
    set buffersize(value:number)
    {
        if (this._buffersize !== value)
        {
            this._buffersize = parseInt(<any>value);
        }
    }

    private _scrollController: ScrollSourceManager;

    private _loadonscroll: boolean;
    //是否在滚动过程中继续加载更多数据
    get loadonscroll() {
        return this._loadonscroll;
    }
    set loadonscroll(value: boolean) {
        if (this._loadonscroll !== value) {
            if (typeof value == "string") {
                this._loadonscroll = (value == "true");
            }
            else {
                this._loadonscroll = value;
            }
        }
    }

    itemContainer: HTMLElement;
    onLoading: () => any;
    onError: (err: string) => any;

    protected itemControls: JListItem[];
    protected itemTemplates: any[];

    private _addEventIndex: number;
    private _removeEventIndex: number;
    private _itemsource: JDataSource;
    get itemsource() {
        return this._itemsource;
    }
    set itemsource(value) {
        if (this._addEventIndex && this._itemsource && this._itemsource instanceof JDataSource)
        {
            this._itemsource.removeEventListener("add", this._addEventIndex);
            this._itemsource.removeEventListener("remove", this._removeEventIndex);
        }

        this._addEventIndex = 0;
        this._removeEventIndex = 0;

        if (value && typeof value == "string") {
            if ((<string>value).indexOf("[") == 0) {
                try {
                    eval("value=" + value);
                }
                catch (e) {
                    return;
                }
            }
            else {
                if ((<string>value).indexOf(",") >= 0) {
                    var typeConfig: any = (<string>value).split(',');
                    this._itemsource = new JServerControllerSource(typeConfig[0].controller(), typeConfig[1]);
                }
                else {
                    this._itemsource = new JServerControllerSource((<any>window).controller, <string>value);
                }
                if (this.buffersize) {
                    this.bindItems();
                }
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

        if (!this.buffersize)
        {
            this.buffersize = 20;
        }

        if (this.buffersize && this.itemsource) {
            this.bindItems();
        }
    }

    //重新绑定
    rebind() {
        for (var i = 0; i < this.itemControls.length; i++) {
            if (this.itemControls[i]) {
                this.itemContainer.removeChild(this.itemControls[i].element);
                this.itemControls[i].dispose();
            }
        }
        this.itemControls = [];
        this.itemsource.clear();
        this.loadMoreData();
    }

    private clearItems()
    {
        if (this.itemControls) {
            for (var i = 0; i < this.itemControls.length; i++) {
                if (this.itemControls[i]) {
                    this.itemControls[i].dispose();
                }
            }
            this.itemControls.length = 0;
        }
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
        if (this.loadonscroll)
        {
            if (!this._scrollController)
            {
                this._scrollController = new ScrollSourceManager(this);
            }
        }
    }

    //加载更多数据
    loadMoreData()
    {

        if (this.itemsource)
        {
            if (this.itemsource instanceof JArraySource) {
                this.itemsource.loadAll();
            }
            else {
                if (this.onLoading) this.onLoading();

                this.itemsource.loadMore(this.buffersize, (count, err) => {
                    if (err)
                    {
                        if (this.onError) this.onError(err);
                    }
                    else {
                        if (this._scrollController) this._scrollController.onListLoadData();
                    }
                });
            }
        }
    }

    protected bindItems()
    {

        if (!this.itemContainer)
            return;

        this.loadMoreData();

        this.itemControls = [];
        for (var i = 0; i < this.itemsource.buffer.length; i++)
        {
            var item = this.addItem(this.itemsource.buffer[i]);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
        }
        this.resetItemIndex();

        this._addEventIndex = this.itemsource.addEventListener("add", (sender, data, index) => {
            var item = this.addItem(data);
            item.textmember = this.textmember;
            item.valuemember = this.valuemember;
            this.resetItemIndex();
        });

        this._removeEventIndex = this.itemsource.addEventListener("remove", (sender, data, index) => {
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
    private _checkedvalue: any[];
    get checkedvalue(): any[] {
        if (!this._checkedvalue)
        {
            this._checkedvalue = [];
        }
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

        JBinder.addPropertyIfNotExist(data, "checked");
        if (typeof data.checked == "undefined" || data.checked == null)
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

            if (!this._onItemDataChangedHanding)
            {
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

        JBinder.addPropertyIfNotExist(data, "checked");
        if (typeof data.checked == "undefined" || data.checked == null)
            data.checked = false;

        if (data instanceof JObserveObject) {
            (<JObserveObject>data).addPropertyChangedListener((s, name, o) => { this.onItemDataChanged(s, name, o); });
        }
        return item;
    }

    private _onItemDataChangedHanding = false;
    protected onItemDataChanged(sender, name: string, originalvalue) {
        if (name == "checked" && this.valuemember && this.valuemember.length > 0) {

            if (this._onItemDataChangedHanding)
                return;

            this._onItemDataChangedHanding = true;
            //先确定sender目前还属于item里面
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

class JCheckbox extends JListItem {
    private _checked: boolean = false;
    get checked()
    {
        return this._checked;
    }
    set checked(value: boolean)
    {
        if (this._checked != value)
        {
            var original = this._checked;
            this._checked = value;
            this.onPropertyChanged("checked", original);
        }
    }

    constructor(element: HTMLElement, templates: any[] = null, datacontext = null) {
        super(element, templates, datacontext);
    }
}

class ScrollSourceManager
{
    contentContainer: HTMLElement;
    list: JList;
    private listener = () => this.onScroll();
    private _checkBufferSize: boolean = false;

    constructor(list: JList)
    {
        this.contentContainer = list.itemContainer;
        this.list = list;

        this.contentContainer.addEventListener("scroll", this.listener, false);
    }

    dispose()
    {
        this.contentContainer.removeEventListener("scroll", this.listener);
    }

    onListLoadData()
    {
        if (!this._checkBufferSize) {
            this._checkBufferSize = true;
            var contentHeight = 0;
            for (var i = 0; i < this.list.itemContainer.children.length; i++) {
                contentHeight += (<HTMLElement>this.list.itemContainer.children[i]).offsetHeight;
            }
            if (contentHeight < this.contentContainer.offsetHeight) {
                this.list.buffersize = this.list.buffersize / (contentHeight / this.contentContainer.offsetHeight);
                this.list.loadMoreData();
            }
        }
        else {
            if (this.contentContainer.scrollHeight == this.contentContainer.offsetHeight)
            {
                this.list.loadMoreData();
            }
        }
    }

    private onScroll()
    {
        if (this.contentContainer.scrollHeight - (this.contentContainer.scrollTop + this.contentContainer.offsetHeight) <= this.contentContainer.offsetHeight * 0.5)
        {
            this.list.loadMoreData();
        }
       
    }
}