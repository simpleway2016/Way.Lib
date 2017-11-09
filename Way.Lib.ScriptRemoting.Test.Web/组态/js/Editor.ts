declare var fileBrowser: FileBrowser;
var windowid = new Date().getTime();
window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

class ToolBoxItem
{
    buildDone: (control: EditorControl) => any;
    get supportMove(): boolean
    {
        return true;
    }
    constructor()
    {

    }

    begin(svgContainer: SVGSVGElement,position:any)
    {
    }
    mousemove(x,y) {
    }
    end(): EditorControl {
        return null;
    }
}

class ToolBox_Line extends ToolBoxItem
{
    control: LineControl;
    constructor()
    {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {

        this.control = new LineControl();
        this.control.lineElement.setAttribute('x1', position.x);
        this.control.lineElement.setAttribute('y1', position.y);
        this.control.lineElement.setAttribute('x2', position.x);
        this.control.lineElement.setAttribute('y2', position.y);

        this.control.virtualLineElement.setAttribute('x1', position.x);
        this.control.virtualLineElement.setAttribute('y1', position.y);
        this.control.virtualLineElement.setAttribute('x2', position.x);
        this.control.virtualLineElement.setAttribute('y2', position.y);
        
        svgContainer.appendChild(this.control.element);
    }
    mousemove(x,y) {
        this.control.lineElement.setAttribute('x2', x);
        this.control.lineElement.setAttribute('y2', y);

        this.control.virtualLineElement.setAttribute('x2', x);
        this.control.virtualLineElement.setAttribute('y2', y);
    }
    end(): EditorControl {
        return this.control;
    }
}

class ToolBox_Rect extends ToolBoxItem {
    control: RectControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new RectControl(null);

        this.control.element.setAttribute('x', position.x);
        this.control.element.setAttribute('y', position.y);
        this.control.element.setAttribute('width',"0");
        this.control.element.setAttribute('height', "0");
        

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        }; 
    }
    end(): EditorControl {
        if (<any>this.control.element.getAttribute("width") == 0 || <any>this.control.element.getAttribute("height") == 0)
        {
            return null;
        }
        return this.control;
    }
}

class ToolBox_Ellipse extends ToolBoxItem {
    control: EllipseControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new EllipseControl();

        this.control.element.setAttribute('cx', position.x);
        this.control.element.setAttribute('cy', position.y);
        this.control.element.setAttribute('rx', "0");
        this.control.element.setAttribute('ry', "0");
       

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.element.setAttribute('rx', <any>Math.abs( x - this.startx));
        this.control.element.setAttribute('ry', <any>Math.abs( y - this.starty));
    }
    end(): EditorControl {
        if (<any>this.control.element.getAttribute("rx") == 0 || <any>this.control.element.getAttribute("ry") == 0) {
            return null;
        }
        return this.control;
    }
}

class ToolBox_Circle extends ToolBoxItem {
    control: CircleControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new CircleControl();

        this.control.element.setAttribute('cx', position.x);
        this.control.element.setAttribute('cy', position.y);
        this.control.element.setAttribute('r', "0");
       

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.element.setAttribute('r', <any>Math.abs(x - this.startx));
    }
    end(): EditorControl {
        if (<any>this.control.element.getAttribute("r") == 0 ) {
            return null;
        }
        return this.control;
    }
}

class ToolBox_Image extends ToolBoxItem {
    control: ImageControl;

    get supportMove(): boolean {
        return false;
    }

    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        fileBrowser.onSelectFile = (path) => {
            fileBrowser.hide();
            this.control = new ImageControl();

            this.control.element.setAttribute('x', position.x);
            this.control.element.setAttribute('y', position.y);
            this.control.element.setAttribute('width', "200");
            this.control.element.setAttribute('height', "200");
            this.control.element.href.baseVal = path;

            svgContainer.appendChild(this.control.element);
            if (this.buildDone)
            {
                this.buildDone(this.control);
            }
        };
        fileBrowser.show();        
    }
}

