var AllSelectedControls: EditorControl[] = [];
declare var editor: Editor;
var ManyPointDefined = 999999;//表示EditorControl是否绑定了多个point
var WatchPointNames: any = [];

function documentElementMouseDown(e: MouseEvent) {
    //while (AllSelectedControls.length > 0) {
    //    AllSelectedControls[0].selected = false;
    //}
}


document.documentElement.addEventListener("mousedown", documentElementMouseDown, false);


class EditorControl
{
    container: IEditorControlContainer;
    propertyDialog: PropertyDialog;
    pointEles: SVGElement[] = [];
    ctrlKey = false;
    isInGroup = false;
    lastSetValueTime: number = 0;
    updatePointValueTimeoutFlag: number = 0;
    isDesignMode = true;
    element: any;
    _selected: boolean = false;
    _moveAllSelectedControl = false;
    get selected(): boolean {
        return this._selected;
    }
    set selected(value: boolean) {
        if (this._selected !== value && this.isDesignMode) {
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
    set rect(v)
    {

    }

    _id: string;
    get id() {
        return this._id;
    }
    set id(v) {
        this._id = v;
    }

    private mouseDownX;
    private mouseDownY;
    private undoMoveObj: UndoMoveControls;

    constructor(element:any)
    {
        this.element = element;
        element._editorControl = this;
        (<HTMLElement>this.element).addEventListener("dragstart", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;
            e.preventDefault();
        }, false);

        (<HTMLElement>this.element).addEventListener("click", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;
            e.stopPropagation();           
        }, false);

        (<HTMLElement>this.element).addEventListener("dblclick", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;
            e.stopPropagation();
            this.showProperty();
        }, false);        

