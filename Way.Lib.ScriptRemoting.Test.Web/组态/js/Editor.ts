declare var fileBrowser: FileBrowser;
var ServerUrl: string;
var windowGuid = new Date().getTime();
var CtrlKey: boolean = false;

window.onerror = (errorMessage, scriptURI, lineNumber) => {
    alert(errorMessage + "\r\nuri:" + scriptURI + "\r\nline:" + lineNumber);
}

if (true)
{
    var index = location.href.indexOf("://");
    var domain = location.href.substr(location.href.indexOf("://") + 3);
    ServerUrl = location.href.substr(0, index) + "://" + domain.substr(0, domain.indexOf("/"));
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
        fileBrowser.onHide = () => {
            if ((<any>window).toolboxDone) {
                (<any>window).toolboxDone();
            }
        };
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

        this.control.rect = position;
       
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

class ToolBox_HistoryTrend extends ToolBoxItem {
    control: HistoryTrendControl;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.control = new HistoryTrendControl();

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
    writeValue(pointName, addr, value);
    isIdExist(id: string): boolean;
    getControl(id: string): EditorControl;
}

class Editor implements IEditorControlContainer
{
    getControl(id: string): EditorControl
    {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i].id == id) {
                return this.controls[i];
            }
        }
        return null;
    }
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

        if ((<any>ctrl).constructor.name == "GroupControl") {
            var groupControl = <GroupControl>ctrl;
            //整理左边距，右边距
            var minleft = 9999999;
            var mintop = 9999999;
            for (var i = 0; i < groupControl.controls.length; i++) {
                var child = groupControl.controls[i];
                var rect = child.rect;
                if (minleft > rect.x)
                    minleft = rect.x;
                if (mintop > rect.y)
                    mintop = rect.y;
            }
            for (var i = 0; i < groupControl.controls.length; i++) {
                var child = groupControl.controls[i];
                var rect = child.rect;
                rect.x -= minleft;
                rect.y -= mintop;
                child.rect = rect;
            }
        }

        if (!ctrl.id || ctrl.id.length == 0) {
            var controlId = (<any>ctrl).constructor.name;
            var index = 1;
            while (this.isIdExist(controlId + index)) {
                index++;
            }
            ctrl.id = controlId + index;
        }

        this.controls.push(ctrl);
        ctrl.container = this;
    }
    writeValue(pointName, addr, value)
    {
        (<any>window).writeValue(pointName, addr, value);
    }

    name: string = "";
    code: string = "";
    divContainer: HTMLElement;
    svgContainer: SVGSVGElement;
    private currentToolBoxItem: ToolBoxItem;
    private svgContainerMouseUpPosition: any;
    private beginedToolBoxItem: ToolBoxItem = null;
    propertyDialog: PropertyDialog;
    controls: any[] = [];
    private selectingElement: SVGRectElement;
    undoMgr: UndoManager;
    changed: boolean = false;
    windowWidth: any;
    windowHeight: any;
    //正在编辑的设备点输入框
    editingPointTextbox: any;

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

    private _customProperties: string = "";
    get customProperties() {
        return this._customProperties;
    }
    set customProperties(v) {
        this._customProperties = v;
    }
    getPropertiesCaption(): string[] {
        return ["名称", "编号", "底色", "背景图", "背景图宽", "背景图高", "窗口宽", "窗口高","自定义变量"];
    }
    getProperties(): string[] {
        return ["name", "code", "colorBG", "imgBg", "bgWidth", "bgHeight", "windowWidth","windowHeight","customProperties"];
    }

    constructor(id: string)
    {
        this.divContainer = <HTMLElement>document.body.querySelector("#" + id);
        this.undoMgr = new UndoManager();

        this.svgContainer = document.createElementNS('http://www.w3.org/2000/svg', 'svg');

        
        this.svgContainer.style.backgroundSize = "100% 100%";
        this.svgContainer.style.backgroundRepeat = "no-repeat";
        this.svgContainer.style.backgroundColor = "#ffffff";
        this.svgContainer.style.height = "8000px";
        this.svgContainer.style.width = "8000px";
        this.divContainer.appendChild(this.svgContainer);

        this.initDivContainer();

        this.initScaleEvent();
        this.initMoveToScrollEvent();
        this.resetScrollbar();

        this.svgContainer.addEventListener("click", (e) => {
            if (e.button == 0) {
                //左键
                if ((<any>this.svgContainer)._notClick) {
                    (<any>this.svgContainer)._notClick = false;
                    return;
                }

                this.svgContainerClick(e);
            }
        });
        this.svgContainer.addEventListener("mousedown", (e) => {
            if (e.button == 0) {
                if (!this.currentToolBoxItem) {
                    (<any>this.svgContainer)._notClick = true;

                    this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
                    (<any>this.selectingElement)._startx = e.layerX;
                    (<any>this.selectingElement)._starty = e.layerY;
                    this.selectingElement.setAttribute('x', <any>(e.layerX));
                    this.selectingElement.setAttribute('y', <any>(e.layerY));
                    this.selectingElement.setAttribute('width', "0");
                    this.selectingElement.setAttribute('height', "0");
                    this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
                    this.svgContainer.appendChild(this.selectingElement);
                    (<any>this.svgContainer).setCapture();
                }
            }
        });
        this.svgContainer.addEventListener("mouseup", (e) => {
            if (e.button == 0) {
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
                    CtrlKey = e.ctrlKey;
                    this.selectControlsByRect(rect);
                    CtrlKey = false;

                    setTimeout(() => {
                        (<any>this.svgContainer)._notClick = false;
                    }, 500);
                }
                else {
                    this.svgContainerMouseUpPosition = {
                        x: e.layerX,
                        y: e.layerY
                    };
                }
            }
            
        });
        this.svgContainer.addEventListener("mousemove", (e) => {
            if (this.selectingElement) {
                var w = e.layerX - (<any>this.selectingElement)._startx ;
                var h = e.layerY - (<any>this.selectingElement)._starty;
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
                this.svgContainerMouseMove(e.layerX, e.layerY);
            }
        });

        this.svgContainer.addEventListener("dragover", (ev) => {
            ev.preventDefault();
        });
        this.svgContainer.addEventListener("drop", (ev: DragEvent) => {
            ev.preventDefault();
            try {
                var data = JSON.parse(ev.dataTransfer.getData("Text"));
                if (data && data.Type == "GroupControl") {
                    //alert(ev.layerX + "," + (ev.layerY) + "：" + data);
                    var rect: any = {};
                    rect.x = ev.layerX;
                    rect.y = ev.layerY;
                    rect.width = null;
                    rect.height = null;
                    var groupControl = this.createGroupControl(data.windowCode, rect);
                }
                else if (data && data.Type == "Point") {
                    if (this.editingPointTextbox) {
                        this.editingPointTextbox.value = data.Name;
                        this.editingPointTextbox.changeFunc();
                    }
                }
            }
            catch (e)
            {

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
            else if (e.ctrlKey && e.keyCode == 88) {
                this.cut();

            }
            else if (e.ctrlKey && e.keyCode == 86) 
            {
                this.paste();
            }
            else if (e.ctrlKey && e.keyCode == 83) {
                this.save();
            }
            else if (e.keyCode == 46) {
                this.delete();
            }
            else if (e.keyCode == 27) {
                if (this.isRunMode == false) {
                    //设计模式，可以退出全屏
                    (<any>window).exitFullScreen();
                }
            }
            else {
                
            }
        }, false);
    }

    private initMoveToScrollEvent() {
        var svg1 = this.svgContainer;

        var scrolling = false;
        var downY;
        var downX;
        var scrollInfo;
        svg1.addEventListener("mousedown", (e) => {
            if (e.button == 2) {
                e.stopPropagation();
                e.preventDefault();
                (<any>svg1).setCapture();
                svg1.style.cursor = "-moz-grab";
                scrolling = true;

                downX = e.clientX;
                downY = e.clientY;
                scrollInfo = {
                    x: svg1.parentElement.scrollLeft,
                    y: svg1.parentElement.scrollTop,
                };
            }
        }, false);

        svg1.addEventListener("mouseup", (e) => {
            if (scrolling) {
                e.stopPropagation();
                e.preventDefault();
                scrolling = false;
                (<any>svg1).releaseCapture();
                svg1.style.cursor = "default";
            }
        }, false);

        svg1.addEventListener("mousemove", (e) => {
            if (scrolling) {
                e.stopPropagation();
                e.preventDefault();

                svg1.parentElement.scrollTo(
                    Math.max(0, scrollInfo.x + downX - e.clientX),
                    Math.max(0,scrollInfo.y + downY - e.clientY)
                );
            }
        }, false);
    }

    private initScaleEvent()
    {
        var svg1 = this.svgContainer;
        svg1.style.transformOrigin = "0 0";

        var scaleFlag = 1;

        var scaling = false;
        var downY;
        var downX;
        var downClientRect;
        var downScroll;
        var downScaleFlag;
        svg1.addEventListener("mousedown", (e) => {
            if (e.button == 1) {
                e.stopPropagation();
                e.preventDefault();
                svg1.style.cursor = "none";
                (<any>svg1).setCapture();
                scaling = true;
                scaleFlag = this.currentScale;
                downY = e.layerY;
                downX = e.layerX;
                downScroll = {
                    h: svg1.parentElement.scrollLeft,
                    v: svg1.parentElement.scrollTop
                };
                downClientRect = {
                    x: e.clientX,
                    y: e.clientY - svg1.parentElement.offsetTop
                };
                downScaleFlag = scaleFlag;
            }
        }, false);

        svg1.addEventListener("mouseup", (e) => {
            if (scaling) {
                e.stopPropagation();
                e.preventDefault();
                scaling = false;
                svg1.style.cursor = "default";
                (<any>svg1).releaseCapture();
            }
        }, false);

        svg1.addEventListener("mousemove", (e)=> {
            if(scaling) {
                e.stopPropagation();
                e.preventDefault();
                scaleFlag = downScaleFlag + parseFloat(<any>(downY - e.layerY)) / 200;
                if (scaleFlag < 1)
                    scaleFlag = 1;

                this.scale(scaleFlag);

                var pointX = downX * scaleFlag;
                var pointY = downY * scaleFlag;
                svg1.parentElement.scrollTo(Math.max(0, pointX - downClientRect.x), Math.max(0, pointY - downClientRect.y));
            }
        }, false);
    }

    private initDivContainer()
    {

        this.divContainer.style.position = "relative";
        var border = document.createElement("DIV");
        border.style.borderRight = "1px solid #eee";
        border.style.borderBottom = "1px solid #eee";
        border.style.position = "absolute";
        border.style.left = "0px";
        border.style.top = "0px";
        border.innerHTML = "<div style='position:absolute;right:0;bottom:0;color:#aaa;font-size:12px;'></div>";
        this.divContainer.insertBefore(border, this.divContainer.children[0]);

        var func = (e) => {
            if (this.isRunMode) {
                this.divContainer.removeChild(border);
                this.divContainer.removeEventListener("mousemove", func, false);
                return;
            }

            if (this.isWatchingRect && e.layerY > 0) {
                border.style.display = "";
                border.style.width = e.layerX + "px";
                border.style.height = (e.layerY) + "px";
                border.children[0].innerHTML = e.layerX + "," + (e.layerY);
            }
            else {
                border.style.display = "none";
            }
        };

        this.divContainer.addEventListener("mousemove", func , false);
    }

    isWatchingRect: boolean = false;
    isRunMode: boolean = false;
    run()
    {
        this.isRunMode = true;
    }

    createGroupControl(windowCode,rect): GroupControl
    {
        try {
           
            var json = JHttpHelper.downloadUrl(ServerUrl + "/Home/GetWindowCode?windowCode=" + encodeURIComponent(windowCode));

            var content;
            eval("content=" + json);
            var groupEle = document.createElementNS('http://www.w3.org/2000/svg', 'g');
            var editor = new GroupControl(groupEle, windowCode);
            eval(content.controlsScript);
            editor.loadCustomProperties(content.customProperties);
            this.addControl(editor);
            editor.rect = rect;
            this.undoMgr.addUndo(new UndoAddControl(this, editor));
            return editor;
        }
        catch (e)
        {
            alert(e.message);
        }
    }

    getScript() {
        var properties = this.getProperties();
        var script = "";
        for (var i = 0; i < properties.length; i ++) {
            if (this[properties[i]])
            {
                script += "editor." + properties[i] + " = " + JSON.stringify(this[properties[i]]) + ";\r\n";
            }
        }
        return script;
    }

    currentScale: number = 1;
    scale(_scale)
    {
        this.currentScale = _scale;
        this.svgContainer.style.transform = "scale(" + this.currentScale + "," + this.currentScale + ")";
    }

    undo()
    {
        this.undoMgr.undo();
    }

    redo()
    {
        this.undoMgr.redo();
    }
    selectAll()
    {
        CtrlKey = true;
        for (var i = 0; i < this.controls.length; i++)
        {
            this.controls[i].selected = true;
        }
        CtrlKey = false;
    }
    selectWebControlByPointName(pointName: string)
    {        
        for (var i = 0; i < this.controls.length; i++) {
            (<EditorControl>this.controls[i]).selected = false;
        }
        CtrlKey = true;
        for (var i = 0; i < this.controls.length; i++) {
            (<EditorControl>this.controls[i]).selectByPointName(pointName);
        }
        CtrlKey = false;
    }
    group() {
        if (AllSelectedControls.length > 0) {
            var items = [];
            for (var i = 0; i < AllSelectedControls.length; i++)
            {
                items.push(AllSelectedControls[i]);
            }
            var undoObj = new UndoGroup(this, items);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
    }
    ungroup() {
        if (AllSelectedControls.length > 0) {
            var items = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                if ((<any>AllSelectedControls[i]).constructor.name == "FreeGroupControl") {
                    items.push(AllSelectedControls[i]);
                }
            }
            if (items.length > 0) {
                var undoObj = new UndoUnGroup(this, items);
                undoObj.redo();
                this.undoMgr.addUndo(undoObj);
            }
        }
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

    /**
     *重新设置editor是否显示滚动条
     */
    resetScrollbar()
    {
        var maxWidth = 0;
        var maxHeight = 0;

        for (var i = 0; i < this.controls.length; i++)
        {
            var rect = this.controls[i].rect;
            if (rect.x + rect.width > maxWidth)
                maxWidth = rect.x + rect.width;

            if (rect.y + rect.height > maxHeight)
                maxHeight = rect.y + rect.height;
        }

        if (maxWidth < this.svgContainer.parentElement.offsetWidth)
        {
            this.svgContainer.parentElement.style.overflowX = "hidden";
            this.svgContainer.parentElement.scrollLeft = 0;
        }
        else {
            this.svgContainer.parentElement.style.overflowX = "auto";
        }

        if (maxHeight < this.svgContainer.parentElement.offsetHeight) {
            this.svgContainer.parentElement.style.overflowY = "hidden";
            this.svgContainer.parentElement.scrollTop = 0;
        }
        else {
            this.svgContainer.parentElement.style.overflowY = "auto";
        }
    }

    save()
    {
        if (this.name.length == 0)
        {
            alert("请点击左上角设置图标，设置监视画面的名称");
            return;
        }
        if (this.code.length == 0) {
            alert("请点击左上角设置图标，设置监视画面的编号");
            return;
        }
        if (this.customProperties && this.customProperties.length > 0)
        {
            var items = this.customProperties.split('\n');
            var reg = /[\W]+/;
            for (var i = 0; i < items.length; i++)
            {
                var name = items[i].trim();
                if (name.length > 0)
                {
                    if (reg.exec(name))
                    {
                        alert("自定义变量“"+name+"”包含特殊符合");
                        return;
                    }
                }
            }
        }
        var scripts = "";
        var windowCodes = [];
        for (var i = 0; i < this.controls.length; i++)
        {
            scripts += this.controls[i].getScript();
            if (this.controls[i].constructor.name == "GroupControl")
            {
                windowCodes.push(this.controls[i].windowCode);
            }
        }
        (<any>window).save(this.name, this.code, this.customProperties, this.getScript(), scripts, windowCodes);
    }

    getSaveInfo()
    {
        var scripts = "";
        for (var i = 0; i < this.controls.length; i++) {
            scripts += this.controls[i].getScript();
        }
     
        return JSON.stringify({ "name": this.name, "code": this.code, "editorScript": this.getScript(), "controlsScript": scripts });
    }
    cut()
    {
        try {
            var copyitems = [];
            var ctrls = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                var control = AllSelectedControls[i];
                var json = control.getJson();
                copyitems.push(json);
                ctrls.push(control);
            }

            window.localStorage.setItem("copy", "");
            window.localStorage.setItem("cut", JSON.stringify(copyitems));
            window.localStorage.setItem("windowGuid", windowGuid + "");

            //删除组件
            var undoObj = new UndoRemoveControls(this, ctrls);
            undoObj.redo();
            this.undoMgr.addUndo(undoObj);
        }
        catch (e) {
            alert(e.message);
        }
    }
    copy()
    {
        try {
            var copyitems = [];
            for (var i = 0; i < AllSelectedControls.length; i++) {
                var control = AllSelectedControls[i];
                var json = control.getJson();
                copyitems.push(json);
            }
            window.localStorage.setItem("cut", "");
            window.localStorage.setItem("copy", JSON.stringify(copyitems));
            window.localStorage.setItem("windowGuid", windowGuid + "");
        }
        catch (e)
        {
            alert(e.message);
        }
    }
    paste()
    {
        try {
            var str = window.localStorage.getItem("copy");
            if (!str || str.length == 0) {
                str = window.localStorage.getItem("cut");
            }
            if (str && str.length > 0) {
                while (AllSelectedControls.length > 0)
                    AllSelectedControls[0].selected = false;

                var isSameWindow = parseInt(window.localStorage.getItem("windowGuid")) == windowGuid;
                var container: IEditorControlContainer = this;

                //var groupEle = document.createElementNS('http://www.w3.org/2000/svg', 'g');
                //isSameWindow = false;
                //var groupCtrl = new GroupControl(groupEle);
                //this.addControl(groupCtrl);
                //container = groupCtrl;

                var copyItems = JSON.parse(str);

                var undoObj = new UndoPaste(this, copyItems, isSameWindow);
                undoObj.redo();
                this.undoMgr.addUndo(undoObj);

            }
        }
        catch (e)
        {
            alert(e.message);
        }
    }

    isIdExist(id: string): boolean
    {
        for (var i = 0; i < this.controls.length; i++)
        {
            if (typeof this.controls[i].isIdExist == "function") {
                var result = (<IEditorControlContainer>this.controls[i]).isIdExist(id);
                if (result)
                    return true;
            }
            if ((<EditorControl>this.controls[i]).id == id)
                return true;           
        }
        return false;
    }

    fireBodyEvent(event)
    {
        //这个方式，winform做的browser不支持

        //var evt = document.createEvent('HTMLEvents');
        //evt.initEvent(event, true, true);
        //document.body.dispatchEvent(evt);
    }

    selectControlsByRect(rect)
    {
        for (var i = 0; i < this.controls.length; i++)
        {
            var intersect = this.controls[i].isIntersectWith(rect);
           
            if (intersect)
            {
                if (CtrlKey && this.controls[i].selected)
                {
                    this.controls[i].selected = false;
                }
                else {
                    this.controls[i].selected = true;
                }
            }
            else {
                if (!CtrlKey)
                {
                    this.controls[i].selected = false;
                }
            }
        }
    }

    setCurrentToolBoxItem(typename: string)
    {
        if (!typename || typename.length == 0)
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