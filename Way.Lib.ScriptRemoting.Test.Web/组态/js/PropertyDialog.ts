
class PropertyDialog
{
    rootElement: HTMLElement;
    control: EditorControl;
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
        for (var i = 0; i < pNames.length; i++)
        {
            var row = document.createElement("DIV");
            row.style.display = "table-row";
            this.rootElement.appendChild(row);

            var cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.whiteSpace = "nowrap";
            cell.innerHTML = captions[i] + "：";
            row.appendChild(cell);
            var captionCell = cell;

            cell = document.createElement("DIV");
            cell.style.display = "table-cell";
            cell.style.paddingRight = "30px";
            if (pNames[i].indexOf("color") >= 0) {
                cell.innerHTML = "<input type='text' class='jscolor'>";
                var picker;
                eval("picker = new jscolor(cell.children[0])");

                (<any>cell.children[0])._picker = picker;
                (<any>cell.children[0])._picker.hash = true;
                (<any>cell.children[0])._picker.fromString(control[pNames[i]]);

                (<any>cell.children[0]).value = (<any>cell.children[0])._picker.toHEXString();
            }
            else if (pNames[i].indexOf("can") == 0) {
                captionCell.innerHTML = "&nbsp;";
                var chknumber = PropertyDialog.CHKNumber++;

                cell.innerHTML = "<input type='checkbox' id='chk" + chknumber + "'><label for='chk" + chknumber + "'>" + captions[i] + "</label>";
                (<any>cell.children[0])._name = pNames[i];
                if (control[pNames[i]])
                {
                    (<any>cell.children[0]).checked = "checked";
                }
                this.setChkItemEvent(cell.children[0]);
            }
            else if (pNames[i].indexOf("script") >= 0) {
                cell.innerHTML = "<textarea style='width:300px;height:100px;'></textarea>";
            }
            else if (pNames[i].indexOf("img") >= 0){
                cell.innerHTML = "<input type='text'>";
                (<any>cell.children[0]).value = control[pNames[i]];
                (<any>cell.children[0])._name = pNames[i];
                this.setImgItemEvent(cell.children[0]);
            }
            else {
                cell.innerHTML = "<input type='text'>";
                if (control[pNames[i]]) {
                    (<any>cell.children[0]).value = control[pNames[i]];
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
    private setChkItemEvent(ele) {
        ele.onclick = () => {
            this.control[ele._name] = !this.control[ele._name];
        };
    }
    private setImgItemEvent(ele)
    {
        ele.onclick = () => {
            fileBrowser.onSelectFile = (path) => {
                ele.value = path;
                this.control[ele._name] = path;
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
            if ((<any>e.target).tagName == "DIV")
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
        input.addEventListener("keydown", (e) => {
            e.stopPropagation();
        }, false);
        input.addEventListener("keyup", (e) => {
            e.stopPropagation();            
        }, false);

        if (input._picker) {
            input.onchange = () => {
                this.control[input._name] = input.value;                
            };
        }
        else {
            input.addEventListener("keyup", (e) => {
                this.control[input._name] = input.value;
            }, false);
        }
    }

    hide()
    {
        this.rootElement.style.visibility = "hidden";
    }
    show()
    {
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