﻿function rgb(r, g, b)
{
    return {
        "r": r,
        "g": g,
        "b":b
    };
}

class PropertyDialog
{
    rootElement: HTMLElement;
    control: EditorControl;
    documentClickEvent: any;
    isShowed: boolean = false;
    static CHKNumber = 0;
    constructor(control: EditorControl)
    {
        this.control = control;
        var captions = control.getPropertiesCaption();
        var pNames = control.getProperties();


        this.rootElement = document.createElement("DIV");
        this.rootElement.style.padding = "3px";
        this.rootElement.style.maxHeight = window.innerHeight*0.95 + "px";
        this.rootElement.style.overflowY = "auto";
        this.rootElement.style.overflowX = "hidden";
        this.rootElement.style.zIndex = "799";

        if (true)
        {
            var row = document.createElement("DIV");
            row.style.display = "table-row";
            this.rootElement.appendChild(row);

            var cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.whiteSpace = "nowrap";
            cell.innerHTML = "类型：";
            row.appendChild(cell);
            var captionCell = cell;

            cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.paddingRight = "30px";
            if ((<any>control).constructor.name == "GroupControl") {
                cell.innerHTML = (<any>control).path;
            }
            else {
                cell.innerHTML = (<any>control).constructor.name;
            }
            row.appendChild(cell);
        }

        for (var i = 0; i < pNames.length; i++)
        {
            var value = control[pNames[i]];
            if (typeof value == "undefined" || (typeof value == "number" && isNaN(value)))
                value = "";

            var row = document.createElement("DIV");
            row.style.display = "table-row";
            this.rootElement.appendChild(row);

            var cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.whiteSpace = "nowrap";
            cell.innerHTML = captions[i] + "：";
            cell.title = pNames[i];
            row.appendChild(cell);
            var captionCell = cell;

            cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.paddingRight = "30px";
            if (pNames[i].indexOf("color") >= 0) {
                cell.innerHTML = "<input type='text'>"; 
                if (value.indexOf("rgb") >= 0)
                {
                    (<any>cell.children[0]).value = "#" + (<any>$).colpick.rgbToHex( eval(value));
                }
                (<any>cell.children[0])._picker = true;
            }
            else if (pNames[i].indexOf("fontFamily") == 0) {
                cell.innerHTML = "<select><option value=''></option><option value='黑体'>黑体</option><option value='宋体'>宋体</option><option value='新宋体'>新宋体</option><option value='仿宋'>仿宋</option><option value='楷体'>楷体</option><option value='雅黑'>雅黑</option></select>"; 
                (<any>cell.children[0]).value = value;
            }
            else if (pNames[i].indexOf("can") == 0 || pNames[i].indexOf("is") == 0) {
                captionCell.innerHTML = "&nbsp;";
                var chknumber = PropertyDialog.CHKNumber++;

                cell.innerHTML = "<input type='checkbox' id='chk" + chknumber + "'><label for='chk" + chknumber + "'>" + captions[i] + "</label>";
                (<any>cell.children[0])._name = pNames[i];
                if (value)
                {
                    (<any>cell.children[0]).checked = "checked";
                }
                this.setChkItemEvent(cell.children[0]);
            }
            else if (pNames[i].indexOf("script") >= 0 || captions[i] == "自定义变量" ) {
                cell.innerHTML = "<textarea style='width:300px;height:100px;'></textarea>";
               
                (<any>cell.children[0]).value = value;
            }
            else if (pNames[i].indexOf("img") >= 0){
                cell.innerHTML = "<input type='text'>";
                (<any>cell.children[0]).value = value;
                (<any>cell.children[0])._name = pNames[i];
                this.setImgItemEvent(cell.children[0]);
            }
            else {
                cell.innerHTML = "<input type='text'>";
                (<any>cell.children[0]).value = value;

                if (captions[i].indexOf("点") >= 0)
                {
                    //设置设备点的textbox
                    this.setPointItemEvent(<any>cell.children[0]);
                }
            }
            (<any>cell.children[0])._name = pNames[i];
           
            this.setInputEvent(cell.children[0]);
            row.appendChild(cell);
        }

        this.rootElement.style.position = "absolute";
        this.rootElement.style.visibility = "hidden";//visible
        if (pNames.length > 0) {
            document.body.appendChild(this.rootElement);
        }

        this.rootElement.className = "propertyDialog";
        this.rootElement.style.cursor = "move";       

        this.setRootEvent();

       
    }

    private _documentElementClick(e: Event)
    {
        var ele: any = e.target;
        while (ele && ele.tagName != "BODY") {
            if (ele == this.rootElement) {
                return;
            }
            else {
                ele = ele.parentElement;
            }
        }
        this.hide();
    }

