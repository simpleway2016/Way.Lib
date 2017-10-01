var AllSelectedControls: EditorControl[] = [];

function documentElementMouseDown(e: MouseEvent) {
    //while (AllSelectedControls.length > 0) {
    //    AllSelectedControls[0].selected = false;
    //}
}


document.documentElement.addEventListener("mousedown", documentElementMouseDown, false);


class EditorControl
{
    propertyDialog: PropertyDialog;
    ctrlKey = false;
    element: any;
    _selected: boolean = false;
    _moveAllSelectedControl = false;
    get selected(): boolean {
        return this._selected;
    }
    set selected(value: boolean) {
        if (this._selected !== value) {
            this._selected = value;
            if (value)
            {
                if (!this.ctrlKey) {
                    while (AllSelectedControls.length > 0) {
                        AllSelectedControls[0].selected = false;
                    }
                }
                AllSelectedControls.push(this);
            }
            else {
                var index = AllSelectedControls.indexOf(this);
                AllSelectedControls.splice(index, 1);
                if (this.propertyDialog)
                    this.propertyDialog.hide();
            }
            this.onSelectedChange();
        }
    }

    get rect()
    {
        return null;
    }

    private mouseDownX;
    private mouseDownY;

    constructor(element:any)
    {
        this.element = element;

        (<HTMLElement>this.element).addEventListener("dragstart", (e) => {
            e.preventDefault();
        }, false);

        (<HTMLElement>this.element).addEventListener("click", (e) => {
            e.stopPropagation();           
        }, false);

        (<HTMLElement>this.element).addEventListener("dblclick", (e) => {
            e.stopPropagation();
            this.showProperty();
        }, false);        

        (<HTMLElement>this.element).addEventListener("mousedown", (e) => {
            if (e.button == 2)
                return;
            this._moveAllSelectedControl = this.selected;

            e.stopPropagation();

            this.ctrlKey = e.ctrlKey;
            if (this.ctrlKey)
                this.selected = !this.selected;
            else
                this.selected = true;

            this.mouseDownX = e.clientX;
            this.mouseDownY = e.clientY;
            if (this._moveAllSelectedControl) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    AllSelectedControls[i].onBeginMoving();
                }
            }
            else {
                this.onBeginMoving();
            }
        }, false);
        document.body.addEventListener("mousemove", (e) => {
            if (this.mouseDownX >= 0) {
                e.stopPropagation();
                if (this._moveAllSelectedControl) {
                    for (var i = 0; i < AllSelectedControls.length; i++)
                    {
                        AllSelectedControls[i].onMoving(this.mouseDownX, this.mouseDownY, e.clientX, e.clientY);
                    }
                }
                else {
                    this.onMoving(this.mouseDownX, this.mouseDownY, e.clientX, e.clientY);
                }
            }
        }, false);
        document.body.addEventListener("mouseup", (e) => {
            if (this.mouseDownX >= 0) {
                e.stopPropagation();
                this.onEndMoving();
                this.mouseDownX = -1;
            }
        }, false);
    }

    getPropertiesCaption(): string[]
    {
        return null;
    }
    getProperties(): string[] {
        return null;
    }

    isIntersectWith(rect):boolean
    {
        return false;
    }

    isIntersect(rect1, rect): boolean {
        return rect.x < rect1.x + rect1.width && rect1.x < rect.x + rect.width && rect.y < rect1.y + rect1.height && rect1.y < rect.y + rect.height;
    }

    showProperty()
    {
        if (!this.propertyDialog)
            this.propertyDialog = new PropertyDialog(this);
        this.propertyDialog.show();
    }

    onSelectedChange()
    {

    }
    onBeginMoving()
    {

    }
    onMoving(downX, downY, nowX, nowY)
    {

    }
    onEndMoving()
    {

    }
}
class LineControl extends EditorControl
{
    lineElement: SVGLineElement;
    pointEles: SVGCircleElement[] = [];
    moving: boolean = false;
    startX: number;
    startY: number;
    valueX: any;
    valueY: any;

    get rect() {
        return {
            x: Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2"))),
            y: Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2"))),
            width: Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2"))),
            height: Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2"))),
        };
    }

    get lineWidth() {
        return this.lineElement.style.strokeWidth;
    }
    set lineWidth(v)
    {
        this.lineElement.style.strokeWidth = v;
    }
    get color() {
        return this.lineElement.style.stroke;
    }
    set color(v) {
        this.lineElement.style.stroke = v;
    }

    constructor(element: any) {
        super(element);
        this.lineElement = element;
    }

    getPropertiesCaption(): string[] {
        return ["线宽","颜色"];
    }
    getProperties(): string[] {
        return ["lineWidth", "color"];
    }

    isIntersectWith(rect): boolean {
        var myrect = this.rect;
        return this.isIntersect(myrect , rect);
    }