class ToolBox_Text extends ToolBoxItem {
    control: TextControl;

    get supportMove(): boolean {
        return false;
    }

    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new TextControl();

        this.control.element.setAttribute('x', position.x);
        this.control.element.setAttribute('y', position.y);
       
        svgContainer.appendChild(this.control.element);

        (<any>this.control.element).onselectstart = (e: Event) => { e.preventDefault(); e.cancelBubble = true; return false; };

        if (this.buildDone) {
            this.buildDone(this.control);
        }
    }
}

class ToolBox_Cylinder extends ToolBoxItem {
    control: CylinderControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new CylinderControl();

        this.control.rectElement.setAttribute('x', position.x);
        this.control.rectElement.setAttribute('y', position.y);

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.rectElement.setAttribute('x', <any>Math.min(x, this.startx));
        this.control.rectElement.setAttribute('y', <any>Math.min(y, this.starty));

        this.control.rectElement.setAttribute('width', <any>Math.abs(x - this.startx));
        this.control.rectElement.setAttribute('height', <any>Math.abs(y - this.starty));
    }
    end(): EditorControl {
        if (<any>this.control.rectElement.getAttribute("width") == 0 || <any>this.control.rectElement.getAttribute("height") == 0) {
            return null;
        }
        this.control.resetCylinder(this.control.rect);
        return this.control;
    }
}

class ToolBox_Trend extends ToolBoxItem {
    control: TrendControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new TrendControl();

        this.control.rectElement.setAttribute('x', position.x);
        this.control.rectElement.setAttribute('y', position.y);
        this.control.rectElement.setAttribute('width', "0");
        this.control.rectElement.setAttribute('height', "0");


        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    }
    end(): EditorControl {
        if (<any>this.control.rectElement.getAttribute("width") == 0 || <any>this.control.rectElement.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    }
}

class ToolBox_ButtonArea extends ToolBoxItem {
    control: ButtonAreaControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new ButtonAreaControl();

        this.control.element.setAttribute('x', position.x);
        this.control.element.setAttribute('y', position.y);
        this.control.element.setAttribute('width', "0");
        this.control.element.setAttribute('height', "0");


        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.control.element);
    }
    mousemove(x, y) {
        this.control.rect = {
            x: Math.min(x, this.startx),
            y: Math.min(y, this.starty),
            width: Math.abs(x - this.startx),
            height: Math.abs(y - this.starty)
        };
    }
    end(): EditorControl {
        if (<any>this.control.element.getAttribute("width") == 0 || <any>this.control.element.getAttribute("height") == 0) {
            return null;
        }
        return this.control;
    }
}

interface IEditorControlContainer
{
    controls: any[];
    addControl(ctrl: EditorControl);
    removeControl(ctrl: EditorControl);
}

class Editor implements IEditorControlContainer
{
    removeControl(ctrl: EditorControl) {
       
        for (var i = 0; i < this.controls.length; i++)
        {
            if (this.controls[i] == ctrl)
            {
                ctrl.container = null;
                this.svgContainer.removeChild(ctrl.element);
                this.controls.splice(i, 1);
                break;
            }
        }
    }
    addControl(ctrl: EditorControl) {
        if (!ctrl.element.parentElement) {
            this.svgContainer.appendChild(ctrl.element);
        }
        this.controls.push(ctrl);
        ctrl.container = this;
    }
    private svgContainer: SVGSVGElement;
    private currentToolBoxItem: ToolBoxItem;
    private svgContainerMouseUpPosition: any;
    private beginedToolBoxItem: ToolBoxItem = null;
    propertyDialog: PropertyDialog;
    controls: any[] = [];
    private selectingElement: SVGRectElement;
    undoMgr: UndoManager;

