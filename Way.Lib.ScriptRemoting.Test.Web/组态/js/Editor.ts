declare var fileBrowser : FileBrowser;
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
    lineElement: SVGLineElement;
    constructor()
    {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.lineElement = document.createElementNS('http://www.w3.org/2000/svg','line');

        this.lineElement.setAttribute('x1', position.x);
        this.lineElement.setAttribute('y1', position.y);
        this.lineElement.setAttribute('x2', position.x);
        this.lineElement.setAttribute('y2', position.y);
        this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;cursor:pointer;');
        
        svgContainer.appendChild(this.lineElement);
    }
    mousemove(x,y) {
        this.lineElement.setAttribute('x2', x);
        this.lineElement.setAttribute('y2', y);
    }
    end(): EditorControl {
        return new LineControl(this.lineElement);
    }
}

class ToolBox_Rect extends ToolBoxItem {
    rectElement: SVGRectElement;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');

        this.rectElement.setAttribute('x', position.x);
        this.rectElement.setAttribute('y', position.y);
        this.rectElement.setAttribute('width',"0");
        this.rectElement.setAttribute('height', "0");
        this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.rectElement);
    }
    mousemove(x, y) {
        this.rectElement.setAttribute('width', <any>Math.max(0 , x - this.startx));
        this.rectElement.setAttribute('height', <any>Math.max(0, y - this.starty));
    }
    end(): EditorControl {
        if (<any>this.rectElement.getAttribute("width") == 0 || <any>this.rectElement.getAttribute("height") == 0)
        {
            return null;
        }
        return new RectControl(this.rectElement);
    }
}

class ToolBox_Ellipse extends ToolBoxItem {
    rootElement: SVGEllipseElement;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'ellipse');

        this.rootElement.setAttribute('cx', position.x);
        this.rootElement.setAttribute('cy', position.y);
        this.rootElement.setAttribute('rx', "0");
        this.rootElement.setAttribute('ry', "0");
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.rootElement);
    }
    mousemove(x, y) {
        this.rootElement.setAttribute('rx', <any>Math.max(0, x - this.startx));
        this.rootElement.setAttribute('ry', <any>Math.max(0, y - this.starty));
    }
    end(): EditorControl {
        if (<any>this.rootElement.getAttribute("rx") == 0 || <any>this.rootElement.getAttribute("ry") == 0) {
            return null;
        }
        return new EllipseControl(this.rootElement);
    }
}

class ToolBox_Circle extends ToolBoxItem {
    rootElement: SVGCircleElement;
    private startx;
    private starty;
    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'circle');

        this.rootElement.setAttribute('cx', position.x);
        this.rootElement.setAttribute('cy', position.y);
        this.rootElement.setAttribute('r', "0");
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;cursor:pointer;');

        this.startx = position.x;
        this.starty = position.y;

        svgContainer.appendChild(this.rootElement);
    }
    mousemove(x, y) {
        this.rootElement.setAttribute('r', <any>Math.max(0, x - this.startx));
    }
    end(): EditorControl {
        if (<any>this.rootElement.getAttribute("r") == 0 ) {
            return null;
        }
        return new CircleControl(this.rootElement);
    }
}

class ToolBox_Image extends ToolBoxItem {
    rootElement: SVGImageElement;

    get supportMove(): boolean {
        return false;
    }

    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        fileBrowser.onSelectFile = (path) => {
            fileBrowser.hide();
            this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'image');

            this.rootElement.setAttribute('x', position.x);
            this.rootElement.setAttribute('y', position.y);
            this.rootElement.setAttribute('width', "200");
            this.rootElement.setAttribute('height', "200");
            this.rootElement.href.baseVal = path;

            svgContainer.appendChild(this.rootElement);
            if (this.buildDone)
            {
                this.buildDone(new ImageControl(this.rootElement));
            }
        };
        fileBrowser.show();        
    }
}

class ToolBox_Text extends ToolBoxItem {
    rootElement: SVGTextElement;

    get supportMove(): boolean {
        return false;
    }

    constructor() {
        super();
    }

    begin(svgContainer: SVGSVGElement, position: any) {
        this.rootElement = document.createElementNS('http://www.w3.org/2000/svg', 'text');

        this.rootElement.setAttribute('x', position.x);
        this.rootElement.setAttribute('id',"aac");
        this.rootElement.setAttribute('y', position.y);
        this.rootElement.textContent = "Text";

        this.rootElement.setAttribute('style', 'fill:#111111;cursor:default;-moz-user-select:none;');
        this.rootElement.setAttribute('font-size', "16");
        svgContainer.appendChild(this.rootElement);

        (<any>this.rootElement).onselectstart = (e: Event) => { e.preventDefault(); e.cancelBubble = true; return false; };

        if (this.buildDone) {
            this.buildDone(new TextControl(this.rootElement));
        }
    }
}

class Editor
{
    private svgContainer: SVGSVGElement;
    private currentToolBoxItem: ToolBoxItem;
    private svgContainerMouseUpPosition: any;
    private beginedToolBoxItem: ToolBoxItem = null;
    propertyDialog: PropertyDialog;
    controls: any[] = [];
    private selectingElement: SVGRectElement;

    constructor(id: string)
    {
        var divContainer: HTMLElement = <HTMLElement>document.body.querySelector("#" + id);
        

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

            this.svgContainerClick(e); this.fireBodyEvent("click");
        });
        this.svgContainer.addEventListener("mousedown", (e) => {
            this.fireBodyEvent("mousedown");         
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
        }, false);
    }

    fireBodyEvent(event)
    {
        var evt = document.createEvent('HTMLEvents');
        evt.initEvent(event, true, true);
        document.body.dispatchEvent(evt);
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
                        this.controls.push(control);
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
                this.controls.push(control);
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
            this.propertyDialog = new PropertyDialog(new SVGContainerControl(this.svgContainer));
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