    onBeginMoving() {
        (<any>this.lineElement)._x1 = parseInt(this.lineElement.getAttribute("x1"));
        (<any>this.lineElement)._y1 = parseInt(this.lineElement.getAttribute("y1"));
        (<any>this.lineElement)._x2 = parseInt(this.lineElement.getAttribute("x2"));
        (<any>this.lineElement)._y2 = parseInt(this.lineElement.getAttribute("y2"));
    }
    onMoving(downX, downY, nowX, nowY) {
        var x1 = <any>((<any>this.lineElement)._x1 + nowX - downX);
        var y1 = <any>((<any>this.lineElement)._y1 + nowY - downY);
        var x2 = <any>((<any>this.lineElement)._x2 + nowX - downX);
        var y2 = <any>((<any>this.lineElement)._y2 + nowY - downY);
        this.lineElement.setAttribute("x1", x1);
        this.lineElement.setAttribute("y1", y1);
        this.lineElement.setAttribute("x2", x2);
        this.lineElement.setAttribute("y2", y2);
        if (this.selected)
        {
            this.pointEles[0].setAttribute("cx", x1);
            this.pointEles[0].setAttribute("cy", y1);
            this.pointEles[1].setAttribute("cx", x2);
            this.pointEles[1].setAttribute("cy", y2);
        }
    }
    onEndMoving() {

    }

    onSelectedChange() {
        if (this.selected)
        {
            var pointEle1 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle1.setAttribute("cx", <any>this.lineElement.x1.animVal.value);
            pointEle1.setAttribute("cy", <any>this.lineElement.y1.animVal.value);
            pointEle1.setAttribute("r", "5");
            pointEle1.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            (<any>pointEle1).xName = "x1";
            (<any>pointEle1).yName = "y1";
            this.lineElement.parentElement.appendChild(pointEle1);

            var pointEle2 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle2.setAttribute("cx", <any>this.lineElement.x2.animVal.value);
            pointEle2.setAttribute("cy", <any>this.lineElement.y2.animVal.value);
            pointEle2.setAttribute("r", "5");
            pointEle2.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            (<any>pointEle2).xName = "x2";
            (<any>pointEle2).yName = "y2";
            this.lineElement.parentElement.appendChild(pointEle2);

            this.pointEles.push(pointEle1);
            this.pointEles.push(pointEle2);

            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i], "x" + (i + 1), "y" + (i + 1));
            }
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++)
            {
                this.lineElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];

            
        }
    }

    setEvent(pointEle, xName, yName)
    {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this.pointMouseDown(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mousemove", (e) => { this.pointMouseMove(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mouseup", (e) => { this.pointMouseUp(e, pointEle); }, false);
    }

    pointMouseDown(e: MouseEvent, pointEle, xName: string, yName: string)
    {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;
        this.valueX = parseInt(this.lineElement.getAttribute(xName));
        this.valueY = parseInt(this.lineElement.getAttribute(yName));

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    pointMouseMove(e: MouseEvent, pointEle, xName: string, yName: string) {
        if (this.moving) {
            e.stopPropagation();
            pointEle.setAttribute("cx", this.valueX + e.clientX - this.startX);
            pointEle.setAttribute("cy", this.valueY + e.clientY - this.startY);
            this.lineElement.setAttribute(xName, <any>(this.valueX + e.clientX - this.startX));
            this.lineElement.setAttribute(yName, <any>(this.valueY + e.clientY - this.startY));
        }
    }
    pointMouseUp(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    }
}

class RectControl extends EditorControl {
    rectElement: SVGRectElement;
    pRightBottom: SVGCircleElement;
    moving: boolean = false;
    startX = 0;
    startY = 0;

    get rect() {
        var myrect = {
            x: parseInt(this.rectElement.getAttribute("x")),
            y: parseInt(this.rectElement.getAttribute("y")),
            width: parseInt(this.rectElement.getAttribute("width")),
            height: parseInt(this.rectElement.getAttribute("height")),
        };
        return myrect;
    }

    get strokeWidth() {
        return this.rectElement.style.strokeWidth;
    }
    set strokeWidth(v) {
        this.rectElement.style.strokeWidth = v;
    }
    get colorStroke() {
        return this.rectElement.style.stroke;
    }
    set colorStroke(v) {
        this.rectElement.style.stroke = v;
    }
    get colorFill() {
        return this.rectElement.style.fill;
    }
    set colorFill(v) {
        this.rectElement.style.fill = v;
    }

    getPropertiesCaption(): string[] {
        return ["边框大小", "边框颜色" , "填充颜色"];
    }
    getProperties(): string[] {
        return ["strokeWidth", "colorStroke", "colorFill"];
    }

