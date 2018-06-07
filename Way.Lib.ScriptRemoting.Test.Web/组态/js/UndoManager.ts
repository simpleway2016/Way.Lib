class UndoManager
{
    items: UndoItem[] = [];
    position: number = 0;
    enable: boolean = false;
    constructor()
    {

    }

    addUndo(undoObj: UndoItem)
    {
        if (!undoObj.enable || !this.enable)
            return;

        if (this.items.length - this.position > 0) {
            this.items.splice(this.position, this.items.length - this.position);
        }

        if (this.items.length > 0 && this.items[this.items.length - 1].isSame(undoObj)) {
            //和上一个undo是同样的action，所以，只是合并一些参数即可
        }
        else {
            this.items.push(undoObj);
            this.position++;
        }
        editor.changed = true;
    }

    undo()
    {
        if (this.position == 0)
            return;

        var item = this.items[this.position - 1];
        item.undo();
        this.position--;
    }
    redo()
    {
        if (this.position == this.items.length)
            return;

        var item = this.items[this.position];
        item.redo();
        this.position++;
    }
}

class UndoItem
{
    editor: Editor;
    enable: boolean = true;
    constructor(_editor: Editor)
    {
        this.editor = _editor;
    }

    /**
     * 判断obj是否和自己同一个action，如果相同，采用obj的参数
     * @param obj
     */
    isSame(obj: UndoItem): boolean
    {
        return false;
    }

    undo()
    {

    }

    redo()
    {

    }
}
class UndoChangeProperty extends UndoItem {
    control: EditorControl;
    proName: string;
    originalValue: any;
    newValue: any;
    constructor(_editor: Editor, _control: EditorControl,proname:string,newValue) {
        super(_editor);
        this.control = _control;
        this.proName = proname;
        this.originalValue = _control[proname];
        this.newValue = newValue;
    }

    isSame(undoObj: UndoItem): boolean
    {
        if ((<any>undoObj).constructor.name != "UndoChangeProperty")
            return false;

        var compareItem: UndoChangeProperty = <UndoChangeProperty>undoObj;
        if (compareItem.control !== this.control)
            return false;

        if (compareItem.proName !== this.proName)
            return false;

        this.newValue = compareItem.newValue;
        return true;
    }

    undo() {
        this.control[this.proName] = this.originalValue;
    }

    redo() {
        this.control[this.proName] = this.newValue;
    }
}
class UndoAddControl extends UndoItem
{
    control: EditorControl;
    constructor(_editor: Editor , _control: EditorControl) {
        super(_editor);
        this.control = _control;
    }

    undo() {
        this.control.selected = false;
        this.editor.removeControl(this.control);
    }

    redo()
    {
        this.editor.addControl(this.control);
    }
}

class UndoRemoveControls extends UndoItem
{
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = _controls;
    }

    undo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.element.style.display = "";
            this.editor.controls.push(control);
        }
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i]; 
            control.selected = false;
            var index = this.editor.controls.indexOf(control);
            control.element.style.display = "none";
            this.editor.controls.splice(index, 1);
        }
    }
}

class UndoMoveControls extends UndoItem {
    controls: EditorControl[];
    rects: any[] = [];
    endRects: any[] = [];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            var control = _controls[i];
            this.controls.push(control);
            this.rects.push(control.rect);
        }
    }

    isSame(undoObj: UndoItem): boolean
    {
        if ((<any>undoObj).constructor.name != "UndoMoveControls")
            return false;

        var compareItem: UndoMoveControls = <UndoMoveControls>undoObj;
        if (compareItem.controls.length != this.controls.length)
            return false;
        for (var i = 0; i < this.controls.length; i++)
        {
            if (this.controls[i] != compareItem.controls[i])
                return false;
        }

        this.endRects = compareItem.endRects;

        return true;
    }

    moveFinish()
    {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            this.endRects.push(control.rect);
        }

        var isdifferent = false;
        for (var i = 0; i < this.controls.length; i++) {
            if (JSON.stringify(this.endRects[i]) != JSON.stringify(this.rects[i]))
            {
                isdifferent = true;
                break;
            }
        }
        if (!isdifferent)
        {
            //如果没有移动，那么不用添加undo
            this.enable = false;
        }
        else {
            editor.resetScrollbar();
        }
    }

    undo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.rect = this.rects[i];
        }
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.rect = this.endRects[i];
        }
    }
}

