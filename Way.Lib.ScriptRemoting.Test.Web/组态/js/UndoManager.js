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
        this.items.push(undoObj);
        this.position++;
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
    UndoItem.prototype.undo = function () {
    };
    UndoItem.prototype.redo = function () {
    };
    return UndoItem;
}());
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
        _this.controls = _controls;
        for (var i = 0; i < _this.controls.length; i++) {
            var control = _this.controls[i];
            _this.rects.push(control.rect);
        }
        return _this;
    }
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
//# sourceMappingURL=UndoManager.js.map