        (<HTMLElement>this.element).addEventListener("mousedown", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;

            if (e.button == 2)
                return;
            this._moveAllSelectedControl = this.selected;

            e.stopPropagation();

            this.ctrlKey = e.ctrlKey;
            if (this.ctrlKey)
                this.selected = !this.selected;
            else
                this.selected = true;

            this.mouseDownX = e.layerX;
            this.mouseDownY = e.layerY;
            var movingCtrls = [];
            if (this._moveAllSelectedControl) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    AllSelectedControls[i].onBeginMoving();
                    movingCtrls.push(AllSelectedControls[i]);
                }
            }
            else {
                this.onBeginMoving();
                movingCtrls.push(this);
            }

            this.undoMoveObj = new UndoMoveControls(editor, movingCtrls);
        }, false);
        document.body.addEventListener("mousemove", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;

            if (this.mouseDownX >= 0) {
                e.stopPropagation();
                if (this._moveAllSelectedControl) {
                    for (var i = 0; i < AllSelectedControls.length; i++)
                    {
                        AllSelectedControls[i].onMoving(this.mouseDownX, this.mouseDownY, e.layerX, e.layerY);
                    }
                }
                else {
                    this.onMoving(this.mouseDownX, this.mouseDownY, e.layerX, e.layerY);
                }
            }
        }, false);
        document.body.addEventListener("mouseup", (e) => {
            if (this.isInGroup || !this.container || !this.isDesignMode)
                return;

            if (this.mouseDownX >= 0) {
                e.stopPropagation();
                this.onEndMoving();
                this.mouseDownX = -1;
                this.undoMoveObj.moveFinish();
                editor.undoMgr.addUndo(this.undoMoveObj);
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

    /**
     * 正式环境中运行模式
     */
    run()
    {
        this.isDesignMode = false;
    }

    /**
     * 当关联的设备点值方式变化时触发
     * @param devPoint
     */
    onDevicePointValueChanged(devPoint: any) {

    }

    /**
     * 获取描述本控件属性的一个json对象
     */
    getJson()
    {
        var obj = {
            rect: this.rect,
            constructorName: (<any>this).constructor.name
        };
        var properites = this.getProperties();
        for (var i = 0; i < properites.length; i++)
        {
            obj[properites[i]] = this[properites[i]];
        }
        return obj;
    }

    /**
     * 获取运行时的执行脚本
     */
    getScript()
    {
        var json = this.getJson();
        var script = "";
        var id = this.id;
        if (!id || id.length == 0)
        {
            id = "eCtrl";
        }
        script += id + " = new " + json.constructorName + "();\r\n";
        script += "editor.addControl(" + id + ");\r\n";
        for (var proName in json)
        {
            if (proName == "rect" || proName == "constructorName")
                continue;
            var type = typeof json[proName];
            if (type == "function" || type == "undefined")
                continue;
            script += id + "." + proName + " = " + JSON.stringify(json[proName]) + ";\r\n";
        }
        script += id + ".rect = " + JSON.stringify(json.rect) + ";\r\n";       
        return script;
    }

    isIntersectWith(rect):boolean
    {
        return this.isIntersect(this.rect, rect);
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

    onSelectedChange() {
        if (this.selected) {

            //左上角
            var pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nwse-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height - (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x - 3));
                ele.setAttribute("y", <any>(rect.y - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //上角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ns-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width,
                    height: ele._value_rect.height - (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width / 2 - 3));
                ele.setAttribute("y", <any>(rect.y - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //右上角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nesw-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height - (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width - 3));
                ele.setAttribute("y", <any>(rect.y - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //右角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height,
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height / 2 - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //右下角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nwse-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height + (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //下角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ns-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width,
                    height: ele._value_rect.height + (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width / 2 - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //左下角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nesw-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y,
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height + (y - ele._startY),
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);

            //左角
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y,
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height,
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height / 2 - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);


            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i]);
            }

            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.element.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
        }
    }

    resetPointLocation() {
        if (!this.selected)
            return;
        var rect = this.rect;
        for (var i = 0; i < this.pointEles.length; i++) {
            (<any>this.pointEles[i])._setLocation(this.pointEles[i], rect);
        }
    }

    setEvent(pointEle) {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this.pointMouseDown(e, pointEle); }, false);
        pointEle.addEventListener("mousemove", (e) => { this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", (e) => { this.pointMouseUp(e, pointEle); }, false);
    }

    private undoObj: UndoMoveControls;
    private movingPoint: boolean = false;
    pointMouseDown(e: MouseEvent, pointEle) {
        e.stopPropagation();
        this.movingPoint = true;
        pointEle._startX = e.layerX;
        pointEle._startY = e.layerY;

        pointEle._value_rect = this.rect;

        this.undoObj = new UndoMoveControls(editor, [this]);

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    pointMouseMove(e: MouseEvent, pointEle) {
        if (this.movingPoint) {
            e.stopPropagation();

            pointEle._moveFunc(pointEle, e.layerX, e.layerY);
            this.resetPointLocation();
        }
    }
    pointMouseUp(e: MouseEvent, pointEle) {
        if (this.movingPoint) {
            e.stopPropagation();
            this.movingPoint = false;
            pointEle.releaseCapture();

            this.undoObj.moveFinish();
            editor.undoMgr.addUndo(this.undoObj);
        }
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
    virtualLineElement: SVGLineElement;

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
    set rect(v) {

        var x = Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2")));
        var y = Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2")));
        this.lineElement.setAttribute("x1", <any>(parseInt(this.lineElement.getAttribute("x1")) + v.x - x));
        this.lineElement.setAttribute("x2", <any>(parseInt(this.lineElement.getAttribute("x2")) + v.x - x));

        this.lineElement.setAttribute("y1", <any>(parseInt(this.lineElement.getAttribute("y1")) + v.y - y));
        this.lineElement.setAttribute("y2", <any>(parseInt(this.lineElement.getAttribute("y2")) + v.y - y));


        var height = Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2")));
        if (parseInt(this.lineElement.getAttribute("y1")) < parseInt(this.lineElement.getAttribute("y2"))) {
            var y2 :any = parseInt(this.lineElement.getAttribute("y2")) + v.height - height;
            this.lineElement.setAttribute("y2" , y2);
        }
        else {
            var y1: any = parseInt(this.lineElement.getAttribute("y1")) + v.height - height;
            this.lineElement.setAttribute("y1", y1);
        }

        var width = Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2")));
        if (parseInt(this.lineElement.getAttribute("x1")) < parseInt(this.lineElement.getAttribute("x2"))) {
            var x2: any = parseInt(this.lineElement.getAttribute("x2")) + v.width - width;
            this.lineElement.setAttribute("x2", x2);
        }
        else {
            var x1: any = parseInt(this.lineElement.getAttribute("x1")) + v.width - width;
            this.lineElement.setAttribute("x1", x1);
        }


        this.virtualLineElement.setAttribute("x1", this.lineElement.getAttribute("x1"));
        this.virtualLineElement.setAttribute("x2", this.lineElement.getAttribute("x2"));
        this.virtualLineElement.setAttribute("y1", this.lineElement.getAttribute("y1"));
        this.virtualLineElement.setAttribute("y2", this.lineElement.getAttribute("y2"));
        this.resetPointLocation();
    }

    get point()
    {
        return {
            x1: parseInt(this.lineElement.getAttribute("x1")),
            x2: parseInt(this.lineElement.getAttribute("x2")),
            y1: parseInt(this.lineElement.getAttribute("y1")),
            y2: parseInt(this.lineElement.getAttribute("y2")),
        };
    }
    set point(v:any)
    {
        this.lineElement.setAttribute("x1", v.x1);
        this.lineElement.setAttribute("x2", v.x2);
        this.lineElement.setAttribute("y1", v.y1);
        this.lineElement.setAttribute("y2", v.y2);

        this.virtualLineElement.setAttribute("x1", v.x1);
        this.virtualLineElement.setAttribute("x2", v.x2);
        this.virtualLineElement.setAttribute("y1", v.y1);
        this.virtualLineElement.setAttribute("y2", v.y2);
    }

    getJson() {
        var obj: any = super.getJson();
        obj.point = this.point;
        return obj;
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

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'g'));

        this.lineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        this.element.appendChild(this.lineElement);
        this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;');

        //virtualLineElement是透明的，主要用来增大LineControl的点击面积
        this.virtualLineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        this.element.appendChild(this.virtualLineElement);
        this.virtualLineElement.setAttribute('style', 'stroke:red;stroke-opacity:0;stroke-width:10;cursor:pointer;');
    }

    getPropertiesCaption(): string[] {
        return ["id","线宽","颜色"];
    }
    getProperties(): string[] {
        return ["id","lineWidth", "color"];
    }

    isIntersectWith(rect): boolean {
        var myrect = this.rect;
        return this.isIntersect(myrect , rect);
    }


    onSelectedChange() {
        if (this.selected) {
            var pointEle1 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle1.setAttribute("r", "5");
            pointEle1.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            (<any>pointEle1).xName = "x1";
            (<any>pointEle1).yName = "y1";
            this.lineElement.parentElement.appendChild(pointEle1);

            var pointEle2 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle2.setAttribute("r", "5");
            pointEle2.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            (<any>pointEle2).xName = "x2";
            (<any>pointEle2).yName = "y2";
            this.lineElement.parentElement.appendChild(pointEle2);

            this.pointEles.push(pointEle1);
            this.pointEles.push(pointEle2);

            this.resetPointLocation();

            for (var i = 0; i < this.pointEles.length; i++) {
                this.mySetEvent(this.pointEles[i], "x" + (i + 1), "y" + (i + 1));
            }
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.lineElement.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];


        }
    }

    resetPointLocation() {
        if (!this.selected)
            return;

        this.pointEles[0].setAttribute("cx", <any>this.lineElement.x1.animVal.value);
        this.pointEles[0].setAttribute("cy", <any>this.lineElement.y1.animVal.value);

        this.pointEles[1].setAttribute("cx", <any>this.lineElement.x2.animVal.value);
        this.pointEles[1].setAttribute("cy", <any>this.lineElement.y2.animVal.value);
    }

    mySetEvent(pointEle, xName, yName) {
        pointEle.addEventListener("click", (e: Event) => { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", (e) => { this._pointMouseDown(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mousemove", (e) => { this._pointMouseMove(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mouseup", (e) => { this._pointMouseUp(e, pointEle); }, false);
    }

    private _undoObj: UndoChangeLinePoint = null;

    _pointMouseDown(e: MouseEvent, pointEle, xName: string, yName: string) {
        e.stopPropagation();

        this.startX = e.layerX;
        this.startY = e.layerY;
        this.valueX = parseInt(this.lineElement.getAttribute(xName));
        this.valueY = parseInt(this.lineElement.getAttribute(yName));

        this._undoObj = new UndoChangeLinePoint(editor, this, xName, yName);

        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            (<any>window).captureEvents((<any>Event).MOUSEMOVE | (<any>Event).MOUSEUP);
    }
    _pointMouseMove(e: MouseEvent, pointEle, xName: string, yName: string) {
        if (this._undoObj) {
            e.stopPropagation();
            pointEle.setAttribute("cx", this.valueX + e.layerX - this.startX);
            pointEle.setAttribute("cy", this.valueY + e.layerY - this.startY);
            this.lineElement.setAttribute(xName, <any>(this.valueX + e.layerX - this.startX));
            this.lineElement.setAttribute(yName, <any>(this.valueY + e.layerY - this.startY));

            this.virtualLineElement.setAttribute(xName, <any>(this.valueX + e.layerX - this.startX));
            this.virtualLineElement.setAttribute(yName, <any>(this.valueY + e.layerY - this.startY));
        }
    }
    _pointMouseUp(e: MouseEvent, pointEle) {
        if (this._undoObj) {
            e.stopPropagation();
            pointEle.releaseCapture();

            this._undoObj.moveFinish();
            editor.undoMgr.addUndo(this._undoObj);
            this._undoObj = null;
        }
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

        this.virtualLineElement.setAttribute("x1", x1);
        this.virtualLineElement.setAttribute("y1", y1);
        this.virtualLineElement.setAttribute("x2", x2);
        this.virtualLineElement.setAttribute("y2", y2);
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

}

class RectControl extends EditorControl {
    rectElement: SVGGraphicsElement;

    startX = 0;
    startY = 0;

    get rect() {
        return {
            x: parseInt(this.rectElement.getAttribute("x")),
            y: parseInt(this.rectElement.getAttribute("y")),
            width: parseInt(this.rectElement.getAttribute("width")),
            height: parseInt(this.rectElement.getAttribute("height")),
        };
    }
    set rect(v:any) {
        this.rectElement.setAttribute("x", v.x);
        this.rectElement.setAttribute("y", v.y);
        this.rectElement.setAttribute("width", v.width);
        this.rectElement.setAttribute("height", v.height);
        this.resetPointLocation();
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

    _devicePoint: string = "";
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }

    _scriptOnValueChange: string;
    get scriptOnValueChange() {
        return this._scriptOnValueChange;
    }
    set scriptOnValueChange(v) {
        this._scriptOnValueChange = v;
    }

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(devPoint: any) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0)
        {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e)
            {
                alert(e.message);
            }
        }
    }

    getPropertiesCaption(): string[] {
        return ["id","边框大小", "边框颜色", "填充颜色","设备点", "值变化脚本"];
    }
    getProperties(): string[] {
        return ["id","strokeWidth", "colorStroke", "colorFill","devicePoint", "scriptOnValueChange"];
    }

    constructor(element:any) {
        super(element ? element : document.createElementNS('http://www.w3.org/2000/svg', 'rect'));
        this.rectElement = this.element;
        this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
    }

    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
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
    set rect(v: any) {
        var rx : any = v.width / 2;
        var ry : any = v.height / 2;
        this.rootElement.setAttribute("cx", v.x + rx);
        this.rootElement.setAttribute("cy", v.y + ry);
        this.rootElement.setAttribute("rx", rx);
        this.rootElement.setAttribute("ry", ry);

        this.resetPointLocation();
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

    _devicePoint: string = "";
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }

    _scriptOnValueChange: string;
    get scriptOnValueChange() {
        return this._scriptOnValueChange;
    }
    set scriptOnValueChange(v) {
        this._scriptOnValueChange = v;
    }

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(devPoint: any) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0) {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e) {
                alert(e.message);
            }
        }
    }

    getPropertiesCaption(): string[] {
        return ["id","边框大小", "边框颜色", "填充颜色","设备点","值变化脚本"];
    }
    getProperties(): string[] {
        return ["id","strokeWidth", "colorStroke", "colorFill", "devicePoint", "scriptOnValueChange"];
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'ellipse'));
        this.rootElement = this.element;
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
    }

    isIntersectWith(rect): boolean {

        return this.isIntersect(this.rect, rect);
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
    set rect(v: any) {
        var r: any = v.width / 2;
        this.rootElement.setAttribute("cx", v.x + r);
        this.rootElement.setAttribute("cy", v.y + r);
        this.rootElement.setAttribute("r", r);

        this.resetPointLocation();
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

    _devicePoint: string = "";
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }

    _scriptOnValueChange: string;
    get scriptOnValueChange() {
        return this._scriptOnValueChange;
    }
    set scriptOnValueChange(v) {
        this._scriptOnValueChange = v;
    }

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(devPoint: any) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0) {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e) {
                alert(e.message);
            }
        }
    }

    getPropertiesCaption(): string[] {
        return ["id","边框大小", "边框颜色", "填充颜色","设备点","值变化脚本"];
    }
    getProperties(): string[] {
        return ["id","strokeWidth", "colorStroke", "colorFill", "devicePoint", "scriptOnValueChange"];
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'circle'));
        this.rootElement = this.element;
        this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
    }

    isIntersectWith(rect): boolean {

        return this.isIntersect(this.rect, rect);
    }

    onSelectedChange() {
        if (this.selected) {
           
            //右角
            var pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            (<any>pointEle)._moveFunc = (ele, x, y) => {
                this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height,
                }
            }
            (<any>pointEle)._setLocation = (ele, rect) => {
                ele.setAttribute("x", <any>(rect.x + rect.width - 3));
                ele.setAttribute("y", <any>(rect.y + rect.height / 2 - 3));
            }
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            

            for (var i = 0; i < this.pointEles.length; i++) {
                this.setEvent(this.pointEles[i]);
            }

            this.resetPointLocation();
        }
        else {
            for (var i = 0; i < this.pointEles.length; i++) {
                this.element.parentElement.removeChild(this.pointEles[i]);
            }
            this.pointEles = [];
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

class ImageControl extends RectControl {
    imgElement: SVGImageElement;
    get imgSrc()
    {
        return this.imgElement.href.baseVal;
    }
    set imgSrc(v)
    {
        this.imgElement.href.baseVal = v;
    }

    getPropertiesCaption(): string[] {
        return ["id","图片"];
    }
    getProperties(): string[] {
        return ["id","imgSrc"];
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'image'));
        this.imgElement = this.element;
    }

}