/**
 * 上移
 */
class UndoMoveControlsLayerUp extends UndoItem {
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            this.controls.push(_controls[i]);
        }
    }


    undo() {
        for (var i = this.controls.length - 1; i >= 0; i --) {
            var control = this.controls[i];
            var preEle: any = (<Element>control.element).previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.editor.svgContainer.removeChild(<any>control.element);
                this.editor.svgContainer.insertBefore(<any>control.element, preEle);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextEle: any = (<Element>control.element).nextElementSibling;
            while (nextEle && !nextEle._editorControl) {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.editor.svgContainer.removeChild(nextEle);
                this.editor.svgContainer.insertBefore(nextEle, <any>control.element);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }
}

/**
 * 下移
 */
class UndoMoveControlsLayerDown extends UndoItem {
    controls: EditorControl[];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            this.controls.push(_controls[i]);
        }
    }


    undo() {
        for (var i = this.controls.length - 1; i >= 0; i--) {
            var control = this.controls[i];
            var nextEle: any = (<Element>control.element).nextElementSibling;
            while (nextEle && !nextEle._editorControl) {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.editor.svgContainer.removeChild(nextEle);
                this.editor.svgContainer.insertBefore(nextEle, <any>control.element);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];           
            var preEle: any = (<Element>control.element).previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.editor.svgContainer.removeChild(<any>control.element);
                this.editor.svgContainer.insertBefore(<any>control.element, preEle);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }
}

/**
 * 顶层
 */
class UndoMoveControlsLayerFront extends UndoItem {
    controls: EditorControl[];
    nextEles: any[] = [];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            this.controls.push(_controls[i]);
            this.nextEles.push((<Element>_controls[i].element).nextElementSibling);
        }
    }


    undo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextSibling = this.nextEles[i];

            this.editor.svgContainer.removeChild(control.element);

            if (nextSibling) {
                this.editor.svgContainer.insertBefore(<any>control.element, nextSibling);
            }
            else {
                this.editor.svgContainer.appendChild(control.element);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            this.editor.svgContainer.removeChild(control.element);
            this.editor.svgContainer.appendChild(control.element);
        }


        //重组controls
        this.editor.rebuildControls();
    }
}


/**
 * 底层
 */
class UndoMoveControlsLayerBottom extends UndoItem {
    controls: EditorControl[];
    nextEles: any[] = [];
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            this.controls.push(_controls[i]);
            this.nextEles.push((<Element>_controls[i].element).nextElementSibling);
        }
    }


    undo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextSibling = this.nextEles[i];

            this.editor.svgContainer.removeChild(control.element);

            if (nextSibling) {
                this.editor.svgContainer.insertBefore(<any>control.element, nextSibling);
            }
            else {
                this.editor.svgContainer.appendChild(control.element);
            }
        }


        //重组controls
        this.editor.rebuildControls();
    }

    redo() {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            if (this.editor.svgContainer.children[0] != control.element) {
                this.editor.svgContainer.removeChild(control.element);
                this.editor.svgContainer.insertBefore(<any>control.element, <any>this.editor.svgContainer.children[0]);
            }
        }

        //重组controls
        this.editor.rebuildControls();
    }
}
class UndoChangeLinePoint extends UndoItem
{
    control: LineControl;
    xname: string;
    yname: string;
    oldvalueX: string;
    oldvalueY: string;
    newvalueX: string;
    newvalueY: string;
    constructor(_editor: Editor, _control: LineControl, _xname: string, _yname: string) {
        super(_editor);
        this.control = _control;
        this.xname = _xname;
        this.yname = _yname;
        this.oldvalueX = this.control.lineElement.getAttribute(this.xname);
        this.oldvalueY = this.control.lineElement.getAttribute(this.yname);
    }

