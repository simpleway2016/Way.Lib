var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var UndoManager = (function () {
    function UndoManager() {
        this.items = [];
        this.position = 0;
        this.enable = false;
    }
    UndoManager.prototype.addUndo = function (undoObj) {
        if (!undoObj.enable || !this.enable)
            return;
        if (this.items.length - this.position > 0) {
            this.items.splice(this.position, this.items.length - this.position);
        }
        if (this.items.length > 0 && this.items[this.items.length - 1].isSame(undoObj)) {
        }
        else {
            this.items.push(undoObj);
            this.position++;
        }
        editor.changed = true;
    };
    UndoManager.prototype.undo = function () {
        if (this.position == 0)
            return;
        var item = this.items[this.position - 1];
        item.undo();
        this.position--;
    };
    UndoManager.prototype.redo = function () {
        if (this.position == this.items.length)
            return;
        var item = this.items[this.position];
        item.redo();
        this.position++;
    };
    return UndoManager;
}());
var UndoItem = (function () {
    function UndoItem(_editor) {
        this.enable = true;
        this.editor = _editor;
    }
    UndoItem.prototype.isSame = function (obj) {
        return false;
    };
    UndoItem.prototype.undo = function () {
    };
    UndoItem.prototype.redo = function () {
    };
    return UndoItem;
}());
var UndoChangeProperty = (function (_super) {
    __extends(UndoChangeProperty, _super);
    function UndoChangeProperty(_editor, _control, proname, newValue) {
        var _this = _super.call(this, _editor) || this;
        _this.control = _control;
        _this.proName = proname;
        _this.originalValue = _control[proname];
        _this.newValue = newValue;
        return _this;
    }
    UndoChangeProperty.prototype.isSame = function (undoObj) {
        if (undoObj.constructor.name != "UndoChangeProperty")
            return false;
        var compareItem = undoObj;
        if (compareItem.control !== this.control)
            return false;
        if (compareItem.proName !== this.proName)
            return false;
        this.newValue = compareItem.newValue;
        return true;
    };
    UndoChangeProperty.prototype.undo = function () {
        this.control[this.proName] = this.originalValue;
    };
    UndoChangeProperty.prototype.redo = function () {
        this.control[this.proName] = this.newValue;
    };
    return UndoChangeProperty;
}(UndoItem));
var UndoAddControl = (function (_super) {
    __extends(UndoAddControl, _super);
    function UndoAddControl(_editor, _control) {
        var _this = _super.call(this, _editor) || this;
        _this.control = _control;
        return _this;
    }
    UndoAddControl.prototype.undo = function () {
        this.control.selected = false;
        this.editor.removeControl(this.control);
    };
    UndoAddControl.prototype.redo = function () {
        this.editor.addControl(this.control);
    };
    return UndoAddControl;
}(UndoItem));
var UndoRemoveControls = (function (_super) {
    __extends(UndoRemoveControls, _super);
    function UndoRemoveControls(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.controls = _controls;
        return _this;
    }
    UndoRemoveControls.prototype.undo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.element.style.display = "";
            this.editor.controls.push(control);
        }
    };
    UndoRemoveControls.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.selected = false;
            var index = this.editor.controls.indexOf(control);
            control.element.style.display = "none";
            this.editor.controls.splice(index, 1);
        }
    };
    return UndoRemoveControls;
}(UndoItem));
var UndoMoveControls = (function (_super) {
    __extends(UndoMoveControls, _super);
    function UndoMoveControls(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.rects = [];
        _this.endRects = [];
        _this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            var control = _controls[i];
            _this.controls.push(control);
            _this.rects.push(control.rect);
        }
        return _this;
    }
    UndoMoveControls.prototype.isSame = function (undoObj) {
        if (undoObj.constructor.name != "UndoMoveControls")
            return false;
        var compareItem = undoObj;
        if (compareItem.controls.length != this.controls.length)
            return false;
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] != compareItem.controls[i])
                return false;
        }
        this.endRects = compareItem.endRects;
        return true;
    };
    UndoMoveControls.prototype.moveFinish = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            this.endRects.push(control.rect);
        }
        var isdifferent = false;
        for (var i = 0; i < this.controls.length; i++) {
            if (JSON.stringify(this.endRects[i]) != JSON.stringify(this.rects[i])) {
                isdifferent = true;
                break;
            }
        }
        if (!isdifferent) {
            this.enable = false;
        }
        else {
            editor.resetScrollbar();
        }
    };
    UndoMoveControls.prototype.undo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.rect = this.rects[i];
        }
    };
    UndoMoveControls.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.rect = this.endRects[i];
        }
    };
    return UndoMoveControls;
}(UndoItem));
var UndoMoveControlsLayerUp = (function (_super) {
    __extends(UndoMoveControlsLayerUp, _super);
    function UndoMoveControlsLayerUp(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            _this.controls.push(_controls[i]);
        }
        return _this;
    }
    UndoMoveControlsLayerUp.prototype.undo = function () {
        for (var i = this.controls.length - 1; i >= 0; i--) {
            var control = this.controls[i];
            var preEle = control.element.previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.editor.svgContainer.removeChild(control.element);
                this.editor.svgContainer.insertBefore(control.element, preEle);
            }
        }
        this.editor.rebuildControls();
    };
    UndoMoveControlsLayerUp.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextEle = control.element.nextElementSibling;
            while (nextEle && !nextEle._editorControl) {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.editor.svgContainer.removeChild(nextEle);
                this.editor.svgContainer.insertBefore(nextEle, control.element);
            }
        }
        this.editor.rebuildControls();
    };
    return UndoMoveControlsLayerUp;
}(UndoItem));
var UndoMoveControlsLayerDown = (function (_super) {
    __extends(UndoMoveControlsLayerDown, _super);
    function UndoMoveControlsLayerDown(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            _this.controls.push(_controls[i]);
        }
        return _this;
    }
    UndoMoveControlsLayerDown.prototype.undo = function () {
        for (var i = this.controls.length - 1; i >= 0; i--) {
            var control = this.controls[i];
            var nextEle = control.element.nextElementSibling;
            while (nextEle && !nextEle._editorControl) {
                nextEle = nextEle.nextElementSibling;
            }
            if (nextEle) {
                this.editor.svgContainer.removeChild(nextEle);
                this.editor.svgContainer.insertBefore(nextEle, control.element);
            }
        }
        this.editor.rebuildControls();
    };
    UndoMoveControlsLayerDown.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var preEle = control.element.previousElementSibling;
            while (preEle && !preEle._editorControl) {
                preEle = preEle.previousElementSibling;
            }
            if (preEle) {
                this.editor.svgContainer.removeChild(control.element);
                this.editor.svgContainer.insertBefore(control.element, preEle);
            }
        }
        this.editor.rebuildControls();
    };
    return UndoMoveControlsLayerDown;
}(UndoItem));
var UndoMoveControlsLayerFront = (function (_super) {
    __extends(UndoMoveControlsLayerFront, _super);
    function UndoMoveControlsLayerFront(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.nextEles = [];
        _this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            _this.controls.push(_controls[i]);
            _this.nextEles.push(_controls[i].element.nextElementSibling);
        }
        return _this;
    }
    UndoMoveControlsLayerFront.prototype.undo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextSibling = this.nextEles[i];
            this.editor.svgContainer.removeChild(control.element);
            if (nextSibling) {
                this.editor.svgContainer.insertBefore(control.element, nextSibling);
            }
            else {
                this.editor.svgContainer.appendChild(control.element);
            }
        }
        this.editor.rebuildControls();
    };
    UndoMoveControlsLayerFront.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            this.editor.svgContainer.removeChild(control.element);
            this.editor.svgContainer.appendChild(control.element);
        }
        this.editor.rebuildControls();
    };
    return UndoMoveControlsLayerFront;
}(UndoItem));
var UndoMoveControlsLayerBottom = (function (_super) {
    __extends(UndoMoveControlsLayerBottom, _super);
    function UndoMoveControlsLayerBottom(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.nextEles = [];
        _this.controls = [];
        for (var i = 0; i < _controls.length; i++) {
            _this.controls.push(_controls[i]);
            _this.nextEles.push(_controls[i].element.nextElementSibling);
        }
        return _this;
    }
    UndoMoveControlsLayerBottom.prototype.undo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            var nextSibling = this.nextEles[i];
            this.editor.svgContainer.removeChild(control.element);
            if (nextSibling) {
                this.editor.svgContainer.insertBefore(control.element, nextSibling);
            }
            else {
                this.editor.svgContainer.appendChild(control.element);
            }
        }
        this.editor.rebuildControls();
    };
    UndoMoveControlsLayerBottom.prototype.redo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            if (this.editor.svgContainer.children[0] != control.element) {
                this.editor.svgContainer.removeChild(control.element);
                this.editor.svgContainer.insertBefore(control.element, this.editor.svgContainer.children[0]);
            }
        }
        this.editor.rebuildControls();
    };
    return UndoMoveControlsLayerBottom;
}(UndoItem));
var UndoChangeLinePoint = (function (_super) {
    __extends(UndoChangeLinePoint, _super);
    function UndoChangeLinePoint(_editor, _control, _xname, _yname) {
        var _this = _super.call(this, _editor) || this;
        _this.control = _control;
        _this.xname = _xname;
        _this.yname = _yname;
        _this.oldvalueX = _this.control.lineElement.getAttribute(_this.xname);
        _this.oldvalueY = _this.control.lineElement.getAttribute(_this.yname);
        return _this;
    }
    UndoChangeLinePoint.prototype.moveFinish = function () {
        this.newvalueX = this.control.lineElement.getAttribute(this.xname);
        this.newvalueY = this.control.lineElement.getAttribute(this.yname);
    };
    UndoChangeLinePoint.prototype.undo = function () {
        this.control.lineElement.setAttribute(this.xname, this.oldvalueX);
        this.control.lineElement.setAttribute(this.yname, this.oldvalueY);
        this.control.resetPointLocation();
    };
    UndoChangeLinePoint.prototype.redo = function () {
        this.control.lineElement.setAttribute(this.xname, this.newvalueX);
        this.control.lineElement.setAttribute(this.yname, this.newvalueY);
        this.control.resetPointLocation();
    };
    return UndoChangeLinePoint;
}(UndoItem));
var UndoGroup = (function (_super) {
    __extends(UndoGroup, _super);
    function UndoGroup(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.controls = _controls;
        _this.groupCtrl = new FreeGroupControl();
        return _this;
    }
    UndoGroup.prototype.undo = function () {
        this.groupCtrl.freeControls();
        this.editor.removeControl(this.groupCtrl);
    };
    UndoGroup.prototype.redo = function () {
        this.groupCtrl.addControls(this.controls);
        this.editor.addControl(this.groupCtrl);
    };
    return UndoGroup;
}(UndoItem));
var UndoUnGroup = (function (_super) {
    __extends(UndoUnGroup, _super);
    function UndoUnGroup(_editor, _controls) {
        var _this = _super.call(this, _editor) || this;
        _this.groups = [];
        for (var i = 0; i < _controls.length; i++) {
            var item = {};
            _this.groups.push(item);
            item.controls = [];
            item.group = _controls[i];
            for (var j = 0; j < _controls[i].controls.length; j++) {
                item.controls.push(_controls[i].controls[j]);
            }
        }
        return _this;
    }
    UndoUnGroup.prototype.undo = function () {
        for (var i = 0; i < this.groups.length; i++) {
            var item = this.groups[i];
            item.group.addControls(item.controls);
            this.editor.addControl(item.group);
        }
    };
    UndoUnGroup.prototype.redo = function () {
        for (var i = 0; i < this.groups.length; i++) {
            var item = this.groups[i];
            item.group.freeControls();
            this.editor.removeControl(item.group);
        }
    };
    return UndoUnGroup;
}(UndoItem));
var UndoPaste = (function (_super) {
    __extends(UndoPaste, _super);
    function UndoPaste(_editor, _copyItems, isSameWindow) {
        var _this = _super.call(this, _editor) || this;
        _this.copyItems = [];
        _this.controls = null;
        _this.copyItems = _copyItems;
        _this.isSameWindow = isSameWindow;
        return _this;
    }
    UndoPaste.prototype.undo = function () {
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selected = false;
            this.editor.removeControl(this.controls[i]);
        }
    };
    UndoPaste.prototype.redo = function () {
        if (!this.controls) {
            this.controls = [];
            for (var i = 0; i < this.copyItems.length; i++) {
                var controlJson = this.copyItems[i];
                if (this.isSameWindow) {
                    controlJson.rect.x += 10;
                    controlJson.rect.y += 10;
                }
                if (controlJson["id"]) {
                    var idname = controlJson["id"];
                    if (this.editor.isIdExist(idname)) {
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
                if (controlJson.constructorName != "GroupControl") {
                    editorctrl.rect = controlJson.rect;
                }
                editorctrl.ctrlKey = true;
                editorctrl.selected = true;
                editorctrl.ctrlKey = false;
                this.controls.push(editorctrl);
            }
        }
        else {
            for (var i = 0; i < this.controls.length; i++) {
                this.editor.addControl(this.controls[i]);
            }
        }
    };
    return UndoPaste;
}(UndoItem));
//# sourceMappingURL=UndoManager.js.map