class TextControl extends EditorControl {
    groupElement: SVGGElement;
    textElement: SVGTextElement;
    //选择状态下的边框
    selectingElement: SVGRectElement;


    get text() {
        return this.textElement.textContent;
    }
    set text(v) {
        if (v != this.textElement.textContent) {
            this.textElement.textContent = v;
            this.resetPointLocation();
        }
    }
    get size() {
        return parseInt(this.textElement.getAttribute("font-size"));
    }
    set size(v) {
        this.textElement.setAttribute("font-size", <any>v);
        this.resetPointLocation();
    }
    get colorFill() {
        return this.textElement.style.fill;
    }
    set colorFill(v) {
        this.textElement.style.fill = v;
    }

    _canSetValue = false;
    get canSetValue() {
        return this._canSetValue;
    }
    set canSetValue(v: boolean) {
        this._canSetValue = v;
    }

    _devicePoint: string = "";
    _lastDevPoint: any;
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    onDevicePointValueChanged(devPoint: any) {
        
        this._lastDevPoint = devPoint;
        this.updateText(devPoint.value);
    }
    updateText(value:any)
    {
        this.text = value;
    }

    private _showedPrompt: boolean = false;
    run() {
        super.run();
        if (this.devicePoint.length > 0 && this.canSetValue) {
            this.textElement.style.cursor = "pointer";
            (<HTMLElement>this.element).addEventListener("click", (e) => {
                e.stopPropagation();

                if (!this._showedPrompt) {
                    this._showedPrompt = true;
                    var newValue: any = window.prompt("请输入新的数值", "");
                    this._showedPrompt = false;
                    if (newValue && newValue.length > 0) {
                        var valueType = typeof this._lastDevPoint.value;
                        if (valueType == "number") {
                            if (newValue.indexOf(".") >= 0)
                                newValue = parseFloat(newValue);
                            else
                                newValue = parseInt(newValue);
                        }
                        if (this._lastDevPoint.value != newValue) {
                            //往设备写入值
                            this.container.writeValue(this.devicePoint, this._lastDevPoint.addr, newValue);
                            this.lastSetValueTime = new Date().getTime();
                            this.updateText(newValue);
                        }

                    }
                }
            }, false);
        }
    }

    getPropertiesCaption(): string[] {
        return ["id", "文字", "大小", "颜色", "旋转角度", "字体","粗体","斜体","下划线","设备点","运行时允许输入值"];
    }

    getProperties(): string[] {
        return ["id", "text", "size", "colorFill", "rotate","fontFamily", "isBold", "isItalic", "isUnderline", "devicePoint","canSetValue"];
    }

    get rect() {
        var transform = this.groupElement.getAttribute("transform");
        var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);

        try {
            var clientRect = this.textElement.getBoundingClientRect();
            //var myrect: SVGRect = (<any>this.groupElement).getBBox();

            return {
                x: parseInt(result[1]),
                y: parseInt(result[2]),
                width: clientRect.width,
                height: clientRect.height
            };
        }
        catch (e)
        {
            return {
                x: parseInt(result[1]),
                y: parseInt(result[2]),
                width: 0,
                height: 0
            };
        }
    }
    set rect(v: any) {
        var x = v.x;
        var y = v.y ;

        this.groupElement.setAttribute("transform", "translate(" + x + " " + y +") scale(1 1)");

        this.resetPointLocation();
    }

    get rotate()
    {
        var transform = this.textElement.getAttribute("transform");
        var result = /rotate\(([0-9]+) /.exec(transform);
        return parseInt(result[1]);
    }
    set rotate(v: any)
    {
        this.textElement.setAttribute("transform", "rotate(" + v + " 0,17)");
        this.resetPointLocation();
    }

    get isUnderline() {
       return this.textElement.getAttribute("text-decoration") === "underline";
    }
    set isUnderline(v: boolean) {
        this.textElement.setAttribute("text-decoration", v ?"underline":"");
    }
    get isBold() {
        return this.textElement.getAttribute("font-weight") === "900";
    }
    set isBold(v: boolean) {
        this.textElement.setAttribute("font-weight", v ? "900" : "");
    }
    get isItalic() {
        return this.textElement.getAttribute("font-style") === "italic";
    }
    set isItalic(v: boolean) {
        this.textElement.setAttribute("font-style", v ? "italic" : "");
    }

    get fontFamily() {
        return this.textElement.getAttribute("font-family");
    }
    set fontFamily(v: string) {
        this.textElement.setAttribute("font-family", v );
    }
    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'g'));
        this.groupElement = this.element;
        this.groupElement.setAttribute("transform", "translate(0 0) scale(1 1)");

        this.textElement = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        this.groupElement.appendChild(this.textElement);
        this.textElement.textContent = "Text";
        this.textElement.setAttribute("x", "0");
        this.textElement.setAttribute("y", "17");
        this.textElement.setAttribute('style', 'fill:#111111;cursor:default;-moz-user-select:none;');
        this.textElement.setAttribute('font-size', "16");
        this.textElement.setAttribute("transform", "rotate(0 0,17)");
    }

    isIntersectWith(rect): boolean {
        var clientRect = this.textElement.getBoundingClientRect();
        var myrect = { x: clientRect.left, y: clientRect.top - editor.divContainer.offsetTop, width: clientRect.width, height: clientRect.height };

        return this.isIntersect(myrect, rect);
    }

    onSelectedChange() {
        if (this.selected) {
            var clientRect = this.textElement.getBoundingClientRect();
            var myrect = { x: clientRect.left, y: clientRect.top - editor.divContainer.offsetTop, width: clientRect.width, height: clientRect.height };

            this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            this.selectingElement.setAttribute('x', <any>(myrect.x - 5));
            this.selectingElement.setAttribute('y', <any>(myrect.y - 5));
            this.selectingElement.setAttribute('width', <any>(myrect.width + 10));
            this.selectingElement.setAttribute('height', <any>(myrect.height + 10));
            this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
            this.selectingElement.onmousedown = (e) => {
                e.stopPropagation();
            };
            this.groupElement.parentElement.appendChild(this.selectingElement);
        }
        else {
            this.groupElement.parentElement.removeChild(this.selectingElement);
        }
    }

    resetPointLocation() {
        if (!this.selected)
            return;

        var clientRect = this.textElement.getBoundingClientRect();
        var myrect = { x: clientRect.left, y: clientRect.top - editor.divContainer.offsetTop, width: clientRect.width, height: clientRect.height };

        this.selectingElement.setAttribute('x', <any>(myrect.x - 5));
        this.selectingElement.setAttribute('y', <any>(myrect.y - 5));
        this.selectingElement.setAttribute('width', <any>(myrect.width + 10));
        this.selectingElement.setAttribute('height', <any>(myrect.height + 10));
    }   

    onBeginMoving() {
        var rect = this.rect;
        (<any>this.groupElement)._x = rect.x;
        (<any>this.groupElement)._y = rect.y;
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.groupElement)._x + nowX - downX);
        var y = <any>((<any>this.groupElement)._y + nowY - downY);
        this.rect = { x: x, y: y };
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