    moveFinish() {
        this.newvalueX = this.control.lineElement.getAttribute(this.xname);
        this.newvalueY = this.control.lineElement.getAttribute(this.yname);
    }

    undo() {
        this.control.lineElement.setAttribute(this.xname, this.oldvalueX);
        this.control.lineElement.setAttribute(this.yname, this.oldvalueY);
        this.control.resetPointLocation();
    }

    redo() {
        this.control.lineElement.setAttribute(this.xname, this.newvalueX);
        this.control.lineElement.setAttribute(this.yname, this.newvalueY);
        this.control.resetPointLocation();
    }
}

class UndoGroup extends UndoItem {
    controls: EditorControl[];
    groupCtrl: FreeGroupControl;
    constructor(_editor: Editor, _controls: EditorControl[]) {
        super(_editor);
        this.controls = _controls;
        this.groupCtrl = new FreeGroupControl();
    }

    undo() {
        this.groupCtrl.freeControls();
        this.editor.removeControl(this.groupCtrl);
    }

    redo() {
       
        this.groupCtrl.addControls(this.controls);
        this.editor.addControl(this.groupCtrl);
    }
}
class UndoUnGroup extends UndoItem {
    groups: any[] = [];
    constructor(_editor: Editor, _controls: FreeGroupControl[]) {
        super(_editor);
        for (var i = 0; i < _controls.length; i++)
        {
            var item: any = {};
            this.groups.push(item);
            item.controls = [];
            item.group = _controls[i];
            for (var j = 0; j < _controls[i].controls.length; j++)
            {
                item.controls.push(_controls[i].controls[j]);
            }
        }
    }

    undo() {
        for (var i = 0; i < this.groups.length; i++) {
            var item = this.groups[i];
            item.group.addControls(item.controls);
            this.editor.addControl(item.group);
        }
    }

    redo() {
        for (var i = 0; i < this.groups.length; i++)
        {
            var item = this.groups[i];
            item.group.freeControls();
            this.editor.removeControl(item.group);
        }
    }
}

class UndoPaste extends UndoItem {
    copyItems: any[] = [];
    controls: any[] = null;
    isSameWindow: boolean;
    constructor(_editor: Editor, _copyItems: any[], isSameWindow: boolean) {
        super(_editor);
        this.copyItems = _copyItems;
        this.isSameWindow = isSameWindow;
    }

    undo() {
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selected = false;
            this.editor.removeControl(this.controls[i]);
        }
    }

    redo() {
        if (!this.controls) {
            this.controls = [];
            for (var i = 0; i < this.copyItems.length; i++) {
                var controlJson = this.copyItems[i];
                if (this.isSameWindow) {
                    controlJson.rect.x += 10;
                    controlJson.rect.y += 10;
                }
                if (controlJson["id"])
                {
                    //判断id是否重复
                    var idname = controlJson["id"];
                    if (this.editor.isIdExist(idname)) {
                        //id存在了，用新id
                        var index = 1;
                        while (this.editor.isIdExist(controlJson.constructorName + index)) {
                            index++;
                        }
                        controlJson["id"] = controlJson.constructorName + index;
                    }
                }

                var editorctrl;
                if (controlJson.constructorName == "GroupControl") {
                    editorctrl = this.editor.createGroupControl(controlJson.windowCode, controlJson.rect);
                }
                else {
                    eval("editorctrl = new " + controlJson.constructorName + "()");
                    this.editor.addControl(editorctrl);
                }

                for (var pname in controlJson) {
                    if (pname != "tagName" && pname != "constructorName" && pname != "rect") {
                        editorctrl[pname] = controlJson[pname];
                    }
                }

                if (controlJson.constructorName != "GroupControl")
                {
                    editorctrl.rect = controlJson.rect;
                }

                editorctrl.ctrlKey = true;//这样才能多个选中
                editorctrl.selected = true;
                editorctrl.ctrlKey = false;
                this.controls.push(editorctrl);

            }
        }
        else {
            for (var i = 0; i < this.controls.length; i++)
            {
                this.editor.addControl(this.controls[i]);
            }
        }
    }
}