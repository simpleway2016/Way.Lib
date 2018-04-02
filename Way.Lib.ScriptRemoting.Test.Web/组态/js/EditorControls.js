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
var AllSelectedControls = [];
var ManyPointDefined = 999999;
var WatchPointNames = [];
var menuDiv1 = null;
function documentElementClick(e) {
    if (menuDiv1) {
        document.body.removeChild(menuDiv1);
        menuDiv1 = null;
    }
}
document.documentElement.addEventListener("click", documentElementClick, false);
var EditorControl = (function () {
    function EditorControl(element) {
        var _this = this;
        this.pointEles = [];
        this.isInGroup = false;
        this.lastSetValueTime = 0;
        this.updatePointValueTimeoutFlag = 0;
        this.isDesignMode = true;
        this._selected = false;
        this._moveAllSelectedControl = false;
        this.movingPoint = false;
        this.element = element;
        element._editorControl = this;
        this.element.addEventListener("dragstart", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode)
                return;
            e.preventDefault();
        }, false);
        this.element.addEventListener("click", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode)
                return;
            e.stopPropagation();
        }, false);
        this.element.addEventListener("dblclick", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode)
                return;
            e.stopPropagation();
            _this.showProperty();
        }, false);
        this.element.addEventListener("mousedown", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode || e.button != 0)
                return;
            _this._moveAllSelectedControl = _this.selected;
            e.stopPropagation();
            if (e.shiftKey) {
                var myrect = _this.rect;
                var ids = [];
                for (var i = 0; i < _this.container.controls.length; i++) {
                    var ctrl = _this.container.controls[i];
                    if (ctrl != _this && ctrl.isIntersectWith(myrect)) {
                        ids.push(ctrl.id);
                    }
                }
                if (ids.length > 0) {
                    menuDiv1 = document.createElement("DIV");
                    menuDiv1.style.visibility = "hidden";
                    document.body.appendChild(menuDiv1);
                    menuDiv1.style.background = "#fff";
                    menuDiv1.style.cursor = "pointer";
                    menuDiv1.style.fontSize = "12px";
                    menuDiv1.style.padding = "3px;";
                    menuDiv1.style.border = "1px solid black";
                    for (var i = 0; i < ids.length; i++) {
                        var itemEle = document.createElement("DIV");
                        itemEle.innerHTML = ids[i];
                        itemEle.style.marginBottom = "4px";
                        itemEle.style.marginLeft = "2px";
                        itemEle.style.marginRight = "2px";
                        itemEle.addEventListener("click", function (e) {
                            CtrlKey = false;
                            var t = _this.container.getControl(e.target.innerHTML);
                            if (t) {
                                t.selected = true;
                            }
                        }, false);
                        menuDiv1.appendChild(itemEle);
                    }
                    menuDiv1.style.position = "absolute";
                    var x = e.clientX;
                    if (x + editor.scrollLeft + menuDiv1.offsetWidth > window.innerWidth)
                        x = window.innerWidth - editor.scrollLeft - menuDiv1.offsetWidth;
                    var y = e.clientY;
                    if (y + editor.scrollTop + menuDiv1.offsetHeight > window.innerHeight)
                        y = window.innerHeight - editor.scrollTop - menuDiv1.offsetHeight;
                    menuDiv1.style.left = x + "px";
                    menuDiv1.style.top = y + "px";
                    menuDiv1.style.visibility = "";
                }
                return;
            }
            CtrlKey = e.ctrlKey;
            if (CtrlKey)
                _this.selected = !_this.selected;
            else
                _this.selected = true;
            _this.mouseDownX = e.layerX;
            _this.mouseDownY = e.layerY;
            var movingCtrls = [];
            if (_this._moveAllSelectedControl) {
                for (var i = 0; i < AllSelectedControls.length; i++) {
                    AllSelectedControls[i].onBeginMoving();
                    movingCtrls.push(AllSelectedControls[i]);
                }
            }
            else {
                _this.onBeginMoving();
                movingCtrls.push(_this);
            }
            _this.undoMoveObj = new UndoMoveControls(editor, movingCtrls);
        }, false);
        document.body.addEventListener("mousemove", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode)
                return;
            if (_this.mouseDownX >= 0) {
                e.stopPropagation();
                if (_this._moveAllSelectedControl) {
                    for (var i = 0; i < AllSelectedControls.length; i++) {
                        AllSelectedControls[i].onMoving(_this.mouseDownX, _this.mouseDownY, e.layerX, e.layerY);
                    }
                }
                else {
                    _this.onMoving(_this.mouseDownX, _this.mouseDownY, e.layerX, e.layerY);
                }
            }
        }, false);
        document.body.addEventListener("mouseup", function (e) {
            if (_this.isInGroup || !_this.container || !_this.isDesignMode || e.button != 0)
                return;
            if (_this.mouseDownX >= 0) {
                e.stopPropagation();
                _this.onEndMoving();
                _this.mouseDownX = -1;
                _this.undoMoveObj.moveFinish();
                editor.undoMgr.addUndo(_this.undoMoveObj);
            }
        }, false);
    }
    Object.defineProperty(EditorControl.prototype, "selected", {
        get: function () {
            return this._selected;
        },
        set: function (value) {
            if (this.container && this.container == editor && this._selected !== value && this.isDesignMode) {
                this._selected = value;
                if (value) {
                    if (!CtrlKey) {
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
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditorControl.prototype, "rect", {
        get: function () {
            return null;
        },
        set: function (v) {
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EditorControl.prototype, "id", {
        get: function () {
            return this._id;
        },
        set: function (v) {
            if (v != this._id) {
                if (this.container && this.container.isIdExist(v)) {
                    throw new Error("id“" + v + "”已存在");
                }
                this._id = v;
            }
        },
        enumerable: true,
        configurable: true
    });
    EditorControl.prototype.getPropertiesCaption = function () {
        return null;
    };
    EditorControl.prototype.getProperties = function () {
        return null;
    };
    EditorControl.prototype.run = function () {
        this.isDesignMode = false;
    };
    EditorControl.prototype.onDevicePointValueChanged = function (devPoint) {
    };
    EditorControl.prototype.getJson = function () {
        var obj = {
            rect: this.rect,
            constructorName: this.constructor.name
        };
        var properites = this.getProperties();
        for (var i = 0; i < properites.length; i++) {
            var value = this[properites[i]];
            if (typeof value == "undefined" || (typeof value == "number" && isNaN(value)))
                continue;
            obj[properites[i]] = this[properites[i]];
        }
        return obj;
    };
    EditorControl.prototype.getScript = function () {
        var json = this.getJson();
        var script = "";
        var id = "eCtrl";
        script += id + " = new " + json.constructorName + "();\r\n";
        script += "editor.addControl(" + id + ");\r\n";
        for (var proName in json) {
            if (proName == "rect" || proName == "constructorName")
                continue;
            var type = typeof json[proName];
            if (type == "function" || type == "undefined")
                continue;
            script += id + "." + proName + " = " + JSON.stringify(json[proName]) + ";\r\n";
        }
        script += id + ".rect = " + JSON.stringify(json.rect) + ";\r\n";
        return script;
    };
    EditorControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    EditorControl.prototype.isIntersect = function (rect1, rect) {
        return rect.x < rect1.x + rect1.width && rect1.x < rect.x + rect.width && rect.y < rect1.y + rect1.height && rect1.y < rect.y + rect.height;
    };
    EditorControl.prototype.selectByPointName = function (pointName) {
    };
    EditorControl.prototype.showProperty = function () {
        if (this.propertyDialog)
            this.propertyDialog.dispose();
        this.propertyDialog = new PropertyDialog(this);
        this.propertyDialog.show();
    };
    EditorControl.prototype.onSelectedChange = function () {
        var _this = this;
        if (this.selected) {
            var pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nwse-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height - (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x - 3));
                ele.setAttribute("y", (rect.y - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ns-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width,
                    height: ele._value_rect.height - (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width / 2 - 3));
                ele.setAttribute("y", (rect.y - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nesw-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y + (y - ele._startY),
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height - (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width - 3));
                ele.setAttribute("y", (rect.y - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height,
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width - 3));
                ele.setAttribute("y", (rect.y + rect.height / 2 - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nwse-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height + (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width - 3));
                ele.setAttribute("y", (rect.y + rect.height - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ns-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width,
                    height: ele._value_rect.height + (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width / 2 - 3));
                ele.setAttribute("y", (rect.y + rect.height - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:nesw-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y,
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height + (y - ele._startY),
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x - 3));
                ele.setAttribute("y", (rect.y + rect.height - 3));
            };
            this.element.parentElement.appendChild(pointEle);
            this.pointEles.push(pointEle);
            pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x + (x - ele._startX),
                    y: ele._value_rect.y,
                    width: ele._value_rect.width - (x - ele._startX),
                    height: ele._value_rect.height,
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x - 3));
                ele.setAttribute("y", (rect.y + rect.height / 2 - 3));
            };
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
    };
    EditorControl.prototype.resetPointLocation = function () {
        if (!this.selected)
            return;
        var rect = this.rect;
        for (var i = 0; i < this.pointEles.length; i++) {
            this.pointEles[i]._setLocation(this.pointEles[i], rect);
        }
    };
    EditorControl.prototype.setEvent = function (pointEle) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) {
            if (e.button == 0) {
                _this.pointMouseDown(e, pointEle);
            }
        }, false);
        pointEle.addEventListener("mousemove", function (e) { _this.pointMouseMove(e, pointEle); }, false);
        pointEle.addEventListener("mouseup", function (e) {
            if (e.button == 0) {
                _this.pointMouseUp(e, pointEle);
            }
        }, false);
    };
    EditorControl.prototype.pointMouseDown = function (e, pointEle) {
        e.stopPropagation();
        this.movingPoint = true;
        pointEle._startX = e.layerX;
        pointEle._startY = e.layerY;
        pointEle._value_rect = this.rect;
        this.undoObj = new UndoMoveControls(editor, [this]);
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    EditorControl.prototype.pointMouseMove = function (e, pointEle) {
        if (this.movingPoint) {
            e.stopPropagation();
            pointEle._moveFunc(pointEle, e.layerX, e.layerY);
            this.resetPointLocation();
        }
    };
    EditorControl.prototype.pointMouseUp = function (e, pointEle) {
        if (this.movingPoint) {
            e.stopPropagation();
            this.movingPoint = false;
            pointEle.releaseCapture();
            this.undoObj.moveFinish();
            editor.undoMgr.addUndo(this.undoObj);
        }
    };
    EditorControl.prototype.onBeginMoving = function () {
    };
    EditorControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
    };
    EditorControl.prototype.onEndMoving = function () {
    };
    return EditorControl;
}());
var LineControl = (function (_super) {
    __extends(LineControl, _super);
    function LineControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this._undoObj = null;
        _this.lineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.element.appendChild(_this.lineElement);
        _this.lineElement.setAttribute('style', 'stroke:#aaaaaa;stroke-width:5;');
        _this.virtualLineElement = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.element.appendChild(_this.virtualLineElement);
        _this.virtualLineElement.setAttribute('style', 'stroke:red;stroke-opacity:0;stroke-width:10;cursor:pointer;');
        return _this;
    }
    Object.defineProperty(LineControl.prototype, "rect", {
        get: function () {
            return {
                x: Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2"))),
                y: Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2"))),
                width: Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2"))),
                height: Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2"))),
            };
        },
        set: function (v) {
            var x = Math.min(parseInt(this.lineElement.getAttribute("x1")), parseInt(this.lineElement.getAttribute("x2")));
            var y = Math.min(parseInt(this.lineElement.getAttribute("y1")), parseInt(this.lineElement.getAttribute("y2")));
            this.lineElement.setAttribute("x1", (parseInt(this.lineElement.getAttribute("x1")) + v.x - x));
            this.lineElement.setAttribute("x2", (parseInt(this.lineElement.getAttribute("x2")) + v.x - x));
            this.lineElement.setAttribute("y1", (parseInt(this.lineElement.getAttribute("y1")) + v.y - y));
            this.lineElement.setAttribute("y2", (parseInt(this.lineElement.getAttribute("y2")) + v.y - y));
            var height = Math.abs(parseInt(this.lineElement.getAttribute("y1")) - parseInt(this.lineElement.getAttribute("y2")));
            if (parseInt(this.lineElement.getAttribute("y1")) < parseInt(this.lineElement.getAttribute("y2"))) {
                var y2 = parseInt(this.lineElement.getAttribute("y2")) + v.height - height;
                this.lineElement.setAttribute("y2", y2);
            }
            else {
                var y1 = parseInt(this.lineElement.getAttribute("y1")) + v.height - height;
                this.lineElement.setAttribute("y1", y1);
            }
            var width = Math.abs(parseInt(this.lineElement.getAttribute("x1")) - parseInt(this.lineElement.getAttribute("x2")));
            if (parseInt(this.lineElement.getAttribute("x1")) < parseInt(this.lineElement.getAttribute("x2"))) {
                var x2 = parseInt(this.lineElement.getAttribute("x2")) + v.width - width;
                this.lineElement.setAttribute("x2", x2);
            }
            else {
                var x1 = parseInt(this.lineElement.getAttribute("x1")) + v.width - width;
                this.lineElement.setAttribute("x1", x1);
            }
            this.virtualLineElement.setAttribute("x1", this.lineElement.getAttribute("x1"));
            this.virtualLineElement.setAttribute("x2", this.lineElement.getAttribute("x2"));
            this.virtualLineElement.setAttribute("y1", this.lineElement.getAttribute("y1"));
            this.virtualLineElement.setAttribute("y2", this.lineElement.getAttribute("y2"));
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineControl.prototype, "point", {
        get: function () {
            return {
                x1: parseInt(this.lineElement.getAttribute("x1")),
                x2: parseInt(this.lineElement.getAttribute("x2")),
                y1: parseInt(this.lineElement.getAttribute("y1")),
                y2: parseInt(this.lineElement.getAttribute("y2")),
            };
        },
        set: function (v) {
            this.lineElement.setAttribute("x1", v.x1);
            this.lineElement.setAttribute("x2", v.x2);
            this.lineElement.setAttribute("y1", v.y1);
            this.lineElement.setAttribute("y2", v.y2);
            this.virtualLineElement.setAttribute("x1", v.x1);
            this.virtualLineElement.setAttribute("x2", v.x2);
            this.virtualLineElement.setAttribute("y1", v.y1);
            this.virtualLineElement.setAttribute("y2", v.y2);
        },
        enumerable: true,
        configurable: true
    });
    LineControl.prototype.getJson = function () {
        var obj = _super.prototype.getJson.call(this);
        obj.point = this.point;
        return obj;
    };
    Object.defineProperty(LineControl.prototype, "lineWidth", {
        get: function () {
            return this.lineElement.style.strokeWidth;
        },
        set: function (v) {
            this.lineElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LineControl.prototype, "color", {
        get: function () {
            return this.lineElement.style.stroke;
        },
        set: function (v) {
            this.lineElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    LineControl.prototype.getPropertiesCaption = function () {
        return ["id", "线宽", "颜色"];
    };
    LineControl.prototype.getProperties = function () {
        return ["id", "lineWidth", "color"];
    };
    LineControl.prototype.isIntersectWith = function (rect) {
        var myrect = this.rect;
        return this.isIntersect(myrect, rect);
    };
    LineControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            var pointEle1 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle1.setAttribute("r", "5");
            pointEle1.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            pointEle1.xName = "x1";
            pointEle1.yName = "y1";
            this.lineElement.parentElement.appendChild(pointEle1);
            var pointEle2 = document.createElementNS('http://www.w3.org/2000/svg', 'circle');
            pointEle2.setAttribute("r", "5");
            pointEle2.setAttribute('style', 'stroke:black;stroke-width:2;fill:white;cursor:ew-resize;');
            pointEle2.xName = "x2";
            pointEle2.yName = "y2";
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
    };
    LineControl.prototype.resetPointLocation = function () {
        if (!this.selected)
            return;
        this.pointEles[0].setAttribute("cx", this.lineElement.x1.animVal.value);
        this.pointEles[0].setAttribute("cy", this.lineElement.y1.animVal.value);
        this.pointEles[1].setAttribute("cx", this.lineElement.x2.animVal.value);
        this.pointEles[1].setAttribute("cy", this.lineElement.y2.animVal.value);
    };
    LineControl.prototype.mySetEvent = function (pointEle, xName, yName) {
        var _this = this;
        pointEle.addEventListener("click", function (e) { e.stopPropagation(); }, false);
        pointEle.addEventListener("mousedown", function (e) {
            if (e.button == 0) {
                _this._pointMouseDown(e, pointEle, xName, yName);
            }
        }, false);
        pointEle.addEventListener("mousemove", function (e) { _this._pointMouseMove(e, pointEle, xName, yName); }, false);
        pointEle.addEventListener("mouseup", function (e) {
            if (e.button == 0) {
                _this._pointMouseUp(e, pointEle);
            }
        }, false);
    };
    LineControl.prototype._pointMouseDown = function (e, pointEle, xName, yName) {
        e.stopPropagation();
        this.startX = e.layerX;
        this.startY = e.layerY;
        this.valueX = parseInt(this.lineElement.getAttribute(xName));
        this.valueY = parseInt(this.lineElement.getAttribute(yName));
        this._undoObj = new UndoChangeLinePoint(editor, this, xName, yName);
        if (pointEle.setCapture)
            pointEle.setCapture();
        else if (window.captureEvents)
            window.captureEvents(Event.MOUSEMOVE | Event.MOUSEUP);
    };
    LineControl.prototype._pointMouseMove = function (e, pointEle, xName, yName) {
        if (this._undoObj) {
            e.stopPropagation();
            pointEle.setAttribute("cx", this.valueX + e.layerX - this.startX);
            pointEle.setAttribute("cy", this.valueY + e.layerY - this.startY);
            this.lineElement.setAttribute(xName, (this.valueX + e.layerX - this.startX));
            this.lineElement.setAttribute(yName, (this.valueY + e.layerY - this.startY));
            this.virtualLineElement.setAttribute(xName, (this.valueX + e.layerX - this.startX));
            this.virtualLineElement.setAttribute(yName, (this.valueY + e.layerY - this.startY));
        }
    };
    LineControl.prototype._pointMouseUp = function (e, pointEle) {
        if (this._undoObj) {
            e.stopPropagation();
            pointEle.releaseCapture();
            this._undoObj.moveFinish();
            editor.undoMgr.addUndo(this._undoObj);
            this._undoObj = null;
        }
    };
    LineControl.prototype.onBeginMoving = function () {
        this.lineElement._x1 = parseInt(this.lineElement.getAttribute("x1"));
        this.lineElement._y1 = parseInt(this.lineElement.getAttribute("y1"));
        this.lineElement._x2 = parseInt(this.lineElement.getAttribute("x2"));
        this.lineElement._y2 = parseInt(this.lineElement.getAttribute("y2"));
    };
    LineControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x1 = (this.lineElement._x1 + nowX - downX);
        var y1 = (this.lineElement._y1 + nowY - downY);
        var x2 = (this.lineElement._x2 + nowX - downX);
        var y2 = (this.lineElement._y2 + nowY - downY);
        this.lineElement.setAttribute("x1", x1);
        this.lineElement.setAttribute("y1", y1);
        this.lineElement.setAttribute("x2", x2);
        this.lineElement.setAttribute("y2", y2);
        this.virtualLineElement.setAttribute("x1", x1);
        this.virtualLineElement.setAttribute("y1", y1);
        this.virtualLineElement.setAttribute("x2", x2);
        this.virtualLineElement.setAttribute("y2", y2);
        if (this.selected) {
            this.pointEles[0].setAttribute("cx", x1);
            this.pointEles[0].setAttribute("cy", y1);
            this.pointEles[1].setAttribute("cx", x2);
            this.pointEles[1].setAttribute("cy", y2);
        }
    };
    LineControl.prototype.onEndMoving = function () {
    };
    return LineControl;
}(EditorControl));
var RectControl = (function (_super) {
    __extends(RectControl, _super);
    function RectControl(element) {
        var _this = _super.call(this, element ? element : document.createElementNS('http://www.w3.org/2000/svg', 'rect')) || this;
        _this.startX = 0;
        _this.startY = 0;
        _this._devicePoint = "";
        _this.rectElement = _this.element;
        _this.rectElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(RectControl.prototype, "rect", {
        get: function () {
            return {
                x: parseInt(this.rectElement.getAttribute("x")),
                y: parseInt(this.rectElement.getAttribute("y")),
                width: parseInt(this.rectElement.getAttribute("width")),
                height: parseInt(this.rectElement.getAttribute("height")),
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "strokeWidth", {
        get: function () {
            return this.rectElement.style.strokeWidth;
        },
        set: function (v) {
            this.rectElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "colorStroke", {
        get: function () {
            return this.rectElement.style.stroke;
        },
        set: function (v) {
            this.rectElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "colorFill", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RectControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    RectControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.rectElement.scrollIntoView();
        }
    };
    Object.defineProperty(RectControl.prototype, "scriptOnValueChange", {
        get: function () {
            return this._scriptOnValueChange;
        },
        set: function (v) {
            this._scriptOnValueChange = v;
        },
        enumerable: true,
        configurable: true
    });
    RectControl.prototype.onDevicePointValueChanged = function (devPoint) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0) {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e) {
                alert(e.message);
            }
        }
    };
    RectControl.prototype.getPropertiesCaption = function () {
        return ["id", "边框大小", "边框颜色", "填充颜色", "设备点", "值变化脚本"];
    };
    RectControl.prototype.getProperties = function () {
        return ["id", "strokeWidth", "colorStroke", "colorFill", "devicePoint", "scriptOnValueChange"];
    };
    RectControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    RectControl.prototype.onBeginMoving = function () {
        this.rectElement._x = parseInt(this.rectElement.getAttribute("x"));
        this.rectElement._y = parseInt(this.rectElement.getAttribute("y"));
    };
    RectControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._x + nowX - downX);
        var y = (this.rectElement._y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    RectControl.prototype.onEndMoving = function () {
    };
    return RectControl;
}(EditorControl));
var EllipseControl = (function (_super) {
    __extends(EllipseControl, _super);
    function EllipseControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'ellipse')) || this;
        _this.startX = 0;
        _this.startY = 0;
        _this._devicePoint = "";
        _this.rootElement = _this.element;
        _this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(EllipseControl.prototype, "rect", {
        get: function () {
            var myrect = {
                x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("rx")),
                y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("ry")),
                width: parseInt(this.rootElement.getAttribute("rx")) * 2,
                height: parseInt(this.rootElement.getAttribute("ry")) * 2,
            };
            return myrect;
        },
        set: function (v) {
            var rx = v.width / 2;
            var ry = v.height / 2;
            this.rootElement.setAttribute("cx", v.x + rx);
            this.rootElement.setAttribute("cy", v.y + ry);
            this.rootElement.setAttribute("rx", rx);
            this.rootElement.setAttribute("ry", ry);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "strokeWidth", {
        get: function () {
            return this.rootElement.style.strokeWidth;
        },
        set: function (v) {
            this.rootElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "colorStroke", {
        get: function () {
            return this.rootElement.style.stroke;
        },
        set: function (v) {
            this.rootElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "colorFill", {
        get: function () {
            return this.rootElement.style.fill;
        },
        set: function (v) {
            this.rootElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(EllipseControl.prototype, "scriptOnValueChange", {
        get: function () {
            return this._scriptOnValueChange;
        },
        set: function (v) {
            this._scriptOnValueChange = v;
        },
        enumerable: true,
        configurable: true
    });
    EllipseControl.prototype.onDevicePointValueChanged = function (devPoint) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0) {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e) {
                alert(e.message);
            }
        }
    };
    EllipseControl.prototype.getPropertiesCaption = function () {
        return ["id", "边框大小", "边框颜色", "填充颜色", "设备点", "值变化脚本"];
    };
    EllipseControl.prototype.getProperties = function () {
        return ["id", "strokeWidth", "colorStroke", "colorFill", "devicePoint", "scriptOnValueChange"];
    };
    EllipseControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    EllipseControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.rootElement.scrollIntoView();
        }
    };
    EllipseControl.prototype.onBeginMoving = function () {
        this.rootElement._cx = parseInt(this.rootElement.getAttribute("cx"));
        this.rootElement._cy = parseInt(this.rootElement.getAttribute("cy"));
    };
    EllipseControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rootElement._cx + nowX - downX);
        var y = (this.rootElement._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    EllipseControl.prototype.onEndMoving = function () {
    };
    return EllipseControl;
}(EditorControl));
var CircleControl = (function (_super) {
    __extends(CircleControl, _super);
    function CircleControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'circle')) || this;
        _this.startX = 0;
        _this.startY = 0;
        _this._devicePoint = "";
        _this.rootElement = _this.element;
        _this.rootElement.setAttribute('style', 'fill:#eeeeee;stroke:#aaaaaa;stroke-width:1;');
        return _this;
    }
    Object.defineProperty(CircleControl.prototype, "rect", {
        get: function () {
            var myrect = {
                x: parseInt(this.rootElement.getAttribute("cx")) - parseInt(this.rootElement.getAttribute("r")),
                y: parseInt(this.rootElement.getAttribute("cy")) - parseInt(this.rootElement.getAttribute("r")),
                width: parseInt(this.rootElement.getAttribute("r")) * 2,
                height: parseInt(this.rootElement.getAttribute("r")) * 2,
            };
            return myrect;
        },
        set: function (v) {
            var r = v.width / 2;
            this.rootElement.setAttribute("cx", v.x + r);
            this.rootElement.setAttribute("cy", v.y + r);
            this.rootElement.setAttribute("r", r);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "strokeWidth", {
        get: function () {
            return this.rootElement.style.strokeWidth;
        },
        set: function (v) {
            this.rootElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "colorStroke", {
        get: function () {
            return this.rootElement.style.stroke;
        },
        set: function (v) {
            this.rootElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "colorFill", {
        get: function () {
            return this.rootElement.style.fill;
        },
        set: function (v) {
            this.rootElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CircleControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    CircleControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.rootElement.scrollIntoView();
        }
    };
    Object.defineProperty(CircleControl.prototype, "scriptOnValueChange", {
        get: function () {
            return this._scriptOnValueChange;
        },
        set: function (v) {
            this._scriptOnValueChange = v;
        },
        enumerable: true,
        configurable: true
    });
    CircleControl.prototype.onDevicePointValueChanged = function (devPoint) {
        if (this._scriptOnValueChange && this._scriptOnValueChange.length > 0) {
            try {
                var value = devPoint.value;
                eval(this._scriptOnValueChange);
            }
            catch (e) {
                alert(e.message);
            }
        }
    };
    CircleControl.prototype.getPropertiesCaption = function () {
        return ["id", "边框大小", "边框颜色", "填充颜色", "设备点", "值变化脚本"];
    };
    CircleControl.prototype.getProperties = function () {
        return ["id", "strokeWidth", "colorStroke", "colorFill", "devicePoint", "scriptOnValueChange"];
    };
    CircleControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    CircleControl.prototype.onSelectedChange = function () {
        var _this = this;
        if (this.selected) {
            var pointEle = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            pointEle.setAttribute("width", "6");
            pointEle.setAttribute("height", "6");
            pointEle.setAttribute('style', 'fill:red;cursor:ew-resize;');
            pointEle._moveFunc = function (ele, x, y) {
                _this.rect = {
                    x: ele._value_rect.x,
                    y: ele._value_rect.y,
                    width: ele._value_rect.width + (x - ele._startX),
                    height: ele._value_rect.height,
                };
            };
            pointEle._setLocation = function (ele, rect) {
                ele.setAttribute("x", (rect.x + rect.width - 3));
                ele.setAttribute("y", (rect.y + rect.height / 2 - 3));
            };
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
    };
    CircleControl.prototype.onBeginMoving = function () {
        this.rootElement._cx = parseInt(this.rootElement.getAttribute("cx"));
        this.rootElement._cy = parseInt(this.rootElement.getAttribute("cy"));
    };
    CircleControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rootElement._cx + nowX - downX);
        var y = (this.rootElement._cy + nowY - downY);
        this.rootElement.setAttribute("cx", x);
        this.rootElement.setAttribute("cy", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    CircleControl.prototype.onEndMoving = function () {
    };
    return CircleControl;
}(EditorControl));
var ImageControl = (function (_super) {
    __extends(ImageControl, _super);
    function ImageControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'image')) || this;
        _this.imgElement = _this.element;
        return _this;
    }
    Object.defineProperty(ImageControl.prototype, "imgSrc", {
        get: function () {
            return this.imgElement.href.baseVal;
        },
        set: function (v) {
            this.imgElement.href.baseVal = v;
        },
        enumerable: true,
        configurable: true
    });
    ImageControl.prototype.getPropertiesCaption = function () {
        return ["id", "图片"];
    };
    ImageControl.prototype.getProperties = function () {
        return ["id", "imgSrc"];
    };
    return ImageControl;
}(RectControl));
var TextControl = (function (_super) {
    __extends(TextControl, _super);
    function TextControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this._canSetValue = false;
        _this._devicePoint = "";
        _this._showedPrompt = false;
        _this.groupElement = _this.element;
        _this.groupElement.setAttribute("transform", "translate(0 0) scale(1 1)");
        _this.textElement = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        _this.groupElement.appendChild(_this.textElement);
        _this.textElement.textContent = "Text";
        _this.textElement.setAttribute("x", "0");
        _this.textElement.setAttribute("y", "17");
        _this.textElement.setAttribute('style', 'fill:#111111;cursor:default;-moz-user-select:none;');
        _this.textElement.setAttribute('font-size', "16");
        _this.textElement.setAttribute("transform", "rotate(0 0,17)");
        return _this;
    }
    Object.defineProperty(TextControl.prototype, "text", {
        get: function () {
            return this.textElement.textContent;
        },
        set: function (v) {
            if (v != this.textElement.textContent) {
                this.textElement.textContent = v;
                this.resetPointLocation();
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "size", {
        get: function () {
            return parseInt(this.textElement.getAttribute("font-size"));
        },
        set: function (v) {
            this.textElement.setAttribute("font-size", v);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "colorFill", {
        get: function () {
            return this.textElement.style.fill;
        },
        set: function (v) {
            this.textElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "canSetValue", {
        get: function () {
            return this._canSetValue;
        },
        set: function (v) {
            this._canSetValue = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    TextControl.prototype.onDevicePointValueChanged = function (devPoint) {
        this._lastDevPoint = devPoint;
        this.updateText(devPoint.value);
    };
    TextControl.prototype.updateText = function (value) {
        this.text = value;
    };
    TextControl.prototype.run = function () {
        var _this = this;
        _super.prototype.run.call(this);
        if (this.devicePoint.length > 0 && this.canSetValue) {
            this.textElement.style.cursor = "pointer";
            this.element.addEventListener("click", function (e) {
                e.stopPropagation();
                if (!_this._showedPrompt) {
                    _this._showedPrompt = true;
                    var newValue = window.prompt("请输入新的数值", "");
                    _this._showedPrompt = false;
                    if (newValue && newValue.length > 0) {
                        var valueType = typeof _this._lastDevPoint.value;
                        if (valueType == "number") {
                            if (newValue.indexOf(".") >= 0)
                                newValue = parseFloat(newValue);
                            else
                                newValue = parseInt(newValue);
                        }
                        if (_this._lastDevPoint.value != newValue) {
                            _this.container.writeValue(_this.devicePoint, _this._lastDevPoint.addr, newValue);
                            _this.lastSetValueTime = new Date().getTime();
                            _this.updateText(newValue);
                        }
                    }
                }
            }, false);
        }
    };
    TextControl.prototype.getPropertiesCaption = function () {
        return ["id", "文字", "大小", "颜色", "旋转角度", "字体", "粗体", "斜体", "下划线", "设备点", "运行时允许输入值"];
    };
    TextControl.prototype.getProperties = function () {
        return ["id", "text", "size", "colorFill", "rotate", "fontFamily", "isBold", "isItalic", "isUnderline", "devicePoint", "canSetValue"];
    };
    Object.defineProperty(TextControl.prototype, "rect", {
        get: function () {
            var transform = this.groupElement.getAttribute("transform");
            var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
            try {
                var clientRect = this.textElement.getBoundingClientRect();
                return {
                    x: parseInt(result[1]),
                    y: parseInt(result[2]),
                    width: clientRect.width / editor.currentScale,
                    height: clientRect.height / editor.currentScale
                };
            }
            catch (e) {
                return {
                    x: parseInt(result[1]),
                    y: parseInt(result[2]),
                    width: 0,
                    height: 0
                };
            }
        },
        set: function (v) {
            var x = v.x;
            var y = v.y;
            this.groupElement.setAttribute("transform", "translate(" + x + " " + y + ") scale(1 1)");
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "rotate", {
        get: function () {
            var transform = this.textElement.getAttribute("transform");
            var result = /rotate\(([0-9]+) /.exec(transform);
            return parseInt(result[1]);
        },
        set: function (v) {
            this.textElement.setAttribute("transform", "rotate(" + v + " 0,17)");
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "isUnderline", {
        get: function () {
            return this.textElement.getAttribute("text-decoration") === "underline";
        },
        set: function (v) {
            this.textElement.setAttribute("text-decoration", v ? "underline" : "");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "isBold", {
        get: function () {
            return this.textElement.getAttribute("font-weight") === "900";
        },
        set: function (v) {
            this.textElement.setAttribute("font-weight", v ? "900" : "");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "isItalic", {
        get: function () {
            return this.textElement.getAttribute("font-style") === "italic";
        },
        set: function (v) {
            this.textElement.setAttribute("font-style", v ? "italic" : "");
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TextControl.prototype, "fontFamily", {
        get: function () {
            return this.textElement.getAttribute("font-family");
        },
        set: function (v) {
            this.textElement.setAttribute("font-family", v);
        },
        enumerable: true,
        configurable: true
    });
    TextControl.prototype.isIntersectWith = function (rect) {
        var clientRect = this.textElement.getBoundingClientRect();
        var myrect = {
            x: (clientRect.left + editor.svgContainer.parentElement.scrollLeft) / editor.currentScale,
            y: (clientRect.top - editor.divContainer.offsetTop + editor.svgContainer.parentElement.scrollTop) / editor.currentScale,
            width: clientRect.width / editor.currentScale, height: clientRect.height / editor.currentScale
        };
        return this.isIntersect(myrect, rect);
    };
    TextControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.textElement.scrollIntoView();
        }
    };
    TextControl.prototype.onSelectedChange = function () {
        if (this.selected) {
            var clientRect = this.textElement.getBoundingClientRect();
            var myrect = {
                x: (clientRect.left + editor.svgContainer.parentElement.scrollLeft) / editor.currentScale,
                y: (clientRect.top - editor.divContainer.offsetTop + editor.svgContainer.parentElement.scrollTop) / editor.currentScale,
                width: clientRect.width / editor.currentScale,
                height: clientRect.height / editor.currentScale
            };
            this.selectingElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            this.selectingElement.setAttribute('x', (myrect.x - 5));
            this.selectingElement.setAttribute('y', (myrect.y - 5));
            this.selectingElement.setAttribute('width', (myrect.width + 10));
            this.selectingElement.setAttribute('height', (myrect.height + 10));
            this.selectingElement.setAttribute('style', 'fill:none;stroke:black;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
            this.selectingElement.onmousedown = function (e) {
                if (e.button != 1) {
                    e.stopPropagation();
                }
            };
            this.groupElement.parentElement.appendChild(this.selectingElement);
        }
        else {
            this.groupElement.parentElement.removeChild(this.selectingElement);
        }
    };
    TextControl.prototype.resetPointLocation = function () {
        if (!this.selected)
            return;
        var clientRect = this.textElement.getBoundingClientRect();
        var myrect = {
            x: (clientRect.left + editor.svgContainer.parentElement.scrollLeft) / editor.currentScale,
            y: (clientRect.top - editor.divContainer.offsetTop + editor.svgContainer.parentElement.scrollTop) / editor.currentScale,
            width: clientRect.width / editor.currentScale,
            height: clientRect.height / editor.currentScale
        };
        this.selectingElement.setAttribute('x', (myrect.x - 5));
        this.selectingElement.setAttribute('y', (myrect.y - 5));
        this.selectingElement.setAttribute('width', (myrect.width + 10));
        this.selectingElement.setAttribute('height', (myrect.height + 10));
    };
    TextControl.prototype.onBeginMoving = function () {
        var rect = this.rect;
        this.groupElement._x = rect.x;
        this.groupElement._y = rect.y;
    };
    TextControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.groupElement._x + nowX - downX);
        var y = (this.groupElement._y + nowY - downY);
        this.rect = { x: x, y: y };
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    TextControl.prototype.onEndMoving = function () {
    };
    return TextControl;
}(EditorControl));
var CylinderControl = (function (_super) {
    __extends(CylinderControl, _super);
    function CylinderControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this._value = 0;
        _this._max = 100;
        _this._min = 0;
        _this._devicePoint = "";
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.rectElement.setAttribute("rx", "200000");
        _this.rectElement.setAttribute("ry", "20");
        _this.rectElement.setAttribute('width', "0");
        _this.rectElement.setAttribute('height', "0");
        _this.rectElement.setAttribute('style', 'fill:#ffffff;stroke:#aaaaaa;stroke-width:1;');
        _this.cylinderElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.cylinderElement.setAttribute("rx", "6");
        _this.cylinderElement.setAttribute("ry", "6");
        _this.cylinderElement.setAttribute("height", "0");
        _this.cylinderElement.setAttribute('style', 'fill:#00BF00;stroke:none;');
        _this.element.appendChild(_this.rectElement);
        _this.element.appendChild(_this.cylinderElement);
        return _this;
    }
    Object.defineProperty(CylinderControl.prototype, "value", {
        get: function () {
            return this._value;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value) {
                this._value = v;
                this.resetCylinder(this.rect);
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "max", {
        get: function () {
            return this._max;
        },
        set: function (v) {
            this._max = parseFloat(v);
            if (this._max <= this._min)
                this._max = this._min + 1;
            try {
                this.resetCylinder(this.rect);
            }
            catch (e) {
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "min", {
        get: function () {
            return this._min;
        },
        set: function (v) {
            this._min = parseFloat(v);
            if (this._min >= this._max)
                this._max = this._max - 1;
            try {
                this.resetCylinder(this.rect);
            }
            catch (e) {
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    CylinderControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.cylinderElement.scrollIntoView();
        }
    };
    CylinderControl.prototype.onDevicePointValueChanged = function (devPoint) {
        if (devPoint.max != null && devPoint.max != this.max)
            this.max = devPoint.max;
        if (devPoint.min != null && devPoint.min != this.min)
            this.min = devPoint.min;
        this.value = devPoint.value;
    };
    Object.defineProperty(CylinderControl.prototype, "rect", {
        get: function () {
            return {
                x: parseInt(this.rectElement.getAttribute("x")),
                y: parseInt(this.rectElement.getAttribute("y")),
                width: parseInt(this.rectElement.getAttribute("width")),
                height: parseInt(this.rectElement.getAttribute("height")),
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.resetCylinder(v);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "strokeWidth", {
        get: function () {
            return this.rectElement.style.strokeWidth;
        },
        set: function (v) {
            this.rectElement.style.strokeWidth = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorStroke", {
        get: function () {
            return this.rectElement.style.stroke;
        },
        set: function (v) {
            this.rectElement.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorFill", {
        get: function () {
            return this.cylinderElement.style.fill;
        },
        set: function (v) {
            this.cylinderElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(CylinderControl.prototype, "colorBg", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    CylinderControl.prototype.getPropertiesCaption = function () {
        return ["id", "边框大小", "边框颜色", "底色", "填充颜色", "值", "最大值", "最小值", "设备点"];
    };
    CylinderControl.prototype.getProperties = function () {
        return ["id", "strokeWidth", "colorStroke", "colorBg", "colorFill", "value", "max", "min", "devicePoint"];
    };
    CylinderControl.prototype.resetCylinder = function (rect) {
        var ctrlHeight = rect.height - 40;
        var myheight = parseInt((((this.value - this.min) * ctrlHeight) / (this.max - this.min)));
        myheight = Math.min(ctrlHeight, myheight);
        if (myheight < 0)
            myheight = 0;
        this.cylinderElement.setAttribute("x", rect.x + 10);
        this.cylinderElement.setAttribute("y", (rect.y + 20 + ctrlHeight - myheight));
        this.cylinderElement.setAttribute("width", (rect.width - 20));
        this.cylinderElement.setAttribute("height", myheight);
    };
    CylinderControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    CylinderControl.prototype.onBeginMoving = function () {
        this.rectElement._rect = this.rect;
    };
    CylinderControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._rect.x + nowX - downX);
        var y = (this.rectElement._rect.y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        this.resetCylinder({ x: x, y: y, width: this.rectElement._rect.width, height: this.rectElement._rect.height });
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    CylinderControl.prototype.onEndMoving = function () {
    };
    return CylinderControl;
}(EditorControl));
var TrendControl = (function (_super) {
    __extends(TrendControl, _super);
    function TrendControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this.values1 = [];
        _this.values2 = [];
        _this.values3 = [];
        _this.values4 = [];
        _this.values5 = [];
        _this.values6 = [];
        _this.values7 = [];
        _this.values8 = [];
        _this.values9 = [];
        _this.values10 = [];
        _this.values11 = [];
        _this.values12 = [];
        _this._value1 = 0;
        _this._value2 = 0;
        _this._value3 = 0;
        _this._value4 = 0;
        _this._value5 = 0;
        _this._value6 = 0;
        _this._value7 = 0;
        _this._value8 = 0;
        _this._value9 = 0;
        _this._value10 = 0;
        _this._value11 = 0;
        _this._value12 = 0;
        _this._devicePoint1 = "";
        _this._devicePoint2 = "";
        _this._devicePoint3 = "";
        _this._devicePoint4 = "";
        _this._devicePoint5 = "";
        _this._devicePoint6 = "";
        _this._devicePoint7 = "";
        _this._devicePoint8 = "";
        _this._devicePoint9 = "";
        _this._devicePoint10 = "";
        _this._devicePoint11 = "";
        _this._devicePoint12 = "";
        _this.running = false;
        _this.startX = 0;
        _this.startY = 0;
        _this.rectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
        _this.element.appendChild(_this.rectElement);
        _this.rectElement.setAttribute('style', 'fill:#000000;stroke:none;');
        _this.line_left_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.line_left_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        _this.element.appendChild(_this.line_left_Ele);
        _this.line_bottom_Ele = document.createElementNS('http://www.w3.org/2000/svg', 'line');
        _this.line_bottom_Ele.setAttribute('style', 'stroke:#ffffff;stroke-width:1;');
        _this.element.appendChild(_this.line_bottom_Ele);
        for (var i = 1; i <= 12; i++) {
            var pe = document.createElementNS('http://www.w3.org/2000/svg', 'path');
            pe.setAttribute('style', 'stroke-width:1;fill:none;stroke:#ffffff;');
            pe.setAttribute("transform", "translate(0 0)");
            _this.element.appendChild(pe);
            _this["pathElement" + i] = pe;
        }
        _this.textGroupElement = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        _this.textGroupElement.setAttribute("transform", "translate(0 0)");
        _this.element.appendChild(_this.textGroupElement);
        _this.devicePoint = ManyPointDefined;
        return _this;
    }
    Object.defineProperty(TrendControl.prototype, "value1", {
        get: function () {
            return this._value1;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value1) {
                this._value1 = v;
                this.values1.push({
                    value: this._value1,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value2", {
        get: function () {
            return this._value2;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value2) {
                this._value2 = v;
                this.values2.push({
                    value: this._value2,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value3", {
        get: function () {
            return this._value3;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value3) {
                this._value3 = v;
                this.values3.push({
                    value: this._value3,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value4", {
        get: function () {
            return this._value4;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value4) {
                this._value4 = v;
                this.values4.push({
                    value: this._value4,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value5", {
        get: function () {
            return this._value5;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value5) {
                this._value5 = v;
                this.values5.push({
                    value: this._value5,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value6", {
        get: function () {
            return this._value6;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value6) {
                this._value6 = v;
                this.values6.push({
                    value: this._value6,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value7", {
        get: function () {
            return this._value7;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value7) {
                this._value7 = v;
                this.values7.push({
                    value: this._value7,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value8", {
        get: function () {
            return this._value8;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value8) {
                this._value8 = v;
                this.values8.push({
                    value: this._value8,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value9", {
        get: function () {
            return this._value9;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value9) {
                this._value9 = v;
                this.values9.push({
                    value: this._value9,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value10", {
        get: function () {
            return this._value10;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value10) {
                this._value10 = v;
                this.values10.push({
                    value: this._value10,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value11", {
        get: function () {
            return this._value11;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value11) {
                this._value11 = v;
                this.values11.push({
                    value: this._value11,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "value12", {
        get: function () {
            return this._value12;
        },
        set: function (v) {
            v = parseFloat(v);
            if (v != this._value12) {
                this._value12 = v;
                this.values12.push({
                    value: this._value12,
                    time: new Date().getTime()
                });
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max1", {
        get: function () {
            return this._max1;
        },
        set: function (v) {
            this._max1 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min1", {
        get: function () {
            return this._min1;
        },
        set: function (v) {
            this._min1 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max2", {
        get: function () {
            return this._max2;
        },
        set: function (v) {
            this._max2 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min2", {
        get: function () {
            return this._min2;
        },
        set: function (v) {
            this._min2 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max3", {
        get: function () {
            return this._max3;
        },
        set: function (v) {
            this._max3 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min3", {
        get: function () {
            return this._min3;
        },
        set: function (v) {
            this._min3 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max4", {
        get: function () {
            return this._max4;
        },
        set: function (v) {
            this._max4 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min4", {
        get: function () {
            return this._min4;
        },
        set: function (v) {
            this._min4 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max5", {
        get: function () {
            return this._max5;
        },
        set: function (v) {
            this._max5 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min5", {
        get: function () {
            return this._min5;
        },
        set: function (v) {
            this._min5 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max6", {
        get: function () {
            return this._max6;
        },
        set: function (v) {
            this._max6 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min6", {
        get: function () {
            return this._min6;
        },
        set: function (v) {
            this._min6 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max7", {
        get: function () {
            return this._max7;
        },
        set: function (v) {
            this._max7 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min7", {
        get: function () {
            return this._min7;
        },
        set: function (v) {
            this._min7 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max8", {
        get: function () {
            return this._max8;
        },
        set: function (v) {
            this._max8 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min8", {
        get: function () {
            return this._min8;
        },
        set: function (v) {
            this._min8 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max9", {
        get: function () {
            return this._max9;
        },
        set: function (v) {
            this._max9 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min9", {
        get: function () {
            return this._min9;
        },
        set: function (v) {
            this._min9 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max10", {
        get: function () {
            return this._max10;
        },
        set: function (v) {
            this._max10 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min10", {
        get: function () {
            return this._min10;
        },
        set: function (v) {
            this._min10 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max11", {
        get: function () {
            return this._max11;
        },
        set: function (v) {
            this._max11 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min11", {
        get: function () {
            return this._min11;
        },
        set: function (v) {
            this._min11 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "max12", {
        get: function () {
            return this._max12;
        },
        set: function (v) {
            this._max12 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "min12", {
        get: function () {
            return this._min12;
        },
        set: function (v) {
            this._min12 = parseFloat(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint1", {
        get: function () {
            return this._devicePoint1;
        },
        set: function (v) {
            this._devicePoint1 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint2", {
        get: function () {
            return this._devicePoint2;
        },
        set: function (v) {
            this._devicePoint2 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint3", {
        get: function () {
            return this._devicePoint3;
        },
        set: function (v) {
            this._devicePoint3 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint4", {
        get: function () {
            return this._devicePoint4;
        },
        set: function (v) {
            this._devicePoint4 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint5", {
        get: function () {
            return this._devicePoint5;
        },
        set: function (v) {
            this._devicePoint5 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint6", {
        get: function () {
            return this._devicePoint6;
        },
        set: function (v) {
            this._devicePoint6 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint7", {
        get: function () {
            return this._devicePoint7;
        },
        set: function (v) {
            this._devicePoint7 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint8", {
        get: function () {
            return this._devicePoint8;
        },
        set: function (v) {
            this._devicePoint8 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint9", {
        get: function () {
            return this._devicePoint9;
        },
        set: function (v) {
            this._devicePoint9 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint10", {
        get: function () {
            return this._devicePoint10;
        },
        set: function (v) {
            this._devicePoint10 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint11", {
        get: function () {
            return this._devicePoint11;
        },
        set: function (v) {
            this._devicePoint11 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "devicePoint12", {
        get: function () {
            return this._devicePoint12;
        },
        set: function (v) {
            this._devicePoint12 = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    TrendControl.prototype.onDevicePointValueChanged = function (devPoint) {
        var number = 0;
        for (var i = 1; i <= 12; i++) {
            if (devPoint.name == this["devicePoint" + i]) {
                number = i;
                if (devPoint.max != null && (typeof this["max" + number] == "undefined" || isNaN(this["max" + number]))) {
                    this["max" + number] = devPoint.max;
                }
                if (devPoint.max != null && (typeof this["min" + number] == "undefined" || isNaN(this["min" + number])))
                    this["min" + number] = devPoint.min;
                if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
                    this["colorLine" + number] = devPoint["colorLine" + number];
                if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
                    this["colorLine" + number] = "#ffffff";
                this["value" + number] = devPoint.value;
            }
        }
    };
    Object.defineProperty(TrendControl.prototype, "rect", {
        get: function () {
            return {
                x: parseInt(this.rectElement.getAttribute("x")),
                y: parseInt(this.rectElement.getAttribute("y")),
                width: parseInt(this.rectElement.getAttribute("width")),
                height: parseInt(this.rectElement.getAttribute("height")),
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            for (var i = 1; i <= 12; i++)
                this["pathElement" + i].setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
            this.textGroupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorFill", {
        get: function () {
            return this.rectElement.style.fill;
        },
        set: function (v) {
            this.rectElement.style.fill = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLineLeftBottom", {
        get: function () {
            return this.line_left_Ele.style.stroke;
        },
        set: function (v) {
            this.line_left_Ele.style.stroke = v;
            this.line_bottom_Ele.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine1", {
        get: function () {
            return this.pathElement1.style.stroke;
        },
        set: function (v) {
            this.pathElement1.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine2", {
        get: function () {
            return this.pathElement2.style.stroke;
        },
        set: function (v) {
            this.pathElement2.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine3", {
        get: function () {
            return this.pathElement3.style.stroke;
        },
        set: function (v) {
            this.pathElement3.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine4", {
        get: function () {
            return this.pathElement4.style.stroke;
        },
        set: function (v) {
            this.pathElement4.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine5", {
        get: function () {
            return this.pathElement5.style.stroke;
        },
        set: function (v) {
            this.pathElement5.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine6", {
        get: function () {
            return this.pathElement6.style.stroke;
        },
        set: function (v) {
            this.pathElement6.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine7", {
        get: function () {
            return this.pathElement7.style.stroke;
        },
        set: function (v) {
            this.pathElement7.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine8", {
        get: function () {
            return this.pathElement8.style.stroke;
        },
        set: function (v) {
            this.pathElement8.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine9", {
        get: function () {
            return this.pathElement9.style.stroke;
        },
        set: function (v) {
            this.pathElement9.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine10", {
        get: function () {
            return this.pathElement10.style.stroke;
        },
        set: function (v) {
            this.pathElement10.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine11", {
        get: function () {
            return this.pathElement11.style.stroke;
        },
        set: function (v) {
            this.pathElement11.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "colorLine12", {
        get: function () {
            return this.pathElement12.style.stroke;
        },
        set: function (v) {
            this.pathElement12.style.stroke = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(TrendControl.prototype, "Minutes", {
        get: function () {
            if (isNaN(this._Minutes))
                this._Minutes = 10;
            return this._Minutes;
        },
        set: function (v) {
            this._Minutes = v;
        },
        enumerable: true,
        configurable: true
    });
    TrendControl.prototype.getPropertiesCaption = function () {
        var arr = ["id", "背景颜色", "量程线颜色", "显示时间(分钟)"];
        for (var i = 1; i <= 12; i++) {
            arr.push("趋势颜色" + i);
            arr.push("设备点" + i);
            arr.push("最大量程" + i);
            arr.push("最小量程" + i);
        }
        return arr;
    };
    TrendControl.prototype.getProperties = function () {
        var arr = ["id", "colorFill", "colorLineLeftBottom", "Minutes"];
        for (var i = 1; i <= 12; i++) {
            arr.push("colorLine" + i);
            arr.push("devicePoint" + i);
            arr.push("max" + i);
            arr.push("min" + i);
        }
        return arr;
    };
    TrendControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    TrendControl.prototype.run = function () {
        var _this = this;
        _super.prototype.run.call(this);
        for (var i = 1; i <= 12; i++) {
            var valueArr = this["values" + i];
            if (valueArr.length > 1) {
                this["_value" + i] = valueArr[valueArr.length - 1].value;
            }
        }
        this.running = true;
        this.reDrawTrend();
        this.element._interval = setInterval(function () { return _this.reDrawTrend(); }, 1000);
    };
    TrendControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint1 ||
            pointName == this.devicePoint2 ||
            pointName == this.devicePoint3 ||
            pointName == this.devicePoint4 ||
            pointName == this.devicePoint5 ||
            pointName == this.devicePoint6 ||
            pointName == this.devicePoint7 ||
            pointName == this.devicePoint8 ||
            pointName == this.devicePoint9 ||
            pointName == this.devicePoint10 ||
            pointName == this.devicePoint11 ||
            pointName == this.devicePoint12) {
            this.selected = true;
            this.element.scrollIntoView();
        }
    };
    TrendControl.prototype.getMin = function (number) {
        var min = this["min" + number];
        if (min == null || isNaN(min)) {
            for (var i = 1; i <= 12; i++) {
                var minval = this["min" + i];
                if (minval != null && !isNaN(minval)) {
                    return minval;
                }
            }
        }
        else {
            return min;
        }
        return 0;
    };
    TrendControl.prototype.getMax = function (number) {
        var max = this["max" + number];
        if (max == null || isNaN(max)) {
            for (var i = 1; i <= 12; i++) {
                var maxval = this["max" + i];
                if (maxval != null && !isNaN(maxval)) {
                    return maxval;
                }
            }
        }
        else {
            return max;
        }
        return 0;
    };
    TrendControl.prototype.getDrawLocation = function (number, secondPixel, leftMargin, rightMargin, topMargin, bottomMargin, valueItem, canDel, rect, now) {
        var x = rect.width - rightMargin - ((now - valueItem.time) / 1000) * secondPixel;
        if (x < leftMargin) {
            if (canDel) {
                return {
                    isDel: true
                };
            }
            else {
                x = leftMargin;
            }
        }
        var min = this.getMin(number);
        var max = this.getMax(number);
        var percent = 1 - (valueItem.value - min) / (max - min);
        var y = topMargin + (rect.height - topMargin - bottomMargin) * percent;
        if (y < topMargin)
            y = topMargin;
        else if (y > rect.height - bottomMargin)
            y = rect.height - bottomMargin;
        return {
            result: x + " " + y + " "
        };
    };
    TrendControl.prototype.resetXYLines = function (rect, leftMargin, rightMargin, topMargin, bottomMargin) {
        this.line_left_Ele.setAttribute("x1", (rect.x + leftMargin));
        this.line_left_Ele.setAttribute("y1", (rect.y + topMargin));
        this.line_left_Ele.setAttribute("x2", (rect.x + leftMargin));
        this.line_left_Ele.setAttribute("y2", (rect.y + rect.height - bottomMargin));
        this.line_bottom_Ele.setAttribute("x1", (rect.x + leftMargin));
        this.line_bottom_Ele.setAttribute("y1", (rect.y + rect.height - bottomMargin));
        this.line_bottom_Ele.setAttribute("x2", (rect.x + rect.width - rightMargin));
        this.line_bottom_Ele.setAttribute("y2", (rect.y + rect.height - bottomMargin));
    };
    TrendControl.prototype.addText = function (text, color, x, y, fontSize) {
        var txtEle = document.createElementNS('http://www.w3.org/2000/svg', 'text');
        txtEle.textContent = text;
        txtEle.setAttribute("x", x);
        txtEle.setAttribute("y", y);
        txtEle.setAttribute('style', 'fill:' + color + ';cursor:default;-moz-user-select:none;visibility:hidden;');
        txtEle.setAttribute('font-size', fontSize);
        this.textGroupElement.appendChild(txtEle);
        return txtEle;
    };
    TrendControl.prototype.formatTime = function (date, fmt) {
        var o = {
            "M+": date.getMonth() + 1,
            "d+": date.getDate(),
            "h+": date.getHours(),
            "m+": date.getMinutes(),
            "s+": date.getSeconds(),
            "q+": Math.floor((date.getMonth() + 3) / 3),
            "S": date.getMilliseconds()
        };
        if (/(y+)/.test(fmt))
            fmt = fmt.replace(RegExp.$1, (date.getFullYear() + "").substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp("(" + k + ")").test(fmt))
                fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (("00" + o[k]).substr(("" + o[k]).length)));
        return fmt;
    };
    TrendControl.prototype.reDrawTrend = function () {
        var leftMargin = 30;
        var rightMargin = 10;
        var bottomMargin = 30;
        var topMargin = 10;
        var secondPixel;
        var txtHeight;
        var fontSize = 12;
        if (true) {
            var maxValue = 0;
            for (var i = 1; i <= 12; i++) {
                var value = this.getMax(i);
                if (value > maxValue) {
                    maxValue = value;
                }
            }
            var txtEle = this.addText(maxValue.toString(), "red", 0, 0, fontSize);
            var txtRect = txtEle.getBoundingClientRect();
            txtHeight = txtRect.height;
            leftMargin = txtRect.width + 15;
        }
        var rect = this.rect;
        this.resetXYLines(rect, leftMargin, rightMargin, topMargin, bottomMargin);
        this.textGroupElement.innerHTML = "";
        var panelWidth = rect.width - leftMargin - rightMargin;
        var seconds = this.Minutes * 60;
        secondPixel = panelWidth / seconds;
        if (secondPixel < 1)
            secondPixel = 1;
        var now = new Date().getTime();
        if (true) {
            var x = leftMargin;
            var y = rect.height - bottomMargin - 30;
            while (true) {
                var lineEle = document.createElementNS('http://www.w3.org/2000/svg', 'line');
                lineEle.setAttribute("x1", (leftMargin - 5));
                lineEle.setAttribute("y1", y);
                lineEle.setAttribute("x2", leftMargin);
                lineEle.setAttribute("y2", y);
                lineEle.setAttribute('style', 'stroke:' + this.colorLineLeftBottom + ';stroke-width:2;');
                this.textGroupElement.appendChild(lineEle);
                y -= 30;
                if (y <= topMargin) {
                    break;
                }
            }
            var cury = topMargin;
            for (var i = 0; i < 12; i++) {
                var max = this["max" + (i + 1)];
                if (max > 0) {
                    var txtEle = this.addText(max, this["colorLine" + (i + 1)], 0, cury + 10, fontSize);
                    var txtRect = txtEle.getBoundingClientRect();
                    txtEle.setAttribute("x", (leftMargin - 4 - txtRect.width));
                    txtEle.style.visibility = "";
                    cury += txtRect.height - 5;
                    if (cury > rect.height - bottomMargin)
                        break;
                }
            }
            var cury = rect.height - bottomMargin;
            for (var i = 11; i >= 0; i--) {
                var max = this["max" + (i + 1)];
                var min = this["min" + (i + 1)];
                if (max > 0) {
                    var txtEle = this.addText(min, this["colorLine" + (i + 1)], 0, cury, fontSize);
                    var txtRect = txtEle.getBoundingClientRect();
                    txtEle.setAttribute("x", (leftMargin - 4 - txtRect.width));
                    txtEle.style.visibility = "";
                    cury -= txtRect.height - 5;
                    if (cury <= topMargin)
                        break;
                }
            }
        }
        if (true) {
            var curTime = new Date();
            var timeText = "2018-01-01 01:01:01";
            var x = rect.width - rightMargin;
            var y = rect.height - bottomMargin + 3;
            while (true) {
                var txtEle = this.addText(timeText, this.colorLineLeftBottom, x, y + 17, fontSize);
                var txtWidth = txtEle.getBoundingClientRect().width;
                var left = x - txtWidth / 2;
                var length = rect.width - rightMargin - left;
                var mySeconds = length / secondPixel;
                curTime = new Date(now - mySeconds * 1000);
                txtEle.textContent = this.formatTime(curTime, "yyyy-MM-dd hh:mm:ss");
                txtEle.setAttribute("x", (x - txtWidth));
                txtEle.style.visibility = "";
                x -= txtWidth + 8;
                if (x <= leftMargin) {
                    break;
                }
            }
            x = leftMargin + 30;
            while (true) {
                var lineEle = document.createElementNS('http://www.w3.org/2000/svg', 'line');
                lineEle.setAttribute("x1", x);
                lineEle.setAttribute("y1", (rect.height - bottomMargin));
                lineEle.setAttribute("x2", x);
                lineEle.setAttribute("y2", (rect.height - bottomMargin + 5));
                lineEle.setAttribute('style', 'stroke:' + this.colorLineLeftBottom + ';stroke-width:2;');
                this.textGroupElement.appendChild(lineEle);
                x += 30;
                if (x > rect.width - rightMargin) {
                    break;
                }
            }
        }
        for (var k = 1; k <= 12; k++) {
            var valueArr = this["values" + k];
            if (valueArr.length == 0)
                continue;
            var dataStr = "";
            var deleteToIndex = -1;
            if (new Date().getTime() - valueArr[valueArr.length - 1].time > 1000) {
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
                var location = this.getDrawLocation(k, secondPixel, leftMargin, rightMargin, topMargin, bottomMargin, valueArr[i], canDel, rect, now);
                if (location.isDel) {
                    deleteToIndex = i;
                    break;
                }
                dataStr += dataStr.length == 0 ? "M" : "L";
                dataStr += location.result;
            }
            if (deleteToIndex >= 0) {
                valueArr.splice(0, deleteToIndex + 1);
            }
            this["pathElement" + k].setAttribute("d", dataStr);
        }
    };
    TrendControl.prototype.onBeginMoving = function () {
        this.rectElement._rect = this.rect;
    };
    TrendControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._rect.x + nowX - downX);
        var y = (this.rectElement._rect.y + nowY - downY);
        this.rect = {
            x: x,
            y: y,
            width: this.rectElement._rect.width,
            height: this.rectElement._rect.height
        };
    };
    TrendControl.prototype.onEndMoving = function () {
    };
    return TrendControl;
}(EditorControl));
var HistoryTrendControl = (function (_super) {
    __extends(HistoryTrendControl, _super);
    function HistoryTrendControl() {
        return _super.call(this) || this;
    }
    HistoryTrendControl.prototype.run = function () {
        var _this = this;
        this.isDesignMode = false;
        var clientRect = this.rectElement.getBoundingClientRect();
        var div = document.createElement("DIV");
        div.style.padding = "3px";
        div.innerHTML = "<input type=text placeholder='请选择起始日期'>至<input type=text placeholder='请选择结束日期'><input type=button value='查询历史'><input style='margin-left:2px;' type=button value='列表窗口'>";
        div.style.position = "absolute";
        div.style.left = clientRect.left + "px";
        div.style.visibility = "hidden";
        editor.svgContainer.parentElement.appendChild(div);
        div.style.top = (clientRect.top - div.offsetHeight) + "px";
        div.style.visibility = "";
        var picker1 = new Pikaday({
            field: div.children[0],
            firstDay: 1,
            minDate: new Date('2018-01-01'),
            maxDate: new Date('2060-12-31'),
            yearRange: [2018, 2060]
        });
        var picker2 = new Pikaday({
            field: div.children[1],
            firstDay: 1,
            minDate: new Date('2018-01-01'),
            maxDate: new Date('2060-12-31'),
            yearRange: [2018, 2060]
        });
        div.children[2].addEventListener("click", function () {
            var startDate = div.children[0].value;
            var endDate = div.children[1].value;
            if (startDate.length == 0) {
                alert("请选择起始日期");
                return;
            }
            if (endDate.length == 0) {
                alert("请选择结束日期");
                return;
            }
            var pointNames = [];
            for (var i = 1; i <= 12; i++) {
                var point = _this["devicePoint" + i];
                if (point && point.length > 0) {
                    pointNames.push(point);
                }
            }
            div.children[2].value = "正在查询...";
            window.remoting.server.SearchHistory(pointNames, startDate, endDate, function (ret, err) {
                div.children[2].value = "查询历史";
                if (err)
                    alert(err);
                else {
                    _this.setData(new Date(startDate), new Date(endDate), ret);
                }
            });
        }, false);
        div.children[3].addEventListener("click", function () {
            var str = "";
            for (var i = 1; i <= 12; i++) {
                var point = _this["devicePoint" + i];
                if (point && point.length > 0) {
                    if (str.length > 0)
                        str += " ";
                    str += "=" + point;
                }
            }
            window.showHistoryWindow(str);
        }, false);
    };
    HistoryTrendControl.prototype.onDevicePointValueChanged = function (devPoint) {
        var number = 0;
        for (var i = 1; i <= 12; i++) {
            if (devPoint.name == this["devicePoint" + i]) {
                number = i;
                break;
            }
        }
        if (number == 0)
            return;
        if (devPoint.max != null && (typeof this["max" + number] == "undefined" || isNaN(this["max" + number])))
            this["max" + number] = devPoint.max;
        if (devPoint.max != null && (typeof this["min" + number] == "undefined" || isNaN(this["min" + number])))
            this["min" + number] = devPoint.min;
        if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
            this["colorLine" + number] = devPoint["colorLine" + number];
        if (!this["colorLine" + number] || this["colorLine" + number].length == 0)
            this["colorLine" + number] = "#ffffff";
    };
    HistoryTrendControl.prototype.parseData = function (dataStartSecs, dataEndSecs, valueDatas) {
        var result = {
            minValue: null,
            maxValue: null,
            minValueBefore: true,
        };
        for (var i = 0; i < valueDatas.length; i++) {
            var item = valueDatas[i];
            var mySec = item.seconds;
            if (mySec >= dataStartSecs && mySec <= dataEndSecs) {
                var value = item.value;
                if (result.minValue == null || value < result.minValue) {
                    result.minValueBefore = false;
                    result.minValue = value;
                }
                if (result.maxValue == null || value > result.maxValue) {
                    result.minValueBefore = true;
                    result.maxValue = value;
                }
            }
            else if (mySec > dataEndSecs)
                break;
        }
        if (result.maxValue == null && result.minValue == null)
            return null;
        if (result.maxValue == result.minValue) {
            result.minValueBefore = false;
            result.minValue = null;
        }
        return result;
    };
    HistoryTrendControl.prototype.setData = function (startTime, endTime, datas) {
        var rect = this.rect;
        var totalSeconds = (endTime.getTime() - startTime.getTime()) / 1000 + 1;
        var secsPerPixel = parseInt((totalSeconds / (rect.width - 20)));
        if (secsPerPixel < 1)
            secsPerPixel = 1;
        var curPointLineResults = [];
        for (var i = 0; i < 12; i++) {
            var lineResult = {
                datas: []
            };
            var pointDatas;
            if (i < datas.length) {
                pointDatas = datas[i];
                curPointLineResults.push(lineResult);
            }
            else
                break;
            var dataStartSecs = startTime.getTime() / 1000;
            var dataEndSecs = dataStartSecs + secsPerPixel - 1;
            for (var j = 0; j < rect.width - 20; j++) {
                var valueResult = this.parseData(dataStartSecs, dataEndSecs, pointDatas);
                lineResult.datas.push(valueResult);
                dataStartSecs += secsPerPixel;
                dataEndSecs = dataStartSecs + secsPerPixel - 1;
            }
        }
        for (var i = 0; i < 12; i++) {
            var min = this.getMin(i + 1);
            var max = this.getMax(i + 1);
            var lineObject = null;
            if (i < curPointLineResults.length) {
                lineObject = curPointLineResults[i];
            }
            else
                lineObject = {
                    datas: []
                };
            var dataStr = "";
            var position = 0;
            var lastY = rect.height - 10;
            for (var j = 0; j < lineObject.datas.length; j++) {
                var valueItem = lineObject.datas[j];
                if (valueItem == null) {
                    continue;
                }
                dataStr += dataStr.length == 0 ? "M" : "L";
                dataStr += (j + 10) + " " + lastY + " ";
                if (valueItem.maxValue != null) {
                    var percent = 1 - (valueItem.maxValue - min) / (max - min);
                    var y = 10 + (rect.height - 20) * percent;
                    if (y < 10)
                        y = 10;
                    else if (y > rect.height - 10)
                        y = rect.height - 10;
                    valueItem.maxValueY = y;
                }
                if (valueItem.minValue != null) {
                    var percent = 1 - (valueItem.minValue - min) / (max - min);
                    var y = 10 + (rect.height - 20) * percent;
                    if (y < 10)
                        y = 10;
                    else if (y > rect.height - 10)
                        y = rect.height - 10;
                    valueItem.minValueY = y;
                }
                if (valueItem.minValueBefore && valueItem.minValue != null) {
                    dataStr += "L" + (j + 10) + " " + valueItem.minValueY + " ";
                    lastY = valueItem.minValueY;
                    if (valueItem.maxValue != null) {
                        dataStr += "L" + (j + 10) + " " + valueItem.maxValueY + " ";
                        lastY = valueItem.maxValueY;
                    }
                }
                else if (valueItem.minValueBefore == false && valueItem.maxValue != null) {
                    dataStr += "L" + (j + 10) + " " + valueItem.maxValueY + " ";
                    lastY = valueItem.maxValueY;
                    if (valueItem.minValueY != null) {
                        dataStr += "L" + (j + 10) + " " + valueItem.minValueY + " ";
                        lastY = valueItem.minValueY;
                    }
                }
            }
            if (dataStr.length > 0) {
                dataStr += "L" + (rect.width - 10) + " " + lastY + " ";
            }
            this["pathElement" + (i + 1)].setAttribute("d", dataStr);
        }
    };
    return HistoryTrendControl;
}(TrendControl));
var ButtonAreaControl = (function (_super) {
    __extends(ButtonAreaControl, _super);
    function ButtonAreaControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'rect')) || this;
        _this.startX = 0;
        _this.startY = 0;
        _this.pointAddr = null;
        _this._devicePoint = "";
        _this._clickValues = null;
        _this._scriptOnClick = null;
        _this.rectElement = _this.element;
        _this.rectElement.setAttribute('style', 'fill:#000000;fill-opacity:0.3;stroke:none;');
        return _this;
    }
    Object.defineProperty(ButtonAreaControl.prototype, "rect", {
        get: function () {
            return {
                x: parseInt(this.rectElement.getAttribute("x")),
                y: parseInt(this.rectElement.getAttribute("y")),
                width: parseInt(this.rectElement.getAttribute("width")),
                height: parseInt(this.rectElement.getAttribute("height")),
            };
        },
        set: function (v) {
            this.rectElement.setAttribute("x", v.x);
            this.rectElement.setAttribute("y", v.y);
            this.rectElement.setAttribute("width", v.width);
            this.rectElement.setAttribute("height", v.height);
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ButtonAreaControl.prototype, "devicePoint", {
        get: function () {
            return this._devicePoint;
        },
        set: function (v) {
            this._devicePoint = v;
            if (WatchPointNames.indexOf(v) < 0)
                WatchPointNames.push(v);
        },
        enumerable: true,
        configurable: true
    });
    ButtonAreaControl.prototype.selectByPointName = function (pointName) {
        if (pointName == this.devicePoint) {
            this.selected = true;
            this.rectElement.scrollIntoView();
        }
    };
    Object.defineProperty(ButtonAreaControl.prototype, "clickValues", {
        get: function () {
            return this._clickValues;
        },
        set: function (v) {
            this._clickValues = v;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(ButtonAreaControl.prototype, "scriptOnClick", {
        get: function () {
            return this._scriptOnClick;
        },
        set: function (v) {
            this._scriptOnClick = v;
        },
        enumerable: true,
        configurable: true
    });
    ButtonAreaControl.prototype.onDevicePointValueChanged = function (devPoint) {
        this.pointValue = devPoint.value;
        this.pointAddr = devPoint.addr;
    };
    ButtonAreaControl.prototype.getPropertiesCaption = function () {
        return ["设备点", "点击设值", "点击脚本"];
    };
    ButtonAreaControl.prototype.getProperties = function () {
        return ["devicePoint", "clickValues", "scriptOnClick"];
    };
    ButtonAreaControl.prototype.run = function () {
        var _this = this;
        _super.prototype.run.call(this);
        this.rectElement.style.fillOpacity = "0";
        this.rectElement.style.cursor = "pointer";
        this.rectElement.addEventListener("click", function (e) {
            e.stopPropagation();
            if (_this.scriptOnClick && _this.scriptOnClick.length > 0) {
                eval(_this.scriptOnClick);
            }
            else {
                if (_this.clickValues && _this.pointAddr && _this.clickValues.length > 0) {
                    var values = _this.clickValues.split(',');
                    var index = values.indexOf(_this.pointValue.toString());
                    index++;
                    if (index >= values.length)
                        index = 0;
                    var nextvalue = values[index];
                    _this.container.writeValue(_this.devicePoint, _this.pointAddr, nextvalue);
                    _this.pointValue = nextvalue;
                    _this.lastSetValueTime = new Date().getTime();
                }
            }
        }, false);
    };
    ButtonAreaControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    ButtonAreaControl.prototype.onBeginMoving = function () {
        this.rectElement._x = parseInt(this.rectElement.getAttribute("x"));
        this.rectElement._y = parseInt(this.rectElement.getAttribute("y"));
    };
    ButtonAreaControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.rectElement._x + nowX - downX);
        var y = (this.rectElement._y + nowY - downY);
        this.rectElement.setAttribute("x", x);
        this.rectElement.setAttribute("y", y);
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    ButtonAreaControl.prototype.onEndMoving = function () {
    };
    return ButtonAreaControl;
}(EditorControl));
var GroupControl = (function (_super) {
    __extends(GroupControl, _super);
    function GroupControl(element, windowCode) {
        var _this = _super.call(this, element) || this;
        _this.controls = [];
        _this.startX = 0;
        _this.startY = 0;
        _this._path = null;
        _this.contentWidth = 0;
        _this.contentHeight = 0;
        _this.customProperties = [];
        _this.windowCode = windowCode;
        element.setAttribute("transform", "translate(0 0) scale(1 1)");
        _this.groupElement = element;
        _this.childGroupElement = document.createElementNS('http://www.w3.org/2000/svg', 'g');
        _this.childGroupElement.style.transform = "rotate(0deg)";
        _this.groupElement.appendChild(_this.childGroupElement);
        if (!_this.virtualRectElement) {
            _this.virtualRectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            _this.groupElement.appendChild(_this.virtualRectElement);
            _this.virtualRectElement.setAttribute('x', "0");
            _this.virtualRectElement.setAttribute('y', "0");
            _this.virtualRectElement.setAttribute('style', 'fill:#ffffff;fill-opacity:0.1;stroke:#cccccc;stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
        }
        return _this;
    }
    GroupControl.prototype.getControl = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i].id == id) {
                return this.controls[i];
            }
        }
        return null;
    };
    GroupControl.prototype.isIdExist = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (typeof this.controls[i].isIdExist == "function") {
                var result = this.controls[i].isIdExist(id);
                if (result)
                    return true;
            }
            if (this.controls[i].id == id)
                return true;
        }
        return false;
    };
    GroupControl.prototype.removeControl = function (ctrl) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                this.groupElement.removeChild(ctrl.element);
                ctrl.isInGroup = false;
                ctrl.container = null;
                this.controls.splice(i, 1);
                break;
            }
        }
    };
    GroupControl.prototype.addControl = function (ctrl) {
        if (!ctrl.id || ctrl.id.length == 0) {
            var controlId = ctrl.constructor.name;
            var index = 1;
            while (this.isIdExist(controlId + index)) {
                index++;
            }
            ctrl.id = controlId + index;
        }
        ctrl.isInGroup = true;
        ctrl.container = this;
        this.childGroupElement.appendChild(ctrl.element);
        this.controls.push(ctrl);
        if (ctrl.constructor.name == "GroupControl") {
            var groupControl = ctrl;
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
    };
    GroupControl.prototype.writeValue = function (pointName, addr, value) {
        for (var p in this) {
            if (p == pointName) {
                this[p] = value;
                return;
            }
        }
        window.writeValue(pointName, addr, value);
    };
    Object.defineProperty(GroupControl.prototype, "path", {
        get: function () {
            if (this._path == null)
                this._path = JHttpHelper.downloadUrl(ServerUrl + "/Home/GetWindowPath?windowCode=" + this.windowCode);
            return this._path;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupControl.prototype, "rect", {
        get: function () {
            var transform = this.groupElement.getAttribute("transform");
            var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
            var myrect = {};
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
            this.virtualRectElement.setAttribute('width', this.contentWidth);
            this.virtualRectElement.setAttribute('height', this.contentHeight);
            myrect.width = parseInt((this.contentWidth * scalex));
            myrect.height = parseInt((this.contentHeight * scaley));
            this.lastRect = myrect;
            return myrect;
        },
        set: function (v) {
            if (v.width == null) {
                this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ") scale(1 1)");
                var r = this.rect;
                return;
            }
            if (this.contentWidth == 0) {
                var r = this.rect;
            }
            var scalex = parseFloat(v.width) / this.contentWidth;
            var scaley = parseFloat(v.height) / this.contentHeight;
            scaley = scalex;
            this.virtualRectElement.setAttribute('width', this.contentWidth);
            this.virtualRectElement.setAttribute('height', this.contentHeight);
            this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ") scale(" + scalex + " " + scaley + ")");
            this.childGroupElement.style.transformOrigin = ((this.contentWidth * scalex) / 2) + "px " + ((this.contentHeight * scalex) / 2) + "px";
            this.lastRect = v;
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(GroupControl.prototype, "rotate", {
        get: function () {
            var transform = this.childGroupElement.style.transform;
            var result = /rotate\(([0-9]+)deg\)/.exec(transform);
            return parseInt(result[1]);
        },
        set: function (v) {
            this.childGroupElement.style.transform = "rotate(" + v + "deg)";
        },
        enumerable: true,
        configurable: true
    });
    GroupControl.prototype.getPropertiesCaption = function () {
        var caps = ["id"];
        caps.push("旋转角度");
        for (var i = 0; i < this.customProperties.length; i++) {
            caps.push(this.customProperties[i] + "设备点");
        }
        return caps;
    };
    GroupControl.prototype.getProperties = function () {
        var pros = ["id"];
        pros.push("rotate");
        for (var i = 0; i < this.customProperties.length; i++) {
            pros.push(this.customProperties[i] + "_devPoint");
        }
        return pros;
    };
    GroupControl.prototype.run = function () {
        _super.prototype.run.call(this);
        if (this.virtualRectElement) {
            this.groupElement.removeChild(this.virtualRectElement);
        }
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].run();
        }
    };
    GroupControl.prototype.isIntersectWith = function (rect) {
        var clientRect = this.childGroupElement.getBoundingClientRect();
        var myrect = {
            x: (clientRect.left + editor.svgContainer.parentElement.scrollLeft) / editor.currentScale,
            y: (clientRect.top - editor.divContainer.offsetTop + editor.svgContainer.parentElement.scrollTop) / editor.currentScale,
            width: clientRect.width / editor.currentScale, height: clientRect.height / editor.currentScale
        };
        return this.isIntersect(myrect, rect);
    };
    GroupControl.prototype.resetPointLocation = function () {
        if (!this.selected)
            return;
        var clientRect = this.childGroupElement.getBoundingClientRect();
        var rect = {
            x: (clientRect.left + editor.svgContainer.parentElement.scrollLeft) / editor.currentScale,
            y: (clientRect.top - editor.divContainer.offsetTop + editor.svgContainer.parentElement.scrollTop) / editor.currentScale,
            width: clientRect.width / editor.currentScale, height: clientRect.height / editor.currentScale
        };
        for (var i = 0; i < this.pointEles.length; i++) {
            this.pointEles[i]._setLocation(this.pointEles[i], rect);
        }
    };
    GroupControl.prototype.selectByPointName = function (pointName) {
        for (var i = 0; i < this.customProperties.length; i++) {
            var proName = this.customProperties[i];
            if (this[proName + "_devPoint"] == pointName) {
                this.selected = true;
                var root = this.groupElement;
                while (root.children.length > 0) {
                    root = root.children[0];
                }
                root.scrollIntoView();
                return;
            }
        }
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selectByPointName(pointName);
        }
    };
    GroupControl.prototype.onDevicePointValueChanged = function (point) {
        for (var i = 0; i < this.customProperties.length; i++) {
            var proName = this.customProperties[i];
            if (this[proName + "_devPoint"] == point.name) {
                this[proName + "_devPoint_addr"] = point.addr;
                this[proName + "_devPoint_max"] = point.max;
                this[proName + "_devPoint_min"] = point.min;
                this["_" + proName] = point.value;
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
            for (var i = 0; i < this.controls.length; i++) {
                var control = this.controls[i];
                this.onChildrenPointValueChanged(control, point);
            }
        }
    };
    GroupControl.prototype.onChildrenPointValueChanged = function (control, point) {
        if (control.constructor.name == "GroupControl" ||
            control.devicePoint == ManyPointDefined || control.devicePoint == point.name) {
            if (new Date().getTime() - control.lastSetValueTime < 2000) {
                window.updateValueLater(control, point);
            }
            else {
                if (control.updatePointValueTimeoutFlag) {
                    clearTimeout(control.updatePointValueTimeoutFlag);
                }
            }
            control.onDevicePointValueChanged(point);
        }
    };
    GroupControl.prototype.getJson = function () {
        var json = _super.prototype.getJson.call(this);
        json.windowCode = this.windowCode;
        return json;
    };
    GroupControl.prototype.getScript = function () {
        var json = this.getJson();
        var script = "";
        var id = "eCtrl";
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
    };
    GroupControl.prototype.loadCustomProperties = function (properties) {
        if (properties && properties.length > 0) {
            var ps = properties.split('\n');
            for (var i = 0; i < ps.length; i++) {
                var name = ps[i].trim();
                if (name.length == 0)
                    continue;
                this.customProperties.push(name);
                this["_" + name] = null;
                this[name + "_devPoint_addr"] = null;
                this[name + "_devPoint_max"] = null;
                this[name + "_devPoint_min"] = null;
                Object.defineProperty(this, name, {
                    get: this.getFuncForCustomProperty(this, name),
                    set: this.setFuncForCustomProperty(this, name),
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
    };
    GroupControl.prototype.getFuncForCustomProperty_DevPoint = function (self, name) {
        return function () {
            return self["_" + name];
        };
    };
    GroupControl.prototype.setFuncForCustomProperty_DevPoint = function (self, name) {
        return function (v) {
            if (self["_" + name] !== v) {
                self["_" + name] = v;
                if (WatchPointNames.indexOf(v) < 0)
                    WatchPointNames.push(v);
            }
        };
    };
    GroupControl.prototype.getFuncForCustomProperty = function (self, name) {
        return function () {
            return self["_" + name];
        };
    };
    GroupControl.prototype.setFuncForCustomProperty = function (self, name) {
        return function (value) {
            if (self["_" + name] !== value) {
                self["_" + name] = value;
                var pointName = self[name + "_devPoint"];
                if (pointName && pointName.length > 0) {
                    self.container.writeValue(pointName, self[name + "_devPoint_addr"], value);
                }
                var point = {
                    max: self[name + "_devPoint_max"],
                    min: self[name + "_devPoint_min"],
                    name: name,
                    value: value,
                    isCustomProperty: true
                };
                for (var i = 0; i < self.controls.length; i++) {
                    var control = self.controls[i];
                    self.onChildrenPointValueChanged(control, point);
                }
            }
        };
    };
    GroupControl.prototype.createGroupControl = function (windowCode, rect) {
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
    };
    GroupControl.prototype.onBeginMoving = function () {
        var rect = this.rect;
        this.groupElement._x = rect.x;
        this.groupElement._y = rect.y;
        this.groupElement._width = rect.width;
        this.groupElement._height = rect.height;
    };
    GroupControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.groupElement._x + nowX - downX);
        var y = (this.groupElement._y + nowY - downY);
        this.rect = { x: x, y: y, width: this.groupElement._width, height: this.groupElement._height };
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    GroupControl.prototype.onEndMoving = function () {
    };
    return GroupControl;
}(EditorControl));
var FreeGroupControl = (function (_super) {
    __extends(FreeGroupControl, _super);
    function FreeGroupControl() {
        var _this = _super.call(this, document.createElementNS('http://www.w3.org/2000/svg', 'g')) || this;
        _this.controls = [];
        _this.contentWidth = 0;
        _this.contentHeight = 0;
        _this.groupElement = _this.element;
        _this.groupElement.setAttribute("transform", "translate(0 0)");
        if (!_this.virtualRectElement) {
            _this.virtualRectElement = document.createElementNS('http://www.w3.org/2000/svg', 'rect');
            _this.groupElement.appendChild(_this.virtualRectElement);
            _this.virtualRectElement.setAttribute('x', "0");
            _this.virtualRectElement.setAttribute('y', "0");
            _this.virtualRectElement.setAttribute('fill-opacity', "0");
            _this.virtualRectElement.setAttribute('stroke', "green");
            _this.virtualRectElement.setAttribute('style', 'stroke-width:1;stroke-dasharray:2;stroke-dashoffset:2;');
        }
        return _this;
    }
    FreeGroupControl.prototype.getControl = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i].id == id) {
                return this.controls[i];
            }
        }
        return null;
    };
    FreeGroupControl.prototype.isIdExist = function (id) {
        for (var i = 0; i < this.controls.length; i++) {
            if (typeof this.controls[i].isIdExist == "function") {
                var result = this.controls[i].isIdExist(id);
                if (result)
                    return true;
            }
            if (this.controls[i].id == id)
                return true;
        }
        return false;
    };
    FreeGroupControl.prototype.removeControl = function (ctrl) {
        for (var i = 0; i < this.controls.length; i++) {
            if (this.controls[i] == ctrl) {
                this.groupElement.removeChild(ctrl.element);
                ctrl.isInGroup = false;
                ctrl.container = null;
                this.controls.splice(i, 1);
                break;
            }
        }
    };
    FreeGroupControl.prototype.addControl = function (ctrl) {
        ctrl.isInGroup = true;
        ctrl.container = this;
        this.groupElement.appendChild(ctrl.element);
        this.controls.push(ctrl);
    };
    FreeGroupControl.prototype.addControls = function (ctrls) {
        this.controls = [];
        var minLeft = 999999999;
        var minTop = 999999999;
        for (var i = 0; i < ctrls.length; i++) {
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
                height: _rect.height
            };
        }
        this.groupChildren();
        this.rect = {
            x: minLeft,
            y: minTop,
            width: this.contentWidth,
            height: this.contentHeight
        };
    };
    FreeGroupControl.prototype.groupChildren = function () {
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
        this.groupElement.removeChild(this.virtualRectElement);
        this.groupElement.appendChild(this.virtualRectElement);
    };
    FreeGroupControl.prototype.freeControls = function () {
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
    };
    FreeGroupControl.prototype.writeValue = function (pointName, addr, value) {
        window.writeValue(pointName, addr, value);
    };
    Object.defineProperty(FreeGroupControl.prototype, "rect", {
        get: function () {
            var transform = this.groupElement.getAttribute("transform");
            var result = /translate\(([0-9]+) ([0-9]+)\)/.exec(transform);
            var myrect = {};
            myrect.x = parseInt(result[1]);
            myrect.y = parseInt(result[2]);
            this.virtualRectElement.setAttribute('width', this.contentWidth);
            this.virtualRectElement.setAttribute('height', this.contentHeight);
            myrect.width = this.contentWidth;
            myrect.height = this.contentHeight;
            this.lastRect = myrect;
            return myrect;
        },
        set: function (v) {
            if (v.width == null) {
                this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
                var r = this.rect;
                return;
            }
            if (this.contentWidth == 0) {
                var r = this.rect;
            }
            this.virtualRectElement.setAttribute('width', this.contentWidth);
            this.virtualRectElement.setAttribute('height', this.contentHeight);
            this.groupElement.setAttribute("transform", "translate(" + v.x + " " + v.y + ")");
            this.lastRect = v;
            this.resetPointLocation();
        },
        enumerable: true,
        configurable: true
    });
    FreeGroupControl.prototype.getPropertiesCaption = function () {
        var caps = ["id"];
        return caps;
    };
    FreeGroupControl.prototype.getProperties = function () {
        var pros = ["id"];
        return pros;
    };
    Object.defineProperty(FreeGroupControl.prototype, "childScripts", {
        set: function (scripts) {
            var my = this;
            for (var i = 0; i < scripts.length; i++) {
                eval("(function(editor){" + scripts[i] + "})(my)");
            }
            this.groupChildren();
        },
        enumerable: true,
        configurable: true
    });
    FreeGroupControl.prototype.selectByPointName = function (pointName) {
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].selectByPointName(pointName);
        }
    };
    FreeGroupControl.prototype.onSelectedChange = function () {
        this.virtualRectElement.setAttribute('stroke', this.selected ? "red" : "green");
    };
    FreeGroupControl.prototype.run = function () {
        _super.prototype.run.call(this);
        if (this.virtualRectElement) {
            this.groupElement.removeChild(this.virtualRectElement);
        }
        for (var i = 0; i < this.controls.length; i++) {
            this.controls[i].run();
        }
    };
    FreeGroupControl.prototype.isIntersectWith = function (rect) {
        return this.isIntersect(this.rect, rect);
    };
    FreeGroupControl.prototype.onDevicePointValueChanged = function (point) {
        for (var i = 0; i < this.controls.length; i++) {
            var control = this.controls[i];
            control.onDevicePointValueChanged(point);
        }
    };
    FreeGroupControl.prototype.getJson = function () {
        var obj = {
            rect: this.rect,
            constructorName: this.constructor.name
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
    };
    FreeGroupControl.prototype.getScript = function () {
        var rect = this.rect;
        var script = "";
        var id = this.id;
        if (!id || id.length == 0) {
            id = "eCtrl";
        }
        script += id + " = new FreeGroupControl();\r\n";
        script += "editor.addControl(" + id + ");\r\n";
        script += id + ".id = " + JSON.stringify(this.id) + ";\r\n";
        for (var i = 0; i < this.controls.length; i++) {
            var childScript = this.controls[i].getScript();
            script += "(function(editor){\r\n" + childScript + "\r\n})(" + id + ");\r\n";
        }
        script += id + ".rect = " + JSON.stringify(rect) + ";\r\n";
        script += id + ".groupChildren();\r\n";
        return script;
    };
    FreeGroupControl.prototype.onBeginMoving = function () {
        var rect = this.rect;
        this.groupElement._x = rect.x;
        this.groupElement._y = rect.y;
        this.groupElement._width = rect.width;
        this.groupElement._height = rect.height;
    };
    FreeGroupControl.prototype.onMoving = function (downX, downY, nowX, nowY) {
        var x = (this.groupElement._x + nowX - downX);
        var y = (this.groupElement._y + nowY - downY);
        this.rect = { x: x, y: y, width: this.groupElement._width, height: this.groupElement._height };
        if (this.selected) {
            this.resetPointLocation();
        }
    };
    FreeGroupControl.prototype.onEndMoving = function () {
    };
    return FreeGroupControl;
}(EditorControl));
//# sourceMappingURL=EditorControls.js.map