class CylinderControl extends EditorControl {
    private _value: number = 0;
    private _max: number = 100;
    private _min: number = 0;
    get value()
    {
        return this._value;
    }
    set value(v: any)
    {
        v = parseFloat(v);
        if (v != this._value) {
            this._value = v;
            this.resetCylinder(this.rect);
        }
    }

    get max() {
        return this._max;
    }
    set max(v: any) {

        this._max = parseFloat(v);
        if (this._max <= this._min)
            this._max = this._min + 1;
        try {
            this.resetCylinder(this.rect);
        }
        catch (e)
        {

        }
    }
    get min() {
        return this._min;
    }
    set min(v: any) {
        this._min = parseFloat(v);
        if (this._min >= this._max)
            this._max = this._max - 1;
        try {
            this.resetCylinder(this.rect);
        }
        catch (e) {

        }
    }

    _devicePoint: string = "";
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    onDevicePointValueChanged(devPoint: any) {
        if (devPoint.max != null && devPoint.max != this.max)
            this.max = devPoint.max;
        if (devPoint.min != null && devPoint.min != this.min)
            this.min = devPoint.min;

        this.value = devPoint.value;
    }

    rectElement: SVGRectElement;
    cylinderElement: SVGRectElement;

    startX = 0;
    startY = 0;

    get rect() {        

        return {
            x: parseInt(this.rectElement.getAttribute("x")),
            y: parseInt(this.rectElement.getAttribute("y")),
            width: parseInt(this.rectElement.getAttribute("width")),
            height: parseInt(this.rectElement.getAttribute("height")),
        };
        
    }
    set rect(v: any) {
        this.rectElement.setAttribute("x", v.x);
        this.rectElement.setAttribute("y", v.y);
        this.rectElement.setAttribute("width", v.width);
        this.rectElement.setAttribute("height", v.height);

        this.resetCylinder(v);
        this.resetPointLocation();
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
        return this.cylinderElement.style.fill;
    }
    set colorFill(v) {
        this.cylinderElement.style.fill = v;
    }
    get colorBg() {
        return this.rectElement.style.fill;
    }
    set colorBg(v) {
        this.rectElement.style.fill = v;
    }
    getPropertiesCaption(): string[] {
        return ["id","边框大小", "边框颜色", "底色", "填充颜色","值","最大值","最小值","设备点"];
    }
    getProperties(): string[] {
        return ["id","strokeWidth", "colorStroke", "colorBg", "colorFill", "value", "max", "min", "devicePoint"];
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'g'));

        this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        this.rectElement.setAttribute("rx", "200000");
        this.rectElement.setAttribute("ry", "20");
        this.rectElement.setAttribute('width', "0");
        this.rectElement.setAttribute('height', "0");
        this.rectElement.setAttribute('style', 'fill:#ffffff;stroke:#aaaaaa;stroke-width:1;');

        this.cylinderElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        this.cylinderElement.setAttribute("rx", "6");
        this.cylinderElement.setAttribute("ry", "6");
        this.cylinderElement.setAttribute("height", "0");
        this.cylinderElement.setAttribute('style', 'fill:#00BF00;stroke:none;');

        this.element.appendChild(this.rectElement);
        this.element.appendChild(this.cylinderElement);
    }

    resetCylinder(rect:any)
    {
        
        var ctrlHeight = rect.height - 40;
        var myheight: any = parseInt(<any>(((this.value - this.min) * ctrlHeight) / (this.max - this.min)));
       
        myheight = Math.min(ctrlHeight, myheight);
        if (myheight < 0)
            myheight = 0;
                
        this.cylinderElement.setAttribute("x", rect.x + 10);
        this.cylinderElement.setAttribute("y", <any>(rect.y + 20 + ctrlHeight - myheight));
        this.cylinderElement.setAttribute("width", <any>(rect.width - 20));
        this.cylinderElement.setAttribute("height", myheight);
    }

    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
    }

    onBeginMoving() {
        (<any>this.rectElement)._rect = this.rect;
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.rectElement)._rect.x + nowX - downX);
        var y = <any>((<any>this.rectElement)._rect.y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);

        this.resetCylinder({ x: x, y: y, width: (<any>this.rectElement)._rect.width, height: (<any>this.rectElement)._rect.height});
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

class TrendControl extends EditorControl {
    rectElement: SVGRectElement;

    line_left_Ele: SVGLineElement;
    line_bottom_Ele: SVGLineElement;
    pathElement1: SVGPathElement;
    pathElement2: SVGPathElement;
    pathElement3: SVGPathElement;
    pathElement4: SVGPathElement;
    pathElement5: SVGPathElement;
    pathElement6: SVGPathElement;
    pathElement7: SVGPathElement;
    pathElement8: SVGPathElement;
    pathElement9: SVGPathElement;
    pathElement10: SVGPathElement;
    pathElement11: SVGPathElement;
    pathElement12: SVGPathElement;
    
    private _max: number;
    private _min: number;

    values1: any[] = [];
    values2: any[] = [];
    values3: any[] = [];
    values4: any[] = [];
    values5: any[] = [];
    values6: any[] = [];
    values7: any[] = [];
    values8: any[] = [];
    values9: any[] = [];
    values10: any[] = [];
    values11: any[] = [];
    values12: any[] = [];
    private _value1: number = 0;
    get value1() {
        return this._value1;
    }
    set value1(v: any) {
        v = parseFloat(v);
        if (v != this._value1) {
            this._value1 = v;
            this.values1.push({
                value: this._value1,
                time: new Date().getTime()
            });
        }
    }
    