    dispose()
    {
        this.isShowed = false;
        this.rootElement.parentElement.removeChild(this.rootElement);
        document.documentElement.removeEventListener("click", this.documentClickEvent, false);
    }
    private setPointItemEvent(ele: HTMLElement) {
        
        ele.addEventListener("focus", (e) => {
            editor.editingPointTextbox = ele;
        }, false);
        ele.addEventListener("blur", (e) => {
            editor.editingPointTextbox = null;
        }, false);
    }
    private setChkItemEvent(ele) {
        ele.onclick = () => {
            var undo = new UndoChangeProperty(editor, this.control, ele._name, !this.control[ele._name]);
            try {
                undo.redo();
                editor.undoMgr.addUndo(undo);
            }
            catch (e) {
                alert(e.message);
            }
        };
    }
    private setImgItemEvent(ele)
    {
        ele.onclick = () => {
            fileBrowser.onSelectFile = (path) => {
                ele.value = path;

                var undo = new UndoChangeProperty(editor, this.control, ele._name, path);
                try {
                    undo.redo();
                    editor.undoMgr.addUndo(undo);
                }
                catch (e) {
                    alert(e.message);
                }

                fileBrowser.hide();
            };
            fileBrowser.show();
        };
    }

    private setRootEvent()
    {
       
        var ele = <any>(this.rootElement);
      
        var moving = false;

        this.rootElement.addEventListener("mousedown", (e) => {
            if ((<any>e.target).tagName == "DIV" && e.clientX < this.rootElement.offsetLeft + this.rootElement.offsetWidth - 50)
            {
                ele._startx = e.clientX;
                ele._starty = e.clientY;
                ele._oldx = parseInt( ele.style.left.replace("px", ""));
                ele._oldy = parseInt(ele.style.top.replace("px", ""));
                moving = true;
            }
        }, false);


        document.body.addEventListener("mousemove", (e) => {
            if (moving) {
                ele.style.left = (ele._oldx + e.clientX - ele._startx) + "px";
                ele.style.top = (ele._oldy + e.clientY - ele._starty) + "px";
            }
        }, false);

        document.body.addEventListener("mouseup", (e) => {
            if (moving) {
                moving = false;
            }
        }, false);
    }

    private setInputEvent(input)
    {
        if (input.tagName == "SELECT")
        {
            input.addEventListener("change", (e) => {

                var undo = new UndoChangeProperty(editor, this.control, input._name, input.value);
                undo.redo();
                editor.undoMgr.addUndo(undo);

                editor.changed = true;
                this.control.onPropertyDialogTextChanged(input._name);
            }, false);
            return;
        }

        input.addEventListener("keydown", (e) => {
            e.stopPropagation();
        }, false);
        input.addEventListener("keyup", (e) => {
            e.stopPropagation();            
        }, false);

        if (input._picker) {
            input._control = this.control;
            (<any>$(input)).colpick({
                submit: 0,
                color:input.value,
                onChange: function (hsb, hex, rgb, el, bySetColor) {
                    input.value = "#" + hex;

                    var undo = new UndoChangeProperty(editor, el._control, input._name, input.value);
                    try {
                        undo.redo();
                        editor.undoMgr.addUndo(undo);
                    }
                    catch (e) {
                        alert(e.message);
                    }

                    editor.changed = true;

                    (<any>el._control).onPropertyDialogTextChanged(input._name);
                }
            });
        }
        else {
            input.changeFunc = () => {
                var undo = new UndoChangeProperty(editor, this.control, input._name, input.value);
                try {
                    undo.redo();
                    editor.undoMgr.addUndo(undo);
                }
                catch (e)
                {
                    alert(e.message);
                }

                editor.changed = true;
            };

            input.addEventListener("keyup", (e) => {
                var undo = new UndoChangeProperty(editor, this.control, input._name, input.value);
                try {
                    undo.redo();
                    editor.undoMgr.addUndo(undo);
                }
                catch (e) {
                    input.value = undo.originalValue;
                    alert(e.message);
                }
                editor.changed = true;
            }, false);

            input.addEventListener("change", (e) => {
                this.control.onPropertyDialogTextChanged(input._name);
            }, false);
        }
    }

    hide()
    {
        document.documentElement.removeEventListener("click", this.documentClickEvent, false);

        this.isShowed = false;
        editor.editingPointTextbox = null;
        this.rootElement.style.visibility = "hidden";
    }
    show()
    {
        this.documentClickEvent = (e) => this._documentElementClick(e);
        document.documentElement.addEventListener("click", this.documentClickEvent, false);

        this.isShowed = true;
        var rect = this.control.rect;
        if (!rect) {
            //有些control没有rect属性
            rect = {
                x: 10,
                y: 10,
                width: 0,
                height: 0
            };
        }
        var x = rect.x + rect.width + 6;
        var y = rect.y + 30;
        if (x + this.rootElement.offsetWidth > window.innerWidth) {
            x = rect.x - this.rootElement.offsetWidth - 6;
            if (x < 0)
                x = window.innerWidth - this.rootElement.offsetWidth;
        }
        if (y + this.rootElement.offsetHeight > window.innerHeight) {
            y = rect.y - this.rootElement.offsetHeight - 6;
            if (y < 0)
                y = window.innerHeight - this.rootElement.offsetHeight;
        }
        this.rootElement.style.left = x + "px";
        this.rootElement.style.top = y + "px";

        this.rootElement.style.visibility = "visible";
    }
}