    constructor(element: any) {
        super(element);
        this.rectElement = element;
    }

    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
    }

    onSelectedChange() {
        if (this.selected) {

            this.pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            this.pRightBottom.setAttribute("r", "5");
            this.pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            this.rectElement.parentElement.appendChild(this.pRightBottom);

            this.setEvent(this.pRightBottom);
            this.resetPointLocation();
        }
        else {
            this.rectElement.parentElement.removeChild(this.pRightBottom);
        }
    }

    resetPointLocation()
    {
        this.pRightBottom.setAttribute("cx", <any>(this.rectElement.x.animVal.value + this.rectElement.width.animVal.value));
        this.pRightBottom.setAttribute("cy", <any>(this.rectElement.y.animVal.value + this.rectElement.height.animVal.value));
    }

    setEvent(pointEle) {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", (e) => { this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", (e) => { this.pointMouseUp(e, pointEle); }, false);
    }

    pointMouseDown(e: MouseEvent, pointEle) {
        e.stopPropagation();
        this.moving = true;
        this.startX = e.clientX;
        this.startY = e.clientY;

        pointEle._valueX = this.rectElement.x.animVal.value;
        pointEle._valueY = this.rectElement.y.animVal.value;
        pointEle._valueWidth = this.rectElement.width.animVal.value;
        pointEle._valueHeight = this.rectElement.height.animVal.value;

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    pointMouseMove(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();

            (<any>this.rectElement).setAttribute("width", Math.max(15, pointEle._valueWidth + (e.clientX - this.startX)));
            (<any>this.rectElement).setAttribute("height", Math.max(15, pointEle._valueHeight + (e.clientY - this.startY)));
            this.resetPointLocation();
        }
    }
    pointMouseUp(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    }


    onBeginMoving() {
        (<any>this.rectElement)._x = parseInt(this.rectElement.getAttribute("x"));
        (<any>this.rectElement)._y = parseInt(this.rectElement.getAttribute("y"));
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.rectElement)._x + nowX - downX);
        var y = <any>((<any>this.rectElement)._y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

class EllipseControl extends EditorControl {
    rootElement: SVGEllipseElement;
    pointEles: SVGCircleElement[] =[];
    moving: boolean = false;
    startX = 0;
    startY = 0;

    get rect() {
        var myrect = {
            x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("rx")),
            y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("ry")),
            width: parseInt(this.rootElement.getAttribute("rx")) * 2,
            height: parseInt(this.rootElement.getAttribute("ry")) * 2,
        };
        return myrect;
    }

    get strokeWidth() {
        return this.rootElement.style.strokeWidth;
    }
    set strokeWidth(v) {
        this.rootElement.style.strokeWidth = v;
    }
    get colorStroke() {
        return this.rootElement.style.stroke;
    }
    set colorStroke(v) {
        this.rootElement.style.stroke = v;
    }
    get colorFill() {
        return this.rootElement.style.fill;
    }
    set colorFill(v) {
        this.rootElement.style.fill = v;
    }

    getPropertiesCaption(): string[] {
        return ["边框大小", "边框颜色", "填充颜色"];
    }
    getProperties(): string[] {
        return ["strokeWidth", "colorStroke", "colorFill"];
    }

    constructor(element: any) {
        super(element);
        this.rootElement = element;
    }

    isIntersectWith(rect): boolean {

        return this.isIntersect(this.rect, rect);
    }

    onSelectedChange() {
        if (this.selected) {

            var pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pRightBottom.setAttribute("r", "5");
            pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            (<any>pRightBottom)._moveFunc = (ele, x, y) => {
                (<any>this.rootElement).setAttribute("rx",Math.max(5, ele._value_rx + (x - ele._startX)));
                (<any>this.rootElement).setAttribute("ry", Math.max(5, ele._value_ry + (y - ele._startY)));
            }
            this.rootElement.parentElement.appendChild(pRightBottom);
            this.pointEles.push(pRightBottom);

            var pCenterBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pCenterBottom.setAttribute("r", "5");
            pCenterBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ns-resize;');
            (<any>pCenterBottom)._moveFunc = (ele, x, y) => {
                (<any>this.rootElement).setAttribute("ry", Math.max(5, ele._value_ry + (y - ele._startY)));
            }
            this.rootElement.parentElement.appendChild(pCenterBottom);
            this.pointEles.push(pCenterBottom);

            for (var i = 0; i < this.pointEles.length; i++)
            {
                this.setEvent(this.pointEles[i]);
            }
            
            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.rootElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    }

    resetPointLocation() {
        this.pointEles[0].setAttribute("cx", <any>(this.rootElement.cx.animVal.value + this.rootElement.rx.animVal.value));
        this.pointEles[0].setAttribute("cy", <any>(this.rootElement.cy.animVal.value + this.rootElement.ry.animVal.value));

        this.pointEles[1].setAttribute("cx", <any>(this.rootElement.cx.animVal.value));
        this.pointEles[1].setAttribute("cy", <any>(this.rootElement.cy.animVal.value + this.rootElement.ry.animVal.value));
    }

    setEvent(pointEle) {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", (e) => { this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", (e) => { this.pointMouseUp(e, pointEle); }, false);
    }

    pointMouseDown(e: MouseEvent, pointEle) {
        e.stopPropagation();
        this.moving = true;
        pointEle._startX = e.clientX;
        pointEle._startY = e.clientY;

        pointEle._value_cx = this.rootElement.cx.animVal.value;
        pointEle._value_cy = this.rootElement.cy.animVal.value;
        pointEle._value_rx = this.rootElement.rx.animVal.value;
        pointEle._value_ry = this.rootElement.ry.animVal.value;

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    pointMouseMove(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();

            pointEle._moveFunc(pointEle, e.clientX, e.clientY);
            this.resetPointLocation();
        }
    }
    pointMouseUp(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    }


    onBeginMoving() {
        (<any>this.rootElement)._cx = parseInt(this.rootElement.getAttribute("cx"));
        (<any>this.rootElement)._cy = parseInt(this.rootElement.getAttribute("cy"));
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.rootElement)._cx + nowX - downX);
        var y = <any>((<any>this.rootElement)._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

class CircleControl extends EditorControl {
    rootElement: SVGCircleElement;
    pointEles: SVGCircleElement[] = [];
    moving: boolean = false;
    startX = 0;
    startY = 0;

    get rect() {
        var myrect = {
            x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("r")),
            y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("r")),
            width: parseInt(this.rootElement.getAttribute("r")) * 2,
            height: parseInt(this.rootElement.getAttribute("r")) * 2,
        };
        return myrect;
    }

    get strokeWidth() {
        return this.rootElement.style.strokeWidth;
    }
    set strokeWidth(v) {
        this.rootElement.style.strokeWidth = v;
    }
    get colorStroke() {
        return this.rootElement.style.stroke;
    }
    set colorStroke(v) {
        this.rootElement.style.stroke = v;
    }
    get colorFill() {
        return this.rootElement.style.fill;
    }
    set colorFill(v) {
        this.rootElement.style.fill = v;
    }

    getPropertiesCaption(): string[] {
        return ["边框大小", "边框颜色", "填充颜色"];
    }
    getProperties(): string[] {
        return ["strokeWidth", "colorStroke", "colorFill"];
    }

    constructor(element: any) {
        super(element);
        this.rootElement = element;
    }

    isIntersectWith(rect): boolean {

        return this.isIntersect(this.rect, rect);
    }

    onSelectedChange() {
        if (this.selected) {

            var pRightBottom = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pRightBottom.setAttribute("r", "5");
            pRightBottom.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:nwse-resize;');
            (<any>pRightBottom)._moveFunc = (ele, x, y) => {
                (<any>this.rootElement).setAttribute("r", Math.max(5, ele._value_r + (x - ele._startX)));
            }
            this.rootElement.parentElement.appendChild(pRightBottom);
            this.pointEles.push(pRightBottom);


            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i]);
            }

            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.rootElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    }

    resetPointLocation() {
        this.pointEles[0].setAttribute("cx", <any>(this.rootElement.cx.animVal.value + this.rootElement.r.animVal.value));
        this.pointEles[0].setAttribute("cy", <any>(this.rootElement.cy.animVal.value + this.rootElement.r.animVal.value));
    }

    setEvent(pointEle) {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", (e) => { this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", (e) => { this.pointMouseUp(e, pointEle); }, false);
    }

    pointMouseDown(e: MouseEvent, pointEle) {
        e.stopPropagation();
        this.moving = true;
        pointEle._startX = e.clientX;
        pointEle._startY = e.clientY;

        pointEle._value_cx = this.rootElement.cx.animVal.value;
        pointEle._value_cy = this.rootElement.cy.animVal.value;
        pointEle._value_r = this.rootElement.r.animVal.value;

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    pointMouseMove(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();

            pointEle._moveFunc(pointEle, e.clientX, e.clientY);
            this.resetPointLocation();
        }
    }
    pointMouseUp(e: MouseEvent, pointEle) {
        if (this.moving) {
            e.stopPropagation();
            this.moving = false;
            pointEle.releaseCapture();
        }
    }


    onBeginMoving() {
        (<any>this.rootElement)._cx = parseInt(this.rootElement.getAttribute("cx"));
        (<any>this.rootElement)._cy = parseInt(this.rootElement.getAttribute("cy"));
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.rootElement)._cx + nowX - downX);
        var y = <any>((<any>this.rootElement)._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}