    get colorBG() {
        return this.svgContainer.style.backgroundColor;
    }
    set colorBG(v) {
        if (v == "")
            v = "#FFFFFF";
        this.svgContainer.style.backgroundColor = v;
    }
    get imgBg() {
        var url = this.svgContainer.style.backgroundImage;
        if (url && url.length > 0) {
            url = url.substr(4, url.length - 5);
        }
        return url;
    }
    set imgBg(v) {
        if (v == "") {
            this.svgContainer.style.backgroundImage = "";
        }
        else {
            this.svgContainer.style.backgroundImage = "url(" + v + ")";
        }
    }
    get bgWidth() {
        var size = this.svgContainer.style.backgroundSize.split(' ');
        return size[0];
    }
    set bgWidth(v) {

        if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
            v += "px";
        var size = this.svgContainer.style.backgroundSize.split(' ');
        this.svgContainer.style.backgroundSize = v + " " + size[1];
    }
    get bgHeight() {
        var size = this.svgContainer.style.backgroundSize.split(' ');
        return size[1];
    }
    set bgHeight(v) {
        if (v.indexOf("%") < 0 && v.indexOf("px") < 0)
            v += "px";

        var size = this.svgContainer.style.backgroundSize.split(' ');
        this.svgContainer.style.backgroundSize = size[0] + " " + v;
    }

    getPropertiesCaption(): string[] {
        return ["底色", "背景图", "背景图宽", "背景图高"];
    }
    getProperties(): string[] {
        return ["colorBG", "imgBg", "bgWidth", "bgHeight"];
    }

