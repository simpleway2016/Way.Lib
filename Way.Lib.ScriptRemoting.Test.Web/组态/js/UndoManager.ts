class UndoManager
{
    items: UndoItem[] = [];
    position: number = 0;
    constructor()
    {

    }

    addUndo(undoObj: UndoItem)
    {
        if (!undoObj.enable)
            return;

        if (this.items.length - this.position > 0) {
            this.items.splice(this.position, this.items.length - this.position);
        }
        this.items.push(undoObj);
        this.position++;
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

    undo()
    {

    }

    redo()
    {

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
        this.controls = _controls;
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            this.rects.push(control.rect);
        }
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