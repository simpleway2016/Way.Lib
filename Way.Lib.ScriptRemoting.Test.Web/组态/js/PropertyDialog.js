var PropertyDialog = (function () {
    function PropertyDialog(control) {
        this.control = control;
        var captions = control.getPropertiesCaption();
        var pNames = control.getProperties();
        this.rootElement = document.createElement("DIV");
        this.rootElement.style.padding = "3px";
        this.rootElement.style.maxHeight = window.innerHeight * 0.95 + "px";
        this.rootElement.style.overflowY = "auto";
        this.rootElement.style.overflowX = "hidden";
        this.rootElement.style.zIndex = "799";
        for (var i = 0; i < pNames.length; i++) {
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
                cell.children[0]._picker = picker;
                cell.children[0]._picker.hash = true;
                cell.children[0]._picker.fromString(control[pNames[i]]);
                cell.children[0].value = cell.children[0]._picker.toHEXString();
            }
            else if (pNames[i].indexOf("can") == 0) {
                captionCell.innerHTML = "&nbsp;";
                var chknumber = PropertyDialog.CHKNumber++;
                cell.innerHTML = "<input type='checkbox' id='chk" + chknumber + "'><label for='chk" + chknumber + "'>" + captions[i] + "</label>";
                cell.children[0]._name = pNames[i];
                if (control[pNames[i]]) {
                    cell.children[0].checked = "checked";
                }
                this.setChkItemEvent(cell.children[0]);
            }
            else if (pNames[i].indexOf("script") >= 0) {
                cell.innerHTML = "<textarea style='width:300px;height:100px;'></textarea>";
                cell.children[0].value = control[pNames[i]];
            }
            else if (pNames[i].indexOf("img") >= 0) {
                cell.innerHTML = "<input type='text'>";
                cell.children[0].value = control[pNames[i]];
                cell.children[0]._name = pNames[i];
                this.setImgItemEvent(cell.children[0]);
            }
            else {
                cell.innerHTML = "<input type='text'>";
                if (control[pNames[i]]) {
                    cell.children[0].value = control[pNames[i]];
                }
            }
            cell.children[0]._name = pNames[i];
            this.setInputEvent(cell.children[0]);
            row.appendChild(cell);
        }
        this.rootElement.style.position = "absolute";
        this.rootElement.style.visibility = "hidden";
        if (pNames.length > 0) {
            document.body.appendChild(this.rootElement);
        }
        this.rootElement.className = "propertyDialog";
        this.rootElement.style.cursor = "move";
        this.setRootEvent();
    }
    PropertyDialog.prototype.setChkItemEvent = function (ele) {
        var _this = this;
        ele.onclick = function () {
            _this.control[ele._name] = !_this.control[ele._name];
        };
    };
    PropertyDialog.prototype.setImgItemEvent = function (ele) {
        var _this = this;
        ele.onclick = function () {
            fileBrowser.onSelectFile = function (path) {
                ele.value = path;
                _this.control[ele._name] = path;
                fileBrowser.hide();
            };
            fileBrowser.show();
        };
    };
    PropertyDialog.prototype.setRootEvent = function () {
        var _this = this;
        var ele = (this.rootElement);
        var moving = false;
        this.rootElement.addEventListener("mousedown", function (e) {
            if (e.target.tagName == "DIV" && e.clientX < _this.rootElement.offsetLeft + _this.rootElement.offsetWidth - 50) {
                ele._startx = e.clientX;
                ele._starty = e.clientY;
                ele._oldx = parseInt(ele.style.left.replace("px", ""));
                ele._oldy = parseInt(ele.style.top.replace("px", ""));
                moving = true;
            }
        }, false);
        document.body.addEventListener("mousemove", function (e) {
            if (moving) {
                ele.style.left = (ele._oldx + e.clientX - ele._startx) + "px";
                ele.style.top = (ele._oldy + e.clientY - ele._starty) + "px";
            }
        }, false);
        document.body.addEventListener("mouseup", function (e) {
            if (moving) {
                moving = false;
            }
        }, false);
    };
    PropertyDialog.prototype.setInputEvent = function (input) {
        var _this = this;
        input.addEventListener("keydown", function (e) {
            e.stopPropagation();
        }, false);
        input.addEventListener("keyup", function (e) {
            e.stopPropagation();
        }, false);
        if (input._picker) {
            input.onchange = function () {
                _this.control[input._name] = input.value;
            };
        }
        else {
            input.addEventListener("keyup", function (e) {
                _this.control[input._name] = input.value;
            }, false);
        }
    };
    PropertyDialog.prototype.hide = function () {
        this.rootElement.style.visibility = "hidden";
    };
    PropertyDialog.prototype.show = function () {
        var rect = this.control.rect;
        if (!rect) {
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
    };
    return PropertyDialog;
}());
PropertyDialog.CHKNumber = 0;
//# sourceMappingURL=PropertyDialog.js.map