    constructor(id: string)
    {
        var divContainer: HTMLElement = <HTMLElement>document.body.querySelector("#" + id);
        this.undoMgr = new UndoManager();

        this.svgContainer = document.createElementNS('http://www.w3.org/2000/svg', 'svg');

        this.svgContainer.setAttribute('width', '100%');
        this.svgContainer.style.backgroundSize = "100% 100%";
        this.svgContainer.style.backgroundRepeat = "no-repeat";
        this.svgContainer.setAttribute('height', '100%');
        this.svgContainer.style.backgroundColor = "#ffffff";
        divContainer.appendChild(this.svgContainer);

        this.svgContainer.addEventListener("click", (e) => {
            if ((<any>this.svgContainer)._notClick) {
                (<any>this.svgContainer)._notClick = false;
                return;
            }

            this.svgContainerClick(e);
        });
        this.svgContainer.addEventListener("mousedown", (e) => {
    
            if (!this.currentToolBoxItem)
            {
                (<any>this.svgContainer)._notClick = true;

                this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                (<any>this.selectingElement)._startx = e.clientX - divContainer.offsetLeft;
                (<any>this.selectingElement)._starty = e.clientY - divContainer.offsetTop;
                this.selectingElement.setAttribute('x', <any>(e.clientX - divContainer.offsetLeft));
                this.selectingElement.setAttribute('y', <any>(e.clientY - divContainer.offsetTop));
                this.selectingElement.setAttribute('width', "0");
                this.selectingElement.setAttribute('height', "0");
                this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
                this.svgContainer.appendChild(this.selectingElement);
                (<any>this.svgContainer).setCapture();
            }
        });
        this.svgContainer.addEventListener("mouseup", (e) => {
          
            if (this.selectingElement) {
                (<any>this.svgContainer).releaseCapture();
                var rect = {
                    x: parseInt(this.selectingElement.getAttribute("x")),
                    y: parseInt(this.selectingElement.getAttribute("y")),
                    width: parseInt(this.selectingElement.getAttribute("width")),
                    height: parseInt(this.selectingElement.getAttribute("height")),
                };
                this.svgContainer.removeChild(this.selectingElement);
                this.selectingElement = null;
                this.selectControlsByRect(rect, e.ctrlKey);

                setTimeout(() => {
                    (<any>this.svgContainer)._notClick = false;
                }, 500);
            }
            else {
                this.svgContainerMouseUpPosition = {
                    x: e.clientX - divContainer.offsetLeft,
                    y: e.clientY - divContainer.offsetTop
                };
            }
            
        });
        this.svgContainer.addEventListener("mousemove", (e) => {
            if (this.selectingElement) {
                var w = e.clientX - divContainer.offsetLeft - (<any>this.selectingElement)._startx ;
                var h = e.clientY - divContainer.offsetTop - (<any>this.selectingElement)._starty;
                if (w < 0)
                {
                    var x = (<any>this.selectingElement)._startx  + w;
                    w = -w;
                    this.selectingElement.setAttribute("x", <any>x);
                }
                if (h < 0) {
                    var y = (<any>this.selectingElement)._starty + h;
                    h = -h;
                    this.selectingElement.setAttribute("y", <any>y);
                }
                this.selectingElement.setAttribute("width", <any>w);
                this.selectingElement.setAttribute("height", <any>h);
            }
            else {
                this.svgContainerMouseMove(e.clientX - divContainer.offsetLeft, e.clientY - divContainer.offsetTop);
            }
        });


        document.body.addEventListener("keydown", (e: KeyboardEvent) => {
            if (e.keyCode == 37)
            {
                //left
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 38) {
                //top
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y--;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 39) {
                //right
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.x++;
                    control.rect = rect;
                }
            }
            else if (e.keyCode == 40) {
                //bottom
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    var control = AllSelectedControls[i];
                    var rect = control.rect;
                    rect.y++;
                    control.rect = rect;
                }
            }
            else if (e.ctrlKey && e.keyCode == 90) 
            {
                this.undo();
            }
            else if (e.ctrlKey && e.keyCode == 89) 
            {
                this.redo();
            }
            else if (e.ctrlKey && e.keyCode == 67) 
            {
                this.copy();

            }
            else if (e.ctrlKey && e.keyCode == 86) 
            {
                this.paste();
            }
        }, false);
    }

    undo()
    {
        this.undoMgr.undo();
    }

    redo()
    {
        this.undoMgr.redo();
    }

    delete()
    {
        var ctrls = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            ctrls.push(control);
        }
        if (ctrls.length > 0)
        {
            var undoObj = new UndoRemoveControls(this, ctrls);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
    }

    copy()
    {
        var copyitems = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var json = control.getJson();
            copyitems.push(json);
        }

        window.localStorage.setItem("copy", JSON.stringify(copyitems));
        window.localStorage.setItem("windowid", windowid + "");
    }
    paste()
    {
        var str = window.localStorage.getItem("copy");
        if (str) {
            while (AllSelectedControls.length > 0)
                AllSelectedControls[0].selected = false;                      

            var isSameWindow = parseInt(window.localStorage.getItem("windowid")) == windowid;
            var container: IEditorControlContainer = this;

            //var groupEle = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            //isSameWindow = false;
            //var groupCtrl = new GroupControl(groupEle);
            //this.addControl(groupCtrl);
            //container = groupCtrl;

            var copyItems = JSON.parse(str);

            for (var i = 0; i < copyItems.length; i++) {
                var controlJson = copyItems[i];
                if (isSameWindow) {
                    controlJson.rect.x += 10;
                    controlJson.rect.y += 10;
                }
                var editorctrl;
                eval("editorctrl = new " + controlJson.constructorName + "()");
                container.addControl(editorctrl);
                if (this == container) {
                    editorctrl.ctrlKey = true;
                    editorctrl.selected = true;
                    editorctrl.ctrlKey = false;
                }

                for (var pname in controlJson) {
                    if (pname != "tagName" && pname != "constructorName" && pname != "rect") {
                        editorctrl[pname] = controlJson[pname];
                    }
                }
                editorctrl.rect = controlJson.rect;
            }
            
        }
    }

    fireBodyEvent(event)
    {
        //这个方式，winform做的browser不支持

        //var evt = document.createEvent('HTMLEvents');
        //evt.initEvent(event, true, true);
        //document.body.dispatchEvent(evt);
    }

    selectControlsByRect(rect, ctrlKey)
    {
        for (var i = 0; i < this.controls.length; i++)
        {
            var original = this.controls[i].ctrlKey;
            this.controls[i].ctrlKey = true;//表示ctrl按下，否则，会把其他control的selected设为false
            var intersect = this.controls[i].isIntersectWith(rect);
           
            if (intersect)
            {
                if (ctrlKey && this.controls[i].selected)
                {
                    this.controls[i].selected = false;
                }
                else {
                    this.controls[i].selected = true;
                }
            }
            else {
                if (!ctrlKey)
                {
                    this.controls[i].selected = false;
                }
            }
            this.controls[i].ctrlKey = original;
        }
    }

    setCurrentToolBoxItem(typename: string)
    {
        if (!typename)
        {
           
            this.currentToolBoxItem = null
            return;
        }
        var item;
        eval("item = new " + typename + "()");
        this.currentToolBoxItem = item;
    }

    svgContainerClick(e: MouseEvent)
    {       
        if (!this.currentToolBoxItem) {
            while (AllSelectedControls.length > 0) {
                AllSelectedControls[0].selected = false;
            }
            return;
        }
       
        if (!this.beginedToolBoxItem) {
                     
            if (this.currentToolBoxItem.supportMove) {
                this.beginedToolBoxItem = this.currentToolBoxItem;
            }
            else {
                this.currentToolBoxItem.buildDone = (control) => {

                    if (control) {
                        this.addControl(control);
                        var undoObj = new UndoAddControl(this, control);
                        this.undoMgr.addUndo(undoObj);
                    }
                    if ((<any>window).toolboxDone) {
                        (<any>window).toolboxDone();
                    }
                };
            }
            this.currentToolBoxItem.begin(this.svgContainer, this.svgContainerMouseUpPosition);
        }
        else {
            var control = this.beginedToolBoxItem.end();
            if (control) {
                this.addControl(control);
                var undoObj = new UndoAddControl(this, control);
                this.undoMgr.addUndo(undoObj);
            }
            this.beginedToolBoxItem = null;
            if ((<any>window).toolboxDone)
            {
                (<any>window).toolboxDone();
            }
        }
    }
    svgContainerMouseMove(x,y) {
        if (this.beginedToolBoxItem)
        {
            this.beginedToolBoxItem.mousemove(x, y);
        }
    }

    setting(e: MouseEvent)
    {
        e.stopPropagation();

        if ((<any>window).toolboxDone) {
            (<any>window).toolboxDone();
        }

        if (!this.propertyDialog)
            this.propertyDialog = new PropertyDialog(<any>this);
        this.propertyDialog.show();
        (<any>this)._svgContainerClickForDialog = (e) => {
            this.svgContainerClickForDialog(e);
        };
        this.svgContainer.addEventListener("click", (<any>this)._svgContainerClickForDialog, false);
    }

    private svgContainerClickForDialog(e: MouseEvent)
    {
        var ele: any = e.target;
        while (ele.tagName != "BODY")
        {
            if (ele == this.propertyDialog.rootElement)
            {
                return;
            }
            else {
                ele = ele.parentElement;
            }
        }
        this.propertyDialog.hide();
        this.svgContainer.removeEventListener("click", (<any>this)._svgContainerClickForDialog, false);

    }

    alignLeft()
    {
        var minLeft = 999999;
        for (var i = 0; i < AllSelectedControls.length; i++)
        {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.x < minLeft)
                minLeft = rect.x;
        }

        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = minLeft;
            control.rect = rect;
        }
    }
    alignRight() {
        var maxRight = -99999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect; 
            if (rect.x + rect.width > maxRight)
                maxRight = rect.x + rect.width;
        }

        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = maxRight - rect.width;
            control.rect = rect;
        }
    }
    alignTop() {
        var minTop = 999999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.y < minTop)
                minTop = rect.y;
        }

        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = minTop;
            control.rect = rect;
        }
    }
    alignBottom() {
        var maxBottom = -99999;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.y + rect.height > maxBottom)
                maxBottom = rect.y + rect.height;
        }

        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = maxBottom - rect.height;
            control.rect = rect;
        }
    }
    hSpacing()
    {
        if (AllSelectedControls.length <= 1)
            return;
        var totalspacing = 0;
        var maxRight = -99999;
        var minLeft = 999999;
        var rects = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.control = control;
            rects.push(rect);
            if (rect.x + rect.width > maxRight)
                maxRight = rect.x + rect.width;
            if (rect.x < minLeft)
                minLeft = rect.x;
        }
        totalspacing = maxRight - minLeft;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            totalspacing -= rect.width;
        }

        //从左到右排列
        for (var i = 0; i < rects.length - 1; i++)
        {
            var rect = rects[i];
            var rect2 = rects[i + 1];
            if (rect.x > rect2.x)
            {
                rects[i] = rect2;
                rects[i + 1] = rect;
                i -= 2;
                if (i < -1)
                    i = -1;
            }
        }

        //间隔
        var interval = totalspacing / (rects.length - 1);
        var left = minLeft;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            var ctrl = rect.control;
            rect.control = null;
            rect.x = left;
            left += rect.width + interval;
            ctrl.rect = rect;            
        }
    }
    vSpacing() {
        if (AllSelectedControls.length <= 1)
            return;
        var totalspacing = 0;
        var maxBottom = -99999;
        var minTop = 999999;
        var rects = [];
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.control = control;
            rects.push(rect);

            if (rect.y + rect.height > maxBottom)
                maxBottom = rect.y + rect.height;
            if (rect.y < minTop)
                minTop = rect.y;
        }       
       
        totalspacing = maxBottom - minTop;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            totalspacing -= rect.height;
        }

        //从左到右排列
        for (var i = 0; i < rects.length - 1; i++) {
            var rect = rects[i];
            var rect2 = rects[i + 1];
            if (rect.y > rect2.y) {
                rects[i] = rect2;
                rects[i + 1] = rect;
                i -= 2;
                if (i < -1)
                    i = -1;
            }
        }

        //间隔
        var interval = totalspacing / (rects.length - 1);
        var top = minTop;
        for (var i = 0; i < rects.length; i++) {
            var rect = rects[i];
            var ctrl = rect.control;
            rect.control = null;
            rect.y = top;
            top += rect.height + interval;
            ctrl.rect = rect;
        }
    }
    hCenter()
    {
        var maxHeight = 0;
        var y;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.height > maxHeight) {
                y = rect.y;
                maxHeight = rect.height;
            }
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.y = y + maxHeight / 2 - rect.height / 2;
            control.rect = rect;
        }
    }
    vCenter()
    {
        var maxWidth = 0;
        var x;
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            if (rect.width > maxWidth) {
                x = rect.x;
                maxWidth = rect.width;
            }
        }
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var rect = control.rect;
            rect.x = x + maxWidth / 2 - rect.width / 2;
            control.rect = rect;
        }
    }
    layerUp()
    {
        for (var i = 0; i < AllSelectedControls.length; i++)
        {
            var control = AllSelectedControls[i];
            var nextEle :any = (<Element>control.element).nextElementSibling;
            while (nextEle && !nextEle._editorControl)
            {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.svgContainer.removeChild(nextEle);
                this.svgContainer.insertBefore(nextEle, <any>control.element);
            }
        }
    }
    layerDown()
    {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            var preEle: any = (<Element>control.element).previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.svgContainer.removeChild(control.element);
                this.svgContainer.insertBefore(<any>control.element, preEle);
            }
        }
    }
    layerFront() {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            this.svgContainer.removeChild(control.element);
            this.svgContainer.appendChild(control.element);
        }
    }
    layerBottom() {
        for (var i = 0; i < AllSelectedControls.length; i++) {
            var control = AllSelectedControls[i];
            if (this.svgContainer.children[0] != control.element)
            {
                this.svgContainer.removeChild(control.element);
                this.svgContainer.insertBefore(<any>control.element, <any>this.svgContainer.children[0]);
            }
        }
    }
    getIndex(element)
    {
        for (var i = 0; i < this.svgContainer.children.length; i++)
        {
            if (this.svgContainer.children[i] == element)
                return i;
        }
        return -1;
    }
}