    private _value2: number = 0;
    get value2() {
        return this._value2;
    }
    set value2(v: any) {
        v = parseFloat(v);
        if (v != this._value2) {
            this._value2 = v;
            this.values2.push({
                value: this._value2,
                time: new Date().getTime()
            });
        }
    }
    private _value3: number = 0;
    get value3() {
        return this._value3;
    }
    set value3(v: any) {
        v = parseFloat(v);
        if (v != this._value3) {
            this._value3 = v;
            this.values3.push({
                value: this._value3,
                time: new Date().getTime()
            });
        }
    }
    private _value4: number = 0;
    get value4() {
        return this._value4;
    }
    set value4(v: any) {
        v = parseFloat(v);
        if (v != this._value4) {
            this._value4 = v;
            this.values4.push({
                value: this._value4,
                time: new Date().getTime()
            });
        }
    }
    private _value5: number = 0;
    get value5() {
        return this._value5;
    }
    set value5(v: any) {
        v = parseFloat(v);
        if (v != this._value5) {
            this._value5 = v;
            this.values5.push({
                value: this._value5,
                time: new Date().getTime()
            });
        }
    }
    private _value6: number = 0;
    get value6() {
        return this._value6;
    }
    set value6(v: any) {
        v = parseFloat(v);
        if (v != this._value6) {
            this._value6 = v;
            this.values6.push({
                value: this._value6,
                time: new Date().getTime()
            });
        }
    }
    private _value7: number = 0;
    get value7() {
        return this._value7;
    }
    set value7(v: any) {
        v = parseFloat(v);
        if (v != this._value7) {
            this._value7 = v;
            this.values7.push({
                value: this._value7,
                time: new Date().getTime()
            });
        }
    }
    private _value8: number = 0;
    get value8() {
        return this._value8;
    }
    set value8(v: any) {
        v = parseFloat(v);
        if (v != this._value8) {
            this._value8 = v;
            this.values8.push({
                value: this._value8,
                time: new Date().getTime()
            });
        }
    }
    private _value9: number = 0;
    get value9() {
        return this._value9;
    }
    set value9(v: any) {
        v = parseFloat(v);
        if (v != this._value9) {
            this._value9 = v;
            this.values9.push({
                value: this._value9,
                time: new Date().getTime()
            });
        }
    }
    private _value10: number = 0;
    get value10() {
        return this._value10;
    }
    set value10(v: any) {
        v = parseFloat(v);
        if (v != this._value10) {
            this._value10 = v;
            this.values10.push({
                value: this._value10,
                time: new Date().getTime()
            });
        }
    }
    private _value11: number = 0;
    get value11() {
        return this._value11;
    }
    set value11(v: any) {
        v = parseFloat(v);
        if (v != this._value11) {
            this._value11 = v;
            this.values11.push({
                value: this._value11,
                time: new Date().getTime()
            });
        }
    }
    private _value12: number = 0;
    get value12() {
        return this._value12;
    }
    set value12(v: any) {
        v = parseFloat(v);
        if (v != this._value12) {
            this._value12 = v;
            this.values12.push({
                value: this._value12,
                time: new Date().getTime()
            });
        }
    }
    get max() {
        return this._max;
    }
    set max(v: any) {
        this._max = parseFloat(v);
        if (this._max <= this._min)
            this._max = this._min + 1;
    }
    get min() {
        return this._min;
    }
    set min(v: any) {
        this._min = parseFloat(v);
        if (this._min >= this._max)
            this._max = this._max - 1;
    }

    _devicePoint1: string = "";
    get devicePoint1() {
        return this._devicePoint1;
    }
    set devicePoint1(v) {
        this._devicePoint1 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint2: string = "";
    get devicePoint2() {
        return this._devicePoint2;
    }
    set devicePoint2(v) {
        this._devicePoint2 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint3: string = "";
    get devicePoint3() {
        return this._devicePoint3;
    }
    set devicePoint3(v) {
        this._devicePoint3 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint4: string = "";
    get devicePoint4() {
        return this._devicePoint4;
    }
    set devicePoint4(v) {
        this._devicePoint4 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint5: string = "";
    get devicePoint5() {
        return this._devicePoint5;
    }
    set devicePoint5(v) {
        this._devicePoint5 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint6: string = "";
    get devicePoint6() {
        return this._devicePoint6;
    }
    set devicePoint6(v) {
        this._devicePoint6 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint7: string = "";
    get devicePoint7() {
        return this._devicePoint7;
    }
    set devicePoint7(v) {
        this._devicePoint7 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint8: string = "";
    get devicePoint8() {
        return this._devicePoint8;
    }
    set devicePoint8(v) {
        this._devicePoint8 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint9: string = "";
    get devicePoint9() {
        return this._devicePoint9;
    }
    set devicePoint9(v) {
        this._devicePoint9 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint10: string = "";
    get devicePoint10() {
        return this._devicePoint10;
    }
    set devicePoint10(v) {
        this._devicePoint10 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint11: string = "";
    get devicePoint11() {
        return this._devicePoint11;
    }
    set devicePoint11(v) {
        this._devicePoint11 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    _devicePoint12: string = "";
    get devicePoint12() {
        return this._devicePoint12;
    }
    set devicePoint12(v) {
        this._devicePoint12 = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }
    onDevicePointValueChanged(devPoint: any) {
        var number = 0;
        for (var i = 1; i <= 12; i++)
        {
            if (devPoint.name == this["devicePoint" + i])
            {
                number = i;
                break;
            }
        }
        if (number == 0)
            return;

        if (devPoint.max != null &&  (typeof this.max == "undefined" || isNaN(this.max)))
            this.max = devPoint.max;
        else if (devPoint.max != null && devPoint.max > this.max)
            this.max = devPoint.max;

        if (devPoint.max != null && (typeof this.min == "undefined" || isNaN(this.min)))
            this.min = devPoint.min;
        else if (devPoint.min != null && devPoint.min < this.min)
            this.min = devPoint.min;


        if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
            this["colorLine" + number] = devPoint["colorLine" + number];
        if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
            this["colorLine" + number] = "#ffffff";

        this["value" + number] = devPoint.value;
    }

    running: boolean = false;
    startX = 0;
    startY = 0;

    get rect() {
        return {
            x: parseInt(this.rectElement.getAttribute("x")),
            y: parseInt(this.rectElement.getAttribute("y")),
            width: parseInt(this.rectElement.getAttribute("width")),
            height: parseInt(this.rectElement.getAttribute("height")),
        };
    }
    set rect(v: any) {
        this.rectElement.setAttribute("x", v.x);
        this.rectElement.setAttribute("y", v.y);
        this.rectElement.setAttribute("width", v.width);
        this.rectElement.setAttribute("height", v.height);

        for (var i = 1; i <= 12; i ++)
            this["pathElement" + i].setAttribute("transform", "translate("+v.x+" "+v.y+")");

        this.resetPointLocation();
    }

    get colorFill() {
        return this.rectElement.style.fill;
    }
    set colorFill(v) {
        this.rectElement.style.fill = v;
    }

    get colorLineLeftBottom() {
        return this.line_left_Ele.style.stroke;
    }
    set colorLineLeftBottom(v) {
        this.line_left_Ele.style.stroke = v;
        this.line_bottom_Ele.style.stroke = v;
    }
    get colorLine1() {
        return this.pathElement1.style.stroke;
    }
    set colorLine1(v) {
        this.pathElement1.style.stroke = v;
    }
    get colorLine2() {
        return this.pathElement2.style.stroke;
    }
    set colorLine2(v) {
        this.pathElement2.style.stroke = v;
    }
    get colorLine3() {
        return this.pathElement3.style.stroke;
    }
    set colorLine3(v) {
        this.pathElement3.style.stroke = v;
    }
    get colorLine4() {
        return this.pathElement4.style.stroke;
    }
    set colorLine4(v) {
        this.pathElement4.style.stroke = v;
    }
    get colorLine5() {
        return this.pathElement5.style.stroke;
    }
    set colorLine5(v) {
        this.pathElement5.style.stroke = v;
    }
    get colorLine6() {
        return this.pathElement6.style.stroke;
    }
    set colorLine6(v) {
        this.pathElement6.style.stroke = v;
    }
    get colorLine7() {
        return this.pathElement7.style.stroke;
    }
    set colorLine7(v) {
        this.pathElement7.style.stroke = v;
    }
    get colorLine8() {
        return this.pathElement8.style.stroke;
    }
    set colorLine8(v) {
        this.pathElement8.style.stroke = v;
    }
    get colorLine9() {
        return this.pathElement9.style.stroke;
    }
    set colorLine9(v) {
        this.pathElement9.style.stroke = v;
    }
    get colorLine10() {
        return this.pathElement10.style.stroke;
    }
    set colorLine10(v) {
        this.pathElement10.style.stroke = v;
    }
    get colorLine11() {
        return this.pathElement11.style.stroke;
    }
    set colorLine11(v) {
        this.pathElement11.style.stroke = v;
    }
    get colorLine12() {
        return this.pathElement12.style.stroke;
    }
    set colorLine12(v) {
        this.pathElement12.style.stroke = v;
    }
    getPropertiesCaption(): string[] {
        var arr = ["id", "背景颜色", "量程线颜色"];
        for (var i = 1; i <= 12; i++) {
            arr.push("趋势颜色" + i);
            arr.push("设备点" + i);
        }
        return arr;
    }
    getProperties(): string[] {
        var arr = ["id", "colorFill", "colorLineLeftBottom"];
        for (var i = 1; i <= 12; i++) {
            arr.push("colorLine" + i);
            arr.push("devicePoint" + i);
        }
        return arr;
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'g'));
        this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        this.element.appendChild(this.rectElement);
        this.rectElement.setAttribute('style', 'fill:#000000;stroke:none;');

        this.line_left_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        this.line_left_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        this.element.appendChild(this.line_left_Ele);

        this.line_bottom_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        this.line_bottom_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        this.element.appendChild(this.line_bottom_Ele);

        for (var i = 1; i <= 12; i++) {
            var pe = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            pe.setAttribute('style', 'stroke-width:1;fill:none;');
            pe.setAttribute("transform", "translate(0 0)");
            this.element.appendChild(pe);
            this["pathElement" + i] = pe;
        }

        (<any>this).devicePoint = ManyPointDefined;
    }

    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
    }

   
    //开始画趋势图,values表示已经有的变化值 value结构{ value:12 , time:1221212 }
    run()
    {
        super.run();

        for (var i = 1; i <= 12; i++) {
            var valueArr = this["values" + i];
            if (valueArr.length > 1) {
                //如果有预先安排的数据
                this["_value" + i] = valueArr[valueArr.length - 1].value;
            }
        }

        this.running = true;
        this.reDrawTrend();
        this.element._interval = setInterval(() => this.reDrawTrend(), 1000);
    }

    getDrawLocation(valueItem: any, canDel: boolean, rect: any, now: any): any {
        var x = rect.width - 10 - ((now - valueItem.time) / 1000) * 2;//1秒占2个像素
        if (x < 10) {
            if (canDel) {
                return {
                    isDel: true
                };
            }
            else {
                x = 10;
            }
        }

        var min = this.min;
        if (typeof min == "undefined" || isNaN(min))
            min = 0;
        var max = this.max;
        if (typeof max == "undefined" || isNaN(max))
            max = 100;

        var percent = 1 - (valueItem.value - min) / (max - min);
        var y = 10 + (rect.height - 20) * percent;
        if (y < 10)
            y = 10;
        else if (y > rect.height - 10)
            y = rect.height - 10;

        return {
            result: x + " " + y + " "
        };
    }

    //重画趋势图
    reDrawTrend()
    {
        var rect = this.rect;
        //先计算目前可以显示多少秒，2个像素表示1秒钟的值
        var width = rect.width - 20 - 2;

        var now = new Date().getTime();
        for (var k = 1; k <= 12; k++) {
           
            var valueArr : any[] = this["values" + k];
            if (valueArr.length == 0)
                continue;

            //小于minTime时间的值不必要显示了
            var dataStr = "";
            var deleteToIndex = -1;
            if (new Date().getTime() - valueArr[valueArr.length - 1].time > 1000) {
                //如果最右边那个点和往左的一个点同值，那么把最右边那个点，移到new Date().getTime()时刻
                if (valueArr.length >= 2 &&
                    valueArr[valueArr.length - 1].value == valueArr[valueArr.length - 2].value) {
                    valueArr[valueArr.length - 1].time = new Date().getTime();
                }
                else {
                    valueArr.push({
                        value: valueArr[valueArr.length - 1].value,
                        time: new Date().getTime()
                    });
                }
            }
            for (var i = valueArr.length - 1; i >= 0; i--) {
                
                var canDel = valueArr.length > 1;
                var location = this.getDrawLocation(valueArr[i], canDel , rect, now);

                if (location.isDel) {
                    deleteToIndex = i;
                    break;
                }

                dataStr += dataStr.length == 0 ? "M" : "L";
                dataStr += location.result;
            }
             
            if (deleteToIndex >= 0) {
                //点已经过时需要删除
                valueArr.splice(0, deleteToIndex + 1);
            }

            this["pathElement" + k].setAttribute("d", dataStr);
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
        for (var i = 1; i <= 12; i++) {
            this["pathElement" + i].setAttribute("transform", "translate(" + x + " " + y + ")");
        }
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

class ButtonAreaControl extends EditorControl {
    rectElement: SVGGraphicsElement;
    startX = 0;
    startY = 0;

    get rect() {
        return {
            x: parseInt(this.rectElement.getAttribute("x")),
            y: parseInt(this.rectElement.getAttribute("y")),
            width: parseInt(this.rectElement.getAttribute("width")),
            height: parseInt(this.rectElement.getAttribute("height")),
        };
    }
    set rect(v: any) {
        this.rectElement.setAttribute("x", v.x);
        this.rectElement.setAttribute("y", v.y);
        this.rectElement.setAttribute("width", v.width);
        this.rectElement.setAttribute("height", v.height);
        this.resetPointLocation();
    }

    private pointAddr = null;
    private pointValue: any;
    _devicePoint: string = "";
    get devicePoint() {
        return this._devicePoint;
    }
    set devicePoint(v) {
        this._devicePoint = v;
        if (WatchPointNames.indexOf(v) < 0)
            WatchPointNames.push(v);
    }    

    _clickValues: string = null;
    get clickValues() {
        return this._clickValues;
    }
    set clickValues(v) {
        this._clickValues = v;
    }  

    _scriptOnClick: string = null;
    get scriptOnClick() {
        return this._scriptOnClick;
    }
    set scriptOnClick(v) {
        this._scriptOnClick = v;
    }    

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(devPoint: any) {
        this.pointValue = devPoint.value;
        this.pointAddr = devPoint.addr;
    }
   
    getPropertiesCaption(): string[] {
        return ["设备点","点击设值","点击脚本"];
    }
    getProperties(): string[] {
        return ["devicePoint","clickValues","scriptOnClick"];
    }

    constructor() {
        super( document.createElementNS('http://www.w3.org/2000/svg', 'rect'));
        this.rectElement = this.element;
        this.rectElement.setAttribute('style', 'fill:#000000;fill-opacity:0.3;stroke:none;');
    }

    run()
    {
        super.run();
        this.rectElement.style.fillOpacity = "0";
        this.rectElement.style.cursor = "pointer";
        this.rectElement.addEventListener("click", (e)=> {
            e.stopPropagation();
            if (this.scriptOnClick && this.scriptOnClick.length > 0)
            {
                eval(this.scriptOnClick);
            }
            else {
                if (this.clickValues && this.pointAddr && this.clickValues.length > 0)
                {
                    var values = this.clickValues.split(',');
                    var index = values.indexOf(this.pointValue.toString());
                    index++;                   
                    if (index >= values.length)
                        index = 0;
                    var nextvalue = values[index];
                    //往设备写入值
                    this.container.writeValue(this.devicePoint, this.pointAddr, nextvalue);
                    this.pointValue = nextvalue;
                    this.lastSetValueTime = new Date().getTime();
                }
            }
        }, false);
    }

    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
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

class GroupControl extends EditorControl implements IEditorControlContainer {
    controls: any[] = [];
    removeControl(ctrl: EditorControl) {

        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                this.groupElement.removeChild(ctrl.element);
                ctrl.isInGroup = false;
                ctrl.container = null;
                this.controls.splice(i, 1);
                break;
            }
        }
    }
    addControl(ctrl: EditorControl) {
        ctrl.isInGroup = true;
        ctrl.container = this;
        this.groupElement.appendChild(ctrl.element);
        this.controls.push(ctrl);

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
    }
    writeValue(pointName, addr, value) {
        for (var p in this)
        {
            if (p == pointName)
            {
                //如果指向的是自定义变量，那么直接设置属性值
                this[p] = value;
                return;
            }
        }
        (<any>window).writeValue(pointName, addr, value);
    }

    groupElement: SVGGElement;

    startX = 0;
    startY = 0;
    windowCode: any;
    private _path: string = null;
    get path(): string {
        if (this._path == null)
            this._path = JHttpHelper.downloadUrl(ServerUrl + "/Home/GetWindowPath?windowCode=" + this.windowCode);
        return this._path;
    }

    virtualRectElement: SVGRectElement;
    contentWidth = 0;
    contentHeight = 0;
    private lastRect: any;
    get rect() {
        var transform = this.groupElement.getAttribute("transform");
        var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
        var myrect: any = {};
        myrect.x = parseInt(result[1]);
        myrect.y = parseInt(result[2]);
        result = /scale\(([0-9|\.]+) ([0-9|\.]+)\)/.exec(transform);
        var scalex = parseFloat(result[1]);
        var scaley = parseFloat(result[2]);

        this.contentWidth = 0;
        this.contentHeight = 0;
        for (var i = 0; i < this.controls.length; i++) {
            var ctrl = this.controls[i];
            var _rect = ctrl.rect;

            if (_rect.x + _rect.width > this.contentWidth)
                this.contentWidth = _rect.x + _rect.width;
            if (_rect.y + _rect.height > this.contentHeight)
                this.contentHeight = _rect.y + _rect.height;
        }
        
        this.virtualRectElement.setAttribute('width', <any>this.contentWidth);
        this.virtualRectElement.setAttribute('height', <any>this.contentHeight);

        myrect.width = parseInt(<any>(this.contentWidth * scalex));
        myrect.height = parseInt(<any>(this.contentHeight * scaley));
        this.lastRect = myrect;
        return myrect;
    }
    set rect(v: any) {

        if (v.width == null) {
            this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ") scale(1 1)");
            var r = this.rect;//目的是获取contentWidth
            return;
        }
        if (this.contentWidth == 0)
        {
            var r = this.rect;//目的是获取contentWidth
        }
        var scalex = parseFloat(<any>v.width) / this.contentWidth;
        var scaley = parseFloat(<any>v.height) / this.contentHeight;
        scaley = scalex;//比例一致

        this.virtualRectElement.setAttribute('width', <any>this.contentWidth);
        this.virtualRectElement.setAttribute('height', <any>this.contentHeight);

        this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ") scale(" + scalex + " " + scaley + ")");
        this.lastRect = v;
        this.resetPointLocation();
    }

    customProperties: any[] = [];
    getPropertiesCaption(): string[] {
        var caps = ["id"];
        for (var i = 0; i < this.customProperties.length; i++)
        {
            caps.push(this.customProperties[i] + "设备点");
        }
        return caps;
    }
    getProperties(): string[] {
        var pros = ["id"];
        for (var i = 0; i < this.customProperties.length; i++) {
            pros.push(this.customProperties[i] + "_devPoint");
        }
        return pros;
    }

    constructor(element: any, windowCode) {
        super(element);
        this.windowCode = windowCode;
        element.setAttribute("transform", "translate(0 0) scale(1 1)");
        this.groupElement = element;


        if (!this.virtualRectElement) {
            this.virtualRectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            this.groupElement.appendChild(this.virtualRectElement);
            this.virtualRectElement.setAttribute('x', "0");
            this.virtualRectElement.setAttribute('y', "0");
            this.virtualRectElement.setAttribute('style', 'fill:#ffffff;fill-opacity:0.1;stroke:#cccccc;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
        }
    }
    run() {
        super.run();
        if (this.virtualRectElement)
        {
            this.groupElement.removeChild(this.virtualRectElement);
        }
        for (var i = 0; i < this.controls.length; i++)
        {
            this.controls[i].run();
        }
    }
    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
    }

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(point: any) {
        for (var i = 0; i < this.customProperties.length; i++)
        {
            var proName = this.customProperties[i];
            //如果和自定义变量对应的点一致，那么设置自定义变量值
            if (this[proName + "_devPoint"] == point.name)
            {
               
                this[proName + "_devPoint_addr"] = point.addr;
                this[proName + "_devPoint_max"] = point.max;
                this[proName + "_devPoint_min"] = point.min;
                this["_" + proName] = point.value;

                //传递自定义属性的变化
                var proPoint = JSON.parse(JSON.stringify(point));
                proPoint.name = proName;
                proPoint.isCustomProperty = true;
                
                for (var i = 0; i < this.controls.length; i++) {
                    var control = this.controls[i];
                    this.onChildrenPointValueChanged(control, proPoint);
                }
            }
        }
        if (!point.isCustomProperty) {
            //如果不是自定义变量发生的变化，那么继续往子控件传递
            for (var i = 0; i < this.controls.length; i++) {
                var control = this.controls[i];
                this.onChildrenPointValueChanged(control, point);
            }
        }
    }

    private onChildrenPointValueChanged(control, point)
    {
        if (control.constructor.name == "GroupControl" ||
            control.devicePoint == ManyPointDefined || control.devicePoint == point.name) {
            if (new Date().getTime() - control.lastSetValueTime < 2000) {
                (<any>window).updateValueLater(control, point);
            }
            else {
                if (control.updatePointValueTimeoutFlag) {
                    clearTimeout(control.updatePointValueTimeoutFlag);
                }
            }

            control.onDevicePointValueChanged(point);
        }
    }

    getJson()
    {
        var json : any = super.getJson();
        json.windowCode = this.windowCode;
        return json;
    }

    getScript() {
        var json = this.getJson();
        var script = "";
        var id = this.id;
        if (!id || id.length == 0) {
            id = "eCtrl";
        }
        script += id + " = editor.createGroupControl(" + JSON.stringify(this.windowCode) + " , " + JSON.stringify(json.rect) + ");\r\n";
        for (var proName in json) {
            if (proName == "rect" || proName == "constructorName")
                continue;
            var type = typeof json[proName];
            if (type == "function" || type == "undefined")
                continue;
            script += id + "." + proName + " = " + JSON.stringify(json[proName]) + ";\r\n";
        }
        return script;
    }

    /**
     * run状态下，加载自定义属性
     * @param properties
     */
    loadCustomProperties(properties: string)
    {
        if (properties && properties.length > 0)
        {
            var ps = properties.split('\n');
            for (var i = 0; i < ps.length; i++)
            {
                var name = ps[i].trim();
                if (name.length == 0)
                    continue;

                this.customProperties.push(name);
                this["_" + name] = null;
                this[name + "_devPoint_addr"] = null;
                this[name + "_devPoint_max"] = null;
                this[name + "_devPoint_min"] = null;

                Object.defineProperty(this, name, {
                    get: this.getFuncForCustomProperty(this,name),
                    set: this.setFuncForCustomProperty(this,name),
                    enumerable: true,
                    configurable: true
                });

                Object.defineProperty(this, name + "_devPoint", {
                    get: this.getFuncForCustomProperty_DevPoint(this, name + "_devPoint"),
                    set: this.setFuncForCustomProperty_DevPoint(this, name + "_devPoint"),
                    enumerable: true,
                    configurable: true
                });
            }
        }
    }

    private getFuncForCustomProperty_DevPoint(self: GroupControl, name): any {
        return function () {
            return self["_" + name];
        }
    }
    private setFuncForCustomProperty_DevPoint(self: GroupControl, name): any {
        return function (v) {
            if (self["_" + name] !== v) {
                self["_" + name] = v;
                if (WatchPointNames.indexOf(v) < 0)
                    WatchPointNames.push(v);
            }
        }
    }

    private getFuncForCustomProperty(self: GroupControl, name): any
    {
        return function () {
            return self["_" + name];
        }
    }
    private setFuncForCustomProperty(self: GroupControl,name): any {
        return function (value) {
            if (self["_" + name] !== value) {
                self["_" + name] = value;
                var pointName: string = self[name + "_devPoint"];
                if (pointName && pointName.length > 0) {
                    self.container.writeValue(pointName, self[name + "_devPoint_addr"], value);
                }

                var point = {
                    max: self[name + "_devPoint_max"],
                    min: self[name + "_devPoint_min"],
                    name: name,
                    value: value,
                    isCustomProperty:true
                };
                for (var i = 0; i < self.controls.length; i++)
                {
                    var control = self.controls[i];
                    self.onChildrenPointValueChanged(control, point);
                }
            }
        }
    }
    createGroupControl(windowCode, rect): GroupControl {
        var json = JHttpHelper.downloadUrl(ServerUrl + "/Home/GetWindowCode?windowCode=" + encodeURIComponent(windowCode));
        var content;
        eval("content=" + json);
        var groupEle = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        var editor = new GroupControl(groupEle, windowCode);
        eval(content.controlsScript);
        editor.loadCustomProperties(content.customProperties);
        this.addControl(editor);
        editor.rect = rect;
        return editor;
    }

    onBeginMoving() {
        var rect = this.rect;
        (<any>this.groupElement)._x = rect.x;
        (<any>this.groupElement)._y = rect.y;
        (<any>this.groupElement)._width = rect.width;
        (<any>this.groupElement)._height = rect.height;
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.groupElement)._x + nowX - downX);
        var y = <any>((<any>this.groupElement)._y + nowY - downY);

        this.rect = { x: x, y: y, width: (<any>this.groupElement)._width, height: (<any>this.groupElement)._height };
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}

//transform-origin:0 0;transform:scale(6,6)
class FreeGroupControl extends EditorControl implements IEditorControlContainer {
    controls: any[] = [];
    removeControl(ctrl: EditorControl) {

        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                this.groupElement.removeChild(ctrl.element);
                ctrl.isInGroup = false;
                ctrl.container = null;
                this.controls.splice(i, 1);
                break;
            }
        }
    }
    addControl(ctrl: EditorControl) {
        ctrl.isInGroup = true;
        ctrl.container = this;
        this.groupElement.appendChild(ctrl.element);
        this.controls.push(ctrl);

    }

    //组成组
    addControls(ctrls: EditorControl[]) {
        this.controls = [];

        var minLeft = 999999999;
        var minTop = 999999999;
        for (var i = 0; i < ctrls.length; i++)
        {
            var rect = ctrls[i].rect;
            if (rect.x < minLeft)
                minLeft = rect.x;
            if (rect.y < minTop)
                minTop = rect.y;
            ctrls[i].selected = false;
            ctrls[i].container.removeControl(ctrls[i]);
            this.addControl(ctrls[i]);
        }

        for (var i = 0; i < this.controls.length; i++) {
            var ctrl = this.controls[i];           
            var _rect = ctrl.rect;

            ctrl.rect = {
                x: _rect.x - minLeft,
                y: _rect.y - minTop,
                width: _rect.width,
                height:_rect.height
            };
        }

        this.groupChildren();

        this.rect = {
            x: minLeft,
            y: minTop,
            width: this.contentWidth,
            height: this.contentHeight
        };
    }
    groupChildren()
    {
        this.contentWidth = 0;
        this.contentHeight = 0;
        for (var i = 0; i < this.controls.length; i++) {
            var ctrl = this.controls[i];
            var _rect = ctrl.rect;

            if (_rect.x + _rect.width > this.contentWidth)
                this.contentWidth = _rect.x + _rect.width;
            if (_rect.y + _rect.height > this.contentHeight)
                this.contentHeight = _rect.y + _rect.height;
        }
        //把virtualRectElement移到最上面
        this.groupElement.removeChild(this.virtualRectElement);
        this.groupElement.appendChild(this.virtualRectElement);        
    }
    //解组
    freeControls()
    {
        this.selected = false;
        var rect = this.rect;
        while (this.controls.length > 0) {
            var ctrl = this.controls[0];            
            var ctrlRect = ctrl.rect;
            this.removeControl(ctrl);

            ctrlRect.x = rect.x + ctrlRect.x;
            ctrlRect.y = rect.y + ctrlRect.y;            
            this.container.addControl(ctrl);
            ctrl.rect = ctrlRect;
        }
    }
    writeValue(pointName, addr, value) {
        (<any>window).writeValue(pointName, addr, value);
    }

    groupElement: SVGGElement;


    virtualRectElement: SVGRectElement;
    contentWidth = 0;
    contentHeight = 0;
    private lastRect: any;
    get rect() {
        var transform = this.groupElement.getAttribute("transform");
        var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
        var myrect: any = {};
        myrect.x = parseInt(result[1]);
        myrect.y = parseInt(result[2]);
       

       

        this.virtualRectElement.setAttribute('width', <any>this.contentWidth);
        this.virtualRectElement.setAttribute('height', <any>this.contentHeight);

        myrect.width = this.contentWidth;
        myrect.height = this.contentHeight;
        this.lastRect = myrect;
        return myrect;
    }
    set rect(v: any) {

        if (v.width == null) {
            this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
            var r = this.rect;//目的是获取contentWidth
            return;
        }
        if (this.contentWidth == 0) {
            var r = this.rect;//目的是获取contentWidth
        }
       
        this.virtualRectElement.setAttribute('width', <any>this.contentWidth);
        this.virtualRectElement.setAttribute('height', <any>this.contentHeight);

        this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
        this.lastRect = v;
        this.resetPointLocation();
    }

    getPropertiesCaption(): string[] {
        var caps = ["id"];
        return caps;
    }
    getProperties(): string[] {
        var pros = ["id"];
        return pros;
    }

    set childScripts(scripts:string[])
    {
        var my = this;
        for (var i = 0; i < scripts.length; i++)
        {
            eval("(function(editor){" + scripts[i] + "})(my)");
        }
        this.groupChildren();
    }

    constructor() {
        super(document.createElementNS('http://www.w3.org/2000/svg', 'g'));
       
        this.groupElement = this.element;
        this.groupElement.setAttribute("transform", "translate(0 0)");

        if (!this.virtualRectElement) {
            this.virtualRectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            this.groupElement.appendChild(this.virtualRectElement);
            this.virtualRectElement.setAttribute('x', "0");
            this.virtualRectElement.setAttribute('y', "0");
            this.virtualRectElement.setAttribute('fill-opacity', "0");
            this.virtualRectElement.setAttribute('stroke', "green");
            this.virtualRectElement.setAttribute('style', 'stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
        }
    }
    onSelectedChange() {
        this.virtualRectElement.setAttribute('stroke', this.selected ? "red" : "green");
    }
    run() {
        super.run();
        if (this.virtualRectElement) {
            this.groupElement.removeChild(this.virtualRectElement);
        }
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].run();
        }
    }
    isIntersectWith(rect): boolean {
        return this.isIntersect(this.rect, rect);
    }

    //当关联的设备点值方式变化时触发
    onDevicePointValueChanged(point: any) {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.onDevicePointValueChanged(point);
        }
    }


    /**
     * 获取描述本控件属性的一个json对象
     */
    getJson() {
        var obj = {
            rect: this.rect,
            constructorName: (<any>this).constructor.name
        };
        var properites = this.getProperties();
        for (var i = 0; i < properites.length; i++) {
            obj[properites[i]] = this[properites[i]];
        }
        var childscripts = [];
        for (var i = 0; i < this.controls.length; i++) {
            var childScript = this.controls[i].getScript();
            childscripts.push(childScript);
        }
        obj["childScripts"] = childscripts;
        return obj;
    }

    /**
     * 获取运行时的执行脚本
     */
    getScript() {
        var rect = this.rect;
        var script = "";
        var id = this.id;
        if (!id || id.length == 0) {
            id = "eCtrl";
        }
        script += id + " = new FreeGroupControl();\r\n"; 
        script += "editor.addControl(" + id + ");\r\n";
        script += id + ".id = " + JSON.stringify(this.id) + ";\r\n";

        for (var i = 0; i < this.controls.length; i++)
        {
            var childScript = this.controls[i].getScript();
            script += "(function(editor){\r\n" + childScript + "\r\n})(" + id + ");\r\n";
        }

        script += id + ".rect = " + JSON.stringify(rect) + ";\r\n";
        script += id + ".groupChildren();\r\n";
        return script;
    }
    

    onBeginMoving() {
        var rect = this.rect;
        (<any>this.groupElement)._x = rect.x;
        (<any>this.groupElement)._y = rect.y;
        (<any>this.groupElement)._width = rect.width;
        (<any>this.groupElement)._height = rect.height;
    }
    onMoving(downX, downY, nowX, nowY) {
        var x = <any>((<any>this.groupElement)._x + nowX - downX);
        var y = <any>((<any>this.groupElement)._y + nowY - downY);

        this.rect = { x: x, y: y, width: (<any>this.groupElement)._width, height: (<any>this.groupElement)._height };
        if (this.selected) {
            this.resetPointLocation();
        }
    }
    onEndMoving() {

    }
}